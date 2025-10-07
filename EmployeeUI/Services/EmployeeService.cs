using EmployeeUI.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using EmployeeUI.Interfaces;

namespace EmployeeUI.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly HttpClient _httpClient;

        public EmployeeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Employee>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Employee>>("") ?? new List<Employee>();
        }

        public async Task<List<Employee>> SearchAsync(string name, string department)
        {
            var url = $"?name={name}&department={department}";
            return await _httpClient.GetFromJsonAsync<List<Employee>>(url) ?? new List<Employee>();
        }
    }
}
