
using Api.Data;
using Api.DataModel;
using ChatApi.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;


namespace ChatApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeesController : ControllerBase
    {


     
        private readonly DataContext _sql;
        public EmployeesController( DataContext sql)
        {
          
            _sql = sql;
        }


        //çalýþan kayýt iþlemi yapýlýr
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] MEmployees.Form form)
        {
            var response = new MethodResponse<MEmployees.Response>();

            try
            {
                var emailControl = _sql.Employees.Where(k => k.Email == form.Email && k.IsDeleted == false)?.FirstOrDefault();

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


                if (string.IsNullOrEmpty(form.Name))
                {
                    response.Status = 400;
                    response.StatusTexts.Add("isim boþ olamaz");
                }

                if (string.IsNullOrEmpty(form.Position))
                {
                    response.Status = 400;
                    response.StatusTexts.Add("Pozisyon bilgisi boþ olamaz");
                }


                if (response.Status == 400)
                {
                    return await Task.FromResult(StatusCode(response.Status, response));
                }


                Employees employe = new()
                {
                    Name = form.Name,
                    CreatedTime = DateTime.Now,
                    CreatedBy = form.AccountID,
                    Email = form.Email,
                    DepartmentID = form.DepartmentID,
                    Position = form.Position,
                };

                _sql.Employees.Add(employe);
                await _sql.SaveChangesAsync();

                

                var data = _sql.Employees.Where(k => k.EmployeeID == employe.EmployeeID).FirstOrDefault();
                if (data != null)
                {

                    response.Item = new MEmployees.Response
                    {

                        IsDeleted = data.IsDeleted,
                        Email = data.Email,
                        EmployeeID = employe.EmployeeID,
                        Name = data.Name,
                        Department = data.Department,
                        Position = data.Position,

                    };
                }

                response.Status = 200;
                response.StatusTexts.Add("Çalýþan baþarýyla güncellendi");
                return await Task.FromResult(StatusCode(response.Status, response));
            }
            catch (Exception ex)
            {
                throw await Task.FromResult(ex);
            }

        }
        //çalýþan bilgileri alýnýr
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {

                var response = new MethodResponse<MEmployees.Response>();
                if (id == null)
                {
                    response.Status = 400;
                    response.StatusTexts.Add("Çalýþan bulunamadý");
                    return await Task.FromResult(StatusCode(response.Status, response));
                }

                var data = _sql.Employees.Where(k => k.EmployeeID == id && k.IsDeleted == false).FirstOrDefault();
                if (data != null)
                {
                    response.Item = new MEmployees.Response
                    {
                        IsDeleted = data.IsDeleted,
                        Email = data.Email,
                        EmployeeID = data.EmployeeID,
                        Name = data.Name,
                        DepartmentID = data.DepartmentID,
                        Position = data.Position,
                        Department = data.DepartmentID != null ? _sql.Departments.Where(d => d.DepartmentID == data.DepartmentID && d.IsDeleted == false).FirstOrDefault() : null
                    };
                }

                return await Task.FromResult(StatusCode(response.Status, response));
            }
            catch (Exception ex)
            {
                throw await Task.FromResult(ex);
            }

        }

        //güncelle
        [HttpPut]
        public async Task<IActionResult> Put([FromForm] MEmployees.Form form)
        {
            try
            {
                var response = new MethodResponse<MEmployees.Response>();
                var emailControl = _sql.Employees.Where(k => k.Email == form.Email && k.IsDeleted == false&&k.EmployeeID!=form.EmployeeID)?.FirstOrDefault();

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


                if (string.IsNullOrEmpty(form.Name))
                {
                    response.Status = 400;
                    response.StatusTexts.Add("isim boþ olamaz");
                }

                if (string.IsNullOrEmpty(form.Position))
                {
                    response.Status = 400;
                    response.StatusTexts.Add("Pozisyon bilgisi boþ olamaz");
                }


                if (response.Status == 400)
                {
                    return await Task.FromResult(StatusCode(response.Status, response));
                }
                var data = _sql.Employees.Where(k => k.EmployeeID == form.EmployeeID && k.IsDeleted == false).FirstOrDefault();
                if (data == null)
                {
                    response.Status = 400;
                    response.StatusTexts.Add("Çalýþan bulunamadý");
                    return await Task.FromResult(StatusCode(response.Status, response));
                }


                data.DepartmentID = form.DepartmentID;
                data.Name = form.Name;
                data.Email = form.Email;
                data.Position = form.Position;



                _sql.Employees.Update(data);
                await _sql.SaveChangesAsync();

                response.Status = 200;
                response.StatusTexts.Add("Çalýþan baþarýyla güncellendi");

                return await Task.FromResult(StatusCode(response.Status, response));

            }
            catch (Exception ex)
            {

                throw await Task.FromResult(ex);
            }
        }
        //sil
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = new MethodResponse<MEmployees.Response>();
                var data = _sql.Employees.Where(k => k.EmployeeID == id && k.IsDeleted == false).FirstOrDefault();
                if (data == null)
                {
                    response.Status = 400;
                    response.StatusTexts.Add("Çalýþan bulunamadý");
                    return await Task.FromResult(StatusCode(response.Status, response));
                }


                data.IsDeleted = true;
                data.DepartmentID = null;



                _sql.Employees.Update(data);
                await _sql.SaveChangesAsync();

                response.Status = 200;
                response.StatusTexts.Add("Çalýþan baþarýyla silindi");

                return await Task.FromResult(StatusCode(response.Status, response));

            }
            catch (Exception ex)
            {

                throw await Task.FromResult(ex);
            }
        }

        //tüm departmanlarýmý bul 
        [HttpPost]
        [Route("MultipleGet")]
        public async Task<IActionResult> MultipleGet([FromForm] MEmployees.Form form)
        {
            try
            {
                MethodResponse<List<MEmployees.Response>> response = new();
             
                var data = _sql.Employees
                         .Where(k => k.CreatedBy == form.AccountID && k.IsDeleted == false)
                         .Where(k => form.DepartmentID == null || k.DepartmentID == form.DepartmentID)
                         .Select(k => new MEmployees.Response
                         {
                             DepartmentID = k.DepartmentID,
                             EmployeeID = k.EmployeeID,
                             Name = k.Name,
                             CreatedTime = k.CreatedTime,
                             CreatedBy = k.CreatedBy,
                             Position = k.Position,
                             Department = k.DepartmentID != null ? _sql.Departments.Where(d => d.DepartmentID == k.DepartmentID && d.IsDeleted == false).FirstOrDefault() : null
                         })
                         .ToList();

                response.Item = data;


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
