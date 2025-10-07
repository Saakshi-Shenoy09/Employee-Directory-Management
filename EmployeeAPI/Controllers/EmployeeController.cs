using EmployeeAPI.DataAccessQueries;
using EmployeeAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeQueries _empQueries;
        public EmployeeController(IConfiguration configuration)
        {
            _empQueries = new EmployeeQueries(configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string not found."));
        }

        /// <summary>
        /// Get all employees (with optional filtering).
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees([FromQuery] string? name, [FromQuery] string? department)
        {
            var employees = await _empQueries.GetEmployeesAsync(name, department);
            return Ok(employees);
        }

        /// <summary>
        /// Get a single employee by ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = await _empQueries.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }

        /// <summary>
        /// Create a new employee.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Employee>> CreateEmployee([FromBody] Employee employee)
        {
            var newId = await _empQueries.CreateEmployeeAsync(employee);
            employee.EmployeeId = newId;
            return CreatedAtAction(nameof(GetEmployee), new { id = newId }, employee);
        }

        /// <summary>
        /// Update an existing employee.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id,[FromBody] Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return BadRequest("ID Mistmatch");
            }
            var success = await _empQueries.UpdateEmployeeAsync(id, employee);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }

        /// <summary>
        /// Delete an employee by ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var success = await _empQueries.DeleteEmployeeAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
