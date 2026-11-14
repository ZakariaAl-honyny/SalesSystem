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
        DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
        lblMode = new Label();
        pnlToolbar = new Panel();
        btnCreateBackup = new Button();
        btnRestoreBackup = new Button();
        btnRefresh = new Button();
        pnlGrid = new Panel();
        gbBackups = new GroupBox();
        dgvBackups = new DataGridView();
        cmsBackups = new ContextMenuStrip(components);
        refreshToolStripMenuItem = new ToolStripMenuItem();
        restoreToolStripMenuItem = new ToolStripMenuItem();
        pnlSummary = new Panel();
        label1 = new Label();
        lblRecordsCount = new Label();
        pnlFooter = new Panel();
        btnClose = new Button();
        pnlToolbar.SuspendLayout();
        pnlGrid.SuspendLayout();
        gbBackups.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvBackups).BeginInit();
        cmsBackups.SuspendLayout();
        pnlSummary.SuspendLayout();
        pnlFooter.SuspendLayout();
        SuspendLayout();
        // 
        // lblMode
        // 
        lblMode.BackColor = Color.FromArgb(14, 165, 103);
        lblMode.Dock = DockStyle.Top;
        lblMode.Font = new Font("Tahoma", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblMode.ForeColor = Color.White;
        lblMode.Location = new Point(0, 0);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(864, 46);
        lblMode.TabIndex = 0;
        lblMode.Text = "النسخ الاحتياطي";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlToolbar
        // 
        pnlToolbar.BackColor = Color.FromArgb(235, 240, 235);
        pnlToolbar.Controls.Add(btnCreateBackup);
        pnlToolbar.Controls.Add(btnRestoreBackup);
        pnlToolbar.Controls.Add(btnRefresh);
        pnlToolbar.Dock = DockStyle.Top;
        pnlToolbar.Location = new Point(0, 46);
        pnlToolbar.Name = "pnlToolbar";
        pnlToolbar.Padding = new Padding(10);
        pnlToolbar.Size = new Size(864, 66);
        pnlToolbar.TabIndex = 1;
        // 
        // btnCreateBackup
        // 
        btnCreateBackup.BackColor = Color.FromArgb(14, 165, 103);
        btnCreateBackup.FlatAppearance.BorderSize = 0;
        btnCreateBackup.FlatStyle = FlatStyle.Flat;
        btnCreateBackup.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnCreateBackup.ForeColor = Color.White;
        btnCreateBackup.Location = new Point(700, 12);
        btnCreateBackup.Name = "btnCreateBackup";
        btnCreateBackup.Size = new Size(150, 42);
        btnCreateBackup.TabIndex = 0;
        btnCreateBackup.Text = "إنشاء نسخة";
        btnCreateBackup.UseVisualStyleBackColor = false;
        btnCreateBackup.Click += btnCreateBackup_Click;
        // 
        // btnRestoreBackup
        // 
        btnRestoreBackup.BackColor = Color.FromArgb(108, 117, 125);
        btnRestoreBackup.FlatAppearance.BorderSize = 0;
        btnRestoreBackup.FlatStyle = FlatStyle.Flat;
        btnRestoreBackup.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnRestoreBackup.ForeColor = Color.White;
        btnRestoreBackup.Location = new Point(540, 12);
        btnRestoreBackup.Name = "btnRestoreBackup";
        btnRestoreBackup.Size = new Size(150, 42);
        btnRestoreBackup.TabIndex = 1;
        btnRestoreBackup.Text = "استعادة النسخة";
        btnRestoreBackup.UseVisualStyleBackColor = false;
        btnRestoreBackup.Click += btnRestoreBackup_Click;
        // 
        // btnRefresh
        // 
        btnRefresh.BackColor = Color.FromArgb(23, 162, 184);
        btnRefresh.FlatAppearance.BorderSize = 0;
        btnRefresh.FlatStyle = FlatStyle.Flat;
        btnRefresh.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnRefresh.ForeColor = Color.White;
        btnRefresh.Location = new Point(380, 12);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(150, 42);
        btnRefresh.TabIndex = 2;
        btnRefresh.Text = "تحديث (F5)";
        btnRefresh.UseVisualStyleBackColor = false;
        btnRefresh.Click += btnRefresh_Click;
        // 
        // pnlGrid
        // 
        pnlGrid.BackColor = Color.FromArgb(170, 220, 170);
        pnlGrid.Controls.Add(gbBackups);
        pnlGrid.Dock = DockStyle.Fill;
        pnlGrid.Location = new Point(0, 112);
        pnlGrid.Name = "pnlGrid";
        pnlGrid.Padding = new Padding(10);
        pnlGrid.Size = new Size(864, 358);
        pnlGrid.TabIndex = 2;
        // 
        // gbBackups
        // 
        gbBackups.BackColor = Color.FromArgb(235, 245, 235);
        gbBackups.Controls.Add(dgvBackups);
        gbBackups.Dock = DockStyle.Fill;
        gbBackups.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        gbBackups.ForeColor = Color.FromArgb(14, 82, 52);
        gbBackups.Location = new Point(10, 10);
        gbBackups.Name = "gbBackups";
        gbBackups.Padding = new Padding(10);
        gbBackups.Size = new Size(844, 338);
        gbBackups.TabIndex = 0;
        gbBackups.TabStop = false;
        gbBackups.Text = "النسخ الاحتياطية المتاحة";
        // 
        // dgvBackups
        // 
        dgvBackups.AllowUserToAddRows = false;
        dgvBackups.AllowUserToDeleteRows = false;
        dataGridViewCellStyle1.BackColor = Color.FromArgb(245, 252, 245);
        dgvBackups.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
        dgvBackups.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvBackups.BackgroundColor = Color.FromArgb(220, 240, 220);
        dgvBackups.BorderStyle = BorderStyle.None;
        dgvBackups.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
        dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
        dataGridViewCellStyle2.BackColor = Color.FromArgb(14, 165, 103);
        dataGridViewCellStyle2.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        dataGridViewCellStyle2.ForeColor = Color.White;
        dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(14, 165, 103);
        dataGridViewCellStyle2.SelectionForeColor = Color.White;
        dgvBackups.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
        dgvBackups.ColumnHeadersHeight = 45;
        dgvBackups.ContextMenuStrip = cmsBackups;
        dataGridViewCellStyle3.BackColor = Color.White;
        dataGridViewCellStyle3.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        dataGridViewCellStyle3.ForeColor = Color.FromArgb(33, 37, 41);
        dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(255, 255, 180);
        dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(33, 37, 41);
        dgvBackups.DefaultCellStyle = dataGridViewCellStyle3;
        dgvBackups.Dock = DockStyle.Fill;
        dgvBackups.EnableHeadersVisualStyles = false;
        dgvBackups.GridColor = Color.FromArgb(205, 225, 205);
        dgvBackups.Location = new Point(10, 34);
        dgvBackups.Margin = new Padding(0);
        dgvBackups.Name = "dgvBackups";
        dgvBackups.ReadOnly = true;
        dgvBackups.RightToLeft = RightToLeft.Yes;
        dgvBackups.RowHeadersVisible = false;
        dgvBackups.RowTemplate.Height = 32;
        dgvBackups.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvBackups.Size = new Size(824, 294);
        dgvBackups.TabIndex = 0;
        dgvBackups.TabStop = false;
        dgvBackups.CellDoubleClick += dgvBackups_CellDoubleClick;
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
        restoreToolStripMenuItem.Text = "استعادة النسخة المحددة";
        restoreToolStripMenuItem.Click += restoreToolStripMenuItem_Click;
        // 
        // pnlSummary
        // 
        pnlSummary.BackColor = Color.FromArgb(235, 240, 235);
        pnlSummary.Controls.Add(label1);
        pnlSummary.Controls.Add(lblRecordsCount);
        pnlSummary.Dock = DockStyle.Bottom;
        pnlSummary.Location = new Point(0, 470);
        pnlSummary.Name = "pnlSummary";
        pnlSummary.Size = new Size(864, 46);
        pnlSummary.TabIndex = 3;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label1.ForeColor = Color.FromArgb(14, 82, 52);
        label1.Location = new Point(700, 11);
        label1.Name = "label1";
        label1.Size = new Size(120, 24);
        label1.TabIndex = 0;
        label1.Text = "عدد النسخ:";
        // 
        // lblRecordsCount
        // 
        lblRecordsCount.AutoSize = true;
        lblRecordsCount.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblRecordsCount.ForeColor = Color.FromArgb(14, 82, 52);
        lblRecordsCount.Location = new Point(630, 11);
        lblRecordsCount.Name = "lblRecordsCount";
        lblRecordsCount.Size = new Size(41, 24);
        lblRecordsCount.TabIndex = 1;
        lblRecordsCount.Text = "0";
        // 
        // pnlFooter
        // 
        pnlFooter.BackColor = Color.FromArgb(235, 240, 235);
        pnlFooter.Controls.Add(btnClose);
        pnlFooter.Dock = DockStyle.Bottom;
        pnlFooter.Location = new Point(0, 516);
        pnlFooter.Name = "pnlFooter";
        pnlFooter.Padding = new Padding(10);
        pnlFooter.Size = new Size(864, 68);
        pnlFooter.TabIndex = 4;
        // 
        // btnClose
        // 
        btnClose.BackColor = Color.FromArgb(108, 117, 125);
        btnClose.DialogResult = DialogResult.Cancel;
        btnClose.FlatAppearance.BorderSize = 0;
        btnClose.FlatStyle = FlatStyle.Flat;
        btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnClose.ForeColor = Color.White;
        btnClose.Location = new Point(12, 12);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(130, 44);
        btnClose.TabIndex = 0;
        btnClose.Text = "إغلاق (Esc)";
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += btnClose_Click;
        // 
        // frmBackup
        // 
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.FromArgb(170, 220, 170);
        CancelButton = btnClose;
        ClientSize = new Size(864, 584);
        Controls.Add(pnlGrid);
        Controls.Add(pnlSummary);
        Controls.Add(pnlFooter);
        Controls.Add(pnlToolbar);
        Controls.Add(lblMode);
        Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "frmBackup";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "إدارة النسخ الاحتياطي";
        Load += frmBackup_Load;
        pnlToolbar.ResumeLayout(false);
        pnlGrid.ResumeLayout(false);
        gbBackups.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvBackups).EndInit();
        cmsBackups.ResumeLayout(false);
        pnlSummary.ResumeLayout(false);
        pnlSummary.PerformLayout();
        pnlFooter.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label lblMode;
    private Panel pnlToolbar;
    private Button btnCreateBackup;
    private Button btnRestoreBackup;
    private Button btnRefresh;
    private Panel pnlGrid;
    private GroupBox gbBackups;
    private DataGridView dgvBackups;
    private ContextMenuStrip cmsBackups;
    private ToolStripMenuItem refreshToolStripMenuItem;
    private ToolStripMenuItem restoreToolStripMenuItem;
    private Panel pnlSummary;
    private Label label1;
    private Label lblRecordsCount;
    private Panel pnlFooter;
    private Button btnClose;
}
