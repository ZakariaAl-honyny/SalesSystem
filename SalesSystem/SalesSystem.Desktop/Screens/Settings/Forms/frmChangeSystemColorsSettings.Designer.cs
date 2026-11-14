namespace SalesSystem.Desktop.Screens.Settings.Forms;

partial class frmChangeSystemColorsSettings
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
        DataGridViewCellStyle styleColorsAlternate = new DataGridViewCellStyle();
        DataGridViewCellStyle styleColorsHeader = new DataGridViewCellStyle();
        DataGridViewCellStyle styleColorsCell = new DataGridViewCellStyle();
        DataGridViewCellStyle stylePreviewHeader = new DataGridViewCellStyle();
        DataGridViewCellStyle stylePreviewCell = new DataGridViewCellStyle();
        DataGridViewCellStyle stylePreviewAlternate = new DataGridViewCellStyle();
        pnlHeader = new Panel();
        lblMode = new Label();
        gbToolbar = new GroupBox();
        flpToolbar = new FlowLayoutPanel();
        btnSave = new Button();
        btnReset = new Button();
        btnPickColor = new Button();
        btnRefresh = new Button();
        btnClose = new Button();
        pnlFilters = new Panel();
        gbFilter = new GroupBox();
        label2 = new Label();
        cbFilterBy = new ComboBox();
        txtFilterValue = new TextBox();
        label1 = new Label();
        cbGroup = new ComboBox();
        chkOnlyChanged = new CheckBox();
        btnClearFilters = new Button();
        btnToggleFilters = new Button();
        pnlSummary = new Panel();
        lblSummary = new Label();
        gbColors = new GroupBox();
        dgvColors = new DataGridView();
        colNo = new DataGridViewTextBoxColumn();
        colGroup = new DataGridViewTextBoxColumn();
        colLabel = new DataGridViewTextBoxColumn();
        colKey = new DataGridViewTextBoxColumn();
        colHex = new DataGridViewTextBoxColumn();
        colSample = new DataGridViewTextBoxColumn();
        lblNoData = new Label();
        cmsColors = new ContextMenuStrip(components);
        miPickColor = new ToolStripMenuItem();
        miCopyHex = new ToolStripMenuItem();
        gbPreview = new GroupBox();
        lblPreviewCaption = new Label();
        pnlPreviewShell = new Panel();
        lblPreviewHeader = new Label();
        lblPreviewField = new Label();
        txtPreviewInput = new TextBox();
        dgvPreview = new DataGridView();
        colPreviewLabel = new DataGridViewTextBoxColumn();
        colPreviewAmount = new DataGridViewTextBoxColumn();
        flpPreviewButtons = new FlowLayoutPanel();
        btnPreviewAdd = new Button();
        btnPreviewSave = new Button();
        btnPreviewDelete = new Button();
        btnPreviewCancel = new Button();
        btnPreviewInfo = new Button();
        txtPreviewDisabled = new TextBox();
        lblSelectedColor = new Label();
        pnlSwatch = new Panel();
        txtHex = new TextBox();
        lblPreviewHint = new Label();
        pnlFooter = new Panel();
        lblFooterHint = new Label();
        flpFooter = new FlowLayoutPanel();
        btnFooterClose = new Button();
        toolTip = new ToolTip(components);
        pnlHeader.SuspendLayout();
        gbToolbar.SuspendLayout();
        flpToolbar.SuspendLayout();
        pnlFilters.SuspendLayout();
        gbFilter.SuspendLayout();
        pnlSummary.SuspendLayout();
        gbColors.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvColors).BeginInit();
        cmsColors.SuspendLayout();
        gbPreview.SuspendLayout();
        pnlPreviewShell.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvPreview).BeginInit();
        flpPreviewButtons.SuspendLayout();
        pnlFooter.SuspendLayout();
        flpFooter.SuspendLayout();
        SuspendLayout();
        // 
        // pnlHeader
        // 
        pnlHeader.BackColor = Color.FromArgb(170, 220, 170);
        pnlHeader.Controls.Add(lblMode);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Location = new Point(0, 0);
        pnlHeader.Name = "pnlHeader";
        pnlHeader.Size = new Size(1200, 56);
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
        lblMode.Size = new Size(1200, 56);
        lblMode.TabIndex = 0;
        lblMode.Text = "ألوان النظام — إعدادات العرض";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // gbToolbar
        // 
        gbToolbar.BackColor = Color.FromArgb(235, 240, 235);
        gbToolbar.Controls.Add(flpToolbar);
        gbToolbar.Dock = DockStyle.Top;
        gbToolbar.Font = new Font("Tahoma", 10F);
        gbToolbar.Location = new Point(0, 56);
        gbToolbar.Name = "gbToolbar";
        gbToolbar.Padding = new Padding(8);
        gbToolbar.Size = new Size(1200, 76);
        gbToolbar.TabIndex = 1;
        gbToolbar.TabStop = false;
        gbToolbar.Text = "أوامر الشاشة";
        // 
        // flpToolbar
        // 
        flpToolbar.BackColor = Color.FromArgb(235, 240, 235);
        flpToolbar.Controls.Add(btnSave);
        flpToolbar.Controls.Add(btnReset);
        flpToolbar.Controls.Add(btnPickColor);
        flpToolbar.Controls.Add(btnRefresh);
        flpToolbar.Controls.Add(btnClose);
        flpToolbar.Dock = DockStyle.Fill;
        flpToolbar.FlowDirection = FlowDirection.LeftToRight;
        flpToolbar.Location = new Point(8, 22);
        flpToolbar.Name = "flpToolbar";
        flpToolbar.Size = new Size(1184, 46);
        flpToolbar.TabIndex = 0;
        flpToolbar.WrapContents = false;
        // 
        // btnSave
        // 
        btnSave.BackColor = Color.FromArgb(25, 135, 84);
        btnSave.Cursor = Cursors.Hand;
        btnSave.FlatAppearance.BorderSize = 0;
        btnSave.FlatStyle = FlatStyle.Flat;
        btnSave.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        btnSave.ForeColor = Color.White;
        btnSave.Location = new Point(0, 0);
        btnSave.Margin = new Padding(0, 0, 8, 0);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(140, 36);
        btnSave.TabIndex = 0;
        btnSave.Text = "حفظ وتطبيق";
        toolTip.SetToolTip(btnSave, "حفظ الألوان وتطبيقها على النظام كله (Enter)");
        btnSave.UseVisualStyleBackColor = false;
        btnSave.Click += btnSave_Click;
        // 
        // btnReset
        // 
        btnReset.BackColor = Color.FromArgb(108, 117, 125);
        btnReset.Cursor = Cursors.Hand;
        btnReset.FlatAppearance.BorderSize = 0;
        btnReset.FlatStyle = FlatStyle.Flat;
        btnReset.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        btnReset.ForeColor = Color.White;
        btnReset.Location = new Point(148, 0);
        btnReset.Margin = new Padding(0, 0, 8, 0);
        btnReset.Name = "btnReset";
        btnReset.Size = new Size(140, 36);
        btnReset.TabIndex = 1;
        btnReset.Text = "استعادة الافتراضي";
        toolTip.SetToolTip(btnReset, "استعادة كل الألوان إلى قيمها الافتراضية بعد تأكيد");
        btnReset.UseVisualStyleBackColor = false;
        btnReset.Click += btnReset_Click;
        // 
        // btnPickColor
        // 
        btnPickColor.BackColor = Color.FromArgb(13, 110, 253);
        btnPickColor.Cursor = Cursors.Hand;
        btnPickColor.FlatAppearance.BorderSize = 0;
        btnPickColor.FlatStyle = FlatStyle.Flat;
        btnPickColor.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        btnPickColor.ForeColor = Color.White;
        btnPickColor.Location = new Point(296, 0);
        btnPickColor.Margin = new Padding(0, 0, 8, 0);
        btnPickColor.Name = "btnPickColor";
        btnPickColor.Size = new Size(140, 36);
        btnPickColor.TabIndex = 2;
        btnPickColor.Text = "اختيار اللون";
        toolTip.SetToolTip(btnPickColor, "اختيار لون للسطر المحدد من لوحة الألوان (F2)");
        btnPickColor.UseVisualStyleBackColor = false;
        btnPickColor.Click += btnPickColor_Click;
        // 
        // btnRefresh
        // 
        btnRefresh.BackColor = Color.FromArgb(248, 249, 250);
        btnRefresh.Cursor = Cursors.Hand;
        btnRefresh.FlatAppearance.BorderColor = Color.FromArgb(222, 226, 230);
        btnRefresh.FlatAppearance.BorderSize = 1;
        btnRefresh.FlatStyle = FlatStyle.Flat;
        btnRefresh.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        btnRefresh.ForeColor = Color.FromArgb(33, 37, 41);
        btnRefresh.Location = new Point(444, 0);
        btnRefresh.Margin = new Padding(0, 0, 8, 0);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(140, 36);
        btnRefresh.TabIndex = 3;
        btnRefresh.Text = "تحديث";
        toolTip.SetToolTip(btnRefresh, "إعادة تحميل الألوان من المخزن ومسح التصفية (F5)");
        btnRefresh.UseVisualStyleBackColor = false;
        btnRefresh.Click += btnRefresh_Click;
        // 
        // btnClose
        // 
        btnClose.BackColor = Color.FromArgb(108, 117, 125);
        btnClose.Cursor = Cursors.Hand;
        btnClose.FlatAppearance.BorderSize = 0;
        btnClose.FlatStyle = FlatStyle.Flat;
        btnClose.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        btnClose.ForeColor = Color.White;
        btnClose.Location = new Point(592, 0);
        btnClose.Margin = new Padding(0, 0, 8, 0);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(140, 36);
        btnClose.TabIndex = 4;
        btnClose.Text = "إغلاق";
        toolTip.SetToolTip(btnClose, "إغلاق الشاشة (Esc)");
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += btnClose_Click;
        // 
        // pnlFilters
        // 
        pnlFilters.Controls.Add(gbFilter);
        pnlFilters.Controls.Add(btnToggleFilters);
        pnlFilters.Dock = DockStyle.Top;
        pnlFilters.Location = new Point(0, 132);
        pnlFilters.Margin = new Padding(0);
        pnlFilters.Name = "pnlFilters";
        pnlFilters.Padding = new Padding(6, 4, 6, 4);
        pnlFilters.Size = new Size(1200, 120);
        pnlFilters.TabIndex = 2;
        // 
        // gbFilter
        // 
        gbFilter.BackColor = Color.FromArgb(235, 245, 235);
        gbFilter.Controls.Add(label2);
        gbFilter.Controls.Add(cbFilterBy);
        gbFilter.Controls.Add(txtFilterValue);
        gbFilter.Controls.Add(label1);
        gbFilter.Controls.Add(cbGroup);
        gbFilter.Controls.Add(chkOnlyChanged);
        gbFilter.Controls.Add(btnClearFilters);
        gbFilter.Dock = DockStyle.Fill;
        gbFilter.Font = new Font("Tahoma", 10F);
        gbFilter.Location = new Point(146, 4);
        gbFilter.Margin = new Padding(0);
        gbFilter.Name = "gbFilter";
        gbFilter.Padding = new Padding(8);
        gbFilter.RightToLeft = RightToLeft.Yes;
        gbFilter.Size = new Size(1048, 112);
        gbFilter.TabIndex = 0;
        gbFilter.TabStop = false;
        gbFilter.Text = "بحث وتصفية";
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Location = new Point(884, 14);
        label2.Name = "label2";
        label2.Size = new Size(88, 24);
        label2.TabIndex = 0;
        label2.Text = "تصفية بـ:";
        // 
        // cbFilterBy
        // 
        cbFilterBy.BackColor = Color.White;
        cbFilterBy.DropDownStyle = ComboBoxStyle.DropDownList;
        cbFilterBy.FlatStyle = FlatStyle.Flat;
        cbFilterBy.Items.AddRange(new object[] { "الكل", "اللون", "المفتاح", "المجموعة" });
        cbFilterBy.Location = new Point(718, 8);
        cbFilterBy.Name = "cbFilterBy";
        cbFilterBy.SelectedIndex = 0;
        cbFilterBy.Size = new Size(160, 30);
        cbFilterBy.TabIndex = 1;
        cbFilterBy.SelectedIndexChanged += Filter_Changed;
        // 
        // txtFilterValue
        // 
        txtFilterValue.BackColor = Color.White;
        txtFilterValue.Location = new Point(400, 8);
        txtFilterValue.Name = "txtFilterValue";
        txtFilterValue.PlaceholderText = "اكتب قيمة التصفية...";
        txtFilterValue.Size = new Size(310, 30);
        txtFilterValue.TabIndex = 2;
        txtFilterValue.TextChanged += Filter_Changed;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new Point(286, 14);
        label1.Name = "label1";
        label1.Size = new Size(106, 24);
        label1.TabIndex = 3;
        label1.Text = "المجموعة:";
        // 
        // cbGroup
        // 
        cbGroup.BackColor = Color.White;
        cbGroup.DropDownStyle = ComboBoxStyle.DropDownList;
        cbGroup.FlatStyle = FlatStyle.Flat;
        cbGroup.Items.AddRange(new object[] { "كل المجموعات" });
        cbGroup.Location = new Point(98, 8);
        cbGroup.Name = "cbGroup";
        cbGroup.SelectedIndex = 0;
        cbGroup.Size = new Size(180, 30);
        cbGroup.TabIndex = 4;
        cbGroup.SelectedIndexChanged += Filter_Changed;
        // 
        // chkOnlyChanged
        // 
        chkOnlyChanged.AutoSize = false;
        chkOnlyChanged.FlatStyle = FlatStyle.Flat;
        chkOnlyChanged.Location = new Point(816, 44);
        chkOnlyChanged.Name = "chkOnlyChanged";
        chkOnlyChanged.Size = new Size(160, 28);
        chkOnlyChanged.TabIndex = 5;
        chkOnlyChanged.Text = "المعدّل فقط";
        chkOnlyChanged.CheckedChanged += Filter_Changed;
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
        btnClearFilters.Location = new Point(576, 44);
        btnClearFilters.Name = "btnClearFilters";
        btnClearFilters.Size = new Size(180, 30);
        btnClearFilters.TabIndex = 6;
        btnClearFilters.Text = "مسح التصفية";
        toolTip.SetToolTip(btnClearFilters, "مسح كل أدوات التصفية — لا يعيد تحميل البيانات");
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
        btnToggleFilters.Size = new Size(140, 112);
        btnToggleFilters.TabIndex = 1;
        btnToggleFilters.Text = "إخفاء التصفية";
        toolTip.SetToolTip(btnToggleFilters, "إخفاء/إظهار صندوق التصفية (F9)");
        btnToggleFilters.UseVisualStyleBackColor = false;
        // 
        // pnlSummary
        // 
        pnlSummary.BackColor = Color.FromArgb(235, 240, 235);
        pnlSummary.Controls.Add(lblSummary);
        pnlSummary.Dock = DockStyle.Top;
        pnlSummary.Location = new Point(0, 252);
        pnlSummary.Margin = new Padding(0);
        pnlSummary.Name = "pnlSummary";
        pnlSummary.Size = new Size(1200, 40);
        pnlSummary.TabIndex = 3;
        // 
        // lblSummary
        // 
        lblSummary.Dock = DockStyle.Fill;
        lblSummary.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblSummary.ForeColor = Color.FromArgb(14, 82, 52);
        lblSummary.Location = new Point(0, 0);
        lblSummary.Name = "lblSummary";
        lblSummary.Size = new Size(1200, 40);
        lblSummary.TabIndex = 0;
        lblSummary.Text = "عدد الألوان: 0 · المعدّل: 0";
        lblSummary.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // gbColors
        // 
        gbColors.BackColor = Color.FromArgb(235, 245, 235);
        gbColors.Controls.Add(dgvColors);
        gbColors.Controls.Add(lblNoData);
        gbColors.Dock = DockStyle.Fill;
        gbColors.Font = new Font("Tahoma", 10F);
        gbColors.Location = new Point(0, 292);
        gbColors.Name = "gbColors";
        gbColors.Padding = new Padding(8);
        gbColors.Size = new Size(820, 472);
        gbColors.TabIndex = 4;
        gbColors.TabStop = false;
        gbColors.Text = "الألوان";
        // 
        // dgvColors
        // 
        dgvColors.AllowUserToAddRows = false;
        dgvColors.AllowUserToDeleteRows = false;
        dgvColors.AllowUserToResizeRows = false;
        styleColorsAlternate.BackColor = Color.FromArgb(245, 252, 245);
        dgvColors.AlternatingRowsDefaultCellStyle = styleColorsAlternate;
        dgvColors.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvColors.BackgroundColor = Color.FromArgb(220, 240, 220);
        dgvColors.BorderStyle = BorderStyle.None;
        dgvColors.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
        dgvColors.ColumnHeadersHeight = 40;
        styleColorsHeader.Alignment = DataGridViewContentAlignment.MiddleCenter;
        styleColorsHeader.BackColor = Color.FromArgb(14, 165, 103);
        styleColorsHeader.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        styleColorsHeader.ForeColor = Color.White;
        styleColorsHeader.SelectionBackColor = Color.FromArgb(14, 165, 103);
        styleColorsHeader.SelectionForeColor = Color.White;
        styleColorsHeader.WrapMode = DataGridViewTriState.True;
        dgvColors.ColumnHeadersDefaultCellStyle = styleColorsHeader;
        dgvColors.Columns.AddRange(new DataGridViewColumn[] { colNo, colGroup, colLabel, colKey, colHex, colSample });
        dgvColors.ContextMenuStrip = cmsColors;
        styleColorsCell.Alignment = DataGridViewContentAlignment.MiddleCenter;
        styleColorsCell.BackColor = Color.White;
        styleColorsCell.Font = new Font("Tahoma", 10F);
        styleColorsCell.ForeColor = Color.FromArgb(33, 37, 41);
        styleColorsCell.SelectionBackColor = Color.FromArgb(255, 255, 180);
        styleColorsCell.SelectionForeColor = Color.FromArgb(33, 37, 41);
        styleColorsCell.WrapMode = DataGridViewTriState.False;
        dgvColors.DefaultCellStyle = styleColorsCell;
        dgvColors.Dock = DockStyle.Fill;
        dgvColors.EnableHeadersVisualStyles = false;
        dgvColors.GridColor = Color.FromArgb(205, 225, 205);
        dgvColors.Location = new Point(8, 30);
        dgvColors.MultiSelect = false;
        dgvColors.Name = "dgvColors";
        dgvColors.ReadOnly = true;
        dgvColors.RowHeadersVisible = false;
        dgvColors.RowHeadersWidth = 51;
        dgvColors.RowTemplate.Height = 28;
        dgvColors.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvColors.Size = new Size(804, 434);
        dgvColors.TabIndex = 0;
        dgvColors.CellDoubleClick += dgvColors_CellDoubleClick;
        dgvColors.CellFormatting += dgvColors_CellFormatting;
        dgvColors.SelectionChanged += dgvColors_SelectionChanged;
        // 
        // colNo
        // 
        colNo.FillWeight = 25F;
        colNo.HeaderText = "#";
        colNo.MinimumWidth = 45;
        colNo.Name = "colNo";
        colNo.ReadOnly = true;
        colNo.SortMode = DataGridViewColumnSortMode.NotSortable;
        // 
        // colGroup
        // 
        colGroup.FillWeight = 110F;
        colGroup.HeaderText = "المجموعة";
        colGroup.MinimumWidth = 110;
        colGroup.Name = "colGroup";
        colGroup.ReadOnly = true;
        colGroup.SortMode = DataGridViewColumnSortMode.NotSortable;
        // 
        // colLabel
        // 
        colLabel.FillWeight = 140F;
        colLabel.HeaderText = "اللون";
        colLabel.MinimumWidth = 120;
        colLabel.Name = "colLabel";
        colLabel.ReadOnly = true;
        colLabel.SortMode = DataGridViewColumnSortMode.NotSortable;
        // 
        // colKey
        // 
        colKey.FillWeight = 170F;
        colKey.HeaderText = "المفتاح";
        colKey.MinimumWidth = 140;
        colKey.Name = "colKey";
        colKey.ReadOnly = true;
        colKey.SortMode = DataGridViewColumnSortMode.NotSortable;
        // 
        // colHex
        // 
        colHex.FillWeight = 100F;
        colHex.HeaderText = "القيمة (HEX)";
        colHex.MinimumWidth = 100;
        colHex.Name = "colHex";
        colHex.ReadOnly = true;
        colHex.SortMode = DataGridViewColumnSortMode.NotSortable;
        // 
        // colSample
        // 
        colSample.FillWeight = 35F;
        colSample.HeaderText = "عيّنة";
        colSample.MinimumWidth = 55;
        colSample.Name = "colSample";
        colSample.ReadOnly = true;
        colSample.SortMode = DataGridViewColumnSortMode.NotSortable;
        // 
        // lblNoData
        // 
        lblNoData.BackColor = Color.FromArgb(220, 240, 220);
        lblNoData.Dock = DockStyle.Fill;
        lblNoData.Font = new Font("Tahoma", 12F, FontStyle.Bold);
        lblNoData.ForeColor = Color.FromArgb(14, 82, 52);
        lblNoData.Location = new Point(8, 30);
        lblNoData.Name = "lblNoData";
        lblNoData.Size = new Size(804, 434);
        lblNoData.TabIndex = 1;
        lblNoData.Text = "لا توجد بيانات";
        lblNoData.TextAlign = ContentAlignment.MiddleCenter;
        lblNoData.Visible = false;
        lblNoData.BringToFront();
        // 
        // cmsColors
        // 
        cmsColors.Items.AddRange(new ToolStripItem[] { miPickColor, miCopyHex });
        cmsColors.Name = "cmsColors";
        cmsColors.RightToLeft = RightToLeft.Yes;
        cmsColors.Size = new Size(190, 52);
        // 
        // miPickColor
        // 
        miPickColor.Name = "miPickColor";
        miPickColor.Size = new Size(189, 24);
        miPickColor.Text = "اختيار اللون";
        miPickColor.Click += miPickColor_Click;
        // 
        // miCopyHex
        // 
        miCopyHex.Name = "miCopyHex";
        miCopyHex.Size = new Size(189, 24);
        miCopyHex.Text = "نسخ قيمة اللون";
        miCopyHex.Click += miCopyHex_Click;
        // 
        // gbPreview
        // 
        gbPreview.BackColor = Color.FromArgb(235, 245, 235);
        gbPreview.Controls.Add(lblPreviewCaption);
        gbPreview.Controls.Add(pnlPreviewShell);
        gbPreview.Controls.Add(lblSelectedColor);
        gbPreview.Controls.Add(pnlSwatch);
        gbPreview.Controls.Add(txtHex);
        gbPreview.Controls.Add(lblPreviewHint);
        gbPreview.Dock = DockStyle.Right;
        gbPreview.Font = new Font("Tahoma", 10F);
        gbPreview.Location = new Point(820, 292);
        gbPreview.Name = "gbPreview";
        gbPreview.Padding = new Padding(8);
        gbPreview.Size = new Size(380, 472);
        gbPreview.TabIndex = 5;
        gbPreview.TabStop = false;
        gbPreview.Text = "معاينة فورية";
        // 
        // lblPreviewCaption
        // 
        lblPreviewCaption.AutoSize = true;
        lblPreviewCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblPreviewCaption.ForeColor = Color.FromArgb(14, 82, 52);
        lblPreviewCaption.Location = new Point(222, 32);
        lblPreviewCaption.Name = "lblPreviewCaption";
        lblPreviewCaption.Size = new Size(140, 24);
        lblPreviewCaption.TabIndex = 0;
        lblPreviewCaption.Text = "معاينة مباشرة";
        // 
        // pnlPreviewShell
        // 
        pnlPreviewShell.BackColor = Color.FromArgb(170, 220, 170);
        pnlPreviewShell.BorderStyle = BorderStyle.FixedSingle;
        pnlPreviewShell.Controls.Add(lblPreviewHeader);
        pnlPreviewShell.Controls.Add(lblPreviewField);
        pnlPreviewShell.Controls.Add(txtPreviewInput);
        pnlPreviewShell.Controls.Add(dgvPreview);
        pnlPreviewShell.Controls.Add(flpPreviewButtons);
        pnlPreviewShell.Controls.Add(txtPreviewDisabled);
        pnlPreviewShell.Location = new Point(10, 60);
        pnlPreviewShell.Name = "pnlPreviewShell";
        pnlPreviewShell.Size = new Size(356, 250);
        pnlPreviewShell.TabIndex = 1;
        // 
        // lblPreviewHeader
        // 
        lblPreviewHeader.BackColor = Color.FromArgb(14, 165, 103);
        lblPreviewHeader.Dock = DockStyle.Top;
        lblPreviewHeader.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblPreviewHeader.ForeColor = Color.White;
        lblPreviewHeader.Location = new Point(0, 0);
        lblPreviewHeader.Name = "lblPreviewHeader";
        lblPreviewHeader.Size = new Size(354, 30);
        lblPreviewHeader.TabIndex = 0;
        lblPreviewHeader.Text = "شريط العنوان";
        lblPreviewHeader.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblPreviewField
        // 
        lblPreviewField.AutoSize = true;
        lblPreviewField.ForeColor = Color.FromArgb(14, 82, 52);
        lblPreviewField.Location = new Point(222, 42);
        lblPreviewField.Name = "lblPreviewField";
        lblPreviewField.Size = new Size(120, 24);
        lblPreviewField.TabIndex = 1;
        lblPreviewField.Text = "اسم العميل:";
        // 
        // txtPreviewInput
        // 
        txtPreviewInput.BackColor = Color.White;
        txtPreviewInput.Location = new Point(52, 38);
        txtPreviewInput.Name = "txtPreviewInput";
        txtPreviewInput.ReadOnly = true;
        txtPreviewInput.Size = new Size(164, 30);
        txtPreviewInput.TabIndex = 2;
        txtPreviewInput.Text = "عميل نقدي";
        // 
        // dgvPreview
        // 
        dgvPreview.AllowUserToAddRows = false;
        dgvPreview.AllowUserToDeleteRows = false;
        dgvPreview.AllowUserToResizeRows = false;
        stylePreviewAlternate.BackColor = Color.FromArgb(245, 252, 245);
        dgvPreview.AlternatingRowsDefaultCellStyle = stylePreviewAlternate;
        dgvPreview.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvPreview.BackgroundColor = Color.FromArgb(220, 240, 220);
        dgvPreview.BorderStyle = BorderStyle.None;
        dgvPreview.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
        dgvPreview.ColumnHeadersHeight = 28;
        dgvPreview.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        stylePreviewHeader.Alignment = DataGridViewContentAlignment.MiddleCenter;
        stylePreviewHeader.BackColor = Color.FromArgb(14, 165, 103);
        stylePreviewHeader.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        stylePreviewHeader.ForeColor = Color.White;
        stylePreviewHeader.SelectionBackColor = Color.FromArgb(14, 165, 103);
        stylePreviewHeader.SelectionForeColor = Color.White;
        dgvPreview.ColumnHeadersDefaultCellStyle = stylePreviewHeader;
        dgvPreview.Columns.AddRange(new DataGridViewColumn[] { colPreviewLabel, colPreviewAmount });
        stylePreviewCell.BackColor = Color.White;
        stylePreviewCell.Font = new Font("Tahoma", 9F);
        stylePreviewCell.ForeColor = Color.FromArgb(33, 37, 41);
        stylePreviewCell.SelectionBackColor = Color.FromArgb(255, 255, 180);
        stylePreviewCell.SelectionForeColor = Color.FromArgb(33, 37, 41);
        stylePreviewCell.WrapMode = DataGridViewTriState.False;
        dgvPreview.DefaultCellStyle = stylePreviewCell;
        dgvPreview.EnableHeadersVisualStyles = false;
        dgvPreview.GridColor = Color.FromArgb(205, 225, 205);
        dgvPreview.Location = new Point(8, 74);
        dgvPreview.MultiSelect = false;
        dgvPreview.Name = "dgvPreview";
        dgvPreview.ReadOnly = true;
        dgvPreview.RowHeadersVisible = false;
        dgvPreview.RowHeadersWidth = 51;
        dgvPreview.RowTemplate.Height = 22;
        dgvPreview.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvPreview.Size = new Size(338, 72);
        dgvPreview.TabIndex = 3;
        // 
        // colPreviewLabel
        // 
        colPreviewLabel.FillWeight = 60F;
        colPreviewLabel.HeaderText = "البيان";
        colPreviewLabel.MinimumWidth = 80;
        colPreviewLabel.Name = "colPreviewLabel";
        colPreviewLabel.ReadOnly = true;
        colPreviewLabel.SortMode = DataGridViewColumnSortMode.NotSortable;
        // 
        // colPreviewAmount
        // 
        colPreviewAmount.FillWeight = 40F;
        colPreviewAmount.HeaderText = "المبلغ";
        colPreviewAmount.MinimumWidth = 60;
        colPreviewAmount.Name = "colPreviewAmount";
        colPreviewAmount.ReadOnly = true;
        colPreviewAmount.SortMode = DataGridViewColumnSortMode.NotSortable;
        // 
        // flpPreviewButtons
        // 
        flpPreviewButtons.BackColor = Color.FromArgb(235, 245, 235);
        flpPreviewButtons.Controls.Add(btnPreviewAdd);
        flpPreviewButtons.Controls.Add(btnPreviewSave);
        flpPreviewButtons.Controls.Add(btnPreviewDelete);
        flpPreviewButtons.Controls.Add(btnPreviewCancel);
        flpPreviewButtons.Controls.Add(btnPreviewInfo);
        flpPreviewButtons.FlowDirection = FlowDirection.LeftToRight;
        flpPreviewButtons.Location = new Point(8, 152);
        flpPreviewButtons.Name = "flpPreviewButtons";
        flpPreviewButtons.Size = new Size(338, 30);
        flpPreviewButtons.TabIndex = 4;
        flpPreviewButtons.WrapContents = false;
        // 
        // btnPreviewAdd
        // 
        btnPreviewAdd.BackColor = Color.FromArgb(13, 110, 253);
        btnPreviewAdd.FlatAppearance.BorderSize = 0;
        btnPreviewAdd.FlatStyle = FlatStyle.Flat;
        btnPreviewAdd.Font = new Font("Tahoma", 8F, FontStyle.Bold);
        btnPreviewAdd.ForeColor = Color.White;
        btnPreviewAdd.Location = new Point(0, 0);
        btnPreviewAdd.Margin = new Padding(0, 0, 4, 0);
        btnPreviewAdd.Name = "btnPreviewAdd";
        btnPreviewAdd.Size = new Size(62, 28);
        btnPreviewAdd.TabIndex = 0;
        btnPreviewAdd.TabStop = false;
        btnPreviewAdd.Text = "جديد";
        toolTip.SetToolTip(btnPreviewAdd, "زر معاينة فقط — يعرض لون زر «جديد/إضافة»");
        btnPreviewAdd.UseVisualStyleBackColor = false;
        // 
        // btnPreviewSave
        // 
        btnPreviewSave.BackColor = Color.FromArgb(25, 135, 84);
        btnPreviewSave.FlatAppearance.BorderSize = 0;
        btnPreviewSave.FlatStyle = FlatStyle.Flat;
        btnPreviewSave.Font = new Font("Tahoma", 8F, FontStyle.Bold);
        btnPreviewSave.ForeColor = Color.White;
        btnPreviewSave.Location = new Point(66, 0);
        btnPreviewSave.Margin = new Padding(0, 0, 4, 0);
        btnPreviewSave.Name = "btnPreviewSave";
        btnPreviewSave.Size = new Size(62, 28);
        btnPreviewSave.TabIndex = 1;
        btnPreviewSave.TabStop = false;
        btnPreviewSave.Text = "حفظ";
        toolTip.SetToolTip(btnPreviewSave, "زر معاينة فقط — يعرض لون زر «حفظ»");
        btnPreviewSave.UseVisualStyleBackColor = false;
        // 
        // btnPreviewDelete
        // 
        btnPreviewDelete.BackColor = Color.FromArgb(220, 53, 69);
        btnPreviewDelete.FlatAppearance.BorderSize = 0;
        btnPreviewDelete.FlatStyle = FlatStyle.Flat;
        btnPreviewDelete.Font = new Font("Tahoma", 8F, FontStyle.Bold);
        btnPreviewDelete.ForeColor = Color.White;
        btnPreviewDelete.Location = new Point(132, 0);
        btnPreviewDelete.Margin = new Padding(0, 0, 4, 0);
        btnPreviewDelete.Name = "btnPreviewDelete";
        btnPreviewDelete.Size = new Size(62, 28);
        btnPreviewDelete.TabIndex = 2;
        btnPreviewDelete.TabStop = false;
        btnPreviewDelete.Text = "حذف";
        toolTip.SetToolTip(btnPreviewDelete, "زر معاينة فقط — يعرض لون زر «حذف»");
        btnPreviewDelete.UseVisualStyleBackColor = false;
        // 
        // btnPreviewCancel
        // 
        btnPreviewCancel.BackColor = Color.FromArgb(108, 117, 125);
        btnPreviewCancel.FlatAppearance.BorderSize = 0;
        btnPreviewCancel.FlatStyle = FlatStyle.Flat;
        btnPreviewCancel.Font = new Font("Tahoma", 8F, FontStyle.Bold);
        btnPreviewCancel.ForeColor = Color.White;
        btnPreviewCancel.Location = new Point(198, 0);
        btnPreviewCancel.Margin = new Padding(0, 0, 4, 0);
        btnPreviewCancel.Name = "btnPreviewCancel";
        btnPreviewCancel.Size = new Size(62, 28);
        btnPreviewCancel.TabIndex = 3;
        btnPreviewCancel.TabStop = false;
        btnPreviewCancel.Text = "إغلاق";
        toolTip.SetToolTip(btnPreviewCancel, "زر معاينة فقط — يعرض لون زر «إلغاء/إغلاق»");
        btnPreviewCancel.UseVisualStyleBackColor = false;
        // 
        // btnPreviewInfo
        // 
        btnPreviewInfo.BackColor = Color.FromArgb(23, 162, 184);
        btnPreviewInfo.FlatAppearance.BorderSize = 0;
        btnPreviewInfo.FlatStyle = FlatStyle.Flat;
        btnPreviewInfo.Font = new Font("Tahoma", 8F, FontStyle.Bold);
        btnPreviewInfo.ForeColor = Color.White;
        btnPreviewInfo.Location = new Point(264, 0);
        btnPreviewInfo.Margin = new Padding(0, 0, 4, 0);
        btnPreviewInfo.Name = "btnPreviewInfo";
        btnPreviewInfo.Size = new Size(62, 28);
        btnPreviewInfo.TabIndex = 4;
        btnPreviewInfo.TabStop = false;
        btnPreviewInfo.Text = "تصدير";
        toolTip.SetToolTip(btnPreviewInfo, "زر معاينة فقط — يعرض لون زر «تصدير/معلومات»");
        btnPreviewInfo.UseVisualStyleBackColor = false;
        // 
        // txtPreviewDisabled
        // 
        txtPreviewDisabled.Enabled = false;
        txtPreviewDisabled.Location = new Point(52, 190);
        txtPreviewDisabled.Name = "txtPreviewDisabled";
        txtPreviewDisabled.Size = new Size(290, 30);
        txtPreviewDisabled.TabIndex = 5;
        txtPreviewDisabled.TabStop = false;
        txtPreviewDisabled.Text = "حقل معطّل";
        // 
        // lblSelectedColor
        // 
        lblSelectedColor.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblSelectedColor.ForeColor = Color.FromArgb(14, 82, 52);
        lblSelectedColor.Location = new Point(12, 322);
        lblSelectedColor.Name = "lblSelectedColor";
        lblSelectedColor.Size = new Size(354, 26);
        lblSelectedColor.TabIndex = 2;
        lblSelectedColor.Text = "اللون المحدد: —";
        lblSelectedColor.TextAlign = ContentAlignment.MiddleRight;
        // 
        // pnlSwatch
        // 
        pnlSwatch.BackColor = Color.White;
        pnlSwatch.BorderStyle = BorderStyle.FixedSingle;
        pnlSwatch.Location = new Point(342, 352);
        pnlSwatch.Name = "pnlSwatch";
        pnlSwatch.Size = new Size(24, 26);
        pnlSwatch.TabIndex = 3;
        // 
        // txtHex
        // 
        txtHex.BackColor = Color.White;
        txtHex.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        txtHex.Location = new Point(230, 350);
        txtHex.Name = "txtHex";
        txtHex.ReadOnly = true;
        txtHex.Size = new Size(106, 28);
        txtHex.TabIndex = 4;
        txtHex.TextAlign = HorizontalAlignment.Center;
        // 
        // lblPreviewHint
        // 
        lblPreviewHint.ForeColor = Color.FromArgb(14, 82, 52);
        lblPreviewHint.Location = new Point(12, 388);
        lblPreviewHint.Name = "lblPreviewHint";
        lblPreviewHint.Size = new Size(354, 50);
        lblPreviewHint.TabIndex = 5;
        lblPreviewHint.Text = "التغييرات تظهر في المعاينة فقط حتى تضغط «حفظ وتطبيق».";
        // 
        // pnlFooter
        // 
        pnlFooter.BackColor = Color.FromArgb(235, 240, 235);
        pnlFooter.Controls.Add(lblFooterHint);
        pnlFooter.Controls.Add(flpFooter);
        pnlFooter.Dock = DockStyle.Bottom;
        pnlFooter.Location = new Point(0, 764);
        pnlFooter.Name = "pnlFooter";
        pnlFooter.Size = new Size(1200, 56);
        pnlFooter.TabIndex = 6;
        // 
        // lblFooterHint
        // 
        lblFooterHint.Dock = DockStyle.Fill;
        lblFooterHint.ForeColor = Color.FromArgb(14, 82, 52);
        lblFooterHint.Location = new Point(0, 0);
        lblFooterHint.Name = "lblFooterHint";
        lblFooterHint.Padding = new Padding(16, 0, 0, 0);
        lblFooterHint.Size = new Size(980, 56);
        lblFooterHint.TabIndex = 0;
        lblFooterHint.Text = "اختر لوناً من الشبكة ثم اضغط «اختيار اللون»، وستظهر النتيجة في المعاينة فوراً قبل الحفظ.";
        lblFooterHint.TextAlign = ContentAlignment.MiddleRight;
        // 
        // flpFooter
        // 
        flpFooter.BackColor = Color.FromArgb(235, 240, 235);
        flpFooter.Controls.Add(btnFooterClose);
        flpFooter.Dock = DockStyle.Right;
        flpFooter.FlowDirection = FlowDirection.LeftToRight;
        flpFooter.Location = new Point(980, 0);
        flpFooter.Name = "flpFooter";
        flpFooter.Padding = new Padding(8, 10, 8, 10);
        flpFooter.Size = new Size(220, 56);
        flpFooter.TabIndex = 1;
        flpFooter.WrapContents = false;
        // 
        // btnFooterClose
        // 
        btnFooterClose.BackColor = Color.FromArgb(108, 117, 125);
        btnFooterClose.Cursor = Cursors.Hand;
        btnFooterClose.FlatAppearance.BorderSize = 0;
        btnFooterClose.FlatStyle = FlatStyle.Flat;
        btnFooterClose.ForeColor = Color.White;
        btnFooterClose.Location = new Point(8, 10);
        btnFooterClose.Margin = new Padding(0);
        btnFooterClose.Name = "btnFooterClose";
        btnFooterClose.Size = new Size(140, 36);
        btnFooterClose.TabIndex = 0;
        btnFooterClose.Text = "إغلاق";
        toolTip.SetToolTip(btnFooterClose, "إغلاق الشاشة (Esc)");
        btnFooterClose.UseVisualStyleBackColor = false;
        btnFooterClose.Click += btnClose_Click;
        // 
        // frmChangeSystemColorsSettings
        // 
        AutoScaleDimensions = new SizeF(11F, 24F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(170, 220, 170);
        ClientSize = new Size(1200, 820);
        Controls.Add(gbColors);
        Controls.Add(gbPreview);
        Controls.Add(pnlFooter);
        Controls.Add(pnlSummary);
        Controls.Add(pnlFilters);
        Controls.Add(gbToolbar);
        Controls.Add(pnlHeader);
        Font = new Font("Tahoma", 10F);
        MinimumSize = new Size(1140, 760);
        Name = "frmChangeSystemColorsSettings";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterParent;
        Text = "ألوان النظام";
        Load += frmChangeSystemColorsSettings_Load;
        pnlHeader.ResumeLayout(false);
        gbToolbar.ResumeLayout(false);
        flpToolbar.ResumeLayout(false);
        pnlFilters.ResumeLayout(false);
        gbFilter.ResumeLayout(false);
        gbFilter.PerformLayout();
        pnlSummary.ResumeLayout(false);
        gbColors.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvColors).EndInit();
        cmsColors.ResumeLayout(false);
        gbPreview.ResumeLayout(false);
        gbPreview.PerformLayout();
        pnlPreviewShell.ResumeLayout(false);
        pnlPreviewShell.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvPreview).EndInit();
        flpPreviewButtons.ResumeLayout(false);
        pnlFooter.ResumeLayout(false);
        flpFooter.ResumeLayout(false);
        ResumeLayout(false);
    }

    private System.Windows.Forms.Panel pnlHeader;
    private System.Windows.Forms.Label lblMode;
    private System.Windows.Forms.GroupBox gbToolbar;
    private System.Windows.Forms.FlowLayoutPanel flpToolbar;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.Button btnReset;
    private System.Windows.Forms.Button btnPickColor;
    private System.Windows.Forms.Button btnRefresh;
    private System.Windows.Forms.Button btnClose;
    private System.Windows.Forms.Panel pnlFilters;
    private System.Windows.Forms.GroupBox gbFilter;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.ComboBox cbFilterBy;
    private System.Windows.Forms.TextBox txtFilterValue;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ComboBox cbGroup;
    private System.Windows.Forms.CheckBox chkOnlyChanged;
    private System.Windows.Forms.Button btnClearFilters;
    private System.Windows.Forms.Button btnToggleFilters;
    private System.Windows.Forms.Panel pnlSummary;
    private System.Windows.Forms.Label lblSummary;
    private System.Windows.Forms.GroupBox gbColors;
    private System.Windows.Forms.DataGridView dgvColors;
    private System.Windows.Forms.DataGridViewTextBoxColumn colNo;
    private System.Windows.Forms.DataGridViewTextBoxColumn colGroup;
    private System.Windows.Forms.DataGridViewTextBoxColumn colLabel;
    private System.Windows.Forms.DataGridViewTextBoxColumn colKey;
    private System.Windows.Forms.DataGridViewTextBoxColumn colHex;
    private System.Windows.Forms.DataGridViewTextBoxColumn colSample;
    private System.Windows.Forms.Label lblNoData;
    private System.Windows.Forms.ContextMenuStrip cmsColors;
    private System.Windows.Forms.ToolStripMenuItem miPickColor;
    private System.Windows.Forms.ToolStripMenuItem miCopyHex;
    private System.Windows.Forms.GroupBox gbPreview;
    private System.Windows.Forms.Label lblPreviewCaption;
    private System.Windows.Forms.Panel pnlPreviewShell;
    private System.Windows.Forms.Label lblPreviewHeader;
    private System.Windows.Forms.Label lblPreviewField;
    private System.Windows.Forms.TextBox txtPreviewInput;
    private System.Windows.Forms.DataGridView dgvPreview;
    private System.Windows.Forms.DataGridViewTextBoxColumn colPreviewLabel;
    private System.Windows.Forms.DataGridViewTextBoxColumn colPreviewAmount;
    private System.Windows.Forms.FlowLayoutPanel flpPreviewButtons;
    private System.Windows.Forms.Button btnPreviewAdd;
    private System.Windows.Forms.Button btnPreviewSave;
    private System.Windows.Forms.Button btnPreviewDelete;
    private System.Windows.Forms.Button btnPreviewCancel;
    private System.Windows.Forms.Button btnPreviewInfo;
    private System.Windows.Forms.TextBox txtPreviewDisabled;
    private System.Windows.Forms.Label lblSelectedColor;
    private System.Windows.Forms.Panel pnlSwatch;
    private System.Windows.Forms.TextBox txtHex;
    private System.Windows.Forms.Label lblPreviewHint;
    private System.Windows.Forms.Panel pnlFooter;
    private System.Windows.Forms.Label lblFooterHint;
    private System.Windows.Forms.FlowLayoutPanel flpFooter;
    private System.Windows.Forms.Button btnFooterClose;
    private System.Windows.Forms.ToolTip toolTip;
}
