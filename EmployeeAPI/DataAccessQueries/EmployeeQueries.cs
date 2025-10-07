using EmployeeAPI.Models;
using Microsoft.Data.SqlClient;

namespace EmployeeAPI.DataAccessQueries
{
    public class EmployeeQueries
    {
        private readonly string _connectionString;

        public EmployeeQueries(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<Employee>> GetEmployeesAsync(string? name, string? department)
        {
            var employees = new List<Employee>();
            var query = @"SELECT EmployeeId, Name, Email, Department, JoinDate FROM Employees WHERE 1=1";

            if (!string.IsNullOrEmpty(name))
            {
                query += " AND Name LIKE '%' + @Name + '%'";
            }

            if (!string.IsNullOrEmpty(department))
            {
                query += " AND Department LIKE '%' + @Department + '%'";
            }
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            using var cmd = new SqlCommand(query, conn);

            if (!string.IsNullOrEmpty(name))
            {
                cmd.Parameters.AddWithValue("@Name", name);
            }

            if (!string.IsNullOrEmpty(department))
            {
                cmd.Parameters.AddWithValue("@Department", department);
            }
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                employees.Add(new Employee
                {
                    EmployeeId = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Email = reader.GetString(2),
                    Department = reader.GetString(3),
                    JoinDate = reader.GetDateTime(4)
                });
            }
            return employees;
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int id)
        {
            const string query = @"SELECT EmployeeId, Name, Email, Department, JoinDate FROM Employees WHERE EmployeeId = @Id";

            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Id", id);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Employee
                {
                    EmployeeId = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Email = reader.GetString(2),
                    Department = reader.GetString(3),
                    JoinDate = reader.GetDateTime(4)
                };
            }
            return null;
        }

        public async Task<int> CreateEmployeeAsync(Employee emp)
        {
            const string query = @"
                INSERT INTO Employees (Name, Email, Department, JoinDate)
                OUTPUT INSERTED.EmployeeId
                VALUES (@Name, @Email, @Department, @JoinDate);";

            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Name", emp.Name);
            cmd.Parameters.AddWithValue("@Email", emp.Email);
            cmd.Parameters.AddWithValue("@Department", emp.Department);
            cmd.Parameters.AddWithValue("@JoinDate", emp.JoinDate);

            var insertedId = (int)(await cmd.ExecuteScalarAsync() ?? 0);
            return insertedId;
        }

        public async Task<bool> UpdateEmployeeAsync(int id, Employee emp)
        {
            const string query = @"
                UPDATE Employees
                SET Name = @Name,
                    Email = @Email,
                    Department = @Department,
                    JoinDate = @JoinDate
                WHERE EmployeeId = @Id";

            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Name", emp.Name);
            cmd.Parameters.AddWithValue("@Email", emp.Email);
            cmd.Parameters.AddWithValue("@Department", emp.Department);
            cmd.Parameters.AddWithValue("@JoinDate", emp.JoinDate);
            cmd.Parameters.AddWithValue("@Id", id);

            var affectedRows = await cmd.ExecuteNonQueryAsync();
            return affectedRows > 0;
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            const string query = @"DELETE FROM Employees WHERE EmployeeId = @Id";

            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Id", id);

            var affectedRows = await cmd.ExecuteNonQueryAsync();
            return affectedRows > 0;
        }
    }
}
