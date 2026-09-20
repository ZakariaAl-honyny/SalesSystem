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
            this.pnlTop = new System.Windows.Forms.Panel();
            this.flpTop = new System.Windows.Forms.FlowLayoutPanel();
            this.flpBarcode = new System.Windows.Forms.FlowLayoutPanel();
            this.lblBarcode = new System.Windows.Forms.Label();
            this.txtBarcode = new System.Windows.Forms.TextBox();
            this.btnPending = new System.Windows.Forms.Button();
            this.flpCurrency = new System.Windows.Forms.FlowLayoutPanel();
            this.lblCurrency = new System.Windows.Forms.Label();
            this.cmbCurrency = new System.Windows.Forms.ComboBox();
            this.flpCustomer = new System.Windows.Forms.FlowLayoutPanel();
            this.lblCustomer = new System.Windows.Forms.Label();
            this.cmbCustomer = new System.Windows.Forms.ComboBox();
            this.lblBalanceBadge = new System.Windows.Forms.Label();
            this.dgvCart = new System.Windows.Forms.DataGridView();
            this.pnlCheckout = new System.Windows.Forms.Panel();
            this.flpCheckout = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlPayment = new System.Windows.Forms.Panel();
            this.flpPay = new System.Windows.Forms.FlowLayoutPanel();
            this.flpPayMethod = new System.Windows.Forms.FlowLayoutPanel();
            this.lblPayMethod = new System.Windows.Forms.Label();
            this.cmbPayMethod = new System.Windows.Forms.ComboBox();
            this.flpPaidCash = new System.Windows.Forms.FlowLayoutPanel();
            this.lblPaidCash = new System.Windows.Forms.Label();
            this.txtPaidCash = new System.Windows.Forms.TextBox();
            this.flpPaidBank = new System.Windows.Forms.FlowLayoutPanel();
            this.lblPaidBank = new System.Windows.Forms.Label();
            this.txtPaidBank = new System.Windows.Forms.TextBox();
            this.flpPaidWallet = new System.Windows.Forms.FlowLayoutPanel();
            this.lblPaidWallet = new System.Windows.Forms.Label();
            this.txtPaidWallet = new System.Windows.Forms.TextBox();
            this.flpDiscount = new System.Windows.Forms.FlowLayoutPanel();
            this.lblDiscount = new System.Windows.Forms.Label();
            this.numDiscount = new System.Windows.Forms.NumericUpDown();
            this.flpTax = new System.Windows.Forms.FlowLayoutPanel();
            this.lblTax = new System.Windows.Forms.Label();
            this.numTax = new System.Windows.Forms.NumericUpDown();
            this.pnlTotals = new System.Windows.Forms.Panel();
            this.tblTotals = new System.Windows.Forms.TableLayoutPanel();
            this.lblSubTotal = new System.Windows.Forms.Label();
            this.lblSubTotalVal = new System.Windows.Forms.Label();
            this.lblDiscountTotal = new System.Windows.Forms.Label();
            this.lblDiscountTotalVal = new System.Windows.Forms.Label();
            this.lblNetTotal = new System.Windows.Forms.Label();
            this.lblNetTotalVal = new System.Windows.Forms.Label();
            this.lblPaidTotal = new System.Windows.Forms.Label();
            this.lblPaidTotalVal = new System.Windows.Forms.Label();
            this.lblRemaining = new System.Windows.Forms.Label();
            this.lblRemainingVal = new System.Windows.Forms.Label();
            this.lblDueToStore = new System.Windows.Forms.Label();
            this.lblDueToStoreVal = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();

            this.pnlTop.SuspendLayout();
            this.flpTop.SuspendLayout();
            this.flpBarcode.SuspendLayout();
            this.flpCurrency.SuspendLayout();
            this.flpCustomer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCart)).BeginInit();
            this.pnlCheckout.SuspendLayout();
            this.flpCheckout.SuspendLayout();
            this.pnlPayment.SuspendLayout();
            this.flpPay.SuspendLayout();
            this.flpPayMethod.SuspendLayout();
            this.flpPaidCash.SuspendLayout();
            this.flpPaidBank.SuspendLayout();
            this.flpPaidWallet.SuspendLayout();
            this.flpDiscount.SuspendLayout();
            this.flpTax.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDiscount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTax)).BeginInit();
            this.pnlTotals.SuspendLayout();
            this.tblTotals.SuspendLayout();
            this.SuspendLayout();

            // ---------- pnlTop (الشريط العلوي) ----------
            this.pnlTop.Controls.Add(this.flpTop);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Height = 64;
            this.pnlTop.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);
            this.pnlTop.BackColor = System.Drawing.Color.White;
            this.pnlTop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            // flpTop
            this.flpTop.Controls.Add(this.flpBarcode);
            this.flpTop.Controls.Add(this.btnPending);
            this.flpTop.Controls.Add(this.flpCurrency);
            this.flpTop.Controls.Add(this.flpCustomer);
            this.flpTop.Controls.Add(this.lblBalanceBadge);
            this.flpTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpTop.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flpTop.WrapContents = true;

            // flpBarcode (باركود + حقل الإدخال)
            this.flpBarcode.Controls.Add(this.lblBarcode);
            this.flpBarcode.Controls.Add(this.txtBarcode);
            this.flpBarcode.AutoSize = true;
            this.flpBarcode.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpBarcode.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flpBarcode.WrapContents = false;
            this.flpBarcode.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);

            this.lblBarcode.Text = "باركود [F2]:";
            this.lblBarcode.AutoSize = true;
            this.lblBarcode.Margin = new System.Windows.Forms.Padding(0, 8, 5, 0);
            this.lblBarcode.Font = new System.Drawing.Font("Tahoma", 12F);

            this.txtBarcode.Width = 200;
            this.txtBarcode.Font = new System.Drawing.Font("Tahoma", 12F);
            this.txtBarcode.PlaceholderText = "امسح الباركود ثم اضغط Enter";
            this.txtBarcode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBarcode_KeyDown);

            // btnPending
            this.btnPending.Text = "فواتير معلقة [F4]";
            this.btnPending.Width = 150;
            this.btnPending.Height = 30;
            this.btnPending.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.btnPending.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPending.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(222, 226, 230);
            this.btnPending.FlatAppearance.BorderSize = 1;
            this.btnPending.BackColor = System.Drawing.Color.White;
            this.btnPending.ForeColor = System.Drawing.Color.FromArgb(33, 37, 41);
            this.btnPending.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.btnPending.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPending.Click += new System.EventHandler(this.btnPending_Click);

            // flpCurrency (العملة)
            this.flpCurrency.Controls.Add(this.lblCurrency);
            this.flpCurrency.Controls.Add(this.cmbCurrency);
            this.flpCurrency.AutoSize = true;
            this.flpCurrency.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpCurrency.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flpCurrency.WrapContents = false;
            this.flpCurrency.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);

            this.lblCurrency.Text = "العملة:";
            this.lblCurrency.AutoSize = true;
            this.lblCurrency.Margin = new System.Windows.Forms.Padding(0, 8, 5, 0);
            this.lblCurrency.Font = new System.Drawing.Font("Tahoma", 12F);

            this.cmbCurrency.Width = 110;
            this.cmbCurrency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCurrency.Font = new System.Drawing.Font("Tahoma", 11F);
            this.cmbCurrency.Items.AddRange(new object[] { "ريال يمني", "دولار أمريكي", "ريال سعودي" });

            // flpCustomer (العميل)
            this.flpCustomer.Controls.Add(this.lblCustomer);
            this.flpCustomer.Controls.Add(this.cmbCustomer);
            this.flpCustomer.AutoSize = true;
            this.flpCustomer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpCustomer.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flpCustomer.WrapContents = false;
            this.flpCustomer.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);

            this.lblCustomer.Text = "العميل:";
            this.lblCustomer.AutoSize = true;
            this.lblCustomer.Margin = new System.Windows.Forms.Padding(0, 8, 5, 0);
            this.lblCustomer.Font = new System.Drawing.Font("Tahoma", 12F);

            this.cmbCustomer.Width = 200;
            this.cmbCustomer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCustomer.Font = new System.Drawing.Font("Tahoma", 11F);
            this.cmbCustomer.SelectedIndexChanged += new System.EventHandler(this.cmbCustomer_SelectedIndexChanged);

            // lblBalanceBadge (شارة الرصيد)
            this.lblBalanceBadge.Text = "حساب خالص";
            this.lblBalanceBadge.Width = 150;
            this.lblBalanceBadge.Height = 32;
            this.lblBalanceBadge.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.lblBalanceBadge.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblBalanceBadge.BackColor = System.Drawing.Color.FromArgb(108, 117, 125);
            this.lblBalanceBadge.ForeColor = System.Drawing.Color.White;
            this.lblBalanceBadge.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.lblBalanceBadge.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            // ---------- dgvCart (شبكة السلة) ----------
            this.dgvCart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCart.BackgroundColor = System.Drawing.Color.White;
            this.dgvCart.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvCart.AllowUserToAddRows = false;
            this.dgvCart.AllowUserToDeleteRows = false;
            this.dgvCart.ReadOnly = true;
            this.dgvCart.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCart.MultiSelect = false;
            this.dgvCart.RowHeadersVisible = false;
            this.dgvCart.AutoGenerateColumns = false;
            this.dgvCart.RowTemplate.Height = 34;
            this.dgvCart.EnableHeadersVisualStyles = false;
            this.dgvCart.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(248, 249, 250);
            this.dgvCart.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.dgvCart.DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 12F);
            this.dgvCart.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            this.dgvCart.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "ColNo", HeaderText = "#", DataPropertyName = "LineNo", Width = 50 });
            this.dgvCart.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "ColName", HeaderText = "اسم الصنف", DataPropertyName = "ProductName", Width = 340 });
            this.dgvCart.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "ColUnit", HeaderText = "الوحدة", DataPropertyName = "UnitName", Width = 100 });
            this.dgvCart.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "ColQty", HeaderText = "الكمية", DataPropertyName = "Quantity", Width = 110, DefaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle { Format = "N3", Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight } });
            this.dgvCart.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "ColPrice", HeaderText = "السعر", DataPropertyName = "UnitPrice", Width = 130, DefaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle { Format = "N2", Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight } });
            this.dgvCart.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "ColTotal", HeaderText = "الإجمالي", DataPropertyName = "LineTotal", Width = 150, DefaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle { Format = "N2", Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight, ForeColor = System.Drawing.Color.FromArgb(13, 110, 253), Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold) } });

            // ---------- pnlCheckout (لوحة الدفع والإجماليات) ----------
            this.pnlCheckout.Controls.Add(this.flpCheckout);
            this.pnlCheckout.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlCheckout.Height = 250;
            this.pnlCheckout.Padding = new System.Windows.Forms.Padding(10, 8, 10, 10);
            this.pnlCheckout.BackColor = System.Drawing.Color.White;
            this.pnlCheckout.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            // flpCheckout
            this.flpCheckout.Controls.Add(this.pnlPayment);
            this.flpCheckout.Controls.Add(this.pnlTotals);
            this.flpCheckout.Controls.Add(this.btnSave);
            this.flpCheckout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpCheckout.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flpCheckout.WrapContents = false;

            // ---------- pnlPayment (طريقة الدفع + الدفع المجزأ + الخصم/الضريبة) ----------
            this.pnlPayment.Width = 470;
            this.pnlPayment.Height = 215;
            this.pnlPayment.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.pnlPayment.Controls.Add(this.flpPay);

            this.flpPay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpPay.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flpPay.WrapContents = true;

            // flpPayMethod
            this.flpPayMethod.Controls.Add(this.lblPayMethod);
            this.flpPayMethod.Controls.Add(this.cmbPayMethod);
            this.flpPayMethod.AutoSize = true;
            this.flpPayMethod.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpPayMethod.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flpPayMethod.WrapContents = false;
            this.flpPayMethod.Margin = new System.Windows.Forms.Padding(0, 4, 8, 0);
            this.flpPay.Controls.Add(this.flpPayMethod);

            this.lblPayMethod.Text = "طريقة الدفع:";
            this.lblPayMethod.AutoSize = true;
            this.lblPayMethod.Margin = new System.Windows.Forms.Padding(0, 6, 5, 0);
            this.lblPayMethod.Font = new System.Drawing.Font("Tahoma", 11F);

            this.cmbPayMethod.Width = 160;
            this.cmbPayMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPayMethod.Font = new System.Drawing.Font("Tahoma", 11F);
            this.cmbPayMethod.Items.AddRange(new object[] { "نقدي", "كريمي", "شبكة / بطاقة", "آجل", "محفظة جيب", "دفع مجزأ" });

            // flpPaidCash
            this.flpPaidCash.Controls.Add(this.lblPaidCash);
            this.flpPaidCash.Controls.Add(this.txtPaidCash);
            this.flpPaidCash.AutoSize = true;
            this.flpPaidCash.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpPaidCash.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flpPaidCash.WrapContents = false;
            this.flpPaidCash.Margin = new System.Windows.Forms.Padding(0, 4, 8, 0);
            this.flpPay.Controls.Add(this.flpPaidCash);

            this.lblPaidCash.Text = "المدفوع نقد:";
            this.lblPaidCash.AutoSize = true;
            this.lblPaidCash.Margin = new System.Windows.Forms.Padding(0, 6, 5, 0);
            this.lblPaidCash.Font = new System.Drawing.Font("Tahoma", 11F);

            this.txtPaidCash.Width = 95;
            this.txtPaidCash.Text = "0";
            this.txtPaidCash.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtPaidCash.Font = new System.Drawing.Font("Tahoma", 11F);
            this.txtPaidCash.TextChanged += new System.EventHandler(this.MoneyField_TextChanged);

            // flpPaidBank
            this.flpPaidBank.Controls.Add(this.lblPaidBank);
            this.flpPaidBank.Controls.Add(this.txtPaidBank);
            this.flpPaidBank.AutoSize = true;
            this.flpPaidBank.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpPaidBank.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flpPaidBank.WrapContents = false;
            this.flpPaidBank.Margin = new System.Windows.Forms.Padding(0, 4, 8, 0);
            this.flpPay.Controls.Add(this.flpPaidBank);

            this.lblPaidBank.Text = "المدفوع بنك:";
            this.lblPaidBank.AutoSize = true;
            this.lblPaidBank.Margin = new System.Windows.Forms.Padding(0, 6, 5, 0);
            this.lblPaidBank.Font = new System.Drawing.Font("Tahoma", 11F);

            this.txtPaidBank.Width = 95;
            this.txtPaidBank.Text = "0";
            this.txtPaidBank.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtPaidBank.Font = new System.Drawing.Font("Tahoma", 11F);
            this.txtPaidBank.TextChanged += new System.EventHandler(this.MoneyField_TextChanged);

            // flpPaidWallet
            this.flpPaidWallet.Controls.Add(this.lblPaidWallet);
            this.flpPaidWallet.Controls.Add(this.txtPaidWallet);
            this.flpPaidWallet.AutoSize = true;
            this.flpPaidWallet.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpPaidWallet.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flpPaidWallet.WrapContents = false;
            this.flpPaidWallet.Margin = new System.Windows.Forms.Padding(0, 4, 8, 0);
            this.flpPay.Controls.Add(this.flpPaidWallet);

            this.lblPaidWallet.Text = "المدفوع محفظة:";
            this.lblPaidWallet.AutoSize = true;
            this.lblPaidWallet.Margin = new System.Windows.Forms.Padding(0, 6, 5, 0);
            this.lblPaidWallet.Font = new System.Drawing.Font("Tahoma", 11F);

            this.txtPaidWallet.Width = 95;
            this.txtPaidWallet.Text = "0";
            this.txtPaidWallet.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtPaidWallet.Font = new System.Drawing.Font("Tahoma", 11F);
            this.txtPaidWallet.TextChanged += new System.EventHandler(this.MoneyField_TextChanged);

            // flpDiscount
            this.flpDiscount.Controls.Add(this.lblDiscount);
            this.flpDiscount.Controls.Add(this.numDiscount);
            this.flpDiscount.AutoSize = true;
            this.flpDiscount.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpDiscount.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flpDiscount.WrapContents = false;
            this.flpDiscount.Margin = new System.Windows.Forms.Padding(0, 4, 8, 0);
            this.flpPay.Controls.Add(this.flpDiscount);

            this.lblDiscount.Text = "خصم:";
            this.lblDiscount.AutoSize = true;
            this.lblDiscount.Margin = new System.Windows.Forms.Padding(0, 6, 5, 0);
            this.lblDiscount.Font = new System.Drawing.Font("Tahoma", 11F);

            this.numDiscount.Width = 95;
            this.numDiscount.DecimalPlaces = 2;
            this.numDiscount.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            this.numDiscount.Value = new decimal(new int[] { 0, 0, 0, 0 });
            this.numDiscount.Font = new System.Drawing.Font("Tahoma", 11F);
            this.numDiscount.ValueChanged += new System.EventHandler(this.MoneyField_TextChanged);

            // flpTax
            this.flpTax.Controls.Add(this.lblTax);
            this.flpTax.Controls.Add(this.numTax);
            this.flpTax.AutoSize = true;
            this.flpTax.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpTax.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flpTax.WrapContents = false;
            this.flpTax.Margin = new System.Windows.Forms.Padding(0, 4, 8, 0);
            this.flpPay.Controls.Add(this.flpTax);

            this.lblTax.Text = "ضريبة %:";
            this.lblTax.AutoSize = true;
            this.lblTax.Margin = new System.Windows.Forms.Padding(0, 6, 5, 0);
            this.lblTax.Font = new System.Drawing.Font("Tahoma", 11F);

            this.numTax.Width = 75;
            this.numTax.DecimalPlaces = 2;
            this.numTax.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
            this.numTax.Value = new decimal(new int[] { 5, 0, 0, 0 });
            this.numTax.Font = new System.Drawing.Font("Tahoma", 11F);
            this.numTax.ValueChanged += new System.EventHandler(this.MoneyField_TextChanged);

            // ---------- pnlTotals (الإجماليات) ----------
            this.pnlTotals.Width = 460;
            this.pnlTotals.Height = 215;
            this.pnlTotals.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.pnlTotals.Controls.Add(this.tblTotals);

            // tblTotals
            this.tblTotals.ColumnCount = 2;
            this.tblTotals.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tblTotals.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblTotals.RowCount = 6;
            this.tblTotals.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tblTotals.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tblTotals.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tblTotals.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tblTotals.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tblTotals.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tblTotals.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblTotals.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.tblTotals.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tblTotals.Controls.Add(this.lblSubTotal, 0, 0);
            this.tblTotals.Controls.Add(this.lblSubTotalVal, 1, 0);
            this.tblTotals.Controls.Add(this.lblDiscountTotal, 0, 1);
            this.tblTotals.Controls.Add(this.lblDiscountTotalVal, 1, 1);
            this.tblTotals.Controls.Add(this.lblNetTotal, 0, 2);
            this.tblTotals.Controls.Add(this.lblNetTotalVal, 1, 2);
            this.tblTotals.Controls.Add(this.lblPaidTotal, 0, 3);
            this.tblTotals.Controls.Add(this.lblPaidTotalVal, 1, 3);
            this.tblTotals.Controls.Add(this.lblRemaining, 0, 4);
            this.tblTotals.Controls.Add(this.lblRemainingVal, 1, 4);
            this.tblTotals.Controls.Add(this.lblDueToStore, 0, 5);
            this.tblTotals.Controls.Add(this.lblDueToStoreVal, 1, 5);

            this.lblSubTotal.Text = "المجموع الفرعي:";
            this.lblSubTotal.AutoSize = true;
            this.lblSubTotal.Font = new System.Drawing.Font("Tahoma", 11F);

            this.lblSubTotalVal.Text = "0.00";
            this.lblSubTotalVal.AutoSize = true;
            this.lblSubTotalVal.Font = new System.Drawing.Font("Tahoma", 11F);

            this.lblDiscountTotal.Text = "إجمالي الخصم:";
            this.lblDiscountTotal.AutoSize = true;
            this.lblDiscountTotal.Font = new System.Drawing.Font("Tahoma", 11F);

            this.lblDiscountTotalVal.Text = "0.00";
            this.lblDiscountTotalVal.AutoSize = true;
            this.lblDiscountTotalVal.Font = new System.Drawing.Font("Tahoma", 11F);

            this.lblNetTotal.Text = "الصافي النهائي:";
            this.lblNetTotal.AutoSize = true;
            this.lblNetTotal.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);

            this.lblNetTotalVal.Text = "0.00";
            this.lblNetTotalVal.AutoSize = true;
            this.lblNetTotalVal.Font = new System.Drawing.Font("Tahoma", 13F, System.Drawing.FontStyle.Bold);
            this.lblNetTotalVal.ForeColor = System.Drawing.Color.FromArgb(13, 110, 253);

            this.lblPaidTotal.Text = "المبلغ المدفوع:";
            this.lblPaidTotal.AutoSize = true;
            this.lblPaidTotal.Font = new System.Drawing.Font("Tahoma", 11F);

            this.lblPaidTotalVal.Text = "0.00";
            this.lblPaidTotalVal.AutoSize = true;
            this.lblPaidTotalVal.Font = new System.Drawing.Font("Tahoma", 11F);

            this.lblRemaining.Text = "الباقي:";
            this.lblRemaining.AutoSize = true;
            this.lblRemaining.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);

            this.lblRemainingVal.Text = "0.00";
            this.lblRemainingVal.AutoSize = true;
            this.lblRemainingVal.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);

            this.lblDueToStore.Text = "المستحق للمحل:";
            this.lblDueToStore.AutoSize = true;
            this.lblDueToStore.Font = new System.Drawing.Font("Tahoma", 11F);

            this.lblDueToStoreVal.Text = "0.00";
            this.lblDueToStoreVal.AutoSize = true;
            this.lblDueToStoreVal.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.lblDueToStoreVal.ForeColor = System.Drawing.Color.FromArgb(25, 135, 84);

            // ---------- btnSave (زر الحفظ والاعتماد) ----------
            this.btnSave.Text = "حفظ واعتماد";
            this.btnSave.Width = 200;
            this.btnSave.Height = 120;
            this.btnSave.Margin = new System.Windows.Forms.Padding(5, 45, 5, 5);
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(25, 135, 84);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold);
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            // ---------- Form ----------
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1200, 820);
            this.Controls.Add(this.dgvCart);
            this.Controls.Add(this.pnlCheckout);
            this.Controls.Add(this.pnlTop);
            this.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(1000, 700);
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "نقطة البيع (POS) — نموذج تجريبي";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmPOS_KeyDown);

            this.pnlTop.ResumeLayout(false);
            this.flpTop.ResumeLayout(false);
            this.flpTop.PerformLayout();
            this.flpBarcode.ResumeLayout(false);
            this.flpBarcode.PerformLayout();
            this.flpCurrency.ResumeLayout(false);
            this.flpCurrency.PerformLayout();
            this.flpCustomer.ResumeLayout(false);
            this.flpCustomer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCart)).EndInit();
            this.pnlCheckout.ResumeLayout(false);
            this.flpCheckout.ResumeLayout(false);
            this.flpCheckout.PerformLayout();
            this.pnlPayment.ResumeLayout(false);
            this.flpPay.ResumeLayout(false);
            this.flpPay.PerformLayout();
            this.flpPayMethod.ResumeLayout(false);
            this.flpPayMethod.PerformLayout();
            this.flpPaidCash.ResumeLayout(false);
            this.flpPaidCash.PerformLayout();
            this.flpPaidBank.ResumeLayout(false);
            this.flpPaidBank.PerformLayout();
            this.flpPaidWallet.ResumeLayout(false);
            this.flpPaidWallet.PerformLayout();
            this.flpDiscount.ResumeLayout(false);
            this.flpDiscount.PerformLayout();
            this.flpTax.ResumeLayout(false);
            this.flpTax.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDiscount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTax)).EndInit();
            this.pnlTotals.ResumeLayout(false);
            this.tblTotals.ResumeLayout(false);
            this.tblTotals.PerformLayout();
            this.ResumeLayout(false);
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
    }
}