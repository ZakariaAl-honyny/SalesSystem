namespace SalesSystem.Desktop.Screens.Roles.Forms;

partial class frmRoleMaster
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
        lblDescription = new Label();
        txtDescription = new TextBox();
        grpPermissions = new GroupBox();
        lblSalesPermissions = new Label();
        numSalesPermissions = new NumericUpDown();
        lblInventoryPermissions = new Label();
        numInventoryPermissions = new NumericUpDown();
        lblAccountingPermissions = new Label();
        numAccountingPermissions = new NumericUpDown();
        lblSystemPermissions = new Label();
        numSystemPermissions = new NumericUpDown();
        chkIsActive = new CheckBox();
        chkIsSystem = new CheckBox();
        pnlButtons = new FlowLayoutPanel();
        btnNew = new Button();
        btnSave = new Button();
        btnDelete = new Button();
        pnlGrid = new Panel();
        dgvRoles = new DataGridView();
        colId = new DataGridViewTextBoxColumn();
        colName = new DataGridViewTextBoxColumn();
        colIsActive = new DataGridViewTextBoxColumn();
        pnlFilter = new Panel();
        txtSearch = new TextBox();
        lblSearch = new Label();
        btnRefresh = new Button();
        errorProvider = new ErrorProvider(components);
        pnlFields.SuspendLayout();
        grpPermissions.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numSalesPermissions).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numInventoryPermissions).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numAccountingPermissions).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numSystemPermissions).BeginInit();
        pnlButtons.SuspendLayout();
        pnlGrid.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvRoles).BeginInit();
        pnlFilter.SuspendLayout();
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
        lblMode.Text = "(( دليل الأدوار ))";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlFields
        // 
        pnlFields.Controls.Add(lblCode);
        pnlFields.Controls.Add(txtCode);
        pnlFields.Controls.Add(lblName);
        pnlFields.Controls.Add(lblNameReq);
        pnlFields.Controls.Add(txtName);
        pnlFields.Controls.Add(lblDescription);
        pnlFields.Controls.Add(txtDescription);
        pnlFields.Controls.Add(grpPermissions);
        pnlFields.Controls.Add(chkIsActive);
        pnlFields.Controls.Add(chkIsSystem);
        pnlFields.Dock = DockStyle.Top;
        pnlFields.Location = new Point(0, 50);
        pnlFields.Name = "pnlFields";
        pnlFields.Size = new Size(1432, 200);
        pnlFields.TabIndex = 1;
        // 
        // lblCode
        // 
        lblCode.AutoSize = true;
        lblCode.Location = new Point(1267, 15);
        lblCode.Name = "lblCode";
        lblCode.Size = new Size(86, 24);
        lblCode.TabIndex = 0;
        lblCode.Text = "رقم الدور";
        // 
        // txtCode
        // 
        txtCode.Location = new Point(1267, 42);
        txtCode.Name = "txtCode";
        txtCode.ReadOnly = true;
        txtCode.Size = new Size(110, 32);
        txtCode.TabIndex = 0;
        // 
        // lblName
        // 
        lblName.AutoSize = true;
        lblName.Location = new Point(967, 15);
        lblName.Name = "lblName";
        lblName.Size = new Size(83, 24);
        lblName.TabIndex = 2;
        lblName.Text = "اسم الدور";
        // 
        // lblNameReq
        // 
        lblNameReq.AutoSize = true;
        lblNameReq.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblNameReq.ForeColor = Color.Red;
        lblNameReq.Location = new Point(949, 15);
        lblNameReq.Name = "lblNameReq";
        lblNameReq.Text = "*";
        // 
        // txtName
        // 
        txtName.Location = new Point(885, 42);
        txtName.Name = "txtName";
        txtName.Size = new Size(356, 32);
        txtName.TabIndex = 1;
        // 
        // lblDescription
        // 
        lblDescription.AutoSize = true;
        lblDescription.Location = new Point(1267, 88);
        lblDescription.Name = "lblDescription";
        lblDescription.Size = new Size(66, 24);
        lblDescription.TabIndex = 4;
        lblDescription.Text = "الوصف";
        // 
        // txtDescription
        // 
        txtDescription.Location = new Point(675, 88);
        txtDescription.Multiline = true;
        txtDescription.Name = "txtDescription";
        txtDescription.Size = new Size(566, 56);
        txtDescription.TabIndex = 2;
        // 
        // grpPermissions
        // 
        grpPermissions.BackColor = Color.FromArgb(200, 230, 200);
        grpPermissions.Controls.Add(lblSalesPermissions);
        grpPermissions.Controls.Add(numSalesPermissions);
        grpPermissions.Controls.Add(lblInventoryPermissions);
        grpPermissions.Controls.Add(numInventoryPermissions);
        grpPermissions.Controls.Add(lblAccountingPermissions);
        grpPermissions.Controls.Add(numAccountingPermissions);
        grpPermissions.Controls.Add(lblSystemPermissions);
        grpPermissions.Controls.Add(numSystemPermissions);
        grpPermissions.Location = new Point(199, 3);
        grpPermissions.Name = "grpPermissions";
        grpPermissions.Size = new Size(470, 170);
        grpPermissions.TabIndex = 3;
        grpPermissions.TabStop = false;
        grpPermissions.Text = "صلاحيات الدور (أقنعة رقمية)";
        // 
        // lblSalesPermissions
        // 
        lblSalesPermissions.AutoSize = true;
        lblSalesPermissions.Location = new Point(248, 27);
        lblSalesPermissions.Name = "lblSalesPermissions";
        lblSalesPermissions.Size = new Size(149, 24);
        lblSalesPermissions.TabIndex = 0;
        lblSalesPermissions.Text = "صلاحيات المبيعات";
        // 
        // numSalesPermissions
        // 
        numSalesPermissions.Location = new Point(248, 54);
        numSalesPermissions.Maximum = 9223372036854775807m;
        numSalesPermissions.Minimum = -9223372036854775808m;
        numSalesPermissions.Name = "numSalesPermissions";
        numSalesPermissions.Size = new Size(190, 32);
        numSalesPermissions.TabIndex = 0;
        // 
        // lblInventoryPermissions
        // 
        lblInventoryPermissions.AutoSize = true;
        lblInventoryPermissions.Location = new Point(30, 27);
        lblInventoryPermissions.Name = "lblInventoryPermissions";
        lblInventoryPermissions.Size = new Size(146, 24);
        lblInventoryPermissions.TabIndex = 2;
        lblInventoryPermissions.Text = "صلاحيات المخازن";
        // 
        // numInventoryPermissions
        // 
        numInventoryPermissions.Location = new Point(30, 54);
        numInventoryPermissions.Maximum = 9223372036854775807m;
        numInventoryPermissions.Minimum = -9223372036854775808m;
        numInventoryPermissions.Name = "numInventoryPermissions";
        numInventoryPermissions.Size = new Size(190, 32);
        numInventoryPermissions.TabIndex = 1;
        // 
        // lblAccountingPermissions
        // 
        lblAccountingPermissions.AutoSize = true;
        lblAccountingPermissions.Location = new Point(248, 95);
        lblAccountingPermissions.Name = "lblAccountingPermissions";
        lblAccountingPermissions.Size = new Size(157, 24);
        lblAccountingPermissions.TabIndex = 4;
        lblAccountingPermissions.Text = "صلاحيات الحسابات";
        // 
        // numAccountingPermissions
        // 
        numAccountingPermissions.Location = new Point(248, 122);
        numAccountingPermissions.Maximum = 9223372036854775807m;
        numAccountingPermissions.Minimum = -9223372036854775808m;
        numAccountingPermissions.Name = "numAccountingPermissions";
        numAccountingPermissions.Size = new Size(190, 32);
        numAccountingPermissions.TabIndex = 2;
        // 
        // lblSystemPermissions
        // 
        lblSystemPermissions.AutoSize = true;
        lblSystemPermissions.Location = new Point(30, 95);
        lblSystemPermissions.Name = "lblSystemPermissions";
        lblSystemPermissions.Size = new Size(133, 24);
        lblSystemPermissions.TabIndex = 6;
        lblSystemPermissions.Text = "صلاحيات النظام";
        // 
        // numSystemPermissions
        // 
        numSystemPermissions.Location = new Point(30, 122);
        numSystemPermissions.Maximum = 9223372036854775807m;
        numSystemPermissions.Minimum = -9223372036854775808m;
        numSystemPermissions.Name = "numSystemPermissions";
        numSystemPermissions.Size = new Size(190, 32);
        numSystemPermissions.TabIndex = 3;
        // 
        // chkIsActive
        // 
        chkIsActive.AutoSize = true;
        chkIsActive.Location = new Point(1188, 158);
        chkIsActive.Name = "chkIsActive";
        chkIsActive.Size = new Size(70, 28);
        chkIsActive.TabIndex = 4;
        chkIsActive.Text = "نشط";
        // 
        // chkIsSystem
        // 
        chkIsSystem.AutoSize = true;
        chkIsSystem.Enabled = false;
        chkIsSystem.Location = new Point(1000, 158);
        chkIsSystem.Name = "chkIsSystem";
        chkIsSystem.Size = new Size(160, 28);
        chkIsSystem.TabIndex = 5;
        chkIsSystem.Text = "دور نظام (للقراءة)";
        // 
        // pnlButtons
        // 
        pnlButtons.BackColor = Color.FromArgb(235, 240, 235);
        pnlButtons.Controls.Add(btnNew);
        pnlButtons.Controls.Add(btnSave);
        pnlButtons.Controls.Add(btnDelete);
        pnlButtons.Dock = DockStyle.Top;
        pnlButtons.Location = new Point(0, 250);
        pnlButtons.Name = "pnlButtons";
        pnlButtons.Padding = new Padding(10);
        pnlButtons.Size = new Size(1432, 60);
        pnlButtons.TabIndex = 2;
        // 
        // btnNew
        // 
        btnNew.BackColor = Color.White;
        btnNew.FlatStyle = FlatStyle.Flat;
        btnNew.Location = new Point(1267, 13);
        btnNew.Name = "btnNew";
        btnNew.Size = new Size(142, 35);
        btnNew.TabIndex = 0;
        btnNew.Text = "🆕 إضافة";
        btnNew.UseVisualStyleBackColor = false;
        // 
        // btnSave
        // 
        btnSave.BackColor = Color.White;
        btnSave.FlatStyle = FlatStyle.Flat;
        btnSave.Location = new Point(1133, 13);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(128, 35);
        btnSave.TabIndex = 1;
        btnSave.Text = "💾 حفظ";
        btnSave.UseVisualStyleBackColor = false;
        // 
        // btnDelete
        // 
        btnDelete.BackColor = Color.White;
        btnDelete.FlatStyle = FlatStyle.Flat;
        btnDelete.Location = new Point(989, 13);
        btnDelete.Name = "btnDelete";
        btnDelete.Size = new Size(138, 35);
        btnDelete.TabIndex = 2;
        btnDelete.Text = "🗑 حذف";
        btnDelete.UseVisualStyleBackColor = false;
        // 
        // pnlGrid
        // 
        pnlGrid.Controls.Add(dgvRoles);
        pnlGrid.Controls.Add(pnlFilter);
        pnlGrid.Dock = DockStyle.Fill;
        pnlGrid.Location = new Point(0, 310);
        pnlGrid.Name = "pnlGrid";
        pnlGrid.Padding = new Padding(10);
        pnlGrid.Size = new Size(1432, 360);
        pnlGrid.TabIndex = 3;
        // 
        // dgvRoles
        // 
        dgvRoles.AllowUserToAddRows = false;
        dgvRoles.AllowUserToDeleteRows = false;
        dgvRoles.AutoGenerateColumns = false;
        dgvRoles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvRoles.BackgroundColor = Color.FromArgb(220, 240, 220);
        dgvRoles.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvRoles.Columns.AddRange(new DataGridViewColumn[] { colId, colName, colIsActive });
        dgvRoles.Dock = DockStyle.Fill;
        dgvRoles.Location = new Point(10, 50);
        dgvRoles.MultiSelect = false;
        dgvRoles.Name = "dgvRoles";
        dgvRoles.ReadOnly = true;
        dgvRoles.RowHeadersVisible = false;
        dgvRoles.RowHeadersWidth = 62;
        dgvRoles.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvRoles.Size = new Size(1412, 300);
        dgvRoles.TabIndex = 0;
        // 
        // colId
        // 
        colId.DataPropertyName = "Id";
        colId.FillWeight = 12F;
        colId.HeaderText = "رقم الدور";
        colId.MinimumWidth = 90;
        colId.Name = "colId";
        colId.ReadOnly = true;
        // 
        // colName
        // 
        colName.DataPropertyName = "Name";
        colName.FillWeight = 70F;
        colName.HeaderText = "اسم الدور";
        colName.MinimumWidth = 220;
        colName.Name = "colName";
        colName.ReadOnly = true;
        // 
        // colIsActive
        // 
        colIsActive.DataPropertyName = "IsActive";
        colIsActive.FillWeight = 18F;
        colIsActive.HeaderText = "نشط";
        colIsActive.MinimumWidth = 70;
        colIsActive.Name = "colIsActive";
        colIsActive.ReadOnly = true;
        // 
        // pnlFilter
        // 
        pnlFilter.Controls.Add(btnRefresh);
        pnlFilter.Controls.Add(txtSearch);
        pnlFilter.Controls.Add(lblSearch);
        pnlFilter.Dock = DockStyle.Top;
        pnlFilter.Location = new Point(10, 10);
        pnlFilter.Name = "pnlFilter";
        pnlFilter.Size = new Size(1412, 40);
        pnlFilter.TabIndex = 1;
        // 
        // txtSearch
        // 
        txtSearch.Location = new Point(690, 7);
        txtSearch.Name = "txtSearch";
        txtSearch.Size = new Size(300, 32);
        txtSearch.TabIndex = 99;
        // 
        // btnRefresh
        // 
        btnRefresh.BackColor = Color.White;
        btnRefresh.FlatStyle = FlatStyle.Flat;
        btnRefresh.Location = new Point(550, 4);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(130, 32);
        btnRefresh.TabIndex = 98;
        btnRefresh.Text = "🔄 تحديث";
        btnRefresh.UseVisualStyleBackColor = false;
        // 
        // lblSearch
        // 
        lblSearch.AutoSize = true;
        lblSearch.Location = new Point(1000, 10);
        lblSearch.Name = "lblSearch";
        lblSearch.Size = new Size(56, 24);
        lblSearch.TabIndex = 1;
        lblSearch.Text = "بحث:";
        // 
        // errorProvider
        // 
        errorProvider.ContainerControl = this;
        errorProvider.RightToLeft = true;
        // 
        // frmRoleMaster
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
        Name = "frmRoleMaster";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterParent;
        Text = "دليل الأدوار";
        pnlFields.ResumeLayout(false);
        pnlFields.PerformLayout();
        grpPermissions.ResumeLayout(false);
        grpPermissions.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numSalesPermissions).EndInit();
        ((System.ComponentModel.ISupportInitialize)numInventoryPermissions).EndInit();
        ((System.ComponentModel.ISupportInitialize)numAccountingPermissions).EndInit();
        ((System.ComponentModel.ISupportInitialize)numSystemPermissions).EndInit();
        pnlButtons.ResumeLayout(false);
        pnlGrid.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvRoles).EndInit();
        pnlFilter.ResumeLayout(false);
        pnlFilter.PerformLayout();
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
    private System.Windows.Forms.GroupBox grpPermissions;
    private System.Windows.Forms.Label lblSalesPermissions;
    private System.Windows.Forms.NumericUpDown numSalesPermissions;
    private System.Windows.Forms.Label lblInventoryPermissions;
    private System.Windows.Forms.NumericUpDown numInventoryPermissions;
    private System.Windows.Forms.Label lblAccountingPermissions;
    private System.Windows.Forms.NumericUpDown numAccountingPermissions;
    private System.Windows.Forms.Label lblSystemPermissions;
    private System.Windows.Forms.NumericUpDown numSystemPermissions;
    private System.Windows.Forms.CheckBox chkIsActive;
    private System.Windows.Forms.CheckBox chkIsSystem;
    private System.Windows.Forms.FlowLayoutPanel pnlButtons;
    private System.Windows.Forms.Button btnNew;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.Button btnDelete;
    private System.Windows.Forms.Panel pnlGrid;
    private System.Windows.Forms.DataGridView dgvRoles;
    private System.Windows.Forms.DataGridViewTextBoxColumn colId;
    private System.Windows.Forms.DataGridViewTextBoxColumn colName;
    private System.Windows.Forms.DataGridViewTextBoxColumn colIsActive;
    private System.Windows.Forms.Panel pnlFilter;
    private System.Windows.Forms.TextBox txtSearch;
    private System.Windows.Forms.Button btnRefresh;
    private System.Windows.Forms.Label lblSearch;
    private System.Windows.Forms.ErrorProvider errorProvider;
}
