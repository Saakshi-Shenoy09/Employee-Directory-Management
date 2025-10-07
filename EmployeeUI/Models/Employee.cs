namespace EmployeeUI.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string ?Name { get; set; }
        public string ?Email { get; set; }
        public string ?Department { get; set; }
        public DateTime JoinDate { get; set; }
    }
}
