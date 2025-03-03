using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using InventoryManagementSystem.DataAccess;
using InventoryManagementSystem.Models;
using Newtonsoft.Json;

namespace InventoryManagementSystem.Repository;

public class ProductRepository
{
    // 🔹 1. Add a new product
    public static bool AddProduct(Product product, string username)
    {
        using (var conn = DatabaseHelper.GetConnection())
        {
            var query =
                @"
            INSERT INTO Products (ProductName, Description, QuantityInStock, Price, SupplierName) 
            VALUES (@ProductName, @Description, @QuantityInStock, @Price, @SupplierName);
            SELECT CAST(SCOPE_IDENTITY() AS INT);";
            var cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
            cmd.Parameters.AddWithValue("@Description", product.Description);
            cmd.Parameters.AddWithValue("@QuantityInStock", product.QuantityInStock);
            cmd.Parameters.AddWithValue("@Price", product.Price);
            cmd.Parameters.AddWithValue("@SupplierName", product.SupplierName);

            try
            {
                conn.Open();

                var result = cmd.ExecuteScalar();
                var productId = result != null && int.TryParse(result.ToString(), out var id) ? id : 0;

                if (productId > 0)
                {
                    var p = GetProductById(productId);
                    var json = JsonConvert.SerializeObject(p, Formatting.Indented);
                    var oldValue = "{}";
                    LogAudit("Add", productId, username, oldValue, json);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding product: {ex.Message}");
                return false;
            }
        }
    }

    // 🔹 2. Retrieve all products
    public static List<Product> GetAllProducts()
    {
        var products = new List<Product>();
        using (var conn = DatabaseHelper.GetConnection())
        {
            var query = "SELECT * FROM Products";
            var cmd = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                    products.Add(new Product
                    {
                        ProductID = Convert.ToInt32(reader["ProductID"]),
                        ProductName = reader["ProductName"].ToString(),
                        Description = reader["Description"].ToString(),
                        QuantityInStock = Convert.ToInt32(reader["QuantityInStock"]),
                        Price = Convert.ToDecimal(reader["Price"]),
                        SupplierName = reader["SupplierName"].ToString()
                    });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving products: {ex.Message}");
            }
        }

        return products;
    }

