using EmployeeAPI.Models;
using Microsoft.Data.SqlClient;

namespace EmployeeAPI.DataAccessQueries
{
    public class AttendanceQueries
    {
        private readonly string _connectionString;

        public AttendanceQueries(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<bool> MarkAttendanceAsync(Attendance attendance)
        {
            //checking for employee existence
            const string checkQuery = "SELECT COUNT(*) FROM Employees WHERE EmployeeId = @EmployeeId";
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using(var checkCmd = new SqlCommand(checkQuery, conn))
            {
                checkCmd.Parameters.AddWithValue("@EmployeeId", attendance.EmployeeId);
                var exists = (int)(await checkCmd.ExecuteScalarAsync() ?? 0);
                if (exists == 0)
                {
                    return false; //if employee does not exist
                }
            }

            //inserting attendance if the employee exists
            const string insertQuery = @"
                INSERT INTO Attendances (EmployeeId, Date, Status)
                VALUES (@EmployeeId, @Date, @Status)";

            using var cmd = new SqlCommand(insertQuery, conn);
            cmd.Parameters.AddWithValue("@EmployeeId", attendance.EmployeeId);
            cmd.Parameters.AddWithValue("@Date", attendance.Date);
            cmd.Parameters.AddWithValue("@Status", attendance.Status);

            var rowsAffected = await cmd.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        public async Task<List<Attendance>> GetAttendanceAsync(int employeeId, DateTime from, DateTime to)
        {
            var records = new List<Attendance>();
            const string query = @"
                SELECT AttendanceId, EmployeeId, Date, Status 
                FROM Attendances
                WHERE EmployeeId = @EmployeeId AND Date >= @From AND Date <= @To";

            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@EmployeeId", employeeId);
            cmd.Parameters.AddWithValue("@From", from);
            cmd.Parameters.AddWithValue("@To", to);

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                records.Add(new Attendance
                {
                    AttendanceId = reader.GetInt32(0),
                    EmployeeId = reader.GetInt32(1),
                    Date = reader.GetDateTime(2),
                    Status = reader.GetString(3)
                });
            }
            return records;
        }
    }
}
