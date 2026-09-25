namespace SalesSystem.Desktop.Screens.Expenses.Forms;

partial class frmPettyCash
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        components?.Dispose(); base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        cmsPettyCash = new ContextMenuStrip(components);
        editToolStripMenuItem = new ToolStripMenuItem();
        deleteToolStripMenuItem = new ToolStripMenuItem();
        dgvPettyCash = new DataGridView();
        lblMode = new Label();
        label1 = new Label();
        lblRecordsCount = new Label();
        txtSearch = new TextBox();
        btnAddNew = new Button();
        btnRefresh = new Button();
        btnClose = new Button();
        cmsPettyCash.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvPettyCash).BeginInit();
        SuspendLayout();
        // 
        // cmsPettyCash
        // 
        cmsPettyCash.BackColor = Color.White;
        cmsPettyCash.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        cmsPettyCash.ImageScalingSize = new Size(24, 24);
        cmsPettyCash.Items.AddRange(new ToolStripItem[] { editToolStripMenuItem, deleteToolStripMenuItem });
        cmsPettyCash.Name = "cmsPettyCash";
        cmsPettyCash.RightToLeft = RightToLeft.Yes;
        cmsPettyCash.Size = new Size(200, 64);
        cmsPettyCash.Opening += cmsPettyCash_Opening;
        // 
        // editToolStripMenuItem
        // 
        editToolStripMenuItem.Name = "editToolStripMenuItem";
        editToolStripMenuItem.Size = new Size(199, 30);
        editToolStripMenuItem.Text = "تعديل الطلب";
        editToolStripMenuItem.Click += editToolStripMenuItem_Click;
        // 
        // deleteToolStripMenuItem
        // 
        deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
        deleteToolStripMenuItem.Size = new Size(199, 30);
        deleteToolStripMenuItem.Text = "حذف الطلب";
        deleteToolStripMenuItem.Click += deleteToolStripMenuItem_Click;
        // 
        // dgvPettyCash
        // 
        dgvPettyCash.AllowUserToAddRows = false;
        dgvPettyCash.AllowUserToDeleteRows = false;
        dgvPettyCash.BackgroundColor = SystemColors.Window;
        dgvPettyCash.BorderStyle = BorderStyle.Fixed3D;
        dgvPettyCash.ColumnHeadersHeight = 50;
        dgvPettyCash.ContextMenuStrip = cmsPettyCash;
        dgvPettyCash.Location = new Point(12, 160);
        dgvPettyCash.Margin = new Padding(4);
        dgvPettyCash.MultiSelect = false;
        dgvPettyCash.Name = "dgvPettyCash";
        dgvPettyCash.ReadOnly = true;
        dgvPettyCash.RowHeadersWidth = 62;
        dgvPettyCash.RowTemplate.Height = 28;
        dgvPettyCash.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvPettyCash.Size = new Size(1549, 482);
        dgvPettyCash.TabIndex = 0;
        dgvPettyCash.TabStop = false;
        dgvPettyCash.DoubleClick += dgvPettyCash_DoubleClick;
        // 
        // lblMode
        // 
        lblMode.Anchor = AnchorStyles.Top;
        lblMode.AutoSize = true;
        lblMode.BackColor = Color.WhiteSmoke;
        lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblMode.ForeColor = Color.Firebrick;
        lblMode.Location = new Point(650, 9);
        lblMode.Margin = new Padding(4, 0, 4, 0);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(330, 39);
        lblMode.TabIndex = 30;
        lblMode.Text = "إدارة العهد النثرية";
        // 
        // label1
        // 
        label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        label1.AutoSize = true;
        label1.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label1.Location = new Point(1284, 660);
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
        lblRecordsCount.Location = new Point(1164, 660);
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
        txtSearch.PlaceholderText = "بحث برقم الطلب أو اسم الموظف أو السبب...";
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
        btnAddNew.Location = new Point(1486, 80);
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
        btnRefresh.Location = new Point(1403, 80);
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
        btnClose.Location = new Point(12, 730);
        btnClose.Margin = new Padding(4);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(181, 61);
        btnClose.TabIndex = 0;
        btnClose.Text = "إغلاق";
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += btnClose_Click;
        // 
        // frmPettyCash
        // 
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.WhiteSmoke;
        CancelButton = btnClose;
        ClientSize = new Size(1574, 803);
        Controls.Add(dgvPettyCash);
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
        Name = "frmPettyCash";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "إدارة العهد النثرية";
        Load += frmPettyCash_Load;
        cmsPettyCash.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvPettyCash).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private DataGridView dgvPettyCash;
    private Label lblMode;
    private Label label1;
    private Label lblRecordsCount;
    private TextBox txtSearch;
    private Button btnAddNew;
    private Button btnRefresh;
    private Button btnClose;
    private ContextMenuStrip cmsPettyCash;
    private ToolStripMenuItem editToolStripMenuItem;
    private ToolStripMenuItem deleteToolStripMenuItem;
}
