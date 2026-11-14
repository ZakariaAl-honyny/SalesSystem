namespace SalesSystem.Desktop.Screens.Inventory.Forms;

partial class frmInventoryList
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            components?.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        cmsMovements = new ContextMenuStrip(components);
        miDrillDown = new ToolStripMenuItem();
        miRefresh = new ToolStripMenuItem();
        miClearFilters = new ToolStripMenuItem();
        toolTip1 = new ToolTip(components);
        lblMode = new Label();
        pnlHeader = new Panel();
        pnlToolbar = new Panel();
        btnRefresh = new Button();
        btnClose = new Button();
        pnlFilters = new Panel();
        gbFilter = new GroupBox();
        lblStatus = new Label();
        cbStatus = new ComboBox();
        lblFrom = new Label();
        dtpFrom = new DateTimePicker();
        lblTo = new Label();
        dtpTo = new DateTimePicker();
        lblFilterBy = new Label();
        cbFilterBy = new ComboBox();
        txtFilterValue = new TextBox();
        btnRangeToday = new Button();
        btnRange30 = new Button();
        btnRangeMonth = new Button();
        chkOnlyOutOfStock = new CheckBox();
        btnClearFilters = new Button();
        btnToggleFilters = new Button();
        pnlGrid = new Panel();
        dgvMovements = new DataGridView();
        lblEmpty = new Label();
        pnlSummary = new Panel();
        lblRecordsCount = new Label();
        label1 = new Label();
        lblSummary = new Label();
        cmsMovements.SuspendLayout();
        pnlHeader.SuspendLayout();
        pnlToolbar.SuspendLayout();
        pnlFilters.SuspendLayout();
        gbFilter.SuspendLayout();
        pnlGrid.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvMovements).BeginInit();
        pnlSummary.SuspendLayout();
        SuspendLayout();
        // 
        // cmsMovements
        // 
        cmsMovements.Items.AddRange(new ToolStripItem[] { miDrillDown, miRefresh, miClearFilters });
        cmsMovements.Name = "cmsMovements";
        cmsMovements.RightToLeft = RightToLeft.Yes;
        cmsMovements.Size = new Size(200, 82);
        // 
        // miDrillDown
        // 
        miDrillDown.Name = "miDrillDown";
        miDrillDown.Size = new Size(199, 26);
        miDrillDown.Text = "تصفية على قيمة الخلية";
        miDrillDown.Click += miDrillDown_Click;
        // 
        // miRefresh
        // 
        miRefresh.Name = "miRefresh";
        miRefresh.Size = new Size(199, 26);
        miRefresh.Text = "تحديث البيانات (F5)";
        miRefresh.Click += miRefresh_Click;
        // 
        // miClearFilters
        // 
        miClearFilters.Name = "miClearFilters";
        miClearFilters.Size = new Size(199, 26);
        miClearFilters.Text = "إلغاء التصفية (Esc)";
        miClearFilters.Click += miClearFilters_Click;
        // 
        // pnlHeader
        // 
        pnlHeader.BackColor = Color.FromArgb(14, 165, 103);
        pnlHeader.Controls.Add(lblMode);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Location = new Point(0, 0);
        pnlHeader.Margin = new Padding(0);
        pnlHeader.Name = "pnlHeader";
        pnlHeader.Padding = new Padding(4);
        pnlHeader.Size = new Size(1360, 56);
        pnlHeader.TabIndex = 0;
        // 
        // lblMode
        // 
        lblMode.BackColor = Color.FromArgb(14, 165, 103);
        lblMode.Dock = DockStyle.Fill;
        lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold);
        lblMode.ForeColor = Color.White;
        lblMode.Location = new Point(4, 4);
        lblMode.Margin = new Padding(0);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(1352, 48);
        lblMode.TabIndex = 0;
        lblMode.Text = "حركات المخزون";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlToolbar
        // 
        pnlToolbar.BackColor = Color.FromArgb(235, 240, 235);
        pnlToolbar.Controls.Add(btnRefresh);
        pnlToolbar.Controls.Add(btnClose);
        pnlToolbar.Dock = DockStyle.Top;
        pnlToolbar.Location = new Point(0, 56);
        pnlToolbar.Margin = new Padding(0);
        pnlToolbar.Name = "pnlToolbar";
        pnlToolbar.Size = new Size(1360, 64);
        pnlToolbar.TabIndex = 1;
        // 
        // btnRefresh
        // 
        btnRefresh.BackColor = Color.FromArgb(23, 162, 184);
        btnRefresh.Cursor = Cursors.Hand;
        btnRefresh.FlatAppearance.BorderSize = 0;
        btnRefresh.FlatStyle = FlatStyle.Flat;
        btnRefresh.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnRefresh.ForeColor = Color.White;
        btnRefresh.Location = new Point(1216, 9);
        btnRefresh.Margin = new Padding(4);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(130, 46);
        btnRefresh.TabIndex = 0;
        btnRefresh.Text = "تحديث";
        btnRefresh.UseVisualStyleBackColor = false;
        btnRefresh.Click += btnRefresh_Click;
        // 
        // btnClose
        // 
        btnClose.BackColor = Color.FromArgb(108, 117, 125);
        btnClose.Cursor = Cursors.Hand;
        btnClose.DialogResult = DialogResult.Cancel;
        btnClose.FlatAppearance.BorderSize = 0;
        btnClose.FlatStyle = FlatStyle.Flat;
        btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnClose.ForeColor = Color.White;
        btnClose.Location = new Point(1072, 9);
        btnClose.Margin = new Padding(4);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(130, 46);
        btnClose.TabIndex = 1;
        btnClose.Text = "إغلاق";
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += btnClose_Click;
        // 
        // pnlFilters
        // 
        pnlFilters.BackColor = Color.FromArgb(235, 240, 235);
        pnlFilters.Controls.Add(gbFilter);
        pnlFilters.Controls.Add(btnToggleFilters);
        pnlFilters.Dock = DockStyle.Top;
        pnlFilters.Location = new Point(0, 120);
        pnlFilters.Margin = new Padding(0);
        pnlFilters.Name = "pnlFilters";
        pnlFilters.Padding = new Padding(6, 4, 6, 4);
        pnlFilters.Size = new Size(1360, 112);
        pnlFilters.TabIndex = 2;
        // 
        // gbFilter
        // 
        gbFilter.BackColor = Color.FromArgb(235, 245, 235);
        gbFilter.Controls.Add(lblStatus);
        gbFilter.Controls.Add(cbStatus);
        gbFilter.Controls.Add(lblFrom);
        gbFilter.Controls.Add(dtpFrom);
        gbFilter.Controls.Add(lblTo);
        gbFilter.Controls.Add(dtpTo);
        gbFilter.Controls.Add(lblFilterBy);
        gbFilter.Controls.Add(cbFilterBy);
        gbFilter.Controls.Add(txtFilterValue);
        gbFilter.Controls.Add(btnRangeToday);
        gbFilter.Controls.Add(btnRange30);
        gbFilter.Controls.Add(btnRangeMonth);
        gbFilter.Controls.Add(chkOnlyOutOfStock);
        gbFilter.Controls.Add(btnClearFilters);
        gbFilter.Dock = DockStyle.Fill;
        gbFilter.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        gbFilter.ForeColor = Color.FromArgb(14, 82, 52);
        gbFilter.Location = new Point(6, 4);
        gbFilter.Margin = new Padding(0);
        gbFilter.Name = "gbFilter";
        gbFilter.Padding = new Padding(8);
        gbFilter.RightToLeft = RightToLeft.Yes;
        gbFilter.Size = new Size(1208, 104);
        gbFilter.TabIndex = 0;
        gbFilter.TabStop = false;
        gbFilter.Text = "بحث وتصفية";
        // 
        // lblStatus
        // 
        lblStatus.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblStatus.ForeColor = Color.FromArgb(14, 82, 52);
        lblStatus.Location = new Point(1104, 28);
        lblStatus.Margin = new Padding(0);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(70, 26);
        lblStatus.TabIndex = 0;
        lblStatus.Text = "الاتجاه:";
        lblStatus.TextAlign = ContentAlignment.MiddleRight;
        // 
        // cbStatus
        // 
        cbStatus.BackColor = Color.White;
        cbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
        cbStatus.FlatStyle = FlatStyle.Flat;
        cbStatus.Font = new Font("Tahoma", 9F, FontStyle.Regular);
        cbStatus.FormattingEnabled = true;
        cbStatus.Items.AddRange(new object[] { "الكل", "وارد (داخل)", "صادر (خارج)" });
        cbStatus.Location = new Point(978, 26);
        cbStatus.Margin = new Padding(0);
        cbStatus.Name = "cbStatus";
        cbStatus.Size = new Size(120, 26);
        cbStatus.TabIndex = 1;
        cbStatus.SelectedIndexChanged += cbStatus_SelectedIndexChanged;
        // 
        // lblFrom
        // 
        lblFrom.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblFrom.ForeColor = Color.FromArgb(14, 82, 52);
        lblFrom.Location = new Point(926, 28);
        lblFrom.Margin = new Padding(0);
        lblFrom.Name = "lblFrom";
        lblFrom.Size = new Size(48, 26);
        lblFrom.TabIndex = 2;
        lblFrom.Text = "من:";
        lblFrom.TextAlign = ContentAlignment.MiddleRight;
        // 
        // dtpFrom
        // 
        dtpFrom.Font = new Font("Tahoma", 9F, FontStyle.Regular);
        dtpFrom.Format = DateTimePickerFormat.Short;
        dtpFrom.Location = new Point(792, 26);
        dtpFrom.Margin = new Padding(0);
        dtpFrom.Name = "dtpFrom";
        dtpFrom.Size = new Size(130, 26);
        dtpFrom.TabIndex = 3;
        dtpFrom.ValueChanged += dtpFrom_ValueChanged;
        // 
        // lblTo
        // 
        lblTo.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblTo.ForeColor = Color.FromArgb(14, 82, 52);
        lblTo.Location = new Point(740, 28);
        lblTo.Margin = new Padding(0);
        lblTo.Name = "lblTo";
        lblTo.Size = new Size(48, 26);
        lblTo.TabIndex = 4;
        lblTo.Text = "إلى:";
        lblTo.TextAlign = ContentAlignment.MiddleRight;
        // 
        // dtpTo
        // 
        dtpTo.Font = new Font("Tahoma", 9F, FontStyle.Regular);
        dtpTo.Format = DateTimePickerFormat.Short;
        dtpTo.Location = new Point(606, 26);
        dtpTo.Margin = new Padding(0);
        dtpTo.Name = "dtpTo";
        dtpTo.Size = new Size(130, 26);
        dtpTo.TabIndex = 5;
        dtpTo.ValueChanged += dtpTo_ValueChanged;
        // 
        // lblFilterBy
        // 
        lblFilterBy.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblFilterBy.ForeColor = Color.FromArgb(14, 82, 52);
        lblFilterBy.Location = new Point(517, 28);
        lblFilterBy.Margin = new Padding(0);
        lblFilterBy.Name = "lblFilterBy";
        lblFilterBy.Size = new Size(85, 26);
        lblFilterBy.TabIndex = 6;
        lblFilterBy.Text = "تصفية بـ:";
        lblFilterBy.TextAlign = ContentAlignment.MiddleRight;
        // 
        // cbFilterBy
        // 
        cbFilterBy.BackColor = Color.White;
        cbFilterBy.DropDownStyle = ComboBoxStyle.DropDownList;
        cbFilterBy.FlatStyle = FlatStyle.Flat;
        cbFilterBy.Font = new Font("Tahoma", 9F, FontStyle.Regular);
        cbFilterBy.FormattingEnabled = true;
        cbFilterBy.Items.AddRange(new object[] { "الكل (مستودع/مرجع/نوع)", "المستودع", "رقم المرجع", "نوع الحركة" });
        cbFilterBy.Location = new Point(353, 26);
        cbFilterBy.Margin = new Padding(0);
        cbFilterBy.Name = "cbFilterBy";
        cbFilterBy.Size = new Size(160, 26);
        cbFilterBy.TabIndex = 7;
        cbFilterBy.SelectedIndexChanged += cbFilterBy_SelectedIndexChanged;
        // 
        // txtFilterValue
        // 
        txtFilterValue.BackColor = Color.White;
        txtFilterValue.Font = new Font("Tahoma", 9F, FontStyle.Regular);
        txtFilterValue.Location = new Point(129, 26);
        txtFilterValue.Margin = new Padding(0);
        txtFilterValue.Name = "txtFilterValue";
        txtFilterValue.PlaceholderText = "اكتب قيمة التصفية...";
        txtFilterValue.RightToLeft = RightToLeft.Yes;
        txtFilterValue.Size = new Size(220, 26);
        txtFilterValue.TabIndex = 8;
        txtFilterValue.TextChanged += txtFilterValue_TextChanged;
        txtFilterValue.KeyDown += txtFilterValue_KeyDown;
        // 
        // btnRangeToday
        // 
        btnRangeToday.BackColor = Color.FromArgb(248, 249, 250);
        btnRangeToday.Cursor = Cursors.Hand;
        btnRangeToday.FlatAppearance.BorderColor = Color.FromArgb(222, 226, 230);
        btnRangeToday.FlatAppearance.BorderSize = 1;
        btnRangeToday.FlatStyle = FlatStyle.Popup;
        btnRangeToday.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        btnRangeToday.ForeColor = Color.FromArgb(33, 37, 41);
        btnRangeToday.Location = new Point(1074, 62);
        btnRangeToday.Margin = new Padding(0);
        btnRangeToday.Name = "btnRangeToday";
        btnRangeToday.Size = new Size(100, 30);
        btnRangeToday.TabIndex = 9;
        btnRangeToday.Text = "اليوم";
        btnRangeToday.UseVisualStyleBackColor = false;
        btnRangeToday.Click += btnRangeToday_Click;
        // 
        // btnRange30
        // 
        btnRange30.BackColor = Color.FromArgb(248, 249, 250);
        btnRange30.Cursor = Cursors.Hand;
        btnRange30.FlatAppearance.BorderColor = Color.FromArgb(222, 226, 230);
        btnRange30.FlatAppearance.BorderSize = 1;
        btnRange30.FlatStyle = FlatStyle.Popup;
        btnRange30.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        btnRange30.ForeColor = Color.FromArgb(33, 37, 41);
        btnRange30.Location = new Point(945, 62);
        btnRange30.Margin = new Padding(0);
        btnRange30.Name = "btnRange30";
        btnRange30.Size = new Size(125, 30);
        btnRange30.TabIndex = 10;
        btnRange30.Text = "آخر 30 يوماً";
        btnRange30.UseVisualStyleBackColor = false;
        btnRange30.Click += btnRange30_Click;
        // 
        // btnRangeMonth
        // 
        btnRangeMonth.BackColor = Color.FromArgb(248, 249, 250);
        btnRangeMonth.Cursor = Cursors.Hand;
        btnRangeMonth.FlatAppearance.BorderColor = Color.FromArgb(222, 226, 230);
        btnRangeMonth.FlatAppearance.BorderSize = 1;
        btnRangeMonth.FlatStyle = FlatStyle.Popup;
        btnRangeMonth.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        btnRangeMonth.ForeColor = Color.FromArgb(33, 37, 41);
        btnRangeMonth.Location = new Point(831, 62);
        btnRangeMonth.Margin = new Padding(0);
        btnRangeMonth.Name = "btnRangeMonth";
        btnRangeMonth.Size = new Size(110, 30);
        btnRangeMonth.TabIndex = 11;
        btnRangeMonth.Text = "هذا الشهر";
        btnRangeMonth.UseVisualStyleBackColor = false;
        btnRangeMonth.Click += btnRangeMonth_Click;
        // 
        // chkOnlyOutOfStock
        // 
        chkOnlyOutOfStock.Font = new Font("Tahoma", 9F, FontStyle.Regular);
        chkOnlyOutOfStock.ForeColor = Color.FromArgb(14, 82, 52);
        chkOnlyOutOfStock.Location = new Point(613, 64);
        chkOnlyOutOfStock.Margin = new Padding(0);
        chkOnlyOutOfStock.Name = "chkOnlyOutOfStock";
        chkOnlyOutOfStock.Size = new Size(200, 26);
        chkOnlyOutOfStock.TabIndex = 12;
        chkOnlyOutOfStock.Text = "الرصيد النافد فقط";
        chkOnlyOutOfStock.UseVisualStyleBackColor = false;
        chkOnlyOutOfStock.CheckedChanged += chkOnlyOutOfStock_CheckedChanged;
        // 
        // btnClearFilters
        // 
        // RULE-N148: زر «مسح التصفية» داخل gbFilter — يمسح أدوات التصفية فقط بلا إعادة تحميل.
        btnClearFilters.BackColor = Color.FromArgb(108, 117, 125);
        btnClearFilters.Cursor = Cursors.Hand;
        btnClearFilters.Enabled = false;
        btnClearFilters.FlatAppearance.BorderSize = 0;
        btnClearFilters.FlatStyle = FlatStyle.Flat;
        btnClearFilters.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        btnClearFilters.ForeColor = Color.White;
        btnClearFilters.Location = new Point(460, 61);
        btnClearFilters.Margin = new Padding(0);
        btnClearFilters.Name = "btnClearFilters";
        btnClearFilters.Size = new Size(145, 32);
        btnClearFilters.TabIndex = 13;
        btnClearFilters.Text = "\U0001f9f9 مسح التصفية";
        toolTip1.SetToolTip(btnClearFilters, "مسح كل أدوات التصفية (Esc) — لا يعيد تحميل البيانات");
        btnClearFilters.UseVisualStyleBackColor = false;
        btnClearFilters.Click += btnClearFilters_Click;
        // 
        // btnToggleFilters
        // 
        // RULE-N145: زر «إخفاء/إظهار التصفية» مُعلَن في المصمّم بقيم حرفية مطابقة لما يُطبَّق
        // وقت التشغيل؛ السلوك (النقرة · F9 · التلميح · الحالة المحفوظة) يُوصِّله FilterPanelHelper.
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
        btnToggleFilters.Margin = new Padding(0);
        btnToggleFilters.Name = "btnToggleFilters";
        btnToggleFilters.Size = new Size(140, 104);
        btnToggleFilters.TabIndex = 1;
        btnToggleFilters.Text = "إخفاء التصفية";
        btnToggleFilters.UseVisualStyleBackColor = false;
        // 
        // pnlGrid
        // 
        pnlGrid.BackColor = Color.FromArgb(235, 240, 235);
        pnlGrid.Controls.Add(dgvMovements);
        pnlGrid.Controls.Add(lblEmpty);
        pnlGrid.Dock = DockStyle.Fill;
        pnlGrid.Location = new Point(0, 232);
        pnlGrid.Margin = new Padding(0);
        pnlGrid.Name = "pnlGrid";
        pnlGrid.Padding = new Padding(6);
        pnlGrid.Size = new Size(1360, 516);
        pnlGrid.TabIndex = 3;
        // 
        // dgvMovements
        // 
        dgvMovements.AllowUserToAddRows = false;
        dgvMovements.AllowUserToDeleteRows = false;
        dgvMovements.AllowUserToOrderColumns = true;
        dgvMovements.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 252, 245);
        dgvMovements.AutoGenerateColumns = false;
        dgvMovements.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvMovements.BackgroundColor = Color.FromArgb(220, 240, 220);
        dgvMovements.BorderStyle = BorderStyle.None;
        dgvMovements.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(14, 165, 103);
        dgvMovements.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        dgvMovements.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(14, 165, 103);
        dgvMovements.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
        dgvMovements.ColumnHeadersHeight = 45;
        dgvMovements.ContextMenuStrip = cmsMovements;
        dgvMovements.DefaultCellStyle.BackColor = Color.White;
        dgvMovements.DefaultCellStyle.ForeColor = Color.FromArgb(33, 37, 41);
        dgvMovements.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 255, 180);
        dgvMovements.DefaultCellStyle.SelectionForeColor = Color.FromArgb(33, 37, 41);
        dgvMovements.Dock = DockStyle.Fill;
        dgvMovements.EnableHeadersVisualStyles = false;
        dgvMovements.GridColor = Color.FromArgb(205, 225, 205);
        dgvMovements.Location = new Point(6, 6);
        dgvMovements.Margin = new Padding(0);
        dgvMovements.MultiSelect = false;
        dgvMovements.Name = "dgvMovements";
        dgvMovements.ReadOnly = true;
        dgvMovements.RowHeadersVisible = false;
        dgvMovements.RowHeadersWidth = 62;
        dgvMovements.RowTemplate.Height = 32;
        dgvMovements.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvMovements.Size = new Size(1348, 504);
        dgvMovements.TabIndex = 0;
        dgvMovements.CellDoubleClick += dgvMovements_CellDoubleClick;
        dgvMovements.CellFormatting += dgvMovements_CellFormatting;
        // 
        // lblEmpty
        // 
        lblEmpty.BackColor = Color.Transparent;
        lblEmpty.Dock = DockStyle.Fill;
        lblEmpty.Font = new Font("Tahoma", 12F, FontStyle.Bold);
        lblEmpty.ForeColor = Color.FromArgb(14, 82, 52);
        lblEmpty.Location = new Point(6, 6);
        lblEmpty.Margin = new Padding(0);
        lblEmpty.Name = "lblEmpty";
        lblEmpty.Size = new Size(1348, 504);
        lblEmpty.TabIndex = 1;
        lblEmpty.Text = "لا توجد بيانات للعرض";
        lblEmpty.TextAlign = ContentAlignment.MiddleCenter;
        lblEmpty.Visible = false;
        // 
        // pnlSummary
        // 
        pnlSummary.BackColor = Color.FromArgb(235, 240, 235);
        pnlSummary.Controls.Add(lblSummary);
        pnlSummary.Controls.Add(lblRecordsCount);
        pnlSummary.Controls.Add(label1);
        pnlSummary.Dock = DockStyle.Bottom;
        pnlSummary.Location = new Point(0, 748);
        pnlSummary.Margin = new Padding(0);
        pnlSummary.Name = "pnlSummary";
        pnlSummary.Size = new Size(1360, 32);
        pnlSummary.TabIndex = 4;
        // 
        // lblSummary
        // 
        lblSummary.Dock = DockStyle.Fill;
        lblSummary.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblSummary.ForeColor = Color.FromArgb(14, 82, 52);
        lblSummary.Location = new Point(0, 0);
        lblSummary.Margin = new Padding(0);
        lblSummary.Name = "lblSummary";
        lblSummary.Padding = new Padding(8, 0, 8, 0);
        lblSummary.Size = new Size(1140, 32);
        lblSummary.TabIndex = 0;
        lblSummary.Text = "المعروض: 0 من 0";
        lblSummary.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblRecordsCount
        // 
        lblRecordsCount.Dock = DockStyle.Right;
        lblRecordsCount.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblRecordsCount.ForeColor = Color.FromArgb(14, 82, 52);
        lblRecordsCount.Location = new Point(1140, 0);
        lblRecordsCount.Margin = new Padding(0);
        lblRecordsCount.Name = "lblRecordsCount";
        lblRecordsCount.Size = new Size(90, 32);
        lblRecordsCount.TabIndex = 1;
        lblRecordsCount.Text = "0";
        lblRecordsCount.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // label1
        // 
        label1.Dock = DockStyle.Right;
        label1.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        label1.ForeColor = Color.FromArgb(14, 82, 52);
        label1.Location = new Point(1230, 0);
        label1.Margin = new Padding(0);
        label1.Name = "label1";
        label1.Size = new Size(130, 32);
        label1.TabIndex = 2;
        label1.Text = "عدد الحركات:";
        label1.TextAlign = ContentAlignment.MiddleRight;
        // 
        // toolTip1
        // 
        toolTip1.AutomaticDelay = 300;
        toolTip1.BackColor = Color.White;
        toolTip1.ForeColor = Color.Black;
        // 
        // frmInventoryList
        // 
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.FromArgb(170, 220, 170);
        CancelButton = btnClose;
        ClientSize = new Size(1360, 780);
        Controls.Add(pnlGrid);
        Controls.Add(pnlSummary);
        Controls.Add(pnlFilters);
        Controls.Add(pnlToolbar);
        Controls.Add(pnlHeader);
        Font = new Font("Tahoma", 10F, FontStyle.Regular);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        Name = "frmInventoryList";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "حركات المخزون";
        Load += frmInventoryList_Load;
        cmsMovements.ResumeLayout(false);
        pnlHeader.ResumeLayout(false);
        pnlToolbar.ResumeLayout(false);
        pnlFilters.ResumeLayout(false);
        gbFilter.ResumeLayout(false);
        gbFilter.PerformLayout();
        pnlGrid.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvMovements).EndInit();
        pnlSummary.ResumeLayout(false);
        ResumeLayout(false);
    }

    #endregion

    private ContextMenuStrip cmsMovements;
    private ToolStripMenuItem miDrillDown;
    private ToolStripMenuItem miRefresh;
    private ToolStripMenuItem miClearFilters;
    private ToolTip toolTip1;
    private Label lblMode;
    private Panel pnlHeader;
    private Panel pnlToolbar;
    private Button btnRefresh;
    private Button btnClose;
    private Panel pnlFilters;
    private GroupBox gbFilter;
    private Label lblStatus;
    private ComboBox cbStatus;
    private Label lblFrom;
    private DateTimePicker dtpFrom;
    private Label lblTo;
    private DateTimePicker dtpTo;
    private Label lblFilterBy;
    private ComboBox cbFilterBy;
    private TextBox txtFilterValue;
    private Button btnRangeToday;
    private Button btnRange30;
    private Button btnRangeMonth;
    private CheckBox chkOnlyOutOfStock;
    private Button btnClearFilters;
    private Button btnToggleFilters;
    private Panel pnlGrid;
    private DataGridView dgvMovements;
    private Label lblEmpty;
    private Panel pnlSummary;
    private Label lblRecordsCount;
    private Label label1;
    private Label lblSummary;
}
