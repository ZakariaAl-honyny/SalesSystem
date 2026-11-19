namespace SalesSystem.Desktop.Screens.Categories.Forms;

partial class frmCRUDCategoryMaster
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
        lblMode = new Label();
        pnlFields = new Panel();
        lblCode = new Label();
        txtCode = new TextBox();
        lblName = new Label();
        lblNameReq = new Label();
        txtName = new TextBox();
        lblDescription = new Label();
        txtDescription = new TextBox();
        pnlButtons = new FlowLayoutPanel();
        btnNew = new Button();
        btnSave = new Button();
        btnEdit = new Button();
        btnDelete = new Button();
        btnReactivate = new Button();
        btnClose = new Button();
        pnlFilters = new Panel();
        gbFilter = new GroupBox();
        label2 = new Label();
        cbFilterBy = new ComboBox();
        lblSearch = new Label();
        txtSearch = new TextBox();
        label1 = new Label();
        cbStatus = new ComboBox();
        chkShowInactive = new CheckBox();
        btnClearFilters = new Button();
        btnRefresh = new Button();
        btnToggleFilters = new Button();
        pnlSummary = new Panel();
        lblSummary = new Label();
        pnlGrid = new Panel();
        dgvCategories = new DataGridView();
        cmsGrid = new ContextMenuStrip(components);
        mnuEdit = new ToolStripMenuItem();
        mnuDeactivate = new ToolStripMenuItem();
        mnuRefresh = new ToolStripMenuItem();
        mnuSeparator = new ToolStripSeparator();
        mnuExport = new ToolStripMenuItem();
        errorProvider = new ErrorProvider(components);
        toolTip = new ToolTip(components);
        pnlFields.SuspendLayout();
        pnlButtons.SuspendLayout();
        pnlFilters.SuspendLayout();
        gbFilter.SuspendLayout();
        pnlSummary.SuspendLayout();
        pnlGrid.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvCategories).BeginInit();
        cmsGrid.SuspendLayout();
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
        lblMode.Size = new Size(1024, 50);
        lblMode.TabIndex = 0;
        lblMode.Text = "(( دليل مجموعات الأصناف ))";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlFields
        // 
        pnlFields.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
        pnlFields.Controls.Add(lblCode);
        pnlFields.Controls.Add(txtCode);
        pnlFields.Controls.Add(lblName);
        pnlFields.Controls.Add(lblNameReq);
        pnlFields.Controls.Add(txtName);
        pnlFields.Controls.Add(lblDescription);
        pnlFields.Controls.Add(txtDescription);
        pnlFields.Dock = DockStyle.Top;
        pnlFields.Location = new Point(0, 50);
        pnlFields.Name = "pnlFields";
        pnlFields.Size = new Size(1024, 100);
        pnlFields.TabIndex = 1;
        // 
        // lblCode
        // 
        lblCode.AutoSize = true;
        lblCode.Location = new Point(800, 14);
        lblCode.Name = "lblCode";
        lblCode.Size = new Size(126, 24);
        lblCode.TabIndex = 0;
        lblCode.Text = "رقم المجموعة";
        // 
        // txtCode
        // 
        txtCode.BackColor = System.Drawing.Color.White;
        txtCode.Location = new Point(800, 41);
        txtCode.Name = "txtCode";
        txtCode.ReadOnly = true;
        txtCode.Size = new Size(130, 32);
        txtCode.TabIndex = 0;
        txtCode.TabStop = false;
        // 
        // lblName
        // 
        lblName.AutoSize = true;
        lblName.Location = new Point(620, 14);
        lblName.Name = "lblName";
        lblName.Size = new Size(136, 24);
        lblName.TabIndex = 2;
        lblName.Text = "اسم المجموعة";
        // 
        // lblNameReq
        // 
        lblNameReq.AutoSize = true;
        lblNameReq.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblNameReq.ForeColor = System.Drawing.Color.Red;
        lblNameReq.Location = new Point(602, 14);
        lblNameReq.Name = "lblNameReq";
        lblNameReq.Size = new Size(23, 24);
        lblNameReq.TabIndex = 3;
        lblNameReq.Text = "*";
        // 
        // txtName
        // 
        txtName.BackColor = System.Drawing.Color.White;
        txtName.Location = new Point(480, 41);
        txtName.Name = "txtName";
        txtName.Size = new Size(290, 32);
        txtName.TabIndex = 1;
        // 
        // lblDescription
        // 
        lblDescription.AutoSize = true;
        lblDescription.Location = new Point(390, 14);
        lblDescription.Name = "lblDescription";
        lblDescription.Size = new Size(63, 24);
        lblDescription.TabIndex = 4;
        lblDescription.Text = "الوصف";
        // 
        // txtDescription
        // 
        txtDescription.BackColor = System.Drawing.Color.White;
        txtDescription.Location = new Point(120, 41);
        txtDescription.Name = "txtDescription";
        txtDescription.Size = new Size(330, 32);
        txtDescription.TabIndex = 2;
        // 
        // pnlButtons
        // 
        pnlButtons.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
        pnlButtons.Controls.Add(btnNew);
        pnlButtons.Controls.Add(btnSave);
        pnlButtons.Controls.Add(btnEdit);
        pnlButtons.Controls.Add(btnDelete);
        pnlButtons.Controls.Add(btnReactivate);
        pnlButtons.Controls.Add(btnRefresh);
        pnlButtons.Controls.Add(btnClose);
        pnlButtons.Dock = DockStyle.Top;
        pnlButtons.Location = new Point(0, 150);
        pnlButtons.Name = "pnlButtons";
        pnlButtons.Padding = new Padding(10);
        pnlButtons.Size = new Size(1024, 60);
        pnlButtons.TabIndex = 2;
        // 
        // btnNew
        // 
        btnNew.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(13), Convert.ToInt32(110), Convert.ToInt32(253));
        btnNew.FlatStyle = FlatStyle.Flat;
        btnNew.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnNew.ForeColor = System.Drawing.Color.White;
        btnNew.Location = new Point(859, 13);
        btnNew.Name = "btnNew";
        btnNew.Size = new Size(142, 35);
        btnNew.TabIndex = 0;
        btnNew.Text = "🆕 جديد";
        btnNew.UseVisualStyleBackColor = false;
        btnNew.Click += btnNew_Click;
        // 
        // btnSave
        // 
        btnSave.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(25), Convert.ToInt32(135), Convert.ToInt32(84));
        btnSave.FlatStyle = FlatStyle.Flat;
        btnSave.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnSave.ForeColor = System.Drawing.Color.White;
        btnSave.Location = new Point(725, 13);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(128, 35);
        btnSave.TabIndex = 1;
        btnSave.Text = "💾 إضافة";
        btnSave.UseVisualStyleBackColor = false;
        btnSave.Click += btnSave_Click;
        // 
        // btnEdit
        // 
        btnEdit.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(255), Convert.ToInt32(193), Convert.ToInt32(7));
        btnEdit.FlatStyle = FlatStyle.Flat;
        btnEdit.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnEdit.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(30), Convert.ToInt32(41), Convert.ToInt32(59));
        btnEdit.Location = new Point(591, 13);
        btnEdit.Name = "btnEdit";
        btnEdit.Size = new Size(128, 35);
        btnEdit.TabIndex = 3;
        btnEdit.Text = "✏️ تعديل";
        btnEdit.UseVisualStyleBackColor = false;
        btnEdit.Click += btnEdit_Click;
        // 
        // btnDelete
        // 
        btnDelete.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(220), Convert.ToInt32(53), Convert.ToInt32(69));
        btnDelete.FlatStyle = FlatStyle.Flat;
        btnDelete.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnDelete.ForeColor = System.Drawing.Color.White;
        btnDelete.Location = new Point(447, 13);
        btnDelete.Name = "btnDelete";
        btnDelete.Size = new Size(138, 35);
        btnDelete.TabIndex = 2;
        btnDelete.Text = "🗑 حذف";
        btnDelete.UseVisualStyleBackColor = false;
        btnDelete.Click += btnDelete_Click;
        // 
        // btnReactivate
        // 
        btnReactivate.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(23), Convert.ToInt32(162), Convert.ToInt32(184));
        btnReactivate.Cursor = Cursors.Hand;
        btnReactivate.Enabled = false;
        btnReactivate.FlatAppearance.BorderSize = 0;
        btnReactivate.FlatStyle = FlatStyle.Flat;
        btnReactivate.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnReactivate.ForeColor = System.Drawing.Color.White;
        btnReactivate.Location = new Point(281, 13);
        btnReactivate.Name = "btnReactivate";
        btnReactivate.Size = new Size(160, 35);
        btnReactivate.TabIndex = 5;
        btnReactivate.Text = "↺ إعادة تنشيط";
        toolTip.SetToolTip(btnReactivate, "إعادة تنشيط السجل المعطّل");
        btnReactivate.UseVisualStyleBackColor = false;
        btnReactivate.Click += btnReactivate_Click;
        // 
        // btnClose
        // 
        btnClose.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(108), Convert.ToInt32(117), Convert.ToInt32(125));
        btnClose.FlatStyle = FlatStyle.Flat;
        btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnClose.ForeColor = System.Drawing.Color.White;
        btnClose.Location = new Point(9, 13);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(130, 35);
        btnClose.TabIndex = 4;
        btnClose.TabStop = false;
        btnClose.Text = "🚪 خروج";
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += btnClose_Click;
        // 
        // pnlFilters
        // 
        pnlFilters.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
        pnlFilters.Controls.Add(gbFilter);
        pnlFilters.Controls.Add(btnToggleFilters);
        pnlFilters.Dock = DockStyle.Top;
        pnlFilters.Location = new Point(0, 210);
        pnlFilters.Margin = new Padding(0);
        pnlFilters.Name = "pnlFilters";
        pnlFilters.Padding = new Padding(6, 4, 6, 4);
        pnlFilters.Size = new Size(1024, 112);
        pnlFilters.TabIndex = 3;
        // 
        // gbFilter
        // 
        gbFilter.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(245), Convert.ToInt32(235));
        gbFilter.Controls.Add(label2);
        gbFilter.Controls.Add(cbFilterBy);
        gbFilter.Controls.Add(lblSearch);
        gbFilter.Controls.Add(txtSearch);
        gbFilter.Controls.Add(label1);
        gbFilter.Controls.Add(cbStatus);
        gbFilter.Controls.Add(chkShowInactive);
        gbFilter.Controls.Add(btnClearFilters);
        gbFilter.Dock = DockStyle.Fill;
        gbFilter.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        gbFilter.Location = new Point(146, 4);
        gbFilter.Margin = new Padding(0);
        gbFilter.Name = "gbFilter";
        gbFilter.Padding = new Padding(8);
        gbFilter.RightToLeft = RightToLeft.Yes;
        gbFilter.Size = new Size(872, 104);
        gbFilter.TabIndex = 0;
        gbFilter.TabStop = false;
        gbFilter.Text = "بحث وتصفية";
        // 
        // label2
        // 
        label2.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        label2.Location = new Point(688, 34);
        label2.Margin = new Padding(0);
        label2.Name = "label2";
        label2.Size = new Size(96, 22);
        label2.TabIndex = 0;
        label2.Text = "تصفية بـ:";
        label2.TextAlign = ContentAlignment.MiddleRight;
        // 
        // cbFilterBy
        // 
        cbFilterBy.BackColor = System.Drawing.Color.White;
        cbFilterBy.DropDownStyle = ComboBoxStyle.DropDownList;
        cbFilterBy.FlatStyle = FlatStyle.Flat;
        cbFilterBy.Font = new Font("Tahoma", 9F);
        cbFilterBy.FormattingEnabled = true;
        cbFilterBy.Items.AddRange(new object[] { "الاسم", "الوصف", "رقم المجموعة" });
        cbFilterBy.Location = new Point(552, 30);
        cbFilterBy.Margin = new Padding(0);
        cbFilterBy.Name = "cbFilterBy";
        cbFilterBy.Size = new Size(130, 30);
        cbFilterBy.TabIndex = 1;
        cbFilterBy.SelectedIndexChanged += cbFilterBy_SelectedIndexChanged;
        // 
        // lblSearch
        // 
        lblSearch.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblSearch.Location = new Point(490, 34);
        lblSearch.Margin = new Padding(0);
        lblSearch.Name = "lblSearch";
        lblSearch.Size = new Size(56, 22);
        lblSearch.TabIndex = 2;
        lblSearch.Text = "بحث:";
        lblSearch.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtSearch
        // 
        txtSearch.BackColor = System.Drawing.Color.White;
        txtSearch.Font = new Font("Tahoma", 9F);
        txtSearch.Location = new Point(254, 30);
        txtSearch.Margin = new Padding(0);
        txtSearch.Name = "txtSearch";
        txtSearch.PlaceholderText = "اكتب قيمة التصفية...";
        txtSearch.Size = new Size(230, 29);
        txtSearch.TabIndex = 3;
        txtSearch.TextChanged += txtSearch_TextChanged;
        txtSearch.KeyDown += txtSearch_KeyDown;
        txtSearch.KeyPress += txtSearch_KeyPress;
        // 
        // label1
        // 
        label1.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        label1.Location = new Point(168, 34);
        label1.Margin = new Padding(0);
        label1.Name = "label1";
        label1.Size = new Size(80, 22);
        label1.TabIndex = 4;
        label1.Text = "الحالة:";
        label1.TextAlign = ContentAlignment.MiddleRight;
        // 
        // cbStatus
        // 
        cbStatus.BackColor = System.Drawing.Color.White;
        cbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
        cbStatus.FlatStyle = FlatStyle.Flat;
        cbStatus.Font = new Font("Tahoma", 9F);
        cbStatus.FormattingEnabled = true;
        cbStatus.Items.AddRange(new object[] { "الكل", "نشط", "معطّل" });
        cbStatus.Location = new Point(52, 30);
        cbStatus.Margin = new Padding(0);
        cbStatus.Name = "cbStatus";
        cbStatus.Size = new Size(110, 30);
        cbStatus.TabIndex = 5;
        cbStatus.SelectedIndexChanged += cbStatus_SelectedIndexChanged;
        // 
        // chkShowInactive
        // 
        chkShowInactive.AutoSize = true;
        chkShowInactive.FlatStyle = FlatStyle.Flat;
        chkShowInactive.Font = new Font("Tahoma", 9F);
        chkShowInactive.Location = new Point(614, 70);
        chkShowInactive.Margin = new Padding(0);
        chkShowInactive.Name = "chkShowInactive";
        chkShowInactive.Size = new Size(155, 26);
        chkShowInactive.TabIndex = 6;
        chkShowInactive.Text = "عرض غير النشط";
        chkShowInactive.CheckedChanged += chkShowInactive_CheckedChanged;
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
        btnClearFilters.Location = new Point(315, 67);
        btnClearFilters.Margin = new Padding(0);
        btnClearFilters.Name = "btnClearFilters";
        btnClearFilters.Size = new Size(169, 32);
        btnClearFilters.TabIndex = 26;
        btnClearFilters.Text = "\U0001f9f9 مسح التصفية";
        toolTip.SetToolTip(btnClearFilters, "مسح كل أدوات التصفية (Esc) — لا يعيد تحميل البيانات");
        btnClearFilters.UseVisualStyleBackColor = false;
        btnClearFilters.Click += btnClearFilters_Click;
        // 
        // btnRefresh
        // 
        btnRefresh.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(23), Convert.ToInt32(162), Convert.ToInt32(184));
        btnRefresh.FlatStyle = FlatStyle.Flat;
        btnRefresh.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnRefresh.ForeColor = System.Drawing.Color.White;
        btnRefresh.Location = new Point(145, 13);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(130, 32);
        btnRefresh.TabIndex = 7;
        btnRefresh.Text = "🔄 تحديث";
        btnRefresh.UseVisualStyleBackColor = false;
        btnRefresh.Click += btnRefresh_Click;
        // 
        // btnToggleFilters
        // 
        btnToggleFilters.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(248), Convert.ToInt32(249), Convert.ToInt32(250));
        btnToggleFilters.Cursor = Cursors.Hand;
        btnToggleFilters.Dock = DockStyle.Left;
        btnToggleFilters.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(Convert.ToInt32(205), Convert.ToInt32(225), Convert.ToInt32(205));
        btnToggleFilters.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(240), Convert.ToInt32(240), Convert.ToInt32(240));
        btnToggleFilters.FlatStyle = FlatStyle.Flat;
        btnToggleFilters.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        btnToggleFilters.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(30), Convert.ToInt32(41), Convert.ToInt32(59));
        btnToggleFilters.Location = new Point(6, 4);
        btnToggleFilters.Name = "btnToggleFilters";
        btnToggleFilters.Size = new Size(140, 104);
        btnToggleFilters.TabIndex = 25;
        btnToggleFilters.Text = "إخفاء التصفية";
        btnToggleFilters.UseVisualStyleBackColor = false;
        // 
        // pnlSummary
        // 
        pnlSummary.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
        pnlSummary.Controls.Add(lblSummary);
        pnlSummary.Dock = DockStyle.Bottom;
        pnlSummary.Location = new Point(0, 668);
        pnlSummary.Margin = new Padding(0);
        pnlSummary.Name = "pnlSummary";
        pnlSummary.Size = new Size(1024, 32);
        pnlSummary.TabIndex = 5;
        // 
        // lblSummary
        // 
        lblSummary.Dock = DockStyle.Fill;
        lblSummary.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblSummary.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
        lblSummary.Location = new Point(0, 0);
        lblSummary.Name = "lblSummary";
        lblSummary.Size = new Size(1024, 32);
        lblSummary.TabIndex = 0;
        lblSummary.Text = "المعروض: 0 من 0";
        lblSummary.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlGrid
        // 
        pnlGrid.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
        pnlGrid.Controls.Add(dgvCategories);
        pnlGrid.Dock = DockStyle.Fill;
        pnlGrid.Location = new Point(0, 322);
        pnlGrid.Name = "pnlGrid";
        pnlGrid.Padding = new Padding(10);
        pnlGrid.Size = new Size(1024, 346);
        pnlGrid.TabIndex = 4;
        // 
        // dgvCategories
        // 
        dgvCategories.AllowUserToAddRows = false;
        dgvCategories.AllowUserToDeleteRows = false;
        dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(245), Convert.ToInt32(252), Convert.ToInt32(245));
        dgvCategories.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
        dgvCategories.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvCategories.BackgroundColor = System.Drawing.Color.FromArgb(Convert.ToInt32(220), Convert.ToInt32(240), Convert.ToInt32(220));
        dgvCategories.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
        dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
        dataGridViewCellStyle5.Font = new Font("Tahoma", 10F);
        dataGridViewCellStyle5.ForeColor = System.Drawing.Color.White;
        dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
        dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.White;
        dataGridViewCellStyle5.WrapMode = DataGridViewTriState.True;
        dgvCategories.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
        dgvCategories.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvCategories.ContextMenuStrip = cmsGrid;
        dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
        dataGridViewCellStyle6.Font = new Font("Tahoma", 10F);
        dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(255), Convert.ToInt32(255), Convert.ToInt32(180));
        dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        dataGridViewCellStyle6.WrapMode = DataGridViewTriState.False;
        dgvCategories.DefaultCellStyle = dataGridViewCellStyle6;
        dgvCategories.Dock = DockStyle.Fill;
        dgvCategories.EnableHeadersVisualStyles = false;
        dgvCategories.GridColor = System.Drawing.Color.FromArgb(Convert.ToInt32(205), Convert.ToInt32(225), Convert.ToInt32(205));
        dgvCategories.Location = new Point(10, 10);
        dgvCategories.MultiSelect = false;
        dgvCategories.Name = "dgvCategories";
        dgvCategories.ReadOnly = true;
        dgvCategories.RowHeadersVisible = false;
        dgvCategories.RowHeadersWidth = 62;
        dgvCategories.RowTemplate.Height = 32;
        dgvCategories.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvCategories.Size = new Size(1004, 326);
        dgvCategories.TabIndex = 0;
        dgvCategories.CellClick += dgvCategories_CellClick;
        dgvCategories.CellDoubleClick += dgvCategories_CellDoubleClick;
        // 
        // cmsGrid
        // 
        cmsGrid.BackColor = System.Drawing.Color.White;
        cmsGrid.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        cmsGrid.ImageScalingSize = new Size(24, 24);
        cmsGrid.Items.AddRange(new ToolStripItem[] { mnuEdit, mnuDeactivate, mnuRefresh, mnuSeparator, mnuExport });
        cmsGrid.Name = "cmsGrid";
        cmsGrid.RightToLeft = RightToLeft.Yes;
        cmsGrid.Size = new Size(284, 130);
        cmsGrid.Opening += cmsGrid_Opening;
        // 
        // mnuEdit
        // 
        mnuEdit.Name = "mnuEdit";
        mnuEdit.Size = new Size(283, 30);
        mnuEdit.Text = "✏️ تعديل السطر المحدد";
        mnuEdit.Click += mnuEdit_Click;
        // 
        // mnuDeactivate
        // 
        mnuDeactivate.Name = "mnuDeactivate";
        mnuDeactivate.Size = new Size(283, 30);
        mnuDeactivate.Text = "🗑 تعطيل السطر المحدد";
        mnuDeactivate.Click += mnuDeactivate_Click;
        // 
        // mnuRefresh
        // 
        mnuRefresh.Name = "mnuRefresh";
        mnuRefresh.Size = new Size(283, 30);
        mnuRefresh.Text = "🔄 تحديث البيانات (F5)";
        mnuRefresh.Click += mnuRefresh_Click;
        // 
        // mnuSeparator
        // 
        mnuSeparator.Name = "mnuSeparator";
        mnuSeparator.Size = new Size(280, 6);
        // 
        // mnuExport
        // 
        mnuExport.Name = "mnuExport";
        mnuExport.Size = new Size(283, 30);
        mnuExport.Text = "📊 تصدير إلى Excel";
        mnuExport.Click += mnuExport_Click;
        // 
        // errorProvider
        // 
        errorProvider.ContainerControl = this;
        errorProvider.RightToLeft = true;
        // 
        // frmCategoryMaster
        // 
        AutoScaleDimensions = new SizeF(11F, 24F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(170), Convert.ToInt32(220), Convert.ToInt32(170));
        ClientSize = new Size(1024, 700);
        Controls.Add(pnlGrid);
        Controls.Add(pnlSummary);
        Controls.Add(pnlFilters);
        Controls.Add(pnlButtons);
        Controls.Add(pnlFields);
        Controls.Add(lblMode);
        Font = new Font("Tahoma", 10F);
        Name = "frmCategoryMaster";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterParent;
        Text = "دليل مجموعات الأصناف";
        Load += frmCategoryMaster_Load;
        pnlFields.ResumeLayout(false);
        pnlFields.PerformLayout();
        pnlButtons.ResumeLayout(false);
        pnlFilters.ResumeLayout(false);
        gbFilter.ResumeLayout(false);
        gbFilter.PerformLayout();
        pnlSummary.ResumeLayout(false);
        pnlGrid.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvCategories).EndInit();
        cmsGrid.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        ResumeLayout(false);
    }

    private System.Windows.Forms.Label lblMode;
    private System.Windows.Forms.Panel pnlFields;
    private System.Windows.Forms.Label lblCode;
    private System.Windows.Forms.TextBox txtCode;
    private System.Windows.Forms.Label lblName;
    private System.Windows.Forms.Label lblNameReq;
    private System.Windows.Forms.TextBox txtName;
    private System.Windows.Forms.Label lblDescription;
    private System.Windows.Forms.TextBox txtDescription;
    private System.Windows.Forms.FlowLayoutPanel pnlButtons;
    private System.Windows.Forms.Button btnNew;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.Button btnEdit;
    private System.Windows.Forms.Button btnDelete;
    private System.Windows.Forms.Button btnReactivate;
    private System.Windows.Forms.Button btnClose;
    private System.Windows.Forms.Panel pnlFilters;
    private System.Windows.Forms.GroupBox gbFilter;
    private System.Windows.Forms.Button btnToggleFilters;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.ComboBox cbFilterBy;
    private System.Windows.Forms.Label lblSearch;
    private System.Windows.Forms.TextBox txtSearch;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ComboBox cbStatus;
    private System.Windows.Forms.CheckBox chkShowInactive;
    private System.Windows.Forms.Button btnClearFilters;
    private System.Windows.Forms.Button btnRefresh;
    private System.Windows.Forms.Panel pnlSummary;
    private System.Windows.Forms.Label lblSummary;
    private System.Windows.Forms.Panel pnlGrid;
    private System.Windows.Forms.DataGridView dgvCategories;
    private System.Windows.Forms.DataGridViewTextBoxColumn colId;
    private System.Windows.Forms.DataGridViewTextBoxColumn colName;
    private System.Windows.Forms.DataGridViewTextBoxColumn colDescription;
    private System.Windows.Forms.DataGridViewTextBoxColumn colIsActive;
    private System.Windows.Forms.ErrorProvider errorProvider;
    private System.Windows.Forms.ToolTip toolTip;
    private System.Windows.Forms.ContextMenuStrip cmsGrid;
    private System.Windows.Forms.ToolStripMenuItem mnuEdit;
    private System.Windows.Forms.ToolStripMenuItem mnuDeactivate;
    private System.Windows.Forms.ToolStripMenuItem mnuRefresh;
    private System.Windows.Forms.ToolStripSeparator mnuSeparator;
    private System.Windows.Forms.ToolStripMenuItem mnuExport;
}
