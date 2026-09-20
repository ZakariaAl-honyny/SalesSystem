namespace SalesSystem.Desktop.Screens.Purchases.Forms
{
    partial class frmPurchases
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
            components = new System.ComponentModel.Container();
            lblMode = new Label();
            tabMain = new TabControl();
            tpNewInvoice = new TabPage();
            pnlTotals = new Panel();
            tblTotals = new TableLayoutPanel();
            lblSubTotal = new Label();
            lblSubTotalVal = new Label();
            lblDiscount = new Label();
            lblDiscountVal = new Label();
            lblTax = new Label();
            lblTaxVal = new Label();
            lblGrandTotal = new Label();
            lblGrandTotalVal = new Label();
            lblPaid = new Label();
            txtPaid = new TextBox();
            lblRemaining = new Label();
            lblRemainingVal = new Label();
            btnSavePost = new Button();
            dgvItems = new DataGridView();
            pnlInvoiceHeader = new Panel();
            tblHeader = new TableLayoutPanel();
            lblSupplier = new Label();
            cmbSupplier = new ComboBox();
            lblWarehouse = new Label();
            cmbWarehouse = new ComboBox();
            lblInvoiceDate = new Label();
            dtpInvoiceDate = new DateTimePicker();
            lblPaymentType = new Label();
            cmbPaymentType = new ComboBox();
            lblInvoiceNoLabel = new Label();
            txtInvoiceNo = new TextBox();
            tpHistory = new TabPage();
            pnlHistoryFilter = new Panel();
            flowFilter = new FlowLayoutPanel();
            lblSearch = new Label();
            txtSearch = new TextBox();
            lblPayTypeFilter = new Label();
            cbPayTypeFilter = new ComboBox();
            lblFrom = new Label();
            dtpFrom = new DateTimePicker();
            lblTo = new Label();
            dtpTo = new DateTimePicker();
            lblHistoryCount = new Label();
            dgvRecords = new DataGridView();
            tpSuppliers = new TabPage();
            pnlSuppliersTop = new Panel();
            lblSuppliersCount = new Label();
            btnAddSupplier = new Button();
            dgvSuppliers = new DataGridView();
            pnlButtons = new Panel();
            lblFooterHint = new Label();
            btnClose = new Button();
            tabMain.SuspendLayout();
            tpNewInvoice.SuspendLayout();
            pnlTotals.SuspendLayout();
            tblTotals.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvItems).BeginInit();
            pnlInvoiceHeader.SuspendLayout();
            tblHeader.SuspendLayout();
            tpHistory.SuspendLayout();
            pnlHistoryFilter.SuspendLayout();
            flowFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvRecords).BeginInit();
            tpSuppliers.SuspendLayout();
            pnlSuppliersTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSuppliers).BeginInit();
            pnlButtons.SuspendLayout();
            SuspendLayout();
            // 
            // lblMode
            // 
            lblMode.BackColor = Color.FromArgb(52, 73, 94);
            lblMode.Dock = DockStyle.Top;
            lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblMode.ForeColor = Color.White;
            lblMode.Height = 50;
            lblMode.Name = "lblMode";
            lblMode.Text = "المشتريات";
            lblMode.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tabMain
            // 
            tabMain.Controls.Add(tpNewInvoice);
            tabMain.Controls.Add(tpHistory);
            tabMain.Controls.Add(tpSuppliers);
            tabMain.Dock = DockStyle.Fill;
            tabMain.Font = new Font("Tahoma", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tabMain.Name = "tabMain";
            tabMain.RightToLeft = RightToLeft.Yes;
            tabMain.SelectedIndex = 0;
            tabMain.Size = new Size(1150, 595);
            tabMain.TabIndex = 1;
            // 
            // tpNewInvoice
            // 
            tpNewInvoice.BackColor = Color.White;
            tpNewInvoice.Controls.Add(dgvItems);
            tpNewInvoice.Controls.Add(pnlTotals);
            tpNewInvoice.Controls.Add(pnlInvoiceHeader);
            tpNewInvoice.Location = new Point(4, 34);
            tpNewInvoice.Name = "tpNewInvoice";
            tpNewInvoice.Padding = new Padding(4);
            tpNewInvoice.Size = new Size(1142, 557);
            tpNewInvoice.TabIndex = 0;
            tpNewInvoice.Text = "فاتورة مشتريات جديدة";
            // 
            // pnlTotals
            // 
            pnlTotals.BackColor = Color.FromArgb(236, 240, 241);
            pnlTotals.Controls.Add(tblTotals);
            pnlTotals.Dock = DockStyle.Bottom;
            pnlTotals.Height = 240;
            pnlTotals.Name = "pnlTotals";
            pnlTotals.Padding = new Padding(8);
            // 
            // tblTotals
            // 
            tblTotals.ColumnCount = 2;
            tblTotals.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
            tblTotals.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblTotals.Controls.Add(lblSubTotal, 0, 0);
            tblTotals.Controls.Add(lblSubTotalVal, 1, 0);
            tblTotals.Controls.Add(lblDiscount, 0, 1);
            tblTotals.Controls.Add(lblDiscountVal, 1, 1);
            tblTotals.Controls.Add(lblTax, 0, 2);
            tblTotals.Controls.Add(lblTaxVal, 1, 2);
            tblTotals.Controls.Add(lblGrandTotal, 0, 3);
            tblTotals.Controls.Add(lblGrandTotalVal, 1, 3);
            tblTotals.Controls.Add(lblPaid, 0, 4);
            tblTotals.Controls.Add(txtPaid, 1, 4);
            tblTotals.Controls.Add(lblRemaining, 0, 5);
            tblTotals.Controls.Add(lblRemainingVal, 1, 5);
            tblTotals.Controls.Add(btnSavePost, 0, 6);
            tblTotals.SetColumnSpan(btnSavePost, 2);
            tblTotals.Dock = DockStyle.Fill;
            tblTotals.Name = "tblTotals";
            tblTotals.RowCount = 7;
            tblTotals.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblTotals.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblTotals.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblTotals.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblTotals.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblTotals.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblTotals.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
            // 
            // lblSubTotal
            // 
            lblSubTotal.Dock = DockStyle.Fill;
            lblSubTotal.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSubTotal.Margin = new Padding(4, 2, 8, 2);
            lblSubTotal.Name = "lblSubTotal";
            lblSubTotal.Text = "المجموع الفرعي:";
            lblSubTotal.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblSubTotalVal
            // 
            lblSubTotalVal.Dock = DockStyle.Fill;
            lblSubTotalVal.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblSubTotalVal.Margin = new Padding(4, 2, 8, 2);
            lblSubTotalVal.Name = "lblSubTotalVal";
            lblSubTotalVal.Text = "0.00";
            lblSubTotalVal.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblDiscount
            // 
            lblDiscount.Dock = DockStyle.Fill;
            lblDiscount.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDiscount.Margin = new Padding(4, 2, 8, 2);
            lblDiscount.Name = "lblDiscount";
            lblDiscount.Text = "الخصم:";
            lblDiscount.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblDiscountVal
            // 
            lblDiscountVal.Dock = DockStyle.Fill;
            lblDiscountVal.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblDiscountVal.Margin = new Padding(4, 2, 8, 2);
            lblDiscountVal.Name = "lblDiscountVal";
            lblDiscountVal.Text = "0.00";
            lblDiscountVal.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblTax
            // 
            lblTax.Dock = DockStyle.Fill;
            lblTax.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTax.Margin = new Padding(4, 2, 8, 2);
            lblTax.Name = "lblTax";
            lblTax.Text = "الضريبة:";
            lblTax.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTaxVal
            // 
            lblTaxVal.Dock = DockStyle.Fill;
            lblTaxVal.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTaxVal.Margin = new Padding(4, 2, 8, 2);
            lblTaxVal.Name = "lblTaxVal";
            lblTaxVal.Text = "0.00";
            lblTaxVal.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblGrandTotal
            // 
            lblGrandTotal.Dock = DockStyle.Fill;
            lblGrandTotal.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblGrandTotal.Margin = new Padding(4, 2, 8, 2);
            lblGrandTotal.Name = "lblGrandTotal";
            lblGrandTotal.Text = "الإجمالي:";
            lblGrandTotal.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblGrandTotalVal
            // 
            lblGrandTotalVal.Dock = DockStyle.Fill;
            lblGrandTotalVal.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblGrandTotalVal.ForeColor = Color.FromArgb(192, 57, 43);
            lblGrandTotalVal.Margin = new Padding(4, 2, 8, 2);
            lblGrandTotalVal.Name = "lblGrandTotalVal";
            lblGrandTotalVal.Text = "0.00";
            lblGrandTotalVal.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblPaid
            // 
            lblPaid.Dock = DockStyle.Fill;
            lblPaid.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPaid.Margin = new Padding(4, 2, 8, 2);
            lblPaid.Name = "lblPaid";
            lblPaid.Text = "المدفوع:";
            lblPaid.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtPaid
            // 
            txtPaid.Dock = DockStyle.Fill;
            txtPaid.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtPaid.Margin = new Padding(4, 2, 8, 2);
            txtPaid.Name = "txtPaid";
            txtPaid.Text = "0";
            txtPaid.TextAlign = HorizontalAlignment.Right;
            txtPaid.TextChanged += txtPaid_TextChanged;
            // 
            // lblRemaining
            // 
            lblRemaining.Dock = DockStyle.Fill;
            lblRemaining.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblRemaining.Margin = new Padding(4, 2, 8, 2);
            lblRemaining.Name = "lblRemaining";
            lblRemaining.Text = "الباقي:";
            lblRemaining.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblRemainingVal
            // 
            lblRemainingVal.Dock = DockStyle.Fill;
            lblRemainingVal.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblRemainingVal.Margin = new Padding(4, 2, 8, 2);
            lblRemainingVal.Name = "lblRemainingVal";
            lblRemainingVal.Text = "0.00";
            lblRemainingVal.TextAlign = ContentAlignment.MiddleRight;
            // 
            // btnSavePost
            // 
            btnSavePost.BackColor = Color.FromArgb(39, 174, 96);
            btnSavePost.Dock = DockStyle.Fill;
            btnSavePost.FlatAppearance.BorderSize = 0;
            btnSavePost.FlatStyle = FlatStyle.Flat;
            btnSavePost.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSavePost.ForeColor = Color.White;
            btnSavePost.Margin = new Padding(4, 8, 4, 2);
            btnSavePost.Name = "btnSavePost";
            btnSavePost.Text = "حفظ وترحيل";
            btnSavePost.UseVisualStyleBackColor = false;
            btnSavePost.Click += btnSavePost_Click;
            // 
            // dgvItems
            // 
            dgvItems.AllowUserToAddRows = false;
            dgvItems.AllowUserToDeleteRows = false;
            dgvItems.AllowUserToResizeRows = false;
            dgvItems.BackgroundColor = Color.White;
            dgvItems.BorderStyle = BorderStyle.Fixed3D;
            dgvItems.ColumnHeadersHeight = 42;
            dgvItems.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvItems.Dock = DockStyle.Fill;
            dgvItems.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dgvItems.Location = new Point(4, 113);
            dgvItems.Name = "dgvItems";
            dgvItems.ReadOnly = true;
            dgvItems.RowHeadersVisible = false;
            dgvItems.RowTemplate.Height = 30;
            dgvItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvItems.Size = new Size(1134, 200);
            dgvItems.TabIndex = 1;
            // 
            // pnlInvoiceHeader
            // 
            pnlInvoiceHeader.BackColor = Color.White;
            pnlInvoiceHeader.Controls.Add(tblHeader);
            pnlInvoiceHeader.Dock = DockStyle.Top;
            pnlInvoiceHeader.Height = 100;
            pnlInvoiceHeader.Name = "pnlInvoiceHeader";
            pnlInvoiceHeader.Padding = new Padding(8, 6, 8, 6);
            // 
            // tblHeader
            // 
            tblHeader.ColumnCount = 6;
            tblHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90F));
            tblHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            tblHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90F));
            tblHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            tblHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90F));
            tblHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tblHeader.Controls.Add(lblSupplier, 0, 0);
            tblHeader.Controls.Add(cmbSupplier, 1, 0);
            tblHeader.Controls.Add(lblWarehouse, 2, 0);
            tblHeader.Controls.Add(cmbWarehouse, 3, 0);
            tblHeader.Controls.Add(lblInvoiceDate, 4, 0);
            tblHeader.Controls.Add(dtpInvoiceDate, 5, 0);
            tblHeader.Controls.Add(lblPaymentType, 0, 1);
            tblHeader.Controls.Add(cmbPaymentType, 1, 1);
            tblHeader.Controls.Add(lblInvoiceNoLabel, 2, 1);
            tblHeader.Controls.Add(txtInvoiceNo, 3, 1);
            tblHeader.Dock = DockStyle.Fill;
            tblHeader.Name = "tblHeader";
            tblHeader.RowCount = 2;
            tblHeader.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tblHeader.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            // 
            // lblSupplier
            // 
            lblSupplier.Dock = DockStyle.Fill;
            lblSupplier.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSupplier.Margin = new Padding(4, 2, 8, 2);
            lblSupplier.Name = "lblSupplier";
            lblSupplier.Text = "المورد:";
            lblSupplier.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cmbSupplier
            // 
            cmbSupplier.Dock = DockStyle.Fill;
            cmbSupplier.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSupplier.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmbSupplier.Margin = new Padding(4, 2, 8, 2);
            cmbSupplier.Name = "cmbSupplier";
            // 
            // lblWarehouse
            // 
            lblWarehouse.Dock = DockStyle.Fill;
            lblWarehouse.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblWarehouse.Margin = new Padding(4, 2, 8, 2);
            lblWarehouse.Name = "lblWarehouse";
            lblWarehouse.Text = "المستودع:";
            lblWarehouse.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cmbWarehouse
            // 
            cmbWarehouse.Dock = DockStyle.Fill;
            cmbWarehouse.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbWarehouse.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmbWarehouse.Margin = new Padding(4, 2, 8, 2);
            cmbWarehouse.Name = "cmbWarehouse";
            // 
            // lblInvoiceDate
            // 
            lblInvoiceDate.Dock = DockStyle.Fill;
            lblInvoiceDate.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblInvoiceDate.Margin = new Padding(4, 2, 8, 2);
            lblInvoiceDate.Name = "lblInvoiceDate";
            lblInvoiceDate.Text = "التاريخ:";
            lblInvoiceDate.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // dtpInvoiceDate
            // 
            dtpInvoiceDate.Dock = DockStyle.Fill;
            dtpInvoiceDate.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dtpInvoiceDate.Format = DateTimePickerFormat.Short;
            dtpInvoiceDate.Margin = new Padding(4, 2, 8, 2);
            dtpInvoiceDate.Name = "dtpInvoiceDate";
            // 
            // lblPaymentType
            // 
            lblPaymentType.Dock = DockStyle.Fill;
            lblPaymentType.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPaymentType.Margin = new Padding(4, 2, 8, 2);
            lblPaymentType.Name = "lblPaymentType";
            lblPaymentType.Text = "نوع الدفع:";
            lblPaymentType.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cmbPaymentType
            // 
            cmbPaymentType.Dock = DockStyle.Fill;
            cmbPaymentType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPaymentType.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmbPaymentType.Margin = new Padding(4, 2, 8, 2);
            cmbPaymentType.Name = "cmbPaymentType";
            // 
            // lblInvoiceNoLabel
            // 
            lblInvoiceNoLabel.Dock = DockStyle.Fill;
            lblInvoiceNoLabel.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblInvoiceNoLabel.Margin = new Padding(4, 2, 8, 2);
            lblInvoiceNoLabel.Name = "lblInvoiceNoLabel";
            lblInvoiceNoLabel.Text = "رقم الفاتورة:";
            lblInvoiceNoLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtInvoiceNo
            // 
            txtInvoiceNo.BackColor = Color.FromArgb(236, 240, 241);
            txtInvoiceNo.Dock = DockStyle.Fill;
            txtInvoiceNo.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtInvoiceNo.Margin = new Padding(4, 2, 8, 2);
            txtInvoiceNo.Name = "txtInvoiceNo";
            txtInvoiceNo.ReadOnly = true;
            txtInvoiceNo.TextAlign = HorizontalAlignment.Center;
            // 
            // tpHistory
            // 
            tpHistory.BackColor = Color.White;
            tpHistory.Controls.Add(dgvRecords);
            tpHistory.Controls.Add(pnlHistoryFilter);
            tpHistory.Location = new Point(4, 34);
            tpHistory.Name = "tpHistory";
            tpHistory.Padding = new Padding(4);
            tpHistory.Size = new Size(1142, 557);
            tpHistory.TabIndex = 1;
            tpHistory.Text = "سجل المشتريات";
            // 
            // pnlHistoryFilter
            // 
            pnlHistoryFilter.BackColor = Color.White;
            pnlHistoryFilter.Controls.Add(flowFilter);
            pnlHistoryFilter.Dock = DockStyle.Top;
            pnlHistoryFilter.Height = 60;
            pnlHistoryFilter.Name = "pnlHistoryFilter";
            pnlHistoryFilter.Padding = new Padding(8, 6, 8, 6);
            // 
            // flowFilter
            // 
            flowFilter.Controls.Add(lblSearch);
            flowFilter.Controls.Add(txtSearch);
            flowFilter.Controls.Add(lblPayTypeFilter);
            flowFilter.Controls.Add(cbPayTypeFilter);
            flowFilter.Controls.Add(lblFrom);
            flowFilter.Controls.Add(dtpFrom);
            flowFilter.Controls.Add(lblTo);
            flowFilter.Controls.Add(dtpTo);
            flowFilter.Controls.Add(lblHistoryCount);
            flowFilter.Dock = DockStyle.Fill;
            flowFilter.FlowDirection = FlowDirection.RightToLeft;
            flowFilter.Name = "flowFilter";
            flowFilter.WrapContents = false;
            // 
            // lblSearch
            // 
            lblSearch.AutoSize = true;
            lblSearch.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSearch.Margin = new Padding(0, 6, 8, 0);
            lblSearch.Name = "lblSearch";
            lblSearch.Text = "بحث:";
            // 
            // txtSearch
            // 
            txtSearch.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtSearch.Margin = new Padding(0, 4, 12, 0);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(170, 28);
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // lblPayTypeFilter
            // 
            lblPayTypeFilter.AutoSize = true;
            lblPayTypeFilter.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPayTypeFilter.Margin = new Padding(0, 6, 8, 0);
            lblPayTypeFilter.Name = "lblPayTypeFilter";
            lblPayTypeFilter.Text = "نوع الدفع:";
            // 
            // cbPayTypeFilter
            // 
            cbPayTypeFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cbPayTypeFilter.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cbPayTypeFilter.Margin = new Padding(0, 4, 12, 0);
            cbPayTypeFilter.Name = "cbPayTypeFilter";
            cbPayTypeFilter.Size = new Size(110, 28);
            cbPayTypeFilter.SelectedIndexChanged += cbPayTypeFilter_SelectedIndexChanged;
            // 
            // lblFrom
            // 
            lblFrom.AutoSize = true;
            lblFrom.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblFrom.Margin = new Padding(0, 6, 8, 0);
            lblFrom.Name = "lblFrom";
            lblFrom.Text = "من:";
            // 
            // dtpFrom
            // 
            dtpFrom.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dtpFrom.Format = DateTimePickerFormat.Short;
            dtpFrom.Margin = new Padding(0, 4, 12, 0);
            dtpFrom.Name = "dtpFrom";
            dtpFrom.Size = new Size(120, 28);
            dtpFrom.ValueChanged += dtpFrom_ValueChanged;
            // 
            // lblTo
            // 
            lblTo.AutoSize = true;
            lblTo.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTo.Margin = new Padding(0, 6, 8, 0);
            lblTo.Name = "lblTo";
            lblTo.Text = "إلى:";
            // 
            // dtpTo
            // 
            dtpTo.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dtpTo.Format = DateTimePickerFormat.Short;
            dtpTo.Margin = new Padding(0, 4, 12, 0);
            dtpTo.Name = "dtpTo";
            dtpTo.Size = new Size(120, 28);
            dtpTo.ValueChanged += dtpTo_ValueChanged;
            // 
            // lblHistoryCount
            // 
            lblHistoryCount.AutoSize = true;
            lblHistoryCount.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblHistoryCount.ForeColor = Color.Firebrick;
            lblHistoryCount.Margin = new Padding(16, 6, 0, 0);
            lblHistoryCount.Name = "lblHistoryCount";
            lblHistoryCount.Text = "عدد السجلات: 0";
            // 
            // dgvRecords
            // 
            dgvRecords.AllowUserToAddRows = false;
            dgvRecords.AllowUserToDeleteRows = false;
            dgvRecords.AllowUserToResizeRows = false;
            dgvRecords.BackgroundColor = Color.White;
            dgvRecords.BorderStyle = BorderStyle.Fixed3D;
            dgvRecords.ColumnHeadersHeight = 42;
            dgvRecords.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvRecords.Dock = DockStyle.Fill;
            dgvRecords.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dgvRecords.Location = new Point(4, 64);
            dgvRecords.Name = "dgvRecords";
            dgvRecords.ReadOnly = true;
            dgvRecords.RowHeadersVisible = false;
            dgvRecords.RowTemplate.Height = 30;
            dgvRecords.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRecords.Size = new Size(1134, 489);
            dgvRecords.TabIndex = 2;
            // 
            // tpSuppliers
            // 
            tpSuppliers.BackColor = Color.White;
            tpSuppliers.Controls.Add(dgvSuppliers);
            tpSuppliers.Controls.Add(pnlSuppliersTop);
            tpSuppliers.Location = new Point(4, 34);
            tpSuppliers.Name = "tpSuppliers";
            tpSuppliers.Padding = new Padding(4);
            tpSuppliers.Size = new Size(1142, 557);
            tpSuppliers.TabIndex = 2;
            tpSuppliers.Text = "الموردون";
            // 
            // pnlSuppliersTop
            // 
            pnlSuppliersTop.BackColor = Color.White;
            pnlSuppliersTop.Controls.Add(btnAddSupplier);
            pnlSuppliersTop.Controls.Add(lblSuppliersCount);
            pnlSuppliersTop.Dock = DockStyle.Top;
            pnlSuppliersTop.Height = 60;
            pnlSuppliersTop.Name = "pnlSuppliersTop";
            pnlSuppliersTop.Padding = new Padding(8, 8, 8, 8);
            // 
            // lblSuppliersCount
            // 
            lblSuppliersCount.Dock = DockStyle.Fill;
            lblSuppliersCount.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSuppliersCount.ForeColor = Color.Firebrick;
            lblSuppliersCount.Name = "lblSuppliersCount";
            lblSuppliersCount.Text = "عدد الموردين: 0";
            lblSuppliersCount.TextAlign = ContentAlignment.MiddleRight;
            // 
            // btnAddSupplier
            // 
            btnAddSupplier.BackColor = Color.White;
            btnAddSupplier.Dock = DockStyle.Right;
            btnAddSupplier.FlatAppearance.BorderColor = Color.Black;
            btnAddSupplier.FlatAppearance.BorderSize = 2;
            btnAddSupplier.FlatStyle = FlatStyle.Popup;
            btnAddSupplier.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAddSupplier.Name = "btnAddSupplier";
            btnAddSupplier.Size = new Size(130, 44);
            btnAddSupplier.TabIndex = 3;
            btnAddSupplier.Text = "إضافة مورد";
            btnAddSupplier.UseVisualStyleBackColor = true;
            btnAddSupplier.Click += btnAddSupplier_Click;
            // 
            // dgvSuppliers
            // 
            dgvSuppliers.AllowUserToAddRows = false;
            dgvSuppliers.AllowUserToDeleteRows = false;
            dgvSuppliers.AllowUserToResizeRows = false;
            dgvSuppliers.BackgroundColor = Color.White;
            dgvSuppliers.BorderStyle = BorderStyle.Fixed3D;
            dgvSuppliers.ColumnHeadersHeight = 42;
            dgvSuppliers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvSuppliers.Dock = DockStyle.Fill;
            dgvSuppliers.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dgvSuppliers.Location = new Point(4, 64);
            dgvSuppliers.Name = "dgvSuppliers";
            dgvSuppliers.ReadOnly = true;
            dgvSuppliers.RowHeadersVisible = false;
            dgvSuppliers.RowTemplate.Height = 30;
            dgvSuppliers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSuppliers.Size = new Size(1134, 489);
            dgvSuppliers.TabIndex = 4;
            // 
            // pnlButtons
            // 
            pnlButtons.BackColor = Color.FromArgb(236, 240, 241);
            pnlButtons.Controls.Add(btnClose);
            pnlButtons.Controls.Add(lblFooterHint);
            pnlButtons.Dock = DockStyle.Bottom;
            pnlButtons.Height = 55;
            pnlButtons.Name = "pnlButtons";
            pnlButtons.Padding = new Padding(8);
            // 
            // lblFooterHint
            // 
            lblFooterHint.Dock = DockStyle.Fill;
            lblFooterHint.Font = new Font("Tahoma", 9F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lblFooterHint.ForeColor = Color.Gray;
            lblFooterHint.Name = "lblFooterHint";
            lblFooterHint.Text = "وضع التجربة (Mock) — بيانات وهمية للعرض فقط، بدون اتصال بالخادم";
            lblFooterHint.TextAlign = ContentAlignment.MiddleRight;
            // 
            // btnClose
            // 
            btnClose.BackColor = Color.White;
            btnClose.Dock = DockStyle.Right;
            btnClose.FlatAppearance.BorderColor = Color.Black;
            btnClose.FlatAppearance.BorderSize = 2;
            btnClose.FlatStyle = FlatStyle.Popup;
            btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(110, 39);
            btnClose.TabIndex = 5;
            btnClose.Text = "إغلاق";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;
            // 
            // frmPurchases
            // 
            AcceptButton = btnSavePost;
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            CancelButton = btnClose;
            ClientSize = new Size(1150, 700);
            Controls.Add(tabMain);
            Controls.Add(lblMode);
            Controls.Add(pnlButtons);
            Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "frmPurchases";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "المشتريات";
            Load += frmPurchases_Load;
            pnlButtons.ResumeLayout(false);
            lblFooterHint.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvSuppliers).EndInit();
            pnlSuppliersTop.ResumeLayout(false);
            tpSuppliers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvRecords).EndInit();
            flowFilter.ResumeLayout(false);
            flowFilter.PerformLayout();
            pnlHistoryFilter.ResumeLayout(false);
            tpHistory.ResumeLayout(false);
            tblHeader.ResumeLayout(false);
            pnlInvoiceHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvItems).EndInit();
            tblTotals.ResumeLayout(false);
            tblTotals.PerformLayout();
            pnlTotals.ResumeLayout(false);
            tpNewInvoice.ResumeLayout(false);
            tabMain.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tpNewInvoice;
        private System.Windows.Forms.TabPage tpHistory;
        private System.Windows.Forms.TabPage tpSuppliers;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblFooterHint;

        private System.Windows.Forms.Panel pnlInvoiceHeader;
        private System.Windows.Forms.TableLayoutPanel tblHeader;
        private System.Windows.Forms.Label lblSupplier;
        private System.Windows.Forms.ComboBox cmbSupplier;
        private System.Windows.Forms.Label lblWarehouse;
        private System.Windows.Forms.ComboBox cmbWarehouse;
        private System.Windows.Forms.Label lblInvoiceDate;
        private System.Windows.Forms.DateTimePicker dtpInvoiceDate;
        private System.Windows.Forms.Label lblPaymentType;
        private System.Windows.Forms.ComboBox cmbPaymentType;
        private System.Windows.Forms.Label lblInvoiceNoLabel;
        private System.Windows.Forms.TextBox txtInvoiceNo;
        private System.Windows.Forms.DataGridView dgvItems;
        private System.Windows.Forms.Panel pnlTotals;
        private System.Windows.Forms.TableLayoutPanel tblTotals;
        private System.Windows.Forms.Label lblSubTotal;
        private System.Windows.Forms.Label lblSubTotalVal;
        private System.Windows.Forms.Label lblDiscount;
        private System.Windows.Forms.Label lblDiscountVal;
        private System.Windows.Forms.Label lblTax;
        private System.Windows.Forms.Label lblTaxVal;
        private System.Windows.Forms.Label lblGrandTotal;
        private System.Windows.Forms.Label lblGrandTotalVal;
        private System.Windows.Forms.Label lblPaid;
        private System.Windows.Forms.TextBox txtPaid;
        private System.Windows.Forms.Label lblRemaining;
        private System.Windows.Forms.Label lblRemainingVal;
        private System.Windows.Forms.Button btnSavePost;

        private System.Windows.Forms.Panel pnlHistoryFilter;
        private System.Windows.Forms.FlowLayoutPanel flowFilter;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblPayTypeFilter;
        private System.Windows.Forms.ComboBox cbPayTypeFilter;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label lblHistoryCount;
        private System.Windows.Forms.DataGridView dgvRecords;

        private System.Windows.Forms.Panel pnlSuppliersTop;
        private System.Windows.Forms.Button btnAddSupplier;
        private System.Windows.Forms.Label lblSuppliersCount;
        private System.Windows.Forms.DataGridView dgvSuppliers;
    }
}