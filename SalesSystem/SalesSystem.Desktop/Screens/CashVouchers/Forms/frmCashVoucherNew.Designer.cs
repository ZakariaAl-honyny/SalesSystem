namespace SalesSystem.Desktop.Screens.CashVouchers.Forms;

partial class frmCashVoucherNew
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        components?.Dispose();base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        errorProvider = new ErrorProvider(components);
        errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        errorProvider.RightToLeft = true;
        ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
        pnlHeader = new Panel();
        lblMode = new Label();
        lblMockBadge = new Label();
        lblAccent = new Label();
        rbReceipt = new RadioButton();
        rbPayment = new RadioButton();
        labelDate = new Label();
        dtpDate = new DateTimePicker();
        labelMethod = new Label();
        cmbMethod = new ComboBox();
        labelTreasury = new Label();
        cmbTreasury = new ComboBox();
        labelPartyType = new Label();
        cmbPartyType = new ComboBox();
        labelContra = new Label();
        cmbContraAccount = new ComboBox();
        labelCostCenter = new Label();
        cmbCostCenter = new ComboBox();
        labelAmount = new Label();
        numAmount = new NumericUpDown();
        labelTafqit = new Label();
        pnlTafqit = new Panel();
        lblTafqit = new Label();
        labelNotes = new Label();
        txtNotes = new TextBox();
        pnlFooter = new Panel();
        lblFooterLine = new Label();
        btnSave = new Button();
        btnCancel = new Button();
        pnlHeader.SuspendLayout();
        pnlTafqit.SuspendLayout();
        pnlFooter.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numAmount).BeginInit();
        SuspendLayout();
        // 
        // pnlHeader
        // 
        pnlHeader.BackColor = Color.FromArgb(33, 43, 54);
        pnlHeader.Controls.Add(lblMockBadge);
        pnlHeader.Controls.Add(lblMode);
        pnlHeader.Controls.Add(lblAccent);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Name = "pnlHeader";
        pnlHeader.Size = new Size(540, 66);
        pnlHeader.TabIndex = 0;
        // 
        // lblMode
        // 
        lblMode.Anchor = AnchorStyles.Top;
        lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblMode.ForeColor = Color.White;
        lblMode.Location = new Point(120, 12);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(300, 40);
        lblMode.TabIndex = 1;
        lblMode.Text = "سند جديد";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblMockBadge
        // 
        lblMockBadge.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        lblMockBadge.AutoSize = true;
        lblMockBadge.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblMockBadge.ForeColor = Color.FromArgb(241, 196, 15);
        lblMockBadge.Location = new Point(12, 24);
        lblMockBadge.Name = "lblMockBadge";
        lblMockBadge.Size = new Size(248, 22);
        lblMockBadge.TabIndex = 2;
        lblMockBadge.Text = "وضع التجربة (Mock) — بيانات وهمية";
        // 
        // lblAccent
        // 
        lblAccent.BackColor = Color.FromArgb(46, 204, 113);
        lblAccent.Dock = DockStyle.Bottom;
        lblAccent.Height = 4;
        lblAccent.Name = "lblAccent";
        lblAccent.TabIndex = 3;
        // 
        // rbReceipt
        // 
        rbReceipt.Appearance = Appearance.Button;
        rbReceipt.BackColor = Color.White;
        rbReceipt.Cursor = Cursors.Hand;
        rbReceipt.FlatAppearance.BorderColor = Color.FromArgb(222, 226, 230);
        rbReceipt.FlatAppearance.CheckedBackColor = Color.FromArgb(46, 204, 113);
        rbReceipt.FlatStyle = FlatStyle.Flat;
        rbReceipt.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
        rbReceipt.ForeColor = Color.FromArgb(108, 117, 125);
        rbReceipt.Location = new Point(20, 76);
        rbReceipt.Name = "rbReceipt";
        rbReceipt.Size = new Size(130, 38);
        rbReceipt.TabIndex = 1;
        rbReceipt.TabStop = true;
        rbReceipt.Text = "قبض";
        rbReceipt.TextAlign = ContentAlignment.MiddleCenter;
        rbReceipt.UseVisualStyleBackColor = false;
        rbReceipt.CheckedChanged += rbReceipt_CheckedChanged;
        // 
        // rbPayment
        // 
        rbPayment.Appearance = Appearance.Button;
        rbPayment.BackColor = Color.White;
        rbPayment.Cursor = Cursors.Hand;
        rbPayment.FlatAppearance.BorderColor = Color.FromArgb(222, 226, 230);
        rbPayment.FlatAppearance.CheckedBackColor = Color.FromArgb(231, 76, 60);
        rbPayment.FlatStyle = FlatStyle.Flat;
        rbPayment.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
        rbPayment.ForeColor = Color.FromArgb(108, 117, 125);
        rbPayment.Location = new Point(160, 76);
        rbPayment.Name = "rbPayment";
        rbPayment.Size = new Size(130, 38);
        rbPayment.TabIndex = 2;
        rbPayment.TabStop = true;
        rbPayment.Text = "صرف";
        rbPayment.TextAlign = ContentAlignment.MiddleCenter;
        rbPayment.UseVisualStyleBackColor = false;
        rbPayment.CheckedChanged += rbPayment_CheckedChanged;
        // 
        // labelDate
        // 
        labelDate.AutoSize = true;
        labelDate.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        labelDate.ForeColor = Color.FromArgb(52, 73, 94);
        labelDate.Location = new Point(20, 128);
        labelDate.Name = "labelDate";
        labelDate.Size = new Size(70, 24);
        labelDate.TabIndex = 3;
        labelDate.Text = "التاريخ";
        // 
        // dtpDate
        // 
        dtpDate.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        dtpDate.Format = DateTimePickerFormat.Short;
        dtpDate.Location = new Point(155, 126);
        dtpDate.Name = "dtpDate";
        dtpDate.RightToLeft = RightToLeft.Yes;
        dtpDate.Size = new Size(365, 30);
        dtpDate.TabIndex = 4;
        // 
        // labelMethod
        // 
        labelMethod.AutoSize = true;
        labelMethod.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        labelMethod.ForeColor = Color.FromArgb(52, 73, 94);
        labelMethod.Location = new Point(20, 172);
        labelMethod.Name = "labelMethod";
        labelMethod.Size = new Size(101, 24);
        labelMethod.TabIndex = 5;
        labelMethod.Text = "طريقة الدفع";
        // 
        // cmbMethod
        // 
        cmbMethod.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbMethod.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        cmbMethod.FormattingEnabled = true;
        cmbMethod.Location = new Point(155, 170);
        cmbMethod.Name = "cmbMethod";
        cmbMethod.RightToLeft = RightToLeft.Yes;
        cmbMethod.Size = new Size(365, 30);
        cmbMethod.TabIndex = 6;
        // 
        // labelTreasury
        // 
        labelTreasury.AutoSize = true;
        labelTreasury.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        labelTreasury.ForeColor = Color.FromArgb(52, 73, 94);
        labelTreasury.Location = new Point(20, 216);
        labelTreasury.Name = "labelTreasury";
        labelTreasury.Size = new Size(113, 24);
        labelTreasury.TabIndex = 7;
        labelTreasury.Text = "حساب الخزينة";
        // 
        // cmbTreasury
        // 
        cmbTreasury.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbTreasury.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        cmbTreasury.FormattingEnabled = true;
        cmbTreasury.Location = new Point(155, 214);
        cmbTreasury.Name = "cmbTreasury";
        cmbTreasury.RightToLeft = RightToLeft.Yes;
        cmbTreasury.Size = new Size(365, 30);
        cmbTreasury.TabIndex = 8;
        // 
        // labelPartyType
        // 
        labelPartyType.AutoSize = true;
        labelPartyType.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        labelPartyType.ForeColor = Color.FromArgb(52, 73, 94);
        labelPartyType.Location = new Point(20, 260);
        labelPartyType.Name = "labelPartyType";
        labelPartyType.Size = new Size(86, 24);
        labelPartyType.TabIndex = 9;
        labelPartyType.Text = "نوع الطرف";
        // 
        // cmbPartyType
        // 
        cmbPartyType.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbPartyType.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        cmbPartyType.FormattingEnabled = true;
        cmbPartyType.Location = new Point(155, 258);
        cmbPartyType.Name = "cmbPartyType";
        cmbPartyType.RightToLeft = RightToLeft.Yes;
        cmbPartyType.Size = new Size(365, 30);
        cmbPartyType.TabIndex = 10;
        cmbPartyType.SelectedIndexChanged += cmbPartyType_SelectedIndexChanged;
        // 
        // labelContra
        // 
        labelContra.AutoSize = true;
        labelContra.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        labelContra.ForeColor = Color.FromArgb(52, 73, 94);
        labelContra.Location = new Point(20, 304);
        labelContra.Name = "labelContra";
        labelContra.Size = new Size(113, 24);
        labelContra.TabIndex = 11;
        labelContra.Text = "الحساب المقابل";
        // 
        // cmbContraAccount
        // 
        cmbContraAccount.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbContraAccount.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        cmbContraAccount.FormattingEnabled = true;
        cmbContraAccount.Location = new Point(155, 302);
        cmbContraAccount.Name = "cmbContraAccount";
        cmbContraAccount.RightToLeft = RightToLeft.Yes;
        cmbContraAccount.Size = new Size(365, 30);
        cmbContraAccount.TabIndex = 12;
        // 
        // labelCostCenter
        // 
        labelCostCenter.AutoSize = true;
        labelCostCenter.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        labelCostCenter.ForeColor = Color.FromArgb(52, 73, 94);
        labelCostCenter.Location = new Point(20, 348);
        labelCostCenter.Name = "labelCostCenter";
        labelCostCenter.Size = new Size(113, 24);
        labelCostCenter.TabIndex = 13;
        labelCostCenter.Text = "مركز التكلفة";
        // 
        // cmbCostCenter
        // 
        cmbCostCenter.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbCostCenter.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        cmbCostCenter.FormattingEnabled = true;
        cmbCostCenter.Location = new Point(155, 346);
        cmbCostCenter.Name = "cmbCostCenter";
        cmbCostCenter.RightToLeft = RightToLeft.Yes;
        cmbCostCenter.Size = new Size(365, 30);
        cmbCostCenter.TabIndex = 14;
        // 
        // labelAmount
        // 
        labelAmount.AutoSize = true;
        labelAmount.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        labelAmount.ForeColor = Color.FromArgb(52, 73, 94);
        labelAmount.Location = new Point(20, 392);
        labelAmount.Name = "labelAmount";
        labelAmount.Size = new Size(62, 24);
        labelAmount.TabIndex = 15;
        labelAmount.Text = "المبلغ";
        // 
        // numAmount
        // 
        numAmount.DecimalPlaces = 2;
        numAmount.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        numAmount.Location = new Point(155, 390);
        numAmount.Maximum = new decimal(new int[] { 999999999, 0, 0, 0 });
        numAmount.Name = "numAmount";
        numAmount.RightToLeft = RightToLeft.Yes;
        numAmount.Size = new Size(365, 30);
        numAmount.TabIndex = 16;
        numAmount.ThousandsSeparator = true;
        numAmount.ValueChanged += numAmount_ValueChanged;
        // 
        // labelTafqit
        // 
        labelTafqit.AutoSize = true;
        labelTafqit.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        labelTafqit.ForeColor = Color.FromArgb(52, 73, 94);
        labelTafqit.Location = new Point(20, 440);
        labelTafqit.Name = "labelTafqit";
        labelTafqit.Size = new Size(59, 24);
        labelTafqit.TabIndex = 17;
        labelTafqit.Text = "التفقيط";
        // 
        // pnlTafqit
        // 
        pnlTafqit.BackColor = Color.White;
        pnlTafqit.BorderStyle = BorderStyle.FixedSingle;
        pnlTafqit.Controls.Add(lblTafqit);
        pnlTafqit.Location = new Point(155, 434);
        pnlTafqit.Name = "pnlTafqit";
        pnlTafqit.Size = new Size(365, 50);
        pnlTafqit.TabIndex = 18;
        // 
        // lblTafqit
        // 
        lblTafqit.Dock = DockStyle.Fill;
        lblTafqit.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblTafqit.ForeColor = Color.FromArgb(46, 204, 113);
        lblTafqit.Location = new Point(0, 0);
        lblTafqit.Name = "lblTafqit";
        lblTafqit.RightToLeft = RightToLeft.Yes;
        lblTafqit.Size = new Size(363, 48);
        lblTafqit.TabIndex = 0;
        lblTafqit.Text = "صفر ريال يمني فقط";
        lblTafqit.TextAlign = ContentAlignment.MiddleRight;
        // 
        // labelNotes
        // 
        labelNotes.AutoSize = true;
        labelNotes.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        labelNotes.ForeColor = Color.FromArgb(52, 73, 94);
        labelNotes.Location = new Point(20, 500);
        labelNotes.Name = "labelNotes";
        labelNotes.Size = new Size(59, 24);
        labelNotes.TabIndex = 19;
        labelNotes.Text = "البيان";
        // 
        // txtNotes
        // 
        txtNotes.AcceptsReturn = true;
        txtNotes.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        txtNotes.Location = new Point(155, 498);
        txtNotes.Multiline = true;
        txtNotes.Name = "txtNotes";
        txtNotes.RightToLeft = RightToLeft.Yes;
        txtNotes.ScrollBars = ScrollBars.Vertical;
        txtNotes.Size = new Size(365, 74);
        txtNotes.TabIndex = 20;
        // 
        // pnlFooter
        // 
        pnlFooter.BackColor = Color.FromArgb(248, 249, 250);
        pnlFooter.Controls.Add(btnCancel);
        pnlFooter.Controls.Add(btnSave);
        pnlFooter.Controls.Add(lblFooterLine);
        pnlFooter.Dock = DockStyle.Bottom;
        pnlFooter.Name = "pnlFooter";
        pnlFooter.Size = new Size(540, 66);
        pnlFooter.TabIndex = 21;
        // 
        // lblFooterLine
        // 
        lblFooterLine.BackColor = Color.FromArgb(222, 226, 230);
        lblFooterLine.Dock = DockStyle.Top;
        lblFooterLine.Height = 2;
        lblFooterLine.Name = "lblFooterLine";
        lblFooterLine.TabIndex = 2;
        // 
        // btnSave
        // 
        btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnSave.BackColor = Color.FromArgb(25, 135, 84);
        btnSave.FlatAppearance.BorderSize = 0;
        btnSave.FlatStyle = FlatStyle.Flat;
        btnSave.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnSave.ForeColor = Color.White;
        btnSave.Location = new Point(292, 12);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(236, 42);
        btnSave.TabIndex = 22;
        btnSave.Text = "حفظ وترحيل وطباعة [F10]";
        btnSave.UseVisualStyleBackColor = false;
        btnSave.Click += btnSave_Click;
        // 
        // btnCancel
        // 
        btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        btnCancel.BackColor = Color.White;
        btnCancel.DialogResult = DialogResult.Cancel;
        btnCancel.FlatAppearance.BorderColor = Color.FromArgb(222, 226, 230);
        btnCancel.FlatAppearance.BorderSize = 1;
        btnCancel.FlatStyle = FlatStyle.Flat;
        btnCancel.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnCancel.ForeColor = Color.FromArgb(52, 73, 94);
        btnCancel.Location = new Point(12, 12);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(110, 42);
        btnCancel.TabIndex = 23;
        btnCancel.Text = "إلغاء";
        btnCancel.UseVisualStyleBackColor = false;
        btnCancel.Click += btnCancel_Click;
        // 
        // frmCashVoucherNew
        // 
        AcceptButton = btnSave;
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.WhiteSmoke;
        CancelButton = btnCancel;
        ClientSize = new Size(540, 680);
        Controls.Add(rbReceipt);
        Controls.Add(rbPayment);
        Controls.Add(labelDate);
        Controls.Add(dtpDate);
        Controls.Add(labelMethod);
        Controls.Add(cmbMethod);
        Controls.Add(labelTreasury);
        Controls.Add(cmbTreasury);
        Controls.Add(labelPartyType);
        Controls.Add(cmbPartyType);
        Controls.Add(labelContra);
        Controls.Add(cmbContraAccount);
        Controls.Add(labelCostCenter);
        Controls.Add(cmbCostCenter);
        Controls.Add(labelAmount);
        Controls.Add(numAmount);
        Controls.Add(labelTafqit);
        Controls.Add(pnlTafqit);
        Controls.Add(labelNotes);
        Controls.Add(txtNotes);
        Controls.Add(pnlFooter);
        Controls.Add(pnlHeader);
        Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "frmCashVoucherNew";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "سند جديد";
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        pnlHeader.ResumeLayout(false);
        pnlHeader.PerformLayout();
        pnlTafqit.ResumeLayout(false);
        pnlFooter.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)numAmount).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Panel pnlHeader;
    private Label lblMode;
    private Label lblMockBadge;
    private Label lblAccent;
    private RadioButton rbReceipt;
    private RadioButton rbPayment;
    private Label labelDate;
    private DateTimePicker dtpDate;
    private Label labelMethod;
    private ComboBox cmbMethod;
    private Label labelTreasury;
    private ComboBox cmbTreasury;
    private Label labelPartyType;
    private ComboBox cmbPartyType;
    private Label labelContra;
    private ComboBox cmbContraAccount;
    private Label labelCostCenter;
    private ComboBox cmbCostCenter;
    private Label labelAmount;
    private NumericUpDown numAmount;
    private Label labelTafqit;
    private Panel pnlTafqit;
    private Label lblTafqit;
    private Label labelNotes;
    private TextBox txtNotes;
    private Panel pnlFooter;
    private Label lblFooterLine;
    private Button btnSave;
    private Button btnCancel;
    private System.Windows.Forms.ErrorProvider errorProvider;
}