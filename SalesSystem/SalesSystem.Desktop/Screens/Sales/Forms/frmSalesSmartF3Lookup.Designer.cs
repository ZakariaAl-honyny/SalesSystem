namespace SalesSystem.Desktop.Screens.Sales.Forms
{
    partial class frmSalesSmartF3Lookup
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
            pnlTitle = new Panel();
            tblHeader = new TableLayoutPanel();
            lblTitle = new Label();
            lblEscHint = new Label();
            pnlSearch = new Panel();
            txtSearch = new TextBox();
            pnlFooter = new Panel();
            flpFooter = new FlowLayoutPanel();
            btnClose = new Button();
            tblMain = new TableLayoutPanel();
            pnlLeft = new Panel();
            dgvProducts = new DataGridView();
            pnlRight = new Panel();
            grpUnits = new GroupBox();
            flpUnits = new FlowLayoutPanel();
            lblTotalStock = new Label();
            btnInsertBase = new Button();
            pnlTitle.SuspendLayout();
            tblHeader.SuspendLayout();
            pnlSearch.SuspendLayout();
            pnlFooter.SuspendLayout();
            flpFooter.SuspendLayout();
            tblMain.SuspendLayout();
            pnlLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvProducts).BeginInit();
            pnlRight.SuspendLayout();
            grpUnits.SuspendLayout();
            SuspendLayout();
            // 
            // pnlTitle
            // 
            pnlTitle.BackColor = Color.FromArgb(33, 43, 54);
            pnlTitle.Controls.Add(tblHeader);
            pnlTitle.Dock = DockStyle.Top;
            pnlTitle.Height = 52;
            // 
            // tblHeader
            // 
            tblHeader.ColumnCount = 2;
            tblHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140F));
            tblHeader.Controls.Add(lblTitle, 0, 0);
            tblHeader.Controls.Add(lblEscHint, 1, 0);
            tblHeader.Dock = DockStyle.Fill;
            tblHeader.RowCount = 1;
            tblHeader.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            // 
            // lblTitle
            // 
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.Font = new Font("Tahoma", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.ForeColor = Color.White;
            lblTitle.Padding = new Padding(12, 0, 0, 0);
            lblTitle.Text = "اختيار المنتج [F3]";
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblEscHint
            // 
            lblEscHint.Dock = DockStyle.Fill;
            lblEscHint.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblEscHint.ForeColor = Color.FromArgb(150, 170, 190);
            lblEscHint.Text = "Esc = إغلاق";
            lblEscHint.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlSearch
            // 
            pnlSearch.BackColor = Color.White;
            pnlSearch.Controls.Add(txtSearch);
            pnlSearch.Dock = DockStyle.Top;
            pnlSearch.Height = 54;
            pnlSearch.Padding = new Padding(8, 8, 8, 8);
            // 
            // txtSearch
            // 
            txtSearch.Dock = DockStyle.Fill;
            txtSearch.Font = new Font("Tahoma", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtSearch.PlaceholderText = "بحث بالاسم / الكود / الباركود / التصنيف...";
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // pnlFooter
            // 
            pnlFooter.BackColor = Color.FromArgb(248, 249, 250);
            pnlFooter.Controls.Add(flpFooter);
            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Height = 60;
            // 
            // flpFooter
            // 
            flpFooter.Controls.Add(btnClose);
            flpFooter.Dock = DockStyle.Fill;
            flpFooter.FlowDirection = FlowDirection.RightToLeft;
            flpFooter.Padding = new Padding(8, 10, 8, 8);
            // 
            // btnClose
            // 
            btnClose.BackColor = Color.White;
            btnClose.DialogResult = DialogResult.Cancel;
            btnClose.FlatStyle = FlatStyle.Popup;
            btnClose.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnClose.ForeColor = Color.FromArgb(33, 43, 54);
            btnClose.Height = 40;
            btnClose.Text = "إغلاق";
            btnClose.Width = 110;
            btnClose.Click += btnClose_Click;
            // 
            // tblMain
            // 
            tblMain.ColumnCount = 2;
            tblMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55F));
            tblMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));
            tblMain.Controls.Add(pnlLeft, 0, 0);
            tblMain.Controls.Add(pnlRight, 1, 0);
            tblMain.Dock = DockStyle.Fill;
            tblMain.RowCount = 1;
            tblMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            // 
            // pnlLeft
            // 
            pnlLeft.BackColor = Color.White;
            pnlLeft.Controls.Add(dgvProducts);
            pnlLeft.Dock = DockStyle.Fill;
            pnlLeft.Padding = new Padding(4);
            // 
            // dgvProducts
            // 
            dgvProducts.AllowUserToAddRows = false;
            dgvProducts.AllowUserToDeleteRows = false;
            dgvProducts.BackgroundColor = Color.White;
            dgvProducts.BorderStyle = BorderStyle.Fixed3D;
            dgvProducts.ColumnHeadersHeight = 40;
            dgvProducts.Dock = DockStyle.Fill;
            dgvProducts.MultiSelect = false;
            dgvProducts.ReadOnly = true;
            dgvProducts.RightToLeft = RightToLeft.Yes;
            dgvProducts.RowHeadersVisible = false;
            dgvProducts.RowTemplate.Height = 34;
            dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProducts.CellDoubleClick += dgvProducts_CellDoubleClick;
            dgvProducts.SelectionChanged += dgvProducts_SelectionChanged;
            // 
            // pnlRight
            // 
            pnlRight.BackColor = Color.White;
            pnlRight.Controls.Add(grpUnits);
            pnlRight.Dock = DockStyle.Fill;
            pnlRight.Padding = new Padding(4);
            // 
            // grpUnits
            // 
            grpUnits.Controls.Add(flpUnits);
            grpUnits.Controls.Add(lblTotalStock);
            grpUnits.Controls.Add(btnInsertBase);
            grpUnits.Dock = DockStyle.Fill;
            grpUnits.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            grpUnits.Padding = new Padding(8, 4, 8, 8);
            grpUnits.Text = "الوحدة";
            // 
            // flpUnits
            // 
            flpUnits.AutoScroll = true;
            flpUnits.Dock = DockStyle.Fill;
            flpUnits.FlowDirection = FlowDirection.TopDown;
            flpUnits.Padding = new Padding(2);
            flpUnits.WrapContents = false;
            // 
            // lblTotalStock
            // 
            lblTotalStock.BackColor = Color.FromArgb(211, 240, 222);
            lblTotalStock.Dock = DockStyle.Bottom;
            lblTotalStock.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalStock.ForeColor = Color.FromArgb(21, 87, 36);
            lblTotalStock.Height = 34;
            lblTotalStock.Text = "إجمالي الرصيد: —";
            lblTotalStock.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnInsertBase
            // 
            btnInsertBase.BackColor = Color.FromArgb(25, 135, 84);
            btnInsertBase.Dock = DockStyle.Bottom;
            btnInsertBase.FlatStyle = FlatStyle.Popup;
            btnInsertBase.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnInsertBase.ForeColor = Color.White;
            btnInsertBase.Height = 44;
            btnInsertBase.Text = "إدراج بالوحدة الأساسية";
            btnInsertBase.Click += btnInsertBase_Click;
            // 
            pnlTitle.ResumeLayout(false);
            tblHeader.ResumeLayout(false);
            pnlSearch.ResumeLayout(false);
            pnlSearch.PerformLayout();
            pnlFooter.ResumeLayout(false);
            flpFooter.ResumeLayout(false);
            tblMain.ResumeLayout(false);
            pnlLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvProducts).EndInit();
            pnlRight.ResumeLayout(false);
            grpUnits.ResumeLayout(false);
            // 
            // frmSalesSmartF3Lookup
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            CancelButton = btnClose;
            ClientSize = new Size(760, 520);
            Controls.Add(tblMain);
            Controls.Add(pnlFooter);
            Controls.Add(pnlSearch);
            Controls.Add(pnlTitle);
            Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmSalesSmartF3Lookup";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "اختيار المنتج [F3] — بحث ذكي";
            KeyDown += frmSalesSmartF3Lookup_KeyDown;
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlTitle;
        private System.Windows.Forms.TableLayoutPanel tblHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblEscHint;
        private System.Windows.Forms.Panel pnlSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.FlowLayoutPanel flpFooter;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TableLayoutPanel tblMain;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.DataGridView dgvProducts;
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.GroupBox grpUnits;
        private System.Windows.Forms.FlowLayoutPanel flpUnits;
        private System.Windows.Forms.Label lblTotalStock;
        private System.Windows.Forms.Button btnInsertBase;
    }
}