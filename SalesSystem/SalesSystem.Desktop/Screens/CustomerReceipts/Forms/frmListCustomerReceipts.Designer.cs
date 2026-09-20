namespace SalesSystem.Desktop.CustomerReceipts.Forms;

partial class frmListCustomerReceipts
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        components?.Dispose();base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        cmsReceipts = new ContextMenuStrip(components);
        editToolStripMenuItem = new ToolStripMenuItem();
        postToolStripMenuItem = new ToolStripMenuItem();
        cancelToolStripMenuItem = new ToolStripMenuItem();
        dgvReceipts = new DataGridView();
        lblMode = new Label();
        label1 = new Label();
        lblRecordsCount = new Label();
        txtSearch = new TextBox();
        btnAddNew = new Button();
        btnRefresh = new Button();
        btnClose = new Button();
        cmsReceipts.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvReceipts).BeginInit();
        SuspendLayout();
        // 
        // cmsReceipts
        // 
        cmsReceipts.BackColor = Color.White;
        cmsReceipts.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        cmsReceipts.ImageScalingSize = new Size(24, 24);
        cmsReceipts.Items.AddRange(new ToolStripItem[] { editToolStripMenuItem, postToolStripMenuItem, cancelToolStripMenuItem });
        cmsReceipts.Name = "cmsReceipts";
        cmsReceipts.RightToLeft = RightToLeft.Yes;
        cmsReceipts.Size = new Size(220, 98);
        cmsReceipts.Opening += cmsReceipts_Opening;
        // 
        // editToolStripMenuItem
        // 
        editToolStripMenuItem.Name = "editToolStripMenuItem";
        editToolStripMenuItem.Size = new Size(219, 30);
        editToolStripMenuItem.Text = "تعديل السند";
        editToolStripMenuItem.Click += editToolStripMenuItem_Click;
        // 
        // postToolStripMenuItem
        // 
        postToolStripMenuItem.Name = "postToolStripMenuItem";
        postToolStripMenuItem.Size = new Size(219, 30);
        postToolStripMenuItem.Text = "ترحيل";
        postToolStripMenuItem.Click += postToolStripMenuItem_Click;
        // 
        // cancelToolStripMenuItem
        // 
        cancelToolStripMenuItem.Name = "cancelToolStripMenuItem";
        cancelToolStripMenuItem.Size = new Size(219, 30);
        cancelToolStripMenuItem.Text = "إلغاء الترحيل";
        cancelToolStripMenuItem.Click += cancelToolStripMenuItem_Click;
        // 
        // dgvReceipts
        // 
        dgvReceipts.AllowUserToAddRows = false;
        dgvReceipts.AllowUserToDeleteRows = false;
        dgvReceipts.BackgroundColor = SystemColors.Window;
        dgvReceipts.BorderStyle = BorderStyle.Fixed3D;
        dgvReceipts.ColumnHeadersHeight = 50;
        dgvReceipts.ContextMenuStrip = cmsReceipts;
        dgvReceipts.Location = new Point(12, 160);
        dgvReceipts.Margin = new Padding(4);
        dgvReceipts.MultiSelect = false;
        dgvReceipts.Name = "dgvReceipts";
        dgvReceipts.ReadOnly = true;
        dgvReceipts.RowHeadersWidth = 62;
        dgvReceipts.RowTemplate.Height = 28;
        dgvReceipts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvReceipts.Size = new Size(1176, 500);
        dgvReceipts.TabIndex = 0;
        dgvReceipts.TabStop = false;
        dgvReceipts.DoubleClick += dgvReceipts_DoubleClick;
        // 
        // lblMode
        // 
        lblMode.Anchor = AnchorStyles.Top;
        lblMode.AutoSize = true;
        lblMode.BackColor = Color.WhiteSmoke;
        lblMode.FlatStyle = FlatStyle.Flat;
        lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblMode.ForeColor = Color.Firebrick;
        lblMode.Location = new Point(380, 20);
        lblMode.Margin = new Padding(4, 0, 4, 0);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(298, 39);
        lblMode.TabIndex = 30;
        lblMode.Text = "إدارة سندات القبض";
        // 
        // label1
        // 
        label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        label1.AutoSize = true;
        label1.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label1.Location = new Point(1050, 670);
        label1.Margin = new Padding(4, 0, 4, 0);
        label1.Name = "label1";
        label1.Size = new Size(115, 24);
        label1.TabIndex = 3;
        label1.Text = "عدد السجلات:";
        // 
        // lblRecordsCount
        // 
        lblRecordsCount.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        lblRecordsCount.AutoSize = true;
        lblRecordsCount.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblRecordsCount.Location = new Point(980, 670);
        lblRecordsCount.Margin = new Padding(4, 0, 4, 0);
        lblRecordsCount.Name = "lblRecordsCount";
        lblRecordsCount.Size = new Size(40, 24);
        lblRecordsCount.TabIndex = 4;
        lblRecordsCount.Text = "[0]";
        // 
        // txtSearch
        // 
        txtSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        txtSearch.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        txtSearch.Location = new Point(470, 112);
        txtSearch.Margin = new Padding(4);
        txtSearch.Name = "txtSearch";
        txtSearch.RightToLeft = RightToLeft.Yes;
        txtSearch.Size = new Size(500, 32);
        txtSearch.TabIndex = 1;
        txtSearch.TextChanged += txtSearch_TextChanged;
        // 
        // btnAddNew
        // 
        btnAddNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnAddNew.BackColor = Color.White;
        btnAddNew.FlatAppearance.BorderColor = Color.Black;
        btnAddNew.FlatAppearance.BorderSize = 2;
        btnAddNew.FlatStyle = FlatStyle.Popup;
        btnAddNew.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnAddNew.Location = new Point(1103, 80);
        btnAddNew.Margin = new Padding(4);
        btnAddNew.Name = "btnAddNew";
        btnAddNew.Size = new Size(75, 75);
        btnAddNew.TabIndex = 5;
        btnAddNew.Text = "+ جديد";
        btnAddNew.UseVisualStyleBackColor = false;
        btnAddNew.Click += btnAddNew_Click;
        // 
        // btnRefresh
        // 
        btnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnRefresh.BackColor = Color.White;
        btnRefresh.FlatAppearance.BorderColor = Color.Black;
        btnRefresh.FlatAppearance.BorderSize = 2;
        btnRefresh.FlatStyle = FlatStyle.Popup;
        btnRefresh.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnRefresh.Location = new Point(1010, 80);
        btnRefresh.Margin = new Padding(4);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(75, 75);
        btnRefresh.TabIndex = 6;
        btnRefresh.Text = "تحديث";
        btnRefresh.UseVisualStyleBackColor = false;
        btnRefresh.Click += btnRefresh_Click;
        // 
        // btnClose
        // 
        btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnClose.BackColor = Color.White;
        btnClose.DialogResult = DialogResult.Cancel;
        btnClose.FlatStyle = FlatStyle.Popup;
        btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnClose.Location = new Point(12, 650);
        btnClose.Margin = new Padding(4);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(181, 61);
        btnClose.TabIndex = 0;
        btnClose.Text = "إغلاق";
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += btnClose_Click;
        // 
        // frmListCustomerReceipts
        // 
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.WhiteSmoke;
        CancelButton = btnClose;
        ClientSize = new Size(1200, 723);
        Controls.Add(dgvReceipts);
        Controls.Add(lblMode);
        Controls.Add(txtSearch);
        Controls.Add(label1);
        Controls.Add(lblRecordsCount);
        Controls.Add(btnAddNew);
        Controls.Add(btnRefresh);
        Controls.Add(btnClose);
        Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        Name = "frmListCustomerReceipts";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "إدارة سندات القبض";
        Load += frmListCustomerReceipts_Load;
        cmsReceipts.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvReceipts).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private DataGridView dgvReceipts;
    private Label lblMode;
    private Label label1;
    private Label lblRecordsCount;
    private TextBox txtSearch;
    private Button btnAddNew;
    private Button btnRefresh;
    private Button btnClose;
    private ContextMenuStrip cmsReceipts;
    private ToolStripMenuItem editToolStripMenuItem;
    private ToolStripMenuItem postToolStripMenuItem;
    private ToolStripMenuItem cancelToolStripMenuItem;
}
