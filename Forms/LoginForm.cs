using System;
using System.Windows.Forms;
using InventoryManagementSystem.Repository;

namespace InventoryManagementSystem.Forms;

public partial class LoginForm : Form
{
    public LoginForm()
    {
        InitializeComponent();
    }

    private void btnLogin_Click(object sender, EventArgs e)
    {
        var username = txtUsername.Text;
        var password = txtPassword.Text;

        var user = AuthRepository.AuthenticateUser(username, password);
        if (user != null)
        {
            MessageBox.Show($"Welcome, {user.Username} ({user.Role})!", "Login Successful");

            if (user.Role == "Admin")
                new AdminForm(user).Show();
            else
                new ViewerForm(user).Show();

            Hide();
        }
        else
        {
            MessageBox.Show("Invalid credentials. Try again.", "Login Failed", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void LoginForm_Load(object sender, EventArgs e)
    {
        txtUsername.Text = "";
        txtPassword.Text = "";
        ActiveControl = txtUsername;
    }


    private void button1_Click_1(object sender, EventArgs e)
    {
        new RegisterForm().Show();
        Hide();
    }

    private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
    {
        // Toggle password visibility
        txtPassword.PasswordChar = chkShowPassword.Checked ? '\0' : '*';
    }
}