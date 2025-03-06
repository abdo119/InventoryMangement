namespace InventoryManagementSystem
{
    partial class ViewerForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dataGridViewProducts;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnTerlikGrid;
        private System.Windows.Forms.Button btnLogout;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridViewProducts = new System.Windows.Forms.DataGridView();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnTerlikGrid = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProducts)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewProducts
            // 
            this.dataGridViewProducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewProducts.Location = new System.Drawing.Point(12, 50);
            this.dataGridViewProducts.Name = "dataGridViewProducts";
            this.dataGridViewProducts.Size = new System.Drawing.Size(560, 250);
            this.dataGridViewProducts.TabIndex = 0;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(53, 13);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnTerlikGrid
            // 
            this.btnTerlikGrid.Location = new System.Drawing.Point(202, 13);
            this.btnTerlikGrid.Name = "btnTerlikGrid";
            this.btnTerlikGrid.Size = new System.Drawing.Size(120, 23);
            this.btnTerlikGrid.TabIndex = 3;
            this.btnTerlikGrid.Text = "Search Using Terlik Grid";
            this.btnTerlikGrid.UseVisualStyleBackColor = true;
            this.btnTerlikGrid.Click += new System.EventHandler(this.btnTerlikGrid_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.Location = new System.Drawing.Point(497, 310);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(75, 23);
            this.btnLogout.TabIndex = 4;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // ViewerForm
            // 
            this.ClientSize = new System.Drawing.Size(584, 361);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.btnTerlikGrid);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.dataGridViewProducts);
            this.Name = "ViewerForm";
            this.Text = "Viewer Dashboard";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProducts)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
