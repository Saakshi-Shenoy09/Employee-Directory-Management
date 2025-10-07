using EmployeeUI.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using EmployeeUI.Interfaces;

namespace EmployeeUI.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly HttpClient _httpClient;

        public AttendanceService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<bool> MarkAttendanceAsync(AttendanceEntry entry)
        {
            var payload = new Attendance
            {
                EmployeeId = entry.SelectedEmployeeId,
                Status = entry.Status,
                Date = entry.Date
            };
            var response = await _httpClient.PostAsJsonAsync("", payload);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<Attendance>> GetFilteredAsync(AttendanceFilter filter)
        {
            string url = $"{filter.EmployeeId}?from={filter.From:yyyy-MM-dd}&to={filter.To:yyyy-MM-dd}";
            var result = await _httpClient.GetFromJsonAsync<List<Attendance>>(url);
            return result ?? new List<Attendance>();
        }
    }
}
