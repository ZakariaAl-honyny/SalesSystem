namespace SalesSystem.Desktop.Screens.CashVouchers.Forms;

partial class frmCashVoucherTransfer
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
        lblMockBadge = new Label();
        lblMode = new Label();
        lblAccent = new Label();
        labelDate = new Label();
        dtpDate = new DateTimePicker();
        labelFrom = new Label();
        cmbFrom = new ComboBox();
        labelTo = new Label();
        cmbTo = new ComboBox();
        labelAmount = new Label();
        numAmount = new NumericUpDown();
        pnlNote = new Panel();
        lblNote = new Label();
        labelNotes = new Label();
        txtNotes = new TextBox();
        pnlFooter = new Panel();
        lblFooterLine = new Label();
        btnSave = new Button();
        btnCancel = new Button();
        pnlHeader.SuspendLayout();
        pnlNote.SuspendLayout();
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
        pnlHeader.Size = new Size(520, 60);
        pnlHeader.TabIndex = 0;
        // 
        // lblMockBadge
        // 
        lblMockBadge.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        lblMockBadge.AutoSize = true;
        lblMockBadge.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblMockBadge.ForeColor = Color.FromArgb(241, 196, 15);
        lblMockBadge.Location = new Point(12, 22);
        lblMockBadge.Name = "lblMockBadge";
        lblMockBadge.Size = new Size(248, 22);
        lblMockBadge.TabIndex = 2;
        lblMockBadge.Text = "وضع التجربة (Mock) — بيانات وهمية";
        // 
        // lblMode
        // 
        lblMode.Anchor = AnchorStyles.Top;
        lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblMode.ForeColor = Color.White;
        lblMode.Location = new Point(110, 10);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(300, 38);
        lblMode.TabIndex = 1;
        lblMode.Text = "تحويل بين الخزائن";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblAccent
        // 
        lblAccent.BackColor = Color.FromArgb(41, 128, 185);
        lblAccent.Dock = DockStyle.Bottom;
        lblAccent.Height = 4;
        lblAccent.Name = "lblAccent";
        lblAccent.TabIndex = 3;
        // 
        // labelDate
        // 
        labelDate.AutoSize = true;
        labelDate.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        labelDate.ForeColor = Color.FromArgb(52, 73, 94);
        labelDate.Location = new Point(20, 78);
        labelDate.Name = "labelDate";
        labelDate.Size = new Size(70, 24);
        labelDate.TabIndex = 3;
        labelDate.Text = "التاريخ";
        // 
        // dtpDate
        // 
        dtpDate.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        dtpDate.Format = DateTimePickerFormat.Short;
        dtpDate.Location = new Point(155, 76);
        dtpDate.Name = "dtpDate";
        dtpDate.RightToLeft = RightToLeft.Yes;
        dtpDate.Size = new Size(365, 30);
        dtpDate.TabIndex = 4;
        // 
        // labelFrom
        // 
        labelFrom.AutoSize = true;
        labelFrom.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        labelFrom.ForeColor = Color.FromArgb(52, 73, 94);
        labelFrom.Location = new Point(20, 124);
        labelFrom.Name = "labelFrom";
        labelFrom.Size = new Size(77, 24);
        labelFrom.TabIndex = 5;
        labelFrom.Text = "من حساب";
        // 
        // cmbFrom
        // 
        cmbFrom.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbFrom.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        cmbFrom.FormattingEnabled = true;
        cmbFrom.Location = new Point(155, 122);
        cmbFrom.Name = "cmbFrom";
        cmbFrom.RightToLeft = RightToLeft.Yes;
        cmbFrom.Size = new Size(365, 30);
        cmbFrom.TabIndex = 6;
        cmbFrom.SelectedIndexChanged += cmbFrom_SelectedIndexChanged;
        // 
        // labelTo
        // 
        labelTo.AutoSize = true;
        labelTo.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        labelTo.ForeColor = Color.FromArgb(52, 73, 94);
        labelTo.Location = new Point(20, 170);
        labelTo.Name = "labelTo";
        labelTo.Size = new Size(77, 24);
        labelTo.TabIndex = 7;
        labelTo.Text = "إلى حساب";
        // 
        // cmbTo
        // 
        cmbTo.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbTo.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        cmbTo.FormattingEnabled = true;
        cmbTo.Location = new Point(155, 168);
        cmbTo.Name = "cmbTo";
        cmbTo.RightToLeft = RightToLeft.Yes;
        cmbTo.Size = new Size(365, 30);
        cmbTo.TabIndex = 8;
        cmbTo.SelectedIndexChanged += cmbTo_SelectedIndexChanged;
        // 
        // labelAmount
        // 
        labelAmount.AutoSize = true;
        labelAmount.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        labelAmount.ForeColor = Color.FromArgb(52, 73, 94);
        labelAmount.Location = new Point(20, 216);
        labelAmount.Name = "labelAmount";
        labelAmount.Size = new Size(62, 24);
        labelAmount.TabIndex = 9;
        labelAmount.Text = "المبلغ";
        // 
        // numAmount
        // 
        numAmount.DecimalPlaces = 2;
        numAmount.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        numAmount.Location = new Point(155, 214);
        numAmount.Maximum = new decimal(new int[] { 999999999, 0, 0, 0 });
        numAmount.Name = "numAmount";
        numAmount.RightToLeft = RightToLeft.Yes;
        numAmount.Size = new Size(365, 30);
        numAmount.TabIndex = 10;
        numAmount.ThousandsSeparator = true;
        // 
        // pnlNote
        // 
        pnlNote.BackColor = Color.FromArgb(235, 245, 252);
        pnlNote.BorderStyle = BorderStyle.FixedSingle;
        pnlNote.Controls.Add(lblNote);
        pnlNote.Location = new Point(155, 260);
        pnlNote.Name = "pnlNote";
        pnlNote.Size = new Size(365, 48);
        pnlNote.TabIndex = 11;
        // 
        // lblNote
        // 
        lblNote.Dock = DockStyle.Fill;
        lblNote.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblNote.ForeColor = Color.FromArgb(41, 128, 185);
        lblNote.Location = new Point(0, 0);
        lblNote.Name = "lblNote";
        lblNote.RightToLeft = RightToLeft.Yes;
        lblNote.Size = new Size(363, 46);
        lblNote.TabIndex = 0;
        lblNote.Text = "قيد مزدوج متوازن: مدين إلى حساب + دائن من حساب";
        lblNote.TextAlign = ContentAlignment.MiddleRight;
        // 
        // labelNotes
        // 
        labelNotes.AutoSize = true;
        labelNotes.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        labelNotes.ForeColor = Color.FromArgb(52, 73, 94);
        labelNotes.Location = new Point(20, 324);
        labelNotes.Name = "labelNotes";
        labelNotes.Size = new Size(59, 24);
        labelNotes.TabIndex = 12;
        labelNotes.Text = "البيان";
        // 
        // txtNotes
        // 
        txtNotes.AcceptsReturn = true;
        txtNotes.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        txtNotes.Location = new Point(155, 322);
        txtNotes.Multiline = true;
        txtNotes.Name = "txtNotes";
        txtNotes.RightToLeft = RightToLeft.Yes;
        txtNotes.ScrollBars = ScrollBars.Vertical;
        txtNotes.Size = new Size(365, 76);
        txtNotes.TabIndex = 13;
        // 
        // pnlFooter
        // 
        pnlFooter.BackColor = Color.FromArgb(248, 249, 250);
        pnlFooter.Controls.Add(btnCancel);
        pnlFooter.Controls.Add(btnSave);
        pnlFooter.Controls.Add(lblFooterLine);
        pnlFooter.Dock = DockStyle.Bottom;
        pnlFooter.Name = "pnlFooter";
        pnlFooter.Size = new Size(520, 60);
        pnlFooter.TabIndex = 14;
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
        btnSave.Location = new Point(272, 10);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(236, 40);
        btnSave.TabIndex = 15;
        btnSave.Text = "حفظ وترحيل [F10]";
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
        btnCancel.Location = new Point(12, 10);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(110, 40);
        btnCancel.TabIndex = 16;
        btnCancel.Text = "إلغاء";
        btnCancel.UseVisualStyleBackColor = false;
        btnCancel.Click += btnCancel_Click;
        // 
        // frmCashVoucherTransfer
        // 
        AcceptButton = btnSave;
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.WhiteSmoke;
        CancelButton = btnCancel;
        ClientSize = new Size(520, 520);
        Controls.Add(labelDate);
        Controls.Add(dtpDate);
        Controls.Add(labelFrom);
        Controls.Add(cmbFrom);
        Controls.Add(labelTo);
        Controls.Add(cmbTo);
        Controls.Add(labelAmount);
        Controls.Add(numAmount);
        Controls.Add(pnlNote);
        Controls.Add(labelNotes);
        Controls.Add(txtNotes);
        Controls.Add(pnlFooter);
        Controls.Add(pnlHeader);
        Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "frmCashVoucherTransfer";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "تحويل بين الخزائن";
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        pnlHeader.ResumeLayout(false);
        pnlHeader.PerformLayout();
        pnlNote.ResumeLayout(false);
        pnlFooter.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)numAmount).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Panel pnlHeader;
    private Label lblMockBadge;
    private Label lblMode;
    private Label lblAccent;
    private Label labelDate;
    private DateTimePicker dtpDate;
    private Label labelFrom;
    private ComboBox cmbFrom;
    private Label labelTo;
    private ComboBox cmbTo;
    private Label labelAmount;
    private NumericUpDown numAmount;
    private Panel pnlNote;
    private Label lblNote;
    private Label labelNotes;
    private TextBox txtNotes;
    private Panel pnlFooter;
    private Label lblFooterLine;
    private Button btnSave;
    private Button btnCancel;
    private System.Windows.Forms.ErrorProvider errorProvider;
}