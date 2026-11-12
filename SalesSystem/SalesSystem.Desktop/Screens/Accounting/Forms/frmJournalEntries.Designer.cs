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
        pnlHeader = new Panel();
        lblMode = new Label();
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
        pnlSummary = new Panel();
        lblSummary = new Label();
        pnlEditor = new Panel();
        gbLines = new GroupBox();
        dgvLines = new DataGridView();
        colLineAccount = new DataGridViewTextBoxColumn();
        colLineCode = new DataGridViewTextBoxColumn();
        colLineDebit = new DataGridViewTextBoxColumn();
        colLineCredit = new DataGridViewTextBoxColumn();
        colLineDescription = new DataGridViewTextBoxColumn();
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
        pnlEntryHeader = new Panel();
        lblEntryInfo = new Label();
        lblEntryDate = new Label();
        dtpEntryDate = new DateTimePicker();
        lblEntryType = new Label();
        cbEntryType = new ComboBox();
        lblEntryDescription = new Label();
        txtEntryDescription = new TextBox();
        lblReferenceNumber = new Label();
        txtReferenceNumber = new TextBox();
        pnlGrid = new Panel();
        lblEmptyState = new Label();
        dgvEntries = new DataGridView();
        colEntryNumber = new DataGridViewTextBoxColumn();
        colEntryDate = new DataGridViewTextBoxColumn();
        colEntryType = new DataGridViewTextBoxColumn();
        colEntryDescription = new DataGridViewTextBoxColumn();
        colReference = new DataGridViewTextBoxColumn();
        colTotalDebit = new DataGridViewTextBoxColumn();
        colTotalCredit = new DataGridViewTextBoxColumn();
        colStatus = new DataGridViewTextBoxColumn();
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
        pnlHeader.BackColor = Color.FromArgb(14, 165, 103);
        pnlHeader.Controls.Add(lblMode);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Location = new Point(0, 0);
        pnlHeader.Name = "pnlHeader";
        pnlHeader.Size = new Size(1440, 56);
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
        lblMode.Size = new Size(1440, 56);
        lblMode.TabIndex = 0;
        lblMode.Text = "قيد محاسبي جديد";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlToolbar
        // 
        pnlToolbar.BackColor = Color.FromArgb(235, 240, 235);
        pnlToolbar.Controls.Add(btnNew);
        pnlToolbar.Controls.Add(btnSave);
        pnlToolbar.Controls.Add(btnEdit);
        pnlToolbar.Controls.Add(btnPost);
        pnlToolbar.Controls.Add(btnCancelEntry);
        pnlToolbar.Controls.Add(btnDeleteDraft);
        pnlToolbar.Controls.Add(btnRefresh);
        pnlToolbar.Controls.Add(btnClose);
        pnlToolbar.Dock = DockStyle.Top;
        pnlToolbar.FlowDirection = FlowDirection.RightToLeft;
        pnlToolbar.Location = new Point(0, 56);
        pnlToolbar.Name = "pnlToolbar";
        pnlToolbar.Padding = new Padding(8, 0, 8, 0);
        pnlToolbar.Size = new Size(1440, 60);
        pnlToolbar.TabIndex = 1;
        pnlToolbar.WrapContents = false;
        // 
        // btnNew
        // 
        btnNew.BackColor = Color.FromArgb(40, 167, 69);
        btnNew.FlatStyle = FlatStyle.Flat;
        btnNew.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnNew.ForeColor = Color.White;
        btnNew.Location = new Point(1298, 11);
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
        btnSave.Location = new Point(1160, 11);
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
        btnEdit.Location = new Point(1022, 11);
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
        btnPost.Location = new Point(884, 11);
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
        btnCancelEntry.Location = new Point(746, 11);
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
        btnDeleteDraft.BackColor = Color.FromArgb(180, 40, 55);
        btnDeleteDraft.FlatStyle = FlatStyle.Flat;
        btnDeleteDraft.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnDeleteDraft.ForeColor = Color.White;
        btnDeleteDraft.Location = new Point(608, 11);
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
        btnRefresh.Location = new Point(470, 11);
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
        btnClose.Location = new Point(332, 11);
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
        pnlFilters.BackColor = Color.FromArgb(235, 240, 235);
        pnlFilters.Controls.Add(gbFilter);
        pnlFilters.Dock = DockStyle.Top;
        pnlFilters.Location = new Point(0, 116);
        pnlFilters.Name = "pnlFilters";
        pnlFilters.Padding = new Padding(8, 4, 8, 4);
        pnlFilters.Size = new Size(1440, 112);
        pnlFilters.TabIndex = 2;
        // 
        // gbFilter
        // 
        gbFilter.BackColor = Color.FromArgb(235, 245, 235);
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
        gbFilter.Dock = DockStyle.Fill;
        gbFilter.Font = new Font("Tahoma", 10F);
        gbFilter.Location = new Point(8, 4);
        gbFilter.Name = "gbFilter";
        gbFilter.Padding = new Padding(8);
        gbFilter.Size = new Size(1424, 104);
        gbFilter.TabIndex = 0;
        gbFilter.TabStop = false;
        gbFilter.Text = "بحث وتصفية";
        // 
        // lblFilterBy
        // 
        lblFilterBy.AutoSize = true;
        lblFilterBy.Location = new Point(20, 30);
        lblFilterBy.Name = "lblFilterBy";
        lblFilterBy.Size = new Size(75, 24);
        lblFilterBy.TabIndex = 0;
        lblFilterBy.Text = "تصفية بـ:";
        // 
        // cbFilterBy
        // 
        cbFilterBy.DropDownStyle = ComboBoxStyle.DropDownList;
        cbFilterBy.Font = new Font("Tahoma", 10F);
        cbFilterBy.Location = new Point(100, 26);
        cbFilterBy.Name = "cbFilterBy";
        cbFilterBy.Size = new Size(150, 28);
        cbFilterBy.TabIndex = 1;
        cbFilterBy.SelectedIndexChanged += cbFilterBy_SelectedIndexChanged;
        // 
        // txtFilterValue
        // 
        txtFilterValue.Location = new Point(258, 26);
        txtFilterValue.Name = "txtFilterValue";
        txtFilterValue.PlaceholderText = "بحث فوري — Esc يمسح التصفية...";
        txtFilterValue.Size = new Size(280, 28);
        txtFilterValue.TabIndex = 2;
        txtFilterValue.TextChanged += txtFilterValue_TextChanged;
        txtFilterValue.KeyDown += txtFilterValue_KeyDown;
        // 
        // lblEntryTypeFilter
        // 
        lblEntryTypeFilter.AutoSize = true;
        lblEntryTypeFilter.Location = new Point(548, 30);
        lblEntryTypeFilter.Name = "lblEntryTypeFilter";
        lblEntryTypeFilter.Size = new Size(55, 24);
        lblEntryTypeFilter.TabIndex = 3;
        lblEntryTypeFilter.Text = "النوع:";
        // 
        // cbEntryTypeFilter
        // 
        cbEntryTypeFilter.DropDownStyle = ComboBoxStyle.DropDownList;
        cbEntryTypeFilter.Font = new Font("Tahoma", 10F);
        cbEntryTypeFilter.Location = new Point(608, 26);
        cbEntryTypeFilter.Name = "cbEntryTypeFilter";
        cbEntryTypeFilter.Size = new Size(180, 28);
        cbEntryTypeFilter.TabIndex = 4;
        cbEntryTypeFilter.SelectedIndexChanged += cbEntryTypeFilter_SelectedIndexChanged;
        // 
        // lblStatusFilter
        // 
        lblStatusFilter.AutoSize = true;
        lblStatusFilter.Location = new Point(798, 30);
        lblStatusFilter.Name = "lblStatusFilter";
        lblStatusFilter.Size = new Size(62, 24);
        lblStatusFilter.TabIndex = 5;
        lblStatusFilter.Text = "الحالة:";
        // 
        // cbStatusFilter
        // 
        cbStatusFilter.DropDownStyle = ComboBoxStyle.DropDownList;
        cbStatusFilter.Font = new Font("Tahoma", 10F);
        cbStatusFilter.Location = new Point(868, 26);
        cbStatusFilter.Name = "cbStatusFilter";
        cbStatusFilter.Size = new Size(160, 28);
        cbStatusFilter.TabIndex = 6;
        cbStatusFilter.SelectedIndexChanged += cbStatusFilter_SelectedIndexChanged;
        // 
        // lblFrom
        // 
        lblFrom.AutoSize = true;
        lblFrom.Location = new Point(1038, 30);
        lblFrom.Name = "lblFrom";
        lblFrom.Size = new Size(40, 24);
        lblFrom.TabIndex = 7;
        lblFrom.Text = "من:";
        // 
        // dtpFrom
        // 
        dtpFrom.CustomFormat = "yyyy-MM-dd";
        dtpFrom.Font = new Font("Tahoma", 10F);
        dtpFrom.Format = DateTimePickerFormat.Custom;
        dtpFrom.Location = new Point(1086, 26);
        dtpFrom.Name = "dtpFrom";
        dtpFrom.Size = new Size(130, 28);
        dtpFrom.TabIndex = 8;
        dtpFrom.ValueChanged += dtpFrom_ValueChanged;
        // 
        // lblTo
        // 
        lblTo.AutoSize = true;
        lblTo.Location = new Point(1224, 30);
        lblTo.Name = "lblTo";
        lblTo.Size = new Size(35, 24);
        lblTo.TabIndex = 9;
        lblTo.Text = "إلى:";
        // 
        // dtpTo
        // 
        dtpTo.CustomFormat = "yyyy-MM-dd";
        dtpTo.Font = new Font("Tahoma", 10F);
        dtpTo.Format = DateTimePickerFormat.Custom;
        dtpTo.Location = new Point(1267, 26);
        dtpTo.Name = "dtpTo";
        dtpTo.Size = new Size(130, 28);
        dtpTo.TabIndex = 10;
        dtpTo.ValueChanged += dtpTo_ValueChanged;
        // 
        // btnRangeMonth
        // 
        btnRangeMonth.BackColor = Color.FromArgb(108, 117, 125);
        btnRangeMonth.FlatStyle = FlatStyle.Flat;
        btnRangeMonth.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        btnRangeMonth.ForeColor = Color.White;
        btnRangeMonth.Location = new Point(20, 62);
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
        chkOnlyCancelled.Location = new Point(150, 65);
        chkOnlyCancelled.Name = "chkOnlyCancelled";
        chkOnlyCancelled.Size = new Size(150, 24);
        chkOnlyCancelled.TabIndex = 12;
        chkOnlyCancelled.Text = "الملغاة فقط";
        chkOnlyCancelled.UseVisualStyleBackColor = true;
        chkOnlyCancelled.CheckedChanged += chkOnlyCancelled_CheckedChanged;
        // 
        // chkOnlyDraft
        // 
        chkOnlyDraft.AutoSize = true;
        chkOnlyDraft.Location = new Point(315, 65);
        chkOnlyDraft.Name = "chkOnlyDraft";
        chkOnlyDraft.Size = new Size(155, 24);
        chkOnlyDraft.TabIndex = 13;
        chkOnlyDraft.Text = "المسودات فقط";
        chkOnlyDraft.UseVisualStyleBackColor = true;
        chkOnlyDraft.CheckedChanged += chkOnlyDraft_CheckedChanged;
        // 
        // pnlSummary
        // 
        pnlSummary.BackColor = Color.FromArgb(235, 240, 235);
        pnlSummary.Controls.Add(lblSummary);
        pnlSummary.Dock = DockStyle.Bottom;
        pnlSummary.Location = new Point(0, 930);
        pnlSummary.Name = "pnlSummary";
        pnlSummary.Size = new Size(1440, 30);
        pnlSummary.TabIndex = 3;
        // 
        // lblSummary
        // 
        lblSummary.Dock = DockStyle.Fill;
        lblSummary.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblSummary.Location = new Point(0, 0);
        lblSummary.Name = "lblSummary";
        lblSummary.Size = new Size(1440, 30);
        lblSummary.TabIndex = 0;
        lblSummary.Text = "المعروض: 0";
        lblSummary.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlEditor
        // 
        pnlEditor.BackColor = Color.FromArgb(235, 240, 235);
        pnlEditor.Controls.Add(gbLines);
        pnlEditor.Controls.Add(pnlEntryHeader);
        pnlEditor.Dock = DockStyle.Bottom;
        pnlEditor.Location = new Point(0, 550);
        pnlEditor.Name = "pnlEditor";
        pnlEditor.Padding = new Padding(8);
        pnlEditor.Size = new Size(1440, 380);
        pnlEditor.TabIndex = 4;
        // 
        // gbLines
        // 
        gbLines.BackColor = Color.FromArgb(235, 245, 235);
        gbLines.Controls.Add(dgvLines);
        gbLines.Controls.Add(lblTotals);
        gbLines.Controls.Add(pnlLineEntry);
        gbLines.Dock = DockStyle.Fill;
        gbLines.Font = new Font("Tahoma", 10F);
        gbLines.Location = new Point(8, 92);
        gbLines.Name = "gbLines";
        gbLines.Padding = new Padding(8, 6, 8, 6);
        gbLines.Size = new Size(1424, 280);
        gbLines.TabIndex = 1;
        gbLines.TabStop = false;
        gbLines.Text = "بنود القيد";
        // 
        // dgvLines
        // 
        dgvLines.AllowUserToAddRows = false;
        dgvLines.AllowUserToDeleteRows = false;
        dgvLines.AllowUserToOrderColumns = true;
        dgvLines.AutoGenerateColumns = false;
        dgvLines.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvLines.BackgroundColor = Color.FromArgb(220, 240, 220);
        dgvLines.BorderStyle = BorderStyle.None;
        dgvLines.ColumnHeadersHeight = 40;
        dgvLines.Columns.AddRange(new DataGridViewColumn[] {
        colLineAccount,
        colLineCode,
        colLineDebit,
        colLineCredit,
        colLineDescription});
        dgvLines.Dock = DockStyle.Fill;
        dgvLines.Location = new Point(8, 70);
        dgvLines.MultiSelect = false;
        dgvLines.Name = "dgvLines";
        dgvLines.ReadOnly = true;
        dgvLines.RowHeadersVisible = false;
        dgvLines.RowTemplate.Height = 30;
        dgvLines.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvLines.Size = new Size(1408, 178);
        dgvLines.TabIndex = 2;
        // 
        // colLineAccount
        // 
        colLineAccount.DataPropertyName = "AccountName";
        colLineAccount.FillWeight = 260F;
        colLineAccount.HeaderText = "الحساب";
        colLineAccount.MinimumWidth = 120;
        colLineAccount.Name = "colLineAccount";
        colLineAccount.ReadOnly = true;
        // 
        // colLineCode
        // 
        colLineCode.DataPropertyName = "AccountCode";
        colLineCode.FillWeight = 90F;
        colLineCode.HeaderText = "الكود";
        colLineCode.MinimumWidth = 70;
        colLineCode.Name = "colLineCode";
        colLineCode.ReadOnly = true;
        // 
        // colLineDebit
        // 
        colLineDebit.DataPropertyName = "Debit";
        colLineDebit.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter, Format = "N2" };
        colLineDebit.FillWeight = 100F;
        colLineDebit.HeaderText = "مدين";
        colLineDebit.MinimumWidth = 90;
        colLineDebit.Name = "colLineDebit";
        colLineDebit.ReadOnly = true;
        // 
        // colLineCredit
        // 
        colLineCredit.DataPropertyName = "Credit";
        colLineCredit.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter, Format = "N2" };
        colLineCredit.FillWeight = 100F;
        colLineCredit.HeaderText = "دائن";
        colLineCredit.MinimumWidth = 90;
        colLineCredit.Name = "colLineCredit";
        colLineCredit.ReadOnly = true;
        // 
        // colLineDescription
        // 
        colLineDescription.DataPropertyName = "Description";
        colLineDescription.FillWeight = 220F;
        colLineDescription.HeaderText = "البيان";
        colLineDescription.MinimumWidth = 120;
        colLineDescription.Name = "colLineDescription";
        colLineDescription.ReadOnly = true;
        // 
        // lblTotals
        // 
        lblTotals.BackColor = Color.FromArgb(235, 240, 235);
        lblTotals.Dock = DockStyle.Bottom;
        lblTotals.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblTotals.Location = new Point(8, 248);
        lblTotals.Name = "lblTotals";
        lblTotals.Size = new Size(1408, 26);
        lblTotals.TabIndex = 1;
        lblTotals.Text = "مجموع المدين: 0.00 · مجموع الدائن: 0.00 · الفرق: 0.00";
        lblTotals.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlLineEntry
        // 
        pnlLineEntry.BackColor = Color.FromArgb(235, 240, 235);
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
        pnlLineEntry.Location = new Point(8, 26);
        pnlLineEntry.Name = "pnlLineEntry";
        pnlLineEntry.Size = new Size(1408, 44);
        pnlLineEntry.TabIndex = 0;
        // 
        // lblLineAccount
        // 
        lblLineAccount.AutoSize = true;
        lblLineAccount.Location = new Point(20, 10);
        lblLineAccount.Name = "lblLineAccount";
        lblLineAccount.Size = new Size(62, 24);
        lblLineAccount.TabIndex = 0;
        lblLineAccount.Text = "الحساب:";
        // 
        // cbLineAccount
        // 
        cbLineAccount.DropDownStyle = ComboBoxStyle.DropDownList;
        cbLineAccount.Font = new Font("Tahoma", 10F);
        cbLineAccount.Location = new Point(95, 8);
        cbLineAccount.Name = "cbLineAccount";
        cbLineAccount.Size = new Size(290, 28);
        cbLineAccount.TabIndex = 1;
        // 
        // lblLineDebit
        // 
        lblLineDebit.AutoSize = true;
        lblLineDebit.Location = new Point(400, 10);
        lblLineDebit.Name = "lblLineDebit";
        lblLineDebit.Size = new Size(48, 24);
        lblLineDebit.TabIndex = 2;
        lblLineDebit.Text = "مدين:";
        // 
        // numLineDebit
        // 
        numLineDebit.BackColor = Color.White;
        numLineDebit.DecimalPlaces = 2;
        numLineDebit.Font = new Font("Tahoma", 10F);
        numLineDebit.Location = new Point(460, 8);
        numLineDebit.Maximum = new decimal(new int[] { 999999999, 0, 0, 0 });
        numLineDebit.Name = "numLineDebit";
        numLineDebit.Size = new Size(115, 28);
        numLineDebit.TabIndex = 3;
        numLineDebit.TextAlign = HorizontalAlignment.Center;
        numLineDebit.ThousandsSeparator = true;
        // 
        // lblLineCredit
        // 
        lblLineCredit.AutoSize = true;
        lblLineCredit.Location = new Point(585, 10);
        lblLineCredit.Name = "lblLineCredit";
        lblLineCredit.Size = new Size(46, 24);
        lblLineCredit.TabIndex = 4;
        lblLineCredit.Text = "دائن:";
        // 
        // numLineCredit
        // 
        numLineCredit.BackColor = Color.White;
        numLineCredit.DecimalPlaces = 2;
        numLineCredit.Font = new Font("Tahoma", 10F);
        numLineCredit.Location = new Point(645, 8);
        numLineCredit.Maximum = new decimal(new int[] { 999999999, 0, 0, 0 });
        numLineCredit.Name = "numLineCredit";
        numLineCredit.Size = new Size(115, 28);
        numLineCredit.TabIndex = 5;
        numLineCredit.TextAlign = HorizontalAlignment.Center;
        numLineCredit.ThousandsSeparator = true;
        // 
        // lblLineDescription
        // 
        lblLineDescription.AutoSize = true;
        lblLineDescription.Location = new Point(770, 10);
        lblLineDescription.Name = "lblLineDescription";
        lblLineDescription.Size = new Size(55, 24);
        lblLineDescription.TabIndex = 6;
        lblLineDescription.Text = "البيان:";
        // 
        // txtLineDescription
        // 
        txtLineDescription.Location = new Point(830, 8);
        txtLineDescription.Name = "txtLineDescription";
        txtLineDescription.PlaceholderText = "بيان البند (اختياري)";
        txtLineDescription.Size = new Size(330, 28);
        txtLineDescription.TabIndex = 7;
        // 
        // btnAddLine
        // 
        btnAddLine.BackColor = Color.FromArgb(40, 167, 69);
        btnAddLine.FlatStyle = FlatStyle.Flat;
        btnAddLine.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        btnAddLine.ForeColor = Color.White;
        btnAddLine.Location = new Point(1175, 7);
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
        btnRemoveLine.Location = new Point(1288, 7);
        btnRemoveLine.Name = "btnRemoveLine";
        btnRemoveLine.Size = new Size(110, 30);
        btnRemoveLine.TabIndex = 9;
        btnRemoveLine.Text = "حذف البند";
        btnRemoveLine.UseVisualStyleBackColor = false;
        btnRemoveLine.Click += btnRemoveLine_Click;
        // 
        // pnlEntryHeader
        // 
        pnlEntryHeader.BackColor = Color.FromArgb(235, 240, 235);
        pnlEntryHeader.Controls.Add(lblEntryInfo);
        pnlEntryHeader.Controls.Add(lblEntryDate);
        pnlEntryHeader.Controls.Add(dtpEntryDate);
        pnlEntryHeader.Controls.Add(lblEntryType);
        pnlEntryHeader.Controls.Add(cbEntryType);
        pnlEntryHeader.Controls.Add(lblEntryDescription);
        pnlEntryHeader.Controls.Add(txtEntryDescription);
        pnlEntryHeader.Controls.Add(lblReferenceNumber);
        pnlEntryHeader.Controls.Add(txtReferenceNumber);
        pnlEntryHeader.Dock = DockStyle.Top;
        pnlEntryHeader.Location = new Point(8, 8);
        pnlEntryHeader.Name = "pnlEntryHeader";
        pnlEntryHeader.Size = new Size(1424, 84);
        pnlEntryHeader.TabIndex = 0;
        // 
        // lblEntryInfo
        // 
        lblEntryInfo.AutoSize = true;
        lblEntryInfo.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblEntryInfo.Location = new Point(20, 2);
        lblEntryInfo.Name = "lblEntryInfo";
        lblEntryInfo.Size = new Size(300, 22);
        lblEntryInfo.TabIndex = 0;
        lblEntryInfo.Text = "رقم القيد: — · الحالة: —";
        // 
        // lblEntryDate
        // 
        lblEntryDate.AutoSize = true;
        lblEntryDate.Location = new Point(20, 30);
        lblEntryDate.Name = "lblEntryDate";
        lblEntryDate.Size = new Size(58, 24);
        lblEntryDate.TabIndex = 1;
        lblEntryDate.Text = "التاريخ:";
        // 
        // dtpEntryDate
        // 
        dtpEntryDate.CustomFormat = "yyyy-MM-dd";
        dtpEntryDate.Font = new Font("Tahoma", 10F);
        dtpEntryDate.Format = DateTimePickerFormat.Custom;
        dtpEntryDate.Location = new Point(90, 26);
        dtpEntryDate.Name = "dtpEntryDate";
        dtpEntryDate.Size = new Size(140, 28);
        dtpEntryDate.TabIndex = 2;
        // 
        // lblEntryType
        // 
        lblEntryType.AutoSize = true;
        lblEntryType.Location = new Point(250, 30);
        lblEntryType.Name = "lblEntryType";
        lblEntryType.Size = new Size(78, 24);
        lblEntryType.TabIndex = 3;
        lblEntryType.Text = "نوع القيد:";
        // 
        // cbEntryType
        // 
        cbEntryType.DropDownStyle = ComboBoxStyle.DropDownList;
        cbEntryType.Font = new Font("Tahoma", 10F);
        cbEntryType.Location = new Point(340, 26);
        cbEntryType.Name = "cbEntryType";
        cbEntryType.Size = new Size(170, 28);
        cbEntryType.TabIndex = 4;
        // 
        // lblEntryDescription
        // 
        lblEntryDescription.AutoSize = true;
        lblEntryDescription.Location = new Point(530, 30);
        lblEntryDescription.Name = "lblEntryDescription";
        lblEntryDescription.Size = new Size(55, 24);
        lblEntryDescription.TabIndex = 5;
        lblEntryDescription.Text = "البيان:";
        // 
        // txtEntryDescription
        // 
        txtEntryDescription.Location = new Point(595, 26);
        txtEntryDescription.Name = "txtEntryDescription";
        txtEntryDescription.PlaceholderText = "بيان القيد المحاسبي (مطلوب)";
        txtEntryDescription.Size = new Size(470, 28);
        txtEntryDescription.TabIndex = 6;
        // 
        // lblReferenceNumber
        // 
        lblReferenceNumber.AutoSize = true;
        lblReferenceNumber.Location = new Point(1080, 30);
        lblReferenceNumber.Name = "lblReferenceNumber";
        lblReferenceNumber.Size = new Size(58, 24);
        lblReferenceNumber.TabIndex = 7;
        lblReferenceNumber.Text = "المرجع:";
        // 
        // txtReferenceNumber
        // 
        txtReferenceNumber.Location = new Point(1145, 26);
        txtReferenceNumber.Name = "txtReferenceNumber";
        txtReferenceNumber.PlaceholderText = "رقم المستند المرجعي (اختياري)";
        txtReferenceNumber.Size = new Size(250, 28);
        txtReferenceNumber.TabIndex = 8;
        // 
        // pnlGrid
        // 
        pnlGrid.BackColor = Color.FromArgb(220, 240, 220);
        pnlGrid.Controls.Add(lblEmptyState);
        pnlGrid.Controls.Add(dgvEntries);
        pnlGrid.Dock = DockStyle.Fill;
        pnlGrid.Location = new Point(0, 228);
        pnlGrid.Name = "pnlGrid";
        pnlGrid.Padding = new Padding(8);
        pnlGrid.Size = new Size(1440, 322);
        pnlGrid.TabIndex = 5;
        // 
        // lblEmptyState
        // 
        lblEmptyState.Dock = DockStyle.Fill;
        lblEmptyState.Font = new Font("Tahoma", 12F, FontStyle.Bold);
        lblEmptyState.Location = new Point(8, 8);
        lblEmptyState.Name = "lblEmptyState";
        lblEmptyState.Size = new Size(1424, 306);
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
        dgvEntries.AutoGenerateColumns = false;
        dgvEntries.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvEntries.BackgroundColor = Color.FromArgb(220, 240, 220);
        dgvEntries.BorderStyle = BorderStyle.None;
        dgvEntries.ColumnHeadersHeight = 45;
        dgvEntries.Columns.AddRange(new DataGridViewColumn[] {
        colEntryNumber,
        colEntryDate,
        colEntryType,
        colEntryDescription,
        colReference,
        colTotalDebit,
        colTotalCredit,
        colStatus});
        dgvEntries.ContextMenuStrip = cmsGrid;
        dgvEntries.Dock = DockStyle.Fill;
        dgvEntries.Location = new Point(8, 8);
        dgvEntries.MultiSelect = false;
        dgvEntries.Name = "dgvEntries";
        dgvEntries.ReadOnly = true;
        dgvEntries.RowHeadersVisible = false;
        dgvEntries.RowTemplate.Height = 32;
        dgvEntries.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvEntries.Size = new Size(1424, 306);
        dgvEntries.TabIndex = 0;
        dgvEntries.CellDoubleClick += dgvEntries_CellDoubleClick;
        dgvEntries.CellFormatting += dgvEntries_CellFormatting;
        dgvEntries.SelectionChanged += dgvEntries_SelectionChanged;
        // 
        // colEntryNumber
        // 
        colEntryNumber.DataPropertyName = "EntryNumber";
        colEntryNumber.FillWeight = 90F;
        colEntryNumber.HeaderText = "رقم القيد";
        colEntryNumber.MinimumWidth = 80;
        colEntryNumber.Name = "colEntryNumber";
        colEntryNumber.ReadOnly = true;
        // 
        // colEntryDate
        // 
        colEntryDate.DataPropertyName = "EntryDate";
        colEntryDate.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter, Format = "yyyy-MM-dd" };
        colEntryDate.FillWeight = 100F;
        colEntryDate.HeaderText = "التاريخ";
        colEntryDate.MinimumWidth = 90;
        colEntryDate.Name = "colEntryDate";
        colEntryDate.ReadOnly = true;
        // 
        // colEntryType
        // 
        colEntryType.DataPropertyName = "EntryType";
        colEntryType.FillWeight = 100F;
        colEntryType.HeaderText = "النوع";
        colEntryType.MinimumWidth = 90;
        colEntryType.Name = "colEntryType";
        colEntryType.ReadOnly = true;
        // 
        // colEntryDescription
        // 
        colEntryDescription.DataPropertyName = "Description";
        colEntryDescription.FillWeight = 260F;
        colEntryDescription.HeaderText = "البيان";
        colEntryDescription.MinimumWidth = 150;
        colEntryDescription.Name = "colEntryDescription";
        colEntryDescription.ReadOnly = true;
        // 
        // colReference
        // 
        colReference.DataPropertyName = "Reference";
        colReference.FillWeight = 110F;
        colReference.HeaderText = "المرجع";
        colReference.MinimumWidth = 90;
        colReference.Name = "colReference";
        colReference.ReadOnly = true;
        // 
        // colTotalDebit
        // 
        colTotalDebit.DataPropertyName = "TotalDebit";
        colTotalDebit.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter, Format = "N2" };
        colTotalDebit.FillWeight = 100F;
        colTotalDebit.HeaderText = "مدين";
        colTotalDebit.MinimumWidth = 90;
        colTotalDebit.Name = "colTotalDebit";
        colTotalDebit.ReadOnly = true;
        // 
        // colTotalCredit
        // 
        colTotalCredit.DataPropertyName = "TotalCredit";
        colTotalCredit.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter, Format = "N2" };
        colTotalCredit.FillWeight = 100F;
        colTotalCredit.HeaderText = "دائن";
        colTotalCredit.MinimumWidth = 90;
        colTotalCredit.Name = "colTotalCredit";
        colTotalCredit.ReadOnly = true;
        // 
        // colStatus
        // 
        colStatus.DataPropertyName = "StatusDisplay";
        colStatus.FillWeight = 90F;
        colStatus.HeaderText = "الحالة";
        colStatus.MinimumWidth = 80;
        colStatus.Name = "colStatus";
        colStatus.ReadOnly = true;
        // 
        // cmsGrid
        // 
        cmsGrid.Font = new Font("Tahoma", 10F);
        cmsGrid.Items.AddRange(new ToolStripItem[] { cmsEdit, cmsPost, cmsCancel, cmsDeleteDraft, cmsRefresh });
        cmsGrid.Name = "cmsGrid";
        cmsGrid.RightToLeft = RightToLeft.Yes;
        cmsGrid.Size = new Size(170, 124);
        // 
        // cmsEdit
        // 
        cmsEdit.Name = "cmsEdit";
        cmsEdit.Size = new Size(169, 24);
        cmsEdit.Text = "تعديل";
        cmsEdit.Click += cmsEdit_Click;
        // 
        // cmsPost
        // 
        cmsPost.Name = "cmsPost";
        cmsPost.Size = new Size(169, 24);
        cmsPost.Text = "ترحيل";
        cmsPost.Click += cmsPost_Click;
        // 
        // cmsCancel
        // 
        cmsCancel.Name = "cmsCancel";
        cmsCancel.Size = new Size(169, 24);
        cmsCancel.Text = "إلغاء القيد";
        cmsCancel.Click += cmsCancel_Click;
        // 
        // cmsDeleteDraft
        // 
        cmsDeleteDraft.Name = "cmsDeleteDraft";
        cmsDeleteDraft.Size = new Size(169, 24);
        cmsDeleteDraft.Text = "حذف مسودة";
        cmsDeleteDraft.Click += cmsDeleteDraft_Click;
        // 
        // cmsRefresh
        // 
        cmsRefresh.Name = "cmsRefresh";
        cmsRefresh.Size = new Size(169, 24);
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
        AutoScaleDimensions = new SizeF(9F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(170, 220, 170);
        ClientSize = new Size(1440, 960);
        Controls.Add(pnlGrid);
        Controls.Add(pnlEditor);
        Controls.Add(pnlSummary);
        Controls.Add(pnlFilters);
        Controls.Add(pnlToolbar);
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
    private System.Windows.Forms.Panel pnlSummary;
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
    private System.Windows.Forms.Panel pnlEntryHeader;
    private System.Windows.Forms.Label lblEntryInfo;
    private System.Windows.Forms.Label lblEntryDate;
    private System.Windows.Forms.DateTimePicker dtpEntryDate;
    private System.Windows.Forms.Label lblEntryType;
    private System.Windows.Forms.ComboBox cbEntryType;
    private System.Windows.Forms.Label lblEntryDescription;
    private System.Windows.Forms.TextBox txtEntryDescription;
    private System.Windows.Forms.Label lblReferenceNumber;
    private System.Windows.Forms.TextBox txtReferenceNumber;
    private System.Windows.Forms.Panel pnlGrid;
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
