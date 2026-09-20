namespace SalesSystem.Desktop.Screens.Inventory.Forms
{
    partial class frmInventory
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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblMode = new System.Windows.Forms.Label();
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tpProducts = new System.Windows.Forms.TabPage();
            this.dgvProducts = new System.Windows.Forms.DataGridView();
            this.pnlProductsFilter = new System.Windows.Forms.Panel();
            this.lblProductsCount = new System.Windows.Forms.Label();
            this.lblProductsCountTitle = new System.Windows.Forms.Label();
            this.cbStockStatusFilter = new System.Windows.Forms.ComboBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.cbWarehouseFilter = new System.Windows.Forms.ComboBox();
            this.lblWarehouse = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.tpCounts = new System.Windows.Forms.TabPage();
            this.dgvCounts = new System.Windows.Forms.DataGridView();
            this.pnlCountsTop = new System.Windows.Forms.Panel();
            this.lblCountsCount = new System.Windows.Forms.Label();
            this.lblCountsCountTitle = new System.Windows.Forms.Label();
            this.tpTransfers = new System.Windows.Forms.TabPage();
            this.dgvTransfers = new System.Windows.Forms.DataGridView();
            this.pnlTransfersTop = new System.Windows.Forms.Panel();
            this.lblTransfersCount = new System.Windows.Forms.Label();
            this.lblTransfersCountTitle = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.tabMain.SuspendLayout();
            this.tpProducts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).BeginInit();
            this.pnlProductsFilter.SuspendLayout();
            this.tpCounts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCounts)).BeginInit();
            this.pnlCountsTop.SuspendLayout();
            this.tpTransfers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransfers)).BeginInit();
            this.pnlTransfersTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblMode
            // 
            this.lblMode.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblMode.AutoSize = true;
            this.lblMode.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblMode.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold);
            this.lblMode.ForeColor = System.Drawing.Color.Firebrick;
            this.lblMode.Location = new System.Drawing.Point(460, 15);
            this.lblMode.Name = "lblMode";
            this.lblMode.Size = new System.Drawing.Size(180, 39);
            this.lblMode.TabIndex = 30;
            this.lblMode.Text = "إدارة المخزون";
            // 
            // tabMain
            // 
            this.tabMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabMain.Controls.Add(this.tpProducts);
            this.tabMain.Controls.Add(this.tpCounts);
            this.tabMain.Controls.Add(this.tpTransfers);
            this.tabMain.Font = new System.Drawing.Font("Tahoma", 10F);
            this.tabMain.Location = new System.Drawing.Point(12, 55);
            this.tabMain.Name = "tabMain";
            this.tabMain.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tabMain.RightToLeftLayout = true;
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(1126, 577);
            this.tabMain.TabIndex = 1;
            // 
            // tpProducts
            // 
            this.tpProducts.BackColor = System.Drawing.Color.White;
            this.tpProducts.Controls.Add(this.dgvProducts);
            this.tpProducts.Controls.Add(this.pnlProductsFilter);
            this.tpProducts.Location = new System.Drawing.Point(4, 30);
            this.tpProducts.Name = "tpProducts";
            this.tpProducts.Padding = new System.Windows.Forms.Padding(3);
            this.tpProducts.Size = new System.Drawing.Size(1118, 543);
            this.tpProducts.TabIndex = 0;
            this.tpProducts.Text = "الأصناف";
            // 
            // dgvProducts
            // 
            this.dgvProducts.AllowUserToAddRows = false;
            this.dgvProducts.AllowUserToDeleteRows = false;
            this.dgvProducts.AllowUserToOrderColumns = true;
            this.dgvProducts.AllowUserToResizeRows = false;
            this.dgvProducts.AutoGenerateColumns = false;
            this.dgvProducts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvProducts.BackgroundColor = System.Drawing.Color.White;
            this.dgvProducts.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvProducts.ColumnHeadersHeight = 50;
            this.dgvProducts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProducts.Location = new System.Drawing.Point(3, 49);
            this.dgvProducts.MultiSelect = false;
            this.dgvProducts.Name = "dgvProducts";
            this.dgvProducts.ReadOnly = true;
            this.dgvProducts.RowHeadersWidth = 51;
            this.dgvProducts.RowTemplate.Height = 28;
            this.dgvProducts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProducts.Size = new System.Drawing.Size(1112, 491);
            this.dgvProducts.TabIndex = 2;
            this.dgvProducts.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvProducts_CellFormatting);
            // 
            // pnlProductsFilter
            // 
            this.pnlProductsFilter.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlProductsFilter.Controls.Add(this.lblProductsCount);
            this.pnlProductsFilter.Controls.Add(this.lblProductsCountTitle);
            this.pnlProductsFilter.Controls.Add(this.cbStockStatusFilter);
            this.pnlProductsFilter.Controls.Add(this.lblStatus);
            this.pnlProductsFilter.Controls.Add(this.cbWarehouseFilter);
            this.pnlProductsFilter.Controls.Add(this.lblWarehouse);
            this.pnlProductsFilter.Controls.Add(this.txtSearch);
            this.pnlProductsFilter.Controls.Add(this.lblSearch);
            this.pnlProductsFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlProductsFilter.Location = new System.Drawing.Point(3, 3);
            this.pnlProductsFilter.Name = "pnlProductsFilter";
            this.pnlProductsFilter.Size = new System.Drawing.Size(1112, 46);
            this.pnlProductsFilter.TabIndex = 1;
            // 
            // lblProductsCount
            // 
            this.lblProductsCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProductsCount.AutoSize = true;
            this.lblProductsCount.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblProductsCount.Location = new System.Drawing.Point(990, 13);
            this.lblProductsCount.Name = "lblProductsCount";
            this.lblProductsCount.Size = new System.Drawing.Size(25, 24);
            this.lblProductsCount.TabIndex = 7;
            this.lblProductsCount.Text = "0";
            // 
            // lblProductsCountTitle
            // 
            this.lblProductsCountTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProductsCountTitle.AutoSize = true;
            this.lblProductsCountTitle.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblProductsCountTitle.Location = new System.Drawing.Point(900, 13);
            this.lblProductsCountTitle.Name = "lblProductsCountTitle";
            this.lblProductsCountTitle.Size = new System.Drawing.Size(96, 24);
            this.lblProductsCountTitle.TabIndex = 6;
            this.lblProductsCountTitle.Text = "عدد الأصناف:";
            // 
            // cbStockStatusFilter
            // 
            this.cbStockStatusFilter.BackColor = System.Drawing.SystemColors.Control;
            this.cbStockStatusFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbStockStatusFilter.Font = new System.Drawing.Font("Tahoma", 10F);
            this.cbStockStatusFilter.FormattingEnabled = true;
            this.cbStockStatusFilter.Items.AddRange(new object[] {
            "الكل",
            "نفذ",
            "محدود",
            "متوفر"});
            this.cbStockStatusFilter.Location = new System.Drawing.Point(610, 9);
            this.cbStockStatusFilter.Name = "cbStockStatusFilter";
            this.cbStockStatusFilter.SelectedIndex = 0;
            this.cbStockStatusFilter.Size = new System.Drawing.Size(110, 32);
            this.cbStockStatusFilter.TabIndex = 5;
            this.cbStockStatusFilter.SelectedIndexChanged += new System.EventHandler(this.cbStockStatusFilter_SelectedIndexChanged);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblStatus.Location = new System.Drawing.Point(520, 13);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(90, 24);
            this.lblStatus.TabIndex = 4;
            this.lblStatus.Text = "حالة الرصيد:";
            // 
            // cbWarehouseFilter
            // 
            this.cbWarehouseFilter.BackColor = System.Drawing.SystemColors.Control;
            this.cbWarehouseFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWarehouseFilter.Font = new System.Drawing.Font("Tahoma", 10F);
            this.cbWarehouseFilter.FormattingEnabled = true;
            this.cbWarehouseFilter.Items.AddRange(new object[] {
            "الكل",
            "الرئيسي",
            "المعرض"});
            this.cbWarehouseFilter.Location = new System.Drawing.Point(380, 9);
            this.cbWarehouseFilter.Name = "cbWarehouseFilter";
            this.cbWarehouseFilter.SelectedIndex = 0;
            this.cbWarehouseFilter.Size = new System.Drawing.Size(120, 32);
            this.cbWarehouseFilter.TabIndex = 3;
            this.cbWarehouseFilter.SelectedIndexChanged += new System.EventHandler(this.cbWarehouseFilter_SelectedIndexChanged);
            // 
            // lblWarehouse
            // 
            this.lblWarehouse.AutoSize = true;
            this.lblWarehouse.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblWarehouse.Location = new System.Drawing.Point(300, 13);
            this.lblWarehouse.Name = "lblWarehouse";
            this.lblWarehouse.Size = new System.Drawing.Size(72, 24);
            this.lblWarehouse.TabIndex = 2;
            this.lblWarehouse.Text = "المستودع:";
            // 
            // txtSearch
            // 
            this.txtSearch.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtSearch.Location = new System.Drawing.Point(48, 9);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(230, 32);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblSearch.Location = new System.Drawing.Point(12, 13);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(37, 24);
            this.lblSearch.TabIndex = 0;
            this.lblSearch.Text = "بحث:";
            // 
            // tpCounts
            // 
            this.tpCounts.BackColor = System.Drawing.Color.White;
            this.tpCounts.Controls.Add(this.dgvCounts);
            this.tpCounts.Controls.Add(this.pnlCountsTop);
            this.tpCounts.Location = new System.Drawing.Point(4, 30);
            this.tpCounts.Name = "tpCounts";
            this.tpCounts.Padding = new System.Windows.Forms.Padding(3);
            this.tpCounts.Size = new System.Drawing.Size(1118, 543);
            this.tpCounts.TabIndex = 1;
            this.tpCounts.Text = "محاضر الجرد";
            // 
            // dgvCounts
            // 
            this.dgvCounts.AllowUserToAddRows = false;
            this.dgvCounts.AllowUserToDeleteRows = false;
            this.dgvCounts.AllowUserToOrderColumns = true;
            this.dgvCounts.AllowUserToResizeRows = false;
            this.dgvCounts.AutoGenerateColumns = false;
            this.dgvCounts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCounts.BackgroundColor = System.Drawing.Color.White;
            this.dgvCounts.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvCounts.ColumnHeadersHeight = 50;
            this.dgvCounts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCounts.Location = new System.Drawing.Point(3, 37);
            this.dgvCounts.MultiSelect = false;
            this.dgvCounts.Name = "dgvCounts";
            this.dgvCounts.ReadOnly = true;
            this.dgvCounts.RowHeadersWidth = 51;
            this.dgvCounts.RowTemplate.Height = 28;
            this.dgvCounts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCounts.Size = new System.Drawing.Size(1112, 503);
            this.dgvCounts.TabIndex = 1;
            this.dgvCounts.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvCounts_CellFormatting);
            // 
            // pnlCountsTop
            // 
            this.pnlCountsTop.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlCountsTop.Controls.Add(this.lblCountsCount);
            this.pnlCountsTop.Controls.Add(this.lblCountsCountTitle);
            this.pnlCountsTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCountsTop.Location = new System.Drawing.Point(3, 3);
            this.pnlCountsTop.Name = "pnlCountsTop";
            this.pnlCountsTop.Size = new System.Drawing.Size(1112, 34);
            this.pnlCountsTop.TabIndex = 0;
            // 
            // lblCountsCount
            // 
            this.lblCountsCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCountsCount.AutoSize = true;
            this.lblCountsCount.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblCountsCount.Location = new System.Drawing.Point(990, 8);
            this.lblCountsCount.Name = "lblCountsCount";
            this.lblCountsCount.Size = new System.Drawing.Size(25, 24);
            this.lblCountsCount.TabIndex = 1;
            this.lblCountsCount.Text = "0";
            // 
            // lblCountsCountTitle
            // 
            this.lblCountsCountTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCountsCountTitle.AutoSize = true;
            this.lblCountsCountTitle.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblCountsCountTitle.Location = new System.Drawing.Point(900, 8);
            this.lblCountsCountTitle.Name = "lblCountsCountTitle";
            this.lblCountsCountTitle.Size = new System.Drawing.Size(96, 24);
            this.lblCountsCountTitle.TabIndex = 0;
            this.lblCountsCountTitle.Text = "عدد المحاضر:";
            // 
            // tpTransfers
            // 
            this.tpTransfers.BackColor = System.Drawing.Color.White;
            this.tpTransfers.Controls.Add(this.dgvTransfers);
            this.tpTransfers.Controls.Add(this.pnlTransfersTop);
            this.tpTransfers.Location = new System.Drawing.Point(4, 30);
            this.tpTransfers.Name = "tpTransfers";
            this.tpTransfers.Padding = new System.Windows.Forms.Padding(3);
            this.tpTransfers.Size = new System.Drawing.Size(1118, 543);
            this.tpTransfers.TabIndex = 2;
            this.tpTransfers.Text = "التحويلات المخزنية";
            // 
            // dgvTransfers
            // 
            this.dgvTransfers.AllowUserToAddRows = false;
            this.dgvTransfers.AllowUserToDeleteRows = false;
            this.dgvTransfers.AllowUserToOrderColumns = true;
            this.dgvTransfers.AllowUserToResizeRows = false;
            this.dgvTransfers.AutoGenerateColumns = false;
            this.dgvTransfers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTransfers.BackgroundColor = System.Drawing.Color.White;
            this.dgvTransfers.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvTransfers.ColumnHeadersHeight = 50;
            this.dgvTransfers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTransfers.Location = new System.Drawing.Point(3, 37);
            this.dgvTransfers.MultiSelect = false;
            this.dgvTransfers.Name = "dgvTransfers";
            this.dgvTransfers.ReadOnly = true;
            this.dgvTransfers.RowHeadersWidth = 51;
            this.dgvTransfers.RowTemplate.Height = 28;
            this.dgvTransfers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTransfers.Size = new System.Drawing.Size(1112, 503);
            this.dgvTransfers.TabIndex = 1;
            this.dgvTransfers.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvTransfers_CellFormatting);
            // 
            // pnlTransfersTop
            // 
            this.pnlTransfersTop.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlTransfersTop.Controls.Add(this.lblTransfersCount);
            this.pnlTransfersTop.Controls.Add(this.lblTransfersCountTitle);
            this.pnlTransfersTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTransfersTop.Location = new System.Drawing.Point(3, 3);
            this.pnlTransfersTop.Name = "pnlTransfersTop";
            this.pnlTransfersTop.Size = new System.Drawing.Size(1112, 34);
            this.pnlTransfersTop.TabIndex = 0;
            // 
            // lblTransfersCount
            // 
            this.lblTransfersCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTransfersCount.AutoSize = true;
            this.lblTransfersCount.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblTransfersCount.Location = new System.Drawing.Point(990, 8);
            this.lblTransfersCount.Name = "lblTransfersCount";
            this.lblTransfersCount.Size = new System.Drawing.Size(25, 24);
            this.lblTransfersCount.TabIndex = 1;
            this.lblTransfersCount.Text = "0";
            // 
            // lblTransfersCountTitle
            // 
            this.lblTransfersCountTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTransfersCountTitle.AutoSize = true;
            this.lblTransfersCountTitle.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblTransfersCountTitle.Location = new System.Drawing.Point(900, 8);
            this.lblTransfersCountTitle.Name = "lblTransfersCountTitle";
            this.lblTransfersCountTitle.Size = new System.Drawing.Size(96, 24);
            this.lblTransfersCountTitle.TabIndex = 0;
            this.lblTransfersCountTitle.Text = "عدد السندات:";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.BackColor = System.Drawing.Color.White;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClose.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btnClose.Location = new System.Drawing.Point(12, 648);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(181, 44);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "إغلاق";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmInventory
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(1150, 700);
            this.Controls.Add(this.tabMain);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblMode);
            this.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "frmInventory";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "إدارة المخزون";
            this.Load += new System.EventHandler(this.frmInventory_Load);
            this.tabMain.ResumeLayout(false);
            this.tpProducts.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).EndInit();
            this.pnlProductsFilter.ResumeLayout(false);
            this.pnlProductsFilter.PerformLayout();
            this.tpCounts.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCounts)).EndInit();
            this.pnlCountsTop.ResumeLayout(false);
            this.pnlCountsTop.PerformLayout();
            this.tpTransfers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransfers)).EndInit();
            this.pnlTransfersTop.ResumeLayout(false);
            this.pnlTransfersTop.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tpProducts;
        private System.Windows.Forms.DataGridView dgvProducts;
        private System.Windows.Forms.Panel pnlProductsFilter;
        private System.Windows.Forms.Label lblProductsCount;
        private System.Windows.Forms.Label lblProductsCountTitle;
        private System.Windows.Forms.ComboBox cbStockStatusFilter;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ComboBox cbWarehouseFilter;
        private System.Windows.Forms.Label lblWarehouse;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TabPage tpCounts;
        private System.Windows.Forms.DataGridView dgvCounts;
        private System.Windows.Forms.Panel pnlCountsTop;
        private System.Windows.Forms.Label lblCountsCount;
        private System.Windows.Forms.Label lblCountsCountTitle;
        private System.Windows.Forms.TabPage tpTransfers;
        private System.Windows.Forms.DataGridView dgvTransfers;
        private System.Windows.Forms.Panel pnlTransfersTop;
        private System.Windows.Forms.Label lblTransfersCount;
        private System.Windows.Forms.Label lblTransfersCountTitle;
        private System.Windows.Forms.Button btnClose;
    }
}