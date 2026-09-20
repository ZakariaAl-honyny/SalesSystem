namespace SalesSystem.Desktop.Screens.POS.Forms
{
    partial class frmPOSHeldOrders
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
            lblSearch = new Label();
            txtSearch = new TextBox();
            dgvHeldOrders = new DataGridView();
            pnlFooter = new Panel();
            flpFooter = new FlowLayoutPanel();
            btnClose = new Button();
            lblCount = new Label();
            pnlHeader.SuspendLayout();
            pnlSearch.SuspendLayout();
            tblSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvHeldOrders).BeginInit();
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
            lblEscHint.Text = "Esc = إغلاق  |  Enter = استرجاع";
            lblEscHint.TextAlign = ContentAlignment.MiddleRight;
            lblEscHint.Width = 240;
            // 
            // lblTitle
            // 
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.Font = new Font("Tahoma", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.ForeColor = Color.White;
            lblTitle.Text = "الفواتير المعلقة [F4]";
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
            tblSearch.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
            tblSearch.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblSearch.Controls.Add(lblSearch, 0, 0);
            tblSearch.Controls.Add(txtSearch, 1, 0);
            tblSearch.Dock = DockStyle.Fill;
            tblSearch.RowCount = 1;
            tblSearch.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            // 
            // lblSearch
            // 
            lblSearch.Dock = DockStyle.Fill;
            lblSearch.Font = new Font("Tahoma", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblSearch.Text = "بحث بالعميل:";
            lblSearch.TextAlign = ContentAlignment.MiddleRight;
            // 
            // txtSearch
            // 
            txtSearch.Dock = DockStyle.Fill;
            txtSearch.Font = new Font("Tahoma", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtSearch.PlaceholderText = "اكتب اسم العميل للبحث...";
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // dgvHeldOrders
            // 
            dgvHeldOrders.AllowUserToAddRows = false;
            dgvHeldOrders.AllowUserToDeleteRows = false;
            dgvHeldOrders.BackgroundColor = Color.White;
            dgvHeldOrders.BorderStyle = BorderStyle.Fixed3D;
            dgvHeldOrders.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(233, 236, 239);
            dgvHeldOrders.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dgvHeldOrders.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(33, 43, 54);
            dgvHeldOrders.ColumnHeadersHeight = 40;
            dgvHeldOrders.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvHeldOrders.DefaultCellStyle.Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dgvHeldOrders.Dock = DockStyle.Fill;
            dgvHeldOrders.MultiSelect = false;
            dgvHeldOrders.ReadOnly = true;
            dgvHeldOrders.RightToLeft = RightToLeft.Yes;
            dgvHeldOrders.RowHeadersVisible = false;
            dgvHeldOrders.RowTemplate.Height = 40;
            dgvHeldOrders.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvHeldOrders.CellContentClick += dgvHeldOrders_CellContentClick;
            dgvHeldOrders.CellDoubleClick += dgvHeldOrders_CellDoubleClick;
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
            flpFooter.Controls.Add(btnClose);
            flpFooter.Dock = DockStyle.Fill;
            flpFooter.FlowDirection = FlowDirection.RightToLeft;
            flpFooter.Padding = new Padding(8, 9, 8, 9);
            // 
            // btnClose
            // 
            btnClose.BackColor = Color.FromArgb(33, 43, 54);
            btnClose.DialogResult = DialogResult.Cancel;
            btnClose.FlatStyle = FlatStyle.Popup;
            btnClose.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnClose.ForeColor = Color.White;
            btnClose.Height = 40;
            btnClose.Margin = new Padding(0, 0, 6, 0);
            btnClose.Text = "إغلاق";
            btnClose.Width = 130;
            // 
            // lblCount
            // 
            lblCount.Dock = DockStyle.Left;
            lblCount.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblCount.ForeColor = Color.FromArgb(73, 80, 87);
            lblCount.Text = "عدد الفواتير المعلقة: 0";
            lblCount.TextAlign = ContentAlignment.MiddleRight;
            lblCount.Width = 280;
            // 
            // frmPOSHeldOrders
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            CancelButton = btnClose;
            ClientSize = new Size(600, 600);
            Controls.Add(dgvHeldOrders);
            Controls.Add(pnlFooter);
            Controls.Add(pnlSearch);
            Controls.Add(pnlHeader);
            Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmPOSHeldOrders";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "الفواتير المعلقة [F4]";
            KeyDown += frmPOSHeldOrders_KeyDown;
            pnlHeader.ResumeLayout(false);
            pnlSearch.ResumeLayout(false);
            tblSearch.ResumeLayout(false);
            tblSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvHeldOrders).EndInit();
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
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.DataGridView dgvHeldOrders;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.FlowLayoutPanel flpFooter;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblCount;
    }
}