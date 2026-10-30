namespace SalesSystem.Desktop.Screens.Reports.Forms;

partial class frmAgingReport
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
        lblMode = new Label();
        dgvReport = new DataGridView();
        btnRefresh = new Button();
        btnExport = new Button();
        btnClose = new Button();
        label1 = new Label();
        lblRecordsCount = new Label();
        loadingPanel = new Panel();
        ((System.ComponentModel.ISupportInitialize)dgvReport).BeginInit();
        loadingPanel.SuspendLayout();
        SuspendLayout();
        // 
        // lblMode
        // 
        lblMode.BackColor = Color.WhiteSmoke;
        lblMode.Dock = DockStyle.Top;
        lblMode.FlatStyle = FlatStyle.Flat;
        lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblMode.ForeColor = Color.Firebrick;
        lblMode.Location = new Point(0, 0);
        lblMode.Margin = new Padding(4);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(1200, 39);
        lblMode.TabIndex = 30;
        lblMode.Text = "تقرير الأعمار المدينة";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // dgvReport
        // 
        dgvReport.AllowUserToAddRows = false;
        dgvReport.AllowUserToDeleteRows = false;
        dgvReport.AllowUserToOrderColumns = true;
        dgvReport.BackgroundColor = SystemColors.Window;
        dgvReport.BorderStyle = BorderStyle.Fixed3D;
        dgvReport.ColumnHeadersHeight = 50;
        dgvReport.Location = new Point(9, 96);
        dgvReport.Margin = new Padding(0, 4, 0, 4);
        dgvReport.Name = "dgvReport";
        dgvReport.ReadOnly = true;
        dgvReport.RowHeadersWidth = 62;
        dgvReport.RowTemplate.Height = 28;
        dgvReport.Size = new Size(1178, 508);
        dgvReport.TabIndex = 34;
        dgvReport.TabStop = false;
        // 
        // btnRefresh
        // 
        btnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnRefresh.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        btnRefresh.BackColor = Color.White;
        btnRefresh.FlatAppearance.BorderColor = Color.Black;
        btnRefresh.FlatAppearance.BorderSize = 2;
        btnRefresh.FlatStyle = FlatStyle.Popup;
        btnRefresh.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnRefresh.Location = new Point(1018, 48);
        btnRefresh.Margin = new Padding(4);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(173, 40);
        btnRefresh.TabIndex = 5;
        btnRefresh.Text = "تحديث";
        btnRefresh.UseVisualStyleBackColor = true;
        btnRefresh.Click += btnRefresh_Click;
        // 
        // btnExport
        // 
        btnExport.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnExport.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        btnExport.BackColor = Color.White;
        btnExport.FlatAppearance.BorderColor = Color.Black;
        btnExport.FlatAppearance.BorderSize = 2;
        btnExport.FlatStyle = FlatStyle.Popup;
        btnExport.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnExport.ForeColor = Color.FromArgb(46, 125, 50);
        btnExport.Location = new Point(190, 47);
        btnExport.Margin = new Padding(4);
        btnExport.Name = "btnExport";
        btnExport.Size = new Size(173, 40);
        btnExport.TabIndex = 6;
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
        btnClose.ImageAlign = ContentAlignment.MiddleRight;
        btnClose.Location = new Point(11, 607);
        btnClose.Margin = new Padding(4);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(181, 61);
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
        label1.Location = new Point(940, 607);
        label1.Margin = new Padding(4);
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
        lblRecordsCount.Location = new Point(1100, 607);
        lblRecordsCount.Margin = new Padding(4);
        lblRecordsCount.Name = "lblRecordsCount";
        lblRecordsCount.Size = new Size(23, 24);
        lblRecordsCount.TabIndex = 4;
        lblRecordsCount.Text = "0";
        // 
        // loadingPanel
        // 
        loadingPanel.BackColor = Color.FromArgb(100, 255, 255, 255);
        loadingPanel.Controls.Add(btnExport);
        loadingPanel.Controls.Add(dgvReport);
        loadingPanel.Dock = DockStyle.Fill;
        loadingPanel.Location = new Point(0, 0);
        loadingPanel.Name = "loadingPanel";
        loadingPanel.Size = new Size(1200, 680);
        loadingPanel.TabIndex = 35;
        loadingPanel.Visible = false;
        // 
        // frmAgingReport
        // 
        AcceptButton = btnClose;
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.WhiteSmoke;
        CancelButton = btnClose;
        ClientSize = new Size(1200, 680);
        Controls.Add(lblRecordsCount);
        Controls.Add(label1);
        Controls.Add(btnRefresh);
        Controls.Add(lblMode);
        Controls.Add(btnClose);
        Controls.Add(loadingPanel);
        Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        Name = "frmAgingReport";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "تقرير الأعمار المدينة";
        Load += frmAgingReport_Load;
        ((System.ComponentModel.ISupportInitialize)dgvReport).EndInit();
        loadingPanel.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private System.Windows.Forms.Label lblMode;
    private System.Windows.Forms.DataGridView dgvReport;
    private System.Windows.Forms.Button btnRefresh;
    private System.Windows.Forms.Button btnExport;
    private System.Windows.Forms.Button btnClose;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label lblRecordsCount;
    private System.Windows.Forms.Panel loadingPanel;
}

