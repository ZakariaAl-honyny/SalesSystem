namespace SalesSystem.Desktop.Screens.Reports.Forms;

partial class frmGeneralLedgerReport
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
    private NumericUpDown nudAccountId;
    private Label label3;
    private Label label4;
    private Label label5;

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
        dtpTo = new DateTimePicker();
        label4 = new Label();
        dtpFrom = new DateTimePicker();
        nudAccountId = new NumericUpDown();
        label3 = new Label();
        label5 = new Label();
        ((System.ComponentModel.ISupportInitialize)dgvReport).BeginInit();
        loadingPanel.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudAccountId).BeginInit();
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
        lblMode.Text = "دفتر الأستاذ العام";
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
        dgvReport.TabIndex = 9;
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
        btnRefresh.Text = "تحديث";
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
        btnExport.Size = new Size(162, 36);
        btnExport.TabIndex = 2;
        btnExport.Text = "تصدير Excel";
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
        btnClose.Text = "إغلاق";
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += btnClose_Click;
        // 
        // label1
        // 
        label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        label1.AutoSize = true;
        label1.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label1.Location = new Point(66, 626);
        label1.Name = "label1";
        label1.Size = new Size(145, 24);
        label1.TabIndex = 11;
        label1.Text = "عدد السجلات:";
        // 
        // lblRecordsCount
        // 
        lblRecordsCount.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        lblRecordsCount.AutoSize = true;
        lblRecordsCount.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblRecordsCount.Location = new Point(1140, 626);
        lblRecordsCount.Name = "lblRecordsCount";
        lblRecordsCount.Size = new Size(23, 24);
        lblRecordsCount.TabIndex = 12;
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
        lblSummary.TabIndex = 10;
        lblSummary.Text = "جاهز";
        // 
        // loadingPanel
        // 
        loadingPanel.BackColor = Color.FromArgb(100, 255, 255, 255);
        loadingPanel.Controls.Add(ll);
        loadingPanel.Controls.Add(dtpTo);
        loadingPanel.Controls.Add(label4);
        loadingPanel.Controls.Add(label5);
        loadingPanel.Controls.Add(dtpFrom);
        loadingPanel.Controls.Add(nudAccountId);
        loadingPanel.Controls.Add(label1);
        loadingPanel.Controls.Add(label3);
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
        ll.Text = "جارٍ التحميل...";
        // 
        // dtpTo
        // 
        dtpTo.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        dtpTo.Format = DateTimePickerFormat.Short;
        dtpTo.Location = new Point(215, 54);
        dtpTo.Name = "dtpTo";
        dtpTo.Size = new Size(164, 32);
        dtpTo.TabIndex = 8;
        // 
        // label4
        // 
        label4.AutoSize = true;
        label4.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label4.Location = new Point(385, 60);
        label4.Name = "label4";
        label4.Size = new Size(52, 24);
        label4.TabIndex = 7;
        label4.Text = "إلى:";
        // 
        // dtpFrom
        // 
        dtpFrom.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        dtpFrom.Format = DateTimePickerFormat.Short;
        dtpFrom.Location = new Point(443, 54);
        dtpFrom.Name = "dtpFrom";
        dtpFrom.Size = new Size(167, 32);
        dtpFrom.TabIndex = 6;
        // 
        // nudAccountId
        // 
        nudAccountId.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        nudAccountId.Location = new Point(668, 54);
        nudAccountId.Maximum = new decimal(new int[] { 99999, 0, 0, 0 });
        nudAccountId.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        nudAccountId.Name = "nudAccountId";
        nudAccountId.Size = new Size(103, 32);
        nudAccountId.TabIndex = 4;
        nudAccountId.Value = new decimal(new int[] { 1, 0, 0, 0 });
        nudAccountId.ValueChanged += nudAccountId_ValueChanged;
        // 
        // label3
        // 
        label3.AutoSize = true;
        label3.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label3.Location = new Point(616, 58);
        label3.Name = "label3";
        label3.Size = new Size(46, 24);
        label3.TabIndex = 5;
        label3.Text = "من:";
        // 
        // label5
        // 
        label5.AutoSize = true;
        label5.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label5.Location = new Point(777, 60);
        label5.Name = "label5";
        label5.Size = new Size(135, 24);
        label5.TabIndex = 3;
        label5.Text = "رقم الحساب:";
        // 
        // frmGeneralLedgerReport
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
        Controls.Add(lblRecordsCount);
        Controls.Add(loadingPanel);
        Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        Name = "frmGeneralLedgerReport";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "دفتر الأستاذ العام";
        ((System.ComponentModel.ISupportInitialize)dgvReport).EndInit();
        loadingPanel.ResumeLayout(false);
        loadingPanel.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)nudAccountId).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    private Label ll;
}

