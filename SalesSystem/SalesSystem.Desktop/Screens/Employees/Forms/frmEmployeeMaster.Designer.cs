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
        pnlGrid = new Panel();
        dgvEmployees = new DataGridView();
        pnlFilter = new Panel();
        groupBox1 = new GroupBox();
        lblSearch = new Label();
        chkShowInactive = new CheckBox();
        txtSearch = new TextBox();
        errorProvider = new ErrorProvider(components);
        pnlFields.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numBasicSalary).BeginInit();
        pnlButtons.SuspendLayout();
        pnlGrid.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvEmployees).BeginInit();
        pnlFilter.SuspendLayout();
        groupBox1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
        SuspendLayout();
        // 
        // lblMode
        // 
        lblMode.BackColor = Color.FromArgb(14, 165, 103);
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
        // pnlGrid
        // 
        pnlGrid.Controls.Add(dgvEmployees);
        pnlGrid.Controls.Add(pnlFilter);
        pnlGrid.Dock = DockStyle.Fill;
        pnlGrid.Location = new Point(0, 260);
        pnlGrid.Name = "pnlGrid";
        pnlGrid.Padding = new Padding(0, 10, 0, 0);
        pnlGrid.Size = new Size(1432, 410);
        pnlGrid.TabIndex = 3;
        // 
        // dgvEmployees
        // 
        dgvEmployees.AllowUserToAddRows = false;
        dgvEmployees.AllowUserToDeleteRows = false;
        dgvEmployees.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvEmployees.BackgroundColor = Color.FromArgb(220, 240, 220);
        dgvEmployees.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvEmployees.Dock = DockStyle.Fill;
        dgvEmployees.Location = new Point(0, 78);
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
        // pnlFilter
        // 
        pnlFilter.Controls.Add(groupBox1);
        pnlFilter.Dock = DockStyle.Top;
        pnlFilter.Location = new Point(0, 10);
        pnlFilter.Name = "pnlFilter";
        pnlFilter.Size = new Size(1432, 68);
        pnlFilter.TabIndex = 21;
        // 
        // groupBox1
        // 
        groupBox1.Controls.Add(lblSearch);
        groupBox1.Controls.Add(chkShowInactive);
        groupBox1.Controls.Add(txtSearch);
        groupBox1.Font = new Font("Tahoma", 10F);
        groupBox1.Location = new Point(803, 3);
        groupBox1.Name = "groupBox1";
        groupBox1.Size = new Size(596, 62);
        groupBox1.TabIndex = 22;
        groupBox1.TabStop = false;
        groupBox1.Text = "محرك البحث";
        // 
        // lblSearch
        // 
        lblSearch.AutoSize = true;
        lblSearch.Location = new Point(501, 32);
        lblSearch.Name = "lblSearch";
        lblSearch.Size = new Size(56, 24);
        lblSearch.TabIndex = 1;
        lblSearch.Text = "بحث:";
        // 
        // chkShowInactive
        // 
        chkShowInactive.AutoSize = true;
        chkShowInactive.FlatStyle = FlatStyle.Flat;
        chkShowInactive.Location = new Point(13, 24);
        chkShowInactive.Name = "chkShowInactive";
        chkShowInactive.Size = new Size(166, 28);
        chkShowInactive.TabIndex = 21;
        chkShowInactive.Text = "عرض غير النشط";
        chkShowInactive.CheckedChanged += chkShowInactive_CheckedChanged;
        // 
        // txtSearch
        // 
        txtSearch.Location = new Point(195, 24);
        txtSearch.Name = "txtSearch";
        txtSearch.Size = new Size(300, 32);
        txtSearch.TabIndex = 20;
        txtSearch.TextChanged += txtSearch_TextChanged;
        txtSearch.KeyPress += txtSearch_KeyPress;
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
        ClientSize = new Size(1432, 670);
        Controls.Add(pnlGrid);
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
        pnlGrid.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvEmployees).EndInit();
        pnlFilter.ResumeLayout(false);
        groupBox1.ResumeLayout(false);
        groupBox1.PerformLayout();
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
    private System.Windows.Forms.Panel pnlGrid;
    private System.Windows.Forms.DataGridView dgvEmployees;
    private System.Windows.Forms.Panel pnlFilter;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.TextBox txtSearch;
    private System.Windows.Forms.CheckBox chkShowInactive;
    private System.Windows.Forms.Label lblSearch;
    private System.Windows.Forms.ErrorProvider errorProvider;
}
