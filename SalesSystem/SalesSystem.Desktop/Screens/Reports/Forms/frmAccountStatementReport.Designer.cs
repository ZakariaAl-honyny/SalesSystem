namespace SalesSystem.Desktop.Screens.Reports.Forms;

partial class frmAccountStatementReport
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
    private NumericUpDown nudPartyId;
    private ComboBox cmbPartyType;
    private Label label3;
    private Label label4;
    private Label label5;
    private Label label6;
    private Label ll;

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
        nudPartyId = new NumericUpDown();
        cmbPartyType = new ComboBox();
        label3 = new Label();
        label4 = new Label();
        label5 = new Label();
        label6 = new Label();
        ((System.ComponentModel.ISupportInitialize)dgvReport).BeginInit();
        loadingPanel.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudPartyId).BeginInit();
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
        lblMode.Text = "ظƒط´ظپ ط­ط³ط§ط¨ ط§ظ„ط·ط±ظپ";
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
        dgvReport.TabIndex = 11;
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
        btnRefresh.Location = new Point(1089, 49);
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
        btnExport.Size = new Size(110, 36);
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
        label1.Location = new Point(66, 626);
        label1.Name = "label1";
        label1.Size = new Size(145, 24);
        label1.TabIndex = 13;
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
        lblRecordsCount.TabIndex = 14;
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
        lblSummary.TabIndex = 12;
        lblSummary.Text = "ط¬ط§ظ‡ط²";
        // 
        // loadingPanel
        // 
        loadingPanel.BackColor = Color.FromArgb(100, 255, 255, 255);
        loadingPanel.Controls.Add(ll);
        loadingPanel.Controls.Add(btnRefresh);
        loadingPanel.Controls.Add(dtpTo);
        loadingPanel.Controls.Add(dtpFrom);
        loadingPanel.Controls.Add(label4);
        loadingPanel.Controls.Add(cmbPartyType);
        loadingPanel.Controls.Add(label3);
        loadingPanel.Controls.Add(label6);
        loadingPanel.Controls.Add(nudPartyId);
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
        dtpFrom.Location = new Point(349, 56);
        dtpFrom.Name = "dtpFrom";
        dtpFrom.Size = new Size(175, 32);
        dtpFrom.TabIndex = 8;
        // 
        // dtpTo
        // 
        dtpTo.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        dtpTo.Format = DateTimePickerFormat.Short;
        dtpTo.Location = new Point(110, 56);
        dtpTo.Name = "dtpTo";
        dtpTo.Size = new Size(175, 32);
        dtpTo.TabIndex = 10;
        // 
        // nudPartyId
        // 
        nudPartyId.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        nudPartyId.Location = new Point(582, 54);
        nudPartyId.Maximum = new decimal(new int[] { 99999, 0, 0, 0 });
        nudPartyId.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        nudPartyId.Name = "nudPartyId";
        nudPartyId.Size = new Size(80, 32);
        nudPartyId.TabIndex = 6;
        nudPartyId.Value = new decimal(new int[] { 1, 0, 0, 0 });
        // 
        // cmbPartyType
        // 
        cmbPartyType.BackColor = SystemColors.Control;
        cmbPartyType.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbPartyType.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        cmbPartyType.Items.AddRange(new object[] { "ط¹ظ…ظٹظ„", "ظ…ظˆط±ط¯" });
        cmbPartyType.Location = new Point(801, 53);
        cmbPartyType.Name = "cmbPartyType";
        cmbPartyType.Size = new Size(90, 32);
        cmbPartyType.TabIndex = 4;
        // 
        // label3
        // 
        label3.AutoSize = true;
        label3.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label3.Location = new Point(530, 62);
        label3.Name = "label3";
        label3.Size = new Size(46, 24);
        label3.TabIndex = 7;
        label3.Text = "ظ…ظ†:";
        // 
        // label4
        // 
        label4.AutoSize = true;
        label4.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label4.Location = new Point(291, 62);
        label4.Name = "label4";
        label4.Size = new Size(52, 24);
        label4.TabIndex = 9;
        label4.Text = "ط¥ظ„ظ‰:";
        // 
        // label5
        // 
        label5.AutoSize = true;
        label5.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label5.Location = new Point(240, 58);
        label5.Name = "label5";
        label5.Size = new Size(63, 24);
        label5.TabIndex = 3;
        label5.Text = "ط§ظ„ظ†ظˆط¹:";
        // 
        // label6
        // 
        label6.AutoSize = true;
        label6.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label6.Location = new Point(668, 58);
        label6.Name = "label6";
        label6.Size = new Size(118, 24);
        label6.TabIndex = 5;
        label6.Text = "ط±ظ‚ظ… ط§ظ„ط·ط±ظپ:";
        // 
        // frmAccountStatementReport
        // 
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.WhiteSmoke;
        CancelButton = btnClose;
        ClientSize = new Size(1200, 680);
        Controls.Add(lblMode);
        Controls.Add(btnExport);
        Controls.Add(label5);
        Controls.Add(dgvReport);
        Controls.Add(btnClose);
        Controls.Add(lblSummary);
        Controls.Add(lblRecordsCount);
        Controls.Add(loadingPanel);
        Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        Name = "frmAccountStatementReport";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "ظƒط´ظپ ط­ط³ط§ط¨ ط§ظ„ط·ط±ظپ";
        ((System.ComponentModel.ISupportInitialize)dgvReport).EndInit();
        loadingPanel.ResumeLayout(false);
        loadingPanel.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)nudPartyId).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
}

