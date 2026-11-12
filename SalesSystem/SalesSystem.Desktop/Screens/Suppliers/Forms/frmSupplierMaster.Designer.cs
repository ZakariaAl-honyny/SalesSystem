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
        lblNameReq = new Label();
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
        btnSave = new Button();
        btnEdit = new Button();
        btnDelete = new Button();
        btnRefresh = new Button();
        btnClose = new Button();
        pnlFilters = new Panel();
        gbFilter = new GroupBox();
        label2 = new Label();
        cbFilterBy = new ComboBox();
        txtSearch = new TextBox();
        label1 = new Label();
        cbStatus = new ComboBox();
        chkShowInactive = new CheckBox();
        chkOnlyWithBalance = new CheckBox();
        chkOnlyOverLimit = new CheckBox();
        pnlSummary = new Panel();
        lblSummary = new Label();
        pnlGrid = new Panel();
        dgvSuppliers = new DataGridView();
        errorProvider = new ErrorProvider(components);
        pnlFields.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numCreditLimit).BeginInit();
        pnlButtons.SuspendLayout();
        pnlFilters.SuspendLayout();
        gbFilter.SuspendLayout();
        pnlSummary.SuspendLayout();
        pnlGrid.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvSuppliers).BeginInit();
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
        pnlFields.Controls.Add(lblNameReq);
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
        txtId.TabStop = false;
        // 
        // lblName
        // 
        lblName.AutoSize = true;
        lblName.Location = new Point(860, 10);
        lblName.Name = "lblName";
        lblName.Size = new Size(120, 24);
        lblName.TabIndex = 2;
        lblName.Text = "اسم المورد";
        // 
        // lblNameReq
        // 
        lblNameReq.AutoSize = true;
        lblNameReq.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblNameReq.ForeColor = Color.Red;
        lblNameReq.Location = new Point(842, 10);
        lblNameReq.Name = "lblNameReq";
        lblNameReq.Size = new Size(23, 24);
        lblNameReq.TabIndex = 3;
        lblNameReq.Text = "*";
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
        pnlButtons.Controls.Add(btnEdit);
        pnlButtons.Controls.Add(btnDelete);
        pnlButtons.Controls.Add(btnRefresh);
        pnlButtons.Controls.Add(btnClose);
        pnlButtons.Dock = DockStyle.Top;
        pnlButtons.Location = new Point(0, 270);
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
        pnlFilters.Location = new Point(0, 330);
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
        gbFilter.Controls.Add(txtSearch);
        gbFilter.Controls.Add(label1);
        gbFilter.Controls.Add(cbStatus);
        gbFilter.Controls.Add(chkShowInactive);
        gbFilter.Controls.Add(chkOnlyWithBalance);
        gbFilter.Controls.Add(chkOnlyOverLimit);
        gbFilter.Dock = DockStyle.Fill;
        gbFilter.Font = new Font("Tahoma", 10F, FontStyle.Bold);
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
        // label2
        // 
        label2.Font = new Font("Tahoma", 10F, FontStyle.Bold);
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
        cbFilterBy.Items.AddRange(new object[] { "الاسم", "الهاتف", "البريد", "الرقم الضريبي" });
        cbFilterBy.Location = new Point(884, 26);
        cbFilterBy.Margin = new Padding(0);
        cbFilterBy.Name = "cbFilterBy";
        cbFilterBy.Size = new Size(146, 32);
        cbFilterBy.TabIndex = 3;
        cbFilterBy.SelectedIndexChanged += cbFilterBy_SelectedIndexChanged;
        // 
        // txtSearch
        // 
        txtSearch.Font = new Font("Tahoma", 10F);
        txtSearch.Location = new Point(460, 26);
        txtSearch.Margin = new Padding(0);
        txtSearch.Name = "txtSearch";
        txtSearch.PlaceholderText = "اكتب قيمة التصفية...";
        txtSearch.Size = new Size(412, 32);
        txtSearch.TabIndex = 4;
        txtSearch.TextChanged += txtSearch_TextChanged;
        txtSearch.KeyPress += txtSearch_KeyPress;
        txtSearch.KeyDown += txtSearch_KeyDown;
        // 
        // label1
        // 
        label1.Font = new Font("Tahoma", 10F, FontStyle.Bold);
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
        // chkShowInactive
        // 
        chkShowInactive.AutoSize = true;
        chkShowInactive.FlatStyle = FlatStyle.Flat;
        chkShowInactive.Location = new Point(1245, 64);
        chkShowInactive.Margin = new Padding(0);
        chkShowInactive.Name = "chkShowInactive";
        chkShowInactive.Size = new Size(170, 28);
        chkShowInactive.TabIndex = 5;
        chkShowInactive.Text = "عرض غير النشط";
        chkShowInactive.UseVisualStyleBackColor = true;
        chkShowInactive.CheckedChanged += chkShowInactive_CheckedChanged;
        // 
        // chkOnlyWithBalance
        // 
        chkOnlyWithBalance.AutoSize = true;
        chkOnlyWithBalance.FlatStyle = FlatStyle.Flat;
        chkOnlyWithBalance.Location = new Point(1040, 64);
        chkOnlyWithBalance.Margin = new Padding(0);
        chkOnlyWithBalance.Name = "chkOnlyWithBalance";
        chkOnlyWithBalance.Size = new Size(195, 28);
        chkOnlyWithBalance.TabIndex = 6;
        chkOnlyWithBalance.Text = "له رصيد مستحق فقط";
        chkOnlyWithBalance.CheckedChanged += chkOnlyWithBalance_CheckedChanged;
        // 
        // chkOnlyOverLimit
        // 
        chkOnlyOverLimit.AutoSize = true;
        chkOnlyOverLimit.FlatStyle = FlatStyle.Flat;
        chkOnlyOverLimit.Location = new Point(860, 64);
        chkOnlyOverLimit.Margin = new Padding(0);
        chkOnlyOverLimit.Name = "chkOnlyOverLimit";
        chkOnlyOverLimit.Size = new Size(170, 28);
        chkOnlyOverLimit.TabIndex = 7;
        chkOnlyOverLimit.Text = "تجاوز حد الائتمان";
        chkOnlyOverLimit.CheckedChanged += chkOnlyOverLimit_CheckedChanged;
        // 
        // pnlSummary
        // 
        pnlSummary.Controls.Add(lblSummary);
        pnlSummary.Dock = DockStyle.Bottom;
        pnlSummary.Location = new Point(0, 788);
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
        pnlGrid.Controls.Add(dgvSuppliers);
        pnlGrid.Dock = DockStyle.Fill;
        pnlGrid.Location = new Point(0, 442);
        pnlGrid.Name = "pnlGrid";
        pnlGrid.Padding = new Padding(0, 10, 0, 0);
        pnlGrid.Size = new Size(1432, 346);
        pnlGrid.TabIndex = 4;
        // 
        // dgvSuppliers
        // 
        dgvSuppliers.AllowUserToAddRows = false;
        dgvSuppliers.AllowUserToDeleteRows = false;
        dgvSuppliers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvSuppliers.BackgroundColor = Color.FromArgb(220, 240, 220);
        dgvSuppliers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvSuppliers.Dock = DockStyle.Fill;
        dgvSuppliers.Location = new Point(0, 10);
        dgvSuppliers.MultiSelect = false;
        dgvSuppliers.Name = "dgvSuppliers";
        dgvSuppliers.ReadOnly = true;
        dgvSuppliers.RowHeadersVisible = false;
        dgvSuppliers.RowHeadersWidth = 62;
        dgvSuppliers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvSuppliers.Size = new Size(1432, 336);
        dgvSuppliers.TabIndex = 0;
        dgvSuppliers.CellClick += dgvSuppliers_CellClick;
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
        ClientSize = new Size(1432, 820);
        Controls.Add(pnlGrid);
        Controls.Add(pnlSummary);
        Controls.Add(pnlFilters);
        Controls.Add(pnlButtons);
        Controls.Add(pnlFields);
        Controls.Add(lblMode);
        Font = new Font("Tahoma", 10F);
        KeyPreview = true;
        MinimumSize = new Size(1200, 700);
        Name = "frmSupplierMaster";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterParent;
        Text = "دليل الموردين";
        Load += frmSupplierMaster_Load;
        pnlFields.ResumeLayout(false);
        pnlFields.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numCreditLimit).EndInit();
        pnlButtons.ResumeLayout(false);
        pnlFilters.ResumeLayout(false);
        gbFilter.ResumeLayout(false);
        gbFilter.PerformLayout();
        pnlSummary.ResumeLayout(false);
        pnlGrid.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvSuppliers).EndInit();
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        ResumeLayout(false);
    }

    private System.Windows.Forms.Label lblMode;
    private System.Windows.Forms.Panel pnlFields;
    private System.Windows.Forms.Label lblId;
    private System.Windows.Forms.TextBox txtId;
    private System.Windows.Forms.Label lblName;
    private System.Windows.Forms.Label lblNameReq;
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
    private System.Windows.Forms.Button btnEdit;
    private System.Windows.Forms.Button btnDelete;
    private System.Windows.Forms.Button btnRefresh;
    private System.Windows.Forms.Button btnClose;
    private System.Windows.Forms.Panel pnlFilters;
    private System.Windows.Forms.GroupBox gbFilter;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.ComboBox cbFilterBy;
    private System.Windows.Forms.TextBox txtSearch;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ComboBox cbStatus;
    private System.Windows.Forms.CheckBox chkShowInactive;
    private System.Windows.Forms.CheckBox chkOnlyWithBalance;
    private System.Windows.Forms.CheckBox chkOnlyOverLimit;
    private System.Windows.Forms.Panel pnlSummary;
    private System.Windows.Forms.Label lblSummary;
    private System.Windows.Forms.Panel pnlGrid;
    private System.Windows.Forms.DataGridView dgvSuppliers;
    private System.Windows.Forms.ErrorProvider errorProvider;
}
