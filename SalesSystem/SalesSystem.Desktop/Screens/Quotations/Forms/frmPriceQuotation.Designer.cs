namespace SalesSystem.Desktop.Screens.Quotations.Forms
{
    partial class frmPriceQuotation
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
            pnlHeader = new Panel();
            lblTitle = new Label();
            lblEscHint = new Label();
            pnlFilter = new Panel();
            tblFilter = new TableLayoutPanel();
            lblSearch = new Label();
            txtSearch = new TextBox();
            lblStatus = new Label();
            cbStatus = new ComboBox();
            lblFrom = new Label();
            dtpFrom = new DateTimePicker();
            lblTo = new Label();
            dtpTo = new DateTimePicker();
            pnlFooter = new Panel();
            flpFooter = new FlowLayoutPanel();
            btnAdd = new Button();
            btnClose = new Button();
            lblRecordsCount = new Label();
            dgvQuotations = new DataGridView();
            pnlHeader.SuspendLayout();
            pnlFilter.SuspendLayout();
            tblFilter.SuspendLayout();
            pnlFooter.SuspendLayout();
            flpFooter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvQuotations).BeginInit();
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
            lblTitle.Text = "عروض الأسعار";
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pnlFilter
            // 
            pnlFilter.BackColor = Color.White;
            pnlFilter.Controls.Add(tblFilter);
            pnlFilter.Dock = DockStyle.Top;
            pnlFilter.Height = 64;
            // 
            // tblFilter
            // 
            tblFilter.ColumnCount = 8;
            tblFilter.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 55F));   // بحث:
            tblFilter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));    // txtSearch
            tblFilter.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 65F));   // الحالة:
            tblFilter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18F));    // cbStatus
            tblFilter.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 45F));   // من:
            tblFilter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16F));    // dtpFrom
            tblFilter.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 45F));   // إلى:
            tblFilter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16F));    // dtpTo
            tblFilter.Controls.Add(lblSearch, 0, 0);
            tblFilter.Controls.Add(txtSearch, 1, 0);
            tblFilter.Controls.Add(lblStatus, 2, 0);
            tblFilter.Controls.Add(cbStatus, 3, 0);
            tblFilter.Controls.Add(lblFrom, 4, 0);
            tblFilter.Controls.Add(dtpFrom, 5, 0);
            tblFilter.Controls.Add(lblTo, 6, 0);
            tblFilter.Controls.Add(dtpTo, 7, 0);
            tblFilter.Dock = DockStyle.Fill;
            tblFilter.Padding = new Padding(10, 8, 10, 8);
            tblFilter.RightToLeft = RightToLeft.Yes;
            tblFilter.RowCount = 1;
            tblFilter.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            // 
            // lblSearch
            // 
            lblSearch.Dock = DockStyle.Fill;
            lblSearch.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSearch.Margin = new Padding(3, 0, 3, 0);
            lblSearch.Text = "بحث:";
            lblSearch.TextAlign = ContentAlignment.MiddleRight;
            // 
            // txtSearch
            // 
            txtSearch.Dock = DockStyle.Fill;
            txtSearch.Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtSearch.Margin = new Padding(3, 4, 3, 4);
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // lblStatus
            // 
            lblStatus.Dock = DockStyle.Fill;
            lblStatus.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblStatus.Margin = new Padding(3, 0, 3, 0);
            lblStatus.Text = "الحالة:";
            lblStatus.TextAlign = ContentAlignment.MiddleRight;
            // 
            // cbStatus
            // 
            cbStatus.BackColor = SystemColors.Window;
            cbStatus.Dock = DockStyle.Fill;
            cbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cbStatus.Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cbStatus.Items.AddRange(new object[] { "الكل", "معتمد", "بانتظار التعميد" });
            cbStatus.Margin = new Padding(3, 4, 3, 4);
            cbStatus.SelectedIndexChanged += cbStatus_SelectedIndexChanged;
            // 
            // lblFrom
            // 
            lblFrom.Dock = DockStyle.Fill;
            lblFrom.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblFrom.Margin = new Padding(3, 0, 3, 0);
            lblFrom.Text = "من:";
            lblFrom.TextAlign = ContentAlignment.MiddleRight;
            // 
            // dtpFrom
            // 
            dtpFrom.CustomFormat = "yyyy/MM/dd";
            dtpFrom.Dock = DockStyle.Fill;
            dtpFrom.Format = DateTimePickerFormat.Custom;
            dtpFrom.Margin = new Padding(3, 4, 3, 4);
            dtpFrom.RightToLeft = RightToLeft.Yes;
            dtpFrom.Value = new DateTime(2026, 9, 1);
            dtpFrom.ValueChanged += dtpFrom_ValueChanged;
            // 
            // lblTo
            // 
            lblTo.Dock = DockStyle.Fill;
            lblTo.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTo.Margin = new Padding(3, 0, 3, 0);
            lblTo.Text = "إلى:";
            lblTo.TextAlign = ContentAlignment.MiddleRight;
            // 
            // dtpTo
            // 
            dtpTo.CustomFormat = "yyyy/MM/dd";
            dtpTo.Dock = DockStyle.Fill;
            dtpTo.Format = DateTimePickerFormat.Custom;
            dtpTo.Margin = new Padding(3, 4, 3, 4);
            dtpTo.RightToLeft = RightToLeft.Yes;
            dtpTo.Value = new DateTime(2026, 9, 21);
            dtpTo.ValueChanged += dtpTo_ValueChanged;
            // 
            // pnlFooter
            // 
            pnlFooter.BackColor = Color.FromArgb(248, 249, 250);
            pnlFooter.Controls.Add(flpFooter);
            pnlFooter.Controls.Add(lblRecordsCount);
            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Height = 58;
            // 
            // flpFooter
            // 
            flpFooter.Controls.Add(btnAdd);
            flpFooter.Controls.Add(btnClose);
            flpFooter.Dock = DockStyle.Fill;
            flpFooter.FlowDirection = FlowDirection.RightToLeft;
            flpFooter.Padding = new Padding(8, 9, 8, 9);
            // 
            // btnAdd
            // 
            btnAdd.BackColor = Color.FromArgb(13, 110, 253);
            btnAdd.FlatStyle = FlatStyle.Popup;
            btnAdd.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAdd.ForeColor = Color.White;
            btnAdd.Height = 40;
            btnAdd.Margin = new Padding(0, 0, 6, 0);
            btnAdd.Text = "إضافة عرض سعر";
            btnAdd.Width = 150;
            btnAdd.Click += btnAdd_Click;
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
            btnClose.Click += btnClose_Click;
            // 
            // lblRecordsCount
            // 
            lblRecordsCount.Dock = DockStyle.Right;
            lblRecordsCount.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblRecordsCount.ForeColor = Color.FromArgb(73, 80, 87);
            lblRecordsCount.Text = "0";
            lblRecordsCount.TextAlign = ContentAlignment.MiddleRight;
            lblRecordsCount.Width = 90;
            // 
            // dgvQuotations
            // 
            dgvQuotations.AllowUserToAddRows = false;
            dgvQuotations.AllowUserToDeleteRows = false;
            dgvQuotations.AllowUserToOrderColumns = false;
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
            // frmPriceQuotation
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(1000, 640);
            Controls.Add(dgvQuotations);
            Controls.Add(pnlFooter);
            Controls.Add(pnlFilter);
            Controls.Add(pnlHeader);
            Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmPriceQuotation";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "عروض الأسعار";
            KeyDown += frmPriceQuotation_KeyDown;
            pnlHeader.ResumeLayout(false);
            pnlFilter.ResumeLayout(false);
            tblFilter.ResumeLayout(false);
            tblFilter.PerformLayout();
            pnlFooter.ResumeLayout(false);
            flpFooter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvQuotations).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblEscHint;
        private System.Windows.Forms.Panel pnlFilter;
        private System.Windows.Forms.TableLayoutPanel tblFilter;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ComboBox cbStatus;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.FlowLayoutPanel flpFooter;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblRecordsCount;
        private System.Windows.Forms.DataGridView dgvQuotations;
    }
}