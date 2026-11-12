namespace SalesSystem.Desktop.Screens.Employees.Forms;

partial class frmEmployeeMaster
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
        lblMode = new Label();
        pnlFields = new Panel();
        lblCode = new Label();
        txtCode = new TextBox();
        lblName = new Label();
        lblNameReq = new Label();
        txtName = new TextBox();
        lblJobTitle = new Label();
        txtJobTitle = new TextBox();
        lblNationalId = new Label();
        txtNationalId = new TextBox();
        lblPhone = new Label();
        txtPhone = new TextBox();
        lblBasicSalary = new Label();
        numBasicSalary = new NumericUpDown();
        lblHireDate = new Label();
        dtpHireDate = new DateTimePicker();
        pnlButtons = new FlowLayoutPanel();
        btnNew = new Button();
        btnSave = new Button();
        btnEdit = new Button();
        btnDelete = new Button();
        btnRefresh = new Button();
        btnClose = new Button();
        pnlFilters = new Panel();
        gbFilter = new GroupBox();
        label2 = new Label();
        cbFilterBy = new ComboBox();
        txtFilterValue = new TextBox();
        label1 = new Label();
        cbStatus = new ComboBox();
        chkShowInactive = new CheckBox();
        chkHasSalary = new CheckBox();
        pnlGrid = new Panel();
        dgvEmployees = new DataGridView();
        pnlSummary = new Panel();
        lblSummary = new Label();
        errorProvider = new ErrorProvider(components);
        tafqeetTip = new ToolTip(components);
        pnlFields.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numBasicSalary).BeginInit();
        pnlButtons.SuspendLayout();
        pnlFilters.SuspendLayout();
        gbFilter.SuspendLayout();
        pnlGrid.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvEmployees).BeginInit();
        pnlSummary.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
        SuspendLayout();
        // 
        // lblMode
        // 
        lblMode.Dock = DockStyle.Top;
        lblMode.Font = new Font("Tahoma", 14F, FontStyle.Bold);
        lblMode.ForeColor = Color.White;
        lblMode.Location = new Point(0, 0);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(1432, 50);
        lblMode.TabIndex = 0;
        lblMode.Text = "(( دليل الموظفين ))";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlFields
        // 
        pnlFields.Controls.Add(lblCode);
        pnlFields.Controls.Add(txtCode);
        pnlFields.Controls.Add(lblName);
        pnlFields.Controls.Add(lblNameReq);
        pnlFields.Controls.Add(txtName);
        pnlFields.Controls.Add(lblJobTitle);
        pnlFields.Controls.Add(txtJobTitle);
        pnlFields.Controls.Add(lblNationalId);
        pnlFields.Controls.Add(txtNationalId);
        pnlFields.Controls.Add(lblPhone);
        pnlFields.Controls.Add(txtPhone);
        pnlFields.Controls.Add(lblBasicSalary);
        pnlFields.Controls.Add(numBasicSalary);
        pnlFields.Controls.Add(lblHireDate);
        pnlFields.Controls.Add(dtpHireDate);
        pnlFields.Dock = DockStyle.Top;
        pnlFields.Location = new Point(0, 50);
        pnlFields.Name = "pnlFields";
        pnlFields.Size = new Size(1432, 150);
        pnlFields.TabIndex = 1;
        // 
        // lblCode
        // 
        lblCode.AutoSize = true;
        lblCode.Location = new Point(1267, 15);
        lblCode.Name = "lblCode";
        lblCode.Size = new Size(108, 24);
        lblCode.TabIndex = 0;
        lblCode.Text = "رقم الموظف";
        // 
        // txtCode
        // 
        txtCode.Location = new Point(1267, 42);
        txtCode.Name = "txtCode";
        txtCode.ReadOnly = true;
        txtCode.Size = new Size(110, 32);
        txtCode.TabIndex = 0;
        txtCode.TabStop = false;
        // 
        // lblName
        // 
        lblName.AutoSize = true;
        lblName.Location = new Point(967, 15);
        lblName.Name = "lblName";
        lblName.Size = new Size(118, 24);
        lblName.TabIndex = 2;
        lblName.Text = "اسم الموظف";
        // 
        // lblNameReq
        // 
        lblNameReq.AutoSize = true;
        lblNameReq.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblNameReq.ForeColor = Color.Red;
        lblNameReq.Location = new Point(949, 15);
        lblNameReq.Name = "lblNameReq";
        lblNameReq.Size = new Size(23, 24);
        lblNameReq.TabIndex = 3;
        lblNameReq.Text = "*";
        // 
        // txtName
        // 
        txtName.Location = new Point(845, 42);
        txtName.Name = "txtName";
        txtName.Size = new Size(340, 32);
        txtName.TabIndex = 1;
        // 
        // lblJobTitle
        // 
        lblJobTitle.AutoSize = true;
        lblJobTitle.Location = new Point(655, 15);
        lblJobTitle.Name = "lblJobTitle";
        lblJobTitle.Size = new Size(162, 24);
        lblJobTitle.TabIndex = 4;
        lblJobTitle.Text = "المسمى الوظيفي";
        // 
        // txtJobTitle
        // 
        txtJobTitle.Location = new Point(505, 42);
        txtJobTitle.Name = "txtJobTitle";
        txtJobTitle.Size = new Size(280, 32);
        txtJobTitle.TabIndex = 2;
        // 
        // lblNationalId
        // 
        lblNationalId.AutoSize = true;
        lblNationalId.Location = new Point(1267, 88);
        lblNationalId.Name = "lblNationalId";
        lblNationalId.Size = new Size(117, 24);
        lblNationalId.TabIndex = 6;
        lblNationalId.Text = "الرقم الوطني";
        // 
        // txtNationalId
        // 
        txtNationalId.Location = new Point(1147, 115);
        txtNationalId.Name = "txtNationalId";
        txtNationalId.Size = new Size(230, 32);
        txtNationalId.TabIndex = 3;
        // 
        // lblPhone
        // 
        lblPhone.AutoSize = true;
        lblPhone.Location = new Point(967, 88);
        lblPhone.Name = "lblPhone";
        lblPhone.Size = new Size(62, 24);
        lblPhone.TabIndex = 8;
        lblPhone.Text = "الهاتف";
        // 
        // txtPhone
        // 
        txtPhone.Location = new Point(847, 115);
        txtPhone.Name = "txtPhone";
        txtPhone.Size = new Size(240, 32);
        txtPhone.TabIndex = 4;
        // 
        // lblBasicSalary
        // 
        lblBasicSalary.AutoSize = true;
        lblBasicSalary.Location = new Point(655, 88);
        lblBasicSalary.Name = "lblBasicSalary";
        lblBasicSalary.Size = new Size(148, 24);
        lblBasicSalary.TabIndex = 10;
        lblBasicSalary.Text = "الراتب الأساسي";
        // 
        // numBasicSalary
        // 
        numBasicSalary.DecimalPlaces = 2;
        numBasicSalary.Location = new Point(505, 115);
        numBasicSalary.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 });
        numBasicSalary.Name = "numBasicSalary";
        numBasicSalary.Size = new Size(220, 32);
        numBasicSalary.TabIndex = 5;
        numBasicSalary.ThousandsSeparator = true;
        // 
        // lblHireDate
        // 
        lblHireDate.AutoSize = true;
        lblHireDate.Location = new Point(355, 88);
        lblHireDate.Name = "lblHireDate";
        lblHireDate.Size = new Size(112, 24);
        lblHireDate.TabIndex = 12;
        lblHireDate.Text = "تاريخ التعيين";
        // 
        // dtpHireDate
        // 
        dtpHireDate.Format = DateTimePickerFormat.Short;
        dtpHireDate.Location = new Point(175, 115);
        dtpHireDate.Name = "dtpHireDate";
        dtpHireDate.ShowCheckBox = true;
        dtpHireDate.Size = new Size(230, 32);
        dtpHireDate.TabIndex = 6;
        // 
        // pnlButtons
        // 
        pnlButtons.BackColor = Color.FromArgb(235, 240, 235);
        pnlButtons.Controls.Add(btnNew);
        pnlButtons.Controls.Add(btnSave);
        pnlButtons.Controls.Add(btnEdit);
        pnlButtons.Controls.Add(btnDelete);
        pnlButtons.Controls.Add(btnRefresh);
        pnlButtons.Controls.Add(btnClose);
        pnlButtons.Dock = DockStyle.Top;
        pnlButtons.Location = new Point(0, 200);
        pnlButtons.Name = "pnlButtons";
        pnlButtons.Padding = new Padding(10);
        pnlButtons.Size = new Size(1432, 60);
        pnlButtons.TabIndex = 2;
        // 
        // btnNew
        // 
        btnNew.BackColor = Color.FromArgb(40, 167, 69);
        btnNew.FlatStyle = FlatStyle.Flat;
        btnNew.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnNew.ForeColor = Color.White;
        btnNew.Location = new Point(1267, 13);
        btnNew.Name = "btnNew";
        btnNew.Size = new Size(142, 35);
        btnNew.TabIndex = 16;
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
        btnSave.Location = new Point(1133, 13);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(128, 35);
        btnSave.TabIndex = 17;
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
        btnEdit.Location = new Point(999, 13);
        btnEdit.Name = "btnEdit";
        btnEdit.Size = new Size(128, 35);
        btnEdit.TabIndex = 19;
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
        btnDelete.Location = new Point(855, 13);
        btnDelete.Name = "btnDelete";
        btnDelete.Size = new Size(138, 35);
        btnDelete.TabIndex = 18;
        btnDelete.Text = "🗑 حذف";
        btnDelete.UseVisualStyleBackColor = false;
        btnDelete.Click += btnDelete_Click;
        // 
        // btnRefresh
        // 
        btnRefresh.BackColor = Color.FromArgb(23, 162, 184);
        btnRefresh.FlatStyle = FlatStyle.Flat;
        btnRefresh.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnRefresh.ForeColor = Color.White;
        btnRefresh.Location = new Point(719, 13);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(130, 35);
        btnRefresh.TabIndex = 21;
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
        btnClose.Location = new Point(583, 13);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(130, 35);
        btnClose.TabIndex = 22;
        btnClose.TabStop = false;
        btnClose.Text = "🚪 خروج";
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += btnClose_Click;
        // 
        // pnlFilters
        // 
        pnlFilters.Controls.Add(gbFilter);
        pnlFilters.Dock = DockStyle.Top;
        pnlFilters.Location = new Point(0, 260);
        pnlFilters.Margin = new Padding(0);
        pnlFilters.Name = "pnlFilters";
        pnlFilters.Padding = new Padding(6, 4, 6, 4);
        pnlFilters.Size = new Size(1432, 112);
        pnlFilters.TabIndex = 3;
        // 
        // gbFilter
        // 
        gbFilter.Controls.Add(label2);
        gbFilter.Controls.Add(cbFilterBy);
        gbFilter.Controls.Add(txtFilterValue);
        gbFilter.Controls.Add(label1);
        gbFilter.Controls.Add(cbStatus);
        gbFilter.Controls.Add(chkShowInactive);
        gbFilter.Controls.Add(chkHasSalary);
        gbFilter.Dock = DockStyle.Fill;
        gbFilter.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        gbFilter.Location = new Point(6, 4);
        gbFilter.Margin = new Padding(0);
        gbFilter.Name = "gbFilter";
        gbFilter.Padding = new Padding(8);
        gbFilter.Size = new Size(1420, 104);
        gbFilter.TabIndex = 0;
        gbFilter.TabStop = false;
        gbFilter.Text = "بحث وتصفية";
        // 
        // label2
        // 
        label2.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        label2.ForeColor = global::SalesSystem.Desktop.Helpers.GreenThemeHelper.TextStrong;
        label2.Location = new Point(1240, 34);
        label2.Margin = new Padding(0);
        label2.Name = "label2";
        label2.Size = new Size(100, 26);
        label2.TabIndex = 0;
        label2.Text = "تصفية بـ:";
        label2.TextAlign = ContentAlignment.MiddleRight;
        // 
        // cbFilterBy
        // 
        cbFilterBy.DropDownStyle = ComboBoxStyle.DropDownList;
        cbFilterBy.FlatStyle = FlatStyle.Flat;
        cbFilterBy.Font = new Font("Tahoma", 10F, FontStyle.Regular);
        cbFilterBy.FormattingEnabled = true;
        cbFilterBy.Items.AddRange(new object[] { "الاسم", "الهاتف", "الوظيفة", "رقم الهوية" });
        cbFilterBy.Location = new Point(1050, 30);
        cbFilterBy.Margin = new Padding(0);
        cbFilterBy.Name = "cbFilterBy";
        cbFilterBy.Size = new Size(184, 32);
        cbFilterBy.TabIndex = 1;
        cbFilterBy.SelectedIndex = 0;
        cbFilterBy.SelectedIndexChanged += cbFilterBy_SelectedIndexChanged;
        // 
        // txtFilterValue
        // 
        txtFilterValue.Font = new Font("Tahoma", 10F, FontStyle.Regular);
        txtFilterValue.Location = new Point(840, 30);
        txtFilterValue.Margin = new Padding(0);
        txtFilterValue.Name = "txtFilterValue";
        txtFilterValue.Size = new Size(200, 32);
        txtFilterValue.TabIndex = 2;
        txtFilterValue.TextChanged += txtSearch_TextChanged;
        txtFilterValue.KeyPress += txtSearch_KeyPress;
        txtFilterValue.KeyDown += txtFilterValue_KeyDown;
        // 
        // label1
        // 
        label1.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        label1.ForeColor = global::SalesSystem.Desktop.Helpers.GreenThemeHelper.TextStrong;
        label1.Location = new Point(760, 34);
        label1.Margin = new Padding(0);
        label1.Name = "label1";
        label1.Size = new Size(72, 26);
        label1.TabIndex = 3;
        label1.Text = "الحالة:";
        label1.TextAlign = ContentAlignment.MiddleRight;
        // 
        // cbStatus
        // 
        cbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
        cbStatus.FlatStyle = FlatStyle.Flat;
        cbStatus.Font = new Font("Tahoma", 10F, FontStyle.Regular);
        cbStatus.FormattingEnabled = true;
        cbStatus.Items.AddRange(new object[] { "الكل", "نشط", "معطّل" });
        cbStatus.Location = new Point(630, 30);
        cbStatus.Margin = new Padding(0);
        cbStatus.Name = "cbStatus";
        cbStatus.Size = new Size(124, 32);
        cbStatus.TabIndex = 4;
        cbStatus.SelectedIndex = 0;
        cbStatus.SelectedIndexChanged += cbStatus_SelectedIndexChanged;
        // 
        // chkShowInactive
        // 
        chkShowInactive.Font = new Font("Tahoma", 10F, FontStyle.Regular);
        chkShowInactive.ForeColor = global::SalesSystem.Desktop.Helpers.GreenThemeHelper.TextStrong;
        chkShowInactive.Location = new Point(1210, 68);
        chkShowInactive.Margin = new Padding(0);
        chkShowInactive.Name = "chkShowInactive";
        chkShowInactive.Size = new Size(190, 28);
        chkShowInactive.TabIndex = 5;
        chkShowInactive.Text = "إظهار غير النشط";
        chkShowInactive.UseVisualStyleBackColor = false;
        chkShowInactive.CheckedChanged += chkShowInactive_CheckedChanged;
        // 
        // chkHasSalary
        // 
        chkHasSalary.Font = new Font("Tahoma", 10F, FontStyle.Regular);
        chkHasSalary.ForeColor = global::SalesSystem.Desktop.Helpers.GreenThemeHelper.TextStrong;
        chkHasSalary.Location = new Point(1010, 68);
        chkHasSalary.Margin = new Padding(0);
        chkHasSalary.Name = "chkHasSalary";
        chkHasSalary.Size = new Size(190, 28);
        chkHasSalary.TabIndex = 6;
        chkHasSalary.Text = "له راتب محدد";
        chkHasSalary.UseVisualStyleBackColor = false;
        chkHasSalary.CheckedChanged += chkHasSalary_CheckedChanged;
        // 
        // pnlGrid
        // 
        pnlGrid.Controls.Add(dgvEmployees);
        pnlGrid.Dock = DockStyle.Fill;
        pnlGrid.Location = new Point(0, 372);
        pnlGrid.Name = "pnlGrid";
        pnlGrid.Padding = new Padding(0, 10, 0, 0);
        pnlGrid.Size = new Size(1432, 342);
        pnlGrid.TabIndex = 4;
        // 
        // dgvEmployees
        // 
        dgvEmployees.AllowUserToAddRows = false;
        dgvEmployees.AllowUserToDeleteRows = false;
        dgvEmployees.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvEmployees.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvEmployees.Dock = DockStyle.Fill;
        dgvEmployees.Location = new Point(0, 10);
        dgvEmployees.MultiSelect = false;
        dgvEmployees.Name = "dgvEmployees";
        dgvEmployees.ReadOnly = true;
        dgvEmployees.RowHeadersVisible = false;
        dgvEmployees.RowHeadersWidth = 62;
        dgvEmployees.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvEmployees.Size = new Size(1432, 332);
        dgvEmployees.TabIndex = 20;
        dgvEmployees.CellClick += dgvEmployees_CellClick;
        // 
        // pnlSummary
        // 
        pnlSummary.Controls.Add(lblSummary);
        pnlSummary.Dock = DockStyle.Bottom;
        pnlSummary.Location = new Point(0, 714);
        pnlSummary.Margin = new Padding(0);
        pnlSummary.Name = "pnlSummary";
        pnlSummary.Size = new Size(1432, 32);
        pnlSummary.TabIndex = 5;
        // 
        // lblSummary
        // 
        lblSummary.Dock = DockStyle.Fill;
        lblSummary.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblSummary.ForeColor = global::SalesSystem.Desktop.Helpers.GreenThemeHelper.TextStrong;
        lblSummary.Location = new Point(0, 0);
        lblSummary.Name = "lblSummary";
        lblSummary.Padding = new Padding(8, 0, 16, 0);
        lblSummary.Size = new Size(1432, 32);
        lblSummary.TabIndex = 0;
        lblSummary.Text = "المعروض: 0 من 0 — نشط: 0 · معطّل: 0";
        lblSummary.TextAlign = ContentAlignment.MiddleRight;
        // 
        // errorProvider
        // 
        errorProvider.ContainerControl = this;
        errorProvider.RightToLeft = true;
        // 
        // frmEmployeeMaster
        // 
        AutoScaleDimensions = new SizeF(11F, 24F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(170, 220, 170);
        ClientSize = new Size(1432, 746);
        Controls.Add(pnlGrid);
        Controls.Add(pnlSummary);
        Controls.Add(pnlFilters);
        Controls.Add(pnlButtons);
        Controls.Add(pnlFields);
        Controls.Add(lblMode);
        Font = new Font("Tahoma", 10F);
        Name = "frmEmployeeMaster";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterParent;
        Text = "دليل الموظفين";
        Load += frmEmployeeMaster_Load;
        pnlFields.ResumeLayout(false);
        pnlFields.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numBasicSalary).EndInit();
        pnlButtons.ResumeLayout(false);
        pnlFilters.ResumeLayout(false);
        gbFilter.ResumeLayout(false);
        gbFilter.PerformLayout();
        pnlGrid.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvEmployees).EndInit();
        pnlSummary.ResumeLayout(false);
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
    private System.Windows.Forms.Label lblJobTitle;
    private System.Windows.Forms.TextBox txtJobTitle;
    private System.Windows.Forms.Label lblNationalId;
    private System.Windows.Forms.TextBox txtNationalId;
    private System.Windows.Forms.Label lblPhone;
    private System.Windows.Forms.TextBox txtPhone;
    private System.Windows.Forms.Label lblBasicSalary;
    private System.Windows.Forms.NumericUpDown numBasicSalary;
    private System.Windows.Forms.Label lblHireDate;
    private System.Windows.Forms.DateTimePicker dtpHireDate;
    private System.Windows.Forms.FlowLayoutPanel pnlButtons;
    private System.Windows.Forms.Button btnNew;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.Button btnEdit;
    private System.Windows.Forms.Button btnDelete;
    private System.Windows.Forms.Button btnRefresh;
    private System.Windows.Forms.Button btnClose;
    private System.Windows.Forms.Panel pnlFilters;
    private System.Windows.Forms.GroupBox gbFilter;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.ComboBox cbFilterBy;
    private System.Windows.Forms.TextBox txtFilterValue;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ComboBox cbStatus;
    private System.Windows.Forms.CheckBox chkShowInactive;
    private System.Windows.Forms.CheckBox chkHasSalary;
    private System.Windows.Forms.Panel pnlGrid;
    private System.Windows.Forms.DataGridView dgvEmployees;
    private System.Windows.Forms.Panel pnlSummary;
    private System.Windows.Forms.Label lblSummary;
    private System.Windows.Forms.ErrorProvider errorProvider;
    private System.Windows.Forms.ToolTip tafqeetTip;
}
