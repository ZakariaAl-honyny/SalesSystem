namespace SalesSystem.Desktop.Screens.Reports.Forms;

partial class frmPurchasesByProductReport
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
        label3 = new Label();
        dtpFrom = new DateTimePicker();
        label4 = new Label();
        dtpTo = new DateTimePicker();
        ((System.ComponentModel.ISupportInitialize)dgvReport).BeginInit();
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
        lblMode.Text = "المشتريات حسب المنتج";
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
        dgvReport.Location = new Point(11, 95);
        dgvReport.Margin = new Padding(0, 4, 0, 4);
        dgvReport.Name = "dgvReport";
        dgvReport.ReadOnly = true;
        dgvReport.RowHeadersWidth = 62;
        dgvReport.RowTemplate.Height = 28;
        dgvReport.Size = new Size(1178, 530);
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
        btnRefresh.Location = new Point(1018, 47);
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
        btnExport.Location = new Point(837, 47);
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
        btnClose.Location = new Point(11, 633);
        btnClose.Margin = new Padding(4);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(181, 40);
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
        label1.Location = new Point(1001, 642);
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
        lblRecordsCount.Location = new Point(1154, 642);
        lblRecordsCount.Margin = new Padding(4);
        lblRecordsCount.Name = "lblRecordsCount";
        lblRecordsCount.Size = new Size(23, 24);
        lblRecordsCount.TabIndex = 4;
        lblRecordsCount.Text = "0";
        // 
        // label3
        // 
        label3.AutoSize = true;
        label3.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label3.Location = new Point(11, 59);
        label3.Margin = new Padding(4);
        label3.Name = "label3";
        label3.Size = new Size(46, 24);
        label3.TabIndex = 9;
        label3.Text = "من:";
        // 
        // dtpFrom
        // 
        dtpFrom.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        dtpFrom.Format = DateTimePickerFormat.Short;
        dtpFrom.Location = new Point(65, 55);
        dtpFrom.Margin = new Padding(4);
        dtpFrom.Name = "dtpFrom";
        dtpFrom.Size = new Size(165, 32);
        dtpFrom.TabIndex = 10;
        // 
        // label4
        // 
        label4.AutoSize = true;
        label4.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label4.Location = new Point(238, 59);
        label4.Margin = new Padding(4);
        label4.Name = "label4";
        label4.Size = new Size(52, 24);
        label4.TabIndex = 11;
        label4.Text = "إلى:";
        // 
        // dtpTo
        // 
        dtpTo.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        dtpTo.Format = DateTimePickerFormat.Short;
        dtpTo.Location = new Point(298, 55);
        dtpTo.Margin = new Padding(4);
        dtpTo.Name = "dtpTo";
        dtpTo.Size = new Size(166, 32);
        dtpTo.TabIndex = 12;
        // 
        // frmPurchasesByProductReport
        // 
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.WhiteSmoke;
        CancelButton = btnClose;
        ClientSize = new Size(1200, 680);
        Controls.Add(dtpTo);
        Controls.Add(label4);
        Controls.Add(dtpFrom);
        Controls.Add(label3);
        Controls.Add(lblRecordsCount);
        Controls.Add(label1);
        Controls.Add(btnExport);
        Controls.Add(btnRefresh);
        Controls.Add(dgvReport);
        Controls.Add(lblMode);
        Controls.Add(btnClose);
        Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        Name = "frmPurchasesByProductReport";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "المشتريات حسب المنتج";
        Load += frmPurchasesByProductReport_Load;
        ((System.ComponentModel.ISupportInitialize)dgvReport).EndInit();
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
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.DateTimePicker dtpFrom;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.DateTimePicker dtpTo;
}

