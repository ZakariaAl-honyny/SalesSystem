namespace SalesSystem.Desktop.Screens.Returns.Forms
{
    partial class frmSalesReturn
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
        //   • المصمّم = التشغيل (RULE-N134 / RULE-N140 / RULE-N204): كل لون معلن هنا بقيمته الحرفية
        //     وبصيغة Convert.ToInt32 حصراً، وكل لون مسمّى بالشكل المؤهَّل System.Drawing.Color.White.
        //   • RULE-N128: صفر مرجع إلى مجلد الأدوات المشتركة (ألوان وخطوط المصمّم حرفية هنا فقط).
        //   • TASK-014: صندوق «إضافة صنف» صار المكوّن المشترك ctrlLineEntryPanel (البيت الواحد للشكل
        //     والسلوك معاً) بدل الصندوق المحلي ولوحته المحلية وزر «إضافة صنف» المحلي — والحاضن يضعه
        //     ولا يضبط له لوناً ولا خطاً ولا يبني له عنصراً. زر «حذف السطر (Del)» انتقل إلى شريط أوامر
        //     المرتجع مع بقية الأزرار.
        //   • RULE-N125 (ترتيب الإرساء): الإضافة بهذا الترتيب حصراً — [gbItems (Fill) · pnlFooter (Bottom)
        //     · ctrlLineEntry (Top) · pnlHeader (Top) · gbToolbar (Top) · lblMode (Top)] — فلا يُعاد دفع
        //     عنصر مراسٍ إلى آخر المجموعة بعد إضافة شريط مثبّت.
        //   • RULE-N135…N138 (الترتيب والتجميع): العنوان ← أوامر المرتجع ← بيانات المرتجع ← إضافة صنف
        //     ← الأصناف ← الإجماليات.
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            errorProvider = new ErrorProvider(components);
            toolTip = new ToolTip(components);
            btnSaveDraft = new Button();
            btnPost = new Button();
            btnClose = new Button();
            btnRemoveItem = new Button();
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
            lblCustomer = new Label();
            cmbCustomer = new ComboBox();
            lblWarehouse = new Label();
            cmbWarehouse = new ComboBox();
            lblReference = new Label();
            txtReference = new TextBox();
            ctrlLineEntry = new SalesSystem.Desktop.Controls.Common.ctrlLineEntryPanel();
            gbItems = new GroupBox();
            dgvItems = new DataGridView();
            pnlFooter = new Panel();
            gbTotals = new GroupBox();
            txtNotes = new TextBox();
            tableLayoutPanel2 = new TableLayoutPanel();
            lblSubTotal = new Label();
            lblSubTotalVal = new Label();
            lblTotal = new Label();
            lblTotalVal = new Label();
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
            btnSaveDraft.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(13), Convert.ToInt32(110), Convert.ToInt32(253));
            btnSaveDraft.FlatStyle = FlatStyle.Flat;
            btnSaveDraft.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnSaveDraft.ForeColor = System.Drawing.Color.White;
            btnSaveDraft.Location = new Point(937, 0);
            btnSaveDraft.Margin = new Padding(0, 0, 8, 0);
            btnSaveDraft.Name = "btnSaveDraft";
            btnSaveDraft.Size = new Size(197, 36);
            btnSaveDraft.TabIndex = 0;
            btnSaveDraft.Text = "حفظ مسودة (F3)";
            toolTip.SetToolTip(btnSaveDraft, "حفظ المرتجع كمسودة — F3");
            btnSaveDraft.UseVisualStyleBackColor = false;
            btnSaveDraft.Click += btnSaveDraft_Click;
            // 
            // btnPost
            // 
            btnPost.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(25), Convert.ToInt32(135), Convert.ToInt32(84));
            btnPost.FlatStyle = FlatStyle.Flat;
            btnPost.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnPost.ForeColor = System.Drawing.Color.White;
            btnPost.Location = new Point(789, 0);
            btnPost.Margin = new Padding(0, 0, 8, 0);
            btnPost.Name = "btnPost";
            btnPost.Size = new Size(140, 36);
            btnPost.TabIndex = 1;
            btnPost.Text = "ترحيل";
            toolTip.SetToolTip(btnPost, "ترحيل المرتجع وزيادة المخزون");
            btnPost.UseVisualStyleBackColor = false;
            btnPost.Click += btnPost_Click;
            // 
            // btnClose
            // 
            btnClose.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(108), Convert.ToInt32(117), Convert.ToInt32(125));
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnClose.ForeColor = System.Drawing.Color.White;
            btnClose.Location = new Point(681, 0);
            btnClose.Margin = new Padding(0, 0, 8, 0);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(100, 36);
            btnClose.TabIndex = 2;
            btnClose.Text = "إغلاق (Esc)";
            toolTip.SetToolTip(btnClose, "إغلاق الشاشة — Esc");
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // btnRemoveItem
            // 
            btnRemoveItem.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(108), Convert.ToInt32(117), Convert.ToInt32(125));
            btnRemoveItem.FlatStyle = FlatStyle.Flat;
            btnRemoveItem.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnRemoveItem.ForeColor = System.Drawing.Color.White;
            btnRemoveItem.Location = new Point(513, 0);
            btnRemoveItem.Margin = new Padding(0, 0, 8, 0);
            btnRemoveItem.Name = "btnRemoveItem";
            btnRemoveItem.Size = new Size(160, 36);
            btnRemoveItem.TabIndex = 3;
            btnRemoveItem.Text = "حذف السطر (Del)";
            toolTip.SetToolTip(btnRemoveItem, "حذف السطر المحدد من الشبكة — Delete");
            btnRemoveItem.UseVisualStyleBackColor = false;
            btnRemoveItem.Click += btnRemoveItem_Click;
            // 
            // lblMode
            // 
            lblMode.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(16), Convert.ToInt32(150), Convert.ToInt32(96));
            lblMode.Dock = DockStyle.Top;
            lblMode.Font = new Font("Tahoma", 14F, FontStyle.Bold);
            lblMode.ForeColor = System.Drawing.Color.White;
            lblMode.Location = new Point(0, 0);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(1150, 46);
            lblMode.TabIndex = 0;
            lblMode.Text = "(( مرتجع مبيعات جديد ))";
            lblMode.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // gbToolbar
            // 
            gbToolbar.BackColor = System.Drawing.Color.White;
            gbToolbar.Controls.Add(pnlToolbar);
            gbToolbar.Dock = DockStyle.Top;
            gbToolbar.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            gbToolbar.Location = new Point(0, 46);
            gbToolbar.Name = "gbToolbar";
            gbToolbar.Padding = new Padding(8, 6, 8, 6);
            gbToolbar.Size = new Size(1150, 70);
            gbToolbar.TabIndex = 1;
            gbToolbar.TabStop = false;
            gbToolbar.Text = "أوامر المرتجع";
            // 
            // pnlToolbar
            // 
            pnlToolbar.Controls.Add(btnSaveDraft);
            pnlToolbar.Controls.Add(btnPost);
            pnlToolbar.Controls.Add(btnClose);
            pnlToolbar.Controls.Add(btnRemoveItem);
            pnlToolbar.Dock = DockStyle.Fill;
            pnlToolbar.Location = new Point(8, 31);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.Size = new Size(1134, 33);
            pnlToolbar.TabIndex = 0;
            pnlToolbar.WrapContents = false;
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = System.Drawing.Color.White;
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
            gbInvoiceInfo.BackColor = System.Drawing.Color.White;
            gbInvoiceInfo.Controls.Add(tableLayoutPanel1);
            gbInvoiceInfo.Dock = DockStyle.Fill;
            gbInvoiceInfo.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            gbInvoiceInfo.Location = new Point(8, 2);
            gbInvoiceInfo.Name = "gbInvoiceInfo";
            gbInvoiceInfo.Padding = new Padding(8, 4, 8, 6);
            gbInvoiceInfo.Size = new Size(1134, 118);
            gbInvoiceInfo.TabIndex = 0;
            gbInvoiceInfo.TabStop = false;
            gbInvoiceInfo.Text = "بيانات المرتجع";
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
            tableLayoutPanel1.Controls.Add(lblCustomer, 0, 1);
            tableLayoutPanel1.Controls.Add(cmbCustomer, 1, 1);
            tableLayoutPanel1.Controls.Add(lblWarehouse, 2, 1);
            tableLayoutPanel1.Controls.Add(cmbWarehouse, 3, 1);
            tableLayoutPanel1.Controls.Add(lblReference, 4, 1);
            tableLayoutPanel1.Controls.Add(txtReference, 5, 1);
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
            lblInvoiceNoLabel.AutoSize = true;
            lblInvoiceNoLabel.Location = new Point(1026, 0);
            lblInvoiceNoLabel.Name = "lblInvoiceNoLabel";
            lblInvoiceNoLabel.Size = new Size(89, 38);
            lblInvoiceNoLabel.TabIndex = 0;
            lblInvoiceNoLabel.Text = "رقم المرتجع:";
            lblInvoiceNoLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblInvoiceNo
            // 
            lblInvoiceNo.BackColor = System.Drawing.Color.White;
            lblInvoiceNo.Dock = DockStyle.Fill;
            lblInvoiceNo.Font = new Font("Tahoma", 10F);
            lblInvoiceNo.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            lblInvoiceNo.Location = new Point(748, 0);
            lblInvoiceNo.Name = "lblInvoiceNo";
            lblInvoiceNo.Padding = new Padding(6, 0, 6, 0);
            lblInvoiceNo.Size = new Size(257, 38);
            lblInvoiceNo.TabIndex = 1;
            lblInvoiceNo.Text = "جديد";
            lblInvoiceNo.TextAlign = ContentAlignment.MiddleCenter;
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
            dtpDate.BackColor = System.Drawing.Color.White;
            dtpDate.Font = new Font("Tahoma", 10F);
            dtpDate.Format = DateTimePickerFormat.Short;
            dtpDate.Location = new Point(385, 3);
            dtpDate.Name = "dtpDate";
            dtpDate.Size = new Size(257, 32);
            dtpDate.TabIndex = 3;
            // 
            // lblStatus
            // 
            lblStatus.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(220), Convert.ToInt32(53), Convert.ToInt32(69));
            lblStatus.Dock = DockStyle.Fill;
            lblStatus.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblStatus.ForeColor = System.Drawing.Color.White;
            lblStatus.Location = new Point(3, 0);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(266, 38);
            lblStatus.TabIndex = 4;
            lblStatus.Text = "جديد";
            lblStatus.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblCustomer
            // 
            lblCustomer.AutoSize = true;
            lblCustomer.Location = new Point(1036, 38);
            lblCustomer.Name = "lblCustomer";
            lblCustomer.Size = new Size(79, 24);
            lblCustomer.TabIndex = 5;
            lblCustomer.Text = "العميل:";
            lblCustomer.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cmbCustomer
            // 
            cmbCustomer.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            cmbCustomer.BackColor = System.Drawing.Color.White;
            cmbCustomer.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCustomer.FlatStyle = FlatStyle.Flat;
            cmbCustomer.Font = new Font("Tahoma", 10F);
            cmbCustomer.Location = new Point(748, 42);
            cmbCustomer.Name = "cmbCustomer";
            cmbCustomer.Size = new Size(257, 32);
            cmbCustomer.TabIndex = 6;
            // 
            // lblWarehouse
            // 
            lblWarehouse.AutoSize = true;
            lblWarehouse.Location = new Point(653, 38);
            lblWarehouse.Name = "lblWarehouse";
            lblWarehouse.Size = new Size(89, 40);
            lblWarehouse.TabIndex = 7;
            lblWarehouse.Text = "المستودع:";
            lblWarehouse.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cmbWarehouse
            // 
            cmbWarehouse.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            cmbWarehouse.BackColor = System.Drawing.Color.White;
            cmbWarehouse.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbWarehouse.FlatStyle = FlatStyle.Flat;
            cmbWarehouse.Font = new Font("Tahoma", 10F);
            cmbWarehouse.Location = new Point(385, 42);
            cmbWarehouse.Name = "cmbWarehouse";
            cmbWarehouse.Size = new Size(257, 32);
            cmbWarehouse.TabIndex = 8;
            // 
            // lblReference
            // 
            lblReference.AutoSize = true;
            lblReference.Location = new Point(292, 38);
            lblReference.Name = "lblReference";
            lblReference.Size = new Size(87, 40);
            lblReference.TabIndex = 9;
            lblReference.Text = "المرجع الأصلي:";
            lblReference.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtReference
            // 
            txtReference.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtReference.BackColor = System.Drawing.Color.White;
            txtReference.Font = new Font("Tahoma", 10F);
            txtReference.Location = new Point(3, 42);
            txtReference.Name = "txtReference";
            txtReference.Size = new Size(266, 32);
            txtReference.TabIndex = 10;
            // 
            // ctrlLineEntry
            // 
            ctrlLineEntry.Dock = DockStyle.Top;
            ctrlLineEntry.Location = new Point(0, 242);
            ctrlLineEntry.Name = "ctrlLineEntry";
            ctrlLineEntry.RightToLeft = RightToLeft.Yes;
            ctrlLineEntry.Size = new Size(1150, 104);
            ctrlLineEntry.TabIndex = 3;
            // 
            // gbItems
            // 
            gbItems.BackColor = System.Drawing.Color.White;
            gbItems.Controls.Add(dgvItems);
            gbItems.Dock = DockStyle.Fill;
            gbItems.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            gbItems.Location = new Point(0, 346);
            gbItems.Name = "gbItems";
            gbItems.Padding = new Padding(6, 4, 6, 6);
            gbItems.Size = new Size(1150, 314);
            gbItems.TabIndex = 4;
            gbItems.TabStop = false;
            gbItems.Text = "الأصناف";
            // 
            // dgvItems
            // 
            dgvItems.AllowUserToAddRows = false;
            dgvItems.AllowUserToDeleteRows = false;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(248), Convert.ToInt32(249), Convert.ToInt32(250));
            dgvItems.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            dgvItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvItems.BackgroundColor = System.Drawing.Color.White;
            dgvItems.BorderStyle = BorderStyle.None;
            dgvItems.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(52), Convert.ToInt32(58), Convert.ToInt32(64));
            dataGridViewCellStyle5.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(52), Convert.ToInt32(58), Convert.ToInt32(64));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.True;
            dgvItems.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            dgvItems.ColumnHeadersHeight = 45;
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.Font = new Font("Tahoma", 10F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(255), Convert.ToInt32(243), Convert.ToInt32(205));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.False;
            dgvItems.DefaultCellStyle = dataGridViewCellStyle6;
            dgvItems.Dock = DockStyle.Fill;
            dgvItems.EnableHeadersVisualStyles = false;
            dgvItems.GridColor = System.Drawing.Color.FromArgb(Convert.ToInt32(222), Convert.ToInt32(226), Convert.ToInt32(230));
            dgvItems.Location = new Point(6, 29);
            dgvItems.Name = "dgvItems";
            dgvItems.ReadOnly = true;
            dgvItems.RowHeadersVisible = false;
            dgvItems.RowHeadersWidth = 62;
            dgvItems.RowTemplate.Height = 32;
            dgvItems.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvItems.Size = new Size(1138, 279);
            dgvItems.TabIndex = 0;
            // 
            // pnlFooter
            // 
            pnlFooter.BackColor = System.Drawing.Color.White;
            pnlFooter.Controls.Add(gbTotals);
            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Location = new Point(0, 660);
            pnlFooter.Name = "pnlFooter";
            pnlFooter.Padding = new Padding(8, 2, 8, 6);
            pnlFooter.Size = new Size(1150, 120);
            pnlFooter.TabIndex = 5;
            // 
            // gbTotals
            // 
            gbTotals.BackColor = System.Drawing.Color.White;
            gbTotals.Controls.Add(txtNotes);
            gbTotals.Controls.Add(tableLayoutPanel2);
            gbTotals.Dock = DockStyle.Fill;
            gbTotals.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            gbTotals.Location = new Point(8, 2);
            gbTotals.Name = "gbTotals";
            gbTotals.Padding = new Padding(8, 4, 8, 6);
            gbTotals.Size = new Size(1134, 112);
            gbTotals.TabIndex = 0;
            gbTotals.TabStop = false;
            gbTotals.Text = "الإجماليات";
            // 
            // txtNotes
            // 
            txtNotes.BackColor = System.Drawing.Color.White;
            txtNotes.Dock = DockStyle.Fill;
            txtNotes.Font = new Font("Tahoma", 10F);
            txtNotes.Location = new Point(8, 29);
            txtNotes.Multiline = true;
            txtNotes.Name = "txtNotes";
            txtNotes.PlaceholderText = "ملاحظات الإرجاع...";
            txtNotes.ScrollBars = ScrollBars.Vertical;
            txtNotes.Size = new Size(738, 77);
            txtNotes.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Controls.Add(lblSubTotal, 0, 0);
            tableLayoutPanel2.Controls.Add(lblSubTotalVal, 1, 0);
            tableLayoutPanel2.Controls.Add(lblTotal, 0, 2);
            tableLayoutPanel2.Controls.Add(lblTotalVal, 1, 2);
            tableLayoutPanel2.Dock = DockStyle.Right;
            tableLayoutPanel2.Location = new Point(746, 29);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 3;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 0F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel2.Size = new Size(380, 77);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // lblSubTotal
            // 
            lblSubTotal.AutoSize = true;
            lblSubTotal.Dock = DockStyle.Fill;
            lblSubTotal.Font = new Font("Tahoma", 10F);
            lblSubTotal.Location = new Point(233, 0);
            lblSubTotal.Name = "lblSubTotal";
            lblSubTotal.Size = new Size(144, 30);
            lblSubTotal.TabIndex = 0;
            lblSubTotal.Text = "المجموع الفرعي:";
            lblSubTotal.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblSubTotalVal
            // 
            lblSubTotalVal.Dock = DockStyle.Fill;
            lblSubTotalVal.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblSubTotalVal.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            lblSubTotalVal.Location = new Point(3, 0);
            lblSubTotalVal.Name = "lblSubTotalVal";
            lblSubTotalVal.Size = new Size(224, 30);
            lblSubTotalVal.TabIndex = 1;
            lblSubTotalVal.Text = "0.00";
            lblSubTotalVal.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTotal
            // 
            lblTotal.AutoSize = true;
            lblTotal.Dock = DockStyle.Fill;
            lblTotal.Font = new Font("Tahoma", 11F, FontStyle.Bold);
            lblTotal.Location = new Point(233, 30);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(144, 47);
            lblTotal.TabIndex = 4;
            lblTotal.Text = "الإجمالي النهائي:";
            lblTotal.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTotalVal
            // 
            lblTotalVal.Dock = DockStyle.Fill;
            lblTotalVal.Font = new Font("Tahoma", 11F, FontStyle.Bold);
            lblTotalVal.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            lblTotalVal.Location = new Point(3, 30);
            lblTotalVal.Name = "lblTotalVal";
            lblTotalVal.Size = new Size(224, 47);
            lblTotalVal.TabIndex = 5;
            lblTotalVal.Text = "0.00";
            lblTotalVal.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // frmSalesReturn
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(240), Convert.ToInt32(242), Convert.ToInt32(245));
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
            Name = "frmSalesReturn";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "مرتجع مبيعات جديد";
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            gbToolbar.ResumeLayout(false);
            pnlToolbar.ResumeLayout(false);
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
        private System.Windows.Forms.Label lblInvoiceNoLabel, lblInvoiceNo, lblDate, lblStatus, lblCustomer, lblWarehouse, lblReference, lblSubTotal, lblSubTotalVal, lblTotal, lblTotalVal;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.ComboBox cmbCustomer, cmbWarehouse;
        private System.Windows.Forms.TextBox txtReference, txtNotes;
        private System.Windows.Forms.DataGridView dgvItems;
        private System.Windows.Forms.Button btnRemoveItem, btnSaveDraft, btnPost, btnClose;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.ToolTip toolTip;
    }
}
