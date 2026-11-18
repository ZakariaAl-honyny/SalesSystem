namespace SalesSystem.Desktop.Screens.CustomerReceipts.Forms;

partial class frmCustomerReceiptsMaster
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        cmsRows = new ContextMenuStrip(components);
        cmsEdit = new ToolStripMenuItem();
        cmsPost = new ToolStripMenuItem();
        cmsCancelDocument = new ToolStripMenuItem();
        cmsSeparator = new ToolStripSeparator();
        cmsRefresh = new ToolStripMenuItem();
        DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
        lblMode = new Label();
        pnlFields = new Panel();
        gbDocument = new GroupBox();
        lblReceiptNo = new Label();
        txtReceiptNo = new TextBox();
        lblDate = new Label();
        dtpDate = new DateTimePicker();
        lblCustomer = new Label();
        cmbCustomer = new ComboBox();
        lblCashBox = new Label();
        cmbCashBox = new ComboBox();
        gbAmount = new GroupBox();
        lblAmount = new Label();
        numAmount = new NumericUpDown();
        lblAmountValue = new Label();
        lblPaymentMethod = new Label();
        lblPaymentMethodValue = new Label();
        lblNotes = new Label();
        txtNotes = new TextBox();
        pnlButtons = new FlowLayoutPanel();
        btnNew = new Button();
        btnSave = new Button();
        btnPost = new Button();
        btnCancelDocument = new Button();
        btnRefresh = new Button();
        btnClose = new Button();
        pnlFilters = new Panel();
        gbFilter = new GroupBox();
        lblFilterBy = new Label();
        cbFilterBy = new ComboBox();
        txtFilterValue = new TextBox();
        lblStatus = new Label();
        cbStatus = new ComboBox();
        lblFrom = new Label();
        dtpFrom = new DateTimePicker();
        lblTo = new Label();
        dtpTo = new DateTimePicker();
        btnRangeToday = new Button();
        btnRangeMonth = new Button();
        btnRange30 = new Button();
        chkOnlyUnapplied = new CheckBox();
        btnClearFilters = new Button();
        btnToggleFilters = new Button();
        pnlGrid = new Panel();
        lblEmptyState = new Label();
        dgvMain = new DataGridView();
        colReceiptNo = new DataGridViewTextBoxColumn();
        colDate = new DataGridViewTextBoxColumn();
        colCustomer = new DataGridViewTextBoxColumn();
        colAmount = new DataGridViewTextBoxColumn();
        colPaymentMethod = new DataGridViewTextBoxColumn();
        colCashBox = new DataGridViewTextBoxColumn();
        colStatus = new DataGridViewTextBoxColumn();
        colNotes = new DataGridViewTextBoxColumn();
        pnlSummary = new Panel();
        lblSummary = new Label();
        errorProvider = new ErrorProvider(components);
        toolTip = new ToolTip(components);
        liveTip = new ToolTip(components);
        cmsRows.SuspendLayout();
        pnlFields.SuspendLayout();
        gbDocument.SuspendLayout();
        gbAmount.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numAmount).BeginInit();
        pnlButtons.SuspendLayout();
        pnlFilters.SuspendLayout();
        gbFilter.SuspendLayout();
        pnlGrid.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvMain).BeginInit();
        pnlSummary.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
        SuspendLayout();
        // 
        // lblMode
        // 
        lblMode.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
        lblMode.Dock = DockStyle.Top;
        lblMode.Font = new Font("Tahoma", 14F, FontStyle.Bold);
        lblMode.ForeColor = System.Drawing.Color.White;
        lblMode.Location = new Point(0, 0);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(1432, 50);
        lblMode.TabIndex = 0;
        lblMode.Text = "(( سندات القبض ))";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlFields
        // 
        pnlFields.Controls.Add(gbDocument);
        pnlFields.Controls.Add(gbAmount);
        pnlFields.Dock = DockStyle.Top;
        pnlFields.Location = new Point(0, 50);
        pnlFields.Name = "pnlFields";
        pnlFields.Size = new Size(1432, 175);
        pnlFields.TabIndex = 1;
        // 
        // gbDocument
        // 
        gbDocument.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(245), Convert.ToInt32(235));
        gbDocument.Controls.Add(lblReceiptNo);
        gbDocument.Controls.Add(txtReceiptNo);
        gbDocument.Controls.Add(lblDate);
        gbDocument.Controls.Add(dtpDate);
        gbDocument.Controls.Add(lblCustomer);
        gbDocument.Controls.Add(cmbCustomer);
        gbDocument.Controls.Add(lblCashBox);
        gbDocument.Controls.Add(cmbCashBox);
        gbDocument.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        gbDocument.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
        gbDocument.Location = new Point(720, 3);
        gbDocument.Name = "gbDocument";
        gbDocument.Size = new Size(700, 165);
        gbDocument.TabIndex = 0;
        gbDocument.TabStop = false;
        gbDocument.Text = "بيانات السند";
        // 
        // lblReceiptNo
        // 
        lblReceiptNo.AutoSize = true;
        lblReceiptNo.Location = new Point(560, 8);
        lblReceiptNo.Name = "lblReceiptNo";
        lblReceiptNo.Size = new Size(120, 24);
        lblReceiptNo.TabIndex = 0;
        lblReceiptNo.Text = "رقم السند:";
        // 
        // txtReceiptNo
        // 
        txtReceiptNo.BackColor = System.Drawing.Color.White;
        txtReceiptNo.Location = new Point(370, 34);
        txtReceiptNo.Name = "txtReceiptNo";
        txtReceiptNo.ReadOnly = true;
        txtReceiptNo.Size = new Size(310, 32);
        txtReceiptNo.TabIndex = 0;
        txtReceiptNo.TabStop = false;
        // 
        // lblDate
        // 
        lblDate.AutoSize = true;
        lblDate.Location = new Point(240, 8);
        lblDate.Name = "lblDate";
        lblDate.Size = new Size(110, 24);
        lblDate.TabIndex = 1;
        lblDate.Text = "التاريخ:";
        // 
        // dtpDate
        // 
        dtpDate.BackColor = System.Drawing.Color.White;
        dtpDate.Format = DateTimePickerFormat.Short;
        dtpDate.Location = new Point(20, 34);
        dtpDate.Name = "dtpDate";
        dtpDate.RightToLeft = RightToLeft.Yes;
        dtpDate.Size = new Size(330, 32);
        dtpDate.TabIndex = 1;
        // 
        // lblCustomer
        // 
        lblCustomer.AutoSize = true;
        lblCustomer.Location = new Point(560, 76);
        lblCustomer.Name = "lblCustomer";
        lblCustomer.Size = new Size(120, 24);
        lblCustomer.TabIndex = 2;
        lblCustomer.Text = "العميل: *";
        // 
        // cmbCustomer
        // 
        cmbCustomer.BackColor = System.Drawing.Color.White;
        cmbCustomer.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbCustomer.FlatStyle = FlatStyle.Flat;
        cmbCustomer.Location = new Point(370, 102);
        cmbCustomer.Name = "cmbCustomer";
        cmbCustomer.Size = new Size(310, 32);
        cmbCustomer.TabIndex = 2;
        // 
        // lblCashBox
        // 
        lblCashBox.AutoSize = true;
        lblCashBox.Location = new Point(240, 76);
        lblCashBox.Name = "lblCashBox";
        lblCashBox.Size = new Size(110, 24);
        lblCashBox.TabIndex = 3;
        lblCashBox.Text = "الصندوق: *";
        // 
        // cmbCashBox
        // 
        cmbCashBox.BackColor = System.Drawing.Color.White;
        cmbCashBox.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbCashBox.FlatStyle = FlatStyle.Flat;
        cmbCashBox.Location = new Point(20, 102);
        cmbCashBox.Name = "cmbCashBox";
        cmbCashBox.Size = new Size(330, 32);
        cmbCashBox.TabIndex = 3;
        // 
        // gbAmount
        // 
        gbAmount.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(245), Convert.ToInt32(235));
        gbAmount.Controls.Add(lblAmount);
        gbAmount.Controls.Add(numAmount);
        gbAmount.Controls.Add(lblAmountValue);
        gbAmount.Controls.Add(lblPaymentMethod);
        gbAmount.Controls.Add(lblPaymentMethodValue);
        gbAmount.Controls.Add(lblNotes);
        gbAmount.Controls.Add(txtNotes);
        gbAmount.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        gbAmount.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
        gbAmount.Location = new Point(10, 3);
        gbAmount.Name = "gbAmount";
        gbAmount.Size = new Size(700, 165);
        gbAmount.TabIndex = 1;
        gbAmount.TabStop = false;
        gbAmount.Text = "المبلغ والبيان";
        // 
        // lblAmount
        // 
        lblAmount.AutoSize = true;
        lblAmount.Location = new Point(560, 8);
        lblAmount.Name = "lblAmount";
        lblAmount.Size = new Size(120, 24);
        lblAmount.TabIndex = 4;
        lblAmount.Text = "المبلغ: *";
        // 
        // numAmount
        // 
        numAmount.BackColor = System.Drawing.Color.White;
        numAmount.DecimalPlaces = 2;
        numAmount.Location = new Point(370, 34);
        numAmount.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 });
        numAmount.Name = "numAmount";
        numAmount.RightToLeft = RightToLeft.Yes;
        numAmount.Size = new Size(310, 32);
        numAmount.TabIndex = 4;
        numAmount.TextAlign = HorizontalAlignment.Right;
        numAmount.ThousandsSeparator = true;
        numAmount.ValueChanged += numAmount_ValueChanged;
        // 
        // lblAmountValue
        // 
        lblAmountValue.Font = new Font("Tahoma", 12F, FontStyle.Bold);
        lblAmountValue.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
        lblAmountValue.Location = new Point(370, 74);
        lblAmountValue.Name = "lblAmountValue";
        lblAmountValue.Size = new Size(310, 34);
        lblAmountValue.TabIndex = 5;
        lblAmountValue.Text = "0.00";
        lblAmountValue.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblPaymentMethod
        // 
        lblPaymentMethod.AutoSize = true;
        lblPaymentMethod.Location = new Point(240, 8);
        lblPaymentMethod.Name = "lblPaymentMethod";
        lblPaymentMethod.Size = new Size(110, 24);
        lblPaymentMethod.TabIndex = 6;
        lblPaymentMethod.Text = "طريقة الدفع:";
        // 
        // lblPaymentMethodValue
        // 
        lblPaymentMethodValue.BackColor = System.Drawing.Color.White;
        lblPaymentMethodValue.BorderStyle = BorderStyle.FixedSingle;
        lblPaymentMethodValue.Font = new Font("Tahoma", 10F, FontStyle.Regular);
        lblPaymentMethodValue.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblPaymentMethodValue.Location = new Point(20, 34);
        lblPaymentMethodValue.Name = "lblPaymentMethodValue";
        lblPaymentMethodValue.Size = new Size(330, 32);
        lblPaymentMethodValue.TabIndex = 7;
        lblPaymentMethodValue.Text = "—";
        lblPaymentMethodValue.TextAlign = ContentAlignment.MiddleRight;
        toolTip.SetToolTip(lblPaymentMethodValue, "طريقة الدفع تُحدَّد من الخدمة ولا تُحرَّر من هذه الشاشة");
        // 
        // lblNotes
        // 
        lblNotes.AutoSize = true;
        lblNotes.Location = new Point(240, 74);
        lblNotes.Name = "lblNotes";
        lblNotes.Size = new Size(110, 24);
        lblNotes.TabIndex = 8;
        lblNotes.Text = "البيان:";
        // 
        // txtNotes
        // 
        txtNotes.BackColor = System.Drawing.Color.White;
        txtNotes.Location = new Point(20, 100);
        txtNotes.Multiline = true;
        txtNotes.Name = "txtNotes";
        txtNotes.RightToLeft = RightToLeft.Yes;
        txtNotes.ScrollBars = ScrollBars.Vertical;
        txtNotes.Size = new Size(330, 52);
        txtNotes.TabIndex = 5;
        // 
        // pnlButtons
        // 
        pnlButtons.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
        pnlButtons.Controls.Add(btnNew);
        pnlButtons.Controls.Add(btnSave);
        pnlButtons.Controls.Add(btnPost);
        pnlButtons.Controls.Add(btnCancelDocument);
        pnlButtons.Controls.Add(btnRefresh);
        pnlButtons.Controls.Add(btnClose);
        pnlButtons.Dock = DockStyle.Top;
        pnlButtons.Location = new Point(0, 225);
        pnlButtons.Name = "pnlButtons";
        pnlButtons.Padding = new Padding(10);
        pnlButtons.Size = new Size(1432, 60);
        pnlButtons.TabIndex = 2;
        // 
        // btnNew
        // 
        btnNew.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(13), Convert.ToInt32(110), Convert.ToInt32(253));
        btnNew.FlatStyle = FlatStyle.Flat;
        btnNew.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnNew.ForeColor = System.Drawing.Color.White;
        btnNew.Location = new Point(1267, 13);
        btnNew.Name = "btnNew";
        btnNew.Size = new Size(142, 35);
        btnNew.TabIndex = 0;
        btnNew.Text = "🆕 جديد";
        toolTip.SetToolTip(btnNew, "سند قبض جديد (F2)");
        btnNew.UseVisualStyleBackColor = false;
        btnNew.Click += btnNew_Click;
        // 
        // btnSave
        // 
        btnSave.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(25), Convert.ToInt32(135), Convert.ToInt32(84));
        btnSave.FlatStyle = FlatStyle.Flat;
        btnSave.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnSave.ForeColor = System.Drawing.Color.White;
        btnSave.Location = new Point(1129, 13);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(128, 35);
        btnSave.TabIndex = 1;
        btnSave.Text = "💾 حفظ";
        toolTip.SetToolTip(btnSave, "حفظ سند القبض (وضع الإضافة أو التعديل)");
        btnSave.UseVisualStyleBackColor = false;
        btnSave.Click += btnSave_Click;
        // 
        // btnPost
        // 
        btnPost.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(23), Convert.ToInt32(162), Convert.ToInt32(184));
        btnPost.Enabled = false;
        btnPost.FlatStyle = FlatStyle.Flat;
        btnPost.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnPost.ForeColor = System.Drawing.Color.White;
        btnPost.Location = new Point(991, 13);
        btnPost.Name = "btnPost";
        btnPost.Size = new Size(128, 35);
        btnPost.TabIndex = 2;
        btnPost.Text = "📤 ترحيل";
        toolTip.SetToolTip(btnPost, "ترحيل السند المسودة (لا يمكن ترحيل غير المسودة)");
        btnPost.UseVisualStyleBackColor = false;
        btnPost.Click += btnPost_Click;
        // 
        // btnCancelDocument
        // 
        btnCancelDocument.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(220), Convert.ToInt32(53), Convert.ToInt32(69));
        btnCancelDocument.Enabled = false;
        btnCancelDocument.FlatStyle = FlatStyle.Flat;
        btnCancelDocument.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnCancelDocument.ForeColor = System.Drawing.Color.White;
        btnCancelDocument.Location = new Point(796, 13);
        btnCancelDocument.Name = "btnCancelDocument";
        btnCancelDocument.Size = new Size(185, 35);
        btnCancelDocument.TabIndex = 3;
        btnCancelDocument.Text = "🚫 إلغاء المستند";
        toolTip.SetToolTip(btnCancelDocument, "إلغاء المستند (بديل الحذف — لا حذف على سند قبض)");
        btnCancelDocument.UseVisualStyleBackColor = false;
        btnCancelDocument.Click += btnCancelDocument_Click;
        // 
        // btnRefresh
        // 
        btnRefresh.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(23), Convert.ToInt32(162), Convert.ToInt32(184));
        btnRefresh.FlatStyle = FlatStyle.Flat;
        btnRefresh.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnRefresh.ForeColor = System.Drawing.Color.White;
        btnRefresh.Location = new Point(656, 13);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(130, 35);
        btnRefresh.TabIndex = 4;
        btnRefresh.Text = "🔄 تحديث";
        toolTip.SetToolTip(btnRefresh, "تحديث كل بيانات الشاشة (F5)");
        btnRefresh.UseVisualStyleBackColor = false;
        btnRefresh.Click += btnRefresh_Click;
        // 
        // btnClose
        // 
        btnClose.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(108), Convert.ToInt32(117), Convert.ToInt32(125));
        btnClose.FlatStyle = FlatStyle.Flat;
        btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnClose.ForeColor = System.Drawing.Color.White;
        btnClose.Location = new Point(516, 13);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(130, 35);
        btnClose.TabIndex = 5;
        btnClose.TabStop = false;
        btnClose.Text = "🚪 خروج";
        toolTip.SetToolTip(btnClose, "إغلاق الشاشة");
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += btnClose_Click;
        // 
        // pnlFilters
        // 
        pnlFilters.Controls.Add(gbFilter);
        pnlFilters.Controls.Add(btnToggleFilters);
        pnlFilters.Dock = DockStyle.Top;
        pnlFilters.Location = new Point(0, 285);
        pnlFilters.Margin = new Padding(0);
        pnlFilters.Name = "pnlFilters";
        pnlFilters.Padding = new Padding(6, 4, 6, 4);
        pnlFilters.Size = new Size(1432, 112);
        pnlFilters.TabIndex = 3;
        // 
        // gbFilter
        // 
        gbFilter.Controls.Add(lblFilterBy);
        gbFilter.Controls.Add(cbFilterBy);
        gbFilter.Controls.Add(txtFilterValue);
        gbFilter.Controls.Add(lblStatus);
        gbFilter.Controls.Add(cbStatus);
        gbFilter.Controls.Add(lblFrom);
        gbFilter.Controls.Add(dtpFrom);
        gbFilter.Controls.Add(lblTo);
        gbFilter.Controls.Add(dtpTo);
        gbFilter.Controls.Add(btnRangeToday);
        gbFilter.Controls.Add(btnRangeMonth);
        gbFilter.Controls.Add(btnRange30);
        gbFilter.Controls.Add(chkOnlyUnapplied);
        gbFilter.Controls.Add(btnClearFilters);
        gbFilter.Dock = DockStyle.Fill;
        gbFilter.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        gbFilter.Location = new Point(146, 4);
        gbFilter.Margin = new Padding(0);
        gbFilter.Name = "gbFilter";
        gbFilter.Padding = new Padding(10);
        gbFilter.RightToLeft = RightToLeft.Yes;
        gbFilter.Size = new Size(1280, 104);
        gbFilter.TabIndex = 0;
        gbFilter.TabStop = false;
        gbFilter.Text = "بحث وتصفية";
        // 
        // lblFilterBy
        // 
        lblFilterBy.AutoSize = true;
        lblFilterBy.Location = new Point(1130, 25);
        lblFilterBy.Name = "lblFilterBy";
        lblFilterBy.Size = new Size(90, 22);
        lblFilterBy.TabIndex = 0;
        lblFilterBy.Text = "تصفية بـ:";
        // 
        // cbFilterBy
        // 
        cbFilterBy.BackColor = System.Drawing.Color.White;
        cbFilterBy.DropDownStyle = ComboBoxStyle.DropDownList;
        cbFilterBy.FlatStyle = FlatStyle.Flat;
        cbFilterBy.Font = new Font("Tahoma", 9F, FontStyle.Regular);
        cbFilterBy.Items.AddRange(new object[] { "العميل", "رقم السند", "البيان", "المبلغ" });
        cbFilterBy.Location = new Point(975, 22);
        cbFilterBy.Name = "cbFilterBy";
        cbFilterBy.SelectedIndex = 0;
        cbFilterBy.Size = new Size(145, 30);
        cbFilterBy.TabIndex = 1;
        cbFilterBy.SelectedIndexChanged += cbFilterBy_SelectedIndexChanged;
        // 
        // txtFilterValue
        // 
        txtFilterValue.BackColor = System.Drawing.Color.White;
        txtFilterValue.Font = new Font("Tahoma", 9F, FontStyle.Regular);
        txtFilterValue.Location = new Point(740, 22);
        txtFilterValue.Name = "txtFilterValue";
        txtFilterValue.PlaceholderText = "اكتب قيمة التصفية...";
        txtFilterValue.RightToLeft = RightToLeft.Yes;
        txtFilterValue.Size = new Size(225, 30);
        txtFilterValue.TabIndex = 2;
        txtFilterValue.TextChanged += txtFilterValue_TextChanged;
        // 
        // lblStatus
        // 
        lblStatus.AutoSize = true;
        lblStatus.Location = new Point(660, 25);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(70, 22);
        lblStatus.TabIndex = 3;
        lblStatus.Text = "الحالة:";
        // 
        // cbStatus
        // 
        cbStatus.BackColor = System.Drawing.Color.White;
        cbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
        cbStatus.FlatStyle = FlatStyle.Flat;
        cbStatus.Font = new Font("Tahoma", 9F, FontStyle.Regular);
        cbStatus.Location = new Point(540, 22);
        cbStatus.Name = "cbStatus";
        cbStatus.Size = new Size(110, 30);
        cbStatus.TabIndex = 4;
        cbStatus.SelectedIndexChanged += cbStatus_SelectedIndexChanged;
        // 
        // lblFrom
        // 
        lblFrom.AutoSize = true;
        lblFrom.Location = new Point(470, 25);
        lblFrom.Name = "lblFrom";
        lblFrom.Size = new Size(60, 22);
        lblFrom.TabIndex = 5;
        lblFrom.Text = "من:";
        // 
        // dtpFrom
        // 
        dtpFrom.BackColor = System.Drawing.Color.White;
        dtpFrom.Font = new Font("Tahoma", 9F, FontStyle.Regular);
        dtpFrom.Format = DateTimePickerFormat.Short;
        dtpFrom.Location = new Point(330, 22);
        dtpFrom.Name = "dtpFrom";
        dtpFrom.RightToLeft = RightToLeft.Yes;
        dtpFrom.Size = new Size(130, 30);
        dtpFrom.TabIndex = 6;
        dtpFrom.ValueChanged += dtpFrom_ValueChanged;
        // 
        // lblTo
        // 
        lblTo.AutoSize = true;
        lblTo.Location = new Point(250, 25);
        lblTo.Name = "lblTo";
        lblTo.Size = new Size(60, 22);
        lblTo.TabIndex = 7;
        lblTo.Text = "إلى:";
        // 
        // dtpTo
        // 
        dtpTo.BackColor = System.Drawing.Color.White;
        dtpTo.Font = new Font("Tahoma", 9F, FontStyle.Regular);
        dtpTo.Format = DateTimePickerFormat.Short;
        dtpTo.Location = new Point(110, 22);
        dtpTo.Name = "dtpTo";
        dtpTo.RightToLeft = RightToLeft.Yes;
        dtpTo.Size = new Size(130, 30);
        dtpTo.TabIndex = 8;
        dtpTo.ValueChanged += dtpTo_ValueChanged;
        // 
        // btnRangeToday
        // 
        btnRangeToday.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(248), Convert.ToInt32(249), Convert.ToInt32(250));
        btnRangeToday.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(Convert.ToInt32(205), Convert.ToInt32(225), Convert.ToInt32(205));
        btnRangeToday.FlatStyle = FlatStyle.Flat;
        btnRangeToday.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        btnRangeToday.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
        btnRangeToday.Location = new Point(1140, 62);
        btnRangeToday.Name = "btnRangeToday";
        btnRangeToday.Size = new Size(100, 30);
        btnRangeToday.TabIndex = 9;
        btnRangeToday.Text = "اليوم";
        toolTip.SetToolTip(btnRangeToday, "سندات اليوم فقط");
        btnRangeToday.UseVisualStyleBackColor = false;
        btnRangeToday.Click += btnRangeToday_Click;
        // 
        // btnRangeMonth
        // 
        btnRangeMonth.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(248), Convert.ToInt32(249), Convert.ToInt32(250));
        btnRangeMonth.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(Convert.ToInt32(205), Convert.ToInt32(225), Convert.ToInt32(205));
        btnRangeMonth.FlatStyle = FlatStyle.Flat;
        btnRangeMonth.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        btnRangeMonth.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
        btnRangeMonth.Location = new Point(1025, 62);
        btnRangeMonth.Name = "btnRangeMonth";
        btnRangeMonth.Size = new Size(105, 30);
        btnRangeMonth.TabIndex = 10;
        btnRangeMonth.Text = "هذا الشهر";
        toolTip.SetToolTip(btnRangeMonth, "من بداية الشهر الحالي حتى اليوم");
        btnRangeMonth.UseVisualStyleBackColor = false;
        btnRangeMonth.Click += btnRangeMonth_Click;
        // 
        // btnRange30
        // 
        btnRange30.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(248), Convert.ToInt32(249), Convert.ToInt32(250));
        btnRange30.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(Convert.ToInt32(205), Convert.ToInt32(225), Convert.ToInt32(205));
        btnRange30.FlatStyle = FlatStyle.Flat;
        btnRange30.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        btnRange30.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
        btnRange30.Location = new Point(910, 62);
        btnRange30.Name = "btnRange30";
        btnRange30.Size = new Size(105, 30);
        btnRange30.TabIndex = 11;
        btnRange30.Text = "آخر 30 يوماً";
        toolTip.SetToolTip(btnRange30, "آخر 30 يوماً");
        btnRange30.UseVisualStyleBackColor = false;
        btnRange30.Click += btnRange30_Click;
        // 
        // chkOnlyUnapplied
        // 
        chkOnlyUnapplied.AutoSize = true;
        chkOnlyUnapplied.FlatStyle = FlatStyle.Flat;
        chkOnlyUnapplied.Font = new Font("Tahoma", 9F, FontStyle.Regular);
        chkOnlyUnapplied.Location = new Point(680, 65);
        chkOnlyUnapplied.Name = "chkOnlyUnapplied";
        chkOnlyUnapplied.Size = new Size(220, 26);
        chkOnlyUnapplied.TabIndex = 12;
        chkOnlyUnapplied.Text = "غير المخصصة فقط";
        toolTip.SetToolTip(chkOnlyUnapplied, "السندات التي لم يُخصَّص كامل مبلغها بعد");
        chkOnlyUnapplied.UseVisualStyleBackColor = false;
        chkOnlyUnapplied.CheckedChanged += chkOnlyUnapplied_CheckedChanged;
        // 
        // btnClearFilters
        // 
        btnClearFilters.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(108), Convert.ToInt32(117), Convert.ToInt32(125));
        btnClearFilters.Cursor = Cursors.Hand;
        btnClearFilters.Enabled = false;
        btnClearFilters.FlatAppearance.BorderSize = 0;
        btnClearFilters.FlatStyle = FlatStyle.Flat;
        btnClearFilters.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        btnClearFilters.ForeColor = System.Drawing.Color.White;
        btnClearFilters.Location = new Point(480, 61);
        btnClearFilters.Name = "btnClearFilters";
        btnClearFilters.Size = new Size(178, 32);
        btnClearFilters.TabIndex = 13;
        btnClearFilters.Text = "\U0001f9f9 مسح التصفية";
        toolTip.SetToolTip(btnClearFilters, "مسح كل أدوات التصفية (Esc) — لا يعيد تحميل البيانات");
        btnClearFilters.UseVisualStyleBackColor = false;
        btnClearFilters.Click += btnClearFilters_Click;
        // 
        // btnToggleFilters
        // 
        btnToggleFilters.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(248), Convert.ToInt32(249), Convert.ToInt32(250));
        btnToggleFilters.Cursor = Cursors.Hand;
        btnToggleFilters.Dock = DockStyle.Left;
        btnToggleFilters.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(Convert.ToInt32(205), Convert.ToInt32(225), Convert.ToInt32(205));
        btnToggleFilters.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(240), Convert.ToInt32(240), Convert.ToInt32(240));
        btnToggleFilters.FlatStyle = FlatStyle.Popup;
        btnToggleFilters.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        btnToggleFilters.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
        btnToggleFilters.Location = new Point(6, 4);
        btnToggleFilters.Name = "btnToggleFilters";
        btnToggleFilters.Size = new Size(140, 104);
        btnToggleFilters.TabIndex = 14;
        btnToggleFilters.Text = "إخفاء التصفية";
        btnToggleFilters.UseVisualStyleBackColor = false;
        // 
        // pnlGrid
        // 
        pnlGrid.Controls.Add(lblEmptyState);
        pnlGrid.Controls.Add(dgvMain);
        pnlGrid.Dock = DockStyle.Fill;
        pnlGrid.Location = new Point(0, 397);
        pnlGrid.Name = "pnlGrid";
        pnlGrid.Padding = new Padding(0, 10, 0, 0);
        pnlGrid.Size = new Size(1432, 371);
        pnlGrid.TabIndex = 4;
        // 
        // lblEmptyState
        // 
        lblEmptyState.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(220), Convert.ToInt32(240), Convert.ToInt32(220));
        lblEmptyState.Dock = DockStyle.Fill;
        lblEmptyState.Font = new Font("Tahoma", 12F, FontStyle.Bold);
        lblEmptyState.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblEmptyState.Location = new Point(0, 10);
        lblEmptyState.Name = "lblEmptyState";
        lblEmptyState.Size = new Size(1432, 361);
        lblEmptyState.TabIndex = 1;
        lblEmptyState.Text = "لا توجد سندات قبض مطابقة للمعايير الحالية";
        lblEmptyState.TextAlign = ContentAlignment.MiddleCenter;
        lblEmptyState.Visible = false;
        // 
        // dgvMain
        // 
        dgvMain.AllowUserToAddRows = false;
        dgvMain.AllowUserToDeleteRows = false;
        dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(245), Convert.ToInt32(252), Convert.ToInt32(245));
        dgvMain.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
        dgvMain.AutoGenerateColumns = false;
        dgvMain.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvMain.BackgroundColor = System.Drawing.Color.FromArgb(Convert.ToInt32(220), Convert.ToInt32(240), Convert.ToInt32(220));
        dgvMain.BorderStyle = BorderStyle.None;
        dgvMain.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
        dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
        dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
        dataGridViewCellStyle2.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
        dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
        dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
        dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
        dgvMain.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
        dgvMain.ColumnHeadersHeight = 45;
        dgvMain.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        dgvMain.Columns.AddRange(new DataGridViewColumn[] { colReceiptNo, colDate, colCustomer, colAmount, colPaymentMethod, colCashBox, colStatus, colNotes });
        dgvMain.ContextMenuStrip = cmsRows;
        dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
        dataGridViewCellStyle3.Font = new Font("Tahoma", 10F);
        dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(255), Convert.ToInt32(255), Convert.ToInt32(180));
        dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
        dgvMain.DefaultCellStyle = dataGridViewCellStyle3;
        dgvMain.Dock = DockStyle.Fill;
        dgvMain.EnableHeadersVisualStyles = false;
        dgvMain.GridColor = System.Drawing.Color.FromArgb(Convert.ToInt32(205), Convert.ToInt32(225), Convert.ToInt32(205));
        dgvMain.Location = new Point(0, 10);
        dgvMain.MultiSelect = false;
        dgvMain.Name = "dgvMain";
        dgvMain.ReadOnly = true;
        dgvMain.RowHeadersVisible = false;
        dgvMain.RowHeadersWidth = 62;
        dgvMain.RowTemplate.Height = 32;
        dgvMain.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvMain.Size = new Size(1432, 361);
        dgvMain.TabIndex = 0;
        dgvMain.CellClick += dgvMain_CellClick;
        dgvMain.CellDoubleClick += dgvMain_CellDoubleClick;
        // 
        // colReceiptNo
        // 
        colReceiptNo.DataPropertyName = "ReceiptNo";
        colReceiptNo.FillWeight = 8F;
        colReceiptNo.HeaderText = "رقم السند";
        colReceiptNo.MinimumWidth = 90;
        colReceiptNo.Name = "colReceiptNo";
        colReceiptNo.ReadOnly = true;
        // 
        // colDate
        // 
        colDate.DataPropertyName = "ReceiptDate";
        colDate.FillWeight = 12F;
        colDate.HeaderText = "التاريخ";
        colDate.MinimumWidth = 110;
        colDate.Name = "colDate";
        colDate.ReadOnly = true;
        // 
        // colCustomer
        // 
        colCustomer.DataPropertyName = "CustomerName";
        colCustomer.FillWeight = 16F;
        colCustomer.HeaderText = "العميل";
        colCustomer.MinimumWidth = 150;
        colCustomer.Name = "colCustomer";
        colCustomer.ReadOnly = true;
        // 
        // colAmount
        // 
        colAmount.DataPropertyName = "Amount";
        colAmount.FillWeight = 10F;
        colAmount.HeaderText = "المبلغ";
        colAmount.MinimumWidth = 100;
        colAmount.Name = "colAmount";
        colAmount.ReadOnly = true;
        // 
        // colPaymentMethod
        // 
        colPaymentMethod.DataPropertyName = "PaymentMethodName";
        colPaymentMethod.FillWeight = 12F;
        colPaymentMethod.HeaderText = "طريقة الدفع";
        colPaymentMethod.MinimumWidth = 120;
        colPaymentMethod.Name = "colPaymentMethod";
        colPaymentMethod.ReadOnly = true;
        // 
        // colCashBox
        // 
        colCashBox.DataPropertyName = "CashBoxName";
        colCashBox.FillWeight = 12F;
        colCashBox.HeaderText = "الصندوق";
        colCashBox.MinimumWidth = 120;
        colCashBox.Name = "colCashBox";
        colCashBox.ReadOnly = true;
        // 
        // colStatus
        // 
        colStatus.DataPropertyName = "StatusName";
        colStatus.FillWeight = 10F;
        colStatus.HeaderText = "الحالة";
        colStatus.MinimumWidth = 100;
        colStatus.Name = "colStatus";
        colStatus.ReadOnly = true;
        // 
        // colNotes
        // 
        colNotes.DataPropertyName = "Notes";
        colNotes.FillWeight = 20F;
        colNotes.HeaderText = "البيان";
        colNotes.MinimumWidth = 160;
        colNotes.Name = "colNotes";
        colNotes.ReadOnly = true;
        // 
        // pnlSummary
        // 
        pnlSummary.Controls.Add(lblSummary);
        pnlSummary.Dock = DockStyle.Bottom;
        pnlSummary.Location = new Point(0, 768);
        pnlSummary.Margin = new Padding(0);
        pnlSummary.Name = "pnlSummary";
        pnlSummary.Size = new Size(1432, 32);
        pnlSummary.TabIndex = 5;
        // 
        // lblSummary
        // 
        lblSummary.Dock = DockStyle.Fill;
        lblSummary.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblSummary.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
        lblSummary.Location = new Point(0, 0);
        lblSummary.Name = "lblSummary";
        lblSummary.Size = new Size(1432, 32);
        lblSummary.TabIndex = 0;
        lblSummary.Text = "المعروض: 0 من 0";
        lblSummary.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // cmsRows
        // 
        // RULE-N23: قائمة سياق على الشبكة — كل عنصر يستدعي نفس زر الشاشة (بلا منطق مكرر).
        cmsRows.Items.AddRange(new ToolStripItem[] { cmsEdit, cmsPost, cmsCancelDocument, cmsSeparator, cmsRefresh });
        cmsRows.Name = "cmsRows";
        cmsRows.Size = new Size(240, 134);
        cmsRows.Opening += cmsRows_Opening;
        // 
        // cmsEdit
        // 
        cmsEdit.Name = "cmsEdit";
        cmsEdit.Size = new Size(239, 30);
        cmsEdit.Text = "✏️ عرض / تحرير";
        cmsEdit.Click += cmsEdit_Click;
        // 
        // cmsPost
        // 
        cmsPost.Name = "cmsPost";
        cmsPost.Size = new Size(239, 30);
        cmsPost.Text = "📤 ترحيل";
        cmsPost.Click += cmsPost_Click;
        // 
        // cmsCancelDocument
        // 
        cmsCancelDocument.Name = "cmsCancelDocument";
        cmsCancelDocument.Size = new Size(239, 30);
        cmsCancelDocument.Text = "🚫 إلغاء المستند";
        cmsCancelDocument.Click += cmsCancelDocument_Click;
        // 
        // cmsSeparator
        // 
        cmsSeparator.Name = "cmsSeparator";
        cmsSeparator.Size = new Size(236, 6);
        // 
        // cmsRefresh
        // 
        cmsRefresh.Name = "cmsRefresh";
        cmsRefresh.Size = new Size(239, 30);
        cmsRefresh.Text = "🔄 تحديث (F5)";
        cmsRefresh.Click += cmsRefresh_Click;
        // 
        // errorProvider
        // 
        errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        errorProvider.ContainerControl = this;
        errorProvider.RightToLeft = true;
        // 
        // frmCustomerReceiptsMaster
        // 
        AutoScaleDimensions = new SizeF(11F, 24F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(170), Convert.ToInt32(220), Convert.ToInt32(170));
        ClientSize = new Size(1432, 800);
        Controls.Add(pnlGrid);
        Controls.Add(pnlSummary);
        Controls.Add(pnlFilters);
        Controls.Add(pnlButtons);
        Controls.Add(pnlFields);
        Controls.Add(lblMode);
        Font = new Font("Tahoma", 10F);
        Name = "frmCustomerReceiptsMaster";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterParent;
        Text = "سندات القبض";
        Load += frmCustomerReceiptsMaster_Load;
        cmsRows.ResumeLayout(false);
        pnlFields.ResumeLayout(false);
        gbDocument.ResumeLayout(false);
        gbDocument.PerformLayout();
        gbAmount.ResumeLayout(false);
        gbAmount.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numAmount).EndInit();
        pnlButtons.ResumeLayout(false);
        pnlFilters.ResumeLayout(false);
        gbFilter.ResumeLayout(false);
        gbFilter.PerformLayout();
        pnlGrid.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvMain).EndInit();
        pnlSummary.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        ResumeLayout(false);
    }

    private System.Windows.Forms.Label lblMode;
    private System.Windows.Forms.Panel pnlFields;
    private System.Windows.Forms.GroupBox gbDocument;
    private System.Windows.Forms.Label lblReceiptNo;
    private System.Windows.Forms.TextBox txtReceiptNo;
    private System.Windows.Forms.Label lblDate;
    private System.Windows.Forms.DateTimePicker dtpDate;
    private System.Windows.Forms.Label lblCustomer;
    private System.Windows.Forms.ComboBox cmbCustomer;
    private System.Windows.Forms.Label lblCashBox;
    private System.Windows.Forms.ComboBox cmbCashBox;
    private System.Windows.Forms.GroupBox gbAmount;
    private System.Windows.Forms.Label lblAmount;
    private System.Windows.Forms.NumericUpDown numAmount;
    private System.Windows.Forms.Label lblAmountValue;
    private System.Windows.Forms.Label lblPaymentMethod;
    private System.Windows.Forms.Label lblPaymentMethodValue;
    private System.Windows.Forms.Label lblNotes;
    private System.Windows.Forms.TextBox txtNotes;
    private System.Windows.Forms.FlowLayoutPanel pnlButtons;
    private System.Windows.Forms.Button btnNew;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.Button btnPost;
    private System.Windows.Forms.Button btnCancelDocument;
    private System.Windows.Forms.Button btnRefresh;
    private System.Windows.Forms.Button btnClose;
    private System.Windows.Forms.Panel pnlFilters;
    private System.Windows.Forms.GroupBox gbFilter;
    private System.Windows.Forms.Label lblFilterBy;
    private System.Windows.Forms.ComboBox cbFilterBy;
    private System.Windows.Forms.TextBox txtFilterValue;
    private System.Windows.Forms.Label lblStatus;
    private System.Windows.Forms.ComboBox cbStatus;
    private System.Windows.Forms.Label lblFrom;
    private System.Windows.Forms.DateTimePicker dtpFrom;
    private System.Windows.Forms.Label lblTo;
    private System.Windows.Forms.DateTimePicker dtpTo;
    private System.Windows.Forms.Button btnRangeToday;
    private System.Windows.Forms.Button btnRangeMonth;
    private System.Windows.Forms.Button btnRange30;
    private System.Windows.Forms.CheckBox chkOnlyUnapplied;
    private System.Windows.Forms.Button btnClearFilters;
    private System.Windows.Forms.Button btnToggleFilters;
    private System.Windows.Forms.Panel pnlGrid;
    private System.Windows.Forms.Label lblEmptyState;
    private System.Windows.Forms.DataGridView dgvMain;
    private System.Windows.Forms.DataGridViewTextBoxColumn colReceiptNo;
    private System.Windows.Forms.DataGridViewTextBoxColumn colDate;
    private System.Windows.Forms.DataGridViewTextBoxColumn colCustomer;
    private System.Windows.Forms.DataGridViewTextBoxColumn colAmount;
    private System.Windows.Forms.DataGridViewTextBoxColumn colPaymentMethod;
    private System.Windows.Forms.DataGridViewTextBoxColumn colCashBox;
    private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
    private System.Windows.Forms.DataGridViewTextBoxColumn colNotes;
    private System.Windows.Forms.Panel pnlSummary;
    private System.Windows.Forms.Label lblSummary;
    private System.Windows.Forms.ContextMenuStrip cmsRows;
    private System.Windows.Forms.ToolStripMenuItem cmsEdit;
    private System.Windows.Forms.ToolStripMenuItem cmsPost;
    private System.Windows.Forms.ToolStripMenuItem cmsCancelDocument;
    private System.Windows.Forms.ToolStripSeparator cmsSeparator;
    private System.Windows.Forms.ToolStripMenuItem cmsRefresh;
    private System.Windows.Forms.ErrorProvider errorProvider;
    private System.Windows.Forms.ToolTip toolTip;
    private System.Windows.Forms.ToolTip liveTip;
}
