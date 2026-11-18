namespace SalesSystem.Desktop.Screens.Purchases.Forms
{
    partial class frmAddUpdatePurchaseInvoice
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

        // ملاحظات المصمّم:
        //   • RULE-N204: كل لون مكتوب هنا بصيغة Color.FromArgb(Convert.ToInt32(...)) حصراً، والنص/الخط
        //     بقيمهما الحرفية نفسها التي يطبّقها وقت التشغيل — فلا فرق بين المصمّم والتشغيل (RULE-N134/N140).
        //   • RULE-N128: صفر مرجع إلى أي أداة مشتركة داخل المصمّم — الألوان/الخطوط قيم حرفية هنا فقط.
        //   • RULE-N135…N138: الترتيب الثابت من أعلى لأسفل: العنوان ← أوامر المستند ← بيانات الفاتورة ←
        //     **إضافة صنف** ← الأصناف ← الإجماليات.
        //   • لوحة «إضافة صنف» هي المكوّن المشترك ctrlLineEntryPanel (TASK-014 · بيت واحد): يعلن حقوله
        //     وزرّيه في مصمّمه ويُلبس ثيمه بنفسه، فلا يُضبط له لون هنا.
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            errorProvider = new ErrorProvider(components);
            toolTip = new ToolTip(components);
            btnSaveDraft = new Button();
            btnPost = new Button();
            btnCancelInvoice = new Button();
            btnPrintA4 = new Button();
            btnClose = new Button();
            btnRemoveItem = new Button();
            rbtnModePurchase = new RadioButton();
            rbtnModeReturn = new RadioButton();
            btnPickInvoice = new Button();
            lblModeDoc = new Label();
            lblMode = new Label();
            gbToolbar = new GroupBox();
            pnlToolbar = new FlowLayoutPanel();
            pnlHeader = new Panel();
            gbInvoiceInfo = new GroupBox();
            tableLayoutPanel1 = new TableLayoutPanel();
            lblInvoiceNoLabel = new Label();
            lblInvoiceNo = new Label();
            lblDate = new Label();
            dtpDate = new DateTimePicker();
            lblStatus = new Label();
            lblSupplier = new Label();
            cmbSupplier = new ComboBox();
            lblWarehouse = new Label();
            cmbWarehouse = new ComboBox();
            lblPaymentType = new Label();
            cmbPaymentType = new ComboBox();
            ctrlLineEntry = new SalesSystem.Desktop.Controls.Common.ctrlLineEntryPanel();
            gbItems = new GroupBox();
            dgvItems = new DataGridView();
            pnlFooter = new Panel();
            gbTotals = new GroupBox();
            txtNotes = new TextBox();
            tableLayoutPanel2 = new TableLayoutPanel();
            lblSubTotal = new Label();
            lblSubTotalVal = new Label();
            lblInvoiceDiscount = new Label();
            numInvoiceDiscount = new NumericUpDown();
            lblTotal = new Label();
            lblTotalVal = new Label();
            lblPaid = new Label();
            txtPaid = new SalesSystem.Desktop.Controls.Common.MoneyTextBox();
            lblDue = new Label();
            lblDueVal = new Label();
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            gbToolbar.SuspendLayout();
            pnlToolbar.SuspendLayout();
            pnlHeader.SuspendLayout();
            gbInvoiceInfo.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            gbItems.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvItems).BeginInit();
            pnlFooter.SuspendLayout();
            gbTotals.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numInvoiceDiscount).BeginInit();
            SuspendLayout();
            // 
            // errorProvider
            // 
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errorProvider.ContainerControl = this;
            errorProvider.RightToLeft = true;
            // 
            // btnSaveDraft
            // 
            btnSaveDraft.BackColor = Color.FromArgb(13, 110, 253);
            btnSaveDraft.FlatStyle = FlatStyle.Flat;
            btnSaveDraft.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnSaveDraft.ForeColor = Color.White;
            btnSaveDraft.Location = new Point(565, 0);
            btnSaveDraft.Margin = new Padding(0, 0, 8, 0);
            btnSaveDraft.Name = "btnSaveDraft";
            btnSaveDraft.Size = new Size(140, 36);
            btnSaveDraft.TabIndex = 0;
            btnSaveDraft.Text = "حفظ مسودة (F3)";
            toolTip.SetToolTip(btnSaveDraft, "حفظ الفاتورة كمسودة — F3");
            btnSaveDraft.UseVisualStyleBackColor = false;
            btnSaveDraft.Click += btnSaveDraft_Click;
            // 
            // btnPost
            // 
            btnPost.BackColor = Color.FromArgb(25, 135, 84);
            btnPost.FlatStyle = FlatStyle.Flat;
            btnPost.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnPost.ForeColor = Color.White;
            btnPost.Location = new Point(713, 0);
            btnPost.Margin = new Padding(0, 0, 8, 0);
            btnPost.Name = "btnPost";
            btnPost.Size = new Size(140, 36);
            btnPost.TabIndex = 1;
            btnPost.Text = "ترحيل";
            toolTip.SetToolTip(btnPost, "ترحيل الفاتورة وزيادة المخزون — على هذا الزر فقط (F4 مخصّص لمنتقي الأصناف في لوحة الإضافة)");
            btnPost.UseVisualStyleBackColor = false;
            btnPost.Click += btnPost_Click;
            // 
            // btnCancelInvoice
            // 
            btnCancelInvoice.BackColor = Color.FromArgb(220, 53, 69);
            btnCancelInvoice.FlatStyle = FlatStyle.Flat;
            btnCancelInvoice.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnCancelInvoice.ForeColor = Color.White;
            btnCancelInvoice.Location = new Point(861, 0);
            btnCancelInvoice.Margin = new Padding(0, 0, 8, 0);
            btnCancelInvoice.Name = "btnCancelInvoice";
            btnCancelInvoice.Size = new Size(150, 36);
            btnCancelInvoice.TabIndex = 2;
            btnCancelInvoice.Text = "إلغاء الفاتورة";
            toolTip.SetToolTip(btnCancelInvoice, "إلغاء الفاتورة المرحّلة وعكس أثر المخزون");
            btnCancelInvoice.UseVisualStyleBackColor = false;
            btnCancelInvoice.Click += btnCancelInvoice_Click;
            // 
            // btnPrintA4
            // 
            btnPrintA4.BackColor = Color.FromArgb(23, 162, 184);
            btnPrintA4.FlatStyle = FlatStyle.Flat;
            btnPrintA4.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnPrintA4.ForeColor = Color.White;
            btnPrintA4.Location = new Point(1019, 0);
            btnPrintA4.Margin = new Padding(0, 0, 8, 0);
            btnPrintA4.Name = "btnPrintA4";
            btnPrintA4.Size = new Size(120, 36);
            btnPrintA4.TabIndex = 3;
            btnPrintA4.Text = "طباعة A4";
            toolTip.SetToolTip(btnPrintA4, "معاينة وطباعة الفاتورة A4");
            btnPrintA4.UseVisualStyleBackColor = false;
            btnPrintA4.Click += btnPrintA4_Click;
            // 
            // btnClose
            // 
            btnClose.BackColor = Color.FromArgb(108, 117, 125);
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnClose.ForeColor = Color.White;
            btnClose.Location = new Point(1147, 0);
            btnClose.Margin = new Padding(0, 0, 8, 0);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(100, 36);
            btnClose.TabIndex = 4;
            btnClose.Text = "إغلاق (Esc)";
            toolTip.SetToolTip(btnClose, "إغلاق الشاشة — Esc");
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // btnRemoveItem
            // 
            btnRemoveItem.BackColor = Color.FromArgb(108, 117, 125);
            btnRemoveItem.FlatStyle = FlatStyle.Flat;
            btnRemoveItem.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnRemoveItem.ForeColor = Color.White;
            btnRemoveItem.Location = new Point(1255, 0);
            btnRemoveItem.Margin = new Padding(0, 0, 8, 0);
            btnRemoveItem.Name = "btnRemoveItem";
            btnRemoveItem.Size = new Size(140, 36);
            btnRemoveItem.TabIndex = 5;
            btnRemoveItem.Text = "حذف السطر (Del)";
            toolTip.SetToolTip(btnRemoveItem, "حذف السطر المحدد من الشبكة — Delete");
            btnRemoveItem.UseVisualStyleBackColor = false;
            btnRemoveItem.Click += btnRemoveItem_Click;
            // 
            // rbtnModePurchase
            // 
            rbtnModePurchase.AutoSize = true;
            rbtnModePurchase.BackColor = Color.FromArgb(235, 240, 235);
            rbtnModePurchase.Checked = true;
            rbtnModePurchase.Font = new Font("Tahoma", 10F);
            rbtnModePurchase.ForeColor = Color.FromArgb(33, 37, 41);
            rbtnModePurchase.Location = new Point(144, 9);
            rbtnModePurchase.Margin = new Padding(2, 9, 2, 0);
            rbtnModePurchase.Name = "rbtnModePurchase";
            rbtnModePurchase.Size = new Size(127, 28);
            rbtnModePurchase.TabIndex = 11;
            rbtnModePurchase.TabStop = true;
            rbtnModePurchase.Text = "فاتورة شراء";
            toolTip.SetToolTip(rbtnModePurchase, "وضع المستند الحالي: فاتورة شراء");
            rbtnModePurchase.UseVisualStyleBackColor = false;
            rbtnModePurchase.CheckedChanged += DocumentMode_CheckedChanged;
            // 
            // rbtnModeReturn
            // 
            rbtnModeReturn.AutoSize = true;
            rbtnModeReturn.BackColor = Color.FromArgb(235, 240, 235);
            rbtnModeReturn.Font = new Font("Tahoma", 10F);
            rbtnModeReturn.ForeColor = Color.FromArgb(33, 37, 41);
            rbtnModeReturn.Location = new Point(275, 9);
            rbtnModeReturn.Margin = new Padding(2, 9, 2, 0);
            rbtnModeReturn.Name = "rbtnModeReturn";
            rbtnModeReturn.Size = new Size(132, 28);
            rbtnModeReturn.TabIndex = 12;
            rbtnModeReturn.Text = "مرتجع شراء";
            toolTip.SetToolTip(rbtnModeReturn, "وضع المستند: مرتجع شراء — يبدأ مستنداً جديداً");
            rbtnModeReturn.UseVisualStyleBackColor = false;
            rbtnModeReturn.CheckedChanged += DocumentMode_CheckedChanged;
            // 
            // btnPickInvoice
            // 
            btnPickInvoice.BackColor = Color.FromArgb(108, 117, 125);
            btnPickInvoice.FlatStyle = FlatStyle.Flat;
            btnPickInvoice.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnPickInvoice.ForeColor = Color.White;
            btnPickInvoice.Location = new Point(417, 0);
            btnPickInvoice.Margin = new Padding(0, 0, 8, 0);
            btnPickInvoice.Name = "btnPickInvoice";
            btnPickInvoice.Size = new Size(140, 36);
            btnPickInvoice.TabIndex = 13;
            btnPickInvoice.Text = "اختيار فاتورة";
            toolTip.SetToolTip(btnPickInvoice, "تعبئة المرتجع من فاتورة شراء موجودة (وضع المرتجع)");
            btnPickInvoice.UseVisualStyleBackColor = false;
            btnPickInvoice.Visible = false;
            btnPickInvoice.Click += btnPickInvoice_Click;
            // 
            // lblModeDoc
            // 
            lblModeDoc.AutoSize = true;
            lblModeDoc.BackColor = Color.Transparent;
            lblModeDoc.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblModeDoc.ForeColor = Color.FromArgb(14, 82, 52);
            lblModeDoc.Location = new Point(2, 10);
            lblModeDoc.Margin = new Padding(8, 10, 2, 0);
            lblModeDoc.Name = "lblModeDoc";
            lblModeDoc.Size = new Size(132, 24);
            lblModeDoc.TabIndex = 10;
            lblModeDoc.Text = "نوع المستند:";
            lblModeDoc.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblMode
            // 
            lblMode.BackColor = Color.FromArgb(16, 150, 96);
            lblMode.Dock = DockStyle.Top;
            lblMode.Font = new Font("Tahoma", 14F, FontStyle.Bold);
            lblMode.ForeColor = Color.White;
            lblMode.Location = new Point(0, 0);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(1150, 46);
            lblMode.TabIndex = 0;
            lblMode.Text = "(( فاتورة مشتريات جديدة ))";
            lblMode.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // gbToolbar
            // 
            gbToolbar.BackColor = Color.White;
            gbToolbar.Controls.Add(pnlToolbar);
            gbToolbar.Dock = DockStyle.Top;
            gbToolbar.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            gbToolbar.Location = new Point(0, 46);
            gbToolbar.Name = "gbToolbar";
            gbToolbar.Padding = new Padding(8, 6, 8, 6);
            gbToolbar.Size = new Size(1150, 70);
            gbToolbar.TabIndex = 1;
            gbToolbar.TabStop = false;
            gbToolbar.Text = "أوامر الفاتورة";
            // 
            // pnlToolbar
            // 
            pnlToolbar.Controls.Add(lblModeDoc);
            pnlToolbar.Controls.Add(rbtnModePurchase);
            pnlToolbar.Controls.Add(rbtnModeReturn);
            pnlToolbar.Controls.Add(btnPickInvoice);
            pnlToolbar.Controls.Add(btnSaveDraft);
            pnlToolbar.Controls.Add(btnPost);
            pnlToolbar.Controls.Add(btnCancelInvoice);
            pnlToolbar.Controls.Add(btnPrintA4);
            pnlToolbar.Controls.Add(btnClose);
            pnlToolbar.Controls.Add(btnRemoveItem);
            pnlToolbar.Dock = DockStyle.Fill;
            pnlToolbar.FlowDirection = FlowDirection.RightToLeft;
            pnlToolbar.Location = new Point(8, 31);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.Size = new Size(1134, 33);
            pnlToolbar.TabIndex = 0;
            pnlToolbar.WrapContents = false;
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.White;
            pnlHeader.Controls.Add(gbInvoiceInfo);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 116);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Padding = new Padding(8, 2, 8, 6);
            pnlHeader.Size = new Size(1150, 126);
            pnlHeader.TabIndex = 2;
            // 
            // gbInvoiceInfo
            // 
            gbInvoiceInfo.BackColor = Color.White;
            gbInvoiceInfo.Controls.Add(tableLayoutPanel1);
            gbInvoiceInfo.Dock = DockStyle.Fill;
            gbInvoiceInfo.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            gbInvoiceInfo.Location = new Point(8, 2);
            gbInvoiceInfo.Name = "gbInvoiceInfo";
            gbInvoiceInfo.Padding = new Padding(8, 4, 8, 6);
            gbInvoiceInfo.Size = new Size(1134, 118);
            gbInvoiceInfo.TabIndex = 0;
            gbInvoiceInfo.TabStop = false;
            gbInvoiceInfo.Text = "بيانات الفاتورة";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 6;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34F));
            tableLayoutPanel1.Controls.Add(lblInvoiceNoLabel, 0, 0);
            tableLayoutPanel1.Controls.Add(lblInvoiceNo, 1, 0);
            tableLayoutPanel1.Controls.Add(lblDate, 2, 0);
            tableLayoutPanel1.Controls.Add(dtpDate, 3, 0);
            tableLayoutPanel1.Controls.Add(lblStatus, 5, 0);
            tableLayoutPanel1.Controls.Add(lblSupplier, 0, 1);
            tableLayoutPanel1.Controls.Add(cmbSupplier, 1, 1);
            tableLayoutPanel1.Controls.Add(lblWarehouse, 2, 1);
            tableLayoutPanel1.Controls.Add(cmbWarehouse, 3, 1);
            tableLayoutPanel1.Controls.Add(lblPaymentType, 4, 1);
            tableLayoutPanel1.Controls.Add(cmbPaymentType, 5, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(8, 29);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 0F));
            tableLayoutPanel1.Size = new Size(1118, 83);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // lblInvoiceNoLabel
            // 
            lblInvoiceNoLabel.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            lblInvoiceNoLabel.AutoSize = true;
            lblInvoiceNoLabel.Location = new Point(1011, 0);
            lblInvoiceNoLabel.Name = "lblInvoiceNoLabel";
            lblInvoiceNoLabel.Size = new Size(104, 38);
            lblInvoiceNoLabel.TabIndex = 0;
            lblInvoiceNoLabel.Text = "رقم الفاتورة:";
            lblInvoiceNoLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblInvoiceNo
            // 
            lblInvoiceNo.BackColor = Color.White;
            lblInvoiceNo.Dock = DockStyle.Fill;
            lblInvoiceNo.Font = new Font("Tahoma", 10F);
            lblInvoiceNo.ForeColor = Color.FromArgb(33, 37, 41);
            lblInvoiceNo.Location = new Point(748, 0);
            lblInvoiceNo.Name = "lblInvoiceNo";
            lblInvoiceNo.Padding = new Padding(6, 0, 6, 0);
            lblInvoiceNo.Size = new Size(257, 38);
            lblInvoiceNo.TabIndex = 1;
            lblInvoiceNo.Text = "جديد";
            lblInvoiceNo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblDate
            // 
            lblDate.AutoSize = true;
            lblDate.Location = new Point(665, 0);
            lblDate.Name = "lblDate";
            lblDate.Size = new Size(77, 24);
            lblDate.TabIndex = 2;
            lblDate.Text = "التاريخ:";
            lblDate.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // dtpDate
            // 
            dtpDate.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            dtpDate.BackColor = Color.White;
            dtpDate.Font = new Font("Tahoma", 10F);
            dtpDate.Format = DateTimePickerFormat.Short;
            dtpDate.Location = new Point(385, 3);
            dtpDate.Name = "dtpDate";
            dtpDate.Size = new Size(257, 32);
            dtpDate.TabIndex = 3;
            // 
            // lblStatus
            // 
            lblStatus.BackColor = Color.FromArgb(13, 110, 253);
            lblStatus.Dock = DockStyle.Fill;
            lblStatus.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblStatus.ForeColor = Color.White;
            lblStatus.Location = new Point(3, 0);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(266, 38);
            lblStatus.TabIndex = 4;
            lblStatus.Text = "جديد";
            lblStatus.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblSupplier
            // 
            lblSupplier.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            lblSupplier.AutoSize = true;
            lblSupplier.Location = new Point(1011, 46);
            lblSupplier.Name = "lblSupplier";
            lblSupplier.Size = new Size(104, 24);
            lblSupplier.TabIndex = 5;
            lblSupplier.Text = "المورد:";
            lblSupplier.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cmbSupplier
            // 
            cmbSupplier.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            cmbSupplier.BackColor = Color.White;
            cmbSupplier.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSupplier.FlatStyle = FlatStyle.Flat;
            cmbSupplier.Font = new Font("Tahoma", 10F);
            cmbSupplier.Location = new Point(748, 42);
            cmbSupplier.Name = "cmbSupplier";
            cmbSupplier.Size = new Size(257, 32);
            cmbSupplier.TabIndex = 6;
            // 
            // lblWarehouse
            // 
            lblWarehouse.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            lblWarehouse.AutoSize = true;
            lblWarehouse.Location = new Point(648, 38);
            lblWarehouse.Name = "lblWarehouse";
            lblWarehouse.Size = new Size(94, 40);
            lblWarehouse.TabIndex = 7;
            lblWarehouse.Text = "المستودع:";
            lblWarehouse.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cmbWarehouse
            // 
            cmbWarehouse.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            cmbWarehouse.BackColor = Color.White;
            cmbWarehouse.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbWarehouse.FlatStyle = FlatStyle.Flat;
            cmbWarehouse.Font = new Font("Tahoma", 10F);
            cmbWarehouse.Location = new Point(385, 42);
            cmbWarehouse.Name = "cmbWarehouse";
            cmbWarehouse.Size = new Size(257, 32);
            cmbWarehouse.TabIndex = 8;
            // 
            // lblPaymentType
            // 
            lblPaymentType.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            lblPaymentType.AutoSize = true;
            lblPaymentType.Location = new Point(275, 38);
            lblPaymentType.Name = "lblPaymentType";
            lblPaymentType.Size = new Size(104, 40);
            lblPaymentType.TabIndex = 9;
            lblPaymentType.Text = "نوع الدفع:";
            lblPaymentType.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cmbPaymentType
            // 
            cmbPaymentType.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            cmbPaymentType.BackColor = Color.White;
            cmbPaymentType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPaymentType.FlatStyle = FlatStyle.Flat;
            cmbPaymentType.Font = new Font("Tahoma", 10F);
            cmbPaymentType.Items.AddRange(new object[] { "نقدي", "آجل", "مختلط" });
            cmbPaymentType.Location = new Point(3, 42);
            cmbPaymentType.Name = "cmbPaymentType";
            cmbPaymentType.Size = new Size(266, 32);
            cmbPaymentType.TabIndex = 10;
            // 
            // ctrlLineEntry
            // 
            ctrlLineEntry.BackColor = Color.FromArgb(170, 220, 170);
            ctrlLineEntry.Dock = DockStyle.Top;
            ctrlLineEntry.Font = new Font("Tahoma", 10F);
            ctrlLineEntry.Location = new Point(0, 242);
            ctrlLineEntry.Name = "ctrlLineEntry";
            ctrlLineEntry.RightToLeft = RightToLeft.Yes;
            ctrlLineEntry.Size = new Size(1150, 104);
            ctrlLineEntry.TabIndex = 3;
            // 
            // gbItems
            // 
            gbItems.BackColor = Color.White;
            gbItems.Controls.Add(dgvItems);
            gbItems.Dock = DockStyle.Fill;
            gbItems.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            gbItems.Location = new Point(0, 346);
            gbItems.Name = "gbItems";
            gbItems.Padding = new Padding(6, 4, 6, 6);
            gbItems.Size = new Size(1150, 222);
            gbItems.TabIndex = 4;
            gbItems.TabStop = false;
            gbItems.Text = "الأصناف";
            // 
            // dgvItems
            // 
            dgvItems.AllowUserToAddRows = false;
            dgvItems.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(248, 249, 250);
            dgvItems.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvItems.BackgroundColor = Color.White;
            dgvItems.BorderStyle = BorderStyle.None;
            dgvItems.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(52, 58, 64);
            dataGridViewCellStyle2.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(52, 58, 64);
            dataGridViewCellStyle2.SelectionForeColor = Color.White;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvItems.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvItems.ColumnHeadersHeight = 45;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Tahoma", 10F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(33, 37, 41);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(255, 243, 205);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(33, 37, 41);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvItems.DefaultCellStyle = dataGridViewCellStyle3;
            dgvItems.Dock = DockStyle.Fill;
            dgvItems.EnableHeadersVisualStyles = false;
            dgvItems.GridColor = Color.FromArgb(222, 226, 230);
            dgvItems.Location = new Point(6, 29);
            dgvItems.Name = "dgvItems";
            dgvItems.ReadOnly = true;
            dgvItems.RowHeadersVisible = false;
            dgvItems.RowHeadersWidth = 62;
            dgvItems.RowTemplate.Height = 32;
            dgvItems.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvItems.Size = new Size(1138, 187);
            dgvItems.TabIndex = 0;
            // 
            // pnlFooter
            // 
            pnlFooter.BackColor = Color.White;
            pnlFooter.Controls.Add(gbTotals);
            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Location = new Point(0, 568);
            pnlFooter.Name = "pnlFooter";
            pnlFooter.Padding = new Padding(8, 2, 8, 6);
            pnlFooter.Size = new Size(1150, 212);
            pnlFooter.TabIndex = 5;
            // 
            // gbTotals
            // 
            gbTotals.BackColor = Color.White;
            gbTotals.Controls.Add(txtNotes);
            gbTotals.Controls.Add(tableLayoutPanel2);
            gbTotals.Dock = DockStyle.Fill;
            gbTotals.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            gbTotals.Location = new Point(8, 2);
            gbTotals.Name = "gbTotals";
            gbTotals.Padding = new Padding(8, 4, 8, 6);
            gbTotals.Size = new Size(1134, 204);
            gbTotals.TabIndex = 0;
            gbTotals.TabStop = false;
            gbTotals.Text = "الإجماليات";
            // 
            // txtNotes
            // 
            txtNotes.BackColor = Color.White;
            txtNotes.Dock = DockStyle.Fill;
            txtNotes.Font = new Font("Tahoma", 10F);
            txtNotes.Location = new Point(8, 29);
            txtNotes.Multiline = true;
            txtNotes.Name = "txtNotes";
            txtNotes.PlaceholderText = "ملاحظات اختيارية...";
            txtNotes.ScrollBars = ScrollBars.Vertical;
            txtNotes.Size = new Size(738, 169);
            txtNotes.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Controls.Add(lblSubTotal, 0, 0);
            tableLayoutPanel2.Controls.Add(lblSubTotalVal, 1, 0);
            tableLayoutPanel2.Controls.Add(lblInvoiceDiscount, 0, 2);
            tableLayoutPanel2.Controls.Add(numInvoiceDiscount, 1, 2);
            tableLayoutPanel2.Controls.Add(lblTotal, 0, 3);
            tableLayoutPanel2.Controls.Add(lblTotalVal, 1, 3);
            tableLayoutPanel2.Controls.Add(lblPaid, 0, 4);
            tableLayoutPanel2.Controls.Add(txtPaid, 1, 4);
            tableLayoutPanel2.Controls.Add(lblDue, 0, 5);
            tableLayoutPanel2.Controls.Add(lblDueVal, 1, 5);
            tableLayoutPanel2.Dock = DockStyle.Right;
            tableLayoutPanel2.Location = new Point(746, 29);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 6;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 0F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            tableLayoutPanel2.Size = new Size(380, 169);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // lblSubTotal
            // 
            lblSubTotal.AutoSize = true;
            lblSubTotal.Dock = DockStyle.Fill;
            lblSubTotal.Font = new Font("Tahoma", 10F);
            lblSubTotal.Location = new Point(233, 0);
            lblSubTotal.Name = "lblSubTotal";
            lblSubTotal.Size = new Size(144, 28);
            lblSubTotal.TabIndex = 0;
            lblSubTotal.Text = "المجموع الفرعي:";
            lblSubTotal.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblSubTotalVal
            // 
            lblSubTotalVal.Dock = DockStyle.Fill;
            lblSubTotalVal.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblSubTotalVal.ForeColor = Color.FromArgb(33, 37, 41);
            lblSubTotalVal.Location = new Point(3, 0);
            lblSubTotalVal.Name = "lblSubTotalVal";
            lblSubTotalVal.Size = new Size(224, 28);
            lblSubTotalVal.TabIndex = 1;
            lblSubTotalVal.Text = "0.00";
            lblSubTotalVal.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblInvoiceDiscount
            // 
            lblInvoiceDiscount.AutoSize = true;
            lblInvoiceDiscount.Dock = DockStyle.Fill;
            lblInvoiceDiscount.Font = new Font("Tahoma", 10F);
            lblInvoiceDiscount.Location = new Point(233, 28);
            lblInvoiceDiscount.Name = "lblInvoiceDiscount";
            lblInvoiceDiscount.Size = new Size(144, 28);
            lblInvoiceDiscount.TabIndex = 4;
            lblInvoiceDiscount.Text = "خصم الفاتورة:";
            lblInvoiceDiscount.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // numInvoiceDiscount
            // 
            numInvoiceDiscount.BackColor = Color.White;
            numInvoiceDiscount.DecimalPlaces = 2;
            numInvoiceDiscount.Dock = DockStyle.Fill;
            numInvoiceDiscount.Font = new Font("Tahoma", 10F);
            numInvoiceDiscount.Location = new Point(3, 31);
            numInvoiceDiscount.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            numInvoiceDiscount.Name = "numInvoiceDiscount";
            numInvoiceDiscount.Size = new Size(224, 32);
            numInvoiceDiscount.TabIndex = 5;
            numInvoiceDiscount.ValueChanged += numInvoiceDiscount_ValueChanged;
            // 
            // lblTotal
            // 
            lblTotal.AutoSize = true;
            lblTotal.Dock = DockStyle.Fill;
            lblTotal.Font = new Font("Tahoma", 11F, FontStyle.Bold);
            lblTotal.Location = new Point(233, 56);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(144, 28);
            lblTotal.TabIndex = 6;
            lblTotal.Text = "الإجمالي النهائي:";
            lblTotal.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTotalVal
            // 
            lblTotalVal.Dock = DockStyle.Fill;
            lblTotalVal.Font = new Font("Tahoma", 11F, FontStyle.Bold);
            lblTotalVal.ForeColor = Color.FromArgb(33, 37, 41);
            lblTotalVal.Location = new Point(3, 56);
            lblTotalVal.Name = "lblTotalVal";
            lblTotalVal.Size = new Size(224, 28);
            lblTotalVal.TabIndex = 7;
            lblTotalVal.Text = "0.00";
            lblTotalVal.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblPaid
            // 
            lblPaid.AutoSize = true;
            lblPaid.Dock = DockStyle.Fill;
            lblPaid.Font = new Font("Tahoma", 10F);
            lblPaid.Location = new Point(233, 84);
            lblPaid.Name = "lblPaid";
            lblPaid.Size = new Size(144, 28);
            lblPaid.TabIndex = 8;
            lblPaid.Text = "المبلغ المدفوع:";
            lblPaid.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtPaid
            // 
            txtPaid.BackColor = Color.White;
            txtPaid.Dock = DockStyle.Fill;
            txtPaid.Font = new Font("Tahoma", 10F);
            txtPaid.Location = new Point(3, 87);
            txtPaid.Name = "txtPaid";
            txtPaid.Size = new Size(224, 32);
            txtPaid.TabIndex = 9;
            txtPaid.Text = "0";
            txtPaid.TextChanged += txtPaid_TextChanged;
            // 
            // lblDue
            // 
            lblDue.AutoSize = true;
            lblDue.Dock = DockStyle.Fill;
            lblDue.Font = new Font("Tahoma", 11F, FontStyle.Bold);
            lblDue.Location = new Point(233, 112);
            lblDue.Name = "lblDue";
            lblDue.Size = new Size(144, 57);
            lblDue.TabIndex = 10;
            lblDue.Text = "المبلغ المتبقي:";
            lblDue.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblDueVal
            // 
            lblDueVal.Dock = DockStyle.Fill;
            lblDueVal.Font = new Font("Tahoma", 11F, FontStyle.Bold);
            lblDueVal.ForeColor = Color.FromArgb(33, 37, 41);
            lblDueVal.Location = new Point(3, 112);
            lblDueVal.Name = "lblDueVal";
            lblDueVal.Size = new Size(224, 57);
            lblDueVal.TabIndex = 11;
            lblDueVal.Text = "0.00";
            lblDueVal.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // frmAddUpdatePurchaseInvoice
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.FromArgb(240, 242, 245);
            ClientSize = new Size(1150, 780);
            Controls.Add(gbItems);
            Controls.Add(pnlFooter);
            Controls.Add(ctrlLineEntry);
            Controls.Add(pnlHeader);
            Controls.Add(gbToolbar);
            Controls.Add(lblMode);
            Font = new Font("Tahoma", 10F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmAddUpdatePurchaseInvoice";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "فاتورة مشتريات جديدة";
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            gbToolbar.ResumeLayout(false);
            pnlToolbar.ResumeLayout(false);
            pnlToolbar.PerformLayout();
            pnlHeader.ResumeLayout(false);
            gbInvoiceInfo.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            gbItems.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvItems).EndInit();
            pnlFooter.ResumeLayout(false);
            gbTotals.ResumeLayout(false);
            gbTotals.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numInvoiceDiscount).EndInit();
            ResumeLayout(false);
        }

        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.GroupBox gbToolbar;
        private System.Windows.Forms.FlowLayoutPanel pnlToolbar;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.GroupBox gbInvoiceInfo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private SalesSystem.Desktop.Controls.Common.ctrlLineEntryPanel ctrlLineEntry;
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
        private System.Windows.Forms.Button btnRemoveItem, btnSaveDraft, btnPost, btnCancelInvoice, btnPrintA4, btnClose;
        private System.Windows.Forms.Label lblModeDoc;
        private System.Windows.Forms.RadioButton rbtnModePurchase, rbtnModeReturn;
        private System.Windows.Forms.Button btnPickInvoice;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.ToolTip toolTip;
    }
}
