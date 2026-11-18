namespace SalesSystem.Desktop.Screens.Customers.Forms;

partial class frmCRUDCustomerMaster
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
        DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
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
        btnShowCard = new Button();
        btnFindCustomer = new Button();
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
        dgvCustomers = new DataGridView();
        cmsGrid = new ContextMenuStrip(components);
        cmsEdit = new ToolStripMenuItem();
        cmsShowCard = new ToolStripMenuItem();
        cmsDelete = new ToolStripMenuItem();
        cmsReactivate = new ToolStripMenuItem();
        cmsFindCustomer = new ToolStripMenuItem();
        cmsSeparator = new ToolStripSeparator();
        cmsRefresh = new ToolStripMenuItem();
        cmsExport = new ToolStripMenuItem();
        errorProvider = new ErrorProvider(components);
        liveTip = new ToolTip(components);
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
        ((System.ComponentModel.ISupportInitialize)dgvCustomers).BeginInit();
        cmsGrid.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
        SuspendLayout();
        // 
        // lblMode
        // 
        lblMode.BackColor = Color.FromArgb(16, 150, 96);
        lblMode.Dock = DockStyle.Top;
        lblMode.Font = new Font("Tahoma", 14F, FontStyle.Bold);
        lblMode.ForeColor = Color.White;
        lblMode.Location = new Point(0, 0);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(1432, 50);
        lblMode.TabIndex = 0;
        lblMode.Text = "(( دليل العملاء ))";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlFields
        // 
        pnlFields.BackColor = Color.White;
        pnlFields.Controls.Add(gbEditor);
        pnlFields.Dock = DockStyle.Top;
        pnlFields.Location = new Point(0, 50);
        pnlFields.Name = "pnlFields";
        pnlFields.Size = new Size(1432, 244);
        pnlFields.TabIndex = 1;
        // 
        // gbEditor
        // 
        gbEditor.BackColor = Color.White;
        gbEditor.Controls.Add(lblId);
        gbEditor.Controls.Add(txtId);
        gbEditor.Controls.Add(lblName);
        gbEditor.Controls.Add(lblNameReq);
        gbEditor.Controls.Add(txtName);
        gbEditor.Controls.Add(lblPhone);
        gbEditor.Controls.Add(txtPhone);
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
        gbEditor.ForeColor = Color.FromArgb(33, 37, 41);
        gbEditor.Location = new Point(0, 0);
        gbEditor.Name = "gbEditor";
        gbEditor.Size = new Size(1432, 244);
        gbEditor.TabIndex = 0;
        gbEditor.TabStop = false;
        gbEditor.Text = "بيانات العميل";
        // 
        // lblId
        // 
        lblId.AutoSize = true;
        lblId.Location = new Point(1150, 30);
        lblId.Name = "lblId";
        lblId.Size = new Size(113, 24);
        lblId.TabIndex = 0;
        lblId.Text = "رقم العميل";
        // 
        // txtId
        // 
        txtId.BackColor = Color.White;
        txtId.Font = new Font("Tahoma", 10F);
        txtId.Location = new Point(1132, 57);
        txtId.Name = "txtId";
        txtId.ReadOnly = true;
        txtId.Size = new Size(157, 32);
        txtId.TabIndex = 0;
        txtId.TabStop = false;
        // 
        // lblName
        // 
        lblName.AutoSize = true;
        lblName.Location = new Point(866, 30);
        lblName.Name = "lblName";
        lblName.Size = new Size(121, 24);
        lblName.TabIndex = 2;
        lblName.Text = "اسم العميل";
        // 
        // lblNameReq
        // 
        lblNameReq.AutoSize = true;
        lblNameReq.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblNameReq.ForeColor = Color.Red;
        lblNameReq.Location = new Point(837, 30);
        lblNameReq.Name = "lblNameReq";
        lblNameReq.Size = new Size(23, 24);
        lblNameReq.TabIndex = 3;
        lblNameReq.Text = "*";
        // 
        // txtName
        // 
        txtName.BackColor = Color.White;
        txtName.Font = new Font("Tahoma", 10F);
        txtName.Location = new Point(801, 56);
        txtName.Name = "txtName";
        txtName.Size = new Size(250, 32);
        txtName.TabIndex = 1;
        // 
        // lblPhone
        // 
        lblPhone.AutoSize = true;
        lblPhone.Location = new Point(611, 30);
        lblPhone.Name = "lblPhone";
        lblPhone.Size = new Size(70, 24);
        lblPhone.TabIndex = 4;
        lblPhone.Text = "الهاتف";
        // 
        // txtPhone
        // 
        txtPhone.BackColor = Color.White;
        txtPhone.Font = new Font("Tahoma", 10F);
        txtPhone.Location = new Point(521, 56);
        txtPhone.Name = "txtPhone";
        txtPhone.Size = new Size(250, 32);
        txtPhone.TabIndex = 2;
        // 
        // lblEmail
        // 
        lblEmail.AutoSize = true;
        lblEmail.Location = new Point(1176, 100);
        lblEmail.Name = "lblEmail";
        lblEmail.Size = new Size(60, 24);
        lblEmail.TabIndex = 8;
        lblEmail.Text = "البريد";
        // 
        // txtEmail
        // 
        txtEmail.BackColor = Color.White;
        txtEmail.Font = new Font("Tahoma", 10F);
        txtEmail.Location = new Point(1081, 126);
        txtEmail.Name = "txtEmail";
        txtEmail.Size = new Size(250, 32);
        txtEmail.TabIndex = 4;
        // 
        // lblCreditLimit
        // 
        lblCreditLimit.AutoSize = true;
        lblCreditLimit.Location = new Point(869, 100);
        lblCreditLimit.Name = "lblCreditLimit";
        lblCreditLimit.Size = new Size(114, 24);
        lblCreditLimit.TabIndex = 10;
        lblCreditLimit.Text = "حد الائتمان";
        // 
        // numCreditLimit
        // 
        numCreditLimit.BackColor = Color.White;
        numCreditLimit.DecimalPlaces = 2;
        numCreditLimit.Font = new Font("Tahoma", 10F);
        numCreditLimit.Location = new Point(801, 126);
        numCreditLimit.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 });
        numCreditLimit.Name = "numCreditLimit";
        numCreditLimit.Size = new Size(250, 32);
        numCreditLimit.TabIndex = 5;
        numCreditLimit.ThousandsSeparator = true;
        // 
        // lblAddress
        // 
        lblAddress.AutoSize = true;
        lblAddress.Location = new Point(468, 100);
        lblAddress.Name = "lblAddress";
        lblAddress.Size = new Size(76, 24);
        lblAddress.TabIndex = 12;
        lblAddress.Text = "العنوان";
        // 
        // txtAddress
        // 
        txtAddress.BackColor = Color.White;
        txtAddress.Font = new Font("Tahoma", 10F);
        txtAddress.Location = new Point(241, 126);
        txtAddress.Name = "txtAddress";
        txtAddress.Size = new Size(530, 32);
        txtAddress.TabIndex = 6;
        // 
        // lblNotes
        // 
        lblNotes.AutoSize = true;
        lblNotes.Location = new Point(739, 170);
        lblNotes.Name = "lblNotes";
        lblNotes.Size = new Size(95, 24);
        lblNotes.TabIndex = 14;
        lblNotes.Text = "ملاحظات";
        // 
        // txtNotes
        // 
        txtNotes.BackColor = Color.White;
        txtNotes.Font = new Font("Tahoma", 10F);
        txtNotes.Location = new Point(241, 196);
        txtNotes.Multiline = true;
        txtNotes.Name = "txtNotes";
        txtNotes.ScrollBars = ScrollBars.Vertical;
        txtNotes.Size = new Size(1090, 34);
        txtNotes.TabIndex = 7;
        // 
        // gbToolbar
        // 
        gbToolbar.BackColor = Color.White;
        gbToolbar.Controls.Add(pnlButtons);
        gbToolbar.Dock = DockStyle.Top;
        gbToolbar.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        gbToolbar.ForeColor = Color.FromArgb(33, 37, 41);
        gbToolbar.Location = new Point(0, 294);
        gbToolbar.Name = "gbToolbar";
        gbToolbar.Size = new Size(1432, 78);
        gbToolbar.TabIndex = 2;
        gbToolbar.TabStop = false;
        gbToolbar.Text = "أوامر العملية";
        // 
        // pnlButtons
        // 
        pnlButtons.BackColor = Color.White;
        pnlButtons.Controls.Add(btnNew);
        pnlButtons.Controls.Add(btnSave);
        pnlButtons.Controls.Add(btnEdit);
        pnlButtons.Controls.Add(btnDelete);
        pnlButtons.Controls.Add(btnReactivate);
        pnlButtons.Controls.Add(btnRefresh);
        pnlButtons.Controls.Add(btnClose);
        pnlButtons.Controls.Add(btnShowCard);
        pnlButtons.Controls.Add(btnFindCustomer);
        pnlButtons.Dock = DockStyle.Fill;
        pnlButtons.Location = new Point(3, 28);
        pnlButtons.Name = "pnlButtons";
        pnlButtons.Padding = new Padding(10);
        pnlButtons.Size = new Size(1426, 47);
        pnlButtons.TabIndex = 0;
        // 
        // btnNew
        // 
        btnNew.BackColor = Color.FromArgb(40, 167, 69);
        btnNew.FlatStyle = FlatStyle.Flat;
        btnNew.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnNew.ForeColor = Color.White;
        btnNew.Location = new Point(1261, 13);
        btnNew.Name = "btnNew";
        btnNew.Size = new Size(142, 35);
        btnNew.TabIndex = 0;
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
        btnSave.Location = new Point(1127, 13);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(128, 35);
        btnSave.TabIndex = 1;
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
        btnEdit.Location = new Point(993, 13);
        btnEdit.Name = "btnEdit";
        btnEdit.Size = new Size(128, 35);
        btnEdit.TabIndex = 3;
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
        btnDelete.Location = new Point(849, 13);
        btnDelete.Name = "btnDelete";
        btnDelete.Size = new Size(138, 35);
        btnDelete.TabIndex = 2;
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
        btnReactivate.Location = new Point(649, 13);
        btnReactivate.Name = "btnReactivate";
        btnReactivate.Size = new Size(194, 35);
        btnReactivate.TabIndex = 6;
        btnReactivate.Text = "↺ إعادة تنشيط";
        btnReactivate.UseVisualStyleBackColor = false;
        btnReactivate.Click += btnReactivate_Click;
        // 
        // btnRefresh
        // 
        btnRefresh.BackColor = Color.FromArgb(23, 162, 184);
        btnRefresh.FlatStyle = FlatStyle.Flat;
        btnRefresh.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnRefresh.ForeColor = Color.White;
        btnRefresh.Location = new Point(513, 13);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(130, 35);
        btnRefresh.TabIndex = 4;
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
        btnClose.Location = new Point(377, 13);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(130, 35);
        btnClose.TabIndex = 5;
        btnClose.TabStop = false;
        btnClose.Text = "🚪 خروج";
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += btnClose_Click;
        // 
        // btnShowCard
        // 
        btnShowCard.BackColor = Color.FromArgb(Convert.ToInt32(108), Convert.ToInt32(117), Convert.ToInt32(125));
        btnShowCard.Cursor = Cursors.Hand;
        btnShowCard.Enabled = false;
        btnShowCard.FlatStyle = FlatStyle.Flat;
        btnShowCard.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnShowCard.ForeColor = Color.White;
        btnShowCard.Location = new Point(241, 13);
        btnShowCard.Name = "btnShowCard";
        btnShowCard.Size = new Size(130, 35);
        btnShowCard.TabIndex = 7;
        btnShowCard.Text = "بطاقة";
        btnShowCard.UseVisualStyleBackColor = false;
        liveTip.SetToolTip(btnShowCard, "عرض بطاقة العميل المحدد");
        btnShowCard.Click += btnShowCard_Click;
        // 
        // btnFindCustomer
        // 
        btnFindCustomer.BackColor = Color.FromArgb(Convert.ToInt32(23), Convert.ToInt32(162), Convert.ToInt32(184));
        btnFindCustomer.Cursor = Cursors.Hand;
        btnFindCustomer.FlatStyle = FlatStyle.Flat;
        btnFindCustomer.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnFindCustomer.ForeColor = Color.White;
        btnFindCustomer.Location = new Point(105, 13);
        btnFindCustomer.Name = "btnFindCustomer";
        btnFindCustomer.Size = new Size(130, 35);
        btnFindCustomer.TabIndex = 8;
        btnFindCustomer.Text = "بحث عن عميل";
        btnFindCustomer.UseVisualStyleBackColor = false;
        liveTip.SetToolTip(btnFindCustomer, "البحث عن عميل وتحديده في الشبكة");
        btnFindCustomer.Click += btnFindCustomer_Click;
        // 
        // pnlFilters
        // 
        pnlFilters.BackColor = Color.White;
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
        gbFilter.BackColor = Color.White;
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
        gbFilter.ForeColor = Color.FromArgb(33, 37, 41);
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
        label2.AutoSize = true;
        label2.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        label2.Location = new Point(1004, 37);
        label2.Margin = new Padding(0);
        label2.Name = "label2";
        label2.Size = new Size(95, 24);
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
        cbFilterBy.Items.AddRange(new object[] { "الاسم", "الهاتف", "البريد" });
        cbFilterBy.Location = new Point(851, 33);
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
        txtSearch.Location = new Point(432, 33);
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
        label1.AutoSize = true;
        label1.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        label1.Location = new Point(996, 70);
        label1.Margin = new Padding(0);
        label1.Name = "label1";
        label1.Size = new Size(72, 24);
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
        cbStatus.Location = new Point(851, 66);
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
        chkShowInactive.Location = new Point(648, 68);
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
        chkOnlyWithBalance.Location = new Point(438, 68);
        chkOnlyWithBalance.Margin = new Padding(0);
        chkOnlyWithBalance.Name = "chkOnlyWithBalance";
        chkOnlyWithBalance.Size = new Size(195, 28);
        chkOnlyWithBalance.TabIndex = 6;
        chkOnlyWithBalance.Text = "عليه مديونية فقط";
        chkOnlyWithBalance.UseVisualStyleBackColor = false;
        chkOnlyWithBalance.CheckedChanged += chkOnlyWithBalance_CheckedChanged;
        // 
        // chkOnlyOverLimit
        // 
        chkOnlyOverLimit.AutoSize = true;
        chkOnlyOverLimit.FlatStyle = FlatStyle.Flat;
        chkOnlyOverLimit.Location = new Point(233, 68);
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
        btnClearFilters.BackColor = Color.FromArgb(108, 117, 125);
        btnClearFilters.Cursor = Cursors.Hand;
        btnClearFilters.Enabled = false;
        btnClearFilters.FlatAppearance.BorderSize = 0;
        btnClearFilters.FlatStyle = FlatStyle.Flat;
        btnClearFilters.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        btnClearFilters.ForeColor = Color.White;
        btnClearFilters.Location = new Point(239, 33);
        btnClearFilters.Name = "btnClearFilters";
        btnClearFilters.Size = new Size(190, 32);
        btnClearFilters.TabIndex = 26;
        btnClearFilters.Text = "\U0001f9f9 مسح التصفية";
        liveTip.SetToolTip(btnClearFilters, "مسح كل أدوات التصفية (Esc) — لا يعيد تحميل البيانات");
        btnClearFilters.UseVisualStyleBackColor = false;
        btnClearFilters.Click += btnClearFilters_Click;
        // 
        // btnToggleFilters
        // 
        btnToggleFilters.BackColor = Color.FromArgb(248, 249, 250);
        btnToggleFilters.Cursor = Cursors.Hand;
        btnToggleFilters.Dock = DockStyle.Left;
        btnToggleFilters.FlatAppearance.BorderColor = Color.FromArgb(222, 226, 230);
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
        pnlSummary.BackColor = Color.White;
        pnlSummary.Controls.Add(gbSummary);
        pnlSummary.Dock = DockStyle.Bottom;
        pnlSummary.Location = new Point(0, 828);
        pnlSummary.Margin = new Padding(0);
        pnlSummary.Name = "pnlSummary";
        pnlSummary.Size = new Size(1432, 66);
        pnlSummary.TabIndex = 5;
        // 
        // gbSummary
        // 
        gbSummary.BackColor = Color.White;
        gbSummary.Controls.Add(lblSummary);
        gbSummary.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        gbSummary.ForeColor = Color.FromArgb(33, 37, 41);
        gbSummary.Location = new Point(0, 3);
        gbSummary.Name = "gbSummary";
        gbSummary.Size = new Size(1432, 60);
        gbSummary.TabIndex = 0;
        gbSummary.TabStop = false;
        gbSummary.Text = "الإجماليات";
        // 
        // lblSummary
        // 
        lblSummary.Dock = DockStyle.Fill;
        lblSummary.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblSummary.ForeColor = Color.FromArgb(33, 37, 41);
        lblSummary.Location = new Point(3, 28);
        lblSummary.Name = "lblSummary";
        lblSummary.Size = new Size(1426, 29);
        lblSummary.TabIndex = 0;
        lblSummary.Text = "المعروض: 0 من 0";
        lblSummary.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlGrid
        // 
        pnlGrid.BackColor = Color.White;
        pnlGrid.Controls.Add(gbGrid);
        pnlGrid.Dock = DockStyle.Fill;
        pnlGrid.Location = new Point(0, 484);
        pnlGrid.Name = "pnlGrid";
        pnlGrid.Size = new Size(1432, 344);
        pnlGrid.TabIndex = 4;
        // 
        // gbGrid
        // 
        gbGrid.BackColor = Color.White;
        gbGrid.Controls.Add(lblEmptyState);
        gbGrid.Controls.Add(dgvCustomers);
        gbGrid.Dock = DockStyle.Fill;
        gbGrid.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        gbGrid.ForeColor = Color.FromArgb(33, 37, 41);
        gbGrid.Location = new Point(0, 0);
        gbGrid.Name = "gbGrid";
        gbGrid.Size = new Size(1432, 344);
        gbGrid.TabIndex = 0;
        gbGrid.TabStop = false;
        gbGrid.Text = "قائمة العملاء";
        // 
        // lblEmptyState
        // 
        lblEmptyState.BackColor = Color.White;
        lblEmptyState.Dock = DockStyle.Fill;
        lblEmptyState.Font = new Font("Tahoma", 12F, FontStyle.Bold);
        lblEmptyState.ForeColor = Color.FromArgb(33, 37, 41);
        lblEmptyState.Location = new Point(3, 28);
        lblEmptyState.Name = "lblEmptyState";
        lblEmptyState.Size = new Size(1426, 313);
        lblEmptyState.TabIndex = 1;
        lblEmptyState.Text = "لا توجد بيانات";
        lblEmptyState.TextAlign = ContentAlignment.MiddleCenter;
        lblEmptyState.Visible = false;
        // 
        // dgvCustomers
        // 
        dgvCustomers.AllowUserToAddRows = false;
        dgvCustomers.AllowUserToDeleteRows = false;
        dataGridViewCellStyle1.BackColor = Color.FromArgb(248, 249, 250);
        dgvCustomers.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
        dgvCustomers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvCustomers.BackgroundColor = Color.White;
        dgvCustomers.BorderStyle = BorderStyle.None;
        dgvCustomers.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
        dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
        dataGridViewCellStyle2.BackColor = Color.FromArgb(52, 58, 64);
        dataGridViewCellStyle2.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        dataGridViewCellStyle2.ForeColor = Color.White;
        dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(52, 58, 64);
        dataGridViewCellStyle2.SelectionForeColor = Color.White;
        dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
        dgvCustomers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
        dgvCustomers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvCustomers.ContextMenuStrip = cmsGrid;
        dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle3.BackColor = Color.White;
        dataGridViewCellStyle3.Font = new Font("Tahoma", 10F);
        dataGridViewCellStyle3.ForeColor = Color.FromArgb(33, 37, 41);
        dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(255, 243, 205);
        dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(33, 37, 41);
        dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
        dgvCustomers.DefaultCellStyle = dataGridViewCellStyle3;
        dgvCustomers.Dock = DockStyle.Fill;
        dgvCustomers.EnableHeadersVisualStyles = false;
        dgvCustomers.GridColor = Color.FromArgb(222, 226, 230);
        dgvCustomers.Location = new Point(3, 28);
        dgvCustomers.MultiSelect = false;
        dgvCustomers.Name = "dgvCustomers";
        dgvCustomers.ReadOnly = true;
        dgvCustomers.RowHeadersVisible = false;
        dgvCustomers.RowHeadersWidth = 62;
        dgvCustomers.RowTemplate.Height = 32;
        dgvCustomers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvCustomers.Size = new Size(1426, 313);
        dgvCustomers.TabIndex = 0;
        dgvCustomers.CellClick += dgvCustomers_CellClick;
        dgvCustomers.CellMouseDown += dgvCustomers_CellMouseDown;
        dgvCustomers.DoubleClick += dgvCustomers_DoubleClick;
        // 
        // cmsGrid
        // 
        cmsGrid.BackColor = Color.White;
        cmsGrid.Font = new Font("Tahoma", 10F);
        cmsGrid.ImageScalingSize = new Size(24, 24);
        cmsGrid.Items.AddRange(new ToolStripItem[] { cmsEdit, cmsShowCard, cmsDelete, cmsReactivate, cmsFindCustomer, cmsSeparator, cmsRefresh, cmsExport });
        cmsGrid.Name = "cmsGrid";
        cmsGrid.RightToLeft = RightToLeft.Yes;
        cmsGrid.Size = new Size(245, 220);
        cmsGrid.Opening += cmsGrid_Opening;
        // 
        // cmsEdit
        // 
        cmsEdit.Name = "cmsEdit";
        cmsEdit.Size = new Size(244, 30);
        cmsEdit.Text = "✏️ تعديل";
        cmsEdit.Click += cmsEdit_Click;
        // 
        // cmsShowCard
        // 
        cmsShowCard.Name = "cmsShowCard";
        cmsShowCard.Size = new Size(244, 30);
        cmsShowCard.Text = "بطاقة العميل";
        cmsShowCard.Click += cmsShowCard_Click;
        // 
        // cmsDelete
        // 
        cmsDelete.Name = "cmsDelete";
        cmsDelete.Size = new Size(244, 30);
        cmsDelete.Text = "🗑 تعطيل";
        cmsDelete.Click += cmsDelete_Click;
        // 
        // cmsReactivate
        // 
        cmsReactivate.Name = "cmsReactivate";
        cmsReactivate.Size = new Size(244, 30);
        cmsReactivate.Text = "↺ إعادة تنشيط";
        cmsReactivate.Click += cmsReactivate_Click;
        // 
        // cmsFindCustomer
        // 
        cmsFindCustomer.Name = "cmsFindCustomer";
        cmsFindCustomer.Size = new Size(244, 30);
        cmsFindCustomer.Text = "بحث وتصفية عن عميل";
        cmsFindCustomer.Click += cmsFindCustomer_Click;
        // 
        // cmsSeparator
        // 
        cmsSeparator.Name = "cmsSeparator";
        cmsSeparator.Size = new Size(241, 6);
        // 
        // cmsRefresh
        // 
        cmsRefresh.Name = "cmsRefresh";
        cmsRefresh.Size = new Size(244, 30);
        cmsRefresh.Text = "🔄 تحديث";
        cmsRefresh.Click += cmsRefresh_Click;
        // 
        // cmsExport
        // 
        cmsExport.Name = "cmsExport";
        cmsExport.Size = new Size(244, 30);
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
        liveTip.AutomaticDelay = 300;
        liveTip.AutoPopDelay = 4000;
        liveTip.InitialDelay = 100;
        liveTip.ReshowDelay = 50;
        liveTip.ShowAlways = true;
        // 
        // frmCustomerMaster
        // 
        AutoScaleDimensions = new SizeF(11F, 24F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(240, 242, 245);
        ClientSize = new Size(1432, 894);
        Controls.Add(pnlGrid);
        Controls.Add(pnlSummary);
        Controls.Add(pnlFilters);
        Controls.Add(gbToolbar);
        Controls.Add(pnlFields);
        Controls.Add(lblMode);
        Font = new Font("Tahoma", 10F);
        KeyPreview = true;
        MinimumSize = new Size(1200, 700);
        Name = "frmCustomerMaster";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterParent;
        Text = "دليل العملاء";
        Load += frmCustomerMaster_Load;
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
        ((System.ComponentModel.ISupportInitialize)dgvCustomers).EndInit();
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
    private System.Windows.Forms.Button btnShowCard;
    private System.Windows.Forms.Button btnFindCustomer;
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
    private System.Windows.Forms.Button btnToggleFilters;
    private System.Windows.Forms.Button btnClearFilters;
    private System.Windows.Forms.Panel pnlSummary;
    private System.Windows.Forms.GroupBox gbSummary;
    private System.Windows.Forms.Label lblSummary;
    private System.Windows.Forms.Panel pnlGrid;
    private System.Windows.Forms.GroupBox gbGrid;
    private System.Windows.Forms.Label lblEmptyState;
    private System.Windows.Forms.DataGridView dgvCustomers;
    private System.Windows.Forms.ContextMenuStrip cmsGrid;
    private System.Windows.Forms.ToolStripMenuItem cmsEdit;
    private System.Windows.Forms.ToolStripMenuItem cmsShowCard;
    private System.Windows.Forms.ToolStripMenuItem cmsDelete;
    private System.Windows.Forms.ToolStripMenuItem cmsReactivate;
    private System.Windows.Forms.ToolStripMenuItem cmsFindCustomer;
    private System.Windows.Forms.ToolStripSeparator cmsSeparator;
    private System.Windows.Forms.ToolStripMenuItem cmsRefresh;
    private System.Windows.Forms.ToolStripMenuItem cmsExport;
    private System.Windows.Forms.ErrorProvider errorProvider;
    private System.Windows.Forms.ToolTip liveTip;
}
