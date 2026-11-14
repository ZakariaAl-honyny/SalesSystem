namespace SalesSystem.Desktop.Screens.Settings.Forms;

partial class frmFormatingSetting
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

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        pnlHeader = new Panel();
        lblMode = new Label();
        gbNumbers = new GroupBox();
        lblMoneyDecimals = new Label();
        numMoneyDecimals = new NumericUpDown();
        lblQuantityDecimals = new Label();
        numQuantityDecimals = new NumericUpDown();
        lblNumberDecimals = new Label();
        numNumberDecimals = new NumericUpDown();
        lblPercentDecimals = new Label();
        numPercentDecimals = new NumericUpDown();
        chkThousandsSeparator = new CheckBox();
        lblDecimalSeparator = new Label();
        cmbDecimalSeparator = new ComboBox();
        chkArabicIndicDigits = new CheckBox();
        gbDates = new GroupBox();
        lblDateShortPattern = new Label();
        txtDateShortPattern = new TextBox();
        lblDateDayMonthYearPattern = new Label();
        txtDateDayMonthYearPattern = new TextBox();
        lblDateLongPattern = new Label();
        txtDateLongPattern = new TextBox();
        lblDateHint = new Label();
        gbPreview = new GroupBox();
        pnlPreviewScroll = new Panel();
        lblPreviewMoneyCaption = new Label();
        lblPreviewMoney = new Label();
        lblPreviewQuantityCaption = new Label();
        lblPreviewQuantity = new Label();
        lblPreviewNumberCaption = new Label();
        lblPreviewNumber = new Label();
        lblPreviewPercentCaption = new Label();
        lblPreviewPercent = new Label();
        lblPreviewDateShortCaption = new Label();
        lblPreviewDateShort = new Label();
        lblPreviewDateDMYCaption = new Label();
        lblPreviewDateDMY = new Label();
        lblPreviewDateLongCaption = new Label();
        lblPreviewDateLong = new Label();
        lblPreviewSample = new Label();
        lblValidation = new Label();
        pnlSummary = new Panel();
        lblSummary = new Label();
        pnlFooter = new Panel();
        lblFooterHint = new Label();
        flpFooter = new FlowLayoutPanel();
        btnSave = new Button();
        btnReset = new Button();
        btnClose = new Button();
        toolTip = new ToolTip(components);
        pnlHeader.SuspendLayout();
        gbNumbers.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numMoneyDecimals).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numQuantityDecimals).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numNumberDecimals).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numPercentDecimals).BeginInit();
        gbDates.SuspendLayout();
        gbPreview.SuspendLayout();
        pnlPreviewScroll.SuspendLayout();
        pnlSummary.SuspendLayout();
        pnlFooter.SuspendLayout();
        flpFooter.SuspendLayout();
        SuspendLayout();
        // 
        // pnlHeader
        // 
        pnlHeader.BackColor = Color.FromArgb(14, 165, 103);
        pnlHeader.Controls.Add(lblMode);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Location = new Point(0, 0);
        pnlHeader.Margin = new Padding(0);
        pnlHeader.Name = "pnlHeader";
        pnlHeader.Size = new Size(1180, 56);
        pnlHeader.TabIndex = 0;
        // 
        // lblMode
        // 
        lblMode.BackColor = Color.FromArgb(14, 165, 103);
        lblMode.Dock = DockStyle.Fill;
        lblMode.Font = new Font("Tahoma", 12F, FontStyle.Bold);
        lblMode.ForeColor = Color.White;
        lblMode.Location = new Point(0, 0);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(1180, 56);
        lblMode.TabIndex = 0;
        lblMode.Text = "إعدادات التنسيق محفوظة";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // gbNumbers
        // 
        gbNumbers.BackColor = Color.FromArgb(235, 245, 235);
        gbNumbers.Controls.Add(lblMoneyDecimals);
        gbNumbers.Controls.Add(numMoneyDecimals);
        gbNumbers.Controls.Add(lblQuantityDecimals);
        gbNumbers.Controls.Add(numQuantityDecimals);
        gbNumbers.Controls.Add(lblNumberDecimals);
        gbNumbers.Controls.Add(numNumberDecimals);
        gbNumbers.Controls.Add(lblPercentDecimals);
        gbNumbers.Controls.Add(numPercentDecimals);
        gbNumbers.Controls.Add(chkThousandsSeparator);
        gbNumbers.Controls.Add(lblDecimalSeparator);
        gbNumbers.Controls.Add(cmbDecimalSeparator);
        gbNumbers.Controls.Add(chkArabicIndicDigits);
        gbNumbers.Dock = DockStyle.Top;
        gbNumbers.Font = new Font("Tahoma", 10F);
        gbNumbers.ForeColor = Color.FromArgb(14, 82, 52);
        gbNumbers.Location = new Point(0, 56);
        gbNumbers.Name = "gbNumbers";
        gbNumbers.Padding = new Padding(8);
        gbNumbers.Size = new Size(1180, 176);
        gbNumbers.TabIndex = 1;
        gbNumbers.TabStop = false;
        gbNumbers.Text = "المبالغ والأرقام";
        // 
        // lblMoneyDecimals
        // 
        lblMoneyDecimals.AutoSize = true;
        lblMoneyDecimals.Location = new Point(1020, 40);
        lblMoneyDecimals.Name = "lblMoneyDecimals";
        lblMoneyDecimals.Size = new Size(94, 24);
        lblMoneyDecimals.TabIndex = 0;
        lblMoneyDecimals.Text = "منازل المبالغ";
        // 
        // numMoneyDecimals
        // 
        numMoneyDecimals.BackColor = Color.White;
        numMoneyDecimals.BorderStyle = BorderStyle.FixedSingle;
        numMoneyDecimals.Font = new Font("Tahoma", 10F);
        numMoneyDecimals.Location = new Point(936, 34);
        numMoneyDecimals.Maximum = new decimal(new int[] { 4, 0, 0, 0 });
        numMoneyDecimals.Name = "numMoneyDecimals";
        numMoneyDecimals.Size = new Size(76, 32);
        numMoneyDecimals.TabIndex = 1;
        numMoneyDecimals.TextAlign = HorizontalAlignment.Center;
        numMoneyDecimals.ValueChanged += Value_Changed;
        // 
        // lblQuantityDecimals
        // 
        lblQuantityDecimals.AutoSize = true;
        lblQuantityDecimals.Location = new Point(745, 40);
        lblQuantityDecimals.Name = "lblQuantityDecimals";
        lblQuantityDecimals.Size = new Size(100, 24);
        lblQuantityDecimals.TabIndex = 2;
        lblQuantityDecimals.Text = "منازل الكميات";
        // 
        // numQuantityDecimals
        // 
        numQuantityDecimals.BackColor = Color.White;
        numQuantityDecimals.BorderStyle = BorderStyle.FixedSingle;
        numQuantityDecimals.Font = new Font("Tahoma", 10F);
        numQuantityDecimals.Location = new Point(661, 34);
        numQuantityDecimals.Maximum = new decimal(new int[] { 4, 0, 0, 0 });
        numQuantityDecimals.Name = "numQuantityDecimals";
        numQuantityDecimals.Size = new Size(76, 32);
        numQuantityDecimals.TabIndex = 3;
        numQuantityDecimals.TextAlign = HorizontalAlignment.Center;
        numQuantityDecimals.Value = new decimal(new int[] { 3, 0, 0, 0 });
        numQuantityDecimals.ValueChanged += Value_Changed;
        // 
        // lblNumberDecimals
        // 
        lblNumberDecimals.AutoSize = true;
        lblNumberDecimals.Location = new Point(480, 40);
        lblNumberDecimals.Name = "lblNumberDecimals";
        lblNumberDecimals.Size = new Size(94, 24);
        lblNumberDecimals.TabIndex = 4;
        lblNumberDecimals.Text = "منازل الأعداد";
        // 
        // numNumberDecimals
        // 
        numNumberDecimals.BackColor = Color.White;
        numNumberDecimals.BorderStyle = BorderStyle.FixedSingle;
        numNumberDecimals.Font = new Font("Tahoma", 10F);
        numNumberDecimals.Location = new Point(396, 34);
        numNumberDecimals.Maximum = new decimal(new int[] { 4, 0, 0, 0 });
        numNumberDecimals.Name = "numNumberDecimals";
        numNumberDecimals.Size = new Size(76, 32);
        numNumberDecimals.TabIndex = 5;
        numNumberDecimals.TextAlign = HorizontalAlignment.Center;
        numNumberDecimals.ValueChanged += Value_Changed;
        // 
        // lblPercentDecimals
        // 
        lblPercentDecimals.AutoSize = true;
        lblPercentDecimals.Location = new Point(215, 40);
        lblPercentDecimals.Name = "lblPercentDecimals";
        lblPercentDecimals.Size = new Size(84, 24);
        lblPercentDecimals.TabIndex = 6;
        lblPercentDecimals.Text = "منازل النسب";
        // 
        // numPercentDecimals
        // 
        numPercentDecimals.BackColor = Color.White;
        numPercentDecimals.BorderStyle = BorderStyle.FixedSingle;
        numPercentDecimals.Font = new Font("Tahoma", 10F);
        numPercentDecimals.Location = new Point(131, 34);
        numPercentDecimals.Maximum = new decimal(new int[] { 4, 0, 0, 0 });
        numPercentDecimals.Name = "numPercentDecimals";
        numPercentDecimals.Size = new Size(76, 32);
        numPercentDecimals.TabIndex = 7;
        numPercentDecimals.TextAlign = HorizontalAlignment.Center;
        numPercentDecimals.Value = new decimal(new int[] { 2, 0, 0, 0 });
        numPercentDecimals.ValueChanged += Value_Changed;
        // 
        // chkThousandsSeparator
        // 
        chkThousandsSeparator.Checked = true;
        chkThousandsSeparator.Font = new Font("Tahoma", 10F);
        chkThousandsSeparator.Location = new Point(860, 96);
        chkThousandsSeparator.Name = "chkThousandsSeparator";
        chkThousandsSeparator.Size = new Size(290, 30);
        chkThousandsSeparator.TabIndex = 8;
        chkThousandsSeparator.Text = "فاصل الآلاف (1,234.56)";
        chkThousandsSeparator.CheckedChanged += Value_Changed;
        // 
        // lblDecimalSeparator
        // 
        lblDecimalSeparator.AutoSize = true;
        lblDecimalSeparator.Location = new Point(700, 102);
        lblDecimalSeparator.Name = "lblDecimalSeparator";
        lblDecimalSeparator.Size = new Size(76, 24);
        lblDecimalSeparator.TabIndex = 9;
        lblDecimalSeparator.Text = "فاصل الكسر";
        // 
        // cmbDecimalSeparator
        // 
        cmbDecimalSeparator.BackColor = Color.White;
        cmbDecimalSeparator.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbDecimalSeparator.FlatStyle = FlatStyle.Flat;
        cmbDecimalSeparator.Font = new Font("Tahoma", 10F);
        cmbDecimalSeparator.Items.AddRange(new object[] { ".", "," });
        cmbDecimalSeparator.Location = new Point(600, 96);
        cmbDecimalSeparator.Name = "cmbDecimalSeparator";
        cmbDecimalSeparator.SelectedIndex = 0;
        cmbDecimalSeparator.Size = new Size(92, 32);
        cmbDecimalSeparator.TabIndex = 10;
        cmbDecimalSeparator.SelectedIndexChanged += Value_Changed;
        // 
        // chkArabicIndicDigits
        // 
        chkArabicIndicDigits.Font = new Font("Tahoma", 10F);
        chkArabicIndicDigits.Location = new Point(200, 96);
        chkArabicIndicDigits.Name = "chkArabicIndicDigits";
        chkArabicIndicDigits.Size = new Size(300, 30);
        chkArabicIndicDigits.TabIndex = 11;
        chkArabicIndicDigits.Text = "أرقام عربية (٠١٢٣٤٥٦٧٨٩)";
        chkArabicIndicDigits.CheckedChanged += Value_Changed;
        // 
        // gbDates
        // 
        gbDates.BackColor = Color.FromArgb(235, 245, 235);
        gbDates.Controls.Add(lblDateShortPattern);
        gbDates.Controls.Add(txtDateShortPattern);
        gbDates.Controls.Add(lblDateDayMonthYearPattern);
        gbDates.Controls.Add(txtDateDayMonthYearPattern);
        gbDates.Controls.Add(lblDateLongPattern);
        gbDates.Controls.Add(txtDateLongPattern);
        gbDates.Controls.Add(lblDateHint);
        gbDates.Dock = DockStyle.Top;
        gbDates.Font = new Font("Tahoma", 10F);
        gbDates.ForeColor = Color.FromArgb(14, 82, 52);
        gbDates.Location = new Point(0, 232);
        gbDates.Name = "gbDates";
        gbDates.Padding = new Padding(8);
        gbDates.Size = new Size(1180, 132);
        gbDates.TabIndex = 2;
        gbDates.TabStop = false;
        gbDates.Text = "التواريخ";
        // 
        // lblDateShortPattern
        // 
        lblDateShortPattern.AutoSize = true;
        lblDateShortPattern.Location = new Point(1010, 40);
        lblDateShortPattern.Name = "lblDateShortPattern";
        lblDateShortPattern.Size = new Size(140, 24);
        lblDateShortPattern.TabIndex = 0;
        lblDateShortPattern.Text = "صيغة التاريخ القصير";
        // 
        // txtDateShortPattern
        // 
        txtDateShortPattern.BackColor = Color.White;
        txtDateShortPattern.Font = new Font("Tahoma", 10F);
        txtDateShortPattern.Location = new Point(790, 34);
        txtDateShortPattern.Name = "txtDateShortPattern";
        txtDateShortPattern.Size = new Size(200, 32);
        txtDateShortPattern.TabIndex = 1;
        txtDateShortPattern.TextAlign = HorizontalAlignment.Center;
        txtDateShortPattern.TextChanged += Value_Changed;
        // 
        // lblDateDayMonthYearPattern
        // 
        lblDateDayMonthYearPattern.AutoSize = true;
        lblDateDayMonthYearPattern.Location = new Point(620, 40);
        lblDateDayMonthYearPattern.Name = "lblDateDayMonthYearPattern";
        lblDateDayMonthYearPattern.Size = new Size(148, 24);
        lblDateDayMonthYearPattern.TabIndex = 2;
        lblDateDayMonthYearPattern.Text = "صيغة يوم/شهر/سنة";
        // 
        // txtDateDayMonthYearPattern
        // 
        txtDateDayMonthYearPattern.BackColor = Color.White;
        txtDateDayMonthYearPattern.Font = new Font("Tahoma", 10F);
        txtDateDayMonthYearPattern.Location = new Point(400, 34);
        txtDateDayMonthYearPattern.Name = "txtDateDayMonthYearPattern";
        txtDateDayMonthYearPattern.Size = new Size(200, 32);
        txtDateDayMonthYearPattern.TabIndex = 3;
        txtDateDayMonthYearPattern.TextAlign = HorizontalAlignment.Center;
        txtDateDayMonthYearPattern.TextChanged += Value_Changed;
        // 
        // lblDateLongPattern
        // 
        lblDateLongPattern.AutoSize = true;
        lblDateLongPattern.Location = new Point(230, 40);
        lblDateLongPattern.Name = "lblDateLongPattern";
        lblDateLongPattern.Size = new Size(148, 24);
        lblDateLongPattern.TabIndex = 4;
        lblDateLongPattern.Text = "صيغة التاريخ والوقت";
        // 
        // txtDateLongPattern
        // 
        txtDateLongPattern.BackColor = Color.White;
        txtDateLongPattern.Font = new Font("Tahoma", 10F);
        txtDateLongPattern.Location = new Point(10, 34);
        txtDateLongPattern.Name = "txtDateLongPattern";
        txtDateLongPattern.Size = new Size(200, 32);
        txtDateLongPattern.TabIndex = 5;
        txtDateLongPattern.TextAlign = HorizontalAlignment.Center;
        txtDateLongPattern.TextChanged += Value_Changed;
        // 
        // lblDateHint
        // 
        lblDateHint.Font = new Font("Tahoma", 9F);
        lblDateHint.Location = new Point(10, 78);
        lblDateHint.Name = "lblDateHint";
        lblDateHint.Size = new Size(1160, 26);
        lblDateHint.TabIndex = 6;
        lblDateHint.Text = "أمثلة مقبولة: …";
        lblDateHint.TextAlign = ContentAlignment.MiddleRight;
        // 
        // gbPreview
        // 
        gbPreview.BackColor = Color.FromArgb(235, 245, 235);
        gbPreview.Controls.Add(pnlPreviewScroll);
        gbPreview.Dock = DockStyle.Fill;
        gbPreview.Font = new Font("Tahoma", 10F);
        gbPreview.ForeColor = Color.FromArgb(14, 82, 52);
        gbPreview.Location = new Point(0, 364);
        gbPreview.Name = "gbPreview";
        gbPreview.Padding = new Padding(8);
        gbPreview.Size = new Size(1180, 364);
        gbPreview.TabIndex = 3;
        gbPreview.TabStop = false;
        gbPreview.Text = "معاينة فورية";
        // 
        // pnlPreviewScroll
        // 
        pnlPreviewScroll.AutoScroll = true;
        pnlPreviewScroll.Controls.Add(lblPreviewMoneyCaption);
        pnlPreviewScroll.Controls.Add(lblPreviewMoney);
        pnlPreviewScroll.Controls.Add(lblPreviewQuantityCaption);
        pnlPreviewScroll.Controls.Add(lblPreviewQuantity);
        pnlPreviewScroll.Controls.Add(lblPreviewNumberCaption);
        pnlPreviewScroll.Controls.Add(lblPreviewNumber);
        pnlPreviewScroll.Controls.Add(lblPreviewPercentCaption);
        pnlPreviewScroll.Controls.Add(lblPreviewPercent);
        pnlPreviewScroll.Controls.Add(lblPreviewDateShortCaption);
        pnlPreviewScroll.Controls.Add(lblPreviewDateShort);
        pnlPreviewScroll.Controls.Add(lblPreviewDateDMYCaption);
        pnlPreviewScroll.Controls.Add(lblPreviewDateDMY);
        pnlPreviewScroll.Controls.Add(lblPreviewDateLongCaption);
        pnlPreviewScroll.Controls.Add(lblPreviewDateLong);
        pnlPreviewScroll.Controls.Add(lblPreviewSample);
        pnlPreviewScroll.Controls.Add(lblValidation);
        pnlPreviewScroll.Dock = DockStyle.Fill;
        pnlPreviewScroll.Location = new Point(8, 30);
        pnlPreviewScroll.Name = "pnlPreviewScroll";
        pnlPreviewScroll.Size = new Size(1164, 326);
        pnlPreviewScroll.TabIndex = 0;
        // 
        // lblPreviewMoneyCaption
        // 
        lblPreviewMoneyCaption.AutoSize = true;
        lblPreviewMoneyCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblPreviewMoneyCaption.ForeColor = Color.FromArgb(14, 82, 52);
        lblPreviewMoneyCaption.Location = new Point(1000, 14);
        lblPreviewMoneyCaption.Name = "lblPreviewMoneyCaption";
        lblPreviewMoneyCaption.Size = new Size(66, 24);
        lblPreviewMoneyCaption.TabIndex = 0;
        lblPreviewMoneyCaption.Text = "المبلغ:";
        // 
        // lblPreviewMoney
        // 
        lblPreviewMoney.AutoSize = true;
        lblPreviewMoney.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblPreviewMoney.ForeColor = Color.FromArgb(14, 82, 52);
        lblPreviewMoney.Location = new Point(640, 14);
        lblPreviewMoney.Name = "lblPreviewMoney";
        lblPreviewMoney.Size = new Size(60, 24);
        lblPreviewMoney.TabIndex = 1;
        lblPreviewMoney.Text = "—";
        // 
        // lblPreviewQuantityCaption
        // 
        lblPreviewQuantityCaption.AutoSize = true;
        lblPreviewQuantityCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblPreviewQuantityCaption.ForeColor = Color.FromArgb(14, 82, 52);
        lblPreviewQuantityCaption.Location = new Point(1000, 48);
        lblPreviewQuantityCaption.Name = "lblPreviewQuantityCaption";
        lblPreviewQuantityCaption.Size = new Size(76, 24);
        lblPreviewQuantityCaption.TabIndex = 2;
        lblPreviewQuantityCaption.Text = "الكمية:";
        // 
        // lblPreviewQuantity
        // 
        lblPreviewQuantity.AutoSize = true;
        lblPreviewQuantity.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblPreviewQuantity.ForeColor = Color.FromArgb(14, 82, 52);
        lblPreviewQuantity.Location = new Point(640, 48);
        lblPreviewQuantity.Name = "lblPreviewQuantity";
        lblPreviewQuantity.Size = new Size(60, 24);
        lblPreviewQuantity.TabIndex = 3;
        lblPreviewQuantity.Text = "—";
        // 
        // lblPreviewNumberCaption
        // 
        lblPreviewNumberCaption.AutoSize = true;
        lblPreviewNumberCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblPreviewNumberCaption.ForeColor = Color.FromArgb(14, 82, 52);
        lblPreviewNumberCaption.Location = new Point(1000, 82);
        lblPreviewNumberCaption.Name = "lblPreviewNumberCaption";
        lblPreviewNumberCaption.Size = new Size(66, 24);
        lblPreviewNumberCaption.TabIndex = 4;
        lblPreviewNumberCaption.Text = "العدد:";
        // 
        // lblPreviewNumber
        // 
        lblPreviewNumber.AutoSize = true;
        lblPreviewNumber.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblPreviewNumber.ForeColor = Color.FromArgb(14, 82, 52);
        lblPreviewNumber.Location = new Point(640, 82);
        lblPreviewNumber.Name = "lblPreviewNumber";
        lblPreviewNumber.Size = new Size(60, 24);
        lblPreviewNumber.TabIndex = 5;
        lblPreviewNumber.Text = "—";
        // 
        // lblPreviewPercentCaption
        // 
        lblPreviewPercentCaption.AutoSize = true;
        lblPreviewPercentCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblPreviewPercentCaption.ForeColor = Color.FromArgb(14, 82, 52);
        lblPreviewPercentCaption.Location = new Point(1000, 116);
        lblPreviewPercentCaption.Name = "lblPreviewPercentCaption";
        lblPreviewPercentCaption.Size = new Size(76, 24);
        lblPreviewPercentCaption.TabIndex = 6;
        lblPreviewPercentCaption.Text = "النسبة:";
        // 
        // lblPreviewPercent
        // 
        lblPreviewPercent.AutoSize = true;
        lblPreviewPercent.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblPreviewPercent.ForeColor = Color.FromArgb(14, 82, 52);
        lblPreviewPercent.Location = new Point(640, 116);
        lblPreviewPercent.Name = "lblPreviewPercent";
        lblPreviewPercent.Size = new Size(60, 24);
        lblPreviewPercent.TabIndex = 7;
        lblPreviewPercent.Text = "—";
        // 
        // lblPreviewDateShortCaption
        // 
        lblPreviewDateShortCaption.AutoSize = true;
        lblPreviewDateShortCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblPreviewDateShortCaption.ForeColor = Color.FromArgb(14, 82, 52);
        lblPreviewDateShortCaption.Location = new Point(1000, 150);
        lblPreviewDateShortCaption.Name = "lblPreviewDateShortCaption";
        lblPreviewDateShortCaption.Size = new Size(136, 24);
        lblPreviewDateShortCaption.TabIndex = 8;
        lblPreviewDateShortCaption.Text = "التاريخ القصير:";
        // 
        // lblPreviewDateShort
        // 
        lblPreviewDateShort.AutoSize = true;
        lblPreviewDateShort.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblPreviewDateShort.ForeColor = Color.FromArgb(14, 82, 52);
        lblPreviewDateShort.Location = new Point(640, 150);
        lblPreviewDateShort.Name = "lblPreviewDateShort";
        lblPreviewDateShort.Size = new Size(60, 24);
        lblPreviewDateShort.TabIndex = 9;
        lblPreviewDateShort.Text = "—";
        // 
        // lblPreviewDateDMYCaption
        // 
        lblPreviewDateDMYCaption.AutoSize = true;
        lblPreviewDateDMYCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblPreviewDateDMYCaption.ForeColor = Color.FromArgb(14, 82, 52);
        lblPreviewDateDMYCaption.Location = new Point(1000, 184);
        lblPreviewDateDMYCaption.Name = "lblPreviewDateDMYCaption";
        lblPreviewDateDMYCaption.Size = new Size(146, 24);
        lblPreviewDateDMYCaption.TabIndex = 10;
        lblPreviewDateDMYCaption.Text = "يوم/شهر/سنة:";
        // 
        // lblPreviewDateDMY
        // 
        lblPreviewDateDMY.AutoSize = true;
        lblPreviewDateDMY.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblPreviewDateDMY.ForeColor = Color.FromArgb(14, 82, 52);
        lblPreviewDateDMY.Location = new Point(640, 184);
        lblPreviewDateDMY.Name = "lblPreviewDateDMY";
        lblPreviewDateDMY.Size = new Size(60, 24);
        lblPreviewDateDMY.TabIndex = 11;
        lblPreviewDateDMY.Text = "—";
        // 
        // lblPreviewDateLongCaption
        // 
        lblPreviewDateLongCaption.AutoSize = true;
        lblPreviewDateLongCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblPreviewDateLongCaption.ForeColor = Color.FromArgb(14, 82, 52);
        lblPreviewDateLongCaption.Location = new Point(1000, 218);
        lblPreviewDateLongCaption.Name = "lblPreviewDateLongCaption";
        lblPreviewDateLongCaption.Size = new Size(148, 24);
        lblPreviewDateLongCaption.TabIndex = 12;
        lblPreviewDateLongCaption.Text = "التاريخ والوقت:";
        // 
        // lblPreviewDateLong
        // 
        lblPreviewDateLong.AutoSize = true;
        lblPreviewDateLong.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblPreviewDateLong.ForeColor = Color.FromArgb(14, 82, 52);
        lblPreviewDateLong.Location = new Point(640, 218);
        lblPreviewDateLong.Name = "lblPreviewDateLong";
        lblPreviewDateLong.Size = new Size(60, 24);
        lblPreviewDateLong.TabIndex = 13;
        lblPreviewDateLong.Text = "—";
        // 
        // lblPreviewSample
        // 
        lblPreviewSample.Font = new Font("Tahoma", 9F);
        lblPreviewSample.ForeColor = Color.FromArgb(14, 82, 52);
        lblPreviewSample.Location = new Point(10, 254);
        lblPreviewSample.Name = "lblPreviewSample";
        lblPreviewSample.Size = new Size(1140, 26);
        lblPreviewSample.TabIndex = 14;
        lblPreviewSample.Text = "العيّنة: 1234567.891 · والتاريخ 2026-11-14 15:30";
        lblPreviewSample.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblValidation
        // 
        lblValidation.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblValidation.ForeColor = Color.FromArgb(220, 53, 69);
        lblValidation.Location = new Point(10, 286);
        lblValidation.Name = "lblValidation";
        lblValidation.Size = new Size(1140, 28);
        lblValidation.TabIndex = 15;
        lblValidation.TextAlign = ContentAlignment.MiddleRight;
        // 
        // pnlSummary
        // 
        pnlSummary.BackColor = Color.FromArgb(235, 240, 235);
        pnlSummary.Controls.Add(lblSummary);
        pnlSummary.Dock = DockStyle.Bottom;
        pnlSummary.Location = new Point(0, 728);
        pnlSummary.Margin = new Padding(0);
        pnlSummary.Name = "pnlSummary";
        pnlSummary.Size = new Size(1180, 42);
        pnlSummary.TabIndex = 4;
        // 
        // lblSummary
        // 
        lblSummary.Dock = DockStyle.Fill;
        lblSummary.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblSummary.ForeColor = Color.FromArgb(14, 82, 52);
        lblSummary.Location = new Point(0, 0);
        lblSummary.Name = "lblSummary";
        lblSummary.Size = new Size(1180, 42);
        lblSummary.TabIndex = 0;
        lblSummary.Text = "عدد الإعدادات: 10 · المعدّلة: 0";
        lblSummary.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlFooter
        // 
        pnlFooter.BackColor = Color.FromArgb(235, 240, 235);
        pnlFooter.Controls.Add(lblFooterHint);
        pnlFooter.Controls.Add(flpFooter);
        pnlFooter.Dock = DockStyle.Bottom;
        pnlFooter.Location = new Point(0, 770);
        pnlFooter.Margin = new Padding(0);
        pnlFooter.Name = "pnlFooter";
        pnlFooter.Size = new Size(1180, 60);
        pnlFooter.TabIndex = 5;
        // 
        // lblFooterHint
        // 
        lblFooterHint.Dock = DockStyle.Fill;
        lblFooterHint.Font = new Font("Tahoma", 9F);
        lblFooterHint.ForeColor = Color.FromArgb(14, 82, 52);
        lblFooterHint.Location = new Point(0, 0);
        lblFooterHint.Name = "lblFooterHint";
        lblFooterHint.Padding = new Padding(16, 0, 0, 0);
        lblFooterHint.Size = new Size(660, 60);
        lblFooterHint.TabIndex = 0;
        lblFooterHint.Text = "اضبط القيم ثم راقب المعاينة الفورية — لا يُطبَّق شيء على الشاشات قبل «حفظ وتطبيق».";
        lblFooterHint.TextAlign = ContentAlignment.MiddleRight;
        // 
        // flpFooter
        // 
        flpFooter.BackColor = Color.FromArgb(235, 240, 235);
        flpFooter.Controls.Add(btnSave);
        flpFooter.Controls.Add(btnReset);
        flpFooter.Controls.Add(btnClose);
        flpFooter.Dock = DockStyle.Right;
        flpFooter.FlowDirection = FlowDirection.LeftToRight;
        flpFooter.Location = new Point(660, 0);
        flpFooter.Name = "flpFooter";
        flpFooter.Padding = new Padding(8, 11, 8, 11);
        flpFooter.Size = new Size(520, 60);
        flpFooter.TabIndex = 1;
        flpFooter.WrapContents = false;
        // 
        // btnSave
        // 
        btnSave.BackColor = Color.FromArgb(25, 135, 84);
        btnSave.Cursor = Cursors.Hand;
        btnSave.FlatAppearance.BorderSize = 0;
        btnSave.FlatStyle = FlatStyle.Flat;
        btnSave.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        btnSave.ForeColor = Color.White;
        btnSave.Location = new Point(8, 11);
        btnSave.Margin = new Padding(0, 0, 8, 0);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(150, 38);
        btnSave.TabIndex = 0;
        btnSave.Text = "حفظ وتطبيق";
        toolTip.SetToolTip(btnSave, "حفظ إعدادات التنسيق وتطبيقها على النظام كله (Enter)");
        btnSave.UseVisualStyleBackColor = false;
        btnSave.Click += btnSave_Click;
        // 
        // btnReset
        // 
        btnReset.BackColor = Color.FromArgb(108, 117, 125);
        btnReset.Cursor = Cursors.Hand;
        btnReset.FlatAppearance.BorderSize = 0;
        btnReset.FlatStyle = FlatStyle.Flat;
        btnReset.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        btnReset.ForeColor = Color.White;
        btnReset.Location = new Point(166, 11);
        btnReset.Margin = new Padding(0, 0, 8, 0);
        btnReset.Name = "btnReset";
        btnReset.Size = new Size(170, 38);
        btnReset.TabIndex = 1;
        btnReset.Text = "استعادة الافتراضي";
        toolTip.SetToolTip(btnReset, "إعادة كل إعدادات التنسيق إلى قيمها الافتراضية بعد تأكيد");
        btnReset.UseVisualStyleBackColor = false;
        btnReset.Click += btnReset_Click;
        // 
        // btnClose
        // 
        btnClose.BackColor = Color.FromArgb(108, 117, 125);
        btnClose.Cursor = Cursors.Hand;
        btnClose.FlatAppearance.BorderSize = 0;
        btnClose.FlatStyle = FlatStyle.Flat;
        btnClose.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        btnClose.ForeColor = Color.White;
        btnClose.Location = new Point(344, 11);
        btnClose.Margin = new Padding(0);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(120, 38);
        btnClose.TabIndex = 2;
        btnClose.Text = "إغلاق";
        toolTip.SetToolTip(btnClose, "إغلاق الشاشة بلا حفظ (Esc)");
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += btnClose_Click;
        // 
        // frmFormatingSetting
        // 
        AutoScaleDimensions = new SizeF(11F, 24F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(170, 220, 170);
        ClientSize = new Size(1180, 830);
        Controls.Add(gbPreview);
        Controls.Add(pnlSummary);
        Controls.Add(pnlFooter);
        Controls.Add(gbDates);
        Controls.Add(gbNumbers);
        Controls.Add(pnlHeader);
        Font = new Font("Tahoma", 10F);
        MinimumSize = new Size(1120, 780);
        Name = "frmFormatingSetting";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterParent;
        Text = "إعدادات التنسيق";
        Load += frmFormatingSetting_Load;
        pnlHeader.ResumeLayout(false);
        gbNumbers.ResumeLayout(false);
        gbNumbers.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numMoneyDecimals).EndInit();
        ((System.ComponentModel.ISupportInitialize)numQuantityDecimals).EndInit();
        ((System.ComponentModel.ISupportInitialize)numNumberDecimals).EndInit();
        ((System.ComponentModel.ISupportInitialize)numPercentDecimals).EndInit();
        gbDates.ResumeLayout(false);
        gbDates.PerformLayout();
        gbPreview.ResumeLayout(false);
        gbPreview.PerformLayout();
        pnlPreviewScroll.ResumeLayout(false);
        pnlPreviewScroll.PerformLayout();
        pnlSummary.ResumeLayout(false);
        pnlFooter.ResumeLayout(false);
        flpFooter.ResumeLayout(false);
        ResumeLayout(false);
    }

    private System.Windows.Forms.Panel pnlHeader;
    private System.Windows.Forms.Label lblMode;
    private System.Windows.Forms.GroupBox gbNumbers;
    private System.Windows.Forms.Label lblMoneyDecimals;
    private System.Windows.Forms.NumericUpDown numMoneyDecimals;
    private System.Windows.Forms.Label lblQuantityDecimals;
    private System.Windows.Forms.NumericUpDown numQuantityDecimals;
    private System.Windows.Forms.Label lblNumberDecimals;
    private System.Windows.Forms.NumericUpDown numNumberDecimals;
    private System.Windows.Forms.Label lblPercentDecimals;
    private System.Windows.Forms.NumericUpDown numPercentDecimals;
    private System.Windows.Forms.CheckBox chkThousandsSeparator;
    private System.Windows.Forms.Label lblDecimalSeparator;
    private System.Windows.Forms.ComboBox cmbDecimalSeparator;
    private System.Windows.Forms.CheckBox chkArabicIndicDigits;
    private System.Windows.Forms.GroupBox gbDates;
    private System.Windows.Forms.Label lblDateShortPattern;
    private System.Windows.Forms.TextBox txtDateShortPattern;
    private System.Windows.Forms.Label lblDateDayMonthYearPattern;
    private System.Windows.Forms.TextBox txtDateDayMonthYearPattern;
    private System.Windows.Forms.Label lblDateLongPattern;
    private System.Windows.Forms.TextBox txtDateLongPattern;
    private System.Windows.Forms.Label lblDateHint;
    private System.Windows.Forms.GroupBox gbPreview;
    private System.Windows.Forms.Panel pnlPreviewScroll;
    private System.Windows.Forms.Label lblPreviewMoneyCaption;
    private System.Windows.Forms.Label lblPreviewMoney;
    private System.Windows.Forms.Label lblPreviewQuantityCaption;
    private System.Windows.Forms.Label lblPreviewQuantity;
    private System.Windows.Forms.Label lblPreviewNumberCaption;
    private System.Windows.Forms.Label lblPreviewNumber;
    private System.Windows.Forms.Label lblPreviewPercentCaption;
    private System.Windows.Forms.Label lblPreviewPercent;
    private System.Windows.Forms.Label lblPreviewDateShortCaption;
    private System.Windows.Forms.Label lblPreviewDateShort;
    private System.Windows.Forms.Label lblPreviewDateDMYCaption;
    private System.Windows.Forms.Label lblPreviewDateDMY;
    private System.Windows.Forms.Label lblPreviewDateLongCaption;
    private System.Windows.Forms.Label lblPreviewDateLong;
    private System.Windows.Forms.Label lblPreviewSample;
    private System.Windows.Forms.Label lblValidation;
    private System.Windows.Forms.Panel pnlSummary;
    private System.Windows.Forms.Label lblSummary;
    private System.Windows.Forms.Panel pnlFooter;
    private System.Windows.Forms.Label lblFooterHint;
    private System.Windows.Forms.FlowLayoutPanel flpFooter;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.Button btnReset;
    private System.Windows.Forms.Button btnClose;
    private System.Windows.Forms.ToolTip toolTip;
}
