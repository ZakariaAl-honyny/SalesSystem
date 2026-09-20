namespace SalesSystem.Desktop.Screens.CashBoxes.Forms;

partial class frmAddUpdateCashBox
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
        ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
        lblMode = new Label();
        label1 = new Label();
        txtName = new TextBox();
        lblBranch = new Label();
        cmbBranch = new ComboBox();
        label2 = new Label();
        txtDescription = new TextBox();
        pnlAccountingInfo = new Panel();
        lblAccountingInfoTitle = new Label();
        lblAccountCodeLabel = new Label();
        lblAccountCodeValue = new Label();
        lblParentAccountLabel = new Label();
        lblParentAccountValue = new Label();
        lblAccountTypeLabel = new Label();
        lblAccountTypeValue = new Label();
        lblAccountNoteLabel = new Label();
        lblAccountNoteValue = new Label();
        gbInitialBalance = new GroupBox();
        pnlInitialBalanceDetails = new Panel();
        pnlTransferDetails = new Panel();
        lblTransferWarning = new Label();
        cmbSourceCashBox = new ComboBox();
        lblSourceCashBox = new Label();
        rbSourceTransfer = new RadioButton();
        rbSourceOpening = new RadioButton();
        lblSource = new Label();
        txtAmount = new TextBox();
        lblAmount = new Label();
        rbYesInitialBalance = new RadioButton();
        rbNoInitialBalance = new RadioButton();
        lblHasInitialBalance = new Label();
        btnSave = new Button();
        btnClose = new Button();
        pnlAccountingInfo.SuspendLayout();
        gbInitialBalance.SuspendLayout();
        pnlInitialBalanceDetails.SuspendLayout();
        pnlTransferDetails.SuspendLayout();
        SuspendLayout();
        // 
        // lblMode
        // 
        lblMode.Anchor = AnchorStyles.Top;
        lblMode.AutoSize = true;
        lblMode.BackColor = Color.WhiteSmoke;
        lblMode.Font = new Font("Tahoma", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblMode.ForeColor = Color.Firebrick;
        lblMode.Location = new Point(250, 15);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(300, 34);
        lblMode.TabIndex = 0;
        lblMode.Text = "إضافة صندوق نقدي جديد";
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        label1.Location = new Point(12, 55);
        label1.Name = "label1";
        label1.Size = new Size(162, 24);
        label1.TabIndex = 1;
        label1.Text = "اسم الصندوق *";
        // 
        // txtName
        // 
        txtName.Font = new Font("Tahoma", 10F);
        txtName.Location = new Point(180, 52);
        txtName.Name = "txtName";
        txtName.RightToLeft = RightToLeft.Yes;
        txtName.Size = new Size(400, 32);
        txtName.TabIndex = 2;
        // 
        // lblBranch
        // 
        lblBranch.AutoSize = true;
        lblBranch.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblBranch.Location = new Point(24, 97);
        lblBranch.Name = "lblBranch";
        lblBranch.Size = new Size(139, 24);
        lblBranch.TabIndex = 3;
        lblBranch.Text = "الفرع التابع له";
        // 
        // cmbBranch
        // 
        cmbBranch.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbBranch.Font = new Font("Tahoma", 10F);
        cmbBranch.Location = new Point(180, 94);
        cmbBranch.Name = "cmbBranch";
        cmbBranch.RightToLeft = RightToLeft.Yes;
        cmbBranch.Size = new Size(400, 32);
        cmbBranch.TabIndex = 4;
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        label2.Location = new Point(46, 140);
        label2.Name = "label2";
        label2.Size = new Size(95, 24);
        label2.TabIndex = 5;
        label2.Text = "ملاحظات";
        // 
        // txtDescription
        // 
        txtDescription.Font = new Font("Tahoma", 10F);
        txtDescription.Location = new Point(180, 137);
        txtDescription.Multiline = true;
        txtDescription.Name = "txtDescription";
        txtDescription.RightToLeft = RightToLeft.Yes;
        txtDescription.Size = new Size(400, 50);
        txtDescription.TabIndex = 6;
        // 
        // pnlAccountingInfo
        // 
        pnlAccountingInfo.BackColor = Color.FromArgb(235, 245, 255);
        pnlAccountingInfo.BorderStyle = BorderStyle.FixedSingle;
        pnlAccountingInfo.Controls.Add(lblAccountingInfoTitle);
        pnlAccountingInfo.Controls.Add(lblAccountCodeLabel);
        pnlAccountingInfo.Controls.Add(lblAccountCodeValue);
        pnlAccountingInfo.Controls.Add(lblParentAccountLabel);
        pnlAccountingInfo.Controls.Add(lblParentAccountValue);
        pnlAccountingInfo.Controls.Add(lblAccountTypeLabel);
        pnlAccountingInfo.Controls.Add(lblAccountTypeValue);
        pnlAccountingInfo.Controls.Add(lblAccountNoteLabel);
        pnlAccountingInfo.Controls.Add(lblAccountNoteValue);
        pnlAccountingInfo.Location = new Point(50, 200);
        pnlAccountingInfo.Name = "pnlAccountingInfo";
        pnlAccountingInfo.RightToLeft = RightToLeft.Yes;
        pnlAccountingInfo.Size = new Size(700, 157);
        pnlAccountingInfo.TabIndex = 7;
        pnlAccountingInfo.Visible = false;
        // 
        // lblAccountingInfoTitle
        // 
        lblAccountingInfoTitle.BackColor = Color.FromArgb(41, 128, 185);
        lblAccountingInfoTitle.Dock = DockStyle.Top;
        lblAccountingInfoTitle.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblAccountingInfoTitle.ForeColor = Color.White;
        lblAccountingInfoTitle.Location = new Point(0, 0);
        lblAccountingInfoTitle.Name = "lblAccountingInfoTitle";
        lblAccountingInfoTitle.Size = new Size(698, 28);
        lblAccountingInfoTitle.TabIndex = 0;
        lblAccountingInfoTitle.Text = "  التوجيه المحاسبي (آلي) — للقراءة فقط";
        lblAccountingInfoTitle.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblAccountCodeLabel
        // 
        lblAccountCodeLabel.AutoSize = true;
        lblAccountCodeLabel.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblAccountCodeLabel.ForeColor = Color.FromArgb(80, 80, 80);
        lblAccountCodeLabel.Location = new Point(532, 38);
        lblAccountCodeLabel.Name = "lblAccountCodeLabel";
        lblAccountCodeLabel.Size = new Size(123, 22);
        lblAccountCodeLabel.TabIndex = 1;
        lblAccountCodeLabel.Text = "رقم الحساب:";
        // 
        // lblAccountCodeValue
        // 
        lblAccountCodeValue.AutoSize = true;
        lblAccountCodeValue.Font = new Font("Tahoma", 9F);
        lblAccountCodeValue.ForeColor = Color.FromArgb(41, 128, 185);
        lblAccountCodeValue.Location = new Point(355, 38);
        lblAccountCodeValue.Name = "lblAccountCodeValue";
        lblAccountCodeValue.Size = new Size(167, 22);
        lblAccountCodeValue.TabIndex = 2;
        lblAccountCodeValue.Text = "يولّد تلقائياً عند الحفظ";
        // 
        // lblParentAccountLabel
        // 
        lblParentAccountLabel.AutoSize = true;
        lblParentAccountLabel.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblParentAccountLabel.ForeColor = Color.FromArgb(80, 80, 80);
        lblParentAccountLabel.Location = new Point(532, 68);
        lblParentAccountLabel.Name = "lblParentAccountLabel";
        lblParentAccountLabel.Size = new Size(127, 22);
        lblParentAccountLabel.TabIndex = 3;
        lblParentAccountLabel.Text = "الحساب الأب:";
        // 
        // lblParentAccountValue
        // 
        lblParentAccountValue.AutoSize = true;
        lblParentAccountValue.Font = new Font("Tahoma", 9F);
        lblParentAccountValue.ForeColor = Color.FromArgb(41, 128, 185);
        lblParentAccountValue.Location = new Point(332, 68);
        lblParentAccountValue.Name = "lblParentAccountValue";
        lblParentAccountValue.Size = new Size(187, 22);
        lblParentAccountValue.TabIndex = 4;
        lblParentAccountValue.Text = "1101 — النقدية صناديق";
        // 
        // lblAccountTypeLabel
        // 
        lblAccountTypeLabel.AutoSize = true;
        lblAccountTypeLabel.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblAccountTypeLabel.ForeColor = Color.FromArgb(80, 80, 80);
        lblAccountTypeLabel.Location = new Point(532, 98);
        lblAccountTypeLabel.Name = "lblAccountTypeLabel";
        lblAccountTypeLabel.Size = new Size(121, 22);
        lblAccountTypeLabel.TabIndex = 5;
        lblAccountTypeLabel.Text = "نوع الحساب:";
        // 
        // lblAccountTypeValue
        // 
        lblAccountTypeValue.AutoSize = true;
        lblAccountTypeValue.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblAccountTypeValue.ForeColor = Color.FromArgb(41, 87, 141);
        lblAccountTypeValue.Location = new Point(372, 98);
        lblAccountTypeValue.Name = "lblAccountTypeValue";
        lblAccountTypeValue.Size = new Size(126, 22);
        lblAccountTypeValue.TabIndex = 6;
        lblAccountTypeValue.Text = "أصول (Asset)";
        // 
        // lblAccountNoteLabel
        // 
        lblAccountNoteLabel.AutoSize = true;
        lblAccountNoteLabel.Font = new Font("Tahoma", 8F, FontStyle.Bold);
        lblAccountNoteLabel.ForeColor = Color.FromArgb(80, 80, 80);
        lblAccountNoteLabel.Location = new Point(530, 120);
        lblAccountNoteLabel.Name = "lblAccountNoteLabel";
        lblAccountNoteLabel.Size = new Size(157, 19);
        lblAccountNoteLabel.TabIndex = 7;
        lblAccountNoteLabel.Text = "الموقع في الشجرة:";
        // 
        // lblAccountNoteValue
        // 
        lblAccountNoteValue.AutoSize = true;
        lblAccountNoteValue.Font = new Font("Tahoma", 8F, FontStyle.Italic);
        lblAccountNoteValue.ForeColor = Color.FromArgb(100, 100, 100);
        lblAccountNoteValue.Location = new Point(20, 120);
        lblAccountNoteValue.Name = "lblAccountNoteValue";
        lblAccountNoteValue.Size = new Size(0, 19);
        lblAccountNoteValue.TabIndex = 8;
        // 
        // gbInitialBalance
        // 
        gbInitialBalance.Controls.Add(pnlInitialBalanceDetails);
        gbInitialBalance.Controls.Add(rbYesInitialBalance);
        gbInitialBalance.Controls.Add(rbNoInitialBalance);
        gbInitialBalance.Controls.Add(lblHasInitialBalance);
        gbInitialBalance.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        gbInitialBalance.Location = new Point(50, 360);
        gbInitialBalance.Name = "gbInitialBalance";
        gbInitialBalance.RightToLeft = RightToLeft.Yes;
        gbInitialBalance.Size = new Size(700, 270);
        gbInitialBalance.TabIndex = 8;
        gbInitialBalance.TabStop = false;
        gbInitialBalance.Text = "الرصيد الابتدائي والتهيئة";
        // 
        // pnlInitialBalanceDetails
        // 
        pnlInitialBalanceDetails.BorderStyle = BorderStyle.FixedSingle;
        pnlInitialBalanceDetails.Controls.Add(pnlTransferDetails);
        pnlInitialBalanceDetails.Controls.Add(rbSourceTransfer);
        pnlInitialBalanceDetails.Controls.Add(rbSourceOpening);
        pnlInitialBalanceDetails.Controls.Add(lblSource);
        pnlInitialBalanceDetails.Controls.Add(txtAmount);
        pnlInitialBalanceDetails.Controls.Add(lblAmount);
        pnlInitialBalanceDetails.Location = new Point(20, 70);
        pnlInitialBalanceDetails.Name = "pnlInitialBalanceDetails";
        pnlInitialBalanceDetails.Size = new Size(660, 190);
        pnlInitialBalanceDetails.TabIndex = 3;
        pnlInitialBalanceDetails.Visible = false;
        // 
        // pnlTransferDetails
        // 
        pnlTransferDetails.Controls.Add(lblTransferWarning);
        pnlTransferDetails.Controls.Add(cmbSourceCashBox);
        pnlTransferDetails.Controls.Add(lblSourceCashBox);
        pnlTransferDetails.Location = new Point(10, 95);
        pnlTransferDetails.Name = "pnlTransferDetails";
        pnlTransferDetails.Size = new Size(640, 85);
        pnlTransferDetails.TabIndex = 5;
        pnlTransferDetails.Visible = false;
        // 
        // lblTransferWarning
        // 
        lblTransferWarning.AutoSize = true;
        lblTransferWarning.Font = new Font("Tahoma", 9F);
        lblTransferWarning.ForeColor = Color.Firebrick;
        lblTransferWarning.Location = new Point(200, 50);
        lblTransferWarning.Name = "lblTransferWarning";
        lblTransferWarning.Size = new Size(317, 22);
        lblTransferWarning.TabIndex = 2;
        lblTransferWarning.Text = "سيُنقص المبلغ من رصيد الصندوق المصدر";
        lblTransferWarning.Visible = false;
        // 
        // cmbSourceCashBox
        // 
        cmbSourceCashBox.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbSourceCashBox.Font = new Font("Tahoma", 10F);
        cmbSourceCashBox.Location = new Point(120, 7);
        cmbSourceCashBox.Name = "cmbSourceCashBox";
        cmbSourceCashBox.Size = new Size(330, 32);
        cmbSourceCashBox.TabIndex = 1;
        // 
        // lblSourceCashBox
        // 
        lblSourceCashBox.AutoSize = true;
        lblSourceCashBox.Font = new Font("Tahoma", 10F);
        lblSourceCashBox.Location = new Point(470, 10);
        lblSourceCashBox.Name = "lblSourceCashBox";
        lblSourceCashBox.Size = new Size(148, 24);
        lblSourceCashBox.TabIndex = 0;
        lblSourceCashBox.Text = "الصندوق المصدر:";
        // 
        // rbSourceTransfer
        // 
        rbSourceTransfer.AutoSize = true;
        rbSourceTransfer.Font = new Font("Tahoma", 10F);
        rbSourceTransfer.Location = new Point(20, 58);
        rbSourceTransfer.Name = "rbSourceTransfer";
        rbSourceTransfer.Size = new Size(235, 28);
        rbSourceTransfer.TabIndex = 4;
        rbSourceTransfer.Text = "تحويل من صندوق موجود";
        rbSourceTransfer.UseVisualStyleBackColor = true;
        // 
        // rbSourceOpening
        // 
        rbSourceOpening.AutoSize = true;
        rbSourceOpening.Checked = true;
        rbSourceOpening.Font = new Font("Tahoma", 10F);
        rbSourceOpening.Location = new Point(261, 58);
        rbSourceOpening.Name = "rbSourceOpening";
        rbSourceOpening.Size = new Size(263, 28);
        rbSourceOpening.TabIndex = 3;
        rbSourceOpening.TabStop = true;
        rbSourceOpening.Text = "رصيد افتتاحي (خارج النظام)";
        rbSourceOpening.UseVisualStyleBackColor = true;
        // 
        // lblSource
        // 
        lblSource.AutoSize = true;
        lblSource.Font = new Font("Tahoma", 10F);
        lblSource.Location = new Point(525, 60);
        lblSource.Name = "lblSource";
        lblSource.Size = new Size(125, 24);
        lblSource.TabIndex = 2;
        lblSource.Text = "مصدر المبلغ *";
        // 
        // txtAmount
        // 
        txtAmount.Font = new Font("Tahoma", 10F);
        txtAmount.Location = new Point(280, 12);
        txtAmount.Name = "txtAmount";
        txtAmount.Size = new Size(250, 32);
        txtAmount.TabIndex = 1;
        // 
        // lblAmount
        // 
        lblAmount.AutoSize = true;
        lblAmount.Font = new Font("Tahoma", 10F);
        lblAmount.Location = new Point(550, 15);
        lblAmount.Name = "lblAmount";
        lblAmount.Size = new Size(76, 24);
        lblAmount.TabIndex = 0;
        lblAmount.Text = "المبلغ *";
        // 
        // rbYesInitialBalance
        // 
        rbYesInitialBalance.AutoSize = true;
        rbYesInitialBalance.Font = new Font("Tahoma", 10F);
        rbYesInitialBalance.Location = new Point(164, 37);
        rbYesInitialBalance.Name = "rbYesInitialBalance";
        rbYesInitialBalance.Size = new Size(118, 28);
        rbYesInitialBalance.TabIndex = 2;
        rbYesInitialBalance.Text = "نعم، رصيد";
        rbYesInitialBalance.UseVisualStyleBackColor = true;
        // 
        // rbNoInitialBalance
        // 
        rbNoInitialBalance.AutoSize = true;
        rbNoInitialBalance.Checked = true;
        rbNoInitialBalance.Font = new Font("Tahoma", 10F);
        rbNoInitialBalance.Location = new Point(294, 37);
        rbNoInitialBalance.Name = "rbNoInitialBalance";
        rbNoInitialBalance.Size = new Size(134, 28);
        rbNoInitialBalance.TabIndex = 1;
        rbNoInitialBalance.TabStop = true;
        rbNoInitialBalance.Text = "لا، يبدأ بصفر";
        rbNoInitialBalance.UseVisualStyleBackColor = true;
        // 
        // lblHasInitialBalance
        // 
        lblHasInitialBalance.AutoSize = true;
        lblHasInitialBalance.Font = new Font("Tahoma", 10F);
        lblHasInitialBalance.Location = new Point(434, 37);
        lblHasInitialBalance.Name = "lblHasInitialBalance";
        lblHasInitialBalance.Size = new Size(246, 24);
        lblHasInitialBalance.TabIndex = 0;
        lblHasInitialBalance.Text = "هل تريد إضافة رصيد ابتدائي؟";
        // 
        // btnSave
        // 
        btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnSave.BackColor = Color.White;
        btnSave.FlatAppearance.BorderColor = Color.Black;
        btnSave.FlatAppearance.BorderSize = 2;
        btnSave.FlatStyle = FlatStyle.Popup;
        btnSave.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnSave.Location = new Point(500, 645);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(120, 45);
        btnSave.TabIndex = 9;
        btnSave.Text = "حفظ";
        btnSave.UseVisualStyleBackColor = false;
        btnSave.Click += btnSave_Click;
        // 
        // btnClose
        // 
        btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnClose.BackColor = Color.White;
        btnClose.DialogResult = DialogResult.Cancel;
        btnClose.FlatAppearance.BorderColor = Color.Black;
        btnClose.FlatAppearance.BorderSize = 2;
        btnClose.FlatStyle = FlatStyle.Popup;
        btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnClose.Location = new Point(12, 645);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(120, 45);
        btnClose.TabIndex = 10;
        btnClose.Text = "إلغاء";
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += btnClose_Click;
        // 
        // frmCashBoxEditor
        // 
        AcceptButton = btnSave;
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.WhiteSmoke;
        CancelButton = btnClose;
        ClientSize = new Size(800, 700);
        Controls.Add(lblMode);
        Controls.Add(label1);
        Controls.Add(txtName);
        Controls.Add(lblBranch);
        Controls.Add(cmbBranch);
        Controls.Add(label2);
        Controls.Add(txtDescription);
        Controls.Add(pnlAccountingInfo);
        Controls.Add(gbInitialBalance);
        Controls.Add(btnSave);
        Controls.Add(btnClose);
        Font = new Font("Tahoma", 12F);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "frmCashBoxEditor";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "إضافة صندوق نقدي جديد";
        Load += frmCashBoxEditor_Load;
        // errorProvider
        errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        errorProvider.RightToLeft = true;
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        pnlAccountingInfo.ResumeLayout(false);
        pnlAccountingInfo.PerformLayout();
        gbInitialBalance.ResumeLayout(false);
        gbInitialBalance.PerformLayout();
        pnlInitialBalanceDetails.ResumeLayout(false);
        pnlInitialBalanceDetails.PerformLayout();
        pnlTransferDetails.ResumeLayout(false);
        pnlTransferDetails.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private System.Windows.Forms.ErrorProvider errorProvider;
    private Label lblMode;
    private Label label1;
    private TextBox txtName;
    private Label lblBranch;
    private ComboBox cmbBranch;
    private Label label2;
    private TextBox txtDescription;
    private Panel pnlAccountingInfo;
    private Label lblAccountingInfoTitle;
    private Label lblAccountCodeLabel;
    internal Label lblAccountCodeValue;
    private Label lblParentAccountLabel;
    internal Label lblParentAccountValue;
    private Label lblAccountTypeLabel;
    private Label lblAccountTypeValue;
    private Label lblAccountNoteLabel;
    internal Label lblAccountNoteValue;
    private GroupBox gbInitialBalance;
    private Label lblHasInitialBalance;
    private RadioButton rbNoInitialBalance;
    private RadioButton rbYesInitialBalance;
    private Panel pnlInitialBalanceDetails;
    private Label lblAmount;
    private TextBox txtAmount;
    private Label lblSource;
    private RadioButton rbSourceOpening;
    private RadioButton rbSourceTransfer;
    private Panel pnlTransferDetails;
    private Label lblSourceCashBox;
    private ComboBox cmbSourceCashBox;
    private Label lblTransferWarning;
    private Button btnSave;
    private Button btnClose;
}
