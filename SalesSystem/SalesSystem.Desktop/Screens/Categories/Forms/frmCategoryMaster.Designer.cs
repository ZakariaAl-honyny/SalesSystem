namespace SalesSystem.Desktop.Screens.Categories.Forms;

partial class frmCategoryMaster
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
        pnlButtons = new FlowLayoutPanel();
        btnNew = new Button();
        btnSave = new Button();
        btnEdit = new Button();
        btnDelete = new Button();
        btnClose = new Button();
        pnlFilters = new Panel();
        gbFilter = new GroupBox();
        label2 = new Label();
        cbFilterBy = new ComboBox();
        lblSearch = new Label();
        txtSearch = new TextBox();
        label1 = new Label();
        cbStatus = new ComboBox();
        chkShowInactive = new CheckBox();
        btnRefresh = new Button();
        pnlSummary = new Panel();
        lblSummary = new Label();
        pnlGrid = new Panel();
        dgvCategories = new DataGridView();
        colId = new DataGridViewTextBoxColumn();
        colName = new DataGridViewTextBoxColumn();
        colDescription = new DataGridViewTextBoxColumn();
        colIsActive = new DataGridViewTextBoxColumn();
        errorProvider = new ErrorProvider(components);
        pnlFields.SuspendLayout();
        pnlButtons.SuspendLayout();
        pnlFilters.SuspendLayout();
        gbFilter.SuspendLayout();
        pnlSummary.SuspendLayout();
        pnlGrid.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvCategories).BeginInit();
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
        lblMode.Size = new Size(940, 50);
        lblMode.TabIndex = 0;
        lblMode.Text = "(( دليل مجموعات الأصناف ))";
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
        pnlFields.Dock = DockStyle.Top;
        pnlFields.Location = new Point(0, 50);
        pnlFields.Name = "pnlFields";
        pnlFields.Size = new Size(940, 100);
        pnlFields.TabIndex = 1;
        // 
        // lblCode
        // 
        lblCode.AutoSize = true;
        lblCode.Location = new Point(740, 14);
        lblCode.Name = "lblCode";
        lblCode.Size = new Size(126, 24);
        lblCode.TabIndex = 0;
        lblCode.Text = "رقم المجموعة";
        // 
        // txtCode
        // 
        txtCode.Location = new Point(740, 41);
        txtCode.Name = "txtCode";
        txtCode.ReadOnly = true;
        txtCode.Size = new Size(130, 32);
        txtCode.TabIndex = 0;
        txtCode.TabStop = false;
        // 
        // lblName
        // 
        lblName.AutoSize = true;
        lblName.Location = new Point(560, 14);
        lblName.Name = "lblName";
        lblName.Size = new Size(136, 24);
        lblName.TabIndex = 2;
        lblName.Text = "اسم المجموعة";
        // 
        // lblNameReq
        // 
        lblNameReq.AutoSize = true;
        lblNameReq.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblNameReq.ForeColor = Color.Red;
        lblNameReq.Location = new Point(542, 14);
        lblNameReq.Name = "lblNameReq";
        lblNameReq.Size = new Size(23, 24);
        lblNameReq.TabIndex = 3;
        lblNameReq.Text = "*";
        // 
        // txtName
        // 
        txtName.Location = new Point(420, 41);
        txtName.Name = "txtName";
        txtName.Size = new Size(290, 32);
        txtName.TabIndex = 1;
        // 
        // lblDescription
        // 
        lblDescription.AutoSize = true;
        lblDescription.Location = new Point(330, 14);
        lblDescription.Name = "lblDescription";
        lblDescription.Size = new Size(63, 24);
        lblDescription.TabIndex = 4;
        lblDescription.Text = "الوصف";
        // 
        // txtDescription
        // 
        txtDescription.Location = new Point(60, 41);
        txtDescription.Name = "txtDescription";
        txtDescription.Size = new Size(330, 32);
        txtDescription.TabIndex = 2;
        // 
        // pnlButtons
        // 
        pnlButtons.BackColor = Color.FromArgb(235, 240, 235);
        pnlButtons.Controls.Add(btnNew);
        pnlButtons.Controls.Add(btnSave);
        pnlButtons.Controls.Add(btnEdit);
        pnlButtons.Controls.Add(btnDelete);
        pnlButtons.Controls.Add(btnClose);
        pnlButtons.Dock = DockStyle.Top;
        pnlButtons.Location = new Point(0, 150);
        pnlButtons.Name = "pnlButtons";
        pnlButtons.Padding = new Padding(10);
        pnlButtons.Size = new Size(940, 60);
        pnlButtons.TabIndex = 2;
        // 
        // btnNew
        // 
        btnNew.BackColor = Color.FromArgb(40, 167, 69);
        btnNew.FlatStyle = FlatStyle.Flat;
        btnNew.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnNew.ForeColor = Color.White;
        btnNew.Location = new Point(775, 13);
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
        btnSave.Location = new Point(641, 13);
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
        btnEdit.Location = new Point(499, 13);
        btnEdit.Name = "btnEdit";
        btnEdit.Size = new Size(128, 35);
        btnEdit.TabIndex = 3;
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
        btnDelete.Location = new Point(357, 13);
        btnDelete.Name = "btnDelete";
        btnDelete.Size = new Size(138, 35);
        btnDelete.TabIndex = 2;
        btnDelete.Text = "🗑 حذف";
        btnDelete.UseVisualStyleBackColor = false;
        btnDelete.Click += btnDelete_Click;
        // 
        // btnClose
        // 
        btnClose.BackColor = Color.FromArgb(108, 117, 125);
        btnClose.FlatStyle = FlatStyle.Flat;
        btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnClose.ForeColor = Color.White;
        btnClose.Location = new Point(215, 13);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(130, 35);
        btnClose.TabIndex = 4;
        btnClose.TabStop = false;
        btnClose.Text = "🚪 خروج";
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += btnClose_Click;
        // 
        // pnlFilters
        // 
        pnlFilters.Controls.Add(gbFilter);
        pnlFilters.Dock = DockStyle.Top;
        pnlFilters.Location = new Point(0, 210);
        pnlFilters.Margin = new Padding(0);
        pnlFilters.Name = "pnlFilters";
        pnlFilters.Padding = new Padding(6, 4, 6, 4);
        pnlFilters.Size = new Size(940, 112);
        pnlFilters.TabIndex = 3;
        // 
        // gbFilter
        // 
        gbFilter.Controls.Add(label2);
        gbFilter.Controls.Add(cbFilterBy);
        gbFilter.Controls.Add(lblSearch);
        gbFilter.Controls.Add(txtSearch);
        gbFilter.Controls.Add(label1);
        gbFilter.Controls.Add(cbStatus);
        gbFilter.Controls.Add(chkShowInactive);
        gbFilter.Controls.Add(btnRefresh);
        gbFilter.Dock = DockStyle.Fill;
        gbFilter.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        gbFilter.Location = new Point(6, 4);
        gbFilter.Margin = new Padding(0);
        gbFilter.Name = "gbFilter";
        gbFilter.Padding = new Padding(8);
        gbFilter.RightToLeft = RightToLeft.Yes;
        gbFilter.Size = new Size(928, 104);
        gbFilter.TabIndex = 0;
        gbFilter.TabStop = false;
        gbFilter.Text = "بحث وتصفية";
        // 
        // label2
        // 
        label2.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        label2.Location = new Point(822, 26);
        label2.Margin = new Padding(0);
        label2.Name = "label2";
        label2.Size = new Size(96, 22);
        label2.TabIndex = 0;
        label2.Text = "تصفية بـ:";
        label2.TextAlign = ContentAlignment.MiddleRight;
        // 
        // cbFilterBy
        // 
        cbFilterBy.DropDownStyle = ComboBoxStyle.DropDownList;
        cbFilterBy.FlatStyle = FlatStyle.Flat;
        cbFilterBy.Font = new Font("Tahoma", 9F);
        cbFilterBy.FormattingEnabled = true;
        cbFilterBy.Items.AddRange(new object[] { "الاسم", "الوصف", "رقم المجموعة" });
        cbFilterBy.Location = new Point(686, 22);
        cbFilterBy.Margin = new Padding(0);
        cbFilterBy.Name = "cbFilterBy";
        cbFilterBy.Size = new Size(130, 26);
        cbFilterBy.TabIndex = 1;
        cbFilterBy.SelectedIndexChanged += cbFilterBy_SelectedIndexChanged;
        // 
        // lblSearch
        // 
        lblSearch.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblSearch.Location = new Point(624, 26);
        lblSearch.Margin = new Padding(0);
        lblSearch.Name = "lblSearch";
        lblSearch.Size = new Size(56, 22);
        lblSearch.TabIndex = 2;
        lblSearch.Text = "بحث:";
        lblSearch.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtSearch
        // 
        txtSearch.Font = new Font("Tahoma", 9F);
        txtSearch.Location = new Point(388, 22);
        txtSearch.Margin = new Padding(0);
        txtSearch.Name = "txtSearch";
        txtSearch.PlaceholderText = "اكتب قيمة التصفية...";
        txtSearch.Size = new Size(230, 26);
        txtSearch.TabIndex = 3;
        txtSearch.TextChanged += txtSearch_TextChanged;
        txtSearch.KeyPress += txtSearch_KeyPress;
        txtSearch.KeyDown += txtSearch_KeyDown;
        // 
        // label1
        // 
        label1.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        label1.Location = new Point(302, 26);
        label1.Margin = new Padding(0);
        label1.Name = "label1";
        label1.Size = new Size(80, 22);
        label1.TabIndex = 4;
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
        cbStatus.Location = new Point(186, 22);
        cbStatus.Margin = new Padding(0);
        cbStatus.Name = "cbStatus";
        cbStatus.Size = new Size(110, 26);
        cbStatus.TabIndex = 5;
        cbStatus.SelectedIndexChanged += cbStatus_SelectedIndexChanged;
        // 
        // chkShowInactive
        // 
        chkShowInactive.AutoSize = true;
        chkShowInactive.FlatStyle = FlatStyle.Flat;
        chkShowInactive.Font = new Font("Tahoma", 9F);
        chkShowInactive.Location = new Point(748, 62);
        chkShowInactive.Margin = new Padding(0);
        chkShowInactive.Name = "chkShowInactive";
        chkShowInactive.Size = new Size(166, 24);
        chkShowInactive.TabIndex = 6;
        chkShowInactive.Text = "عرض غير النشط";
        chkShowInactive.UseVisualStyleBackColor = true;
        chkShowInactive.CheckedChanged += chkShowInactive_CheckedChanged;
        // 
        // btnRefresh
        // 
        btnRefresh.BackColor = Color.FromArgb(23, 162, 184);
        btnRefresh.FlatStyle = FlatStyle.Flat;
        btnRefresh.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnRefresh.ForeColor = Color.White;
        btnRefresh.Location = new Point(24, 58);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(130, 32);
        btnRefresh.TabIndex = 7;
        btnRefresh.Text = "🔄 تحديث";
        btnRefresh.UseVisualStyleBackColor = false;
        btnRefresh.Click += btnRefresh_Click;
        // 
        // pnlSummary
        // 
        pnlSummary.Controls.Add(lblSummary);
        pnlSummary.Dock = DockStyle.Bottom;
        pnlSummary.Location = new Point(0, 668);
        pnlSummary.Margin = new Padding(0);
        pnlSummary.Name = "pnlSummary";
        pnlSummary.Size = new Size(940, 32);
        pnlSummary.TabIndex = 5;
        // 
        // lblSummary
        // 
        lblSummary.Dock = DockStyle.Fill;
        lblSummary.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblSummary.Location = new Point(0, 0);
        lblSummary.Name = "lblSummary";
        lblSummary.Size = new Size(940, 32);
        lblSummary.TabIndex = 0;
        lblSummary.Text = "المعروض: 0 من 0";
        lblSummary.TextAlign = ContentAlignment.MiddleRight;
        // 
        // pnlGrid
        // 
        pnlGrid.Controls.Add(dgvCategories);
        pnlGrid.Dock = DockStyle.Fill;
        pnlGrid.Location = new Point(0, 322);
        pnlGrid.Name = "pnlGrid";
        pnlGrid.Padding = new Padding(10);
        pnlGrid.Size = new Size(940, 346);
        pnlGrid.TabIndex = 4;
        // 
        // dgvCategories
        // 
        dgvCategories.AllowUserToAddRows = false;
        dgvCategories.AllowUserToDeleteRows = false;
        dgvCategories.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvCategories.BackgroundColor = Color.FromArgb(220, 240, 220);
        dgvCategories.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvCategories.Columns.AddRange(new DataGridViewColumn[] { colId, colName, colDescription, colIsActive });
        dgvCategories.Dock = DockStyle.Fill;
        dgvCategories.Location = new Point(10, 10);
        dgvCategories.MultiSelect = false;
        dgvCategories.Name = "dgvCategories";
        dgvCategories.ReadOnly = true;
        dgvCategories.RowHeadersVisible = false;
        dgvCategories.RowHeadersWidth = 62;
        dgvCategories.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvCategories.Size = new Size(920, 326);
        dgvCategories.TabIndex = 0;
        dgvCategories.CellClick += dgvCategories_CellClick;
        // 
        // colId
        // 
        colId.DataPropertyName = "Id";
        colId.FillWeight = 8F;
        colId.HeaderText = "رقم المجموعة";
        colId.MinimumWidth = 80;
        colId.Name = "colId";
        colId.ReadOnly = true;
        // 
        // colName
        // 
        colName.DataPropertyName = "Name";
        colName.FillWeight = 30F;
        colName.HeaderText = "اسم المجموعة";
        colName.MinimumWidth = 200;
        colName.Name = "colName";
        colName.ReadOnly = true;
        // 
        // colDescription
        // 
        colDescription.DataPropertyName = "Description";
        colDescription.FillWeight = 45F;
        colDescription.HeaderText = "الوصف";
        colDescription.MinimumWidth = 250;
        colDescription.Name = "colDescription";
        colDescription.ReadOnly = true;
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
        // frmCategoryMaster
        // 
        AutoScaleDimensions = new SizeF(11F, 24F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(170, 220, 170);
        ClientSize = new Size(940, 700);
        Controls.Add(pnlGrid);
        Controls.Add(pnlSummary);
        Controls.Add(pnlFilters);
        Controls.Add(pnlButtons);
        Controls.Add(pnlFields);
        Controls.Add(lblMode);
        Font = new Font("Tahoma", 10F);
        Name = "frmCategoryMaster";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterParent;
        Text = "دليل مجموعات الأصناف";
        Load += frmCategoryMaster_Load;
        pnlFields.ResumeLayout(false);
        pnlFields.PerformLayout();
        pnlButtons.ResumeLayout(false);
        pnlFilters.ResumeLayout(false);
        gbFilter.ResumeLayout(false);
        gbFilter.PerformLayout();
        pnlSummary.ResumeLayout(false);
        pnlGrid.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvCategories).EndInit();
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
    private System.Windows.Forms.FlowLayoutPanel pnlButtons;
    private System.Windows.Forms.Button btnNew;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.Button btnEdit;
    private System.Windows.Forms.Button btnDelete;
    private System.Windows.Forms.Button btnClose;
    private System.Windows.Forms.Panel pnlFilters;
    private System.Windows.Forms.GroupBox gbFilter;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.ComboBox cbFilterBy;
    private System.Windows.Forms.Label lblSearch;
    private System.Windows.Forms.TextBox txtSearch;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ComboBox cbStatus;
    private System.Windows.Forms.CheckBox chkShowInactive;
    private System.Windows.Forms.Button btnRefresh;
    private System.Windows.Forms.Panel pnlSummary;
    private System.Windows.Forms.Label lblSummary;
    private System.Windows.Forms.Panel pnlGrid;
    private System.Windows.Forms.DataGridView dgvCategories;
    private System.Windows.Forms.DataGridViewTextBoxColumn colId;
    private System.Windows.Forms.DataGridViewTextBoxColumn colName;
    private System.Windows.Forms.DataGridViewTextBoxColumn colDescription;
    private System.Windows.Forms.DataGridViewTextBoxColumn colIsActive;
    private System.Windows.Forms.ErrorProvider errorProvider;
}
