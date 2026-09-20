namespace SalesSystem.Desktop.Screens.CashBoxes.Forms;

partial class frmCashBoxTransactions
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
        label1 = new Label();
        cmbCashBox = new ComboBox();
        dgvTransactions = new DataGridView();
        cmsTransactions = new ContextMenuStrip(components);
        refreshToolStripMenuItem = new ToolStripMenuItem();
        label2 = new Label();
        lblRecordsCount = new Label();
        btnRefresh = new Button();
        btnClose = new Button();
        ((System.ComponentModel.ISupportInitialize)dgvTransactions).BeginInit();
        cmsTransactions.SuspendLayout();
        SuspendLayout();
        // 
        // lblMode
        // 
        lblMode.Anchor = AnchorStyles.Top;
        lblMode.AutoSize = true;
        lblMode.BackColor = Color.WhiteSmoke;
        lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblMode.ForeColor = Color.Firebrick;
        lblMode.Location = new Point(340, 15);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(257, 39);
        lblMode.TabIndex = 30;
        lblMode.Text = "حركات الصندوق";
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label1.Location = new Point(370, 75);
        label1.Name = "label1";
        label1.Size = new Size(139, 24);
        label1.TabIndex = 0;
        label1.Text = "اختر الصندوق";
        // 
        // cmbCashBox
        // 
        cmbCashBox.BackColor = SystemColors.Control;
        cmbCashBox.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbCashBox.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        cmbCashBox.FormattingEnabled = true;
        cmbCashBox.Location = new Point(40, 72);
        cmbCashBox.Name = "cmbCashBox";
        cmbCashBox.Size = new Size(320, 32);
        cmbCashBox.TabIndex = 1;
        cmbCashBox.SelectedIndexChanged += cmbCashBox_SelectedIndexChanged;
        // 
        // dgvTransactions
        // 
        dgvTransactions.AllowUserToAddRows = false;
        dgvTransactions.AllowUserToDeleteRows = false;
        dgvTransactions.BackgroundColor = SystemColors.Window;
        dgvTransactions.BorderStyle = BorderStyle.Fixed3D;
        dgvTransactions.ColumnHeadersHeight = 50;
        dgvTransactions.ContextMenuStrip = cmsTransactions;
        dgvTransactions.Location = new Point(12, 120);
        dgvTransactions.Margin = new Padding(4);
        dgvTransactions.MultiSelect = false;
        dgvTransactions.Name = "dgvTransactions";
        dgvTransactions.ReadOnly = true;
        dgvTransactions.RowHeadersWidth = 62;
        dgvTransactions.RowTemplate.Height = 28;
        dgvTransactions.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvTransactions.Size = new Size(1176, 500);
        dgvTransactions.TabIndex = 2;
        dgvTransactions.TabStop = false;
        // 
        // cmsTransactions
        // 
        cmsTransactions.BackColor = Color.White;
        cmsTransactions.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        cmsTransactions.ImageScalingSize = new Size(24, 24);
        cmsTransactions.Items.AddRange(new ToolStripItem[] { refreshToolStripMenuItem });
        cmsTransactions.Name = "cmsTransactions";
        cmsTransactions.RightToLeft = RightToLeft.Yes;
        cmsTransactions.Size = new Size(137, 34);
        cmsTransactions.Opening += cmsTransactions_Opening;
        // 
        // refreshToolStripMenuItem
        // 
        refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
        refreshToolStripMenuItem.Size = new Size(136, 30);
        refreshToolStripMenuItem.Text = "تحديث";
        refreshToolStripMenuItem.Click += refreshToolStripMenuItem_Click;
        // 
        // label2
        // 
        label2.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        label2.AutoSize = true;
        label2.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label2.Location = new Point(469, 565);
        label2.Name = "label2";
        label2.Size = new Size(135, 24);
        label2.TabIndex = 3;
        label2.Text = "عدد الحركات:";
        // 
        // lblRecordsCount
        // 
        lblRecordsCount.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        lblRecordsCount.AutoSize = true;
        lblRecordsCount.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblRecordsCount.Location = new Point(610, 565);
        lblRecordsCount.Name = "lblRecordsCount";
        lblRecordsCount.Size = new Size(41, 24);
        lblRecordsCount.TabIndex = 4;
        lblRecordsCount.Text = "[0]";
        // 
        // btnRefresh
        // 
        btnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnRefresh.BackColor = Color.White;
        btnRefresh.FlatAppearance.BorderColor = Color.Black;
        btnRefresh.FlatAppearance.BorderSize = 2;
        btnRefresh.FlatStyle = FlatStyle.Popup;
        btnRefresh.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnRefresh.Location = new Point(1103, 80);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(75, 75);
        btnRefresh.TabIndex = 5;
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
        btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnClose.Location = new Point(12, 650);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(181, 61);
        btnClose.TabIndex = 0;
        btnClose.Text = "إغلاق";
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += btnClose_Click;
        // 
        // frmCashBoxTransactions
        // 
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.WhiteSmoke;
        CancelButton = btnClose;
        ClientSize = new Size(1200, 723);
        Controls.Add(dgvTransactions);
        Controls.Add(lblMode);
        Controls.Add(label1);
        Controls.Add(cmbCashBox);
        Controls.Add(label2);
        Controls.Add(lblRecordsCount);
        Controls.Add(btnRefresh);
        Controls.Add(btnClose);
        Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        Name = "frmCashBoxTransactions";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "حركات الصندوق";
        Load += frmCashBoxTransactions_Load;
        ((System.ComponentModel.ISupportInitialize)dgvTransactions).EndInit();
        cmsTransactions.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label lblMode;
    private Label label1;
    private ComboBox cmbCashBox;
    private DataGridView dgvTransactions;
    private Label label2;
    private Label lblRecordsCount;
    private Button btnRefresh;
    private Button btnClose;
    private ContextMenuStrip cmsTransactions;
    private ToolStripMenuItem refreshToolStripMenuItem;
}
