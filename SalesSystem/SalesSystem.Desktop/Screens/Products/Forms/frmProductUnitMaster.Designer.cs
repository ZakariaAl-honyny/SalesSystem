namespace SalesSystem.Desktop.Screens.Products.Forms;

partial class frmProductUnitMaster
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

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        lblMode = new Label();
        pnlFields = new Panel();
        pnlFieldsRow1 = new FlowLayoutPanel();
        lblUnit = new Label();
        lblUnitReq = new Label();
        cmbUnit = new ComboBox();
        lblBarcode = new Label();
        txtBarcode = new TextBox();
        lblFactor = new Label();
        numFactor = new NumericUpDown();
        lblSalePrice = new Label();
        numSalePrice = new NumericUpDown();
        pnlFieldsRow2 = new FlowLayoutPanel();
        lblWholesalePrice = new Label();
        numWholesalePrice = new NumericUpDown();
        lblCostPrice = new Label();
        numCostPrice = new NumericUpDown();
        lblMaxDiscount = new Label();
        numMaxDiscount = new NumericUpDown();
        lblProduct = new Label();
        pnlButtons = new FlowLayoutPanel();
        btnNew = new Button();
        btnRefresh = new Button();
        btnSave = new Button();
        btnDelete = new Button();
        btnClose = new Button();
        pnlGrid = new Panel();
        dgvUnits = new DataGridView();
        colId = new DataGridViewTextBoxColumn();
        colUnitName = new DataGridViewTextBoxColumn();
        colBarcode = new DataGridViewTextBoxColumn();
        colConversionFactor = new DataGridViewTextBoxColumn();
        colSalePrice = new DataGridViewTextBoxColumn();
        colCostPrice = new DataGridViewTextBoxColumn();
        colIsBaseUnit = new DataGridViewCheckBoxColumn();
        pnlFilter = new Panel();
        txtSearch = new TextBox();
        lblSearch = new Label();
        errorProvider = new ErrorProvider(components);
        pnlFields.SuspendLayout();
        pnlFieldsRow1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numFactor).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numSalePrice).BeginInit();
        pnlFieldsRow2.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numWholesalePrice).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numCostPrice).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numMaxDiscount).BeginInit();
        pnlButtons.SuspendLayout();
        pnlGrid.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvUnits).BeginInit();
        pnlFilter.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
        SuspendLayout();
        // 
        // lblMode
        // 
        lblMode.BackColor = Color.FromArgb(14, 165, 103);
        lblMode.Dock = DockStyle.Top;
        lblMode.Font = new Font("Tahoma", 14F, FontStyle.Bold);
        lblMode.ForeColor = Color.White;
        lblMode.Location = new Point(0, 0);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(1200, 50);
        lblMode.TabIndex = 0;
        lblMode.Text = "(( وحدات الصنف ))";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlFields
        // 
        pnlFields.BackColor = Color.FromArgb(235, 245, 235);
        pnlFields.Controls.Add(pnlFieldsRow2);
        pnlFields.Controls.Add(pnlFieldsRow1);
        pnlFields.Controls.Add(lblProduct);
        pnlFields.Dock = DockStyle.Top;
        pnlFields.Location = new Point(0, 50);
        pnlFields.Name = "pnlFields";
        pnlFields.Size = new Size(1200, 132);
        pnlFields.TabIndex = 1;
        // 
        // pnlFieldsRow1
        // 
        pnlFieldsRow1.Controls.Add(lblUnit);
        pnlFieldsRow1.Controls.Add(lblUnitReq);
        pnlFieldsRow1.Controls.Add(cmbUnit);
        pnlFieldsRow1.Controls.Add(lblBarcode);
        pnlFieldsRow1.Controls.Add(txtBarcode);
        pnlFieldsRow1.Controls.Add(lblFactor);
        pnlFieldsRow1.Controls.Add(numFactor);
        pnlFieldsRow1.Controls.Add(lblSalePrice);
        pnlFieldsRow1.Controls.Add(numSalePrice);
        pnlFieldsRow1.Dock = DockStyle.Top;
        pnlFieldsRow1.Location = new Point(0, 30);
        pnlFieldsRow1.Name = "pnlFieldsRow1";
        pnlFieldsRow1.Padding = new Padding(10, 4, 10, 4);
        pnlFieldsRow1.Size = new Size(1200, 50);
        pnlFieldsRow1.TabIndex = 0;
        pnlFieldsRow1.WrapContents = false;
        // 
        // lblUnit
        // 
        lblUnit.AutoSize = true;
        lblUnit.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblUnit.Margin = new Padding(10, 12, 3, 0);
        lblUnit.Name = "lblUnit";
        lblUnit.Size = new Size(66, 24);
        lblUnit.TabIndex = 0;
        lblUnit.Text = "الوحدة:";
        // 
        // lblUnitReq
        // 
        lblUnitReq.AutoSize = true;
        lblUnitReq.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblUnitReq.ForeColor = Color.Red;
        lblUnitReq.Margin = new Padding(0, 12, 6, 0);
        lblUnitReq.Name = "lblUnitReq";
        lblUnitReq.Text = "*";
        // 
        // cmbUnit
        // 
        cmbUnit.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbUnit.Font = new Font("Tahoma", 10F);
        cmbUnit.FormattingEnabled = true;
        cmbUnit.Margin = new Padding(3, 6, 20, 0);
        cmbUnit.Name = "cmbUnit";
        cmbUnit.Size = new Size(200, 32);
        cmbUnit.TabIndex = 0;
        // 
        // lblBarcode
        // 
        lblBarcode.AutoSize = true;
        lblBarcode.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblBarcode.Margin = new Padding(10, 12, 3, 0);
        lblBarcode.Name = "lblBarcode";
        lblBarcode.Size = new Size(90, 24);
        lblBarcode.TabIndex = 2;
        lblBarcode.Text = "الباركود:";
        // 
        // txtBarcode
        // 
        txtBarcode.Font = new Font("Tahoma", 10F);
        txtBarcode.Margin = new Padding(3, 6, 20, 0);
        txtBarcode.Name = "txtBarcode";
        txtBarcode.Size = new Size(180, 32);
        txtBarcode.TabIndex = 1;
        // 
        // lblFactor
        // 
        lblFactor.AutoSize = true;
        lblFactor.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblFactor.Margin = new Padding(10, 12, 3, 0);
        lblFactor.Name = "lblFactor";
        lblFactor.Size = new Size(130, 24);
        lblFactor.TabIndex = 4;
        lblFactor.Text = "معامل التحويل:";
        // 
        // numFactor
        // 
        numFactor.DecimalPlaces = 3;
        numFactor.Font = new Font("Tahoma", 10F);
        numFactor.Location = new Point(0, 0);
        numFactor.Margin = new Padding(3, 6, 20, 0);
        numFactor.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
        numFactor.Minimum = new decimal(new int[] { 1, 0, 0, 196608 });
        numFactor.Name = "numFactor";
        numFactor.Size = new Size(110, 32);
        numFactor.TabIndex = 2;
        numFactor.Value = new decimal(new int[] { 1, 0, 0, 0 });
        // 
        // lblSalePrice
        // 
        lblSalePrice.AutoSize = true;
        lblSalePrice.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblSalePrice.Margin = new Padding(10, 12, 3, 0);
        lblSalePrice.Name = "lblSalePrice";
        lblSalePrice.Size = new Size(110, 24);
        lblSalePrice.TabIndex = 6;
        lblSalePrice.Text = "سعر التجزئة:";
        // 
        // numSalePrice
        // 
        numSalePrice.DecimalPlaces = 2;
        numSalePrice.Font = new Font("Tahoma", 10F);
        numSalePrice.Location = new Point(0, 0);
        numSalePrice.Margin = new Padding(3, 6, 20, 0);
        numSalePrice.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 });
        numSalePrice.Name = "numSalePrice";
        numSalePrice.Size = new Size(130, 32);
        numSalePrice.TabIndex = 3;
        numSalePrice.ThousandsSeparator = true;
        // 
        // pnlFieldsRow2
        // 
        pnlFieldsRow2.Controls.Add(lblWholesalePrice);
        pnlFieldsRow2.Controls.Add(numWholesalePrice);
        pnlFieldsRow2.Controls.Add(lblCostPrice);
        pnlFieldsRow2.Controls.Add(numCostPrice);
        pnlFieldsRow2.Controls.Add(lblMaxDiscount);
        pnlFieldsRow2.Controls.Add(numMaxDiscount);
        pnlFieldsRow2.Dock = DockStyle.Top;
        pnlFieldsRow2.Location = new Point(0, 80);
        pnlFieldsRow2.Name = "pnlFieldsRow2";
        pnlFieldsRow2.Padding = new Padding(10, 4, 10, 4);
        pnlFieldsRow2.Size = new Size(1200, 50);
        pnlFieldsRow2.TabIndex = 1;
        pnlFieldsRow2.WrapContents = false;
        // 
        // lblWholesalePrice
        // 
        lblWholesalePrice.AutoSize = true;
        lblWholesalePrice.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblWholesalePrice.Margin = new Padding(10, 12, 3, 0);
        lblWholesalePrice.Name = "lblWholesalePrice";
        lblWholesalePrice.Size = new Size(100, 24);
        lblWholesalePrice.TabIndex = 0;
        lblWholesalePrice.Text = "سعر الجملة:";
        // 
        // numWholesalePrice
        // 
        numWholesalePrice.DecimalPlaces = 2;
        numWholesalePrice.Font = new Font("Tahoma", 10F);
        numWholesalePrice.Location = new Point(0, 0);
        numWholesalePrice.Margin = new Padding(3, 6, 20, 0);
        numWholesalePrice.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 });
        numWholesalePrice.Name = "numWholesalePrice";
        numWholesalePrice.Size = new Size(130, 32);
        numWholesalePrice.TabIndex = 0;
        numWholesalePrice.ThousandsSeparator = true;
        // 
        // lblCostPrice
        // 
        lblCostPrice.AutoSize = true;
        lblCostPrice.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblCostPrice.Margin = new Padding(10, 12, 3, 0);
        lblCostPrice.Name = "lblCostPrice";
        lblCostPrice.Size = new Size(100, 24);
        lblCostPrice.TabIndex = 2;
        lblCostPrice.Text = "سعر التكلفة:";
        // 
        // numCostPrice
        // 
        numCostPrice.DecimalPlaces = 2;
        numCostPrice.Font = new Font("Tahoma", 10F);
        numCostPrice.Location = new Point(0, 0);
        numCostPrice.Margin = new Padding(3, 6, 20, 0);
        numCostPrice.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 });
        numCostPrice.Name = "numCostPrice";
        numCostPrice.Size = new Size(130, 32);
        numCostPrice.TabIndex = 1;
        numCostPrice.ThousandsSeparator = true;
        // 
        // lblMaxDiscount
        // 
        lblMaxDiscount.AutoSize = true;
        lblMaxDiscount.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblMaxDiscount.Margin = new Padding(10, 12, 3, 0);
        lblMaxDiscount.Name = "lblMaxDiscount";
        lblMaxDiscount.Size = new Size(100, 24);
        lblMaxDiscount.TabIndex = 4;
        lblMaxDiscount.Text = "أقصى خصم:";
        // 
        // numMaxDiscount
        // 
        numMaxDiscount.DecimalPlaces = 2;
        numMaxDiscount.Font = new Font("Tahoma", 10F);
        numMaxDiscount.Location = new Point(0, 0);
        numMaxDiscount.Margin = new Padding(3, 6, 20, 0);
        numMaxDiscount.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 });
        numMaxDiscount.Name = "numMaxDiscount";
        numMaxDiscount.Size = new Size(130, 32);
        numMaxDiscount.TabIndex = 2;
        numMaxDiscount.ThousandsSeparator = true;
        // 
        // lblProduct
        // 
        lblProduct.Dock = DockStyle.Top;
        lblProduct.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblProduct.ForeColor = Color.FromArgb(30, 86, 160);
        lblProduct.Location = new Point(0, 0);
        lblProduct.Name = "lblProduct";
        lblProduct.Padding = new Padding(10, 6, 0, 0);
        lblProduct.Size = new Size(1200, 30);
        lblProduct.TabIndex = 2;
        lblProduct.Text = "الصنف:";
        // 
        // pnlButtons
        // 
        pnlButtons.BackColor = Color.FromArgb(235, 240, 235);
        pnlButtons.Controls.Add(btnNew);
        pnlButtons.Controls.Add(btnSave);
        pnlButtons.Controls.Add(btnDelete);
        pnlButtons.Controls.Add(btnClose);
        pnlButtons.Dock = DockStyle.Top;
        pnlButtons.Location = new Point(0, 182);
        pnlButtons.Name = "pnlButtons";
        pnlButtons.Padding = new Padding(10);
        pnlButtons.Size = new Size(1200, 60);
        pnlButtons.TabIndex = 2;
        // 
        // btnNew
        // 
        btnNew.BackColor = Color.White;
        btnNew.FlatStyle = FlatStyle.Flat;
        btnNew.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnNew.Margin = new Padding(3, 3, 8, 3);
        btnNew.Name = "btnNew";
        btnNew.Size = new Size(150, 36);
        btnNew.TabIndex = 0;
        btnNew.Text = "🆕 إضافة";
        btnNew.UseVisualStyleBackColor = false;
        // 
        // btnSave
        // 
        btnSave.BackColor = Color.White;
        btnSave.FlatStyle = FlatStyle.Flat;
        btnSave.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnSave.Margin = new Padding(3, 3, 8, 3);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(150, 36);
        btnSave.TabIndex = 1;
        btnSave.Text = "💾 حفظ";
        btnSave.UseVisualStyleBackColor = false;
        // 
        // btnDelete
        // 
        btnDelete.BackColor = Color.White;
        btnDelete.FlatStyle = FlatStyle.Flat;
        btnDelete.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnDelete.Margin = new Padding(3, 3, 24, 3);
        btnDelete.Name = "btnDelete";
        btnDelete.Size = new Size(150, 36);
        btnDelete.TabIndex = 2;
        btnDelete.Text = "🗑 حذف";
        btnDelete.UseVisualStyleBackColor = false;
        // 
        // btnClose
        // 
        btnClose.BackColor = Color.FromArgb(30, 86, 160);
        btnClose.FlatStyle = FlatStyle.Flat;
        btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnClose.ForeColor = Color.White;
        btnClose.Margin = new Padding(3, 3, 8, 3);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(140, 36);
        btnClose.TabIndex = 3;
        btnClose.Text = "إغلاق";
        btnClose.UseVisualStyleBackColor = false;
        // 
        // pnlGrid
        // 
        pnlGrid.Controls.Add(dgvUnits);
        pnlGrid.Controls.Add(pnlFilter);
        pnlGrid.Dock = DockStyle.Fill;
        pnlGrid.Location = new Point(0, 242);
        pnlGrid.Name = "pnlGrid";
        pnlGrid.Padding = new Padding(10);
        pnlGrid.Size = new Size(1200, 458);
        pnlGrid.TabIndex = 3;
        // 
        // dgvUnits
        // 
        dgvUnits.AllowUserToAddRows = false;
        dgvUnits.AllowUserToDeleteRows = false;
        dgvUnits.AutoGenerateColumns = false;
        dgvUnits.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvUnits.BackgroundColor = Color.FromArgb(220, 240, 220);
        dgvUnits.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvUnits.Columns.AddRange(new DataGridViewColumn[] { colId, colUnitName, colBarcode, colConversionFactor, colSalePrice, colCostPrice, colIsBaseUnit });
        dgvUnits.Dock = DockStyle.Fill;
        dgvUnits.Font = new Font("Tahoma", 10F);
        dgvUnits.Location = new Point(10, 50);
        dgvUnits.MultiSelect = false;
        dgvUnits.Name = "dgvUnits";
        dgvUnits.ReadOnly = true;
        dgvUnits.RowHeadersVisible = false;
        dgvUnits.RowHeadersWidth = 62;
        dgvUnits.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvUnits.Size = new Size(1180, 398);
        dgvUnits.TabIndex = 0;
        // 
        // colId
        // 
        colId.DataPropertyName = "Id";
        colId.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter };
        colId.FillWeight = 8F;
        colId.HeaderText = "رقم الوحدة";
        colId.MinimumWidth = 90;
        colId.Name = "colId";
        colId.ReadOnly = true;
        // 
        // colUnitName
        // 
        colUnitName.DataPropertyName = "UnitName";
        colUnitName.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter };
        colUnitName.FillWeight = 18F;
        colUnitName.HeaderText = "الوحدة";
        colUnitName.MinimumWidth = 130;
        colUnitName.Name = "colUnitName";
        colUnitName.ReadOnly = true;
        // 
        // colBarcode
        // 
        colBarcode.DataPropertyName = "Barcode";
        colBarcode.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter };
        colBarcode.FillWeight = 16F;
        colBarcode.HeaderText = "الباركود";
        colBarcode.MinimumWidth = 130;
        colBarcode.Name = "colBarcode";
        colBarcode.ReadOnly = true;
        // 
        // colConversionFactor
        // 
        colConversionFactor.DataPropertyName = "ConversionFactor";
        colConversionFactor.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter, Format = "N3" };
        colConversionFactor.FillWeight = 12F;
        colConversionFactor.HeaderText = "معامل التحويل";
        colConversionFactor.MinimumWidth = 110;
        colConversionFactor.Name = "colConversionFactor";
        colConversionFactor.ReadOnly = true;
        // 
        // colSalePrice
        // 
        colSalePrice.DataPropertyName = "SalePrice";
        colSalePrice.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter, Format = "N2" };
        colSalePrice.FillWeight = 13F;
        colSalePrice.HeaderText = "سعر البيع";
        colSalePrice.MinimumWidth = 110;
        colSalePrice.Name = "colSalePrice";
        colSalePrice.ReadOnly = true;
        // 
        // colCostPrice
        // 
        colCostPrice.DataPropertyName = "CostPrice";
        colCostPrice.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter, Format = "N2" };
        colCostPrice.FillWeight = 13F;
        colCostPrice.HeaderText = "سعر التكلفة";
        colCostPrice.MinimumWidth = 110;
        colCostPrice.Name = "colCostPrice";
        colCostPrice.ReadOnly = true;
        // 
        // colIsBaseUnit
        // 
        colIsBaseUnit.DataPropertyName = "IsBaseUnit";
        colIsBaseUnit.FillWeight = 10F;
        colIsBaseUnit.HeaderText = "الوحدة الأساسية";
        colIsBaseUnit.MinimumWidth = 110;
        colIsBaseUnit.Name = "colIsBaseUnit";
        colIsBaseUnit.ReadOnly = true;
        // 
        // pnlFilter
        // 
        pnlFilter.Controls.Add(btnRefresh);
        pnlFilter.Controls.Add(txtSearch);
        pnlFilter.Controls.Add(lblSearch);
        pnlFilter.Dock = DockStyle.Top;
        pnlFilter.Location = new Point(10, 10);
        pnlFilter.Name = "pnlFilter";
        pnlFilter.Size = new Size(1180, 40);
        pnlFilter.TabIndex = 1;
        // 
        // txtSearch
        // 
        txtSearch.Font = new Font("Tahoma", 10F);
        txtSearch.Location = new Point(690, 4);
        txtSearch.Name = "txtSearch";
        txtSearch.Size = new Size(300, 32);
        txtSearch.TabIndex = 99;
        // 
        // btnRefresh
        // 
        btnRefresh.BackColor = Color.White;
        btnRefresh.FlatStyle = FlatStyle.Flat;
        btnRefresh.Location = new Point(550, 4);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(130, 32);
        btnRefresh.TabIndex = 98;
        btnRefresh.Text = "🔄 تحديث";
        btnRefresh.UseVisualStyleBackColor = false;
        // 
        // lblSearch
        // 
        lblSearch.AutoSize = true;
        lblSearch.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblSearch.Location = new Point(1000, 8);
        lblSearch.Name = "lblSearch";
        lblSearch.Size = new Size(56, 24);
        lblSearch.TabIndex = 1;
        lblSearch.Text = "بحث:";
        // 
        // errorProvider
        // 
        errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        errorProvider.ContainerControl = this;
        errorProvider.RightToLeft = true;
        // 
        // frmProductUnitMaster
        // 
        AutoScaleDimensions = new SizeF(11F, 24F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(170, 220, 170);
        ClientSize = new Size(1200, 700);
        Controls.Add(pnlGrid);
        Controls.Add(pnlButtons);
        Controls.Add(pnlFields);
        Controls.Add(lblMode);
        Font = new Font("Tahoma", 10F);
        MinimumSize = new Size(900, 600);
        Name = "frmProductUnitMaster";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        ShowIcon = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "وحدات الصنف";
        pnlFields.ResumeLayout(false);
        pnlFieldsRow1.ResumeLayout(false);
        pnlFieldsRow1.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numFactor).EndInit();
        ((System.ComponentModel.ISupportInitialize)numSalePrice).EndInit();
        pnlFieldsRow2.ResumeLayout(false);
        pnlFieldsRow2.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numWholesalePrice).EndInit();
        ((System.ComponentModel.ISupportInitialize)numCostPrice).EndInit();
        ((System.ComponentModel.ISupportInitialize)numMaxDiscount).EndInit();
        pnlButtons.ResumeLayout(false);
        pnlGrid.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvUnits).EndInit();
        pnlFilter.ResumeLayout(false);
        pnlFilter.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        ResumeLayout(false);
    }

    private System.Windows.Forms.Label lblMode;
    private System.Windows.Forms.Panel pnlFields;
    private System.Windows.Forms.Label lblProduct;
    private System.Windows.Forms.FlowLayoutPanel pnlFieldsRow1;
    private System.Windows.Forms.Label lblUnit;
    private System.Windows.Forms.Label lblUnitReq;
    private System.Windows.Forms.ComboBox cmbUnit;
    private System.Windows.Forms.Label lblBarcode;
    private System.Windows.Forms.TextBox txtBarcode;
    private System.Windows.Forms.Label lblFactor;
    private System.Windows.Forms.NumericUpDown numFactor;
    private System.Windows.Forms.Label lblSalePrice;
    private System.Windows.Forms.NumericUpDown numSalePrice;
    private System.Windows.Forms.FlowLayoutPanel pnlFieldsRow2;
    private System.Windows.Forms.Label lblWholesalePrice;
    private System.Windows.Forms.NumericUpDown numWholesalePrice;
    private System.Windows.Forms.Label lblCostPrice;
    private System.Windows.Forms.NumericUpDown numCostPrice;
    private System.Windows.Forms.Label lblMaxDiscount;
    private System.Windows.Forms.NumericUpDown numMaxDiscount;
    private System.Windows.Forms.FlowLayoutPanel pnlButtons;
    private System.Windows.Forms.Button btnNew;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.Button btnDelete;
    private System.Windows.Forms.Button btnClose;
    private System.Windows.Forms.Panel pnlGrid;
    private System.Windows.Forms.DataGridView dgvUnits;
    private System.Windows.Forms.DataGridViewTextBoxColumn colId;
    private System.Windows.Forms.DataGridViewTextBoxColumn colUnitName;
    private System.Windows.Forms.DataGridViewTextBoxColumn colBarcode;
    private System.Windows.Forms.DataGridViewTextBoxColumn colConversionFactor;
    private System.Windows.Forms.DataGridViewTextBoxColumn colSalePrice;
    private System.Windows.Forms.DataGridViewTextBoxColumn colCostPrice;
    private System.Windows.Forms.DataGridViewCheckBoxColumn colIsBaseUnit;
    private System.Windows.Forms.Panel pnlFilter;
    private System.Windows.Forms.TextBox txtSearch;
    private System.Windows.Forms.Button btnRefresh;
    private System.Windows.Forms.Label lblSearch;
    private System.Windows.Forms.ErrorProvider errorProvider;
}
