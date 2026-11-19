namespace SalesSystem.Desktop.Suppliers.Forms
{
    partial class frmSupplierSelection
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            components?.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            // RULE-N134: ألوان المصمّم = ثوابت GreenThemeHelper حرفياً (المصمّم = وقت التشغيل).
            // RULE-N204: كل لون بصيغة Convert.ToInt32 — لا صيغة مختصرة ولا صيغة التحويل الصريح.
            // RULE-N125: ترتيب Controls.Add بصري معكوس (المضاف أخيراً يُرسى أولاً).
            // الهيكل: نفس هيكل frmListSales — pnlHeader · pnlToolbar · pnlFilters · pnlGrid · pnlSummary · pnlFooter.
            components = new System.ComponentModel.Container();
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
            pnlFilters = new Panel();
            gbFilter = new GroupBox();
            lblSearch = new Label();
            txtSearch = new TextBox();
            btnClearFilters = new Button();
            btnToggleFilters = new Button();
            pnlSummary = new Panel();
            flpSummary = new FlowLayoutPanel();
            lblRecordsCount = new Label();
            pnlFooter = new Panel();
            lblHint = new Label();
            pnlGrid = new Panel();
            lblNoData = new Label();
            dgvSuppliers = new DataGridView();
            dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dgvSuppliers).BeginInit();
            pnlHeader.SuspendLayout();
            pnlToolbar.SuspendLayout();
            flpToolbar.SuspendLayout();
            pnlFilters.SuspendLayout();
            gbFilter.SuspendLayout();
            pnlSummary.SuspendLayout();
            flpSummary.SuspendLayout();
            pnlFooter.SuspendLayout();
            pnlGrid.SuspendLayout();
            cmsRows.SuspendLayout();
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
            pnlHeader.Size = new Size(940, 56);
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
            lblMode.Size = new Size(940, 56);
            lblMode.TabIndex = 0;
            lblMode.Text = "اختيار مورد";
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
            pnlToolbar.Size = new Size(940, 64);
            pnlToolbar.TabIndex = 1;
            // 
            // flpToolbar
            // 
            flpToolbar.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
            flpToolbar.Controls.Add(btnSelect);
            flpToolbar.Controls.Add(btnCancel);
            flpToolbar.Dock = DockStyle.Fill;
            flpToolbar.Location = new Point(0, 0);
            flpToolbar.Margin = new Padding(0);
            flpToolbar.Name = "flpToolbar";
            flpToolbar.Padding = new Padding(10);
            flpToolbar.Size = new Size(940, 64);
            flpToolbar.TabIndex = 0;
            flpToolbar.WrapContents = false;
            // 
            // btnSelect
            // 
            btnSelect.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(25), Convert.ToInt32(135), Convert.ToInt32(84));
            btnSelect.Cursor = Cursors.Hand;
            btnSelect.FlatAppearance.BorderSize = 0;
            btnSelect.FlatStyle = FlatStyle.Flat;
            btnSelect.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnSelect.ForeColor = System.Drawing.Color.White;
            btnSelect.Location = new Point(806, 16);
            btnSelect.Margin = new Padding(6);
            btnSelect.Name = "btnSelect";
            btnSelect.Size = new Size(118, 40);
            btnSelect.TabIndex = 0;
            btnSelect.Text = "تحديد";
            tooltip.SetToolTip(btnSelect, "تحديد المورد المحدد (Enter)");
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
            btnCancel.Location = new Point(676, 16);
            btnCancel.Margin = new Padding(6);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(118, 40);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "إلغاء";
            tooltip.SetToolTip(btnCancel, "إلغاء و إغلاق النافذة (Esc)");
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // pnlFilters
            // 
            pnlFilters.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
            pnlFilters.Controls.Add(gbFilter);
            pnlFilters.Controls.Add(btnToggleFilters);
            pnlFilters.Dock = DockStyle.Top;
            pnlFilters.Location = new Point(0, 120);
            pnlFilters.Margin = new Padding(0);
            pnlFilters.Name = "pnlFilters";
            pnlFilters.Padding = new Padding(6, 4, 6, 4);
            pnlFilters.Size = new Size(940, 110);
            pnlFilters.TabIndex = 2;
            // 
            // gbFilter
            // 
            gbFilter.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(245), Convert.ToInt32(235));
            gbFilter.Controls.Add(lblSearch);
            gbFilter.Controls.Add(txtSearch);
            gbFilter.Controls.Add(btnClearFilters);
            gbFilter.Dock = DockStyle.Fill;
            gbFilter.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            gbFilter.Location = new Point(146, 4);
            gbFilter.Margin = new Padding(0);
            gbFilter.Name = "gbFilter";
            gbFilter.Padding = new Padding(10);
            gbFilter.RightToLeft = RightToLeft.Yes;
            gbFilter.Size = new Size(788, 102);
            gbFilter.TabIndex = 0;
            gbFilter.TabStop = false;
            gbFilter.Text = "بحث وتصفية";
            // 
            // lblSearch
            // 
            lblSearch.AutoSize = true;
            lblSearch.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblSearch.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            lblSearch.Location = new Point(636, 34);
            lblSearch.Margin = new Padding(0);
            lblSearch.Name = "lblSearch";
            lblSearch.Size = new Size(56, 24);
            lblSearch.TabIndex = 0;
            lblSearch.Text = "بحث:";
            lblSearch.TextAlign = ContentAlignment.MiddleRight;
            // 
            // txtSearch
            // 
            txtSearch.BackColor = System.Drawing.Color.White;
            txtSearch.BorderStyle = BorderStyle.FixedSingle;
            txtSearch.Font = new Font("Tahoma", 10F);
            txtSearch.Location = new Point(330, 30);
            txtSearch.Margin = new Padding(0);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(296, 32);
            txtSearch.TabIndex = 1;
            tooltip.SetToolTip(txtSearch, "بحث فوري بالاسم أو رقم الهاتف — F5 لتحديث القائمة");
            txtSearch.TextChanged += txtSearch_TextChanged;
            txtSearch.KeyDown += txtSearch_KeyDown;
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
            btnClearFilters.Location = new Point(116, 32);
            btnClearFilters.Name = "btnClearFilters";
            btnClearFilters.Size = new Size(200, 32);
            btnClearFilters.TabIndex = 2;
            btnClearFilters.Text = "مسح التصفية";
            tooltip.SetToolTip(btnClearFilters, "مسح التصفية فقط — لا يعيد تحميل البيانات ولا يغيّر السطر المحدد");
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
            btnToggleFilters.TabIndex = 3;
            btnToggleFilters.Text = "إخفاء التصفية";
            tooltip.SetToolTip(btnToggleFilters, "إخفاء/إظهار لوحة التصفية — F9");
            btnToggleFilters.UseVisualStyleBackColor = false;
            // 
            // pnlSummary
            // 
            pnlSummary.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
            pnlSummary.Controls.Add(flpSummary);
            pnlSummary.Dock = DockStyle.Bottom;
            pnlSummary.Location = new Point(0, 576);
            pnlSummary.Margin = new Padding(0);
            pnlSummary.Name = "pnlSummary";
            pnlSummary.Size = new Size(940, 46);
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
            flpSummary.Size = new Size(940, 46);
            flpSummary.TabIndex = 0;
            flpSummary.WrapContents = false;
            // 
            // lblRecordsCount
            // 
            lblRecordsCount.AutoSize = true;
            lblRecordsCount.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblRecordsCount.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
            lblRecordsCount.Location = new Point(826, 8);
            lblRecordsCount.Margin = new Padding(4, 0, 4, 0);
            lblRecordsCount.Name = "lblRecordsCount";
            lblRecordsCount.Size = new Size(104, 24);
            lblRecordsCount.TabIndex = 0;
            lblRecordsCount.Text = "النتائج: 0";
            // 
            // pnlFooter
            // 
            pnlFooter.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
            pnlFooter.Controls.Add(lblHint);
            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Location = new Point(0, 622);
            pnlFooter.Margin = new Padding(0);
            pnlFooter.Name = "pnlFooter";
            pnlFooter.Padding = new Padding(10, 0, 10, 0);
            pnlFooter.Size = new Size(940, 58);
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
            lblHint.Size = new Size(920, 58);
            lblHint.TabIndex = 0;
            lblHint.Text = "↓ انتقال إلى القائمة · Enter اختيار · Esc إلغاء · F5 تحديث";
            lblHint.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlGrid
            // 
            pnlGrid.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
            pnlGrid.Controls.Add(lblNoData);
            pnlGrid.Controls.Add(dgvSuppliers);
            pnlGrid.Dock = DockStyle.Fill;
            pnlGrid.Location = new Point(0, 230);
            pnlGrid.Margin = new Padding(0);
            pnlGrid.Name = "pnlGrid";
            pnlGrid.Padding = new Padding(0, 10, 0, 0);
            pnlGrid.Size = new Size(940, 346);
            pnlGrid.TabIndex = 4;
            // 
            // lblNoData
            // 
            lblNoData.AutoSize = false;
            lblNoData.BackColor = System.Drawing.Color.Transparent;
            lblNoData.Dock = DockStyle.Fill;
            lblNoData.Font = new Font("Tahoma", 12F, FontStyle.Bold);
            lblNoData.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            lblNoData.Location = new Point(0, 10);
            lblNoData.Margin = new Padding(0);
            lblNoData.Name = "lblNoData";
            lblNoData.Size = new Size(940, 336);
            lblNoData.TabIndex = 1;
            lblNoData.Text = "لا توجد بيانات";
            lblNoData.TextAlign = ContentAlignment.MiddleCenter;
            lblNoData.Visible = false;
            // 
            // dgvSuppliers
            // 
            dgvSuppliers.AllowUserToAddRows = false;
            dgvSuppliers.AllowUserToDeleteRows = false;
            dgvSuppliers.AllowUserToResizeRows = false;
            dgvSuppliers.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(245), Convert.ToInt32(252), Convert.ToInt32(245));
            dgvSuppliers.AutoGenerateColumns = false;
            dgvSuppliers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvSuppliers.BackgroundColor = System.Drawing.Color.FromArgb(Convert.ToInt32(220), Convert.ToInt32(240), Convert.ToInt32(220));
            dgvSuppliers.BorderStyle = BorderStyle.None;
            dgvSuppliers.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvSuppliers.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvSuppliers.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
            dgvSuppliers.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            dgvSuppliers.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dgvSuppliers.ColumnHeadersDefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
            dgvSuppliers.ColumnHeadersDefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            dgvSuppliers.ColumnHeadersHeight = 45;
            dgvSuppliers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvSuppliers.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn1, dataGridViewTextBoxColumn2, dataGridViewTextBoxColumn3, dataGridViewTextBoxColumn4 });
            dgvSuppliers.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            dgvSuppliers.DefaultCellStyle.Font = new Font("Tahoma", 10F);
            dgvSuppliers.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            dgvSuppliers.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(255), Convert.ToInt32(255), Convert.ToInt32(180));
            dgvSuppliers.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            dgvSuppliers.Dock = DockStyle.Fill;
            dgvSuppliers.EnableHeadersVisualStyles = false;
            dgvSuppliers.GridColor = System.Drawing.Color.FromArgb(Convert.ToInt32(205), Convert.ToInt32(225), Convert.ToInt32(205));
            dgvSuppliers.Location = new Point(0, 10);
            dgvSuppliers.Margin = new Padding(0);
            dgvSuppliers.MultiSelect = false;
            dgvSuppliers.Name = "dgvSuppliers";
            dgvSuppliers.ContextMenuStrip = cmsRows;
            dgvSuppliers.ReadOnly = true;
            dgvSuppliers.RowHeadersVisible = false;
            dgvSuppliers.RowHeadersWidth = 62;
            dgvSuppliers.RowTemplate.Height = 32;
            dgvSuppliers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSuppliers.Size = new Size(940, 336);
            dgvSuppliers.TabIndex = 0;
            dgvSuppliers.TabStop = false;
            tooltip.SetToolTip(dgvSuppliers, "انقر مرتين (أو Enter) لتحديد المورد");
            dgvSuppliers.SelectionChanged += dgvSuppliers_SelectionChanged;
            dgvSuppliers.DoubleClick += dgvSuppliers_DoubleClick;
            dgvSuppliers.KeyDown += dgvSuppliers_KeyDown;
            // 
            // cmsRows
            // 
            // RULE-N23: قائمة سياق على الشبكة — «اختيار» يستدعي نفس زر الشاشة (بلا منطق مكرر).
            cmsRows.Items.AddRange(new ToolStripItem[] { cmsSelect, cmsSeparator, cmsRefresh });
            cmsRows.Name = "cmsRows";
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
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewTextBoxColumn1.DataPropertyName = "Id";
            dataGridViewTextBoxColumn1.FillWeight = 15F;
            dataGridViewTextBoxColumn1.HeaderText = "رقم المورد";
            dataGridViewTextBoxColumn1.MinimumWidth = 8;
            dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewTextBoxColumn2.DataPropertyName = "Name";
            dataGridViewTextBoxColumn2.FillWeight = 40F;
            dataGridViewTextBoxColumn2.HeaderText = "اسم المورد";
            dataGridViewTextBoxColumn2.MinimumWidth = 8;
            dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            dataGridViewTextBoxColumn3.DataPropertyName = "Phone";
            dataGridViewTextBoxColumn3.FillWeight = 20F;
            dataGridViewTextBoxColumn3.HeaderText = "رقم الهاتف";
            dataGridViewTextBoxColumn3.MinimumWidth = 8;
            dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            dataGridViewTextBoxColumn4.DataPropertyName = "Email";
            dataGridViewTextBoxColumn4.FillWeight = 25F;
            dataGridViewTextBoxColumn4.HeaderText = "البريد الإلكتروني";
            dataGridViewTextBoxColumn4.MinimumWidth = 8;
            dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // frmSupplierSelection
            // 
            AcceptButton = btnSelect;
            AutoScaleMode = AutoScaleMode.None;
            BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(170), Convert.ToInt32(220), Convert.ToInt32(170));
            CancelButton = btnCancel;
            ClientSize = new Size(940, 680);
            Controls.Add(pnlGrid);
            Controls.Add(pnlSummary);
            Controls.Add(pnlFooter);
            Controls.Add(pnlFilters);
            Controls.Add(pnlToolbar);
            Controls.Add(pnlHeader);
            Font = new Font("Tahoma", 10F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmSupplierSelection";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "اختيار مورد";
            Load += frmSupplierSelection_Load;
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
            ((System.ComponentModel.ISupportInitialize)dgvSuppliers).EndInit();
            cmsRows.ResumeLayout(false);
            ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.Panel pnlToolbar;
        private System.Windows.Forms.FlowLayoutPanel flpToolbar;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel pnlFilters;
        private System.Windows.Forms.GroupBox gbFilter;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnClearFilters;
        private System.Windows.Forms.Button btnToggleFilters;
        private System.Windows.Forms.Panel pnlSummary;
        private System.Windows.Forms.FlowLayoutPanel flpSummary;
        private System.Windows.Forms.Label lblRecordsCount;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Label lblHint;
        private System.Windows.Forms.Panel pnlGrid;
        private System.Windows.Forms.Label lblNoData;
        private System.Windows.Forms.DataGridView dgvSuppliers;
        private ToolTip tooltip;
        private System.Windows.Forms.ContextMenuStrip cmsRows;
        private System.Windows.Forms.ToolStripMenuItem cmsSelect;
        private System.Windows.Forms.ToolStripSeparator cmsSeparator;
        private System.Windows.Forms.ToolStripMenuItem cmsRefresh;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
    }
}
