namespace SalesSystem.Desktop.Screens.POS.Forms
{
    partial class frmPOS
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            components?.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            pnlTop = new Panel();
            flpTop = new FlowLayoutPanel();
            flpBarcode = new FlowLayoutPanel();
            lblBarcode = new Label();
            txtBarcode = new TextBox();
            btnPending = new Button();
            flpCurrency = new FlowLayoutPanel();
            lblCurrency = new Label();
            cmbCurrency = new ComboBox();
            flpCustomer = new FlowLayoutPanel();
            lblCustomer = new Label();
            cmbCustomer = new ComboBox();
            lblBalanceBadge = new Label();
            dgvCart = new DataGridView();
            pnlCheckout = new Panel();
            flpCheckout = new FlowLayoutPanel();
            pnlPayment = new Panel();
            flpPay = new FlowLayoutPanel();
            flpPayMethod = new FlowLayoutPanel();
            lblPayMethod = new Label();
            cmbPayMethod = new ComboBox();
            flpPaidCash = new FlowLayoutPanel();
            lblPaidCash = new Label();
            txtPaidCash = new TextBox();
            flpPaidBank = new FlowLayoutPanel();
            lblPaidBank = new Label();
            txtPaidBank = new TextBox();
            flpPaidWallet = new FlowLayoutPanel();
            lblPaidWallet = new Label();
            txtPaidWallet = new TextBox();
            flpDiscount = new FlowLayoutPanel();
            lblDiscount = new Label();
            numDiscount = new NumericUpDown();
            flpTax = new FlowLayoutPanel();
            lblTax = new Label();
            numTax = new NumericUpDown();
            pnlTotals = new Panel();
            tblTotals = new TableLayoutPanel();
            lblSubTotal = new Label();
            lblSubTotalVal = new Label();
            lblDiscountTotal = new Label();
            lblDiscountTotalVal = new Label();
            lblNetTotal = new Label();
            lblNetTotalVal = new Label();
            lblPaidTotal = new Label();
            lblPaidTotalVal = new Label();
            lblRemaining = new Label();
            lblRemainingVal = new Label();
            lblDueToStore = new Label();
            lblDueToStoreVal = new Label();
            btnSave = new Button();
            dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn6 = new DataGridViewTextBoxColumn();
            pnlTop.SuspendLayout();
            flpTop.SuspendLayout();
            flpBarcode.SuspendLayout();
            flpCurrency.SuspendLayout();
            flpCustomer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCart).BeginInit();
            pnlCheckout.SuspendLayout();
            flpCheckout.SuspendLayout();
            pnlPayment.SuspendLayout();
            flpPay.SuspendLayout();
            flpPayMethod.SuspendLayout();
            flpPaidCash.SuspendLayout();
            flpPaidBank.SuspendLayout();
            flpPaidWallet.SuspendLayout();
            flpDiscount.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numDiscount).BeginInit();
            flpTax.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numTax).BeginInit();
            pnlTotals.SuspendLayout();
            tblTotals.SuspendLayout();
            SuspendLayout();
            // 
            // pnlTop
            // 
            pnlTop.BackColor = Color.White;
            pnlTop.BorderStyle = BorderStyle.FixedSingle;
            pnlTop.Controls.Add(flpTop);
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Location = new Point(0, 0);
            pnlTop.Name = "pnlTop";
            pnlTop.Padding = new Padding(10, 8, 10, 8);
            pnlTop.Size = new Size(1200, 64);
            pnlTop.TabIndex = 2;
            // 
            // flpTop
            // 
            flpTop.Controls.Add(flpBarcode);
            flpTop.Controls.Add(btnPending);
            flpTop.Controls.Add(flpCurrency);
            flpTop.Controls.Add(flpCustomer);
            flpTop.Controls.Add(lblBalanceBadge);
            flpTop.Dock = DockStyle.Fill;
            flpTop.FlowDirection = FlowDirection.RightToLeft;
            flpTop.Location = new Point(10, 8);
            flpTop.Name = "flpTop";
            flpTop.Size = new Size(1178, 46);
            flpTop.TabIndex = 0;
            // 
            // flpBarcode
            // 
            flpBarcode.AutoSize = true;
            flpBarcode.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpBarcode.Controls.Add(lblBarcode);
            flpBarcode.Controls.Add(txtBarcode);
            flpBarcode.FlowDirection = FlowDirection.RightToLeft;
            flpBarcode.Location = new Point(8, 0);
            flpBarcode.Margin = new Padding(8, 0, 8, 0);
            flpBarcode.Name = "flpBarcode";
            flpBarcode.Size = new Size(341, 42);
            flpBarcode.TabIndex = 0;
            flpBarcode.WrapContents = false;
            // 
            // lblBarcode
            // 
            lblBarcode.AutoSize = true;
            lblBarcode.Font = new Font("Tahoma", 12F);
            lblBarcode.Location = new Point(5, 8);
            lblBarcode.Margin = new Padding(0, 8, 5, 0);
            lblBarcode.Name = "lblBarcode";
            lblBarcode.Size = new Size(130, 29);
            lblBarcode.TabIndex = 0;
            lblBarcode.Text = "باركود [F2]:";
            // 
            // txtBarcode
            // 
            txtBarcode.Font = new Font("Tahoma", 12F);
            txtBarcode.Location = new Point(138, 3);
            txtBarcode.Name = "txtBarcode";
            txtBarcode.PlaceholderText = "امسح الباركود ثم اضغط Enter";
            txtBarcode.Size = new Size(200, 36);
            txtBarcode.TabIndex = 1;
            txtBarcode.KeyDown += txtBarcode_KeyDown;
            // 
            // btnPending
            // 
            btnPending.BackColor = Color.White;
            btnPending.Cursor = Cursors.Hand;
            btnPending.FlatAppearance.BorderColor = Color.FromArgb(222, 226, 230);
            btnPending.FlatStyle = FlatStyle.Flat;
            btnPending.Font = new Font("Tahoma", 11F, FontStyle.Bold);
            btnPending.ForeColor = Color.FromArgb(33, 37, 41);
            btnPending.Location = new Point(367, 0);
            btnPending.Margin = new Padding(10, 0, 10, 0);
            btnPending.Name = "btnPending";
            btnPending.Size = new Size(150, 39);
            btnPending.TabIndex = 1;
            btnPending.Text = "فواتير معلقة [F4]";
            btnPending.UseVisualStyleBackColor = false;
            btnPending.Click += btnPending_Click;
            // 
            // flpCurrency
            // 
            flpCurrency.AutoSize = true;
            flpCurrency.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpCurrency.Controls.Add(lblCurrency);
            flpCurrency.Controls.Add(cmbCurrency);
            flpCurrency.FlowDirection = FlowDirection.RightToLeft;
            flpCurrency.Location = new Point(535, 0);
            flpCurrency.Margin = new Padding(8, 0, 8, 0);
            flpCurrency.Name = "flpCurrency";
            flpCurrency.Size = new Size(204, 39);
            flpCurrency.TabIndex = 2;
            flpCurrency.WrapContents = false;
            // 
            // lblCurrency
            // 
            lblCurrency.AutoSize = true;
            lblCurrency.Font = new Font("Tahoma", 12F);
            lblCurrency.Location = new Point(5, 8);
            lblCurrency.Margin = new Padding(0, 8, 5, 0);
            lblCurrency.Name = "lblCurrency";
            lblCurrency.Size = new Size(83, 29);
            lblCurrency.TabIndex = 0;
            lblCurrency.Text = "العملة:";
            // 
            // cmbCurrency
            // 
            cmbCurrency.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCurrency.Font = new Font("Tahoma", 11F);
            cmbCurrency.Items.AddRange(new object[] { "ريال يمني", "دولار أمريكي", "ريال سعودي" });
            cmbCurrency.Location = new Point(91, 3);
            cmbCurrency.Name = "cmbCurrency";
            cmbCurrency.Size = new Size(110, 35);
            cmbCurrency.TabIndex = 1;
            // 
            // flpCustomer
            // 
            flpCustomer.AutoSize = true;
            flpCustomer.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpCustomer.Controls.Add(lblCustomer);
            flpCustomer.Controls.Add(cmbCustomer);
            flpCustomer.FlowDirection = FlowDirection.RightToLeft;
            flpCustomer.Location = new Point(755, 0);
            flpCustomer.Margin = new Padding(8, 0, 8, 0);
            flpCustomer.Name = "flpCustomer";
            flpCustomer.Size = new Size(297, 39);
            flpCustomer.TabIndex = 3;
            flpCustomer.WrapContents = false;
            // 
            // lblCustomer
            // 
            lblCustomer.AutoSize = true;
            lblCustomer.Font = new Font("Tahoma", 12F);
            lblCustomer.Location = new Point(5, 8);
            lblCustomer.Margin = new Padding(0, 8, 5, 0);
            lblCustomer.Name = "lblCustomer";
            lblCustomer.Size = new Size(86, 29);
            lblCustomer.TabIndex = 0;
            lblCustomer.Text = "العميل:";
            // 
            // cmbCustomer
            // 
            cmbCustomer.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCustomer.Font = new Font("Tahoma", 11F);
            cmbCustomer.Location = new Point(94, 3);
            cmbCustomer.Name = "cmbCustomer";
            cmbCustomer.Size = new Size(200, 35);
            cmbCustomer.TabIndex = 1;
            cmbCustomer.SelectedIndexChanged += cmbCustomer_SelectedIndexChanged;
            // 
            // lblBalanceBadge
            // 
            lblBalanceBadge.BackColor = Color.FromArgb(108, 117, 125);
            lblBalanceBadge.BorderStyle = BorderStyle.FixedSingle;
            lblBalanceBadge.Font = new Font("Tahoma", 11F, FontStyle.Bold);
            lblBalanceBadge.ForeColor = Color.White;
            lblBalanceBadge.Location = new Point(0, 42);
            lblBalanceBadge.Margin = new Padding(10, 0, 0, 0);
            lblBalanceBadge.Name = "lblBalanceBadge";
            lblBalanceBadge.Size = new Size(150, 32);
            lblBalanceBadge.TabIndex = 4;
            lblBalanceBadge.Text = "حساب خالص";
            lblBalanceBadge.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // dgvCart
            // 
            dgvCart.AllowUserToAddRows = false;
            dgvCart.AllowUserToDeleteRows = false;
            dataGridViewCellStyle4.BackColor = Color.FromArgb(242, 242, 242);
            dgvCart.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            dgvCart.BackgroundColor = Color.White;
            dgvCart.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = Color.FromArgb(248, 249, 250);
            dataGridViewCellStyle5.Font = new Font("Tahoma", 11F, FontStyle.Bold);
            dataGridViewCellStyle5.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.True;
            dgvCart.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            dgvCart.ColumnHeadersHeight = 34;
            dgvCart.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn1, dataGridViewTextBoxColumn2, dataGridViewTextBoxColumn3, dataGridViewTextBoxColumn4, dataGridViewTextBoxColumn5, dataGridViewTextBoxColumn6 });
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = SystemColors.Window;
            dataGridViewCellStyle6.Font = new Font("Tahoma", 12F);
            dataGridViewCellStyle6.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.False;
            dgvCart.DefaultCellStyle = dataGridViewCellStyle6;
            dgvCart.Dock = DockStyle.Fill;
            dgvCart.EnableHeadersVisualStyles = false;
            dgvCart.Location = new Point(0, 64);
            dgvCart.MultiSelect = false;
            dgvCart.Name = "dgvCart";
            dgvCart.ReadOnly = true;
            dgvCart.RowHeadersVisible = false;
            dgvCart.RowHeadersWidth = 62;
            dgvCart.RowTemplate.Height = 34;
            dgvCart.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCart.Size = new Size(1200, 506);
            dgvCart.TabIndex = 0;
            // 
            // pnlCheckout
            // 
            pnlCheckout.BackColor = Color.White;
            pnlCheckout.BorderStyle = BorderStyle.FixedSingle;
            pnlCheckout.Controls.Add(flpCheckout);
            pnlCheckout.Dock = DockStyle.Bottom;
            pnlCheckout.Location = new Point(0, 570);
            pnlCheckout.Name = "pnlCheckout";
            pnlCheckout.Padding = new Padding(10, 8, 10, 10);
            pnlCheckout.Size = new Size(1200, 250);
            pnlCheckout.TabIndex = 1;
            // 
            // flpCheckout
            // 
            flpCheckout.Controls.Add(pnlPayment);
            flpCheckout.Controls.Add(pnlTotals);
            flpCheckout.Controls.Add(btnSave);
            flpCheckout.Dock = DockStyle.Fill;
            flpCheckout.FlowDirection = FlowDirection.RightToLeft;
            flpCheckout.Location = new Point(10, 8);
            flpCheckout.Name = "flpCheckout";
            flpCheckout.Size = new Size(1178, 230);
            flpCheckout.TabIndex = 0;
            flpCheckout.WrapContents = false;
            // 
            // pnlPayment
            // 
            pnlPayment.Controls.Add(flpPay);
            pnlPayment.Location = new Point(5, 0);
            pnlPayment.Margin = new Padding(5, 0, 5, 0);
            pnlPayment.Name = "pnlPayment";
            pnlPayment.Size = new Size(470, 215);
            pnlPayment.TabIndex = 0;
            // 
            // flpPay
            // 
            flpPay.Controls.Add(flpPayMethod);
            flpPay.Controls.Add(flpPaidCash);
            flpPay.Controls.Add(flpPaidBank);
            flpPay.Controls.Add(flpPaidWallet);
            flpPay.Controls.Add(flpDiscount);
            flpPay.Controls.Add(flpTax);
            flpPay.Dock = DockStyle.Fill;
            flpPay.FlowDirection = FlowDirection.RightToLeft;
            flpPay.Location = new Point(0, 0);
            flpPay.Name = "flpPay";
            flpPay.Size = new Size(470, 215);
            flpPay.TabIndex = 0;
            // 
            // flpPayMethod
            // 
            flpPayMethod.AutoSize = true;
            flpPayMethod.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpPayMethod.Controls.Add(lblPayMethod);
            flpPayMethod.Controls.Add(cmbPayMethod);
            flpPayMethod.FlowDirection = FlowDirection.RightToLeft;
            flpPayMethod.Location = new Point(8, 4);
            flpPayMethod.Margin = new Padding(0, 4, 8, 0);
            flpPayMethod.Name = "flpPayMethod";
            flpPayMethod.Size = new Size(297, 39);
            flpPayMethod.TabIndex = 0;
            flpPayMethod.WrapContents = false;
            // 
            // lblPayMethod
            // 
            lblPayMethod.AutoSize = true;
            lblPayMethod.Font = new Font("Tahoma", 11F);
            lblPayMethod.Location = new Point(5, 6);
            lblPayMethod.Margin = new Padding(0, 6, 5, 0);
            lblPayMethod.Name = "lblPayMethod";
            lblPayMethod.Size = new Size(126, 27);
            lblPayMethod.TabIndex = 0;
            lblPayMethod.Text = "طريقة الدفع:";
            // 
            // cmbPayMethod
            // 
            cmbPayMethod.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPayMethod.Font = new Font("Tahoma", 11F);
            cmbPayMethod.Items.AddRange(new object[] { "نقدي", "كريمي", "شبكة / بطاقة", "آجل", "محفظة جيب", "دفع مجزأ" });
            cmbPayMethod.Location = new Point(134, 3);
            cmbPayMethod.Name = "cmbPayMethod";
            cmbPayMethod.Size = new Size(160, 35);
            cmbPayMethod.TabIndex = 1;
            // 
            // flpPaidCash
            // 
            flpPaidCash.AutoSize = true;
            flpPaidCash.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpPaidCash.Controls.Add(lblPaidCash);
            flpPaidCash.Controls.Add(txtPaidCash);
            flpPaidCash.FlowDirection = FlowDirection.RightToLeft;
            flpPaidCash.Location = new Point(8, 47);
            flpPaidCash.Margin = new Padding(0, 4, 8, 0);
            flpPaidCash.Name = "flpPaidCash";
            flpPaidCash.Size = new Size(230, 40);
            flpPaidCash.TabIndex = 1;
            flpPaidCash.WrapContents = false;
            // 
            // lblPaidCash
            // 
            lblPaidCash.AutoSize = true;
            lblPaidCash.Font = new Font("Tahoma", 11F);
            lblPaidCash.Location = new Point(5, 6);
            lblPaidCash.Margin = new Padding(0, 6, 5, 0);
            lblPaidCash.Name = "lblPaidCash";
            lblPaidCash.Size = new Size(124, 27);
            lblPaidCash.TabIndex = 0;
            lblPaidCash.Text = "المدفوع نقد:";
            // 
            // txtPaidCash
            // 
            txtPaidCash.Font = new Font("Tahoma", 11F);
            txtPaidCash.Location = new Point(132, 3);
            txtPaidCash.Name = "txtPaidCash";
            txtPaidCash.Size = new Size(95, 34);
            txtPaidCash.TabIndex = 1;
            txtPaidCash.Text = "0";
            txtPaidCash.TextAlign = HorizontalAlignment.Right;
            txtPaidCash.TextChanged += MoneyField_TextChanged;
            // 
            // flpPaidBank
            // 
            flpPaidBank.AutoSize = true;
            flpPaidBank.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpPaidBank.Controls.Add(lblPaidBank);
            flpPaidBank.Controls.Add(txtPaidBank);
            flpPaidBank.FlowDirection = FlowDirection.RightToLeft;
            flpPaidBank.Location = new Point(8, 91);
            flpPaidBank.Margin = new Padding(0, 4, 8, 0);
            flpPaidBank.Name = "flpPaidBank";
            flpPaidBank.Size = new Size(233, 40);
            flpPaidBank.TabIndex = 2;
            flpPaidBank.WrapContents = false;
            // 
            // lblPaidBank
            // 
            lblPaidBank.AutoSize = true;
            lblPaidBank.Font = new Font("Tahoma", 11F);
            lblPaidBank.Location = new Point(5, 6);
            lblPaidBank.Margin = new Padding(0, 6, 5, 0);
            lblPaidBank.Name = "lblPaidBank";
            lblPaidBank.Size = new Size(127, 27);
            lblPaidBank.TabIndex = 0;
            lblPaidBank.Text = "المدفوع بنك:";
            // 
            // txtPaidBank
            // 
            txtPaidBank.Font = new Font("Tahoma", 11F);
            txtPaidBank.Location = new Point(135, 3);
            txtPaidBank.Name = "txtPaidBank";
            txtPaidBank.Size = new Size(95, 34);
            txtPaidBank.TabIndex = 1;
            txtPaidBank.Text = "0";
            txtPaidBank.TextAlign = HorizontalAlignment.Right;
            txtPaidBank.TextChanged += MoneyField_TextChanged;
            // 
            // flpPaidWallet
            // 
            flpPaidWallet.AutoSize = true;
            flpPaidWallet.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpPaidWallet.Controls.Add(lblPaidWallet);
            flpPaidWallet.Controls.Add(txtPaidWallet);
            flpPaidWallet.FlowDirection = FlowDirection.RightToLeft;
            flpPaidWallet.Location = new Point(8, 135);
            flpPaidWallet.Margin = new Padding(0, 4, 8, 0);
            flpPaidWallet.Name = "flpPaidWallet";
            flpPaidWallet.Size = new Size(266, 40);
            flpPaidWallet.TabIndex = 3;
            flpPaidWallet.WrapContents = false;
            // 
            // lblPaidWallet
            // 
            lblPaidWallet.AutoSize = true;
            lblPaidWallet.Font = new Font("Tahoma", 11F);
            lblPaidWallet.Location = new Point(5, 6);
            lblPaidWallet.Margin = new Padding(0, 6, 5, 0);
            lblPaidWallet.Name = "lblPaidWallet";
            lblPaidWallet.Size = new Size(160, 27);
            lblPaidWallet.TabIndex = 0;
            lblPaidWallet.Text = "المدفوع محفظة:";
            // 
            // txtPaidWallet
            // 
            txtPaidWallet.Font = new Font("Tahoma", 11F);
            txtPaidWallet.Location = new Point(168, 3);
            txtPaidWallet.Name = "txtPaidWallet";
            txtPaidWallet.Size = new Size(95, 34);
            txtPaidWallet.TabIndex = 1;
            txtPaidWallet.Text = "0";
            txtPaidWallet.TextAlign = HorizontalAlignment.Right;
            txtPaidWallet.TextChanged += MoneyField_TextChanged;
            // 
            // flpDiscount
            // 
            flpDiscount.AutoSize = true;
            flpDiscount.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpDiscount.Controls.Add(lblDiscount);
            flpDiscount.Controls.Add(numDiscount);
            flpDiscount.FlowDirection = FlowDirection.RightToLeft;
            flpDiscount.Location = new Point(282, 135);
            flpDiscount.Margin = new Padding(0, 4, 8, 0);
            flpDiscount.Name = "flpDiscount";
            flpDiscount.Size = new Size(173, 40);
            flpDiscount.TabIndex = 4;
            flpDiscount.WrapContents = false;
            // 
            // lblDiscount
            // 
            lblDiscount.AutoSize = true;
            lblDiscount.Font = new Font("Tahoma", 11F);
            lblDiscount.Location = new Point(5, 6);
            lblDiscount.Margin = new Padding(0, 6, 5, 0);
            lblDiscount.Name = "lblDiscount";
            lblDiscount.Size = new Size(67, 27);
            lblDiscount.TabIndex = 0;
            lblDiscount.Text = "خصم:";
            // 
            // numDiscount
            // 
            numDiscount.DecimalPlaces = 2;
            numDiscount.Font = new Font("Tahoma", 11F);
            numDiscount.Location = new Point(75, 3);
            numDiscount.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            numDiscount.Name = "numDiscount";
            numDiscount.Size = new Size(95, 34);
            numDiscount.TabIndex = 1;
            numDiscount.ValueChanged += MoneyField_TextChanged;
            // 
            // flpTax
            // 
            flpTax.AutoSize = true;
            flpTax.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpTax.Controls.Add(lblTax);
            flpTax.Controls.Add(numTax);
            flpTax.FlowDirection = FlowDirection.RightToLeft;
            flpTax.Location = new Point(8, 179);
            flpTax.Margin = new Padding(0, 4, 8, 0);
            flpTax.Name = "flpTax";
            flpTax.Size = new Size(187, 40);
            flpTax.TabIndex = 5;
            flpTax.WrapContents = false;
            // 
            // lblTax
            // 
            lblTax.AutoSize = true;
            lblTax.Font = new Font("Tahoma", 11F);
            lblTax.Location = new Point(5, 6);
            lblTax.Margin = new Padding(0, 6, 5, 0);
            lblTax.Name = "lblTax";
            lblTax.Size = new Size(101, 27);
            lblTax.TabIndex = 0;
            lblTax.Text = "ضريبة %:";
            // 
            // numTax
            // 
            numTax.DecimalPlaces = 2;
            numTax.Font = new Font("Tahoma", 11F);
            numTax.Location = new Point(109, 3);
            numTax.Name = "numTax";
            numTax.Size = new Size(75, 34);
            numTax.TabIndex = 1;
            numTax.Value = new decimal(new int[] { 5, 0, 0, 0 });
            numTax.ValueChanged += MoneyField_TextChanged;
            // 
            // pnlTotals
            // 
            pnlTotals.Controls.Add(tblTotals);
            pnlTotals.Location = new Point(485, 0);
            pnlTotals.Margin = new Padding(5, 0, 5, 0);
            pnlTotals.Name = "pnlTotals";
            pnlTotals.Size = new Size(460, 215);
            pnlTotals.TabIndex = 1;
            // 
            // tblTotals
            // 
            tblTotals.ColumnCount = 2;
            tblTotals.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tblTotals.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblTotals.Controls.Add(lblSubTotal, 0, 0);
            tblTotals.Controls.Add(lblSubTotalVal, 1, 0);
            tblTotals.Controls.Add(lblDiscountTotal, 0, 1);
            tblTotals.Controls.Add(lblDiscountTotalVal, 1, 1);
            tblTotals.Controls.Add(lblNetTotal, 0, 2);
            tblTotals.Controls.Add(lblNetTotalVal, 1, 2);
            tblTotals.Controls.Add(lblPaidTotal, 0, 3);
            tblTotals.Controls.Add(lblPaidTotalVal, 1, 3);
            tblTotals.Controls.Add(lblRemaining, 0, 4);
            tblTotals.Controls.Add(lblRemainingVal, 1, 4);
            tblTotals.Controls.Add(lblDueToStore, 0, 5);
            tblTotals.Controls.Add(lblDueToStoreVal, 1, 5);
            tblTotals.Dock = DockStyle.Fill;
            tblTotals.Location = new Point(0, 0);
            tblTotals.Name = "tblTotals";
            tblTotals.Padding = new Padding(10, 5, 10, 5);
            tblTotals.RightToLeft = RightToLeft.Yes;
            tblTotals.RowCount = 6;
            tblTotals.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tblTotals.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tblTotals.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tblTotals.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tblTotals.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tblTotals.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tblTotals.Size = new Size(460, 215);
            tblTotals.TabIndex = 0;
            // 
            // lblSubTotal
            // 
            lblSubTotal.AutoSize = true;
            lblSubTotal.Font = new Font("Tahoma", 11F);
            lblSubTotal.Location = new Point(360, 5);
            lblSubTotal.Name = "lblSubTotal";
            lblSubTotal.Size = new Size(87, 30);
            lblSubTotal.TabIndex = 0;
            lblSubTotal.Text = "المجموع الفرعي:";
            // 
            // lblSubTotalVal
            // 
            lblSubTotalVal.AutoSize = true;
            lblSubTotalVal.Font = new Font("Tahoma", 11F);
            lblSubTotalVal.Location = new Point(242, 5);
            lblSubTotalVal.Name = "lblSubTotalVal";
            lblSubTotalVal.Size = new Size(55, 27);
            lblSubTotalVal.TabIndex = 1;
            lblSubTotalVal.Text = "0.00";
            // 
            // lblDiscountTotal
            // 
            lblDiscountTotal.AutoSize = true;
            lblDiscountTotal.Font = new Font("Tahoma", 11F);
            lblDiscountTotal.Location = new Point(367, 35);
            lblDiscountTotal.Name = "lblDiscountTotal";
            lblDiscountTotal.Size = new Size(80, 30);
            lblDiscountTotal.TabIndex = 2;
            lblDiscountTotal.Text = "إجمالي الخصم:";
            // 
            // lblDiscountTotalVal
            // 
            lblDiscountTotalVal.AutoSize = true;
            lblDiscountTotalVal.Font = new Font("Tahoma", 11F);
            lblDiscountTotalVal.Location = new Point(242, 35);
            lblDiscountTotalVal.Name = "lblDiscountTotalVal";
            lblDiscountTotalVal.Size = new Size(55, 27);
            lblDiscountTotalVal.TabIndex = 3;
            lblDiscountTotalVal.Text = "0.00";
            // 
            // lblNetTotal
            // 
            lblNetTotal.AutoSize = true;
            lblNetTotal.Font = new Font("Tahoma", 12F, FontStyle.Bold);
            lblNetTotal.Location = new Point(344, 65);
            lblNetTotal.Name = "lblNetTotal";
            lblNetTotal.Size = new Size(103, 30);
            lblNetTotal.TabIndex = 4;
            lblNetTotal.Text = "الصافي النهائي:";
            // 
            // lblNetTotalVal
            // 
            lblNetTotalVal.AutoSize = true;
            lblNetTotalVal.Font = new Font("Tahoma", 13F, FontStyle.Bold);
            lblNetTotalVal.ForeColor = Color.FromArgb(13, 110, 253);
            lblNetTotalVal.Location = new Point(224, 65);
            lblNetTotalVal.Name = "lblNetTotalVal";
            lblNetTotalVal.Size = new Size(73, 30);
            lblNetTotalVal.TabIndex = 5;
            lblNetTotalVal.Text = "0.00";
            // 
            // lblPaidTotal
            // 
            lblPaidTotal.AutoSize = true;
            lblPaidTotal.Font = new Font("Tahoma", 11F);
            lblPaidTotal.Location = new Point(359, 95);
            lblPaidTotal.Name = "lblPaidTotal";
            lblPaidTotal.Size = new Size(88, 30);
            lblPaidTotal.TabIndex = 6;
            lblPaidTotal.Text = "المبلغ المدفوع:";
            // 
            // lblPaidTotalVal
            // 
            lblPaidTotalVal.AutoSize = true;
            lblPaidTotalVal.Font = new Font("Tahoma", 11F);
            lblPaidTotalVal.Location = new Point(242, 95);
            lblPaidTotalVal.Name = "lblPaidTotalVal";
            lblPaidTotalVal.Size = new Size(55, 27);
            lblPaidTotalVal.TabIndex = 7;
            lblPaidTotalVal.Text = "0.00";
            // 
            // lblRemaining
            // 
            lblRemaining.AutoSize = true;
            lblRemaining.Font = new Font("Tahoma", 12F, FontStyle.Bold);
            lblRemaining.Location = new Point(353, 125);
            lblRemaining.Name = "lblRemaining";
            lblRemaining.Size = new Size(94, 29);
            lblRemaining.TabIndex = 8;
            lblRemaining.Text = "الباقي:";
            // 
            // lblRemainingVal
            // 
            lblRemainingVal.AutoSize = true;
            lblRemainingVal.Font = new Font("Tahoma", 12F, FontStyle.Bold);
            lblRemainingVal.Location = new Point(231, 125);
            lblRemainingVal.Name = "lblRemainingVal";
            lblRemainingVal.Size = new Size(66, 29);
            lblRemainingVal.TabIndex = 9;
            lblRemainingVal.Text = "0.00";
            // 
            // lblDueToStore
            // 
            lblDueToStore.AutoSize = true;
            lblDueToStore.Font = new Font("Tahoma", 11F);
            lblDueToStore.Location = new Point(345, 155);
            lblDueToStore.Name = "lblDueToStore";
            lblDueToStore.Size = new Size(102, 54);
            lblDueToStore.TabIndex = 10;
            lblDueToStore.Text = "المستحق للمحل:";
            // 
            // lblDueToStoreVal
            // 
            lblDueToStoreVal.AutoSize = true;
            lblDueToStoreVal.Font = new Font("Tahoma", 12F, FontStyle.Bold);
            lblDueToStoreVal.ForeColor = Color.FromArgb(25, 135, 84);
            lblDueToStoreVal.Location = new Point(231, 155);
            lblDueToStoreVal.Name = "lblDueToStoreVal";
            lblDueToStoreVal.Size = new Size(66, 29);
            lblDueToStoreVal.TabIndex = 11;
            lblDueToStoreVal.Text = "0.00";
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(25, 135, 84);
            btnSave.Cursor = Cursors.Hand;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(955, 45);
            btnSave.Margin = new Padding(5, 45, 5, 5);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(200, 120);
            btnSave.TabIndex = 2;
            btnSave.Text = "حفظ واعتماد";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewTextBoxColumn1.MinimumWidth = 8;
            dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            dataGridViewTextBoxColumn1.ReadOnly = true;
            dataGridViewTextBoxColumn1.Width = 150;
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewTextBoxColumn2.MinimumWidth = 8;
            dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            dataGridViewTextBoxColumn2.ReadOnly = true;
            dataGridViewTextBoxColumn2.Width = 150;
            // 
            // dataGridViewTextBoxColumn3
            // 
            dataGridViewTextBoxColumn3.MinimumWidth = 8;
            dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            dataGridViewTextBoxColumn3.ReadOnly = true;
            dataGridViewTextBoxColumn3.Width = 150;
            // 
            // dataGridViewTextBoxColumn4
            // 
            dataGridViewTextBoxColumn4.MinimumWidth = 8;
            dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            dataGridViewTextBoxColumn4.ReadOnly = true;
            dataGridViewTextBoxColumn4.Width = 150;
            // 
            // dataGridViewTextBoxColumn5
            // 
            dataGridViewTextBoxColumn5.MinimumWidth = 8;
            dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            dataGridViewTextBoxColumn5.ReadOnly = true;
            dataGridViewTextBoxColumn5.Width = 150;
            // 
            // dataGridViewTextBoxColumn6
            // 
            dataGridViewTextBoxColumn6.MinimumWidth = 8;
            dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            dataGridViewTextBoxColumn6.ReadOnly = true;
            dataGridViewTextBoxColumn6.Width = 150;
            // 
            // frmPOS
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(1200, 820);
            Controls.Add(dgvCart);
            Controls.Add(pnlCheckout);
            Controls.Add(pnlTop);
            Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            KeyPreview = true;
            MinimumSize = new Size(1000, 700);
            Name = "frmPOS";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "نقطة البيع (POS) — نموذج تجريبي";
            KeyDown += frmPOS_KeyDown;
            pnlTop.ResumeLayout(false);
            flpTop.ResumeLayout(false);
            flpTop.PerformLayout();
            flpBarcode.ResumeLayout(false);
            flpBarcode.PerformLayout();
            flpCurrency.ResumeLayout(false);
            flpCurrency.PerformLayout();
            flpCustomer.ResumeLayout(false);
            flpCustomer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCart).EndInit();
            pnlCheckout.ResumeLayout(false);
            flpCheckout.ResumeLayout(false);
            pnlPayment.ResumeLayout(false);
            flpPay.ResumeLayout(false);
            flpPay.PerformLayout();
            flpPayMethod.ResumeLayout(false);
            flpPayMethod.PerformLayout();
            flpPaidCash.ResumeLayout(false);
            flpPaidCash.PerformLayout();
            flpPaidBank.ResumeLayout(false);
            flpPaidBank.PerformLayout();
            flpPaidWallet.ResumeLayout(false);
            flpPaidWallet.PerformLayout();
            flpDiscount.ResumeLayout(false);
            flpDiscount.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numDiscount).EndInit();
            flpTax.ResumeLayout(false);
            flpTax.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numTax).EndInit();
            pnlTotals.ResumeLayout(false);
            tblTotals.ResumeLayout(false);
            tblTotals.PerformLayout();
            ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlTop, pnlCheckout, pnlPayment, pnlTotals;
        private System.Windows.Forms.FlowLayoutPanel flpTop, flpBarcode, flpCurrency, flpCustomer, flpCheckout, flpPay, flpPayMethod, flpPaidCash, flpPaidBank, flpPaidWallet, flpDiscount, flpTax;
        private System.Windows.Forms.Label lblBarcode, lblCurrency, lblCustomer, lblBalanceBadge, lblPayMethod, lblPaidCash, lblPaidBank, lblPaidWallet, lblDiscount, lblTax, lblSubTotal, lblSubTotalVal, lblDiscountTotal, lblDiscountTotalVal, lblNetTotal, lblNetTotalVal, lblPaidTotal, lblPaidTotalVal, lblRemaining, lblRemainingVal, lblDueToStore, lblDueToStoreVal;
        private System.Windows.Forms.TextBox txtBarcode, txtPaidCash, txtPaidBank, txtPaidWallet;
        private System.Windows.Forms.Button btnPending, btnSave;
        private System.Windows.Forms.ComboBox cmbCurrency, cmbCustomer, cmbPayMethod;
        private System.Windows.Forms.NumericUpDown numDiscount, numTax;
        private System.Windows.Forms.DataGridView dgvCart;
        private System.Windows.Forms.TableLayoutPanel tblTotals;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
    }
}