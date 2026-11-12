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
        pnlHeader = new Panel();
        lblMode = new Label();
        pnlToolbar = new FlowLayoutPanel();
        btnNew = new Button();
        btnSave = new Button();
        btnEdit = new Button();
        btnDelete = new Button();
        btnPermanentDelete = new Button();
        btnRefresh = new Button();
        btnClose = new Button();
        pnlFields = new Panel();
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
        pnlSummary = new Panel();
        lblSummary = new Label();
        pnlGrid = new Panel();
        lblEmptyState = new Label();
        dgvAccounts = new DataGridView();
        colCode = new DataGridViewTextBoxColumn();
        colNameAr = new DataGridViewTextBoxColumn();
        colNameEn = new DataGridViewTextBoxColumn();
        colNature = new DataGridViewTextBoxColumn();
        colLevel = new DataGridViewTextBoxColumn();
        colParent = new DataGridViewTextBoxColumn();
        colIsLeaf = new DataGridViewTextBoxColumn();
        colIsSystem = new DataGridViewTextBoxColumn();
        colStatus = new DataGridViewTextBoxColumn();
        cmsGrid = new ContextMenuStrip(components);
        cmsEdit = new ToolStripMenuItem();
        cmsDelete = new ToolStripMenuItem();
        cmsRefresh = new ToolStripMenuItem();
        cmsTree = new ToolStripMenuItem();
        errorProvider = new ErrorProvider(components);
        toolTip = new ToolTip(components);
        liveTip = new ToolTip(components);
        pnlHeader.SuspendLayout();
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
        pnlHeader.BackColor = Color.FromArgb(14, 165, 103);
        pnlHeader.Controls.Add(lblMode);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Location = new Point(0, 0);
        pnlHeader.Name = "pnlHeader";
        pnlHeader.Size = new Size(1432, 56);
        pnlHeader.TabIndex = 0;
        // 
        // lblMode
        // 
        lblMode.BackColor = Color.FromArgb(14, 165, 103);
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
        // pnlToolbar
        // 
        pnlToolbar.BackColor = Color.FromArgb(235, 240, 235);
        pnlToolbar.Controls.Add(btnNew);
        pnlToolbar.Controls.Add(btnSave);
        pnlToolbar.Controls.Add(btnEdit);
        pnlToolbar.Controls.Add(btnDelete);
        pnlToolbar.Controls.Add(btnPermanentDelete);
        pnlToolbar.Controls.Add(btnRefresh);
        pnlToolbar.Controls.Add(btnClose);
        pnlToolbar.Dock = DockStyle.Top;
        pnlToolbar.FlowDirection = FlowDirection.RightToLeft;
        pnlToolbar.Location = new Point(0, 56);
        pnlToolbar.Name = "pnlToolbar";
        pnlToolbar.Padding = new Padding(8, 0, 8, 0);
        pnlToolbar.Size = new Size(1432, 60);
        pnlToolbar.TabIndex = 1;
        pnlToolbar.WrapContents = false;
        // 
        // btnNew
        // 
        btnNew.BackColor = Color.FromArgb(40, 167, 69);
        btnNew.FlatStyle = FlatStyle.Flat;
        btnNew.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnNew.ForeColor = Color.White;
        btnNew.Location = new Point(1262, 11);
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
        btnSave.Location = new Point(1124, 11);
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
        btnEdit.Location = new Point(986, 11);
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
        btnDelete.Location = new Point(848, 11);
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
        btnPermanentDelete.BackColor = Color.FromArgb(120, 40, 60);
        btnPermanentDelete.FlatStyle = FlatStyle.Flat;
        btnPermanentDelete.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnPermanentDelete.ForeColor = Color.White;
        btnPermanentDelete.Location = new Point(710, 11);
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
        btnRefresh.Location = new Point(572, 11);
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
        btnClose.Location = new Point(434, 11);
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
        pnlFields.BackColor = Color.FromArgb(235, 245, 235);
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
        pnlFields.Location = new Point(0, 116);
        pnlFields.Name = "pnlFields";
        pnlFields.Size = new Size(1432, 190);
        pnlFields.TabIndex = 2;
        // 
        // lblAccountCode
        // 
        lblAccountCode.AutoSize = true;
        lblAccountCode.Location = new Point(40, 6);
        lblAccountCode.Name = "lblAccountCode";
        lblAccountCode.Size = new Size(105, 24);
        lblAccountCode.TabIndex = 0;
        lblAccountCode.Text = "رمز الحساب";
        // 
        // txtAccountCode
        // 
        txtAccountCode.Location = new Point(40, 28);
        txtAccountCode.Name = "txtAccountCode";
        txtAccountCode.ReadOnly = true;
        txtAccountCode.Size = new Size(220, 28);
        txtAccountCode.TabIndex = 1;
        txtAccountCode.TabStop = false;
        // 
        // lblNameAr
        // 
        lblNameAr.AutoSize = true;
        lblNameAr.Location = new Point(290, 6);
        lblNameAr.Name = "lblNameAr";
        lblNameAr.Size = new Size(110, 24);
        lblNameAr.TabIndex = 2;
        lblNameAr.Text = "الاسم العربي";
        // 
        // lblNameArReq
        // 
        lblNameArReq.AutoSize = true;
        lblNameArReq.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblNameArReq.ForeColor = Color.Red;
        lblNameArReq.Location = new Point(272, 6);
        lblNameArReq.Name = "lblNameArReq";
        lblNameArReq.Size = new Size(18, 24);
        lblNameArReq.TabIndex = 3;
        lblNameArReq.Text = "*";
        // 
        // txtNameAr
        // 
        txtNameAr.Location = new Point(290, 28);
        txtNameAr.Name = "txtNameAr";
        txtNameAr.Size = new Size(220, 28);
        txtNameAr.TabIndex = 2;
        // 
        // lblNameEn
        // 
        lblNameEn.AutoSize = true;
        lblNameEn.Location = new Point(540, 6);
        lblNameEn.Name = "lblNameEn";
        lblNameEn.Size = new Size(120, 24);
        lblNameEn.TabIndex = 5;
        lblNameEn.Text = "الاسم الإنجليزي";
        // 
        // txtNameEn
        // 
        txtNameEn.Location = new Point(540, 28);
        txtNameEn.Name = "txtNameEn";
        txtNameEn.Size = new Size(220, 28);
        txtNameEn.TabIndex = 3;
        // 
        // lblNature
        // 
        lblNature.AutoSize = true;
        lblNature.Location = new Point(790, 6);
        lblNature.Name = "lblNature";
        lblNature.Size = new Size(85, 24);
        lblNature.TabIndex = 7;
        lblNature.Text = "نوع الحساب";
        // 
        // cbNature
        // 
        cbNature.DropDownStyle = ComboBoxStyle.DropDownList;
        cbNature.Font = new Font("Tahoma", 10F);
        cbNature.Location = new Point(790, 28);
        cbNature.Name = "cbNature";
        cbNature.Size = new Size(220, 28);
        cbNature.TabIndex = 4;
        // 
        // lblParent
        // 
        lblParent.AutoSize = true;
        lblParent.Location = new Point(40, 68);
        lblParent.Name = "lblParent";
        lblParent.Size = new Size(110, 24);
        lblParent.TabIndex = 9;
        lblParent.Text = "الحساب الأب";
        // 
        // cbParent
        // 
        cbParent.DropDownStyle = ComboBoxStyle.DropDownList;
        cbParent.Font = new Font("Tahoma", 10F);
        cbParent.Location = new Point(40, 90);
        cbParent.Name = "cbParent";
        cbParent.Size = new Size(250, 28);
        cbParent.TabIndex = 5;
        // 
        // lblOpeningBalance
        // 
        lblOpeningBalance.AutoSize = true;
        lblOpeningBalance.Location = new Point(320, 68);
        lblOpeningBalance.Name = "lblOpeningBalance";
        lblOpeningBalance.Size = new Size(130, 24);
        lblOpeningBalance.TabIndex = 11;
        lblOpeningBalance.Text = "الرصيد الافتتاحي";
        // 
        // numOpeningBalance
        // 
        numOpeningBalance.DecimalPlaces = 2;
        numOpeningBalance.Font = new Font("Tahoma", 10F);
        numOpeningBalance.Location = new Point(320, 90);
        numOpeningBalance.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 });
        numOpeningBalance.Name = "numOpeningBalance";
        numOpeningBalance.Size = new Size(220, 28);
        numOpeningBalance.TabIndex = 6;
        numOpeningBalance.ThousandsSeparator = true;
        // 
        // chkIsLeaf
        // 
        chkIsLeaf.AutoSize = true;
        chkIsLeaf.Location = new Point(580, 94);
        chkIsLeaf.Name = "chkIsLeaf";
        chkIsLeaf.Size = new Size(220, 24);
        chkIsLeaf.TabIndex = 7;
        chkIsLeaf.Text = "حساب فرعي قابل للترحيل";
        chkIsLeaf.UseVisualStyleBackColor = true;
        // 
        // chkIsSystem
        // 
        chkIsSystem.AutoSize = true;
        chkIsSystem.Location = new Point(840, 94);
        chkIsSystem.Name = "chkIsSystem";
        chkIsSystem.Size = new Size(180, 24);
        chkIsSystem.TabIndex = 8;
        chkIsSystem.Text = "حساب نظامي";
        chkIsSystem.UseVisualStyleBackColor = true;
        // 
        // lblDescription
        // 
        lblDescription.AutoSize = true;
        lblDescription.Location = new Point(40, 130);
        lblDescription.Name = "lblDescription";
        lblDescription.Size = new Size(70, 24);
        lblDescription.TabIndex = 15;
        lblDescription.Text = "الوصف";
        // 
        // txtDescription
        // 
        txtDescription.Location = new Point(40, 152);
        txtDescription.Name = "txtDescription";
        txtDescription.Size = new Size(560, 28);
        txtDescription.TabIndex = 9;
        // 
        // lblNotes
        // 
        lblNotes.AutoSize = true;
        lblNotes.Location = new Point(620, 130);
        lblNotes.Name = "lblNotes";
        lblNotes.Size = new Size(80, 24);
        lblNotes.TabIndex = 17;
        lblNotes.Text = "ملاحظات";
        // 
        // txtNotes
        // 
        txtNotes.Location = new Point(620, 152);
        txtNotes.Name = "txtNotes";
        txtNotes.Size = new Size(560, 28);
        txtNotes.TabIndex = 10;
        // 
        // pnlFilters
        // 
        pnlFilters.BackColor = Color.FromArgb(235, 240, 235);
        pnlFilters.Controls.Add(gbFilter);
        pnlFilters.Dock = DockStyle.Top;
        pnlFilters.Location = new Point(0, 306);
        pnlFilters.Name = "pnlFilters";
        pnlFilters.Padding = new Padding(8, 4, 8, 4);
        pnlFilters.Size = new Size(1432, 104);
        pnlFilters.TabIndex = 3;
        // 
        // gbFilter
        // 
        gbFilter.BackColor = Color.FromArgb(235, 245, 235);
        gbFilter.Controls.Add(lblFilterBy);
        gbFilter.Controls.Add(cbFilterBy);
        gbFilter.Controls.Add(txtFilterValue);
        gbFilter.Controls.Add(lblNatureFilter);
        gbFilter.Controls.Add(cbNatureFilter);
        gbFilter.Controls.Add(chkShowInactive);
        gbFilter.Controls.Add(chkOnlyLeaf);
        gbFilter.Controls.Add(chkOnlySystem);
        gbFilter.Controls.Add(chkOnlyParents);
        gbFilter.Dock = DockStyle.Fill;
        gbFilter.Font = new Font("Tahoma", 10F);
        gbFilter.Location = new Point(8, 4);
        gbFilter.Name = "gbFilter";
        gbFilter.Padding = new Padding(8);
        gbFilter.Size = new Size(1416, 96);
        gbFilter.TabIndex = 0;
        gbFilter.TabStop = false;
        gbFilter.Text = "بحث وتصفية";
        // 
        // lblFilterBy
        // 
        lblFilterBy.AutoSize = true;
        lblFilterBy.Location = new Point(40, 0);
        lblFilterBy.Name = "lblFilterBy";
        lblFilterBy.Size = new Size(75, 24);
        lblFilterBy.TabIndex = 0;
        lblFilterBy.Text = "تصفية بـ:";
        // 
        // cbFilterBy
        // 
        cbFilterBy.DropDownStyle = ComboBoxStyle.DropDownList;
        cbFilterBy.Font = new Font("Tahoma", 10F);
        cbFilterBy.Location = new Point(40, 20);
        cbFilterBy.Name = "cbFilterBy";
        cbFilterBy.Size = new Size(200, 28);
        cbFilterBy.TabIndex = 1;
        cbFilterBy.SelectedIndexChanged += cbFilterBy_SelectedIndexChanged;
        // 
        // txtFilterValue
        // 
        txtFilterValue.Location = new Point(250, 20);
        txtFilterValue.Name = "txtFilterValue";
        txtFilterValue.PlaceholderText = "اكتب نص البحث...";
        txtFilterValue.Size = new Size(240, 28);
        txtFilterValue.TabIndex = 2;
        txtFilterValue.TextChanged += txtFilterValue_TextChanged;
        txtFilterValue.KeyDown += txtFilterValue_KeyDown;
        // 
        // lblNatureFilter
        // 
        lblNatureFilter.AutoSize = true;
        lblNatureFilter.Location = new Point(510, 0);
        lblNatureFilter.Name = "lblNatureFilter";
        lblNatureFilter.Size = new Size(50, 24);
        lblNatureFilter.TabIndex = 3;
        lblNatureFilter.Text = "النوع:";
        // 
        // cbNatureFilter
        // 
        cbNatureFilter.DropDownStyle = ComboBoxStyle.DropDownList;
        cbNatureFilter.Font = new Font("Tahoma", 10F);
        cbNatureFilter.Location = new Point(510, 20);
        cbNatureFilter.Name = "cbNatureFilter";
        cbNatureFilter.Size = new Size(200, 28);
        cbNatureFilter.TabIndex = 4;
        cbNatureFilter.SelectedIndexChanged += cbNatureFilter_SelectedIndexChanged;
        // 
        // chkShowInactive
        // 
        chkShowInactive.AutoSize = true;
        chkShowInactive.Location = new Point(40, 50);
        chkShowInactive.Name = "chkShowInactive";
        chkShowInactive.Size = new Size(160, 24);
        chkShowInactive.TabIndex = 5;
        chkShowInactive.Text = "إظهار غير النشط";
        chkShowInactive.UseVisualStyleBackColor = true;
        chkShowInactive.CheckedChanged += chkShowInactive_CheckedChanged;
        // 
        // chkOnlyLeaf
        // 
        chkOnlyLeaf.AutoSize = true;
        chkOnlyLeaf.Location = new Point(200, 50);
        chkOnlyLeaf.Name = "chkOnlyLeaf";
        chkOnlyLeaf.Size = new Size(180, 24);
        chkOnlyLeaf.TabIndex = 6;
        chkOnlyLeaf.Text = "الحسابات الفرعية فقط";
        chkOnlyLeaf.UseVisualStyleBackColor = true;
        chkOnlyLeaf.CheckedChanged += chkOnlyLeaf_CheckedChanged;
        // 
        // chkOnlySystem
        // 
        chkOnlySystem.AutoSize = true;
        chkOnlySystem.Location = new Point(390, 50);
        chkOnlySystem.Name = "chkOnlySystem";
        chkOnlySystem.Size = new Size(170, 24);
        chkOnlySystem.TabIndex = 7;
        chkOnlySystem.Text = "حسابات النظام فقط";
        chkOnlySystem.UseVisualStyleBackColor = true;
        chkOnlySystem.CheckedChanged += chkOnlySystem_CheckedChanged;
        // 
        // chkOnlyParents
        // 
        chkOnlyParents.AutoSize = true;
        chkOnlyParents.Location = new Point(560, 50);
        chkOnlyParents.Name = "chkOnlyParents";
        chkOnlyParents.Size = new Size(170, 24);
        chkOnlyParents.TabIndex = 8;
        chkOnlyParents.Text = "الحسابات الرئيسية فقط";
        chkOnlyParents.UseVisualStyleBackColor = true;
        chkOnlyParents.CheckedChanged += chkOnlyParents_CheckedChanged;
        // 
        // pnlSummary
        // 
        pnlSummary.BackColor = Color.FromArgb(235, 240, 235);
        pnlSummary.Controls.Add(lblSummary);
        pnlSummary.Dock = DockStyle.Bottom;
        pnlSummary.Location = new Point(0, 770);
        pnlSummary.Name = "pnlSummary";
        pnlSummary.Size = new Size(1432, 30);
        pnlSummary.TabIndex = 5;
        // 
        // lblSummary
        // 
        lblSummary.Dock = DockStyle.Fill;
        lblSummary.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblSummary.ForeColor = Color.FromArgb(14, 82, 52);
        lblSummary.Location = new Point(0, 0);
        lblSummary.Name = "lblSummary";
        lblSummary.Size = new Size(1432, 30);
        lblSummary.TabIndex = 0;
        lblSummary.Text = "المعروض: 0 من 0";
        lblSummary.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlGrid
        // 
        pnlGrid.BackColor = Color.FromArgb(220, 240, 220);
        pnlGrid.Controls.Add(lblEmptyState);
        pnlGrid.Controls.Add(dgvAccounts);
        pnlGrid.Dock = DockStyle.Fill;
        pnlGrid.Location = new Point(0, 410);
        pnlGrid.Name = "pnlGrid";
        pnlGrid.Padding = new Padding(8);
        pnlGrid.Size = new Size(1432, 360);
        pnlGrid.TabIndex = 4;
        // 
        // lblEmptyState
        // 
        lblEmptyState.Dock = DockStyle.Fill;
        lblEmptyState.Font = new Font("Tahoma", 12F, FontStyle.Bold);
        lblEmptyState.Location = new Point(8, 8);
        lblEmptyState.Name = "lblEmptyState";
        lblEmptyState.Size = new Size(1416, 344);
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
        dgvAccounts.AutoGenerateColumns = false;
        dgvAccounts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvAccounts.BackgroundColor = Color.FromArgb(220, 240, 220);
        dgvAccounts.BorderStyle = BorderStyle.None;
        dgvAccounts.ColumnHeadersHeight = 45;
        dgvAccounts.Columns.AddRange(new DataGridViewColumn[] {
        colCode,
        colNameAr,
        colNameEn,
        colNature,
        colLevel,
        colParent,
        colIsLeaf,
        colIsSystem,
        colStatus});
        dgvAccounts.ContextMenuStrip = cmsGrid;
        dgvAccounts.Dock = DockStyle.Fill;
        dgvAccounts.Location = new Point(8, 8);
        dgvAccounts.MultiSelect = false;
        dgvAccounts.Name = "dgvAccounts";
        dgvAccounts.ReadOnly = true;
        dgvAccounts.RowHeadersVisible = false;
        dgvAccounts.RowTemplate.Height = 32;
        dgvAccounts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvAccounts.Size = new Size(1416, 344);
        dgvAccounts.TabIndex = 0;
        dgvAccounts.CellDoubleClick += dgvAccounts_CellDoubleClick;
        dgvAccounts.CellFormatting += dgvAccounts_CellFormatting;
        dgvAccounts.SelectionChanged += dgvAccounts_SelectionChanged;
        // 
        // colCode
        // 
        colCode.DataPropertyName = "AccountCode";
        colCode.FillWeight = 60F;
        colCode.HeaderText = "الكود";
        colCode.MinimumWidth = 70;
        colCode.Name = "colCode";
        colCode.ReadOnly = true;
        // 
        // colNameAr
        // 
        colNameAr.DataPropertyName = "NameArIndented";
        colNameAr.FillWeight = 230F;
        colNameAr.HeaderText = "الاسم العربي";
        colNameAr.MinimumWidth = 150;
        colNameAr.Name = "colNameAr";
        colNameAr.ReadOnly = true;
        // 
        // colNameEn
        // 
        colNameEn.DataPropertyName = "NameEn";
        colNameEn.FillWeight = 150F;
        colNameEn.HeaderText = "الاسم الإنجليزي";
        colNameEn.MinimumWidth = 100;
        colNameEn.Name = "colNameEn";
        colNameEn.ReadOnly = true;
        // 
        // colNature
        // 
        colNature.DataPropertyName = "NatureDisplay";
        colNature.FillWeight = 70F;
        colNature.HeaderText = "النوع";
        colNature.MinimumWidth = 70;
        colNature.Name = "colNature";
        colNature.ReadOnly = true;
        // 
        // colLevel
        // 
        colLevel.DataPropertyName = "LevelDisplay";
        colLevel.FillWeight = 80F;
        colLevel.HeaderText = "المستوى";
        colLevel.MinimumWidth = 80;
        colLevel.Name = "colLevel";
        colLevel.ReadOnly = true;
        // 
        // colParent
        // 
        colParent.DataPropertyName = "ParentAccountName";
        colParent.FillWeight = 140F;
        colParent.HeaderText = "الحساب الأب";
        colParent.MinimumWidth = 90;
        colParent.Name = "colParent";
        colParent.ReadOnly = true;
        // 
        // colIsLeaf
        // 
        colIsLeaf.DataPropertyName = "IsLeafText";
        colIsLeaf.FillWeight = 80F;
        colIsLeaf.HeaderText = "قابل للترحيل";
        colIsLeaf.MinimumWidth = 70;
        colIsLeaf.Name = "colIsLeaf";
        colIsLeaf.ReadOnly = true;
        // 
        // colIsSystem
        // 
        colIsSystem.DataPropertyName = "IsSystemText";
        colIsSystem.FillWeight = 80F;
        colIsSystem.HeaderText = "حساب نظامي";
        colIsSystem.MinimumWidth = 70;
        colIsSystem.Name = "colIsSystem";
        colIsSystem.ReadOnly = true;
        // 
        // colStatus
        // 
        colStatus.DataPropertyName = "Status";
        colStatus.FillWeight = 70F;
        colStatus.HeaderText = "الحالة";
        colStatus.MinimumWidth = 70;
        colStatus.Name = "colStatus";
        colStatus.ReadOnly = true;
        // 
        // cmsGrid
        // 
        cmsGrid.Font = new Font("Tahoma", 10F);
        cmsGrid.Items.AddRange(new ToolStripItem[] { cmsEdit, cmsDelete, cmsRefresh, cmsTree });
        cmsGrid.Name = "cmsGrid";
        cmsGrid.RightToLeft = RightToLeft.Yes;
        cmsGrid.Size = new Size(160, 104);
        // 
        // cmsEdit
        // 
        cmsEdit.Name = "cmsEdit";
        cmsEdit.Size = new Size(159, 24);
        cmsEdit.Text = "تعديل";
        cmsEdit.Click += cmsEdit_Click;
        // 
        // cmsDelete
        // 
        cmsDelete.Name = "cmsDelete";
        cmsDelete.Size = new Size(159, 24);
        cmsDelete.Text = "تعطيل";
        cmsDelete.Click += cmsDelete_Click;
        // 
        // cmsRefresh
        // 
        cmsRefresh.Name = "cmsRefresh";
        cmsRefresh.Size = new Size(159, 24);
        cmsRefresh.Text = "تحديث";
        cmsRefresh.Click += cmsRefresh_Click;
        // 
        // cmsTree
        // 
        cmsTree.Name = "cmsTree";
        cmsTree.Size = new Size(159, 24);
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
        AutoScaleDimensions = new SizeF(9F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(170, 220, 170);
        ClientSize = new Size(1432, 800);
        Controls.Add(pnlGrid);
        Controls.Add(pnlSummary);
        Controls.Add(pnlFilters);
        Controls.Add(pnlFields);
        Controls.Add(pnlToolbar);
        Controls.Add(pnlHeader);
        Font = new Font("Tahoma", 10F);
        Name = "frmAccounts";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterParent;
        Text = "دليل الحسابات";
        Load += frmAccounts_Load;
        pnlHeader.ResumeLayout(false);
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
    }

    private System.Windows.Forms.Panel pnlHeader;
    private System.Windows.Forms.Label lblMode;
    private System.Windows.Forms.FlowLayoutPanel pnlToolbar;
    private System.Windows.Forms.Button btnNew;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.Button btnEdit;
    private System.Windows.Forms.Button btnDelete;
    private System.Windows.Forms.Button btnPermanentDelete;
    private System.Windows.Forms.Button btnRefresh;
    private System.Windows.Forms.Button btnClose;
    private System.Windows.Forms.Panel pnlFields;
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
    private System.Windows.Forms.Label lblFilterBy;
    private System.Windows.Forms.ComboBox cbFilterBy;
    private System.Windows.Forms.TextBox txtFilterValue;
    private System.Windows.Forms.Label lblNatureFilter;
    private System.Windows.Forms.ComboBox cbNatureFilter;
    private System.Windows.Forms.CheckBox chkShowInactive;
    private System.Windows.Forms.CheckBox chkOnlyLeaf;
    private System.Windows.Forms.CheckBox chkOnlySystem;
    private System.Windows.Forms.CheckBox chkOnlyParents;
    private System.Windows.Forms.Panel pnlSummary;
    private System.Windows.Forms.Label lblSummary;
    private System.Windows.Forms.Panel pnlGrid;
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
