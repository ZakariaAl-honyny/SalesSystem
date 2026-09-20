namespace SalesSystem.Desktop.Screens.Sales.Forms
{
    partial class frmSalesImportConvert
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
            tabMain = new TabControl();
            tpQuotations = new TabPage();
            dgvQuotations = new DataGridView();
            lblQuotationNote = new Label();
            tpInvoices = new TabPage();
            dgvInvoices = new DataGridView();
            pnlFooter = new Panel();
            flpFooter = new FlowLayoutPanel();
            btnClose = new Button();
            lblCount = new Label();
            pnlHeader.SuspendLayout();
            tabMain.SuspendLayout();
            tpQuotations.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvQuotations).BeginInit();
            tpInvoices.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvInvoices).BeginInit();
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
            lblEscHint.Text = "Esc = إغلاق";
            lblEscHint.TextAlign = ContentAlignment.MiddleRight;
            lblEscHint.Width = 120;
            // 
            // lblTitle
            // 
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.Font = new Font("Tahoma", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.ForeColor = Color.White;
            lblTitle.Text = "تحويل عرض سعر / تكرار فاتورة";
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // tabMain
            // 
            tabMain.Controls.Add(tpQuotations);
            tabMain.Controls.Add(tpInvoices);
            tabMain.Dock = DockStyle.Fill;
            tabMain.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tabMain.Padding = new Point(10, 6);
            tabMain.RightToLeft = RightToLeft.Yes;
            tabMain.RightToLeftLayout = true;
            tabMain.SelectedIndex = 0;
            tabMain.SelectedIndexChanged += tabMain_SelectedIndexChanged;
            // 
            // tpQuotations
            // 
            tpQuotations.BackColor = Color.White;
            tpQuotations.Controls.Add(dgvQuotations);
            tpQuotations.Controls.Add(lblQuotationNote);
            tpQuotations.Padding = new Padding(6);
            tpQuotations.Text = "عروض الأسعار المعتمدة";
            // 
            // dgvQuotations
            // 
            dgvQuotations.AllowUserToAddRows = false;
            dgvQuotations.AllowUserToDeleteRows = false;
            dgvQuotations.BackgroundColor = Color.White;
            dgvQuotations.BorderStyle = BorderStyle.Fixed3D;
            dgvQuotations.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(233, 236, 239);
            dgvQuotations.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dgvQuotations.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(33, 43, 54);
            dgvQuotations.ColumnHeadersHeight = 40;
            dgvQuotations.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvQuotations.DefaultCellStyle.Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dgvQuotations.Dock = DockStyle.Fill;
            dgvQuotations.MultiSelect = false;
            dgvQuotations.ReadOnly = true;
            dgvQuotations.RightToLeft = RightToLeft.Yes;
            dgvQuotations.RowHeadersVisible = false;
            dgvQuotations.RowTemplate.Height = 36;
            dgvQuotations.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvQuotations.CellContentClick += dgvQuotations_CellContentClick;
            dgvQuotations.CellDoubleClick += dgvQuotations_CellDoubleClick;
            dgvQuotations.CellPainting += dgvQuotations_CellPainting;
            dgvQuotations.CellToolTipTextNeeded += dgvQuotations_CellToolTipTextNeeded;
            // 
            // lblQuotationNote
            // 
            lblQuotationNote.BackColor = Color.FromArgb(255, 249, 219);
            lblQuotationNote.Dock = DockStyle.Bottom;
            lblQuotationNote.Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblQuotationNote.ForeColor = Color.FromArgb(122, 79, 0);
            lblQuotationNote.Height = 46;
            lblQuotationNote.Padding = new Padding(10, 0, 10, 0);
            lblQuotationNote.Text = "التحويل يملأ أصناف الفاتورة مع الحفاظ على المستند المصدر والوحدات والأسعار والخصومات.";
            lblQuotationNote.TextAlign = ContentAlignment.MiddleRight;
            // 
            // tpInvoices
            // 
            tpInvoices.BackColor = Color.White;
            tpInvoices.Controls.Add(dgvInvoices);
            tpInvoices.Padding = new Padding(6);
            tpInvoices.Text = "تكرار فاتورة سابقة";
            // 
            // dgvInvoices
            // 
            dgvInvoices.AllowUserToAddRows = false;
            dgvInvoices.AllowUserToDeleteRows = false;
            dgvInvoices.BackgroundColor = Color.White;
            dgvInvoices.BorderStyle = BorderStyle.Fixed3D;
            dgvInvoices.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(233, 236, 239);
            dgvInvoices.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dgvInvoices.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(33, 43, 54);
            dgvInvoices.ColumnHeadersHeight = 40;
            dgvInvoices.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvInvoices.DefaultCellStyle.Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dgvInvoices.Dock = DockStyle.Fill;
            dgvInvoices.MultiSelect = false;
            dgvInvoices.ReadOnly = true;
            dgvInvoices.RightToLeft = RightToLeft.Yes;
            dgvInvoices.RowHeadersVisible = false;
            dgvInvoices.RowTemplate.Height = 36;
            dgvInvoices.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvInvoices.CellContentClick += dgvInvoices_CellContentClick;
            dgvInvoices.CellDoubleClick += dgvInvoices_CellDoubleClick;
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
            btnClose.BackColor = Color.White;
            btnClose.DialogResult = DialogResult.Cancel;
            btnClose.FlatStyle = FlatStyle.Popup;
            btnClose.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnClose.ForeColor = Color.FromArgb(33, 43, 54);
            btnClose.Height = 40;
            btnClose.Margin = new Padding(0, 0, 6, 0);
            btnClose.Text = "إغلاق";
            btnClose.Width = 100;
            // 
            // lblCount
            // 
            lblCount.Dock = DockStyle.Left;
            lblCount.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblCount.ForeColor = Color.FromArgb(73, 80, 87);
            lblCount.Text = "عروض الأسعار: 0";
            lblCount.TextAlign = ContentAlignment.MiddleRight;
            lblCount.Width = 260;
            // 
            // frmSalesImportConvert
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(720, 560);
            Controls.Add(tabMain);
            Controls.Add(pnlFooter);
            Controls.Add(pnlHeader);
            Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmSalesImportConvert";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "تحويل عرض سعر / تكرار فاتورة";
            KeyDown += frmSalesImportConvert_KeyDown;
            pnlHeader.ResumeLayout(false);
            tabMain.ResumeLayout(false);
            tpQuotations.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvQuotations).EndInit();
            tpInvoices.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvInvoices).EndInit();
            pnlFooter.ResumeLayout(false);
            flpFooter.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblEscHint;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tpQuotations;
        private System.Windows.Forms.DataGridView dgvQuotations;
        private System.Windows.Forms.Label lblQuotationNote;
        private System.Windows.Forms.TabPage tpInvoices;
        private System.Windows.Forms.DataGridView dgvInvoices;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.FlowLayoutPanel flpFooter;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblCount;
    }
}