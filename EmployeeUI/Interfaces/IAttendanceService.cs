using EmployeeUI.Models;

namespace EmployeeUI.Interfaces
{
    public interface IAttendanceService
    {
        Task<bool> MarkAttendanceAsync(AttendanceEntry entry);
        Task<List<Attendance>> GetFilteredAsync(AttendanceFilter filter);
    }

}
