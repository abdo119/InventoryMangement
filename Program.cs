using System;
using System.Collections.Generic;
using System.Windows.Forms;
using InventoryManagementSystem.Models;
using InventoryManagementSystem.Repository;

namespace InventoryManagementSystem;

internal class Program
{
    [STAThread]
    public static void Main()
    {
        User loggedInUser = null;
        Console.WriteLine("Welcome to the Inventory Management System!");

        while (loggedInUser == null)
        {
            Console.WriteLine("\n1- Sign Up   2- Sign In  3- Exit");
            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    RegisterUser();
                    break;
                case "2":
                    loggedInUser = AuthenticateUser();
                    if (loggedInUser == null)
                        Console.WriteLine("Invalid credentials. Try again.");
                    break;
                case "3":
                    Console.WriteLine("Exiting...");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Try again.");
                    break;
            }
        }

        Console.WriteLine($"Welcome, {loggedInUser.Username} ({loggedInUser.Role})!");

        while (true)
            if (loggedInUser.Role == "Admin")
            {
                AdminMenu(loggedInUser);
                break;
            }
            else if (loggedInUser.Role == "Viewer")
            {
                ViewerMenu(loggedInUser);
                break;
            }
            else
            {
                Console.WriteLine("Invalid role detected.");
                break;
            }
    }

    private static void RegisterUser()
    {
        Console.WriteLine("Enter Username:");
        var username = Console.ReadLine();
        Console.WriteLine("Enter Password:");
        var password = Console.ReadLine();
        AuthRepository.RegisterUser(username, password, "Viewer");
        Console.WriteLine("User registered successfully!");
    }

    private static User AuthenticateUser()
    {
        Console.Write("Enter Username: ");
        var username = Console.ReadLine();
        Console.Write("Enter Password: ");
        var password = Console.ReadLine();
        return AuthRepository.AuthenticateUser(username, password);
    }

    private static void AdminMenu(User loggedInUser)
    {
        while (true)
        {
            Console.WriteLine("\nAdmin Menu:");
            Console.WriteLine(
                "1. View Products\n2. Add Product\n3. Update Product\n4. Delete Product\n5. Search Product\n6. Low Stock Alert\n7. Search Using Terlik Grid\n8. Logout");
            Console.Write("Choose an option: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ViewProducts();
                    break;
                case "2":
                    AddProduct(loggedInUser);
                    break;
                case "3":
                    UpdateProduct(loggedInUser);
                    break;
                case "4":
                    DeleteProduct(loggedInUser);
                    break;
                case "5":
                    SearchProduct(loggedInUser);
                    break;
                case "6":
                    LowStockAlert();
                    break;
                case "7":
                    SearchUsingTalkGrid();
                    break;
                case "8":
                    Console.WriteLine("Logging out...");
                    return;

                default:
                    Console.WriteLine("Invalid option. Try again.");
                    break;
            }
        }
    }

    private static void ViewerMenu(User loggedInUser)
    {
        while (true)
        {
            Console.WriteLine("\nViewer Menu:");
            Console.WriteLine("1. View Products\n2. Search Product\n3. Search Using Terlik Grid\n4. Logout");
            Console.Write("Choose an option: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ViewProducts();
                    break;
                case "2":
                    SearchProduct(loggedInUser);
                    break;

                case "3":
                    SearchUsingTalkGrid();
                    break;
                case "4":
                    Console.WriteLine("Logging out...");
                    return;
                default:
                    Console.WriteLine("Invalid option. Try again.");
                    break;
            }
        }
    }

    private static void ViewProducts()
    {
        var products = ProductRepository.GetAllProducts();
        foreach (var product in products)
            Console.WriteLine($"{product.ProductID}: {product.ProductName} - {product.QuantityInStock} in stock");
    }

    private static void AddProduct(User loggedInUser)
    {
        try
        {
            Console.Write("Product Name: ");
            var name = Console.ReadLine();
            Console.Write("Description: ");
            var desc = Console.ReadLine();
            Console.Write("Quantity: ");
            var qty = int.Parse(Console.ReadLine());
            Console.Write("Price: ");
            var price = decimal.Parse(Console.ReadLine());
            Console.Write("Supplier: ");
            var supplier = Console.ReadLine();

            var newProduct = new Product
            {
                ProductName = name,
                Description = desc,
                QuantityInStock = qty,
                Price = price,
                SupplierName = supplier
            };

            if (ProductRepository.AddProduct(newProduct, loggedInUser.Username))
                Console.WriteLine("Product added successfully!");
            else
                Console.WriteLine("Failed to add product.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: Invalid input. Please try again.");
        }
    }

    private static void UpdateProduct(User loggedInUser)
    {
        try
        {
            Console.Write("Enter Product ID to update: ");
            var updateId = int.Parse(Console.ReadLine());
            var existingProduct = ProductRepository.GetProductById(updateId);

            if (existingProduct != null)
            {
                Console.Write("New Name: ");
                existingProduct.ProductName = Console.ReadLine();
                Console.Write("New Description: ");
                existingProduct.Description = Console.ReadLine();
                Console.Write("New Quantity: ");
                existingProduct.QuantityInStock = int.Parse(Console.ReadLine());
                Console.Write("New Price: ");
                existingProduct.Price = decimal.Parse(Console.ReadLine());
                Console.Write("New Supplier: ");
                existingProduct.SupplierName = Console.ReadLine();

                if (ProductRepository.UpdateProduct(existingProduct, loggedInUser.Username))
                    Console.WriteLine("Product updated successfully!");
                else
                    Console.WriteLine("Failed to update product.");
            }
            else
            {
                Console.WriteLine("Product not found.");
            }
        }
        catch
        {
            Console.WriteLine("Invalid input. Please enter valid details.");
        }
    }

    private static void DeleteProduct(User loggedInUser)
    {
        try
        {
            Console.Write("Enter Product ID to delete: ");
            var deleteId = int.Parse(Console.ReadLine());

            if (ProductRepository.DeleteProduct(deleteId, loggedInUser.Username))
                Console.WriteLine("Product deleted successfully!");
            else
                Console.WriteLine("Failed to delete product.");
        }
        catch
        {
            Console.WriteLine("Invalid input. Please enter a valid product ID.");
        }
    }

    private static void SearchProduct(User loggedInUser)
    {
        Console.WriteLine("1. Search By Product\n2. Search By Supplier\n3. Exit");
        var choice = Console.ReadLine();
        var searchResults = new List<Product>();
        var searchName = "";
        switch (choice)
        {
            case "1":
                Console.Write("Enter Product Name to search: ");
                searchName = Console.ReadLine();
                searchResults = ProductRepository.SearchProducts(searchName, loggedInUser.Username);
                if (searchResults.Count > 0)
                    foreach (var p in searchResults)
                        Console.WriteLine($"{p.ProductID}: {p.ProductName} - {p.QuantityInStock} in stock");
                else
                    Console.WriteLine("No products found.");
                break;
            case "2":
                Console.Write("Enter Supplier Name to search: ");
                searchName = Console.ReadLine();
                searchResults = ProductRepository.FilterBySupplier(searchName, loggedInUser.Username);
                if (searchResults.Count > 0)
                    foreach (var p in searchResults)
                        Console.WriteLine($"{p.ProductID}: {p.ProductName} - {p.QuantityInStock} in stock");
                else
                    Console.WriteLine("No products found.");
                break;
            case "3":
                break;
            default:
                Console.WriteLine("Invalid input. Please enter valid details.");
                break;
        }
    }

    private static void LowStockAlert()
    {
        var lowStock = ProductRepository.GetLowStockProducts();
        Console.WriteLine("Low Stock Products:");

        if (lowStock.Count > 0)
            foreach (var p in lowStock)
                Console.WriteLine($"{p.ProductID}: {p.ProductName} - {p.QuantityInStock} left!");
        else
            Console.WriteLine("No low-stock products.");
    }

    private static void SearchUsingTalkGrid()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new InventoryForm());
    }
}