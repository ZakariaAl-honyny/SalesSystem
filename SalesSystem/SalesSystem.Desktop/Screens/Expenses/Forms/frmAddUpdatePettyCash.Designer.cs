namespace SalesSystem.Desktop.Screens.Expenses.Forms;

partial class frmAddUpdatePettyCash
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        components?.Dispose(); base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        errorProvider = new ErrorProvider(components);
        errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        errorProvider.RightToLeft = true;
        ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
        lblMode = new Label();
        label1 = new Label();
        dtpDate = new DateTimePicker();
        label2 = new Label();
        txtEmployeeName = new TextBox();
        label3 = new Label();
        numAmount = new NumericUpDown();
        label4 = new Label();
        txtReason = new TextBox();
        label5 = new Label();
        txtNotes = new TextBox();
        btnSave = new Button();
        btnClose = new Button();
        ((System.ComponentModel.ISupportInitialize)numAmount).BeginInit();
        SuspendLayout();
        // 
        // errorProvider
        // 
        errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        errorProvider.ContainerControl = this;
        errorProvider.RightToLeft = true;
        // 
        // lblMode
        // 
        lblMode.Anchor = AnchorStyles.Top;
        lblMode.AutoSize = true;
        lblMode.BackColor = Color.WhiteSmoke;
        lblMode.FlatStyle = FlatStyle.Flat;
        lblMode.Font = new Font("Tahoma", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblMode.ForeColor = Color.Firebrick;
        lblMode.Location = new Point(140, 15);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(370, 34);
        lblMode.TabIndex = 30;
        lblMode.Text = "إضافة طلب عهدة نثرية";
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label1.Location = new Point(520, 70);
        label1.Name = "label1";
        label1.Size = new Size(70, 24);
        label1.TabIndex = 0;
        label1.Text = "التاريخ";
        // 
        // dtpDate
        // 
        dtpDate.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        dtpDate.Format = DateTimePickerFormat.Short;
        dtpDate.Location = new Point(140, 70);
        dtpDate.Name = "dtpDate";
        dtpDate.RightToLeft = RightToLeft.Yes;
        dtpDate.Size = new Size(350, 32);
        dtpDate.TabIndex = 1;
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label2.Location = new Point(480, 115);
        label2.Name = "label2";
        label2.Size = new Size(126, 24);
        label2.TabIndex = 2;
        label2.Text = "اسم الموظف *";
        // 
        // txtEmployeeName
        // 
        txtEmployeeName.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        txtEmployeeName.Location = new Point(140, 115);
        txtEmployeeName.Name = "txtEmployeeName";
        txtEmployeeName.RightToLeft = RightToLeft.Yes;
        txtEmployeeName.Size = new Size(350, 32);
        txtEmployeeName.TabIndex = 2;
        // 
        // label3
        // 
        label3.AutoSize = true;
        label3.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label3.Location = new Point(520, 160);
        label3.Name = "label3";
        label3.Size = new Size(85, 24);
        label3.TabIndex = 4;
        label3.Text = "المبلغ *";
        // 
        // numAmount
        // 
        numAmount.DecimalPlaces = 2;
        numAmount.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        numAmount.Location = new Point(140, 160);
        numAmount.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 });
        numAmount.Name = "numAmount";
        numAmount.RightToLeft = RightToLeft.Yes;
        numAmount.Size = new Size(350, 32);
        numAmount.TabIndex = 3;
        numAmount.ThousandsSeparator = true;
        // 
        // label4
        // 
        label4.AutoSize = true;
        label4.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label4.Location = new Point(490, 205);
        label4.Name = "label4";
        label4.Size = new Size(115, 24);
        label4.TabIndex = 6;
        label4.Text = "سبب العهدة *";
        // 
        // txtReason
        // 
        txtReason.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        txtReason.Location = new Point(140, 205);
        txtReason.Multiline = true;
        txtReason.Name = "txtReason";
        txtReason.RightToLeft = RightToLeft.Yes;
        txtReason.Size = new Size(350, 60);
        txtReason.TabIndex = 4;
        // 
        // label5
        // 
        label5.AutoSize = true;
        label5.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label5.Location = new Point(510, 280);
        label5.Name = "label5";
        label5.Size = new Size(95, 24);
        label5.TabIndex = 8;
        label5.Text = "ملاحظات";
        // 
        // txtNotes
        // 
        txtNotes.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        txtNotes.Location = new Point(140, 280);
        txtNotes.Multiline = true;
        txtNotes.Name = "txtNotes";
        txtNotes.RightToLeft = RightToLeft.Yes;
        txtNotes.Size = new Size(350, 60);
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
        btnSave.Location = new Point(470, 370);
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
        btnClose.Location = new Point(12, 370);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(120, 45);
        btnClose.TabIndex = 7;
        btnClose.Text = "إلغاء";
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += btnClose_Click;
        // 
        // frmAddUpdatePettyCash
        // 
        AcceptButton = btnSave;
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.WhiteSmoke;
        CancelButton = btnClose;
        ClientSize = new Size(650, 440);
        Controls.Add(lblMode);
        Controls.Add(label1);
        Controls.Add(dtpDate);
        Controls.Add(label2);
        Controls.Add(txtEmployeeName);
        Controls.Add(label3);
        Controls.Add(numAmount);
        Controls.Add(label4);
        Controls.Add(txtReason);
        Controls.Add(label5);
        Controls.Add(txtNotes);
        Controls.Add(btnSave);
        Controls.Add(btnClose);
        Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "frmAddUpdatePettyCash";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "إضافة طلب عهدة نثرية";
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        ((System.ComponentModel.ISupportInitialize)numAmount).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label lblMode;
    private Label label1;
    private DateTimePicker dtpDate;
    private Label label2;
    private TextBox txtEmployeeName;
    private Label label3;
    private NumericUpDown numAmount;
    private Label label4;
    private TextBox txtReason;
    private Label label5;
    private TextBox txtNotes;
    private Button btnSave;
    private Button btnClose;
    private System.Windows.Forms.ErrorProvider errorProvider;
}
