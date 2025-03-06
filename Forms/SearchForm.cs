using System;
using System.Collections.Generic;
using System.Windows.Forms;
using InventoryManagementSystem.Models;
using InventoryManagementSystem.Repository;

namespace InventoryManagementSystem
{
    public partial class SearchForm : Form
    {
        private readonly User loggedInUser;

        public SearchForm(User user)
        {
            InitializeComponent();
            loggedInUser = user;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var searchResults = new List<Product>();
            var searchName = txtSearch.Text.Trim();

            if (rbProduct.Checked)
            {
                searchResults = ProductRepository.SearchProducts(searchName, loggedInUser.Username);
            }
            else if (rbSupplier.Checked)
            {
                searchResults = ProductRepository.FilterBySupplier(searchName, loggedInUser.Username);
            }

            DisplaySearchResults(searchResults);
        }

        private void DisplaySearchResults(List<Product> searchResults)
        {
            if (searchResults.Count > 0)
            {
                dataGridViewResults.DataSource = searchResults;
            }
            else
            {
                MessageBox.Show("No products found.", "Search Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}