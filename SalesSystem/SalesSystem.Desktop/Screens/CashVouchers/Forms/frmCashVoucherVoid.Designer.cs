namespace SalesSystem.Desktop.Screens.CashVouchers.Forms;

partial class frmCashVoucherVoid
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        components?.Dispose();
        base.Dispose(disposing);
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
        lblMockBadge = new Label();
        lblMode = new Label();
        lblAccent = new Label();
        pnlInfo = new Panel();
        lblVoucherInfo = new Label();
        lblWarning = new Label();
        labelReason = new Label();
        cmbReason = new ComboBox();
        labelReasonDetail = new Label();
        txtReasonDetails = new TextBox();
        pnlFooter = new Panel();
        lblFooterLine = new Label();
        btnConfirm = new Button();
        btnCancel = new Button();
        pnlHeader.SuspendLayout();
        pnlInfo.SuspendLayout();
        pnlFooter.SuspendLayout();
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
        pnlHeader.Size = new Size(480, 56);
        pnlHeader.TabIndex = 0;
        // 
        // lblMockBadge
        // 
        lblMockBadge.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        lblMockBadge.AutoSize = true;
        lblMockBadge.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblMockBadge.ForeColor = Color.FromArgb(241, 196, 15);
        lblMockBadge.Location = new Point(12, 18);
        lblMockBadge.Name = "lblMockBadge";
        lblMockBadge.Size = new Size(248, 22);
        lblMockBadge.TabIndex = 1;
        lblMockBadge.Text = "وضع التجربة (Mock) — بيانات وهمية";
        // 
        // lblMode
        // 
        lblMode.Anchor = AnchorStyles.Top;
        lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblMode.ForeColor = Color.White;
        lblMode.Location = new Point(110, 9);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(260, 38);
        lblMode.TabIndex = 2;
        lblMode.Text = "إلغاء سند";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblAccent
        // 
        lblAccent.BackColor = Color.FromArgb(220, 53, 69);
        lblAccent.Dock = DockStyle.Bottom;
        lblAccent.Height = 4;
        lblAccent.Name = "lblAccent";
        lblAccent.TabIndex = 3;
        // 
        // pnlInfo
        // 
        pnlInfo.BackColor = Color.FromArgb(253, 240, 238);
        pnlInfo.BorderStyle = BorderStyle.FixedSingle;
        pnlInfo.Controls.Add(lblVoucherInfo);
        pnlInfo.Controls.Add(lblWarning);
        pnlInfo.Location = new Point(20, 74);
        pnlInfo.Name = "pnlInfo";
        pnlInfo.Size = new Size(440, 76);
        pnlInfo.TabIndex = 4;
        // 
        // lblVoucherInfo
        // 
        lblVoucherInfo.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblVoucherInfo.ForeColor = Color.FromArgb(52, 73, 94);
        lblVoucherInfo.Location = new Point(12, 8);
        lblVoucherInfo.Name = "lblVoucherInfo";
        lblVoucherInfo.Size = new Size(414, 32);
        lblVoucherInfo.TabIndex = 0;
        lblVoucherInfo.Text = "سيتم إلغاء السند رقم 1003 — قبض — 150,000 ريال يمني";
        lblVoucherInfo.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblWarning
        // 
        lblWarning.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblWarning.ForeColor = Color.FromArgb(192, 57, 43);
        lblWarning.Location = new Point(12, 42);
        lblWarning.Name = "lblWarning";
        lblWarning.Size = new Size(414, 26);
        lblWarning.TabIndex = 1;
        lblWarning.Text = "تحذير: الإلغاء لا يمكن التراجع عنه";
        lblWarning.TextAlign = ContentAlignment.MiddleRight;
        // 
        // labelReason
        // 
        labelReason.AutoSize = true;
        labelReason.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        labelReason.ForeColor = Color.FromArgb(52, 73, 94);
        labelReason.Location = new Point(20, 168);
        labelReason.Name = "labelReason";
        labelReason.Size = new Size(115, 24);
        labelReason.TabIndex = 5;
        labelReason.Text = "سبب الإلغاء *";
        // 
        // cmbReason
        // 
        cmbReason.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbReason.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        cmbReason.FormattingEnabled = true;
        cmbReason.Location = new Point(120, 164);
        cmbReason.Name = "cmbReason";
        cmbReason.RightToLeft = RightToLeft.Yes;
        cmbReason.Size = new Size(340, 30);
        cmbReason.TabIndex = 6;
        // 
        // labelReasonDetail
        // 
        labelReasonDetail.AutoSize = true;
        labelReasonDetail.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        labelReasonDetail.ForeColor = Color.FromArgb(52, 73, 94);
        labelReasonDetail.Location = new Point(20, 208);
        labelReasonDetail.Name = "labelReasonDetail";
        labelReasonDetail.Size = new Size(193, 24);
        labelReasonDetail.TabIndex = 7;
        labelReasonDetail.Text = "تفاصيل إضافية (اختياري)";
        // 
        // txtReasonDetails
        // 
        txtReasonDetails.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        txtReasonDetails.Location = new Point(120, 204);
        txtReasonDetails.Multiline = true;
        txtReasonDetails.Name = "txtReasonDetails";
        txtReasonDetails.RightToLeft = RightToLeft.Yes;
        txtReasonDetails.ScrollBars = ScrollBars.Vertical;
        txtReasonDetails.Size = new Size(340, 70);
        txtReasonDetails.TabIndex = 8;
        // 
        // pnlFooter
        // 
        pnlFooter.BackColor = Color.FromArgb(248, 249, 250);
        pnlFooter.Controls.Add(btnCancel);
        pnlFooter.Controls.Add(btnConfirm);
        pnlFooter.Controls.Add(lblFooterLine);
        pnlFooter.Dock = DockStyle.Bottom;
        pnlFooter.Name = "pnlFooter";
        pnlFooter.Size = new Size(480, 54);
        pnlFooter.TabIndex = 9;
        // 
        // lblFooterLine
        // 
        lblFooterLine.BackColor = Color.FromArgb(222, 226, 230);
        lblFooterLine.Dock = DockStyle.Top;
        lblFooterLine.Height = 2;
        lblFooterLine.Name = "lblFooterLine";
        lblFooterLine.TabIndex = 2;
        // 
        // btnConfirm
        // 
        btnConfirm.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnConfirm.BackColor = Color.FromArgb(220, 53, 69);
        btnConfirm.FlatAppearance.BorderSize = 0;
        btnConfirm.FlatStyle = FlatStyle.Flat;
        btnConfirm.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnConfirm.ForeColor = Color.White;
        btnConfirm.Location = new Point(248, 8);
        btnConfirm.Name = "btnConfirm";
        btnConfirm.Size = new Size(220, 40);
        btnConfirm.TabIndex = 10;
        btnConfirm.Text = "تأكيد الإلغاء [F10]";
        btnConfirm.UseVisualStyleBackColor = false;
        btnConfirm.Click += btnConfirm_Click;
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
        btnCancel.Location = new Point(12, 8);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(110, 40);
        btnCancel.TabIndex = 11;
        btnCancel.Text = "تراجع";
        btnCancel.UseVisualStyleBackColor = false;
        btnCancel.Click += btnCancel_Click;
        // 
        // frmCashVoucherVoid
        // 
        AcceptButton = btnConfirm;
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.WhiteSmoke;
        CancelButton = btnCancel;
        ClientSize = new Size(480, 360);
        Controls.Add(pnlInfo);
        Controls.Add(labelReason);
        Controls.Add(cmbReason);
        Controls.Add(labelReasonDetail);
        Controls.Add(txtReasonDetails);
        Controls.Add(pnlFooter);
        Controls.Add(pnlHeader);
        Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "frmCashVoucherVoid";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "إلغاء سند";
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        pnlHeader.ResumeLayout(false);
        pnlHeader.PerformLayout();
        pnlInfo.ResumeLayout(false);
        pnlFooter.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Panel pnlHeader;
    private Label lblMockBadge;
    private Label lblMode;
    private Label lblAccent;
    private Panel pnlInfo;
    private Label lblVoucherInfo;
    private Label lblWarning;
    private Label labelReason;
    private ComboBox cmbReason;
    private Label labelReasonDetail;
    private TextBox txtReasonDetails;
    private Panel pnlFooter;
    private Label lblFooterLine;
    private Button btnConfirm;
    private Button btnCancel;
    private System.Windows.Forms.ErrorProvider errorProvider;
}