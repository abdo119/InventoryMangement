using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using InventoryManagementSystem.DataAccess;
using Telerik.WinControls.UI;

public class InventoryForm : Form
{
    private RadGridView radGridView1;

    public InventoryForm()
    {
        InitializeRadGridView();
        LoadInventoryData();
    }

    private void InitializeRadGridView()
    {
        radGridView1 = new RadGridView
        {
            Dock = DockStyle.Fill, // Make it fill the form
            AutoGenerateColumns = true,
            EnableFiltering = true, // Enable built-in filtering
            EnableSorting = true // Enable built-in sorting
        };

        Controls.Add(radGridView1);
    }

    private void LoadInventoryData()
    {
        using (var conn = DatabaseHelper.GetConnection())
        {
            conn.Open();
            var adapter = new SqlDataAdapter("SELECT * FROM Products", conn);
            var dt = new DataTable();
            adapter.Fill(dt);

            radGridView1.DataSource = dt; // Bind data to Telerik Grid
        }
    }
}