using System;
using System.Windows.Forms;
using InventoryManagementSystem.Models;
using InventoryManagementSystem.Repository;

namespace InventoryManagementSystem
{
    public partial class ProductForm : Form
    {
        private readonly User loggedInUser;

        public ProductForm(User user)
        {
            InitializeComponent();
            LoadProducts();
            loggedInUser = user;

        }

        private void LoadProducts()
        {
            var products = ProductRepository.GetAllProducts();
            dataGridViewProducts.DataSource = products;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var name = txtName.Text.Trim();
            var desc = txtDescription.Text.Trim();
            var qty = int.Parse(txtQuantity.Text);
            var price = decimal.Parse(txtPrice.Text);
            var supplier = txtSupplier.Text.Trim();

            var newProduct = new Product
            {
                ProductName = name,
                Description = desc,
                QuantityInStock = qty,
                Price = price,
                SupplierName = supplier
            };

            if (ProductRepository.AddProduct(newProduct, "Admin"))
            {
                MessageBox.Show("Product added successfully!");
                LoadProducts();
            }
            else
            {
                MessageBox.Show("Failed to add product.");
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridViewProducts.SelectedRows.Count == 0) return;

            var selectedRow = dataGridViewProducts.SelectedRows[0];
            var productId = Convert.ToInt32(selectedRow.Cells["ProductID"].Value);

            var updatedProduct = new Product
            {
                ProductID = productId,
                ProductName = txtName.Text.Trim(),
                Description = txtDescription.Text.Trim(),
                QuantityInStock = int.Parse(txtQuantity.Text),
                Price = decimal.Parse(txtPrice.Text),
                SupplierName = txtSupplier.Text.Trim()
            };

            if (ProductRepository.UpdateProduct(updatedProduct, "Admin"))
            {
                MessageBox.Show("Product updated successfully!");
                LoadProducts();
            }
            else
            {
                MessageBox.Show("Failed to update product.");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewProducts.SelectedRows.Count == 0) return;

            var selectedRow = dataGridViewProducts.SelectedRows[0];
            var productId = Convert.ToInt32(selectedRow.Cells["ProductID"].Value);

            if (ProductRepository.DeleteProduct(productId, "Admin"))
            {
                MessageBox.Show("Product deleted successfully!");
                LoadProducts();
            }
            else
            {
                MessageBox.Show("Failed to delete product.");
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            new SearchForm(loggedInUser).Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
