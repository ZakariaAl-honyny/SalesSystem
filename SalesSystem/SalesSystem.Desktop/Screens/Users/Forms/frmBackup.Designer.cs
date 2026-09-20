namespace SalesSystem.Desktop.Screens.Users.Forms;

partial class frmBackup
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
        lblMode = new Label();
        dgvBackups = new DataGridView();
        cmsBackups = new ContextMenuStrip(components);
        refreshToolStripMenuItem = new ToolStripMenuItem();
        restoreToolStripMenuItem = new ToolStripMenuItem();
        label1 = new Label();
        lblRecordsCount = new Label();
        btnCreateBackup = new Button();
        btnRestoreBackup = new Button();
        btnRefresh = new Button();
        btnClose = new Button();
        ((System.ComponentModel.ISupportInitialize)dgvBackups).BeginInit();
        cmsBackups.SuspendLayout();
        SuspendLayout();
        // 
        // lblMode
        // 
        lblMode.Anchor = AnchorStyles.Top;
        lblMode.AutoSize = true;
        lblMode.BackColor = Color.WhiteSmoke;
        lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblMode.ForeColor = Color.Firebrick;
        lblMode.Location = new Point(320, 15);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(274, 39);
        lblMode.TabIndex = 30;
        lblMode.Text = "النسخ الاحتياطي";
        // 
        // dgvBackups
        // 
        dgvBackups.AllowUserToAddRows = false;
        dgvBackups.AllowUserToDeleteRows = false;
        dgvBackups.BackgroundColor = SystemColors.Window;
        dgvBackups.BorderStyle = BorderStyle.Fixed3D;
        dgvBackups.ColumnHeadersHeight = 50;
        dgvBackups.ContextMenuStrip = cmsBackups;
        dgvBackups.Location = new Point(12, 120);
        dgvBackups.Margin = new Padding(4);
        dgvBackups.Name = "dgvBackups";
        dgvBackups.ReadOnly = true;
        dgvBackups.RightToLeft = RightToLeft.Yes;
        dgvBackups.RowHeadersWidth = 62;
        dgvBackups.RowTemplate.Height = 28;
        dgvBackups.Size = new Size(840, 380);
            dgvBackups.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBackups.TabIndex = 0;
            dgvBackups.TabStop = false;
        // 
        // cmsBackups
        // 
        cmsBackups.BackColor = Color.White;
        cmsBackups.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        cmsBackups.ImageScalingSize = new Size(24, 24);
        cmsBackups.Items.AddRange(new ToolStripItem[] { refreshToolStripMenuItem, restoreToolStripMenuItem });
        cmsBackups.Name = "cmsBackups";
        cmsBackups.RightToLeft = RightToLeft.Yes;
        cmsBackups.Size = new Size(147, 64);
        cmsBackups.Opening += cmsBackups_Opening;
        // 
        // refreshToolStripMenuItem
        // 
        refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
        refreshToolStripMenuItem.Size = new Size(146, 30);
        refreshToolStripMenuItem.Text = "تحديث";
        refreshToolStripMenuItem.Click += refreshToolStripMenuItem_Click;
        // 
        // restoreToolStripMenuItem
        // 
        restoreToolStripMenuItem.Name = "restoreToolStripMenuItem";
        restoreToolStripMenuItem.Size = new Size(146, 30);
        restoreToolStripMenuItem.Text = "استعادة";
        restoreToolStripMenuItem.Click += restoreToolStripMenuItem_Click;
        // 
        // label1
        // 
        label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        label1.AutoSize = true;
        label1.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label1.Location = new Point(700, 525);
        label1.Name = "label1";
        label1.Size = new Size(120, 24);
        label1.TabIndex = 1;
        label1.Text = "عدد النسخ:";
        // 
        // lblRecordsCount
        // 
        lblRecordsCount.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        lblRecordsCount.AutoSize = true;
        lblRecordsCount.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblRecordsCount.Location = new Point(610, 525);
        lblRecordsCount.Name = "lblRecordsCount";
        lblRecordsCount.Size = new Size(41, 24);
        lblRecordsCount.TabIndex = 2;
        lblRecordsCount.Text = "[0]";
        // 
        // btnCreateBackup
        // 
        btnCreateBackup.BackColor = Color.White;
        btnCreateBackup.FlatAppearance.BorderColor = Color.Black;
        btnCreateBackup.FlatAppearance.BorderSize = 2;
        btnCreateBackup.FlatStyle = FlatStyle.Popup;
        btnCreateBackup.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnCreateBackup.Location = new Point(630, 70);
        btnCreateBackup.Name = "btnCreateBackup";
        btnCreateBackup.Size = new Size(130, 40);
        btnCreateBackup.TabIndex = 3;
        btnCreateBackup.Text = "إنشاء نسخة";
        btnCreateBackup.UseVisualStyleBackColor = false;
        btnCreateBackup.Click += btnCreateBackup_Click;
        // 
        // btnRestoreBackup
        // 
        btnRestoreBackup.BackColor = Color.White;
        btnRestoreBackup.FlatAppearance.BorderColor = Color.Black;
        btnRestoreBackup.FlatAppearance.BorderSize = 2;
        btnRestoreBackup.FlatStyle = FlatStyle.Popup;
        btnRestoreBackup.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnRestoreBackup.Location = new Point(480, 70);
        btnRestoreBackup.Name = "btnRestoreBackup";
        btnRestoreBackup.Size = new Size(130, 40);
        btnRestoreBackup.TabIndex = 4;
        btnRestoreBackup.Text = "استعادة النسخة";
        btnRestoreBackup.UseVisualStyleBackColor = false;
        btnRestoreBackup.Click += btnRestoreBackup_Click;
        // 
        // btnRefresh
        // 
        btnRefresh.BackColor = Color.White;
        btnRefresh.FlatAppearance.BorderColor = Color.Black;
        btnRefresh.FlatAppearance.BorderSize = 2;
        btnRefresh.FlatStyle = FlatStyle.Popup;
        btnRefresh.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnRefresh.Location = new Point(330, 70);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(130, 40);
        btnRefresh.TabIndex = 5;
        btnRefresh.Text = "تحديث";
        btnRefresh.UseVisualStyleBackColor = false;
        btnRefresh.Click += btnRefresh_Click;
        // 
        // btnClose
        // 
        btnClose.BackColor = Color.White;
        btnClose.DialogResult = DialogResult.Cancel;
        btnClose.FlatAppearance.BorderColor = Color.Black;
        btnClose.FlatAppearance.BorderSize = 2;
        btnClose.FlatStyle = FlatStyle.Popup;
        btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnClose.Location = new Point(12, 520);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(120, 45);
        btnClose.TabIndex = 6;
        btnClose.Text = "إغلاق";
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += btnClose_Click;
        // 
        // frmBackup
        // 
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.WhiteSmoke;
        CancelButton = btnClose;
        ClientSize = new Size(864, 584);
        Controls.Add(dgvBackups);
        Controls.Add(lblMode);
        Controls.Add(label1);
        Controls.Add(lblRecordsCount);
        Controls.Add(btnCreateBackup);
        Controls.Add(btnRestoreBackup);
        Controls.Add(btnRefresh);
        Controls.Add(btnClose);
        Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        Name = "frmBackup";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "إدارة النسخ الاحتياطي";
        Load += frmBackup_Load;
        ((System.ComponentModel.ISupportInitialize)dgvBackups).EndInit();
        cmsBackups.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label lblMode;
    private DataGridView dgvBackups;
    private Label label1;
    private Label lblRecordsCount;
    private Button btnCreateBackup;
    private Button btnRestoreBackup;
    private Button btnRefresh;
    private Button btnClose;
    private ContextMenuStrip cmsBackups;
    private ToolStripMenuItem refreshToolStripMenuItem;
    private ToolStripMenuItem restoreToolStripMenuItem;
}
