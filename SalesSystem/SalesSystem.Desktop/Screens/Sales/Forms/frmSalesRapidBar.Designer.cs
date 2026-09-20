namespace SalesSystem.Desktop.Screens.Sales.Forms
{
    partial class frmSalesRapidBar
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            components?.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlTitle = new System.Windows.Forms.Panel();
            this.lblMode = new System.Windows.Forms.Label();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.flpHeader = new System.Windows.Forms.FlowLayoutPanel();
            this.flpInvoiceNo = new System.Windows.Forms.FlowLayoutPanel();
            this.lblInvoiceNo = new System.Windows.Forms.Label();
            this.txtInvoiceNo = new System.Windows.Forms.TextBox();
            this.flpCustomer = new System.Windows.Forms.FlowLayoutPanel();
            this.lblCustomer = new System.Windows.Forms.Label();
            this.cmbCustomer = new System.Windows.Forms.ComboBox();
            this.lblBalanceBadge = new System.Windows.Forms.Label();
            this.pnlRapidBar = new System.Windows.Forms.Panel();
            this.flpRapidBar = new System.Windows.Forms.FlowLayoutPanel();
            this.flpSearch = new System.Windows.Forms.FlowLayoutPanel();
            this.lblSearch = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnPickProduct = new System.Windows.Forms.Button();
            this.flpQty = new System.Windows.Forms.FlowLayoutPanel();
            this.lblQty = new System.Windows.Forms.Label();
            this.numQty = new System.Windows.Forms.NumericUpDown();
            this.flpUnit = new System.Windows.Forms.FlowLayoutPanel();
            this.lblUnit = new System.Windows.Forms.Label();
            this.cmbUnit = new System.Windows.Forms.ComboBox();
            this.flpPrice = new System.Windows.Forms.FlowLayoutPanel();
            this.lblPrice = new System.Windows.Forms.Label();
            this.numPrice = new System.Windows.Forms.NumericUpDown();
            this.btnInsert = new System.Windows.Forms.Button();
            this.pnlGrid = new System.Windows.Forms.Panel();
            this.lblGridNote = new System.Windows.Forms.Label();
            this.dgvCart = new System.Windows.Forms.DataGridView();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.flpFooter = new System.Windows.Forms.FlowLayoutPanel();
            this.tblTotals = new System.Windows.Forms.TableLayoutPanel();
            this.lblSubTotal = new System.Windows.Forms.Label();
            this.lblSubTotalVal = new System.Windows.Forms.Label();
            this.lblDiscountTotal = new System.Windows.Forms.Label();
            this.lblDiscountTotalVal = new System.Windows.Forms.Label();
            this.lblNetTotal = new System.Windows.Forms.Label();
            this.lblNetTotalVal = new System.Windows.Forms.Label();
            this.flpDiscount = new System.Windows.Forms.FlowLayoutPanel();
            this.lblDiscount = new System.Windows.Forms.Label();
            this.numDiscount = new System.Windows.Forms.NumericUpDown();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lstMatches = new System.Windows.Forms.ListBox();

            this.pnlTitle.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.flpHeader.SuspendLayout();
            this.flpInvoiceNo.SuspendLayout();
            this.flpCustomer.SuspendLayout();
            this.pnlRapidBar.SuspendLayout();
            this.flpRapidBar.SuspendLayout();
            this.flpSearch.SuspendLayout();
            this.flpQty.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numQty)).BeginInit();
            this.flpUnit.SuspendLayout();
            this.flpPrice.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPrice)).BeginInit();
            this.pnlGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCart)).BeginInit();
            this.pnlFooter.SuspendLayout();
            this.flpFooter.SuspendLayout();
            this.tblTotals.SuspendLayout();
            this.flpDiscount.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDiscount)).BeginInit();
            this.SuspendLayout();

            // ---------- pnlTitle (شريط العنوان) ----------
            this.pnlTitle.Controls.Add(this.lblMode);
            this.pnlTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTitle.Height = 46;
            this.pnlTitle.BackColor = System.Drawing.Color.FromArgb(13, 110, 253);

            // lblMode
            this.lblMode.Text = "شريط الإدخال السريع — فاتورة بيع";
            this.lblMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblMode.ForeColor = System.Drawing.Color.White;
            this.lblMode.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);

            // ---------- pnlHeader (رقم الفاتورة + العميل) ----------
            this.pnlHeader.Controls.Add(this.flpHeader);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Height = 48;
            this.pnlHeader.Padding = new System.Windows.Forms.Padding(10, 6, 10, 6);
            this.pnlHeader.BackColor = System.Drawing.Color.White;
            this.pnlHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            // flpHeader
            this.flpHeader.Controls.Add(this.flpInvoiceNo);
            this.flpHeader.Controls.Add(this.flpCustomer);
            this.flpHeader.Controls.Add(this.lblBalanceBadge);
            this.flpHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpHeader.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flpHeader.WrapContents = false;

            // flpInvoiceNo
            this.flpInvoiceNo.Controls.Add(this.lblInvoiceNo);
            this.flpInvoiceNo.Controls.Add(this.txtInvoiceNo);
            this.flpInvoiceNo.AutoSize = true;
            this.flpInvoiceNo.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpInvoiceNo.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flpInvoiceNo.WrapContents = false;
            this.flpInvoiceNo.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);

            this.lblInvoiceNo.Text = "رقم الفاتورة:";
            this.lblInvoiceNo.AutoSize = true;
            this.lblInvoiceNo.Margin = new System.Windows.Forms.Padding(0, 7, 5, 0);
            this.lblInvoiceNo.Font = new System.Drawing.Font("Tahoma", 11F);

            this.txtInvoiceNo.Width = 130;
            this.txtInvoiceNo.ReadOnly = true;
            this.txtInvoiceNo.BackColor = System.Drawing.Color.FromArgb(233, 236, 239);
            this.txtInvoiceNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtInvoiceNo.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);

            // flpCustomer
            this.flpCustomer.Controls.Add(this.lblCustomer);
            this.flpCustomer.Controls.Add(this.cmbCustomer);
            this.flpCustomer.AutoSize = true;
            this.flpCustomer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpCustomer.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flpCustomer.WrapContents = false;
            this.flpCustomer.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);

            this.lblCustomer.Text = "العميل:";
            this.lblCustomer.AutoSize = true;
            this.lblCustomer.Margin = new System.Windows.Forms.Padding(0, 7, 5, 0);
            this.lblCustomer.Font = new System.Drawing.Font("Tahoma", 11F);

            this.cmbCustomer.Width = 220;
            this.cmbCustomer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCustomer.Font = new System.Drawing.Font("Tahoma", 11F);
            this.cmbCustomer.SelectedIndexChanged += new System.EventHandler(this.cmbCustomer_SelectedIndexChanged);

            // lblBalanceBadge (شارة الرصيد)
            this.lblBalanceBadge.Text = "حساب خالص";
            this.lblBalanceBadge.Width = 170;
            this.lblBalanceBadge.Height = 32;
            this.lblBalanceBadge.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.lblBalanceBadge.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblBalanceBadge.BackColor = System.Drawing.Color.FromArgb(108, 117, 125);
            this.lblBalanceBadge.ForeColor = System.Drawing.Color.White;
            this.lblBalanceBadge.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.lblBalanceBadge.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            // ---------- pnlRapidBar (شريط الإدخال السريع) ----------
            this.pnlRapidBar.Controls.Add(this.flpRapidBar);
            this.pnlRapidBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlRapidBar.Height = 104;
            this.pnlRapidBar.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);
            this.pnlRapidBar.BackColor = System.Drawing.Color.FromArgb(230, 242, 255);
            this.pnlRapidBar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            // flpRapidBar
            this.flpRapidBar.Controls.Add(this.flpSearch);
            this.flpRapidBar.Controls.Add(this.flpQty);
            this.flpRapidBar.Controls.Add(this.flpUnit);
            this.flpRapidBar.Controls.Add(this.flpPrice);
            this.flpRapidBar.Controls.Add(this.btnInsert);
            this.flpRapidBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpRapidBar.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flpRapidBar.WrapContents = true;

            // flpSearch (بحث + اختيار منتج)
            this.flpSearch.Controls.Add(this.lblSearch);
            this.flpSearch.Controls.Add(this.txtSearch);
            this.flpSearch.Controls.Add(this.btnPickProduct);
            this.flpSearch.AutoSize = true;
            this.flpSearch.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpSearch.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flpSearch.WrapContents = false;
            this.flpSearch.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);

            this.lblSearch.Text = "بحث بالباركود / الاسم / الكود:";
            this.lblSearch.AutoSize = true;
            this.lblSearch.Margin = new System.Windows.Forms.Padding(0, 8, 5, 0);
            this.lblSearch.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);

            this.txtSearch.Width = 330;
            this.txtSearch.Height = 34;
            this.txtSearch.Font = new System.Drawing.Font("Tahoma", 13F);
            this.txtSearch.BackColor = System.Drawing.Color.FromArgb(255, 251, 214);
            this.txtSearch.PlaceholderText = "اكتب أو امسح الباركود ثم Enter...";
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            this.txtSearch.Leave += new System.EventHandler(this.txtSearch_Leave);

            // btnPickProduct
            this.btnPickProduct.Text = "اختيار منتج [F3]";
            this.btnPickProduct.Width = 150;
            this.btnPickProduct.Height = 34;
            this.btnPickProduct.Margin = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.btnPickProduct.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPickProduct.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(13, 110, 253);
            this.btnPickProduct.FlatAppearance.BorderSize = 1;
            this.btnPickProduct.BackColor = System.Drawing.Color.White;
            this.btnPickProduct.ForeColor = System.Drawing.Color.FromArgb(13, 110, 253);
            this.btnPickProduct.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.btnPickProduct.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPickProduct.Click += new System.EventHandler(this.btnPickProduct_Click);

            // flpQty
            this.flpQty.Controls.Add(this.lblQty);
            this.flpQty.Controls.Add(this.numQty);
            this.flpQty.AutoSize = true;
            this.flpQty.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpQty.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flpQty.WrapContents = false;
            this.flpQty.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);

            this.lblQty.Text = "الكمية:";
            this.lblQty.AutoSize = true;
            this.lblQty.Margin = new System.Windows.Forms.Padding(0, 8, 5, 0);
            this.lblQty.Font = new System.Drawing.Font("Tahoma", 11F);

            this.numQty.Width = 90;
            this.numQty.Height = 34;
            this.numQty.DecimalPlaces = 3;
            this.numQty.Maximum = new decimal(new int[] { 99999, 0, 0, 0 });
            this.numQty.Value = new decimal(new int[] { 1, 0, 0, 0 });
            this.numQty.Font = new System.Drawing.Font("Tahoma", 12F);
            this.numQty.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numQty_KeyDown);

            // flpUnit
            this.flpUnit.Controls.Add(this.lblUnit);
            this.flpUnit.Controls.Add(this.cmbUnit);
            this.flpUnit.AutoSize = true;
            this.flpUnit.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpUnit.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flpUnit.WrapContents = false;
            this.flpUnit.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);

            this.lblUnit.Text = "الوحدة:";
            this.lblUnit.AutoSize = true;
            this.lblUnit.Margin = new System.Windows.Forms.Padding(0, 8, 5, 0);
            this.lblUnit.Font = new System.Drawing.Font("Tahoma", 11F);

            this.cmbUnit.Width = 110;
            this.cmbUnit.Height = 34;
            this.cmbUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUnit.Font = new System.Drawing.Font("Tahoma", 12F);

            // flpPrice
            this.flpPrice.Controls.Add(this.lblPrice);
            this.flpPrice.Controls.Add(this.numPrice);
            this.flpPrice.AutoSize = true;
            this.flpPrice.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpPrice.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flpPrice.WrapContents = false;
            this.flpPrice.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);

            this.lblPrice.Text = "سعر الوحدة:";
            this.lblPrice.AutoSize = true;
            this.lblPrice.Margin = new System.Windows.Forms.Padding(0, 8, 5, 0);
            this.lblPrice.Font = new System.Drawing.Font("Tahoma", 11F);

            this.numPrice.Width = 130;
            this.numPrice.Height = 34;
            this.numPrice.DecimalPlaces = 2;
            this.numPrice.Maximum = new decimal(new int[] { 9999999, 0, 0, 0 });
            this.numPrice.Font = new System.Drawing.Font("Tahoma", 12F);
            this.numPrice.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numPrice_KeyDown);

            // btnInsert
            this.btnInsert.Text = "إدراج";
            this.btnInsert.Width = 110;
            this.btnInsert.Height = 40;
            this.btnInsert.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.btnInsert.BackColor = System.Drawing.Color.FromArgb(25, 135, 84);
            this.btnInsert.ForeColor = System.Drawing.Color.White;
            this.btnInsert.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInsert.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnInsert.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);

            // ---------- pnlGrid (الجدول للقراءة فقط) ----------
            this.pnlGrid.Controls.Add(this.dgvCart);
            this.pnlGrid.Controls.Add(this.lblGridNote);
            this.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGrid.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlGrid.Padding = new System.Windows.Forms.Padding(4, 0, 4, 4);

            // lblGridNote
            this.lblGridNote.Text = "الجدول للقراءة فقط — الإدخال من الشريط العلوي";
            this.lblGridNote.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblGridNote.Height = 26;
            this.lblGridNote.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblGridNote.BackColor = System.Drawing.Color.FromArgb(241, 243, 245);
            this.lblGridNote.ForeColor = System.Drawing.Color.FromArgb(108, 117, 125);
            this.lblGridNote.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Italic);

            // dgvCart
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
            this.dgvCart.RowTemplate.Height = 32;
            this.dgvCart.EnableHeadersVisualStyles = false;
            this.dgvCart.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(248, 249, 250);
            this.dgvCart.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.dgvCart.DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 11F);
            this.dgvCart.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(242, 242, 242);
            this.dgvCart.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "ColNo", HeaderText = "#", DataPropertyName = "LineNo", Width = 45 });
            this.dgvCart.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "ColName", HeaderText = "اسم الصنف", DataPropertyName = "ProductName", Width = 360 });
            this.dgvCart.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "ColUnit", HeaderText = "الوحدة", DataPropertyName = "UnitName", Width = 95 });
            this.dgvCart.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "ColQty", HeaderText = "الكمية", DataPropertyName = "Quantity", Width = 110, DefaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle { Format = "N3", Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight } });
            this.dgvCart.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "ColPrice", HeaderText = "السعر", DataPropertyName = "UnitPrice", Width = 130, DefaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle { Format = "N2", Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight } });
            this.dgvCart.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "ColTotal", HeaderText = "الإجمالي", DataPropertyName = "LineTotal", Width = 150, DefaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle { Format = "N2", Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight, ForeColor = System.Drawing.Color.FromArgb(13, 110, 253), Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold) } });

            // ---------- pnlFooter (الإجماليات + الأزرار) ----------
            this.pnlFooter.Controls.Add(this.flpFooter);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Height = 106;
            this.pnlFooter.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);
            this.pnlFooter.BackColor = System.Drawing.Color.White;
            this.pnlFooter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            // flpFooter
            this.flpFooter.Controls.Add(this.tblTotals);
            this.flpFooter.Controls.Add(this.flpDiscount);
            this.flpFooter.Controls.Add(this.btnSave);
            this.flpFooter.Controls.Add(this.btnClose);
            this.flpFooter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpFooter.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flpFooter.WrapContents = false;

            // tblTotals
            this.tblTotals.ColumnCount = 2;
            this.tblTotals.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tblTotals.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblTotals.RowCount = 3;
            this.tblTotals.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tblTotals.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tblTotals.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tblTotals.Width = 380;
            this.tblTotals.Height = 90;
            this.tblTotals.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.tblTotals.Padding = new System.Windows.Forms.Padding(6, 2, 6, 2);
            this.tblTotals.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tblTotals.Controls.Add(this.lblSubTotal, 0, 0);
            this.tblTotals.Controls.Add(this.lblSubTotalVal, 1, 0);
            this.tblTotals.Controls.Add(this.lblDiscountTotal, 0, 1);
            this.tblTotals.Controls.Add(this.lblDiscountTotalVal, 1, 1);
            this.tblTotals.Controls.Add(this.lblNetTotal, 0, 2);
            this.tblTotals.Controls.Add(this.lblNetTotalVal, 1, 2);

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

            // flpDiscount
            this.flpDiscount.Controls.Add(this.lblDiscount);
            this.flpDiscount.Controls.Add(this.numDiscount);
            this.flpDiscount.AutoSize = true;
            this.flpDiscount.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpDiscount.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flpDiscount.WrapContents = false;
            this.flpDiscount.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);

            this.lblDiscount.Text = "خصم الفاتورة:";
            this.lblDiscount.AutoSize = true;
            this.lblDiscount.Margin = new System.Windows.Forms.Padding(0, 8, 5, 0);
            this.lblDiscount.Font = new System.Drawing.Font("Tahoma", 11F);

            this.numDiscount.Width = 120;
            this.numDiscount.Height = 32;
            this.numDiscount.DecimalPlaces = 2;
            this.numDiscount.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
            this.numDiscount.Font = new System.Drawing.Font("Tahoma", 11F);
            this.numDiscount.ValueChanged += new System.EventHandler(this.numDiscount_ValueChanged);

            // btnSave
            this.btnSave.Text = "حفظ واعتماد";
            this.btnSave.Width = 180;
            this.btnSave.Height = 76;
            this.btnSave.Margin = new System.Windows.Forms.Padding(8, 4, 8, 4);
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(25, 135, 84);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            // btnClose
            this.btnClose.Text = "إغلاق";
            this.btnClose.Width = 100;
            this.btnClose.Height = 76;
            this.btnClose.Margin = new System.Windows.Forms.Padding(8, 4, 8, 4);
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(108, 117, 125);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);

            // ---------- lstMatches (قائمة نتائج البحث السريع — تظهر فوق الجدول) ----------
            this.lstMatches.IntegralHeight = false;
            this.lstMatches.Height = 112;
            this.lstMatches.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstMatches.BackColor = System.Drawing.Color.White;
            this.lstMatches.Font = new System.Drawing.Font("Tahoma", 11F);
            this.lstMatches.Visible = false;
            this.lstMatches.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstMatches_KeyDown);
            this.lstMatches.DoubleClick += new System.EventHandler(this.lstMatches_DoubleClick);
            this.lstMatches.Leave += new System.EventHandler(this.lstMatches_Leave);

            // ---------- Form ----------
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1000, 640);
            this.Controls.Add(this.pnlGrid);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.pnlRapidBar);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.pnlTitle);
            this.Controls.Add(this.lstMatches);
            this.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(940, 600);
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "شريط الإدخال السريع — فاتورة بيع";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmSalesRapidBar_KeyDown);

            this.pnlTitle.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            this.flpHeader.ResumeLayout(false);
            this.flpHeader.PerformLayout();
            this.flpInvoiceNo.ResumeLayout(false);
            this.flpInvoiceNo.PerformLayout();
            this.flpCustomer.ResumeLayout(false);
            this.flpCustomer.PerformLayout();
            this.pnlRapidBar.ResumeLayout(false);
            this.flpRapidBar.ResumeLayout(false);
            this.flpRapidBar.PerformLayout();
            this.flpSearch.ResumeLayout(false);
            this.flpSearch.PerformLayout();
            this.flpQty.ResumeLayout(false);
            this.flpQty.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numQty)).EndInit();
            this.flpUnit.ResumeLayout(false);
            this.flpUnit.PerformLayout();
            this.flpPrice.ResumeLayout(false);
            this.flpPrice.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPrice)).EndInit();
            this.pnlGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCart)).EndInit();
            this.pnlFooter.ResumeLayout(false);
            this.flpFooter.ResumeLayout(false);
            this.flpFooter.PerformLayout();
            this.tblTotals.ResumeLayout(false);
            this.tblTotals.PerformLayout();
            this.flpDiscount.ResumeLayout(false);
            this.flpDiscount.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDiscount)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlTitle, pnlHeader, pnlRapidBar, pnlGrid, pnlFooter;
        private System.Windows.Forms.FlowLayoutPanel flpHeader, flpRapidBar, flpInvoiceNo, flpCustomer, flpSearch, flpQty, flpUnit, flpPrice, flpDiscount, flpFooter;
        private System.Windows.Forms.Label lblMode, lblInvoiceNo, lblCustomer, lblBalanceBadge, lblSearch, lblQty, lblUnit, lblPrice, lblGridNote, lblDiscount, lblSubTotal, lblSubTotalVal, lblDiscountTotal, lblDiscountTotalVal, lblNetTotal, lblNetTotalVal;
        private System.Windows.Forms.TextBox txtInvoiceNo, txtSearch;
        private System.Windows.Forms.ComboBox cmbCustomer, cmbUnit;
        private System.Windows.Forms.NumericUpDown numQty, numPrice, numDiscount;
        private System.Windows.Forms.Button btnPickProduct, btnInsert, btnSave, btnClose;
        private System.Windows.Forms.DataGridView dgvCart;
        private System.Windows.Forms.TableLayoutPanel tblTotals;
        private System.Windows.Forms.ListBox lstMatches;
    }
}