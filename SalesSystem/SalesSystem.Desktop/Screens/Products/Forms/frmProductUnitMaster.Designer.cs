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
        pnlUnitFields = new FlowLayoutPanel();
        lblUnit = new Label();
        cmbUnit = new ComboBox();
        lblFactor = new Label();
        numFactor = new NumericUpDown();
        chkIsBaseUnit = new CheckBox();
        lblProduct = new Label();
        pnlButtons = new FlowLayoutPanel();
        btnNewUnit = new Button();
        btnSaveUnit = new Button();
        btnDeleteUnit = new Button();
        btnAddPrice = new Button();
        btnDeletePrice = new Button();
        btnClose = new Button();
        pnlGrid = new Panel();
        dgvUnits = new DataGridView();
        lblUnitsHeader = new Label();
        grpPrices = new GroupBox();
        dgvPrices = new DataGridView();
        pnlPriceFields = new FlowLayoutPanel();
        lblPrice = new Label();
        numPrice = new NumericUpDown();
        lblEffectiveFrom = new Label();
        dtpEffectiveFrom = new DateTimePicker();
        lblEffectiveTo = new Label();
        dtpEffectiveTo = new DateTimePicker();
        lblDevColumns = new Label();
        errorProvider = new ErrorProvider(components);
        pnlFields.SuspendLayout();
        pnlUnitFields.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numFactor).BeginInit();
        pnlButtons.SuspendLayout();
        pnlGrid.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvUnits).BeginInit();
        grpPrices.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvPrices).BeginInit();
        pnlPriceFields.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numPrice).BeginInit();
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
        lblMode.Text = "(( وحدات وأسعار الصنف ))";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlFields
        // 
        pnlFields.BackColor = Color.FromArgb(235, 245, 235);
        pnlFields.Controls.Add(pnlUnitFields);
        pnlFields.Controls.Add(lblProduct);
        pnlFields.Dock = DockStyle.Top;
        pnlFields.Location = new Point(0, 50);
        pnlFields.Name = "pnlFields";
        pnlFields.Size = new Size(1200, 110);
        pnlFields.TabIndex = 1;
        // 
        // pnlUnitFields
        // 
        pnlUnitFields.Controls.Add(lblUnit);
        pnlUnitFields.Controls.Add(cmbUnit);
        pnlUnitFields.Controls.Add(lblFactor);
        pnlUnitFields.Controls.Add(numFactor);
        pnlUnitFields.Controls.Add(chkIsBaseUnit);
        pnlUnitFields.Dock = DockStyle.Top;
        pnlUnitFields.Location = new Point(0, 32);
        pnlUnitFields.Name = "pnlUnitFields";
        pnlUnitFields.Padding = new Padding(10, 6, 10, 6);
        pnlUnitFields.Size = new Size(1200, 70);
        pnlUnitFields.TabIndex = 1;
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
        // cmbUnit
        // 
        cmbUnit.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbUnit.Font = new Font("Tahoma", 10F);
        cmbUnit.FormattingEnabled = true;
        cmbUnit.Margin = new Padding(3, 6, 20, 0);
        cmbUnit.Name = "cmbUnit";
        cmbUnit.Size = new Size(220, 32);
        cmbUnit.TabIndex = 1;
        // 
        // lblFactor
        // 
        lblFactor.AutoSize = true;
        lblFactor.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblFactor.Margin = new Padding(10, 12, 3, 0);
        lblFactor.Name = "lblFactor";
        lblFactor.Size = new Size(140, 24);
        lblFactor.TabIndex = 2;
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
        numFactor.Size = new Size(120, 32);
        numFactor.TabIndex = 3;
        numFactor.Value = new decimal(new int[] { 1, 0, 0, 0 });
        // 
        // chkIsBaseUnit
        // 
        chkIsBaseUnit.AutoSize = true;
        chkIsBaseUnit.Font = new Font("Tahoma", 10F);
        chkIsBaseUnit.Margin = new Padding(10, 9, 15, 0);
        chkIsBaseUnit.Name = "chkIsBaseUnit";
        chkIsBaseUnit.Size = new Size(170, 28);
        chkIsBaseUnit.TabIndex = 4;
        chkIsBaseUnit.Text = "الوحدة الأساسية";
        // 
        // lblProduct
        // 
        lblProduct.AutoSize = true;
        lblProduct.Dock = DockStyle.Top;
        lblProduct.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblProduct.ForeColor = Color.FromArgb(30, 86, 160);
        lblProduct.Location = new Point(0, 0);
        lblProduct.Name = "lblProduct";
        lblProduct.Padding = new Padding(10, 6, 0, 0);
        lblProduct.Size = new Size(1200, 32);
        lblProduct.TabIndex = 0;
        lblProduct.Text = "الصنف:";
        // 
        // pnlButtons
        // 
        pnlButtons.BackColor = Color.FromArgb(235, 240, 235);
        pnlButtons.Controls.Add(btnNewUnit);
        pnlButtons.Controls.Add(btnSaveUnit);
        pnlButtons.Controls.Add(btnDeleteUnit);
        pnlButtons.Controls.Add(btnAddPrice);
        pnlButtons.Controls.Add(btnDeletePrice);
        pnlButtons.Controls.Add(btnClose);
        pnlButtons.Dock = DockStyle.Top;
        pnlButtons.Location = new Point(0, 160);
        pnlButtons.Name = "pnlButtons";
        pnlButtons.Padding = new Padding(10);
        pnlButtons.Size = new Size(1200, 60);
        pnlButtons.TabIndex = 2;
        // 
        // btnNewUnit
        // 
        btnNewUnit.BackColor = Color.White;
        btnNewUnit.FlatStyle = FlatStyle.Flat;
        btnNewUnit.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnNewUnit.Margin = new Padding(3, 3, 8, 3);
        btnNewUnit.Name = "btnNewUnit";
        btnNewUnit.Size = new Size(160, 36);
        btnNewUnit.TabIndex = 0;
        btnNewUnit.Text = "➕ وحدة جديدة";
        btnNewUnit.UseVisualStyleBackColor = false;
        // 
        // btnSaveUnit
        // 
        btnSaveUnit.BackColor = Color.White;
        btnSaveUnit.FlatStyle = FlatStyle.Flat;
        btnSaveUnit.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnSaveUnit.Margin = new Padding(3, 3, 8, 3);
        btnSaveUnit.Name = "btnSaveUnit";
        btnSaveUnit.Size = new Size(160, 36);
        btnSaveUnit.TabIndex = 1;
        btnSaveUnit.Text = "💾 حفظ الوحدة";
        btnSaveUnit.UseVisualStyleBackColor = false;
        // 
        // btnDeleteUnit
        // 
        btnDeleteUnit.BackColor = Color.White;
        btnDeleteUnit.FlatStyle = FlatStyle.Flat;
        btnDeleteUnit.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnDeleteUnit.Margin = new Padding(3, 3, 24, 3);
        btnDeleteUnit.Name = "btnDeleteUnit";
        btnDeleteUnit.Size = new Size(160, 36);
        btnDeleteUnit.TabIndex = 2;
        btnDeleteUnit.Text = "🗑 حذف الوحدة";
        btnDeleteUnit.UseVisualStyleBackColor = false;
        // 
        // btnAddPrice
        // 
        btnAddPrice.BackColor = Color.White;
        btnAddPrice.FlatStyle = FlatStyle.Flat;
        btnAddPrice.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnAddPrice.Margin = new Padding(3, 3, 8, 3);
        btnAddPrice.Name = "btnAddPrice";
        btnAddPrice.Size = new Size(160, 36);
        btnAddPrice.TabIndex = 3;
        btnAddPrice.Text = "➕ إضافة سعر";
        btnAddPrice.UseVisualStyleBackColor = false;
        // 
        // btnDeletePrice
        // 
        btnDeletePrice.BackColor = Color.White;
        btnDeletePrice.FlatStyle = FlatStyle.Flat;
        btnDeletePrice.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnDeletePrice.Margin = new Padding(3, 3, 24, 3);
        btnDeletePrice.Name = "btnDeletePrice";
        btnDeletePrice.Size = new Size(160, 36);
        btnDeletePrice.TabIndex = 4;
        btnDeletePrice.Text = "🗑 حذف السعر";
        btnDeletePrice.UseVisualStyleBackColor = false;
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
        btnClose.TabIndex = 5;
        btnClose.Text = "إغلاق";
        btnClose.UseVisualStyleBackColor = false;
        // 
        // pnlGrid
        // 
        pnlGrid.Controls.Add(dgvUnits);
        pnlGrid.Controls.Add(lblUnitsHeader);
        pnlGrid.Controls.Add(grpPrices);
        pnlGrid.Controls.Add(lblDevColumns);
        pnlGrid.Dock = DockStyle.Fill;
        pnlGrid.Location = new Point(0, 220);
        pnlGrid.Name = "pnlGrid";
        pnlGrid.Padding = new Padding(10);
        pnlGrid.Size = new Size(1200, 560);
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
        dgvUnits.Dock = DockStyle.Fill;
        dgvUnits.Font = new Font("Tahoma", 10F);
        dgvUnits.Location = new Point(10, 36);
        dgvUnits.MultiSelect = false;
        dgvUnits.Name = "dgvUnits";
        dgvUnits.ReadOnly = true;
        dgvUnits.RowHeadersVisible = false;
        dgvUnits.RowHeadersWidth = 62;
        dgvUnits.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvUnits.Size = new Size(1180, 154);
        dgvUnits.TabIndex = 0;
        // 
        // lblUnitsHeader
        // 
        lblUnitsHeader.Dock = DockStyle.Top;
        lblUnitsHeader.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblUnitsHeader.ForeColor = Color.FromArgb(0, 90, 50);
        lblUnitsHeader.Location = new Point(10, 10);
        lblUnitsHeader.Name = "lblUnitsHeader";
        lblUnitsHeader.Size = new Size(1180, 26);
        lblUnitsHeader.TabIndex = 1;
        lblUnitsHeader.Text = "وحدات الصنف:";
        // 
        // grpPrices
        // 
        grpPrices.BackColor = Color.FromArgb(200, 230, 200);
        grpPrices.Controls.Add(dgvPrices);
        grpPrices.Controls.Add(pnlPriceFields);
        grpPrices.Dock = DockStyle.Bottom;
        grpPrices.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        grpPrices.Location = new Point(10, 190);
        grpPrices.Name = "grpPrices";
        grpPrices.Size = new Size(1180, 320);
        grpPrices.TabIndex = 2;
        grpPrices.TabStop = false;
        grpPrices.Text = "أسعار الوحدة المحددة";
        // 
        // dgvPrices
        // 
        dgvPrices.AllowUserToAddRows = false;
        dgvPrices.AllowUserToDeleteRows = false;
        dgvPrices.AutoGenerateColumns = false;
        dgvPrices.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvPrices.BackgroundColor = Color.FromArgb(220, 240, 220);
        dgvPrices.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvPrices.Dock = DockStyle.Fill;
        dgvPrices.Font = new Font("Tahoma", 10F);
        dgvPrices.Location = new Point(3, 76);
        dgvPrices.MultiSelect = false;
        dgvPrices.Name = "dgvPrices";
        dgvPrices.ReadOnly = true;
        dgvPrices.RowHeadersVisible = false;
        dgvPrices.RowHeadersWidth = 62;
        dgvPrices.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvPrices.Size = new Size(1174, 241);
        dgvPrices.TabIndex = 0;
        // 
        // pnlPriceFields
        // 
        pnlPriceFields.Controls.Add(lblPrice);
        pnlPriceFields.Controls.Add(numPrice);
        pnlPriceFields.Controls.Add(lblEffectiveFrom);
        pnlPriceFields.Controls.Add(dtpEffectiveFrom);
        pnlPriceFields.Controls.Add(lblEffectiveTo);
        pnlPriceFields.Controls.Add(dtpEffectiveTo);
        pnlPriceFields.Dock = DockStyle.Top;
        pnlPriceFields.Font = new Font("Tahoma", 10F);
        pnlPriceFields.Location = new Point(3, 26);
        pnlPriceFields.Name = "pnlPriceFields";
        pnlPriceFields.Padding = new Padding(10, 6, 10, 6);
        pnlPriceFields.Size = new Size(1174, 50);
        pnlPriceFields.TabIndex = 1;
        // 
        // lblPrice
        // 
        lblPrice.AutoSize = true;
        lblPrice.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblPrice.Margin = new Padding(10, 8, 3, 0);
        lblPrice.Name = "lblPrice";
        lblPrice.Size = new Size(62, 24);
        lblPrice.TabIndex = 0;
        lblPrice.Text = "السعر:";
        // 
        // numPrice
        // 
        numPrice.DecimalPlaces = 2;
        numPrice.Font = new Font("Tahoma", 10F);
        numPrice.Location = new Point(0, 0);
        numPrice.Margin = new Padding(3, 4, 20, 0);
        numPrice.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 });
        numPrice.Name = "numPrice";
        numPrice.Size = new Size(140, 32);
        numPrice.TabIndex = 1;
        // 
        // lblEffectiveFrom
        // 
        lblEffectiveFrom.AutoSize = true;
        lblEffectiveFrom.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblEffectiveFrom.Margin = new Padding(10, 8, 3, 0);
        lblEffectiveFrom.Name = "lblEffectiveFrom";
        lblEffectiveFrom.Size = new Size(78, 24);
        lblEffectiveFrom.TabIndex = 2;
        lblEffectiveFrom.Text = "ساري من:";
        // 
        // dtpEffectiveFrom
        // 
        dtpEffectiveFrom.Font = new Font("Tahoma", 10F);
        dtpEffectiveFrom.Format = DateTimePickerFormat.Short;
        dtpEffectiveFrom.Location = new Point(0, 0);
        dtpEffectiveFrom.Margin = new Padding(3, 4, 20, 0);
        dtpEffectiveFrom.Name = "dtpEffectiveFrom";
        dtpEffectiveFrom.Size = new Size(160, 32);
        dtpEffectiveFrom.TabIndex = 3;
        // 
        // lblEffectiveTo
        // 
        lblEffectiveTo.AutoSize = true;
        lblEffectiveTo.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblEffectiveTo.Margin = new Padding(10, 8, 3, 0);
        lblEffectiveTo.Name = "lblEffectiveTo";
        lblEffectiveTo.Size = new Size(86, 24);
        lblEffectiveTo.TabIndex = 4;
        lblEffectiveTo.Text = "ساري حتى:";
        // 
        // dtpEffectiveTo
        // 
        dtpEffectiveTo.Font = new Font("Tahoma", 10F);
        dtpEffectiveTo.Format = DateTimePickerFormat.Short;
        dtpEffectiveTo.Location = new Point(0, 0);
        dtpEffectiveTo.Margin = new Padding(3, 4, 20, 0);
        dtpEffectiveTo.Name = "dtpEffectiveTo";
        dtpEffectiveTo.ShowCheckBox = true;
        dtpEffectiveTo.Size = new Size(160, 32);
        dtpEffectiveTo.TabIndex = 5;
        // 
        // lblDevColumns
        // 
        lblDevColumns.Dock = DockStyle.Bottom;
        lblDevColumns.Font = new Font("Tahoma", 9F);
        lblDevColumns.ForeColor = Color.DimGray;
        lblDevColumns.Location = new Point(10, 510);
        lblDevColumns.Name = "lblDevColumns";
        lblDevColumns.Size = new Size(1180, 22);
        lblDevColumns.TabIndex = 3;
        lblDevColumns.Text = "الأعمدة:";
        lblDevColumns.Visible = false;
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
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.FromArgb(170, 220, 170);
        ClientSize = new Size(1200, 780);
        Controls.Add(pnlGrid);
        Controls.Add(pnlButtons);
        Controls.Add(pnlFields);
        Controls.Add(lblMode);
        Font = new Font("Tahoma", 10F);
        MinimumSize = new Size(1000, 680);
        Name = "frmProductUnitMaster";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterParent;
        Text = "وحدات وأسعار الصنف";
        pnlFields.ResumeLayout(false);
        pnlFields.PerformLayout();
        pnlUnitFields.ResumeLayout(false);
        pnlUnitFields.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numFactor).EndInit();
        pnlButtons.ResumeLayout(false);
        pnlGrid.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvUnits).EndInit();
        grpPrices.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvPrices).EndInit();
        pnlPriceFields.ResumeLayout(false);
        pnlPriceFields.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numPrice).EndInit();
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        ResumeLayout(false);
    }

    private System.Windows.Forms.Label lblMode;
    private System.Windows.Forms.Panel pnlFields;
    private System.Windows.Forms.Label lblProduct;
    private System.Windows.Forms.FlowLayoutPanel pnlUnitFields;
    private System.Windows.Forms.Label lblUnit;
    private System.Windows.Forms.ComboBox cmbUnit;
    private System.Windows.Forms.Label lblFactor;
    private System.Windows.Forms.NumericUpDown numFactor;
    private System.Windows.Forms.CheckBox chkIsBaseUnit;
    private System.Windows.Forms.FlowLayoutPanel pnlButtons;
    private System.Windows.Forms.Button btnNewUnit;
    private System.Windows.Forms.Button btnSaveUnit;
    private System.Windows.Forms.Button btnDeleteUnit;
    private System.Windows.Forms.Button btnAddPrice;
    private System.Windows.Forms.Button btnDeletePrice;
    private System.Windows.Forms.Button btnClose;
    private System.Windows.Forms.Panel pnlGrid;
    private System.Windows.Forms.DataGridView dgvUnits;
    private System.Windows.Forms.Label lblUnitsHeader;
    private System.Windows.Forms.GroupBox grpPrices;
    private System.Windows.Forms.DataGridView dgvPrices;
    private System.Windows.Forms.FlowLayoutPanel pnlPriceFields;
    private System.Windows.Forms.Label lblPrice;
    private System.Windows.Forms.NumericUpDown numPrice;
    private System.Windows.Forms.Label lblEffectiveFrom;
    private System.Windows.Forms.DateTimePicker dtpEffectiveFrom;
    private System.Windows.Forms.Label lblEffectiveTo;
    private System.Windows.Forms.DateTimePicker dtpEffectiveTo;
    private System.Windows.Forms.Label lblDevColumns;
    private System.Windows.Forms.ErrorProvider errorProvider;
}
