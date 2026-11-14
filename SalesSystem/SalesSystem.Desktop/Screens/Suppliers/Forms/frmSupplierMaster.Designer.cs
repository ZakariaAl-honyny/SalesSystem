namespace SalesSystem.Desktop.Suppliers.Forms;

partial class frmSupplierMaster
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
        lblMode = new Label();
        pnlFields = new Panel();
        gbEditor = new GroupBox();
        lblId = new Label();
        txtId = new TextBox();
        lblName = new Label();
        lblNameReq = new Label();
        txtName = new TextBox();
        lblPhone = new Label();
        txtPhone = new TextBox();
        lblTaxNumber = new Label();
        txtTaxNumber = new TextBox();
        lblEmail = new Label();
        txtEmail = new TextBox();
        lblCreditLimit = new Label();
        numCreditLimit = new NumericUpDown();
        lblAddress = new Label();
        txtAddress = new TextBox();
        lblNotes = new Label();
        txtNotes = new TextBox();
        gbToolbar = new GroupBox();
        pnlButtons = new FlowLayoutPanel();
        btnNew = new Button();
        btnSave = new Button();
        btnEdit = new Button();
        btnDelete = new Button();
        btnReactivate = new Button();
        btnRefresh = new Button();
        btnClose = new Button();
        pnlFilters = new Panel();
        gbFilter = new GroupBox();
        label2 = new Label();
        cbFilterBy = new ComboBox();
        txtSearch = new TextBox();
        label1 = new Label();
        cbStatus = new ComboBox();
        chkShowInactive = new CheckBox();
        chkOnlyWithBalance = new CheckBox();
        chkOnlyOverLimit = new CheckBox();
        btnClearFilters = new Button();
        btnToggleFilters = new Button();
        pnlSummary = new Panel();
        gbSummary = new GroupBox();
        lblSummary = new Label();
        pnlGrid = new Panel();
        gbGrid = new GroupBox();
        lblEmptyState = new Label();
        dgvSuppliers = new DataGridView();
        cmsGrid = new ContextMenuStrip(components);
        cmsEdit = new ToolStripMenuItem();
        cmsDelete = new ToolStripMenuItem();
        cmsReactivate = new ToolStripMenuItem();
        cmsSeparator = new ToolStripSeparator();
        cmsRefresh = new ToolStripMenuItem();
        cmsExport = new ToolStripMenuItem();
        errorProvider = new ErrorProvider(components);
        liveTip = new ToolTip(components);
        toolTip = new ToolTip(components);
        pnlFields.SuspendLayout();
        gbEditor.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numCreditLimit).BeginInit();
        gbToolbar.SuspendLayout();
        pnlButtons.SuspendLayout();
        pnlFilters.SuspendLayout();
        gbFilter.SuspendLayout();
        pnlSummary.SuspendLayout();
        gbSummary.SuspendLayout();
        pnlGrid.SuspendLayout();
        gbGrid.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvSuppliers).BeginInit();
        cmsGrid.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
        SuspendLayout();
        // 
        // lblMode
        // 
        // RULE-N134: نفس قيم ثيم GreenThemeHelper حرفياً — ليتساوى المصمّم ووقت التشغيل.
        lblMode.BackColor = Color.FromArgb(14, 165, 103);
        lblMode.Dock = DockStyle.Top;
        lblMode.Font = new Font("Tahoma", 14F, FontStyle.Bold);
        lblMode.ForeColor = Color.White;
        lblMode.Location = new Point(0, 0);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(1432, 50);
        lblMode.TabIndex = 0;
        lblMode.Text = "(( دليل الموردين ))";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlFields
        // 
        pnlFields.BackColor = Color.FromArgb(235, 245, 235);
        pnlFields.Controls.Add(gbEditor);
        pnlFields.Dock = DockStyle.Top;
        pnlFields.Location = new Point(0, 50);
        pnlFields.Name = "pnlFields";
        pnlFields.Size = new Size(1432, 244);
        pnlFields.TabIndex = 1;
        // 
        // gbEditor
        // 
        // RULE-N136: كتلة المحرر داخل صندوق مجموعة بعنوان عربي.
        gbEditor.BackColor = Color.FromArgb(235, 245, 235);
        gbEditor.Controls.Add(lblId);
        gbEditor.Controls.Add(txtId);
        gbEditor.Controls.Add(lblName);
        gbEditor.Controls.Add(lblNameReq);
        gbEditor.Controls.Add(txtName);
        gbEditor.Controls.Add(lblPhone);
        gbEditor.Controls.Add(txtPhone);
        gbEditor.Controls.Add(lblTaxNumber);
        gbEditor.Controls.Add(txtTaxNumber);
        gbEditor.Controls.Add(lblEmail);
        gbEditor.Controls.Add(txtEmail);
        gbEditor.Controls.Add(lblCreditLimit);
        gbEditor.Controls.Add(numCreditLimit);
        gbEditor.Controls.Add(lblAddress);
        gbEditor.Controls.Add(txtAddress);
        gbEditor.Controls.Add(lblNotes);
        gbEditor.Controls.Add(txtNotes);
        gbEditor.Dock = DockStyle.Fill;
        gbEditor.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        gbEditor.ForeColor = Color.FromArgb(14, 82, 52);
        gbEditor.Location = new Point(0, 0);
        gbEditor.Name = "gbEditor";
        gbEditor.Size = new Size(1432, 244);
        gbEditor.TabIndex = 0;
        gbEditor.TabStop = false;
        gbEditor.Text = "بيانات المورد";
        // 
        // lblId
        // 
        lblId.AutoSize = true;
        lblId.Location = new Point(1140, 30);
        lblId.Name = "lblId";
        lblId.Size = new Size(93, 24);
        lblId.TabIndex = 0;
        lblId.Text = "رقم المورد";
        // 
        // txtId
        // 
        // RULE-N143: حقول الإدخال بيضاء في المصمّم ووقت التشغيل.
        txtId.BackColor = Color.White;
        txtId.Font = new Font("Tahoma", 10F);
        txtId.Location = new Point(1140, 56);
        txtId.Name = "txtId";
        txtId.ReadOnly = true;
        txtId.Size = new Size(250, 32);
        txtId.TabIndex = 0;
        txtId.TabStop = false;
        // 
        // lblName
        // 
        lblName.AutoSize = true;
        lblName.Location = new Point(860, 30);
        lblName.Name = "lblName";
        lblName.Size = new Size(103, 24);
        lblName.TabIndex = 2;
        lblName.Text = "اسم المورد";
        // 
        // lblNameReq
        // 
        lblNameReq.AutoSize = true;
        lblNameReq.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblNameReq.ForeColor = Color.Red;
        lblNameReq.Location = new Point(842, 30);
        lblNameReq.Name = "lblNameReq";
        lblNameReq.Size = new Size(23, 24);
        lblNameReq.TabIndex = 3;
        lblNameReq.Text = "*";
        // 
        // txtName
        // 
        txtName.BackColor = Color.White;
        txtName.Font = new Font("Tahoma", 10F);
        txtName.Location = new Point(860, 56);
        txtName.Name = "txtName";
        txtName.Size = new Size(250, 32);
        txtName.TabIndex = 1;
        // 
        // lblPhone
        // 
        lblPhone.AutoSize = true;
        lblPhone.Location = new Point(580, 30);
        lblPhone.Name = "lblPhone";
        lblPhone.Size = new Size(62, 24);
        lblPhone.TabIndex = 4;
        lblPhone.Text = "الهاتف";
        // 
        // txtPhone
        // 
        txtPhone.BackColor = Color.White;
        txtPhone.Font = new Font("Tahoma", 10F);
        txtPhone.Location = new Point(580, 56);
        txtPhone.Name = "txtPhone";
        txtPhone.Size = new Size(250, 32);
        txtPhone.TabIndex = 2;
        // 
        // lblTaxNumber
        // 
        lblTaxNumber.AutoSize = true;
        lblTaxNumber.Location = new Point(300, 30);
        lblTaxNumber.Name = "lblTaxNumber";
        lblTaxNumber.Size = new Size(124, 24);
        lblTaxNumber.TabIndex = 6;
        lblTaxNumber.Text = "الرقم الضريبي";
        // 
        // txtTaxNumber
        // 
        txtTaxNumber.BackColor = Color.White;
        txtTaxNumber.Font = new Font("Tahoma", 10F);
        txtTaxNumber.Location = new Point(300, 56);
        txtTaxNumber.Name = "txtTaxNumber";
        txtTaxNumber.Size = new Size(250, 32);
        txtTaxNumber.TabIndex = 3;
        // 
        // lblEmail
        // 
        lblEmail.AutoSize = true;
        lblEmail.Location = new Point(1140, 100);
        lblEmail.Name = "lblEmail";
        lblEmail.Size = new Size(53, 24);
        lblEmail.TabIndex = 8;
        lblEmail.Text = "البريد";
        // 
        // txtEmail
        // 
        txtEmail.BackColor = Color.White;
        txtEmail.Font = new Font("Tahoma", 10F);
        txtEmail.Location = new Point(1140, 126);
        txtEmail.Name = "txtEmail";
        txtEmail.Size = new Size(250, 32);
        txtEmail.TabIndex = 4;
        // 
        // lblCreditLimit
        // 
        lblCreditLimit.AutoSize = true;
        lblCreditLimit.Location = new Point(860, 100);
        lblCreditLimit.Name = "lblCreditLimit";
        lblCreditLimit.Size = new Size(101, 24);
        lblCreditLimit.TabIndex = 10;
        lblCreditLimit.Text = "حد الائتمان";
        // 
        // numCreditLimit
        // 
        numCreditLimit.BackColor = Color.White;
        numCreditLimit.Font = new Font("Tahoma", 10F);
        numCreditLimit.DecimalPlaces = 2;
        numCreditLimit.Location = new Point(860, 126);
        numCreditLimit.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 });
        numCreditLimit.Name = "numCreditLimit";
        numCreditLimit.Size = new Size(250, 32);
        numCreditLimit.TabIndex = 5;
        numCreditLimit.ThousandsSeparator = true;
        // 
        // lblAddress
        // 
        lblAddress.AutoSize = true;
        lblAddress.Location = new Point(300, 100);
        lblAddress.Name = "lblAddress";
        lblAddress.Size = new Size(65, 24);
        lblAddress.TabIndex = 12;
        lblAddress.Text = "العنوان";
        // 
        // txtAddress
        // 
        txtAddress.BackColor = Color.White;
        txtAddress.Font = new Font("Tahoma", 10F);
        txtAddress.Location = new Point(300, 126);
        txtAddress.Name = "txtAddress";
        txtAddress.Size = new Size(530, 32);
        txtAddress.TabIndex = 6;
        // 
        // lblNotes
        // 
        lblNotes.AutoSize = true;
        lblNotes.Location = new Point(300, 170);
        lblNotes.Name = "lblNotes";
        lblNotes.Size = new Size(83, 24);
        lblNotes.TabIndex = 14;
        lblNotes.Text = "ملاحظات";
        // 
        // txtNotes
        // 
        txtNotes.BackColor = Color.White;
        txtNotes.Font = new Font("Tahoma", 10F);
        txtNotes.Location = new Point(300, 196);
        txtNotes.Multiline = true;
        txtNotes.Name = "txtNotes";
        txtNotes.ScrollBars = ScrollBars.Vertical;
        txtNotes.Size = new Size(1090, 34);
        txtNotes.TabIndex = 7;
        // 
        // gbToolbar
        // 
        // RULE-N136: كتلة أوامر الشاشة داخل صندوق مجموعة.
        gbToolbar.BackColor = Color.FromArgb(235, 240, 235);
        gbToolbar.Controls.Add(pnlButtons);
        gbToolbar.Dock = DockStyle.Top;
        gbToolbar.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        gbToolbar.ForeColor = Color.FromArgb(14, 82, 52);
        gbToolbar.Location = new Point(0, 294);
        gbToolbar.Name = "gbToolbar";
        gbToolbar.Size = new Size(1432, 78);
        gbToolbar.TabIndex = 2;
        gbToolbar.TabStop = false;
        gbToolbar.Text = "أوامر العملية";
        // 
        // pnlButtons
        // 
        pnlButtons.BackColor = Color.FromArgb(235, 240, 235);
        pnlButtons.Controls.Add(btnNew);
        pnlButtons.Controls.Add(btnSave);
        pnlButtons.Controls.Add(btnEdit);
        pnlButtons.Controls.Add(btnDelete);
        pnlButtons.Controls.Add(btnReactivate);
        pnlButtons.Controls.Add(btnRefresh);
        pnlButtons.Controls.Add(btnClose);
        pnlButtons.Dock = DockStyle.Fill;
        pnlButtons.Location = new Point(3, 23);
        pnlButtons.Name = "pnlButtons";
        pnlButtons.Padding = new Padding(10);
        pnlButtons.Size = new Size(1426, 52);
        pnlButtons.TabIndex = 0;
        // 
        // btnNew
        // 
        btnNew.BackColor = Color.FromArgb(40, 167, 69);
        btnNew.FlatStyle = FlatStyle.Flat;
        btnNew.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnNew.ForeColor = Color.White;
        btnNew.Location = new Point(1267, 13);
        btnNew.Name = "btnNew";
        btnNew.Size = new Size(142, 35);
        btnNew.TabIndex = 16;
        btnNew.Text = "🆕 جديد";
        btnNew.UseVisualStyleBackColor = false;
        btnNew.Click += btnNew_Click;
        // 
        // btnSave
        // 
        btnSave.BackColor = Color.FromArgb(0, 123, 255);
        btnSave.FlatStyle = FlatStyle.Flat;
        btnSave.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnSave.ForeColor = Color.White;
        btnSave.Location = new Point(1133, 13);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(128, 35);
        btnSave.TabIndex = 17;
        btnSave.Text = "💾 إضافة";
        btnSave.UseVisualStyleBackColor = false;
        btnSave.Click += btnSave_Click;
        // 
        // btnEdit
        // 
        btnEdit.BackColor = Color.FromArgb(255, 152, 0);
        btnEdit.FlatStyle = FlatStyle.Flat;
        btnEdit.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnEdit.ForeColor = Color.White;
        btnEdit.Location = new Point(999, 13);
        btnEdit.Name = "btnEdit";
        btnEdit.Size = new Size(128, 35);
        btnEdit.TabIndex = 19;
        btnEdit.Text = "✏️ تعديل";
        btnEdit.UseVisualStyleBackColor = false;
        btnEdit.Click += btnEdit_Click;
        // 
        // btnDelete
        // 
        btnDelete.BackColor = Color.FromArgb(220, 53, 69);
        btnDelete.FlatStyle = FlatStyle.Flat;
        btnDelete.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnDelete.ForeColor = Color.White;
        btnDelete.Location = new Point(855, 13);
        btnDelete.Name = "btnDelete";
        btnDelete.Size = new Size(138, 35);
        btnDelete.TabIndex = 18;
        btnDelete.Text = "🗑 حذف";
        btnDelete.UseVisualStyleBackColor = false;
        btnDelete.Click += btnDelete_Click;
        // 
        // btnReactivate
        // 
        btnReactivate.BackColor = Color.FromArgb(23, 162, 184);
        btnReactivate.Enabled = false;
        btnReactivate.FlatStyle = FlatStyle.Flat;
        btnReactivate.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnReactivate.ForeColor = Color.White;
        btnReactivate.Location = new Point(711, 13);
        btnReactivate.Name = "btnReactivate";
        btnReactivate.Size = new Size(138, 35);
        btnReactivate.TabIndex = 20;
        btnReactivate.Text = "إعادة تنشيط";
        btnReactivate.UseVisualStyleBackColor = false;
        btnReactivate.Click += btnReactivate_Click;
        toolTip.SetToolTip(btnReactivate, "إعادة تنشيط السجل المعطّل");
        // 
        // btnRefresh
        // 
        btnRefresh.BackColor = Color.FromArgb(23, 162, 184);
        btnRefresh.FlatStyle = FlatStyle.Flat;
        btnRefresh.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnRefresh.ForeColor = Color.White;
        btnRefresh.Location = new Point(575, 13);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(130, 35);
        btnRefresh.TabIndex = 21;
        btnRefresh.Text = "🔄 تحديث";
        btnRefresh.UseVisualStyleBackColor = false;
        btnRefresh.Click += btnRefresh_Click;
        // 
        // btnClose
        // 
        btnClose.BackColor = Color.FromArgb(108, 117, 125);
        btnClose.FlatStyle = FlatStyle.Flat;
        btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnClose.ForeColor = Color.White;
        btnClose.Location = new Point(439, 13);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(130, 35);
        btnClose.TabIndex = 22;
        btnClose.TabStop = false;
        btnClose.Text = "🚪 خروج";
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += btnClose_Click;
        // 
        // pnlFilters
        // 
        pnlFilters.BackColor = Color.FromArgb(235, 240, 235);
        pnlFilters.Controls.Add(gbFilter);
        pnlFilters.Controls.Add(btnToggleFilters);
        pnlFilters.Dock = DockStyle.Top;
        pnlFilters.Location = new Point(0, 372);
        pnlFilters.Margin = new Padding(0);
        pnlFilters.Name = "pnlFilters";
        pnlFilters.Padding = new Padding(6, 4, 6, 4);
        pnlFilters.Size = new Size(1432, 112);
        pnlFilters.TabIndex = 3;
        // 
        // gbFilter
        // 
        gbFilter.BackColor = Color.FromArgb(235, 245, 235);
        gbFilter.Controls.Add(label2);
        gbFilter.Controls.Add(cbFilterBy);
        gbFilter.Controls.Add(txtSearch);
        gbFilter.Controls.Add(label1);
        gbFilter.Controls.Add(cbStatus);
        gbFilter.Controls.Add(chkShowInactive);
        gbFilter.Controls.Add(chkOnlyWithBalance);
        gbFilter.Controls.Add(chkOnlyOverLimit);
        gbFilter.Controls.Add(btnClearFilters);
        gbFilter.Dock = DockStyle.Fill;
        gbFilter.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        gbFilter.ForeColor = Color.FromArgb(14, 82, 52);
        gbFilter.Location = new Point(146, 4);
        gbFilter.Margin = new Padding(0);
        gbFilter.Name = "gbFilter";
        gbFilter.Padding = new Padding(8);
        gbFilter.RightToLeft = RightToLeft.Yes;
        gbFilter.Size = new Size(1280, 104);
        gbFilter.TabIndex = 0;
        gbFilter.TabStop = false;
        gbFilter.Text = "بحث وتصفية";
        // 
        // label2
        // 
        label2.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        label2.Location = new Point(1032, 30);
        label2.Margin = new Padding(0);
        label2.Name = "label2";
        label2.Size = new Size(110, 26);
        label2.TabIndex = 2;
        label2.Text = "تصفية بـ:";
        label2.TextAlign = ContentAlignment.MiddleRight;
        // 
        // cbFilterBy
        // 
        cbFilterBy.BackColor = Color.White;
        cbFilterBy.DropDownStyle = ComboBoxStyle.DropDownList;
        cbFilterBy.FlatStyle = FlatStyle.Flat;
        cbFilterBy.Font = new Font("Tahoma", 10F);
        cbFilterBy.FormattingEnabled = true;
        cbFilterBy.Items.AddRange(new object[] { "الاسم", "الهاتف", "البريد", "الرقم الضريبي" });
        cbFilterBy.Location = new Point(884, 26);
        cbFilterBy.Margin = new Padding(0);
        cbFilterBy.Name = "cbFilterBy";
        cbFilterBy.Size = new Size(146, 32);
        cbFilterBy.TabIndex = 3;
        cbFilterBy.SelectedIndexChanged += cbFilterBy_SelectedIndexChanged;
        // 
        // txtSearch
        // 
        txtSearch.BackColor = Color.White;
        txtSearch.Font = new Font("Tahoma", 10F);
        txtSearch.Location = new Point(460, 26);
        txtSearch.Margin = new Padding(0);
        txtSearch.Name = "txtSearch";
        txtSearch.PlaceholderText = "اكتب قيمة التصفية...";
        txtSearch.Size = new Size(412, 32);
        txtSearch.TabIndex = 4;
        txtSearch.TextChanged += txtSearch_TextChanged;
        txtSearch.KeyDown += txtSearch_KeyDown;
        txtSearch.KeyPress += txtSearch_KeyPress;
        // 
        // label1
        // 
        label1.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        label1.Location = new Point(1290, 30);
        label1.Margin = new Padding(0);
        label1.Name = "label1";
        label1.Size = new Size(90, 26);
        label1.TabIndex = 0;
        label1.Text = "الحالة:";
        label1.TextAlign = ContentAlignment.MiddleRight;
        // 
        // cbStatus
        // 
        cbStatus.BackColor = Color.White;
        cbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
        cbStatus.FlatStyle = FlatStyle.Flat;
        cbStatus.Font = new Font("Tahoma", 10F);
        cbStatus.FormattingEnabled = true;
        cbStatus.Items.AddRange(new object[] { "الكل", "نشط", "معطّل" });
        cbStatus.Location = new Point(1152, 26);
        cbStatus.Margin = new Padding(0);
        cbStatus.Name = "cbStatus";
        cbStatus.Size = new Size(130, 32);
        cbStatus.TabIndex = 1;
        cbStatus.SelectedIndexChanged += cbStatus_SelectedIndexChanged;
        // 
        // chkShowInactive
        // 
        chkShowInactive.AutoSize = true;
        chkShowInactive.FlatStyle = FlatStyle.Flat;
        chkShowInactive.Location = new Point(1094, 68);
        chkShowInactive.Margin = new Padding(0);
        chkShowInactive.Name = "chkShowInactive";
        chkShowInactive.Size = new Size(188, 28);
        chkShowInactive.TabIndex = 5;
        chkShowInactive.Text = "عرض غير النشط";
        chkShowInactive.UseVisualStyleBackColor = false;
        chkShowInactive.CheckedChanged += chkShowInactive_CheckedChanged;
        // 
        // chkOnlyWithBalance
        // 
        chkOnlyWithBalance.AutoSize = true;
        chkOnlyWithBalance.FlatStyle = FlatStyle.Flat;
        chkOnlyWithBalance.Location = new Point(849, 68);
        chkOnlyWithBalance.Margin = new Padding(0);
        chkOnlyWithBalance.Name = "chkOnlyWithBalance";
        chkOnlyWithBalance.Size = new Size(235, 28);
        chkOnlyWithBalance.TabIndex = 6;
        chkOnlyWithBalance.Text = "له رصيد مستحق فقط";
        chkOnlyWithBalance.UseVisualStyleBackColor = false;
        chkOnlyWithBalance.CheckedChanged += chkOnlyWithBalance_CheckedChanged;
        // 
        // chkOnlyOverLimit
        // 
        chkOnlyOverLimit.AutoSize = true;
        chkOnlyOverLimit.FlatStyle = FlatStyle.Flat;
        chkOnlyOverLimit.Location = new Point(634, 68);
        chkOnlyOverLimit.Margin = new Padding(0);
        chkOnlyOverLimit.Name = "chkOnlyOverLimit";
        chkOnlyOverLimit.Size = new Size(190, 28);
        chkOnlyOverLimit.TabIndex = 7;
        chkOnlyOverLimit.Text = "تجاوز حد الائتمان";
        chkOnlyOverLimit.UseVisualStyleBackColor = false;
        chkOnlyOverLimit.CheckedChanged += chkOnlyOverLimit_CheckedChanged;
        // 
        // btnClearFilters
        // 
        // RULE-N148/RULE-N134: زر «مسح التصفية» داخل gbFilter بقيم حرفية مطابقة لـ
        // GreenThemeHelper.ActionNeutralBackground (108,117,125) — يظهر في المصمّم كما في التشغيل.
        // يمسح أدوات التصفية فقط ولا يعيد تحميل البيانات («تحديث» هو المسؤول عن التحميل).
        btnClearFilters.BackColor = Color.FromArgb(108, 117, 125);
        btnClearFilters.Cursor = Cursors.Hand;
        btnClearFilters.Enabled = false;
        btnClearFilters.FlatAppearance.BorderSize = 0;
        btnClearFilters.FlatStyle = FlatStyle.Flat;
        btnClearFilters.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        btnClearFilters.ForeColor = Color.White;
        btnClearFilters.Location = new Point(481, 66);
        btnClearFilters.Name = "btnClearFilters";
        btnClearFilters.Size = new Size(145, 32);
        btnClearFilters.TabIndex = 26;
        btnClearFilters.Text = "\U0001f9f9 مسح التصفية";
        toolTip.SetToolTip(btnClearFilters, "مسح كل أدوات التصفية (Esc) — لا يعيد تحميل البيانات");
        btnClearFilters.UseVisualStyleBackColor = false;
        btnClearFilters.Click += btnClearFilters_Click;
        // 
        // btnToggleFilters
        // 
        // RULE-N145: زر «إخفاء/إظهار التصفية» مُعلَن في المصمّم بقيم حرفية مطابقة لـ
        // ThemeHelper.Secondary + FilterPanelHelper حتى يظهر في المصمّم كما يظهر وقت التشغيل.
        // السلوك (النقرة · F9 · التلميح · الحالة المحفوظة) يُوصِّله FilterPanelHelper.Attach.
        btnToggleFilters.BackColor = Color.FromArgb(248, 249, 250);
        btnToggleFilters.Cursor = Cursors.Hand;
        btnToggleFilters.Dock = DockStyle.Left;
        btnToggleFilters.FlatAppearance.BorderColor = Color.FromArgb(222, 226, 230);
        btnToggleFilters.FlatAppearance.BorderSize = 1;
        btnToggleFilters.FlatAppearance.MouseOverBackColor = Color.FromArgb(240, 240, 240);
        btnToggleFilters.FlatStyle = FlatStyle.Popup;
        btnToggleFilters.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        btnToggleFilters.ForeColor = Color.FromArgb(33, 37, 41);
        btnToggleFilters.Location = new Point(6, 4);
        btnToggleFilters.Name = "btnToggleFilters";
        btnToggleFilters.Size = new Size(140, 104);
        btnToggleFilters.TabIndex = 25;
        btnToggleFilters.Text = "إخفاء التصفية";
        btnToggleFilters.UseVisualStyleBackColor = false;
        // 
        // pnlSummary
        // 
        pnlSummary.BackColor = Color.FromArgb(235, 240, 235);
        pnlSummary.Controls.Add(gbSummary);
        pnlSummary.Dock = DockStyle.Bottom;
        pnlSummary.Location = new Point(0, 830);
        pnlSummary.Margin = new Padding(0);
        pnlSummary.Name = "pnlSummary";
        pnlSummary.Size = new Size(1432, 48);
        pnlSummary.TabIndex = 5;
        // 
        // gbSummary
        // 
        // RULE-N136: كتلة الإجماليات داخل صندوق مجموعة.
        gbSummary.BackColor = Color.FromArgb(235, 245, 235);
        gbSummary.Controls.Add(lblSummary);
        gbSummary.Dock = DockStyle.Fill;
        gbSummary.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        gbSummary.ForeColor = Color.FromArgb(14, 82, 52);
        gbSummary.Location = new Point(0, 0);
        gbSummary.Name = "gbSummary";
        gbSummary.Size = new Size(1432, 48);
        gbSummary.TabIndex = 0;
        gbSummary.TabStop = false;
        gbSummary.Text = "الإجماليات";
        // 
        // lblSummary
        // 
        lblSummary.Dock = DockStyle.Fill;
        lblSummary.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblSummary.ForeColor = Color.FromArgb(14, 82, 52);
        lblSummary.Location = new Point(3, 23);
        lblSummary.Name = "lblSummary";
        lblSummary.Size = new Size(1426, 22);
        lblSummary.TabIndex = 0;
        lblSummary.Text = "المعروض: 0 من 0";
        lblSummary.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlGrid
        // 
        pnlGrid.BackColor = Color.FromArgb(220, 240, 220);
        pnlGrid.Controls.Add(gbGrid);
        pnlGrid.Dock = DockStyle.Fill;
        pnlGrid.Location = new Point(0, 484);
        pnlGrid.Name = "pnlGrid";
        pnlGrid.Size = new Size(1432, 346);
        pnlGrid.TabIndex = 4;
        // 
        // gbGrid
        // 
        // RULE-N136: كتلة المحتوى الرئيسي (الشبكة) داخل صندوق مجموعة.
        gbGrid.BackColor = Color.FromArgb(220, 240, 220);
        gbGrid.Controls.Add(lblEmptyState);
        gbGrid.Controls.Add(dgvSuppliers);
        gbGrid.Dock = DockStyle.Fill;
        gbGrid.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        gbGrid.ForeColor = Color.FromArgb(14, 82, 52);
        gbGrid.Location = new Point(0, 0);
        gbGrid.Name = "gbGrid";
        gbGrid.Size = new Size(1432, 346);
        gbGrid.TabIndex = 0;
        gbGrid.TabStop = false;
        gbGrid.Text = "قائمة الموردين";
        // 
        // lblEmptyState
        // 
        // RULE-N22: سطر «لا توجد بيانات» فوق الشبكة عند فراغ النتيجة.
        lblEmptyState.BackColor = Color.White;
        lblEmptyState.Dock = DockStyle.Fill;
        lblEmptyState.Font = new Font("Tahoma", 12F, FontStyle.Bold);
        lblEmptyState.ForeColor = Color.FromArgb(14, 82, 52);
        lblEmptyState.Location = new Point(3, 23);
        lblEmptyState.Name = "lblEmptyState";
        lblEmptyState.Size = new Size(1426, 320);
        lblEmptyState.TabIndex = 1;
        lblEmptyState.Text = "لا توجد بيانات";
        lblEmptyState.TextAlign = ContentAlignment.MiddleCenter;
        lblEmptyState.Visible = false;
        // 
        // dgvSuppliers
        // 
        // RULE-N134: نفس قيم ثيم GreenThemeHelper حرفياً — ليتساوى المصمّم ووقت التشغيل.
        dgvSuppliers.AllowUserToAddRows = false;
        dgvSuppliers.AllowUserToDeleteRows = false;
        dgvSuppliers.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 252, 245);
        dgvSuppliers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvSuppliers.BackgroundColor = Color.FromArgb(220, 240, 220);
        dgvSuppliers.BorderStyle = BorderStyle.None;
        dgvSuppliers.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
        dgvSuppliers.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        dgvSuppliers.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(14, 165, 103);
        dgvSuppliers.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        dgvSuppliers.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        dgvSuppliers.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(14, 165, 103);
        dgvSuppliers.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
        dgvSuppliers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvSuppliers.ContextMenuStrip = cmsGrid;
        dgvSuppliers.DefaultCellStyle.BackColor = Color.White;
        dgvSuppliers.DefaultCellStyle.Font = new Font("Tahoma", 10F);
        dgvSuppliers.DefaultCellStyle.ForeColor = Color.FromArgb(33, 37, 41);
        dgvSuppliers.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 255, 180);
        dgvSuppliers.DefaultCellStyle.SelectionForeColor = Color.FromArgb(33, 37, 41);
        dgvSuppliers.Dock = DockStyle.Fill;
        dgvSuppliers.EnableHeadersVisualStyles = false;
        dgvSuppliers.GridColor = Color.FromArgb(205, 225, 205);
        dgvSuppliers.Location = new Point(3, 23);
        dgvSuppliers.MultiSelect = false;
        dgvSuppliers.Name = "dgvSuppliers";
        dgvSuppliers.ReadOnly = true;
        dgvSuppliers.RowHeadersVisible = false;
        dgvSuppliers.RowHeadersWidth = 62;
        dgvSuppliers.RowTemplate.Height = 32;
        dgvSuppliers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvSuppliers.Size = new Size(1426, 320);
        dgvSuppliers.TabIndex = 0;
        dgvSuppliers.CellClick += dgvSuppliers_CellClick;
        dgvSuppliers.CellMouseDown += dgvSuppliers_CellMouseDown;
        dgvSuppliers.DoubleClick += dgvSuppliers_DoubleClick;
        // 
        // cmsGrid
        // 
        // RULE-N23: قائمة سياق على شبكة الدليل — كل عنصر يستدعي نفس زر الشاشة (بلا منطق مكرر).
        cmsGrid.BackColor = Color.White;
        cmsGrid.Font = new Font("Tahoma", 10F);
        cmsGrid.ImageScalingSize = new Size(24, 24);
        cmsGrid.Items.AddRange(new ToolStripItem[] { cmsEdit, cmsDelete, cmsReactivate, cmsSeparator, cmsRefresh, cmsExport });
        cmsGrid.Name = "cmsGrid";
        cmsGrid.RightToLeft = RightToLeft.Yes;
        cmsGrid.Size = new Size(240, 164);
        cmsGrid.Opening += cmsGrid_Opening;
        // 
        // cmsEdit
        // 
        cmsEdit.Name = "cmsEdit";
        cmsEdit.Size = new Size(239, 30);
        cmsEdit.Text = "✏️ تعديل";
        cmsEdit.Click += cmsEdit_Click;
        // 
        // cmsDelete
        // 
        cmsDelete.Name = "cmsDelete";
        cmsDelete.Size = new Size(239, 30);
        cmsDelete.Text = "🗑 تعطيل";
        cmsDelete.Click += cmsDelete_Click;
        // 
        // cmsReactivate
        // 
        cmsReactivate.Name = "cmsReactivate";
        cmsReactivate.Size = new Size(239, 30);
        cmsReactivate.Text = "↺ إعادة تنشيط";
        cmsReactivate.Click += cmsReactivate_Click;
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
        cmsRefresh.Text = "🔄 تحديث";
        cmsRefresh.Click += cmsRefresh_Click;
        // 
        // cmsExport
        // 
        cmsExport.Name = "cmsExport";
        cmsExport.Size = new Size(239, 30);
        cmsExport.Text = "📤 تصدير إلى Excel";
        cmsExport.Click += cmsExport_Click;
        // 
        // errorProvider
        // 
        errorProvider.ContainerControl = this;
        errorProvider.RightToLeft = true;
        // 
        // liveTip
        // 
        // RULE-N97: تلميح التفقيط الحيّ على الحقل المالي.
        liveTip.AutomaticDelay = 300;
        liveTip.AutoPopDelay = 4000;
        liveTip.InitialDelay = 100;
        liveTip.ReshowDelay = 50;
        liveTip.ShowAlways = true;
        // 
        // frmSupplierMaster
        // 
        AutoScaleDimensions = new SizeF(11F, 24F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(170, 220, 170);
        ClientSize = new Size(1432, 878);
        Controls.Add(pnlGrid);
        Controls.Add(pnlSummary);
        Controls.Add(pnlFilters);
        Controls.Add(gbToolbar);
        Controls.Add(pnlFields);
        Controls.Add(lblMode);
        Font = new Font("Tahoma", 10F);
        KeyPreview = true;
        MinimumSize = new Size(1200, 700);
        Name = "frmSupplierMaster";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterParent;
        Text = "دليل الموردين";
        Load += frmSupplierMaster_Load;
        pnlFields.ResumeLayout(false);
        gbEditor.ResumeLayout(false);
        gbEditor.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numCreditLimit).EndInit();
        gbToolbar.ResumeLayout(false);
        pnlButtons.ResumeLayout(false);
        pnlFilters.ResumeLayout(false);
        gbFilter.ResumeLayout(false);
        gbFilter.PerformLayout();
        pnlSummary.ResumeLayout(false);
        gbSummary.ResumeLayout(false);
        pnlGrid.ResumeLayout(false);
        gbGrid.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvSuppliers).EndInit();
        cmsGrid.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        ResumeLayout(false);
    }

    private System.Windows.Forms.Label lblMode;
    private System.Windows.Forms.Panel pnlFields;
    private System.Windows.Forms.GroupBox gbEditor;
    private System.Windows.Forms.Label lblId;
    private System.Windows.Forms.TextBox txtId;
    private System.Windows.Forms.Label lblName;
    private System.Windows.Forms.Label lblNameReq;
    private System.Windows.Forms.TextBox txtName;
    private System.Windows.Forms.Label lblPhone;
    private System.Windows.Forms.TextBox txtPhone;
    private System.Windows.Forms.Label lblTaxNumber;
    private System.Windows.Forms.TextBox txtTaxNumber;
    private System.Windows.Forms.Label lblEmail;
    private System.Windows.Forms.TextBox txtEmail;
    private System.Windows.Forms.Label lblCreditLimit;
    private System.Windows.Forms.NumericUpDown numCreditLimit;
    private System.Windows.Forms.Label lblAddress;
    private System.Windows.Forms.TextBox txtAddress;
    private System.Windows.Forms.Label lblNotes;
    private System.Windows.Forms.TextBox txtNotes;
    private System.Windows.Forms.GroupBox gbToolbar;
    private System.Windows.Forms.FlowLayoutPanel pnlButtons;
    private System.Windows.Forms.Button btnNew;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.Button btnEdit;
    private System.Windows.Forms.Button btnDelete;
    private System.Windows.Forms.Button btnReactivate;
    private System.Windows.Forms.Button btnRefresh;
    private System.Windows.Forms.Button btnClose;
    private System.Windows.Forms.Panel pnlFilters;
    private System.Windows.Forms.GroupBox gbFilter;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.ComboBox cbFilterBy;
    private System.Windows.Forms.TextBox txtSearch;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ComboBox cbStatus;
    private System.Windows.Forms.CheckBox chkShowInactive;
    private System.Windows.Forms.CheckBox chkOnlyWithBalance;
    private System.Windows.Forms.CheckBox chkOnlyOverLimit;
    private System.Windows.Forms.Button btnClearFilters;
    private System.Windows.Forms.Button btnToggleFilters;
    private System.Windows.Forms.Panel pnlSummary;
    private System.Windows.Forms.GroupBox gbSummary;
    private System.Windows.Forms.Label lblSummary;
    private System.Windows.Forms.Panel pnlGrid;
    private System.Windows.Forms.GroupBox gbGrid;
    private System.Windows.Forms.Label lblEmptyState;
    private System.Windows.Forms.DataGridView dgvSuppliers;
    private System.Windows.Forms.ContextMenuStrip cmsGrid;
    private System.Windows.Forms.ToolStripMenuItem cmsEdit;
    private System.Windows.Forms.ToolStripMenuItem cmsDelete;
    private System.Windows.Forms.ToolStripMenuItem cmsReactivate;
    private System.Windows.Forms.ToolStripSeparator cmsSeparator;
    private System.Windows.Forms.ToolStripMenuItem cmsRefresh;
    private System.Windows.Forms.ToolStripMenuItem cmsExport;
    private System.Windows.Forms.ErrorProvider errorProvider;
    private System.Windows.Forms.ToolTip liveTip;
    private System.Windows.Forms.ToolTip toolTip;
}
