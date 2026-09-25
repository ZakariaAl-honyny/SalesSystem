namespace SalesSystem.Desktop.Screens.CashBoxes.Forms;

partial class frmListTreasuryTransfers
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        components?.Dispose();
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        cmsTransfers = new ContextMenuStrip(components);
        viewDetailsToolStripMenuItem = new ToolStripMenuItem();
        refreshToolStripMenuItem = new ToolStripMenuItem();
        dgvTransfers = new DataGridView();
        lblMode = new Label();
        label1 = new Label();
        lblRecordsCount = new Label();
        txtSearch = new TextBox();
        btnAddNew = new Button();
        btnRefresh = new Button();
        btnClose = new Button();
        cmsTransfers.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvTransfers).BeginInit();
        SuspendLayout();
        // 
        // cmsTransfers
        // 
        cmsTransfers.BackColor = Color.White;
        cmsTransfers.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        cmsTransfers.ImageScalingSize = new Size(24, 24);
        cmsTransfers.Items.AddRange(new ToolStripItem[] { viewDetailsToolStripMenuItem, refreshToolStripMenuItem });
        cmsTransfers.Name = "cmsTransfers";
        cmsTransfers.RightToLeft = RightToLeft.Yes;
        cmsTransfers.Size = new Size(200, 64);
        cmsTransfers.Opening += cmsTransfers_Opening;
        // 
        // viewDetailsToolStripMenuItem
        // 
        viewDetailsToolStripMenuItem.Name = "viewDetailsToolStripMenuItem";
        viewDetailsToolStripMenuItem.Size = new Size(199, 30);
        viewDetailsToolStripMenuItem.Text = "عرض التفاصيل";
        viewDetailsToolStripMenuItem.Click += viewDetailsToolStripMenuItem_Click;
        // 
        // refreshToolStripMenuItem
        // 
        refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
        refreshToolStripMenuItem.Size = new Size(199, 30);
        refreshToolStripMenuItem.Text = "تحديث";
        refreshToolStripMenuItem.Click += refreshToolStripMenuItem_Click;
        // 
        // dgvTransfers
        // 
        dgvTransfers.AllowUserToAddRows = false;
        dgvTransfers.AllowUserToDeleteRows = false;
        dgvTransfers.BackgroundColor = SystemColors.Window;
        dgvTransfers.BorderStyle = BorderStyle.Fixed3D;
        dgvTransfers.ColumnHeadersHeight = 50;
        dgvTransfers.ContextMenuStrip = cmsTransfers;
        dgvTransfers.Location = new Point(12, 160);
        dgvTransfers.Margin = new Padding(4);
        dgvTransfers.MultiSelect = false;
        dgvTransfers.Name = "dgvTransfers";
        dgvTransfers.ReadOnly = true;
        dgvTransfers.RowHeadersWidth = 62;
        dgvTransfers.RowTemplate.Height = 28;
        dgvTransfers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvTransfers.Size = new Size(1176, 480);
        dgvTransfers.TabIndex = 0;
        dgvTransfers.TabStop = false;
        dgvTransfers.DoubleClick += dgvTransfers_DoubleClick;
        // 
        // lblMode
        // 
        lblMode.Anchor = AnchorStyles.Top;
        lblMode.AutoSize = true;
        lblMode.BackColor = Color.WhiteSmoke;
        lblMode.FlatStyle = FlatStyle.Flat;
        lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblMode.ForeColor = Color.Firebrick;
        lblMode.Location = new Point(420, 20);
        lblMode.Margin = new Padding(4, 0, 4, 0);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(360, 39);
        lblMode.TabIndex = 30;
        lblMode.Text = "إدارة تحويلات الخزينة";
        // 
        // label1
        // 
        label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        label1.AutoSize = true;
        label1.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label1.Location = new Point(1050, 655);
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
        lblRecordsCount.Location = new Point(980, 655);
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
        txtSearch.PlaceholderText = "بحث برقم التحويل، الاسم، أو الملاحظات...";
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
        btnClose.Location = new Point(12, 640);
        btnClose.Margin = new Padding(4);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(181, 55);
        btnClose.TabIndex = 0;
        btnClose.Text = "إغلاق";
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += btnClose_Click;
        // 
        // frmListTreasuryTransfers
        // 
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.WhiteSmoke;
        CancelButton = btnClose;
        ClientSize = new Size(1200, 710);
        Controls.Add(dgvTransfers);
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
        Name = "frmListTreasuryTransfers";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "إدارة تحويلات الخزينة";
        Load += frmListTreasuryTransfers_Load;
        cmsTransfers.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvTransfers).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private DataGridView dgvTransfers;
    private Label lblMode;
    private Label label1;
    private Label lblRecordsCount;
    private TextBox txtSearch;
    private Button btnAddNew;
    private Button btnRefresh;
    private Button btnClose;
    private ContextMenuStrip cmsTransfers;
    private ToolStripMenuItem viewDetailsToolStripMenuItem;
    private ToolStripMenuItem refreshToolStripMenuItem;
}
