using EmployeeUI.Interfaces;
using EmployeeUI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EmployeeUI.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IEmployeeService employeeService, IAttendanceService attendanceService)
        {
            _employeeService = employeeService;
            _attendanceService = attendanceService;
        }

        [HttpGet]
        public async Task<IActionResult> Mark()
        {
            if (HttpContext.Session.GetString("HRUser") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var employees = await _employeeService.GetAllAsync();
            var viewModel = new AttendanceEntry
            {
                Employees = employees,
                Date = DateTime.Today
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Mark(AttendanceEntry entry)
        {
            if (HttpContext.Session.GetString("HRUser") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (!ModelState.IsValid)
            {
                entry.Employees = await _employeeService.GetAllAsync();
                return View(entry);
            }
            var success = await _attendanceService.MarkAttendanceAsync(entry); 

            if (success)
            {
                TempData["Success"] = "Attendance marked successfully!";
            }
            else
            {
                TempData["Error"] = "Failed to mark attendance.";
            }
            return RedirectToAction("Mark");
        }


        [HttpGet]
        public async Task<IActionResult> Report()
        {
            if (HttpContext.Session.GetString("HRUser") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var filter = new AttendanceFilter
            {
                Employees = await _employeeService.GetAllAsync(),
                From = DateTime.Today,
                To = DateTime.Today
            };
            return View(filter);
        }

        [HttpPost]
        public async Task<IActionResult> Report(AttendanceFilter filter)
        {
            if (HttpContext.Session.GetString("HRUser") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            filter.Employees = await _employeeService.GetAllAsync();
            if (filter.EmployeeId > 0)
            {
                filter.Results = await _attendanceService.GetFilteredAsync(filter);
            }
            return View(filter);
        }
    }
}
