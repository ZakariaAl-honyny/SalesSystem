namespace SalesSystem.Desktop.Screens.Returns.Forms
{
    partial class frmListReturns
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            components?.Dispose();base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            lblMode = new Label();
            tabReturns = new TabControl();
            tpSales = new TabPage();
            dgvSales = new DataGridView();
            tpPurchases = new TabPage();
            dgvPurchases = new DataGridView();
            btnAdd = new Button();
            btnRefresh = new Button();
            btnClose = new Button();
            lblRecordsCount = new Label();
            label1 = new Label();

            tabReturns.SuspendLayout();
            tpSales.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSales).BeginInit();
            tpPurchases.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPurchases).BeginInit();
            SuspendLayout();

            // lblMode
            lblMode.Anchor = AnchorStyles.Top;
            lblMode.AutoSize = true;
            lblMode.BackColor = Color.WhiteSmoke;
            lblMode.FlatStyle = FlatStyle.Flat;
            lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            lblMode.ForeColor = Color.Firebrick;
            lblMode.Location = new Point(690, 20);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(214, 39);
            lblMode.TabIndex = 30;
            lblMode.Text = "إدارة المرتجعات";

            // tabReturns
            tabReturns.Controls.Add(tpSales);
            tabReturns.Controls.Add(tpPurchases);
            tabReturns.Font = new Font("Tahoma", 10F);
            tabReturns.Location = new Point(12, 100);
            tabReturns.Name = "tabReturns";
            tabReturns.RightToLeft = RightToLeft.Yes;
            tabReturns.RightToLeftLayout = true;
            tabReturns.SelectedIndex = 0;
            tabReturns.Size = new Size(1540, 590);
            tabReturns.TabIndex = 0;

            // tpSales
            tpSales.BackColor = Color.White;
            tpSales.Controls.Add(dgvSales);
            tpSales.Location = new Point(4, 34);
            tpSales.Name = "tpSales";
            tpSales.Padding = new Padding(3);
            tpSales.Size = new Size(1532, 552);
            tpSales.TabIndex = 0;
            tpSales.Text = "مرتجعـات المبيعات";

            // dgvSales
            dgvSales.AllowUserToAddRows = false;
            dgvSales.AllowUserToDeleteRows = false;
            dgvSales.AllowUserToOrderColumns = true;
            dgvSales.BackgroundColor = SystemColors.Window;
            dgvSales.BorderStyle = BorderStyle.Fixed3D;
            dgvSales.ColumnHeadersHeight = 50;
            dgvSales.Dock = DockStyle.Fill;
            dgvSales.Location = new Point(3, 3);
            dgvSales.MultiSelect = false;
            dgvSales.Name = "dgvSales";
            dgvSales.ReadOnly = true;
            dgvSales.RowHeadersWidth = 62;
            dgvSales.RowTemplate.Height = 28;
            dgvSales.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSales.Size = new Size(1526, 546);
            dgvSales.TabIndex = 0;
            dgvSales.TabStop = false;
            dgvSales.DoubleClick += dgvSales_DoubleClick;

            // tpPurchases
            tpPurchases.BackColor = Color.White;
            tpPurchases.Controls.Add(dgvPurchases);
            tpPurchases.Location = new Point(4, 34);
            tpPurchases.Name = "tpPurchases";
            tpPurchases.Padding = new Padding(3);
            tpPurchases.Size = new Size(1532, 552);
            tpPurchases.TabIndex = 1;
            tpPurchases.Text = "مرتجعـات المشتريات";

            // dgvPurchases
            dgvPurchases.AllowUserToAddRows = false;
            dgvPurchases.AllowUserToDeleteRows = false;
            dgvPurchases.AllowUserToOrderColumns = true;
            dgvPurchases.BackgroundColor = SystemColors.Window;
            dgvPurchases.BorderStyle = BorderStyle.Fixed3D;
            dgvPurchases.ColumnHeadersHeight = 50;
            dgvPurchases.Dock = DockStyle.Fill;
            dgvPurchases.Location = new Point(3, 3);
            dgvPurchases.MultiSelect = false;
            dgvPurchases.Name = "dgvPurchases";
            dgvPurchases.ReadOnly = true;
            dgvPurchases.RowHeadersWidth = 62;
            dgvPurchases.RowTemplate.Height = 28;
            dgvPurchases.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPurchases.Size = new Size(1526, 546);
            dgvPurchases.TabIndex = 0;
            dgvPurchases.TabStop = false;
            dgvPurchases.DoubleClick += dgvPurchases_DoubleClick;

            // btnAdd
            btnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAdd.BackColor = Color.White;
            btnAdd.FlatAppearance.BorderColor = Color.Black;
            btnAdd.FlatAppearance.BorderSize = 2;
            btnAdd.FlatStyle = FlatStyle.Popup;
            btnAdd.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnAdd.Location = new Point(1470, 20);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(75, 75);
            btnAdd.TabIndex = 5;
            btnAdd.Text = "+ جديد";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;

            // btnRefresh
            btnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRefresh.BackColor = Color.White;
            btnRefresh.FlatAppearance.BorderColor = Color.Black;
            btnRefresh.FlatAppearance.BorderSize = 2;
            btnRefresh.FlatStyle = FlatStyle.Popup;
            btnRefresh.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnRefresh.Location = new Point(1380, 20);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(75, 75);
            btnRefresh.TabIndex = 6;
            btnRefresh.Text = "تحديث";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += btnRefresh_Click;

            // btnClose
            btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnClose.BackColor = Color.White;
            btnClose.DialogResult = DialogResult.Cancel;
            btnClose.FlatStyle = FlatStyle.Popup;
            btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnClose.Location = new Point(12, 700);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(181, 61);
            btnClose.TabIndex = 0;
            btnClose.Text = "إغلاق";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;

            // lblRecordsCount
            lblRecordsCount.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            lblRecordsCount.AutoSize = true;
            lblRecordsCount.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblRecordsCount.Location = new Point(1380, 715);
            lblRecordsCount.Name = "lblRecordsCount";
            lblRecordsCount.Size = new Size(72, 24);
            lblRecordsCount.TabIndex = 4;
            lblRecordsCount.Text = "[????]";

            // label1
            label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label1.Location = new Point(1220, 715);
            label1.Name = "label1";
            label1.Size = new Size(152, 24);
            label1.TabIndex = 3;
            label1.Text = "عدد المرتجعات:";

            // frmListReturns
            AcceptButton = btnAdd;
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            CancelButton = btnClose;
            ClientSize = new Size(1564, 773);
            Controls.Add(lblMode);
            Controls.Add(tabReturns);
            Controls.Add(btnAdd);
            Controls.Add(btnRefresh);
            Controls.Add(label1);
            Controls.Add(lblRecordsCount);
            Controls.Add(btnClose);
            Font = new Font("Tahoma", 12F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "frmListReturns";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "إدارة المرتجعات";
            Load += frmListReturns_Load;
            tabReturns.ResumeLayout(false);
            tpSales.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvSales).EndInit();
            tpPurchases.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvPurchases).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.TabControl tabReturns;
        private System.Windows.Forms.TabPage tpSales;
        private System.Windows.Forms.DataGridView dgvSales;
        private System.Windows.Forms.TabPage tpPurchases;
        private System.Windows.Forms.DataGridView dgvPurchases;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblRecordsCount;
        private System.Windows.Forms.Label label1;
    }
}
