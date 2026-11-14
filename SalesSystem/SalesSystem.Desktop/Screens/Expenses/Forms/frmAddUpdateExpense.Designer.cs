namespace SalesSystem.Desktop.Screens.Expenses.Forms;

partial class frmAddUpdateExpense
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
        pnlHeader = new Panel();
        lblMode = new Label();
        gbToolbar = new GroupBox();
        flpToolbar = new FlowLayoutPanel();
        btnSave = new Button();
        btnClose = new Button();
        gbDocumentInfo = new GroupBox();
        label1 = new Label();
        dtpDate = new DateTimePicker();
        label2 = new Label();
        cmbCashBox = new ComboBox();
        label3 = new Label();
        numAmount = new NumericUpDown();
        gbDetails = new GroupBox();
        label4 = new Label();
        txtNotes = new TextBox();
        gbTotals = new GroupBox();
        label6 = new Label();
        lblNetTotalAmount = new Label();
        pnlFooter = new Panel();
        lblHint = new Label();
        toolTip1 = new ToolTip(components);
        liveTip = new ToolTip(components);
        errorProvider = new ErrorProvider(components);
        pnlHeader.SuspendLayout();
        gbToolbar.SuspendLayout();
        flpToolbar.SuspendLayout();
        gbDocumentInfo.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numAmount).BeginInit();
        gbDetails.SuspendLayout();
        gbTotals.SuspendLayout();
        pnlFooter.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
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
        pnlHeader.Size = new Size(700, 50);
        pnlHeader.TabIndex = 0;
        // 
        // lblMode
        // 
        lblMode.BackColor = Color.FromArgb(14, 165, 103);
        lblMode.Dock = DockStyle.Fill;
        lblMode.Font = new Font("Tahoma", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblMode.ForeColor = Color.White;
        lblMode.Location = new Point(0, 0);
        lblMode.Margin = new Padding(0);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(700, 50);
        lblMode.TabIndex = 0;
        lblMode.Text = "إضافة مصروف جديد";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // gbToolbar
        // 
        gbToolbar.BackColor = Color.FromArgb(235, 245, 235);
        gbToolbar.Controls.Add(flpToolbar);
        gbToolbar.Dock = DockStyle.Top;
        gbToolbar.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        gbToolbar.ForeColor = Color.FromArgb(14, 82, 52);
        gbToolbar.Location = new Point(0, 50);
        gbToolbar.Margin = new Padding(0);
        gbToolbar.Name = "gbToolbar";
        gbToolbar.Padding = new Padding(8, 4, 8, 4);
        gbToolbar.Size = new Size(700, 84);
        gbToolbar.TabIndex = 1;
        gbToolbar.TabStop = false;
        gbToolbar.Text = "أوامر السند";
        // 
        // flpToolbar
        // 
        flpToolbar.BackColor = Color.FromArgb(235, 240, 235);
        flpToolbar.Controls.Add(btnSave);
        flpToolbar.Controls.Add(btnClose);
        flpToolbar.Dock = DockStyle.Fill;
        flpToolbar.FlowDirection = FlowDirection.RightToLeft;
        flpToolbar.Location = new Point(8, 24);
        flpToolbar.Margin = new Padding(0);
        flpToolbar.Name = "flpToolbar";
        flpToolbar.Padding = new Padding(6, 0, 6, 0);
        flpToolbar.Size = new Size(684, 44);
        flpToolbar.TabIndex = 0;
        flpToolbar.WrapContents = false;
        // 
        // btnSave
        // 
        btnSave.BackColor = Color.FromArgb(25, 135, 84);
        btnSave.FlatAppearance.BorderSize = 0;
        btnSave.FlatStyle = FlatStyle.Flat;
        btnSave.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnSave.ForeColor = Color.White;
        btnSave.Location = new Point(530, 6);
        btnSave.Margin = new Padding(4, 6, 4, 6);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(140, 40);
        btnSave.TabIndex = 0;
        btnSave.Text = "حفظ (F4)";
        btnSave.UseVisualStyleBackColor = false;
        btnSave.Click += btnSave_Click;
        // 
        // btnClose
        // 
        btnClose.BackColor = Color.FromArgb(108, 117, 125);
        btnClose.FlatAppearance.BorderSize = 0;
        btnClose.FlatStyle = FlatStyle.Flat;
        btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnClose.ForeColor = Color.White;
        btnClose.Location = new Point(382, 6);
        btnClose.Margin = new Padding(4, 6, 4, 6);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(140, 40);
        btnClose.TabIndex = 1;
        btnClose.Text = "إلغاء (Esc)";
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += btnClose_Click;
        // 
        // gbDocumentInfo
        // 
        gbDocumentInfo.BackColor = Color.FromArgb(235, 245, 235);
        gbDocumentInfo.Controls.Add(label1);
        gbDocumentInfo.Controls.Add(dtpDate);
        gbDocumentInfo.Controls.Add(label2);
        gbDocumentInfo.Controls.Add(cmbCashBox);
        gbDocumentInfo.Controls.Add(label3);
        gbDocumentInfo.Controls.Add(numAmount);
        gbDocumentInfo.Dock = DockStyle.Top;
        gbDocumentInfo.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        gbDocumentInfo.ForeColor = Color.FromArgb(14, 82, 52);
        gbDocumentInfo.Location = new Point(0, 134);
        gbDocumentInfo.Margin = new Padding(0);
        gbDocumentInfo.Name = "gbDocumentInfo";
        gbDocumentInfo.Padding = new Padding(10, 4, 10, 4);
        gbDocumentInfo.Size = new Size(700, 176);
        gbDocumentInfo.TabIndex = 2;
        gbDocumentInfo.TabStop = false;
        gbDocumentInfo.Text = "بيانات السند";
        // 
        // label1
        // 
        label1.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label1.ForeColor = Color.FromArgb(14, 82, 52);
        label1.Location = new Point(45, 32);
        label1.Margin = new Padding(0);
        label1.Name = "label1";
        label1.Size = new Size(130, 26);
        label1.TabIndex = 0;
        label1.Text = "التاريخ:";
        label1.TextAlign = ContentAlignment.MiddleRight;
        // 
        // dtpDate
        // 
        dtpDate.BackColor = Color.White;
        dtpDate.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        dtpDate.Format = DateTimePickerFormat.Short;
        dtpDate.Location = new Point(190, 30);
        dtpDate.Margin = new Padding(0);
        dtpDate.Name = "dtpDate";
        dtpDate.RightToLeft = RightToLeft.Yes;
        dtpDate.Size = new Size(300, 32);
        dtpDate.TabIndex = 0;
        // 
        // label2
        // 
        label2.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label2.ForeColor = Color.FromArgb(14, 82, 52);
        label2.Location = new Point(45, 82);
        label2.Margin = new Padding(0);
        label2.Name = "label2";
        label2.Size = new Size(130, 26);
        label2.TabIndex = 1;
        label2.Text = "الصندوق: *";
        label2.TextAlign = ContentAlignment.MiddleRight;
        // 
        // cmbCashBox
        // 
        cmbCashBox.BackColor = Color.White;
        cmbCashBox.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbCashBox.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        cmbCashBox.FormattingEnabled = true;
        cmbCashBox.Location = new Point(190, 80);
        cmbCashBox.Margin = new Padding(0);
        cmbCashBox.Name = "cmbCashBox";
        cmbCashBox.RightToLeft = RightToLeft.Yes;
        cmbCashBox.Size = new Size(300, 32);
        cmbCashBox.TabIndex = 1;
        // 
        // label3
        // 
        label3.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label3.ForeColor = Color.FromArgb(14, 82, 52);
        label3.Location = new Point(45, 132);
        label3.Margin = new Padding(0);
        label3.Name = "label3";
        label3.Size = new Size(130, 26);
        label3.TabIndex = 2;
        label3.Text = "المبلغ: *";
        label3.TextAlign = ContentAlignment.MiddleRight;
        // 
        // numAmount
        // 
        numAmount.BackColor = Color.White;
        numAmount.DecimalPlaces = 2;
        numAmount.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        numAmount.Location = new Point(190, 130);
        numAmount.Margin = new Padding(0);
        numAmount.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 });
        numAmount.Name = "numAmount";
        numAmount.RightToLeft = RightToLeft.Yes;
        numAmount.Size = new Size(300, 32);
        numAmount.TabIndex = 2;
        numAmount.TextAlign = HorizontalAlignment.Right;
        numAmount.ThousandsSeparator = true;
        numAmount.ValueChanged += numAmount_ValueChanged;
        // 
        // gbDetails
        // 
        gbDetails.BackColor = Color.FromArgb(235, 245, 235);
        gbDetails.Controls.Add(label4);
        gbDetails.Controls.Add(txtNotes);
        gbDetails.Dock = DockStyle.Top;
        gbDetails.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        gbDetails.ForeColor = Color.FromArgb(14, 82, 52);
        gbDetails.Location = new Point(0, 310);
        gbDetails.Margin = new Padding(0);
        gbDetails.Name = "gbDetails";
        gbDetails.Padding = new Padding(10, 4, 10, 4);
        gbDetails.Size = new Size(700, 116);
        gbDetails.TabIndex = 3;
        gbDetails.TabStop = false;
        gbDetails.Text = "التفاصيل";
        // 
        // label4
        // 
        label4.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label4.ForeColor = Color.FromArgb(14, 82, 52);
        label4.Location = new Point(20, 28);
        label4.Margin = new Padding(0);
        label4.Name = "label4";
        label4.Size = new Size(160, 26);
        label4.TabIndex = 0;
        label4.Text = "البيان / ملاحظات:";
        label4.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtNotes
        // 
        txtNotes.BackColor = Color.White;
        txtNotes.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        txtNotes.Location = new Point(190, 26);
        txtNotes.Margin = new Padding(0);
        txtNotes.Multiline = true;
        txtNotes.Name = "txtNotes";
        txtNotes.RightToLeft = RightToLeft.Yes;
        txtNotes.ScrollBars = ScrollBars.Vertical;
        txtNotes.Size = new Size(460, 76);
        txtNotes.TabIndex = 0;
        // 
        // gbTotals
        // 
        gbTotals.BackColor = Color.FromArgb(235, 245, 235);
        gbTotals.Controls.Add(label6);
        gbTotals.Controls.Add(lblNetTotalAmount);
        gbTotals.Dock = DockStyle.Top;
        gbTotals.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        gbTotals.ForeColor = Color.FromArgb(14, 82, 52);
        gbTotals.Location = new Point(0, 426);
        gbTotals.Margin = new Padding(0);
        gbTotals.Name = "gbTotals";
        gbTotals.Padding = new Padding(10, 4, 10, 4);
        gbTotals.Size = new Size(700, 64);
        gbTotals.TabIndex = 4;
        gbTotals.TabStop = false;
        gbTotals.Text = "الإجماليات";
        // 
        // label6
        // 
        label6.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label6.ForeColor = Color.FromArgb(14, 82, 52);
        label6.Location = new Point(60, 22);
        label6.Margin = new Padding(0);
        label6.Name = "label6";
        label6.Size = new Size(160, 28);
        label6.TabIndex = 0;
        label6.Text = "صافي المبلغ:";
        label6.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblNetTotalAmount
        // 
        lblNetTotalAmount.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblNetTotalAmount.ForeColor = Color.FromArgb(14, 82, 52);
        lblNetTotalAmount.Location = new Point(230, 20);
        lblNetTotalAmount.Margin = new Padding(0);
        lblNetTotalAmount.Name = "lblNetTotalAmount";
        lblNetTotalAmount.Size = new Size(240, 30);
        lblNetTotalAmount.TabIndex = 1;
        lblNetTotalAmount.Text = "0.00";
        lblNetTotalAmount.TextAlign = ContentAlignment.MiddleRight;
        // 
        // pnlFooter
        // 
        pnlFooter.BackColor = Color.FromArgb(235, 240, 235);
        pnlFooter.Controls.Add(lblHint);
        pnlFooter.Dock = DockStyle.Bottom;
        pnlFooter.Location = new Point(0, 490);
        pnlFooter.Margin = new Padding(0);
        pnlFooter.Name = "pnlFooter";
        pnlFooter.Size = new Size(700, 34);
        pnlFooter.TabIndex = 5;
        // 
        // lblHint
        // 
        lblHint.Dock = DockStyle.Fill;
        lblHint.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblHint.ForeColor = Color.FromArgb(14, 82, 52);
        lblHint.Location = new Point(0, 0);
        lblHint.Margin = new Padding(0);
        lblHint.Name = "lblHint";
        lblHint.Padding = new Padding(10, 0, 10, 0);
        lblHint.Size = new Size(700, 34);
        lblHint.TabIndex = 0;
        lblHint.Text = "F2 سند جديد · F4 حفظ · F5 تحديث · Esc إلغاء";
        lblHint.TextAlign = ContentAlignment.MiddleRight;
        // 
        // toolTip1
        // 
        toolTip1.AutomaticDelay = 300;
        toolTip1.BackColor = Color.White;
        toolTip1.ForeColor = Color.Black;
        // 
        // liveTip
        // 
        liveTip.AutomaticDelay = 300;
        liveTip.BackColor = Color.White;
        liveTip.ForeColor = Color.Black;
        // 
        // errorProvider
        // 
        errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        errorProvider.ContainerControl = this;
        errorProvider.RightToLeft = true;
        // 
        // frmAddUpdateExpense
        // 
        AcceptButton = btnSave;
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.FromArgb(170, 220, 170);
        CancelButton = btnClose;
        ClientSize = new Size(700, 524);
        Controls.Add(pnlFooter);
        Controls.Add(gbTotals);
        Controls.Add(gbDetails);
        Controls.Add(gbDocumentInfo);
        Controls.Add(gbToolbar);
        Controls.Add(pnlHeader);
        Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        KeyPreview = true;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "frmAddUpdateExpense";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "إضافة مصروف جديد";
        pnlHeader.ResumeLayout(false);
        gbToolbar.ResumeLayout(false);
        flpToolbar.ResumeLayout(false);
        gbDocumentInfo.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)numAmount).EndInit();
        gbDetails.ResumeLayout(false);
        gbDetails.PerformLayout();
        gbTotals.ResumeLayout(false);
        pnlFooter.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private Panel pnlHeader;
    private Label lblMode;
    private GroupBox gbToolbar;
    private FlowLayoutPanel flpToolbar;
    private Button btnSave;
    private Button btnClose;
    private GroupBox gbDocumentInfo;
    private Label label1;
    private DateTimePicker dtpDate;
    private Label label2;
    private ComboBox cmbCashBox;
    private Label label3;
    private NumericUpDown numAmount;
    private GroupBox gbDetails;
    private Label label4;
    private TextBox txtNotes;
    private GroupBox gbTotals;
    private Label label6;
    private Label lblNetTotalAmount;
    private Panel pnlFooter;
    private Label lblHint;
    private ToolTip toolTip1;
    private ToolTip liveTip;
    private System.Windows.Forms.ErrorProvider errorProvider;
}
