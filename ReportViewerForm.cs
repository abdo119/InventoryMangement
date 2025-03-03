using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Windows.Forms;
using InventoryManagementSystem.DataAccess;

namespace InventoryManagementSystem
{
    public class ReportViewerForm : Form
    {
        private readonly CrystalReportViewer crystalReportViewer;

        public ReportViewerForm()
        {
            // Manually initialize UI components
            Text = "Inventory Report";
            Width = 800;
            Height = 600;

            // Initialize and configure the Crystal Report Viewer
            crystalReportViewer = new CrystalReportViewer
            {
                Dock = DockStyle.Fill
            };

            // Add viewer to the form
            Controls.Add(crystalReportViewer);

            // Load the report when form loads
            Load += ReportViewerForm_Load;
        }

        private void ReportViewerForm_Load(object sender, EventArgs e)
        {
            try
            {
                // Fetch data using ADO.NET
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    var query = "SELECT ProductName, QuantityInStock FROM Products WHERE QuantityInStock < 10";
                    var adapter = new SqlDataAdapter(query, conn);
                    var ds = new DataSet();
                    adapter.Fill(ds, "LowStockItems");

                    // Load the report
                    var rptDoc = new ReportDocument();
                    var reportPath =
                        @"/report.rpt"; // Update this path
                    if (!File.Exists(reportPath))
                    {
                        MessageBox.Show("Report file not found: " + reportPath);
                        return;
                    }

                    rptDoc.Load(reportPath); // Update path

                    // Assign data to report
                    rptDoc.SetDataSource(ds.Tables["LowStockItems"]);

                    // Set the report source for the viewer
                    crystalReportViewer.ReportSource = rptDoc;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading report: " + ex.Message);
            }
        }
    }
}