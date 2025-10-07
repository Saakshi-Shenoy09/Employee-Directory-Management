using EmployeeUI.Interfaces;
using EmployeeUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeUI.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IConfiguration _configuration;
        public EmployeeController(IEmployeeService employeeService, IConfiguration configuration)
        {
            _employeeService = employeeService;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index(string name, string department)
        {
            List<Employee> employees;
            if (!string.IsNullOrEmpty(name) || !string.IsNullOrEmpty(department))
            {
                employees = await _employeeService.SearchAsync(name ?? "", department ?? "");
            }
            else
            {
                employees = await _employeeService.GetAllAsync();
            }
            bool showDepartment = _configuration.GetValue<bool>("DisplaySettings:ShowDepartment");
            ViewBag.ShowDepartment = showDepartment;

            return View(employees);
        }
    }
}
