namespace SalesSystem.Desktop.Screens.Purchases.Forms
{
    partial class frmPurchaseCurrency
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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            errorProvider = new ErrorProvider(components);
            toolTip = new ToolTip(components);
            tipTafqeet = new ToolTip(components);
            pnlTitle = new Panel();
            lblTitle = new Label();
            pnlBody = new Panel();
            tblMain = new TableLayoutPanel();
            lblPrompt = new Label();
            gbCurrency = new GroupBox();
            tblCards = new TableLayoutPanel();
            pnlYer = new Panel();
            tblYer = new TableLayoutPanel();
            rbYer = new RadioButton();
            lblYerInfo = new Label();
            pnlUsd = new Panel();
            tblUsd = new TableLayoutPanel();
            rbUsd = new RadioButton();
            lblUsdInfo = new Label();
            pnlSar = new Panel();
            tblSar = new TableLayoutPanel();
            rbSar = new RadioButton();
            lblSarInfo = new Label();
            grpRate = new GroupBox();
            tblRate = new TableLayoutPanel();
            lblRateCaption = new Label();
            numRate = new NumericUpDown();
            lblRangeCaption = new Label();
            lblRangeValue = new Label();
            btnRestore = new Button();
            lblRangeStatus = new Label();
            pnlEquiv = new Panel();
            tblEquiv = new TableLayoutPanel();
            lblEquivCaption = new Label();
            lblEquivValue = new Label();
            lblNote = new Label();
            pnlFooter = new Panel();
            lblFooterLine = new Label();
            flpFooter = new FlowLayoutPanel();
            btnOk = new Button();
            btnCancel = new Button();
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numRate).BeginInit();
            pnlTitle.SuspendLayout();
            pnlBody.SuspendLayout();
            tblMain.SuspendLayout();
            gbCurrency.SuspendLayout();
            tblCards.SuspendLayout();
            pnlYer.SuspendLayout();
            tblYer.SuspendLayout();
            pnlUsd.SuspendLayout();
            tblUsd.SuspendLayout();
            pnlSar.SuspendLayout();
            tblSar.SuspendLayout();
            grpRate.SuspendLayout();
            tblRate.SuspendLayout();
            pnlEquiv.SuspendLayout();
            tblEquiv.SuspendLayout();
            pnlFooter.SuspendLayout();
            flpFooter.SuspendLayout();
            SuspendLayout();
            // 
            // errorProvider
            // 
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errorProvider.ContainerControl = this;
            // 
            // pnlTitle
            // 
            pnlTitle.BackColor = Color.FromArgb(14, 165, 103);
            pnlTitle.Controls.Add(lblTitle);
            pnlTitle.Dock = DockStyle.Top;
            pnlTitle.Location = new Point(0, 0);
            pnlTitle.Margin = new Padding(0);
            pnlTitle.Name = "pnlTitle";
            pnlTitle.Size = new Size(480, 46);
            pnlTitle.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.BackColor = Color.FromArgb(14, 165, 103);
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.Font = new Font("Tahoma", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(0, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(480, 46);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "عملة المشتريات";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlBody
            // 
            pnlBody.Controls.Add(tblMain);
            pnlBody.Dock = DockStyle.Fill;
            pnlBody.Location = new Point(0, 46);
            pnlBody.Margin = new Padding(0);
            pnlBody.Name = "pnlBody";
            pnlBody.Padding = new Padding(10, 8, 10, 8);
            pnlBody.Size = new Size(480, 472);
            pnlBody.TabIndex = 1;
            // 
            // tblMain
            // 
            tblMain.ColumnCount = 1;
            tblMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblMain.Controls.Add(lblPrompt, 0, 0);
            tblMain.Controls.Add(gbCurrency, 0, 1);
            tblMain.Controls.Add(grpRate, 0, 2);
            tblMain.Controls.Add(pnlEquiv, 0, 3);
            tblMain.Controls.Add(lblNote, 0, 4);
            tblMain.Dock = DockStyle.Fill;
            tblMain.Location = new Point(10, 8);
            tblMain.Name = "tblMain";
            tblMain.RowCount = 6;
            tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 172F));
            tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 146F));
            tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
            tblMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblMain.Size = new Size(460, 456);
            tblMain.TabIndex = 0;
            // 
            // lblPrompt
            // 
            lblPrompt.Dock = DockStyle.Fill;
            lblPrompt.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblPrompt.ForeColor = Color.FromArgb(14, 82, 52);
            lblPrompt.Location = new Point(3, 0);
            lblPrompt.Name = "lblPrompt";
            lblPrompt.Size = new Size(454, 24);
            lblPrompt.TabIndex = 0;
            lblPrompt.Text = "اختر عملة شراء الفاتورة — يُستخدم سعر الصرف عند تسعير بنود الشراء";
            lblPrompt.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // gbCurrency
            // 
            gbCurrency.BackColor = Color.FromArgb(235, 245, 235);
            gbCurrency.Controls.Add(tblCards);
            gbCurrency.Dock = DockStyle.Fill;
            gbCurrency.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            gbCurrency.ForeColor = Color.FromArgb(14, 82, 52);
            gbCurrency.Location = new Point(3, 27);
            gbCurrency.Name = "gbCurrency";
            gbCurrency.Padding = new Padding(10, 4, 10, 6);
            gbCurrency.Size = new Size(454, 166);
            gbCurrency.TabIndex = 1;
            gbCurrency.TabStop = false;
            gbCurrency.Text = "بيانات عملة الشراء";
            // 
            // tblCards
            // 
            tblCards.ColumnCount = 1;
            tblCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblCards.Controls.Add(pnlYer, 0, 0);
            tblCards.Controls.Add(pnlUsd, 0, 1);
            tblCards.Controls.Add(pnlSar, 0, 2);
            tblCards.Dock = DockStyle.Fill;
            tblCards.Location = new Point(10, 20);
            tblCards.Name = "tblCards";
            tblCards.RowCount = 3;
            tblCards.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
            tblCards.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
            tblCards.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
            tblCards.Size = new Size(434, 140);
            tblCards.TabIndex = 0;
            // 
            // pnlYer
            // 
            pnlYer.BackColor = Color.White;
            pnlYer.BorderStyle = BorderStyle.FixedSingle;
            pnlYer.Controls.Add(tblYer);
            pnlYer.Dock = DockStyle.Fill;
            pnlYer.Location = new Point(3, 3);
            pnlYer.Name = "pnlYer";
            pnlYer.Padding = new Padding(6, 2, 6, 2);
            pnlYer.Size = new Size(428, 40);
            pnlYer.TabIndex = 0;
            // 
            // tblYer
            // 
            tblYer.ColumnCount = 2;
            tblYer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblYer.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220F));
            tblYer.Controls.Add(rbYer, 0, 0);
            tblYer.Controls.Add(lblYerInfo, 1, 0);
            tblYer.Dock = DockStyle.Fill;
            tblYer.Location = new Point(6, 2);
            tblYer.Name = "tblYer";
            tblYer.RowCount = 1;
            tblYer.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblYer.Size = new Size(414, 34);
            tblYer.TabIndex = 0;
            // 
            // rbYer
            // 
            rbYer.BackColor = Color.White;
            rbYer.Dock = DockStyle.Fill;
            rbYer.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            rbYer.ForeColor = Color.FromArgb(14, 82, 52);
            rbYer.Location = new Point(197, 3);
            rbYer.Name = "rbYer";
            rbYer.Size = new Size(214, 28);
            rbYer.TabIndex = 0;
            rbYer.Text = "ريال يمني (محلي)";
            rbYer.UseVisualStyleBackColor = false;
            rbYer.CheckedChanged += rbYer_CheckedChanged;
            // 
            // lblYerInfo
            // 
            lblYerInfo.Dock = DockStyle.Fill;
            lblYerInfo.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblYerInfo.ForeColor = Color.FromArgb(14, 82, 52);
            lblYerInfo.Location = new Point(3, 3);
            lblYerInfo.Name = "lblYerInfo";
            lblYerInfo.Size = new Size(188, 28);
            lblYerInfo.TabIndex = 1;
            lblYerInfo.Text = "الرمز: YER    |    المعامل: 1.000";
            lblYerInfo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pnlUsd
            // 
            pnlUsd.BackColor = Color.White;
            pnlUsd.BorderStyle = BorderStyle.FixedSingle;
            pnlUsd.Controls.Add(tblUsd);
            pnlUsd.Dock = DockStyle.Fill;
            pnlUsd.Location = new Point(3, 49);
            pnlUsd.Name = "pnlUsd";
            pnlUsd.Padding = new Padding(6, 2, 6, 2);
            pnlUsd.Size = new Size(428, 40);
            pnlUsd.TabIndex = 1;
            // 
            // tblUsd
            // 
            tblUsd.ColumnCount = 2;
            tblUsd.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblUsd.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220F));
            tblUsd.Controls.Add(rbUsd, 0, 0);
            tblUsd.Controls.Add(lblUsdInfo, 1, 0);
            tblUsd.Dock = DockStyle.Fill;
            tblUsd.Location = new Point(6, 2);
            tblUsd.Name = "tblUsd";
            tblUsd.RowCount = 1;
            tblUsd.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblUsd.Size = new Size(414, 34);
            tblUsd.TabIndex = 0;
            // 
            // rbUsd
            // 
            rbUsd.BackColor = Color.White;
            rbUsd.Dock = DockStyle.Fill;
            rbUsd.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            rbUsd.ForeColor = Color.FromArgb(14, 82, 52);
            rbUsd.Location = new Point(197, 3);
            rbUsd.Name = "rbUsd";
            rbUsd.Size = new Size(214, 28);
            rbUsd.TabIndex = 0;
            rbUsd.Text = "دولار أمريكي";
            rbUsd.UseVisualStyleBackColor = false;
            rbUsd.CheckedChanged += rbUsd_CheckedChanged;
            // 
            // lblUsdInfo
            // 
            lblUsdInfo.Dock = DockStyle.Fill;
            lblUsdInfo.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblUsdInfo.ForeColor = Color.FromArgb(14, 82, 52);
            lblUsdInfo.Location = new Point(3, 3);
            lblUsdInfo.Name = "lblUsdInfo";
            lblUsdInfo.Size = new Size(188, 28);
            lblUsdInfo.TabIndex = 1;
            lblUsdInfo.Text = "الرمز: USD    |    السعر الرسمي: 250.00 ر.ي";
            lblUsdInfo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pnlSar
            // 
            pnlSar.BackColor = Color.White;
            pnlSar.BorderStyle = BorderStyle.FixedSingle;
            pnlSar.Controls.Add(tblSar);
            pnlSar.Dock = DockStyle.Fill;
            pnlSar.Location = new Point(3, 95);
            pnlSar.Name = "pnlSar";
            pnlSar.Padding = new Padding(6, 2, 6, 2);
            pnlSar.Size = new Size(428, 40);
            pnlSar.TabIndex = 2;
            // 
            // tblSar
            // 
            tblSar.ColumnCount = 2;
            tblSar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblSar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220F));
            tblSar.Controls.Add(rbSar, 0, 0);
            tblSar.Controls.Add(lblSarInfo, 1, 0);
            tblSar.Dock = DockStyle.Fill;
            tblSar.Location = new Point(6, 2);
            tblSar.Name = "tblSar";
            tblSar.RowCount = 1;
            tblSar.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblSar.Size = new Size(414, 34);
            tblSar.TabIndex = 0;
            // 
            // rbSar
            // 
            rbSar.BackColor = Color.White;
            rbSar.Dock = DockStyle.Fill;
            rbSar.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            rbSar.ForeColor = Color.FromArgb(14, 82, 52);
            rbSar.Location = new Point(197, 3);
            rbSar.Name = "rbSar";
            rbSar.Size = new Size(214, 28);
            rbSar.TabIndex = 0;
            rbSar.Text = "ريال سعودي";
            rbSar.UseVisualStyleBackColor = false;
            rbSar.CheckedChanged += rbSar_CheckedChanged;
            // 
            // lblSarInfo
            // 
            lblSarInfo.Dock = DockStyle.Fill;
            lblSarInfo.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblSarInfo.ForeColor = Color.FromArgb(14, 82, 52);
            lblSarInfo.Location = new Point(3, 3);
            lblSarInfo.Name = "lblSarInfo";
            lblSarInfo.Size = new Size(188, 28);
            lblSarInfo.TabIndex = 1;
            lblSarInfo.Text = "الرمز: SAR    |    السعر الرسمي: 66.00 ر.ي";
            lblSarInfo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // grpRate
            // 
            grpRate.BackColor = Color.FromArgb(235, 245, 235);
            grpRate.Controls.Add(tblRate);
            grpRate.Dock = DockStyle.Fill;
            grpRate.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            grpRate.ForeColor = Color.FromArgb(14, 82, 52);
            grpRate.Location = new Point(3, 199);
            grpRate.Name = "grpRate";
            grpRate.Padding = new Padding(10, 4, 10, 6);
            grpRate.Size = new Size(454, 140);
            grpRate.TabIndex = 2;
            grpRate.TabStop = false;
            grpRate.Text = "سعر الصرف (للعملات الأجنبية فقط)";
            // 
            // tblRate
            // 
            tblRate.ColumnCount = 2;
            tblRate.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tblRate.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblRate.Controls.Add(lblRateCaption, 0, 0);
            tblRate.Controls.Add(numRate, 1, 0);
            tblRate.Controls.Add(lblRangeCaption, 0, 1);
            tblRate.Controls.Add(lblRangeValue, 1, 1);
            tblRate.Controls.Add(btnRestore, 0, 2);
            tblRate.Controls.Add(lblRangeStatus, 1, 2);
            tblRate.Dock = DockStyle.Fill;
            tblRate.Location = new Point(10, 20);
            tblRate.Name = "tblRate";
            tblRate.RowCount = 3;
            tblRate.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            tblRate.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tblRate.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblRate.Size = new Size(434, 114);
            tblRate.TabIndex = 0;
            // 
            // lblRateCaption
            // 
            lblRateCaption.Dock = DockStyle.Fill;
            lblRateCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblRateCaption.ForeColor = Color.FromArgb(14, 82, 52);
            lblRateCaption.Margin = new Padding(0, 0, 8, 0);
            lblRateCaption.Name = "lblRateCaption";
            lblRateCaption.Size = new Size(142, 36);
            lblRateCaption.TabIndex = 0;
            lblRateCaption.Text = "سعر الصرف:";
            lblRateCaption.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // numRate
            // 
            numRate.BackColor = Color.White;
            numRate.DecimalPlaces = 2;
            numRate.Dock = DockStyle.Fill;
            numRate.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            numRate.Location = new Point(153, 3);
            numRate.Maximum = 9999999M;
            numRate.Minimum = 0.01M;
            numRate.Name = "numRate";
            numRate.Size = new Size(278, 23);
            numRate.TabIndex = 1;
            numRate.ThousandsSeparator = true;
            numRate.ValueChanged += numRate_ValueChanged;
            // 
            // lblRangeCaption
            // 
            lblRangeCaption.Dock = DockStyle.Fill;
            lblRangeCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblRangeCaption.ForeColor = Color.FromArgb(14, 82, 52);
            lblRangeCaption.Margin = new Padding(0, 0, 8, 0);
            lblRangeCaption.Name = "lblRangeCaption";
            lblRangeCaption.Size = new Size(142, 30);
            lblRangeCaption.TabIndex = 2;
            lblRangeCaption.Text = "النطاق المسموح:";
            lblRangeCaption.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblRangeValue
            // 
            lblRangeValue.Dock = DockStyle.Fill;
            lblRangeValue.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblRangeValue.ForeColor = Color.FromArgb(14, 82, 52);
            lblRangeValue.Location = new Point(153, 36);
            lblRangeValue.Name = "lblRangeValue";
            lblRangeValue.Size = new Size(278, 30);
            lblRangeValue.TabIndex = 3;
            lblRangeValue.Text = "237.50 – 262.50";
            lblRangeValue.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // btnRestore
            // 
            btnRestore.BackColor = Color.FromArgb(248, 249, 250);
            btnRestore.Dock = DockStyle.Fill;
            btnRestore.FlatAppearance.BorderColor = Color.FromArgb(222, 226, 230);
            btnRestore.FlatAppearance.BorderSize = 1;
            btnRestore.FlatStyle = FlatStyle.Flat;
            btnRestore.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnRestore.ForeColor = Color.FromArgb(33, 37, 41);
            btnRestore.Location = new Point(3, 69);
            btnRestore.Name = "btnRestore";
            btnRestore.Size = new Size(144, 42);
            btnRestore.TabIndex = 4;
            btnRestore.Text = "استعادة السعر الرسمي";
            btnRestore.UseVisualStyleBackColor = false;
            btnRestore.Click += btnRestore_Click;
            // 
            // lblRangeStatus
            // 
            lblRangeStatus.Dock = DockStyle.Fill;
            lblRangeStatus.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblRangeStatus.ForeColor = Color.FromArgb(220, 53, 69);
            lblRangeStatus.Location = new Point(153, 66);
            lblRangeStatus.Name = "lblRangeStatus";
            lblRangeStatus.Size = new Size(278, 48);
            lblRangeStatus.TabIndex = 5;
            lblRangeStatus.Text = "تنبيه: السعر المُدخل خارج النطاق المسموح";
            lblRangeStatus.TextAlign = ContentAlignment.MiddleLeft;
            lblRangeStatus.Visible = false;
            // 
            // pnlEquiv
            // 
            pnlEquiv.BackColor = Color.FromArgb(235, 240, 235);
            pnlEquiv.Controls.Add(tblEquiv);
            pnlEquiv.Dock = DockStyle.Fill;
            pnlEquiv.Location = new Point(3, 345);
            pnlEquiv.Name = "pnlEquiv";
            pnlEquiv.Padding = new Padding(10, 4, 10, 4);
            pnlEquiv.Size = new Size(454, 50);
            pnlEquiv.TabIndex = 3;
            // 
            // tblEquiv
            // 
            tblEquiv.ColumnCount = 2;
            tblEquiv.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90F));
            tblEquiv.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblEquiv.Controls.Add(lblEquivCaption, 0, 0);
            tblEquiv.Controls.Add(lblEquivValue, 1, 0);
            tblEquiv.Dock = DockStyle.Fill;
            tblEquiv.Location = new Point(10, 4);
            tblEquiv.Name = "tblEquiv";
            tblEquiv.RowCount = 1;
            tblEquiv.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblEquiv.Size = new Size(434, 42);
            tblEquiv.TabIndex = 0;
            // 
            // lblEquivCaption
            // 
            lblEquivCaption.Dock = DockStyle.Fill;
            lblEquivCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblEquivCaption.ForeColor = Color.FromArgb(14, 82, 52);
            lblEquivCaption.Location = new Point(3, 0);
            lblEquivCaption.Name = "lblEquivCaption";
            lblEquivCaption.Size = new Size(84, 42);
            lblEquivCaption.TabIndex = 0;
            lblEquivCaption.Text = "المعادل:";
            lblEquivCaption.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblEquivValue
            // 
            lblEquivValue.Dock = DockStyle.Fill;
            lblEquivValue.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblEquivValue.ForeColor = Color.FromArgb(14, 82, 52);
            lblEquivValue.Location = new Point(93, 0);
            lblEquivValue.Name = "lblEquivValue";
            lblEquivValue.Size = new Size(338, 42);
            lblEquivValue.TabIndex = 1;
            lblEquivValue.Text = "عملة محلية — 1 ريال يمني = 1.000 ريال يمني (لا يتطلب تحويلاً)";
            lblEquivValue.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblNote
            // 
            lblNote.BackColor = Color.FromArgb(235, 240, 235);
            lblNote.Dock = DockStyle.Fill;
            lblNote.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblNote.ForeColor = Color.FromArgb(14, 82, 52);
            lblNote.Location = new Point(3, 401);
            lblNote.Name = "lblNote";
            lblNote.Size = new Size(454, 20);
            lblNote.TabIndex = 4;
            lblNote.Text = "يُسجَّل سعر الصرف المعتمد على فاتورة الشراء عند إتمام الاعتماد";
            lblNote.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlFooter
            // 
            pnlFooter.BackColor = Color.FromArgb(235, 240, 235);
            pnlFooter.Controls.Add(flpFooter);
            pnlFooter.Controls.Add(lblFooterLine);
            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Location = new Point(0, 518);
            pnlFooter.Margin = new Padding(0);
            pnlFooter.Name = "pnlFooter";
            pnlFooter.Size = new Size(480, 62);
            pnlFooter.TabIndex = 2;
            // 
            // lblFooterLine
            // 
            lblFooterLine.BackColor = Color.FromArgb(205, 225, 205);
            lblFooterLine.Dock = DockStyle.Top;
            lblFooterLine.Location = new Point(0, 0);
            lblFooterLine.Name = "lblFooterLine";
            lblFooterLine.Size = new Size(480, 2);
            lblFooterLine.TabIndex = 0;
            // 
            // flpFooter
            // 
            flpFooter.BackColor = Color.FromArgb(235, 240, 235);
            flpFooter.Controls.Add(btnOk);
            flpFooter.Controls.Add(btnCancel);
            flpFooter.Dock = DockStyle.Fill;
            flpFooter.FlowDirection = FlowDirection.RightToLeft;
            flpFooter.Location = new Point(0, 2);
            flpFooter.Name = "flpFooter";
            flpFooter.Padding = new Padding(10, 8, 10, 6);
            flpFooter.Size = new Size(480, 60);
            flpFooter.TabIndex = 1;
            flpFooter.WrapContents = false;
            // 
            // btnOk
            // 
            btnOk.BackColor = Color.FromArgb(25, 135, 84);
            btnOk.FlatAppearance.BorderSize = 0;
            btnOk.FlatStyle = FlatStyle.Flat;
            btnOk.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnOk.ForeColor = Color.White;
            btnOk.Location = new Point(330, 8);
            btnOk.Margin = new Padding(0, 0, 8, 0);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size(140, 40);
            btnOk.TabIndex = 0;
            btnOk.Text = "تأكيد واعتماد";
            btnOk.UseVisualStyleBackColor = false;
            btnOk.Click += btnOk_Click;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.FromArgb(248, 249, 250);
            btnCancel.FlatAppearance.BorderColor = Color.FromArgb(222, 226, 230);
            btnCancel.FlatAppearance.BorderSize = 1;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCancel.ForeColor = Color.FromArgb(33, 37, 41);
            btnCancel.Location = new Point(230, 8);
            btnCancel.Margin = new Padding(0);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(92, 40);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "إلغاء";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // frmPurchaseCurrency
            // 
            AcceptButton = btnOk;
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.FromArgb(170, 220, 170);
            CancelButton = btnCancel;
            ClientSize = new Size(480, 580);
            Controls.Add(pnlBody);
            Controls.Add(pnlFooter);
            Controls.Add(pnlTitle);
            Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmPurchaseCurrency";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterParent;
            Text = "عملة المشتريات";
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ((System.ComponentModel.ISupportInitialize)numRate).EndInit();
            pnlTitle.ResumeLayout(false);
            pnlBody.ResumeLayout(false);
            tblMain.ResumeLayout(false);
            gbCurrency.ResumeLayout(false);
            tblCards.ResumeLayout(false);
            pnlYer.ResumeLayout(false);
            tblYer.ResumeLayout(false);
            pnlUsd.ResumeLayout(false);
            tblUsd.ResumeLayout(false);
            pnlSar.ResumeLayout(false);
            tblSar.ResumeLayout(false);
            grpRate.ResumeLayout(false);
            tblRate.ResumeLayout(false);
            pnlEquiv.ResumeLayout(false);
            tblEquiv.ResumeLayout(false);
            pnlFooter.ResumeLayout(false);
            flpFooter.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel pnlTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlBody;
        private System.Windows.Forms.TableLayoutPanel tblMain;
        private System.Windows.Forms.Label lblPrompt;
        private System.Windows.Forms.GroupBox gbCurrency;
        private System.Windows.Forms.TableLayoutPanel tblCards;
        private System.Windows.Forms.Panel pnlYer;
        private System.Windows.Forms.TableLayoutPanel tblYer;
        private System.Windows.Forms.RadioButton rbYer;
        private System.Windows.Forms.Label lblYerInfo;
        private System.Windows.Forms.Panel pnlUsd;
        private System.Windows.Forms.TableLayoutPanel tblUsd;
        private System.Windows.Forms.RadioButton rbUsd;
        private System.Windows.Forms.Label lblUsdInfo;
        private System.Windows.Forms.Panel pnlSar;
        private System.Windows.Forms.TableLayoutPanel tblSar;
        private System.Windows.Forms.RadioButton rbSar;
        private System.Windows.Forms.Label lblSarInfo;
        private System.Windows.Forms.GroupBox grpRate;
        private System.Windows.Forms.TableLayoutPanel tblRate;
        private System.Windows.Forms.Label lblRateCaption;
        private System.Windows.Forms.NumericUpDown numRate;
        private System.Windows.Forms.Label lblRangeCaption;
        private System.Windows.Forms.Label lblRangeValue;
        private System.Windows.Forms.Button btnRestore;
        private System.Windows.Forms.Label lblRangeStatus;
        private System.Windows.Forms.Panel pnlEquiv;
        private System.Windows.Forms.TableLayoutPanel tblEquiv;
        private System.Windows.Forms.Label lblEquivCaption;
        private System.Windows.Forms.Label lblEquivValue;
        private System.Windows.Forms.Label lblNote;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Label lblFooterLine;
        private System.Windows.Forms.FlowLayoutPanel flpFooter;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ToolTip tipTafqeet;
    }
}
