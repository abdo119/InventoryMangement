using System;
using System.Windows.Forms;
using InventoryManagementSystem.Forms;
using InventoryManagementSystem.Models;

namespace InventoryManagementSystem;

public partial class AdminForm : Form
{
    private readonly User loggedInUser;

    public AdminForm(User user)
    {
        InitializeComponent();
        loggedInUser = user;
    }

    private void btnViewProducts_Click(object sender, EventArgs e)
    {
        new ProductForm(loggedInUser).Show();
    }

    private void btnTerlikGrid_Click(object sender, EventArgs e)
    {
        Application.EnableVisualStyles();
        new InventoryForm().Show();
    }

    private void btnSearch_Click(object sender, EventArgs e)
    {
        new SearchForm(loggedInUser).Show();
    }


    private void btnLogout_Click(object sender, EventArgs e)
    {
        Close();
        new LoginForm().Show();
    }

    private void AdminForm_Load(object sender, EventArgs e)
    {
    }
}