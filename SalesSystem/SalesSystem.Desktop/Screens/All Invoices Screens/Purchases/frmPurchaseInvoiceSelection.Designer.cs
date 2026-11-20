namespace SalesSystem.Desktop.Screens.Purchases.Forms
{
    partial class frmPurchaseInvoiceSelection
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            // RULE-N134: ألوان المصمّم = افتراضيات ThemePalette الحيّة حرفياً (المصمّم = وقت التشغيل).
            // RULE-N204: كل لون بصيغة Convert.ToInt32، والألوان المسمّاة مؤهّلة System.Drawing.Color.*
            // RULE-N79 (أ) — هيكل المستند/القائمة حرفياً كما في frmListSales:
            // pnlHeader + pnlToolbar + pnlFilters + pnlGrid + pnlSummary + pnlFooter.
            tooltip = new ToolTip(components);
            cmsRows = new ContextMenuStrip(components);
            cmsSelect = new ToolStripMenuItem();
            cmsSeparator = new ToolStripSeparator();
            cmsRefresh = new ToolStripMenuItem();
            pnlHeader = new Panel();
            lblMode = new Label();
            pnlToolbar = new Panel();
            flpToolbar = new FlowLayoutPanel();
            btnSelect = new Button();
            btnCancel = new Button();
            btnRefresh = new Button();
            pnlFilters = new Panel();
            gbFilter = new GroupBox();
            lblSearch = new Label();
            txtSearch = new TextBox();
            lblStatus = new Label();
            cbStatus = new ComboBox();
            lblFrom = new Label();
            dtpFrom = new DateTimePicker();
            lblTo = new Label();
            dtpTo = new DateTimePicker();
            btnClearFilters = new Button();
            btnToggleFilters = new Button();
            pnlSummary = new Panel();
            flpSummary = new FlowLayoutPanel();
            lblRecordsCount = new Label();
            pnlFooter = new Panel();
            lblHint = new Label();
            pnlGrid = new Panel();
            dgvInvoices = new DataGridView();
            colInvId = new DataGridViewTextBoxColumn();
            colInvNo = new DataGridViewTextBoxColumn();
            colInvDate = new DataGridViewTextBoxColumn();
            colInvSupplier = new DataGridViewTextBoxColumn();
            colInvPaymentType = new DataGridViewTextBoxColumn();
            colInvTotal = new DataGridViewTextBoxColumn();
            colInvPaid = new DataGridViewTextBoxColumn();
            colInvRemaining = new DataGridViewTextBoxColumn();
            colInvStatus = new DataGridViewTextBoxColumn();
            lblInvoiceSummary = new Label();
            dgvLines = new DataGridView();
            colLineProduct = new DataGridViewTextBoxColumn();
            colLineQuantity = new DataGridViewTextBoxColumn();
            colLineCost = new DataGridViewTextBoxColumn();
            colLineDiscount = new DataGridViewTextBoxColumn();
            colLineTotal = new DataGridViewTextBoxColumn();
            lblNoData = new Label();
            pnlHeader.SuspendLayout();
            pnlToolbar.SuspendLayout();
            flpToolbar.SuspendLayout();
            pnlFilters.SuspendLayout();
            gbFilter.SuspendLayout();
            pnlSummary.SuspendLayout();
            flpSummary.SuspendLayout();
            pnlFooter.SuspendLayout();
            pnlGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvInvoices).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvLines).BeginInit();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
            pnlHeader.Controls.Add(lblMode);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Margin = new Padding(0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1200, 56);
            pnlHeader.TabIndex = 0;
            // 
            // lblMode
            // 
            lblMode.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
            lblMode.Dock = DockStyle.Fill;
            lblMode.Font = new Font("Tahoma", 14F, FontStyle.Bold);
            lblMode.ForeColor = System.Drawing.Color.White;
            lblMode.Location = new Point(0, 0);
            lblMode.Margin = new Padding(0);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(1200, 56);
            lblMode.TabIndex = 0;
            lblMode.Text = "اختيار فاتورة شراء";
            lblMode.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlToolbar
            // 
            pnlToolbar.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
            pnlToolbar.Controls.Add(flpToolbar);
            pnlToolbar.Dock = DockStyle.Top;
            pnlToolbar.Location = new Point(0, 56);
            pnlToolbar.Margin = new Padding(0);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.Size = new Size(1200, 64);
            pnlToolbar.TabIndex = 1;
            // 
            // flpToolbar
            // 
            flpToolbar.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
            flpToolbar.Controls.Add(btnSelect);
            flpToolbar.Controls.Add(btnCancel);
            flpToolbar.Controls.Add(btnRefresh);
            flpToolbar.Dock = DockStyle.Fill;
            flpToolbar.Location = new Point(0, 0);
            flpToolbar.Margin = new Padding(0);
            flpToolbar.Name = "flpToolbar";
            flpToolbar.Padding = new Padding(10);
            flpToolbar.Size = new Size(1200, 64);
            flpToolbar.TabIndex = 0;
            flpToolbar.WrapContents = false;
            // 
            // btnSelect
            // 
            btnSelect.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(25), Convert.ToInt32(135), Convert.ToInt32(84));
            btnSelect.Cursor = Cursors.Hand;
            btnSelect.Enabled = false;
            btnSelect.FlatAppearance.BorderSize = 0;
            btnSelect.FlatStyle = FlatStyle.Flat;
            btnSelect.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnSelect.ForeColor = System.Drawing.Color.White;
            btnSelect.Location = new Point(1062, 13);
            btnSelect.Margin = new Padding(4, 0, 4, 0);
            btnSelect.Name = "btnSelect";
            btnSelect.Size = new Size(118, 40);
            btnSelect.TabIndex = 0;
            btnSelect.Text = "اختيار";
            tooltip.SetToolTip(btnSelect, "اختيار الفاتورة المحددة والرجوع بها إلى شاشة المرتجع (Enter)");
            btnSelect.UseVisualStyleBackColor = false;
            btnSelect.Click += btnSelect_Click;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(108), Convert.ToInt32(117), Convert.ToInt32(125));
            btnCancel.Cursor = Cursors.Hand;
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnCancel.ForeColor = System.Drawing.Color.White;
            btnCancel.Location = new Point(936, 13);
            btnCancel.Margin = new Padding(4, 0, 4, 0);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(118, 40);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "إلغاء";
            tooltip.SetToolTip(btnCancel, "إلغاء و إغلاق النافذة (Esc)");
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(23), Convert.ToInt32(162), Convert.ToInt32(184));
            btnRefresh.Cursor = Cursors.Hand;
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnRefresh.ForeColor = System.Drawing.Color.White;
            btnRefresh.Location = new Point(810, 13);
            btnRefresh.Margin = new Padding(4, 0, 4, 0);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(118, 40);
            btnRefresh.TabIndex = 2;
            btnRefresh.Text = "تحديث";
            tooltip.SetToolTip(btnRefresh, "إعادة تحميل القائمة من أولها (F5)");
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // pnlFilters
            // 
            // RULE-N125/RULE-N63: gbFilter يُضاف أولاً فيملأ الباقي، وbtnToggleFilters بعده فيُقتطع على الحافة.
            pnlFilters.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
            pnlFilters.Controls.Add(gbFilter);
            pnlFilters.Controls.Add(btnToggleFilters);
            pnlFilters.Dock = DockStyle.Top;
            pnlFilters.Location = new Point(0, 120);
            pnlFilters.Margin = new Padding(0);
            pnlFilters.Name = "pnlFilters";
            pnlFilters.Padding = new Padding(6, 4, 6, 4);
            pnlFilters.Size = new Size(1200, 110);
            pnlFilters.TabIndex = 2;
            // 
            // gbFilter
            // 
            gbFilter.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(245), Convert.ToInt32(235));
            gbFilter.Controls.Add(lblSearch);
            gbFilter.Controls.Add(txtSearch);
            gbFilter.Controls.Add(lblStatus);
            gbFilter.Controls.Add(cbStatus);
            gbFilter.Controls.Add(lblFrom);
            gbFilter.Controls.Add(dtpFrom);
            gbFilter.Controls.Add(lblTo);
            gbFilter.Controls.Add(dtpTo);
            gbFilter.Controls.Add(btnClearFilters);
            gbFilter.Dock = DockStyle.Fill;
            gbFilter.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            gbFilter.Location = new Point(146, 4);
            gbFilter.Margin = new Padding(0);
            gbFilter.Name = "gbFilter";
            gbFilter.Padding = new Padding(10);
            gbFilter.RightToLeft = RightToLeft.Yes;
            gbFilter.Size = new Size(1048, 102);
            gbFilter.TabIndex = 0;
            gbFilter.TabStop = false;
            gbFilter.Text = "بحث وتصفية";
            // 
            // lblSearch
            // 
            lblSearch.AutoSize = true;
            lblSearch.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblSearch.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            lblSearch.Location = new Point(980, 34);
            lblSearch.Margin = new Padding(0);
            lblSearch.Name = "lblSearch";
            lblSearch.Size = new Size(50, 24);
            lblSearch.TabIndex = 0;
            lblSearch.Text = "بحث:";
            lblSearch.TextAlign = ContentAlignment.MiddleRight;
            // 
            // txtSearch
            // 
            txtSearch.BackColor = System.Drawing.Color.White;
            txtSearch.BorderStyle = BorderStyle.FixedSingle;
            txtSearch.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtSearch.Location = new Point(560, 30);
            txtSearch.Margin = new Padding(0);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(400, 32);
            txtSearch.TabIndex = 1;
            tooltip.SetToolTip(txtSearch, "بحث فوري برقم الفاتورة أو اسم المورد — F5 لتحديث القائمة");
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblStatus.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            lblStatus.Location = new Point(480, 34);
            lblStatus.Margin = new Padding(0);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(62, 24);
            lblStatus.TabIndex = 2;
            lblStatus.Text = "الحالة:";
            lblStatus.TextAlign = ContentAlignment.MiddleRight;
            // 
            // cbStatus
            // 
            cbStatus.BackColor = System.Drawing.Color.White;
            cbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cbStatus.FlatStyle = FlatStyle.Flat;
            cbStatus.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cbStatus.FormattingEnabled = true;
            cbStatus.Location = new Point(290, 30);
            cbStatus.Margin = new Padding(0);
            cbStatus.Name = "cbStatus";
            cbStatus.Size = new Size(190, 32);
            cbStatus.TabIndex = 3;
            tooltip.SetToolTip(cbStatus, "تصفية حسب حالة الفاتورة");
            cbStatus.SelectedIndexChanged += cbStatus_SelectedIndexChanged;
            // 
            // lblFrom
            // 
            lblFrom.AutoSize = true;
            lblFrom.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblFrom.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            lblFrom.Location = new Point(980, 70);
            lblFrom.Margin = new Padding(0);
            lblFrom.Name = "lblFrom";
            lblFrom.Size = new Size(34, 24);
            lblFrom.TabIndex = 4;
            lblFrom.Text = "من:";
            lblFrom.TextAlign = ContentAlignment.MiddleRight;
            // 
            // dtpFrom
            // 
            dtpFrom.BackColor = System.Drawing.Color.White;
            dtpFrom.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dtpFrom.Format = DateTimePickerFormat.Short;
            dtpFrom.Location = new Point(810, 66);
            dtpFrom.Margin = new Padding(0);
            dtpFrom.Name = "dtpFrom";
            dtpFrom.Size = new Size(154, 32);
            dtpFrom.TabIndex = 5;
            dtpFrom.ValueChanged += dtpFrom_ValueChanged;
            // 
            // lblTo
            // 
            lblTo.AutoSize = true;
            lblTo.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblTo.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            lblTo.Location = new Point(770, 70);
            lblTo.Margin = new Padding(0);
            lblTo.Name = "lblTo";
            lblTo.Size = new Size(30, 24);
            lblTo.TabIndex = 6;
            lblTo.Text = "إلى:";
            lblTo.TextAlign = ContentAlignment.MiddleRight;
            // 
            // dtpTo
            // 
            dtpTo.BackColor = System.Drawing.Color.White;
            dtpTo.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dtpTo.Format = DateTimePickerFormat.Short;
            dtpTo.Location = new Point(600, 66);
            dtpTo.Margin = new Padding(0);
            dtpTo.Name = "dtpTo";
            dtpTo.Size = new Size(154, 32);
            dtpTo.TabIndex = 7;
            dtpTo.ValueChanged += dtpTo_ValueChanged;
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
            btnClearFilters.Location = new Point(150, 66);
            btnClearFilters.Margin = new Padding(0);
            btnClearFilters.Name = "btnClearFilters";
            btnClearFilters.Size = new Size(200, 32);
            btnClearFilters.TabIndex = 8;
            btnClearFilters.Text = "مسح التصفية";
            tooltip.SetToolTip(btnClearFilters, "مسح التصفية فقط — لا يعيد تحميل بيانات الشاشة ولا يغيّر الفاتورة المحددة");
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
            btnToggleFilters.FlatStyle = FlatStyle.Flat;
            btnToggleFilters.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            btnToggleFilters.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(30), Convert.ToInt32(41), Convert.ToInt32(59));
            btnToggleFilters.Location = new Point(6, 4);
            btnToggleFilters.Name = "btnToggleFilters";
            btnToggleFilters.Size = new Size(140, 102);
            btnToggleFilters.TabIndex = 1;
            btnToggleFilters.Text = "إخفاء التصفية";
            btnToggleFilters.UseVisualStyleBackColor = false;
            // 
            // pnlSummary
            // 
            pnlSummary.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
            pnlSummary.Controls.Add(flpSummary);
            pnlSummary.Dock = DockStyle.Bottom;
            pnlSummary.Location = new Point(0, 756);
            pnlSummary.Margin = new Padding(0);
            pnlSummary.Name = "pnlSummary";
            pnlSummary.Size = new Size(1200, 46);
            pnlSummary.TabIndex = 3;
            // 
            // flpSummary
            // 
            flpSummary.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
            flpSummary.Controls.Add(lblRecordsCount);
            flpSummary.Dock = DockStyle.Fill;
            flpSummary.Location = new Point(0, 0);
            flpSummary.Margin = new Padding(0);
            flpSummary.Name = "flpSummary";
            flpSummary.Padding = new Padding(10, 8, 10, 8);
            flpSummary.Size = new Size(1200, 46);
            flpSummary.TabIndex = 0;
            flpSummary.WrapContents = false;
            // 
            // lblRecordsCount
            // 
            lblRecordsCount.AutoSize = true;
            lblRecordsCount.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblRecordsCount.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
            lblRecordsCount.Location = new Point(1092, 8);
            lblRecordsCount.Margin = new Padding(4, 0, 24, 0);
            lblRecordsCount.Name = "lblRecordsCount";
            lblRecordsCount.Size = new Size(98, 24);
            lblRecordsCount.TabIndex = 0;
            lblRecordsCount.Text = "النتائج: 0";
            // 
            // pnlFooter
            // 
            pnlFooter.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
            pnlFooter.Controls.Add(lblHint);
            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Location = new Point(0, 802);
            pnlFooter.Margin = new Padding(0);
            pnlFooter.Name = "pnlFooter";
            pnlFooter.Padding = new Padding(10, 0, 10, 0);
            pnlFooter.Size = new Size(1200, 58);
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
            lblHint.Size = new Size(1180, 58);
            lblHint.TabIndex = 0;
            lblHint.Text = "↓ انتقال إلى القائمة · Enter اختيار · F5 تحديث · Esc إلغاء";
            lblHint.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlGrid
            // 
            // RULE-N125: يُضاف عنصر Fill الوحيد أولاً ليُرسى آخراً ويأخذ الباقي — كما في frmListSales.
            pnlGrid.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
            pnlGrid.Controls.Add(dgvLines);
            pnlGrid.Controls.Add(lblNoData);
            pnlGrid.Controls.Add(lblInvoiceSummary);
            pnlGrid.Controls.Add(dgvInvoices);
            pnlGrid.Dock = DockStyle.Fill;
            pnlGrid.Location = new Point(0, 230);
            pnlGrid.Margin = new Padding(0);
            pnlGrid.Name = "pnlGrid";
            pnlGrid.Padding = new Padding(0, 10, 0, 0);
            pnlGrid.Size = new Size(1200, 526);
            pnlGrid.TabIndex = 4;
            // 
            // dgvInvoices
            // 
            dgvInvoices.AllowUserToAddRows = false;
            dgvInvoices.AllowUserToDeleteRows = false;
            dgvInvoices.AllowUserToResizeRows = false;
            dgvInvoices.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(245), Convert.ToInt32(252), Convert.ToInt32(245));
            dgvInvoices.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvInvoices.BackgroundColor = System.Drawing.Color.FromArgb(Convert.ToInt32(220), Convert.ToInt32(240), Convert.ToInt32(220));
            dgvInvoices.BorderStyle = BorderStyle.None;
            dgvInvoices.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvInvoices.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvInvoices.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
            dgvInvoices.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            dgvInvoices.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dgvInvoices.ColumnHeadersDefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
            dgvInvoices.ColumnHeadersDefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            dgvInvoices.ColumnHeadersHeight = 45;
            dgvInvoices.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvInvoices.Columns.AddRange(new DataGridViewColumn[] { colInvId, colInvNo, colInvDate, colInvSupplier, colInvPaymentType, colInvTotal, colInvPaid, colInvRemaining, colInvStatus });
            dgvInvoices.ContextMenuStrip = cmsRows;
            dgvInvoices.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            dgvInvoices.DefaultCellStyle.Font = new Font("Tahoma", 10F);
            dgvInvoices.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            dgvInvoices.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(255), Convert.ToInt32(255), Convert.ToInt32(180));
            dgvInvoices.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            dgvInvoices.Dock = DockStyle.Top;
            dgvInvoices.EnableHeadersVisualStyles = false;
            dgvInvoices.GridColor = System.Drawing.Color.FromArgb(Convert.ToInt32(205), Convert.ToInt32(225), Convert.ToInt32(205));
            dgvInvoices.Location = new Point(0, 10);
            dgvInvoices.Margin = new Padding(0);
            dgvInvoices.MultiSelect = false;
            dgvInvoices.Name = "dgvInvoices";
            dgvInvoices.ReadOnly = true;
            dgvInvoices.RowHeadersVisible = false;
            dgvInvoices.RowHeadersWidth = 62;
            dgvInvoices.RowTemplate.Height = 32;
            dgvInvoices.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvInvoices.Size = new Size(1200, 300);
            dgvInvoices.TabIndex = 0;
            tooltip.SetToolTip(dgvInvoices, "قائمة فواتير الشراء — انقر مرتين (أو Enter) للاختيار");
            dgvInvoices.SelectionChanged += dgvInvoices_SelectionChanged;
            dgvInvoices.DoubleClick += dgvInvoices_DoubleClick;
            // 
            // colInvId
            // 
            colInvId.HeaderText = "المعرّف";
            colInvId.MinimumWidth = 8;
            colInvId.Name = "colInvId";
            colInvId.ReadOnly = true;
            colInvId.Visible = false;
            // 
            // colInvNo
            // 
            colInvNo.FillWeight = 10F;
            colInvNo.HeaderText = "رقم الفاتورة";
            colInvNo.MinimumWidth = 80;
            colInvNo.Name = "colInvNo";
            colInvNo.ReadOnly = true;
            // 
            // colInvDate
            // 
            colInvDate.FillWeight = 11F;
            colInvDate.HeaderText = "التاريخ";
            colInvDate.MinimumWidth = 90;
            colInvDate.Name = "colInvDate";
            colInvDate.ReadOnly = true;
            // 
            // colInvSupplier
            // 
            colInvSupplier.FillWeight = 24F;
            colInvSupplier.HeaderText = "المورد";
            colInvSupplier.MinimumWidth = 140;
            colInvSupplier.Name = "colInvSupplier";
            colInvSupplier.ReadOnly = true;
            // 
            // colInvPaymentType
            // 
            colInvPaymentType.FillWeight = 10F;
            colInvPaymentType.HeaderText = "نوع الدفع";
            colInvPaymentType.MinimumWidth = 80;
            colInvPaymentType.Name = "colInvPaymentType";
            colInvPaymentType.ReadOnly = true;
            colInvPaymentType.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colInvTotal
            // 
            colInvTotal.FillWeight = 11F;
            colInvTotal.HeaderText = "الإجمالي";
            colInvTotal.MinimumWidth = 100;
            colInvTotal.Name = "colInvTotal";
            colInvTotal.ReadOnly = true;
            colInvTotal.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colInvPaid
            // 
            colInvPaid.FillWeight = 11F;
            colInvPaid.HeaderText = "المدفوع";
            colInvPaid.MinimumWidth = 100;
            colInvPaid.Name = "colInvPaid";
            colInvPaid.ReadOnly = true;
            colInvPaid.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colInvRemaining
            // 
            colInvRemaining.FillWeight = 11F;
            colInvRemaining.HeaderText = "المتبقي";
            colInvRemaining.MinimumWidth = 100;
            colInvRemaining.Name = "colInvRemaining";
            colInvRemaining.ReadOnly = true;
            colInvRemaining.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colInvStatus
            // 
            colInvStatus.FillWeight = 10F;
            colInvStatus.HeaderText = "الحالة";
            colInvStatus.MinimumWidth = 80;
            colInvStatus.Name = "colInvStatus";
            colInvStatus.ReadOnly = true;
            colInvStatus.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // lblInvoiceSummary
            // 
            lblInvoiceSummary.AutoEllipsis = true;
            lblInvoiceSummary.Dock = DockStyle.Top;
            lblInvoiceSummary.Font = new Font("Tahoma", 11F, FontStyle.Bold);
            lblInvoiceSummary.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
            lblInvoiceSummary.Location = new Point(0, 310);
            lblInvoiceSummary.Margin = new Padding(0);
            lblInvoiceSummary.Name = "lblInvoiceSummary";
            lblInvoiceSummary.Size = new Size(1200, 28);
            lblInvoiceSummary.TabIndex = 1;
            lblInvoiceSummary.Text = "لم تُحدَّد فاتورة";
            lblInvoiceSummary.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // dgvLines
            // 
            dgvLines.AllowUserToAddRows = false;
            dgvLines.AllowUserToDeleteRows = false;
            dgvLines.AllowUserToResizeRows = false;
            dgvLines.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(245), Convert.ToInt32(252), Convert.ToInt32(245));
            dgvLines.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvLines.BackgroundColor = System.Drawing.Color.FromArgb(Convert.ToInt32(220), Convert.ToInt32(240), Convert.ToInt32(220));
            dgvLines.BorderStyle = BorderStyle.None;
            dgvLines.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvLines.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvLines.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
            dgvLines.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            dgvLines.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dgvLines.ColumnHeadersDefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
            dgvLines.ColumnHeadersDefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            dgvLines.ColumnHeadersHeight = 45;
            dgvLines.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvLines.Columns.AddRange(new DataGridViewColumn[] { colLineProduct, colLineQuantity, colLineCost, colLineDiscount, colLineTotal });
            dgvLines.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            dgvLines.DefaultCellStyle.Font = new Font("Tahoma", 10F);
            dgvLines.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            dgvLines.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(255), Convert.ToInt32(255), Convert.ToInt32(180));
            dgvLines.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            dgvLines.Dock = DockStyle.Fill;
            dgvLines.EnableHeadersVisualStyles = false;
            dgvLines.GridColor = System.Drawing.Color.FromArgb(Convert.ToInt32(205), Convert.ToInt32(225), Convert.ToInt32(205));
            dgvLines.Location = new Point(0, 338);
            dgvLines.Margin = new Padding(0);
            dgvLines.MultiSelect = false;
            dgvLines.Name = "dgvLines";
            dgvLines.ReadOnly = true;
            dgvLines.RowHeadersVisible = false;
            dgvLines.RowHeadersWidth = 62;
            dgvLines.RowTemplate.Height = 32;
            dgvLines.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLines.Size = new Size(1200, 164);
            dgvLines.TabIndex = 2;
            tooltip.SetToolTip(dgvLines, "سطور الفاتورة المحددة");
            // 
            // colLineProduct
            // 
            colLineProduct.FillWeight = 46F;
            colLineProduct.HeaderText = "الصنف";
            colLineProduct.MinimumWidth = 140;
            colLineProduct.Name = "colLineProduct";
            colLineProduct.ReadOnly = true;
            // 
            // colLineQuantity
            // 
            colLineQuantity.FillWeight = 12F;
            colLineQuantity.HeaderText = "الكمية";
            colLineQuantity.MinimumWidth = 80;
            colLineQuantity.Name = "colLineQuantity";
            colLineQuantity.ReadOnly = true;
            colLineQuantity.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colLineCost
            // 
            colLineCost.FillWeight = 14F;
            colLineCost.HeaderText = "سعر التكلفة";
            colLineCost.MinimumWidth = 100;
            colLineCost.Name = "colLineCost";
            colLineCost.ReadOnly = true;
            colLineCost.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colLineDiscount
            // 
            colLineDiscount.FillWeight = 14F;
            colLineDiscount.HeaderText = "الخصم";
            colLineDiscount.MinimumWidth = 90;
            colLineDiscount.Name = "colLineDiscount";
            colLineDiscount.ReadOnly = true;
            colLineDiscount.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colLineTotal
            // 
            colLineTotal.FillWeight = 14F;
            colLineTotal.HeaderText = "الإجمالي";
            colLineTotal.MinimumWidth = 100;
            colLineTotal.Name = "colLineTotal";
            colLineTotal.ReadOnly = true;
            colLineTotal.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // lblNoData
            // 
            lblNoData.AutoSize = false;
            lblNoData.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(245), Convert.ToInt32(252), Convert.ToInt32(245));
            lblNoData.Dock = DockStyle.Bottom;
            lblNoData.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblNoData.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
            lblNoData.Location = new Point(0, 502);
            lblNoData.Name = "lblNoData";
            lblNoData.Size = new Size(1200, 24);
            lblNoData.TabIndex = 3;
            lblNoData.Text = "لا توجد فواتير مطابقة للمعايير الحالية";
            lblNoData.TextAlign = ContentAlignment.MiddleCenter;
            lblNoData.Visible = false;
            // 
            // cmsRows
            // 
            // RULE-N23: قائمة سياق على الشبكة — «اختيار» يستدعي نفس زر الشاشة (بلا منطق مكرر).
            cmsRows.Items.AddRange(new ToolStripItem[] { cmsSelect, cmsSeparator, cmsRefresh });
            cmsRows.Name = "cmsRows";
            cmsRows.RightToLeft = RightToLeft.Yes;
            cmsRows.Size = new Size(240, 104);
            cmsRows.Opening += cmsRows_Opening;
            // 
            // cmsSelect
            // 
            cmsSelect.Name = "cmsSelect";
            cmsSelect.Size = new Size(239, 30);
            cmsSelect.Text = "✔ اختيار";
            cmsSelect.Click += cmsSelect_Click;
            // 
            // cmsSeparator
            // 
            cmsSeparator.Name = "cmsSeparator";
            cmsSeparator.Size = new Size(236, 6);
            // 
            // cmsRefresh
            // 
            cmsRefresh.Name = "cmsRefresh";
            cmsRefresh.Size = new Size(239, 30);
            cmsRefresh.Text = "🔄 تحديث (F5)";
            cmsRefresh.Click += cmsRefresh_Click;
            // 
            // frmPurchaseInvoiceSelection
            // 
            AcceptButton = btnSelect;
            AutoScaleMode = AutoScaleMode.None;
            BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(170), Convert.ToInt32(220), Convert.ToInt32(170));
            CancelButton = btnCancel;
            ClientSize = new Size(1200, 860);
            Controls.Add(pnlGrid);
            Controls.Add(pnlSummary);
            Controls.Add(pnlFooter);
            Controls.Add(pnlFilters);
            Controls.Add(pnlToolbar);
            Controls.Add(pnlHeader);
            Font = new Font("Tahoma", 10F);
            KeyPreview = true;
            MinimumSize = new Size(1000, 700);
            Name = "frmPurchaseInvoiceSelection";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterParent;
            Text = "اختيار فاتورة شراء";
            Load += frmPurchaseInvoiceSelection_Load;
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
            pnlGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvInvoices).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvLines).EndInit();
            cmsRows.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.ToolTip tooltip;
        private System.Windows.Forms.ContextMenuStrip cmsRows;
        private System.Windows.Forms.ToolStripMenuItem cmsSelect;
        private System.Windows.Forms.ToolStripSeparator cmsSeparator;
        private System.Windows.Forms.ToolStripMenuItem cmsRefresh;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.Panel pnlToolbar;
        private System.Windows.Forms.FlowLayoutPanel flpToolbar;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Panel pnlFilters;
        private System.Windows.Forms.GroupBox gbFilter;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ComboBox cbStatus;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Button btnClearFilters;
        private System.Windows.Forms.Button btnToggleFilters;
        private System.Windows.Forms.Panel pnlSummary;
        private System.Windows.Forms.FlowLayoutPanel flpSummary;
        private System.Windows.Forms.Label lblRecordsCount;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Label lblHint;
        private System.Windows.Forms.Panel pnlGrid;
        private System.Windows.Forms.DataGridView dgvInvoices;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInvId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInvNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInvDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInvSupplier;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInvPaymentType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInvTotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInvPaid;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInvRemaining;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInvStatus;
        private System.Windows.Forms.Label lblInvoiceSummary;
        private System.Windows.Forms.DataGridView dgvLines;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLineProduct;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLineQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLineCost;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLineDiscount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLineTotal;
        private System.Windows.Forms.Label lblNoData;
    }
}
