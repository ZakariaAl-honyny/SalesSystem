namespace SalesSystem.Desktop.Products.Forms
{
    partial class frmProductSelection
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
                _scope?.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            // RULE-N134: ألوان المصمّم = افتراضيات ThemePalette حرفياً (المصمّم = وقت التشغيل).
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
            dgvProducts = new DataGridView();
            colId = new DataGridViewTextBoxColumn();
            colName = new DataGridViewTextBoxColumn();
            colCategory = new DataGridViewTextBoxColumn();
            colBarcode = new DataGridViewTextBoxColumn();
            colStock = new DataGridViewTextBoxColumn();
            lblNoData = new Label();
            lblSelectedProduct = new Label();
            dgvUnits = new DataGridView();
            colUnitId = new DataGridViewTextBoxColumn();
            colUnitName = new DataGridViewTextBoxColumn();
            colConversion = new DataGridViewTextBoxColumn();
            colAvailable = new DataGridViewTextBoxColumn();
            colSalePrice = new DataGridViewTextBoxColumn();
            lblNoUnits = new Label();
            pnlHeader.SuspendLayout();
            pnlToolbar.SuspendLayout();
            flpToolbar.SuspendLayout();
            pnlFilters.SuspendLayout();
            gbFilter.SuspendLayout();
            pnlSummary.SuspendLayout();
            flpSummary.SuspendLayout();
            pnlFooter.SuspendLayout();
            pnlGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvProducts).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvUnits).BeginInit();
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
            pnlHeader.Size = new Size(1000, 56);
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
            lblMode.Size = new Size(1000, 56);
            lblMode.TabIndex = 0;
            lblMode.Text = "اختيار منتج";
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
            pnlToolbar.Size = new Size(1000, 64);
            pnlToolbar.TabIndex = 1;
            // 
            // flpToolbar
            // 
            flpToolbar.BackColor = System.Drawing.Color.White;
            flpToolbar.Controls.Add(btnSelect);
            flpToolbar.Controls.Add(btnCancel);
            flpToolbar.Dock = DockStyle.Fill;
            flpToolbar.Location = new Point(0, 0);
            flpToolbar.Margin = new Padding(0);
            flpToolbar.Name = "flpToolbar";
            flpToolbar.Padding = new Padding(10);
            flpToolbar.Size = new Size(1000, 64);
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
            btnSelect.Location = new Point(872, 13);
            btnSelect.Name = "btnSelect";
            btnSelect.Size = new Size(118, 40);
            btnSelect.TabIndex = 0;
            btnSelect.Text = "اختيار";
            tooltip.SetToolTip(btnSelect, "اختيار المنتج والوحدة المحددة (Enter)");
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
            btnCancel.Location = new Point(754, 13);
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
            // RULE-N125/RULE-N63: gbFilter يُضاف أولاً فيملأ الباقي، وbtnToggleFilters بعده فيُقتطع على الحافة.
            pnlFilters.BackColor = System.Drawing.Color.White;
            pnlFilters.Controls.Add(gbFilter);
            pnlFilters.Controls.Add(btnToggleFilters);
            pnlFilters.Dock = DockStyle.Top;
            pnlFilters.Location = new Point(0, 120);
            pnlFilters.Margin = new Padding(0);
            pnlFilters.Name = "pnlFilters";
            pnlFilters.Padding = new Padding(6, 4, 6, 4);
            pnlFilters.Size = new Size(1000, 110);
            pnlFilters.TabIndex = 2;
            // 
            // gbFilter
            // 
            gbFilter.BackColor = System.Drawing.Color.White;
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
            gbFilter.Size = new Size(848, 102);
            gbFilter.TabIndex = 0;
            gbFilter.TabStop = false;
            gbFilter.Text = "بحث وتصفية";
            // 
            // lblSearch
            // 
            lblSearch.AutoSize = true;
            lblSearch.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblSearch.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            lblSearch.Location = new Point(740, 34);
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
            txtSearch.Location = new Point(330, 30);
            txtSearch.Margin = new Padding(0);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(400, 32);
            txtSearch.TabIndex = 1;
            tooltip.SetToolTip(txtSearch, "بحث فوري بالاسم أو الباركود أو التصنيف — F5 لتحديث القائمة");
            txtSearch.TextChanged += txtSearch_TextChanged;
            txtSearch.KeyDown += txtSearch_KeyDown;
            txtSearch.KeyPress += txtSearch_KeyPress;
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
            btnClearFilters.Location = new Point(110, 30);
            btnClearFilters.Name = "btnClearFilters";
            btnClearFilters.Size = new Size(190, 32);
            btnClearFilters.TabIndex = 2;
            btnClearFilters.Text = "مسح التصفية";
            tooltip.SetToolTip(btnClearFilters, "مسح التصفية فقط — لا يعيد تحميل البيانات ولا يغيّر السطر المحدد");
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
            btnToggleFilters.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(240), Convert.ToInt32(240), Convert.ToInt32(240));
            btnToggleFilters.FlatStyle = FlatStyle.Popup;
            btnToggleFilters.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            btnToggleFilters.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
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
            pnlSummary.Location = new Point(0, 716);
            pnlSummary.Margin = new Padding(0);
            pnlSummary.Name = "pnlSummary";
            pnlSummary.Size = new Size(1000, 46);
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
            flpSummary.Size = new Size(1000, 46);
            flpSummary.TabIndex = 0;
            flpSummary.WrapContents = false;
            // 
            // lblRecordsCount
            // 
            lblRecordsCount.AutoSize = true;
            lblRecordsCount.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblRecordsCount.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
            lblRecordsCount.Location = new Point(872, 8);
            lblRecordsCount.Margin = new Padding(4, 0, 24, 0);
            lblRecordsCount.Name = "lblRecordsCount";
            lblRecordsCount.Size = new Size(98, 24);
            lblRecordsCount.TabIndex = 0;
            lblRecordsCount.Text = "النتائج: 0";
            // 
            // pnlFooter
            // 
            pnlFooter.BackColor = System.Drawing.Color.White;
            pnlFooter.Controls.Add(lblHint);
            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Location = new Point(0, 762);
            pnlFooter.Margin = new Padding(0);
            pnlFooter.Name = "pnlFooter";
            pnlFooter.Padding = new Padding(10, 0, 10, 0);
            pnlFooter.Size = new Size(1000, 58);
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
            lblHint.Size = new Size(980, 58);
            lblHint.TabIndex = 0;
            lblHint.Text = "↓ انتقال إلى القائمة · Enter اختيار · Esc إلغاء · F5 تحديث";
            lblHint.TextAlign = ContentAlignment.MiddleRight;
            // 
            // pnlGrid
            // 
            // RULE-N125: يُضاف عنصر Fill الوحيد أولاً ليُرسى آخراً ويأخذ الباقي — كما في frmListSales.
            pnlGrid.BackColor = System.Drawing.Color.White;
            pnlGrid.Controls.Add(dgvUnits);
            pnlGrid.Controls.Add(lblNoData);
            pnlGrid.Controls.Add(lblNoUnits);
            pnlGrid.Controls.Add(lblSelectedProduct);
            pnlGrid.Controls.Add(dgvProducts);
            pnlGrid.Dock = DockStyle.Fill;
            pnlGrid.Location = new Point(0, 230);
            pnlGrid.Margin = new Padding(0);
            pnlGrid.Name = "pnlGrid";
            pnlGrid.Padding = new Padding(0, 10, 0, 0);
            pnlGrid.Size = new Size(1000, 486);
            pnlGrid.TabIndex = 4;
            // 
            // dgvProducts
            // 
            dgvProducts.AllowUserToAddRows = false;
            dgvProducts.AllowUserToDeleteRows = false;
            dgvProducts.AllowUserToResizeRows = false;
            dgvProducts.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(245), Convert.ToInt32(252), Convert.ToInt32(245));
            dgvProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProducts.BackgroundColor = System.Drawing.Color.FromArgb(Convert.ToInt32(220), Convert.ToInt32(240), Convert.ToInt32(220));
            dgvProducts.BorderStyle = BorderStyle.None;
            dgvProducts.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvProducts.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvProducts.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
            dgvProducts.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            dgvProducts.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dgvProducts.ColumnHeadersDefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
            dgvProducts.ColumnHeadersDefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            dgvProducts.ColumnHeadersHeight = 45;
            dgvProducts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvProducts.Columns.AddRange(new DataGridViewColumn[] { colId, colName, colCategory, colBarcode, colStock });
            dgvProducts.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            dgvProducts.DefaultCellStyle.Font = new Font("Tahoma", 10F);
            dgvProducts.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            dgvProducts.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(255), Convert.ToInt32(255), Convert.ToInt32(180));
            dgvProducts.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            dgvProducts.Dock = DockStyle.Top;
            dgvProducts.EnableHeadersVisualStyles = false;
            dgvProducts.GridColor = System.Drawing.Color.FromArgb(Convert.ToInt32(205), Convert.ToInt32(225), Convert.ToInt32(205));
            dgvProducts.Height = 240;
            dgvProducts.Location = new Point(0, 10);
            dgvProducts.Margin = new Padding(0);
            dgvProducts.MultiSelect = false;
            dgvProducts.Name = "dgvProducts";
            dgvProducts.ContextMenuStrip = cmsRows;
            dgvProducts.ReadOnly = true;
            dgvProducts.RowHeadersVisible = false;
            dgvProducts.RowHeadersWidth = 62;
            dgvProducts.RowTemplate.Height = 32;
            dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProducts.Size = new Size(1000, 240);
            dgvProducts.TabIndex = 0;
            dgvProducts.TabStop = false;
            tooltip.SetToolTip(dgvProducts, "انقر مرتين (أو Enter) للانتقال إلى وحدات المنتج");
            dgvProducts.SelectionChanged += dgvProducts_SelectionChanged;
            dgvProducts.DoubleClick += dgvProducts_DoubleClick;
            dgvProducts.KeyDown += dgvProducts_KeyDown;
            // 
            // colId
            // 
            colId.DataPropertyName = "Id";
            colId.HeaderText = "الرقم";
            colId.MinimumWidth = 8;
            colId.Name = "colId";
            colId.ReadOnly = true;
            // 
            // colName
            // 
            colName.DataPropertyName = "Name";
            colName.HeaderText = "اسم المنتج";
            colName.MinimumWidth = 8;
            colName.Name = "colName";
            colName.ReadOnly = true;
            // 
            // colCategory
            // 
            colCategory.DataPropertyName = "CategoryName";
            colCategory.HeaderText = "التصنيف";
            colCategory.MinimumWidth = 8;
            colCategory.Name = "colCategory";
            colCategory.ReadOnly = true;
            // 
            // colBarcode
            // 
            colBarcode.DataPropertyName = "Barcode";
            colBarcode.HeaderText = "الباركود";
            colBarcode.MinimumWidth = 8;
            colBarcode.Name = "colBarcode";
            colBarcode.ReadOnly = true;
            // 
            // colStock
            // 
            colStock.DataPropertyName = "Available";
            colStock.HeaderText = "المتاح (وحدة أساسية)";
            colStock.MinimumWidth = 8;
            colStock.Name = "colStock";
            colStock.ReadOnly = true;
            colStock.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // lblNoData
            // 
            lblNoData.AutoSize = false;
            lblNoData.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(245), Convert.ToInt32(252), Convert.ToInt32(245));
            lblNoData.Dock = DockStyle.Bottom;
            lblNoData.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblNoData.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
            lblNoData.Height = 24;
            lblNoData.Location = new Point(0, 438);
            lblNoData.Name = "lblNoData";
            lblNoData.Size = new Size(1000, 24);
            lblNoData.TabIndex = 4;
            lblNoData.Text = "لا توجد بيانات";
            lblNoData.TextAlign = ContentAlignment.MiddleCenter;
            lblNoData.Visible = false;
            // 
            // lblSelectedProduct
            // 
            lblSelectedProduct.AutoEllipsis = true;
            lblSelectedProduct.Dock = DockStyle.Top;
            lblSelectedProduct.Font = new Font("Tahoma", 11F, FontStyle.Bold);
            lblSelectedProduct.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
            lblSelectedProduct.Height = 28;
            lblSelectedProduct.Location = new Point(0, 250);
            lblSelectedProduct.Name = "lblSelectedProduct";
            lblSelectedProduct.Size = new Size(1000, 28);
            lblSelectedProduct.TabIndex = 1;
            lblSelectedProduct.Text = "المنتج المحدد: —";
            lblSelectedProduct.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // dgvUnits
            // 
            dgvUnits.AllowUserToAddRows = false;
            dgvUnits.AllowUserToDeleteRows = false;
            dgvUnits.AllowUserToResizeRows = false;
            dgvUnits.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(245), Convert.ToInt32(252), Convert.ToInt32(245));
            dgvUnits.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUnits.BackgroundColor = System.Drawing.Color.FromArgb(Convert.ToInt32(220), Convert.ToInt32(240), Convert.ToInt32(220));
            dgvUnits.BorderStyle = BorderStyle.None;
            dgvUnits.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvUnits.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvUnits.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
            dgvUnits.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            dgvUnits.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dgvUnits.ColumnHeadersDefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
            dgvUnits.ColumnHeadersDefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            dgvUnits.ColumnHeadersHeight = 45;
            dgvUnits.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvUnits.Columns.AddRange(new DataGridViewColumn[] { colUnitId, colUnitName, colConversion, colAvailable, colSalePrice });
            dgvUnits.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            dgvUnits.DefaultCellStyle.Font = new Font("Tahoma", 10F);
            dgvUnits.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            dgvUnits.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(255), Convert.ToInt32(255), Convert.ToInt32(180));
            dgvUnits.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            dgvUnits.Dock = DockStyle.Fill;
            dgvUnits.EnableHeadersVisualStyles = false;
            dgvUnits.GridColor = System.Drawing.Color.FromArgb(Convert.ToInt32(205), Convert.ToInt32(225), Convert.ToInt32(205));
            dgvUnits.Location = new Point(0, 278);
            dgvUnits.Margin = new Padding(0);
            dgvUnits.MultiSelect = false;
            dgvUnits.Name = "dgvUnits";
            dgvUnits.ReadOnly = true;
            dgvUnits.RowHeadersVisible = false;
            dgvUnits.RowHeadersWidth = 62;
            dgvUnits.RowTemplate.Height = 32;
            dgvUnits.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUnits.Size = new Size(1000, 160);
            dgvUnits.TabIndex = 2;
            tooltip.SetToolTip(dgvUnits, "انقر مرتين (أو Enter) لاختيار الوحدة");
            dgvUnits.DoubleClick += dgvUnits_DoubleClick;
            dgvUnits.KeyDown += dgvUnits_KeyDown;
            // 
            // colUnitId
            // 
            colUnitId.DataPropertyName = "UnitId";
            colUnitId.HeaderText = "معرف الوحدة";
            colUnitId.MinimumWidth = 8;
            colUnitId.Name = "colUnitId";
            colUnitId.ReadOnly = true;
            colUnitId.Visible = false;
            // 
            // colUnitName
            // 
            colUnitName.DataPropertyName = "UnitName";
            colUnitName.FillWeight = 60F;
            colUnitName.HeaderText = "الوحدة";
            colUnitName.MinimumWidth = 8;
            colUnitName.Name = "colUnitName";
            colUnitName.ReadOnly = true;
            // 
            // colConversion
            // 
            colConversion.DataPropertyName = "ConversionFactor";
            colConversion.FillWeight = 40F;
            colConversion.HeaderText = "معامل التحويل";
            colConversion.MinimumWidth = 8;
            colConversion.Name = "colConversion";
            colConversion.ReadOnly = true;
            colConversion.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colAvailable
            // 
            colAvailable.DataPropertyName = "Available";
            colAvailable.FillWeight = 45F;
            colAvailable.HeaderText = "الكمية المتاحة";
            colAvailable.MinimumWidth = 8;
            colAvailable.Name = "colAvailable";
            colAvailable.ReadOnly = true;
            colAvailable.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colSalePrice
            // 
            colSalePrice.DataPropertyName = "SalePrice";
            colSalePrice.FillWeight = 45F;
            colSalePrice.HeaderText = "سعر البيع";
            colSalePrice.MinimumWidth = 8;
            colSalePrice.Name = "colSalePrice";
            colSalePrice.ReadOnly = true;
            colSalePrice.DefaultCellStyle.Format = "N2";
            // 
            // lblNoUnits
            // 
            lblNoUnits.AutoSize = false;
            lblNoUnits.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(245), Convert.ToInt32(252), Convert.ToInt32(245));
            lblNoUnits.Dock = DockStyle.Bottom;
            lblNoUnits.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblNoUnits.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
            lblNoUnits.Height = 24;
            lblNoUnits.Location = new Point(0, 462);
            lblNoUnits.Name = "lblNoUnits";
            lblNoUnits.Size = new Size(1000, 24);
            lblNoUnits.TabIndex = 3;
            lblNoUnits.Text = "لا توجد وحدات لهذا الصنف";
            lblNoUnits.TextAlign = ContentAlignment.MiddleCenter;
            lblNoUnits.Visible = false;
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
            // frmProductSelection
            // 
            AcceptButton = btnSelect;
            AutoScaleMode = AutoScaleMode.None;
            BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(170), Convert.ToInt32(220), Convert.ToInt32(170));
            CancelButton = btnCancel;
            ClientSize = new Size(1000, 820);
            Controls.Add(pnlGrid);
            Controls.Add(pnlSummary);
            Controls.Add(pnlFooter);
            Controls.Add(pnlFilters);
            Controls.Add(pnlToolbar);
            Controls.Add(pnlHeader);
            Font = new Font("Tahoma", 10F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmProductSelection";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "اختيار منتج";
            Load += frmProductSelection_Load;
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
            ((System.ComponentModel.ISupportInitialize)dgvProducts).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvUnits).EndInit();
            cmsRows.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.DataGridView dgvProducts;
        private System.Windows.Forms.DataGridView dgvUnits;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ContextMenuStrip cmsRows;
        private System.Windows.Forms.ToolStripMenuItem cmsSelect;
        private System.Windows.Forms.ToolStripSeparator cmsSeparator;
        private System.Windows.Forms.ToolStripMenuItem cmsRefresh;
        private DataGridViewTextBoxColumn colId;
        private DataGridViewTextBoxColumn colName;
        private DataGridViewTextBoxColumn colCategory;
        private DataGridViewTextBoxColumn colBarcode;
        private DataGridViewTextBoxColumn colStock;
        private DataGridViewTextBoxColumn colUnitId;
        private DataGridViewTextBoxColumn colUnitName;
        private DataGridViewTextBoxColumn colConversion;
        private DataGridViewTextBoxColumn colAvailable;
        private DataGridViewTextBoxColumn colSalePrice;
        private Label lblSelectedProduct;
        private Label lblNoUnits;
        private Label lblSearch;
        private Label lblMode;
        private Label lblRecordsCount;
        private Label lblNoData;
        private ToolTip tooltip;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Panel pnlToolbar;
        private System.Windows.Forms.FlowLayoutPanel flpToolbar;
        private System.Windows.Forms.Panel pnlFilters;
        private System.Windows.Forms.GroupBox gbFilter;
        private System.Windows.Forms.Button btnClearFilters;
        private System.Windows.Forms.Button btnToggleFilters;
        private System.Windows.Forms.Panel pnlSummary;
        private System.Windows.Forms.FlowLayoutPanel flpSummary;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Label lblHint;
        private System.Windows.Forms.Panel pnlGrid;
    }
}
