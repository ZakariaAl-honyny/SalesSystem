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
        lblProduct = new Label();
        lblUnit = new Label();
        lblUnitReq = new Label();
        cmbUnit = new ComboBox();
        lblBarcode = new Label();
        txtBarcode = new TextBox();
        lblFactor = new Label();
        numFactor = new NumericUpDown();
        lblSalePrice = new Label();
        numSalePrice = new NumericUpDown();
        lblWholesalePrice = new Label();
        numWholesalePrice = new NumericUpDown();
        lblCostPrice = new Label();
        numCostPrice = new NumericUpDown();
        lblMaxDiscount = new Label();
        numMaxDiscount = new NumericUpDown();
        pnlButtons = new FlowLayoutPanel();
        btnNew = new Button();
        btnSave = new Button();
        btnEdit = new Button();
        btnDelete = new Button();
        btnRefresh = new Button();
        btnClose = new Button();
        pnlGrid = new Panel();
        dgvUnits = new DataGridView();
        pnlFilter = new Panel();
        groupBox1 = new GroupBox();
        lblSearch = new Label();
        txtSearch = new TextBox();
        errorProvider = new ErrorProvider(components);
        pnlFields.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numFactor).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numSalePrice).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numWholesalePrice).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numCostPrice).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numMaxDiscount).BeginInit();
        pnlButtons.SuspendLayout();
        pnlGrid.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvUnits).BeginInit();
        pnlFilter.SuspendLayout();
        groupBox1.SuspendLayout();
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
        pnlFields.Controls.Add(lblProduct);
        pnlFields.Controls.Add(lblUnit);
        pnlFields.Controls.Add(lblUnitReq);
        pnlFields.Controls.Add(cmbUnit);
        pnlFields.Controls.Add(lblBarcode);
        pnlFields.Controls.Add(txtBarcode);
        pnlFields.Controls.Add(lblFactor);
        pnlFields.Controls.Add(numFactor);
        pnlFields.Controls.Add(lblSalePrice);
        pnlFields.Controls.Add(numSalePrice);
        pnlFields.Controls.Add(lblWholesalePrice);
        pnlFields.Controls.Add(numWholesalePrice);
        pnlFields.Controls.Add(lblCostPrice);
        pnlFields.Controls.Add(numCostPrice);
        pnlFields.Controls.Add(lblMaxDiscount);
        pnlFields.Controls.Add(numMaxDiscount);
        pnlFields.Dock = DockStyle.Top;
        pnlFields.Location = new Point(0, 50);
        pnlFields.Name = "pnlFields";
        pnlFields.Size = new Size(1200, 180);
        pnlFields.TabIndex = 1;
        // 
        // lblProduct
        // 
        lblProduct.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblProduct.ForeColor = Color.FromArgb(30, 86, 160);
        lblProduct.Location = new Point(20, 6);
        lblProduct.Name = "lblProduct";
        lblProduct.Size = new Size(1160, 26);
        lblProduct.TabIndex = 0;
        lblProduct.Text = "الصنف:";
        lblProduct.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblUnit
        // 
        lblUnit.AutoSize = true;
        lblUnit.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblUnit.Location = new Point(996, 38);
        lblUnit.Name = "lblUnit";
        lblUnit.Size = new Size(79, 24);
        lblUnit.TabIndex = 1;
        lblUnit.Text = "الوحدة:";
        // 
        // lblUnitReq
        // 
        lblUnitReq.AutoSize = true;
        lblUnitReq.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblUnitReq.ForeColor = Color.Red;
        lblUnitReq.Location = new Point(1024, 38);
        lblUnitReq.Name = "lblUnitReq";
        lblUnitReq.Size = new Size(23, 24);
        lblUnitReq.TabIndex = 2;
        lblUnitReq.Text = "*";
        // 
        // cmbUnit
        // 
        cmbUnit.BackColor = Color.White;
        cmbUnit.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbUnit.FlatStyle = FlatStyle.Flat;
        cmbUnit.Font = new Font("Tahoma", 10F);
        cmbUnit.Location = new Point(910, 65);
        cmbUnit.Name = "cmbUnit";
        cmbUnit.Size = new Size(250, 32);
        cmbUnit.TabIndex = 0;
        // 
        // lblBarcode
        // 
        lblBarcode.AutoSize = true;
        lblBarcode.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblBarcode.Location = new Point(732, 38);
        lblBarcode.Name = "lblBarcode";
        lblBarcode.Size = new Size(87, 24);
        lblBarcode.TabIndex = 3;
        lblBarcode.Text = "الباركود:";
        // 
        // txtBarcode
        // 
        txtBarcode.Font = new Font("Tahoma", 10F);
        txtBarcode.Location = new Point(650, 65);
        txtBarcode.Name = "txtBarcode";
        txtBarcode.Size = new Size(250, 32);
        txtBarcode.TabIndex = 1;
        // 
        // lblFactor
        // 
        lblFactor.AutoSize = true;
        lblFactor.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblFactor.Location = new Point(439, 38);
        lblFactor.Name = "lblFactor";
        lblFactor.Size = new Size(152, 24);
        lblFactor.TabIndex = 4;
        lblFactor.Text = "معامل التحويل:";
        // 
        // numFactor
        // 
        numFactor.DecimalPlaces = 3;
        numFactor.Font = new Font("Tahoma", 10F);
        numFactor.Location = new Point(390, 65);
        numFactor.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
        numFactor.Minimum = new decimal(new int[] { 1, 0, 0, 196608 });
        numFactor.Name = "numFactor";
        numFactor.Size = new Size(250, 32);
        numFactor.TabIndex = 2;
        numFactor.Value = new decimal(new int[] { 1, 0, 0, 0 });
        // 
        // lblSalePrice
        // 
        lblSalePrice.AutoSize = true;
        lblSalePrice.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblSalePrice.Location = new Point(189, 38);
        lblSalePrice.Name = "lblSalePrice";
        lblSalePrice.Size = new Size(133, 24);
        lblSalePrice.TabIndex = 5;
        lblSalePrice.Text = "سعر التجزئة:";
        // 
        // numSalePrice
        // 
        numSalePrice.DecimalPlaces = 2;
        numSalePrice.Font = new Font("Tahoma", 10F);
        numSalePrice.Location = new Point(130, 65);
        numSalePrice.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 });
        numSalePrice.Name = "numSalePrice";
        numSalePrice.Size = new Size(250, 32);
        numSalePrice.TabIndex = 3;
        numSalePrice.ThousandsSeparator = true;
        // 
        // lblWholesalePrice
        // 
        lblWholesalePrice.AutoSize = true;
        lblWholesalePrice.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblWholesalePrice.Location = new Point(971, 105);
        lblWholesalePrice.Name = "lblWholesalePrice";
        lblWholesalePrice.Size = new Size(129, 24);
        lblWholesalePrice.TabIndex = 6;
        lblWholesalePrice.Text = "سعر الجملة:";
        // 
        // numWholesalePrice
        // 
        numWholesalePrice.DecimalPlaces = 2;
        numWholesalePrice.Font = new Font("Tahoma", 10F);
        numWholesalePrice.Location = new Point(910, 132);
        numWholesalePrice.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 });
        numWholesalePrice.Name = "numWholesalePrice";
        numWholesalePrice.Size = new Size(250, 32);
        numWholesalePrice.TabIndex = 4;
        numWholesalePrice.ThousandsSeparator = true;
        // 
        // lblCostPrice
        // 
        lblCostPrice.AutoSize = true;
        lblCostPrice.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblCostPrice.Location = new Point(709, 105);
        lblCostPrice.Name = "lblCostPrice";
        lblCostPrice.Size = new Size(132, 24);
        lblCostPrice.TabIndex = 7;
        lblCostPrice.Text = "سعر التكلفة:";
        // 
        // numCostPrice
        // 
        numCostPrice.DecimalPlaces = 2;
        numCostPrice.Font = new Font("Tahoma", 10F);
        numCostPrice.Location = new Point(650, 132);
        numCostPrice.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 });
        numCostPrice.Name = "numCostPrice";
        numCostPrice.Size = new Size(250, 32);
        numCostPrice.TabIndex = 5;
        numCostPrice.ThousandsSeparator = true;
        // 
        // lblMaxDiscount
        // 
        lblMaxDiscount.AutoSize = true;
        lblMaxDiscount.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblMaxDiscount.Location = new Point(449, 105);
        lblMaxDiscount.Name = "lblMaxDiscount";
        lblMaxDiscount.Size = new Size(132, 24);
        lblMaxDiscount.TabIndex = 8;
        lblMaxDiscount.Text = "أقصى خصم:";
        // 
        // numMaxDiscount
        // 
        numMaxDiscount.DecimalPlaces = 2;
        numMaxDiscount.Font = new Font("Tahoma", 10F);
        numMaxDiscount.Location = new Point(390, 132);
        numMaxDiscount.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 });
        numMaxDiscount.Name = "numMaxDiscount";
        numMaxDiscount.Size = new Size(250, 32);
        numMaxDiscount.TabIndex = 6;
        numMaxDiscount.ThousandsSeparator = true;
        // 
        // pnlButtons
        // 
        pnlButtons.BackColor = Color.FromArgb(235, 240, 235);
        pnlButtons.Controls.Add(btnNew);
        pnlButtons.Controls.Add(btnSave);
        pnlButtons.Controls.Add(btnEdit);
        pnlButtons.Controls.Add(btnDelete);
        pnlButtons.Controls.Add(btnRefresh);
        pnlButtons.Controls.Add(btnClose);
        pnlButtons.Dock = DockStyle.Top;
        pnlButtons.FlowDirection = FlowDirection.TopDown;
        pnlButtons.Location = new Point(0, 230);
        pnlButtons.Name = "pnlButtons";
        pnlButtons.Padding = new Padding(10);
        pnlButtons.Size = new Size(1200, 60);
        pnlButtons.TabIndex = 2;
        // 
        // btnNew
        // 
        btnNew.BackColor = Color.FromArgb(40, 167, 69);
        btnNew.FlatStyle = FlatStyle.Flat;
        btnNew.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnNew.ForeColor = Color.White;
        btnNew.Location = new Point(1057, 13);
        btnNew.Margin = new Padding(3, 3, 8, 3);
        btnNew.Name = "btnNew";
        btnNew.Size = new Size(120, 35);
        btnNew.TabIndex = 0;
        btnNew.Text = "🆕 جديد";
        btnNew.UseVisualStyleBackColor = false;
        btnNew.Click += btnNew_Click;
        // 
        // btnSave
        // 
        btnSave.BackColor = Color.FromArgb(0, 123, 255);
        btnSave.FlatStyle = FlatStyle.Flat;
        btnSave.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnSave.ForeColor = Color.White;
        btnSave.Location = new Point(918, 13);
        btnSave.Margin = new Padding(3, 3, 8, 3);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(128, 35);
        btnSave.TabIndex = 1;
        btnSave.Text = "💾 إضافة";
        btnSave.UseVisualStyleBackColor = false;
        btnSave.Click += btnSave_Click;
        // 
        // btnEdit
        // 
        btnEdit.BackColor = Color.FromArgb(255, 152, 0);
        btnEdit.FlatStyle = FlatStyle.Flat;
        btnEdit.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnEdit.ForeColor = Color.White;
        btnEdit.Location = new Point(787, 13);
        btnEdit.Margin = new Padding(3, 3, 8, 3);
        btnEdit.Name = "btnEdit";
        btnEdit.Size = new Size(120, 35);
        btnEdit.TabIndex = 2;
        btnEdit.Text = "✏️ تعديل";
        btnEdit.UseVisualStyleBackColor = false;
        btnEdit.Click += btnEdit_Click;
        // 
        // btnDelete
        // 
        btnDelete.BackColor = Color.FromArgb(220, 53, 69);
        btnDelete.FlatStyle = FlatStyle.Flat;
        btnDelete.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnDelete.ForeColor = Color.White;
        btnDelete.Location = new Point(656, 13);
        btnDelete.Margin = new Padding(3, 3, 8, 3);
        btnDelete.Name = "btnDelete";
        btnDelete.Size = new Size(120, 35);
        btnDelete.TabIndex = 3;
        btnDelete.Text = "🗑 حذف";
        btnDelete.UseVisualStyleBackColor = false;
        btnDelete.Click += btnDelete_Click;
        // 
        // btnRefresh
        // 
        btnRefresh.BackColor = Color.FromArgb(23, 162, 184);
        btnRefresh.FlatStyle = FlatStyle.Flat;
        btnRefresh.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnRefresh.ForeColor = Color.White;
        btnRefresh.Location = new Point(515, 13);
        btnRefresh.Margin = new Padding(3, 3, 8, 3);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(130, 35);
        btnRefresh.TabIndex = 4;
        btnRefresh.Text = "🔄 تحديث";
        btnRefresh.UseVisualStyleBackColor = false;
        btnRefresh.Click += btnRefresh_Click;
        // 
        // btnClose
        // 
        btnClose.BackColor = Color.FromArgb(108, 117, 125);
        btnClose.FlatStyle = FlatStyle.Flat;
        btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnClose.ForeColor = Color.White;
        btnClose.Location = new Point(384, 13);
        btnClose.Margin = new Padding(3, 3, 8, 3);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(120, 35);
        btnClose.TabIndex = 5;
        btnClose.TabStop = false;
        btnClose.Text = "🚪 خروج";
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += btnClose_Click;
        // 
        // pnlGrid
        // 
        pnlGrid.Controls.Add(dgvUnits);
        pnlGrid.Controls.Add(pnlFilter);
        pnlGrid.Dock = DockStyle.Fill;
        pnlGrid.Location = new Point(0, 290);
        pnlGrid.Name = "pnlGrid";
        pnlGrid.Padding = new Padding(0, 10, 0, 0);
        pnlGrid.Size = new Size(1200, 410);
        pnlGrid.TabIndex = 3;
        // 
        // dgvUnits
        // 
        dgvUnits.AllowUserToAddRows = false;
        dgvUnits.AllowUserToDeleteRows = false;
        dgvUnits.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvUnits.BackgroundColor = Color.FromArgb(220, 240, 220);
        dgvUnits.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvUnits.Dock = DockStyle.Fill;
        dgvUnits.Font = new Font("Tahoma", 10F);
        dgvUnits.Location = new Point(0, 78);
        dgvUnits.MultiSelect = false;
        dgvUnits.Name = "dgvUnits";
        dgvUnits.ReadOnly = true;
        dgvUnits.RowHeadersVisible = false;
        dgvUnits.RowHeadersWidth = 62;
        dgvUnits.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvUnits.Size = new Size(1200, 332);
        dgvUnits.TabIndex = 0;
        dgvUnits.CellClick += dgvUnits_CellClick;
        dgvUnits.SelectionChanged += dgvUnits_SelectionChanged;
        // 
        // pnlFilter
        // 
        pnlFilter.Controls.Add(groupBox1);
        pnlFilter.Dock = DockStyle.Top;
        pnlFilter.Location = new Point(0, 10);
        pnlFilter.Name = "pnlFilter";
        pnlFilter.Size = new Size(1200, 68);
        pnlFilter.TabIndex = 1;
        // 
        // groupBox1
        // 
        groupBox1.Controls.Add(lblSearch);
        groupBox1.Controls.Add(txtSearch);
        groupBox1.Font = new Font("Tahoma", 10F);
        groupBox1.Location = new Point(570, 3);
        groupBox1.Name = "groupBox1";
        groupBox1.Size = new Size(620, 62);
        groupBox1.TabIndex = 0;
        groupBox1.TabStop = false;
        groupBox1.Text = "محرك البحث";
        // 
        // lblSearch
        // 
        lblSearch.AutoSize = true;
        lblSearch.Location = new Point(520, 32);
        lblSearch.Name = "lblSearch";
        lblSearch.Size = new Size(56, 24);
        lblSearch.TabIndex = 1;
        lblSearch.Text = "بحث:";
        // 
        // txtSearch
        // 
        txtSearch.Location = new Point(160, 24);
        txtSearch.Name = "txtSearch";
        txtSearch.Size = new Size(340, 32);
        txtSearch.TabIndex = 0;
        txtSearch.TextChanged += txtSearch_TextChanged;
        txtSearch.KeyPress += txtSearch_KeyPress;
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
        Load += frmProductUnitMaster_Load;
        pnlFields.ResumeLayout(false);
        pnlFields.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numFactor).EndInit();
        ((System.ComponentModel.ISupportInitialize)numSalePrice).EndInit();
        ((System.ComponentModel.ISupportInitialize)numWholesalePrice).EndInit();
        ((System.ComponentModel.ISupportInitialize)numCostPrice).EndInit();
        ((System.ComponentModel.ISupportInitialize)numMaxDiscount).EndInit();
        pnlButtons.ResumeLayout(false);
        pnlGrid.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvUnits).EndInit();
        pnlFilter.ResumeLayout(false);
        groupBox1.ResumeLayout(false);
        groupBox1.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        ResumeLayout(false);
    }

    private System.Windows.Forms.Label lblMode;
    private System.Windows.Forms.Panel pnlFields;
    private System.Windows.Forms.Label lblProduct;
    private System.Windows.Forms.Label lblUnit;
    private System.Windows.Forms.Label lblUnitReq;
    private System.Windows.Forms.ComboBox cmbUnit;
    private System.Windows.Forms.Label lblBarcode;
    private System.Windows.Forms.TextBox txtBarcode;
    private System.Windows.Forms.Label lblFactor;
    private System.Windows.Forms.NumericUpDown numFactor;
    private System.Windows.Forms.Label lblSalePrice;
    private System.Windows.Forms.NumericUpDown numSalePrice;
    private System.Windows.Forms.Label lblWholesalePrice;
    private System.Windows.Forms.NumericUpDown numWholesalePrice;
    private System.Windows.Forms.Label lblCostPrice;
    private System.Windows.Forms.NumericUpDown numCostPrice;
    private System.Windows.Forms.Label lblMaxDiscount;
    private System.Windows.Forms.NumericUpDown numMaxDiscount;
    private System.Windows.Forms.FlowLayoutPanel pnlButtons;
    private System.Windows.Forms.Button btnNew;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.Button btnEdit;
    private System.Windows.Forms.Button btnDelete;
    private System.Windows.Forms.Button btnRefresh;
    private System.Windows.Forms.Button btnClose;
    private System.Windows.Forms.Panel pnlGrid;
    private System.Windows.Forms.DataGridView dgvUnits;
    private System.Windows.Forms.Panel pnlFilter;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.Label lblSearch;
    private System.Windows.Forms.TextBox txtSearch;
    private System.Windows.Forms.ErrorProvider errorProvider;
}
