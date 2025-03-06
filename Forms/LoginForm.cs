using System;
using System.Windows.Forms;
using InventoryManagementSystem.Models;
using InventoryManagementSystem.Repository;

namespace InventoryManagementSystem.Forms
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            User user = AuthRepository.AuthenticateUser(username, password);
            if (user != null)
            {
                MessageBox.Show($"Welcome, {user.Username} ({user.Role})!", "Login Successful");

                if (user.Role == "Admin")
                    new AdminForm(user).Show();
                else
                    new ViewerForm(user).Show();

                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid credentials. Try again.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            txtUsername.Text = ""; 
            txtPassword.Text = ""; 
            ActiveControl = txtUsername; 
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit(); // Terminate the app
        }
    }
}