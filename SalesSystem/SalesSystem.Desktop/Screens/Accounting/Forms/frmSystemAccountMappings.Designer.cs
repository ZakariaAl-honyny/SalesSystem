namespace SalesSystem.Desktop.Screens.Accounting.Forms
{
    partial class frmSystemAccountMappings
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            components?.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            lblMode = new Label();
            dgvMappings = new DataGridView();
            btnClose = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvMappings).BeginInit();
            SuspendLayout();

            // lblMode
            lblMode.Anchor = AnchorStyles.Top;
            lblMode.AutoSize = true;
            lblMode.BackColor = Color.WhiteSmoke;
            lblMode.FlatStyle = FlatStyle.Flat;
            lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblMode.ForeColor = Color.Firebrick;
            lblMode.Location = new Point(210, 15);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(364, 39);
            lblMode.TabIndex = 0;
            lblMode.Text = "ربط الحسابات النظامية";

            // dgvMappings
            dgvMappings.AllowUserToAddRows = false;
            dgvMappings.AllowUserToDeleteRows = false;
            dgvMappings.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMappings.BackgroundColor = Color.White;
            dgvMappings.BorderStyle = BorderStyle.None;
            dgvMappings.ColumnHeadersHeight = 50;
            dgvMappings.Location = new Point(12, 70);
            dgvMappings.MultiSelect = false;
            dgvMappings.Name = "dgvMappings";
            dgvMappings.ReadOnly = true;
            dgvMappings.RightToLeft = RightToLeft.Yes;
            dgvMappings.RowHeadersWidth = 40;
            dgvMappings.RowTemplate.Height = 28;
            dgvMappings.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMappings.Size = new Size(760, 420);
            dgvMappings.TabIndex = 1;
            dgvMappings.DoubleClick += dgvMappings_DoubleClick;

            // btnClose
            btnClose.BackColor = Color.White;
            btnClose.FlatStyle = FlatStyle.Popup;
            btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnClose.Location = new Point(12, 508);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(80, 36);
            btnClose.TabIndex = 2;
            btnClose.Text = "إغلاق";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;

            // frmSystemAccountMappings
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(784, 556);
            Controls.Add(btnClose);
            Controls.Add(dgvMappings);
            Controls.Add(lblMode);
            MaximizeBox = false;
            MinimizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Name = "frmSystemAccountMappings";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ربط الحسابات النظامية";
            Load += frmSystemAccountMappings_Load;
            ((System.ComponentModel.ISupportInitialize)dgvMappings).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.DataGridView dgvMappings;
        private System.Windows.Forms.Button btnClose;
    }
}
