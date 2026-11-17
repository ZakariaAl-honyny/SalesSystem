namespace SalesSystem.Desktop.Screens.Accounting.Forms;

partial class frmJournalEntries
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
        DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle13 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle14 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle15 = new DataGridViewCellStyle();
        pnlHeader = new Panel();
        lblMode = new Label();
        gbCommands = new GroupBox();
        pnlToolbar = new FlowLayoutPanel();
        btnNew = new Button();
        btnSave = new Button();
        btnEdit = new Button();
        btnPost = new Button();
        btnCancelEntry = new Button();
        btnDeleteDraft = new Button();
        btnRefresh = new Button();
        btnClose = new Button();
        pnlFilters = new Panel();
        gbFilter = new GroupBox();
        lblFilterBy = new Label();
        cbFilterBy = new ComboBox();
        txtFilterValue = new TextBox();
        lblEntryTypeFilter = new Label();
        cbEntryTypeFilter = new ComboBox();
        lblStatusFilter = new Label();
        cbStatusFilter = new ComboBox();
        lblFrom = new Label();
        dtpFrom = new DateTimePicker();
        lblTo = new Label();
        dtpTo = new DateTimePicker();
        btnRangeMonth = new Button();
        chkOnlyCancelled = new CheckBox();
        chkOnlyDraft = new CheckBox();
        btnClearFilters = new Button();
        btnToggleFilters = new Button();
        pnlSummary = new GroupBox();
        lblSummary = new Label();
        pnlEditor = new Panel();
        gbLines = new GroupBox();
        dgvLines = new DataGridView();
        lblTotals = new Label();
        pnlLineEntry = new Panel();
        lblLineAccount = new Label();
        cbLineAccount = new ComboBox();
        lblLineDebit = new Label();
        numLineDebit = new NumericUpDown();
        lblLineCredit = new Label();
        numLineCredit = new NumericUpDown();
        lblLineDescription = new Label();
        txtLineDescription = new TextBox();
        btnAddLine = new Button();
        btnRemoveLine = new Button();
        pnlEntryHeader = new GroupBox();
        lblEntryInfo = new Label();
        lblEntryDate = new Label();
        dtpEntryDate = new DateTimePicker();
        txtEntryDescription = new TextBox();
        txtReferenceNumber = new TextBox();
        lblEntryType = new Label();
        cbEntryType = new ComboBox();
        lblEntryDescription = new Label();
        lblReferenceNumber = new Label();
        pnlGrid = new GroupBox();
        lblEmptyState = new Label();
        dgvEntries = new DataGridView();
        cmsGrid = new ContextMenuStrip(components);
        cmsEdit = new ToolStripMenuItem();
        cmsPost = new ToolStripMenuItem();
        cmsCancel = new ToolStripMenuItem();
        cmsDeleteDraft = new ToolStripMenuItem();
        cmsRefresh = new ToolStripMenuItem();
        errorProvider = new ErrorProvider(components);
        toolTip = new ToolTip(components);
        liveTip = new ToolTip(components);
        pnlHeader.SuspendLayout();
        gbCommands.SuspendLayout();
        pnlToolbar.SuspendLayout();
        pnlFilters.SuspendLayout();
        gbFilter.SuspendLayout();
        pnlSummary.SuspendLayout();
        pnlEditor.SuspendLayout();
        gbLines.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvLines).BeginInit();
        pnlLineEntry.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numLineDebit).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numLineCredit).BeginInit();
        pnlEntryHeader.SuspendLayout();
        pnlGrid.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvEntries).BeginInit();
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
        pnlHeader.Size = new Size(1440, 56);
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
        lblMode.Size = new Size(1440, 56);
        lblMode.TabIndex = 0;
        lblMode.Text = "قيد محاسبي جديد";
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
        gbCommands.Size = new Size(1440, 82);
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
        pnlToolbar.Controls.Add(btnPost);
        pnlToolbar.Controls.Add(btnCancelEntry);
        pnlToolbar.Controls.Add(btnDeleteDraft);
        pnlToolbar.Controls.Add(btnRefresh);
        pnlToolbar.Controls.Add(btnClose);
        pnlToolbar.Dock = DockStyle.Fill;
        pnlToolbar.Location = new Point(3, 28);
        pnlToolbar.Name = "pnlToolbar";
        pnlToolbar.Padding = new Padding(8, 4, 8, 0);
        pnlToolbar.Size = new Size(1434, 51);
        pnlToolbar.TabIndex = 0;
        pnlToolbar.WrapContents = false;
        // 
        // btnNew
        // 
        btnNew.BackColor = Color.FromArgb(40, 167, 69);
        btnNew.FlatStyle = FlatStyle.Flat;
        btnNew.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnNew.ForeColor = Color.White;
        btnNew.Location = new Point(1284, 8);
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
        btnSave.Location = new Point(1146, 8);
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
        btnEdit.Location = new Point(1008, 8);
        btnEdit.Margin = new Padding(4);
        btnEdit.Name = "btnEdit";
        btnEdit.Size = new Size(130, 38);
        btnEdit.TabIndex = 2;
        btnEdit.Text = "تعديل";
        btnEdit.UseVisualStyleBackColor = false;
        btnEdit.Click += btnEdit_Click;
        // 
        // btnPost
        // 
        btnPost.BackColor = Color.FromArgb(23, 162, 184);
        btnPost.FlatStyle = FlatStyle.Flat;
        btnPost.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnPost.ForeColor = Color.White;
        btnPost.Location = new Point(870, 8);
        btnPost.Margin = new Padding(4);
        btnPost.Name = "btnPost";
        btnPost.Size = new Size(130, 38);
        btnPost.TabIndex = 3;
        btnPost.Text = "ترحيل";
        btnPost.UseVisualStyleBackColor = false;
        btnPost.Click += btnPost_Click;
        // 
        // btnCancelEntry
        // 
        btnCancelEntry.BackColor = Color.FromArgb(220, 53, 69);
        btnCancelEntry.FlatStyle = FlatStyle.Flat;
        btnCancelEntry.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnCancelEntry.ForeColor = Color.White;
        btnCancelEntry.Location = new Point(732, 8);
        btnCancelEntry.Margin = new Padding(4);
        btnCancelEntry.Name = "btnCancelEntry";
        btnCancelEntry.Size = new Size(130, 38);
        btnCancelEntry.TabIndex = 4;
        btnCancelEntry.Text = "إلغاء القيد";
        btnCancelEntry.UseVisualStyleBackColor = false;
        btnCancelEntry.Click += btnCancelEntry_Click;
        // 
        // btnDeleteDraft
        // 
        btnDeleteDraft.BackColor = Color.FromArgb(220, 53, 69);
        btnDeleteDraft.FlatStyle = FlatStyle.Flat;
        btnDeleteDraft.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnDeleteDraft.ForeColor = Color.White;
        btnDeleteDraft.Location = new Point(594, 8);
        btnDeleteDraft.Margin = new Padding(4);
        btnDeleteDraft.Name = "btnDeleteDraft";
        btnDeleteDraft.Size = new Size(130, 38);
        btnDeleteDraft.TabIndex = 5;
        btnDeleteDraft.Text = "حذف مسودة";
        btnDeleteDraft.UseVisualStyleBackColor = false;
        btnDeleteDraft.Click += btnDeleteDraft_Click;
        // 
        // btnRefresh
        // 
        btnRefresh.BackColor = Color.FromArgb(108, 117, 125);
        btnRefresh.FlatStyle = FlatStyle.Flat;
        btnRefresh.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnRefresh.ForeColor = Color.White;
        btnRefresh.Location = new Point(456, 8);
        btnRefresh.Margin = new Padding(4);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(130, 38);
        btnRefresh.TabIndex = 6;
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
        btnClose.Location = new Point(318, 8);
        btnClose.Margin = new Padding(4);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(130, 38);
        btnClose.TabIndex = 7;
        btnClose.Text = "إغلاق";
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += btnClose_Click;
        // 
        // pnlFilters
        // 
        pnlFilters.BackColor = Color.White;
        pnlFilters.Controls.Add(gbFilter);
        pnlFilters.Controls.Add(btnToggleFilters);
        pnlFilters.Dock = DockStyle.Top;
        pnlFilters.Location = new Point(0, 138);
        pnlFilters.Name = "pnlFilters";
        pnlFilters.Padding = new Padding(8, 4, 8, 4);
        pnlFilters.Size = new Size(1440, 112);
        pnlFilters.TabIndex = 2;
        // 
        // gbFilter
        // 
        gbFilter.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        gbFilter.BackColor = Color.White;
        gbFilter.Controls.Add(lblFilterBy);
        gbFilter.Controls.Add(cbFilterBy);
        gbFilter.Controls.Add(txtFilterValue);
        gbFilter.Controls.Add(lblEntryTypeFilter);
        gbFilter.Controls.Add(cbEntryTypeFilter);
        gbFilter.Controls.Add(lblStatusFilter);
        gbFilter.Controls.Add(cbStatusFilter);
        gbFilter.Controls.Add(lblFrom);
        gbFilter.Controls.Add(dtpFrom);
        gbFilter.Controls.Add(lblTo);
        gbFilter.Controls.Add(dtpTo);
        gbFilter.Controls.Add(btnRangeMonth);
        gbFilter.Controls.Add(chkOnlyCancelled);
        gbFilter.Controls.Add(chkOnlyDraft);
        gbFilter.Controls.Add(btnClearFilters);
        gbFilter.Font = new Font("Tahoma", 10F);
        gbFilter.Location = new Point(148, 4);
        gbFilter.Name = "gbFilter";
        gbFilter.Padding = new Padding(8);
        gbFilter.Size = new Size(1284, 104);
        gbFilter.TabIndex = 0;
        gbFilter.TabStop = false;
        gbFilter.Text = "بحث وتصفية";
        // 
        // lblFilterBy
        // 
        lblFilterBy.AutoSize = true;
        lblFilterBy.Location = new Point(1065, 30);
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
        cbFilterBy.Location = new Point(915, 26);
        cbFilterBy.Name = "cbFilterBy";
        cbFilterBy.Size = new Size(140, 32);
        cbFilterBy.TabIndex = 1;
        cbFilterBy.SelectedIndexChanged += cbFilterBy_SelectedIndexChanged;
        // 
        // txtFilterValue
        // 
        txtFilterValue.BackColor = Color.White;
        txtFilterValue.Font = new Font("Tahoma", 10F);
        txtFilterValue.Location = new Point(655, 26);
        txtFilterValue.Name = "txtFilterValue";
        txtFilterValue.PlaceholderText = "بحث فوري — Esc يمسح التصفية...";
        txtFilterValue.Size = new Size(250, 32);
        txtFilterValue.TabIndex = 2;
        txtFilterValue.TextChanged += txtFilterValue_TextChanged;
        txtFilterValue.KeyDown += txtFilterValue_KeyDown;
        // 
        // lblEntryTypeFilter
        // 
        lblEntryTypeFilter.AutoSize = true;
        lblEntryTypeFilter.Location = new Point(850, 67);
        lblEntryTypeFilter.Name = "lblEntryTypeFilter";
        lblEntryTypeFilter.Size = new Size(56, 24);
        lblEntryTypeFilter.TabIndex = 3;
        lblEntryTypeFilter.Text = "النوع:";
        // 
        // cbEntryTypeFilter
        // 
        cbEntryTypeFilter.BackColor = Color.White;
        cbEntryTypeFilter.DropDownStyle = ComboBoxStyle.DropDownList;
        cbEntryTypeFilter.Font = new Font("Tahoma", 10F);
        cbEntryTypeFilter.Location = new Point(675, 63);
        cbEntryTypeFilter.Name = "cbEntryTypeFilter";
        cbEntryTypeFilter.Size = new Size(165, 32);
        cbEntryTypeFilter.TabIndex = 4;
        cbEntryTypeFilter.SelectedIndexChanged += cbEntryTypeFilter_SelectedIndexChanged;
        // 
        // lblStatusFilter
        // 
        lblStatusFilter.AutoSize = true;
        lblStatusFilter.Location = new Point(1066, 67);
        lblStatusFilter.Name = "lblStatusFilter";
        lblStatusFilter.Size = new Size(65, 24);
        lblStatusFilter.TabIndex = 5;
        lblStatusFilter.Text = "الحالة:";
        // 
        // cbStatusFilter
        // 
        cbStatusFilter.BackColor = Color.White;
        cbStatusFilter.DropDownStyle = ComboBoxStyle.DropDownList;
        cbStatusFilter.Font = new Font("Tahoma", 10F);
        cbStatusFilter.Location = new Point(916, 63);
        cbStatusFilter.Name = "cbStatusFilter";
        cbStatusFilter.Size = new Size(140, 32);
        cbStatusFilter.TabIndex = 6;
        cbStatusFilter.SelectedIndexChanged += cbStatusFilter_SelectedIndexChanged;
        // 
        // lblFrom
        // 
        lblFrom.AutoSize = true;
        lblFrom.Location = new Point(603, 30);
        lblFrom.Name = "lblFrom";
        lblFrom.Size = new Size(42, 24);
        lblFrom.TabIndex = 7;
        lblFrom.Text = "من:";
        // 
        // dtpFrom
        // 
        dtpFrom.BackColor = Color.White;
        dtpFrom.CustomFormat = "yyyy-MM-dd";
        dtpFrom.Font = new Font("Tahoma", 10F);
        dtpFrom.Format = DateTimePickerFormat.Custom;
        dtpFrom.Location = new Point(444, 26);
        dtpFrom.Name = "dtpFrom";
        dtpFrom.Size = new Size(149, 32);
        dtpFrom.TabIndex = 8;
        dtpFrom.ValueChanged += dtpFrom_ValueChanged;
        // 
        // lblTo
        // 
        lblTo.AutoSize = true;
        lblTo.Location = new Point(386, 30);
        lblTo.Name = "lblTo";
        lblTo.Size = new Size(48, 24);
        lblTo.TabIndex = 9;
        lblTo.Text = "إلى:";
        lblTo.Click += lblTo_Click;
        // 
        // dtpTo
        // 
        dtpTo.BackColor = Color.White;
        dtpTo.CustomFormat = "yyyy-MM-dd";
        dtpTo.Font = new Font("Tahoma", 10F);
        dtpTo.Format = DateTimePickerFormat.Custom;
        dtpTo.Location = new Point(227, 26);
        dtpTo.Name = "dtpTo";
        dtpTo.Size = new Size(149, 32);
        dtpTo.TabIndex = 10;
        dtpTo.ValueChanged += dtpTo_ValueChanged;
        // 
        // btnRangeMonth
        // 
        btnRangeMonth.BackColor = Color.FromArgb(108, 117, 125);
        btnRangeMonth.FlatStyle = FlatStyle.Flat;
        btnRangeMonth.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        btnRangeMonth.ForeColor = Color.White;
        btnRangeMonth.Location = new Point(97, 27);
        btnRangeMonth.Name = "btnRangeMonth";
        btnRangeMonth.Size = new Size(120, 30);
        btnRangeMonth.TabIndex = 11;
        btnRangeMonth.Text = "هذا الشهر";
        btnRangeMonth.UseVisualStyleBackColor = false;
        btnRangeMonth.Click += btnRangeMonth_Click;
        // 
        // chkOnlyCancelled
        // 
        chkOnlyCancelled.AutoSize = true;
        chkOnlyCancelled.Location = new Point(369, 65);
        chkOnlyCancelled.Name = "chkOnlyCancelled";
        chkOnlyCancelled.Size = new Size(129, 28);
        chkOnlyCancelled.TabIndex = 12;
        chkOnlyCancelled.Text = "الملغاة فقط";
        chkOnlyCancelled.UseVisualStyleBackColor = false;
        chkOnlyCancelled.CheckedChanged += chkOnlyCancelled_CheckedChanged;
        // 
        // chkOnlyDraft
        // 
        chkOnlyDraft.AutoSize = true;
        chkOnlyDraft.Location = new Point(508, 65);
        chkOnlyDraft.Name = "chkOnlyDraft";
        chkOnlyDraft.Size = new Size(157, 28);
        chkOnlyDraft.TabIndex = 13;
        chkOnlyDraft.Text = "المسودات فقط";
        chkOnlyDraft.UseVisualStyleBackColor = false;
        chkOnlyDraft.CheckedChanged += chkOnlyDraft_CheckedChanged;
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
        btnClearFilters.Location = new Point(178, 63);
        btnClearFilters.Name = "btnClearFilters";
        btnClearFilters.Size = new Size(181, 32);
        btnClearFilters.TabIndex = 14;
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
        btnToggleFilters.Size = new Size(140, 104);
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
        pnlSummary.Location = new Point(0, 908);
        pnlSummary.Name = "pnlSummary";
        pnlSummary.Padding = new Padding(8, 18, 8, 4);
        pnlSummary.Size = new Size(1440, 52);
        pnlSummary.TabIndex = 3;
        pnlSummary.TabStop = false;
        pnlSummary.Text = "الإجماليات";
        // 
        // lblSummary
        // 
        lblSummary.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblSummary.ForeColor = Color.FromArgb(33, 37, 41);
        lblSummary.Location = new Point(8, 15);
        lblSummary.Name = "lblSummary";
        lblSummary.Size = new Size(1424, 33);
        lblSummary.TabIndex = 0;
        lblSummary.Text = "المعروض: 0";
        lblSummary.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlEditor
        // 
        pnlEditor.BackColor = Color.White;
        pnlEditor.Controls.Add(gbLines);
        pnlEditor.Controls.Add(pnlEntryHeader);
        pnlEditor.Dock = DockStyle.Bottom;
        pnlEditor.Location = new Point(0, 528);
        pnlEditor.Name = "pnlEditor";
        pnlEditor.Padding = new Padding(8);
        pnlEditor.Size = new Size(1440, 380);
        pnlEditor.TabIndex = 4;
        // 
        // gbLines
        // 
        gbLines.BackColor = Color.White;
        gbLines.Controls.Add(dgvLines);
        gbLines.Controls.Add(lblTotals);
        gbLines.Controls.Add(pnlLineEntry);
        gbLines.Dock = DockStyle.Fill;
        gbLines.Font = new Font("Tahoma", 10F);
        gbLines.Location = new Point(8, 8);
        gbLines.Name = "gbLines";
        gbLines.Padding = new Padding(8, 6, 8, 6);
        gbLines.Size = new Size(1424, 364);
        gbLines.TabIndex = 1;
        gbLines.TabStop = false;
        gbLines.Text = "بنود القيد";
        // 
        // dgvLines
        // 
        dgvLines.AllowUserToAddRows = false;
        dgvLines.AllowUserToDeleteRows = false;
        dgvLines.AllowUserToOrderColumns = true;
        dataGridViewCellStyle4.BackColor = Color.FromArgb(248, 249, 250);
        dgvLines.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
        dgvLines.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvLines.BackgroundColor = Color.White;
        dgvLines.BorderStyle = BorderStyle.None;
        dgvLines.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
        dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle5.BackColor = Color.FromArgb(52, 58, 64);
        dataGridViewCellStyle5.Font = new Font("Tahoma", 10F);
        dataGridViewCellStyle5.ForeColor = Color.White;
        dataGridViewCellStyle5.SelectionBackColor = Color.FromArgb(52, 58, 64);
        dataGridViewCellStyle5.SelectionForeColor = Color.White;
        dataGridViewCellStyle5.WrapMode = DataGridViewTriState.True;
        dgvLines.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
        dgvLines.ColumnHeadersHeight = 40;
        dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle6.BackColor = Color.White;
        dataGridViewCellStyle6.Font = new Font("Tahoma", 10F);
        dataGridViewCellStyle6.ForeColor = Color.FromArgb(33, 37, 41);
        dataGridViewCellStyle6.SelectionBackColor = Color.FromArgb(255, 243, 205);
        dataGridViewCellStyle6.SelectionForeColor = Color.FromArgb(33, 37, 41);
        dataGridViewCellStyle6.WrapMode = DataGridViewTriState.False;
        dgvLines.DefaultCellStyle = dataGridViewCellStyle6;
        dgvLines.Dock = DockStyle.Fill;
        dgvLines.EnableHeadersVisualStyles = false;
        dgvLines.GridColor = Color.FromArgb(222, 226, 230);
        dgvLines.Location = new Point(8, 75);
        dgvLines.MultiSelect = false;
        dgvLines.Name = "dgvLines";
        dgvLines.ReadOnly = true;
        dgvLines.RowHeadersVisible = false;
        dgvLines.RowHeadersWidth = 62;
        dgvLines.RowTemplate.Height = 30;
        dgvLines.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvLines.Size = new Size(1408, 257);
        dgvLines.TabIndex = 2;
        // 
        // lblTotals
        // 
        lblTotals.BackColor = Color.White;
        lblTotals.Dock = DockStyle.Bottom;
        lblTotals.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblTotals.Location = new Point(8, 332);
        lblTotals.Name = "lblTotals";
        lblTotals.Size = new Size(1408, 26);
        lblTotals.TabIndex = 1;
        lblTotals.Text = "مجموع المدين: 0.00 · مجموع الدائن: 0.00 · الفرق: 0.00";
        lblTotals.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlLineEntry
        // 
        pnlLineEntry.BackColor = Color.White;
        pnlLineEntry.Controls.Add(lblLineAccount);
        pnlLineEntry.Controls.Add(cbLineAccount);
        pnlLineEntry.Controls.Add(lblLineDebit);
        pnlLineEntry.Controls.Add(numLineDebit);
        pnlLineEntry.Controls.Add(lblLineCredit);
        pnlLineEntry.Controls.Add(numLineCredit);
        pnlLineEntry.Controls.Add(lblLineDescription);
        pnlLineEntry.Controls.Add(txtLineDescription);
        pnlLineEntry.Controls.Add(btnAddLine);
        pnlLineEntry.Controls.Add(btnRemoveLine);
        pnlLineEntry.Dock = DockStyle.Top;
        pnlLineEntry.Location = new Point(8, 31);
        pnlLineEntry.Name = "pnlLineEntry";
        pnlLineEntry.Size = new Size(1408, 44);
        pnlLineEntry.TabIndex = 0;
        // 
        // lblLineAccount
        // 
        lblLineAccount.AutoSize = true;
        lblLineAccount.Location = new Point(286, 10);
        lblLineAccount.Name = "lblLineAccount";
        lblLineAccount.Size = new Size(85, 24);
        lblLineAccount.TabIndex = 0;
        lblLineAccount.Text = "الحساب:";
        // 
        // cbLineAccount
        // 
        cbLineAccount.BackColor = Color.White;
        cbLineAccount.DropDownStyle = ComboBoxStyle.DropDownList;
        cbLineAccount.Font = new Font("Tahoma", 10F);
        cbLineAccount.Location = new Point(22, 6);
        cbLineAccount.Name = "cbLineAccount";
        cbLineAccount.Size = new Size(258, 32);
        cbLineAccount.TabIndex = 1;
        // 
        // lblLineDebit
        // 
        lblLineDebit.AutoSize = true;
        lblLineDebit.Location = new Point(498, 10);
        lblLineDebit.Name = "lblLineDebit";
        lblLineDebit.Size = new Size(59, 24);
        lblLineDebit.TabIndex = 2;
        lblLineDebit.Text = "مدين:";
        // 
        // numLineDebit
        // 
        numLineDebit.BackColor = Color.White;
        numLineDebit.DecimalPlaces = 2;
        numLineDebit.Font = new Font("Tahoma", 10F);
        numLineDebit.Location = new Point(377, 5);
        numLineDebit.Maximum = new decimal(new int[] { 999999999, 0, 0, 0 });
        numLineDebit.Name = "numLineDebit";
        numLineDebit.Size = new Size(115, 32);
        numLineDebit.TabIndex = 3;
        numLineDebit.TextAlign = HorizontalAlignment.Center;
        numLineDebit.ThousandsSeparator = true;
        // 
        // lblLineCredit
        // 
        lblLineCredit.AutoSize = true;
        lblLineCredit.Location = new Point(684, 8);
        lblLineCredit.Name = "lblLineCredit";
        lblLineCredit.Size = new Size(52, 24);
        lblLineCredit.TabIndex = 4;
        lblLineCredit.Text = "دائن:";
        // 
        // numLineCredit
        // 
        numLineCredit.BackColor = Color.White;
        numLineCredit.DecimalPlaces = 2;
        numLineCredit.Font = new Font("Tahoma", 10F);
        numLineCredit.Location = new Point(563, 6);
        numLineCredit.Maximum = new decimal(new int[] { 999999999, 0, 0, 0 });
        numLineCredit.Name = "numLineCredit";
        numLineCredit.Size = new Size(115, 32);
        numLineCredit.TabIndex = 5;
        numLineCredit.TextAlign = HorizontalAlignment.Center;
        numLineCredit.ThousandsSeparator = true;
        // 
        // lblLineDescription
        // 
        lblLineDescription.AutoSize = true;
        lblLineDescription.Location = new Point(1071, 10);
        lblLineDescription.Name = "lblLineDescription";
        lblLineDescription.Size = new Size(61, 24);
        lblLineDescription.TabIndex = 6;
        lblLineDescription.Text = "البيان:";
        // 
        // txtLineDescription
        // 
        txtLineDescription.BackColor = Color.White;
        txtLineDescription.Font = new Font("Tahoma", 10F);
        txtLineDescription.Location = new Point(742, 5);
        txtLineDescription.Name = "txtLineDescription";
        txtLineDescription.PlaceholderText = "بيان البند (اختياري)";
        txtLineDescription.Size = new Size(323, 32);
        txtLineDescription.TabIndex = 7;
        // 
        // btnAddLine
        // 
        btnAddLine.BackColor = Color.FromArgb(40, 167, 69);
        btnAddLine.FlatStyle = FlatStyle.Flat;
        btnAddLine.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        btnAddLine.ForeColor = Color.White;
        btnAddLine.Location = new Point(1146, 7);
        btnAddLine.Name = "btnAddLine";
        btnAddLine.Size = new Size(105, 30);
        btnAddLine.TabIndex = 8;
        btnAddLine.Text = "إضافة بند";
        btnAddLine.UseVisualStyleBackColor = false;
        btnAddLine.Click += btnAddLine_Click;
        // 
        // btnRemoveLine
        // 
        btnRemoveLine.BackColor = Color.FromArgb(220, 53, 69);
        btnRemoveLine.FlatStyle = FlatStyle.Flat;
        btnRemoveLine.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        btnRemoveLine.ForeColor = Color.White;
        btnRemoveLine.Location = new Point(1276, 7);
        btnRemoveLine.Name = "btnRemoveLine";
        btnRemoveLine.Size = new Size(110, 30);
        btnRemoveLine.TabIndex = 9;
        btnRemoveLine.Text = "حذف البند";
        btnRemoveLine.UseVisualStyleBackColor = false;
        btnRemoveLine.Click += btnRemoveLine_Click;
        // 
        // pnlEntryHeader
        // 
        pnlEntryHeader.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        pnlEntryHeader.BackColor = Color.White;
        pnlEntryHeader.Controls.Add(lblEntryInfo);
        pnlEntryHeader.Controls.Add(lblEntryDate);
        pnlEntryHeader.Controls.Add(dtpEntryDate);
        pnlEntryHeader.Controls.Add(txtEntryDescription);
        pnlEntryHeader.Controls.Add(txtReferenceNumber);
        pnlEntryHeader.Controls.Add(lblEntryType);
        pnlEntryHeader.Controls.Add(cbEntryType);
        pnlEntryHeader.Controls.Add(lblEntryDescription);
        pnlEntryHeader.Controls.Add(lblReferenceNumber);
        pnlEntryHeader.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        pnlEntryHeader.Location = new Point(8, 8);
        pnlEntryHeader.Name = "pnlEntryHeader";
        pnlEntryHeader.Size = new Size(1424, 90);
        pnlEntryHeader.TabIndex = 0;
        pnlEntryHeader.TabStop = false;
        pnlEntryHeader.Text = "بيانات القيد";
        // 
        // lblEntryInfo
        // 
        lblEntryInfo.AutoSize = true;
        lblEntryInfo.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblEntryInfo.Location = new Point(1080, 18);
        lblEntryInfo.Name = "lblEntryInfo";
        lblEntryInfo.Size = new Size(208, 22);
        lblEntryInfo.TabIndex = 0;
        lblEntryInfo.Text = "رقم القيد: — · الحالة: —";
        // 
        // lblEntryDate
        // 
        lblEntryDate.AutoSize = true;
        lblEntryDate.Location = new Point(318, 45);
        lblEntryDate.Name = "lblEntryDate";
        lblEntryDate.Size = new Size(77, 24);
        lblEntryDate.TabIndex = 1;
        lblEntryDate.Text = "التاريخ:";
        // 
        // dtpEntryDate
        // 
        dtpEntryDate.BackColor = Color.White;
        dtpEntryDate.CustomFormat = "yyyy-MM-dd";
        dtpEntryDate.Font = new Font("Tahoma", 10F);
        dtpEntryDate.Format = DateTimePickerFormat.Custom;
        dtpEntryDate.Location = new Point(172, 43);
        dtpEntryDate.Name = "dtpEntryDate";
        dtpEntryDate.Size = new Size(140, 32);
        dtpEntryDate.TabIndex = 2;
        // 
        // txtEntryDescription
        // 
        txtEntryDescription.BackColor = Color.White;
        txtEntryDescription.Font = new Font("Tahoma", 10F);
        txtEntryDescription.Location = new Point(682, 42);
        txtEntryDescription.Name = "txtEntryDescription";
        txtEntryDescription.PlaceholderText = "بيان القيد المحاسبي (مطلوب)";
        txtEntryDescription.Size = new Size(271, 32);
        txtEntryDescription.TabIndex = 6;
        // 
        // txtReferenceNumber
        // 
        txtReferenceNumber.BackColor = Color.White;
        txtReferenceNumber.Font = new Font("Tahoma", 10F);
        txtReferenceNumber.Location = new Point(1034, 43);
        txtReferenceNumber.Name = "txtReferenceNumber";
        txtReferenceNumber.PlaceholderText = "رقم المستند المرجعي (اختياري)";
        txtReferenceNumber.Size = new Size(287, 32);
        txtReferenceNumber.TabIndex = 8;
        // 
        // lblEntryType
        // 
        lblEntryType.AutoSize = true;
        lblEntryType.Location = new Point(577, 46);
        lblEntryType.Name = "lblEntryType";
        lblEntryType.Size = new Size(99, 24);
        lblEntryType.TabIndex = 3;
        lblEntryType.Text = "نوع القيد:";
        // 
        // cbEntryType
        // 
        cbEntryType.BackColor = Color.White;
        cbEntryType.DropDownStyle = ComboBoxStyle.DropDownList;
        cbEntryType.Font = new Font("Tahoma", 10F);
        cbEntryType.Location = new Point(401, 42);
        cbEntryType.Name = "cbEntryType";
        cbEntryType.Size = new Size(170, 32);
        cbEntryType.TabIndex = 4;
        cbEntryType.SelectedIndexChanged += cbEntryType_SelectedIndexChanged;
        // 
        // lblEntryDescription
        // 
        lblEntryDescription.AutoSize = true;
        lblEntryDescription.Location = new Point(959, 46);
        lblEntryDescription.Name = "lblEntryDescription";
        lblEntryDescription.Size = new Size(69, 24);
        lblEntryDescription.TabIndex = 5;
        lblEntryDescription.Text = "البيان:";
        // 
        // lblReferenceNumber
        // 
        lblReferenceNumber.AutoSize = true;
        lblReferenceNumber.Location = new Point(1327, 46);
        lblReferenceNumber.Name = "lblReferenceNumber";
        lblReferenceNumber.Size = new Size(82, 24);
        lblReferenceNumber.TabIndex = 7;
        lblReferenceNumber.Text = "المرجع:";
        // 
        // pnlGrid
        // 
        pnlGrid.BackColor = Color.White;
        pnlGrid.Controls.Add(lblEmptyState);
        pnlGrid.Controls.Add(dgvEntries);
        pnlGrid.Dock = DockStyle.Fill;
        pnlGrid.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        pnlGrid.Location = new Point(0, 250);
        pnlGrid.Name = "pnlGrid";
        pnlGrid.Padding = new Padding(8);
        pnlGrid.Size = new Size(1440, 278);
        pnlGrid.TabIndex = 5;
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
        lblEmptyState.Size = new Size(1424, 237);
        lblEmptyState.TabIndex = 1;
        lblEmptyState.Text = "لا توجد بيانات";
        lblEmptyState.TextAlign = ContentAlignment.MiddleCenter;
        lblEmptyState.Visible = false;
        // 
        // dgvEntries
        // 
        dgvEntries.AllowUserToAddRows = false;
        dgvEntries.AllowUserToDeleteRows = false;
        dgvEntries.AllowUserToOrderColumns = true;
        dataGridViewCellStyle13.BackColor = Color.FromArgb(248, 249, 250);
        dgvEntries.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle13;
        dgvEntries.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvEntries.BackgroundColor = Color.White;
        dgvEntries.BorderStyle = BorderStyle.None;
        dgvEntries.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
        dataGridViewCellStyle14.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle14.BackColor = Color.FromArgb(52, 58, 64);
        dataGridViewCellStyle14.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        dataGridViewCellStyle14.ForeColor = Color.White;
        dataGridViewCellStyle14.SelectionBackColor = Color.FromArgb(52, 58, 64);
        dataGridViewCellStyle14.SelectionForeColor = Color.White;
        dataGridViewCellStyle14.WrapMode = DataGridViewTriState.True;
        dgvEntries.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle14;
        dgvEntries.ColumnHeadersHeight = 45;
        dgvEntries.ContextMenuStrip = cmsGrid;
        dataGridViewCellStyle15.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle15.BackColor = Color.White;
        dataGridViewCellStyle15.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        dataGridViewCellStyle15.ForeColor = Color.FromArgb(33, 37, 41);
        dataGridViewCellStyle15.SelectionBackColor = Color.FromArgb(255, 243, 205);
        dataGridViewCellStyle15.SelectionForeColor = Color.FromArgb(33, 37, 41);
        dataGridViewCellStyle15.WrapMode = DataGridViewTriState.False;
        dgvEntries.DefaultCellStyle = dataGridViewCellStyle15;
        dgvEntries.Dock = DockStyle.Fill;
        dgvEntries.EnableHeadersVisualStyles = false;
        dgvEntries.GridColor = Color.FromArgb(222, 226, 230);
        dgvEntries.Location = new Point(8, 33);
        dgvEntries.MultiSelect = false;
        dgvEntries.Name = "dgvEntries";
        dgvEntries.ReadOnly = true;
        dgvEntries.RowHeadersVisible = false;
        dgvEntries.RowHeadersWidth = 62;
        dgvEntries.RowTemplate.Height = 32;
        dgvEntries.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvEntries.Size = new Size(1424, 237);
        dgvEntries.TabIndex = 0;
        dgvEntries.CellDoubleClick += dgvEntries_CellDoubleClick;
        dgvEntries.CellFormatting += dgvEntries_CellFormatting;
        dgvEntries.SelectionChanged += dgvEntries_SelectionChanged;
        // 
        // cmsGrid
        // 
        cmsGrid.Font = new Font("Tahoma", 10F);
        cmsGrid.ImageScalingSize = new Size(24, 24);
        cmsGrid.Items.AddRange(new ToolStripItem[] { cmsEdit, cmsPost, cmsCancel, cmsDeleteDraft, cmsRefresh });
        cmsGrid.Name = "cmsGrid";
        cmsGrid.RightToLeft = RightToLeft.Yes;
        cmsGrid.Size = new Size(186, 154);
        // 
        // cmsEdit
        // 
        cmsEdit.Name = "cmsEdit";
        cmsEdit.Size = new Size(185, 30);
        cmsEdit.Text = "تعديل";
        cmsEdit.Click += cmsEdit_Click;
        // 
        // cmsPost
        // 
        cmsPost.Name = "cmsPost";
        cmsPost.Size = new Size(185, 30);
        cmsPost.Text = "ترحيل";
        cmsPost.Click += cmsPost_Click;
        // 
        // cmsCancel
        // 
        cmsCancel.Name = "cmsCancel";
        cmsCancel.Size = new Size(185, 30);
        cmsCancel.Text = "إلغاء القيد";
        cmsCancel.Click += cmsCancel_Click;
        // 
        // cmsDeleteDraft
        // 
        cmsDeleteDraft.Name = "cmsDeleteDraft";
        cmsDeleteDraft.Size = new Size(185, 30);
        cmsDeleteDraft.Text = "حذف مسودة";
        cmsDeleteDraft.Click += cmsDeleteDraft_Click;
        // 
        // cmsRefresh
        // 
        cmsRefresh.Name = "cmsRefresh";
        cmsRefresh.Size = new Size(185, 30);
        cmsRefresh.Text = "تحديث";
        cmsRefresh.Click += cmsRefresh_Click;
        // 
        // errorProvider
        // 
        errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        errorProvider.ContainerControl = this;
        errorProvider.RightToLeft = true;
        // 
        // frmJournalEntries
        // 
        AutoScaleDimensions = new SizeF(11F, 24F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(240, 242, 245);
        ClientSize = new Size(1440, 960);
        Controls.Add(pnlGrid);
        Controls.Add(pnlEditor);
        Controls.Add(pnlSummary);
        Controls.Add(pnlFilters);
        Controls.Add(gbCommands);
        Controls.Add(pnlHeader);
        Font = new Font("Tahoma", 10F);
        KeyPreview = true;
        Name = "frmJournalEntries";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterParent;
        Text = "القيود المحاسبية";
        Load += frmJournalEntries_Load;
        pnlHeader.ResumeLayout(false);
        gbCommands.ResumeLayout(false);
        pnlToolbar.ResumeLayout(false);
        pnlFilters.ResumeLayout(false);
        gbFilter.ResumeLayout(false);
        gbFilter.PerformLayout();
        pnlSummary.ResumeLayout(false);
        pnlEditor.ResumeLayout(false);
        gbLines.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvLines).EndInit();
        pnlLineEntry.ResumeLayout(false);
        pnlLineEntry.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numLineDebit).EndInit();
        ((System.ComponentModel.ISupportInitialize)numLineCredit).EndInit();
        pnlEntryHeader.ResumeLayout(false);
        pnlEntryHeader.PerformLayout();
        pnlGrid.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvEntries).EndInit();
        cmsGrid.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        ResumeLayout(false);
    }

    private System.Windows.Forms.Panel pnlHeader;
    private System.Windows.Forms.Label lblMode;
    private System.Windows.Forms.GroupBox gbCommands;
    private System.Windows.Forms.FlowLayoutPanel pnlToolbar;
    private System.Windows.Forms.Button btnNew;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.Button btnEdit;
    private System.Windows.Forms.Button btnPost;
    private System.Windows.Forms.Button btnCancelEntry;
    private System.Windows.Forms.Button btnDeleteDraft;
    private System.Windows.Forms.Button btnRefresh;
    private System.Windows.Forms.Button btnClose;
    private System.Windows.Forms.Panel pnlFilters;
    private System.Windows.Forms.GroupBox gbFilter;
    private System.Windows.Forms.Button btnToggleFilters;
    private System.Windows.Forms.Label lblFilterBy;
    private System.Windows.Forms.ComboBox cbFilterBy;
    private System.Windows.Forms.TextBox txtFilterValue;
    private System.Windows.Forms.Label lblEntryTypeFilter;
    private System.Windows.Forms.ComboBox cbEntryTypeFilter;
    private System.Windows.Forms.Label lblStatusFilter;
    private System.Windows.Forms.ComboBox cbStatusFilter;
    private System.Windows.Forms.Label lblFrom;
    private System.Windows.Forms.DateTimePicker dtpFrom;
    private System.Windows.Forms.Label lblTo;
    private System.Windows.Forms.DateTimePicker dtpTo;
    private System.Windows.Forms.Button btnRangeMonth;
    private System.Windows.Forms.CheckBox chkOnlyCancelled;
    private System.Windows.Forms.CheckBox chkOnlyDraft;
    private System.Windows.Forms.Button btnClearFilters;
    private System.Windows.Forms.GroupBox pnlSummary;
    private System.Windows.Forms.Label lblSummary;
    private System.Windows.Forms.Panel pnlEditor;
    private System.Windows.Forms.GroupBox gbLines;
    private System.Windows.Forms.DataGridView dgvLines;
    private System.Windows.Forms.DataGridViewTextBoxColumn colLineAccount;
    private System.Windows.Forms.DataGridViewTextBoxColumn colLineCode;
    private System.Windows.Forms.DataGridViewTextBoxColumn colLineDebit;
    private System.Windows.Forms.DataGridViewTextBoxColumn colLineCredit;
    private System.Windows.Forms.DataGridViewTextBoxColumn colLineDescription;
    private System.Windows.Forms.Label lblTotals;
    private System.Windows.Forms.Panel pnlLineEntry;
    private System.Windows.Forms.Label lblLineAccount;
    private System.Windows.Forms.ComboBox cbLineAccount;
    private System.Windows.Forms.Label lblLineDebit;
    private System.Windows.Forms.NumericUpDown numLineDebit;
    private System.Windows.Forms.Label lblLineCredit;
    private System.Windows.Forms.NumericUpDown numLineCredit;
    private System.Windows.Forms.Label lblLineDescription;
    private System.Windows.Forms.TextBox txtLineDescription;
    private System.Windows.Forms.Button btnAddLine;
    private System.Windows.Forms.Button btnRemoveLine;
    private System.Windows.Forms.GroupBox pnlEntryHeader;
    private System.Windows.Forms.Label lblEntryInfo;
    private System.Windows.Forms.Label lblEntryDate;
    private System.Windows.Forms.DateTimePicker dtpEntryDate;
    private System.Windows.Forms.Label lblEntryType;
    private System.Windows.Forms.ComboBox cbEntryType;
    private System.Windows.Forms.Label lblEntryDescription;
    private System.Windows.Forms.TextBox txtEntryDescription;
    private System.Windows.Forms.Label lblReferenceNumber;
    private System.Windows.Forms.TextBox txtReferenceNumber;
    private System.Windows.Forms.GroupBox pnlGrid;
    private System.Windows.Forms.Label lblEmptyState;
    private System.Windows.Forms.DataGridView dgvEntries;
    private System.Windows.Forms.DataGridViewTextBoxColumn colEntryNumber;
    private System.Windows.Forms.DataGridViewTextBoxColumn colEntryDate;
    private System.Windows.Forms.DataGridViewTextBoxColumn colEntryType;
    private System.Windows.Forms.DataGridViewTextBoxColumn colEntryDescription;
    private System.Windows.Forms.DataGridViewTextBoxColumn colReference;
    private System.Windows.Forms.DataGridViewTextBoxColumn colTotalDebit;
    private System.Windows.Forms.DataGridViewTextBoxColumn colTotalCredit;
    private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
    private System.Windows.Forms.ContextMenuStrip cmsGrid;
    private System.Windows.Forms.ToolStripMenuItem cmsEdit;
    private System.Windows.Forms.ToolStripMenuItem cmsPost;
    private System.Windows.Forms.ToolStripMenuItem cmsCancel;
    private System.Windows.Forms.ToolStripMenuItem cmsDeleteDraft;
    private System.Windows.Forms.ToolStripMenuItem cmsRefresh;
    private System.Windows.Forms.ErrorProvider errorProvider;
    private System.Windows.Forms.ToolTip toolTip;
    private System.Windows.Forms.ToolTip liveTip;
}
