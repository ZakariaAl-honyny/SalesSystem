namespace SalesSystem.Desktop.Screens.Currencies.Forms;

partial class frmListCurrencies
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
        cmsCurrencies = new ContextMenuStrip(components);
        editToolStripMenuItem = new ToolStripMenuItem();
        dgvCurrencies = new DataGridView();
        dgvRates = new DataGridView();
        lblMode = new Label();
        lblRecordsLabel = new Label();
        lblRecordsCount = new Label();
        txtSearch = new TextBox();
        btnAddNew = new Button();
        btnRefresh = new Button();
        btnClose = new Button();
        lblRatesTitle = new Label();
        splMain = new SplitContainer();
        cmsCurrencies.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvCurrencies).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvRates).BeginInit();
        ((System.ComponentModel.ISupportInitialize)splMain).BeginInit();
        splMain.Panel1.SuspendLayout();
        splMain.Panel2.SuspendLayout();
        splMain.SuspendLayout();
        SuspendLayout();
        // 
        // cmsCurrencies
        // 
        cmsCurrencies.BackColor = Color.White;
        cmsCurrencies.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        cmsCurrencies.ImageScalingSize = new Size(24, 24);
        cmsCurrencies.Items.AddRange(new ToolStripItem[] { editToolStripMenuItem });
        cmsCurrencies.Name = "cmsCurrencies";
        cmsCurrencies.RightToLeft = RightToLeft.Yes;
        cmsCurrencies.Size = new Size(189, 36);
        cmsCurrencies.Opening += cmsCurrencies_Opening;
        // 
        // editToolStripMenuItem
        // 
        editToolStripMenuItem.Name = "editToolStripMenuItem";
        editToolStripMenuItem.Size = new Size(188, 30);
        editToolStripMenuItem.Text = "تعديل العملة";
        editToolStripMenuItem.Click += editToolStripMenuItem_Click;
        // 
        // dgvCurrencies
        // 
        dgvCurrencies.AllowUserToAddRows = false;
        dgvCurrencies.AllowUserToDeleteRows = false;
        dgvCurrencies.BackgroundColor = SystemColors.Window;
        dgvCurrencies.BorderStyle = BorderStyle.Fixed3D;
        dgvCurrencies.ColumnHeadersHeight = 50;
        dgvCurrencies.ContextMenuStrip = cmsCurrencies;
        dgvCurrencies.Dock = DockStyle.Fill;
        dgvCurrencies.Margin = new Padding(4);
        dgvCurrencies.Name = "dgvCurrencies";
        dgvCurrencies.ReadOnly = true;
        dgvCurrencies.RowHeadersWidth = 62;
        dgvCurrencies.RowTemplate.Height = 28;
        dgvCurrencies.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvCurrencies.TabIndex = 0;
        dgvCurrencies.TabStop = false;
        dgvCurrencies.DoubleClick += dgvCurrencies_DoubleClick;
        dgvCurrencies.SelectionChanged += dgvCurrencies_SelectionChanged;
        // 
        // dgvRates
        // 
        dgvRates.AllowUserToAddRows = false;
        dgvRates.AllowUserToDeleteRows = false;
        dgvRates.BackgroundColor = SystemColors.Window;
        dgvRates.BorderStyle = BorderStyle.Fixed3D;
        dgvRates.ColumnHeadersHeight = 45;
        dgvRates.Dock = DockStyle.Fill;
        dgvRates.Margin = new Padding(4);
        dgvRates.Name = "dgvRates";
        dgvRates.ReadOnly = true;
        dgvRates.RowHeadersWidth = 62;
        dgvRates.RowTemplate.Height = 26;
        dgvRates.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvRates.TabIndex = 0;
        dgvRates.TabStop = false;
        // 
        // splMain
        // 
        splMain.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        splMain.Location = new Point(12, 155);
        splMain.Name = "splMain";
        splMain.Orientation = Orientation.Horizontal;
        // 
        // splMain.Panel1
        // 
        splMain.Panel1.Controls.Add(dgvCurrencies);
        splMain.Panel1.RightToLeft = RightToLeft.Yes;
        splMain.Panel1MinSize = 150;
        // 
        // splMain.Panel2
        // 
        splMain.Panel2.Controls.Add(dgvRates);
        splMain.Panel2.Controls.Add(lblRatesTitle);
        splMain.Panel2.RightToLeft = RightToLeft.Yes;
        splMain.Panel2MinSize = 100;
        splMain.Size = new Size(1549, 480);
        splMain.SplitterDistance = 260;
        splMain.TabIndex = 31;
        // 
        // lblMode
        // 
        lblMode.Anchor = AnchorStyles.Top;
        lblMode.AutoSize = true;
        lblMode.BackColor = Color.WhiteSmoke;
        lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblMode.ForeColor = Color.Firebrick;
        lblMode.Location = new Point(720, 9);
        lblMode.Margin = new Padding(4, 0, 4, 0);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(195, 39);
        lblMode.TabIndex = 30;
        lblMode.Text = "إدارة العملات";
        // 
        // lblRecordsLabel
        // 
        lblRecordsLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        lblRecordsLabel.AutoSize = true;
        lblRecordsLabel.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblRecordsLabel.Location = new Point(1184, 645);
        lblRecordsLabel.Margin = new Padding(4, 0, 4, 0);
        lblRecordsLabel.Name = "lblRecordsLabel";
        lblRecordsLabel.Size = new Size(145, 24);
        lblRecordsLabel.TabIndex = 3;
        lblRecordsLabel.Text = "عدد السجلات:";
        // 
        // lblRecordsCount
        // 
        lblRecordsCount.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        lblRecordsCount.AutoSize = true;
        lblRecordsCount.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblRecordsCount.Location = new Point(1064, 645);
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
        txtSearch.Location = new Point(13, 118);
        txtSearch.Margin = new Padding(4);
        txtSearch.Name = "txtSearch";
        txtSearch.PlaceholderText = "بحث بالاسم أو الكود...";
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
        btnAddNew.Location = new Point(1486, 75);
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
        btnRefresh.Location = new Point(1403, 75);
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
        btnClose.Location = new Point(12, 645);
        btnClose.Margin = new Padding(4);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(181, 61);
        btnClose.TabIndex = 0;
        btnClose.Text = "إغلاق";
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += btnClose_Click;
        // 
        // lblRatesTitle
        // 
        lblRatesTitle.Dock = DockStyle.Top;
        lblRatesTitle.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblRatesTitle.ForeColor = Color.DarkSlateGray;
        lblRatesTitle.Location = new Point(0, 0);
        lblRatesTitle.Name = "lblRatesTitle";
        lblRatesTitle.Padding = new Padding(0, 4, 0, 4);
        lblRatesTitle.Size = new Size(1549, 30);
        lblRatesTitle.TabIndex = 1;
        lblRatesTitle.Text = "أسعار الصرف";
        // 
        // frmListCurrencies
        // 
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.WhiteSmoke;
        CancelButton = btnClose;
        ClientSize = new Size(1574, 723);
        Controls.Add(splMain);
        Controls.Add(lblMode);
        Controls.Add(txtSearch);
        Controls.Add(lblRecordsLabel);
        Controls.Add(lblRecordsCount);
        Controls.Add(btnAddNew);
        Controls.Add(btnRefresh);
        Controls.Add(btnClose);
        Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        Name = "frmListCurrencies";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "إدارة العملات";
        Load += frmListCurrencies_Load;
        cmsCurrencies.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvCurrencies).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvRates).EndInit();
        splMain.Panel1.ResumeLayout(false);
        splMain.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splMain).EndInit();
        splMain.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private DataGridView dgvCurrencies;
    private DataGridView dgvRates;
    private Label lblMode;
    private Label lblRecordsLabel;
    private Label lblRecordsCount;
    private TextBox txtSearch;
    private Button btnAddNew;
    private Button btnRefresh;
    private Button btnClose;
    private Label lblRatesTitle;
    private SplitContainer splMain;
    private ContextMenuStrip cmsCurrencies;
    private ToolStripMenuItem editToolStripMenuItem;
}
