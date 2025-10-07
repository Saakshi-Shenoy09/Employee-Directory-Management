using EmployeeAPI.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EmployeeAPI.Data
{
    public class DepartmentQueries
    {
        private readonly string _connectionString;
        public DepartmentQueries(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<Department>> GetAllDepartmentsAsync()
        {
            var departments = new List<Department>();
            const string query = "SELECT DepartmentId, Name FROM Departments";

            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            using var cmd = new SqlCommand(query, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                departments.Add(new Department
                {
                    DepartmentId = reader.GetInt32(0),
                    Name = reader.GetString(1)
                });
            }
            return departments;
        }

        public async Task<Department?> GetDepartmentByIdAsync(int id)
        {
            const string query = "SELECT DepartmentId, Name FROM Departments WHERE DepartmentId = @Id";

            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Department
                {
                    DepartmentId = reader.GetInt32(0),
                    Name = reader.GetString(1)
                };
            }
            return null;
        }

        public async Task<int> CreateDepartmentAsync(Department department)
        {
            const string query = @"
                INSERT INTO Departments (Name)
                OUTPUT INSERTED.DepartmentId
                VALUES (@Name);";

            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Name", department.Name);

            var insertedId = (int)(await cmd.ExecuteScalarAsync() ?? 0);
            return insertedId;
        }

        public async Task<bool> UpdateDepartmentAsync(int id, Department department)
        {
            const string query = @"
                UPDATE Departments
                SET Name = @Name
                WHERE DepartmentId = @Id";

            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Name", department.Name);
            cmd.Parameters.AddWithValue("@Id", id);

            var rowsAffected = await cmd.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteDepartmentAsync(int id)
        {
            const string query = "DELETE FROM Departments WHERE DepartmentId = @Id";

            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Id", id);

            var rowsAffected = await cmd.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }
    }
}
