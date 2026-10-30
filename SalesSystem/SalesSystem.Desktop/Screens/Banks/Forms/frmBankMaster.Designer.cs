namespace SalesSystem.Desktop.Screens.Banks.Forms;

partial class frmBankMaster
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
        lblAccountNumber = new Label();
        txtAccountNumber = new TextBox();
        lblIban = new Label();
        txtIban = new TextBox();
        lblAccount = new Label();
        txtAccountName = new TextBox();
        pnlButtons = new FlowLayoutPanel();
        btnNew = new Button();
        btnSave = new Button();
        btnDelete = new Button();
        pnlGrid = new Panel();
        dgvBanks = new DataGridView();
        colId = new DataGridViewTextBoxColumn();
        colName = new DataGridViewTextBoxColumn();
        colAccountNumber = new DataGridViewTextBoxColumn();
        colIban = new DataGridViewTextBoxColumn();
        colIsActive = new DataGridViewTextBoxColumn();
        pnlFilter = new Panel();
        btnRefresh = new Button();
        txtSearch = new TextBox();
        lblSearch = new Label();
        errorProvider = new ErrorProvider(components);
        pnlFields.SuspendLayout();
        pnlButtons.SuspendLayout();
        pnlGrid.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvBanks).BeginInit();
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
        lblMode.Text = "(( دليل البنوك ))";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlFields
        // 
        pnlFields.Controls.Add(lblCode);
        pnlFields.Controls.Add(txtCode);
        pnlFields.Controls.Add(lblName);
        pnlFields.Controls.Add(lblNameReq);
        pnlFields.Controls.Add(txtName);
        pnlFields.Controls.Add(lblAccountNumber);
        pnlFields.Controls.Add(txtAccountNumber);
        pnlFields.Controls.Add(lblIban);
        pnlFields.Controls.Add(txtIban);
        pnlFields.Controls.Add(lblAccount);
        pnlFields.Controls.Add(txtAccountName);
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
        lblCode.Size = new Size(84, 24);
        lblCode.TabIndex = 0;
        lblCode.Text = "رقم البنك";
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
        lblName.Size = new Size(90, 24);
        lblName.TabIndex = 2;
        lblName.Text = "اسم البنك";
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
        txtName.Size = new Size(300, 32);
        txtName.TabIndex = 1;
        // 
        // lblAccountNumber
        // 
        lblAccountNumber.AutoSize = true;
        lblAccountNumber.Location = new Point(655, 15);
        lblAccountNumber.Name = "lblAccountNumber";
        lblAccountNumber.Size = new Size(150, 24);
        lblAccountNumber.TabIndex = 4;
        lblAccountNumber.Text = "رقم الحساب البنكي";
        // 
        // txtAccountNumber
        // 
        txtAccountNumber.Location = new Point(555, 42);
        txtAccountNumber.Name = "txtAccountNumber";
        txtAccountNumber.Size = new Size(280, 32);
        txtAccountNumber.TabIndex = 2;
        // 
        // lblIban
        // 
        lblIban.AutoSize = true;
        lblIban.Location = new Point(1267, 88);
        lblIban.Name = "lblIban";
        lblIban.Size = new Size(84, 24);
        lblIban.TabIndex = 6;
        lblIban.Text = "الآيبان";
        // 
        // txtIban
        // 
        txtIban.Location = new Point(1067, 115);
        txtIban.Name = "txtIban";
        txtIban.Size = new Size(310, 32);
        txtIban.TabIndex = 3;
        // 
        // lblAccount
        // 
        lblAccount.AutoSize = true;
        lblAccount.Location = new Point(967, 88);
        lblAccount.Name = "lblAccount";
        lblAccount.Size = new Size(130, 24);
        lblAccount.TabIndex = 8;
        lblAccount.Text = "الحساب المحاسبي";
        // 
        // txtAccountName
        // 
        txtAccountName.Location = new Point(767, 115);
        txtAccountName.Name = "txtAccountName";
        txtAccountName.ReadOnly = true;
        txtAccountName.Size = new Size(280, 32);
        txtAccountName.TabIndex = 4;
        // 
        // pnlButtons
        // 
        pnlButtons.BackColor = Color.FromArgb(235, 240, 235);
        pnlButtons.Controls.Add(btnNew);
        pnlButtons.Controls.Add(btnSave);
        pnlButtons.Controls.Add(btnDelete);
        pnlButtons.Dock = DockStyle.Top;
        pnlButtons.Location = new Point(0, 200);
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
        btnSave.Text = "💾 إضافة";
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
        pnlGrid.Controls.Add(dgvBanks);
        pnlGrid.Controls.Add(pnlFilter);
        pnlGrid.Dock = DockStyle.Fill;
        pnlGrid.Location = new Point(0, 260);
        pnlGrid.Name = "pnlGrid";
        pnlGrid.Padding = new Padding(10);
        pnlGrid.Size = new Size(1432, 410);
        pnlGrid.TabIndex = 3;
        // 
        // dgvBanks
        // 
        dgvBanks.AllowUserToAddRows = false;
        dgvBanks.AllowUserToDeleteRows = false;
        dgvBanks.AutoGenerateColumns = false;
        dgvBanks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvBanks.BackgroundColor = Color.FromArgb(220, 240, 220);
        dgvBanks.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvBanks.Columns.AddRange(new DataGridViewColumn[] { colId, colName, colAccountNumber, colIban, colIsActive });
        dgvBanks.Dock = DockStyle.Fill;
        dgvBanks.Location = new Point(10, 50);
        dgvBanks.MultiSelect = false;
        dgvBanks.Name = "dgvBanks";
        dgvBanks.ReadOnly = true;
        dgvBanks.RowHeadersVisible = false;
        dgvBanks.RowHeadersWidth = 62;
        dgvBanks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvBanks.Size = new Size(1412, 350);
        dgvBanks.TabIndex = 20;
        // 
        // colId
        // 
        colId.DataPropertyName = "Id";
        colId.FillWeight = 8F;
        colId.HeaderText = "رقم البنك";
        colId.MinimumWidth = 90;
        colId.Name = "colId";
        colId.ReadOnly = true;
        // 
        // colName
        // 
        colName.DataPropertyName = "Name";
        colName.FillWeight = 28F;
        colName.HeaderText = "اسم البنك";
        colName.MinimumWidth = 200;
        colName.Name = "colName";
        colName.ReadOnly = true;
        // 
        // colAccountNumber
        // 
        colAccountNumber.DataPropertyName = "AccountNumber";
        colAccountNumber.FillWeight = 24F;
        colAccountNumber.HeaderText = "رقم الحساب";
        colAccountNumber.MinimumWidth = 160;
        colAccountNumber.Name = "colAccountNumber";
        colAccountNumber.ReadOnly = true;
        // 
        // colIban
        // 
        colIban.DataPropertyName = "Iban";
        colIban.FillWeight = 30F;
        colIban.HeaderText = "الآيبان";
        colIban.MinimumWidth = 180;
        colIban.Name = "colIban";
        colIban.ReadOnly = true;
        // 
        // colIsActive
        // 
        colIsActive.DataPropertyName = "IsActive";
        colIsActive.FillWeight = 8F;
        colIsActive.HeaderText = "نشط";
        colIsActive.MinimumWidth = 60;
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
        pnlFilter.TabIndex = 21;
        // 
        // btnRefresh
        // 
        btnRefresh.BackColor = Color.White;
        btnRefresh.FlatStyle = FlatStyle.Flat;
        btnRefresh.Location = new Point(550, 4);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(130, 32);
        btnRefresh.TabIndex = 99;
        btnRefresh.Text = "🔄 تحديث";
        btnRefresh.UseVisualStyleBackColor = false;
        // 
        // txtSearch
        // 
        txtSearch.Location = new Point(690, 7);
        txtSearch.Name = "txtSearch";
        txtSearch.Size = new Size(300, 32);
        txtSearch.TabIndex = 98;
        // 
        // lblSearch
        // 
        lblSearch.AutoSize = true;
        lblSearch.Location = new Point(1000, 10);
        lblSearch.Name = "lblSearch";
        lblSearch.Size = new Size(56, 24);
        lblSearch.TabIndex = 97;
        lblSearch.Text = "بحث:";
        // 
        // errorProvider
        // 
        errorProvider.ContainerControl = this;
        errorProvider.RightToLeft = true;
        // 
        // frmBankMaster
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
        Name = "frmBankMaster";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterParent;
        Text = "دليل البنوك";
        pnlFields.ResumeLayout(false);
        pnlFields.PerformLayout();
        pnlButtons.ResumeLayout(false);
        pnlGrid.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvBanks).EndInit();
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
    private System.Windows.Forms.Label lblAccountNumber;
    private System.Windows.Forms.TextBox txtAccountNumber;
    private System.Windows.Forms.Label lblIban;
    private System.Windows.Forms.TextBox txtIban;
    private System.Windows.Forms.Label lblAccount;
    private System.Windows.Forms.TextBox txtAccountName;
    private System.Windows.Forms.FlowLayoutPanel pnlButtons;
    private System.Windows.Forms.Button btnNew;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.Button btnDelete;
    private System.Windows.Forms.Panel pnlGrid;
    private System.Windows.Forms.DataGridView dgvBanks;
    private System.Windows.Forms.DataGridViewTextBoxColumn colId;
    private System.Windows.Forms.DataGridViewTextBoxColumn colName;
    private System.Windows.Forms.DataGridViewTextBoxColumn colAccountNumber;
    private System.Windows.Forms.DataGridViewTextBoxColumn colIban;
    private System.Windows.Forms.DataGridViewTextBoxColumn colIsActive;
    private System.Windows.Forms.Panel pnlFilter;
    private System.Windows.Forms.TextBox txtSearch;
    private System.Windows.Forms.Button btnRefresh;
    private System.Windows.Forms.Label lblSearch;
    private System.Windows.Forms.ErrorProvider errorProvider;
}
