namespace EmployeeUI.Models
{
    public class AttendanceFilter
    {
        public int EmployeeId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public List<Employee>? Employees { get; set; }
        public List<Attendance>? Results { get; set; }
    }
}
