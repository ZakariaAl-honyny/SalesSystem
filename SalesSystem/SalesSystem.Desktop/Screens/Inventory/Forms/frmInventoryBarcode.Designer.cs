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
        btnPrint = new Button();
        btnClose = new Button();
        cmbProduct = new ComboBox();
        txtBarcode = new TextBox();
        lblBarcodeValue = new Label();
        nudLabelsCount = new NumericUpDown();
        cmbLabelSize = new ComboBox();
        pnlBarcodePreview = new Panel();
        lblMode = new Label();
        gbToolbar = new GroupBox();
        gbInfo = new GroupBox();
        lblLabelSize = new Label();
        lblLabelsCount = new Label();
        lblBarcode = new Label();
        lblScanBarcode = new Label();
        lblProduct = new Label();
        gbPreview = new GroupBox();
        ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudLabelsCount).BeginInit();
        gbToolbar.SuspendLayout();
        gbInfo.SuspendLayout();
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
        // btnPrint
        // 
        btnPrint.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(23), Convert.ToInt32(162), Convert.ToInt32(184));
        btnPrint.Cursor = Cursors.Hand;
        btnPrint.FlatAppearance.BorderSize = 0;
        btnPrint.FlatStyle = FlatStyle.Flat;
        btnPrint.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnPrint.ForeColor = System.Drawing.Color.White;
        btnPrint.Location = new Point(491, 26);
        btnPrint.MinimumSize = new Size(100, 34);
        btnPrint.Name = "btnPrint";
        btnPrint.Size = new Size(150, 36);
        btnPrint.TabIndex = 0;
        btnPrint.Text = "طباعة الملصقات";
        tooltip.SetToolTip(btnPrint, "طباعة الملصقات (نسخة تجريبية)");
        btnPrint.UseVisualStyleBackColor = false;
        btnPrint.Click += btnPrint_Click;
        // 
        // btnClose
        // 
        btnClose.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(108), Convert.ToInt32(117), Convert.ToInt32(125));
        btnClose.Cursor = Cursors.Hand;
        btnClose.DialogResult = DialogResult.Cancel;
        btnClose.FlatAppearance.BorderSize = 0;
        btnClose.FlatStyle = FlatStyle.Flat;
        btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnClose.ForeColor = System.Drawing.Color.White;
        btnClose.Location = new Point(346, 26);
        btnClose.MinimumSize = new Size(100, 34);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(140, 36);
        btnClose.TabIndex = 1;
        btnClose.Text = "إغلاق";
        tooltip.SetToolTip(btnClose, "إغلاق النافذة (Esc)");
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += btnClose_Click;
        // 
        // cmbProduct
        // 
        cmbProduct.BackColor = System.Drawing.Color.White;
        cmbProduct.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbProduct.FlatStyle = FlatStyle.Flat;
        cmbProduct.Font = new Font("Tahoma", 10F);
        cmbProduct.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        cmbProduct.FormattingEnabled = true;
        cmbProduct.Location = new Point(83, 36);
        cmbProduct.Name = "cmbProduct";
        cmbProduct.Size = new Size(430, 32);
        cmbProduct.TabIndex = 1;
        tooltip.SetToolTip(cmbProduct, "اختر الصنف لعرض الباركود الخاص به");
        cmbProduct.SelectedIndexChanged += cmbProduct_SelectedIndexChanged;
        // 
        // txtBarcode
        // 
        txtBarcode.BackColor = System.Drawing.Color.White;
        txtBarcode.Font = new Font("Tahoma", 10F);
        txtBarcode.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        txtBarcode.Location = new Point(56, 81);
        txtBarcode.Name = "txtBarcode";
        txtBarcode.Size = new Size(230, 32);
        txtBarcode.TabIndex = 3;
        txtBarcode.TextAlign = HorizontalAlignment.Center;
        tooltip.SetToolTip(txtBarcode, "امسح الباركود ثم Enter — يُحدَّد الصنف فوراً ويبقى الحقل جاهزاً للمسح التالي");
        txtBarcode.KeyDown += txtBarcode_KeyDown;
        // 
        // lblBarcodeValue
        // 
        lblBarcodeValue.BackColor = System.Drawing.Color.White;
        lblBarcodeValue.BorderStyle = BorderStyle.FixedSingle;
        lblBarcodeValue.Font = new Font("Consolas", 10F, FontStyle.Bold);
        lblBarcodeValue.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblBarcodeValue.Location = new Point(112, 120);
        lblBarcodeValue.Name = "lblBarcodeValue";
        lblBarcodeValue.Size = new Size(418, 28);
        lblBarcodeValue.TabIndex = 5;
        lblBarcodeValue.Text = "----------";
        lblBarcodeValue.TextAlign = ContentAlignment.MiddleCenter;
        tooltip.SetToolTip(lblBarcodeValue, "باركود الوحدة الممسوحة (أو باركود الوحدة الأساسية عند الاختيار من القائمة)");
        // 
        // nudLabelsCount
        // 
        nudLabelsCount.BackColor = System.Drawing.Color.White;
        nudLabelsCount.Font = new Font("Tahoma", 10F);
        nudLabelsCount.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        nudLabelsCount.Location = new Point(41, 157);
        nudLabelsCount.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
        nudLabelsCount.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        nudLabelsCount.Name = "nudLabelsCount";
        nudLabelsCount.Size = new Size(150, 32);
        nudLabelsCount.TabIndex = 7;
        nudLabelsCount.TextAlign = HorizontalAlignment.Center;
        tooltip.SetToolTip(nudLabelsCount, "عدد ملصقات الباركود المطلوب طباعتها (1 - 1000)");
        nudLabelsCount.Value = new decimal(new int[] { 10, 0, 0, 0 });
        // 
        // cmbLabelSize
        // 
        cmbLabelSize.BackColor = System.Drawing.Color.White;
        cmbLabelSize.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbLabelSize.FlatStyle = FlatStyle.Flat;
        cmbLabelSize.Font = new Font("Tahoma", 10F);
        cmbLabelSize.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        cmbLabelSize.FormattingEnabled = true;
        cmbLabelSize.Items.AddRange(new object[] { "صغير", "متوسط", "كبير" });
        cmbLabelSize.Location = new Point(354, 157);
        cmbLabelSize.Name = "cmbLabelSize";
        cmbLabelSize.Size = new Size(110, 32);
        cmbLabelSize.TabIndex = 9;
        tooltip.SetToolTip(cmbLabelSize, "اختر حجم ملصق الباركود");
        // 
        // pnlBarcodePreview
        // 
        pnlBarcodePreview.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(170), Convert.ToInt32(220), Convert.ToInt32(170));
        pnlBarcodePreview.BorderStyle = BorderStyle.FixedSingle;
        pnlBarcodePreview.Dock = DockStyle.Fill;
        pnlBarcodePreview.Location = new Point(8, 33);
        pnlBarcodePreview.Name = "pnlBarcodePreview";
        pnlBarcodePreview.Size = new Size(633, 349);
        pnlBarcodePreview.TabIndex = 0;
        tooltip.SetToolTip(pnlBarcodePreview, "معاينة الباركود قبل الطباعة");
        pnlBarcodePreview.Paint += pnlBarcodePreview_Paint;
        // 
        // lblMode
        // 
        lblMode.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
        lblMode.Dock = DockStyle.Top;
        lblMode.FlatStyle = FlatStyle.Flat;
        lblMode.Font = new Font("Tahoma", 14F, FontStyle.Bold);
        lblMode.ForeColor = System.Drawing.Color.White;
        lblMode.Location = new Point(0, 0);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(649, 42);
        lblMode.TabIndex = 0;
        lblMode.Text = "طباعة باركود الأصناف";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // gbToolbar
        // 
        gbToolbar.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(170), Convert.ToInt32(220), Convert.ToInt32(170));
        gbToolbar.Controls.Add(btnClose);
        gbToolbar.Controls.Add(btnPrint);
        gbToolbar.Dock = DockStyle.Top;
        gbToolbar.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        gbToolbar.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        gbToolbar.Location = new Point(0, 42);
        gbToolbar.Name = "gbToolbar";
        gbToolbar.Padding = new Padding(8);
        gbToolbar.Size = new Size(649, 68);
        gbToolbar.TabIndex = 1;
        gbToolbar.TabStop = false;
        gbToolbar.Text = "أوامر الملصق";
        // 
        // gbInfo
        // 
        gbInfo.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(170), Convert.ToInt32(220), Convert.ToInt32(170));
        gbInfo.Controls.Add(cmbLabelSize);
        gbInfo.Controls.Add(lblLabelsCount);
        gbInfo.Controls.Add(lblLabelSize);
        gbInfo.Controls.Add(nudLabelsCount);
        gbInfo.Controls.Add(lblBarcodeValue);
        gbInfo.Controls.Add(lblBarcode);
        gbInfo.Controls.Add(txtBarcode);
        gbInfo.Controls.Add(lblScanBarcode);
        gbInfo.Controls.Add(cmbProduct);
        gbInfo.Controls.Add(lblProduct);
        gbInfo.Dock = DockStyle.Top;
        gbInfo.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        gbInfo.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        gbInfo.Location = new Point(0, 110);
        gbInfo.Name = "gbInfo";
        gbInfo.Padding = new Padding(8);
        gbInfo.RightToLeft = RightToLeft.Yes;
        gbInfo.Size = new Size(649, 200);
        gbInfo.TabIndex = 2;
        gbInfo.TabStop = false;
        gbInfo.Text = "بيانات الملصق";
        // 
        // lblLabelSize
        // 
        lblLabelSize.AutoSize = true;
        lblLabelSize.Font = new Font("Tahoma", 10F);
        lblLabelSize.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblLabelSize.Location = new Point(470, 161);
        lblLabelSize.Name = "lblLabelSize";
        lblLabelSize.Size = new Size(127, 24);
        lblLabelSize.TabIndex = 8;
        lblLabelSize.Text = "حجم الملصق:";
        // 
        // lblLabelsCount
        // 
        lblLabelsCount.AutoSize = true;
        lblLabelsCount.Font = new Font("Tahoma", 10F);
        lblLabelsCount.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblLabelsCount.Location = new Point(197, 161);
        lblLabelsCount.Name = "lblLabelsCount";
        lblLabelsCount.Size = new Size(132, 24);
        lblLabelsCount.TabIndex = 6;
        lblLabelsCount.Text = "عدد الملصقات:";
        lblLabelsCount.Click += lblLabelsCount_Click;
        // 
        // lblBarcode
        // 
        lblBarcode.AutoSize = true;
        lblBarcode.Font = new Font("Tahoma", 10F);
        lblBarcode.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblBarcode.Location = new Point(536, 124);
        lblBarcode.Name = "lblBarcode";
        lblBarcode.Size = new Size(76, 24);
        lblBarcode.TabIndex = 4;
        lblBarcode.Text = "الباركود:";
        // 
        // lblScanBarcode
        // 
        lblScanBarcode.AutoSize = true;
        lblScanBarcode.Font = new Font("Tahoma", 10F);
        lblScanBarcode.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblScanBarcode.Location = new Point(292, 84);
        lblScanBarcode.Name = "lblScanBarcode";
        lblScanBarcode.Size = new Size(345, 24);
        lblScanBarcode.TabIndex = 2;
        lblScanBarcode.Text = "بحث بالباركود (امسح أو اكتب ثم Enter):";
        // 
        // lblProduct
        // 
        lblProduct.AutoSize = true;
        lblProduct.Font = new Font("Tahoma", 10F);
        lblProduct.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblProduct.Location = new Point(519, 44);
        lblProduct.Name = "lblProduct";
        lblProduct.Size = new Size(68, 24);
        lblProduct.TabIndex = 0;
        lblProduct.Text = "الصنف:";
        // 
        // gbPreview
        // 
        gbPreview.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(170), Convert.ToInt32(220), Convert.ToInt32(170));
        gbPreview.Controls.Add(pnlBarcodePreview);
        gbPreview.Dock = DockStyle.Fill;
        gbPreview.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        gbPreview.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        gbPreview.Location = new Point(0, 310);
        gbPreview.Name = "gbPreview";
        gbPreview.Padding = new Padding(8);
        gbPreview.RightToLeft = RightToLeft.Yes;
        gbPreview.Size = new Size(649, 390);
        gbPreview.TabIndex = 3;
        gbPreview.TabStop = false;
        gbPreview.Text = "معاينة الملصق";
        // 
        // frmInventoryBarcode
        // 
        AcceptButton = btnPrint;
        AutoScaleMode = AutoScaleMode.None;
        AutoValidate = AutoValidate.EnableAllowFocusChange;
        BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(170), Convert.ToInt32(220), Convert.ToInt32(170));
        CancelButton = btnClose;
        ClientSize = new Size(649, 700);
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
        ((System.ComponentModel.ISupportInitialize)nudLabelsCount).EndInit();
        gbToolbar.ResumeLayout(false);
        gbInfo.ResumeLayout(false);
        gbInfo.PerformLayout();
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
