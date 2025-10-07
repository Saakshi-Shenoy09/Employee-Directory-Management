using EmployeeUI.Interfaces;
using EmployeeUI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
namespace EmployeeUI.ViewComponents
{
    public class NewJoineesEmpViewComponent : ViewComponent
    {
        private readonly IEmployeeService _employeeService;

        public NewJoineesEmpViewComponent(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var employees = await _employeeService.GetAllAsync();
            var recentEmps = employees.OrderByDescending(e => e.JoinDate).Take(5).ToList();
            return View(recentEmps);
        }
    }
}
