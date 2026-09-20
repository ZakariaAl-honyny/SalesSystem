namespace SalesSystem.Desktop.Screens.Banks.Forms;

partial class frmListBanks
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
        cmsBanks = new ContextMenuStrip(components);
        editToolStripMenuItem = new ToolStripMenuItem();
        deactivateToolStripMenuItem = new ToolStripMenuItem();
        dgvBanks = new DataGridView();
        lblMode = new Label();
        label1 = new Label();
        lblRecordsCount = new Label();
        txtSearch = new TextBox();
        btnAddNew = new Button();
        btnRefresh = new Button();
        btnClose = new Button();
        cmsBanks.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvBanks).BeginInit();
        SuspendLayout();
        // 
        // cmsBanks
        // 
        cmsBanks.BackColor = Color.White;
        cmsBanks.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        cmsBanks.ImageScalingSize = new Size(24, 24);
        cmsBanks.Items.AddRange(new ToolStripItem[] { editToolStripMenuItem, deactivateToolStripMenuItem });
        cmsBanks.Name = "cmsBanks";
        cmsBanks.RightToLeft = RightToLeft.Yes;
        cmsBanks.Size = new Size(220, 70);
        cmsBanks.Opening += cmsBanks_Opening;
        // 
        // editToolStripMenuItem
        // 
        editToolStripMenuItem.Name = "editToolStripMenuItem";
        editToolStripMenuItem.Size = new Size(219, 30);
        editToolStripMenuItem.Text = "تعديل البنك";
        editToolStripMenuItem.Click += editToolStripMenuItem_Click;
        // 
        // deactivateToolStripMenuItem
        // 
        deactivateToolStripMenuItem.Name = "deactivateToolStripMenuItem";
        deactivateToolStripMenuItem.Size = new Size(219, 30);
        deactivateToolStripMenuItem.Text = "إلغاء التنشيط";
        deactivateToolStripMenuItem.Click += deactivateToolStripMenuItem_Click;
        // 
        // dgvBanks
        // 
        dgvBanks.AllowUserToAddRows = false;
        dgvBanks.AllowUserToDeleteRows = false;
        dgvBanks.BackgroundColor = SystemColors.Window;
        dgvBanks.BorderStyle = BorderStyle.Fixed3D;
        dgvBanks.ColumnHeadersHeight = 50;
        dgvBanks.ContextMenuStrip = cmsBanks;
        dgvBanks.Location = new Point(12, 160);
        dgvBanks.Margin = new Padding(4);
        dgvBanks.Name = "dgvBanks";
        dgvBanks.ReadOnly = true;
        dgvBanks.RowHeadersWidth = 62;
        dgvBanks.RowTemplate.Height = 28;
        dgvBanks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvBanks.Size = new Size(1176, 500);
        dgvBanks.TabIndex = 0;
        dgvBanks.TabStop = false;
        dgvBanks.DoubleClick += dgvBanks_DoubleClick;
        // 
        // lblMode
        // 
        lblMode.Anchor = AnchorStyles.Top;
        lblMode.AutoSize = true;
        lblMode.BackColor = Color.WhiteSmoke;
        lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblMode.ForeColor = Color.Firebrick;
        lblMode.Location = new Point(390, 20);
        lblMode.Margin = new Padding(4, 0, 4, 0);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(198, 39);
        lblMode.TabIndex = 30;
        lblMode.Text = "إدارة البنوك";
        // 
        // label1
        // 
        label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        label1.AutoSize = true;
        label1.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label1.Location = new Point(810, 580);
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
        lblRecordsCount.Location = new Point(690, 580);
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
        txtSearch.Location = new Point(480, 88);
        txtSearch.Margin = new Padding(4);
        txtSearch.Name = "txtSearch";
        txtSearch.RightToLeft = RightToLeft.Yes;
        txtSearch.Size = new Size(400, 32);
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
        // frmListBanks
        // 
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.WhiteSmoke;
        CancelButton = btnClose;
        ClientSize = new Size(1200, 723);
        Controls.Add(dgvBanks);
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
        Name = "frmListBanks";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "إدارة البنوك";
        Load += frmListBanks_Load;
        cmsBanks.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvBanks).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private DataGridView dgvBanks;
    private Label lblMode;
    private Label label1;
    private Label lblRecordsCount;
    private TextBox txtSearch;
    private Button btnAddNew;
    private Button btnRefresh;
    private Button btnClose;
    private ContextMenuStrip cmsBanks;
    private ToolStripMenuItem editToolStripMenuItem;
    private ToolStripMenuItem deactivateToolStripMenuItem;
}
