namespace SalesSystem.Desktop.Screens.Returns.Forms
{
    partial class frmListReturns
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
            components = new System.ComponentModel.Container();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            pnlHeader = new Panel();
            lblMode = new Label();
            pnlToolbar = new Panel();
            flpToolbar = new FlowLayoutPanel();
            btnAdd = new Button();
            btnView = new Button();
            btnRefresh = new Button();
            btnClose = new Button();
            pnlFilters = new Panel();
            gbFilter = new GroupBox();
            label1 = new Label();
            cbStatus = new ComboBox();
            label3 = new Label();
            dtpFrom = new DateTimePicker();
            label4 = new Label();
            dtpTo = new DateTimePicker();
            label2 = new Label();
            cbFilterBy = new ComboBox();
            txtFilterValue = new TextBox();
            chkOnlyCancelled = new CheckBox();
            btnRangeToday = new Button();
            btnRangeMonth = new Button();
            btnRange30 = new Button();
            btnClearFilters = new Button();
            btnToggleFilters = new Button();
            pnlSummary = new Panel();
            flpSummary = new FlowLayoutPanel();
            lblCountCaption = new Label();
            lblRecordsCount = new Label();
            lblSalesCaption = new Label();
            lblSalesTotal = new Label();
            lblPurchaseCaption = new Label();
            lblPurchaseTotal = new Label();
            lblSummary = new Label();
            pnlFooter = new Panel();
            lblHint = new Label();
            tabReturns = new TabControl();
            tpSales = new TabPage();
            lblNoSales = new Label();
            dgvSales = new DataGridView();
            colSalesReturnNo = new DataGridViewTextBoxColumn();
            colSalesDate = new DataGridViewTextBoxColumn();
            colSalesCustomer = new DataGridViewTextBoxColumn();
            colSalesTotal = new DataGridViewTextBoxColumn();
            colSalesStatus = new DataGridViewTextBoxColumn();
            tpPurchases = new TabPage();
            lblNoPurchases = new Label();
            dgvPurchases = new DataGridView();
            colPurchaseReturnNo = new DataGridViewTextBoxColumn();
            colPurchaseDate = new DataGridViewTextBoxColumn();
            colPurchaseSupplier = new DataGridViewTextBoxColumn();
            colPurchaseTotal = new DataGridViewTextBoxColumn();
            colPurchaseStatus = new DataGridViewTextBoxColumn();
            ctxMenu = new ContextMenuStrip(components);
            tsmiOpen = new ToolStripMenuItem();
            tsSep1 = new ToolStripSeparator();
            tsmiRefresh = new ToolStripMenuItem();
            toolTip = new ToolTip(components);
            pnlHeader.SuspendLayout();
            pnlToolbar.SuspendLayout();
            flpToolbar.SuspendLayout();
            pnlFilters.SuspendLayout();
            gbFilter.SuspendLayout();
            pnlSummary.SuspendLayout();
            flpSummary.SuspendLayout();
            pnlFooter.SuspendLayout();
            tabReturns.SuspendLayout();
            tpSales.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSales).BeginInit();
            tpPurchases.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPurchases).BeginInit();
            ctxMenu.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
            pnlHeader.Controls.Add(lblMode);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Margin = new Padding(0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1564, 56);
            pnlHeader.TabIndex = 0;
            // 
            // lblMode
            // 
            lblMode.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
            lblMode.Dock = DockStyle.Fill;
            lblMode.Font = new Font("Tahoma", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblMode.ForeColor = System.Drawing.Color.White;
            lblMode.Location = new Point(0, 0);
            lblMode.Margin = new Padding(0);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(1564, 56);
            lblMode.TabIndex = 0;
            lblMode.Text = "إدارة المرتجعات";
            lblMode.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlToolbar
            // 
            pnlToolbar.BackColor = System.Drawing.Color.White;
            pnlToolbar.Controls.Add(flpToolbar);
            pnlToolbar.Dock = DockStyle.Top;
            pnlToolbar.Location = new Point(0, 56);
            pnlToolbar.Margin = new Padding(0);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.Size = new Size(1564, 64);
            pnlToolbar.TabIndex = 1;
            // 
            // flpToolbar
            // 
            flpToolbar.BackColor = System.Drawing.Color.White;
            flpToolbar.Controls.Add(btnAdd);
            flpToolbar.Controls.Add(btnView);
            flpToolbar.Controls.Add(btnRefresh);
            flpToolbar.Controls.Add(btnClose);
            flpToolbar.Dock = DockStyle.Fill;
            flpToolbar.Location = new Point(0, 0);
            flpToolbar.Margin = new Padding(0);
            flpToolbar.Name = "flpToolbar";
            flpToolbar.Padding = new Padding(10);
            flpToolbar.Size = new Size(1564, 64);
            flpToolbar.TabIndex = 0;
            flpToolbar.WrapContents = false;
            // 
            // btnAdd
            // 
            btnAdd.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(13), Convert.ToInt32(110), Convert.ToInt32(253));
            btnAdd.Cursor = Cursors.Hand;
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAdd.ForeColor = System.Drawing.Color.White;
            btnAdd.Location = new Point(1401, 13);
            btnAdd.Margin = new Padding(4, 0, 4, 0);
            btnAdd.Name = "btnAdd";
            btnAdd.Padding = new Padding(12, 0, 12, 0);
            btnAdd.Size = new Size(140, 40);
            btnAdd.TabIndex = 0;
            btnAdd.Text = "مرتجع جديد";
            btnAdd.UseVisualStyleBackColor = false;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnView
            // 
            btnView.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(248), Convert.ToInt32(249), Convert.ToInt32(250));
            btnView.Cursor = Cursors.Hand;
            btnView.Enabled = false;
            btnView.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(Convert.ToInt32(205), Convert.ToInt32(225), Convert.ToInt32(205));
            btnView.FlatAppearance.BorderSize = 1;
            btnView.FlatStyle = FlatStyle.Flat;
            btnView.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnView.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
            btnView.Location = new Point(1253, 13);
            btnView.Margin = new Padding(4, 0, 4, 0);
            btnView.Name = "btnView";
            btnView.Padding = new Padding(12, 0, 12, 0);
            btnView.Size = new Size(140, 40);
            btnView.TabIndex = 1;
            btnView.Text = "عرض / تعديل";
            btnView.UseVisualStyleBackColor = false;
            btnView.Click += btnView_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(108), Convert.ToInt32(117), Convert.ToInt32(125));
            btnRefresh.Cursor = Cursors.Hand;
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnRefresh.ForeColor = System.Drawing.Color.White;
            btnRefresh.Location = new Point(1105, 13);
            btnRefresh.Margin = new Padding(4, 0, 4, 0);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Padding = new Padding(12, 0, 12, 0);
            btnRefresh.Size = new Size(140, 40);
            btnRefresh.TabIndex = 2;
            btnRefresh.Text = "تحديث";
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // btnClose
            // 
            btnClose.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(108), Convert.ToInt32(117), Convert.ToInt32(125));
            btnClose.Cursor = Cursors.Hand;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnClose.ForeColor = System.Drawing.Color.White;
            btnClose.Location = new Point(957, 13);
            btnClose.Margin = new Padding(4, 0, 4, 0);
            btnClose.Name = "btnClose";
            btnClose.Padding = new Padding(12, 0, 12, 0);
            btnClose.Size = new Size(140, 40);
            btnClose.TabIndex = 3;
            btnClose.Text = "إغلاق";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // pnlFilters
            // 
            pnlFilters.BackColor = System.Drawing.Color.White;
            pnlFilters.Controls.Add(gbFilter);
            pnlFilters.Controls.Add(btnToggleFilters);
            pnlFilters.Dock = DockStyle.Top;
            pnlFilters.Location = new Point(0, 120);
            pnlFilters.Margin = new Padding(0);
            pnlFilters.Name = "pnlFilters";
            pnlFilters.Padding = new Padding(6, 4, 6, 4);
            pnlFilters.Size = new Size(1564, 110);
            pnlFilters.TabIndex = 2;
            // 
            // gbFilter
            // 
            gbFilter.BackColor = System.Drawing.Color.White;
            gbFilter.Controls.Add(label1);
            gbFilter.Controls.Add(cbStatus);
            gbFilter.Controls.Add(label3);
            gbFilter.Controls.Add(dtpFrom);
            gbFilter.Controls.Add(label4);
            gbFilter.Controls.Add(dtpTo);
            gbFilter.Controls.Add(label2);
            gbFilter.Controls.Add(cbFilterBy);
            gbFilter.Controls.Add(txtFilterValue);
            gbFilter.Controls.Add(chkOnlyCancelled);
            gbFilter.Controls.Add(btnRangeToday);
            gbFilter.Controls.Add(btnRangeMonth);
            gbFilter.Controls.Add(btnRange30);
            gbFilter.Controls.Add(btnClearFilters);
            gbFilter.Dock = DockStyle.Fill;
            gbFilter.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            gbFilter.Location = new Point(146, 4);
            gbFilter.Margin = new Padding(0);
            gbFilter.Name = "gbFilter";
            gbFilter.Padding = new Padding(10);
            gbFilter.Size = new Size(1412, 102);
            gbFilter.TabIndex = 0;
            gbFilter.TabStop = false;
            gbFilter.Text = "بحث وتصفية";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            label1.Location = new Point(1157, 67);
            label1.Margin = new Padding(0);
            label1.Name = "label1";
            label1.Size = new Size(72, 24);
            label1.TabIndex = 0;
            label1.Text = "الحالة:";
            label1.TextAlign = ContentAlignment.MiddleRight;
            // 
            // cbStatus
            // 
            cbStatus.BackColor = System.Drawing.Color.White;
            cbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cbStatus.FlatStyle = FlatStyle.Flat;
            cbStatus.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cbStatus.FormattingEnabled = true;
            cbStatus.Location = new Point(967, 65);
            cbStatus.Margin = new Padding(0);
            cbStatus.Name = "cbStatus";
            cbStatus.Size = new Size(190, 32);
            cbStatus.TabIndex = 1;
            cbStatus.SelectedIndexChanged += cbStatus_SelectedIndexChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            label3.Location = new Point(699, 34);
            label3.Margin = new Padding(0);
            label3.Name = "label3";
            label3.Size = new Size(46, 24);
            label3.TabIndex = 2;
            label3.Text = "من:";
            label3.TextAlign = ContentAlignment.MiddleRight;
            // 
            // dtpFrom
            // 
            dtpFrom.BackColor = System.Drawing.Color.White;
            dtpFrom.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dtpFrom.Format = DateTimePickerFormat.Short;
            dtpFrom.Location = new Point(539, 30);
            dtpFrom.Margin = new Padding(0);
            dtpFrom.Name = "dtpFrom";
            dtpFrom.Size = new Size(154, 32);
            dtpFrom.TabIndex = 3;
            dtpFrom.ValueChanged += dtpFrom_ValueChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            label4.Location = new Point(479, 34);
            label4.Margin = new Padding(0);
            label4.Name = "label4";
            label4.Size = new Size(52, 24);
            label4.TabIndex = 4;
            label4.Text = "إلى:";
            label4.TextAlign = ContentAlignment.MiddleRight;
            // 
            // dtpTo
            // 
            dtpTo.BackColor = System.Drawing.Color.White;
            dtpTo.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dtpTo.Format = DateTimePickerFormat.Short;
            dtpTo.Location = new Point(319, 30);
            dtpTo.Margin = new Padding(0);
            dtpTo.Name = "dtpTo";
            dtpTo.Size = new Size(154, 32);
            dtpTo.TabIndex = 5;
            dtpTo.ValueChanged += dtpTo_ValueChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            label2.Location = new Point(1157, 34);
            label2.Margin = new Padding(0);
            label2.Name = "label2";
            label2.Size = new Size(95, 24);
            label2.TabIndex = 6;
            label2.Text = "تصفية بـ:";
            label2.TextAlign = ContentAlignment.MiddleRight;
            // 
            // cbFilterBy
            // 
            cbFilterBy.BackColor = System.Drawing.Color.White;
            cbFilterBy.DropDownStyle = ComboBoxStyle.DropDownList;
            cbFilterBy.FlatStyle = FlatStyle.Flat;
            cbFilterBy.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cbFilterBy.FormattingEnabled = true;
            cbFilterBy.Items.AddRange(new object[] { "العميل/المورد", "رقم المرتجع", "البيان", "المبلغ" });
            cbFilterBy.Location = new Point(967, 30);
            cbFilterBy.Margin = new Padding(0);
            cbFilterBy.Name = "cbFilterBy";
            cbFilterBy.Size = new Size(184, 32);
            cbFilterBy.TabIndex = 7;
            cbFilterBy.SelectedIndexChanged += cbFilterBy_SelectedIndexChanged;
            // 
            // txtFilterValue
            // 
            txtFilterValue.BackColor = System.Drawing.Color.White;
            txtFilterValue.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtFilterValue.Location = new Point(763, 30);
            txtFilterValue.Margin = new Padding(0);
            txtFilterValue.Name = "txtFilterValue";
            txtFilterValue.Size = new Size(200, 32);
            txtFilterValue.TabIndex = 8;
            txtFilterValue.TextChanged += txtFilterValue_TextChanged;
            txtFilterValue.KeyPress += txtFilterValue_KeyPress;
            // 
            // chkOnlyCancelled
            // 
            chkOnlyCancelled.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            chkOnlyCancelled.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            chkOnlyCancelled.Location = new Point(803, 69);
            chkOnlyCancelled.Margin = new Padding(0);
            chkOnlyCancelled.Name = "chkOnlyCancelled";
            chkOnlyCancelled.Size = new Size(160, 28);
            chkOnlyCancelled.TabIndex = 9;
            chkOnlyCancelled.Text = "الملغاة فقط";
            chkOnlyCancelled.UseVisualStyleBackColor = false;
            chkOnlyCancelled.CheckedChanged += chkOnlyCancelled_CheckedChanged;
            // 
            // btnRangeToday
            // 
            btnRangeToday.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(108), Convert.ToInt32(117), Convert.ToInt32(125));
            btnRangeToday.Cursor = Cursors.Hand;
            btnRangeToday.FlatAppearance.BorderSize = 0;
            btnRangeToday.FlatStyle = FlatStyle.Flat;
            btnRangeToday.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnRangeToday.ForeColor = System.Drawing.Color.White;
            btnRangeToday.Location = new Point(645, 65);
            btnRangeToday.Margin = new Padding(4, 0, 4, 0);
            btnRangeToday.Name = "btnRangeToday";
            btnRangeToday.Padding = new Padding(12, 0, 12, 0);
            btnRangeToday.Size = new Size(100, 30);
            btnRangeToday.TabIndex = 10;
            btnRangeToday.Text = "اليوم";
            btnRangeToday.UseVisualStyleBackColor = false;
            btnRangeToday.Click += btnRangeToday_Click;
            // 
            // btnRangeMonth
            // 
            btnRangeMonth.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(108), Convert.ToInt32(117), Convert.ToInt32(125));
            btnRangeMonth.Cursor = Cursors.Hand;
            btnRangeMonth.FlatAppearance.BorderSize = 0;
            btnRangeMonth.FlatStyle = FlatStyle.Flat;
            btnRangeMonth.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnRangeMonth.ForeColor = System.Drawing.Color.White;
            btnRangeMonth.Location = new Point(371, 65);
            btnRangeMonth.Margin = new Padding(4, 0, 4, 0);
            btnRangeMonth.Name = "btnRangeMonth";
            btnRangeMonth.Padding = new Padding(12, 0, 12, 0);
            btnRangeMonth.Size = new Size(131, 30);
            btnRangeMonth.TabIndex = 11;
            btnRangeMonth.Text = "هذا الشهر";
            btnRangeMonth.UseVisualStyleBackColor = false;
            btnRangeMonth.Click += btnRangeMonth_Click;
            // 
            // btnRange30
            // 
            btnRange30.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(108), Convert.ToInt32(117), Convert.ToInt32(125));
            btnRange30.Cursor = Cursors.Hand;
            btnRange30.FlatAppearance.BorderSize = 0;
            btnRange30.FlatStyle = FlatStyle.Flat;
            btnRange30.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnRange30.ForeColor = System.Drawing.Color.White;
            btnRange30.Location = new Point(508, 65);
            btnRange30.Margin = new Padding(4, 0, 4, 0);
            btnRange30.Name = "btnRange30";
            btnRange30.Padding = new Padding(12, 0, 12, 0);
            btnRange30.Size = new Size(128, 30);
            btnRange30.TabIndex = 12;
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
            btnClearFilters.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnClearFilters.ForeColor = System.Drawing.Color.White;
            btnClearFilters.Location = new Point(116, 30);
            btnClearFilters.Margin = new Padding(0);
            btnClearFilters.Name = "btnClearFilters";
            btnClearFilters.Padding = new Padding(12, 0, 12, 0);
            btnClearFilters.Size = new Size(200, 32);
            btnClearFilters.TabIndex = 13;
            btnClearFilters.Text = "\U0001f9f9 مسح التصفية";
            toolTip.SetToolTip(btnClearFilters, "مسح كل أدوات التصفية (Esc) — لا يعيد تحميل البيانات");
            btnClearFilters.UseVisualStyleBackColor = false;
            btnClearFilters.Click += btnClearFilters_Click;
            // 
            // btnToggleFilters
            // 
            btnToggleFilters.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(248), Convert.ToInt32(249), Convert.ToInt32(250));
            btnToggleFilters.Cursor = Cursors.Hand;
            btnToggleFilters.Dock = DockStyle.Left;
            btnToggleFilters.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(Convert.ToInt32(205), Convert.ToInt32(225), Convert.ToInt32(205));
            btnToggleFilters.FlatStyle = FlatStyle.Popup;
            btnToggleFilters.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnToggleFilters.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            btnToggleFilters.Location = new Point(6, 4);
            btnToggleFilters.Margin = new Padding(0);
            btnToggleFilters.Name = "btnToggleFilters";
            btnToggleFilters.Size = new Size(140, 102);
            btnToggleFilters.TabIndex = 13;
            btnToggleFilters.Text = "إخفاء التصفية";
            btnToggleFilters.UseVisualStyleBackColor = false;
            // 
            // pnlSummary
            // 
            pnlSummary.BackColor = System.Drawing.Color.White;
            pnlSummary.Controls.Add(flpSummary);
            pnlSummary.Dock = DockStyle.Bottom;
            pnlSummary.Location = new Point(0, 757);
            pnlSummary.Margin = new Padding(0);
            pnlSummary.Name = "pnlSummary";
            pnlSummary.Size = new Size(1564, 46);
            pnlSummary.TabIndex = 3;
            // 
            // flpSummary
            // 
            flpSummary.BackColor = System.Drawing.Color.White;
            flpSummary.Controls.Add(lblCountCaption);
            flpSummary.Controls.Add(lblRecordsCount);
            flpSummary.Controls.Add(lblSalesCaption);
            flpSummary.Controls.Add(lblSalesTotal);
            flpSummary.Controls.Add(lblPurchaseCaption);
            flpSummary.Controls.Add(lblPurchaseTotal);
            flpSummary.Controls.Add(lblSummary);
            flpSummary.Dock = DockStyle.Fill;
            flpSummary.Location = new Point(0, 0);
            flpSummary.Margin = new Padding(0);
            flpSummary.Name = "flpSummary";
            flpSummary.Padding = new Padding(10, 8, 10, 8);
            flpSummary.Size = new Size(1564, 46);
            flpSummary.TabIndex = 0;
            flpSummary.WrapContents = false;
            // 
            // lblCountCaption
            // 
            lblCountCaption.AutoSize = true;
            lblCountCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCountCaption.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            lblCountCaption.Location = new Point(1391, 8);
            lblCountCaption.Margin = new Padding(4, 0, 4, 0);
            lblCountCaption.Name = "lblCountCaption";
            lblCountCaption.Size = new Size(141, 24);
            lblCountCaption.TabIndex = 0;
            lblCountCaption.Text = "عدد المرتجعات:";
            // 
            // lblRecordsCount
            // 
            lblRecordsCount.AutoSize = true;
            lblRecordsCount.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblRecordsCount.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            lblRecordsCount.Location = new Point(1360, 8);
            lblRecordsCount.Margin = new Padding(4, 0, 24, 0);
            lblRecordsCount.Name = "lblRecordsCount";
            lblRecordsCount.Size = new Size(23, 24);
            lblRecordsCount.TabIndex = 1;
            lblRecordsCount.Text = "0";
            // 
            // lblSalesCaption
            // 
            lblSalesCaption.AutoSize = true;
            lblSalesCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSalesCaption.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            lblSalesCaption.Location = new Point(1163, 8);
            lblSalesCaption.Margin = new Padding(4, 0, 4, 0);
            lblSalesCaption.Name = "lblSalesCaption";
            lblSalesCaption.Size = new Size(189, 24);
            lblSalesCaption.TabIndex = 2;
            lblSalesCaption.Text = "إجمالي مرتجعات المبيعات:";
            // 
            // lblSalesTotal
            // 
            lblSalesTotal.AutoSize = true;
            lblSalesTotal.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSalesTotal.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            lblSalesTotal.Location = new Point(1100, 8);
            lblSalesTotal.Margin = new Padding(4, 0, 24, 0);
            lblSalesTotal.Name = "lblSalesTotal";
            lblSalesTotal.Size = new Size(55, 24);
            lblSalesTotal.TabIndex = 3;
            lblSalesTotal.Text = "0.00";
            // 
            // lblPurchaseCaption
            // 
            lblPurchaseCaption.AutoSize = true;
            lblPurchaseCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPurchaseCaption.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            lblPurchaseCaption.Location = new Point(879, 8);
            lblPurchaseCaption.Margin = new Padding(4, 0, 4, 0);
            lblPurchaseCaption.Name = "lblPurchaseCaption";
            lblPurchaseCaption.Size = new Size(213, 24);
            lblPurchaseCaption.TabIndex = 4;
            lblPurchaseCaption.Text = "إجمالي مرتجعات المشتريات:";
            // 
            // lblPurchaseTotal
            // 
            lblPurchaseTotal.AutoSize = true;
            lblPurchaseTotal.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPurchaseTotal.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            lblPurchaseTotal.Location = new Point(816, 8);
            lblPurchaseTotal.Margin = new Padding(4, 0, 24, 0);
            lblPurchaseTotal.Name = "lblPurchaseTotal";
            lblPurchaseTotal.Size = new Size(55, 24);
            lblPurchaseTotal.TabIndex = 5;
            lblPurchaseTotal.Text = "0.00";
            // 
            // lblSummary
            // 
            lblSummary.AutoSize = true;
            lblSummary.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSummary.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
            lblSummary.Location = new Point(672, 8);
            lblSummary.Margin = new Padding(4, 0, 4, 0);
            lblSummary.Name = "lblSummary";
            lblSummary.Size = new Size(140, 24);
            lblSummary.TabIndex = 6;
            lblSummary.Text = "المعروض: 0 من 0";
            // 
            // pnlFooter
            // 
            pnlFooter.BackColor = System.Drawing.Color.White;
            pnlFooter.Controls.Add(lblHint);
            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Location = new Point(0, 803);
            pnlFooter.Margin = new Padding(0);
            pnlFooter.Name = "pnlFooter";
            pnlFooter.Padding = new Padding(10, 0, 10, 0);
            pnlFooter.Size = new Size(1564, 58);
            pnlFooter.TabIndex = 5;
            // 
            // lblHint
            // 
            lblHint.Dock = DockStyle.Fill;
            lblHint.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblHint.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            lblHint.Location = new Point(10, 0);
            lblHint.Margin = new Padding(0);
            lblHint.Name = "lblHint";
            lblHint.Size = new Size(1544, 58);
            lblHint.TabIndex = 0;
            lblHint.Text = "F2 مرتجع جديد · F3 عرض/تعديل · F5 تحديث وإرجاع التصفية · Esc مسح التصفية/إغلاق · نقر مزدوج = فتح";
            lblHint.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tabReturns
            // 
            tabReturns.Controls.Add(tpSales);
            tabReturns.Controls.Add(tpPurchases);
            tabReturns.Dock = DockStyle.Fill;
            tabReturns.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tabReturns.Location = new Point(0, 230);
            tabReturns.Margin = new Padding(0);
            tabReturns.Name = "tabReturns";
            tabReturns.RightToLeft = RightToLeft.Yes;
            tabReturns.RightToLeftLayout = true;
            tabReturns.SelectedIndex = 0;
            tabReturns.Size = new Size(1564, 527);
            tabReturns.TabIndex = 4;
            tabReturns.SelectedIndexChanged += tabReturns_SelectedIndexChanged;
            // 
            // tpSales
            // 
            tpSales.BackColor = System.Drawing.Color.White;
            tpSales.Controls.Add(lblNoSales);
            tpSales.Controls.Add(dgvSales);
            tpSales.Location = new Point(4, 33);
            tpSales.Name = "tpSales";
            tpSales.Padding = new Padding(3);
            tpSales.Size = new Size(1556, 490);
            tpSales.TabIndex = 0;
            tpSales.Text = "مرتجعـات المبيعات";
            // 
            // lblNoSales
            // 
            lblNoSales.BackColor = System.Drawing.Color.Transparent;
            lblNoSales.Dock = DockStyle.Fill;
            lblNoSales.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNoSales.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            lblNoSales.Location = new Point(3, 3);
            lblNoSales.Margin = new Padding(0);
            lblNoSales.Name = "lblNoSales";
            lblNoSales.Size = new Size(1550, 484);
            lblNoSales.TabIndex = 1;
            lblNoSales.Text = "لا توجد مرتجعات مبيعات مطابقة للمعايير الحالية";
            lblNoSales.TextAlign = ContentAlignment.MiddleCenter;
            lblNoSales.Visible = false;
            // 
            // dgvSales
            // 
            dgvSales.AllowUserToAddRows = false;
            dgvSales.AllowUserToDeleteRows = false;
            dgvSales.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(245), Convert.ToInt32(252), Convert.ToInt32(245));
            dgvSales.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvSales.AutoGenerateColumns = false;
            dgvSales.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvSales.BackgroundColor = System.Drawing.Color.FromArgb(Convert.ToInt32(220), Convert.ToInt32(240), Convert.ToInt32(220));
            dgvSales.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
            dataGridViewCellStyle2.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvSales.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvSales.ColumnHeadersHeight = 45;
            dgvSales.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvSales.Columns.AddRange(new DataGridViewColumn[] { colSalesReturnNo, colSalesDate, colSalesCustomer, colSalesTotal, colSalesStatus });
            dgvSales.ContextMenuStrip = ctxMenu;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(255), Convert.ToInt32(255), Convert.ToInt32(180));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvSales.DefaultCellStyle = dataGridViewCellStyle3;
            dgvSales.Dock = DockStyle.Fill;
            dgvSales.EnableHeadersVisualStyles = false;
            dgvSales.GridColor = System.Drawing.Color.FromArgb(Convert.ToInt32(205), Convert.ToInt32(225), Convert.ToInt32(205));
            dgvSales.Location = new Point(3, 3);
            dgvSales.Margin = new Padding(0);
            dgvSales.MultiSelect = false;
            dgvSales.Name = "dgvSales";
            dgvSales.ReadOnly = true;
            dgvSales.RowHeadersVisible = false;
            dgvSales.RowHeadersWidth = 51;
            dgvSales.RowTemplate.Height = 32;
            dgvSales.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSales.Size = new Size(1550, 484);
            dgvSales.TabIndex = 0;
            dgvSales.TabStop = false;
            dgvSales.CellFormatting += dgvSales_CellFormatting;
            dgvSales.SelectionChanged += dgvSales_SelectionChanged;
            dgvSales.DoubleClick += dgvSales_DoubleClick;
            // 
            // colSalesReturnNo
            // 
            colSalesReturnNo.FillWeight = 10F;
            colSalesReturnNo.HeaderText = "رقم المرتجع";
            colSalesReturnNo.MinimumWidth = 90;
            colSalesReturnNo.Name = "colSalesReturnNo";
            colSalesReturnNo.ReadOnly = true;
            colSalesReturnNo.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colSalesDate
            // 
            colSalesDate.FillWeight = 12F;
            colSalesDate.HeaderText = "التاريخ";
            colSalesDate.MinimumWidth = 100;
            colSalesDate.Name = "colSalesDate";
            colSalesDate.ReadOnly = true;
            colSalesDate.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colSalesCustomer
            // 
            colSalesCustomer.FillWeight = 40F;
            colSalesCustomer.HeaderText = "العميل";
            colSalesCustomer.MinimumWidth = 160;
            colSalesCustomer.Name = "colSalesCustomer";
            colSalesCustomer.ReadOnly = true;
            // 
            // colSalesTotal
            // 
            colSalesTotal.FillWeight = 25F;
            colSalesTotal.HeaderText = "الإجمالي";
            colSalesTotal.MinimumWidth = 120;
            colSalesTotal.Name = "colSalesTotal";
            colSalesTotal.ReadOnly = true;
            colSalesTotal.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colSalesStatus
            // 
            colSalesStatus.FillWeight = 13F;
            colSalesStatus.HeaderText = "الحالة";
            colSalesStatus.MinimumWidth = 90;
            colSalesStatus.Name = "colSalesStatus";
            colSalesStatus.ReadOnly = true;
            colSalesStatus.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // tpPurchases
            // 
            tpPurchases.BackColor = System.Drawing.Color.White;
            tpPurchases.Controls.Add(lblNoPurchases);
            tpPurchases.Controls.Add(dgvPurchases);
            tpPurchases.Location = new Point(4, 33);
            tpPurchases.Name = "tpPurchases";
            tpPurchases.Padding = new Padding(3);
            tpPurchases.Size = new Size(1556, 490);
            tpPurchases.TabIndex = 1;
            tpPurchases.Text = "مرتجعـات المشتريات";
            // 
            // lblNoPurchases
            // 
            lblNoPurchases.BackColor = System.Drawing.Color.Transparent;
            lblNoPurchases.Dock = DockStyle.Fill;
            lblNoPurchases.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNoPurchases.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            lblNoPurchases.Location = new Point(3, 3);
            lblNoPurchases.Margin = new Padding(0);
            lblNoPurchases.Name = "lblNoPurchases";
            lblNoPurchases.Size = new Size(1550, 484);
            lblNoPurchases.TabIndex = 1;
            lblNoPurchases.Text = "لا توجد مرتجعات مشتريات مطابقة للمعايير الحالية";
            lblNoPurchases.TextAlign = ContentAlignment.MiddleCenter;
            lblNoPurchases.Visible = false;
            // 
            // dgvPurchases
            // 
            dgvPurchases.AllowUserToAddRows = false;
            dgvPurchases.AllowUserToDeleteRows = false;
            dgvPurchases.AllowUserToOrderColumns = true;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(245), Convert.ToInt32(252), Convert.ToInt32(245));
            dgvPurchases.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            dgvPurchases.AutoGenerateColumns = false;
            dgvPurchases.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPurchases.BackgroundColor = System.Drawing.Color.FromArgb(Convert.ToInt32(220), Convert.ToInt32(240), Convert.ToInt32(220));
            dgvPurchases.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
            dataGridViewCellStyle5.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.True;
            dgvPurchases.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            dgvPurchases.ColumnHeadersHeight = 45;
            dgvPurchases.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvPurchases.Columns.AddRange(new DataGridViewColumn[] { colPurchaseReturnNo, colPurchaseDate, colPurchaseSupplier, colPurchaseTotal, colPurchaseStatus });
            dgvPurchases.ContextMenuStrip = ctxMenu;
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(255), Convert.ToInt32(255), Convert.ToInt32(180));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.False;
            dgvPurchases.DefaultCellStyle = dataGridViewCellStyle6;
            dgvPurchases.Dock = DockStyle.Fill;
            dgvPurchases.EnableHeadersVisualStyles = false;
            dgvPurchases.GridColor = System.Drawing.Color.FromArgb(Convert.ToInt32(205), Convert.ToInt32(225), Convert.ToInt32(205));
            dgvPurchases.Location = new Point(3, 3);
            dgvPurchases.Margin = new Padding(0);
            dgvPurchases.MultiSelect = false;
            dgvPurchases.Name = "dgvPurchases";
            dgvPurchases.ReadOnly = true;
            dgvPurchases.RowHeadersVisible = false;
            dgvPurchases.RowHeadersWidth = 51;
            dgvPurchases.RowTemplate.Height = 32;
            dgvPurchases.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPurchases.Size = new Size(1550, 484);
            dgvPurchases.TabIndex = 0;
            dgvPurchases.TabStop = false;
            dgvPurchases.CellFormatting += dgvPurchases_CellFormatting;
            dgvPurchases.SelectionChanged += dgvPurchases_SelectionChanged;
            dgvPurchases.DoubleClick += dgvPurchases_DoubleClick;
            // 
            // colPurchaseReturnNo
            // 
            colPurchaseReturnNo.FillWeight = 10F;
            colPurchaseReturnNo.HeaderText = "رقم المرتجع";
            colPurchaseReturnNo.MinimumWidth = 90;
            colPurchaseReturnNo.Name = "colPurchaseReturnNo";
            colPurchaseReturnNo.ReadOnly = true;
            colPurchaseReturnNo.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colPurchaseDate
            // 
            colPurchaseDate.FillWeight = 12F;
            colPurchaseDate.HeaderText = "التاريخ";
            colPurchaseDate.MinimumWidth = 100;
            colPurchaseDate.Name = "colPurchaseDate";
            colPurchaseDate.ReadOnly = true;
            colPurchaseDate.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colPurchaseSupplier
            // 
            colPurchaseSupplier.FillWeight = 40F;
            colPurchaseSupplier.HeaderText = "المورد";
            colPurchaseSupplier.MinimumWidth = 160;
            colPurchaseSupplier.Name = "colPurchaseSupplier";
            colPurchaseSupplier.ReadOnly = true;
            // 
            // colPurchaseTotal
            // 
            colPurchaseTotal.FillWeight = 25F;
            colPurchaseTotal.HeaderText = "الإجمالي";
            colPurchaseTotal.MinimumWidth = 120;
            colPurchaseTotal.Name = "colPurchaseTotal";
            colPurchaseTotal.ReadOnly = true;
            colPurchaseTotal.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colPurchaseStatus
            // 
            colPurchaseStatus.FillWeight = 13F;
            colPurchaseStatus.HeaderText = "الحالة";
            colPurchaseStatus.MinimumWidth = 90;
            colPurchaseStatus.Name = "colPurchaseStatus";
            colPurchaseStatus.ReadOnly = true;
            colPurchaseStatus.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // ctxMenu
            // 
            ctxMenu.ImageScalingSize = new Size(24, 24);
            ctxMenu.Items.AddRange(new ToolStripItem[] { tsmiOpen, tsSep1, tsmiRefresh });
            ctxMenu.Name = "ctxMenu";
            ctxMenu.RightToLeft = RightToLeft.Yes;
            ctxMenu.Size = new Size(214, 74);
            ctxMenu.Opening += ctxMenu_Opening;
            // 
            // tsmiOpen
            // 
            tsmiOpen.Name = "tsmiOpen";
            tsmiOpen.ShortcutKeyDisplayString = "F3";
            tsmiOpen.Size = new Size(213, 32);
            tsmiOpen.Text = "فتح / تعديل";
            tsmiOpen.Click += tsmiOpen_Click;
            // 
            // tsSep1
            // 
            tsSep1.Name = "tsSep1";
            tsSep1.Size = new Size(210, 6);
            // 
            // tsmiRefresh
            // 
            tsmiRefresh.Name = "tsmiRefresh";
            tsmiRefresh.ShortcutKeyDisplayString = "F5";
            tsmiRefresh.Size = new Size(213, 32);
            tsmiRefresh.Text = "تحديث القائمة";
            tsmiRefresh.Click += tsmiRefresh_Click;
            // 
            // frmListReturns
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(170), Convert.ToInt32(220), Convert.ToInt32(170));
            ClientSize = new Size(1564, 861);
            Controls.Add(tabReturns);
            Controls.Add(pnlSummary);
            Controls.Add(pnlFooter);
            Controls.Add(pnlFilters);
            Controls.Add(pnlToolbar);
            Controls.Add(pnlHeader);
            Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            KeyPreview = true;
            MinimumSize = new Size(1200, 700);
            Name = "frmListReturns";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "إدارة المرتجعات";
            Load += frmListReturns_Load;
            pnlHeader.ResumeLayout(false);
            pnlToolbar.ResumeLayout(false);
            flpToolbar.ResumeLayout(false);
            pnlFilters.ResumeLayout(false);
            gbFilter.ResumeLayout(false);
            gbFilter.PerformLayout();
            pnlSummary.ResumeLayout(false);
            flpSummary.ResumeLayout(false);
            flpSummary.PerformLayout();
            pnlFooter.ResumeLayout(false);
            tabReturns.ResumeLayout(false);
            tpSales.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvSales).EndInit();
            tpPurchases.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvPurchases).EndInit();
            ctxMenu.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.Panel pnlToolbar;
        private System.Windows.Forms.FlowLayoutPanel flpToolbar;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel pnlFilters;
        private System.Windows.Forms.GroupBox gbFilter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbStatus;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbFilterBy;
        private System.Windows.Forms.TextBox txtFilterValue;
        private System.Windows.Forms.CheckBox chkOnlyCancelled;
        private System.Windows.Forms.Button btnRangeToday;
        private System.Windows.Forms.Button btnRangeMonth;
        private System.Windows.Forms.Button btnRange30;
        private System.Windows.Forms.Button btnClearFilters;
        private System.Windows.Forms.Button btnToggleFilters;
        private System.Windows.Forms.Panel pnlSummary;
        private System.Windows.Forms.FlowLayoutPanel flpSummary;
        private System.Windows.Forms.Label lblCountCaption;
        private System.Windows.Forms.Label lblRecordsCount;
        private System.Windows.Forms.Label lblSalesCaption;
        private System.Windows.Forms.Label lblSalesTotal;
        private System.Windows.Forms.Label lblPurchaseCaption;
        private System.Windows.Forms.Label lblPurchaseTotal;
        private System.Windows.Forms.Label lblSummary;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Label lblHint;
        private System.Windows.Forms.TabControl tabReturns;
        private System.Windows.Forms.TabPage tpSales;
        private System.Windows.Forms.Label lblNoSales;
        private System.Windows.Forms.DataGridView dgvSales;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSalesReturnNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSalesDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSalesCustomer;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSalesTotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSalesStatus;
        private System.Windows.Forms.TabPage tpPurchases;
        private System.Windows.Forms.Label lblNoPurchases;
        private System.Windows.Forms.DataGridView dgvPurchases;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPurchaseReturnNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPurchaseDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPurchaseSupplier;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPurchaseTotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPurchaseStatus;
        private System.Windows.Forms.ContextMenuStrip ctxMenu;
        private System.Windows.Forms.ToolStripMenuItem tsmiOpen;
        private System.Windows.Forms.ToolStripSeparator tsSep1;
        private System.Windows.Forms.ToolStripMenuItem tsmiRefresh;
        private System.Windows.Forms.ToolTip toolTip;
    }
}