    public static Product GetProductById(int productId)
    {
        using (var conn = DatabaseHelper.GetConnection())
        {
            var query = "SELECT * FROM Products WHERE ProductID = @id";
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@id", productId);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                        return new Product
                        {
                            ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                            ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                            Description = reader.GetString(reader.GetOrdinal("Description")),
                            QuantityInStock = reader.GetInt32(reader.GetOrdinal("QuantityInStock")),
                            Price = reader.GetDecimal(reader.GetOrdinal("Price"))
                        };
                }
            }
        }

        return null; // Return null if product not found
    }


    // 🔹 3. Update a product
    public static bool UpdateProduct(Product product, string username)
    {
        using (var conn = DatabaseHelper.GetConnection())
        {
            var query =
                "UPDATE Products SET ProductName=@ProductName, Description=@Description, QuantityInStock=@QuantityInStock, Price=@Price, SupplierName=@SupplierName WHERE ProductID=@ProductID";
            var cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@ProductID", product.ProductID);
            cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
            cmd.Parameters.AddWithValue("@Description", product.Description);
            cmd.Parameters.AddWithValue("@QuantityInStock", product.QuantityInStock);
            cmd.Parameters.AddWithValue("@Price", product.Price);
            cmd.Parameters.AddWithValue("@SupplierName", product.SupplierName);

            try
            {
                conn.Open();
                var oldProduct = GetProductById(product.ProductID);
                var rowsAffected = cmd.ExecuteNonQuery();
                LogAudit("Update", product.ProductID, username, toJson(oldProduct),
                    toJson(GetProductById(product.ProductID)));

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating product: {ex.Message}");
                return false;
            }
        }
    }

    // 🔹 4. Delete a product
    public static bool DeleteProduct(int productId, string username)
    {
        using (var conn = DatabaseHelper.GetConnection())
        {
            var query = "DELETE FROM Products WHERE ProductID=@ProductID";
            var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@ProductID", productId);

            try
            {
                conn.Open();
                var oldProduct = GetProductById(productId);
                var rowsAffected = cmd.ExecuteNonQuery();
                var newValue = "{}";
                LogAudit("Delete", productId, username, toJson(oldProduct), newValue);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting product: {ex.Message}");
                return false;
            }
        }
    }

    // 🔹 Search Products by Name
    public static List<Product> SearchProducts(string productName, string username)
    {
        var products = new List<Product>();
        using (var conn = DatabaseHelper.GetConnection())
        {
            var query = "SELECT * FROM Products WHERE ProductName LIKE @ProductName";
            var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@ProductName", "%" + productName + "%");

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var p = new Product
                    {
                        ProductID = Convert.ToInt32(reader["ProductID"]),
                        ProductName = reader["ProductName"].ToString(),
                        Description = reader["Description"].ToString(),
                        QuantityInStock = Convert.ToInt32(reader["QuantityInStock"]),
                        Price = Convert.ToDecimal(reader["Price"]),
                        SupplierName = reader["SupplierName"].ToString()
                    };
                    var newValue = "{}";
                    var oldValue = "{}";
                    products.Add(p);
                    LogAudit("Search", p.ProductID, username, oldValue, newValue);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching products: {ex.Message}");
            }
        }

        return products;
    }

    // 🔹 Filter Products by Supplier
    public static List<Product> FilterBySupplier(string supplierName, string username)
    {
        var products = new List<Product>();
        using (var conn = DatabaseHelper.GetConnection())
        {
            var query = "SELECT * FROM Products WHERE SupplierName LIKE @SupplierName";
            var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@SupplierName", "%" + supplierName + "%");

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var p = new Product
                    {
                        ProductID = Convert.ToInt32(reader["ProductID"]),
                        ProductName = reader["ProductName"].ToString(),
                        Description = reader["Description"].ToString(),
                        QuantityInStock = Convert.ToInt32(reader["QuantityInStock"]),
                        Price = Convert.ToDecimal(reader["Price"]),
                        SupplierName = reader["SupplierName"].ToString()
                    };
                    var newValue = "{}";
                    var oldValue = "{}";
                    products.Add(p);
                    LogAudit("Search", p.ProductID, username, oldValue, newValue);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error filtering by supplier: {ex.Message}");
            }
        }

        return products;
    }

    // 🔹 Get Low Stock Products (Stock < 5)
    public static List<Product> GetLowStockProducts()
    {
        var products = new List<Product>();
        using (var conn = DatabaseHelper.GetConnection())
        {
            var query = "SELECT * FROM Products WHERE QuantityInStock < 5";
            var cmd = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                    products.Add(new Product
                    {
                        ProductID = Convert.ToInt32(reader["ProductID"]),
                        ProductName = reader["ProductName"].ToString(),
                        Description = reader["Description"].ToString(),
                        QuantityInStock = Convert.ToInt32(reader["QuantityInStock"]),
                        Price = Convert.ToDecimal(reader["Price"]),
                        SupplierName = reader["SupplierName"].ToString()
                    });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving low-stock products: {ex.Message}");
            }
        }

        return products;
    }

    private static void LogAudit(string action, int productId, string username, string oldProduct, string newProduct)
    {
        using (var conn = DatabaseHelper.GetConnection())
        {
            var query =
                "INSERT INTO AuditLogs (ActionType, ProductId, Username, Timestamp, OldValues, NewValues) VALUES (@action, @prodId, @user, @time, @old, @new)";
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@action", action);
                cmd.Parameters.AddWithValue("@prodId", productId);
                cmd.Parameters.AddWithValue("@user", username);
                cmd.Parameters.AddWithValue("@time", DateTime.Now);
                cmd.Parameters.AddWithValue("@old", oldProduct);
                cmd.Parameters.AddWithValue("@new", newProduct);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }

    public static string toJson(Product product)
    {
        return JsonConvert.SerializeObject(product);
    }
}