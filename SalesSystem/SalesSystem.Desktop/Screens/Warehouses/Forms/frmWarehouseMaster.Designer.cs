namespace SalesSystem.Desktop.Screens.Warehouses.Forms;

partial class frmWarehouseMaster
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
        txtName = new TextBox();
        lblPhone = new Label();
        txtPhone = new TextBox();
        lblAddress = new Label();
        txtAddress = new TextBox();
        lblNotes = new Label();
        txtNotes = new TextBox();
        pnlButtons = new FlowLayoutPanel();
        btnNew = new Button();
        btnRefresh = new Button();
        btnSave = new Button();
        btnDelete = new Button();
        btnSetDefault = new Button();
        pnlGrid = new Panel();
        dgvWarehouses = new DataGridView();
        colId = new DataGridViewTextBoxColumn();
        colName = new DataGridViewTextBoxColumn();
        colPhone = new DataGridViewTextBoxColumn();
        colAddress = new DataGridViewTextBoxColumn();
        colIsActive = new DataGridViewTextBoxColumn();
        pnlFilter = new Panel();
        txtSearch = new TextBox();
        lblSearch = new Label();
        errorProvider = new ErrorProvider(components);
        pnlFields.SuspendLayout();
        pnlButtons.SuspendLayout();
        pnlGrid.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvWarehouses).BeginInit();
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
        lblMode.Text = "(( دليل المخازن ))";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlFields
        // 
        pnlFields.Controls.Add(lblCode);
        pnlFields.Controls.Add(txtCode);
        pnlFields.Controls.Add(lblName);
        pnlFields.Controls.Add(txtName);
        pnlFields.Controls.Add(lblPhone);
        pnlFields.Controls.Add(txtPhone);
        pnlFields.Controls.Add(lblAddress);
        pnlFields.Controls.Add(txtAddress);
        pnlFields.Controls.Add(lblNotes);
        pnlFields.Controls.Add(txtNotes);
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
        lblCode.Size = new Size(100, 24);
        lblCode.TabIndex = 0;
        lblCode.Text = "رقم المخزن";
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
        lblName.Size = new Size(106, 24);
        lblName.TabIndex = 2;
        lblName.Text = "اسم المخزن";
        // 
        // txtName
        // 
        txtName.Location = new Point(911, 42);
        txtName.Name = "txtName";
        txtName.Size = new Size(330, 32);
        txtName.TabIndex = 1;
        // 
        // lblPhone
        // 
        lblPhone.AutoSize = true;
        lblPhone.Location = new Point(694, 15);
        lblPhone.Name = "lblPhone";
        lblPhone.Size = new Size(56, 24);
        lblPhone.TabIndex = 4;
        lblPhone.Text = "الهاتف";
        // 
        // txtPhone
        // 
        txtPhone.Location = new Point(675, 42);
        txtPhone.Name = "txtPhone";
        txtPhone.Size = new Size(210, 32);
        txtPhone.TabIndex = 2;
        // 
        // lblAddress
        // 
        lblAddress.AutoSize = true;
        lblAddress.Location = new Point(430, 15);
        lblAddress.Name = "lblAddress";
        lblAddress.Size = new Size(66, 24);
        lblAddress.TabIndex = 6;
        lblAddress.Text = "العنوان";
        // 
        // txtAddress
        // 
        txtAddress.Location = new Point(390, 42);
        txtAddress.Name = "txtAddress";
        txtAddress.Size = new Size(265, 32);
        txtAddress.TabIndex = 3;
        // 
        // lblNotes
        // 
        lblNotes.AutoSize = true;
        lblNotes.Location = new Point(1267, 88);
        lblNotes.Name = "lblNotes";
        lblNotes.Size = new Size(86, 24);
        lblNotes.TabIndex = 8;
        lblNotes.Text = "ملاحظات";
        // 
        // txtNotes
        // 
        txtNotes.Location = new Point(675, 85);
        txtNotes.Multiline = true;
        txtNotes.Name = "txtNotes";
        txtNotes.Size = new Size(702, 56);
        txtNotes.TabIndex = 4;
        // 
        // pnlButtons
        // 
        pnlButtons.BackColor = Color.FromArgb(235, 240, 235);
        pnlButtons.Controls.Add(btnNew);
        pnlButtons.Controls.Add(btnSave);
        pnlButtons.Controls.Add(btnDelete);
        pnlButtons.Controls.Add(btnSetDefault);
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
        // btnSetDefault
        // 
        btnSetDefault.BackColor = Color.FromArgb(30, 86, 160);
        btnSetDefault.FlatStyle = FlatStyle.Flat;
        btnSetDefault.ForeColor = Color.White;
        btnSetDefault.Location = new Point(793, 13);
        btnSetDefault.Name = "btnSetDefault";
        btnSetDefault.Size = new Size(190, 35);
        btnSetDefault.TabIndex = 3;
        btnSetDefault.Text = "⭐ تعيين كمخزن افتراضي";
        btnSetDefault.UseVisualStyleBackColor = false;
        // 
        // pnlGrid
        // 
        pnlGrid.Controls.Add(dgvWarehouses);
        pnlGrid.Controls.Add(pnlFilter);
        pnlGrid.Dock = DockStyle.Fill;
        pnlGrid.Location = new Point(0, 260);
        pnlGrid.Name = "pnlGrid";
        pnlGrid.Padding = new Padding(10);
        pnlGrid.Size = new Size(1432, 410);
        pnlGrid.TabIndex = 3;
        // 
        // dgvWarehouses
        // 
        dgvWarehouses.AllowUserToAddRows = false;
        dgvWarehouses.AllowUserToDeleteRows = false;
        dgvWarehouses.AutoGenerateColumns = false;
        dgvWarehouses.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvWarehouses.BackgroundColor = Color.FromArgb(220, 240, 220);
        dgvWarehouses.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvWarehouses.Columns.AddRange(new DataGridViewColumn[] { colId, colName, colPhone, colAddress, colIsActive });
        dgvWarehouses.Dock = DockStyle.Fill;
        dgvWarehouses.Location = new Point(10, 50);
        dgvWarehouses.MultiSelect = false;
        dgvWarehouses.Name = "dgvWarehouses";
        dgvWarehouses.ReadOnly = true;
        dgvWarehouses.RowHeadersVisible = false;
        dgvWarehouses.RowHeadersWidth = 62;
        dgvWarehouses.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvWarehouses.Size = new Size(1412, 350);
        dgvWarehouses.TabIndex = 0;
        // 
        // colId
        // 
        colId.DataPropertyName = "Id";
        colId.FillWeight = 8F;
        colId.HeaderText = "رقم المخزن";
        colId.MinimumWidth = 80;
        colId.Name = "colId";
        colId.ReadOnly = true;
        // 
        // colName
        // 
        colName.DataPropertyName = "Name";
        colName.FillWeight = 24F;
        colName.HeaderText = "اسم المخزن";
        colName.MinimumWidth = 200;
        colName.Name = "colName";
        colName.ReadOnly = true;
        // 
        // colPhone
        // 
        colPhone.DataPropertyName = "Phone";
        colPhone.FillWeight = 14F;
        colPhone.HeaderText = "الهاتف";
        colPhone.MinimumWidth = 130;
        colPhone.Name = "colPhone";
        colPhone.ReadOnly = true;
        // 
        // colAddress
        // 
        colAddress.DataPropertyName = "Address";
        colAddress.FillWeight = 32F;
        colAddress.HeaderText = "العنوان";
        colAddress.MinimumWidth = 200;
        colAddress.Name = "colAddress";
        colAddress.ReadOnly = true;
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
        // frmWarehouseMaster
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
        Name = "frmWarehouseMaster";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterParent;
        Text = "دليل المخازن";
        pnlFields.ResumeLayout(false);
        pnlFields.PerformLayout();
        pnlButtons.ResumeLayout(false);
        pnlGrid.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvWarehouses).EndInit();
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
    private System.Windows.Forms.TextBox txtName;
    private System.Windows.Forms.Label lblPhone;
    private System.Windows.Forms.TextBox txtPhone;
    private System.Windows.Forms.Label lblAddress;
    private System.Windows.Forms.TextBox txtAddress;
    private System.Windows.Forms.Label lblNotes;
    private System.Windows.Forms.TextBox txtNotes;
    private System.Windows.Forms.FlowLayoutPanel pnlButtons;
    private System.Windows.Forms.Button btnNew;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.Button btnDelete;
    private System.Windows.Forms.Button btnSetDefault;
    private System.Windows.Forms.Panel pnlGrid;
    private System.Windows.Forms.DataGridView dgvWarehouses;
    private System.Windows.Forms.DataGridViewTextBoxColumn colId;
    private System.Windows.Forms.DataGridViewTextBoxColumn colName;
    private System.Windows.Forms.DataGridViewTextBoxColumn colPhone;
    private System.Windows.Forms.DataGridViewTextBoxColumn colAddress;
    private System.Windows.Forms.DataGridViewTextBoxColumn colIsActive;
    private System.Windows.Forms.Panel pnlFilter;
    private System.Windows.Forms.TextBox txtSearch;
    private System.Windows.Forms.Button btnRefresh;
    private System.Windows.Forms.Label lblSearch;
    private System.Windows.Forms.ErrorProvider errorProvider;
}
