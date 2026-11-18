namespace SalesSystem.Desktop.Screens.Sales.Forms
{
    partial class frmSalesInvoiceSelection
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
            // RULE-N134: ألوان المصمّم = افتراضيات ThemePalette الحيّة حرفياً (المصمّم = وقت التشغيل).
            // RULE-N204: كل لون بصيغة Convert.ToInt32(...) والألوان المسمّاة مؤهّلة System.Drawing.Color.*
            // RULE-N79 + §2.34: هيكل (أ) للقوائم — العنوان · الأوامر · التصفية · الشبكة · الملخّص · الأسفل،
            // بإرساء Dock فقط وبالبنية الحرفية لـ frmListSales (RULE-N125: ترتيب Controls.Add ملزم).
            tooltip = new ToolTip(components);
            cmsInvoices = new ContextMenuStrip(components);
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
            dgvLines = new DataGridView();
            colLineProduct = new DataGridViewTextBoxColumn();
            colLineQty = new DataGridViewTextBoxColumn();
            colLinePrice = new DataGridViewTextBoxColumn();
            colLineDiscount = new DataGridViewTextBoxColumn();
            colLineTotal = new DataGridViewTextBoxColumn();
            lblNoData = new Label();
            lblInvoiceSummary = new Label();
            dgvInvoices = new DataGridView();
            colInvId = new DataGridViewTextBoxColumn();
            colInvNo = new DataGridViewTextBoxColumn();
            colInvDate = new DataGridViewTextBoxColumn();
            colInvCustomer = new DataGridViewTextBoxColumn();
            colInvPaymentType = new DataGridViewTextBoxColumn();
            colInvNetTotal = new DataGridViewTextBoxColumn();
            colInvPaid = new DataGridViewTextBoxColumn();
            colInvRemaining = new DataGridViewTextBoxColumn();
            colInvStatus = new DataGridViewTextBoxColumn();
            pnlHeader.SuspendLayout();
            pnlToolbar.SuspendLayout();
            flpToolbar.SuspendLayout();
            pnlFilters.SuspendLayout();
            gbFilter.SuspendLayout();
            pnlSummary.SuspendLayout();
            flpSummary.SuspendLayout();
            pnlFooter.SuspendLayout();
            pnlGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvLines).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvInvoices).BeginInit();
            cmsInvoices.SuspendLayout();
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
            lblMode.Text = "اختيار فاتورة بيع";
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
            pnlToolbar.Size = new Size(1200, 64);
            pnlToolbar.TabIndex = 1;
            // 
            // flpToolbar
            // 
            flpToolbar.BackColor = System.Drawing.Color.White;
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
            btnSelect.Location = new Point(1052, 13);
            btnSelect.Margin = new Padding(4, 0, 4, 0);
            btnSelect.Name = "btnSelect";
            btnSelect.Size = new Size(118, 40);
            btnSelect.TabIndex = 0;
            btnSelect.Text = "اختيار";
            tooltip.SetToolTip(btnSelect, "اختيار الفاتورة المحددة لبناء المرتجع (Enter)");
            btnSelect.UseVisualStyleBackColor = false;
            btnSelect.Click += btnSelect_Click;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(108), Convert.ToInt32(117), Convert.ToInt32(125));
            btnCancel.Cursor = Cursors.Hand;
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnCancel.ForeColor = System.Drawing.Color.White;
            btnCancel.Location = new Point(926, 13);
            btnCancel.Margin = new Padding(4, 0, 4, 0);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(118, 40);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "إلغاء";
            tooltip.SetToolTip(btnCancel, "إلغاء وإغلاق الشاشة (Esc)");
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
            btnRefresh.Location = new Point(800, 13);
            btnRefresh.Margin = new Padding(4, 0, 4, 0);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(118, 40);
            btnRefresh.TabIndex = 2;
            btnRefresh.Text = "تحديث";
            tooltip.SetToolTip(btnRefresh, "إعادة تحميل قائمة الفواتير (F5)");
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // pnlFilters
            // 
            // RULE-N125/RULE-N63: gbFilter يُضاف أولاً فيملأ الباقي، وbtnToggleFilters بعده فيُقتطع على الحافة.
            pnlFilters.BackColor = System.Drawing.Color.White;
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
            gbFilter.BackColor = System.Drawing.Color.White;
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
            gbFilter.Padding = new Padding(8);
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
            lblSearch.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
            lblSearch.Location = new Point(960, 32);
            lblSearch.Margin = new Padding(0);
            lblSearch.Name = "lblSearch";
            lblSearch.Size = new Size(48, 24);
            lblSearch.TabIndex = 0;
            lblSearch.Text = "بحث:";
            // 
            // txtSearch
            // 
            txtSearch.BackColor = System.Drawing.Color.White;
            txtSearch.BorderStyle = BorderStyle.FixedSingle;
            txtSearch.Font = new Font("Tahoma", 10F);
            txtSearch.Location = new Point(540, 28);
            txtSearch.Margin = new Padding(0);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(400, 32);
            txtSearch.TabIndex = 1;
            tooltip.SetToolTip(txtSearch, "بحث فوري برقم الفاتورة أو اسم العميل — F5 لتحديث القائمة");
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblStatus.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
            lblStatus.Location = new Point(440, 32);
            lblStatus.Margin = new Padding(0);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(68, 24);
            lblStatus.TabIndex = 2;
            lblStatus.Text = "الحالة:";
            // 
            // cbStatus
            // 
            cbStatus.BackColor = System.Drawing.Color.White;
            cbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cbStatus.FlatStyle = FlatStyle.Flat;
            cbStatus.Font = new Font("Tahoma", 10F);
            cbStatus.FormattingEnabled = true;
            cbStatus.Location = new Point(220, 28);
            cbStatus.Margin = new Padding(0);
            cbStatus.Name = "cbStatus";
            cbStatus.Size = new Size(205, 32);
            cbStatus.TabIndex = 3;
            cbStatus.SelectedIndexChanged += cbStatus_SelectedIndexChanged;
            // 
            // lblFrom
            // 
            lblFrom.AutoSize = true;
            lblFrom.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblFrom.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
            lblFrom.Location = new Point(960, 70);
            lblFrom.Margin = new Padding(0);
            lblFrom.Name = "lblFrom";
            lblFrom.Size = new Size(42, 24);
            lblFrom.TabIndex = 4;
            lblFrom.Text = "من:";
            // 
            // dtpFrom
            // 
            dtpFrom.BackColor = System.Drawing.Color.White;
            dtpFrom.Checked = false;
            dtpFrom.Font = new Font("Tahoma", 10F);
            dtpFrom.Format = DateTimePickerFormat.Short;
            dtpFrom.Location = new Point(796, 66);
            dtpFrom.Margin = new Padding(0);
            dtpFrom.Name = "dtpFrom";
            dtpFrom.ShowCheckBox = true;
            dtpFrom.Size = new Size(154, 32);
            dtpFrom.TabIndex = 5;
            dtpFrom.Value = new DateTime(2026, 11, 18, 0, 0, 0, 0);
            tooltip.SetToolTip(dtpFrom, "حدّد المربع لتفعيل تاريخ البداية");
            dtpFrom.ValueChanged += dtpFrom_ValueChanged;
            // 
            // lblTo
            // 
            lblTo.AutoSize = true;
            lblTo.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblTo.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
            lblTo.Location = new Point(740, 70);
            lblTo.Margin = new Padding(0);
            lblTo.Name = "lblTo";
            lblTo.Size = new Size(48, 24);
            lblTo.TabIndex = 6;
            lblTo.Text = "إلى:";
            // 
            // dtpTo
            // 
            dtpTo.BackColor = System.Drawing.Color.White;
            dtpTo.Checked = false;
            dtpTo.Font = new Font("Tahoma", 10F);
            dtpTo.Format = DateTimePickerFormat.Short;
            dtpTo.Location = new Point(576, 66);
            dtpTo.Margin = new Padding(0);
            dtpTo.Name = "dtpTo";
            dtpTo.ShowCheckBox = true;
            dtpTo.Size = new Size(154, 32);
            dtpTo.TabIndex = 7;
            dtpTo.Value = new DateTime(2026, 11, 18, 0, 0, 0, 0);
            tooltip.SetToolTip(dtpTo, "حدّد المربع لتفعيل تاريخ النهاية (شامل)");
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
            btnClearFilters.Location = new Point(350, 66);
            btnClearFilters.Margin = new Padding(0);
            btnClearFilters.Name = "btnClearFilters";
            btnClearFilters.Size = new Size(200, 32);
            btnClearFilters.TabIndex = 8;
            btnClearFilters.Text = "مسح التصفية";
            tooltip.SetToolTip(btnClearFilters, "مسح أدوات التصفية فقط — لا يغيّر الفاتورة المحددة");
            btnClearFilters.UseVisualStyleBackColor = false;
            btnClearFilters.Click += btnClearFilters_Click;
            // 
            // btnToggleFilters
            // 
            // RULE-N145: الزر مُعلَن هنا ويتبنّاه FilterPanelHelper من المصمّم (لا يُنشأ وقت التشغيل).
            btnToggleFilters.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(248), Convert.ToInt32(249), Convert.ToInt32(250));
            btnToggleFilters.Cursor = Cursors.Hand;
            btnToggleFilters.Dock = DockStyle.Left;
            btnToggleFilters.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(Convert.ToInt32(205), Convert.ToInt32(225), Convert.ToInt32(205));
            btnToggleFilters.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(245), Convert.ToInt32(235));
            btnToggleFilters.FlatStyle = FlatStyle.Popup;
            btnToggleFilters.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            btnToggleFilters.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
            btnToggleFilters.Location = new Point(6, 4);
            btnToggleFilters.Name = "btnToggleFilters";
            btnToggleFilters.Size = new Size(140, 102);
            btnToggleFilters.TabIndex = 1;
            btnToggleFilters.Text = "إخفاء التصفية";
            btnToggleFilters.UseVisualStyleBackColor = false;
            // 
            // pnlSummary
            // 
            pnlSummary.BackColor = System.Drawing.Color.White;
            pnlSummary.Controls.Add(flpSummary);
            pnlSummary.Dock = DockStyle.Bottom;
            pnlSummary.Location = new Point(0, 796);
            pnlSummary.Margin = new Padding(0);
            pnlSummary.Name = "pnlSummary";
            pnlSummary.Size = new Size(1200, 46);
            pnlSummary.TabIndex = 3;
            // 
            // flpSummary
            // 
            flpSummary.BackColor = System.Drawing.Color.White;
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
            lblRecordsCount.Location = new Point(1132, 8);
            lblRecordsCount.Margin = new Padding(4, 0, 4, 0);
            lblRecordsCount.Name = "lblRecordsCount";
            lblRecordsCount.Size = new Size(178, 24);
            lblRecordsCount.TabIndex = 0;
            lblRecordsCount.Text = "النتائج: 0";
            // 
            // pnlFooter
            // 
            pnlFooter.BackColor = System.Drawing.Color.White;
            pnlFooter.Controls.Add(lblHint);
            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Location = new Point(0, 842);
            pnlFooter.Margin = new Padding(0);
            pnlFooter.Name = "pnlFooter";
            pnlFooter.Padding = new Padding(10, 0, 10, 0);
            pnlFooter.Size = new Size(1200, 58);
            pnlFooter.TabIndex = 5;
            // 
            // lblHint
            // 
            lblHint.Dock = DockStyle.Fill;
            lblHint.Font = new Font("Tahoma", 9F, FontStyle.Regular);
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
            // RULE-N125: الشبكة الممتدة (Fill) تُضاف أولاً، ثم الرسائل والأشرطة المراسية بعدها فتأخذ حافتها.
            pnlGrid.BackColor = System.Drawing.Color.White;
            pnlGrid.Controls.Add(dgvLines);
            pnlGrid.Controls.Add(lblNoData);
            pnlGrid.Controls.Add(lblInvoiceSummary);
            pnlGrid.Controls.Add(dgvInvoices);
            pnlGrid.Dock = DockStyle.Fill;
            pnlGrid.Location = new Point(0, 230);
            pnlGrid.Margin = new Padding(0);
            pnlGrid.Name = "pnlGrid";
            pnlGrid.Padding = new Padding(0, 10, 0, 0);
            pnlGrid.Size = new Size(1200, 566);
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
            dgvInvoices.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
            dataGridViewCellStyle1.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvInvoices.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvInvoices.ColumnHeadersHeight = 45;
            dgvInvoices.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvInvoices.Columns.AddRange(new DataGridViewColumn[] { colInvId, colInvNo, colInvDate, colInvCustomer, colInvPaymentType, colInvNetTotal, colInvPaid, colInvRemaining, colInvStatus });
            dgvInvoices.ContextMenuStrip = cmsInvoices;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new Font("Tahoma", 10F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(255), Convert.ToInt32(255), Convert.ToInt32(180));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvInvoices.DefaultCellStyle = dataGridViewCellStyle2;
            dgvInvoices.Dock = DockStyle.Top;
            dgvInvoices.EnableHeadersVisualStyles = false;
            dgvInvoices.GridColor = System.Drawing.Color.FromArgb(Convert.ToInt32(205), Convert.ToInt32(225), Convert.ToInt32(205));
            dgvInvoices.Location = new Point(0, 10);
            dgvInvoices.Margin = new Padding(0);
            dgvInvoices.MultiSelect = false;
            dgvInvoices.Name = "dgvInvoices";
            dgvInvoices.ReadOnly = true;
            dgvInvoices.RowHeadersVisible = false;
            dgvInvoices.RowHeadersWidth = 51;
            dgvInvoices.RowTemplate.Height = 32;
            dgvInvoices.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvInvoices.Size = new Size(1200, 280);
            dgvInvoices.TabIndex = 0;
            dgvInvoices.TabStop = false;
            tooltip.SetToolTip(dgvInvoices, "قائمة فواتير البيع — انقر مرتين (أو Enter) للاختيار");
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
            colInvNo.FillWeight = 9F;
            colInvNo.HeaderText = "رقم الفاتورة";
            colInvNo.MinimumWidth = 80;
            colInvNo.Name = "colInvNo";
            colInvNo.ReadOnly = true;
            colInvNo.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colInvDate
            // 
            colInvDate.FillWeight = 9F;
            colInvDate.HeaderText = "التاريخ";
            colInvDate.MinimumWidth = 90;
            colInvDate.Name = "colInvDate";
            colInvDate.ReadOnly = true;
            colInvDate.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colInvCustomer
            // 
            colInvCustomer.FillWeight = 20F;
            colInvCustomer.HeaderText = "العميل";
            colInvCustomer.MinimumWidth = 140;
            colInvCustomer.Name = "colInvCustomer";
            colInvCustomer.ReadOnly = true;
            colInvCustomer.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colInvPaymentType
            // 
            colInvPaymentType.FillWeight = 9F;
            colInvPaymentType.HeaderText = "نوع الدفع";
            colInvPaymentType.MinimumWidth = 80;
            colInvPaymentType.Name = "colInvPaymentType";
            colInvPaymentType.ReadOnly = true;
            colInvPaymentType.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colInvNetTotal
            // 
            colInvNetTotal.FillWeight = 11F;
            colInvNetTotal.HeaderText = "الإجمالي";
            colInvNetTotal.MinimumWidth = 90;
            colInvNetTotal.Name = "colInvNetTotal";
            colInvNetTotal.ReadOnly = true;
            colInvNetTotal.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colInvPaid
            // 
            colInvPaid.FillWeight = 11F;
            colInvPaid.HeaderText = "المدفوع";
            colInvPaid.MinimumWidth = 90;
            colInvPaid.Name = "colInvPaid";
            colInvPaid.ReadOnly = true;
            colInvPaid.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colInvRemaining
            // 
            colInvRemaining.FillWeight = 11F;
            colInvRemaining.HeaderText = "المتبقي";
            colInvRemaining.MinimumWidth = 90;
            colInvRemaining.Name = "colInvRemaining";
            colInvRemaining.ReadOnly = true;
            colInvRemaining.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colInvStatus
            // 
            colInvStatus.FillWeight = 9F;
            colInvStatus.HeaderText = "الحالة";
            colInvStatus.MinimumWidth = 80;
            colInvStatus.Name = "colInvStatus";
            colInvStatus.ReadOnly = true;
            colInvStatus.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // lblInvoiceSummary
            // 
            lblInvoiceSummary.BackColor = System.Drawing.Color.White;
            lblInvoiceSummary.Dock = DockStyle.Top;
            lblInvoiceSummary.Font = new Font("Tahoma", 11F, FontStyle.Bold);
            lblInvoiceSummary.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
            lblInvoiceSummary.Location = new Point(0, 290);
            lblInvoiceSummary.Margin = new Padding(0);
            lblInvoiceSummary.Name = "lblInvoiceSummary";
            lblInvoiceSummary.Size = new Size(1200, 28);
            lblInvoiceSummary.TabIndex = 1;
            lblInvoiceSummary.Text = "لم تُحدَّد فاتورة";
            lblInvoiceSummary.TextAlign = ContentAlignment.MiddleCenter;
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
            dgvLines.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
            dataGridViewCellStyle3.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            dgvLines.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dgvLines.ColumnHeadersHeight = 45;
            dgvLines.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvLines.Columns.AddRange(new DataGridViewColumn[] { colLineProduct, colLineQty, colLinePrice, colLineDiscount, colLineTotal });
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.Font = new Font("Tahoma", 10F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(255), Convert.ToInt32(255), Convert.ToInt32(180));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.False;
            dgvLines.DefaultCellStyle = dataGridViewCellStyle4;
            dgvLines.Dock = DockStyle.Fill;
            dgvLines.EnableHeadersVisualStyles = false;
            dgvLines.GridColor = System.Drawing.Color.FromArgb(Convert.ToInt32(205), Convert.ToInt32(225), Convert.ToInt32(205));
            dgvLines.Location = new Point(0, 318);
            dgvLines.Margin = new Padding(0);
            dgvLines.MultiSelect = false;
            dgvLines.Name = "dgvLines";
            dgvLines.ReadOnly = true;
            dgvLines.RowHeadersVisible = false;
            dgvLines.RowHeadersWidth = 51;
            dgvLines.RowTemplate.Height = 32;
            dgvLines.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLines.Size = new Size(1200, 224);
            dgvLines.TabIndex = 2;
            dgvLines.TabStop = false;
            tooltip.SetToolTip(dgvLines, "سطور الفاتورة المحددة");
            // 
            // colLineProduct
            // 
            colLineProduct.FillWeight = 40F;
            colLineProduct.HeaderText = "الصنف";
            colLineProduct.MinimumWidth = 140;
            colLineProduct.Name = "colLineProduct";
            colLineProduct.ReadOnly = true;
            colLineProduct.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colLineQty
            // 
            colLineQty.FillWeight = 12F;
            colLineQty.HeaderText = "الكمية";
            colLineQty.MinimumWidth = 80;
            colLineQty.Name = "colLineQty";
            colLineQty.ReadOnly = true;
            colLineQty.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colLinePrice
            // 
            colLinePrice.FillWeight = 16F;
            colLinePrice.HeaderText = "السعر";
            colLinePrice.MinimumWidth = 90;
            colLinePrice.Name = "colLinePrice";
            colLinePrice.ReadOnly = true;
            colLinePrice.SortMode = DataGridViewColumnSortMode.NotSortable;
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
            colLineTotal.FillWeight = 18F;
            colLineTotal.HeaderText = "الإجمالي";
            colLineTotal.MinimumWidth = 100;
            colLineTotal.Name = "colLineTotal";
            colLineTotal.ReadOnly = true;
            colLineTotal.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // lblNoData
            // 
            lblNoData.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(245), Convert.ToInt32(252), Convert.ToInt32(245));
            lblNoData.Dock = DockStyle.Bottom;
            lblNoData.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            lblNoData.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            lblNoData.Location = new Point(0, 542);
            lblNoData.Margin = new Padding(0);
            lblNoData.Name = "lblNoData";
            lblNoData.Size = new Size(1200, 24);
            lblNoData.TabIndex = 3;
            lblNoData.Text = "لا توجد فواتير مطابقة للتصفية الحالية";
            lblNoData.TextAlign = ContentAlignment.MiddleCenter;
            lblNoData.Visible = false;
            // 
            // cmsInvoices
            // 
            // RULE-N23: قائمة سياق على الشبكة — «اختيار» يستدعي نفس زر الشاشة (بلا منطق مكرر).
            cmsInvoices.ImageScalingSize = new Size(24, 24);
            cmsInvoices.Items.AddRange(new ToolStripItem[] { cmsSelect, cmsSeparator, cmsRefresh });
            cmsInvoices.Name = "cmsInvoices";
            cmsInvoices.RightToLeft = RightToLeft.Yes;
            cmsInvoices.Size = new Size(198, 74);
            cmsInvoices.Opening += cmsInvoices_Opening;
            // 
            // cmsSelect
            // 
            cmsSelect.Name = "cmsSelect";
            cmsSelect.Size = new Size(197, 32);
            cmsSelect.Text = "✔ اختيار";
            cmsSelect.Click += cmsSelect_Click;
            // 
            // cmsSeparator
            // 
            cmsSeparator.Name = "cmsSeparator";
            cmsSeparator.Size = new Size(194, 6);
            // 
            // cmsRefresh
            // 
            cmsRefresh.Name = "cmsRefresh";
            cmsRefresh.Size = new Size(197, 32);
            cmsRefresh.Text = "🔄 تحديث (F5)";
            cmsRefresh.Click += cmsRefresh_Click;
            // 
            // frmSalesInvoiceSelection
            // 
            AcceptButton = btnSelect;
            AutoScaleMode = AutoScaleMode.None;
            BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(170), Convert.ToInt32(220), Convert.ToInt32(170));
            CancelButton = btnCancel;
            ClientSize = new Size(1200, 900);
            Controls.Add(pnlGrid);
            Controls.Add(pnlSummary);
            Controls.Add(pnlFooter);
            Controls.Add(pnlFilters);
            Controls.Add(pnlToolbar);
            Controls.Add(pnlHeader);
            Font = new Font("Tahoma", 10F);
            KeyPreview = true;
            MinimumSize = new Size(1000, 700);
            Name = "frmSalesInvoiceSelection";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "اختيار فاتورة بيع";
            Load += frmSalesInvoiceSelection_Load;
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
            ((System.ComponentModel.ISupportInitialize)dgvLines).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvInvoices).EndInit();
            cmsInvoices.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

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
        private System.Windows.Forms.DataGridViewTextBoxColumn colInvCustomer;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInvPaymentType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInvNetTotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInvPaid;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInvRemaining;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInvStatus;
        private System.Windows.Forms.Label lblInvoiceSummary;
        private System.Windows.Forms.DataGridView dgvLines;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLineProduct;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLineQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLinePrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLineDiscount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLineTotal;
        private System.Windows.Forms.Label lblNoData;
        private System.Windows.Forms.ContextMenuStrip cmsInvoices;
        private System.Windows.Forms.ToolStripMenuItem cmsSelect;
        private System.Windows.Forms.ToolStripSeparator cmsSeparator;
        private System.Windows.Forms.ToolStripMenuItem cmsRefresh;
        private System.Windows.Forms.ToolTip tooltip;
    }
}
