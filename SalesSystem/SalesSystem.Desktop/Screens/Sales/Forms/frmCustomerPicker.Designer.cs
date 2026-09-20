namespace SalesSystem.Desktop.Screens.Sales.Forms
{
    partial class frmCustomerPicker
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            pnlHeader = new Panel();
            lblTitle = new Label();
            lblEscHint = new Label();
            pnlSearch = new Panel();
            tblSearch = new TableLayoutPanel();
            btnCashCustomer = new DashedButton();
            txtSearch = new TextBox();
            dgvCustomers = new DataGridView();
            pnlFooter = new Panel();
            flpFooter = new FlowLayoutPanel();
            btnOk = new Button();
            btnCancel = new Button();
            lblCount = new Label();
            pnlHeader.SuspendLayout();
            pnlSearch.SuspendLayout();
            tblSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCustomers).BeginInit();
            pnlFooter.SuspendLayout();
            flpFooter.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(33, 43, 54);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblEscHint);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 56;
            pnlHeader.Padding = new Padding(14, 0, 14, 0);
            // 
            // lblEscHint
            // 
            lblEscHint.Dock = DockStyle.Left;
            lblEscHint.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblEscHint.ForeColor = Color.FromArgb(150, 170, 190);
            lblEscHint.Text = "Esc = إغلاق  |  Enter = اختيار";
            lblEscHint.TextAlign = ContentAlignment.MiddleRight;
            lblEscHint.Width = 220;
            // 
            // lblTitle
            // 
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.Font = new Font("Tahoma", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.ForeColor = Color.White;
            lblTitle.Text = "اختيار العميل";
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pnlSearch
            // 
            pnlSearch.BackColor = Color.White;
            pnlSearch.Controls.Add(tblSearch);
            pnlSearch.Dock = DockStyle.Top;
            pnlSearch.Height = 56;
            pnlSearch.Padding = new Padding(8, 8, 8, 8);
            // 
            // tblSearch
            // 
            tblSearch.ColumnCount = 2;
            tblSearch.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 178F));
            tblSearch.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblSearch.Controls.Add(btnCashCustomer, 0, 0);
            tblSearch.Controls.Add(txtSearch, 1, 0);
            tblSearch.Dock = DockStyle.Fill;
            tblSearch.RowCount = 1;
            tblSearch.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            // 
            // btnCashCustomer
            // 
            btnCashCustomer.Dock = DockStyle.Fill;
            btnCashCustomer.Margin = new Padding(0, 0, 6, 0);
            btnCashCustomer.Text = "عميل نقدي عام";
            btnCashCustomer.Click += btnCashCustomer_Click;
            // 
            // txtSearch
            // 
            txtSearch.Dock = DockStyle.Fill;
            txtSearch.Font = new Font("Tahoma", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtSearch.PlaceholderText = "بحث بالاسم / الكود / الهاتف...";
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // dgvCustomers
            // 
            dgvCustomers.AllowUserToAddRows = false;
            dgvCustomers.AllowUserToDeleteRows = false;
            dgvCustomers.BackgroundColor = Color.White;
            dgvCustomers.BorderStyle = BorderStyle.Fixed3D;
            dgvCustomers.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(233, 236, 239);
            dgvCustomers.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dgvCustomers.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(33, 43, 54);
            dgvCustomers.ColumnHeadersHeight = 40;
            dgvCustomers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvCustomers.DefaultCellStyle.Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dgvCustomers.Dock = DockStyle.Fill;
            dgvCustomers.MultiSelect = false;
            dgvCustomers.ReadOnly = true;
            dgvCustomers.RightToLeft = RightToLeft.Yes;
            dgvCustomers.RowHeadersVisible = false;
            dgvCustomers.RowTemplate.Height = 36;
            dgvCustomers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCustomers.CellDoubleClick += dgvCustomers_CellDoubleClick;
            dgvCustomers.CellPainting += dgvCustomers_CellPainting;
            dgvCustomers.CellToolTipTextNeeded += dgvCustomers_CellToolTipTextNeeded;
            dgvCustomers.SelectionChanged += dgvCustomers_SelectionChanged;
            // 
            // pnlFooter
            // 
            pnlFooter.BackColor = Color.FromArgb(248, 249, 250);
            pnlFooter.Controls.Add(flpFooter);
            pnlFooter.Controls.Add(lblCount);
            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Height = 58;
            // 
            // flpFooter
            // 
            flpFooter.Controls.Add(btnOk);
            flpFooter.Controls.Add(btnCancel);
            flpFooter.Dock = DockStyle.Fill;
            flpFooter.FlowDirection = FlowDirection.RightToLeft;
            flpFooter.Padding = new Padding(8, 9, 8, 9);
            // 
            // btnOk
            // 
            btnOk.BackColor = Color.FromArgb(25, 135, 84);
            btnOk.FlatStyle = FlatStyle.Popup;
            btnOk.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnOk.ForeColor = Color.White;
            btnOk.Height = 40;
            btnOk.Margin = new Padding(0, 0, 6, 0);
            btnOk.Text = "اختيار";
            btnOk.Width = 130;
            btnOk.Click += btnOk_Click;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.White;
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.FlatStyle = FlatStyle.Popup;
            btnCancel.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCancel.ForeColor = Color.FromArgb(33, 43, 54);
            btnCancel.Height = 40;
            btnCancel.Margin = new Padding(0, 0, 6, 0);
            btnCancel.Text = "إلغاء";
            btnCancel.Width = 100;
            // 
            // lblCount
            // 
            lblCount.Dock = DockStyle.Left;
            lblCount.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblCount.ForeColor = Color.FromArgb(73, 80, 87);
            lblCount.Text = "عدد العملاء: 0";
            lblCount.TextAlign = ContentAlignment.MiddleRight;
            lblCount.Width = 260;
            // 
            // frmCustomerPicker
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(520, 600);
            Controls.Add(dgvCustomers);
            Controls.Add(pnlFooter);
            Controls.Add(pnlSearch);
            Controls.Add(pnlHeader);
            Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmCustomerPicker";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "اختيار العميل";
            KeyDown += frmCustomerPicker_KeyDown;
            pnlHeader.ResumeLayout(false);
            pnlSearch.ResumeLayout(false);
            tblSearch.ResumeLayout(false);
            tblSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCustomers).EndInit();
            pnlFooter.ResumeLayout(false);
            flpFooter.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblEscHint;
        private System.Windows.Forms.Panel pnlSearch;
        private System.Windows.Forms.TableLayoutPanel tblSearch;
        private DashedButton btnCashCustomer;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.DataGridView dgvCustomers;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.FlowLayoutPanel flpFooter;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblCount;
    }
}