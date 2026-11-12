namespace SalesSystem.Desktop.Screens.Expenses.Forms;

partial class frmListExpenses
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        components?.Dispose();
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        cmsExpenses = new ContextMenuStrip(components);
        editToolStripMenuItem = new ToolStripMenuItem();
        postToolStripMenuItem = new ToolStripMenuItem();
        cancelToolStripMenuItem = new ToolStripMenuItem();
        deleteToolStripMenuItem = new ToolStripMenuItem();
        dgvExpenses = new DataGridView();
        lblMode = new Label();
        pnlHeader = new Panel();
        pnlToolbar = new Panel();
        btnAddNew = new Button();
        btnRefresh = new Button();
        btnClose = new Button();
        pnlFilters = new Panel();
        gbFilter = new GroupBox();
        label1 = new Label();
        cbStatus = new ComboBox();
        label3 = new Label();
        dtpFrom = new DateTimePicker();
        label4 = new Label();
        dtpTo = new DateTimePicker();
        label2 = new Label();
        cbFilterBy = new ComboBox();
        txtSearch = new TextBox();
        btnRangeToday = new Button();
        btnRange30 = new Button();
        btnRangeMonth = new Button();
        chkOnlyCancelled = new CheckBox();
        pnlGrid = new Panel();
        lblEmpty = new Label();
        pnlSummary = new Panel();
        lblSummary = new Label();
        lblRecordsCount = new Label();
        toolTip1 = new ToolTip(components);
        liveTip = new ToolTip(components);
        cmsExpenses.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvExpenses).BeginInit();
        pnlHeader.SuspendLayout();
        pnlToolbar.SuspendLayout();
        pnlFilters.SuspendLayout();
        gbFilter.SuspendLayout();
        pnlGrid.SuspendLayout();
        pnlSummary.SuspendLayout();
        SuspendLayout();
        // 
        // cmsExpenses
        // 
        cmsExpenses.BackColor = Color.White;
        cmsExpenses.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        cmsExpenses.ImageScalingSize = new Size(24, 24);
        cmsExpenses.Items.AddRange(new ToolStripItem[] { editToolStripMenuItem, postToolStripMenuItem, cancelToolStripMenuItem, deleteToolStripMenuItem });
        cmsExpenses.Name = "cmsExpenses";
        cmsExpenses.RightToLeft = RightToLeft.Yes;
        cmsExpenses.Size = new Size(220, 124);
        cmsExpenses.Opening += cmsExpenses_Opening;
        // 
        // editToolStripMenuItem
        // 
        editToolStripMenuItem.Name = "editToolStripMenuItem";
        editToolStripMenuItem.Size = new Size(219, 30);
        editToolStripMenuItem.Text = "تعديل المصروف";
        editToolStripMenuItem.Click += editToolStripMenuItem_Click;
        // 
        // postToolStripMenuItem
        // 
        postToolStripMenuItem.Name = "postToolStripMenuItem";
        postToolStripMenuItem.Size = new Size(219, 30);
        postToolStripMenuItem.Text = "ترحيل";
        postToolStripMenuItem.Click += postToolStripMenuItem_Click;
        // 
        // cancelToolStripMenuItem
        // 
        cancelToolStripMenuItem.Name = "cancelToolStripMenuItem";
        cancelToolStripMenuItem.Size = new Size(219, 30);
        cancelToolStripMenuItem.Text = "إلغاء الترحيل";
        cancelToolStripMenuItem.Click += cancelToolStripMenuItem_Click;
        // 
        // deleteToolStripMenuItem
        // 
        deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
        deleteToolStripMenuItem.Size = new Size(219, 30);
        deleteToolStripMenuItem.Text = "حذف";
        deleteToolStripMenuItem.Click += deleteToolStripMenuItem_Click;
        // 
        // dgvExpenses
        // 
        dgvExpenses.AllowUserToAddRows = false;
        dgvExpenses.AllowUserToDeleteRows = false;
        dgvExpenses.BackgroundColor = SystemColors.Window;
        dgvExpenses.BorderStyle = BorderStyle.Fixed3D;
        dgvExpenses.ColumnHeadersHeight = 50;
        dgvExpenses.ContextMenuStrip = cmsExpenses;
        dgvExpenses.Dock = DockStyle.Fill;
        dgvExpenses.Location = new Point(6, 6);
        dgvExpenses.Margin = new Padding(4);
        dgvExpenses.MultiSelect = false;
        dgvExpenses.Name = "dgvExpenses";
        dgvExpenses.ReadOnly = true;
        dgvExpenses.RowHeadersWidth = 62;
        dgvExpenses.RowTemplate.Height = 28;
        dgvExpenses.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvExpenses.Size = new Size(1188, 447);
        dgvExpenses.TabIndex = 0;
        dgvExpenses.TabStop = false;
        dgvExpenses.DoubleClick += dgvExpenses_DoubleClick;
        // 
        // pnlHeader
        // 
        pnlHeader.BackColor = Color.WhiteSmoke;
        pnlHeader.Controls.Add(lblMode);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Location = new Point(0, 0);
        pnlHeader.Margin = new Padding(0);
        pnlHeader.Name = "pnlHeader";
        pnlHeader.Padding = new Padding(4, 4, 4, 4);
        pnlHeader.Size = new Size(1200, 56);
        pnlHeader.TabIndex = 0;
        // 
        // lblMode
        // 
        lblMode.BackColor = Color.WhiteSmoke;
        lblMode.Dock = DockStyle.Fill;
        lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblMode.ForeColor = Color.Firebrick;
        lblMode.Location = new Point(4, 4);
        lblMode.Margin = new Padding(0);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(1192, 48);
        lblMode.TabIndex = 0;
        lblMode.Text = "إدارة المصروفات";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlToolbar
        // 
        pnlToolbar.BackColor = Color.WhiteSmoke;
        pnlToolbar.Controls.Add(btnAddNew);
        pnlToolbar.Controls.Add(btnRefresh);
        pnlToolbar.Controls.Add(btnClose);
        pnlToolbar.Dock = DockStyle.Top;
        pnlToolbar.Location = new Point(0, 56);
        pnlToolbar.Margin = new Padding(0);
        pnlToolbar.Name = "pnlToolbar";
        pnlToolbar.Size = new Size(1200, 64);
        pnlToolbar.TabIndex = 1;
        // 
        // btnAddNew
        // 
        btnAddNew.BackColor = Color.White;
        btnAddNew.FlatAppearance.BorderColor = Color.Black;
        btnAddNew.FlatAppearance.BorderSize = 2;
        btnAddNew.FlatStyle = FlatStyle.Popup;
        btnAddNew.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnAddNew.Location = new Point(1056, 9);
        btnAddNew.Margin = new Padding(4);
        btnAddNew.Name = "btnAddNew";
        btnAddNew.Size = new Size(130, 46);
        btnAddNew.TabIndex = 0;
        btnAddNew.Text = "+ جديد";
        btnAddNew.UseVisualStyleBackColor = false;
        btnAddNew.Click += btnAddNew_Click;
        // 
        // btnRefresh
        // 
        btnRefresh.BackColor = Color.White;
        btnRefresh.FlatAppearance.BorderColor = Color.Black;
        btnRefresh.FlatAppearance.BorderSize = 2;
        btnRefresh.FlatStyle = FlatStyle.Popup;
        btnRefresh.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnRefresh.Location = new Point(912, 9);
        btnRefresh.Margin = new Padding(4);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(130, 46);
        btnRefresh.TabIndex = 1;
        btnRefresh.Text = "تحديث";
        btnRefresh.UseVisualStyleBackColor = false;
        btnRefresh.Click += btnRefresh_Click;
        // 
        // btnClose
        // 
        btnClose.BackColor = Color.White;
        btnClose.DialogResult = DialogResult.Cancel;
        btnClose.FlatStyle = FlatStyle.Popup;
        btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnClose.Location = new Point(768, 9);
        btnClose.Margin = new Padding(4);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(130, 46);
        btnClose.TabIndex = 2;
        btnClose.Text = "إغلاق";
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += btnClose_Click;
        // 
        // pnlFilters
        // 
        pnlFilters.BackColor = Color.WhiteSmoke;
        pnlFilters.Controls.Add(gbFilter);
        pnlFilters.Dock = DockStyle.Top;
        pnlFilters.Location = new Point(0, 120);
        pnlFilters.Margin = new Padding(0);
        pnlFilters.Name = "pnlFilters";
        pnlFilters.Padding = new Padding(6, 4, 6, 4);
        pnlFilters.Size = new Size(1200, 112);
        pnlFilters.TabIndex = 2;
        // 
        // gbFilter
        // 
        gbFilter.Controls.Add(label1);
        gbFilter.Controls.Add(cbStatus);
        gbFilter.Controls.Add(label3);
        gbFilter.Controls.Add(dtpFrom);
        gbFilter.Controls.Add(label4);
        gbFilter.Controls.Add(dtpTo);
        gbFilter.Controls.Add(label2);
        gbFilter.Controls.Add(cbFilterBy);
        gbFilter.Controls.Add(txtSearch);
        gbFilter.Controls.Add(btnRangeToday);
        gbFilter.Controls.Add(btnRange30);
        gbFilter.Controls.Add(btnRangeMonth);
        gbFilter.Controls.Add(chkOnlyCancelled);
        gbFilter.Dock = DockStyle.Fill;
        gbFilter.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        gbFilter.Location = new Point(6, 4);
        gbFilter.Margin = new Padding(0);
        gbFilter.Name = "gbFilter";
        gbFilter.Padding = new Padding(8, 8, 8, 8);
        gbFilter.RightToLeft = RightToLeft.Yes;
        gbFilter.Size = new Size(1188, 104);
        gbFilter.TabIndex = 0;
        gbFilter.TabStop = false;
        gbFilter.Text = "بحث وتصفية";
        // 
        // label1
        // 
        label1.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label1.Location = new Point(1104, 28);
        label1.Margin = new Padding(0);
        label1.Name = "label1";
        label1.Size = new Size(70, 26);
        label1.TabIndex = 0;
        label1.Text = "الحالة:";
        label1.TextAlign = ContentAlignment.MiddleRight;
        // 
        // cbStatus
        // 
        cbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
        cbStatus.FlatStyle = FlatStyle.Flat;
        cbStatus.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        cbStatus.FormattingEnabled = true;
        cbStatus.Items.AddRange(new object[] { "الكل", "مسودة", "مرحّل", "ملغى" });
        cbStatus.Location = new Point(978, 26);
        cbStatus.Margin = new Padding(0);
        cbStatus.Name = "cbStatus";
        cbStatus.Size = new Size(120, 26);
        cbStatus.TabIndex = 1;
        cbStatus.SelectedIndexChanged += cbStatus_SelectedIndexChanged;
        // 
        // label3
        // 
        label3.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label3.Location = new Point(926, 28);
        label3.Margin = new Padding(0);
        label3.Name = "label3";
        label3.Size = new Size(48, 26);
        label3.TabIndex = 2;
        label3.Text = "من:";
        label3.TextAlign = ContentAlignment.MiddleRight;
        // 
        // dtpFrom
        // 
        dtpFrom.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        dtpFrom.Format = DateTimePickerFormat.Short;
        dtpFrom.Location = new Point(792, 26);
        dtpFrom.Margin = new Padding(0);
        dtpFrom.Name = "dtpFrom";
        dtpFrom.Size = new Size(130, 26);
        dtpFrom.TabIndex = 3;
        dtpFrom.ValueChanged += dtpFrom_ValueChanged;
        // 
        // label4
        // 
        label4.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label4.Location = new Point(740, 28);
        label4.Margin = new Padding(0);
        label4.Name = "label4";
        label4.Size = new Size(48, 26);
        label4.TabIndex = 4;
        label4.Text = "إلى:";
        label4.TextAlign = ContentAlignment.MiddleRight;
        // 
        // dtpTo
        // 
        dtpTo.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        dtpTo.Format = DateTimePickerFormat.Short;
        dtpTo.Location = new Point(606, 26);
        dtpTo.Margin = new Padding(0);
        dtpTo.Name = "dtpTo";
        dtpTo.Size = new Size(130, 26);
        dtpTo.TabIndex = 5;
        dtpTo.ValueChanged += dtpTo_ValueChanged;
        // 
        // label2
        // 
        label2.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label2.Location = new Point(517, 28);
        label2.Margin = new Padding(0);
        label2.Name = "label2";
        label2.Size = new Size(85, 26);
        label2.TabIndex = 6;
        label2.Text = "تصفية بـ:";
        label2.TextAlign = ContentAlignment.MiddleRight;
        // 
        // cbFilterBy
        // 
        cbFilterBy.DropDownStyle = ComboBoxStyle.DropDownList;
        cbFilterBy.FlatStyle = FlatStyle.Flat;
        cbFilterBy.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        cbFilterBy.FormattingEnabled = true;
        cbFilterBy.Items.AddRange(new object[] { "لا شيء", "رقم السند", "البيان", "حساب المصروف", "الصندوق", "المبلغ" });
        cbFilterBy.Location = new Point(353, 26);
        cbFilterBy.Margin = new Padding(0);
        cbFilterBy.Name = "cbFilterBy";
        cbFilterBy.Size = new Size(160, 26);
        cbFilterBy.TabIndex = 7;
        cbFilterBy.SelectedIndexChanged += cbFilterBy_SelectedIndexChanged;
        // 
        // txtSearch
        // 
        txtSearch.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        txtSearch.Location = new Point(129, 26);
        txtSearch.Margin = new Padding(0);
        txtSearch.Name = "txtSearch";
        txtSearch.RightToLeft = RightToLeft.Yes;
        txtSearch.Size = new Size(220, 26);
        txtSearch.TabIndex = 8;
        txtSearch.TextChanged += txtSearch_TextChanged;
        txtSearch.KeyDown += txtSearch_KeyDown;
        // 
        // btnRangeToday
        // 
        btnRangeToday.BackColor = Color.White;
        btnRangeToday.FlatStyle = FlatStyle.Popup;
        btnRangeToday.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
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
        btnRange30.BackColor = Color.White;
        btnRange30.FlatStyle = FlatStyle.Popup;
        btnRange30.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
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
        btnRangeMonth.BackColor = Color.White;
        btnRangeMonth.FlatStyle = FlatStyle.Popup;
        btnRangeMonth.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnRangeMonth.Location = new Point(831, 62);
        btnRangeMonth.Margin = new Padding(0);
        btnRangeMonth.Name = "btnRangeMonth";
        btnRangeMonth.Size = new Size(110, 30);
        btnRangeMonth.TabIndex = 11;
        btnRangeMonth.Text = "هذا الشهر";
        btnRangeMonth.UseVisualStyleBackColor = false;
        btnRangeMonth.Click += btnRangeMonth_Click;
        // 
        // chkOnlyCancelled
        // 
        chkOnlyCancelled.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        chkOnlyCancelled.Location = new Point(683, 64);
        chkOnlyCancelled.Margin = new Padding(0);
        chkOnlyCancelled.Name = "chkOnlyCancelled";
        chkOnlyCancelled.Size = new Size(140, 26);
        chkOnlyCancelled.TabIndex = 12;
        chkOnlyCancelled.Text = "الملغاة فقط";
        chkOnlyCancelled.UseVisualStyleBackColor = false;
        chkOnlyCancelled.CheckedChanged += chkOnlyCancelled_CheckedChanged;
        // 
        // pnlGrid
        // 
        pnlGrid.Controls.Add(dgvExpenses);
        pnlGrid.Controls.Add(lblEmpty);
        pnlGrid.Dock = DockStyle.Fill;
        pnlGrid.Location = new Point(0, 232);
        pnlGrid.Margin = new Padding(0);
        pnlGrid.Name = "pnlGrid";
        pnlGrid.Padding = new Padding(6, 6, 6, 6);
        pnlGrid.Size = new Size(1200, 459);
        pnlGrid.TabIndex = 3;
        // 
        // lblEmpty
        // 
        lblEmpty.BackColor = Color.Transparent;
        lblEmpty.Dock = DockStyle.Fill;
        lblEmpty.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblEmpty.Location = new Point(6, 6);
        lblEmpty.Margin = new Padding(0);
        lblEmpty.Name = "lblEmpty";
        lblEmpty.Size = new Size(1188, 447);
        lblEmpty.TabIndex = 1;
        lblEmpty.Text = "لا توجد بيانات للعرض";
        lblEmpty.TextAlign = ContentAlignment.MiddleCenter;
        lblEmpty.Visible = false;
        // 
        // pnlSummary
        // 
        pnlSummary.Controls.Add(lblSummary);
        pnlSummary.Controls.Add(lblRecordsCount);
        pnlSummary.Dock = DockStyle.Bottom;
        pnlSummary.Location = new Point(0, 691);
        pnlSummary.Margin = new Padding(0);
        pnlSummary.Name = "pnlSummary";
        pnlSummary.Size = new Size(1200, 32);
        pnlSummary.TabIndex = 4;
        // 
        // lblSummary
        // 
        lblSummary.Dock = DockStyle.Fill;
        lblSummary.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblSummary.Location = new Point(90, 0);
        lblSummary.Margin = new Padding(0);
        lblSummary.Name = "lblSummary";
        lblSummary.Padding = new Padding(8, 0, 8, 0);
        lblSummary.Size = new Size(1110, 32);
        lblSummary.TabIndex = 0;
        lblSummary.Text = "المعروض: 0 من 0 — الإجمالي: 0.00 · مرحّل: 0 · ملغى: 0";
        lblSummary.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblRecordsCount
        // 
        lblRecordsCount.Dock = DockStyle.Right;
        lblRecordsCount.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblRecordsCount.Location = new Point(1110, 0);
        lblRecordsCount.Margin = new Padding(0);
        lblRecordsCount.Name = "lblRecordsCount";
        lblRecordsCount.Size = new Size(90, 32);
        lblRecordsCount.TabIndex = 1;
        lblRecordsCount.Text = "0";
        lblRecordsCount.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // toolTip1
        // 
        toolTip1.AutomaticDelay = 300;
        toolTip1.BackColor = Color.White;
        toolTip1.ForeColor = Color.Black;
        // 
        // liveTip
        // 
        liveTip.AutomaticDelay = 300;
        liveTip.BackColor = Color.White;
        liveTip.ForeColor = Color.Black;
        // 
        // frmListExpenses
        // 
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.WhiteSmoke;
        ClientSize = new Size(1200, 723);
        Controls.Add(pnlGrid);
        Controls.Add(pnlSummary);
        Controls.Add(pnlFilters);
        Controls.Add(pnlToolbar);
        Controls.Add(pnlHeader);
        Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        Name = "frmListExpenses";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "إدارة المصروفات";
        Load += frmListExpenses_Load;
        cmsExpenses.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvExpenses).EndInit();
        pnlHeader.ResumeLayout(false);
        pnlToolbar.ResumeLayout(false);
        pnlFilters.ResumeLayout(false);
        gbFilter.ResumeLayout(false);
        gbFilter.PerformLayout();
        pnlGrid.ResumeLayout(false);
        pnlSummary.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private DataGridView dgvExpenses;
    private Label lblMode;
    private Panel pnlHeader;
    private Panel pnlToolbar;
    private Button btnAddNew;
    private Button btnRefresh;
    private Button btnClose;
    private Panel pnlFilters;
    private GroupBox gbFilter;
    private Label label1;
    private ComboBox cbStatus;
    private Label label3;
    private DateTimePicker dtpFrom;
    private Label label4;
    private DateTimePicker dtpTo;
    private Label label2;
    private ComboBox cbFilterBy;
    private TextBox txtSearch;
    private Button btnRangeToday;
    private Button btnRange30;
    private Button btnRangeMonth;
    private CheckBox chkOnlyCancelled;
    private Panel pnlGrid;
    private Label lblEmpty;
    private Panel pnlSummary;
    private Label lblSummary;
    private Label lblRecordsCount;
    private ToolTip toolTip1;
    private ToolTip liveTip;
    private ContextMenuStrip cmsExpenses;
    private ToolStripMenuItem editToolStripMenuItem;
    private ToolStripMenuItem postToolStripMenuItem;
    private ToolStripMenuItem cancelToolStripMenuItem;
    private ToolStripMenuItem deleteToolStripMenuItem;
}
