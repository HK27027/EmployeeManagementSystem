
using Api.Data;
using Api.DataModel;
using ChatApi.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;


namespace ChatApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DepartmentsController : ControllerBase
    {
    
      
      
        private readonly DataContext _sql;
        public DepartmentsController(DataContext sql)
        {
           
            _sql = sql;
        }


        //departman ekleme
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] MDepartments.Form form)
        {
            var response = new MethodResponse<MDepartments.Response>();

            try
            {
               
            
                if (string.IsNullOrEmpty(form.DepartmentName))
                {
                    response.Status = 400;
                    response.StatusTexts.Add("Departman ismi boþ olamaz");
                }

                var department = _sql.Departments.Where(k => k.DepartmentName == form.DepartmentName && k.IsDeleted == false
                &&k.CreatedBy==form.AccountID).Any();
                if (department)
                {
                    response.Status = 400;
                    response.StatusTexts.Add("Departman ismi kullanýlýyor");
                }
                if (response.Status==400)
                {
                    return await Task.FromResult(StatusCode(response.Status, response));
                }

              
                Departments departments = new()
                {
               DepartmentName = form.DepartmentName,
               CreatedBy = form.AccountID,
               CreatedTime = DateTime.UtcNow,
               
                };

                _sql.Departments.Add(departments);
                await _sql.SaveChangesAsync();

                // Baþarýlý iþlemin cevabý

                var data = _sql.Departments.Where(k => k.DepartmentID == departments.DepartmentID).FirstOrDefault();
                if (data != null)
                {
                    
                    response.Item = new MDepartments.Response
                    {
                        
                      IsDeleted = data.IsDeleted,
                      DepartmentID=data.DepartmentID,
                      DepartmentName=data.DepartmentName,
                    
                    };
                }
                response.Status = 200;
                response.StatusTexts.Add("Departman baþarýyla eklendi");
                return await Task.FromResult(StatusCode(response.Status, response));
            }
            catch (Exception ex)
            {
                throw await Task.FromResult(ex);
            }

        }
        //tek  bir departmanýn  bilgileri alýnýr
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
          
                var response = new MethodResponse<MDepartments.Response>();
                if (id == null)
                {
                    response.Status = 400;
                    response.StatusTexts.Add("Hesap bulunamadý");
                    return await Task.FromResult(StatusCode(response.Status, response));
                }

                var data=_sql.Departments.Where(k=>k.DepartmentID==id&&k.IsDeleted==false).FirstOrDefault();
                if (data!=null)
                {
                    response.Item = new MDepartments.Response
                    {
                        IsDeleted = data.IsDeleted,
                        DepartmentID = data.DepartmentID,
                        DepartmentName = data.DepartmentName,
                        CreatedBy = data.CreatedBy,
                        CreatedTime=data.CreatedTime,
                        Employees=_sql.Employees.Where(k=>k.DepartmentID==data.DepartmentID&&k.IsDeleted==false).ToList().Select(k=> new MEmployees.Response
                        {
                            EmployeeID=k.EmployeeID,
                            IsDeleted=k.IsDeleted,
                            DepartmentID=k.DepartmentID,
                            CreatedBy=k.CreatedBy,
                            CreatedTime = k.CreatedTime,
                            Name = k.Name,
                            Position=k.Position,
                            Email=k.Email,
                            
                        }).ToList(),
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
        public async Task<IActionResult> Put([FromForm] MDepartments.Form form)
        {
            try
            {
                var response = new MethodResponse<MDepartments.Response>();
                var data = _sql.Departments.Where(k => k.DepartmentID == form.DepartmentID && k.IsDeleted == false).FirstOrDefault();
                if(data==null) {
                    response.Status = 400;
                    response.StatusTexts.Add("Departman bulunamadý");
                    return await Task.FromResult(StatusCode(response.Status, response));
                }
                if (string.IsNullOrEmpty(form.DepartmentName))
                {
                    response.Status = 400;
                    response.StatusTexts.Add("Departman ismi boþ olamaz");
                    return await Task.FromResult(StatusCode(response.Status, response));
                }
                var department = _sql.Departments.Where(k => k.DepartmentName == form.DepartmentName && k.IsDeleted == false
            && k.CreatedBy == form.AccountID&&k.DepartmentID!=form.DepartmentID).Any();
                if (department)
                {
                    response.Status = 400;
                    response.StatusTexts.Add("Departman ismi kullanýlýyor");
                }

                data.DepartmentName = form.DepartmentName;
               

             
                _sql.Departments.Update(data);
                await _sql.SaveChangesAsync();

                response.Status = 200;
                response.StatusTexts.Add("Departman baþarýyla güncellendi");

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
                var response = new MethodResponse<MDepartments.Response>();
                //silinecek departmanýn çalýþanlarýndaki departman alaný kaldýrýlýr
                var users = _sql.Employees.Where(k => k.DepartmentID == id).ToList();
                foreach (var item in users)
                {
                    item.DepartmentID = null;



                    _sql.Employees.Update(item);
                    await _sql.SaveChangesAsync();
                }


                var data = _sql.Departments.Where(k => k.DepartmentID == id && k.IsDeleted == false).FirstOrDefault();
                if (data == null)
                {
                    response.Status = 400;
                    response.StatusTexts.Add("Departman bulunamadý");
                    return await Task.FromResult(StatusCode(response.Status, response));
                }


                data.IsDeleted = true;



                _sql.Departments.Update(data);
                await _sql.SaveChangesAsync();

                response.Status = 200;
                response.StatusTexts.Add("Departman baþarýyla silindi");

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
        public async Task<IActionResult> MultipleGet([FromForm] MDepartments.Form form)
        {
            try
            {
                MethodResponse<List<MDepartments.Response>> response = new();


                var data = _sql.Departments
                         .Where(k => k.CreatedBy == form.AccountID && k.IsDeleted == false)
                         .Select(k => new MDepartments.Response
                         {
                             DepartmentID = k.DepartmentID,
                             DepartmentName = k.DepartmentName,
                             CreatedTime = k.CreatedTime ,
                             CreatedBy=k.CreatedBy,
                             EmpolyeCount =_sql.Employees.Where(e=>e.DepartmentID==k.DepartmentID&&k.IsDeleted==false).ToList().Count                  
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

    }
}
