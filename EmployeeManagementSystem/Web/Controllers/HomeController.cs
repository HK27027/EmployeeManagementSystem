using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Route("Register")]
        public IActionResult Register()
        {
            return View("Register");
        }
        [Route("Login")]
        public IActionResult Login()
        {
            return View("Login");
        }

        [Route("DepartmentDetail/{id}")]
        public IActionResult DepartmentDetail(int id)
        {
            
            return View();
        }
        [Route("EmployeeDetail/{id}")]
        public IActionResult EmployeeDetail(int id)
        {

            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
