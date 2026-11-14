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
        cmbMoneyDecimals = new ComboBox();
        lblQuantityDecimals = new Label();
        cmbQuantityDecimals = new ComboBox();
        lblNumberDecimals = new Label();
        cmbNumberDecimals = new ComboBox();
        lblPercentDecimals = new Label();
        cmbPercentDecimals = new ComboBox();
        lblThousandsSeparator = new Label();
        pnlThousandsOptions = new Panel();
        rdoThousandsYes = new RadioButton();
        rdoThousandsNo = new RadioButton();
        lblArabicIndicDigits = new Label();
        pnlArabicDigitsOptions = new Panel();
        rdoArabicYes = new RadioButton();
        rdoArabicNo = new RadioButton();
        lblDecimalSeparator = new Label();
        cmbDecimalSeparator = new ComboBox();
        gbDates = new GroupBox();
        lblDateShortPattern = new Label();
        cmbDateShortPattern = new ComboBox();
        lblDateDayMonthYearPattern = new Label();
        cmbDateDayMonthYearPattern = new ComboBox();
        lblDateLongPattern = new Label();
        cmbDateLongPattern = new ComboBox();
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
        pnlThousandsOptions.SuspendLayout();
        pnlArabicDigitsOptions.SuspendLayout();
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
        gbNumbers.Controls.Add(cmbMoneyDecimals);
        gbNumbers.Controls.Add(lblQuantityDecimals);
        gbNumbers.Controls.Add(cmbQuantityDecimals);
        gbNumbers.Controls.Add(lblNumberDecimals);
        gbNumbers.Controls.Add(cmbNumberDecimals);
        gbNumbers.Controls.Add(lblPercentDecimals);
        gbNumbers.Controls.Add(cmbPercentDecimals);
        gbNumbers.Controls.Add(lblThousandsSeparator);
        gbNumbers.Controls.Add(pnlThousandsOptions);
        gbNumbers.Controls.Add(lblArabicIndicDigits);
        gbNumbers.Controls.Add(pnlArabicDigitsOptions);
        gbNumbers.Controls.Add(lblDecimalSeparator);
        gbNumbers.Controls.Add(cmbDecimalSeparator);
        gbNumbers.Dock = DockStyle.Top;
        gbNumbers.Font = new Font("Tahoma", 10F);
        gbNumbers.ForeColor = Color.FromArgb(14, 82, 52);
        gbNumbers.Location = new Point(0, 56);
        gbNumbers.Name = "gbNumbers";
        gbNumbers.Padding = new Padding(8);
        gbNumbers.Size = new Size(1180, 200);
        gbNumbers.TabIndex = 1;
        gbNumbers.TabStop = false;
        gbNumbers.Text = "المبالغ والأرقام";
        // 
        // lblMoneyDecimals
        // 
        lblMoneyDecimals.AutoSize = true;
        lblMoneyDecimals.Location = new Point(1051, 38);
        lblMoneyDecimals.Name = "lblMoneyDecimals";
        lblMoneyDecimals.Size = new Size(113, 24);
        lblMoneyDecimals.TabIndex = 0;
        lblMoneyDecimals.Text = "منازل المبالغ";
        // 
        // cmbMoneyDecimals
        // 
        cmbMoneyDecimals.BackColor = Color.White;
        cmbMoneyDecimals.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbMoneyDecimals.FlatStyle = FlatStyle.Flat;
        cmbMoneyDecimals.Font = new Font("Tahoma", 10F);
        cmbMoneyDecimals.Location = new Point(893, 34);
        cmbMoneyDecimals.Name = "cmbMoneyDecimals";
        cmbMoneyDecimals.Size = new Size(150, 32);
        cmbMoneyDecimals.TabIndex = 1;
        cmbMoneyDecimals.SelectedIndexChanged += Value_Changed;
        // 
        // lblQuantityDecimals
        // 
        lblQuantityDecimals.AutoSize = true;
        lblQuantityDecimals.Location = new Point(763, 38);
        lblQuantityDecimals.Name = "lblQuantityDecimals";
        lblQuantityDecimals.Size = new Size(122, 24);
        lblQuantityDecimals.TabIndex = 2;
        lblQuantityDecimals.Text = "منازل الكميات";
        // 
        // cmbQuantityDecimals
        // 
        cmbQuantityDecimals.BackColor = Color.White;
        cmbQuantityDecimals.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbQuantityDecimals.FlatStyle = FlatStyle.Flat;
        cmbQuantityDecimals.Font = new Font("Tahoma", 10F);
        cmbQuantityDecimals.Location = new Point(605, 34);
        cmbQuantityDecimals.Name = "cmbQuantityDecimals";
        cmbQuantityDecimals.Size = new Size(150, 32);
        cmbQuantityDecimals.TabIndex = 3;
        cmbQuantityDecimals.SelectedIndexChanged += Value_Changed;
        // 
        // lblNumberDecimals
        // 
        lblNumberDecimals.AutoSize = true;
        lblNumberDecimals.Location = new Point(484, 38);
        lblNumberDecimals.Name = "lblNumberDecimals";
        lblNumberDecimals.Size = new Size(113, 24);
        lblNumberDecimals.TabIndex = 4;
        lblNumberDecimals.Text = "منازل الأعداد";
        // 
        // cmbNumberDecimals
        // 
        cmbNumberDecimals.BackColor = Color.White;
        cmbNumberDecimals.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbNumberDecimals.FlatStyle = FlatStyle.Flat;
        cmbNumberDecimals.Font = new Font("Tahoma", 10F);
        cmbNumberDecimals.Location = new Point(326, 34);
        cmbNumberDecimals.Name = "cmbNumberDecimals";
        cmbNumberDecimals.Size = new Size(150, 32);
        cmbNumberDecimals.TabIndex = 5;
        cmbNumberDecimals.SelectedIndexChanged += Value_Changed;
        // 
        // lblPercentDecimals
        // 
        lblPercentDecimals.AutoSize = true;
        lblPercentDecimals.Location = new Point(201, 38);
        lblPercentDecimals.Name = "lblPercentDecimals";
        lblPercentDecimals.Size = new Size(117, 24);
        lblPercentDecimals.TabIndex = 6;
        lblPercentDecimals.Text = "منازل النسب";
        // 
        // cmbPercentDecimals
        // 
        cmbPercentDecimals.BackColor = Color.White;
        cmbPercentDecimals.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbPercentDecimals.FlatStyle = FlatStyle.Flat;
        cmbPercentDecimals.Font = new Font("Tahoma", 10F);
        cmbPercentDecimals.Location = new Point(43, 34);
        cmbPercentDecimals.Name = "cmbPercentDecimals";
        cmbPercentDecimals.Size = new Size(150, 32);
        cmbPercentDecimals.TabIndex = 7;
        cmbPercentDecimals.SelectedIndexChanged += Value_Changed;
        // 
        // lblThousandsSeparator
        // 
        lblThousandsSeparator.AutoSize = true;
        lblThousandsSeparator.Location = new Point(1044, 90);
        lblThousandsSeparator.Name = "lblThousandsSeparator";
        lblThousandsSeparator.Size = new Size(120, 24);
        lblThousandsSeparator.TabIndex = 8;
        lblThousandsSeparator.Text = "فاصل الآلاف";
        // 
        // pnlThousandsOptions
        // 
        pnlThousandsOptions.BackColor = Color.FromArgb(235, 245, 235);
        pnlThousandsOptions.Controls.Add(rdoThousandsYes);
        pnlThousandsOptions.Controls.Add(rdoThousandsNo);
        pnlThousandsOptions.Location = new Point(736, 86);
        pnlThousandsOptions.Name = "pnlThousandsOptions";
        pnlThousandsOptions.Size = new Size(300, 36);
        pnlThousandsOptions.TabIndex = 9;
        // 
        // rdoThousandsYes
        // 
        rdoThousandsYes.AutoSize = true;
        rdoThousandsYes.Checked = true;
        rdoThousandsYes.Font = new Font("Tahoma", 10F);
        rdoThousandsYes.ForeColor = Color.FromArgb(14, 82, 52);
        rdoThousandsYes.Location = new Point(220, 6);
        rdoThousandsYes.Name = "rdoThousandsYes";
        rdoThousandsYes.Size = new Size(66, 26);
        rdoThousandsYes.TabIndex = 0;
        rdoThousandsYes.TabStop = true;
        rdoThousandsYes.Text = "نعم";
        rdoThousandsYes.CheckedChanged += Value_Changed;
        // 
        // rdoThousandsNo
        // 
        rdoThousandsNo.AutoSize = true;
        rdoThousandsNo.Font = new Font("Tahoma", 10F);
        rdoThousandsNo.ForeColor = Color.FromArgb(14, 82, 52);
        rdoThousandsNo.Location = new Point(140, 6);
        rdoThousandsNo.Name = "rdoThousandsNo";
        rdoThousandsNo.Size = new Size(52, 26);
        rdoThousandsNo.TabIndex = 1;
        rdoThousandsNo.TabStop = true;
        rdoThousandsNo.Text = "لا";
        rdoThousandsNo.CheckedChanged += Value_Changed;
        // 
        // lblArabicIndicDigits
        // 
        lblArabicIndicDigits.AutoSize = true;
        lblArabicIndicDigits.Location = new Point(476, 90);
        lblArabicIndicDigits.Name = "lblArabicIndicDigits";
        lblArabicIndicDigits.Size = new Size(240, 24);
        lblArabicIndicDigits.TabIndex = 10;
        lblArabicIndicDigits.Text = "أرقام عربية (٠١٢٣٤٥٦٧٨٩)";
        // 
        // pnlArabicDigitsOptions
        // 
        pnlArabicDigitsOptions.BackColor = Color.FromArgb(235, 245, 235);
        pnlArabicDigitsOptions.Controls.Add(rdoArabicYes);
        pnlArabicDigitsOptions.Controls.Add(rdoArabicNo);
        pnlArabicDigitsOptions.Location = new Point(168, 86);
        pnlArabicDigitsOptions.Name = "pnlArabicDigitsOptions";
        pnlArabicDigitsOptions.Size = new Size(300, 36);
        pnlArabicDigitsOptions.TabIndex = 11;
        // 
        // rdoArabicYes
        // 
        rdoArabicYes.AutoSize = true;
        rdoArabicYes.Font = new Font("Tahoma", 10F);
        rdoArabicYes.ForeColor = Color.FromArgb(14, 82, 52);
        rdoArabicYes.Location = new Point(220, 6);
        rdoArabicYes.Name = "rdoArabicYes";
        rdoArabicYes.Size = new Size(66, 26);
        rdoArabicYes.TabIndex = 0;
        rdoArabicYes.TabStop = true;
        rdoArabicYes.Text = "نعم";
        rdoArabicYes.CheckedChanged += Value_Changed;
        // 
        // rdoArabicNo
        // 
        rdoArabicNo.AutoSize = true;
        rdoArabicNo.Checked = true;
        rdoArabicNo.Font = new Font("Tahoma", 10F);
        rdoArabicNo.ForeColor = Color.FromArgb(14, 82, 52);
        rdoArabicNo.Location = new Point(140, 6);
        rdoArabicNo.Name = "rdoArabicNo";
        rdoArabicNo.Size = new Size(52, 26);
        rdoArabicNo.TabIndex = 1;
        rdoArabicNo.TabStop = true;
        rdoArabicNo.Text = "لا";
        rdoArabicNo.CheckedChanged += Value_Changed;
        // 
        // lblDecimalSeparator
        // 
        lblDecimalSeparator.AutoSize = true;
        lblDecimalSeparator.Location = new Point(1044, 142);
        lblDecimalSeparator.Name = "lblDecimalSeparator";
        lblDecimalSeparator.Size = new Size(120, 24);
        lblDecimalSeparator.TabIndex = 12;
        lblDecimalSeparator.Text = "فاصل الكسر";
        // 
        // cmbDecimalSeparator
        // 
        cmbDecimalSeparator.BackColor = Color.White;
        cmbDecimalSeparator.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbDecimalSeparator.FlatStyle = FlatStyle.Flat;
        cmbDecimalSeparator.Font = new Font("Tahoma", 10F);
        cmbDecimalSeparator.Location = new Point(776, 138);
        cmbDecimalSeparator.Name = "cmbDecimalSeparator";
        cmbDecimalSeparator.Size = new Size(260, 32);
        cmbDecimalSeparator.TabIndex = 13;
        cmbDecimalSeparator.SelectedIndexChanged += Value_Changed;
        // 
        // gbDates
        // 
        gbDates.BackColor = Color.FromArgb(235, 245, 235);
        gbDates.Controls.Add(lblDateShortPattern);
        gbDates.Controls.Add(cmbDateShortPattern);
        gbDates.Controls.Add(lblDateDayMonthYearPattern);
        gbDates.Controls.Add(cmbDateDayMonthYearPattern);
        gbDates.Controls.Add(lblDateLongPattern);
        gbDates.Controls.Add(cmbDateLongPattern);
        gbDates.Controls.Add(lblDateHint);
        gbDates.Dock = DockStyle.Top;
        gbDates.Font = new Font("Tahoma", 10F);
        gbDates.ForeColor = Color.FromArgb(14, 82, 52);
        gbDates.Location = new Point(0, 256);
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
        lblDateShortPattern.Location = new Point(1034, 38);
        lblDateShortPattern.Name = "lblDateShortPattern";
        lblDateShortPattern.Size = new Size(130, 24);
        lblDateShortPattern.TabIndex = 0;
        lblDateShortPattern.Text = "التاريخ القصير";
        // 
        // cmbDateShortPattern
        // 
        cmbDateShortPattern.BackColor = Color.White;
        cmbDateShortPattern.DropDownStyle = ComboBoxStyle.DropDown;
        cmbDateShortPattern.FlatStyle = FlatStyle.Flat;
        cmbDateShortPattern.Font = new Font("Tahoma", 10F);
        cmbDateShortPattern.Location = new Point(806, 34);
        cmbDateShortPattern.Name = "cmbDateShortPattern";
        cmbDateShortPattern.Size = new Size(220, 32);
        cmbDateShortPattern.TabIndex = 1;
        cmbDateShortPattern.SelectedIndexChanged += Value_Changed;
        cmbDateShortPattern.TextChanged += Value_Changed;
        // 
        // lblDateDayMonthYearPattern
        // 
        lblDateDayMonthYearPattern.AutoSize = true;
        lblDateDayMonthYearPattern.Location = new Point(678, 38);
        lblDateDayMonthYearPattern.Name = "lblDateDayMonthYearPattern";
        lblDateDayMonthYearPattern.Size = new Size(120, 24);
        lblDateDayMonthYearPattern.TabIndex = 2;
        lblDateDayMonthYearPattern.Text = "يوم/شهر/سنة";
        // 
        // cmbDateDayMonthYearPattern
        // 
        cmbDateDayMonthYearPattern.BackColor = Color.White;
        cmbDateDayMonthYearPattern.DropDownStyle = ComboBoxStyle.DropDown;
        cmbDateDayMonthYearPattern.FlatStyle = FlatStyle.Flat;
        cmbDateDayMonthYearPattern.Font = new Font("Tahoma", 10F);
        cmbDateDayMonthYearPattern.Location = new Point(450, 34);
        cmbDateDayMonthYearPattern.Name = "cmbDateDayMonthYearPattern";
        cmbDateDayMonthYearPattern.Size = new Size(220, 32);
        cmbDateDayMonthYearPattern.TabIndex = 3;
        cmbDateDayMonthYearPattern.SelectedIndexChanged += Value_Changed;
        cmbDateDayMonthYearPattern.TextChanged += Value_Changed;
        // 
        // lblDateLongPattern
        // 
        lblDateLongPattern.AutoSize = true;
        lblDateLongPattern.Location = new Point(302, 38);
        lblDateLongPattern.Name = "lblDateLongPattern";
        lblDateLongPattern.Size = new Size(140, 24);
        lblDateLongPattern.TabIndex = 4;
        lblDateLongPattern.Text = "التاريخ والوقت";
        // 
        // cmbDateLongPattern
        // 
        cmbDateLongPattern.BackColor = Color.White;
        cmbDateLongPattern.DropDownStyle = ComboBoxStyle.DropDown;
        cmbDateLongPattern.FlatStyle = FlatStyle.Flat;
        cmbDateLongPattern.Font = new Font("Tahoma", 10F);
        cmbDateLongPattern.Location = new Point(74, 34);
        cmbDateLongPattern.Name = "cmbDateLongPattern";
        cmbDateLongPattern.Size = new Size(220, 32);
        cmbDateLongPattern.TabIndex = 5;
        cmbDateLongPattern.SelectedIndexChanged += Value_Changed;
        cmbDateLongPattern.TextChanged += Value_Changed;
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
        gbPreview.Location = new Point(0, 388);
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
        pnlPreviewScroll.Location = new Point(8, 33);
        pnlPreviewScroll.Name = "pnlPreviewScroll";
        pnlPreviewScroll.Size = new Size(1164, 323);
        pnlPreviewScroll.TabIndex = 0;
        // 
        // lblPreviewMoneyCaption
        // 
        lblPreviewMoneyCaption.AutoSize = true;
        lblPreviewMoneyCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblPreviewMoneyCaption.ForeColor = Color.FromArgb(14, 82, 52);
        lblPreviewMoneyCaption.Location = new Point(1000, 14);
        lblPreviewMoneyCaption.Name = "lblPreviewMoneyCaption";
        lblPreviewMoneyCaption.Size = new Size(73, 24);
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
        lblPreviewMoney.Size = new Size(28, 24);
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
        lblPreviewQuantity.Size = new Size(28, 24);
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
        lblPreviewNumberCaption.Size = new Size(65, 24);
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
        lblPreviewNumber.Size = new Size(28, 24);
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
        lblPreviewPercentCaption.Size = new Size(82, 24);
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
        lblPreviewPercent.Size = new Size(28, 24);
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
        lblPreviewDateShortCaption.Size = new Size(145, 24);
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
        lblPreviewDateShort.Size = new Size(28, 24);
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
        lblPreviewDateDMYCaption.Size = new Size(160, 24);
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
        lblPreviewDateDMY.Size = new Size(28, 24);
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
        lblPreviewDateLongCaption.Size = new Size(150, 24);
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
        lblPreviewDateLong.Size = new Size(28, 24);
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
        pnlSummary.Location = new Point(0, 752);
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
        pnlFooter.Location = new Point(0, 794);
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
        btnSave.Location = new Point(354, 11);
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
        btnReset.Location = new Point(159, 11);
        btnReset.Margin = new Padding(0, 0, 8, 0);
        btnReset.Name = "btnReset";
        btnReset.Size = new Size(187, 38);
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
        btnClose.Location = new Point(31, 11);
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
        ClientSize = new Size(1180, 854);
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
        pnlThousandsOptions.ResumeLayout(false);
        pnlThousandsOptions.PerformLayout();
        pnlArabicDigitsOptions.ResumeLayout(false);
        pnlArabicDigitsOptions.PerformLayout();
        gbDates.ResumeLayout(false);
        gbDates.PerformLayout();
        gbPreview.ResumeLayout(false);
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
    private System.Windows.Forms.ComboBox cmbMoneyDecimals;
    private System.Windows.Forms.Label lblQuantityDecimals;
    private System.Windows.Forms.ComboBox cmbQuantityDecimals;
    private System.Windows.Forms.Label lblNumberDecimals;
    private System.Windows.Forms.ComboBox cmbNumberDecimals;
    private System.Windows.Forms.Label lblPercentDecimals;
    private System.Windows.Forms.ComboBox cmbPercentDecimals;
    private System.Windows.Forms.Label lblThousandsSeparator;
    private System.Windows.Forms.Panel pnlThousandsOptions;
    private System.Windows.Forms.RadioButton rdoThousandsYes;
    private System.Windows.Forms.RadioButton rdoThousandsNo;
    private System.Windows.Forms.Label lblArabicIndicDigits;
    private System.Windows.Forms.Panel pnlArabicDigitsOptions;
    private System.Windows.Forms.RadioButton rdoArabicYes;
    private System.Windows.Forms.RadioButton rdoArabicNo;
    private System.Windows.Forms.Label lblDecimalSeparator;
    private System.Windows.Forms.ComboBox cmbDecimalSeparator;
    private System.Windows.Forms.GroupBox gbDates;
    private System.Windows.Forms.Label lblDateShortPattern;
    private System.Windows.Forms.ComboBox cmbDateShortPattern;
    private System.Windows.Forms.Label lblDateDayMonthYearPattern;
    private System.Windows.Forms.ComboBox cmbDateDayMonthYearPattern;
    private System.Windows.Forms.Label lblDateLongPattern;
    private System.Windows.Forms.ComboBox cmbDateLongPattern;
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
