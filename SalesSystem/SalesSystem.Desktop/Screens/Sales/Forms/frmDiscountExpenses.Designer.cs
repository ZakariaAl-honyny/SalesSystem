namespace SalesSystem.Desktop.Screens.Sales.Forms
{
    partial class frmDiscountExpenses
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            pnlHeader = new Panel();
            tblHeader = new TableLayoutPanel();
            lblTitle = new Label();
            lblEscHint = new Label();
            pnlBody = new Panel();
            pnlSummary = new Panel();
            tblSummary = new TableLayoutPanel();
            lblSummarySubtotalCaption = new Label();
            lblSummarySubtotalVal = new Label();
            lblSummaryDiscountCaption = new Label();
            lblSummaryDiscountVal = new Label();
            lblSummaryTaxCaption = new Label();
            lblSummaryTaxVal = new Label();
            lblSummaryExpenseCaption = new Label();
            lblSummaryExpenseVal = new Label();
            lblSummaryNetCaption = new Label();
            lblSummaryNetVal = new Label();
            lblSummaryRemainingCaption = new Label();
            lblSummaryRemainingVal = new Label();
            grpPayment = new GroupBox();
            tblPayment = new TableLayoutPanel();
            lblPaymentMethod = new Label();
            cmbPaymentMethod = new ComboBox();
            lblPaidSingle = new Label();
            numPaidAmount = new NumericUpDown();
            lblPaidCash = new Label();
            numPaidCash = new NumericUpDown();
            lblPaidBank = new Label();
            numPaidBank = new NumericUpDown();
            lblPaidWallet = new Label();
            numPaidWallet = new NumericUpDown();
            grpExpense = new GroupBox();
            tblExpense = new TableLayoutPanel();
            lblExpenseAccount = new Label();
            cmbExpenseAccount = new ComboBox();
            lblExpenseAmount = new Label();
            numExpenseAmount = new NumericUpDown();
            grpTax = new GroupBox();
            tblTax = new TableLayoutPanel();
            lblTaxRate = new Label();
            cmbTaxRate = new ComboBox();
            grpDiscount = new GroupBox();
            tblDiscount = new TableLayoutPanel();
            flpDiscountType = new FlowLayoutPanel();
            rbDiscountAmount = new RadioButton();
            rbDiscountPercent = new RadioButton();
            lblDiscountValue = new Label();
            numDiscountValue = new NumericUpDown();
            pnlFooter = new Panel();
            flpFooter = new FlowLayoutPanel();
            btnOk = new Button();
            btnCancel = new Button();
            pnlHeader.SuspendLayout();
            tblHeader.SuspendLayout();
            pnlBody.SuspendLayout();
            pnlSummary.SuspendLayout();
            tblSummary.SuspendLayout();
            grpPayment.SuspendLayout();
            tblPayment.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numPaidAmount).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numPaidCash).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numPaidBank).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numPaidWallet).BeginInit();
            grpExpense.SuspendLayout();
            tblExpense.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numExpenseAmount).BeginInit();
            grpTax.SuspendLayout();
            tblTax.SuspendLayout();
            grpDiscount.SuspendLayout();
            tblDiscount.SuspendLayout();
            flpDiscountType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numDiscountValue).BeginInit();
            pnlFooter.SuspendLayout();
            flpFooter.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(33, 43, 54);
            pnlHeader.Controls.Add(tblHeader);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 50;
            // 
            // tblHeader
            // 
            tblHeader.ColumnCount = 2;
            tblHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
            tblHeader.Controls.Add(lblTitle, 0, 0);
            tblHeader.Controls.Add(lblEscHint, 1, 0);
            tblHeader.Dock = DockStyle.Fill;
            tblHeader.RowCount = 1;
            tblHeader.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            // 
            // lblTitle
            // 
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.Font = new Font("Tahoma", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.ForeColor = Color.White;
            lblTitle.Text = "الخصم والمصروفات والدفع [F7]";
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblEscHint
            // 
            lblEscHint.Dock = DockStyle.Fill;
            lblEscHint.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblEscHint.ForeColor = Color.FromArgb(150, 170, 190);
            lblEscHint.Text = "Esc = إغلاق";
            lblEscHint.TextAlign = ContentAlignment.MiddleRight;
            // 
            // pnlBody
            // 
            pnlBody.BackColor = Color.WhiteSmoke;
            pnlBody.Controls.Add(pnlSummary);
            pnlBody.Controls.Add(grpPayment);
            pnlBody.Controls.Add(grpExpense);
            pnlBody.Controls.Add(grpTax);
            pnlBody.Controls.Add(grpDiscount);
            pnlBody.Dock = DockStyle.Fill;
            pnlBody.Padding = new Padding(10, 8, 10, 8);
            // 
            // pnlSummary
            // 
            pnlSummary.BackColor = Color.FromArgb(232, 244, 253);
            pnlSummary.Controls.Add(tblSummary);
            pnlSummary.Dock = DockStyle.Fill;
            pnlSummary.Padding = new Padding(10, 6, 10, 6);
            // 
            // tblSummary
            // 
            tblSummary.ColumnCount = 2;
            tblSummary.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 170F));
            tblSummary.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblSummary.Controls.Add(lblSummarySubtotalCaption, 0, 0);
            tblSummary.Controls.Add(lblSummarySubtotalVal, 1, 0);
            tblSummary.Controls.Add(lblSummaryDiscountCaption, 0, 1);
            tblSummary.Controls.Add(lblSummaryDiscountVal, 1, 1);
            tblSummary.Controls.Add(lblSummaryTaxCaption, 0, 2);
            tblSummary.Controls.Add(lblSummaryTaxVal, 1, 2);
            tblSummary.Controls.Add(lblSummaryExpenseCaption, 0, 3);
            tblSummary.Controls.Add(lblSummaryExpenseVal, 1, 3);
            tblSummary.Controls.Add(lblSummaryNetCaption, 0, 4);
            tblSummary.Controls.Add(lblSummaryNetVal, 1, 4);
            tblSummary.Controls.Add(lblSummaryRemainingCaption, 0, 5);
            tblSummary.Controls.Add(lblSummaryRemainingVal, 1, 5);
            tblSummary.Dock = DockStyle.Fill;
            tblSummary.RowCount = 6;
            tblSummary.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tblSummary.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tblSummary.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tblSummary.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tblSummary.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tblSummary.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            // 
            // lblSummarySubtotalCaption
            // 
            lblSummarySubtotalCaption.Dock = DockStyle.Fill;
            lblSummarySubtotalCaption.Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblSummarySubtotalCaption.ForeColor = Color.FromArgb(80, 90, 100);
            lblSummarySubtotalCaption.Text = "المجموع الفرعي:";
            lblSummarySubtotalCaption.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblSummarySubtotalVal
            // 
            lblSummarySubtotalVal.Dock = DockStyle.Fill;
            lblSummarySubtotalVal.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSummarySubtotalVal.ForeColor = Color.FromArgb(33, 43, 54);
            lblSummarySubtotalVal.Text = "50,000.00 YER";
            lblSummarySubtotalVal.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblSummaryDiscountCaption
            // 
            lblSummaryDiscountCaption.Dock = DockStyle.Fill;
            lblSummaryDiscountCaption.Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblSummaryDiscountCaption.ForeColor = Color.FromArgb(80, 90, 100);
            lblSummaryDiscountCaption.Text = "إجمالي الخصم:";
            lblSummaryDiscountCaption.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblSummaryDiscountVal
            // 
            lblSummaryDiscountVal.Dock = DockStyle.Fill;
            lblSummaryDiscountVal.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSummaryDiscountVal.ForeColor = Color.FromArgb(33, 43, 54);
            lblSummaryDiscountVal.Text = "-0.00 YER";
            lblSummaryDiscountVal.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblSummaryTaxCaption
            // 
            lblSummaryTaxCaption.Dock = DockStyle.Fill;
            lblSummaryTaxCaption.Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblSummaryTaxCaption.ForeColor = Color.FromArgb(80, 90, 100);
            lblSummaryTaxCaption.Text = "الضريبة:";
            lblSummaryTaxCaption.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblSummaryTaxVal
            // 
            lblSummaryTaxVal.Dock = DockStyle.Fill;
            lblSummaryTaxVal.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSummaryTaxVal.ForeColor = Color.FromArgb(33, 43, 54);
            lblSummaryTaxVal.Text = "+0.00 YER";
            lblSummaryTaxVal.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblSummaryExpenseCaption
            // 
            lblSummaryExpenseCaption.Dock = DockStyle.Fill;
            lblSummaryExpenseCaption.Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblSummaryExpenseCaption.ForeColor = Color.FromArgb(80, 90, 100);
            lblSummaryExpenseCaption.Text = "المصروفات:";
            lblSummaryExpenseCaption.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblSummaryExpenseVal
            // 
            lblSummaryExpenseVal.Dock = DockStyle.Fill;
            lblSummaryExpenseVal.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSummaryExpenseVal.ForeColor = Color.FromArgb(33, 43, 54);
            lblSummaryExpenseVal.Text = "+0.00 YER";
            lblSummaryExpenseVal.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblSummaryNetCaption
            // 
            lblSummaryNetCaption.Dock = DockStyle.Fill;
            lblSummaryNetCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSummaryNetCaption.ForeColor = Color.FromArgb(21, 87, 36);
            lblSummaryNetCaption.Text = "الصافي النهائي:";
            lblSummaryNetCaption.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblSummaryNetVal
            // 
            lblSummaryNetVal.Dock = DockStyle.Fill;
            lblSummaryNetVal.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSummaryNetVal.ForeColor = Color.FromArgb(21, 87, 36);
            lblSummaryNetVal.Text = "50,000.00 YER";
            lblSummaryNetVal.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblSummaryRemainingCaption
            // 
            lblSummaryRemainingCaption.Dock = DockStyle.Fill;
            lblSummaryRemainingCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSummaryRemainingCaption.ForeColor = Color.FromArgb(33, 43, 54);
            lblSummaryRemainingCaption.Text = "الباقي:";
            lblSummaryRemainingCaption.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblSummaryRemainingVal
            // 
            lblSummaryRemainingVal.Dock = DockStyle.Fill;
            lblSummaryRemainingVal.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSummaryRemainingVal.ForeColor = Color.FromArgb(21, 87, 36);
            lblSummaryRemainingVal.Text = "50,000.00 YER";
            lblSummaryRemainingVal.TextAlign = ContentAlignment.MiddleRight;
            // 
            // grpPayment
            // 
            grpPayment.BackColor = Color.White;
            grpPayment.Controls.Add(tblPayment);
            grpPayment.Dock = DockStyle.Top;
            grpPayment.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            grpPayment.ForeColor = Color.FromArgb(33, 43, 54);
            grpPayment.Height = 152;
            grpPayment.Padding = new Padding(10, 4, 10, 6);
            grpPayment.Text = "الدفع";
            // 
            // tblPayment
            // 
            tblPayment.ColumnCount = 2;
            tblPayment.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tblPayment.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblPayment.Controls.Add(lblPaymentMethod, 0, 0);
            tblPayment.Controls.Add(cmbPaymentMethod, 1, 0);
            tblPayment.Controls.Add(lblPaidSingle, 0, 1);
            tblPayment.Controls.Add(numPaidAmount, 1, 1);
            tblPayment.Controls.Add(lblPaidCash, 0, 2);
            tblPayment.Controls.Add(numPaidCash, 1, 2);
            tblPayment.Controls.Add(lblPaidBank, 0, 3);
            tblPayment.Controls.Add(numPaidBank, 1, 3);
            tblPayment.Controls.Add(lblPaidWallet, 0, 4);
            tblPayment.Controls.Add(numPaidWallet, 1, 4);
            tblPayment.Dock = DockStyle.Fill;
            tblPayment.RowCount = 6;
            tblPayment.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tblPayment.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tblPayment.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            tblPayment.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            tblPayment.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            tblPayment.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            // 
            // lblPaymentMethod
            // 
            lblPaymentMethod.Dock = DockStyle.Fill;
            lblPaymentMethod.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblPaymentMethod.ForeColor = Color.FromArgb(80, 90, 100);
            lblPaymentMethod.Text = "طريقة الدفع:";
            lblPaymentMethod.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cmbPaymentMethod
            // 
            cmbPaymentMethod.Dock = DockStyle.Fill;
            cmbPaymentMethod.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPaymentMethod.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmbPaymentMethod.Items.AddRange(new object[] { "نقدي", "آجل", "دفع مجزأ" });
            cmbPaymentMethod.SelectedIndexChanged += cmbPaymentMethod_SelectedIndexChanged;
            // 
            // lblPaidSingle
            // 
            lblPaidSingle.Dock = DockStyle.Fill;
            lblPaidSingle.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblPaidSingle.ForeColor = Color.FromArgb(80, 90, 100);
            lblPaidSingle.Text = "المبلغ المدفوع:";
            lblPaidSingle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // numPaidAmount
            // 
            numPaidAmount.DecimalPlaces = 2;
            numPaidAmount.Dock = DockStyle.Fill;
            numPaidAmount.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            numPaidAmount.Maximum = 9999999;
            numPaidAmount.ThousandsSeparator = true;
            numPaidAmount.ValueChanged += numPaidAmount_ValueChanged;
            // 
            // lblPaidCash
            // 
            lblPaidCash.Dock = DockStyle.Fill;
            lblPaidCash.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblPaidCash.ForeColor = Color.FromArgb(80, 90, 100);
            lblPaidCash.Text = "المدفوع نقد:";
            lblPaidCash.TextAlign = ContentAlignment.MiddleLeft;
            lblPaidCash.Visible = false;
            // 
            // numPaidCash
            // 
            numPaidCash.DecimalPlaces = 2;
            numPaidCash.Dock = DockStyle.Fill;
            numPaidCash.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            numPaidCash.Maximum = 9999999;
            numPaidCash.ThousandsSeparator = true;
            numPaidCash.Visible = false;
            numPaidCash.ValueChanged += numPaidCash_ValueChanged;
            // 
            // lblPaidBank
            // 
            lblPaidBank.Dock = DockStyle.Fill;
            lblPaidBank.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblPaidBank.ForeColor = Color.FromArgb(80, 90, 100);
            lblPaidBank.Text = "المدفوع بنك:";
            lblPaidBank.TextAlign = ContentAlignment.MiddleLeft;
            lblPaidBank.Visible = false;
            // 
            // numPaidBank
            // 
            numPaidBank.DecimalPlaces = 2;
            numPaidBank.Dock = DockStyle.Fill;
            numPaidBank.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            numPaidBank.Maximum = 9999999;
            numPaidBank.ThousandsSeparator = true;
            numPaidBank.Visible = false;
            numPaidBank.ValueChanged += numPaidBank_ValueChanged;
            // 
            // lblPaidWallet
            // 
            lblPaidWallet.Dock = DockStyle.Fill;
            lblPaidWallet.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblPaidWallet.ForeColor = Color.FromArgb(80, 90, 100);
            lblPaidWallet.Text = "المدفوع محفظة:";
            lblPaidWallet.TextAlign = ContentAlignment.MiddleLeft;
            lblPaidWallet.Visible = false;
            // 
            // numPaidWallet
            // 
            numPaidWallet.DecimalPlaces = 2;
            numPaidWallet.Dock = DockStyle.Fill;
            numPaidWallet.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            numPaidWallet.Maximum = 9999999;
            numPaidWallet.ThousandsSeparator = true;
            numPaidWallet.Visible = false;
            numPaidWallet.ValueChanged += numPaidWallet_ValueChanged;
            // 
            // grpExpense
            // 
            grpExpense.BackColor = Color.White;
            grpExpense.Controls.Add(tblExpense);
            grpExpense.Dock = DockStyle.Top;
            grpExpense.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            grpExpense.ForeColor = Color.FromArgb(33, 43, 54);
            grpExpense.Height = 84;
            grpExpense.Padding = new Padding(10, 4, 10, 6);
            grpExpense.Text = "المصروفات";
            // 
            // tblExpense
            // 
            tblExpense.ColumnCount = 2;
            tblExpense.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tblExpense.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblExpense.Controls.Add(lblExpenseAccount, 0, 0);
            tblExpense.Controls.Add(cmbExpenseAccount, 1, 0);
            tblExpense.Controls.Add(lblExpenseAmount, 0, 1);
            tblExpense.Controls.Add(numExpenseAmount, 1, 1);
            tblExpense.Dock = DockStyle.Fill;
            tblExpense.RowCount = 2;
            tblExpense.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tblExpense.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            // 
            // lblExpenseAccount
            // 
            lblExpenseAccount.Dock = DockStyle.Fill;
            lblExpenseAccount.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblExpenseAccount.ForeColor = Color.FromArgb(80, 90, 100);
            lblExpenseAccount.Text = "حساب المصروف:";
            lblExpenseAccount.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cmbExpenseAccount
            // 
            cmbExpenseAccount.Dock = DockStyle.Fill;
            cmbExpenseAccount.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbExpenseAccount.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            // 
            // lblExpenseAmount
            // 
            lblExpenseAmount.Dock = DockStyle.Fill;
            lblExpenseAmount.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblExpenseAmount.ForeColor = Color.FromArgb(80, 90, 100);
            lblExpenseAmount.Text = "مبلغ المصروف:";
            lblExpenseAmount.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // numExpenseAmount
            // 
            numExpenseAmount.DecimalPlaces = 2;
            numExpenseAmount.Dock = DockStyle.Fill;
            numExpenseAmount.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            numExpenseAmount.Maximum = 9999999;
            numExpenseAmount.ThousandsSeparator = true;
            numExpenseAmount.ValueChanged += numExpenseAmount_ValueChanged;
            // 
            // grpTax
            // 
            grpTax.BackColor = Color.White;
            grpTax.Controls.Add(tblTax);
            grpTax.Dock = DockStyle.Top;
            grpTax.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            grpTax.ForeColor = Color.FromArgb(33, 43, 54);
            grpTax.Height = 56;
            grpTax.Padding = new Padding(10, 4, 10, 6);
            grpTax.Text = "الضريبة";
            // 
            // tblTax
            // 
            tblTax.ColumnCount = 2;
            tblTax.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tblTax.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblTax.Controls.Add(lblTaxRate, 0, 0);
            tblTax.Controls.Add(cmbTaxRate, 1, 0);
            tblTax.Dock = DockStyle.Fill;
            tblTax.RowCount = 1;
            tblTax.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            // 
            // lblTaxRate
            // 
            lblTaxRate.Dock = DockStyle.Fill;
            lblTaxRate.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTaxRate.ForeColor = Color.FromArgb(80, 90, 100);
            lblTaxRate.Text = "الضريبة %:";
            lblTaxRate.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cmbTaxRate
            // 
            cmbTaxRate.Dock = DockStyle.Fill;
            cmbTaxRate.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTaxRate.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmbTaxRate.Items.AddRange(new object[] { "0%", "5%", "15%" });
            cmbTaxRate.SelectedIndexChanged += cmbTaxRate_SelectedIndexChanged;
            // 
            // grpDiscount
            // 
            grpDiscount.BackColor = Color.White;
            grpDiscount.Controls.Add(tblDiscount);
            grpDiscount.Dock = DockStyle.Top;
            grpDiscount.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            grpDiscount.ForeColor = Color.FromArgb(33, 43, 54);
            grpDiscount.Height = 84;
            grpDiscount.Padding = new Padding(10, 4, 10, 6);
            grpDiscount.Text = "الخصم";
            // 
            // tblDiscount
            // 
            tblDiscount.ColumnCount = 2;
            tblDiscount.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tblDiscount.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblDiscount.Controls.Add(flpDiscountType, 0, 0);
            tblDiscount.SetColumnSpan(flpDiscountType, 2);
            tblDiscount.Controls.Add(lblDiscountValue, 0, 1);
            tblDiscount.Controls.Add(numDiscountValue, 1, 1);
            tblDiscount.Dock = DockStyle.Fill;
            tblDiscount.RowCount = 2;
            tblDiscount.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            tblDiscount.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            // 
            // flpDiscountType
            // 
            flpDiscountType.Controls.Add(rbDiscountAmount);
            flpDiscountType.Controls.Add(rbDiscountPercent);
            flpDiscountType.Dock = DockStyle.Fill;
            flpDiscountType.FlowDirection = FlowDirection.RightToLeft;
            // 
            // rbDiscountAmount
            // 
            rbDiscountAmount.AutoSize = true;
            rbDiscountAmount.Checked = true;
            rbDiscountAmount.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            rbDiscountAmount.ForeColor = Color.FromArgb(33, 43, 54);
            rbDiscountAmount.Margin = new Padding(0, 0, 18, 0);
            rbDiscountAmount.Text = "مبلغ";
            rbDiscountAmount.CheckedChanged += rbDiscountType_CheckedChanged;
            // 
            // rbDiscountPercent
            // 
            rbDiscountPercent.AutoSize = true;
            rbDiscountPercent.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            rbDiscountPercent.ForeColor = Color.FromArgb(33, 43, 54);
            rbDiscountPercent.Text = "نسبة %";
            rbDiscountPercent.CheckedChanged += rbDiscountType_CheckedChanged;
            // 
            // lblDiscountValue
            // 
            lblDiscountValue.Dock = DockStyle.Fill;
            lblDiscountValue.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblDiscountValue.ForeColor = Color.FromArgb(80, 90, 100);
            lblDiscountValue.Text = "قيمة الخصم:";
            lblDiscountValue.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // numDiscountValue
            // 
            numDiscountValue.DecimalPlaces = 2;
            numDiscountValue.Dock = DockStyle.Fill;
            numDiscountValue.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            numDiscountValue.Maximum = 9999999;
            numDiscountValue.ThousandsSeparator = true;
            numDiscountValue.ValueChanged += numDiscountValue_ValueChanged;
            // 
            // pnlFooter
            // 
            pnlFooter.BackColor = Color.FromArgb(248, 249, 250);
            pnlFooter.Controls.Add(flpFooter);
            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Height = 56;
            // 
            // flpFooter
            // 
            flpFooter.Controls.Add(btnOk);
            flpFooter.Controls.Add(btnCancel);
            flpFooter.Dock = DockStyle.Fill;
            flpFooter.FlowDirection = FlowDirection.RightToLeft;
            flpFooter.Padding = new Padding(6, 8, 6, 6);
            // 
            // btnOk
            // 
            btnOk.BackColor = Color.FromArgb(25, 135, 84);
            btnOk.FlatStyle = FlatStyle.Popup;
            btnOk.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnOk.ForeColor = Color.White;
            btnOk.Height = 40;
            btnOk.Margin = new Padding(0, 0, 8, 0);
            btnOk.Text = "موافق";
            btnOk.Width = 150;
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
            btnCancel.Text = "إلغاء";
            btnCancel.Width = 90;
            // 
            pnlHeader.ResumeLayout(false);
            tblHeader.ResumeLayout(false);
            pnlBody.ResumeLayout(false);
            pnlSummary.ResumeLayout(false);
            tblSummary.ResumeLayout(false);
            grpPayment.ResumeLayout(false);
            tblPayment.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)numPaidAmount).EndInit();
            ((System.ComponentModel.ISupportInitialize)numPaidCash).EndInit();
            ((System.ComponentModel.ISupportInitialize)numPaidBank).EndInit();
            ((System.ComponentModel.ISupportInitialize)numPaidWallet).EndInit();
            grpExpense.ResumeLayout(false);
            tblExpense.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)numExpenseAmount).EndInit();
            grpTax.ResumeLayout(false);
            tblTax.ResumeLayout(false);
            grpDiscount.ResumeLayout(false);
            tblDiscount.ResumeLayout(false);
            flpDiscountType.ResumeLayout(false);
            flpDiscountType.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numDiscountValue).EndInit();
            pnlFooter.ResumeLayout(false);
            flpFooter.ResumeLayout(false);
            // 
            // frmDiscountExpenses
            // 
            AcceptButton = btnOk;
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            CancelButton = btnCancel;
            ClientSize = new Size(480, 636);
            Controls.Add(pnlBody);
            Controls.Add(pnlFooter);
            Controls.Add(pnlHeader);
            Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmDiscountExpenses";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "الخصم والمصروفات والدفع [F7]";
            KeyDown += frmDiscountExpenses_KeyDown;
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.TableLayoutPanel tblHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblEscHint;
        private System.Windows.Forms.Panel pnlBody;
        private System.Windows.Forms.Panel pnlSummary;
        private System.Windows.Forms.TableLayoutPanel tblSummary;
        private System.Windows.Forms.Label lblSummarySubtotalCaption;
        private System.Windows.Forms.Label lblSummarySubtotalVal;
        private System.Windows.Forms.Label lblSummaryDiscountCaption;
        private System.Windows.Forms.Label lblSummaryDiscountVal;
        private System.Windows.Forms.Label lblSummaryTaxCaption;
        private System.Windows.Forms.Label lblSummaryTaxVal;
        private System.Windows.Forms.Label lblSummaryExpenseCaption;
        private System.Windows.Forms.Label lblSummaryExpenseVal;
        private System.Windows.Forms.Label lblSummaryNetCaption;
        private System.Windows.Forms.Label lblSummaryNetVal;
        private System.Windows.Forms.Label lblSummaryRemainingCaption;
        private System.Windows.Forms.Label lblSummaryRemainingVal;
        private System.Windows.Forms.GroupBox grpPayment;
        private System.Windows.Forms.TableLayoutPanel tblPayment;
        private System.Windows.Forms.Label lblPaymentMethod;
        private System.Windows.Forms.ComboBox cmbPaymentMethod;
        private System.Windows.Forms.Label lblPaidSingle;
        private System.Windows.Forms.NumericUpDown numPaidAmount;
        private System.Windows.Forms.Label lblPaidCash;
        private System.Windows.Forms.NumericUpDown numPaidCash;
        private System.Windows.Forms.Label lblPaidBank;
        private System.Windows.Forms.NumericUpDown numPaidBank;
        private System.Windows.Forms.Label lblPaidWallet;
        private System.Windows.Forms.NumericUpDown numPaidWallet;
        private System.Windows.Forms.GroupBox grpExpense;
        private System.Windows.Forms.TableLayoutPanel tblExpense;
        private System.Windows.Forms.Label lblExpenseAccount;
        private System.Windows.Forms.ComboBox cmbExpenseAccount;
        private System.Windows.Forms.Label lblExpenseAmount;
        private System.Windows.Forms.NumericUpDown numExpenseAmount;
        private System.Windows.Forms.GroupBox grpTax;
        private System.Windows.Forms.TableLayoutPanel tblTax;
        private System.Windows.Forms.Label lblTaxRate;
        private System.Windows.Forms.ComboBox cmbTaxRate;
        private System.Windows.Forms.GroupBox grpDiscount;
        private System.Windows.Forms.TableLayoutPanel tblDiscount;
        private System.Windows.Forms.FlowLayoutPanel flpDiscountType;
        private System.Windows.Forms.RadioButton rbDiscountAmount;
        private System.Windows.Forms.RadioButton rbDiscountPercent;
        private System.Windows.Forms.Label lblDiscountValue;
        private System.Windows.Forms.NumericUpDown numDiscountValue;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.FlowLayoutPanel flpFooter;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
    }
}