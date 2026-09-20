namespace SalesSystem.Desktop.Screens.CashBoxes.Forms;

partial class frmDailyClosure
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
        lblMode = new Label();
        label1 = new Label();
        dtpDate = new DateTimePicker();
        label2 = new Label();
        txtTotalIncome = new TextBox();
        label3 = new Label();
        txtTotalExpense = new TextBox();
        label4 = new Label();
        txtNetBalance = new TextBox();
        btnPrint = new Button();
        btnClose = new Button();
        lblSeparator = new Label();
        SuspendLayout();
        // 
        // lblMode
        // 
        lblMode.Anchor = AnchorStyles.Top;
        lblMode.AutoSize = true;
        lblMode.BackColor = Color.WhiteSmoke;
        lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblMode.ForeColor = Color.Firebrick;
        lblMode.Location = new Point(175, 9);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(242, 39);
        lblMode.TabIndex = 30;
        lblMode.Text = "الإغلاق اليومي";
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label1.Location = new Point(380, 80);
        label1.Name = "label1";
        label1.Size = new Size(70, 24);
        label1.TabIndex = 0;
        label1.Text = "التاريخ";
        // 
        // dtpDate
        // 
        dtpDate.CustomFormat = "dd/MM/yyyy";
        dtpDate.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        dtpDate.Format = DateTimePickerFormat.Custom;
        dtpDate.Location = new Point(80, 77);
        dtpDate.Name = "dtpDate";
        dtpDate.Size = new Size(290, 32);
        dtpDate.TabIndex = 1;
        dtpDate.ValueChanged += dtpDate_ValueChanged;
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label2.Location = new Point(380, 150);
        label2.Name = "label2";
        label2.Size = new Size(166, 24);
        label2.TabIndex = 2;
        label2.Text = "إجمالي الإيرادات";
        // 
        // txtTotalIncome
        // 
        txtTotalIncome.BackColor = SystemColors.Control;
        txtTotalIncome.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        txtTotalIncome.Location = new Point(80, 147);
        txtTotalIncome.Name = "txtTotalIncome";
        txtTotalIncome.ReadOnly = true;
        txtTotalIncome.RightToLeft = RightToLeft.Yes;
        txtTotalIncome.Size = new Size(290, 32);
        txtTotalIncome.TabIndex = 3;
        txtTotalIncome.Text = "0.00";
        txtTotalIncome.TextAlign = HorizontalAlignment.Center;
        // 
        // label3
        // 
        label3.AutoSize = true;
        label3.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label3.Location = new Point(380, 210);
        label3.Name = "label3";
        label3.Size = new Size(191, 24);
        label3.TabIndex = 4;
        label3.Text = "إجمالي المصروفات";
        // 
        // txtTotalExpense
        // 
        txtTotalExpense.BackColor = SystemColors.Control;
        txtTotalExpense.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        txtTotalExpense.Location = new Point(80, 207);
        txtTotalExpense.Name = "txtTotalExpense";
        txtTotalExpense.ReadOnly = true;
        txtTotalExpense.RightToLeft = RightToLeft.Yes;
        txtTotalExpense.Size = new Size(290, 32);
        txtTotalExpense.TabIndex = 5;
        txtTotalExpense.Text = "0.00";
        txtTotalExpense.TextAlign = HorizontalAlignment.Center;
        // 
        // label4
        // 
        label4.AutoSize = true;
        label4.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label4.Location = new Point(380, 280);
        label4.Name = "label4";
        label4.Size = new Size(138, 24);
        label4.TabIndex = 6;
        label4.Text = "صافي الرصيد";
        // 
        // txtNetBalance
        // 
        txtNetBalance.BackColor = SystemColors.Control;
        txtNetBalance.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        txtNetBalance.ForeColor = Color.ForestGreen;
        txtNetBalance.Location = new Point(80, 277);
        txtNetBalance.Name = "txtNetBalance";
        txtNetBalance.ReadOnly = true;
        txtNetBalance.RightToLeft = RightToLeft.Yes;
        txtNetBalance.Size = new Size(290, 32);
        txtNetBalance.TabIndex = 7;
        txtNetBalance.Text = "0.00";
        txtNetBalance.TextAlign = HorizontalAlignment.Center;
        // 
        // btnPrint
        // 
        btnPrint.BackColor = Color.White;
        btnPrint.FlatAppearance.BorderColor = Color.Black;
        btnPrint.FlatAppearance.BorderSize = 2;
        btnPrint.FlatStyle = FlatStyle.Popup;
        btnPrint.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnPrint.Location = new Point(270, 370);
        btnPrint.Name = "btnPrint";
        btnPrint.Size = new Size(120, 45);
        btnPrint.TabIndex = 9;
        btnPrint.Text = "طباعة";
        btnPrint.UseVisualStyleBackColor = false;
        btnPrint.Click += btnPrint_Click;
        // 
        // btnClose
        // 
        btnClose.BackColor = Color.White;
        btnClose.DialogResult = DialogResult.Cancel;
        btnClose.FlatAppearance.BorderColor = Color.Black;
        btnClose.FlatAppearance.BorderSize = 2;
        btnClose.FlatStyle = FlatStyle.Popup;
        btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnClose.Location = new Point(130, 370);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(120, 45);
        btnClose.TabIndex = 10;
        btnClose.Text = "إغلاق";
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += btnClose_Click;
        // 
        // lblSeparator
        // 
        lblSeparator.BorderStyle = BorderStyle.Fixed3D;
        lblSeparator.Location = new Point(40, 340);
        lblSeparator.Name = "lblSeparator";
        lblSeparator.Size = new Size(440, 2);
        lblSeparator.TabIndex = 8;
        // 
        // frmDailyClosure
        // 
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.WhiteSmoke;
        CancelButton = btnClose;
        ClientSize = new Size(597, 450);
        Controls.Add(lblMode);
        Controls.Add(label1);
        Controls.Add(dtpDate);
        Controls.Add(label2);
        Controls.Add(txtTotalIncome);
        Controls.Add(label3);
        Controls.Add(txtTotalExpense);
        Controls.Add(label4);
        Controls.Add(txtNetBalance);
        Controls.Add(lblSeparator);
        Controls.Add(btnPrint);
        Controls.Add(btnClose);
        Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        Name = "frmDailyClosure";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "الإغلاق اليومي";
        Load += frmDailyClosure_Load;
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label lblMode;
    private Label label1;
    private DateTimePicker dtpDate;
    private Label label2;
    private TextBox txtTotalIncome;
    private Label label3;
    private TextBox txtTotalExpense;
    private Label label4;
    private TextBox txtNetBalance;
    private Label lblSeparator;
    private Button btnPrint;
    private Button btnClose;
}
