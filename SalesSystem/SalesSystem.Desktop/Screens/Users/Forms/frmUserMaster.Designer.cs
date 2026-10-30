namespace SalesSystem.Desktop.Screens.Users.Forms;

partial class frmUserMaster
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
        lblUserName = new Label();
        lblUserNameReq = new Label();
        txtUserName = new TextBox();
        lblRole = new Label();
        lblRoleReq = new Label();
        cmbRole = new ComboBox();
        lblBranch = new Label();
        cmbBranch = new ComboBox();
        lblPassword = new Label();
        txtPassword = new TextBox();
        chkMustChangePassword = new CheckBox();
        chkIsLocked = new CheckBox();
        chkIsActive = new CheckBox();
        pnlButtons = new FlowLayoutPanel();
        btnNew = new Button();
        btnSave = new Button();
        btnDelete = new Button();
        pnlGrid = new Panel();
        dgvUsers = new DataGridView();
        colId = new DataGridViewTextBoxColumn();
        colUserName = new DataGridViewTextBoxColumn();
        colIsLocked = new DataGridViewTextBoxColumn();
        colIsActive = new DataGridViewTextBoxColumn();
        pnlFilter = new Panel();
        txtSearch = new TextBox();
        lblSearch = new Label();
        btnRefresh = new Button();
        errorProvider = new ErrorProvider(components);
        pnlFields.SuspendLayout();
        pnlButtons.SuspendLayout();
        pnlGrid.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvUsers).BeginInit();
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
        lblMode.Text = "(( إدارة المستخدمين ))";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlFields
        // 
        pnlFields.Controls.Add(lblCode);
        pnlFields.Controls.Add(txtCode);
        pnlFields.Controls.Add(lblUserName);
        pnlFields.Controls.Add(lblUserNameReq);
        pnlFields.Controls.Add(txtUserName);
        pnlFields.Controls.Add(lblRole);
        pnlFields.Controls.Add(lblRoleReq);
        pnlFields.Controls.Add(cmbRole);
        pnlFields.Controls.Add(lblBranch);
        pnlFields.Controls.Add(cmbBranch);
        pnlFields.Controls.Add(lblPassword);
        pnlFields.Controls.Add(txtPassword);
        pnlFields.Controls.Add(chkMustChangePassword);
        pnlFields.Controls.Add(chkIsLocked);
        pnlFields.Controls.Add(chkIsActive);
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
        lblCode.Size = new Size(110, 24);
        lblCode.TabIndex = 0;
        lblCode.Text = "رقم المستخدم";
        // 
        // txtCode
        // 
        txtCode.Location = new Point(1267, 42);
        txtCode.Name = "txtCode";
        txtCode.ReadOnly = true;
        txtCode.Size = new Size(110, 32);
        txtCode.TabIndex = 0;
        // 
        // lblUserName
        // 
        lblUserName.AutoSize = true;
        lblUserName.Location = new Point(1160, 15);
        lblUserName.Name = "lblUserName";
        lblUserName.Size = new Size(102, 24);
        lblUserName.TabIndex = 2;
        lblUserName.Text = "اسم المستخدم";
        // 
        // lblUserNameReq
        // 
        lblUserNameReq.AutoSize = true;
        lblUserNameReq.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblUserNameReq.ForeColor = Color.Red;
        lblUserNameReq.Location = new Point(1142, 15);
        lblUserNameReq.Name = "lblUserNameReq";
        lblUserNameReq.Text = "*";
        // 
        // txtUserName
        // 
        txtUserName.Location = new Point(960, 42);
        txtUserName.Name = "txtUserName";
        txtUserName.Size = new Size(300, 32);
        txtUserName.TabIndex = 1;
        // 
        // lblRole
        // 
        lblRole.AutoSize = true;
        lblRole.Location = new Point(860, 15);
        lblRole.Name = "lblRole";
        lblRole.Size = new Size(41, 24);
        lblRole.TabIndex = 4;
        lblRole.Text = "الدور";
        // 
        // lblRoleReq
        // 
        lblRoleReq.AutoSize = true;
        lblRoleReq.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblRoleReq.ForeColor = Color.Red;
        lblRoleReq.Location = new Point(842, 15);
        lblRoleReq.Name = "lblRoleReq";
        lblRoleReq.Text = "*";
        // 
        // cmbRole
        // 
        cmbRole.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbRole.Location = new Point(760, 42);
        cmbRole.Name = "cmbRole";
        cmbRole.Size = new Size(240, 32);
        cmbRole.TabIndex = 2;
        // 
        // lblBranch
        // 
        lblBranch.AutoSize = true;
        lblBranch.Location = new Point(1267, 95);
        lblBranch.Name = "lblBranch";
        lblBranch.Size = new Size(48, 24);
        lblBranch.TabIndex = 6;
        lblBranch.Text = "الفرع";
        // 
        // cmbBranch
        // 
        cmbBranch.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbBranch.Location = new Point(1157, 122);
        cmbBranch.Name = "cmbBranch";
        cmbBranch.Size = new Size(220, 32);
        cmbBranch.TabIndex = 3;
        // 
        // lblPassword
        // 
        lblPassword.AutoSize = true;
        lblPassword.Location = new Point(1070, 95);
        lblPassword.Name = "lblPassword";
        lblPassword.Size = new Size(137, 24);
        lblPassword.TabIndex = 8;
        lblPassword.Text = "كلمة المرور (اختياري)";
        // 
        // txtPassword
        // 
        txtPassword.Location = new Point(950, 122);
        txtPassword.Name = "txtPassword";
        txtPassword.PasswordChar = '●';
        txtPassword.Size = new Size(257, 32);
        txtPassword.TabIndex = 4;
        // 
        // chkMustChangePassword
        // 
        chkMustChangePassword.AutoSize = true;
        chkMustChangePassword.Enabled = false;
        chkMustChangePassword.Location = new Point(1070, 160);
        chkMustChangePassword.Name = "chkMustChangePassword";
        chkMustChangePassword.Size = new Size(260, 28);
        chkMustChangePassword.TabIndex = 7;
        chkMustChangePassword.Text = "يجب تغيير كلمة المرور (للقراءة)";
        // 
        // chkIsLocked
        // 
        chkIsLocked.AutoSize = true;
        chkIsLocked.Location = new Point(900, 160);
        chkIsLocked.Name = "chkIsLocked";
        chkIsLocked.Size = new Size(90, 28);
        chkIsLocked.TabIndex = 5;
        chkIsLocked.Text = "مقفل";
        // 
        // chkIsActive
        // 
        chkIsActive.AutoSize = true;
        chkIsActive.Location = new Point(780, 160);
        chkIsActive.Name = "chkIsActive";
        chkIsActive.Size = new Size(70, 28);
        chkIsActive.TabIndex = 6;
        chkIsActive.Text = "نشط";
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
        pnlGrid.Controls.Add(dgvUsers);
        pnlGrid.Controls.Add(pnlFilter);
        pnlGrid.Dock = DockStyle.Fill;
        pnlGrid.Location = new Point(0, 310);
        pnlGrid.Name = "pnlGrid";
        pnlGrid.Padding = new Padding(10);
        pnlGrid.Size = new Size(1432, 360);
        pnlGrid.TabIndex = 3;
        // 
        // dgvUsers
        // 
        dgvUsers.AllowUserToAddRows = false;
        dgvUsers.AllowUserToDeleteRows = false;
        dgvUsers.AutoGenerateColumns = false;
        dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvUsers.BackgroundColor = Color.FromArgb(220, 240, 220);
        dgvUsers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvUsers.Columns.AddRange(new DataGridViewColumn[] { colId, colUserName, colIsLocked, colIsActive });
        dgvUsers.Dock = DockStyle.Fill;
        dgvUsers.Location = new Point(10, 50);
        dgvUsers.MultiSelect = false;
        dgvUsers.Name = "dgvUsers";
        dgvUsers.ReadOnly = true;
        dgvUsers.RowHeadersVisible = false;
        dgvUsers.RowHeadersWidth = 62;
        dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvUsers.Size = new Size(1412, 300);
        dgvUsers.TabIndex = 0;
        // 
        // colId
        // 
        colId.DataPropertyName = "Id";
        colId.FillWeight = 12F;
        colId.HeaderText = "رقم المستخدم";
        colId.MinimumWidth = 90;
        colId.Name = "colId";
        colId.ReadOnly = true;
        // 
        // colUserName
        // 
        colUserName.DataPropertyName = "UserName";
        colUserName.FillWeight = 58F;
        colUserName.HeaderText = "اسم المستخدم";
        colUserName.MinimumWidth = 220;
        colUserName.Name = "colUserName";
        colUserName.ReadOnly = true;
        // 
        // colIsLocked
        // 
        colIsLocked.DataPropertyName = "IsLocked";
        colIsLocked.FillWeight = 15F;
        colIsLocked.HeaderText = "مقفل";
        colIsLocked.MinimumWidth = 80;
        colIsLocked.Name = "colIsLocked";
        colIsLocked.ReadOnly = true;
        // 
        // colIsActive
        // 
        colIsActive.DataPropertyName = "IsActive";
        colIsActive.FillWeight = 15F;
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
        // frmUserMaster
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
        Name = "frmUserMaster";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterParent;
        Text = "إدارة المستخدمين";
        pnlFields.ResumeLayout(false);
        pnlFields.PerformLayout();
        pnlButtons.ResumeLayout(false);
        pnlGrid.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvUsers).EndInit();
        pnlFilter.ResumeLayout(false);
        pnlFilter.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        ResumeLayout(false);
    }

    private System.Windows.Forms.Label lblMode;
    private System.Windows.Forms.Panel pnlFields;
    private System.Windows.Forms.Label lblCode;
    private System.Windows.Forms.TextBox txtCode;
    private System.Windows.Forms.Label lblUserName;
    private System.Windows.Forms.Label lblUserNameReq;
    private System.Windows.Forms.TextBox txtUserName;
    private System.Windows.Forms.Label lblRole;
    private System.Windows.Forms.Label lblRoleReq;
    private System.Windows.Forms.ComboBox cmbRole;
    private System.Windows.Forms.Label lblBranch;
    private System.Windows.Forms.ComboBox cmbBranch;
    private System.Windows.Forms.Label lblPassword;
    private System.Windows.Forms.TextBox txtPassword;
    private System.Windows.Forms.CheckBox chkMustChangePassword;
    private System.Windows.Forms.CheckBox chkIsLocked;
    private System.Windows.Forms.CheckBox chkIsActive;
    private System.Windows.Forms.FlowLayoutPanel pnlButtons;
    private System.Windows.Forms.Button btnNew;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.Button btnDelete;
    private System.Windows.Forms.Panel pnlGrid;
    private System.Windows.Forms.DataGridView dgvUsers;
    private System.Windows.Forms.DataGridViewTextBoxColumn colId;
    private System.Windows.Forms.DataGridViewTextBoxColumn colUserName;
    private System.Windows.Forms.DataGridViewTextBoxColumn colIsLocked;
    private System.Windows.Forms.DataGridViewTextBoxColumn colIsActive;
    private System.Windows.Forms.Panel pnlFilter;
    private System.Windows.Forms.TextBox txtSearch;
    private System.Windows.Forms.Button btnRefresh;
    private System.Windows.Forms.Label lblSearch;
    private System.Windows.Forms.ErrorProvider errorProvider;
}
