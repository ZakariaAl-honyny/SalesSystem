namespace SalesSystem.Desktop.CustomerReceipts.Forms;

partial class frmAddUpdateCustomerReceipt
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
        lblMode = new Label();
        label1 = new Label();
        cmbCustomer = new ComboBox();
        label2 = new Label();
        cmbCashBox = new ComboBox();
        label3 = new Label();
        numAmount = new NumericUpDown();
        label4 = new Label();
        dtpDate = new DateTimePicker();
        label5 = new Label();
        txtNotes = new TextBox();
        btnSave = new Button();
        btnClose = new Button();
        ((System.ComponentModel.ISupportInitialize)numAmount).BeginInit();
        SuspendLayout();
        // 
        // lblMode
        // 
        lblMode.Anchor = AnchorStyles.Top;
        lblMode.AutoSize = true;
        lblMode.BackColor = Color.WhiteSmoke;
        lblMode.FlatStyle = FlatStyle.Flat;
        lblMode.Font = new Font("Tahoma", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblMode.ForeColor = Color.Firebrick;
        lblMode.Location = new Point(200, 15);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(200, 34);
        lblMode.TabIndex = 30;
        lblMode.Text = "سند قبض جديد";
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label1.Location = new Point(460, 75);
        label1.Name = "label1";
        label1.Size = new Size(60, 24);
        label1.TabIndex = 0;
        label1.Text = "العميل *";
        // 
        // cmbCustomer
        // 
        cmbCustomer.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbCustomer.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        cmbCustomer.FormattingEnabled = true;
        cmbCustomer.Location = new Point(80, 72);
        cmbCustomer.Name = "cmbCustomer";
        cmbCustomer.RightToLeft = RightToLeft.Yes;
        cmbCustomer.Size = new Size(360, 32);
        cmbCustomer.TabIndex = 1;
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label2.Location = new Point(460, 125);
        label2.Name = "label2";
        label2.Size = new Size(115, 24);
        label2.TabIndex = 2;
        label2.Text = "الصندوق *";
        // 
        // cmbCashBox
        // 
        cmbCashBox.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbCashBox.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        cmbCashBox.FormattingEnabled = true;
        cmbCashBox.Location = new Point(80, 122);
        cmbCashBox.Name = "cmbCashBox";
        cmbCashBox.RightToLeft = RightToLeft.Yes;
        cmbCashBox.Size = new Size(360, 32);
        cmbCashBox.TabIndex = 2;
        // 
        // label3
        // 
        label3.AutoSize = true;
        label3.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label3.Location = new Point(460, 175);
        label3.Name = "label3";
        label3.Size = new Size(70, 24);
        label3.TabIndex = 4;
        label3.Text = "المبلغ *";
        // 
        // numAmount
        // 
        numAmount.DecimalPlaces = 2;
        numAmount.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        numAmount.Location = new Point(80, 172);
        numAmount.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 });
        numAmount.Name = "numAmount";
        numAmount.RightToLeft = RightToLeft.Yes;
        numAmount.Size = new Size(360, 32);
        numAmount.TabIndex = 3;
        // 
        // label4
        // 
        label4.AutoSize = true;
        label4.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label4.Location = new Point(460, 225);
        label4.Name = "label4";
        label4.Size = new Size(55, 24);
        label4.TabIndex = 6;
        label4.Text = "التاريخ";
        // 
        // dtpDate
        // 
        dtpDate.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        dtpDate.Format = DateTimePickerFormat.Short;
        dtpDate.Location = new Point(80, 222);
        dtpDate.Name = "dtpDate";
        dtpDate.RightToLeft = RightToLeft.Yes;
        dtpDate.Size = new Size(360, 32);
        dtpDate.TabIndex = 4;
        // 
        // label5
        // 
        label5.AutoSize = true;
        label5.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label5.Location = new Point(460, 275);
        label5.Name = "label5";
        label5.Size = new Size(55, 24);
        label5.TabIndex = 8;
        label5.Text = "ملاحظات";
        // 
        // txtNotes
        // 
        txtNotes.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        txtNotes.Location = new Point(80, 272);
        txtNotes.Multiline = true;
        txtNotes.Name = "txtNotes";
        txtNotes.RightToLeft = RightToLeft.Yes;
        txtNotes.Size = new Size(360, 50);
        txtNotes.TabIndex = 5;
        // 
        // btnSave
        // 
        btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnSave.BackColor = Color.White;
        btnSave.FlatAppearance.BorderColor = Color.Black;
        btnSave.FlatAppearance.BorderSize = 2;
        btnSave.FlatStyle = FlatStyle.Popup;
        btnSave.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnSave.Location = new Point(430, 350);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(120, 45);
        btnSave.TabIndex = 6;
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
        btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnClose.Location = new Point(12, 350);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(120, 45);
        btnClose.TabIndex = 7;
        btnClose.Text = "إلغاء";
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += btnClose_Click;
        // 
        // errorProvider
        // 
        this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
        this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
        // 
        // frmCustomerReceiptEditor
        // 
        AcceptButton = btnSave;
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.WhiteSmoke;
        CancelButton = btnClose;
        ClientSize = new Size(600, 430);
        Controls.Add(lblMode);
        Controls.Add(label1);
        Controls.Add(cmbCustomer);
        Controls.Add(label2);
        Controls.Add(cmbCashBox);
        Controls.Add(label3);
        Controls.Add(numAmount);
        Controls.Add(label4);
        Controls.Add(dtpDate);
        Controls.Add(label5);
        Controls.Add(txtNotes);
        Controls.Add(btnSave);
        Controls.Add(btnClose);
        Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "frmCustomerReceiptEditor";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "سند قبض جديد";
        ((System.ComponentModel.ISupportInitialize)numAmount).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label lblMode;
    private Label label1;
    private ComboBox cmbCustomer;
    private Label label2;
    private ComboBox cmbCashBox;
    private Label label3;
    private NumericUpDown numAmount;
    private Label label4;
    private DateTimePicker dtpDate;
    private Label label5;
    private TextBox txtNotes;
    private Button btnSave;
    private Button btnClose;
    private System.Windows.Forms.ErrorProvider errorProvider;
}
