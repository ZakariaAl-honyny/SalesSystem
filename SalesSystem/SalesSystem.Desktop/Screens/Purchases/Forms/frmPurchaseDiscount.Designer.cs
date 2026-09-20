namespace SalesSystem.Desktop.Screens.Purchases.Forms;

partial class frmPurchaseDiscount
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
        grpDiscount = new GroupBox();
        tblDiscount = new TableLayoutPanel();
        lblTypeCaption = new Label();
        flpTypes = new FlowLayoutPanel();
        rbAmount = new RadioButton();
        rbPercent = new RadioButton();
        lblValueCaption = new Label();
        tblValueRow = new TableLayoutPanel();
        numDiscount = new NumericUpDown();
        lblUnit = new Label();
        lblRateCaption = new Label();
        cmbTax = new ComboBox();
        grpSummary = new GroupBox();
        tblSummary = new TableLayoutPanel();
        lblSubtotalCaption = new Label();
        lblSubtotalValue = new Label();
        lblDiscountCaption = new Label();
        lblDiscountValue = new Label();
        lblTaxCaption = new Label();
        lblTaxValue = new Label();
        lblNetCaption = new Label();
        lblNetValue = new Label();
        lblNote = new Label();
        pnlFooter = new Panel();
        lblFooterLine = new Label();
        flpFooter = new FlowLayoutPanel();
        btnOk = new Button();
        btnCancel = new Button();
        pnlTitle.SuspendLayout();
        pnlBody.SuspendLayout();
        tblMain.SuspendLayout();
        grpDiscount.SuspendLayout();
        tblDiscount.SuspendLayout();
        flpTypes.SuspendLayout();
        tblValueRow.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numDiscount).BeginInit();
        grpSummary.SuspendLayout();
        tblSummary.SuspendLayout();
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
        pnlTitle.Size = new Size(480, 46);
        pnlTitle.TabIndex = 0;
        // 
        // lblTitle
        // 
        lblTitle.Dock = DockStyle.Fill;
        lblTitle.Font = new Font("Tahoma", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblTitle.ForeColor = Color.White;
        lblTitle.Name = "lblTitle";
        lblTitle.Text = "خصم المشتريات";
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
        tblMain.Controls.Add(grpDiscount, 0, 0);
        tblMain.Controls.Add(grpSummary, 0, 1);
        tblMain.Controls.Add(lblNote, 0, 2);
        tblMain.Dock = DockStyle.Fill;
        tblMain.Name = "tblMain";
        tblMain.RowCount = 4;
        tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 150F));
        tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 158F));
        tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        tblMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        // 
        // grpDiscount
        // 
        grpDiscount.BackColor = Color.White;
        grpDiscount.Controls.Add(tblDiscount);
        grpDiscount.Dock = DockStyle.Fill;
        grpDiscount.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        grpDiscount.ForeColor = Color.FromArgb(33, 43, 54);
        grpDiscount.Name = "grpDiscount";
        grpDiscount.Padding = new Padding(10, 4, 10, 6);
        grpDiscount.Text = "نوع الخصم وقيمته";
        // 
        // tblDiscount
        // 
        tblDiscount.ColumnCount = 2;
        tblDiscount.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
        tblDiscount.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tblDiscount.Controls.Add(lblTypeCaption, 0, 0);
        tblDiscount.Controls.Add(flpTypes, 1, 0);
        tblDiscount.Controls.Add(lblValueCaption, 0, 1);
        tblDiscount.Controls.Add(tblValueRow, 1, 1);
        tblDiscount.Controls.Add(lblRateCaption, 0, 2);
        tblDiscount.Controls.Add(cmbTax, 1, 2);
        tblDiscount.Dock = DockStyle.Fill;
        tblDiscount.Name = "tblDiscount";
        tblDiscount.RowCount = 4;
        tblDiscount.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
        tblDiscount.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
        tblDiscount.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
        tblDiscount.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        // 
        // lblTypeCaption
        // 
        lblTypeCaption.Dock = DockStyle.Fill;
        lblTypeCaption.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblTypeCaption.ForeColor = Color.FromArgb(80, 90, 100);
        lblTypeCaption.Name = "lblTypeCaption";
        lblTypeCaption.Text = "نوع الخصم:";
        lblTypeCaption.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // flpTypes
        // 
        flpTypes.Controls.Add(rbAmount);
        flpTypes.Controls.Add(rbPercent);
        flpTypes.Dock = DockStyle.Fill;
        flpTypes.FlowDirection = FlowDirection.RightToLeft;
        flpTypes.Name = "flpTypes";
        // 
        // rbAmount
        // 
        rbAmount.AutoSize = true;
        rbAmount.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        rbAmount.ForeColor = Color.FromArgb(52, 73, 94);
        rbAmount.Name = "rbAmount";
        rbAmount.Text = "مبلغ";
        rbAmount.CheckedChanged += rbDiscountType_CheckedChanged;
        // 
        // rbPercent
        // 
        rbPercent.AutoSize = true;
        rbPercent.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        rbPercent.ForeColor = Color.FromArgb(52, 73, 94);
        rbPercent.Name = "rbPercent";
        rbPercent.Text = "نسبة %";
        rbPercent.CheckedChanged += rbDiscountType_CheckedChanged;
        // 
        // lblValueCaption
        // 
        lblValueCaption.Dock = DockStyle.Fill;
        lblValueCaption.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblValueCaption.ForeColor = Color.FromArgb(80, 90, 100);
        lblValueCaption.Name = "lblValueCaption";
        lblValueCaption.Text = "قيمة الخصم:";
        lblValueCaption.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // tblValueRow
        // 
        tblValueRow.ColumnCount = 2;
        tblValueRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tblValueRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50F));
        tblValueRow.Controls.Add(numDiscount, 0, 0);
        tblValueRow.Controls.Add(lblUnit, 1, 0);
        tblValueRow.Dock = DockStyle.Fill;
        tblValueRow.Name = "tblValueRow";
        tblValueRow.RowCount = 1;
        tblValueRow.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        // 
        // numDiscount
        // 
        numDiscount.DecimalPlaces = 2;
        numDiscount.Dock = DockStyle.Fill;
        numDiscount.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        numDiscount.Maximum = 9999999;
        numDiscount.Minimum = 0;
        numDiscount.Name = "numDiscount";
        numDiscount.ThousandsSeparator = true;
        numDiscount.ValueChanged += numDiscount_ValueChanged;
        // 
        // lblUnit
        // 
        lblUnit.Dock = DockStyle.Fill;
        lblUnit.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblUnit.ForeColor = Color.FromArgb(108, 117, 125);
        lblUnit.Name = "lblUnit";
        lblUnit.Text = "ريال";
        lblUnit.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblRateCaption
        // 
        lblRateCaption.Dock = DockStyle.Fill;
        lblRateCaption.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblRateCaption.ForeColor = Color.FromArgb(80, 90, 100);
        lblRateCaption.Name = "lblRateCaption";
        lblRateCaption.Text = "الضريبة %:";
        lblRateCaption.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // cmbTax
        // 
        cmbTax.Dock = DockStyle.Fill;
        cmbTax.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbTax.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        cmbTax.FormattingEnabled = true;
        cmbTax.Name = "cmbTax";
        cmbTax.RightToLeft = RightToLeft.Yes;
        cmbTax.SelectedIndexChanged += cmbTax_SelectedIndexChanged;
        // 
        // grpSummary
        // 
        grpSummary.BackColor = Color.White;
        grpSummary.Controls.Add(tblSummary);
        grpSummary.Dock = DockStyle.Fill;
        grpSummary.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        grpSummary.ForeColor = Color.FromArgb(33, 43, 54);
        grpSummary.Name = "grpSummary";
        grpSummary.Padding = new Padding(10, 4, 10, 6);
        grpSummary.Text = "ملخص الحساب";
        // 
        // tblSummary
        // 
        tblSummary.ColumnCount = 2;
        tblSummary.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
        tblSummary.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tblSummary.Controls.Add(lblSubtotalCaption, 0, 0);
        tblSummary.Controls.Add(lblSubtotalValue, 1, 0);
        tblSummary.Controls.Add(lblDiscountCaption, 0, 1);
        tblSummary.Controls.Add(lblDiscountValue, 1, 1);
        tblSummary.Controls.Add(lblTaxCaption, 0, 2);
        tblSummary.Controls.Add(lblTaxValue, 1, 2);
        tblSummary.Controls.Add(lblNetCaption, 0, 3);
        tblSummary.Controls.Add(lblNetValue, 1, 3);
        tblSummary.Dock = DockStyle.Fill;
        tblSummary.Name = "tblSummary";
        tblSummary.RowCount = 5;
        tblSummary.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
        tblSummary.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
        tblSummary.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
        tblSummary.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
        tblSummary.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        // 
        // lblSubtotalCaption
        // 
        lblSubtotalCaption.Dock = DockStyle.Fill;
        lblSubtotalCaption.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblSubtotalCaption.ForeColor = Color.FromArgb(80, 90, 100);
        lblSubtotalCaption.Name = "lblSubtotalCaption";
        lblSubtotalCaption.Text = "المجموع الفرعي:";
        lblSubtotalCaption.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblSubtotalValue
        // 
        lblSubtotalValue.Dock = DockStyle.Fill;
        lblSubtotalValue.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblSubtotalValue.ForeColor = Color.FromArgb(33, 43, 54);
        lblSubtotalValue.Name = "lblSubtotalValue";
        lblSubtotalValue.Text = "0.00";
        lblSubtotalValue.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblDiscountCaption
        // 
        lblDiscountCaption.Dock = DockStyle.Fill;
        lblDiscountCaption.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblDiscountCaption.ForeColor = Color.FromArgb(80, 90, 100);
        lblDiscountCaption.Name = "lblDiscountCaption";
        lblDiscountCaption.Text = "الخصم:";
        lblDiscountCaption.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblDiscountValue
        // 
        lblDiscountValue.Dock = DockStyle.Fill;
        lblDiscountValue.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblDiscountValue.ForeColor = Color.FromArgb(33, 43, 54);
        lblDiscountValue.Name = "lblDiscountValue";
        lblDiscountValue.Text = "0.00";
        lblDiscountValue.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblTaxCaption
        // 
        lblTaxCaption.Dock = DockStyle.Fill;
        lblTaxCaption.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblTaxCaption.ForeColor = Color.FromArgb(80, 90, 100);
        lblTaxCaption.Name = "lblTaxCaption";
        lblTaxCaption.Text = "الضريبة:";
        lblTaxCaption.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblTaxValue
        // 
        lblTaxValue.Dock = DockStyle.Fill;
        lblTaxValue.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblTaxValue.ForeColor = Color.FromArgb(33, 43, 54);
        lblTaxValue.Name = "lblTaxValue";
        lblTaxValue.Text = "0.00";
        lblTaxValue.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblNetCaption
        // 
        lblNetCaption.Dock = DockStyle.Fill;
        lblNetCaption.Font = new Font("Tahoma", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblNetCaption.ForeColor = Color.FromArgb(33, 43, 54);
        lblNetCaption.Name = "lblNetCaption";
        lblNetCaption.Text = "الصافي النهائي:";
        lblNetCaption.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblNetValue
        // 
        lblNetValue.BackColor = Color.FromArgb(232, 244, 253);
        lblNetValue.Dock = DockStyle.Fill;
        lblNetValue.Font = new Font("Tahoma", 13F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblNetValue.ForeColor = Color.FromArgb(21, 87, 36);
        lblNetValue.Name = "lblNetValue";
        lblNetValue.Text = "0.00";
        lblNetValue.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblNote
        // 
        lblNote.BackColor = Color.FromArgb(255, 249, 231);
        lblNote.Dock = DockStyle.Fill;
        lblNote.Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblNote.ForeColor = Color.FromArgb(133, 100, 4);
        lblNote.Name = "lblNote";
        lblNote.Text = "الضريبة تُحسب على المجموع بعد الخصم — (وضع تجريبي: بيانات وهمية في الذاكرة)";
        lblNote.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlFooter
        // 
        pnlFooter.BackColor = Color.FromArgb(248, 249, 250);
        pnlFooter.Controls.Add(flpFooter);
        pnlFooter.Controls.Add(lblFooterLine);
        pnlFooter.Dock = DockStyle.Bottom;
        pnlFooter.Name = "pnlFooter";
        pnlFooter.Size = new Size(480, 60);
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
        btnOk.TabIndex = 0;
        btnOk.Text = "موافق";
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
        btnCancel.TabIndex = 1;
        btnCancel.Text = "إلغاء";
        btnCancel.UseVisualStyleBackColor = false;
        btnCancel.Click += btnCancel_Click;
        // 
        // frmPurchaseDiscount
        // 
        AcceptButton = btnOk;
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.WhiteSmoke;
        CancelButton = btnCancel;
        ClientSize = new Size(480, 480);
        Controls.Add(pnlBody);
        Controls.Add(pnlFooter);
        Controls.Add(pnlTitle);
        Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "frmPurchaseDiscount";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "خصم المشتريات";
        pnlTitle.ResumeLayout(false);
        pnlBody.ResumeLayout(false);
        tblMain.ResumeLayout(false);
        grpDiscount.ResumeLayout(false);
        tblDiscount.ResumeLayout(false);
        flpTypes.ResumeLayout(false);
        flpTypes.PerformLayout();
        tblValueRow.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)numDiscount).EndInit();
        grpSummary.ResumeLayout(false);
        tblSummary.ResumeLayout(false);
        pnlFooter.ResumeLayout(false);
        flpFooter.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Panel pnlTitle;
    private Label lblTitle;
    private Panel pnlBody;
    private TableLayoutPanel tblMain;
    private GroupBox grpDiscount;
    private TableLayoutPanel tblDiscount;
    private Label lblTypeCaption;
    private FlowLayoutPanel flpTypes;
    private RadioButton rbAmount;
    private RadioButton rbPercent;
    private Label lblValueCaption;
    private TableLayoutPanel tblValueRow;
    private NumericUpDown numDiscount;
    private Label lblUnit;
    private Label lblRateCaption;
    private ComboBox cmbTax;
    private GroupBox grpSummary;
    private TableLayoutPanel tblSummary;
    private Label lblSubtotalCaption;
    private Label lblSubtotalValue;
    private Label lblDiscountCaption;
    private Label lblDiscountValue;
    private Label lblTaxCaption;
    private Label lblTaxValue;
    private Label lblNetCaption;
    private Label lblNetValue;
    private Label lblNote;
    private Panel pnlFooter;
    private Label lblFooterLine;
    private FlowLayoutPanel flpFooter;
    private Button btnOk;
    private Button btnCancel;
}