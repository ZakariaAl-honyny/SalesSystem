namespace SalesSystem.Desktop.Screens.CashBoxes.Forms;

partial class frmListCashBoxes
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
        cmsCashBoxes = new ContextMenuStrip(components);
        editToolStripMenuItem = new ToolStripMenuItem();
        deactivateToolStripMenuItem = new ToolStripMenuItem();
        showTransactionsToolStripMenuItem = new ToolStripMenuItem();
        dailyClosureToolStripMenuItem = new ToolStripMenuItem();
        dgvCashBoxes = new DataGridView();
        lblMode = new Label();
        label1 = new Label();
        lblRecordsCount = new Label();
        txtSearch = new TextBox();
        btnAddNew = new Button();
        btnRefresh = new Button();
        btnClose = new Button();
        cmsCashBoxes.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvCashBoxes).BeginInit();
        SuspendLayout();
        // 
        // cmsCashBoxes
        // 
        cmsCashBoxes.BackColor = Color.White;
        cmsCashBoxes.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        cmsCashBoxes.ImageScalingSize = new Size(24, 24);
        cmsCashBoxes.Items.AddRange(new ToolStripItem[] { editToolStripMenuItem, deactivateToolStripMenuItem, showTransactionsToolStripMenuItem, dailyClosureToolStripMenuItem });
        cmsCashBoxes.Name = "cmsCashBoxes";
        cmsCashBoxes.RightToLeft = RightToLeft.Yes;
        cmsCashBoxes.Size = new Size(209, 124);
        cmsCashBoxes.Opening += cmsCashBoxes_Opening;
        // 
        // editToolStripMenuItem
        // 
        editToolStripMenuItem.Name = "editToolStripMenuItem";
        editToolStripMenuItem.Size = new Size(208, 30);
        editToolStripMenuItem.Text = "تعديل الصندوق";
        editToolStripMenuItem.Click += editToolStripMenuItem_Click;
        // 
        // deactivateToolStripMenuItem
        // 
        deactivateToolStripMenuItem.Name = "deactivateToolStripMenuItem";
        deactivateToolStripMenuItem.Size = new Size(208, 30);
        deactivateToolStripMenuItem.Text = "إلغاء التنشيط";
        deactivateToolStripMenuItem.Click += deactivateToolStripMenuItem_Click;
        // 
        // showTransactionsToolStripMenuItem
        // 
        showTransactionsToolStripMenuItem.Name = "showTransactionsToolStripMenuItem";
        showTransactionsToolStripMenuItem.Size = new Size(208, 30);
        showTransactionsToolStripMenuItem.Text = "حركات الصندوق";
        showTransactionsToolStripMenuItem.Click += showTransactionsToolStripMenuItem_Click;
        // 
        // dailyClosureToolStripMenuItem
        // 
        dailyClosureToolStripMenuItem.Name = "dailyClosureToolStripMenuItem";
        dailyClosureToolStripMenuItem.Size = new Size(208, 30);
        dailyClosureToolStripMenuItem.Text = "الإغلاق اليومي";
        dailyClosureToolStripMenuItem.Click += dailyClosureToolStripMenuItem_Click;
        // 
        // dgvCashBoxes
        // 
        dgvCashBoxes.AllowUserToAddRows = false;
        dgvCashBoxes.AllowUserToDeleteRows = false;
        dgvCashBoxes.BackgroundColor = SystemColors.Window;
        dgvCashBoxes.BorderStyle = BorderStyle.Fixed3D;
        dgvCashBoxes.ColumnHeadersHeight = 50;
        dgvCashBoxes.ContextMenuStrip = cmsCashBoxes;
        dgvCashBoxes.Location = new Point(12, 160);
        dgvCashBoxes.Margin = new Padding(4);
        dgvCashBoxes.MultiSelect = false;
        dgvCashBoxes.Name = "dgvCashBoxes";
        dgvCashBoxes.ReadOnly = true;
        dgvCashBoxes.RowHeadersWidth = 62;
        dgvCashBoxes.RowTemplate.Height = 28;
        dgvCashBoxes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvCashBoxes.Size = new Size(1176, 500);
        dgvCashBoxes.TabIndex = 0;
        dgvCashBoxes.TabStop = false;
        dgvCashBoxes.DoubleClick += dgvCashBoxes_DoubleClick;
        // 
        // lblMode
        // 
        lblMode.Anchor = AnchorStyles.Top;
        lblMode.AutoSize = true;
        lblMode.BackColor = Color.WhiteSmoke;
        lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblMode.ForeColor = Color.Firebrick;
        lblMode.Location = new Point(452, 20);
        lblMode.Margin = new Padding(4, 0, 4, 0);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(343, 39);
        lblMode.TabIndex = 30;
        lblMode.Text = "إدارة الصناديق النقدية";
        // 
        // label1
        // 
        label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        label1.AutoSize = true;
        label1.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label1.Location = new Point(955, 580);
        label1.Margin = new Padding(4, 0, 4, 0);
        label1.Name = "label1";
        label1.Size = new Size(145, 24);
        label1.TabIndex = 3;
        label1.Text = "عدد السجلات:";
        // 
        // lblRecordsCount
        // 
        lblRecordsCount.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        lblRecordsCount.AutoSize = true;
        lblRecordsCount.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblRecordsCount.Location = new Point(835, 580);
        lblRecordsCount.Margin = new Padding(4, 0, 4, 0);
        lblRecordsCount.Name = "lblRecordsCount";
        lblRecordsCount.Size = new Size(41, 24);
        lblRecordsCount.TabIndex = 4;
        lblRecordsCount.Text = "[0]";
        // 
        // txtSearch
        // 
        txtSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        txtSearch.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        txtSearch.Location = new Point(200, 159);
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
        // frmListCashBoxes
        // 
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.WhiteSmoke;
        CancelButton = btnClose;
        ClientSize = new Size(1200, 723);
        Controls.Add(dgvCashBoxes);
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
        Name = "frmListCashBoxes";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "إدارة الصناديق النقدية";
        Load += frmListCashBoxes_Load;
        cmsCashBoxes.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvCashBoxes).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private DataGridView dgvCashBoxes;
    private Label lblMode;
    private Label label1;
    private Label lblRecordsCount;
    private TextBox txtSearch;
    private Button btnAddNew;
    private Button btnRefresh;
    private Button btnClose;
    private Button btnTransactions;
    private Button btnDailyClosure;
    private ContextMenuStrip cmsCashBoxes;
    private ToolStripMenuItem editToolStripMenuItem;
    private ToolStripMenuItem deactivateToolStripMenuItem;
    private ToolStripMenuItem showTransactionsToolStripMenuItem;
    private ToolStripMenuItem dailyClosureToolStripMenuItem;
}
