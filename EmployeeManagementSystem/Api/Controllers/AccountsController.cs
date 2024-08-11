
using Api.Data;
using Api.DataModel;
using ChatApi.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;


namespace ChatApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountsController : ControllerBase
    {
    
      
       
        private readonly DataContext _sql;
        public AccountsController(DataContext sql)
        {
         
            _sql = sql;
        }


        //hesap kayýt iþlemi yapýlýr
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] MAccount.Form form)
        {
            var response = new MethodResponse<MAccount.Response>();

            try
            {
                var emailControl = _sql.Accounts.Where(k => k.Email == form.Email&&k.IsDeleted==false)?.FirstOrDefault();
            
                if (string.IsNullOrEmpty(form.Email))
                {
                    response.Status = 400;
                    response.StatusTexts.Add("Email boþ olamaz");
                }
                else if (!IsValidEmail(form.Email))
                {
                    response.Status = 400;
                    response.StatusTexts.Add("Geçersiz e-posta adresi");
                }
                else if (emailControl != null)
                {
                    response.Status = 400;
                    response.StatusTexts.Add("Email kullanýlýyor");
                }
             

                if (string.IsNullOrEmpty(form.AccountName))
                {
                    response.Status = 400;
                    response.StatusTexts.Add("Hesap ismi boþ olamaz");
                }

                if (string.IsNullOrEmpty(form.Password))
                {
                    response.Status = 400;
                    response.StatusTexts.Add("Þifre boþ olamaz");
                }
                else if (form.Password.Length < 4)
                {
                    response.Status = 400;
                    response.StatusTexts.Add("Þifre en az 4 karakter uzunluðunda olmalýdýr");
                }

                if (response.Status==400)
                {
                    return await Task.FromResult(StatusCode(response.Status, response));
                }


                Accounts account = new()
                {
               AccountName=form.AccountName,
               CreatedTime= DateTime.UtcNow,
               Email=form.Email,
               Password=form.Password,
                };

                _sql.Accounts.Add(account);
                await _sql.SaveChangesAsync();

                // Baþarýlý iþlemin cevabý

                var data = _sql.Accounts.Where(k => k.AccountID == account.AccountID).FirstOrDefault();
                if (data != null)
                {
                    
                    response.Item = new MAccount.Response
                    {
                        
                      IsDeleted = data.IsDeleted,
                      Email=data.Email,
                      AccountID=account.AccountID,
                      AccountName=form.AccountName,
                    
                    };
                }
                return await Task.FromResult(StatusCode(response.Status, response));
            }
            catch (Exception ex)
            {
                throw await Task.FromResult(ex);
            }

        }
        //hesap bilgileri alýnýr
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
          
                var response = new MethodResponse<MAccount.Response>();
                if (id == null)
                {
                    response.Status = 400;
                    response.StatusTexts.Add("Hesap bulunamadý");
                    return await Task.FromResult(StatusCode(response.Status, response));
                }

                var data=_sql.Accounts.Where(k=>k.AccountID==id&&k.IsDeleted==false).FirstOrDefault();
                if (data!=null)
                {
                    response.Item = new MAccount.Response
                    {
                        IsDeleted = data.IsDeleted,
                        Email = data.Email,
                        AccountID = data.AccountID,
                        AccountName = data.AccountName,
                    };
                }

                return await Task.FromResult(StatusCode(response.Status, response));
            }
            catch (Exception ex)
            {
                throw await Task.FromResult(ex);
            }

        }


        //hesapp giriþi kontrolü
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromForm] MAccount.Form form)
        {
            try
            {
                var response = new MethodResponse<MAccount.Response>();
             
                if (form.Email == null)
                {
                    response.Status = 400;
                    response.StatusTexts.Add("Hatalý giriþ");
                    return await Task.FromResult(StatusCode(response.Status, response));
                }
                if (form.Password == null)
                {
                    response.Status = 400;
                    response.StatusTexts.Add("Hatalý giriþ");
                    return await Task.FromResult(StatusCode(response.Status, response));
                }
                var data = _sql.Accounts.Where(k => k.Email == form.Email && k.IsDeleted == false && form.Password == k.Password)?.FirstOrDefault();
                if (data==null)
                {
                    response.Status = 400;
                    response.StatusTexts.Add("Hatalý Giriþ");
                    return await Task.FromResult(StatusCode(response.Status, response));
                }

                if (data != null)
                {
                    response.Item = new MAccount.Response
                    {
                        IsDeleted = data.IsDeleted,
                        Email = data.Email,
                        AccountID = data.AccountID,
                        AccountName = data.AccountName,
                    };
                }

                return await Task.FromResult(StatusCode(response.Status, response));
            }
            catch (Exception ex)
            {
                throw await Task.FromResult(ex);
            }    

        }

        // E-posta doðrulama fonksiyonu
        private bool IsValidEmail(string email)
        {
            try
            {
                var mailAddress = new System.Net.Mail.MailAddress(email);
                return mailAddress.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
