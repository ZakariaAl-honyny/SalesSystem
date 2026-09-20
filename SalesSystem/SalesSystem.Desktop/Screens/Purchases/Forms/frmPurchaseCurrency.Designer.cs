namespace SalesSystem.Desktop.Screens.Purchases.Forms;

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
        pnlTitle = new Panel();
        lblTitle = new Label();
        pnlBody = new Panel();
        tblMain = new TableLayoutPanel();
        lblPrompt = new Label();
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
        pnlTitle.SuspendLayout();
        pnlBody.SuspendLayout();
        tblMain.SuspendLayout();
        pnlYer.SuspendLayout();
        tblYer.SuspendLayout();
        pnlUsd.SuspendLayout();
        tblUsd.SuspendLayout();
        pnlSar.SuspendLayout();
        tblSar.SuspendLayout();
        grpRate.SuspendLayout();
        tblRate.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numRate).BeginInit();
        pnlEquiv.SuspendLayout();
        tblEquiv.SuspendLayout();
        pnlFooter.SuspendLayout();
        flpFooter.SuspendLayout();
        SuspendLayout();
        // 
        // pnlTitle
        // 
        pnlTitle.BackColor = Color.Firebrick;
        pnlTitle.Controls.Add(lblTitle);
        pnlTitle.Dock = DockStyle.Top;
        pnlTitle.Name = "pnlTitle";
        pnlTitle.Size = new Size(460, 46);
        pnlTitle.TabIndex = 0;
        // 
        // lblTitle
        // 
        lblTitle.Dock = DockStyle.Fill;
        lblTitle.Font = new Font("Tahoma", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblTitle.ForeColor = Color.White;
        lblTitle.Name = "lblTitle";
        lblTitle.Text = "عملة المشتريات";
        lblTitle.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlBody
        // 
        pnlBody.BackColor = Color.WhiteSmoke;
        pnlBody.Controls.Add(tblMain);
        pnlBody.Dock = DockStyle.Fill;
        pnlBody.Name = "pnlBody";
        pnlBody.Padding = new Padding(10, 8, 10, 8);
        pnlBody.TabIndex = 1;
        // 
        // tblMain
        // 
        tblMain.ColumnCount = 1;
        tblMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tblMain.Controls.Add(lblPrompt, 0, 0);
        tblMain.Controls.Add(pnlYer, 0, 1);
        tblMain.Controls.Add(pnlUsd, 0, 2);
        tblMain.Controls.Add(pnlSar, 0, 3);
        tblMain.Controls.Add(grpRate, 0, 4);
        tblMain.Controls.Add(pnlEquiv, 0, 5);
        tblMain.Controls.Add(lblNote, 0, 6);
        tblMain.Dock = DockStyle.Fill;
        tblMain.Name = "tblMain";
        tblMain.RowCount = 7;
        tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
        tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
        tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
        tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
        tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 142F));
        tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
        tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
        // 
        // lblPrompt
        // 
        lblPrompt.Dock = DockStyle.Fill;
        lblPrompt.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblPrompt.ForeColor = Color.FromArgb(108, 117, 125);
        lblPrompt.Name = "lblPrompt";
        lblPrompt.Text = "اختر عملة شراء الفاتورة — يُستخدم سعر الصرف عند تسعير بنود الشراء";
        lblPrompt.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // pnlYer
        // 
        pnlYer.BackColor = Color.White;
        pnlYer.BorderStyle = BorderStyle.FixedSingle;
        pnlYer.Controls.Add(tblYer);
        pnlYer.Dock = DockStyle.Fill;
        pnlYer.Name = "pnlYer";
        pnlYer.Padding = new Padding(6, 2, 6, 2);
        // 
        // tblYer
        // 
        tblYer.ColumnCount = 2;
        tblYer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tblYer.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220F));
        tblYer.Controls.Add(rbYer, 0, 0);
        tblYer.Controls.Add(lblYerInfo, 1, 0);
        tblYer.Dock = DockStyle.Fill;
        tblYer.Name = "tblYer";
        tblYer.RowCount = 1;
        tblYer.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        // 
        // rbYer
        // 
        rbYer.Dock = DockStyle.Fill;
        rbYer.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        rbYer.ForeColor = Color.FromArgb(33, 43, 54);
        rbYer.Name = "rbYer";
        rbYer.TabIndex = 1;
        rbYer.Text = "ريال يمني (محلي)";
        rbYer.UseVisualStyleBackColor = true;
        rbYer.CheckedChanged += rbYer_CheckedChanged;
        // 
        // lblYerInfo
        // 
        lblYerInfo.Dock = DockStyle.Fill;
        lblYerInfo.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblYerInfo.ForeColor = Color.FromArgb(80, 90, 100);
        lblYerInfo.Name = "lblYerInfo";
        lblYerInfo.Text = "الرمز: YER    |    المعامل: 1.000";
        lblYerInfo.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // pnlUsd
        // 
        pnlUsd.BackColor = Color.White;
        pnlUsd.BorderStyle = BorderStyle.FixedSingle;
        pnlUsd.Controls.Add(tblUsd);
        pnlUsd.Dock = DockStyle.Fill;
        pnlUsd.Name = "pnlUsd";
        pnlUsd.Padding = new Padding(6, 2, 6, 2);
        // 
        // tblUsd
        // 
        tblUsd.ColumnCount = 2;
        tblUsd.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tblUsd.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220F));
        tblUsd.Controls.Add(rbUsd, 0, 0);
        tblUsd.Controls.Add(lblUsdInfo, 1, 0);
        tblUsd.Dock = DockStyle.Fill;
        tblUsd.Name = "tblUsd";
        tblUsd.RowCount = 1;
        tblUsd.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        // 
        // rbUsd
        // 
        rbUsd.Dock = DockStyle.Fill;
        rbUsd.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        rbUsd.ForeColor = Color.FromArgb(33, 43, 54);
        rbUsd.Name = "rbUsd";
        rbUsd.TabIndex = 2;
        rbUsd.Text = "دولار أمريكي";
        rbUsd.UseVisualStyleBackColor = true;
        rbUsd.CheckedChanged += rbUsd_CheckedChanged;
        // 
        // lblUsdInfo
        // 
        lblUsdInfo.Dock = DockStyle.Fill;
        lblUsdInfo.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblUsdInfo.ForeColor = Color.FromArgb(80, 90, 100);
        lblUsdInfo.Name = "lblUsdInfo";
        lblUsdInfo.Text = "الرمز: USD    |    السعر الرسمي: 250.00 ر.ي";
        lblUsdInfo.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // pnlSar
        // 
        pnlSar.BackColor = Color.White;
        pnlSar.BorderStyle = BorderStyle.FixedSingle;
        pnlSar.Controls.Add(tblSar);
        pnlSar.Dock = DockStyle.Fill;
        pnlSar.Name = "pnlSar";
        pnlSar.Padding = new Padding(6, 2, 6, 2);
        // 
        // tblSar
        // 
        tblSar.ColumnCount = 2;
        tblSar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tblSar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220F));
        tblSar.Controls.Add(rbSar, 0, 0);
        tblSar.Controls.Add(lblSarInfo, 1, 0);
        tblSar.Dock = DockStyle.Fill;
        tblSar.Name = "tblSar";
        tblSar.RowCount = 1;
        tblSar.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        // 
        // rbSar
        // 
        rbSar.Dock = DockStyle.Fill;
        rbSar.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        rbSar.ForeColor = Color.FromArgb(33, 43, 54);
        rbSar.Name = "rbSar";
        rbSar.TabIndex = 3;
        rbSar.Text = "ريال سعودي";
        rbSar.UseVisualStyleBackColor = true;
        rbSar.CheckedChanged += rbSar_CheckedChanged;
        // 
        // lblSarInfo
        // 
        lblSarInfo.Dock = DockStyle.Fill;
        lblSarInfo.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblSarInfo.ForeColor = Color.FromArgb(80, 90, 100);
        lblSarInfo.Name = "lblSarInfo";
        lblSarInfo.Text = "الرمز: SAR    |    السعر الرسمي: 66.00 ر.ي";
        lblSarInfo.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // grpRate
        // 
        grpRate.BackColor = Color.White;
        grpRate.Controls.Add(tblRate);
        grpRate.Dock = DockStyle.Fill;
        grpRate.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        grpRate.ForeColor = Color.FromArgb(33, 43, 54);
        grpRate.Name = "grpRate";
        grpRate.Padding = new Padding(10, 4, 10, 4);
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
        tblRate.Name = "tblRate";
        tblRate.RowCount = 3;
        tblRate.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        tblRate.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        tblRate.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        // 
        // lblRateCaption
        // 
        lblRateCaption.Dock = DockStyle.Fill;
        lblRateCaption.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblRateCaption.ForeColor = Color.FromArgb(80, 90, 100);
        lblRateCaption.Name = "lblRateCaption";
        lblRateCaption.Text = "سعر الصرف:";
        lblRateCaption.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // numRate
        // 
        numRate.DecimalPlaces = 2;
        numRate.Dock = DockStyle.Fill;
        numRate.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        numRate.Maximum = 9999999;
        numRate.Minimum = 0.01m;
        numRate.Name = "numRate";
        numRate.ThousandsSeparator = true;
        numRate.TabIndex = 4;
        numRate.ValueChanged += numRate_ValueChanged;
        // 
        // lblRangeCaption
        // 
        lblRangeCaption.Dock = DockStyle.Fill;
        lblRangeCaption.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblRangeCaption.ForeColor = Color.FromArgb(80, 90, 100);
        lblRangeCaption.Name = "lblRangeCaption";
        lblRangeCaption.Text = "النطاق المسموح:";
        lblRangeCaption.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblRangeValue
        // 
        lblRangeValue.Dock = DockStyle.Fill;
        lblRangeValue.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblRangeValue.ForeColor = Color.FromArgb(21, 87, 36);
        lblRangeValue.Name = "lblRangeValue";
        lblRangeValue.Text = "237.50 – 262.50";
        lblRangeValue.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // btnRestore
        // 
        btnRestore.BackColor = Color.FromArgb(232, 244, 253);
        btnRestore.Dock = DockStyle.Fill;
        btnRestore.FlatAppearance.BorderColor = Color.FromArgb(145, 190, 235);
        btnRestore.FlatStyle = FlatStyle.Popup;
        btnRestore.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnRestore.ForeColor = Color.FromArgb(33, 43, 54);
        btnRestore.Name = "btnRestore";
        btnRestore.TabIndex = 5;
        btnRestore.Text = "استعادة السعر الرسمي";
        btnRestore.UseVisualStyleBackColor = false;
        btnRestore.Click += btnRestore_Click;
        // 
        // lblRangeStatus
        // 
        lblRangeStatus.Dock = DockStyle.Fill;
        lblRangeStatus.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblRangeStatus.ForeColor = Color.Firebrick;
        lblRangeStatus.Name = "lblRangeStatus";
        lblRangeStatus.Text = "تنبيه: السعر المُدخل خارج النطاق المسموح";
        lblRangeStatus.TextAlign = ContentAlignment.MiddleLeft;
        lblRangeStatus.Visible = false;
        // 
        // pnlEquiv
        // 
        pnlEquiv.BackColor = Color.FromArgb(232, 244, 253);
        pnlEquiv.Controls.Add(tblEquiv);
        pnlEquiv.Dock = DockStyle.Fill;
        pnlEquiv.Name = "pnlEquiv";
        pnlEquiv.Padding = new Padding(10, 4, 10, 4);
        // 
        // tblEquiv
        // 
        tblEquiv.ColumnCount = 2;
        tblEquiv.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90F));
        tblEquiv.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tblEquiv.Controls.Add(lblEquivCaption, 0, 0);
        tblEquiv.Controls.Add(lblEquivValue, 1, 0);
        tblEquiv.Dock = DockStyle.Fill;
        tblEquiv.Name = "tblEquiv";
        tblEquiv.RowCount = 1;
        tblEquiv.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        // 
        // lblEquivCaption
        // 
        lblEquivCaption.Dock = DockStyle.Fill;
        lblEquivCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblEquivCaption.ForeColor = Color.FromArgb(33, 43, 54);
        lblEquivCaption.Name = "lblEquivCaption";
        lblEquivCaption.Text = "المعادل:";
        lblEquivCaption.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblEquivValue
        // 
        lblEquivValue.Dock = DockStyle.Fill;
        lblEquivValue.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblEquivValue.ForeColor = Color.FromArgb(21, 87, 36);
        lblEquivValue.Name = "lblEquivValue";
        lblEquivValue.Text = "عملة محلية — 1 ريال يمني = 1.000 ريال يمني (لا يتطلب تحويلاً)";
        lblEquivValue.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblNote
        // 
        lblNote.BackColor = Color.FromArgb(255, 249, 231);
        lblNote.Dock = DockStyle.Fill;
        lblNote.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblNote.ForeColor = Color.FromArgb(133, 100, 4);
        lblNote.Name = "lblNote";
        lblNote.Text = "يُسجَّل سعر الصرف المعتمد على فاتورة الشراء عند إتمام الاعتماد";
        lblNote.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlFooter
        // 
        pnlFooter.BackColor = Color.FromArgb(248, 249, 250);
        pnlFooter.Controls.Add(flpFooter);
        pnlFooter.Controls.Add(lblFooterLine);
        pnlFooter.Dock = DockStyle.Bottom;
        pnlFooter.Name = "pnlFooter";
        pnlFooter.Size = new Size(460, 60);
        pnlFooter.TabIndex = 3;
        // 
        // lblFooterLine
        // 
        lblFooterLine.BackColor = Color.FromArgb(222, 226, 230);
        lblFooterLine.Dock = DockStyle.Top;
        lblFooterLine.Height = 1;
        lblFooterLine.Name = "lblFooterLine";
        // 
        // flpFooter
        // 
        flpFooter.Controls.Add(btnOk);
        flpFooter.Controls.Add(btnCancel);
        flpFooter.Dock = DockStyle.Fill;
        flpFooter.FlowDirection = FlowDirection.RightToLeft;
        flpFooter.Name = "flpFooter";
        flpFooter.Padding = new Padding(8, 8, 8, 6);
        // 
        // btnOk
        // 
        btnOk.BackColor = Color.FromArgb(25, 135, 84);
        btnOk.FlatStyle = FlatStyle.Popup;
        btnOk.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnOk.ForeColor = Color.White;
        btnOk.Height = 40;
        btnOk.Margin = new Padding(0, 0, 8, 0);
        btnOk.Name = "btnOk";
        btnOk.Size = new Size(150, 40);
        btnOk.TabIndex = 6;
        btnOk.Text = "تأكيد واعتماد";
        btnOk.UseVisualStyleBackColor = false;
        btnOk.Click += btnOk_Click;
        // 
        // btnCancel
        // 
        btnCancel.BackColor = Color.White;
        btnCancel.DialogResult = DialogResult.Cancel;
        btnCancel.FlatStyle = FlatStyle.Popup;
        btnCancel.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnCancel.ForeColor = Color.FromArgb(33, 43, 54);
        btnCancel.Height = 40;
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(90, 40);
        btnCancel.TabIndex = 7;
        btnCancel.Text = "إلغاء";
        btnCancel.UseVisualStyleBackColor = false;
        btnCancel.Click += btnCancel_Click;
        // 
        // frmPurchaseCurrency
        // 
        AcceptButton = btnOk;
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.WhiteSmoke;
        CancelButton = btnCancel;
        ClientSize = new Size(460, 502);
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
        StartPosition = FormStartPosition.CenterScreen;
        Text = "عملة المشتريات";
        pnlTitle.ResumeLayout(false);
        pnlBody.ResumeLayout(false);
        tblMain.ResumeLayout(false);
        pnlYer.ResumeLayout(false);
        tblYer.ResumeLayout(false);
        pnlUsd.ResumeLayout(false);
        tblUsd.ResumeLayout(false);
        pnlSar.ResumeLayout(false);
        tblSar.ResumeLayout(false);
        grpRate.ResumeLayout(false);
        tblRate.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)numRate).EndInit();
        pnlEquiv.ResumeLayout(false);
        tblEquiv.ResumeLayout(false);
        pnlFooter.ResumeLayout(false);
        flpFooter.ResumeLayout(false);
        ResumeLayout(false);
    }

    #endregion

    private Panel pnlTitle;
    private Label lblTitle;
    private Panel pnlBody;
    private TableLayoutPanel tblMain;
    private Label lblPrompt;
    private Panel pnlYer;
    private TableLayoutPanel tblYer;
    private RadioButton rbYer;
    private Label lblYerInfo;
    private Panel pnlUsd;
    private TableLayoutPanel tblUsd;
    private RadioButton rbUsd;
    private Label lblUsdInfo;
    private Panel pnlSar;
    private TableLayoutPanel tblSar;
    private RadioButton rbSar;
    private Label lblSarInfo;
    private GroupBox grpRate;
    private TableLayoutPanel tblRate;
    private Label lblRateCaption;
    private NumericUpDown numRate;
    private Label lblRangeCaption;
    private Label lblRangeValue;
    private Button btnRestore;
    private Label lblRangeStatus;
    private Panel pnlEquiv;
    private TableLayoutPanel tblEquiv;
    private Label lblEquivCaption;
    private Label lblEquivValue;
    private Label lblNote;
    private Panel pnlFooter;
    private Label lblFooterLine;
    private FlowLayoutPanel flpFooter;
    private Button btnOk;
    private Button btnCancel;
}