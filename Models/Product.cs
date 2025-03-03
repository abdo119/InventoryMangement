namespace InventoryManagementSystem.Models;

public class Product
{
    public int ProductID { get; set; }
    public string ProductName { get; set; }
    public string Description { get; set; }
    public int QuantityInStock { get; set; }
    public decimal Price { get; set; }
    public string SupplierName { get; set; }
}