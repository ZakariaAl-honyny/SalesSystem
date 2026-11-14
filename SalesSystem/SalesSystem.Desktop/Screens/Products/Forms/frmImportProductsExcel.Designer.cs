namespace SalesSystem.Desktop.Screens.Products.Forms;

partial class frmImportProductsExcel
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
        {
            components.Dispose();
        }
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
        gbFile = new GroupBox();
        btnSelectFile = new Button();
        btnDownloadTemplate = new Button();
        lblFileCaption = new Label();
        lblFilePath = new Label();
        gbOptions = new GroupBox();
        btnGenerateBarcodes = new Button();
        gbPreview = new GroupBox();
        dgvPreview = new DataGridView();
        pnlPreviewTop = new Panel();
        lblSearch = new Label();
        txtSearch = new TextBox();
        lblNoData = new Label();
        pnlSummary = new Panel();
        lblCount = new Label();
        lblSummary = new Label();
        pnlFooter = new Panel();
        btnSave = new Button();
        btnClose = new Button();
        cmsRows = new ContextMenuStrip(components);
        tsmiCopyCellValue = new ToolStripMenuItem();
        tsmiGenerateMissingBarcode = new ToolStripMenuItem();
        gbFile.SuspendLayout();
        gbOptions.SuspendLayout();
        gbPreview.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvPreview).BeginInit();
        pnlPreviewTop.SuspendLayout();
        pnlSummary.SuspendLayout();
        pnlFooter.SuspendLayout();
        cmsRows.SuspendLayout();
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
        lblMode.Size = new Size(1000, 48);
        lblMode.TabIndex = 0;
        lblMode.Text = "استيراد الأصناف من ملف Excel";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // gbFile
        // 
        gbFile.BackColor = Color.FromArgb(235, 245, 235);
        gbFile.Controls.Add(btnSelectFile);
        gbFile.Controls.Add(btnDownloadTemplate);
        gbFile.Controls.Add(lblFileCaption);
        gbFile.Controls.Add(lblFilePath);
        gbFile.Dock = DockStyle.Top;
        gbFile.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        gbFile.ForeColor = Color.FromArgb(14, 82, 52);
        gbFile.Location = new Point(0, 48);
        gbFile.Name = "gbFile";
        gbFile.Padding = new Padding(8);
        gbFile.Size = new Size(1000, 86);
        gbFile.TabIndex = 1;
        gbFile.TabStop = false;
        gbFile.Text = "ملف الاستيراد";
        // 
        // btnSelectFile
        // 
        btnSelectFile.BackColor = Color.FromArgb(14, 165, 103);
        btnSelectFile.FlatAppearance.BorderSize = 0;
        btnSelectFile.FlatStyle = FlatStyle.Flat;
        btnSelectFile.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnSelectFile.ForeColor = Color.White;
        btnSelectFile.Location = new Point(808, 34);
        btnSelectFile.Name = "btnSelectFile";
        btnSelectFile.Size = new Size(160, 38);
        btnSelectFile.TabIndex = 0;
        btnSelectFile.Text = "اختر ملف إكسل";
        btnSelectFile.UseVisualStyleBackColor = false;
        btnSelectFile.Click += btnSelectFile_Click;
        // 
        // btnDownloadTemplate
        // 
        btnDownloadTemplate.BackColor = Color.FromArgb(108, 117, 125);
        btnDownloadTemplate.FlatAppearance.BorderSize = 0;
        btnDownloadTemplate.FlatStyle = FlatStyle.Flat;
        btnDownloadTemplate.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnDownloadTemplate.ForeColor = Color.White;
        btnDownloadTemplate.Location = new Point(640, 34);
        btnDownloadTemplate.Name = "btnDownloadTemplate";
        btnDownloadTemplate.Size = new Size(160, 38);
        btnDownloadTemplate.TabIndex = 1;
        btnDownloadTemplate.Text = "تحميل نموذج";
        btnDownloadTemplate.UseVisualStyleBackColor = false;
        btnDownloadTemplate.Click += btnDownloadTemplate_Click;
        // 
        // lblFileCaption
        // 
        lblFileCaption.AutoSize = true;
        lblFileCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblFileCaption.Location = new Point(573, 41);
        lblFileCaption.Name = "lblFileCaption";
        lblFileCaption.Size = new Size(60, 24);
        lblFileCaption.TabIndex = 2;
        lblFileCaption.Text = "الملف:";
        // 
        // lblFilePath
        // 
        lblFilePath.BackColor = Color.White;
        lblFilePath.BorderStyle = BorderStyle.FixedSingle;
        lblFilePath.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblFilePath.ForeColor = Color.FromArgb(33, 37, 41);
        lblFilePath.Location = new Point(24, 38);
        lblFilePath.Name = "lblFilePath";
        lblFilePath.Size = new Size(540, 30);
        lblFilePath.TabIndex = 3;
        lblFilePath.Text = "لم يتم اختيار ملف بعد";
        lblFilePath.TextAlign = ContentAlignment.MiddleRight;
        // 
        // gbOptions
        // 
        gbOptions.BackColor = Color.FromArgb(235, 245, 235);
        gbOptions.Controls.Add(btnGenerateBarcodes);
        gbOptions.Dock = DockStyle.Top;
        gbOptions.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        gbOptions.ForeColor = Color.FromArgb(14, 82, 52);
        gbOptions.Location = new Point(0, 134);
        gbOptions.Name = "gbOptions";
        gbOptions.Padding = new Padding(8);
        gbOptions.Size = new Size(1000, 78);
        gbOptions.TabIndex = 2;
        gbOptions.TabStop = false;
        gbOptions.Text = "خيارات الاستيراد";
        // 
        // btnGenerateBarcodes
        // 
        btnGenerateBarcodes.BackColor = Color.FromArgb(23, 162, 184);
        btnGenerateBarcodes.FlatAppearance.BorderSize = 0;
        btnGenerateBarcodes.FlatStyle = FlatStyle.Flat;
        btnGenerateBarcodes.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnGenerateBarcodes.ForeColor = Color.White;
        btnGenerateBarcodes.Location = new Point(718, 27);
        btnGenerateBarcodes.Name = "btnGenerateBarcodes";
        btnGenerateBarcodes.Size = new Size(250, 38);
        btnGenerateBarcodes.TabIndex = 0;
        btnGenerateBarcodes.Text = "توليد باركود للأصناف الفارغة";
        btnGenerateBarcodes.UseVisualStyleBackColor = false;
        btnGenerateBarcodes.Click += btnGenerateBarcodes_Click;
        // 
        // gbPreview
        // 
        gbPreview.BackColor = Color.FromArgb(235, 245, 235);
        gbPreview.Controls.Add(dgvPreview);
        gbPreview.Controls.Add(pnlPreviewTop);
        gbPreview.Dock = DockStyle.Fill;
        gbPreview.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        gbPreview.ForeColor = Color.FromArgb(14, 82, 52);
        gbPreview.Location = new Point(0, 212);
        gbPreview.Name = "gbPreview";
        gbPreview.Padding = new Padding(10);
        gbPreview.Size = new Size(1000, 334);
        gbPreview.TabIndex = 3;
        gbPreview.TabStop = false;
        gbPreview.Text = "معاينة الصفوف";
        // 
        // dgvPreview
        // 
        dgvPreview.AllowUserToAddRows = false;
        dgvPreview.AllowUserToDeleteRows = false;
        dataGridViewCellStyle1.BackColor = Color.FromArgb(245, 252, 245);
        dgvPreview.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
        dgvPreview.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvPreview.BackgroundColor = Color.FromArgb(220, 240, 220);
        dgvPreview.BorderStyle = BorderStyle.None;
        dgvPreview.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
        dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
        dataGridViewCellStyle2.BackColor = Color.FromArgb(14, 165, 103);
        dataGridViewCellStyle2.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        dataGridViewCellStyle2.ForeColor = Color.White;
        dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(14, 165, 103);
        dataGridViewCellStyle2.SelectionForeColor = Color.White;
        dgvPreview.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
        dgvPreview.ColumnHeadersHeight = 45;
        dgvPreview.ContextMenuStrip = cmsRows;
        dataGridViewCellStyle3.BackColor = Color.White;
        dataGridViewCellStyle3.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        dataGridViewCellStyle3.ForeColor = Color.FromArgb(33, 37, 41);
        dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(255, 255, 180);
        dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(33, 37, 41);
        dgvPreview.DefaultCellStyle = dataGridViewCellStyle3;
        dgvPreview.Dock = DockStyle.Fill;
        dgvPreview.EnableHeadersVisualStyles = false;
        dgvPreview.GridColor = Color.FromArgb(205, 225, 205);
        dgvPreview.Location = new Point(10, 54);
        dgvPreview.Margin = new Padding(0);
        dgvPreview.Name = "dgvPreview";
        dgvPreview.ReadOnly = true;
        dgvPreview.RightToLeft = RightToLeft.Yes;
        dgvPreview.RowHeadersVisible = false;
        dgvPreview.RowTemplate.Height = 32;
        dgvPreview.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvPreview.Size = new Size(980, 270);
        dgvPreview.TabIndex = 1;
        dgvPreview.CellValueChanged += dgvPreview_CellValueChanged;
        // 
        // pnlPreviewTop
        // 
        pnlPreviewTop.BackColor = Color.FromArgb(235, 240, 235);
        pnlPreviewTop.Controls.Add(lblSearch);
        pnlPreviewTop.Controls.Add(txtSearch);
        pnlPreviewTop.Controls.Add(lblNoData);
        pnlPreviewTop.Dock = DockStyle.Top;
        pnlPreviewTop.Location = new Point(10, 34);
        pnlPreviewTop.Name = "pnlPreviewTop";
        pnlPreviewTop.Size = new Size(980, 44);
        pnlPreviewTop.TabIndex = 0;
        // 
        // lblSearch
        // 
        lblSearch.AutoSize = true;
        lblSearch.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblSearch.Location = new Point(870, 10);
        lblSearch.Name = "lblSearch";
        lblSearch.Size = new Size(60, 24);
        lblSearch.TabIndex = 0;
        lblSearch.Text = "بحث:";
        // 
        // txtSearch
        // 
        txtSearch.BackColor = Color.White;
        txtSearch.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        txtSearch.Location = new Point(700, 8);
        txtSearch.Name = "txtSearch";
        txtSearch.RightToLeft = RightToLeft.Yes;
        txtSearch.Size = new Size(160, 32);
        txtSearch.TabIndex = 1;
        txtSearch.TextChanged += txtSearch_TextChanged;
        // 
        // lblNoData
        // 
        lblNoData.AutoSize = true;
        lblNoData.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblNoData.ForeColor = Color.FromArgb(14, 82, 52);
        lblNoData.Location = new Point(24, 12);
        lblNoData.Name = "lblNoData";
        lblNoData.Size = new Size(380, 24);
        lblNoData.TabIndex = 2;
        lblNoData.Text = "لا توجد بيانات — اختر ملف Excel لبدء المعاينة";
        // 
        // pnlSummary
        // 
        pnlSummary.BackColor = Color.FromArgb(235, 240, 235);
        pnlSummary.Controls.Add(lblCount);
        pnlSummary.Controls.Add(lblSummary);
        pnlSummary.Dock = DockStyle.Bottom;
        pnlSummary.Location = new Point(0, 546);
        pnlSummary.Name = "pnlSummary";
        pnlSummary.Size = new Size(1000, 46);
        pnlSummary.TabIndex = 4;
        // 
        // lblCount
        // 
        lblCount.AutoSize = true;
        lblCount.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblCount.ForeColor = Color.FromArgb(14, 82, 52);
        lblCount.Location = new Point(790, 11);
        lblCount.Name = "lblCount";
        lblCount.Size = new Size(180, 24);
        lblCount.TabIndex = 0;
        lblCount.Text = "عدد الأصناف: 0";
        // 
        // lblSummary
        // 
        lblSummary.AutoSize = true;
        lblSummary.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblSummary.ForeColor = Color.FromArgb(14, 82, 52);
        lblSummary.Location = new Point(450, 11);
        lblSummary.Name = "lblSummary";
        lblSummary.Size = new Size(300, 24);
        lblSummary.TabIndex = 1;
        lblSummary.Text = "المقبولة: 0  ·  المرفوضة: 0";
        // 
        // pnlFooter
        // 
        pnlFooter.BackColor = Color.FromArgb(235, 240, 235);
        pnlFooter.Controls.Add(btnSave);
        pnlFooter.Controls.Add(btnClose);
        pnlFooter.Dock = DockStyle.Bottom;
        pnlFooter.Location = new Point(0, 592);
        pnlFooter.Name = "pnlFooter";
        pnlFooter.Padding = new Padding(10);
        pnlFooter.Size = new Size(1000, 68);
        pnlFooter.TabIndex = 5;
        // 
        // btnSave
        // 
        btnSave.BackColor = Color.FromArgb(14, 165, 103);
        btnSave.FlatAppearance.BorderSize = 0;
        btnSave.FlatStyle = FlatStyle.Flat;
        btnSave.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnSave.ForeColor = Color.White;
        btnSave.Location = new Point(788, 12);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(200, 44);
        btnSave.TabIndex = 0;
        btnSave.Text = "اعتماد وحفظ الأصناف (F2)";
        btnSave.UseVisualStyleBackColor = false;
        btnSave.Click += btnSave_Click;
        // 
        // btnClose
        // 
        btnClose.BackColor = Color.FromArgb(108, 117, 125);
        btnClose.DialogResult = DialogResult.Cancel;
        btnClose.FlatAppearance.BorderSize = 0;
        btnClose.FlatStyle = FlatStyle.Flat;
        btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnClose.ForeColor = Color.White;
        btnClose.Location = new Point(656, 12);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(120, 44);
        btnClose.TabIndex = 1;
        btnClose.Text = "إغلاق (Esc)";
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += btnClose_Click;
        // 
        // cmsRows — RULE-N23: قائمة سطر السياق لشبكة المعاينة (تُعلَن في المصمّم — RULE-N145)
        // 
        cmsRows.BackColor = Color.White;
        cmsRows.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        cmsRows.ImageScalingSize = new Size(24, 24);
        cmsRows.Items.AddRange(new ToolStripItem[] { tsmiCopyCellValue, tsmiGenerateMissingBarcode });
        cmsRows.Name = "cmsRows";
        cmsRows.RightToLeft = RightToLeft.Yes;
        cmsRows.Size = new Size(240, 68);
        cmsRows.Opening += cmsRows_Opening;
        // 
        // tsmiCopyCellValue
        // 
        tsmiCopyCellValue.Name = "tsmiCopyCellValue";
        tsmiCopyCellValue.Size = new Size(239, 30);
        tsmiCopyCellValue.Text = "نسخ قيمة الخلية";
        tsmiCopyCellValue.Click += tsmiCopyCellValue_Click;
        // 
        // tsmiGenerateMissingBarcode
        // 
        tsmiGenerateMissingBarcode.Name = "tsmiGenerateMissingBarcode";
        tsmiGenerateMissingBarcode.Size = new Size(239, 30);
        tsmiGenerateMissingBarcode.Text = "توليد الباركود المفقود";
        tsmiGenerateMissingBarcode.Click += tsmiGenerateMissingBarcode_Click;
        // 
        // frmImportProductsExcel
        // 
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.FromArgb(170, 220, 170);
        CancelButton = btnClose;
        ClientSize = new Size(1000, 660);
        Controls.Add(gbPreview);
        Controls.Add(pnlSummary);
        Controls.Add(pnlFooter);
        Controls.Add(gbOptions);
        Controls.Add(gbFile);
        Controls.Add(lblMode);
        Font = new Font("Tahoma", 10F);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "frmImportProductsExcel";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "استيراد الأصناف من Excel";
        gbFile.ResumeLayout(false);
        gbFile.PerformLayout();
        gbOptions.ResumeLayout(false);
        gbPreview.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvPreview).EndInit();
        pnlPreviewTop.ResumeLayout(false);
        pnlPreviewTop.PerformLayout();
        pnlSummary.ResumeLayout(false);
        pnlSummary.PerformLayout();
        pnlFooter.ResumeLayout(false);
        cmsRows.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label lblMode;
    private GroupBox gbFile;
    private Button btnSelectFile;
    private Button btnDownloadTemplate;
    private Label lblFileCaption;
    private Label lblFilePath;
    private GroupBox gbOptions;
    private Button btnGenerateBarcodes;
    private GroupBox gbPreview;
    private DataGridView dgvPreview;
    private Panel pnlPreviewTop;
    private Label lblSearch;
    private TextBox txtSearch;
    private Label lblNoData;
    private Panel pnlSummary;
    private Label lblCount;
    private Label lblSummary;
    private Panel pnlFooter;
    private Button btnSave;
    private Button btnClose;
    private ContextMenuStrip cmsRows;
    private ToolStripMenuItem tsmiCopyCellValue;
    private ToolStripMenuItem tsmiGenerateMissingBarcode;
}
