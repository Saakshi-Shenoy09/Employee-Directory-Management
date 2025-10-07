using EmployeeAPI.DataAccessQueries;
using EmployeeAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace EmployeeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttendanceController : ControllerBase
    {
        private readonly AttendanceQueries _attendanceQueries;
        public AttendanceController(IConfiguration configuration)
        {
            _attendanceQueries = new AttendanceQueries(configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string not found."));
        }

        /// <summary>
        /// Mark attendance for an employee.
        /// </summary>
        /// <param name="attendance">Attendance object containing EmployeeId, Date, and Status</param>
        [HttpPost]
        public async Task<ActionResult<Attendance>> MarkAttendance([FromBody] Attendance attendance)
        {
            var success = await _attendanceQueries.MarkAttendanceAsync(attendance);
            if (!success)
            {
                return BadRequest("Invalid Employee ID");
            }
            return Ok(attendance); 
        }

        /// <summary>
        /// Get attendance records for an employee within a date range.
        /// </summary>
        /// <param name="employeeId">Employee ID</param>
        /// <param name="from">Start date of the range</param>
        /// <param name="to">End date of the range</param>
        [HttpGet("{employeeId}")]
        public async Task<ActionResult<IEnumerable<Attendance>>> GetAttendance(int employeeId, [FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            var records = await _attendanceQueries.GetAttendanceAsync(employeeId, from, to);
            return Ok(records);
        }
    }
}
