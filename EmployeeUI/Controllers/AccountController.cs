using EmployeeUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly HRUser _hrUser;

        public AccountController(IConfiguration config)
        {
            _hrUser = config.GetSection("HRUser").Get<HRUser>();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (username == _hrUser.Username && password == _hrUser.Password)
            {
                HttpContext.Session.SetString("HRUser", username);
                return View();
            }
            ViewBag.Error = "Invalid credentials";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
