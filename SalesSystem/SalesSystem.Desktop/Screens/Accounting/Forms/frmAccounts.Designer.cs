namespace SalesSystem.Desktop.Screens.Accounting.Forms;

partial class frmAccounts
{
    private System.ComponentModel.IContainer components = null;

    // RULE-N30: تنظيف المكوّنات (cmsGrid · toolTip) المملوكة للحاوية.
    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    // ملاحظات المصمّم:
    //   • المصمّم = التشغيل (RULE-N134 / RULE-N140 / RULE-N204): كل لون بصيغة Convert.ToInt32
    //     وبالقيمة الحيّة للوحة (الجيل الرابع — الأخضر): خلفية النموذج (170,220,170) ·
    //     شريط العنوان (14,165,103) · رؤوس الشبكة (14,165,103) · التحديد (255,255,180) ·
    //     الأسطر المتبادلة (245,252,245) · خطوط الشبكة (205,225,205) · النص المميّز (14,82,52).
    //     ولا مرجع لأي عضو في Helpers/ داخل هذا الملف (RULE-N128).
    //   • الهيكل (§2.34 / RULE-N135…N138) من أعلى لأسفل، كل كتلة صندوق واحد بعنوان
    //     ولا صندوق داخل صندوق (RULE-N137):
    //       (1) pnlHeader        «شريط العنوان»  (lblMode)
    //       (2) gbCommands       «أوامر الحسابات» — ctrlCrudToolbar المشترك + صف «حذف نهائي»
    //       (3) pnlFilters       «بحث وتصفية»    — gbFilter كما هو + زر الطيّ
    //       (4) pnlBody          المنطقة الوسطى  — شجرة الحسابات (يسار) + محرّر الحساب (الباقي)
    //       (5) pnlGrid          «بيانات الحسابات» — الشبكة
    //       (6) pnlSummary       «الإجماليات»
    //   • RULE-N125 (ترتيب الإرساء): تُرتَّب الاستدعاءات Fill أولاً ثم Bottom (الداخلي ← الخارجي)
    //     ثم Top (الداخلي ← الخارجي) — لأن آخر ما يُضاف يُرسى أولاً:
    //       pnlBody(Fill) · pnlGrid(Bottom) · pnlSummary(Bottom) ·
    //       pnlFilters(Top) · gbCommands(Top) · pnlHeader(Top)  ← النتيجة: العنوان أعلى،
    //       ثم الأوامر، ثم التصفية، ثم الجسم، ثم الشبكة، ثم الملخص في القاع.
    //   • RULE-N137: العنصران المضمَّنان (ctrlTreeOfAccounts1 · ctrlAccountEditor1) يعيشان
    //     داخل **لوحة** لا داخل GroupBox: كلاهما يملك صناديقه المعنونة بنفسه
    //     («شجرة الحسابات» · «بيانات الحساب») — فالصندوق حولهما كان سيُكرّر العنوان
    //     ويصنع صندوقاً داخل صندوق.
    //   • pnlEditorHost.AutoScroll: المحرّر محتواه 640 بكسل؛ اللوحة القابلة للتمرير تُظهر
    //     كل حقوله في النافذة الصغيرة بلا قطع (RULE-N129 لا علاقة له: لا طبقة شفافة).
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
        pnlHeader = new Panel();
        lblMode = new Label();
        gbCommands = new GroupBox();
        flpSecondary = new FlowLayoutPanel();
        btnPermanentDelete = new Button();
        ctrlCrudToolbar1 = new SalesSystem.Desktop.Controls.Common.ctrlCrudToolbar();
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
        pnlBody = new Panel();
        pnlEditorHost = new Panel();
        ctrlAccountEditor1 = new SalesSystem.Desktop.Screens.Accounting.Controls.ctrlAccountEditor();
        pnlTreeHost = new Panel();
        ctrlTreeOfAccounts1 = new SalesSystem.Desktop.Screens.Accounting.Controls.ctrlTreeOfAccounts();
        pnlGrid = new GroupBox();
        lblEmptyState = new Label();
        dgvAccounts = new DataGridView();
        colCode = new DataGridViewTextBoxColumn();
        colNameAr = new DataGridViewTextBoxColumn();
        colNameEn = new DataGridViewTextBoxColumn();
        colNature = new DataGridViewTextBoxColumn();
        colLevel = new DataGridViewTextBoxColumn();
        colParent = new DataGridViewTextBoxColumn();
        colTreePath = new DataGridViewTextBoxColumn();
        colBalance = new DataGridViewTextBoxColumn();
        colFinancialStatement = new DataGridViewTextBoxColumn();
        colCategory = new DataGridViewTextBoxColumn();
        colIsLeaf = new DataGridViewTextBoxColumn();
        colIsSystem = new DataGridViewTextBoxColumn();
        colStatus = new DataGridViewTextBoxColumn();
        pnlSummary = new GroupBox();
        lblSummary = new Label();
        cmsGrid = new ContextMenuStrip(components);
        cmsEdit = new ToolStripMenuItem();
        cmsDelete = new ToolStripMenuItem();
        cmsRefresh = new ToolStripMenuItem();
        toolTip = new ToolTip(components);
        pnlHeader.SuspendLayout();
        gbCommands.SuspendLayout();
        flpSecondary.SuspendLayout();
        pnlFilters.SuspendLayout();
        gbFilter.SuspendLayout();
        pnlBody.SuspendLayout();
        pnlEditorHost.SuspendLayout();
        pnlTreeHost.SuspendLayout();
        pnlGrid.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvAccounts).BeginInit();
        pnlSummary.SuspendLayout();
        cmsGrid.SuspendLayout();
        SuspendLayout();
        // 
        // pnlHeader
        // 
        pnlHeader.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
        pnlHeader.Controls.Add(lblMode);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Location = new Point(0, 0);
        pnlHeader.Name = "pnlHeader";
        pnlHeader.Size = new Size(1432, 56);
        pnlHeader.TabIndex = 5;
        // 
        // lblMode
        // 
        lblMode.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
        lblMode.Dock = DockStyle.Fill;
        lblMode.Font = new Font("Tahoma", 14F, FontStyle.Bold);
        lblMode.ForeColor = System.Drawing.Color.White;
        lblMode.Location = new Point(0, 0);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(1432, 56);
        lblMode.TabIndex = 0;
        lblMode.Text = "دليل الحسابات — وضع العرض";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // gbCommands
        // 
        gbCommands.BackColor = System.Drawing.Color.White;
        gbCommands.Controls.Add(flpSecondary);
        gbCommands.Controls.Add(ctrlCrudToolbar1);
        gbCommands.Dock = DockStyle.Top;
        gbCommands.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        gbCommands.Location = new Point(0, 56);
        gbCommands.Name = "gbCommands";
        gbCommands.Size = new Size(1432, 118);
        gbCommands.TabIndex = 4;
        gbCommands.TabStop = false;
        gbCommands.Text = "أوامر الحسابات";
        // 
        // flpSecondary
        // 
        flpSecondary.BackColor = System.Drawing.Color.White;
        flpSecondary.Controls.Add(btnPermanentDelete);
        flpSecondary.Dock = DockStyle.Top;
        flpSecondary.FlowDirection = FlowDirection.RightToLeft;
        flpSecondary.Location = new Point(3, 72);
        flpSecondary.Name = "flpSecondary";
        flpSecondary.Padding = new Padding(8, 2, 8, 0);
        flpSecondary.Size = new Size(1426, 42);
        flpSecondary.TabIndex = 1;
        flpSecondary.WrapContents = false;
        // 
        // btnPermanentDelete
        // 
        btnPermanentDelete.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(220), Convert.ToInt32(53), Convert.ToInt32(69));
        btnPermanentDelete.Cursor = Cursors.Hand;
        btnPermanentDelete.FlatAppearance.BorderSize = 0;
        btnPermanentDelete.FlatStyle = FlatStyle.Flat;
        btnPermanentDelete.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        btnPermanentDelete.ForeColor = System.Drawing.Color.White;
        btnPermanentDelete.Location = new Point(1288, 2);
        btnPermanentDelete.Margin = new Padding(4, 0, 0, 0);
        btnPermanentDelete.Name = "btnPermanentDelete";
        btnPermanentDelete.Size = new Size(130, 34);
        btnPermanentDelete.TabIndex = 0;
        btnPermanentDelete.Text = "حذف نهائي";
        toolTip.SetToolTip(btnPermanentDelete, "حذف الحساب المحدد نهائياً — غير النظامي فقط، وبعد تأكيد صريح");
        btnPermanentDelete.UseVisualStyleBackColor = false;
        btnPermanentDelete.Click += btnPermanentDelete_Click;
        // 
        // ctrlCrudToolbar1
        // 
        ctrlCrudToolbar1.Dock = DockStyle.Top;
        ctrlCrudToolbar1.Font = new Font("Tahoma", 10F);
        ctrlCrudToolbar1.Location = new Point(3, 26);
        ctrlCrudToolbar1.Name = "ctrlCrudToolbar1";
        ctrlCrudToolbar1.RightToLeft = RightToLeft.Yes;
        ctrlCrudToolbar1.Size = new Size(1426, 46);
        ctrlCrudToolbar1.TabIndex = 0;
        // 
        // pnlFilters
        // 
        pnlFilters.BackColor = System.Drawing.Color.White;
        pnlFilters.Controls.Add(gbFilter);
        pnlFilters.Controls.Add(btnToggleFilters);
        pnlFilters.Dock = DockStyle.Top;
        pnlFilters.Location = new Point(0, 174);
        pnlFilters.Name = "pnlFilters";
        pnlFilters.Padding = new Padding(8, 4, 8, 4);
        pnlFilters.Size = new Size(1432, 104);
        pnlFilters.TabIndex = 3;
        // 
        // gbFilter
        // 
        gbFilter.BackColor = System.Drawing.Color.White;
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
        cbFilterBy.BackColor = System.Drawing.Color.White;
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
        txtFilterValue.BackColor = System.Drawing.Color.White;
        txtFilterValue.Font = new Font("Tahoma", 10F);
        txtFilterValue.Location = new Point(540, 21);
        txtFilterValue.Name = "txtFilterValue";
        txtFilterValue.PlaceholderText = "اكتب نص البحث...";
        txtFilterValue.Size = new Size(240, 32);
        txtFilterValue.TabIndex = 2;
        toolTip.SetToolTip(txtFilterValue, "بحث فوري — Enter يطبّق وEsc يمسح كل التصفية");
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
        cbNatureFilter.BackColor = System.Drawing.Color.White;
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
        btnClearFilters.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(108), Convert.ToInt32(117), Convert.ToInt32(125));
        btnClearFilters.Cursor = Cursors.Hand;
        btnClearFilters.Enabled = false;
        btnClearFilters.FlatAppearance.BorderSize = 0;
        btnClearFilters.FlatStyle = FlatStyle.Flat;
        btnClearFilters.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        btnClearFilters.ForeColor = System.Drawing.Color.White;
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
        btnToggleFilters.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(248), Convert.ToInt32(249), Convert.ToInt32(250));
        btnToggleFilters.Cursor = Cursors.Hand;
        btnToggleFilters.Dock = DockStyle.Left;
        btnToggleFilters.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(Convert.ToInt32(222), Convert.ToInt32(226), Convert.ToInt32(230));
        btnToggleFilters.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(240), Convert.ToInt32(240), Convert.ToInt32(240));
        btnToggleFilters.FlatStyle = FlatStyle.Popup;
        btnToggleFilters.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        btnToggleFilters.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        btnToggleFilters.Location = new Point(8, 4);
        btnToggleFilters.Name = "btnToggleFilters";
        btnToggleFilters.Size = new Size(140, 96);
        btnToggleFilters.TabIndex = 20;
        btnToggleFilters.Text = "إخفاء التصفية";
        btnToggleFilters.UseVisualStyleBackColor = false;
        // 
        // pnlBody
        // 
        pnlBody.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
        pnlBody.Controls.Add(pnlEditorHost);
        pnlBody.Controls.Add(pnlTreeHost);
        pnlBody.Dock = DockStyle.Fill;
        pnlBody.Location = new Point(0, 278);
        pnlBody.Name = "pnlBody";
        pnlBody.Size = new Size(1432, 350);
        pnlBody.TabIndex = 2;
        // 
        // pnlEditorHost
        // 
        pnlEditorHost.AutoScroll = true;
        pnlEditorHost.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
        pnlEditorHost.Controls.Add(ctrlAccountEditor1);
        pnlEditorHost.Dock = DockStyle.Fill;
        pnlEditorHost.Location = new Point(360, 0);
        pnlEditorHost.Name = "pnlEditorHost";
        pnlEditorHost.Size = new Size(1072, 350);
        pnlEditorHost.TabIndex = 0;
        // 
        // ctrlAccountEditor1
        // 
        ctrlAccountEditor1.Dock = DockStyle.Top;
        ctrlAccountEditor1.Font = new Font("Tahoma", 10F);
        ctrlAccountEditor1.Location = new Point(0, 0);
        ctrlAccountEditor1.Name = "ctrlAccountEditor1";
        ctrlAccountEditor1.RightToLeft = RightToLeft.Yes;
        ctrlAccountEditor1.Size = new Size(1072, 640);
        ctrlAccountEditor1.TabIndex = 0;
        // 
        // pnlTreeHost
        // 
        pnlTreeHost.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
        pnlTreeHost.Controls.Add(ctrlTreeOfAccounts1);
        pnlTreeHost.Dock = DockStyle.Left;
        pnlTreeHost.Location = new Point(0, 0);
        pnlTreeHost.Name = "pnlTreeHost";
        pnlTreeHost.Padding = new Padding(0, 0, 4, 0);
        pnlTreeHost.Size = new Size(360, 350);
        pnlTreeHost.TabIndex = 1;
        // 
        // ctrlTreeOfAccounts1
        // 
        ctrlTreeOfAccounts1.Dock = DockStyle.Fill;
        ctrlTreeOfAccounts1.Font = new Font("Tahoma", 10F);
        ctrlTreeOfAccounts1.Location = new Point(0, 0);
        ctrlTreeOfAccounts1.Name = "ctrlTreeOfAccounts1";
        ctrlTreeOfAccounts1.RightToLeft = RightToLeft.Yes;
        ctrlTreeOfAccounts1.ShowCrudActions = false;
        ctrlTreeOfAccounts1.Size = new Size(356, 350);
        ctrlTreeOfAccounts1.TabIndex = 0;
        // 
        // pnlGrid
        // 
        pnlGrid.BackColor = System.Drawing.Color.White;
        pnlGrid.Controls.Add(lblEmptyState);
        pnlGrid.Controls.Add(dgvAccounts);
        pnlGrid.Dock = DockStyle.Bottom;
        pnlGrid.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        pnlGrid.Location = new Point(0, 628);
        pnlGrid.Name = "pnlGrid";
        pnlGrid.Padding = new Padding(8);
        pnlGrid.Size = new Size(1432, 220);
        pnlGrid.TabIndex = 1;
        pnlGrid.TabStop = false;
        pnlGrid.Text = "بيانات الحسابات";
        // 
        // lblEmptyState
        // 
        lblEmptyState.BackColor = System.Drawing.Color.White;
        lblEmptyState.Dock = DockStyle.Fill;
        lblEmptyState.Font = new Font("Tahoma", 12F, FontStyle.Bold);
        lblEmptyState.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
        lblEmptyState.Location = new Point(8, 33);
        lblEmptyState.Name = "lblEmptyState";
        lblEmptyState.Size = new Size(1416, 179);
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
        dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(245), Convert.ToInt32(252), Convert.ToInt32(245));
        dgvAccounts.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
        dgvAccounts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvAccounts.AutoGenerateColumns = false;
        dgvAccounts.BackgroundColor = System.Drawing.Color.FromArgb(Convert.ToInt32(220), Convert.ToInt32(240), Convert.ToInt32(220));
        dgvAccounts.BorderStyle = BorderStyle.None;
        dgvAccounts.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
        dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
        dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
        dataGridViewCellStyle2.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
        dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
        dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
        dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
        dgvAccounts.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
        dgvAccounts.ColumnHeadersHeight = 45;
        dgvAccounts.Columns.AddRange(new DataGridViewColumn[] { colCode, colNameAr, colNameEn, colNature, colLevel, colParent, colTreePath, colBalance, colFinancialStatement, colCategory, colIsLeaf, colIsSystem, colStatus });
        dgvAccounts.ContextMenuStrip = cmsGrid;
        dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
        dataGridViewCellStyle3.Font = new Font("Tahoma", 10F);
        dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(255), Convert.ToInt32(255), Convert.ToInt32(180));
        dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
        dgvAccounts.DefaultCellStyle = dataGridViewCellStyle3;
        dgvAccounts.Dock = DockStyle.Fill;
        dgvAccounts.EnableHeadersVisualStyles = false;
        dgvAccounts.GridColor = System.Drawing.Color.FromArgb(Convert.ToInt32(205), Convert.ToInt32(225), Convert.ToInt32(205));
        dgvAccounts.Location = new Point(8, 33);
        dgvAccounts.MultiSelect = false;
        dgvAccounts.Name = "dgvAccounts";
        dgvAccounts.ReadOnly = true;
        dgvAccounts.RowHeadersVisible = false;
        dgvAccounts.RowHeadersWidth = 62;
        dgvAccounts.RowTemplate.Height = 32;
        dgvAccounts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvAccounts.Size = new Size(1416, 179);
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
        colCode.Width = 80;
        // 
        // colNameAr
        // 
        colNameAr.DataPropertyName = "NameArIndented";
        colNameAr.FillWeight = 150F;
        colNameAr.HeaderText = "الاسم العربي";
        colNameAr.MinimumWidth = 120;
        colNameAr.Name = "colNameAr";
        colNameAr.Width = 180;
        // 
        // colNameEn
        // 
        colNameEn.DataPropertyName = "NameEn";
        colNameEn.FillWeight = 120F;
        colNameEn.HeaderText = "الاسم الإنجليزي";
        colNameEn.MinimumWidth = 100;
        colNameEn.Name = "colNameEn";
        colNameEn.Width = 140;
        // 
        // colNature
        // 
        colNature.DataPropertyName = "NatureDisplay";
        colNature.FillWeight = 70F;
        colNature.HeaderText = "النوع";
        colNature.MinimumWidth = 70;
        colNature.Name = "colNature";
        colNature.Width = 90;
        // 
        // colLevel
        // 
        colLevel.DataPropertyName = "LevelDisplay";
        colLevel.FillWeight = 80F;
        colLevel.HeaderText = "المستوى";
        colLevel.MinimumWidth = 80;
        colLevel.Name = "colLevel";
        colLevel.Width = 100;
        // 
        // colParent
        // 
        colParent.DataPropertyName = "ParentAccountName";
        colParent.FillWeight = 130F;
        colParent.HeaderText = "الحساب الأب";
        colParent.MinimumWidth = 110;
        colParent.Name = "colParent";
        colParent.Width = 150;
        // 
        // colTreePath
        // 
        colTreePath.DataPropertyName = "TreePath";
        colTreePath.FillWeight = 110F;
        colTreePath.HeaderText = "المسار";
        colTreePath.MinimumWidth = 90;
        colTreePath.Name = "colTreePath";
        colTreePath.Width = 130;
        // 
        // colBalance
        // 
        colBalance.DataPropertyName = "Balance";
        colBalance.FillWeight = 90F;
        colBalance.HeaderText = "الرصيد";
        colBalance.MinimumWidth = 90;
        colBalance.Name = "colBalance";
        colBalance.Width = 110;
        // 
        // colFinancialStatement
        // 
        colFinancialStatement.DataPropertyName = "FinancialStatement";
        colFinancialStatement.FillWeight = 110F;
        colFinancialStatement.HeaderText = "القائمة المالية";
        colFinancialStatement.MinimumWidth = 100;
        colFinancialStatement.Name = "colFinancialStatement";
        colFinancialStatement.Width = 130;
        // 
        // colCategory
        // 
        colCategory.DataPropertyName = "Category";
        colCategory.FillWeight = 110F;
        colCategory.HeaderText = "التصنيف";
        colCategory.MinimumWidth = 100;
        colCategory.Name = "colCategory";
        colCategory.Width = 130;
        // 
        // colIsLeaf
        // 
        colIsLeaf.DataPropertyName = "IsLeafText";
        colIsLeaf.FillWeight = 80F;
        colIsLeaf.HeaderText = "قابل للترحيل";
        colIsLeaf.MinimumWidth = 80;
        colIsLeaf.Name = "colIsLeaf";
        colIsLeaf.Width = 100;
        // 
        // colIsSystem
        // 
        colIsSystem.DataPropertyName = "IsSystemText";
        colIsSystem.FillWeight = 70F;
        colIsSystem.HeaderText = "نظامي";
        colIsSystem.MinimumWidth = 70;
        colIsSystem.Name = "colIsSystem";
        colIsSystem.Width = 90;
        // 
        // colStatus
        // 
        colStatus.DataPropertyName = "Status";
        colStatus.FillWeight = 60F;
        colStatus.HeaderText = "الحالة";
        colStatus.MinimumWidth = 60;
        colStatus.Name = "colStatus";
        colStatus.Width = 80;
        // 
        // pnlSummary
        // 
        pnlSummary.BackColor = System.Drawing.Color.White;
        pnlSummary.Controls.Add(lblSummary);
        pnlSummary.Dock = DockStyle.Bottom;
        pnlSummary.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        pnlSummary.Location = new Point(0, 848);
        pnlSummary.Name = "pnlSummary";
        pnlSummary.Padding = new Padding(8, 18, 8, 4);
        pnlSummary.Size = new Size(1432, 52);
        pnlSummary.TabIndex = 0;
        pnlSummary.TabStop = false;
        pnlSummary.Text = "الإجماليات";
        // 
        // lblSummary
        // 
        lblSummary.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblSummary.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
        lblSummary.Location = new Point(8, 19);
        lblSummary.Name = "lblSummary";
        lblSummary.Size = new Size(1416, 29);
        lblSummary.TabIndex = 0;
        lblSummary.Text = "المعروض: 0 من 0";
        lblSummary.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // cmsGrid
        // 
        cmsGrid.Font = new Font("Tahoma", 10F);
        cmsGrid.ImageScalingSize = new Size(24, 24);
        cmsGrid.Items.AddRange(new ToolStripItem[] { cmsEdit, cmsDelete, cmsRefresh });
        cmsGrid.Name = "cmsGrid";
        cmsGrid.RightToLeft = RightToLeft.Yes;
        cmsGrid.Size = new Size(193, 94);
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
        // frmAccounts
        // 
        AutoScaleDimensions = new SizeF(11F, 24F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(170), Convert.ToInt32(220), Convert.ToInt32(170));
        ClientSize = new Size(1432, 900);
        Controls.Add(pnlBody);
        Controls.Add(pnlGrid);
        Controls.Add(pnlSummary);
        Controls.Add(pnlFilters);
        Controls.Add(gbCommands);
        Controls.Add(pnlHeader);
        Font = new Font("Tahoma", 10F);
        MinimumSize = new Size(1100, 760);
        Name = "frmAccounts";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterParent;
        Text = "دليل الحسابات";
        Load += frmAccounts_Load;
        pnlHeader.ResumeLayout(false);
        gbCommands.ResumeLayout(false);
        flpSecondary.ResumeLayout(false);
        pnlFilters.ResumeLayout(false);
        gbFilter.ResumeLayout(false);
        gbFilter.PerformLayout();
        pnlBody.ResumeLayout(false);
        pnlEditorHost.ResumeLayout(false);
        pnlTreeHost.ResumeLayout(false);
        pnlGrid.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvAccounts).EndInit();
        pnlSummary.ResumeLayout(false);
        cmsGrid.ResumeLayout(false);
        ResumeLayout(false);
    }

    private System.Windows.Forms.Panel pnlHeader;
    private System.Windows.Forms.Label lblMode;
    private System.Windows.Forms.GroupBox gbCommands;
    private System.Windows.Forms.FlowLayoutPanel flpSecondary;
    private System.Windows.Forms.Button btnPermanentDelete;
    private SalesSystem.Desktop.Controls.Common.ctrlCrudToolbar ctrlCrudToolbar1;
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
    private System.Windows.Forms.Panel pnlBody;
    private System.Windows.Forms.Panel pnlEditorHost;
    private SalesSystem.Desktop.Screens.Accounting.Controls.ctrlAccountEditor ctrlAccountEditor1;
    private System.Windows.Forms.Panel pnlTreeHost;
    private SalesSystem.Desktop.Screens.Accounting.Controls.ctrlTreeOfAccounts ctrlTreeOfAccounts1;
    private System.Windows.Forms.GroupBox pnlGrid;
    private System.Windows.Forms.Label lblEmptyState;
    private System.Windows.Forms.DataGridView dgvAccounts;
    private System.Windows.Forms.DataGridViewTextBoxColumn colCode;
    private System.Windows.Forms.DataGridViewTextBoxColumn colNameAr;
    private System.Windows.Forms.DataGridViewTextBoxColumn colNameEn;
    private System.Windows.Forms.DataGridViewTextBoxColumn colNature;
    private System.Windows.Forms.DataGridViewTextBoxColumn colLevel;
    private System.Windows.Forms.DataGridViewTextBoxColumn colParent;
    private System.Windows.Forms.DataGridViewTextBoxColumn colTreePath;
    private System.Windows.Forms.DataGridViewTextBoxColumn colBalance;
    private System.Windows.Forms.DataGridViewTextBoxColumn colFinancialStatement;
    private System.Windows.Forms.DataGridViewTextBoxColumn colCategory;
    private System.Windows.Forms.DataGridViewTextBoxColumn colIsLeaf;
    private System.Windows.Forms.DataGridViewTextBoxColumn colIsSystem;
    private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
    private System.Windows.Forms.GroupBox pnlSummary;
    private System.Windows.Forms.Label lblSummary;
    private System.Windows.Forms.ContextMenuStrip cmsGrid;
    private System.Windows.Forms.ToolStripMenuItem cmsEdit;
    private System.Windows.Forms.ToolStripMenuItem cmsDelete;
    private System.Windows.Forms.ToolStripMenuItem cmsRefresh;
    private System.Windows.Forms.ToolTip toolTip;
}
