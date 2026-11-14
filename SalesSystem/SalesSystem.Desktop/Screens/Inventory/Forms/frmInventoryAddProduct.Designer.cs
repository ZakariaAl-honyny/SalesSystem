namespace SalesSystem.Desktop.Screens.Inventory.Forms;

partial class frmInventoryAddProduct
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
        errorProvider = new ErrorProvider(components);
        toolTip1 = new ToolTip(components);
        liveTip = new ToolTip(components);
        lblMode = new Label();
        lblName = new Label();
        txtName = new TextBox();
        lblBarcode = new Label();
        txtBarcode = new TextBox();
        lblCategory = new Label();
        cmbCategory = new ComboBox();
        lblReorderLevel = new Label();
        nudReorderLevel = new NumericUpDown();
        chkTrackExpiry = new CheckBox();
        lblUnitsHeader = new Label();
        dgvUnits = new DataGridView();
        colUnit = new DataGridViewComboBoxColumn();
        colFactor = new DataGridViewTextBoxColumn();
        colBarcode = new DataGridViewTextBoxColumn();
        colSellPrice = new DataGridViewTextBoxColumn();
        colCost = new DataGridViewTextBoxColumn();
        grpOpeningStock = new GroupBox();
        lblExpiry = new Label();
        dtpExpiry = new DateTimePicker();
        lblBatchNo = new Label();
        txtBatchNo = new TextBox();
        lblOpeningQty = new Label();
        nudOpeningQty = new NumericUpDown();
        lblWarehouse = new Label();
        cmbWarehouse = new ComboBox();
        lblNote = new Label();
        btnSave = new Button();
        btnCancel = new Button();
        cmsRows = new ContextMenuStrip(components);
        tsmiDeleteUnitRow = new ToolStripMenuItem();
        ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudReorderLevel).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvUnits).BeginInit();
        grpOpeningStock.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudOpeningQty).BeginInit();
        cmsRows.SuspendLayout();
        SuspendLayout();
        // 
        // errorProvider
        // 
        errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        errorProvider.ContainerControl = this;
        errorProvider.RightToLeft = true;
        // 
        // toolTip1
        // 
        toolTip1.AutomaticDelay = 300;
        toolTip1.BackColor = Color.White;
        toolTip1.ForeColor = Color.Black;
        // 
        // liveTip
        // 
        liveTip.AutomaticDelay = 300;
        liveTip.BackColor = Color.White;
        liveTip.ForeColor = Color.Black;
        // 
        // lblMode
        // 
        lblMode.BackColor = Color.FromArgb(14, 165, 103);
        lblMode.Dock = DockStyle.Top;
        lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold);
        lblMode.ForeColor = Color.White;
        lblMode.Location = new Point(0, 0);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(640, 39);
        lblMode.TabIndex = 0;
        lblMode.Text = "إضافة صنف";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblName
        // 
        lblName.AutoSize = true;
        lblName.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblName.ForeColor = Color.FromArgb(14, 82, 52);
        lblName.Location = new Point(20, 57);
        lblName.Name = "lblName";
        lblName.Size = new Size(108, 24);
        lblName.TabIndex = 1;
        lblName.Text = "اسم الصنف :";
        // 
        // txtName
        // 
        txtName.BackColor = Color.White;
        txtName.Font = new Font("Tahoma", 10F);
        txtName.Location = new Point(140, 54);
        txtName.Name = "txtName";
        txtName.Size = new Size(480, 30);
        txtName.TabIndex = 2;
        // 
        // lblBarcode
        // 
        lblBarcode.AutoSize = true;
        lblBarcode.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblBarcode.ForeColor = Color.FromArgb(14, 82, 52);
        lblBarcode.Location = new Point(20, 102);
        lblBarcode.Name = "lblBarcode";
        lblBarcode.Size = new Size(87, 24);
        lblBarcode.TabIndex = 3;
        lblBarcode.Text = "الباركود :";
        // 
        // txtBarcode
        // 
        txtBarcode.BackColor = Color.White;
        txtBarcode.Font = new Font("Tahoma", 10F);
        txtBarcode.Location = new Point(140, 99);
        txtBarcode.Name = "txtBarcode";
        txtBarcode.Size = new Size(480, 30);
        txtBarcode.TabIndex = 4;
        // 
        // lblCategory
        // 
        lblCategory.AutoSize = true;
        lblCategory.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblCategory.ForeColor = Color.FromArgb(14, 82, 52);
        lblCategory.Location = new Point(20, 150);
        lblCategory.Name = "lblCategory";
        lblCategory.Size = new Size(85, 24);
        lblCategory.TabIndex = 5;
        lblCategory.Text = "التصنيف :";
        // 
        // cmbCategory
        // 
        cmbCategory.BackColor = Color.White;
        cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbCategory.FlatStyle = FlatStyle.Flat;
        cmbCategory.Font = new Font("Tahoma", 10F);
        cmbCategory.FormattingEnabled = true;
        cmbCategory.Location = new Point(140, 147);
        cmbCategory.Name = "cmbCategory";
        cmbCategory.Size = new Size(200, 30);
        cmbCategory.TabIndex = 6;
        // 
        // lblReorderLevel
        // 
        lblReorderLevel.AutoSize = true;
        lblReorderLevel.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblReorderLevel.ForeColor = Color.FromArgb(14, 82, 52);
        lblReorderLevel.Location = new Point(390, 150);
        lblReorderLevel.Name = "lblReorderLevel";
        lblReorderLevel.Size = new Size(83, 24);
        lblReorderLevel.TabIndex = 7;
        lblReorderLevel.Text = "حد الطلب :";
        // 
        // nudReorderLevel
        // 
        nudReorderLevel.BackColor = Color.White;
        nudReorderLevel.DecimalPlaces = 0;
        nudReorderLevel.Font = new Font("Tahoma", 10F);
        nudReorderLevel.Location = new Point(490, 147);
        nudReorderLevel.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
        nudReorderLevel.Name = "nudReorderLevel";
        nudReorderLevel.Size = new Size(130, 30);
        nudReorderLevel.TabIndex = 8;
        nudReorderLevel.ThousandsSeparator = true;
        // 
        // chkTrackExpiry
        // 
        chkTrackExpiry.AutoSize = true;
        chkTrackExpiry.BackColor = Color.FromArgb(170, 220, 170);
        chkTrackExpiry.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        chkTrackExpiry.ForeColor = Color.FromArgb(14, 82, 52);
        chkTrackExpiry.Location = new Point(20, 192);
        chkTrackExpiry.Name = "chkTrackExpiry";
        chkTrackExpiry.Size = new Size(195, 28);
        chkTrackExpiry.TabIndex = 9;
        chkTrackExpiry.Text = "يتبع تاريخ صلاحية";
        chkTrackExpiry.UseVisualStyleBackColor = false;
        chkTrackExpiry.CheckedChanged += chkTrackExpiry_CheckedChanged;
        // 
        // lblUnitsHeader
        // 
        lblUnitsHeader.AutoSize = true;
        lblUnitsHeader.Font = new Font("Tahoma", 11F, FontStyle.Bold);
        lblUnitsHeader.ForeColor = Color.FromArgb(14, 82, 52);
        lblUnitsHeader.Location = new Point(20, 232);
        lblUnitsHeader.Name = "lblUnitsHeader";
        lblUnitsHeader.Size = new Size(176, 27);
        lblUnitsHeader.TabIndex = 10;
        lblUnitsHeader.Text = "الوحدات والأسعار";
        // 
        // dgvUnits
        // 
        // شبكة إدخال سطور (RULE-N22a): تُدخَل فيها الوحدات والأسعار مباشرةً داخل الخلية.
        dgvUnits.AllowUserToAddRows = true;
        dgvUnits.AllowUserToDeleteRows = true;
        dgvUnits.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 252, 245);
        dgvUnits.AutoGenerateColumns = false;
        dgvUnits.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvUnits.BackgroundColor = Color.FromArgb(220, 240, 220);
        dgvUnits.BorderStyle = BorderStyle.None;
        dgvUnits.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(14, 165, 103);
        dgvUnits.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        dgvUnits.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(14, 165, 103);
        dgvUnits.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
        dgvUnits.ColumnHeadersHeight = 40;
        dgvUnits.Columns.AddRange(new DataGridViewColumn[] { colUnit, colFactor, colBarcode, colSellPrice, colCost });
        dgvUnits.ContextMenuStrip = cmsRows;
        dgvUnits.DefaultCellStyle.BackColor = Color.White;
        dgvUnits.DefaultCellStyle.ForeColor = Color.FromArgb(33, 37, 41);
        dgvUnits.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 255, 180);
        dgvUnits.DefaultCellStyle.SelectionForeColor = Color.FromArgb(33, 37, 41);
        dgvUnits.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
        dgvUnits.EnableHeadersVisualStyles = false;
        dgvUnits.GridColor = Color.FromArgb(205, 225, 205);
        dgvUnits.Location = new Point(12, 266);
        dgvUnits.Name = "dgvUnits";
        dgvUnits.ReadOnly = false;
        dgvUnits.RowHeadersVisible = false;
        dgvUnits.RowTemplate.Height = 30;
        dgvUnits.SelectionMode = DataGridViewSelectionMode.CellSelect;
        dgvUnits.Size = new Size(616, 160);
        dgvUnits.TabIndex = 11;
        dgvUnits.EditingControlShowing += dgvUnits_EditingControlShowing;
        // 
        // colUnit
        // 
        colUnit.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
        colUnit.FillWeight = 22F;
        colUnit.FlatStyle = FlatStyle.Flat;
        colUnit.HeaderText = "الوحدة";
        colUnit.Items.AddRange(new object[] { "كيس", "كرتون", "عبوة", "قطعة" });
        colUnit.MinimumWidth = 80;
        colUnit.Name = "colUnit";
        colUnit.Resizable = DataGridViewTriState.True;
        colUnit.SortMode = DataGridViewColumnSortMode.Automatic;
        // 
        // colFactor
        // 
        colFactor.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        colFactor.DefaultCellStyle.Format = "N3";
        colFactor.FillWeight = 18F;
        colFactor.HeaderText = "معامل التحويل";
        colFactor.MinimumWidth = 80;
        colFactor.Name = "colFactor";
        // 
        // colBarcode
        // 
        colBarcode.FillWeight = 25F;
        colBarcode.HeaderText = "الباركود";
        colBarcode.MinimumWidth = 100;
        colBarcode.Name = "colBarcode";
        // 
        // colSellPrice
        // 
        colSellPrice.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        colSellPrice.DefaultCellStyle.Format = "N2";
        colSellPrice.FillWeight = 18F;
        colSellPrice.HeaderText = "سعر البيع";
        colSellPrice.MinimumWidth = 80;
        colSellPrice.Name = "colSellPrice";
        // 
        // colCost
        // 
        colCost.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        colCost.DefaultCellStyle.Format = "N2";
        colCost.FillWeight = 17F;
        colCost.HeaderText = "التكلفة";
        colCost.MinimumWidth = 80;
        colCost.Name = "colCost";
        // 
        // grpOpeningStock
        // 
        grpOpeningStock.BackColor = Color.FromArgb(235, 245, 235);
        grpOpeningStock.Controls.Add(lblExpiry);
        grpOpeningStock.Controls.Add(dtpExpiry);
        grpOpeningStock.Controls.Add(lblBatchNo);
        grpOpeningStock.Controls.Add(txtBatchNo);
        grpOpeningStock.Controls.Add(lblOpeningQty);
        grpOpeningStock.Controls.Add(nudOpeningQty);
        grpOpeningStock.Controls.Add(lblWarehouse);
        grpOpeningStock.Controls.Add(cmbWarehouse);
        grpOpeningStock.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        grpOpeningStock.ForeColor = Color.FromArgb(14, 82, 52);
        grpOpeningStock.Location = new Point(12, 440);
        grpOpeningStock.Name = "grpOpeningStock";
        grpOpeningStock.RightToLeft = RightToLeft.Yes;
        grpOpeningStock.Size = new Size(616, 150);
        grpOpeningStock.TabIndex = 12;
        grpOpeningStock.TabStop = false;
        grpOpeningStock.Text = "المخزون الافتتاحي";
        // 
        // lblWarehouse
        // 
        lblWarehouse.AutoSize = true;
        lblWarehouse.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblWarehouse.ForeColor = Color.FromArgb(14, 82, 52);
        lblWarehouse.Location = new Point(15, 25);
        lblWarehouse.Name = "lblWarehouse";
        lblWarehouse.Size = new Size(84, 24);
        lblWarehouse.TabIndex = 0;
        lblWarehouse.Text = "المستودع :";
        // 
        // cmbWarehouse
        // 
        cmbWarehouse.BackColor = Color.White;
        cmbWarehouse.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbWarehouse.FlatStyle = FlatStyle.Flat;
        cmbWarehouse.Font = new Font("Tahoma", 10F);
        cmbWarehouse.FormattingEnabled = true;
        cmbWarehouse.Location = new Point(130, 22);
        cmbWarehouse.Name = "cmbWarehouse";
        cmbWarehouse.Size = new Size(200, 30);
        cmbWarehouse.TabIndex = 1;
        // 
        // lblOpeningQty
        // 
        lblOpeningQty.AutoSize = true;
        lblOpeningQty.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblOpeningQty.ForeColor = Color.FromArgb(14, 82, 52);
        lblOpeningQty.Location = new Point(370, 25);
        lblOpeningQty.Name = "lblOpeningQty";
        lblOpeningQty.Size = new Size(121, 24);
        lblOpeningQty.TabIndex = 2;
        lblOpeningQty.Text = "الكمية الافتتاحية :";
        // 
        // nudOpeningQty
        // 
        nudOpeningQty.BackColor = Color.White;
        nudOpeningQty.DecimalPlaces = 3;
        nudOpeningQty.Font = new Font("Tahoma", 10F);
        nudOpeningQty.Location = new Point(500, 22);
        nudOpeningQty.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 });
        nudOpeningQty.Name = "nudOpeningQty";
        nudOpeningQty.Size = new Size(100, 30);
        nudOpeningQty.TabIndex = 3;
        nudOpeningQty.ThousandsSeparator = true;
        // 
        // lblBatchNo
        // 
        lblBatchNo.AutoSize = true;
        lblBatchNo.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblBatchNo.ForeColor = Color.FromArgb(14, 82, 52);
        lblBatchNo.Location = new Point(15, 80);
        lblBatchNo.Name = "lblBatchNo";
        lblBatchNo.Size = new Size(105, 24);
        lblBatchNo.TabIndex = 4;
        lblBatchNo.Text = "رقم التشغيلة :";
        // 
        // txtBatchNo
        // 
        txtBatchNo.BackColor = Color.White;
        txtBatchNo.Font = new Font("Tahoma", 10F);
        txtBatchNo.Location = new Point(130, 77);
        txtBatchNo.Name = "txtBatchNo";
        txtBatchNo.Size = new Size(200, 30);
        txtBatchNo.TabIndex = 5;
        // 
        // lblExpiry
        // 
        lblExpiry.AutoSize = true;
        lblExpiry.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblExpiry.ForeColor = Color.FromArgb(14, 82, 52);
        lblExpiry.Location = new Point(370, 80);
        lblExpiry.Name = "lblExpiry";
        lblExpiry.Size = new Size(107, 24);
        lblExpiry.TabIndex = 6;
        lblExpiry.Text = "تاريخ الصلاحية :";
        // 
        // dtpExpiry
        // 
        dtpExpiry.Enabled = false;
        dtpExpiry.Font = new Font("Tahoma", 10F);
        dtpExpiry.Format = DateTimePickerFormat.Short;
        dtpExpiry.Location = new Point(490, 77);
        dtpExpiry.Name = "dtpExpiry";
        dtpExpiry.Size = new Size(110, 30);
        dtpExpiry.TabIndex = 7;
        // 
        // lblNote
        // 
        lblNote.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        lblNote.Font = new Font("Tahoma", 10F, FontStyle.Italic);
        lblNote.ForeColor = Color.FromArgb(14, 82, 52);
        lblNote.Location = new Point(12, 600);
        lblNote.Name = "lblNote";
        lblNote.Size = new Size(616, 26);
        lblNote.TabIndex = 13;
        lblNote.Text = "تكلفة المخزون تُدار عبر طبقات FIFO/FEFO تلقائياً";
        lblNote.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // btnSave
        // 
        btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnSave.BackColor = Color.FromArgb(40, 167, 69);
        btnSave.Cursor = Cursors.Hand;
        btnSave.FlatAppearance.BorderSize = 0;
        btnSave.FlatStyle = FlatStyle.Flat;
        btnSave.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnSave.ForeColor = Color.White;
        btnSave.Location = new Point(390, 632);
        btnSave.MinimumSize = new Size(100, 35);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(130, 40);
        btnSave.TabIndex = 14;
        btnSave.Text = "حفظ";
        btnSave.UseVisualStyleBackColor = false;
        btnSave.Click += btnSave_Click;
        // 
        // btnCancel
        // 
        btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnCancel.BackColor = Color.FromArgb(108, 117, 125);
        btnCancel.Cursor = Cursors.Hand;
        btnCancel.DialogResult = DialogResult.Cancel;
        btnCancel.FlatAppearance.BorderSize = 0;
        btnCancel.FlatStyle = FlatStyle.Flat;
        btnCancel.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnCancel.ForeColor = Color.White;
        btnCancel.Location = new Point(250, 632);
        btnCancel.MinimumSize = new Size(100, 35);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(130, 40);
        btnCancel.TabIndex = 15;
        btnCancel.Text = "إلغاء";
        btnCancel.UseVisualStyleBackColor = false;
        btnCancel.Click += btnCancel_Click;
        // 
        // cmsRows — RULE-N23: قائمة سطر السياق لشبكة الوحدات (تُعلَن في المصمّم — RULE-N145)
        // 
        cmsRows.BackColor = Color.White;
        cmsRows.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        cmsRows.ImageScalingSize = new Size(24, 24);
        cmsRows.Items.AddRange(new ToolStripItem[] { tsmiDeleteUnitRow });
        cmsRows.Name = "cmsRows";
        cmsRows.RightToLeft = RightToLeft.Yes;
        cmsRows.Size = new Size(200, 34);
        cmsRows.Opening += cmsRows_Opening;
        // 
        // tsmiDeleteUnitRow
        // 
        tsmiDeleteUnitRow.Name = "tsmiDeleteUnitRow";
        tsmiDeleteUnitRow.Size = new Size(199, 30);
        tsmiDeleteUnitRow.Text = "حذف السطر";
        tsmiDeleteUnitRow.Click += tsmiDeleteUnitRow_Click;
        // 
        // frmInventoryAddProduct
        // 
        AcceptButton = btnSave;
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.FromArgb(170, 220, 170);
        CancelButton = btnCancel;
        ClientSize = new Size(640, 680);
        Controls.Add(btnCancel);
        Controls.Add(btnSave);
        Controls.Add(lblNote);
        Controls.Add(grpOpeningStock);
        Controls.Add(dgvUnits);
        Controls.Add(lblUnitsHeader);
        Controls.Add(chkTrackExpiry);
        Controls.Add(nudReorderLevel);
        Controls.Add(lblReorderLevel);
        Controls.Add(cmbCategory);
        Controls.Add(lblCategory);
        Controls.Add(txtBarcode);
        Controls.Add(lblBarcode);
        Controls.Add(txtName);
        Controls.Add(lblName);
        Controls.Add(lblMode);
        Font = new Font("Tahoma", 10F, FontStyle.Regular);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "frmInventoryAddProduct";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterParent;
        Text = "إضافة صنف";
        Load += frmInventoryAddProduct_Load;
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudReorderLevel).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvUnits).EndInit();
        grpOpeningStock.ResumeLayout(false);
        grpOpeningStock.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)nudOpeningQty).EndInit();
        cmsRows.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    private ErrorProvider errorProvider;
    private ToolTip toolTip1;
    private ToolTip liveTip;
    private Label lblMode;
    private Label lblName;
    private TextBox txtName;
    private Label lblBarcode;
    private TextBox txtBarcode;
    private Label lblCategory;
    private ComboBox cmbCategory;
    private Label lblReorderLevel;
    private NumericUpDown nudReorderLevel;
    private CheckBox chkTrackExpiry;
    private Label lblUnitsHeader;
    private DataGridView dgvUnits;
    private DataGridViewComboBoxColumn colUnit;
    private DataGridViewTextBoxColumn colFactor;
    private DataGridViewTextBoxColumn colBarcode;
    private DataGridViewTextBoxColumn colSellPrice;
    private DataGridViewTextBoxColumn colCost;
    private GroupBox grpOpeningStock;
    private Label lblWarehouse;
    private ComboBox cmbWarehouse;
    private Label lblOpeningQty;
    private NumericUpDown nudOpeningQty;
    private Label lblBatchNo;
    private TextBox txtBatchNo;
    private Label lblExpiry;
    private DateTimePicker dtpExpiry;
    private Label lblNote;
    private Button btnSave;
    private Button btnCancel;
    private ContextMenuStrip cmsRows;
    private ToolStripMenuItem tsmiDeleteUnitRow;
}
