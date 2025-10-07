using EmployeeAPI.Data;
using EmployeeAPI.DataAccessQueries;
using EmployeeAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly DepartmentQueries _departmentQueries;
        public DepartmentController(IConfiguration configuration)
        {
            _departmentQueries = new DepartmentQueries(configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string not found."));
        }

        /// <summary>
        /// Get all departments.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> GetAllDepartments()
        {
            var departments = await _departmentQueries.GetAllDepartmentsAsync();
            return Ok(departments);
        }

        /// <summary>
        /// Get a department by ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Department>> GetDepartment(int id)
        {
            var department = await _departmentQueries.GetDepartmentByIdAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            return Ok(department);
        }

        /// <summary>
        /// Create a new department.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Department>> CreateDepartment([FromBody] Department department)
        {
            var newId = await _departmentQueries.CreateDepartmentAsync(department);
            department.DepartmentId = newId;
            return CreatedAtAction(nameof(GetDepartment), new { id = newId }, department);
        }

        /// <summary>
        /// Update an existing department.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment(int id, [FromBody] Department department)
        {
            if (id != department.DepartmentId)
            {
                return BadRequest("ID mismatch.");
            }
            var updated = await _departmentQueries.UpdateDepartmentAsync(id, department);
            if (!updated)
            {
                return NotFound();
            }
            return NoContent();
        }

        /// <summary>
        /// Delete a department by ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var deleted = await _departmentQueries.DeleteDepartmentAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
