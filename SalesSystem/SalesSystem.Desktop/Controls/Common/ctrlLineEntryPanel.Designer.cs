namespace SalesSystem.Desktop.Controls.Common;

partial class ctrlLineEntryPanel
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    // ملاحظات المصمّم:
    //   • المصمّم = التشغيل (RULE-N134 / RULE-N140 / RULE-N204): كل لون معلن هنا بالقيمة الحرفية
    //     نفسها التي تحتفظ بها لوحة الألوان وقت التشغيل، وبصيغة Convert.ToInt32 حصراً:
    //     النص المميّز (14,82,52) · نص القيم (33,37,41) · زر «إضافة السطر» (25,135,84)
    //     · زر «إلغاء» (108,117,125) — والأبيض للأشرطة والحقول.
    //   • RULE-N145: العنصر دائم يُعلَن هنا ولا يُبنى وقت التشغيل — والـUserControl لا ينشئ عنصراً
    //     في الكود، ولا يضبط لوناً/خطاً/حجماً (المظهر كله في هذا الملف).
    //   • حقل «الانتهاء» (lblExpiryCaption + dtpExpiry) عنصر دائم معلن هنا لكنه **مخفي افتراضياً**
    //     (`Visible = false`) لأن شاشات المشتريات والمرتجعين لا تعرفه؛ شاشة فاتورة البيع تُظهره
    //     وقت التشغيل بضبط `ShowExpiry = true` — فلا فرق بين المصمّم والتشغيل في الوضع الافتراضي.
    //   • RULE-N128: صفر مرجع إلى Helpers داخل المصمّم.
    //   • ‏UserControl لا يملك RightToLeftLayout (خاصية Form فقط — مقيس بالانعكاس، نفس ملاحظة
    //     ctrlCustomerCard وctrlDashboard) فالاتجاه RTL يأتي من RightToLeft هنا ومن النموذج الحاضن.
    //   • RULE-N145/RULE-N193: عناصر اللوحة العشرة (باركود · كود · اسم · وحدة · كمية · سعر · خصم
    //     · إجمالي · زرّان) أعلنت في المصمّم، ويوصّل الـUserControl أحداثها في هذا الملف فيبقى
    //     كل السلوك في مكان واحد.
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        toolTip1 = new ToolTip(components);
        gbEntry = new GroupBox();
        pnlEntry = new FlowLayoutPanel();
        lblBarcodeCaption = new Label();
        txtBarcode = new TextBox();
        lblItemCodeCaption = new Label();
        txtItemCode = new TextBox();
        lblProductCaption = new Label();
        lblProductName = new Label();
        lblUnitCaption = new Label();
        cmbUnit = new ComboBox();
        lblQtyCaption = new Label();
        txtQuantity = new TextBox();
        lblPriceCaption = new Label();
        txtPrice = new TextBox();
        lblDiscountCaption = new Label();
        txtDiscount = new TextBox();
        lblExpiryCaption = new Label();
        dtpExpiry = new DateTimePicker();
        lblTotalCaption = new Label();
        lblLineTotal = new Label();
        btnAddLine = new Button();
        btnCancelLine = new Button();
        gbEntry.SuspendLayout();
        pnlEntry.SuspendLayout();
        SuspendLayout();
        // 
        // gbEntry
        // 
        gbEntry.BackColor = System.Drawing.Color.White;
        gbEntry.Controls.Add(pnlEntry);
        gbEntry.Dock = DockStyle.Fill;
        gbEntry.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        gbEntry.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
        gbEntry.Location = new Point(0, 0);
        gbEntry.Name = "gbEntry";
        gbEntry.Padding = new Padding(8, 4, 8, 4);
        gbEntry.RightToLeft = RightToLeft.Yes;
        gbEntry.Size = new Size(1432, 100);
        gbEntry.TabIndex = 0;
        gbEntry.TabStop = false;
        gbEntry.Text = "إضافة صنف";
        // 
        // pnlEntry
        // 
        pnlEntry.AutoScroll = false;
        pnlEntry.Controls.Add(lblBarcodeCaption);
        pnlEntry.Controls.Add(txtBarcode);
        pnlEntry.Controls.Add(lblItemCodeCaption);
        pnlEntry.Controls.Add(txtItemCode);
        pnlEntry.Controls.Add(lblProductCaption);
        pnlEntry.Controls.Add(lblProductName);
        pnlEntry.Controls.Add(lblUnitCaption);
        pnlEntry.Controls.Add(cmbUnit);
        pnlEntry.Controls.Add(lblQtyCaption);
        pnlEntry.Controls.Add(txtQuantity);
        pnlEntry.Controls.Add(lblPriceCaption);
        pnlEntry.Controls.Add(txtPrice);
        pnlEntry.Controls.Add(lblDiscountCaption);
        pnlEntry.Controls.Add(txtDiscount);
        pnlEntry.Controls.Add(lblExpiryCaption);
        pnlEntry.Controls.Add(dtpExpiry);
        pnlEntry.Controls.Add(lblTotalCaption);
        pnlEntry.Controls.Add(lblLineTotal);
        pnlEntry.Controls.Add(btnAddLine);
        pnlEntry.Controls.Add(btnCancelLine);
        pnlEntry.Dock = DockStyle.Fill;
        pnlEntry.FlowDirection = FlowDirection.RightToLeft;
        pnlEntry.Font = new Font("Tahoma", 10F);
        pnlEntry.Location = new Point(8, 26);
        pnlEntry.Name = "pnlEntry";
        pnlEntry.Size = new Size(1416, 70);
        pnlEntry.TabIndex = 0;
        pnlEntry.WrapContents = true;
        // 
        // lblBarcodeCaption
        // 
        lblBarcodeCaption.AutoSize = true;
        lblBarcodeCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblBarcodeCaption.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
        lblBarcodeCaption.Location = new Point(1356, 3);
        lblBarcodeCaption.Margin = new Padding(3, 6, 3, 3);
        lblBarcodeCaption.Name = "lblBarcodeCaption";
        lblBarcodeCaption.Size = new Size(68, 24);
        lblBarcodeCaption.TabIndex = 0;
        lblBarcodeCaption.Text = "الباركود:";
        lblBarcodeCaption.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtBarcode
        // 
        txtBarcode.BackColor = System.Drawing.Color.White;
        txtBarcode.BorderStyle = BorderStyle.FixedSingle;
        txtBarcode.Font = new Font("Tahoma", 10F);
        txtBarcode.Location = new Point(1216, 3);
        txtBarcode.Margin = new Padding(3, 3, 8, 3);
        txtBarcode.Name = "txtBarcode";
        txtBarcode.Size = new Size(140, 27);
        txtBarcode.TabIndex = 1;
        txtBarcode.TextAlign = HorizontalAlignment.Left;
        toolTip1.SetToolTip(txtBarcode, "امسح الباركود ثم Enter — أو F4 لمنتقي الأصناف");
        txtBarcode.KeyDown += txtBarcode_KeyDown;
        txtBarcode.PreviewKeyDown += Entry_PreviewKeyDown;
        // 
        // lblItemCodeCaption
        // 
        lblItemCodeCaption.AutoSize = true;
        lblItemCodeCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblItemCodeCaption.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
        lblItemCodeCaption.Location = new Point(1161, 3);
        lblItemCodeCaption.Margin = new Padding(3, 6, 3, 3);
        lblItemCodeCaption.Name = "lblItemCodeCaption";
        lblItemCodeCaption.Size = new Size(55, 24);
        lblItemCodeCaption.TabIndex = 2;
        lblItemCodeCaption.Text = "الكود:";
        lblItemCodeCaption.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtItemCode
        // 
        txtItemCode.BackColor = System.Drawing.Color.White;
        txtItemCode.BorderStyle = BorderStyle.FixedSingle;
        txtItemCode.Font = new Font("Tahoma", 10F);
        txtItemCode.Location = new Point(1058, 3);
        txtItemCode.Margin = new Padding(3, 3, 8, 3);
        txtItemCode.Name = "txtItemCode";
        txtItemCode.Size = new Size(100, 27);
        txtItemCode.TabIndex = 3;
        txtItemCode.TextAlign = HorizontalAlignment.Left;
        toolTip1.SetToolTip(txtItemCode, "كود الصنف (يُملأ تلقائياً عند اختيار الصنف)");
        txtItemCode.PreviewKeyDown += Entry_PreviewKeyDown;
        // 
        // lblProductCaption
        // 
        lblProductCaption.AutoSize = true;
        lblProductCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblProductCaption.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
        lblProductCaption.Location = new Point(1000, 3);
        lblProductCaption.Margin = new Padding(3, 6, 3, 3);
        lblProductCaption.Name = "lblProductCaption";
        lblProductCaption.Size = new Size(58, 24);
        lblProductCaption.TabIndex = 4;
        lblProductCaption.Text = "الصنف:";
        lblProductCaption.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblProductName
        // 
        lblProductName.AutoSize = true;
        lblProductName.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblProductName.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
        lblProductName.Location = new Point(777, 3);
        lblProductName.Margin = new Padding(3, 6, 8, 3);
        lblProductName.MinimumSize = new Size(220, 0);
        lblProductName.Name = "lblProductName";
        lblProductName.Size = new Size(220, 24);
        lblProductName.TabIndex = 5;
        lblProductName.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblUnitCaption
        // 
        lblUnitCaption.AutoSize = true;
        lblUnitCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblUnitCaption.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
        lblUnitCaption.Location = new Point(716, 3);
        lblUnitCaption.Margin = new Padding(3, 6, 3, 3);
        lblUnitCaption.Name = "lblUnitCaption";
        lblUnitCaption.Size = new Size(58, 24);
        lblUnitCaption.TabIndex = 6;
        lblUnitCaption.Text = "الوحدة:";
        lblUnitCaption.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // cmbUnit
        // 
        cmbUnit.BackColor = System.Drawing.Color.White;
        cmbUnit.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbUnit.Font = new Font("Tahoma", 10F);
        cmbUnit.FormattingEnabled = true;
        cmbUnit.Location = new Point(596, 3);
        cmbUnit.Margin = new Padding(3, 3, 8, 3);
        cmbUnit.Name = "cmbUnit";
        cmbUnit.Size = new Size(110, 25);
        cmbUnit.TabIndex = 7;
        toolTip1.SetToolTip(cmbUnit, "وحدة الصنف — تغييرها يعيد السعر الافتراضي للوحدة");
        cmbUnit.SelectedIndexChanged += cmbUnit_SelectedIndexChanged;
        cmbUnit.KeyDown += Entry_KeyDown;
        cmbUnit.PreviewKeyDown += Entry_PreviewKeyDown;
        // 
        // lblQtyCaption
        // 
        lblQtyCaption.AutoSize = true;
        lblQtyCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblQtyCaption.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
        lblQtyCaption.Location = new Point(533, 3);
        lblQtyCaption.Margin = new Padding(3, 6, 3, 3);
        lblQtyCaption.Name = "lblQtyCaption";
        lblQtyCaption.Size = new Size(60, 24);
        lblQtyCaption.TabIndex = 8;
        lblQtyCaption.Text = "الكمية:";
        lblQtyCaption.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtQuantity
        // 
        txtQuantity.BackColor = System.Drawing.Color.White;
        txtQuantity.BorderStyle = BorderStyle.FixedSingle;
        txtQuantity.Font = new Font("Tahoma", 10F);
        txtQuantity.Location = new Point(453, 3);
        txtQuantity.Margin = new Padding(3, 3, 8, 3);
        txtQuantity.Name = "txtQuantity";
        txtQuantity.Size = new Size(70, 27);
        txtQuantity.TabIndex = 9;
        txtQuantity.Text = "1";
        txtQuantity.TextAlign = HorizontalAlignment.Left;
        toolTip1.SetToolTip(txtQuantity, "الكمية — يجب أن تكون أكبر من صفر");
        txtQuantity.KeyDown += Entry_KeyDown;
        txtQuantity.PreviewKeyDown += Entry_PreviewKeyDown;
        txtQuantity.TextChanged += Entry_Amount_TextChanged;
        // 
        // lblPriceCaption
        // 
        lblPriceCaption.AutoSize = true;
        lblPriceCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblPriceCaption.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
        lblPriceCaption.Location = new Point(399, 3);
        lblPriceCaption.Margin = new Padding(3, 6, 3, 3);
        lblPriceCaption.Name = "lblPriceCaption";
        lblPriceCaption.Size = new Size(48, 24);
        lblPriceCaption.TabIndex = 10;
        lblPriceCaption.Text = "السعر:";
        lblPriceCaption.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtPrice
        // 
        txtPrice.BackColor = System.Drawing.Color.White;
        txtPrice.BorderStyle = BorderStyle.FixedSingle;
        txtPrice.Font = new Font("Tahoma", 10F);
        txtPrice.Location = new Point(299, 3);
        txtPrice.Margin = new Padding(3, 3, 8, 3);
        txtPrice.Name = "txtPrice";
        txtPrice.Size = new Size(90, 27);
        txtPrice.TabIndex = 11;
        txtPrice.TextAlign = HorizontalAlignment.Left;
        toolTip1.SetToolTip(txtPrice, "السعر الافتراضي للوحدة المختارة — قابل للتعديل");
        txtPrice.KeyDown += Entry_KeyDown;
        txtPrice.PreviewKeyDown += Entry_PreviewKeyDown;
        txtPrice.TextChanged += Entry_Amount_TextChanged;
        // 
        // lblDiscountCaption
        // 
        lblDiscountCaption.AutoSize = true;
        lblDiscountCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblDiscountCaption.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
        lblDiscountCaption.Location = new Point(233, 3);
        lblDiscountCaption.Margin = new Padding(3, 6, 3, 3);
        lblDiscountCaption.Name = "lblDiscountCaption";
        lblDiscountCaption.Size = new Size(55, 24);
        lblDiscountCaption.TabIndex = 12;
        lblDiscountCaption.Text = "الخصم:";
        lblDiscountCaption.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtDiscount
        // 
        txtDiscount.BackColor = System.Drawing.Color.White;
        txtDiscount.BorderStyle = BorderStyle.FixedSingle;
        txtDiscount.Font = new Font("Tahoma", 10F);
        txtDiscount.Location = new Point(153, 3);
        txtDiscount.Margin = new Padding(3, 3, 8, 3);
        txtDiscount.Name = "txtDiscount";
        txtDiscount.Size = new Size(70, 27);
        txtDiscount.TabIndex = 13;
        txtDiscount.Text = "0";
        txtDiscount.TextAlign = HorizontalAlignment.Left;
        toolTip1.SetToolTip(txtDiscount, "خصم السطر — لا يتجاوز (الكمية × السعر)");
        txtDiscount.KeyDown += Entry_KeyDown;
        txtDiscount.PreviewKeyDown += Entry_PreviewKeyDown;
        txtDiscount.TextChanged += Entry_Amount_TextChanged;
        // 
        // lblExpiryCaption
        // 
        lblExpiryCaption.AutoSize = true;
        lblExpiryCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblExpiryCaption.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
        lblExpiryCaption.Location = new Point(1356, 37);
        lblExpiryCaption.Margin = new Padding(3, 9, 3, 3);
        lblExpiryCaption.Name = "lblExpiryCaption";
        lblExpiryCaption.Size = new Size(62, 24);
        lblExpiryCaption.TabIndex = 14;
        lblExpiryCaption.Text = "الانتهاء:";
        lblExpiryCaption.TextAlign = ContentAlignment.MiddleLeft;
        lblExpiryCaption.Visible = false;
        // 
        // dtpExpiry
        // 
        dtpExpiry.BackColor = System.Drawing.Color.White;
        dtpExpiry.Checked = false;
        dtpExpiry.Font = new Font("Tahoma", 10F);
        dtpExpiry.Format = DateTimePickerFormat.Short;
        dtpExpiry.Location = new Point(1228, 37);
        dtpExpiry.Margin = new Padding(3, 9, 8, 3);
        dtpExpiry.Name = "dtpExpiry";
        dtpExpiry.ShowCheckBox = true;
        dtpExpiry.Size = new Size(120, 27);
        dtpExpiry.TabIndex = 15;
        dtpExpiry.Visible = false;
        toolTip1.SetToolTip(dtpExpiry, "تاريخ انتهاء صلاحية السطر — اتركه بلا علامة لسطر بلا انتهاء");
        dtpExpiry.KeyDown += Entry_KeyDown;
        dtpExpiry.PreviewKeyDown += Entry_PreviewKeyDown;
        // 
        // lblTotalCaption
        // 
        lblTotalCaption.AutoSize = true;
        lblTotalCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblTotalCaption.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
        lblTotalCaption.Location = new Point(1120, 37);
        lblTotalCaption.Margin = new Padding(3, 9, 3, 3);
        lblTotalCaption.Name = "lblTotalCaption";
        lblTotalCaption.Size = new Size(105, 24);
        lblTotalCaption.TabIndex = 16;
        lblTotalCaption.Text = "إجمالي السطر:";
        lblTotalCaption.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblLineTotal
        // 
        lblLineTotal.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblLineTotal.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
        lblLineTotal.Location = new Point(1002, 37);
        lblLineTotal.Margin = new Padding(3, 9, 8, 3);
        lblLineTotal.Name = "lblLineTotal";
        lblLineTotal.Size = new Size(110, 27);
        lblLineTotal.TabIndex = 17;
        lblLineTotal.Text = "0.00";
        lblLineTotal.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // btnAddLine
        // 
        btnAddLine.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(25), Convert.ToInt32(135), Convert.ToInt32(84));
        btnAddLine.Cursor = Cursors.Hand;
        btnAddLine.FlatAppearance.BorderSize = 0;
        btnAddLine.FlatStyle = FlatStyle.Flat;
        btnAddLine.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnAddLine.ForeColor = System.Drawing.Color.White;
        btnAddLine.Location = new Point(866, 37);
        btnAddLine.Margin = new Padding(3, 9, 8, 3);
        btnAddLine.Name = "btnAddLine";
        btnAddLine.Size = new Size(130, 30);
        btnAddLine.TabIndex = 18;
        btnAddLine.Text = "إضافة السطر";
        toolTip1.SetToolTip(btnAddLine, "إضافة السطر الحالي إلى الشبكة (Enter في آخر حقل)");
        btnAddLine.UseVisualStyleBackColor = false;
        btnAddLine.Click += btnAddLine_Click;
        // 
        // btnCancelLine
        // 
        btnCancelLine.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(108), Convert.ToInt32(117), Convert.ToInt32(125));
        btnCancelLine.Cursor = Cursors.Hand;
        btnCancelLine.FlatAppearance.BorderSize = 0;
        btnCancelLine.FlatStyle = FlatStyle.Flat;
        btnCancelLine.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnCancelLine.ForeColor = System.Drawing.Color.White;
        btnCancelLine.Location = new Point(764, 37);
        btnCancelLine.Margin = new Padding(3, 9, 3, 3);
        btnCancelLine.Name = "btnCancelLine";
        btnCancelLine.Size = new Size(90, 30);
        btnCancelLine.TabIndex = 19;
        btnCancelLine.Text = "إلغاء";
        toolTip1.SetToolTip(btnCancelLine, "إلغاء السطر الحالي وتفريغ اللوحة (Esc)");
        btnCancelLine.UseVisualStyleBackColor = false;
        btnCancelLine.Click += btnCancelLine_Click;
        // 
        // ctrlLineEntryPanel
        // 
        AutoScaleMode = AutoScaleMode.None;
        AutoSize = false;
        BackColor = System.Drawing.Color.White;
        Controls.Add(gbEntry);
        Font = new Font("Tahoma", 10F);
        Name = "ctrlLineEntryPanel";
        Padding = new Padding(0);
        RightToLeft = RightToLeft.Yes;
        Size = new Size(1432, 100);
        gbEntry.ResumeLayout(false);
        gbEntry.PerformLayout();
        pnlEntry.ResumeLayout(false);
        pnlEntry.PerformLayout();
        ResumeLayout(false);
    }

    private System.Windows.Forms.ToolTip toolTip1;
    private System.Windows.Forms.GroupBox gbEntry;
    private System.Windows.Forms.FlowLayoutPanel pnlEntry;
    private System.Windows.Forms.Label lblBarcodeCaption;
    private System.Windows.Forms.TextBox txtBarcode;
    private System.Windows.Forms.Label lblItemCodeCaption;
    private System.Windows.Forms.TextBox txtItemCode;
    private System.Windows.Forms.Label lblProductCaption;
    private System.Windows.Forms.Label lblProductName;
    private System.Windows.Forms.Label lblUnitCaption;
    private System.Windows.Forms.ComboBox cmbUnit;
    private System.Windows.Forms.Label lblQtyCaption;
    private System.Windows.Forms.TextBox txtQuantity;
    private System.Windows.Forms.Label lblPriceCaption;
    private System.Windows.Forms.TextBox txtPrice;
    private System.Windows.Forms.Label lblDiscountCaption;
    private System.Windows.Forms.TextBox txtDiscount;
    private System.Windows.Forms.Label lblExpiryCaption;
    private System.Windows.Forms.DateTimePicker dtpExpiry;
    private System.Windows.Forms.Label lblTotalCaption;
    private System.Windows.Forms.Label lblLineTotal;
    private System.Windows.Forms.Button btnAddLine;
    private System.Windows.Forms.Button btnCancelLine;
}
