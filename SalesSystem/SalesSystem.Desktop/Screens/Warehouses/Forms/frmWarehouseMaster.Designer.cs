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
        lblNameReq = new Label();
        txtName = new TextBox();
        lblPhone = new Label();
        txtPhone = new TextBox();
        lblAddress = new Label();
        txtAddress = new TextBox();
        lblNotes = new Label();
        txtNotes = new TextBox();
        pnlButtons = new FlowLayoutPanel();
        btnNew = new Button();
        btnSave = new Button();
        btnEdit = new Button();
        btnDelete = new Button();
        btnSetDefault = new Button();
        btnRefresh = new Button();
        btnClose = new Button();
        pnlFilters = new Panel();
        gbFilter = new GroupBox();
        label1 = new Label();
        cbStatus = new ComboBox();
        label2 = new Label();
        cbFilterBy = new ComboBox();
        txtFilterValue = new TextBox();
        chkShowInactive = new CheckBox();
        pnlSummary = new Panel();
        lblSummary = new Label();
        pnlGrid = new Panel();
        dgvWarehouses = new DataGridView();
        colId = new DataGridViewTextBoxColumn();
        colName = new DataGridViewTextBoxColumn();
        colPhone = new DataGridViewTextBoxColumn();
        colAddress = new DataGridViewTextBoxColumn();
        colIsActive = new DataGridViewTextBoxColumn();
        errorProvider = new ErrorProvider(components);
        pnlFields.SuspendLayout();
        pnlButtons.SuspendLayout();
        pnlFilters.SuspendLayout();
        gbFilter.SuspendLayout();
        pnlSummary.SuspendLayout();
        pnlGrid.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvWarehouses).BeginInit();
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
        pnlFields.Controls.Add(lblNameReq);
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
        pnlButtons.Controls.Add(btnEdit);
        pnlButtons.Controls.Add(btnDelete);
        pnlButtons.Controls.Add(btnSetDefault);
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
        // btnSetDefault
        // 
        btnSetDefault.BackColor = Color.FromArgb(111, 66, 193);
        btnSetDefault.FlatStyle = FlatStyle.Flat;
        btnSetDefault.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnSetDefault.ForeColor = Color.White;
        btnSetDefault.Location = new Point(659, 13);
        btnSetDefault.Name = "btnSetDefault";
        btnSetDefault.Size = new Size(190, 35);
        btnSetDefault.TabIndex = 4;
        btnSetDefault.Text = "⭐ تعيين كمخزن افتراضي";
        btnSetDefault.UseVisualStyleBackColor = false;
        btnSetDefault.Click += btnSetDefault_Click;
        // 
        // btnRefresh
        // 
        btnRefresh.BackColor = Color.FromArgb(23, 162, 184);
        btnRefresh.FlatStyle = FlatStyle.Flat;
        btnRefresh.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnRefresh.ForeColor = Color.White;
        btnRefresh.Location = new Point(523, 13);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(130, 35);
        btnRefresh.TabIndex = 5;
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
        btnClose.Location = new Point(387, 13);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(130, 35);
        btnClose.TabIndex = 6;
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
        gbFilter.Controls.Add(label1);
        gbFilter.Controls.Add(cbStatus);
        gbFilter.Controls.Add(label2);
        gbFilter.Controls.Add(cbFilterBy);
        gbFilter.Controls.Add(txtFilterValue);
        gbFilter.Controls.Add(chkShowInactive);
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
        label1.Location = new Point(1290, 30);
        label1.Margin = new Padding(0);
        label1.Name = "label1";
        label1.Size = new Size(90, 26);
        label1.TabIndex = 0;
        label1.Text = "الحالة:";
        label1.TextAlign = ContentAlignment.MiddleRight;
        // 
        // cbStatus
        // 
        cbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
        cbStatus.FlatStyle = FlatStyle.Flat;
        cbStatus.Font = new Font("Tahoma", 10F);
        cbStatus.FormattingEnabled = true;
        cbStatus.Items.AddRange(new object[] { "الكل", "نشط", "معطّل" });
        cbStatus.Location = new Point(1152, 26);
        cbStatus.Margin = new Padding(0);
        cbStatus.Name = "cbStatus";
        cbStatus.Size = new Size(130, 32);
        cbStatus.TabIndex = 1;
        cbStatus.SelectedIndexChanged += cbStatus_SelectedIndexChanged;
        // 
        // label2
        // 
        label2.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        label2.Location = new Point(1032, 30);
        label2.Margin = new Padding(0);
        label2.Name = "label2";
        label2.Size = new Size(110, 26);
        label2.TabIndex = 2;
        label2.Text = "تصفية بـ:";
        label2.TextAlign = ContentAlignment.MiddleRight;
        // 
        // cbFilterBy
        // 
        cbFilterBy.DropDownStyle = ComboBoxStyle.DropDownList;
        cbFilterBy.FlatStyle = FlatStyle.Flat;
        cbFilterBy.Font = new Font("Tahoma", 10F);
        cbFilterBy.FormattingEnabled = true;
        cbFilterBy.Items.AddRange(new object[] { "الكل", "الاسم", "العنوان", "الهاتف" });
        cbFilterBy.Location = new Point(884, 26);
        cbFilterBy.Margin = new Padding(0);
        cbFilterBy.Name = "cbFilterBy";
        cbFilterBy.Size = new Size(146, 32);
        cbFilterBy.TabIndex = 3;
        cbFilterBy.SelectedIndexChanged += cbFilterBy_SelectedIndexChanged;
        // 
        // txtFilterValue
        // 
        txtFilterValue.Font = new Font("Tahoma", 10F);
        txtFilterValue.Location = new Point(460, 26);
        txtFilterValue.Margin = new Padding(0);
        txtFilterValue.Name = "txtFilterValue";
        txtFilterValue.PlaceholderText = "اكتب قيمة التصفية...";
        txtFilterValue.Size = new Size(412, 32);
        txtFilterValue.TabIndex = 4;
        txtFilterValue.TextChanged += txtFilterValue_TextChanged;
        txtFilterValue.KeyPress += txtFilterValue_KeyPress;
        txtFilterValue.KeyDown += txtFilterValue_KeyDown;
        // 
        // chkShowInactive
        // 
        chkShowInactive.AutoSize = true;
        chkShowInactive.FlatStyle = FlatStyle.Flat;
        chkShowInactive.Location = new Point(1245, 64);
        chkShowInactive.Margin = new Padding(0);
        chkShowInactive.Name = "chkShowInactive";
        chkShowInactive.Size = new Size(166, 28);
        chkShowInactive.TabIndex = 5;
        chkShowInactive.Text = "عرض غير النشط";
        chkShowInactive.UseVisualStyleBackColor = true;
        chkShowInactive.CheckedChanged += chkShowInactive_CheckedChanged;
        // 
        // pnlSummary
        // 
        pnlSummary.Controls.Add(lblSummary);
        pnlSummary.Dock = DockStyle.Bottom;
        pnlSummary.Location = new Point(0, 768);
        pnlSummary.Margin = new Padding(0);
        pnlSummary.Name = "pnlSummary";
        pnlSummary.Size = new Size(1432, 32);
        pnlSummary.TabIndex = 5;
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
        pnlGrid.Controls.Add(dgvWarehouses);
        pnlGrid.Dock = DockStyle.Fill;
        pnlGrid.Location = new Point(0, 372);
        pnlGrid.Name = "pnlGrid";
        pnlGrid.Padding = new Padding(10);
        pnlGrid.Size = new Size(1432, 396);
        pnlGrid.TabIndex = 4;
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
        dgvWarehouses.Location = new Point(10, 10);
        dgvWarehouses.MultiSelect = false;
        dgvWarehouses.Name = "dgvWarehouses";
        dgvWarehouses.ReadOnly = true;
        dgvWarehouses.RowHeadersVisible = false;
        dgvWarehouses.RowHeadersWidth = 62;
        dgvWarehouses.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvWarehouses.Size = new Size(1412, 376);
        dgvWarehouses.TabIndex = 0;
        dgvWarehouses.CellClick += dgvWarehouses_CellClick;
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
        ClientSize = new Size(1432, 800);
        Controls.Add(pnlGrid);
        Controls.Add(pnlSummary);
        Controls.Add(pnlFilters);
        Controls.Add(pnlButtons);
        Controls.Add(pnlFields);
        Controls.Add(lblMode);
        Font = new Font("Tahoma", 10F);
        KeyPreview = true;
        MinimumSize = new Size(1200, 700);
        Name = "frmWarehouseMaster";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterParent;
        Text = "دليل المخازن";
        Load += frmWarehouseMaster_Load;
        pnlFields.ResumeLayout(false);
        pnlFields.PerformLayout();
        pnlButtons.ResumeLayout(false);
        pnlFilters.ResumeLayout(false);
        gbFilter.ResumeLayout(false);
        gbFilter.PerformLayout();
        pnlSummary.ResumeLayout(false);
        pnlGrid.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvWarehouses).EndInit();
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
    private System.Windows.Forms.Label lblPhone;
    private System.Windows.Forms.TextBox txtPhone;
    private System.Windows.Forms.Label lblAddress;
    private System.Windows.Forms.TextBox txtAddress;
    private System.Windows.Forms.Label lblNotes;
    private System.Windows.Forms.TextBox txtNotes;
    private System.Windows.Forms.FlowLayoutPanel pnlButtons;
    private System.Windows.Forms.Button btnNew;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.Button btnEdit;
    private System.Windows.Forms.Button btnDelete;
    private System.Windows.Forms.Button btnSetDefault;
    private System.Windows.Forms.Button btnRefresh;
    private System.Windows.Forms.Button btnClose;
    private System.Windows.Forms.Panel pnlFilters;
    private System.Windows.Forms.GroupBox gbFilter;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ComboBox cbStatus;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.ComboBox cbFilterBy;
    private System.Windows.Forms.TextBox txtFilterValue;
    private System.Windows.Forms.CheckBox chkShowInactive;
    private System.Windows.Forms.Panel pnlSummary;
    private System.Windows.Forms.Label lblSummary;
    private System.Windows.Forms.Panel pnlGrid;
    private System.Windows.Forms.DataGridView dgvWarehouses;
    private System.Windows.Forms.DataGridViewTextBoxColumn colId;
    private System.Windows.Forms.DataGridViewTextBoxColumn colName;
    private System.Windows.Forms.DataGridViewTextBoxColumn colPhone;
    private System.Windows.Forms.DataGridViewTextBoxColumn colAddress;
    private System.Windows.Forms.DataGridViewTextBoxColumn colIsActive;
    private System.Windows.Forms.ErrorProvider errorProvider;
}
