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
        tooltip = new ToolTip(components);
        lblMode = new Label();
        lblProduct = new Label();
        cmbProduct = new ComboBox();
        lblBarcode = new Label();
        lblBarcodeValue = new Label();
        lblLabelsCount = new Label();
        nudLabelsCount = new NumericUpDown();
        lblLabelSize = new Label();
        cmbLabelSize = new ComboBox();
        pnlBarcodePreview = new Panel();
        btnPrint = new Button();
        btnClose = new Button();
        ((System.ComponentModel.ISupportInitialize)nudLabelsCount).BeginInit();
        SuspendLayout();
        // 
        // tooltip
        // 
        tooltip.AutoPopDelay = 5000;
        tooltip.InitialDelay = 300;
        tooltip.ReshowDelay = 100;
        // 
        // lblMode
        // 
        lblMode.BackColor = Color.WhiteSmoke;
        lblMode.Dock = DockStyle.Top;
        lblMode.FlatStyle = FlatStyle.Flat;
        lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold);
        lblMode.ForeColor = Color.Firebrick;
        lblMode.Location = new Point(0, 0);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(520, 39);
        lblMode.TabIndex = 0;
        lblMode.Text = "طباعة باركود";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblProduct
        // 
        lblProduct.AutoSize = true;
        lblProduct.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblProduct.Location = new Point(12, 63);
        lblProduct.Name = "lblProduct";
        lblProduct.Size = new Size(65, 24);
        lblProduct.TabIndex = 1;
        lblProduct.Text = "الصنف:";
        // 
        // cmbProduct
        // 
        cmbProduct.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbProduct.Font = new Font("Tahoma", 10F);
        cmbProduct.FormattingEnabled = true;
        cmbProduct.Location = new Point(100, 60);
        cmbProduct.Name = "cmbProduct";
        cmbProduct.Size = new Size(408, 32);
        cmbProduct.TabIndex = 2;
        cmbProduct.SelectedIndexChanged += cmbProduct_SelectedIndexChanged;
        tooltip.SetToolTip(cmbProduct, "اختر الصنف لعرض الباركود الخاص به");
        // 
        // lblBarcode
        // 
        lblBarcode.AutoSize = true;
        lblBarcode.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblBarcode.Location = new Point(12, 108);
        lblBarcode.Name = "lblBarcode";
        lblBarcode.Size = new Size(97, 24);
        lblBarcode.TabIndex = 3;
        lblBarcode.Text = "الباركود:";
        // 
        // lblBarcodeValue
        // 
        lblBarcodeValue.BackColor = Color.White;
        lblBarcodeValue.BorderStyle = BorderStyle.FixedSingle;
        lblBarcodeValue.Font = new Font("Consolas", 10F, FontStyle.Bold);
        lblBarcodeValue.Location = new Point(112, 105);
        lblBarcodeValue.Name = "lblBarcodeValue";
        lblBarcodeValue.Size = new Size(396, 32);
        lblBarcodeValue.TabIndex = 4;
        lblBarcodeValue.Text = "----------";
        lblBarcodeValue.TextAlign = ContentAlignment.MiddleCenter;
        tooltip.SetToolTip(lblBarcodeValue, "باركود الصنف المحدد (يتم تعبئته تلقائياً)");
        // 
        // lblLabelsCount
        // 
        lblLabelsCount.AutoSize = true;
        lblLabelsCount.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblLabelsCount.Location = new Point(12, 153);
        lblLabelsCount.Name = "lblLabelsCount";
        lblLabelsCount.Size = new Size(151, 24);
        lblLabelsCount.TabIndex = 5;
        lblLabelsCount.Text = "عدد الملصقات:";
        // 
        // nudLabelsCount
        // 
        nudLabelsCount.Font = new Font("Tahoma", 10F);
        nudLabelsCount.Location = new Point(170, 150);
        nudLabelsCount.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
        nudLabelsCount.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        nudLabelsCount.Name = "nudLabelsCount";
        nudLabelsCount.Size = new Size(150, 32);
        nudLabelsCount.TabIndex = 6;
        nudLabelsCount.TextAlign = HorizontalAlignment.Center;
        nudLabelsCount.Value = new decimal(new int[] { 10, 0, 0, 0 });
        tooltip.SetToolTip(nudLabelsCount, "عدد ملصقات الباركود المطلوب طباعتها (1 - 1000)");
        // 
        // lblLabelSize
        // 
        lblLabelSize.AutoSize = true;
        lblLabelSize.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblLabelSize.Location = new Point(12, 198);
        lblLabelSize.Name = "lblLabelSize";
        lblLabelSize.Size = new Size(130, 24);
        lblLabelSize.TabIndex = 7;
        lblLabelSize.Text = "حجم الملصق:";
        // 
        // cmbLabelSize
        // 
        cmbLabelSize.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbLabelSize.Font = new Font("Tahoma", 10F);
        cmbLabelSize.FormattingEnabled = true;
        cmbLabelSize.Items.AddRange(new object[] { "صغير", "متوسط", "كبير" });
        cmbLabelSize.Location = new Point(170, 195);
        cmbLabelSize.Name = "cmbLabelSize";
        cmbLabelSize.Size = new Size(150, 32);
        cmbLabelSize.TabIndex = 8;
        tooltip.SetToolTip(cmbLabelSize, "اختر حجم ملصق الباركود");
        // 
        // pnlBarcodePreview
        // 
        pnlBarcodePreview.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        pnlBarcodePreview.BackColor = Color.White;
        pnlBarcodePreview.BorderStyle = BorderStyle.FixedSingle;
        pnlBarcodePreview.Location = new Point(12, 240);
        pnlBarcodePreview.Name = "pnlBarcodePreview";
        pnlBarcodePreview.Size = new Size(496, 256);
        pnlBarcodePreview.TabIndex = 9;
        pnlBarcodePreview.Paint += pnlBarcodePreview_Paint;
        tooltip.SetToolTip(pnlBarcodePreview, "معاينة الباركود قبل الطباعة");
        // 
        // btnPrint
        // 
        btnPrint.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnPrint.BackColor = Color.FromArgb(25, 135, 84);
        btnPrint.Cursor = Cursors.Hand;
        btnPrint.FlatAppearance.BorderSize = 0;
        btnPrint.FlatStyle = FlatStyle.Flat;
        btnPrint.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnPrint.ForeColor = Color.White;
        btnPrint.Location = new Point(12, 508);
        btnPrint.MinimumSize = new Size(100, 35);
        btnPrint.Name = "btnPrint";
        btnPrint.Size = new Size(150, 40);
        btnPrint.TabIndex = 10;
        btnPrint.Text = "طباعة";
        btnPrint.UseVisualStyleBackColor = false;
        btnPrint.Click += btnPrint_Click;
        tooltip.SetToolTip(btnPrint, "طباعة الملصقات (نسخة تجريبية)");
        // 
        // btnClose
        // 
        btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnClose.BackColor = Color.FromArgb(108, 117, 125);
        btnClose.Cursor = Cursors.Hand;
        btnClose.DialogResult = DialogResult.Cancel;
        btnClose.FlatAppearance.BorderSize = 0;
        btnClose.FlatStyle = FlatStyle.Flat;
        btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnClose.ForeColor = Color.White;
        btnClose.Location = new Point(172, 508);
        btnClose.MinimumSize = new Size(100, 35);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(150, 40);
        btnClose.TabIndex = 11;
        btnClose.Text = "إغلاق";
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += btnClose_Click;
        tooltip.SetToolTip(btnClose, "إغلاق النافذة");
        // 
        // frmInventoryBarcode
        // 
        AcceptButton = btnPrint;
        AutoScaleMode = AutoScaleMode.None;
        AutoValidate = AutoValidate.EnableAllowFocusChange;
        BackColor = Color.WhiteSmoke;
        CancelButton = btnClose;
        ClientSize = new Size(520, 560);
        Controls.Add(btnClose);
        Controls.Add(btnPrint);
        Controls.Add(pnlBarcodePreview);
        Controls.Add(cmbLabelSize);
        Controls.Add(lblLabelSize);
        Controls.Add(nudLabelsCount);
        Controls.Add(lblLabelsCount);
        Controls.Add(lblBarcodeValue);
        Controls.Add(lblBarcode);
        Controls.Add(cmbProduct);
        Controls.Add(lblProduct);
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
        ((System.ComponentModel.ISupportInitialize)nudLabelsCount).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    private ToolTip tooltip;
    private Label lblMode;
    private Label lblProduct;
    private ComboBox cmbProduct;
    private Label lblBarcode;
    private Label lblBarcodeValue;
    private Label lblLabelsCount;
    private NumericUpDown nudLabelsCount;
    private Label lblLabelSize;
    private ComboBox cmbLabelSize;
    private Panel pnlBarcodePreview;
    private Button btnPrint;
    private Button btnClose;
}