namespace SalesSystem.Desktop.Screens.Payments.Forms
{
    partial class frmListPayments
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
            this.components = new System.ComponentModel.Container();
            this.lblMode = new System.Windows.Forms.Label();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.pnlFilters = new System.Windows.Forms.Panel();
            this.gbFilter = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbFilterBy = new System.Windows.Forms.ComboBox();
            this.txtFilterValue = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbStatus = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.btnRangeToday = new System.Windows.Forms.Button();
            this.btnRange30 = new System.Windows.Forms.Button();
            this.btnRangeMonth = new System.Windows.Forms.Button();
            this.chkOnlyCancelled = new System.Windows.Forms.CheckBox();
            this.pnlSummary = new System.Windows.Forms.Panel();
            this.lblSummary = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tabPayments = new System.Windows.Forms.TabControl();
            this.tpCustomerPayments = new System.Windows.Forms.TabPage();
            this.dgvCustomerPayments = new System.Windows.Forms.DataGridView();
            this.pnlCustomerTop = new System.Windows.Forms.Panel();
            this.btnAddCustomerPayment = new System.Windows.Forms.Button();
            this.lblCustomerCount = new System.Windows.Forms.Label();
            this.lblCustomerCountTitle = new System.Windows.Forms.Label();
            this.tpSupplierPayments = new System.Windows.Forms.TabPage();
            this.dgvSupplierPayments = new System.Windows.Forms.DataGridView();
            this.pnlSupplierTop = new System.Windows.Forms.Panel();
            this.btnAddSupplierPayment = new System.Windows.Forms.Button();
            this.lblSupplierCount = new System.Windows.Forms.Label();
            this.lblSupplierCountTitle = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.cmsCustomerPayments = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiCustomerRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiCustomerEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCustomerAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsSupplierPayments = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiSupplierRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiSupplierEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSupplierAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlHeader.SuspendLayout();
            this.pnlFilters.SuspendLayout();
            this.gbFilter.SuspendLayout();
            this.pnlSummary.SuspendLayout();
            this.tabPayments.SuspendLayout();
            this.tpCustomerPayments.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomerPayments)).BeginInit();
            this.pnlCustomerTop.SuspendLayout();
            this.tpSupplierPayments.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSupplierPayments)).BeginInit();
            this.pnlSupplierTop.SuspendLayout();
            this.cmsCustomerPayments.SuspendLayout();
            this.cmsSupplierPayments.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblMode
            // 
            this.lblMode.AutoSize = false;
            this.lblMode.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblMode.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold);
            this.lblMode.ForeColor = System.Drawing.Color.Firebrick;
            this.lblMode.Location = new System.Drawing.Point(0, 0);
            this.lblMode.Name = "lblMode";
            this.lblMode.Size = new System.Drawing.Size(1070, 56);
            this.lblMode.TabIndex = 30;
            this.lblMode.Text = "إدارة المقبوضات والمدفوعات";
            this.lblMode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.lblMode);
            this.pnlHeader.Controls.Add(this.btnClose);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1200, 56);
            this.pnlHeader.TabIndex = 30;
            // 
            // pnlFilters
            // 
            this.pnlFilters.Controls.Add(this.gbFilter);
            this.pnlFilters.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFilters.Location = new System.Drawing.Point(0, 56);
            this.pnlFilters.Margin = new System.Windows.Forms.Padding(0);
            this.pnlFilters.Name = "pnlFilters";
            this.pnlFilters.Padding = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.pnlFilters.Size = new System.Drawing.Size(1200, 112);
            this.pnlFilters.TabIndex = 20;
            // 
            // gbFilter
            // 
            this.gbFilter.Controls.Add(this.label2);
            this.gbFilter.Controls.Add(this.cbFilterBy);
            this.gbFilter.Controls.Add(this.txtFilterValue);
            this.gbFilter.Controls.Add(this.label1);
            this.gbFilter.Controls.Add(this.cbStatus);
            this.gbFilter.Controls.Add(this.label3);
            this.gbFilter.Controls.Add(this.dtpFrom);
            this.gbFilter.Controls.Add(this.label4);
            this.gbFilter.Controls.Add(this.dtpTo);
            this.gbFilter.Controls.Add(this.btnRangeToday);
            this.gbFilter.Controls.Add(this.btnRange30);
            this.gbFilter.Controls.Add(this.btnRangeMonth);
            this.gbFilter.Controls.Add(this.chkOnlyCancelled);
            this.gbFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbFilter.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.gbFilter.Location = new System.Drawing.Point(6, 4);
            this.gbFilter.Margin = new System.Windows.Forms.Padding(0);
            this.gbFilter.Name = "gbFilter";
            this.gbFilter.Padding = new System.Windows.Forms.Padding(8);
            this.gbFilter.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.gbFilter.Size = new System.Drawing.Size(1188, 104);
            this.gbFilter.TabIndex = 0;
            this.gbFilter.TabStop = false;
            this.gbFilter.Text = "بحث وتصفية";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(12, 30);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 26);
            this.label2.TabIndex = 0;
            this.label2.Text = "تصفية بـ:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbFilterBy
            // 
            this.cbFilterBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFilterBy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbFilterBy.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.cbFilterBy.FormattingEnabled = true;
            this.cbFilterBy.Location = new System.Drawing.Point(112, 26);
            this.cbFilterBy.Margin = new System.Windows.Forms.Padding(0);
            this.cbFilterBy.Name = "cbFilterBy";
            this.cbFilterBy.Size = new System.Drawing.Size(176, 30);
            this.cbFilterBy.TabIndex = 1;
            this.cbFilterBy.SelectedIndexChanged += new System.EventHandler(this.cbFilterBy_SelectedIndexChanged);
            // 
            // txtFilterValue
            // 
            this.txtFilterValue.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.txtFilterValue.Location = new System.Drawing.Point(292, 26);
            this.txtFilterValue.Margin = new System.Windows.Forms.Padding(0);
            this.txtFilterValue.Name = "txtFilterValue";
            this.txtFilterValue.Size = new System.Drawing.Size(236, 30);
            this.txtFilterValue.TabIndex = 2;
            this.txtFilterValue.TextChanged += new System.EventHandler(this.txtFilterValue_TextChanged);
            this.txtFilterValue.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtFilterValue_KeyDown);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(536, 30);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 26);
            this.label1.TabIndex = 3;
            this.label1.Text = "الحالة:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbStatus
            // 
            this.cbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbStatus.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.cbStatus.FormattingEnabled = true;
            this.cbStatus.Location = new System.Drawing.Point(612, 26);
            this.cbStatus.Margin = new System.Windows.Forms.Padding(0);
            this.cbStatus.Name = "cbStatus";
            this.cbStatus.Size = new System.Drawing.Size(124, 30);
            this.cbStatus.TabIndex = 4;
            this.cbStatus.SelectedIndexChanged += new System.EventHandler(this.cbStatus_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(744, 30);
            this.label3.Margin = new System.Windows.Forms.Padding(0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 26);
            this.label3.TabIndex = 5;
            this.label3.Text = "من:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpFrom
            // 
            this.dtpFrom.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFrom.Location = new System.Drawing.Point(800, 26);
            this.dtpFrom.Margin = new System.Windows.Forms.Padding(0);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(146, 30);
            this.dtpFrom.TabIndex = 6;
            this.dtpFrom.ValueChanged += new System.EventHandler(this.dtpFrom_ValueChanged);
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(954, 30);
            this.label4.Margin = new System.Windows.Forms.Padding(0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 26);
            this.label4.TabIndex = 7;
            this.label4.Text = "إلى:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpTo
            // 
            this.dtpTo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTo.Location = new System.Drawing.Point(1010, 26);
            this.dtpTo.Margin = new System.Windows.Forms.Padding(0);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(146, 30);
            this.dtpTo.TabIndex = 8;
            this.dtpTo.ValueChanged += new System.EventHandler(this.dtpTo_ValueChanged);
            // 
            // btnRangeToday
            // 
            this.btnRangeToday.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnRangeToday.Location = new System.Drawing.Point(12, 62);
            this.btnRangeToday.Name = "btnRangeToday";
            this.btnRangeToday.Size = new System.Drawing.Size(100, 32);
            this.btnRangeToday.TabIndex = 9;
            this.btnRangeToday.Text = "اليوم";
            this.btnRangeToday.UseVisualStyleBackColor = true;
            this.btnRangeToday.Click += new System.EventHandler(this.btnRangeToday_Click);
            // 
            // btnRange30
            // 
            this.btnRange30.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnRange30.Location = new System.Drawing.Point(122, 62);
            this.btnRange30.Name = "btnRange30";
            this.btnRange30.Size = new System.Drawing.Size(138, 32);
            this.btnRange30.TabIndex = 10;
            this.btnRange30.Text = "آخر 30 يوماً";
            this.btnRange30.UseVisualStyleBackColor = true;
            this.btnRange30.Click += new System.EventHandler(this.btnRange30_Click);
            // 
            // btnRangeMonth
            // 
            this.btnRangeMonth.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnRangeMonth.Location = new System.Drawing.Point(270, 62);
            this.btnRangeMonth.Name = "btnRangeMonth";
            this.btnRangeMonth.Size = new System.Drawing.Size(130, 32);
            this.btnRangeMonth.TabIndex = 11;
            this.btnRangeMonth.Text = "هذا الشهر";
            this.btnRangeMonth.UseVisualStyleBackColor = true;
            this.btnRangeMonth.Click += new System.EventHandler(this.btnRangeMonth_Click);
            // 
            // chkOnlyCancelled
            // 
            this.chkOnlyCancelled.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this.chkOnlyCancelled.Location = new System.Drawing.Point(412, 66);
            this.chkOnlyCancelled.Margin = new System.Windows.Forms.Padding(0);
            this.chkOnlyCancelled.Name = "chkOnlyCancelled";
            this.chkOnlyCancelled.Size = new System.Drawing.Size(170, 26);
            this.chkOnlyCancelled.TabIndex = 12;
            this.chkOnlyCancelled.Text = "الملغاة فقط";
            this.chkOnlyCancelled.UseVisualStyleBackColor = false;
            this.chkOnlyCancelled.CheckedChanged += new System.EventHandler(this.chkOnlyCancelled_CheckedChanged);
            // 
            // pnlSummary
            // 
            this.pnlSummary.Controls.Add(this.lblSummary);
            this.pnlSummary.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlSummary.Location = new System.Drawing.Point(0, 671);
            this.pnlSummary.Margin = new System.Windows.Forms.Padding(0);
            this.pnlSummary.Name = "pnlSummary";
            this.pnlSummary.Size = new System.Drawing.Size(1200, 32);
            this.pnlSummary.TabIndex = 10;
            // 
            // lblSummary
            // 
            this.lblSummary.AutoSize = false;
            this.lblSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSummary.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblSummary.Location = new System.Drawing.Point(0, 0);
            this.lblSummary.Name = "lblSummary";
            this.lblSummary.Size = new System.Drawing.Size(1200, 32);
            this.lblSummary.TabIndex = 0;
            this.lblSummary.Text = "المعروض: 0 من 0 — الإجمالي: 0.00";
            this.lblSummary.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabPayments
            // 
            this.tabPayments.Controls.Add(this.tpCustomerPayments);
            this.tabPayments.Controls.Add(this.tpSupplierPayments);
            this.tabPayments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabPayments.Font = new System.Drawing.Font("Tahoma", 9F);
            this.tabPayments.Location = new System.Drawing.Point(0, 168);
            this.tabPayments.Name = "tabPayments";
            this.tabPayments.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tabPayments.RightToLeftLayout = true;
            this.tabPayments.SelectedIndex = 0;
            this.tabPayments.Size = new System.Drawing.Size(1200, 503);
            this.tabPayments.TabIndex = 1;
            this.tabPayments.SelectedIndexChanged += new System.EventHandler(this.tabPayments_SelectedIndexChanged);
            // 
            // tpCustomerPayments
            // 
            this.tpCustomerPayments.BackColor = System.Drawing.Color.White;
            this.tpCustomerPayments.Controls.Add(this.dgvCustomerPayments);
            this.tpCustomerPayments.Controls.Add(this.pnlCustomerTop);
            this.tpCustomerPayments.Location = new System.Drawing.Point(4, 28);
            this.tpCustomerPayments.Name = "tpCustomerPayments";
            this.tpCustomerPayments.Padding = new System.Windows.Forms.Padding(3);
            this.tpCustomerPayments.Size = new System.Drawing.Size(1192, 471);
            this.tpCustomerPayments.TabIndex = 0;
            this.tpCustomerPayments.Text = "\u0645\u0642\u0628\u0648\u0636\u0627\u062a \u0627\u0644\u0639\u0645\u0644\u0627\u0621";
            // 
            // dgvCustomerPayments
            // 
            this.dgvCustomerPayments.AllowUserToAddRows = false;
            this.dgvCustomerPayments.AllowUserToDeleteRows = false;
            this.dgvCustomerPayments.AllowUserToOrderColumns = true;
            this.dgvCustomerPayments.AllowUserToResizeRows = false;
            this.dgvCustomerPayments.AutoGenerateColumns = false;
            this.dgvCustomerPayments.BackgroundColor = System.Drawing.Color.White;
            this.dgvCustomerPayments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvCustomerPayments.ColumnHeadersHeight = 50;
            this.dgvCustomerPayments.ContextMenuStrip = this.cmsCustomerPayments;
            this.dgvCustomerPayments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCustomerPayments.Location = new System.Drawing.Point(3, 43);
            this.dgvCustomerPayments.MultiSelect = false;
            this.dgvCustomerPayments.Name = "dgvCustomerPayments";
            this.dgvCustomerPayments.ReadOnly = true;
            this.dgvCustomerPayments.RowHeadersWidth = 51;
            this.dgvCustomerPayments.RowTemplate.Height = 28;
            this.dgvCustomerPayments.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCustomerPayments.Size = new System.Drawing.Size(1186, 425);
            this.dgvCustomerPayments.TabIndex = 1;
            // 
            // pnlCustomerTop
            // 
            this.pnlCustomerTop.Controls.Add(this.btnAddCustomerPayment);
            this.pnlCustomerTop.Controls.Add(this.lblCustomerCount);
            this.pnlCustomerTop.Controls.Add(this.lblCustomerCountTitle);
            this.pnlCustomerTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCustomerTop.Height = 40;
            this.pnlCustomerTop.Name = "pnlCustomerTop";
            this.pnlCustomerTop.TabIndex = 0;
            // 
            // btnAddCustomerPayment
            // 
            this.btnAddCustomerPayment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddCustomerPayment.Location = new System.Drawing.Point(920, 5);
            this.btnAddCustomerPayment.Name = "btnAddCustomerPayment";
            this.btnAddCustomerPayment.Size = new System.Drawing.Size(106, 30);
            this.btnAddCustomerPayment.TabIndex = 2;
            this.btnAddCustomerPayment.Text = "\u0642\u0628\u0636 \u062c\u062f\u064a\u062f";
            this.btnAddCustomerPayment.UseVisualStyleBackColor = true;
            this.btnAddCustomerPayment.Click += new System.EventHandler(this.btnAddCustomerPayment_Click);
            // 
            // lblCustomerCount
            // 
            this.lblCustomerCount.AutoSize = true;
            this.lblCustomerCount.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblCustomerCount.Location = new System.Drawing.Point(50, 10);
            this.lblCustomerCount.Name = "lblCustomerCount";
            this.lblCustomerCount.Size = new System.Drawing.Size(22, 18);
            this.lblCustomerCount.TabIndex = 1;
            this.lblCustomerCount.Text = "0";
            // 
            // lblCustomerCountTitle
            // 
            this.lblCustomerCountTitle.AutoSize = true;
            this.lblCustomerCountTitle.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblCustomerCountTitle.Location = new System.Drawing.Point(10, 10);
            this.lblCustomerCountTitle.Name = "lblCustomerCountTitle";
            this.lblCustomerCountTitle.Size = new System.Drawing.Size(40, 18);
            this.lblCustomerCountTitle.TabIndex = 0;
            this.lblCustomerCountTitle.Text = "\u0627\u0644\u0639\u062f\u062f:";
            // 
            // tpSupplierPayments
            // 
            this.tpSupplierPayments.BackColor = System.Drawing.Color.White;
            this.tpSupplierPayments.Controls.Add(this.dgvSupplierPayments);
            this.tpSupplierPayments.Controls.Add(this.pnlSupplierTop);
            this.tpSupplierPayments.Location = new System.Drawing.Point(4, 28);
            this.tpSupplierPayments.Name = "tpSupplierPayments";
            this.tpSupplierPayments.Padding = new System.Windows.Forms.Padding(3);
            this.tpSupplierPayments.Size = new System.Drawing.Size(1192, 471);
            this.tpSupplierPayments.TabIndex = 1;
            this.tpSupplierPayments.Text = "\u0645\u062f\u0641\u0648\u0639\u0627\u062a \u0627\u0644\u0645\u0648\u0631\u062f\u064a\u0646";
            // 
            // dgvSupplierPayments
            // 
            this.dgvSupplierPayments.AllowUserToAddRows = false;
            this.dgvSupplierPayments.AllowUserToDeleteRows = false;
            this.dgvSupplierPayments.AllowUserToOrderColumns = true;
            this.dgvSupplierPayments.AllowUserToResizeRows = false;
            this.dgvSupplierPayments.AutoGenerateColumns = false;
            this.dgvSupplierPayments.BackgroundColor = System.Drawing.Color.White;
            this.dgvSupplierPayments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvSupplierPayments.ColumnHeadersHeight = 50;
            this.dgvSupplierPayments.ContextMenuStrip = this.cmsSupplierPayments;
            this.dgvSupplierPayments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSupplierPayments.Location = new System.Drawing.Point(3, 43);
            this.dgvSupplierPayments.MultiSelect = false;
            this.dgvSupplierPayments.Name = "dgvSupplierPayments";
            this.dgvSupplierPayments.ReadOnly = true;
            this.dgvSupplierPayments.RowHeadersWidth = 51;
            this.dgvSupplierPayments.RowTemplate.Height = 28;
            this.dgvSupplierPayments.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSupplierPayments.Size = new System.Drawing.Size(1186, 425);
            this.dgvSupplierPayments.TabIndex = 1;
            // 
            // pnlSupplierTop
            // 
            this.pnlSupplierTop.Controls.Add(this.btnAddSupplierPayment);
            this.pnlSupplierTop.Controls.Add(this.lblSupplierCount);
            this.pnlSupplierTop.Controls.Add(this.lblSupplierCountTitle);
            this.pnlSupplierTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSupplierTop.Height = 40;
            this.pnlSupplierTop.Name = "pnlSupplierTop";
            this.pnlSupplierTop.TabIndex = 0;
            // 
            // btnAddSupplierPayment
            // 
            this.btnAddSupplierPayment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddSupplierPayment.Location = new System.Drawing.Point(920, 5);
            this.btnAddSupplierPayment.Name = "btnAddSupplierPayment";
            this.btnAddSupplierPayment.Size = new System.Drawing.Size(106, 30);
            this.btnAddSupplierPayment.TabIndex = 2;
            this.btnAddSupplierPayment.Text = "\u0635\u0631\u0641 \u062c\u062f\u064a\u062f";
            this.btnAddSupplierPayment.UseVisualStyleBackColor = true;
            this.btnAddSupplierPayment.Click += new System.EventHandler(this.btnAddSupplierPayment_Click);
            // 
            // lblSupplierCount
            // 
            this.lblSupplierCount.AutoSize = true;
            this.lblSupplierCount.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblSupplierCount.Location = new System.Drawing.Point(50, 10);
            this.lblSupplierCount.Name = "lblSupplierCount";
            this.lblSupplierCount.Size = new System.Drawing.Size(22, 18);
            this.lblSupplierCount.TabIndex = 1;
            this.lblSupplierCount.Text = "0";
            // 
            // lblSupplierCountTitle
            // 
            this.lblSupplierCountTitle.AutoSize = true;
            this.lblSupplierCountTitle.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblSupplierCountTitle.Location = new System.Drawing.Point(10, 10);
            this.lblSupplierCountTitle.Name = "lblSupplierCountTitle";
            this.lblSupplierCountTitle.Size = new System.Drawing.Size(40, 18);
            this.lblSupplierCountTitle.TabIndex = 0;
            this.lblSupplierCountTitle.Text = "\u0627\u0644\u0639\u062f\u062f:";
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.White;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClose.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btnClose.Location = new System.Drawing.Point(1070, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(130, 56);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "إغلاق";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // cmsCustomerPayments
            // 
            this.cmsCustomerPayments.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmsCustomerPayments.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiCustomerRefresh,
            this.toolStripSeparator1,
            this.tsmiCustomerEdit,
            this.tsmiCustomerAdd});
            this.cmsCustomerPayments.Name = "cmsCustomerPayments";
            this.cmsCustomerPayments.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cmsCustomerPayments.Size = new System.Drawing.Size(161, 88);
            this.cmsCustomerPayments.Opening += new System.ComponentModel.CancelEventHandler(this.cmsCustomerPayments_Opening);
            // 
            // tsmiCustomerRefresh
            // 
            this.tsmiCustomerRefresh.Name = "tsmiCustomerRefresh";
            this.tsmiCustomerRefresh.Size = new System.Drawing.Size(160, 26);
            this.tsmiCustomerRefresh.Text = "\u062a\u062d\u062f\u064a\u062b";
            this.tsmiCustomerRefresh.Click += new System.EventHandler(this.tsmiCustomerRefresh_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(157, 6);
            // 
            // tsmiCustomerEdit
            // 
            this.tsmiCustomerEdit.Name = "tsmiCustomerEdit";
            this.tsmiCustomerEdit.Size = new System.Drawing.Size(160, 26);
            this.tsmiCustomerEdit.Text = "\u0641\u062a\u062d \u0627\u0644\u0633\u0646\u062f";
            this.tsmiCustomerEdit.Click += new System.EventHandler(this.tsmiCustomerEdit_Click);
            // 
            // tsmiCustomerAdd
            // 
            this.tsmiCustomerAdd.Name = "tsmiCustomerAdd";
            this.tsmiCustomerAdd.Size = new System.Drawing.Size(160, 26);
            this.tsmiCustomerAdd.Text = "\u0642\u0628\u0636 \u062c\u062f\u064a\u062f";
            this.tsmiCustomerAdd.Click += new System.EventHandler(this.tsmiCustomerAdd_Click);
            // 
            // cmsSupplierPayments
            // 
            this.cmsSupplierPayments.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmsSupplierPayments.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiSupplierRefresh,
            this.toolStripSeparator2,
            this.tsmiSupplierEdit,
            this.tsmiSupplierAdd});
            this.cmsSupplierPayments.Name = "cmsSupplierPayments";
            this.cmsSupplierPayments.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cmsSupplierPayments.Size = new System.Drawing.Size(161, 88);
            this.cmsSupplierPayments.Opening += new System.ComponentModel.CancelEventHandler(this.cmsSupplierPayments_Opening);
            // 
            // tsmiSupplierRefresh
            // 
            this.tsmiSupplierRefresh.Name = "tsmiSupplierRefresh";
            this.tsmiSupplierRefresh.Size = new System.Drawing.Size(160, 26);
            this.tsmiSupplierRefresh.Text = "\u062a\u062d\u062f\u064a\u062b";
            this.tsmiSupplierRefresh.Click += new System.EventHandler(this.tsmiSupplierRefresh_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(157, 6);
            // 
            // tsmiSupplierEdit
            // 
            this.tsmiSupplierEdit.Name = "tsmiSupplierEdit";
            this.tsmiSupplierEdit.Size = new System.Drawing.Size(160, 26);
            this.tsmiSupplierEdit.Text = "\u0641\u062a\u062d \u0627\u0644\u0633\u0646\u062f";
            this.tsmiSupplierEdit.Click += new System.EventHandler(this.tsmiSupplierEdit_Click);
            // 
            // tsmiSupplierAdd
            // 
            this.tsmiSupplierAdd.Name = "tsmiSupplierAdd";
            this.tsmiSupplierAdd.Size = new System.Drawing.Size(160, 26);
            this.tsmiSupplierAdd.Text = "\u0635\u0631\u0641 \u062c\u062f\u064a\u062f";
            this.tsmiSupplierAdd.Click += new System.EventHandler(this.tsmiSupplierAdd_Click);
            // 
            // frmListPayments
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(1200, 703);
            this.Controls.Add(this.tabPayments);
            this.Controls.Add(this.pnlSummary);
            this.Controls.Add(this.pnlFilters);
            this.Controls.Add(this.pnlHeader);
            this.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "frmListPayments";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "إدارة المقبوضات والمدفوعات";
            this.Load += new System.EventHandler(this.frmListPayments_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlFilters.ResumeLayout(false);
            this.gbFilter.ResumeLayout(false);
            this.gbFilter.PerformLayout();
            this.pnlSummary.ResumeLayout(false);
            this.tabPayments.ResumeLayout(false);
            this.tpCustomerPayments.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomerPayments)).EndInit();
            this.pnlCustomerTop.ResumeLayout(false);
            this.pnlCustomerTop.PerformLayout();
            this.tpSupplierPayments.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSupplierPayments)).EndInit();
            this.pnlSupplierTop.ResumeLayout(false);
            this.pnlSupplierTop.PerformLayout();
            this.cmsCustomerPayments.ResumeLayout(false);
            this.cmsSupplierPayments.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.Panel pnlFilters;
        private System.Windows.Forms.GroupBox gbFilter;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbFilterBy;
        private System.Windows.Forms.TextBox txtFilterValue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbStatus;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Button btnRangeToday;
        private System.Windows.Forms.Button btnRange30;
        private System.Windows.Forms.Button btnRangeMonth;
        private System.Windows.Forms.CheckBox chkOnlyCancelled;
        private System.Windows.Forms.Panel pnlSummary;
        private System.Windows.Forms.Label lblSummary;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TabControl tabPayments;
        private System.Windows.Forms.TabPage tpCustomerPayments;
        private System.Windows.Forms.TabPage tpSupplierPayments;
        private System.Windows.Forms.DataGridView dgvCustomerPayments;
        private System.Windows.Forms.Panel pnlCustomerTop;
        private System.Windows.Forms.Button btnAddCustomerPayment;
        private System.Windows.Forms.Label lblCustomerCount;
        private System.Windows.Forms.Label lblCustomerCountTitle;
        private System.Windows.Forms.DataGridView dgvSupplierPayments;
        private System.Windows.Forms.Panel pnlSupplierTop;
        private System.Windows.Forms.Button btnAddSupplierPayment;
        private System.Windows.Forms.Label lblSupplierCount;
        private System.Windows.Forms.Label lblSupplierCountTitle;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ContextMenuStrip cmsCustomerPayments;
        private System.Windows.Forms.ToolStripMenuItem tsmiCustomerRefresh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tsmiCustomerEdit;
        private System.Windows.Forms.ToolStripMenuItem tsmiCustomerAdd;
        private System.Windows.Forms.ContextMenuStrip cmsSupplierPayments;
        private System.Windows.Forms.ToolStripMenuItem tsmiSupplierRefresh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem tsmiSupplierEdit;
        private System.Windows.Forms.ToolStripMenuItem tsmiSupplierAdd;
    }
}
