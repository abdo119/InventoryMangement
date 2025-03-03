using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using InventoryManagementSystem.DataAccess;
using InventoryManagementSystem.Models;

namespace InventoryManagementSystem.Repository;

public class AuthRepository
{
    // 🔹 Hash a password using SHA-256
    private static string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            var builder = new StringBuilder();
            foreach (var b in bytes) builder.Append(b.ToString("x2"));
            return builder.ToString();
        }
    }

    // 🔹 Authenticate user
    public static User AuthenticateUser(string username, string password)
    {
        using (var conn = DatabaseHelper.GetConnection())
        {
            var query =
                "SELECT UserID, Username, Role FROM Users WHERE Username=@Username AND PasswordHash=@PasswordHash";
            var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Username", username);
            cmd.Parameters.AddWithValue("@PasswordHash", HashPassword(password));

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                    return new User
                    {
                        UserID = Convert.ToInt32(reader["UserID"]),
                        Username = reader["Username"].ToString(),
                        Role = reader["Role"].ToString()
                    };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login failed: {ex.Message}");
            }
        }

        return null;
    }

    // 🔹 Register a new user
    public static bool RegisterUser(string username, string password, string role)
    {
        using (var conn = DatabaseHelper.GetConnection())
        {
            var query = "INSERT INTO Users (Username, PasswordHash, Role) VALUES (@Username, @PasswordHash, @Role)";
            var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Username", username);
            cmd.Parameters.AddWithValue("@PasswordHash", HashPassword(password));
            cmd.Parameters.AddWithValue("@Role", role);

            try
            {
                conn.Open();
                var rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error registering user: {ex.Message}");
                return false;
            }
        }
    }
}