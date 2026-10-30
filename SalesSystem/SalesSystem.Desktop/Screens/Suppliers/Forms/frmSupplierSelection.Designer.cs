namespace SalesSystem.Desktop.Suppliers.Forms
{
    partial class frmSupplierSelection
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            components?.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            txtSearch = new TextBox();
            dgvSuppliers = new DataGridView();
            btnSelect = new Button();
            btnCancel = new Button();
            dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dgvSuppliers).BeginInit();
            SuspendLayout();
            // 
            // txtSearch
            // 
            txtSearch.Font = new Font("Tahoma", 10F);
            txtSearch.Location = new Point(12, 12);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(400, 32);
            txtSearch.TabIndex = 0;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // dgvSuppliers
            // 
            dgvSuppliers.AllowUserToAddRows = false;
            dgvSuppliers.AllowUserToDeleteRows = false;
            dgvSuppliers.BackgroundColor = SystemColors.Window;
            dgvSuppliers.BorderStyle = BorderStyle.Fixed3D;
            dgvSuppliers.ColumnHeadersHeight = 50;
            dgvSuppliers.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn1, dataGridViewTextBoxColumn2, dataGridViewTextBoxColumn3, dataGridViewTextBoxColumn4 });
            dgvSuppliers.Location = new Point(12, 50);
            dgvSuppliers.MultiSelect = false;
            dgvSuppliers.Name = "dgvSuppliers";
            dgvSuppliers.ReadOnly = true;
            dgvSuppliers.RowHeadersWidth = 62;
            dgvSuppliers.RowTemplate.Height = 28;
            dgvSuppliers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSuppliers.Size = new Size(676, 380);
            dgvSuppliers.TabIndex = 1;
            dgvSuppliers.TabStop = false;
            dgvSuppliers.DoubleClick += dgvSuppliers_DoubleClick;
            // 
            // btnSelect
            // 
            btnSelect.BackColor = Color.White;
            btnSelect.FlatAppearance.BorderColor = Color.Black;
            btnSelect.FlatAppearance.BorderSize = 2;
            btnSelect.FlatStyle = FlatStyle.Popup;
            btnSelect.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnSelect.Location = new Point(570, 440);
            btnSelect.Name = "btnSelect";
            btnSelect.Size = new Size(118, 35);
            btnSelect.TabIndex = 2;
            btnSelect.Text = "تحديد";
            btnSelect.UseVisualStyleBackColor = true;
            btnSelect.Click += btnSelect_Click;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.White;
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.FlatAppearance.BorderColor = Color.Black;
            btnCancel.FlatAppearance.BorderSize = 2;
            btnCancel.FlatStyle = FlatStyle.Popup;
            btnCancel.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnCancel.Location = new Point(440, 440);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(118, 35);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "إلغاء";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewTextBoxColumn1.HeaderText = "رقم المورد";
            dataGridViewTextBoxColumn1.MinimumWidth = 8;
            dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            dataGridViewTextBoxColumn1.ReadOnly = true;
            dataGridViewTextBoxColumn1.Width = 150;
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewTextBoxColumn2.HeaderText = "اسم المورد";
            dataGridViewTextBoxColumn2.MinimumWidth = 8;
            dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            dataGridViewTextBoxColumn2.ReadOnly = true;
            dataGridViewTextBoxColumn2.Width = 150;
            // 
            // dataGridViewTextBoxColumn3
            // 
            dataGridViewTextBoxColumn3.HeaderText = "رقم الهاتف";
            dataGridViewTextBoxColumn3.MinimumWidth = 8;
            dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            dataGridViewTextBoxColumn3.ReadOnly = true;
            dataGridViewTextBoxColumn3.Width = 150;
            // 
            // dataGridViewTextBoxColumn4
            // 
            dataGridViewTextBoxColumn4.HeaderText = "البريد الإلكتروني";
            dataGridViewTextBoxColumn4.MinimumWidth = 8;
            dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            dataGridViewTextBoxColumn4.ReadOnly = true;
            dataGridViewTextBoxColumn4.Width = 150;
            // 
            // frmSupplierSelection
            // 
            AcceptButton = btnSelect;
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            CancelButton = btnCancel;
            ClientSize = new Size(700, 490);
            Controls.Add(btnCancel);
            Controls.Add(btnSelect);
            Controls.Add(dgvSuppliers);
            Controls.Add(txtSearch);
            Font = new Font("Tahoma", 12F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmSupplierSelection";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "اختيار مورد";
            Load += frmSupplierSelection_Load;
            ((System.ComponentModel.ISupportInitialize)dgvSuppliers).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.DataGridView dgvSuppliers;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Button btnCancel;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
    }
}
