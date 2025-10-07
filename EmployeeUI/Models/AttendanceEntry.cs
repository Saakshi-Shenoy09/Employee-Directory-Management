namespace EmployeeUI.Models
{
    public class AttendanceEntry
    {
        public int SelectedEmployeeId { get; set; }
        public string ?Status { get; set; }
        public DateTime Date { get; set; }
        public List<Employee> ?Employees { get; set; }
    }
}
