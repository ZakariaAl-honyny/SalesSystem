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
        btnEdit = new Button();
        btnDelete = new Button();
        btnRefresh = new Button();
        btnClose = new Button();
        pnlFilters = new Panel();
        gbFilter = new GroupBox();
        label1 = new Label();
        cbStatus = new ComboBox();
        label2 = new Label();
        cbFilterBy = new ComboBox();
        lblSearch = new Label();
        txtSearch = new TextBox();
        chkShowInactive = new CheckBox();
        chkOnlyLocked = new CheckBox();
        chkOnlyMustChangePassword = new CheckBox();
        pnlSummary = new Panel();
        lblSummary = new Label();
        pnlGrid = new Panel();
        dgvUsers = new DataGridView();
        colId = new DataGridViewTextBoxColumn();
        colUserName = new DataGridViewTextBoxColumn();
        colIsLocked = new DataGridViewTextBoxColumn();
        colIsActive = new DataGridViewTextBoxColumn();
        errorProvider = new ErrorProvider(components);
        pnlFields.SuspendLayout();
        pnlButtons.SuspendLayout();
        pnlFilters.SuspendLayout();
        gbFilter.SuspendLayout();
        pnlSummary.SuspendLayout();
        pnlGrid.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvUsers).BeginInit();
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
        txtCode.TabStop = false;
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
        cmbBranch.BackColor = Color.White;
        cmbBranch.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbBranch.FlatStyle = FlatStyle.Flat;
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
        pnlButtons.Controls.Add(btnEdit);
        pnlButtons.Controls.Add(btnDelete);
        pnlButtons.Controls.Add(btnRefresh);
        pnlButtons.Controls.Add(btnClose);
        pnlButtons.Dock = DockStyle.Top;
        pnlButtons.Location = new Point(0, 250);
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
        btnNew.TabIndex = 0;
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
        btnSave.TabIndex = 1;
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
        btnEdit.TabIndex = 2;
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
        btnDelete.TabIndex = 3;
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
        btnRefresh.TabIndex = 4;
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
        btnClose.TabIndex = 5;
        btnClose.TabStop = false;
        btnClose.Text = "🚪 خروج";
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += btnClose_Click;
        // 
        // pnlFilters
        // 
        pnlFilters.Controls.Add(gbFilter);
        pnlFilters.Dock = DockStyle.Top;
        pnlFilters.Location = new Point(0, 310);
        pnlFilters.Margin = new Padding(0);
        pnlFilters.Name = "pnlFilters";
        pnlFilters.Padding = new Padding(6, 4, 6, 4);
        pnlFilters.Size = new Size(1432, 112);
        pnlFilters.TabIndex = 3;
        // 
        // gbFilter
        // 
        gbFilter.Controls.Add(label1);
        gbFilter.Controls.Add(cbStatus);
        gbFilter.Controls.Add(label2);
        gbFilter.Controls.Add(cbFilterBy);
        gbFilter.Controls.Add(lblSearch);
        gbFilter.Controls.Add(txtSearch);
        gbFilter.Controls.Add(chkShowInactive);
        gbFilter.Controls.Add(chkOnlyLocked);
        gbFilter.Controls.Add(chkOnlyMustChangePassword);
        gbFilter.Dock = DockStyle.Fill;
        gbFilter.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        gbFilter.Location = new Point(6, 4);
        gbFilter.Margin = new Padding(0);
        gbFilter.Name = "gbFilter";
        gbFilter.Padding = new Padding(8);
        gbFilter.RightToLeft = RightToLeft.Yes;
        gbFilter.Size = new Size(1420, 104);
        gbFilter.TabIndex = 0;
        gbFilter.TabStop = false;
        gbFilter.Text = "بحث وتصفية";
        // 
        // label1
        // 
        label1.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        label1.Location = new Point(1290, 20);
        label1.Margin = new Padding(0);
        label1.Name = "label1";
        label1.Size = new Size(90, 24);
        label1.TabIndex = 0;
        label1.Text = "الحالة:";
        label1.TextAlign = ContentAlignment.MiddleRight;
        // 
        // cbStatus
        // 
        cbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
        cbStatus.FlatStyle = FlatStyle.Flat;
        cbStatus.Font = new Font("Tahoma", 9F);
        cbStatus.FormattingEnabled = true;
        cbStatus.Items.AddRange(new object[] { "الكل", "نشط", "معطّل" });
        cbStatus.Location = new Point(1152, 18);
        cbStatus.Margin = new Padding(0);
        cbStatus.Name = "cbStatus";
        cbStatus.Size = new Size(130, 26);
        cbStatus.TabIndex = 1;
        cbStatus.SelectedIndexChanged += cbStatus_SelectedIndexChanged;
        // 
        // label2
        // 
        label2.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        label2.Location = new Point(1032, 20);
        label2.Margin = new Padding(0);
        label2.Name = "label2";
        label2.Size = new Size(110, 24);
        label2.TabIndex = 2;
        label2.Text = "تصفية بـ:";
        label2.TextAlign = ContentAlignment.MiddleRight;
        // 
        // cbFilterBy
        // 
        cbFilterBy.DropDownStyle = ComboBoxStyle.DropDownList;
        cbFilterBy.FlatStyle = FlatStyle.Flat;
        cbFilterBy.Font = new Font("Tahoma", 9F);
        cbFilterBy.FormattingEnabled = true;
        cbFilterBy.Items.AddRange(new object[] { "اسم المستخدم", "رقم المستخدم" });
        cbFilterBy.Location = new Point(884, 18);
        cbFilterBy.Margin = new Padding(0);
        cbFilterBy.Name = "cbFilterBy";
        cbFilterBy.Size = new Size(146, 26);
        cbFilterBy.TabIndex = 3;
        cbFilterBy.SelectedIndexChanged += cbFilterBy_SelectedIndexChanged;
        // 
        // lblSearch
        // 
        lblSearch.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblSearch.Location = new Point(800, 20);
        lblSearch.Margin = new Padding(0);
        lblSearch.Name = "lblSearch";
        lblSearch.Size = new Size(76, 24);
        lblSearch.TabIndex = 4;
        lblSearch.Text = "بحث:";
        lblSearch.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtSearch
        // 
        txtSearch.Font = new Font("Tahoma", 9F);
        txtSearch.Location = new Point(460, 18);
        txtSearch.Margin = new Padding(0);
        txtSearch.Name = "txtSearch";
        txtSearch.PlaceholderText = "اكتب قيمة التصفية...";
        txtSearch.Size = new Size(330, 26);
        txtSearch.TabIndex = 5;
        txtSearch.TextChanged += txtSearch_TextChanged;
        txtSearch.KeyPress += txtSearch_KeyPress;
        txtSearch.KeyDown += txtSearch_KeyDown;
        // 
        // chkShowInactive
        // 
        chkShowInactive.AutoSize = true;
        chkShowInactive.FlatStyle = FlatStyle.Flat;
        chkShowInactive.Location = new Point(1245, 54);
        chkShowInactive.Margin = new Padding(0);
        chkShowInactive.Name = "chkShowInactive";
        chkShowInactive.Size = new Size(166, 24);
        chkShowInactive.TabIndex = 6;
        chkShowInactive.Text = "عرض غير النشط";
        chkShowInactive.UseVisualStyleBackColor = true;
        chkShowInactive.CheckedChanged += chkShowInactive_CheckedChanged;
        // 
        // chkOnlyLocked
        // 
        chkOnlyLocked.AutoSize = true;
        chkOnlyLocked.FlatStyle = FlatStyle.Flat;
        chkOnlyLocked.Location = new Point(1055, 54);
        chkOnlyLocked.Margin = new Padding(0);
        chkOnlyLocked.Name = "chkOnlyLocked";
        chkOnlyLocked.Size = new Size(160, 24);
        chkOnlyLocked.TabIndex = 7;
        chkOnlyLocked.Text = "المقفلون فقط";
        chkOnlyLocked.UseVisualStyleBackColor = true;
        chkOnlyLocked.CheckedChanged += chkOnlyLocked_CheckedChanged;
        // 
        // chkOnlyMustChangePassword
        // 
        chkOnlyMustChangePassword.AutoSize = true;
        chkOnlyMustChangePassword.FlatStyle = FlatStyle.Flat;
        chkOnlyMustChangePassword.Location = new Point(740, 54);
        chkOnlyMustChangePassword.Margin = new Padding(0);
        chkOnlyMustChangePassword.Name = "chkOnlyMustChangePassword";
        chkOnlyMustChangePassword.Size = new Size(300, 24);
        chkOnlyMustChangePassword.TabIndex = 8;
        chkOnlyMustChangePassword.Text = "ملزمون بتغيير كلمة المرور";
        chkOnlyMustChangePassword.UseVisualStyleBackColor = true;
        chkOnlyMustChangePassword.CheckedChanged += chkOnlyMustChangePassword_CheckedChanged;
        // 
        // pnlSummary
        // 
        pnlSummary.Controls.Add(lblSummary);
        pnlSummary.Dock = DockStyle.Bottom;
        pnlSummary.Location = new Point(0, 750);
        pnlSummary.Margin = new Padding(0);
        pnlSummary.Name = "pnlSummary";
        pnlSummary.Size = new Size(1432, 32);
        pnlSummary.TabIndex = 4;
        // 
        // lblSummary
        // 
        lblSummary.Dock = DockStyle.Fill;
        lblSummary.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblSummary.Location = new Point(0, 0);
        lblSummary.Name = "lblSummary";
        lblSummary.Size = new Size(1432, 32);
        lblSummary.TabIndex = 0;
        lblSummary.Text = "المعروض: 0 من 0";
        lblSummary.TextAlign = ContentAlignment.MiddleRight;
        // 
        // pnlGrid
        // 
        pnlGrid.Controls.Add(dgvUsers);
        pnlGrid.Dock = DockStyle.Fill;
        pnlGrid.Location = new Point(0, 422);
        pnlGrid.Name = "pnlGrid";
        pnlGrid.Padding = new Padding(10);
        pnlGrid.Size = new Size(1432, 328);
        pnlGrid.TabIndex = 5;
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
        dgvUsers.Location = new Point(10, 10);
        dgvUsers.MultiSelect = false;
        dgvUsers.Name = "dgvUsers";
        dgvUsers.ReadOnly = true;
        dgvUsers.RowHeadersVisible = false;
        dgvUsers.RowHeadersWidth = 62;
        dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvUsers.Size = new Size(1412, 308);
        dgvUsers.TabIndex = 0;
        dgvUsers.CellClick += dgvUsers_CellClick;
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
        ClientSize = new Size(1432, 782);
        Controls.Add(pnlGrid);
        Controls.Add(pnlSummary);
        Controls.Add(pnlFilters);
        Controls.Add(pnlButtons);
        Controls.Add(pnlFields);
        Controls.Add(lblMode);
        Font = new Font("Tahoma", 10F);
        KeyPreview = true;
        MinimumSize = new Size(1200, 700);
        Name = "frmUserMaster";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterParent;
        Text = "إدارة المستخدمين";
        Load += frmUserMaster_Load;
        pnlFields.ResumeLayout(false);
        pnlFields.PerformLayout();
        pnlButtons.ResumeLayout(false);
        pnlFilters.ResumeLayout(false);
        gbFilter.ResumeLayout(false);
        gbFilter.PerformLayout();
        pnlSummary.ResumeLayout(false);
        pnlGrid.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvUsers).EndInit();
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
    private System.Windows.Forms.Button btnEdit;
    private System.Windows.Forms.Button btnDelete;
    private System.Windows.Forms.Button btnRefresh;
    private System.Windows.Forms.Button btnClose;
    private System.Windows.Forms.Panel pnlFilters;
    private System.Windows.Forms.GroupBox gbFilter;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ComboBox cbStatus;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.ComboBox cbFilterBy;
    private System.Windows.Forms.Label lblSearch;
    private System.Windows.Forms.TextBox txtSearch;
    private System.Windows.Forms.CheckBox chkShowInactive;
    private System.Windows.Forms.CheckBox chkOnlyLocked;
    private System.Windows.Forms.CheckBox chkOnlyMustChangePassword;
    private System.Windows.Forms.Panel pnlSummary;
    private System.Windows.Forms.Label lblSummary;
    private System.Windows.Forms.Panel pnlGrid;
    private System.Windows.Forms.DataGridView dgvUsers;
    private System.Windows.Forms.DataGridViewTextBoxColumn colId;
    private System.Windows.Forms.DataGridViewTextBoxColumn colUserName;
    private System.Windows.Forms.DataGridViewTextBoxColumn colIsLocked;
    private System.Windows.Forms.DataGridViewTextBoxColumn colIsActive;
    private System.Windows.Forms.ErrorProvider errorProvider;
}
