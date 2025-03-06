using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using InventoryManagementSystem.Repository;

namespace InventoryManagementSystem.Forms;

public partial class RegisterForm : Form
{
    public RegisterForm()
    {
        InitializeComponent();
    }

    private void btnRegister_Click(object sender, EventArgs e)
    {
        // Retrieve and trim user input
        var username = txtUsername.Text.Trim();
        var password = txtPassword.Text.Trim();

        if (!ValidateInput(username, password)) return;

        if (AuthRepository.UserExists(username))
        {
            MessageBox.Show("Username already exists. Please choose another username.", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return;
        }

        if (AuthRepository.RegisterUser(username, password, "Viewer"))
        {
            MessageBox.Show("Registration successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
            new LoginForm().Show();
        }
        else
        {
            MessageBox.Show("There was an error during registration. Please try again.", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void btnExit_Click(object sender, EventArgs e)
    {
        Close();
        new LoginForm().Show();
    }

    private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
    {
        // Toggle password visibility
        txtPassword.PasswordChar = chkShowPassword.Checked ? '\0' : '*';
    }

    private bool ValidateInput(string username, string password)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            MessageBox.Show("All fields are required!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }


        if (password.Length < 6)
        {
            MessageBox.Show("Password must be at least 6 characters long.", "Input Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return false;
        }

        if (!IsPasswordComplex(password))
        {
            MessageBox.Show(
                "Password must contain at least one uppercase letter, one number, and one special character.",
                "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        return true;
    }

    // Password complexity check: At least 1 uppercase 1 lowercase, 1 number, and 1 special character
    private bool IsPasswordComplex(string password)
    {
        var hasUpperCase = new Regex(@"[A-Z]");
        var hasLowerCase = new Regex(@"[a-z]");
        var hasDigits = new Regex(@"[0-9]");
        var hasSpecialChar = new Regex(@"[\W_]");
        return hasUpperCase.IsMatch(password) && hasLowerCase.IsMatch(password) &&
               hasDigits.IsMatch(password) && hasSpecialChar.IsMatch(password);
    }
}