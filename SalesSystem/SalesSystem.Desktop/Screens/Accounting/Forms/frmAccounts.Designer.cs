namespace SalesSystem.Desktop.Screens.Accounting.Forms;

partial class frmAccounts
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
        pnlHeader = new Panel();
        lblMode = new Label();
        gbCommands = new GroupBox();
        pnlToolbar = new FlowLayoutPanel();
        btnNew = new Button();
        btnSave = new Button();
        btnEdit = new Button();
        btnDelete = new Button();
        btnPermanentDelete = new Button();
        btnRefresh = new Button();
        btnClose = new Button();
        pnlFields = new GroupBox();
        lblAccountCode = new Label();
        txtAccountCode = new TextBox();
        lblNameAr = new Label();
        lblNameArReq = new Label();
        txtNameAr = new TextBox();
        lblNameEn = new Label();
        txtNameEn = new TextBox();
        lblNature = new Label();
        cbNature = new ComboBox();
        lblParent = new Label();
        cbParent = new ComboBox();
        lblOpeningBalance = new Label();
        numOpeningBalance = new NumericUpDown();
        chkIsLeaf = new CheckBox();
        chkIsSystem = new CheckBox();
        lblDescription = new Label();
        txtDescription = new TextBox();
        lblNotes = new Label();
        txtNotes = new TextBox();
        pnlFilters = new Panel();
        gbFilter = new GroupBox();
        lblFilterBy = new Label();
        cbFilterBy = new ComboBox();
        txtFilterValue = new TextBox();
        lblNatureFilter = new Label();
        cbNatureFilter = new ComboBox();
        chkShowInactive = new CheckBox();
        chkOnlyLeaf = new CheckBox();
        chkOnlySystem = new CheckBox();
        chkOnlyParents = new CheckBox();
        btnClearFilters = new Button();
        btnToggleFilters = new Button();
        pnlSummary = new GroupBox();
        lblSummary = new Label();
        pnlGrid = new GroupBox();
        lblEmptyState = new Label();
        dgvAccounts = new DataGridView();
        cmsGrid = new ContextMenuStrip(components);
        cmsEdit = new ToolStripMenuItem();
        cmsDelete = new ToolStripMenuItem();
        cmsRefresh = new ToolStripMenuItem();
        cmsTree = new ToolStripMenuItem();
        errorProvider = new ErrorProvider(components);
        toolTip = new ToolTip(components);
        liveTip = new ToolTip(components);
        pnlHeader.SuspendLayout();
        gbCommands.SuspendLayout();
        pnlToolbar.SuspendLayout();
        pnlFields.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numOpeningBalance).BeginInit();
        pnlFilters.SuspendLayout();
        gbFilter.SuspendLayout();
        pnlSummary.SuspendLayout();
        pnlGrid.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvAccounts).BeginInit();
        cmsGrid.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
        SuspendLayout();
        // 
        // pnlHeader
        // 
        pnlHeader.BackColor = Color.FromArgb(16, 150, 96);
        pnlHeader.Controls.Add(lblMode);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Location = new Point(0, 0);
        pnlHeader.Name = "pnlHeader";
        pnlHeader.Size = new Size(1432, 56);
        pnlHeader.TabIndex = 0;
        // 
        // lblMode
        // 
        lblMode.BackColor = Color.FromArgb(16, 150, 96);
        lblMode.Dock = DockStyle.Fill;
        lblMode.Font = new Font("Tahoma", 14F, FontStyle.Bold);
        lblMode.ForeColor = Color.White;
        lblMode.Location = new Point(0, 0);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(1432, 56);
        lblMode.TabIndex = 0;
        lblMode.Text = "دليل الحسابات — إضافة حساب جديد";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // gbCommands
        // 
        gbCommands.BackColor = Color.White;
        gbCommands.Controls.Add(pnlToolbar);
        gbCommands.Dock = DockStyle.Top;
        gbCommands.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        gbCommands.Location = new Point(0, 56);
        gbCommands.Name = "gbCommands";
        gbCommands.Size = new Size(1432, 82);
        gbCommands.TabIndex = 1;
        gbCommands.TabStop = false;
        gbCommands.Text = "أوامر الشاشة";
        // 
        // pnlToolbar
        // 
        pnlToolbar.BackColor = Color.White;
        pnlToolbar.Controls.Add(btnNew);
        pnlToolbar.Controls.Add(btnSave);
        pnlToolbar.Controls.Add(btnEdit);
        pnlToolbar.Controls.Add(btnDelete);
        pnlToolbar.Controls.Add(btnPermanentDelete);
        pnlToolbar.Controls.Add(btnRefresh);
        pnlToolbar.Controls.Add(btnClose);
        pnlToolbar.Dock = DockStyle.Fill;
        pnlToolbar.Location = new Point(3, 28);
        pnlToolbar.Name = "pnlToolbar";
        pnlToolbar.Padding = new Padding(8, 4, 8, 0);
        pnlToolbar.Size = new Size(1426, 51);
        pnlToolbar.TabIndex = 0;
        pnlToolbar.WrapContents = false;
        // 
        // btnNew
        // 
        btnNew.BackColor = Color.FromArgb(40, 167, 69);
        btnNew.FlatStyle = FlatStyle.Flat;
        btnNew.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnNew.ForeColor = Color.White;
        btnNew.Location = new Point(1276, 8);
        btnNew.Margin = new Padding(4);
        btnNew.Name = "btnNew";
        btnNew.Size = new Size(130, 38);
        btnNew.TabIndex = 0;
        btnNew.Text = "جديد";
        btnNew.UseVisualStyleBackColor = false;
        btnNew.Click += btnNew_Click;
        // 
        // btnSave
        // 
        btnSave.BackColor = Color.FromArgb(0, 123, 255);
        btnSave.FlatStyle = FlatStyle.Flat;
        btnSave.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnSave.ForeColor = Color.White;
        btnSave.Location = new Point(1138, 8);
        btnSave.Margin = new Padding(4);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(130, 38);
        btnSave.TabIndex = 1;
        btnSave.Text = "حفظ";
        btnSave.UseVisualStyleBackColor = false;
        btnSave.Click += btnSave_Click;
        // 
        // btnEdit
        // 
        btnEdit.BackColor = Color.FromArgb(255, 152, 0);
        btnEdit.FlatStyle = FlatStyle.Flat;
        btnEdit.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnEdit.ForeColor = Color.White;
        btnEdit.Location = new Point(1000, 8);
        btnEdit.Margin = new Padding(4);
        btnEdit.Name = "btnEdit";
        btnEdit.Size = new Size(130, 38);
        btnEdit.TabIndex = 2;
        btnEdit.Text = "تعديل";
        btnEdit.UseVisualStyleBackColor = false;
        btnEdit.Click += btnEdit_Click;
        // 
        // btnDelete
        // 
        btnDelete.BackColor = Color.FromArgb(220, 53, 69);
        btnDelete.FlatStyle = FlatStyle.Flat;
        btnDelete.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnDelete.ForeColor = Color.White;
        btnDelete.Location = new Point(862, 8);
        btnDelete.Margin = new Padding(4);
        btnDelete.Name = "btnDelete";
        btnDelete.Size = new Size(130, 38);
        btnDelete.TabIndex = 3;
        btnDelete.Text = "تعطيل/حذف";
        btnDelete.UseVisualStyleBackColor = false;
        btnDelete.Click += btnDelete_Click;
        // 
        // btnPermanentDelete
        // 
        btnPermanentDelete.BackColor = Color.FromArgb(220, 53, 69);
        btnPermanentDelete.FlatStyle = FlatStyle.Flat;
        btnPermanentDelete.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnPermanentDelete.ForeColor = Color.White;
        btnPermanentDelete.Location = new Point(724, 8);
        btnPermanentDelete.Margin = new Padding(4);
        btnPermanentDelete.Name = "btnPermanentDelete";
        btnPermanentDelete.Size = new Size(130, 38);
        btnPermanentDelete.TabIndex = 4;
        btnPermanentDelete.Text = "حذف نهائي";
        btnPermanentDelete.UseVisualStyleBackColor = false;
        btnPermanentDelete.Click += btnPermanentDelete_Click;
        // 
        // btnRefresh
        // 
        btnRefresh.BackColor = Color.FromArgb(23, 162, 184);
        btnRefresh.FlatStyle = FlatStyle.Flat;
        btnRefresh.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnRefresh.ForeColor = Color.White;
        btnRefresh.Location = new Point(586, 8);
        btnRefresh.Margin = new Padding(4);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(130, 38);
        btnRefresh.TabIndex = 5;
        btnRefresh.Text = "تحديث";
        btnRefresh.UseVisualStyleBackColor = false;
        btnRefresh.Click += btnRefresh_Click;
        // 
        // btnClose
        // 
        btnClose.BackColor = Color.FromArgb(108, 117, 125);
        btnClose.FlatStyle = FlatStyle.Flat;
        btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnClose.ForeColor = Color.White;
        btnClose.Location = new Point(448, 8);
        btnClose.Margin = new Padding(4);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(130, 38);
        btnClose.TabIndex = 6;
        btnClose.Text = "إغلاق";
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += btnClose_Click;
        // 
        // pnlFields
        // 
        pnlFields.AutoSize = true;
        pnlFields.BackColor = Color.White;
        pnlFields.Controls.Add(lblAccountCode);
        pnlFields.Controls.Add(txtAccountCode);
        pnlFields.Controls.Add(lblNameAr);
        pnlFields.Controls.Add(lblNameArReq);
        pnlFields.Controls.Add(txtNameAr);
        pnlFields.Controls.Add(lblNameEn);
        pnlFields.Controls.Add(txtNameEn);
        pnlFields.Controls.Add(lblNature);
        pnlFields.Controls.Add(cbNature);
        pnlFields.Controls.Add(lblParent);
        pnlFields.Controls.Add(cbParent);
        pnlFields.Controls.Add(lblOpeningBalance);
        pnlFields.Controls.Add(numOpeningBalance);
        pnlFields.Controls.Add(chkIsLeaf);
        pnlFields.Controls.Add(chkIsSystem);
        pnlFields.Controls.Add(lblDescription);
        pnlFields.Controls.Add(txtDescription);
        pnlFields.Controls.Add(lblNotes);
        pnlFields.Controls.Add(txtNotes);
        pnlFields.Dock = DockStyle.Top;
        pnlFields.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        pnlFields.Location = new Point(0, 138);
        pnlFields.Name = "pnlFields";
        pnlFields.Size = new Size(1432, 233);
        pnlFields.TabIndex = 2;
        pnlFields.TabStop = false;
        pnlFields.Text = "بيانات الحساب";
        // 
        // lblAccountCode
        // 
        lblAccountCode.AutoSize = true;
        lblAccountCode.Location = new Point(291, 19);
        lblAccountCode.Name = "lblAccountCode";
        lblAccountCode.Size = new Size(124, 24);
        lblAccountCode.TabIndex = 0;
        lblAccountCode.Text = "رمز الحساب";
        // 
        // txtAccountCode
        // 
        txtAccountCode.BackColor = Color.White;
        txtAccountCode.Font = new Font("Tahoma", 10F);
        txtAccountCode.Location = new Point(243, 46);
        txtAccountCode.Name = "txtAccountCode";
        txtAccountCode.ReadOnly = true;
        txtAccountCode.Size = new Size(220, 32);
        txtAccountCode.TabIndex = 1;
        txtAccountCode.TabStop = false;
        // 
        // lblNameAr
        // 
        lblNameAr.AutoSize = true;
        lblNameAr.Location = new Point(736, 19);
        lblNameAr.Name = "lblNameAr";
        lblNameAr.Size = new Size(138, 24);
        lblNameAr.TabIndex = 2;
        lblNameAr.Text = "الاسم العربي";
        // 
        // lblNameArReq
        // 
        lblNameArReq.AutoSize = true;
        lblNameArReq.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblNameArReq.ForeColor = Color.Red;
        lblNameArReq.Location = new Point(720, 19);
        lblNameArReq.Name = "lblNameArReq";
        lblNameArReq.Size = new Size(23, 24);
        lblNameArReq.TabIndex = 3;
        lblNameArReq.Text = "*";
        // 
        // txtNameAr
        // 
        txtNameAr.BackColor = Color.White;
        txtNameAr.Font = new Font("Tahoma", 10F);
        txtNameAr.Location = new Point(695, 46);
        txtNameAr.Name = "txtNameAr";
        txtNameAr.Size = new Size(220, 32);
        txtNameAr.TabIndex = 2;
        // 
        // lblNameEn
        // 
        lblNameEn.AutoSize = true;
        lblNameEn.Location = new Point(499, 19);
        lblNameEn.Name = "lblNameEn";
        lblNameEn.Size = new Size(160, 24);
        lblNameEn.TabIndex = 5;
        lblNameEn.Text = "الاسم الإنجليزي";
        // 
        // txtNameEn
        // 
        txtNameEn.BackColor = Color.White;
        txtNameEn.Font = new Font("Tahoma", 10F);
        txtNameEn.Location = new Point(469, 46);
        txtNameEn.Name = "txtNameEn";
        txtNameEn.Size = new Size(220, 32);
        txtNameEn.TabIndex = 3;
        // 
        // lblNature
        // 
        lblNature.AutoSize = true;
        lblNature.Location = new Point(969, 19);
        lblNature.Name = "lblNature";
        lblNature.Size = new Size(125, 24);
        lblNature.TabIndex = 7;
        lblNature.Text = "نوع الحساب";
        // 
        // cbNature
        // 
        cbNature.BackColor = Color.White;
        cbNature.DropDownStyle = ComboBoxStyle.DropDownList;
        cbNature.Font = new Font("Tahoma", 10F);
        cbNature.Location = new Point(921, 46);
        cbNature.Name = "cbNature";
        cbNature.Size = new Size(220, 32);
        cbNature.TabIndex = 4;
        // 
        // lblParent
        // 
        lblParent.AutoSize = true;
        lblParent.Location = new Point(287, 81);
        lblParent.Name = "lblParent";
        lblParent.Size = new Size(132, 24);
        lblParent.TabIndex = 9;
        lblParent.Text = "الحساب الأب";
        // 
        // cbParent
        // 
        cbParent.BackColor = Color.White;
        cbParent.DropDownStyle = ComboBoxStyle.DropDownList;
        cbParent.Font = new Font("Tahoma", 10F);
        cbParent.Location = new Point(243, 108);
        cbParent.Name = "cbParent";
        cbParent.Size = new Size(220, 32);
        cbParent.TabIndex = 5;
        // 
        // lblOpeningBalance
        // 
        lblOpeningBalance.AutoSize = true;
        lblOpeningBalance.Location = new Point(494, 81);
        lblOpeningBalance.Name = "lblOpeningBalance";
        lblOpeningBalance.Size = new Size(170, 24);
        lblOpeningBalance.TabIndex = 11;
        lblOpeningBalance.Text = "الرصيد الافتتاحي";
        lblOpeningBalance.Click += lblOpeningBalance_Click;
        // 
        // numOpeningBalance
        // 
        numOpeningBalance.BackColor = Color.White;
        numOpeningBalance.DecimalPlaces = 2;
        numOpeningBalance.Font = new Font("Tahoma", 10F);
        numOpeningBalance.Location = new Point(469, 108);
        numOpeningBalance.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 });
        numOpeningBalance.Name = "numOpeningBalance";
        numOpeningBalance.Size = new Size(220, 32);
        numOpeningBalance.TabIndex = 6;
        numOpeningBalance.ThousandsSeparator = true;
        // 
        // chkIsLeaf
        // 
        chkIsLeaf.AutoSize = true;
        chkIsLeaf.Location = new Point(695, 114);
        chkIsLeaf.Name = "chkIsLeaf";
        chkIsLeaf.Size = new Size(289, 28);
        chkIsLeaf.TabIndex = 7;
        chkIsLeaf.Text = "حساب فرعي قابل للترحيل";
        chkIsLeaf.UseVisualStyleBackColor = false;
        // 
        // chkIsSystem
        // 
        chkIsSystem.AutoSize = true;
        chkIsSystem.Location = new Point(990, 113);
        chkIsSystem.Name = "chkIsSystem";
        chkIsSystem.Size = new Size(172, 28);
        chkIsSystem.TabIndex = 8;
        chkIsSystem.Text = "حساب نظامي";
        chkIsSystem.UseVisualStyleBackColor = false;
        // 
        // lblDescription
        // 
        lblDescription.AutoSize = true;
        lblDescription.Location = new Point(429, 141);
        lblDescription.Name = "lblDescription";
        lblDescription.Size = new Size(74, 24);
        lblDescription.TabIndex = 15;
        lblDescription.Text = "الوصف";
        // 
        // txtDescription
        // 
        txtDescription.BackColor = Color.White;
        txtDescription.Font = new Font("Tahoma", 10F);
        txtDescription.Location = new Point(243, 170);
        txtDescription.Name = "txtDescription";
        txtDescription.Size = new Size(446, 32);
        txtDescription.TabIndex = 9;
        // 
        // lblNotes
        // 
        lblNotes.AutoSize = true;
        lblNotes.Location = new Point(871, 141);
        lblNotes.Name = "lblNotes";
        lblNotes.Size = new Size(95, 24);
        lblNotes.TabIndex = 17;
        lblNotes.Text = "ملاحظات";
        // 
        // txtNotes
        // 
        txtNotes.BackColor = Color.White;
        txtNotes.Font = new Font("Tahoma", 10F);
        txtNotes.Location = new Point(695, 170);
        txtNotes.Name = "txtNotes";
        txtNotes.Size = new Size(446, 32);
        txtNotes.TabIndex = 10;
        // 
        // pnlFilters
        // 
        pnlFilters.BackColor = Color.White;
        pnlFilters.Controls.Add(gbFilter);
        pnlFilters.Controls.Add(btnToggleFilters);
        pnlFilters.Dock = DockStyle.Top;
        pnlFilters.Location = new Point(0, 371);
        pnlFilters.Name = "pnlFilters";
        pnlFilters.Padding = new Padding(8, 4, 8, 4);
        pnlFilters.Size = new Size(1432, 104);
        pnlFilters.TabIndex = 3;
        // 
        // gbFilter
        // 
        gbFilter.BackColor = Color.White;
        gbFilter.Controls.Add(lblFilterBy);
        gbFilter.Controls.Add(cbFilterBy);
        gbFilter.Controls.Add(txtFilterValue);
        gbFilter.Controls.Add(lblNatureFilter);
        gbFilter.Controls.Add(cbNatureFilter);
        gbFilter.Controls.Add(chkShowInactive);
        gbFilter.Controls.Add(chkOnlyLeaf);
        gbFilter.Controls.Add(chkOnlySystem);
        gbFilter.Controls.Add(chkOnlyParents);
        gbFilter.Controls.Add(btnClearFilters);
        gbFilter.Font = new Font("Tahoma", 10F);
        gbFilter.Location = new Point(148, 4);
        gbFilter.Name = "gbFilter";
        gbFilter.Padding = new Padding(8);
        gbFilter.Size = new Size(1276, 96);
        gbFilter.TabIndex = 0;
        gbFilter.TabStop = false;
        gbFilter.Text = "بحث وتصفية";
        // 
        // lblFilterBy
        // 
        lblFilterBy.AutoSize = true;
        lblFilterBy.Location = new Point(993, 24);
        lblFilterBy.Name = "lblFilterBy";
        lblFilterBy.Size = new Size(83, 24);
        lblFilterBy.TabIndex = 0;
        lblFilterBy.Text = "تصفية بـ:";
        // 
        // cbFilterBy
        // 
        cbFilterBy.BackColor = Color.White;
        cbFilterBy.DropDownStyle = ComboBoxStyle.DropDownList;
        cbFilterBy.Font = new Font("Tahoma", 10F);
        cbFilterBy.Location = new Point(786, 21);
        cbFilterBy.Name = "cbFilterBy";
        cbFilterBy.Size = new Size(200, 32);
        cbFilterBy.TabIndex = 1;
        cbFilterBy.SelectedIndexChanged += cbFilterBy_SelectedIndexChanged;
        // 
        // txtFilterValue
        // 
        txtFilterValue.BackColor = Color.White;
        txtFilterValue.Font = new Font("Tahoma", 10F);
        txtFilterValue.Location = new Point(540, 21);
        txtFilterValue.Name = "txtFilterValue";
        txtFilterValue.PlaceholderText = "اكتب نص البحث...";
        txtFilterValue.Size = new Size(240, 32);
        txtFilterValue.TabIndex = 2;
        txtFilterValue.TextChanged += txtFilterValue_TextChanged;
        txtFilterValue.KeyDown += txtFilterValue_KeyDown;
        // 
        // lblNatureFilter
        // 
        lblNatureFilter.AutoSize = true;
        lblNatureFilter.Location = new Point(478, 25);
        lblNatureFilter.Name = "lblNatureFilter";
        lblNatureFilter.Size = new Size(56, 24);
        lblNatureFilter.TabIndex = 3;
        lblNatureFilter.Text = "النوع:";
        // 
        // cbNatureFilter
        // 
        cbNatureFilter.BackColor = Color.White;
        cbNatureFilter.DropDownStyle = ComboBoxStyle.DropDownList;
        cbNatureFilter.Font = new Font("Tahoma", 10F);
        cbNatureFilter.Location = new Point(272, 21);
        cbNatureFilter.Name = "cbNatureFilter";
        cbNatureFilter.Size = new Size(200, 32);
        cbNatureFilter.TabIndex = 4;
        cbNatureFilter.SelectedIndexChanged += cbNatureFilter_SelectedIndexChanged;
        // 
        // chkShowInactive
        // 
        chkShowInactive.AutoSize = true;
        chkShowInactive.Location = new Point(163, 61);
        chkShowInactive.Name = "chkShowInactive";
        chkShowInactive.Size = new Size(170, 28);
        chkShowInactive.TabIndex = 5;
        chkShowInactive.Text = "إظهار غير النشط";
        chkShowInactive.UseVisualStyleBackColor = false;
        chkShowInactive.CheckedChanged += chkShowInactive_CheckedChanged;
        // 
        // chkOnlyLeaf
        // 
        chkOnlyLeaf.AutoSize = true;
        chkOnlyLeaf.Location = new Point(339, 61);
        chkOnlyLeaf.Name = "chkOnlyLeaf";
        chkOnlyLeaf.Size = new Size(219, 28);
        chkOnlyLeaf.TabIndex = 6;
        chkOnlyLeaf.Text = "الحسابات الفرعية فقط";
        chkOnlyLeaf.UseVisualStyleBackColor = false;
        chkOnlyLeaf.CheckedChanged += chkOnlyLeaf_CheckedChanged;
        // 
        // chkOnlySystem
        // 
        chkOnlySystem.AutoSize = true;
        chkOnlySystem.Location = new Point(564, 61);
        chkOnlySystem.Name = "chkOnlySystem";
        chkOnlySystem.Size = new Size(198, 28);
        chkOnlySystem.TabIndex = 7;
        chkOnlySystem.Text = "حسابات النظام فقط";
        chkOnlySystem.UseVisualStyleBackColor = false;
        chkOnlySystem.CheckedChanged += chkOnlySystem_CheckedChanged;
        // 
        // chkOnlyParents
        // 
        chkOnlyParents.AutoSize = true;
        chkOnlyParents.Location = new Point(768, 61);
        chkOnlyParents.Name = "chkOnlyParents";
        chkOnlyParents.Size = new Size(231, 28);
        chkOnlyParents.TabIndex = 8;
        chkOnlyParents.Text = "الحسابات الرئيسية فقط";
        chkOnlyParents.UseVisualStyleBackColor = false;
        chkOnlyParents.CheckedChanged += chkOnlyParents_CheckedChanged;
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
        btnClearFilters.Location = new Point(77, 21);
        btnClearFilters.Name = "btnClearFilters";
        btnClearFilters.Size = new Size(184, 32);
        btnClearFilters.TabIndex = 21;
        btnClearFilters.Text = "\U0001f9f9 مسح التصفية";
        toolTip.SetToolTip(btnClearFilters, "مسح كل أدوات التصفية (Esc) — لا يعيد تحميل البيانات");
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
        btnToggleFilters.Location = new Point(8, 4);
        btnToggleFilters.Name = "btnToggleFilters";
        btnToggleFilters.Size = new Size(140, 96);
        btnToggleFilters.TabIndex = 20;
        btnToggleFilters.Text = "إخفاء التصفية";
        btnToggleFilters.UseVisualStyleBackColor = false;
        // 
        // pnlSummary
        // 
        pnlSummary.BackColor = Color.White;
        pnlSummary.Controls.Add(lblSummary);
        pnlSummary.Dock = DockStyle.Bottom;
        pnlSummary.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        pnlSummary.Location = new Point(0, 748);
        pnlSummary.Name = "pnlSummary";
        pnlSummary.Padding = new Padding(8, 18, 8, 4);
        pnlSummary.Size = new Size(1432, 52);
        pnlSummary.TabIndex = 5;
        pnlSummary.TabStop = false;
        pnlSummary.Text = "الإجماليات";
        // 
        // lblSummary
        // 
        lblSummary.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblSummary.ForeColor = Color.FromArgb(33, 37, 41);
        lblSummary.Location = new Point(8, 19);
        lblSummary.Name = "lblSummary";
        lblSummary.Size = new Size(1416, 29);
        lblSummary.TabIndex = 0;
        lblSummary.Text = "المعروض: 0 من 0";
        lblSummary.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlGrid
        // 
        pnlGrid.BackColor = Color.White;
        pnlGrid.Controls.Add(lblEmptyState);
        pnlGrid.Controls.Add(dgvAccounts);
        pnlGrid.Dock = DockStyle.Fill;
        pnlGrid.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        pnlGrid.Location = new Point(0, 475);
        pnlGrid.Name = "pnlGrid";
        pnlGrid.Padding = new Padding(8);
        pnlGrid.Size = new Size(1432, 273);
        pnlGrid.TabIndex = 4;
        pnlGrid.TabStop = false;
        pnlGrid.Text = "البيانات";
        // 
        // lblEmptyState
        // 
        lblEmptyState.BackColor = Color.White;
        lblEmptyState.Dock = DockStyle.Fill;
        lblEmptyState.Font = new Font("Tahoma", 12F, FontStyle.Bold);
        lblEmptyState.ForeColor = Color.FromArgb(33, 37, 41);
        lblEmptyState.Location = new Point(8, 33);
        lblEmptyState.Name = "lblEmptyState";
        lblEmptyState.Size = new Size(1416, 232);
        lblEmptyState.TabIndex = 1;
        lblEmptyState.Text = "لا توجد بيانات";
        lblEmptyState.TextAlign = ContentAlignment.MiddleCenter;
        lblEmptyState.Visible = false;
        // 
        // dgvAccounts
        // 
        dgvAccounts.AllowUserToAddRows = false;
        dgvAccounts.AllowUserToDeleteRows = false;
        dgvAccounts.AllowUserToOrderColumns = true;
        dataGridViewCellStyle1.BackColor = Color.FromArgb(248, 249, 250);
        dgvAccounts.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
        dgvAccounts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvAccounts.BackgroundColor = Color.White;
        dgvAccounts.BorderStyle = BorderStyle.None;
        dgvAccounts.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
        dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle2.BackColor = Color.FromArgb(52, 58, 64);
        dataGridViewCellStyle2.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        dataGridViewCellStyle2.ForeColor = Color.White;
        dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(52, 58, 64);
        dataGridViewCellStyle2.SelectionForeColor = Color.White;
        dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
        dgvAccounts.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
        dgvAccounts.ColumnHeadersHeight = 45;
        dgvAccounts.ContextMenuStrip = cmsGrid;
        dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle3.BackColor = Color.White;
        dataGridViewCellStyle3.Font = new Font("Tahoma", 10F);
        dataGridViewCellStyle3.ForeColor = Color.FromArgb(33, 37, 41);
        dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(255, 243, 205);
        dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(33, 37, 41);
        dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
        dgvAccounts.DefaultCellStyle = dataGridViewCellStyle3;
        dgvAccounts.Dock = DockStyle.Fill;
        dgvAccounts.EnableHeadersVisualStyles = false;
        dgvAccounts.GridColor = Color.FromArgb(222, 226, 230);
        dgvAccounts.Location = new Point(8, 33);
        dgvAccounts.MultiSelect = false;
        dgvAccounts.Name = "dgvAccounts";
        dgvAccounts.ReadOnly = true;
        dgvAccounts.RowHeadersVisible = false;
        dgvAccounts.RowHeadersWidth = 62;
        dgvAccounts.RowTemplate.Height = 32;
        dgvAccounts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvAccounts.Size = new Size(1416, 232);
        dgvAccounts.TabIndex = 0;
        dgvAccounts.CellDoubleClick += dgvAccounts_CellDoubleClick;
        dgvAccounts.CellFormatting += dgvAccounts_CellFormatting;
        dgvAccounts.SelectionChanged += dgvAccounts_SelectionChanged;
        // 
        // cmsGrid
        // 
        cmsGrid.Font = new Font("Tahoma", 10F);
        cmsGrid.ImageScalingSize = new Size(24, 24);
        cmsGrid.Items.AddRange(new ToolStripItem[] { cmsEdit, cmsDelete, cmsRefresh, cmsTree });
        cmsGrid.Name = "cmsGrid";
        cmsGrid.RightToLeft = RightToLeft.Yes;
        cmsGrid.Size = new Size(193, 124);
        // 
        // cmsEdit
        // 
        cmsEdit.Name = "cmsEdit";
        cmsEdit.Size = new Size(192, 30);
        cmsEdit.Text = "تعديل";
        cmsEdit.Click += cmsEdit_Click;
        // 
        // cmsDelete
        // 
        cmsDelete.Name = "cmsDelete";
        cmsDelete.Size = new Size(192, 30);
        cmsDelete.Text = "تعطيل";
        cmsDelete.Click += cmsDelete_Click;
        // 
        // cmsRefresh
        // 
        cmsRefresh.Name = "cmsRefresh";
        cmsRefresh.Size = new Size(192, 30);
        cmsRefresh.Text = "تحديث";
        cmsRefresh.Click += cmsRefresh_Click;
        // 
        // cmsTree
        // 
        cmsTree.Name = "cmsTree";
        cmsTree.Size = new Size(192, 30);
        cmsTree.Text = "عرض الشجرة";
        cmsTree.Click += cmsTree_Click;
        // 
        // errorProvider
        // 
        errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        errorProvider.ContainerControl = this;
        errorProvider.RightToLeft = true;
        // 
        // frmAccounts
        // 
        AutoScaleDimensions = new SizeF(11F, 24F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(240, 242, 245);
        ClientSize = new Size(1432, 800);
        Controls.Add(pnlGrid);
        Controls.Add(pnlSummary);
        Controls.Add(pnlFilters);
        Controls.Add(pnlFields);
        Controls.Add(gbCommands);
        Controls.Add(pnlHeader);
        Font = new Font("Tahoma", 10F);
        Name = "frmAccounts";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterParent;
        Text = "دليل الحسابات";
        Load += frmAccounts_Load;
        pnlHeader.ResumeLayout(false);
        gbCommands.ResumeLayout(false);
        pnlToolbar.ResumeLayout(false);
        pnlFields.ResumeLayout(false);
        pnlFields.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numOpeningBalance).EndInit();
        pnlFilters.ResumeLayout(false);
        gbFilter.ResumeLayout(false);
        gbFilter.PerformLayout();
        pnlSummary.ResumeLayout(false);
        pnlGrid.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvAccounts).EndInit();
        cmsGrid.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    private System.Windows.Forms.Panel pnlHeader;
    private System.Windows.Forms.Label lblMode;
    private System.Windows.Forms.GroupBox gbCommands;
    private System.Windows.Forms.FlowLayoutPanel pnlToolbar;
    private System.Windows.Forms.Button btnNew;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.Button btnEdit;
    private System.Windows.Forms.Button btnDelete;
    private System.Windows.Forms.Button btnPermanentDelete;
    private System.Windows.Forms.Button btnRefresh;
    private System.Windows.Forms.Button btnClose;
    private System.Windows.Forms.GroupBox pnlFields;
    private System.Windows.Forms.Label lblAccountCode;
    private System.Windows.Forms.TextBox txtAccountCode;
    private System.Windows.Forms.Label lblNameAr;
    private System.Windows.Forms.Label lblNameArReq;
    private System.Windows.Forms.TextBox txtNameAr;
    private System.Windows.Forms.Label lblNameEn;
    private System.Windows.Forms.TextBox txtNameEn;
    private System.Windows.Forms.Label lblNature;
    private System.Windows.Forms.ComboBox cbNature;
    private System.Windows.Forms.Label lblParent;
    private System.Windows.Forms.ComboBox cbParent;
    private System.Windows.Forms.Label lblOpeningBalance;
    private System.Windows.Forms.NumericUpDown numOpeningBalance;
    private System.Windows.Forms.CheckBox chkIsLeaf;
    private System.Windows.Forms.CheckBox chkIsSystem;
    private System.Windows.Forms.Label lblDescription;
    private System.Windows.Forms.TextBox txtDescription;
    private System.Windows.Forms.Label lblNotes;
    private System.Windows.Forms.TextBox txtNotes;
    private System.Windows.Forms.Panel pnlFilters;
    private System.Windows.Forms.GroupBox gbFilter;
    private System.Windows.Forms.Button btnToggleFilters;
    private System.Windows.Forms.Button btnClearFilters;
    private System.Windows.Forms.Label lblFilterBy;
    private System.Windows.Forms.ComboBox cbFilterBy;
    private System.Windows.Forms.TextBox txtFilterValue;
    private System.Windows.Forms.Label lblNatureFilter;
    private System.Windows.Forms.ComboBox cbNatureFilter;
    private System.Windows.Forms.CheckBox chkShowInactive;
    private System.Windows.Forms.CheckBox chkOnlyLeaf;
    private System.Windows.Forms.CheckBox chkOnlySystem;
    private System.Windows.Forms.CheckBox chkOnlyParents;
    private System.Windows.Forms.GroupBox pnlSummary;
    private System.Windows.Forms.Label lblSummary;
    private System.Windows.Forms.GroupBox pnlGrid;
    private System.Windows.Forms.Label lblEmptyState;
    private System.Windows.Forms.DataGridView dgvAccounts;
    private System.Windows.Forms.DataGridViewTextBoxColumn colCode;
    private System.Windows.Forms.DataGridViewTextBoxColumn colNameAr;
    private System.Windows.Forms.DataGridViewTextBoxColumn colNameEn;
    private System.Windows.Forms.DataGridViewTextBoxColumn colNature;
    private System.Windows.Forms.DataGridViewTextBoxColumn colLevel;
    private System.Windows.Forms.DataGridViewTextBoxColumn colParent;
    private System.Windows.Forms.DataGridViewTextBoxColumn colIsLeaf;
    private System.Windows.Forms.DataGridViewTextBoxColumn colIsSystem;
    private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
    private System.Windows.Forms.ContextMenuStrip cmsGrid;
    private System.Windows.Forms.ToolStripMenuItem cmsEdit;
    private System.Windows.Forms.ToolStripMenuItem cmsDelete;
    private System.Windows.Forms.ToolStripMenuItem cmsRefresh;
    private System.Windows.Forms.ToolStripMenuItem cmsTree;
    private System.Windows.Forms.ErrorProvider errorProvider;
    private System.Windows.Forms.ToolTip toolTip;
    private System.Windows.Forms.ToolTip liveTip;
}
