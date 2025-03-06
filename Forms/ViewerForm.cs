using System;
using System.Windows.Forms;
using InventoryManagementSystem.Forms;
using InventoryManagementSystem.Models;
using InventoryManagementSystem.Repository;

namespace InventoryManagementSystem
{
    public partial class ViewerForm : Form
    {
        private readonly User loggedInUser;

        public ViewerForm(User user)
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
           new SearchForm(loggedInUser).Show();
        }

        private void btnTerlikGrid_Click(object sender, EventArgs e)
        {
            Application.EnableVisualStyles();
            new InventoryForm().Show();

        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
            new LoginForm().Show();
        }
    }
}