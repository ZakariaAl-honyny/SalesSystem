namespace SalesSystem.Desktop.Screens.Inventory.Forms
{
    partial class frmListInventoryBatches
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            components?.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            contextMenuStrip = new ContextMenuStrip(components);
            tsmiView = new ToolStripMenuItem();
            tsmiExport = new ToolStripMenuItem();
            tsmiRefresh = new ToolStripMenuItem();
            pnlHeader = new Panel();
            lblMode = new Label();
            gbCommands = new GroupBox();
            pnlToolbar = new Panel();
            flpToolbar = new FlowLayoutPanel();
            btnRefresh = new Button();
            btnClose = new Button();
            pnlFilters = new Panel();
            gbFilter = new GroupBox();
            btnToggleFilters = new Button();
            label2 = new Label();
            cbFilterBy = new ComboBox();
            txtFilterValue = new TextBox();
            label1 = new Label();
            cbStatus = new ComboBox();
            label3 = new Label();
            dtpFrom = new DateTimePicker();
            label4 = new Label();
            dtpTo = new DateTimePicker();
            btnRangeToday = new Button();
            btnRange30 = new Button();
            btnRangeMonth = new Button();
            btnClearFilters = new Button();
            chkOnlyExpiringSoon = new CheckBox();
            chkOnlyExpired = new CheckBox();
            lblProduct = new Label();
            cmbProduct = new ComboBox();
            pnlGrid = new GroupBox();
            lblEmpty = new Label();
            dgvBatches = new DataGridView();
            pnlSummary = new Panel();
            lblSummary = new Label();
            toolTip1 = new ToolTip(components);

            contextMenuStrip.SuspendLayout();
            pnlHeader.SuspendLayout();
            gbCommands.SuspendLayout();
            pnlToolbar.SuspendLayout();
            flpToolbar.SuspendLayout();
            pnlFilters.SuspendLayout();
            gbFilter.SuspendLayout();
            pnlGrid.SuspendLayout();
            pnlSummary.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvBatches).BeginInit();
            SuspendLayout();

            // ── contextMenuStrip (RULE-N23) ─────────────────────────────────
            contextMenuStrip.Font = new Font("Tahoma", 10F);
            contextMenuStrip.Items.AddRange(new ToolStripItem[] { tsmiView, tsmiExport, tsmiRefresh });
            contextMenuStrip.Name = "contextMenuStrip";
            contextMenuStrip.RightToLeft = RightToLeft.Yes;
            contextMenuStrip.Size = new Size(180, 94);

            // ── tsmiView ────────────────────────────────────────────────────
            tsmiView.Name = "tsmiView";
            tsmiView.Size = new Size(179, 30);
            tsmiView.Text = "عرض التفاصيل";
            tsmiView.Click += tsmiView_Click;

            // ── tsmiExport ──────────────────────────────────────────────────
            tsmiExport.Name = "tsmiExport";
            tsmiExport.Size = new Size(179, 30);
            tsmiExport.Text = "تصدير إلى Excel";
            tsmiExport.Click += tsmiExport_Click;

            // ── tsmiRefresh ─────────────────────────────────────────────────
            tsmiRefresh.Name = "tsmiRefresh";
            tsmiRefresh.Size = new Size(179, 30);
            tsmiRefresh.Text = "تحديث";
            tsmiRefresh.Click += tsmiRefresh_Click;

            // ── pnlHeader ───────────────────────────────────────────────────
            pnlHeader.BackColor = Color.FromArgb(14, 165, 103);
            pnlHeader.Controls.Add(lblMode);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Margin = new Padding(0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1564, 56);
            pnlHeader.TabIndex = 0;

            // ── lblMode ─────────────────────────────────────────────────────
            lblMode.BackColor = Color.FromArgb(14, 165, 103);
            lblMode.Dock = DockStyle.Fill;
            lblMode.Font = new Font("Tahoma", 15F, FontStyle.Bold);
            lblMode.ForeColor = Color.White;
            lblMode.Location = new Point(0, 0);
            lblMode.Margin = new Padding(0);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(1564, 56);
            lblMode.TabIndex = 0;
            lblMode.Text = "دفعات المخزون";
            lblMode.TextAlign = ContentAlignment.MiddleCenter;

            // ── gbCommands — «أوامر الشاشة» (RULE-N136) ─────────────────────
            gbCommands.BackColor = Color.FromArgb(235, 240, 235);
            gbCommands.Controls.Add(pnlToolbar);
            gbCommands.Dock = DockStyle.Top;
            gbCommands.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            gbCommands.Location = new Point(0, 56);
            gbCommands.Margin = new Padding(0);
            gbCommands.Name = "gbCommands";
            gbCommands.Size = new Size(1564, 82);
            gbCommands.TabIndex = 1;
            gbCommands.TabStop = false;
            gbCommands.Text = "أوامر الشاشة";

            // ── pnlToolbar ──────────────────────────────────────────────────
            pnlToolbar.BackColor = Color.FromArgb(235, 240, 235);
            pnlToolbar.Controls.Add(flpToolbar);
            pnlToolbar.Dock = DockStyle.Fill;
            pnlToolbar.Location = new Point(3, 22);
            pnlToolbar.Margin = new Padding(0);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.Size = new Size(1558, 57);
            pnlToolbar.TabIndex = 0;

            // ── flpToolbar ──────────────────────────────────────────────────
            flpToolbar.BackColor = Color.FromArgb(235, 240, 235);
            flpToolbar.Controls.Add(btnRefresh);
            flpToolbar.Controls.Add(btnClose);
            flpToolbar.Dock = DockStyle.Fill;
            flpToolbar.Location = new Point(0, 0);
            flpToolbar.Margin = new Padding(0);
            flpToolbar.Name = "flpToolbar";
            flpToolbar.Padding = new Padding(10);
            flpToolbar.Size = new Size(1558, 57);
            flpToolbar.TabIndex = 0;
            flpToolbar.WrapContents = false;

            // ── btnRefresh ──────────────────────────────────────────────────
            btnRefresh.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnRefresh.Location = new Point(1420, 13);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(124, 36);
            btnRefresh.TabIndex = 0;
            btnRefresh.Text = "تحديث";
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Click += btnRefresh_Click;

            // ── btnClose ────────────────────────────────────────────────────
            btnClose.DialogResult = DialogResult.Cancel;
            btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnClose.Location = new Point(1290, 13);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(124, 36);
            btnClose.TabIndex = 1;
            btnClose.Text = "إغلاق";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;

            // ── pnlFilters ──────────────────────────────────────────────────
            pnlFilters.BackColor = Color.FromArgb(235, 240, 235);
            pnlFilters.Controls.Add(gbFilter);
            pnlFilters.Controls.Add(btnToggleFilters);
            pnlFilters.Dock = DockStyle.Top;
            pnlFilters.Location = new Point(0, 138);
            pnlFilters.Margin = new Padding(0);
            pnlFilters.Name = "pnlFilters";
            pnlFilters.Padding = new Padding(6, 4, 6, 4);
            pnlFilters.Size = new Size(1564, 112);
            pnlFilters.TabIndex = 2;

            // ── gbFilter ────────────────────────────────────────────────────
            gbFilter.BackColor = Color.FromArgb(235, 245, 235);
            gbFilter.Controls.Add(label2);
            gbFilter.Controls.Add(cbFilterBy);
            gbFilter.Controls.Add(txtFilterValue);
            gbFilter.Controls.Add(label1);
            gbFilter.Controls.Add(cbStatus);
            gbFilter.Controls.Add(label3);
            gbFilter.Controls.Add(dtpFrom);
            gbFilter.Controls.Add(label4);
            gbFilter.Controls.Add(dtpTo);
            gbFilter.Controls.Add(btnRangeToday);
            gbFilter.Controls.Add(btnRange30);
            gbFilter.Controls.Add(btnRangeMonth);
            gbFilter.Controls.Add(btnClearFilters);
            gbFilter.Controls.Add(chkOnlyExpiringSoon);
            gbFilter.Controls.Add(chkOnlyExpired);
            gbFilter.Controls.Add(lblProduct);
            gbFilter.Controls.Add(cmbProduct);
            gbFilter.Dock = DockStyle.Fill;
            gbFilter.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            gbFilter.Location = new Point(146, 4);
            gbFilter.Margin = new Padding(0);
            gbFilter.Name = "gbFilter";
            gbFilter.Padding = new Padding(8);
            gbFilter.Size = new Size(1412, 104);
            gbFilter.TabIndex = 0;
            gbFilter.TabStop = false;
            gbFilter.Text = "بحث وتصفية";

            // ── btnToggleFilters — RULE-N145/RULE-N63: يُعلَن في المصمّم ويتبنّاه FilterPanelHelper ──
            btnToggleFilters.BackColor = Color.FromArgb(248, 249, 250);
            btnToggleFilters.Cursor = Cursors.Hand;
            btnToggleFilters.Dock = DockStyle.Left;
            btnToggleFilters.FlatAppearance.BorderColor = Color.FromArgb(222, 226, 230);
            btnToggleFilters.FlatAppearance.BorderSize = 1;
            btnToggleFilters.FlatAppearance.MouseOverBackColor = Color.FromArgb(240, 240, 240);
            btnToggleFilters.FlatStyle = FlatStyle.Popup;
            btnToggleFilters.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            btnToggleFilters.ForeColor = Color.FromArgb(33, 37, 41);
            btnToggleFilters.Location = new Point(6, 4);
            btnToggleFilters.Name = "btnToggleFilters";
            btnToggleFilters.Size = new Size(140, 104);
            btnToggleFilters.TabIndex = 20;
            btnToggleFilters.Text = "إخفاء التصفية";
            btnToggleFilters.UseVisualStyleBackColor = false;

            // ── label2 «تصفية بـ:» ──────────────────────────────────────────
            label2.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            label2.ForeColor = Color.FromArgb(14, 82, 52);
            label2.Location = new Point(1311, 26);
            label2.Margin = new Padding(0);
            label2.Name = "label2";
            label2.Size = new Size(85, 26);
            label2.TabIndex = 0;
            label2.Text = "تصفية بـ:";
            label2.TextAlign = ContentAlignment.MiddleRight;

            // ── cbFilterBy ──────────────────────────────────────────────────
            cbFilterBy.BackColor = Color.White;
            cbFilterBy.DropDownStyle = ComboBoxStyle.DropDownList;
            cbFilterBy.FlatStyle = FlatStyle.Flat;
            cbFilterBy.Font = new Font("Tahoma", 9F, FontStyle.Regular);
            cbFilterBy.FormattingEnabled = true;
            cbFilterBy.Items.AddRange(new object[] { "الكل", "الصنف", "رقم الدفعة", "المستودع" });
            cbFilterBy.Location = new Point(1155, 23);
            cbFilterBy.Margin = new Padding(0);
            cbFilterBy.Name = "cbFilterBy";
            cbFilterBy.Size = new Size(150, 26);
            cbFilterBy.TabIndex = 1;
            cbFilterBy.SelectedIndexChanged += cbFilterBy_SelectedIndexChanged;

            // ── txtFilterValue ──────────────────────────────────────────────
            txtFilterValue.BackColor = Color.White;
            txtFilterValue.Font = new Font("Tahoma", 9F, FontStyle.Regular);
            txtFilterValue.Location = new Point(949, 23);
            txtFilterValue.Margin = new Padding(0);
            txtFilterValue.Name = "txtFilterValue";
            txtFilterValue.Size = new Size(200, 25);
            txtFilterValue.TabIndex = 2;
            txtFilterValue.TextChanged += txtFilterValue_TextChanged;
            txtFilterValue.KeyPress += txtFilterValue_KeyPress;

            // ── label1 «الحالة:» ────────────────────────────────────────────
            label1.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            label1.ForeColor = Color.FromArgb(14, 82, 52);
            label1.Location = new Point(873, 26);
            label1.Margin = new Padding(0);
            label1.Name = "label1";
            label1.Size = new Size(70, 26);
            label1.TabIndex = 3;
            label1.Text = "الحالة:";
            label1.TextAlign = ContentAlignment.MiddleRight;

            // ── cbStatus ────────────────────────────────────────────────────
            cbStatus.BackColor = Color.White;
            cbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cbStatus.FlatStyle = FlatStyle.Flat;
            cbStatus.Font = new Font("Tahoma", 9F, FontStyle.Regular);
            cbStatus.FormattingEnabled = true;
            cbStatus.Items.AddRange(new object[] { "الكل", "سارية", "قاربت الانتهاء", "منتهية" });
            cbStatus.Location = new Point(737, 23);
            cbStatus.Margin = new Padding(0);
            cbStatus.Name = "cbStatus";
            cbStatus.Size = new Size(130, 26);
            cbStatus.TabIndex = 4;
            cbStatus.SelectedIndexChanged += cbStatus_SelectedIndexChanged;

            // ── label3 «من:» ────────────────────────────────────────────────
            label3.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            label3.ForeColor = Color.FromArgb(14, 82, 52);
            label3.Location = new Point(687, 26);
            label3.Margin = new Padding(0);
            label3.Name = "label3";
            label3.Size = new Size(44, 26);
            label3.TabIndex = 5;
            label3.Text = "من:";
            label3.TextAlign = ContentAlignment.MiddleRight;

            // ── dtpFrom ─────────────────────────────────────────────────────
            dtpFrom.BackColor = Color.White;
            dtpFrom.Font = new Font("Tahoma", 9F, FontStyle.Regular);
            dtpFrom.Format = DateTimePickerFormat.Short;
            dtpFrom.Location = new Point(556, 23);
            dtpFrom.Margin = new Padding(0);
            dtpFrom.Name = "dtpFrom";
            dtpFrom.Size = new Size(125, 25);
            dtpFrom.TabIndex = 6;
            dtpFrom.ValueChanged += dtpFrom_ValueChanged;

            // ── label4 «إلى:» ───────────────────────────────────────────────
            label4.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            label4.ForeColor = Color.FromArgb(14, 82, 52);
            label4.Location = new Point(506, 26);
            label4.Margin = new Padding(0);
            label4.Name = "label4";
            label4.Size = new Size(44, 26);
            label4.TabIndex = 7;
            label4.Text = "إلى:";
            label4.TextAlign = ContentAlignment.MiddleRight;

            // ── dtpTo ───────────────────────────────────────────────────────
            dtpTo.BackColor = Color.White;
            dtpTo.Font = new Font("Tahoma", 9F, FontStyle.Regular);
            dtpTo.Format = DateTimePickerFormat.Short;
            dtpTo.Location = new Point(375, 23);
            dtpTo.Margin = new Padding(0);
            dtpTo.Name = "dtpTo";
            dtpTo.Size = new Size(125, 25);
            dtpTo.TabIndex = 8;
            dtpTo.ValueChanged += dtpTo_ValueChanged;

            // ── btnRangeToday ───────────────────────────────────────────────
            btnRangeToday.BackColor = Color.White;
            btnRangeToday.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            btnRangeToday.Location = new Point(1296, 62);
            btnRangeToday.Name = "btnRangeToday";
            btnRangeToday.Size = new Size(100, 30);
            btnRangeToday.TabIndex = 9;
            btnRangeToday.Text = "اليوم";
            btnRangeToday.UseVisualStyleBackColor = false;
            btnRangeToday.Click += btnRangeToday_Click;

            // ── btnRange30 ──────────────────────────────────────────────────
            btnRange30.BackColor = Color.White;
            btnRange30.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            btnRange30.Location = new Point(1162, 62);
            btnRange30.Name = "btnRange30";
            btnRange30.Size = new Size(128, 30);
            btnRange30.TabIndex = 10;
            btnRange30.Text = "آخر 30 يوماً";
            btnRange30.UseVisualStyleBackColor = false;
            btnRange30.Click += btnRange30_Click;

            // ── btnRangeMonth ───────────────────────────────────────────────
            btnRangeMonth.BackColor = Color.White;
            btnRangeMonth.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            btnRangeMonth.Location = new Point(1056, 62);
            btnRangeMonth.Name = "btnRangeMonth";
            btnRangeMonth.Size = new Size(100, 30);
            btnRangeMonth.TabIndex = 11;
            btnRangeMonth.Text = "هذا الشهر";
            btnRangeMonth.UseVisualStyleBackColor = false;
            btnRangeMonth.Click += btnRangeMonth_Click;

            // ── btnClearFilters «مسح التصفية» (RULE-N148) — داخل gbFilter، معطّل حتى توجد تصفية نشطة ──
            btnClearFilters.BackColor = Color.FromArgb(108, 117, 125);
            btnClearFilters.Cursor = Cursors.Hand;
            btnClearFilters.Enabled = false;
            btnClearFilters.FlatAppearance.BorderSize = 0;
            btnClearFilters.FlatStyle = FlatStyle.Flat;
            btnClearFilters.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            btnClearFilters.ForeColor = Color.White;
            btnClearFilters.Location = new Point(457, 62);
            btnClearFilters.Margin = new Padding(0);
            btnClearFilters.Name = "btnClearFilters";
            btnClearFilters.Size = new Size(145, 32);
            btnClearFilters.TabIndex = 16;
            btnClearFilters.Text = "🧹 مسح التصفية";
            btnClearFilters.UseVisualStyleBackColor = false;
            btnClearFilters.Click += btnClearFilters_Click;
            toolTip1.SetToolTip(btnClearFilters, "مسح كل أدوات التصفية (Esc) — لا يعيد تحميل البيانات");

            // ── chkOnlyExpiringSoon ─────────────────────────────────────────
            chkOnlyExpiringSoon.Font = new Font("Tahoma", 9F, FontStyle.Regular);
            chkOnlyExpiringSoon.ForeColor = Color.FromArgb(14, 82, 52);
            chkOnlyExpiringSoon.Location = new Point(800, 64);
            chkOnlyExpiringSoon.Margin = new Padding(0);
            chkOnlyExpiringSoon.Name = "chkOnlyExpiringSoon";
            chkOnlyExpiringSoon.Size = new Size(210, 26);
            chkOnlyExpiringSoon.TabIndex = 12;
            chkOnlyExpiringSoon.Text = "قاربت الانتهاء فقط";
            chkOnlyExpiringSoon.UseVisualStyleBackColor = false;
            chkOnlyExpiringSoon.CheckedChanged += chkOnlyExpiringSoon_CheckedChanged;

            // ── chkOnlyExpired ──────────────────────────────────────────────
            chkOnlyExpired.Font = new Font("Tahoma", 9F, FontStyle.Regular);
            chkOnlyExpired.ForeColor = Color.FromArgb(14, 82, 52);
            chkOnlyExpired.Location = new Point(610, 64);
            chkOnlyExpired.Margin = new Padding(0);
            chkOnlyExpired.Name = "chkOnlyExpired";
            chkOnlyExpired.Size = new Size(180, 26);
            chkOnlyExpired.TabIndex = 13;
            chkOnlyExpired.Text = "المنتهية فقط";
            chkOnlyExpired.UseVisualStyleBackColor = false;
            chkOnlyExpired.CheckedChanged += chkOnlyExpired_CheckedChanged;

            // ── lblProduct «الصنف:» (مرشّح المصدر — يبقى داخل gbFilter وفق RULE-N62) ──
            lblProduct.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            lblProduct.ForeColor = Color.FromArgb(14, 82, 52);
            lblProduct.Location = new Point(305, 26);
            lblProduct.Margin = new Padding(0);
            lblProduct.Name = "lblProduct";
            lblProduct.Size = new Size(64, 26);
            lblProduct.TabIndex = 14;
            lblProduct.Text = "الصنف:";
            lblProduct.TextAlign = ContentAlignment.MiddleRight;

            // ── cmbProduct ──────────────────────────────────────────────────
            cmbProduct.BackColor = Color.White;
            cmbProduct.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbProduct.FlatStyle = FlatStyle.Flat;
            cmbProduct.Font = new Font("Tahoma", 9F, FontStyle.Regular);
            cmbProduct.FormattingEnabled = true;
            cmbProduct.Location = new Point(64, 23);
            cmbProduct.Margin = new Padding(0);
            cmbProduct.Name = "cmbProduct";
            cmbProduct.Size = new Size(235, 26);
            cmbProduct.TabIndex = 15;
            cmbProduct.SelectedIndexChanged += cmbProduct_SelectedIndexChanged;

            // ── pnlGrid — «البيانات» (RULE-N136) ────────────────────────────
            pnlGrid.BackColor = Color.FromArgb(235, 245, 235);
            pnlGrid.Controls.Add(dgvBatches);
            pnlGrid.Controls.Add(lblEmpty);
            pnlGrid.Dock = DockStyle.Fill;
            pnlGrid.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            pnlGrid.Location = new Point(0, 250);
            pnlGrid.Margin = new Padding(0);
            pnlGrid.Name = "pnlGrid";
            pnlGrid.Padding = new Padding(6);
            pnlGrid.Size = new Size(1564, 538);
            pnlGrid.TabIndex = 3;
            pnlGrid.TabStop = false;
            pnlGrid.Text = "البيانات";

            // ── lblEmpty ────────────────────────────────────────────────────
            lblEmpty.BackColor = Color.Transparent;
            lblEmpty.Dock = DockStyle.Fill;
            lblEmpty.Font = new Font("Tahoma", 12F, FontStyle.Bold);
            lblEmpty.ForeColor = Color.FromArgb(14, 82, 52);
            lblEmpty.Location = new Point(6, 6);
            lblEmpty.Margin = new Padding(0);
            lblEmpty.Name = "lblEmpty";
            lblEmpty.Size = new Size(1552, 526);
            lblEmpty.TabIndex = 1;
            lblEmpty.Text = "لا توجد بيانات للعرض";
            lblEmpty.TextAlign = ContentAlignment.MiddleCenter;
            lblEmpty.Visible = false;

            // ── pnlSummary — «الإجماليات» (RULE-N136) ───────────────────────
            pnlSummary.BackColor = Color.FromArgb(235, 240, 235);
            pnlSummary.Controls.Add(lblSummary);
            pnlSummary.Dock = DockStyle.Bottom;
            pnlSummary.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            pnlSummary.Location = new Point(0, 788);
            pnlSummary.Margin = new Padding(0);
            pnlSummary.Name = "pnlSummary";
            pnlSummary.Padding = new Padding(10, 18, 10, 4);
            pnlSummary.Size = new Size(1564, 52);
            pnlSummary.TabIndex = 4;
            pnlSummary.TabStop = false;
            pnlSummary.Text = "الإجماليات";

            // ── lblSummary ──────────────────────────────────────────────────
            lblSummary.Dock = DockStyle.Fill;
            lblSummary.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            lblSummary.ForeColor = Color.FromArgb(14, 82, 52);
            lblSummary.Location = new Point(10, 18);
            lblSummary.Margin = new Padding(0);
            lblSummary.Name = "lblSummary";
            lblSummary.Size = new Size(1544, 26);
            lblSummary.TabIndex = 0;
            lblSummary.Text = "المعروض: 0 من 0 — إجمالي الكمية: 0.000 · منتهية: 0 · قاربت الانتهاء: 0 · إجمالي القيمة: 0.00";
            lblSummary.TextAlign = ContentAlignment.MiddleRight;

            // ── dgvBatches ──────────────────────────────────────────────────
            // RULE-N134: نفس ألوان GreenThemeHelper حرفياً في المصمّم ووقت التشغيل.
            dgvBatches.AllowUserToAddRows = false;
            dgvBatches.AllowUserToDeleteRows = false;
            dgvBatches.AllowUserToOrderColumns = true;
            dgvBatches.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 252, 245);
            dgvBatches.AutoGenerateColumns = false;
            dgvBatches.BackgroundColor = Color.FromArgb(220, 240, 220);
            dgvBatches.BorderStyle = BorderStyle.None;
            dgvBatches.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvBatches.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(14, 165, 103);
            dgvBatches.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            dgvBatches.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvBatches.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(14, 165, 103);
            dgvBatches.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            dgvBatches.ColumnHeadersHeight = 45;
            dgvBatches.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvBatches.ContextMenuStrip = contextMenuStrip;
            dgvBatches.DefaultCellStyle.BackColor = Color.White;
            dgvBatches.DefaultCellStyle.Font = new Font("Tahoma", 10F);
            dgvBatches.DefaultCellStyle.ForeColor = Color.FromArgb(33, 37, 41);
            dgvBatches.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 255, 180);
            dgvBatches.DefaultCellStyle.SelectionForeColor = Color.FromArgb(33, 37, 41);
            dgvBatches.Dock = DockStyle.Fill;
            dgvBatches.EnableHeadersVisualStyles = false;
            dgvBatches.GridColor = Color.FromArgb(205, 225, 205);
            dgvBatches.Location = new Point(6, 6);
            dgvBatches.Margin = new Padding(0);
            dgvBatches.MultiSelect = false;
            dgvBatches.Name = "dgvBatches";
            dgvBatches.ReadOnly = true;
            dgvBatches.RowHeadersVisible = false;
            dgvBatches.RowHeadersWidth = 51;
            dgvBatches.RowTemplate.Height = 32;
            dgvBatches.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBatches.Size = new Size(1552, 526);
            dgvBatches.TabIndex = 0;
            dgvBatches.TabStop = false;
            dgvBatches.CellDoubleClick += dgvBatches_CellDoubleClick;
            dgvBatches.CellFormatting += dgvBatches_CellFormatting;
            dgvBatches.MouseDown += dgvBatches_MouseDown;

            // ── frmListInventoryBatches ─────────────────────────────────────
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.FromArgb(170, 220, 170);
            CancelButton = btnClose;
            ClientSize = new Size(1564, 840);
            Controls.Add(pnlGrid);
            Controls.Add(pnlSummary);
            Controls.Add(pnlFilters);
            Controls.Add(gbCommands);
            Controls.Add(pnlHeader);
            Font = new Font("Tahoma", 10F, FontStyle.Regular);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            KeyPreview = true;
            MaximizeBox = false;
            MinimumSize = new Size(1200, 700);
            Name = "frmListInventoryBatches";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "دفعات المخزون";
            Load += frmListInventoryBatches_Load;

            contextMenuStrip.ResumeLayout(false);
            pnlHeader.ResumeLayout(false);
            gbCommands.ResumeLayout(false);
            pnlToolbar.ResumeLayout(false);
            flpToolbar.ResumeLayout(false);
            pnlFilters.ResumeLayout(false);
            gbFilter.ResumeLayout(false);
            gbFilter.PerformLayout();
            pnlGrid.ResumeLayout(false);
            pnlSummary.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvBatches).EndInit();
            ResumeLayout(false);
        }

        #region Fields
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem tsmiView;
        private System.Windows.Forms.ToolStripMenuItem tsmiExport;
        private System.Windows.Forms.ToolStripMenuItem tsmiRefresh;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.GroupBox gbCommands;
        private System.Windows.Forms.Panel pnlToolbar;
        private System.Windows.Forms.FlowLayoutPanel flpToolbar;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel pnlFilters;
        private System.Windows.Forms.GroupBox gbFilter;
        private System.Windows.Forms.Button btnToggleFilters;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbFilterBy;
        private System.Windows.Forms.TextBox txtFilterValue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbStatus;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Button btnRangeToday;
        private System.Windows.Forms.Button btnRange30;
        private System.Windows.Forms.Button btnRangeMonth;
        private System.Windows.Forms.Button btnClearFilters;
        private System.Windows.Forms.CheckBox chkOnlyExpiringSoon;
        private System.Windows.Forms.CheckBox chkOnlyExpired;
        private System.Windows.Forms.Label lblProduct;
        private System.Windows.Forms.ComboBox cmbProduct;
        private System.Windows.Forms.GroupBox pnlGrid;
        private System.Windows.Forms.Label lblEmpty;
        private System.Windows.Forms.Panel pnlSummary;
        private System.Windows.Forms.Label lblSummary;
        private System.Windows.Forms.DataGridView dgvBatches;
        private System.Windows.Forms.ToolTip toolTip1;
        #endregion
    }
}
