namespace SalesSystem.Desktop.Screens.CashBoxes.Forms;

partial class frmCashTransfer
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
        lblMode = new Label();
        label1 = new Label();
        cmbSource = new ComboBox();
        label2 = new Label();
        cmbDestination = new ComboBox();
        label3 = new Label();
        numAmount = new NumericUpDown();
        label4 = new Label();
        txtNotes = new TextBox();
        btnTransfer = new Button();
        btnClose = new Button();
        ((System.ComponentModel.ISupportInitialize)numAmount).BeginInit();
        SuspendLayout();
        // 
        // lblMode
        // 
        lblMode.Anchor = AnchorStyles.Top;
        lblMode.AutoSize = true;
        lblMode.BackColor = Color.WhiteSmoke;
        lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblMode.ForeColor = Color.Firebrick;
        lblMode.Location = new Point(179, 15);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(423, 39);
        lblMode.TabIndex = 30;
        lblMode.Text = "تحويل بين الصناديق النقدية";
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label1.Location = new Point(440, 75);
        label1.Name = "label1";
        label1.Size = new Size(186, 24);
        label1.TabIndex = 0;
        label1.Text = "الصندوق المصدر *";
        // 
        // cmbSource
        // 
        cmbSource.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbSource.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        cmbSource.FormattingEnabled = true;
        cmbSource.Location = new Point(80, 72);
        cmbSource.Name = "cmbSource";
        cmbSource.RightToLeft = RightToLeft.Yes;
        cmbSource.Size = new Size(340, 32);
        cmbSource.TabIndex = 1;
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label2.Location = new Point(440, 125);
        label2.Name = "label2";
        label2.Size = new Size(176, 24);
        label2.TabIndex = 2;
        label2.Text = "الصندوق الهدف *";
        // 
        // cmbDestination
        // 
        cmbDestination.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbDestination.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        cmbDestination.FormattingEnabled = true;
        cmbDestination.Location = new Point(80, 122);
        cmbDestination.Name = "cmbDestination";
        cmbDestination.RightToLeft = RightToLeft.Yes;
        cmbDestination.Size = new Size(340, 32);
        cmbDestination.TabIndex = 2;
        // 
        // label3
        // 
        label3.AutoSize = true;
        label3.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label3.Location = new Point(440, 175);
        label3.Name = "label3";
        label3.Size = new Size(85, 24);
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
        numAmount.Size = new Size(340, 32);
        numAmount.TabIndex = 3;
        // 
        // label4
        // 
        label4.AutoSize = true;
        label4.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label4.Location = new Point(440, 225);
        label4.Name = "label4";
        label4.Size = new Size(95, 24);
        label4.TabIndex = 6;
        label4.Text = "ملاحظات";
        // 
        // txtNotes
        // 
        txtNotes.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        txtNotes.Location = new Point(80, 222);
        txtNotes.Multiline = true;
        txtNotes.Name = "txtNotes";
        txtNotes.RightToLeft = RightToLeft.Yes;
        txtNotes.Size = new Size(340, 60);
        txtNotes.TabIndex = 4;
        // 
        // btnTransfer
        // 
        btnTransfer.BackColor = Color.White;
        btnTransfer.FlatAppearance.BorderColor = Color.Black;
        btnTransfer.FlatAppearance.BorderSize = 2;
        btnTransfer.FlatStyle = FlatStyle.Popup;
        btnTransfer.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnTransfer.Location = new Point(310, 310);
        btnTransfer.Name = "btnTransfer";
        btnTransfer.Size = new Size(120, 45);
        btnTransfer.TabIndex = 5;
        btnTransfer.Text = "تحويل";
        btnTransfer.UseVisualStyleBackColor = false;
        btnTransfer.Click += btnTransfer_Click;
        // 
        // btnClose
        // 
        btnClose.BackColor = Color.White;
        btnClose.DialogResult = DialogResult.Cancel;
        btnClose.FlatStyle = FlatStyle.Popup;
        btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnClose.Location = new Point(170, 310);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(120, 45);
        btnClose.TabIndex = 6;
        btnClose.Text = "إغلاق";
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += btnClose_Click;
        // 
        // frmCashTransfer
        // 
        AcceptButton = btnTransfer;
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.WhiteSmoke;
        CancelButton = btnClose;
        ClientSize = new Size(638, 380);
        Controls.Add(lblMode);
        Controls.Add(label1);
        Controls.Add(cmbSource);
        Controls.Add(label2);
        Controls.Add(cmbDestination);
        Controls.Add(label3);
        Controls.Add(numAmount);
        Controls.Add(label4);
        Controls.Add(txtNotes);
        Controls.Add(btnTransfer);
        Controls.Add(btnClose);
        Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        Name = "frmCashTransfer";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "تحويل بين الصناديق النقدية";
        ((System.ComponentModel.ISupportInitialize)numAmount).EndInit();
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label lblMode;
    private Label label1;
    private ComboBox cmbSource;
    private Label label2;
    private ComboBox cmbDestination;
    private Label label3;
    private NumericUpDown numAmount;
    private Label label4;
    private TextBox txtNotes;
    private Button btnTransfer;
    private Button btnClose;
    private ErrorProvider errorProvider;
}
