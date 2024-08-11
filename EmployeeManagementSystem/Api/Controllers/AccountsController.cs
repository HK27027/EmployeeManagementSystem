
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


        //hesap kay�t i�lemi yap�l�r
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
                    response.StatusTexts.Add("Email bo� olamaz");
                }
                else if (!IsValidEmail(form.Email))
                {
                    response.Status = 400;
                    response.StatusTexts.Add("Ge�ersiz e-posta adresi");
                }
                else if (emailControl != null)
                {
                    response.Status = 400;
                    response.StatusTexts.Add("Email kullan�l�yor");
                }
             

                if (string.IsNullOrEmpty(form.AccountName))
                {
                    response.Status = 400;
                    response.StatusTexts.Add("Hesap ismi bo� olamaz");
                }

                if (string.IsNullOrEmpty(form.Password))
                {
                    response.Status = 400;
                    response.StatusTexts.Add("�ifre bo� olamaz");
                }
                else if (form.Password.Length < 4)
                {
                    response.Status = 400;
                    response.StatusTexts.Add("�ifre en az 4 karakter uzunlu�unda olmal�d�r");
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

                // Ba�ar�l� i�lemin cevab�

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
        //hesap bilgileri al�n�r
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
          
                var response = new MethodResponse<MAccount.Response>();
                if (id == null)
                {
                    response.Status = 400;
                    response.StatusTexts.Add("Hesap bulunamad�");
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


        //hesapp giri�i kontrol�
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
                    response.StatusTexts.Add("Hatal� giri�");
                    return await Task.FromResult(StatusCode(response.Status, response));
                }
                if (form.Password == null)
                {
                    response.Status = 400;
                    response.StatusTexts.Add("Hatal� giri�");
                    return await Task.FromResult(StatusCode(response.Status, response));
                }
                var data = _sql.Accounts.Where(k => k.Email == form.Email && k.IsDeleted == false && form.Password == k.Password)?.FirstOrDefault();
                if (data==null)
                {
                    response.Status = 400;
                    response.StatusTexts.Add("Hatal� Giri�");
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

        // E-posta do�rulama fonksiyonu
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
