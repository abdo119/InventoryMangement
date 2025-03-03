using System.Data.SqlClient;

namespace InventoryManagementSystem.DataAccess;

public static class DatabaseHelper
{
    private static readonly string connectionString =
        "Server=localhost;Database=InventoryDB;User Id=SA;Password=YourStrong!Passw0rd;TrustServerCertificate=true;";

    public static SqlConnection GetConnection()
    {
        return new SqlConnection(connectionString);
    }
}