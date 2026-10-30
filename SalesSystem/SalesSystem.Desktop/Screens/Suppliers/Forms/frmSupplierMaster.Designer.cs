namespace SalesSystem.Desktop.Suppliers.Forms;

partial class frmSupplierMaster
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
        lblId = new Label();
        txtId = new TextBox();
        lblName = new Label();
        txtName = new TextBox();
        lblPhone = new Label();
        txtPhone = new TextBox();
        lblTaxNumber = new Label();
        txtTaxNumber = new TextBox();
        lblEmail = new Label();
        txtEmail = new TextBox();
        lblCreditLimit = new Label();
        numCreditLimit = new NumericUpDown();
        lblAddress = new Label();
        txtAddress = new TextBox();
        lblNotes = new Label();
        txtNotes = new TextBox();
        pnlButtons = new FlowLayoutPanel();
        btnNew = new Button();
        btnRefresh = new Button();
        btnSave = new Button();
        btnDelete = new Button();
        pnlGrid = new Panel();
        dgvSuppliers = new DataGridView();
        colId = new DataGridViewTextBoxColumn();
        colName = new DataGridViewTextBoxColumn();
        colPhone = new DataGridViewTextBoxColumn();
        colCreditLimit = new DataGridViewTextBoxColumn();
        colIsActive = new DataGridViewTextBoxColumn();
        pnlFilter = new Panel();
        txtSearch = new TextBox();
        lblSearch = new Label();
        errorProvider = new ErrorProvider(components);
        pnlFields.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numCreditLimit).BeginInit();
        pnlButtons.SuspendLayout();
        pnlGrid.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvSuppliers).BeginInit();
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
        lblMode.Text = "(( دليل الموردين ))";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlFields
        // 
        pnlFields.Controls.Add(lblId);
        pnlFields.Controls.Add(txtId);
        pnlFields.Controls.Add(lblName);
        pnlFields.Controls.Add(txtName);
        pnlFields.Controls.Add(lblPhone);
        pnlFields.Controls.Add(txtPhone);
        pnlFields.Controls.Add(lblTaxNumber);
        pnlFields.Controls.Add(txtTaxNumber);
        pnlFields.Controls.Add(lblEmail);
        pnlFields.Controls.Add(txtEmail);
        pnlFields.Controls.Add(lblCreditLimit);
        pnlFields.Controls.Add(numCreditLimit);
        pnlFields.Controls.Add(lblAddress);
        pnlFields.Controls.Add(txtAddress);
        pnlFields.Controls.Add(lblNotes);
        pnlFields.Controls.Add(txtNotes);
        pnlFields.Dock = DockStyle.Top;
        pnlFields.Location = new Point(0, 50);
        pnlFields.Name = "pnlFields";
        pnlFields.Size = new Size(1432, 220);
        pnlFields.TabIndex = 1;
        // 
        // lblId
        // 
        lblId.AutoSize = true;
        lblId.Location = new Point(1140, 10);
        lblId.Name = "lblId";
        lblId.Size = new Size(93, 24);
        lblId.TabIndex = 0;
        lblId.Text = "رقم المورد";
        // 
        // txtId
        // 
        txtId.Location = new Point(1140, 36);
        txtId.Name = "txtId";
        txtId.ReadOnly = true;
        txtId.Size = new Size(250, 32);
        txtId.TabIndex = 0;
        // 
        // lblName
        // 
        lblName.AutoSize = true;
        lblName.Location = new Point(860, 10);
        lblName.Name = "lblName";
        lblName.Size = new Size(120, 24);
        lblName.TabIndex = 2;
        lblName.Text = "اسم المورد *";
        // 
        // txtName
        // 
        txtName.Location = new Point(860, 36);
        txtName.Name = "txtName";
        txtName.Size = new Size(250, 32);
        txtName.TabIndex = 1;
        // 
        // lblPhone
        // 
        lblPhone.AutoSize = true;
        lblPhone.Location = new Point(580, 10);
        lblPhone.Name = "lblPhone";
        lblPhone.Size = new Size(62, 24);
        lblPhone.TabIndex = 4;
        lblPhone.Text = "الهاتف";
        // 
        // txtPhone
        // 
        txtPhone.Location = new Point(580, 36);
        txtPhone.Name = "txtPhone";
        txtPhone.Size = new Size(250, 32);
        txtPhone.TabIndex = 2;
        // 
        // lblTaxNumber
        // 
        lblTaxNumber.AutoSize = true;
        lblTaxNumber.Location = new Point(300, 10);
        lblTaxNumber.Name = "lblTaxNumber";
        lblTaxNumber.Size = new Size(124, 24);
        lblTaxNumber.TabIndex = 6;
        lblTaxNumber.Text = "الرقم الضريبي";
        // 
        // txtTaxNumber
        // 
        txtTaxNumber.Location = new Point(300, 36);
        txtTaxNumber.Name = "txtTaxNumber";
        txtTaxNumber.Size = new Size(250, 32);
        txtTaxNumber.TabIndex = 3;
        // 
        // lblEmail
        // 
        lblEmail.AutoSize = true;
        lblEmail.Location = new Point(1140, 80);
        lblEmail.Name = "lblEmail";
        lblEmail.Size = new Size(53, 24);
        lblEmail.TabIndex = 8;
        lblEmail.Text = "البريد";
        // 
        // txtEmail
        // 
        txtEmail.Location = new Point(1140, 106);
        txtEmail.Name = "txtEmail";
        txtEmail.Size = new Size(250, 32);
        txtEmail.TabIndex = 4;
        // 
        // lblCreditLimit
        // 
        lblCreditLimit.AutoSize = true;
        lblCreditLimit.Location = new Point(860, 80);
        lblCreditLimit.Name = "lblCreditLimit";
        lblCreditLimit.Size = new Size(101, 24);
        lblCreditLimit.TabIndex = 10;
        lblCreditLimit.Text = "حد الائتمان";
        // 
        // numCreditLimit
        // 
        numCreditLimit.DecimalPlaces = 2;
        numCreditLimit.Location = new Point(860, 106);
        numCreditLimit.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 });
        numCreditLimit.Name = "numCreditLimit";
        numCreditLimit.Size = new Size(250, 32);
        numCreditLimit.TabIndex = 5;
        numCreditLimit.ThousandsSeparator = true;
        // 
        // lblAddress
        // 
        lblAddress.AutoSize = true;
        lblAddress.Location = new Point(300, 80);
        lblAddress.Name = "lblAddress";
        lblAddress.Size = new Size(65, 24);
        lblAddress.TabIndex = 12;
        lblAddress.Text = "العنوان";
        // 
        // txtAddress
        // 
        txtAddress.Location = new Point(300, 106);
        txtAddress.Name = "txtAddress";
        txtAddress.Size = new Size(530, 32);
        txtAddress.TabIndex = 6;
        // 
        // lblNotes
        // 
        lblNotes.AutoSize = true;
        lblNotes.Location = new Point(300, 150);
        lblNotes.Name = "lblNotes";
        lblNotes.Size = new Size(83, 24);
        lblNotes.TabIndex = 14;
        lblNotes.Text = "ملاحظات";
        // 
        // txtNotes
        // 
        txtNotes.Location = new Point(300, 176);
        txtNotes.Multiline = true;
        txtNotes.Name = "txtNotes";
        txtNotes.ScrollBars = ScrollBars.Vertical;
        txtNotes.Size = new Size(1090, 34);
        txtNotes.TabIndex = 7;
        // 
        // pnlButtons
        // 
        pnlButtons.BackColor = Color.FromArgb(235, 240, 235);
        pnlButtons.Controls.Add(btnNew);
        pnlButtons.Controls.Add(btnSave);
        pnlButtons.Controls.Add(btnDelete);
        pnlButtons.Dock = DockStyle.Top;
        pnlButtons.Location = new Point(0, 270);
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
        pnlGrid.Controls.Add(dgvSuppliers);
        pnlGrid.Controls.Add(pnlFilter);
        pnlGrid.Dock = DockStyle.Fill;
        pnlGrid.Location = new Point(0, 330);
        pnlGrid.Name = "pnlGrid";
        pnlGrid.Padding = new Padding(10);
        pnlGrid.Size = new Size(1432, 340);
        pnlGrid.TabIndex = 3;
        // 
        // dgvSuppliers
        // 
        dgvSuppliers.AllowUserToAddRows = false;
        dgvSuppliers.AllowUserToDeleteRows = false;
        dgvSuppliers.AutoGenerateColumns = false;
        dgvSuppliers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvSuppliers.BackgroundColor = Color.FromArgb(220, 240, 220);
        dgvSuppliers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvSuppliers.Columns.AddRange(new DataGridViewColumn[] { colId, colName, colPhone, colCreditLimit, colIsActive });
        dgvSuppliers.Dock = DockStyle.Fill;
        dgvSuppliers.Location = new Point(10, 50);
        dgvSuppliers.MultiSelect = false;
        dgvSuppliers.Name = "dgvSuppliers";
        dgvSuppliers.ReadOnly = true;
        dgvSuppliers.RowHeadersVisible = false;
        dgvSuppliers.RowHeadersWidth = 62;
        dgvSuppliers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvSuppliers.Size = new Size(1412, 280);
        dgvSuppliers.TabIndex = 0;
        // 
        // colId
        // 
        colId.DataPropertyName = "Id";
        colId.FillWeight = 8F;
        colId.HeaderText = "رقم المورد";
        colId.MinimumWidth = 90;
        colId.Name = "colId";
        colId.ReadOnly = true;
        // 
        // colName
        // 
        colName.DataPropertyName = "Name";
        colName.FillWeight = 40F;
        colName.HeaderText = "اسم المورد";
        colName.MinimumWidth = 220;
        colName.Name = "colName";
        colName.ReadOnly = true;
        // 
        // colPhone
        // 
        colPhone.DataPropertyName = "Phone";
        colPhone.FillWeight = 20F;
        colPhone.HeaderText = "الهاتف";
        colPhone.MinimumWidth = 130;
        colPhone.Name = "colPhone";
        colPhone.ReadOnly = true;
        // 
        // colCreditLimit
        // 
        colCreditLimit.DataPropertyName = "CreditLimit";
        colCreditLimit.FillWeight = 20F;
        colCreditLimit.HeaderText = "حد الائتمان";
        colCreditLimit.MinimumWidth = 130;
        colCreditLimit.Name = "colCreditLimit";
        colCreditLimit.ReadOnly = true;
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
        pnlFilter.TabIndex = 1;
        // 
        // txtSearch
        // 
        txtSearch.Location = new Point(690, 7);
        txtSearch.Name = "txtSearch";
        txtSearch.PlaceholderText = "بحث بالاسم أو الهاتف أو البريد أو الرقم الضريبي...";
        txtSearch.Size = new Size(400, 32);
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
        lblSearch.Location = new Point(1100, 10);
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
        // frmSupplierMaster
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
        Name = "frmSupplierMaster";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterParent;
        Text = "دليل الموردين";
        pnlFields.ResumeLayout(false);
        pnlFields.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numCreditLimit).EndInit();
        pnlButtons.ResumeLayout(false);
        pnlGrid.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvSuppliers).EndInit();
        pnlFilter.ResumeLayout(false);
        pnlFilter.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        ResumeLayout(false);
    }

    private System.Windows.Forms.Label lblMode;
    private System.Windows.Forms.Panel pnlFields;
    private System.Windows.Forms.Label lblId;
    private System.Windows.Forms.TextBox txtId;
    private System.Windows.Forms.Label lblName;
    private System.Windows.Forms.TextBox txtName;
    private System.Windows.Forms.Label lblPhone;
    private System.Windows.Forms.TextBox txtPhone;
    private System.Windows.Forms.Label lblTaxNumber;
    private System.Windows.Forms.TextBox txtTaxNumber;
    private System.Windows.Forms.Label lblEmail;
    private System.Windows.Forms.TextBox txtEmail;
    private System.Windows.Forms.Label lblCreditLimit;
    private System.Windows.Forms.NumericUpDown numCreditLimit;
    private System.Windows.Forms.Label lblAddress;
    private System.Windows.Forms.TextBox txtAddress;
    private System.Windows.Forms.Label lblNotes;
    private System.Windows.Forms.TextBox txtNotes;
    private System.Windows.Forms.FlowLayoutPanel pnlButtons;
    private System.Windows.Forms.Button btnNew;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.Button btnDelete;
    private System.Windows.Forms.Panel pnlGrid;
    private System.Windows.Forms.DataGridView dgvSuppliers;
    private System.Windows.Forms.DataGridViewTextBoxColumn colId;
    private System.Windows.Forms.DataGridViewTextBoxColumn colName;
    private System.Windows.Forms.DataGridViewTextBoxColumn colPhone;
    private System.Windows.Forms.DataGridViewTextBoxColumn colCreditLimit;
    private System.Windows.Forms.DataGridViewTextBoxColumn colIsActive;
    private System.Windows.Forms.Panel pnlFilter;
    private System.Windows.Forms.TextBox txtSearch;
    private System.Windows.Forms.Button btnRefresh;
    private System.Windows.Forms.Label lblSearch;
    private System.Windows.Forms.ErrorProvider errorProvider;
}
