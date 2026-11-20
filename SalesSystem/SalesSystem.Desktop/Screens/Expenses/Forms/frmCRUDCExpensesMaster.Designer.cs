namespace SalesSystem.Desktop.Screens.Expenses.Forms;

partial class frmCRUDCExpensesMaster
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
        cmsRows = new ContextMenuStrip(components);
        cmsView = new ToolStripMenuItem();
        cmsPost = new ToolStripMenuItem();
        cmsCancel = new ToolStripMenuItem();
        cmsDelete = new ToolStripMenuItem();
        cmsSeparator = new ToolStripSeparator();
        cmsRefresh = new ToolStripMenuItem();
        lblMode = new Label();
        pnlFields = new Panel();
        gbDocument = new GroupBox();
        lblExpenseNo = new Label();
        txtExpenseNo = new TextBox();
        lblDate = new Label();
        dtpDate = new DateTimePicker();
        lblCashBox = new Label();
        lblCashBoxReq = new Label();
        cmbCashBox = new ComboBox();
        lblAmount = new Label();
        lblAmountReq = new Label();
        numAmount = new NumericUpDown();
        lblNetTotal = new Label();
        lblNetTotalValue = new Label();
        lblStatus = new Label();
        lblStatusValue = new Label();
        lblNotes = new Label();
        lblNotesReq = new Label();
        txtNotes = new TextBox();
        pnlButtons = new FlowLayoutPanel();
        btnNew = new Button();
        btnSave = new Button();
        btnEdit = new Button();
        btnDelete = new Button();
        btnPost = new Button();
        btnCancelDocument = new Button();
        btnRefresh = new Button();
        btnClose = new Button();
        pnlFilters = new Panel();
        gbFilter = new GroupBox();
        lblFilterBy = new Label();
        cbFilterBy = new ComboBox();
        txtFilterValue = new TextBox();
        lblStatusFilter = new Label();
        cbStatus = new ComboBox();
        chkOnlyCancelled = new CheckBox();
        lblFrom = new Label();
        dtpFrom = new DateTimePicker();
        lblTo = new Label();
        dtpTo = new DateTimePicker();
        btnRangeToday = new Button();
        btnRangeMonth = new Button();
        btnRange30 = new Button();
        btnClearFilters = new Button();
        btnToggleFilters = new Button();
        pnlGrid = new Panel();
        dgvMain = new DataGridView();
        lblEmpty = new Label();
        pnlSummary = new Panel();
        lblSummary = new Label();
        toolTip = new ToolTip(components);
        liveTip = new ToolTip(components);
        errorProvider = new ErrorProvider(components);
        cmsRows.SuspendLayout();
        pnlFields.SuspendLayout();
        gbDocument.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numAmount).BeginInit();
        pnlButtons.SuspendLayout();
        pnlFilters.SuspendLayout();
        gbFilter.SuspendLayout();
        pnlGrid.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvMain).BeginInit();
        pnlSummary.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
        SuspendLayout();
        // 
        // cmsRows
        // 
        cmsRows.ImageScalingSize = new Size(24, 24);
        cmsRows.Items.AddRange(new ToolStripItem[] { cmsView, cmsPost, cmsCancel, cmsDelete, cmsSeparator, cmsRefresh });
        cmsRows.Name = "cmsRows";
        cmsRows.RightToLeft = RightToLeft.Yes;
        cmsRows.Size = new Size(237, 170);
        cmsRows.Opening += cmsRows_Opening;
        // 
        // cmsView
        // 
        cmsView.Name = "cmsView";
        cmsView.Size = new Size(236, 32);
        cmsView.Text = "👁 عرض في المحرر";
        cmsView.Click += cmsView_Click;
        // 
        // cmsPost
        // 
        cmsPost.Name = "cmsPost";
        cmsPost.Size = new Size(236, 32);
        cmsPost.Text = "📤 ترحيل";
        cmsPost.Click += cmsPost_Click;
        // 
        // cmsCancel
        // 
        cmsCancel.Name = "cmsCancel";
        cmsCancel.Size = new Size(236, 32);
        cmsCancel.Text = "🚫 إلغاء المستند";
        cmsCancel.Click += cmsCancel_Click;
        // 
        // cmsDelete
        // 
        cmsDelete.Name = "cmsDelete";
        cmsDelete.Size = new Size(236, 32);
        cmsDelete.Text = "🗑 حذف";
        cmsDelete.Click += cmsDelete_Click;
        // 
        // cmsSeparator
        // 
        cmsSeparator.Name = "cmsSeparator";
        cmsSeparator.Size = new Size(233, 6);
        // 
        // cmsRefresh
        // 
        cmsRefresh.Name = "cmsRefresh";
        cmsRefresh.Size = new Size(236, 32);
        cmsRefresh.Text = "🔄 تحديث (F5)";
        cmsRefresh.Click += cmsRefresh_Click;
        // 
        // lblMode
        // 
        lblMode.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
        lblMode.Dock = DockStyle.Top;
        lblMode.Font = new Font("Tahoma", 14F, FontStyle.Bold);
        lblMode.ForeColor = System.Drawing.Color.White;
        lblMode.Location = new Point(0, 0);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(1432, 50);
        lblMode.TabIndex = 0;
        lblMode.Text = "(( إدارة المصروفات ))";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlFields
        // 
        pnlFields.Controls.Add(gbDocument);
        pnlFields.Dock = DockStyle.Top;
        pnlFields.Location = new Point(0, 50);
        pnlFields.Name = "pnlFields";
        pnlFields.Padding = new Padding(6, 4, 6, 4);
        pnlFields.Size = new Size(1432, 210);
        pnlFields.TabIndex = 1;
        // 
        // gbDocument
        // 
        gbDocument.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(245), Convert.ToInt32(235));
        gbDocument.Controls.Add(lblExpenseNo);
        gbDocument.Controls.Add(txtExpenseNo);
        gbDocument.Controls.Add(lblDate);
        gbDocument.Controls.Add(dtpDate);
        gbDocument.Controls.Add(lblCashBox);
        gbDocument.Controls.Add(lblCashBoxReq);
        gbDocument.Controls.Add(cmbCashBox);
        gbDocument.Controls.Add(lblAmount);
        gbDocument.Controls.Add(lblAmountReq);
        gbDocument.Controls.Add(numAmount);
        gbDocument.Controls.Add(lblNetTotal);
        gbDocument.Controls.Add(lblNetTotalValue);
        gbDocument.Controls.Add(lblStatus);
        gbDocument.Controls.Add(lblStatusValue);
        gbDocument.Controls.Add(lblNotes);
        gbDocument.Controls.Add(lblNotesReq);
        gbDocument.Controls.Add(txtNotes);
        gbDocument.Dock = DockStyle.Fill;
        gbDocument.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        gbDocument.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        gbDocument.Location = new Point(6, 4);
        gbDocument.Name = "gbDocument";
        gbDocument.Padding = new Padding(10, 4, 10, 4);
        gbDocument.Size = new Size(1420, 202);
        gbDocument.TabIndex = 0;
        gbDocument.TabStop = false;
        gbDocument.Text = "بيانات المصروف";
        // 
        // lblExpenseNo
        // 
        lblExpenseNo.AutoSize = true;
        lblExpenseNo.Location = new Point(1056, 29);
        lblExpenseNo.Name = "lblExpenseNo";
        lblExpenseNo.Size = new Size(138, 24);
        lblExpenseNo.TabIndex = 0;
        lblExpenseNo.Text = "رقم المصروف";
        // 
        // txtExpenseNo
        // 
        txtExpenseNo.BackColor = System.Drawing.Color.White;
        txtExpenseNo.Location = new Point(1063, 55);
        txtExpenseNo.Name = "txtExpenseNo";
        txtExpenseNo.ReadOnly = true;
        txtExpenseNo.Size = new Size(124, 32);
        txtExpenseNo.TabIndex = 0;
        txtExpenseNo.TabStop = false;
        // 
        // lblDate
        // 
        lblDate.AutoSize = true;
        lblDate.Location = new Point(946, 29);
        lblDate.Name = "lblDate";
        lblDate.Size = new Size(70, 24);
        lblDate.TabIndex = 0;
        lblDate.Text = "التاريخ";
        // 
        // dtpDate
        // 
        dtpDate.BackColor = System.Drawing.Color.White;
        dtpDate.Font = new Font("Tahoma", 10F);
        dtpDate.Format = DateTimePickerFormat.Short;
        dtpDate.Location = new Point(903, 55);
        dtpDate.Name = "dtpDate";
        dtpDate.RightToLeft = RightToLeft.Yes;
        dtpDate.Size = new Size(156, 32);
        dtpDate.TabIndex = 0;
        // 
        // lblCashBox
        // 
        lblCashBox.AutoSize = true;
        lblCashBox.Location = new Point(768, 28);
        lblCashBox.Name = "lblCashBox";
        lblCashBox.Size = new Size(94, 24);
        lblCashBox.TabIndex = 0;
        lblCashBox.Text = "الصندوق";
        // 
        // lblCashBoxReq
        // 
        lblCashBoxReq.AutoSize = true;
        lblCashBoxReq.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblCashBoxReq.ForeColor = System.Drawing.Color.Red;
        lblCashBoxReq.Location = new Point(750, 28);
        lblCashBoxReq.Name = "lblCashBoxReq";
        lblCashBoxReq.Size = new Size(23, 24);
        lblCashBoxReq.TabIndex = 0;
        lblCashBoxReq.Text = "*";
        // 
        // cmbCashBox
        // 
        cmbCashBox.BackColor = System.Drawing.Color.White;
        cmbCashBox.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbCashBox.FlatStyle = FlatStyle.Flat;
        cmbCashBox.Font = new Font("Tahoma", 10F);
        cmbCashBox.Location = new Point(728, 56);
        cmbCashBox.Name = "cmbCashBox";
        cmbCashBox.Size = new Size(168, 32);
        cmbCashBox.TabIndex = 1;
        // 
        // lblAmount
        // 
        lblAmount.AutoSize = true;
        lblAmount.Location = new Point(604, 30);
        lblAmount.Name = "lblAmount";
        lblAmount.Size = new Size(66, 24);
        lblAmount.TabIndex = 0;
        lblAmount.Text = "المبلغ";
        // 
        // lblAmountReq
        // 
        lblAmountReq.AutoSize = true;
        lblAmountReq.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblAmountReq.ForeColor = System.Drawing.Color.Red;
        lblAmountReq.Location = new Point(586, 30);
        lblAmountReq.Name = "lblAmountReq";
        lblAmountReq.Size = new Size(23, 24);
        lblAmountReq.TabIndex = 0;
        lblAmountReq.Text = "*";
        // 
        // numAmount
        // 
        numAmount.BackColor = System.Drawing.Color.White;
        numAmount.DecimalPlaces = 2;
        numAmount.Font = new Font("Tahoma", 10F);
        numAmount.Location = new Point(552, 56);
        numAmount.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 });
        numAmount.Name = "numAmount";
        numAmount.RightToLeft = RightToLeft.Yes;
        numAmount.Size = new Size(170, 32);
        numAmount.TabIndex = 2;
        numAmount.TextAlign = HorizontalAlignment.Right;
        numAmount.ThousandsSeparator = true;
        numAmount.ValueChanged += numAmount_ValueChanged;
        // 
        // lblNetTotal
        // 
        lblNetTotal.AutoSize = true;
        lblNetTotal.Location = new Point(390, 30);
        lblNetTotal.Name = "lblNetTotal";
        lblNetTotal.Size = new Size(132, 24);
        lblNetTotal.TabIndex = 0;
        lblNetTotal.Text = "صافي المبلغ";
        // 
        // lblNetTotalValue
        // 
        lblNetTotalValue.BackColor = System.Drawing.Color.White;
        lblNetTotalValue.BorderStyle = BorderStyle.FixedSingle;
        lblNetTotalValue.Font = new Font("Tahoma", 11F, FontStyle.Bold);
        lblNetTotalValue.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
        lblNetTotalValue.Location = new Point(366, 56);
        lblNetTotalValue.Name = "lblNetTotalValue";
        lblNetTotalValue.Size = new Size(180, 32);
        lblNetTotalValue.TabIndex = 0;
        lblNetTotalValue.Text = "0.00";
        lblNetTotalValue.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblStatus
        // 
        lblStatus.AutoSize = true;
        lblStatus.Location = new Point(243, 31);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(65, 24);
        lblStatus.TabIndex = 0;
        lblStatus.Text = "الحالة";
        // 
        // lblStatusValue
        // 
        lblStatusValue.BackColor = System.Drawing.Color.White;
        lblStatusValue.BorderStyle = BorderStyle.FixedSingle;
        lblStatusValue.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblStatusValue.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
        lblStatusValue.Location = new Point(190, 57);
        lblStatusValue.Name = "lblStatusValue";
        lblStatusValue.Size = new Size(170, 32);
        lblStatusValue.TabIndex = 0;
        lblStatusValue.Text = "—";
        lblStatusValue.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblNotes
        // 
        lblNotes.Location = new Point(1057, 106);
        lblNotes.Name = "lblNotes";
        lblNotes.Size = new Size(130, 26);
        lblNotes.TabIndex = 0;
        lblNotes.Text = "البيان / ملاحظات";
        lblNotes.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblNotesReq
        // 
        lblNotesReq.AutoSize = true;
        lblNotesReq.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblNotesReq.ForeColor = System.Drawing.Color.Red;
        lblNotesReq.Location = new Point(1039, 106);
        lblNotesReq.Name = "lblNotesReq";
        lblNotesReq.Size = new Size(23, 24);
        lblNotesReq.TabIndex = 0;
        lblNotesReq.Text = "*";
        // 
        // txtNotes
        // 
        txtNotes.BackColor = System.Drawing.Color.White;
        txtNotes.Font = new Font("Tahoma", 10F);
        txtNotes.Location = new Point(518, 104);
        txtNotes.Multiline = true;
        txtNotes.Name = "txtNotes";
        txtNotes.RightToLeft = RightToLeft.Yes;
        txtNotes.ScrollBars = ScrollBars.Vertical;
        txtNotes.Size = new Size(520, 60);
        txtNotes.TabIndex = 3;
        // 
        // pnlButtons
        // 
        pnlButtons.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
        pnlButtons.Controls.Add(btnNew);
        pnlButtons.Controls.Add(btnSave);
        pnlButtons.Controls.Add(btnEdit);
        pnlButtons.Controls.Add(btnDelete);
        pnlButtons.Controls.Add(btnPost);
        pnlButtons.Controls.Add(btnCancelDocument);
        pnlButtons.Controls.Add(btnRefresh);
        pnlButtons.Controls.Add(btnClose);
        pnlButtons.Dock = DockStyle.Top;
        pnlButtons.Location = new Point(0, 260);
        pnlButtons.Name = "pnlButtons";
        pnlButtons.Padding = new Padding(10);
        pnlButtons.Size = new Size(1432, 60);
        pnlButtons.TabIndex = 2;
        // 
        // btnNew
        // 
        btnNew.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(13), Convert.ToInt32(110), Convert.ToInt32(253));
        btnNew.FlatStyle = FlatStyle.Flat;
        btnNew.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnNew.ForeColor = System.Drawing.Color.White;
        btnNew.Location = new Point(1267, 13);
        btnNew.Name = "btnNew";
        btnNew.Size = new Size(142, 35);
        btnNew.TabIndex = 0;
        btnNew.Text = "🆕 جديد";
        btnNew.UseVisualStyleBackColor = false;
        btnNew.Click += btnNew_Click;
        // 
        // btnSave
        // 
        btnSave.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(25), Convert.ToInt32(135), Convert.ToInt32(84));
        btnSave.FlatStyle = FlatStyle.Flat;
        btnSave.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnSave.ForeColor = System.Drawing.Color.White;
        btnSave.Location = new Point(1133, 13);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(128, 35);
        btnSave.TabIndex = 1;
        btnSave.Text = "💾 حفظ";
        btnSave.UseVisualStyleBackColor = false;
        btnSave.Click += btnSave_Click;
        // 
        // btnEdit
        // 
        btnEdit.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(255), Convert.ToInt32(193), Convert.ToInt32(7));
        btnEdit.FlatStyle = FlatStyle.Flat;
        btnEdit.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnEdit.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(30), Convert.ToInt32(41), Convert.ToInt32(59));
        btnEdit.Location = new Point(999, 13);
        btnEdit.Name = "btnEdit";
        btnEdit.Size = new Size(128, 35);
        btnEdit.TabIndex = 2;
        btnEdit.Text = "✏️ تعديل";
        btnEdit.UseVisualStyleBackColor = false;
        btnEdit.Click += btnEdit_Click;
        // 
        // btnDelete
        // 
        btnDelete.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(220), Convert.ToInt32(53), Convert.ToInt32(69));
        btnDelete.FlatStyle = FlatStyle.Flat;
        btnDelete.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnDelete.ForeColor = System.Drawing.Color.White;
        btnDelete.Location = new Point(855, 13);
        btnDelete.Name = "btnDelete";
        btnDelete.Size = new Size(138, 35);
        btnDelete.TabIndex = 3;
        btnDelete.Text = "🗑 حذف";
        btnDelete.UseVisualStyleBackColor = false;
        btnDelete.Click += btnDelete_Click;
        // 
        // btnPost
        // 
        btnPost.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(23), Convert.ToInt32(162), Convert.ToInt32(184));
        btnPost.FlatStyle = FlatStyle.Flat;
        btnPost.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnPost.ForeColor = System.Drawing.Color.White;
        btnPost.Location = new Point(707, 13);
        btnPost.Name = "btnPost";
        btnPost.Size = new Size(142, 35);
        btnPost.TabIndex = 4;
        btnPost.Text = "📤 ترحيل";
        btnPost.UseVisualStyleBackColor = false;
        btnPost.Click += btnPost_Click;
        // 
        // btnCancelDocument
        // 
        btnCancelDocument.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(220), Convert.ToInt32(53), Convert.ToInt32(69));
        btnCancelDocument.FlatStyle = FlatStyle.Flat;
        btnCancelDocument.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnCancelDocument.ForeColor = System.Drawing.Color.White;
        btnCancelDocument.Location = new Point(527, 13);
        btnCancelDocument.Name = "btnCancelDocument";
        btnCancelDocument.Size = new Size(174, 35);
        btnCancelDocument.TabIndex = 5;
        btnCancelDocument.Text = "🚫 إلغاء المستند";
        btnCancelDocument.UseVisualStyleBackColor = false;
        btnCancelDocument.Click += btnCancelDocument_Click;
        // 
        // btnRefresh
        // 
        btnRefresh.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(23), Convert.ToInt32(162), Convert.ToInt32(184));
        btnRefresh.FlatStyle = FlatStyle.Flat;
        btnRefresh.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnRefresh.ForeColor = System.Drawing.Color.White;
        btnRefresh.Location = new Point(391, 13);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(130, 35);
        btnRefresh.TabIndex = 6;
        btnRefresh.Text = "🔄 تحديث";
        btnRefresh.UseVisualStyleBackColor = false;
        btnRefresh.Click += btnRefresh_Click;
        // 
        // btnClose
        // 
        btnClose.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(248), Convert.ToInt32(249), Convert.ToInt32(250));
        btnClose.FlatStyle = FlatStyle.Flat;
        btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnClose.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(30), Convert.ToInt32(41), Convert.ToInt32(59));
        btnClose.Location = new Point(255, 13);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(130, 35);
        btnClose.TabIndex = 7;
        btnClose.TabStop = false;
        btnClose.Text = "🚪 خروج";
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += btnClose_Click;
        // 
        // pnlFilters
        // 
        pnlFilters.Controls.Add(gbFilter);
        pnlFilters.Controls.Add(btnToggleFilters);
        pnlFilters.Dock = DockStyle.Top;
        pnlFilters.Location = new Point(0, 320);
        pnlFilters.Margin = new Padding(0);
        pnlFilters.Name = "pnlFilters";
        pnlFilters.Padding = new Padding(6, 4, 6, 4);
        pnlFilters.Size = new Size(1432, 112);
        pnlFilters.TabIndex = 3;
        // 
        // gbFilter
        // 
        gbFilter.Controls.Add(lblFilterBy);
        gbFilter.Controls.Add(cbFilterBy);
        gbFilter.Controls.Add(txtFilterValue);
        gbFilter.Controls.Add(lblStatusFilter);
        gbFilter.Controls.Add(cbStatus);
        gbFilter.Controls.Add(chkOnlyCancelled);
        gbFilter.Controls.Add(lblFrom);
        gbFilter.Controls.Add(dtpFrom);
        gbFilter.Controls.Add(lblTo);
        gbFilter.Controls.Add(dtpTo);
        gbFilter.Controls.Add(btnRangeToday);
        gbFilter.Controls.Add(btnRangeMonth);
        gbFilter.Controls.Add(btnRange30);
        gbFilter.Controls.Add(btnClearFilters);
        gbFilter.Dock = DockStyle.Fill;
        gbFilter.Font = new Font("Tahoma", 9F);
        gbFilter.Location = new Point(146, 4);
        gbFilter.Margin = new Padding(0);
        gbFilter.Name = "gbFilter";
        gbFilter.Padding = new Padding(8);
        gbFilter.RightToLeft = RightToLeft.Yes;
        gbFilter.Size = new Size(1280, 104);
        gbFilter.TabIndex = 0;
        gbFilter.TabStop = false;
        gbFilter.Text = "بحث وتصفية";
        // 
        // lblFilterBy
        // 
        lblFilterBy.AutoSize = true;
        lblFilterBy.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblFilterBy.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblFilterBy.Location = new Point(1196, 30);
        lblFilterBy.Name = "lblFilterBy";
        lblFilterBy.Size = new Size(86, 22);
        lblFilterBy.TabIndex = 0;
        lblFilterBy.Text = "تصفية بـ:";
        // 
        // cbFilterBy
        // 
        cbFilterBy.BackColor = System.Drawing.Color.White;
        cbFilterBy.DropDownStyle = ComboBoxStyle.DropDownList;
        cbFilterBy.FlatStyle = FlatStyle.Flat;
        cbFilterBy.Font = new Font("Tahoma", 9F);
        cbFilterBy.Items.AddRange(new object[] { "لا شيء", "رقم السند", "البيان", "الصندوق", "المبلغ" });
        cbFilterBy.Location = new Point(1040, 28);
        cbFilterBy.Name = "cbFilterBy";
        cbFilterBy.Size = new Size(150, 30);
        cbFilterBy.TabIndex = 1;
        cbFilterBy.SelectedIndexChanged += cbFilterBy_SelectedIndexChanged;
        // 
        // txtFilterValue
        // 
        txtFilterValue.BackColor = System.Drawing.Color.White;
        txtFilterValue.Font = new Font("Tahoma", 9F);
        txtFilterValue.Location = new Point(800, 28);
        txtFilterValue.Name = "txtFilterValue";
        txtFilterValue.PlaceholderText = "اكتب قيمة التصفية...";
        txtFilterValue.RightToLeft = RightToLeft.Yes;
        txtFilterValue.Size = new Size(230, 29);
        txtFilterValue.TabIndex = 2;
        txtFilterValue.TextChanged += txtFilterValue_TextChanged;
        txtFilterValue.KeyDown += txtFilterValue_KeyDown;
        // 
        // lblStatusFilter
        // 
        lblStatusFilter.AutoSize = true;
        lblStatusFilter.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblStatusFilter.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblStatusFilter.Location = new Point(706, 30);
        lblStatusFilter.Name = "lblStatusFilter";
        lblStatusFilter.Size = new Size(67, 22);
        lblStatusFilter.TabIndex = 0;
        lblStatusFilter.Text = "الحالة:";
        // 
        // cbStatus
        // 
        cbStatus.BackColor = System.Drawing.Color.White;
        cbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
        cbStatus.FlatStyle = FlatStyle.Flat;
        cbStatus.Font = new Font("Tahoma", 9F);
        cbStatus.Location = new Point(560, 28);
        cbStatus.Name = "cbStatus";
        cbStatus.Size = new Size(140, 30);
        cbStatus.TabIndex = 3;
        cbStatus.SelectedIndexChanged += cbStatus_SelectedIndexChanged;
        // 
        // chkOnlyCancelled
        // 
        chkOnlyCancelled.AutoSize = true;
        chkOnlyCancelled.FlatStyle = FlatStyle.Flat;
        chkOnlyCancelled.Font = new Font("Tahoma", 9F);
        chkOnlyCancelled.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        chkOnlyCancelled.Location = new Point(380, 30);
        chkOnlyCancelled.Name = "chkOnlyCancelled";
        chkOnlyCancelled.Size = new Size(114, 26);
        chkOnlyCancelled.TabIndex = 4;
        chkOnlyCancelled.Text = "الملغاة فقط";
        chkOnlyCancelled.CheckedChanged += chkOnlyCancelled_CheckedChanged;
        // 
        // lblFrom
        // 
        lblFrom.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblFrom.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblFrom.Location = new Point(1230, 68);
        lblFrom.Name = "lblFrom";
        lblFrom.Size = new Size(48, 26);
        lblFrom.TabIndex = 0;
        lblFrom.Text = "من:";
        lblFrom.TextAlign = ContentAlignment.MiddleRight;
        // 
        // dtpFrom
        // 
        dtpFrom.BackColor = System.Drawing.Color.White;
        dtpFrom.Font = new Font("Tahoma", 9F);
        dtpFrom.Format = DateTimePickerFormat.Short;
        dtpFrom.Location = new Point(1096, 66);
        dtpFrom.Name = "dtpFrom";
        dtpFrom.Size = new Size(128, 29);
        dtpFrom.TabIndex = 5;
        dtpFrom.ValueChanged += dtpFrom_ValueChanged;
        // 
        // lblTo
        // 
        lblTo.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblTo.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblTo.Location = new Point(1048, 68);
        lblTo.Name = "lblTo";
        lblTo.Size = new Size(48, 26);
        lblTo.TabIndex = 0;
        lblTo.Text = "إلى:";
        lblTo.TextAlign = ContentAlignment.MiddleRight;
        // 
        // dtpTo
        // 
        dtpTo.BackColor = System.Drawing.Color.White;
        dtpTo.Font = new Font("Tahoma", 9F);
        dtpTo.Format = DateTimePickerFormat.Short;
        dtpTo.Location = new Point(916, 66);
        dtpTo.Name = "dtpTo";
        dtpTo.Size = new Size(128, 29);
        dtpTo.TabIndex = 6;
        dtpTo.ValueChanged += dtpTo_ValueChanged;
        // 
        // btnRangeToday
        // 
        btnRangeToday.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(248), Convert.ToInt32(249), Convert.ToInt32(250));
        btnRangeToday.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(Convert.ToInt32(205), Convert.ToInt32(225), Convert.ToInt32(205));
        btnRangeToday.FlatStyle = FlatStyle.Flat;
        btnRangeToday.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        btnRangeToday.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(30), Convert.ToInt32(41), Convert.ToInt32(59));
        btnRangeToday.Location = new Point(806, 66);
        btnRangeToday.Name = "btnRangeToday";
        btnRangeToday.Size = new Size(100, 30);
        btnRangeToday.TabIndex = 7;
        btnRangeToday.Text = "اليوم";
        btnRangeToday.UseVisualStyleBackColor = false;
        btnRangeToday.Click += btnRangeToday_Click;
        // 
        // btnRangeMonth
        // 
        btnRangeMonth.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(248), Convert.ToInt32(249), Convert.ToInt32(250));
        btnRangeMonth.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(Convert.ToInt32(205), Convert.ToInt32(225), Convert.ToInt32(205));
        btnRangeMonth.FlatStyle = FlatStyle.Flat;
        btnRangeMonth.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        btnRangeMonth.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(30), Convert.ToInt32(41), Convert.ToInt32(59));
        btnRangeMonth.Location = new Point(686, 66);
        btnRangeMonth.Name = "btnRangeMonth";
        btnRangeMonth.Size = new Size(110, 30);
        btnRangeMonth.TabIndex = 8;
        btnRangeMonth.Text = "هذا الشهر";
        btnRangeMonth.UseVisualStyleBackColor = false;
        btnRangeMonth.Click += btnRangeMonth_Click;
        // 
        // btnRange30
        // 
        btnRange30.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(248), Convert.ToInt32(249), Convert.ToInt32(250));
        btnRange30.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(Convert.ToInt32(205), Convert.ToInt32(225), Convert.ToInt32(205));
        btnRange30.FlatStyle = FlatStyle.Flat;
        btnRange30.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        btnRange30.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(30), Convert.ToInt32(41), Convert.ToInt32(59));
        btnRange30.Location = new Point(551, 66);
        btnRange30.Name = "btnRange30";
        btnRange30.Size = new Size(125, 30);
        btnRange30.TabIndex = 9;
        btnRange30.Text = "آخر 30 يوماً";
        btnRange30.UseVisualStyleBackColor = false;
        btnRange30.Click += btnRange30_Click;
        // 
        // btnClearFilters
        // 
        btnClearFilters.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(108), Convert.ToInt32(117), Convert.ToInt32(125));
        btnClearFilters.Cursor = Cursors.Hand;
        btnClearFilters.Enabled = false;
        btnClearFilters.FlatAppearance.BorderSize = 0;
        btnClearFilters.FlatStyle = FlatStyle.Flat;
        btnClearFilters.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        btnClearFilters.ForeColor = System.Drawing.Color.White;
        btnClearFilters.Location = new Point(340, 66);
        btnClearFilters.Name = "btnClearFilters";
        btnClearFilters.Size = new Size(200, 32);
        btnClearFilters.TabIndex = 10;
        btnClearFilters.Text = "\U0001f9f9 مسح التصفية";
        btnClearFilters.UseVisualStyleBackColor = false;
        btnClearFilters.Click += btnClearFilters_Click;
        // 
        // btnToggleFilters
        // 
        btnToggleFilters.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(248), Convert.ToInt32(249), Convert.ToInt32(250));
        btnToggleFilters.Cursor = Cursors.Hand;
        btnToggleFilters.Dock = DockStyle.Left;
        btnToggleFilters.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(Convert.ToInt32(205), Convert.ToInt32(225), Convert.ToInt32(205));
        btnToggleFilters.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(240), Convert.ToInt32(240), Convert.ToInt32(240));
        btnToggleFilters.FlatStyle = FlatStyle.Popup;
        btnToggleFilters.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        btnToggleFilters.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
        btnToggleFilters.Location = new Point(6, 4);
        btnToggleFilters.Name = "btnToggleFilters";
        btnToggleFilters.Size = new Size(140, 104);
        btnToggleFilters.TabIndex = 11;
        btnToggleFilters.Text = "إخفاء التصفية";
        btnToggleFilters.UseVisualStyleBackColor = false;
        // 
        // pnlGrid
        // 
        pnlGrid.Controls.Add(dgvMain);
        pnlGrid.Controls.Add(lblEmpty);
        pnlGrid.Dock = DockStyle.Fill;
        pnlGrid.Location = new Point(0, 432);
        pnlGrid.Margin = new Padding(0);
        pnlGrid.Name = "pnlGrid";
        pnlGrid.Padding = new Padding(0, 10, 0, 0);
        pnlGrid.Size = new Size(1432, 316);
        pnlGrid.TabIndex = 4;
        // 
        // dgvMain
        // 
        dgvMain.AllowUserToAddRows = false;
        dgvMain.AllowUserToDeleteRows = false;
        dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(245), Convert.ToInt32(252), Convert.ToInt32(245));
        dgvMain.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
        dgvMain.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvMain.BackgroundColor = System.Drawing.Color.FromArgb(Convert.ToInt32(220), Convert.ToInt32(240), Convert.ToInt32(220));
        dgvMain.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
        dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
        dataGridViewCellStyle5.Font = new Font("Tahoma", 10F);
        dataGridViewCellStyle5.ForeColor = System.Drawing.Color.White;
        dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
        dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.White;
        dataGridViewCellStyle5.WrapMode = DataGridViewTriState.True;
        dgvMain.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
        dgvMain.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvMain.ContextMenuStrip = cmsRows;
        dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
        dataGridViewCellStyle6.Font = new Font("Tahoma", 10F);
        dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(255), Convert.ToInt32(255), Convert.ToInt32(180));
        dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        dataGridViewCellStyle6.WrapMode = DataGridViewTriState.False;
        dgvMain.DefaultCellStyle = dataGridViewCellStyle6;
        dgvMain.Dock = DockStyle.Fill;
        dgvMain.EnableHeadersVisualStyles = false;
        dgvMain.GridColor = System.Drawing.Color.FromArgb(Convert.ToInt32(205), Convert.ToInt32(225), Convert.ToInt32(205));
        dgvMain.Location = new Point(0, 10);
        dgvMain.MultiSelect = false;
        dgvMain.Name = "dgvMain";
        dgvMain.ReadOnly = true;
        dgvMain.RowHeadersVisible = false;
        dgvMain.RowHeadersWidth = 62;
        dgvMain.RowTemplate.Height = 32;
        dgvMain.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvMain.Size = new Size(1432, 306);
        dgvMain.TabIndex = 0;
        dgvMain.CellClick += dgvMain_CellClick;
        dgvMain.DoubleClick += dgvMain_DoubleClick;
        // 
        // lblEmpty
        // 
        lblEmpty.BackColor = System.Drawing.Color.Transparent;
        lblEmpty.Dock = DockStyle.Fill;
        lblEmpty.Font = new Font("Tahoma", 12F, FontStyle.Bold);
        lblEmpty.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblEmpty.Location = new Point(0, 10);
        lblEmpty.Name = "lblEmpty";
        lblEmpty.Size = new Size(1432, 306);
        lblEmpty.TabIndex = 1;
        lblEmpty.Text = "لا توجد بيانات للعرض";
        lblEmpty.TextAlign = ContentAlignment.MiddleCenter;
        lblEmpty.Visible = false;
        // 
        // pnlSummary
        // 
        pnlSummary.Controls.Add(lblSummary);
        pnlSummary.Dock = DockStyle.Bottom;
        pnlSummary.Location = new Point(0, 748);
        pnlSummary.Margin = new Padding(0);
        pnlSummary.Name = "pnlSummary";
        pnlSummary.Size = new Size(1432, 32);
        pnlSummary.TabIndex = 5;
        // 
        // lblSummary
        // 
        lblSummary.Dock = DockStyle.Fill;
        lblSummary.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblSummary.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
        lblSummary.Location = new Point(0, 0);
        lblSummary.Name = "lblSummary";
        lblSummary.Size = new Size(1432, 32);
        lblSummary.TabIndex = 0;
        lblSummary.Text = "المعروض: 0 من 0 — الإجمالي: 0.00";
        lblSummary.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // toolTip
        // 
        toolTip.AutomaticDelay = 300;
        toolTip.BackColor = System.Drawing.Color.White;
        toolTip.ForeColor = System.Drawing.Color.Black;
        // 
        // liveTip
        // 
        liveTip.AutomaticDelay = 300;
        liveTip.BackColor = System.Drawing.Color.White;
        liveTip.ForeColor = System.Drawing.Color.Black;
        // 
        // errorProvider
        // 
        errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        errorProvider.ContainerControl = this;
        errorProvider.RightToLeft = true;
        // 
        // frmExpensesMaster
        // 
        AutoScaleDimensions = new SizeF(11F, 24F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(170), Convert.ToInt32(220), Convert.ToInt32(170));
        ClientSize = new Size(1432, 780);
        Controls.Add(pnlGrid);
        Controls.Add(pnlSummary);
        Controls.Add(pnlFilters);
        Controls.Add(pnlButtons);
        Controls.Add(pnlFields);
        Controls.Add(lblMode);
        Font = new Font("Tahoma", 10F);
        Name = "frmExpensesMaster";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterParent;
        Text = "إدارة المصروفات";
        Load += frmExpensesMaster_Load;
        cmsRows.ResumeLayout(false);
        pnlFields.ResumeLayout(false);
        gbDocument.ResumeLayout(false);
        gbDocument.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numAmount).EndInit();
        pnlButtons.ResumeLayout(false);
        pnlFilters.ResumeLayout(false);
        gbFilter.ResumeLayout(false);
        gbFilter.PerformLayout();
        pnlGrid.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvMain).EndInit();
        pnlSummary.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private System.Windows.Forms.Label lblMode;
    private System.Windows.Forms.Panel pnlFields;
    private System.Windows.Forms.GroupBox gbDocument;
    private System.Windows.Forms.Label lblExpenseNo;
    private System.Windows.Forms.TextBox txtExpenseNo;
    private System.Windows.Forms.Label lblDate;
    private System.Windows.Forms.DateTimePicker dtpDate;
    private System.Windows.Forms.Label lblCashBox;
    private System.Windows.Forms.Label lblCashBoxReq;
    private System.Windows.Forms.ComboBox cmbCashBox;
    private System.Windows.Forms.Label lblAmount;
    private System.Windows.Forms.Label lblAmountReq;
    private System.Windows.Forms.NumericUpDown numAmount;
    private System.Windows.Forms.Label lblNetTotal;
    private System.Windows.Forms.Label lblNetTotalValue;
    private System.Windows.Forms.Label lblStatus;
    private System.Windows.Forms.Label lblStatusValue;
    private System.Windows.Forms.Label lblNotes;
    private System.Windows.Forms.Label lblNotesReq;
    private System.Windows.Forms.TextBox txtNotes;
    private System.Windows.Forms.FlowLayoutPanel pnlButtons;
    private System.Windows.Forms.Button btnNew;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.Button btnEdit;
    private System.Windows.Forms.Button btnDelete;
    private System.Windows.Forms.Button btnPost;
    private System.Windows.Forms.Button btnCancelDocument;
    private System.Windows.Forms.Button btnRefresh;
    private System.Windows.Forms.Button btnClose;
    private System.Windows.Forms.Panel pnlFilters;
    private System.Windows.Forms.GroupBox gbFilter;
    private System.Windows.Forms.Label lblFilterBy;
    private System.Windows.Forms.ComboBox cbFilterBy;
    private System.Windows.Forms.TextBox txtFilterValue;
    private System.Windows.Forms.Label lblStatusFilter;
    private System.Windows.Forms.ComboBox cbStatus;
    private System.Windows.Forms.CheckBox chkOnlyCancelled;
    private System.Windows.Forms.Label lblFrom;
    private System.Windows.Forms.DateTimePicker dtpFrom;
    private System.Windows.Forms.Label lblTo;
    private System.Windows.Forms.DateTimePicker dtpTo;
    private System.Windows.Forms.Button btnRangeToday;
    private System.Windows.Forms.Button btnRangeMonth;
    private System.Windows.Forms.Button btnRange30;
    private System.Windows.Forms.Button btnClearFilters;
    private System.Windows.Forms.Button btnToggleFilters;
    private System.Windows.Forms.Panel pnlGrid;
    private System.Windows.Forms.DataGridView dgvMain;
    private System.Windows.Forms.DataGridViewTextBoxColumn colId;
    private System.Windows.Forms.DataGridViewTextBoxColumn colExpenseNo;
    private System.Windows.Forms.DataGridViewTextBoxColumn colExpenseDate;
    private System.Windows.Forms.DataGridViewTextBoxColumn colCashBoxName;
    private System.Windows.Forms.DataGridViewTextBoxColumn colAmount;
    private System.Windows.Forms.DataGridViewTextBoxColumn colNotes;
    private System.Windows.Forms.DataGridViewTextBoxColumn colStatusName;
    private System.Windows.Forms.Label lblEmpty;
    private System.Windows.Forms.Panel pnlSummary;
    private System.Windows.Forms.Label lblSummary;
    private System.Windows.Forms.ContextMenuStrip cmsRows;
    private System.Windows.Forms.ToolStripMenuItem cmsView;
    private System.Windows.Forms.ToolStripMenuItem cmsPost;
    private System.Windows.Forms.ToolStripMenuItem cmsCancel;
    private System.Windows.Forms.ToolStripMenuItem cmsDelete;
    private System.Windows.Forms.ToolStripSeparator cmsSeparator;
    private System.Windows.Forms.ToolStripMenuItem cmsRefresh;
    private System.Windows.Forms.ToolTip toolTip;
    private System.Windows.Forms.ToolTip liveTip;
    private System.Windows.Forms.ErrorProvider errorProvider;
}
