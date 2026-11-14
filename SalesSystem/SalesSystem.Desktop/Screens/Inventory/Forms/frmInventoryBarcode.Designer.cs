namespace SalesSystem.Desktop.Screens.Inventory.Forms;

partial class frmInventoryBarcode
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            components?.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        errorProvider = new ErrorProvider(components);
        tooltip = new ToolTip(components);
        lblMode = new Label();
        gbToolbar = new GroupBox();
        btnPrint = new Button();
        btnClose = new Button();
        gbInfo = new GroupBox();
        lblProduct = new Label();
        cmbProduct = new ComboBox();
        lblScanBarcode = new Label();
        txtBarcode = new TextBox();
        lblBarcode = new Label();
        lblBarcodeValue = new Label();
        lblLabelsCount = new Label();
        nudLabelsCount = new NumericUpDown();
        lblLabelSize = new Label();
        cmbLabelSize = new ComboBox();
        gbPreview = new GroupBox();
        pnlBarcodePreview = new Panel();
        ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
        gbToolbar.SuspendLayout();
        gbInfo.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudLabelsCount).BeginInit();
        gbPreview.SuspendLayout();
        SuspendLayout();
        // 
        // errorProvider
        // 
        errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        errorProvider.ContainerControl = this;
        // 
        // tooltip
        // 
        tooltip.AutoPopDelay = 6000;
        tooltip.InitialDelay = 300;
        tooltip.ReshowDelay = 100;
        // 
        // lblMode
        // 
        lblMode.BackColor = Color.FromArgb(14, 165, 103);
        lblMode.Dock = DockStyle.Top;
        lblMode.FlatStyle = FlatStyle.Flat;
        lblMode.Font = new Font("Tahoma", 14F, FontStyle.Bold);
        lblMode.ForeColor = Color.White;
        lblMode.Location = new Point(0, 0);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(580, 42);
        lblMode.TabIndex = 0;
        lblMode.Text = "طباعة باركود الأصناف";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // gbToolbar
        // 
        gbToolbar.BackColor = Color.FromArgb(235, 245, 235);
        gbToolbar.Controls.Add(btnClose);
        gbToolbar.Controls.Add(btnPrint);
        gbToolbar.Dock = DockStyle.Top;
        gbToolbar.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        gbToolbar.ForeColor = Color.FromArgb(14, 82, 52);
        gbToolbar.Location = new Point(0, 42);
        gbToolbar.Name = "gbToolbar";
        gbToolbar.Padding = new Padding(8);
        gbToolbar.RightToLeft = RightToLeft.Yes;
        gbToolbar.Size = new Size(580, 68);
        gbToolbar.TabIndex = 1;
        gbToolbar.TabStop = false;
        gbToolbar.Text = "أوامر الملصق";
        // 
        // btnPrint
        // 
        btnPrint.BackColor = Color.FromArgb(25, 135, 84);
        btnPrint.Cursor = Cursors.Hand;
        btnPrint.FlatAppearance.BorderSize = 0;
        btnPrint.FlatStyle = FlatStyle.Flat;
        btnPrint.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnPrint.ForeColor = Color.White;
        btnPrint.Location = new Point(12, 16);
        btnPrint.MinimumSize = new Size(100, 34);
        btnPrint.Name = "btnPrint";
        btnPrint.Size = new Size(150, 36);
        btnPrint.TabIndex = 0;
        btnPrint.Text = "طباعة الملصقات";
        btnPrint.UseVisualStyleBackColor = false;
        btnPrint.Click += btnPrint_Click;
        tooltip.SetToolTip(btnPrint, "طباعة الملصقات (نسخة تجريبية)");
        // 
        // btnClose
        // 
        btnClose.BackColor = Color.FromArgb(108, 117, 125);
        btnClose.Cursor = Cursors.Hand;
        btnClose.DialogResult = DialogResult.Cancel;
        btnClose.FlatAppearance.BorderSize = 0;
        btnClose.FlatStyle = FlatStyle.Flat;
        btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnClose.ForeColor = Color.White;
        btnClose.Location = new Point(172, 16);
        btnClose.MinimumSize = new Size(100, 34);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(140, 36);
        btnClose.TabIndex = 1;
        btnClose.Text = "إغلاق";
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += btnClose_Click;
        tooltip.SetToolTip(btnClose, "إغلاق النافذة (Esc)");
        // 
        // gbInfo
        // 
        gbInfo.BackColor = Color.FromArgb(235, 245, 235);
        gbInfo.Controls.Add(cmbLabelSize);
        gbInfo.Controls.Add(lblLabelSize);
        gbInfo.Controls.Add(nudLabelsCount);
        gbInfo.Controls.Add(lblLabelsCount);
        gbInfo.Controls.Add(lblBarcodeValue);
        gbInfo.Controls.Add(lblBarcode);
        gbInfo.Controls.Add(txtBarcode);
        gbInfo.Controls.Add(lblScanBarcode);
        gbInfo.Controls.Add(cmbProduct);
        gbInfo.Controls.Add(lblProduct);
        gbInfo.Dock = DockStyle.Top;
        gbInfo.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        gbInfo.ForeColor = Color.FromArgb(14, 82, 52);
        gbInfo.Location = new Point(0, 110);
        gbInfo.Name = "gbInfo";
        gbInfo.Padding = new Padding(8);
        gbInfo.RightToLeft = RightToLeft.Yes;
        gbInfo.Size = new Size(580, 200);
        gbInfo.TabIndex = 2;
        gbInfo.TabStop = false;
        gbInfo.Text = "بيانات الملصق";
        // 
        // lblProduct
        // 
        lblProduct.AutoSize = true;
        lblProduct.Font = new Font("Tahoma", 10F);
        lblProduct.ForeColor = Color.FromArgb(33, 37, 41);
        lblProduct.Location = new Point(12, 32);
        lblProduct.Name = "lblProduct";
        lblProduct.Size = new Size(60, 21);
        lblProduct.TabIndex = 0;
        lblProduct.Text = "الصنف:";
        // 
        // cmbProduct
        // 
        cmbProduct.BackColor = Color.White;
        cmbProduct.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbProduct.FlatStyle = FlatStyle.Flat;
        cmbProduct.Font = new Font("Tahoma", 10F);
        cmbProduct.ForeColor = Color.FromArgb(33, 37, 41);
        cmbProduct.FormattingEnabled = true;
        cmbProduct.Location = new Point(100, 28);
        cmbProduct.Name = "cmbProduct";
        cmbProduct.Size = new Size(430, 29);
        cmbProduct.TabIndex = 1;
        cmbProduct.SelectedIndexChanged += cmbProduct_SelectedIndexChanged;
        tooltip.SetToolTip(cmbProduct, "اختر الصنف لعرض الباركود الخاص به");
        // 
        // lblScanBarcode
        // 
        lblScanBarcode.AutoSize = true;
        lblScanBarcode.Font = new Font("Tahoma", 10F);
        lblScanBarcode.ForeColor = Color.FromArgb(33, 37, 41);
        lblScanBarcode.Location = new Point(12, 78);
        lblScanBarcode.Name = "lblScanBarcode";
        lblScanBarcode.Size = new Size(236, 21);
        lblScanBarcode.TabIndex = 2;
        lblScanBarcode.Text = "بحث بالباركود (امسح أو اكتب ثم Enter):";
        // 
        // txtBarcode
        // 
        txtBarcode.BackColor = Color.White;
        txtBarcode.Font = new Font("Tahoma", 10F);
        txtBarcode.ForeColor = Color.FromArgb(33, 37, 41);
        txtBarcode.Location = new Point(300, 74);
        txtBarcode.Name = "txtBarcode";
        txtBarcode.Size = new Size(230, 27);
        txtBarcode.TabIndex = 3;
        txtBarcode.TextAlign = HorizontalAlignment.Center;
        txtBarcode.KeyDown += txtBarcode_KeyDown;
        tooltip.SetToolTip(txtBarcode, "امسح الباركود ثم Enter — يُحدَّد الصنف فوراً ويبقى الحقل جاهزاً للمسح التالي");
        // 
        // lblBarcode
        // 
        lblBarcode.AutoSize = true;
        lblBarcode.Font = new Font("Tahoma", 10F);
        lblBarcode.ForeColor = Color.FromArgb(33, 37, 41);
        lblBarcode.Location = new Point(12, 124);
        lblBarcode.Name = "lblBarcode";
        lblBarcode.Size = new Size(78, 21);
        lblBarcode.TabIndex = 4;
        lblBarcode.Text = "الباركود:";
        // 
        // lblBarcodeValue
        // 
        lblBarcodeValue.BackColor = Color.White;
        lblBarcodeValue.BorderStyle = BorderStyle.FixedSingle;
        lblBarcodeValue.Font = new Font("Consolas", 10F, FontStyle.Bold);
        lblBarcodeValue.ForeColor = Color.FromArgb(33, 37, 41);
        lblBarcodeValue.Location = new Point(112, 120);
        lblBarcodeValue.Name = "lblBarcodeValue";
        lblBarcodeValue.Size = new Size(418, 28);
        lblBarcodeValue.TabIndex = 5;
        lblBarcodeValue.Text = "----------";
        lblBarcodeValue.TextAlign = ContentAlignment.MiddleCenter;
        tooltip.SetToolTip(lblBarcodeValue, "باركود الصنف المحدد (يتم تعبئته تلقائياً)");
        // 
        // lblLabelsCount
        // 
        lblLabelsCount.AutoSize = true;
        lblLabelsCount.Font = new Font("Tahoma", 10F);
        lblLabelsCount.ForeColor = Color.FromArgb(33, 37, 41);
        lblLabelsCount.Location = new Point(12, 170);
        lblLabelsCount.Name = "lblLabelsCount";
        lblLabelsCount.Size = new Size(100, 21);
        lblLabelsCount.TabIndex = 6;
        lblLabelsCount.Text = "عدد الملصقات:";
        // 
        // nudLabelsCount
        // 
        nudLabelsCount.BackColor = Color.White;
        nudLabelsCount.Font = new Font("Tahoma", 10F);
        nudLabelsCount.ForeColor = Color.FromArgb(33, 37, 41);
        nudLabelsCount.Location = new Point(140, 166);
        nudLabelsCount.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
        nudLabelsCount.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        nudLabelsCount.Name = "nudLabelsCount";
        nudLabelsCount.Size = new Size(150, 27);
        nudLabelsCount.TabIndex = 7;
        nudLabelsCount.TextAlign = HorizontalAlignment.Center;
        nudLabelsCount.Value = new decimal(new int[] { 10, 0, 0, 0 });
        tooltip.SetToolTip(nudLabelsCount, "عدد ملصقات الباركود المطلوب طباعتها (1 - 1000)");
        // 
        // lblLabelSize
        // 
        lblLabelSize.AutoSize = true;
        lblLabelSize.Font = new Font("Tahoma", 10F);
        lblLabelSize.ForeColor = Color.FromArgb(33, 37, 41);
        lblLabelSize.Location = new Point(320, 170);
        lblLabelSize.Name = "lblLabelSize";
        lblLabelSize.Size = new Size(83, 21);
        lblLabelSize.TabIndex = 8;
        lblLabelSize.Text = "حجم الملصق:";
        // 
        // cmbLabelSize
        // 
        cmbLabelSize.BackColor = Color.White;
        cmbLabelSize.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbLabelSize.FlatStyle = FlatStyle.Flat;
        cmbLabelSize.Font = new Font("Tahoma", 10F);
        cmbLabelSize.ForeColor = Color.FromArgb(33, 37, 41);
        cmbLabelSize.FormattingEnabled = true;
        cmbLabelSize.Items.AddRange(new object[] { "صغير", "متوسط", "كبير" });
        cmbLabelSize.Location = new Point(420, 166);
        cmbLabelSize.Name = "cmbLabelSize";
        cmbLabelSize.Size = new Size(110, 29);
        cmbLabelSize.TabIndex = 9;
        tooltip.SetToolTip(cmbLabelSize, "اختر حجم ملصق الباركود");
        // 
        // gbPreview
        // 
        gbPreview.BackColor = Color.FromArgb(235, 245, 235);
        gbPreview.Controls.Add(pnlBarcodePreview);
        gbPreview.Dock = DockStyle.Fill;
        gbPreview.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        gbPreview.ForeColor = Color.FromArgb(14, 82, 52);
        gbPreview.Location = new Point(0, 310);
        gbPreview.Name = "gbPreview";
        gbPreview.Padding = new Padding(8);
        gbPreview.RightToLeft = RightToLeft.Yes;
        gbPreview.Size = new Size(580, 390);
        gbPreview.TabIndex = 3;
        gbPreview.TabStop = false;
        gbPreview.Text = "معاينة الملصق";
        // 
        // pnlBarcodePreview
        // 
        pnlBarcodePreview.BackColor = Color.White;
        pnlBarcodePreview.BorderStyle = BorderStyle.FixedSingle;
        pnlBarcodePreview.Dock = DockStyle.Fill;
        pnlBarcodePreview.Location = new Point(8, 24);
        pnlBarcodePreview.Name = "pnlBarcodePreview";
        pnlBarcodePreview.Size = new Size(564, 358);
        pnlBarcodePreview.TabIndex = 0;
        pnlBarcodePreview.Paint += pnlBarcodePreview_Paint;
        tooltip.SetToolTip(pnlBarcodePreview, "معاينة الباركود قبل الطباعة");
        // 
        // frmInventoryBarcode
        // 
        AcceptButton = btnPrint;
        AutoScaleMode = AutoScaleMode.None;
        AutoValidate = AutoValidate.EnableAllowFocusChange;
        BackColor = Color.FromArgb(170, 220, 170);
        CancelButton = btnClose;
        ClientSize = new Size(580, 700);
        Controls.Add(gbPreview);
        Controls.Add(gbInfo);
        Controls.Add(gbToolbar);
        Controls.Add(lblMode);
        Font = new Font("Tahoma", 12F);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "frmInventoryBarcode";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "طباعة باركود";
        Load += frmInventoryBarcode_Load;
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        gbToolbar.ResumeLayout(false);
        gbInfo.ResumeLayout(false);
        gbInfo.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)nudLabelsCount).EndInit();
        gbPreview.ResumeLayout(false);
        ResumeLayout(false);
    }

    private ErrorProvider errorProvider;
    private ToolTip tooltip;
    private Label lblMode;
    private GroupBox gbToolbar;
    private Button btnPrint;
    private Button btnClose;
    private GroupBox gbInfo;
    private Label lblProduct;
    private ComboBox cmbProduct;
    private Label lblScanBarcode;
    private TextBox txtBarcode;
    private Label lblBarcode;
    private Label lblBarcodeValue;
    private Label lblLabelsCount;
    private NumericUpDown nudLabelsCount;
    private Label lblLabelSize;
    private ComboBox cmbLabelSize;
    private GroupBox gbPreview;
    private Panel pnlBarcodePreview;
}
