namespace SalesSystem.Desktop.Screens.Purchases.Forms
{
    partial class frmPurchaseInvoice
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.lblMode = new System.Windows.Forms.Label();
            this.gbToolbar = new System.Windows.Forms.GroupBox();
            this.pnlToolbar = new System.Windows.Forms.FlowLayoutPanel();
            this.btnSaveDraft = new System.Windows.Forms.Button();
            this.btnPost = new System.Windows.Forms.Button();
            this.btnCancelInvoice = new System.Windows.Forms.Button();
            this.btnPrintA4 = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.gbInvoiceInfo = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblInvoiceNoLabel = new System.Windows.Forms.Label();
            this.lblInvoiceNo = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblSupplier = new System.Windows.Forms.Label();
            this.cmbSupplier = new System.Windows.Forms.ComboBox();
            this.lblWarehouse = new System.Windows.Forms.Label();
            this.cmbWarehouse = new System.Windows.Forms.ComboBox();
            this.lblPaymentType = new System.Windows.Forms.Label();
            this.cmbPaymentType = new System.Windows.Forms.ComboBox();
            this.gbEntry = new System.Windows.Forms.GroupBox();
            this.pnlEntry = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAddItem = new System.Windows.Forms.Button();
            this.btnRemoveItem = new System.Windows.Forms.Button();
            this.gbItems = new System.Windows.Forms.GroupBox();
            this.dgvItems = new System.Windows.Forms.DataGridView();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.gbTotals = new System.Windows.Forms.GroupBox();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.lblSubTotal = new System.Windows.Forms.Label();
            this.lblSubTotalVal = new System.Windows.Forms.Label();
            this.lblInvoiceDiscount = new System.Windows.Forms.Label();
            this.numInvoiceDiscount = new System.Windows.Forms.NumericUpDown();
            this.lblTotal = new System.Windows.Forms.Label();
            this.lblTotalVal = new System.Windows.Forms.Label();
            this.lblPaid = new System.Windows.Forms.Label();
            this.txtPaid = new SalesSystem.Desktop.Controls.Common.MoneyTextBox();
            this.lblDue = new System.Windows.Forms.Label();
            this.lblDueVal = new System.Windows.Forms.Label();

            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.gbToolbar.SuspendLayout();
            this.pnlToolbar.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.gbInvoiceInfo.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.gbEntry.SuspendLayout();
            this.pnlEntry.SuspendLayout();
            this.gbItems.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).BeginInit();
            this.pnlFooter.SuspendLayout();
            this.gbTotals.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numInvoiceDiscount)).BeginInit();
            this.SuspendLayout();
            // 
            // errorProvider
            // 
            this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errorProvider.ContainerControl = this;
            this.errorProvider.RightToLeft = true;
            // 
            // lblMode — شريط العنوان (RULE-N44 / RULE-N53). القيم حرفية مطابقة لثوابت الثيم الأخضر.
            // 
            this.lblMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(165)))), ((int)(((byte)(103)))));
            this.lblMode.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblMode.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.lblMode.ForeColor = System.Drawing.Color.White;
            this.lblMode.Name = "lblMode";
            this.lblMode.Size = new System.Drawing.Size(1150, 46);
            this.lblMode.TabIndex = 0;
            this.lblMode.Text = "(( فاتورة مشتريات جديدة ))";
            this.lblMode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gbToolbar — أوامر الفاتورة (RULE-N135 / RULE-N136)
            // 
            this.gbToolbar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(245)))), ((int)(((byte)(235)))));
            this.gbToolbar.Controls.Add(this.pnlToolbar);
            this.gbToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbToolbar.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.gbToolbar.Name = "gbToolbar";
            this.gbToolbar.Padding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.gbToolbar.Size = new System.Drawing.Size(1150, 70);
            this.gbToolbar.TabIndex = 1;
            this.gbToolbar.TabStop = false;
            this.gbToolbar.Text = "أوامر الفاتورة";
            // 
            // pnlToolbar
            // 
            this.pnlToolbar.Controls.Add(this.btnSaveDraft);
            this.pnlToolbar.Controls.Add(this.btnPost);
            this.pnlToolbar.Controls.Add(this.btnCancelInvoice);
            this.pnlToolbar.Controls.Add(this.btnPrintA4);
            this.pnlToolbar.Controls.Add(this.btnClose);
            this.pnlToolbar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlToolbar.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.pnlToolbar.Name = "pnlToolbar";
            this.pnlToolbar.TabIndex = 0;
            this.pnlToolbar.WrapContents = false;
            // 
            // btnSaveDraft
            // 
            this.btnSaveDraft.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(110)))), ((int)(((byte)(253)))));
            this.btnSaveDraft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveDraft.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btnSaveDraft.ForeColor = System.Drawing.Color.White;
            this.btnSaveDraft.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnSaveDraft.Name = "btnSaveDraft";
            this.btnSaveDraft.Size = new System.Drawing.Size(140, 36);
            this.btnSaveDraft.TabIndex = 0;
            this.btnSaveDraft.Text = "حفظ مسودة (F3)";
            this.btnSaveDraft.UseVisualStyleBackColor = false;
            this.toolTip.SetToolTip(this.btnSaveDraft, "حفظ الفاتورة كمسودة — F3");
            this.btnSaveDraft.Click += new System.EventHandler(this.btnSaveDraft_Click);
            // 
            // btnPost
            // 
            this.btnPost.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(135)))), ((int)(((byte)(84)))));
            this.btnPost.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPost.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btnPost.ForeColor = System.Drawing.Color.White;
            this.btnPost.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnPost.Name = "btnPost";
            this.btnPost.Size = new System.Drawing.Size(140, 36);
            this.btnPost.TabIndex = 1;
            this.btnPost.Text = "ترحيل (F4)";
            this.btnPost.UseVisualStyleBackColor = false;
            this.toolTip.SetToolTip(this.btnPost, "ترحيل الفاتورة وزيادة المخزون — F4");
            this.btnPost.Click += new System.EventHandler(this.btnPost_Click);
            // 
            // btnCancelInvoice
            // 
            this.btnCancelInvoice.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnCancelInvoice.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelInvoice.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btnCancelInvoice.ForeColor = System.Drawing.Color.White;
            this.btnCancelInvoice.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnCancelInvoice.Name = "btnCancelInvoice";
            this.btnCancelInvoice.Size = new System.Drawing.Size(150, 36);
            this.btnCancelInvoice.TabIndex = 2;
            this.btnCancelInvoice.Text = "إلغاء الفاتورة";
            this.btnCancelInvoice.UseVisualStyleBackColor = false;
            this.toolTip.SetToolTip(this.btnCancelInvoice, "إلغاء الفاتورة المرحّلة وعكس أثر المخزون");
            this.btnCancelInvoice.Click += new System.EventHandler(this.btnCancelInvoice_Click);
            // 
            // btnPrintA4 — مُعلَن في المصمّم (RULE-N145) بدل إنشائه وقت التشغيل
            // 
            this.btnPrintA4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(162)))), ((int)(((byte)(184)))));
            this.btnPrintA4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrintA4.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btnPrintA4.ForeColor = System.Drawing.Color.White;
            this.btnPrintA4.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnPrintA4.Name = "btnPrintA4";
            this.btnPrintA4.Size = new System.Drawing.Size(120, 36);
            this.btnPrintA4.TabIndex = 3;
            this.btnPrintA4.Text = "طباعة A4";
            this.btnPrintA4.UseVisualStyleBackColor = false;
            this.toolTip.SetToolTip(this.btnPrintA4, "معاينة وطباعة الفاتورة A4");
            this.btnPrintA4.Click += new System.EventHandler(this.btnPrintA4_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 36);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "إغلاق (Esc)";
            this.btnClose.UseVisualStyleBackColor = false;
            this.toolTip.SetToolTip(this.btnClose, "إغلاق الشاشة — Esc");
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // pnlHeader — حاوية «بيانات الفاتورة» (يبقى الاسم لأن UpdateUIState يعطّل pnlHeader)
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(240)))), ((int)(((byte)(235)))));
            this.pnlHeader.Controls.Add(this.gbInvoiceInfo);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Padding = new System.Windows.Forms.Padding(8, 2, 8, 6);
            this.pnlHeader.Size = new System.Drawing.Size(1150, 126);
            this.pnlHeader.TabIndex = 2;
            // 
            // gbInvoiceInfo
            // 
            this.gbInvoiceInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(245)))), ((int)(((byte)(235)))));
            this.gbInvoiceInfo.Controls.Add(this.tableLayoutPanel1);
            this.gbInvoiceInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbInvoiceInfo.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.gbInvoiceInfo.Name = "gbInvoiceInfo";
            this.gbInvoiceInfo.Padding = new System.Windows.Forms.Padding(8, 4, 8, 6);
            this.gbInvoiceInfo.TabIndex = 0;
            this.gbInvoiceInfo.TabStop = false;
            this.gbInvoiceInfo.Text = "بيانات الفاتورة";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 6;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34F));
            this.tableLayoutPanel1.Controls.Add(this.lblInvoiceNoLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblInvoiceNo, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblDate, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.dtpDate, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblStatus, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblSupplier, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.cmbSupplier, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblWarehouse, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.cmbWarehouse, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblPaymentType, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.cmbPaymentType, 5, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            // صف الضريبة أُفرغ (قرار المالك: MVP V1 بلا ضريبة على مستندات الشراء) — الارتفاع صفر بلا تغيير RowCount.
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 0F));
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lblInvoiceNoLabel
            // 
            this.lblInvoiceNoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblInvoiceNoLabel.AutoSize = true;
            this.lblInvoiceNoLabel.Location = new System.Drawing.Point(3, 8);
            this.lblInvoiceNoLabel.Name = "lblInvoiceNoLabel";
            this.lblInvoiceNoLabel.Size = new System.Drawing.Size(104, 24);
            this.lblInvoiceNoLabel.TabIndex = 0;
            this.lblInvoiceNoLabel.Text = "رقم الفاتورة:";
            this.lblInvoiceNoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblInvoiceNo — قيمة للقراءة فقط (ليست حقلاً)
            // 
            this.lblInvoiceNo.BackColor = System.Drawing.Color.White;
            this.lblInvoiceNo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblInvoiceNo.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblInvoiceNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(82)))), ((int)(((byte)(52)))));
            this.lblInvoiceNo.Location = new System.Drawing.Point(113, 0);
            this.lblInvoiceNo.Name = "lblInvoiceNo";
            this.lblInvoiceNo.Padding = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblInvoiceNo.Size = new System.Drawing.Size(300, 40);
            this.lblInvoiceNo.TabIndex = 1;
            this.lblInvoiceNo.Text = "جديد";
            this.lblInvoiceNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Location = new System.Drawing.Point(419, 8);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(64, 24);
            this.lblDate.TabIndex = 2;
            this.lblDate.Text = "التاريخ:";
            this.lblDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dtpDate
            // 
            this.dtpDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpDate.Font = new System.Drawing.Font("Tahoma", 10F);
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDate.Location = new System.Drawing.Point(525, 5);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.BackColor = System.Drawing.Color.White;
            this.dtpDate.Size = new System.Drawing.Size(300, 32);
            this.dtpDate.TabIndex = 3;
            // 
            // lblStatus
            // 
            this.lblStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(110)))), ((int)(((byte)(253)))));
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatus.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblStatus.ForeColor = System.Drawing.Color.White;
            this.lblStatus.Location = new System.Drawing.Point(831, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(300, 40);
            this.lblStatus.TabIndex = 4;
            this.lblStatus.Text = "جديد";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSupplier
            // 
            this.lblSupplier.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSupplier.AutoSize = true;
            this.lblSupplier.Location = new System.Drawing.Point(3, 50);
            this.lblSupplier.Name = "lblSupplier";
            this.lblSupplier.Size = new System.Drawing.Size(104, 24);
            this.lblSupplier.TabIndex = 5;
            this.lblSupplier.Text = "المورد:";
            this.lblSupplier.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbSupplier
            // 
            this.cmbSupplier.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbSupplier.BackColor = System.Drawing.Color.White;
            this.cmbSupplier.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSupplier.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbSupplier.Font = new System.Drawing.Font("Tahoma", 10F);
            this.cmbSupplier.Location = new System.Drawing.Point(113, 46);
            this.cmbSupplier.Name = "cmbSupplier";
            this.cmbSupplier.Size = new System.Drawing.Size(300, 32);
            this.cmbSupplier.TabIndex = 6;
            // 
            // lblWarehouse
            // 
            this.lblWarehouse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblWarehouse.AutoSize = true;
            this.lblWarehouse.Location = new System.Drawing.Point(419, 50);
            this.lblWarehouse.Name = "lblWarehouse";
            this.lblWarehouse.Size = new System.Drawing.Size(94, 24);
            this.lblWarehouse.TabIndex = 7;
            this.lblWarehouse.Text = "المستودع:";
            this.lblWarehouse.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbWarehouse
            // 
            this.cmbWarehouse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbWarehouse.BackColor = System.Drawing.Color.White;
            this.cmbWarehouse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbWarehouse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbWarehouse.Font = new System.Drawing.Font("Tahoma", 10F);
            this.cmbWarehouse.Location = new System.Drawing.Point(525, 46);
            this.cmbWarehouse.Name = "cmbWarehouse";
            this.cmbWarehouse.Size = new System.Drawing.Size(300, 32);
            this.cmbWarehouse.TabIndex = 8;
            // 
            // lblPaymentType
            // 
            this.lblPaymentType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPaymentType.AutoSize = true;
            this.lblPaymentType.Location = new System.Drawing.Point(831, 50);
            this.lblPaymentType.Name = "lblPaymentType";
            this.lblPaymentType.Size = new System.Drawing.Size(104, 24);
            this.lblPaymentType.TabIndex = 9;
            this.lblPaymentType.Text = "نوع الدفع:";
            this.lblPaymentType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbPaymentType
            // 
            this.cmbPaymentType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbPaymentType.BackColor = System.Drawing.Color.White;
            this.cmbPaymentType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPaymentType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbPaymentType.Font = new System.Drawing.Font("Tahoma", 10F);
            this.cmbPaymentType.Items.AddRange(new object[] { "نقدي", "آجل", "مختلط" });
            this.cmbPaymentType.Location = new System.Drawing.Point(941, 46);
            this.cmbPaymentType.Name = "cmbPaymentType";
            this.cmbPaymentType.Size = new System.Drawing.Size(190, 32);
            this.cmbPaymentType.TabIndex = 10;
            // 
            // gbEntry — إضافة صنف (RULE-N136)
            // 
            this.gbEntry.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(245)))), ((int)(((byte)(235)))));
            this.gbEntry.Controls.Add(this.pnlEntry);
            this.gbEntry.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbEntry.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.gbEntry.Name = "gbEntry";
            this.gbEntry.Padding = new System.Windows.Forms.Padding(8, 4, 8, 4);
            this.gbEntry.Size = new System.Drawing.Size(1150, 74);
            this.gbEntry.TabIndex = 3;
            this.gbEntry.TabStop = false;
            this.gbEntry.Text = "إضافة صنف";
            // 
            // pnlEntry
            // 
            this.pnlEntry.Controls.Add(this.btnAddItem);
            this.pnlEntry.Controls.Add(this.btnRemoveItem);
            this.pnlEntry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlEntry.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.pnlEntry.Name = "pnlEntry";
            this.pnlEntry.TabIndex = 0;
            this.pnlEntry.WrapContents = false;
            // 
            // btnAddItem
            // 
            this.btnAddItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.btnAddItem.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(226)))), ((int)(((byte)(230)))));
            this.btnAddItem.FlatAppearance.BorderSize = 1;
            this.btnAddItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddItem.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btnAddItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this.btnAddItem.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnAddItem.Name = "btnAddItem";
            this.btnAddItem.Size = new System.Drawing.Size(160, 34);
            this.btnAddItem.TabIndex = 0;
            this.btnAddItem.Text = "إضافة صنف (F2)";
            this.btnAddItem.UseVisualStyleBackColor = false;
            this.toolTip.SetToolTip(this.btnAddItem, "إضافة سطر صنف جديد — F2");
            this.btnAddItem.Click += new System.EventHandler(this.btnAddItem_Click);
            // 
            // btnRemoveItem
            // 
            this.btnRemoveItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnRemoveItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveItem.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btnRemoveItem.ForeColor = System.Drawing.Color.White;
            this.btnRemoveItem.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnRemoveItem.Name = "btnRemoveItem";
            this.btnRemoveItem.Size = new System.Drawing.Size(160, 34);
            this.btnRemoveItem.TabIndex = 1;
            this.btnRemoveItem.Text = "حذف صنف (Del)";
            this.btnRemoveItem.UseVisualStyleBackColor = false;
            this.toolTip.SetToolTip(this.btnRemoveItem, "حذف السطر المحدد من الشبكة — Delete");
            this.btnRemoveItem.Click += new System.EventHandler(this.btnRemoveItem_Click);
            // 
            // gbItems — الأصناف (RULE-N136)
            // 
            this.gbItems.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(245)))), ((int)(((byte)(235)))));
            this.gbItems.Controls.Add(this.dgvItems);
            this.gbItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbItems.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.gbItems.Name = "gbItems";
            this.gbItems.Padding = new System.Windows.Forms.Padding(6, 4, 6, 6);
            this.gbItems.TabIndex = 4;
            this.gbItems.TabStop = false;
            this.gbItems.Text = "الأصناف";
            // 
            // dgvItems — شبكة إدخال السطور (RULE-N22a): تحرير داخل الخلية
            // القيم حرفية مطابقة لثوابت ثيم الشبكة الأخضر (RULE-N134)
            // 
            this.dgvItems.AllowUserToAddRows = true;
            this.dgvItems.AllowUserToDeleteRows = false;
            this.dgvItems.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(252)))), ((int)(((byte)(245)))));
            this.dgvItems.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvItems.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(240)))), ((int)(((byte)(220)))));
            this.dgvItems.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvItems.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvItems.ColumnHeadersDefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgvItems.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(165)))), ((int)(((byte)(103)))));
            this.dgvItems.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.dgvItems.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            this.dgvItems.ColumnHeadersDefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(165)))), ((int)(((byte)(103)))));
            this.dgvItems.ColumnHeadersDefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            this.dgvItems.ColumnHeadersHeight = 45;
            this.dgvItems.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.dgvItems.DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 10F);
            this.dgvItems.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this.dgvItems.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(180)))));
            this.dgvItems.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this.dgvItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvItems.EnableHeadersVisualStyles = false;
            this.dgvItems.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(225)))), ((int)(((byte)(205)))));
            this.dgvItems.Name = "dgvItems";
            this.dgvItems.RowHeadersVisible = false;
            this.dgvItems.RowTemplate.Height = 32;
            this.dgvItems.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.RowHeaderSelect;
            this.dgvItems.TabIndex = 0;
            // 
            // pnlFooter — حاوية «الإجماليات» (يبقى الاسم لأن UpdateUIState يعطّل pnlFooter)
            // 
            this.pnlFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(240)))), ((int)(((byte)(235)))));
            this.pnlFooter.Controls.Add(this.gbTotals);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Padding = new System.Windows.Forms.Padding(8, 2, 8, 6);
            this.pnlFooter.Size = new System.Drawing.Size(1150, 212);
            this.pnlFooter.TabIndex = 5;
            // 
            // gbTotals
            // 
            this.gbTotals.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(245)))), ((int)(((byte)(235)))));
            this.gbTotals.Controls.Add(this.txtNotes);
            this.gbTotals.Controls.Add(this.tableLayoutPanel2);
            this.gbTotals.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbTotals.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.gbTotals.Name = "gbTotals";
            this.gbTotals.Padding = new System.Windows.Forms.Padding(8, 4, 8, 6);
            this.gbTotals.TabIndex = 0;
            this.gbTotals.TabStop = false;
            this.gbTotals.Text = "الإجماليات";
            // 
            // txtNotes
            // 
            this.txtNotes.BackColor = System.Drawing.Color.White;
            this.txtNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNotes.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.PlaceholderText = "ملاحظات اختيارية...";
            this.txtNotes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtNotes.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.lblSubTotal, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblSubTotalVal, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblInvoiceDiscount, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.numInvoiceDiscount, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.lblTotal, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.lblTotalVal, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.lblPaid, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.txtPaid, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.lblDue, 0, 5);
            this.tableLayoutPanel2.Controls.Add(this.lblDueVal, 1, 5);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 6;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            // صف مبلغ الضريبة أُفرغ (بلا ضريبة على مستندات الشراء) — الارتفاع صفر بلا تغيير RowCount.
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 0F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(380, 140);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // lblSubTotal
            // 
            this.lblSubTotal.AutoSize = true;
            this.lblSubTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSubTotal.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblSubTotal.Location = new System.Drawing.Point(3, 0);
            this.lblSubTotal.Name = "lblSubTotal";
            this.lblSubTotal.Size = new System.Drawing.Size(144, 28);
            this.lblSubTotal.TabIndex = 0;
            this.lblSubTotal.Text = "المجموع الفرعي:";
            this.lblSubTotal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSubTotalVal
            // 
            this.lblSubTotalVal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSubTotalVal.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblSubTotalVal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(82)))), ((int)(((byte)(52)))));
            this.lblSubTotalVal.Location = new System.Drawing.Point(153, 0);
            this.lblSubTotalVal.Name = "lblSubTotalVal";
            this.lblSubTotalVal.Size = new System.Drawing.Size(224, 28);
            this.lblSubTotalVal.TabIndex = 1;
            this.lblSubTotalVal.Text = "0.00";
            this.lblSubTotalVal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblInvoiceDiscount
            // 
            this.lblInvoiceDiscount.AutoSize = true;
            this.lblInvoiceDiscount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblInvoiceDiscount.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblInvoiceDiscount.Location = new System.Drawing.Point(3, 56);
            this.lblInvoiceDiscount.Name = "lblInvoiceDiscount";
            this.lblInvoiceDiscount.Size = new System.Drawing.Size(144, 28);
            this.lblInvoiceDiscount.TabIndex = 4;
            this.lblInvoiceDiscount.Text = "خصم الفاتورة:";
            this.lblInvoiceDiscount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numInvoiceDiscount
            // 
            this.numInvoiceDiscount.BackColor = System.Drawing.Color.White;
            this.numInvoiceDiscount.DecimalPlaces = 2;
            this.numInvoiceDiscount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numInvoiceDiscount.Font = new System.Drawing.Font("Tahoma", 10F);
            this.numInvoiceDiscount.Location = new System.Drawing.Point(153, 59);
            this.numInvoiceDiscount.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            this.numInvoiceDiscount.Name = "numInvoiceDiscount";
            this.numInvoiceDiscount.Size = new System.Drawing.Size(224, 31);
            this.numInvoiceDiscount.TabIndex = 5;
            this.numInvoiceDiscount.ValueChanged += new System.EventHandler(this.numInvoiceDiscount_ValueChanged);
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTotal.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.lblTotal.Location = new System.Drawing.Point(3, 84);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(144, 28);
            this.lblTotal.TabIndex = 6;
            this.lblTotal.Text = "الإجمالي النهائي:";
            this.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTotalVal
            // 
            this.lblTotalVal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTotalVal.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.lblTotalVal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(82)))), ((int)(((byte)(52)))));
            this.lblTotalVal.Location = new System.Drawing.Point(153, 84);
            this.lblTotalVal.Name = "lblTotalVal";
            this.lblTotalVal.Size = new System.Drawing.Size(224, 28);
            this.lblTotalVal.TabIndex = 7;
            this.lblTotalVal.Text = "0.00";
            this.lblTotalVal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPaid
            // 
            this.lblPaid.AutoSize = true;
            this.lblPaid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPaid.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblPaid.Location = new System.Drawing.Point(3, 112);
            this.lblPaid.Name = "lblPaid";
            this.lblPaid.Size = new System.Drawing.Size(144, 28);
            this.lblPaid.TabIndex = 8;
            this.lblPaid.Text = "المبلغ المدفوع:";
            this.lblPaid.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtPaid
            // 
            this.txtPaid.BackColor = System.Drawing.Color.White;
            this.txtPaid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPaid.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtPaid.Location = new System.Drawing.Point(153, 115);
            this.txtPaid.Name = "txtPaid";
            this.txtPaid.Size = new System.Drawing.Size(224, 31);
            this.txtPaid.TabIndex = 9;
            this.txtPaid.Text = "0";
            this.txtPaid.TextChanged += new System.EventHandler(this.txtPaid_TextChanged);
            // 
            // lblDue
            // 
            this.lblDue.AutoSize = true;
            this.lblDue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDue.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.lblDue.Location = new System.Drawing.Point(3, 140);
            this.lblDue.Name = "lblDue";
            this.lblDue.Size = new System.Drawing.Size(144, 28);
            this.lblDue.TabIndex = 10;
            this.lblDue.Text = "المبلغ المتبقي:";
            this.lblDue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDueVal
            // 
            this.lblDueVal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDueVal.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.lblDueVal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(82)))), ((int)(((byte)(52)))));
            this.lblDueVal.Location = new System.Drawing.Point(153, 140);
            this.lblDueVal.Name = "lblDueVal";
            this.lblDueVal.Size = new System.Drawing.Size(224, 28);
            this.lblDueVal.TabIndex = 11;
            this.lblDueVal.Text = "0.00";
            this.lblDueVal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // frmPurchaseInvoice
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(220)))), ((int)(((byte)(170)))));
            this.ClientSize = new System.Drawing.Size(1150, 780);
            this.Controls.Add(this.gbItems);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.gbEntry);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.gbToolbar);
            this.Controls.Add(this.lblMode);
            this.Font = new System.Drawing.Font("Tahoma", 10F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPurchaseInvoice";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "فاتورة مشتريات جديدة";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.gbToolbar.ResumeLayout(false);
            this.pnlToolbar.ResumeLayout(false);
            this.pnlToolbar.PerformLayout();
            this.pnlHeader.ResumeLayout(false);
            this.gbInvoiceInfo.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.gbEntry.ResumeLayout(false);
            this.pnlEntry.ResumeLayout(false);
            this.pnlEntry.PerformLayout();
            this.gbItems.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).EndInit();
            this.pnlFooter.ResumeLayout(false);
            this.gbTotals.ResumeLayout(false);
            this.gbTotals.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numInvoiceDiscount)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.GroupBox gbToolbar;
        private System.Windows.Forms.FlowLayoutPanel pnlToolbar;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.GroupBox gbInvoiceInfo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox gbEntry;
        private System.Windows.Forms.FlowLayoutPanel pnlEntry;
        private System.Windows.Forms.GroupBox gbItems;
        private System.Windows.Forms.GroupBox gbTotals;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label lblInvoiceNoLabel, lblInvoiceNo, lblDate, lblStatus, lblSupplier, lblWarehouse, lblPaymentType, lblSubTotal, lblSubTotalVal, lblInvoiceDiscount, lblTotal, lblTotalVal, lblPaid, lblDue, lblDueVal;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.ComboBox cmbSupplier, cmbWarehouse, cmbPaymentType;
        private System.Windows.Forms.DataGridView dgvItems;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.NumericUpDown numInvoiceDiscount;
        private SalesSystem.Desktop.Controls.Common.MoneyTextBox txtPaid;
        private System.Windows.Forms.Button btnAddItem, btnRemoveItem, btnSaveDraft, btnPost, btnCancelInvoice, btnPrintA4, btnClose;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.ToolTip toolTip;
    }
}
