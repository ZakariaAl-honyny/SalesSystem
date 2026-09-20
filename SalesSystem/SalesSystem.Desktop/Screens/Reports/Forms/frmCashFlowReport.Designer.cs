namespace SalesSystem.Desktop.Screens.Reports.Forms;

partial class frmCashFlowReport
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        components?.Dispose();
        base.Dispose(disposing);
    }

    private Label lblMode;
    private DataGridView dgvReport;
    private Button btnRefresh;
    private Button btnExport;
    private Button btnClose;
    private Label label1;
    private Label lblRecordsCount;
    private Label lblSummary;
    private Panel loadingPanel;
    private DateTimePicker dtpFrom;
    private DateTimePicker dtpTo;
    private Label label3;
    private Label label4;

    private void InitializeComponent()
    {
        lblMode = new Label();
        dgvReport = new DataGridView();
        btnRefresh = new Button();
        btnExport = new Button();
        btnClose = new Button();
        label1 = new Label();
        lblRecordsCount = new Label();
        lblSummary = new Label();
        loadingPanel = new Panel();
        ll = new Label();
        dtpFrom = new DateTimePicker();
        dtpTo = new DateTimePicker();
        label3 = new Label();
        label4 = new Label();
        ((System.ComponentModel.ISupportInitialize)dgvReport).BeginInit();
        loadingPanel.SuspendLayout();
        SuspendLayout();
        // 
        // lblMode
        // 
        lblMode.BackColor = Color.WhiteSmoke;
        lblMode.Dock = DockStyle.Top;
        lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblMode.ForeColor = Color.Firebrick;
        lblMode.Location = new Point(0, 0);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(1200, 39);
        lblMode.TabIndex = 30;
        lblMode.Text = "طھظ‚ط±ظٹط± ط§ظ„طھط¯ظپظ‚ط§طھ ط§ظ„ظ†ظ‚ط¯ظٹط©";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // dgvReport
        // 
        dgvReport.AllowUserToAddRows = false;
        dgvReport.AllowUserToDeleteRows = false;
        dgvReport.AllowUserToOrderColumns = true;
        dgvReport.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvReport.BackgroundColor = SystemColors.Window;
        dgvReport.BorderStyle = BorderStyle.Fixed3D;
        dgvReport.ColumnHeadersHeight = 50;
        dgvReport.Location = new Point(11, 100);
        dgvReport.Name = "dgvReport";
        dgvReport.ReadOnly = true;
        dgvReport.RowHeadersWidth = 62;
        dgvReport.RowTemplate.Height = 28;
        dgvReport.Size = new Size(1178, 500);
        dgvReport.TabIndex = 7;
        dgvReport.TabStop = false;
        // 
        // btnRefresh
        // 
        btnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnRefresh.BackColor = Color.White;
        btnRefresh.FlatAppearance.BorderColor = Color.Black;
        btnRefresh.FlatAppearance.BorderSize = 2;
        btnRefresh.FlatStyle = FlatStyle.Popup;
        btnRefresh.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnRefresh.Location = new Point(10, 50);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(100, 36);
        btnRefresh.TabIndex = 1;
        btnRefresh.Text = "طھط­ط¯ظٹط«";
        btnRefresh.UseVisualStyleBackColor = true;
        btnRefresh.Click += btnRefresh_Click;
        // 
        // btnExport
        // 
        btnExport.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnExport.BackColor = Color.White;
        btnExport.FlatAppearance.BorderColor = Color.Black;
        btnExport.FlatAppearance.BorderSize = 2;
        btnExport.FlatStyle = FlatStyle.Popup;
        btnExport.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnExport.ForeColor = Color.FromArgb(46, 125, 50);
        btnExport.Location = new Point(120, 50);
        btnExport.Name = "btnExport";
        btnExport.Size = new Size(173, 36);
        btnExport.TabIndex = 2;
        btnExport.Text = "طھطµط¯ظٹط± Excel";
        btnExport.UseVisualStyleBackColor = true;
        btnExport.Click += btnExport_Click;
        // 
        // btnClose
        // 
        btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnClose.BackColor = Color.White;
        btnClose.DialogResult = DialogResult.Cancel;
        btnClose.FlatStyle = FlatStyle.Popup;
        btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnClose.Location = new Point(11, 616);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(140, 50);
        btnClose.TabIndex = 0;
        btnClose.Text = "ط¥ط؛ظ„ط§ظ‚";
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += btnClose_Click;
        // 
        // label1
        // 
        label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        label1.AutoSize = true;
        label1.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label1.Location = new Point(1030, 626);
        label1.Name = "label1";
        label1.Size = new Size(145, 24);
        label1.TabIndex = 9;
        label1.Text = "ط¹ط¯ط¯ ط§ظ„ط³ط¬ظ„ط§طھ:";
        // 
        // lblRecordsCount
        // 
        lblRecordsCount.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        lblRecordsCount.AutoSize = true;
        lblRecordsCount.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblRecordsCount.Location = new Point(1140, 626);
        lblRecordsCount.Name = "lblRecordsCount";
        lblRecordsCount.Size = new Size(23, 24);
        lblRecordsCount.TabIndex = 10;
        lblRecordsCount.Text = "0";
        // 
        // lblSummary
        // 
        lblSummary.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        lblSummary.AutoSize = true;
        lblSummary.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblSummary.Location = new Point(170, 626);
        lblSummary.Name = "lblSummary";
        lblSummary.Size = new Size(58, 24);
        lblSummary.TabIndex = 8;
        lblSummary.Text = "ط¬ط§ظ‡ط²";
        // 
        // loadingPanel
        // 
        loadingPanel.BackColor = Color.FromArgb(100, 255, 255, 255);
        loadingPanel.Controls.Add(ll);
        loadingPanel.Controls.Add(dtpTo);
        loadingPanel.Controls.Add(label4);
        loadingPanel.Controls.Add(label3);
        loadingPanel.Controls.Add(dtpFrom);
        loadingPanel.Dock = DockStyle.Fill;
        loadingPanel.Location = new Point(0, 0);
        loadingPanel.Name = "loadingPanel";
        loadingPanel.Size = new Size(1200, 680);
        loadingPanel.TabIndex = 31;
        loadingPanel.Visible = false;
        // 
        // ll
        // 
        ll.AutoSize = true;
        ll.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
        ll.ForeColor = Color.Firebrick;
        ll.Location = new Point(540, 320);
        ll.Name = "ll";
        ll.Size = new Size(168, 29);
        ll.TabIndex = 0;
        ll.Text = "ط¬ط§ط±ظچ ط§ظ„طھط­ظ…ظٹظ„...";
        // 
        // dtpFrom
        // 
        dtpFrom.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        dtpFrom.Format = DateTimePickerFormat.Short;
        dtpFrom.Location = new Point(664, 52);
        dtpFrom.Name = "dtpFrom";
        dtpFrom.Size = new Size(161, 32);
        dtpFrom.TabIndex = 4;
        // 
        // dtpTo
        // 
        dtpTo.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        dtpTo.Format = DateTimePickerFormat.Short;
        dtpTo.Location = new Point(434, 52);
        dtpTo.Name = "dtpTo";
        dtpTo.Size = new Size(166, 32);
        dtpTo.TabIndex = 6;
        // 
        // label3
        // 
        label3.AutoSize = true;
        label3.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label3.Location = new Point(831, 58);
        label3.Name = "label3";
        label3.Size = new Size(46, 24);
        label3.TabIndex = 3;
        label3.Text = "ظ…ظ†:";
        // 
        // label4
        // 
        label4.AutoSize = true;
        label4.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label4.Location = new Point(606, 56);
        label4.Name = "label4";
        label4.Size = new Size(52, 24);
        label4.TabIndex = 5;
        label4.Text = "ط¥ظ„ظ‰:";
        // 
        // frmCashFlowReport
        // 
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.WhiteSmoke;
        CancelButton = btnClose;
        ClientSize = new Size(1200, 680);
        Controls.Add(lblMode);
        Controls.Add(btnRefresh);
        Controls.Add(btnExport);
        Controls.Add(dgvReport);
        Controls.Add(btnClose);
        Controls.Add(lblSummary);
        Controls.Add(label1);
        Controls.Add(lblRecordsCount);
        Controls.Add(loadingPanel);
        Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        Name = "frmCashFlowReport";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "طھظ‚ط±ظٹط± ط§ظ„طھط¯ظپظ‚ط§طھ ط§ظ„ظ†ظ‚ط¯ظٹط©";
        ((System.ComponentModel.ISupportInitialize)dgvReport).EndInit();
        loadingPanel.ResumeLayout(false);
        loadingPanel.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    private Label ll;
}

