using System.ComponentModel.DataAnnotations;

namespace EmployeeAPI.Models
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
