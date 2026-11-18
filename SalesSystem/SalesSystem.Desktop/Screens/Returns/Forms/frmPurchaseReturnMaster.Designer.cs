namespace SalesSystem.Desktop.Screens.Returns.Forms;

partial class frmPurchaseReturnMaster
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
        cmsRows = new ContextMenuStrip(components);
        cmsView = new ToolStripMenuItem();
        cmsRefresh = new ToolStripMenuItem();
        errorProvider = new ErrorProvider(components);
        toolTip = new ToolTip(components);
        DataGridViewCellStyle styleMainHeader = new DataGridViewCellStyle();
        DataGridViewCellStyle styleMainCell = new DataGridViewCellStyle();
        DataGridViewCellStyle styleMainAlt = new DataGridViewCellStyle();
        DataGridViewCellStyle styleLineHeader = new DataGridViewCellStyle();
        DataGridViewCellStyle styleLineCell = new DataGridViewCellStyle();
        DataGridViewCellStyle styleLineAlt = new DataGridViewCellStyle();
        lblMode = new Label();
        pnlFields = new Panel();
        tblHeader = new TableLayoutPanel();
        lblInvoiceNoCaption = new Label();
        lblInvoiceNo = new Label();
        lblDateCaption = new Label();
        dtpDate = new DateTimePicker();
        lblStatusCaption = new Label();
        lblStatus = new Label();
        lblSupplierCaption = new Label();
        cmbSupplier = new ComboBox();
        lblWarehouseCaption = new Label();
        cmbWarehouse = new ComboBox();
        lblNotesCaption = new Label();
        txtNotes = new TextBox();
        pnlLines = new Panel();
        gbLines = new GroupBox();
        dgvItems = new DataGridView();
        colCode = new DataGridViewTextBoxColumn();
        colName = new DataGridViewTextBoxColumn();
        colQuantity = new DataGridViewTextBoxColumn();
        colUnitPrice = new DataGridViewTextBoxColumn();
        colLineTotal = new DataGridViewTextBoxColumn();
        flpLineBar = new FlowLayoutPanel();
        btnAddItem = new Button();
        btnRemoveItem = new Button();
        lblTotals = new Label();
        pnlButtons = new FlowLayoutPanel();
        btnNew = new Button();
        btnSaveDraft = new Button();
        btnPost = new Button();
        btnCancelInvoice = new Button();
        btnRefresh = new Button();
        btnClose = new Button();
        pnlFilters = new Panel();
        gbFilter = new GroupBox();
        flpFilterRow2 = new FlowLayoutPanel();
        btnRangeToday = new Button();
        btnRangeMonth = new Button();
        btnRange30 = new Button();
        chkOnlyCancelled = new CheckBox();
        btnClearFilters = new Button();
        flpFilterRow1 = new FlowLayoutPanel();
        labelFilterBy = new Label();
        cbFilterBy = new ComboBox();
        txtFilterValue = new TextBox();
        labelStatus = new Label();
        cbStatus = new ComboBox();
        labelFrom = new Label();
        dtpFrom = new DateTimePicker();
        labelTo = new Label();
        dtpTo = new DateTimePicker();
        btnToggleFilters = new Button();
        pnlGrid = new Panel();
        lblNoData = new Label();
        dgvMain = new DataGridView();
        colReturnNo = new DataGridViewTextBoxColumn();
        colReturnDate = new DataGridViewTextBoxColumn();
        colSupplier = new DataGridViewTextBoxColumn();
        colTotal = new DataGridViewTextBoxColumn();
        colStatus = new DataGridViewTextBoxColumn();
        pnlSummary = new Panel();
        lblSummary = new Label();
        ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
        pnlFields.SuspendLayout();
        tblHeader.SuspendLayout();
        pnlLines.SuspendLayout();
        gbLines.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvItems).BeginInit();
        flpLineBar.SuspendLayout();
        pnlButtons.SuspendLayout();
        pnlFilters.SuspendLayout();
        gbFilter.SuspendLayout();
        flpFilterRow2.SuspendLayout();
        flpFilterRow1.SuspendLayout();
        pnlGrid.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvMain).BeginInit();
        pnlSummary.SuspendLayout();
        SuspendLayout();
        // 
        // lblMode
        // 
        lblMode.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
        lblMode.Dock = DockStyle.Top;
        lblMode.Font = new Font("Tahoma", 14F, FontStyle.Bold);
        lblMode.ForeColor = System.Drawing.Color.White;
        lblMode.Location = new Point(0, 0);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(1432, 50);
        lblMode.TabIndex = 0;
        lblMode.Text = "(( مرتجع مشتريات ))";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlFields
        // 
        pnlFields.Controls.Add(tblHeader);
        pnlFields.Dock = DockStyle.Top;
        pnlFields.Location = new Point(0, 50);
        pnlFields.Name = "pnlFields";
        pnlFields.Size = new Size(1432, 104);
        pnlFields.TabIndex = 1;
        // 
        // tblHeader
        // 
        tblHeader.ColumnCount = 6;
        tblHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
        tblHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34F));
        tblHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90F));
        tblHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33F));
        tblHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90F));
        tblHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33F));
        tblHeader.Controls.Add(lblInvoiceNoCaption, 0, 0);
        tblHeader.Controls.Add(lblInvoiceNo, 1, 0);
        tblHeader.Controls.Add(lblDateCaption, 2, 0);
        tblHeader.Controls.Add(dtpDate, 3, 0);
        tblHeader.Controls.Add(lblStatusCaption, 4, 0);
        tblHeader.Controls.Add(lblStatus, 5, 0);
        tblHeader.Controls.Add(lblSupplierCaption, 0, 1);
        tblHeader.Controls.Add(cmbSupplier, 1, 1);
        tblHeader.Controls.Add(lblWarehouseCaption, 2, 1);
        tblHeader.Controls.Add(cmbWarehouse, 3, 1);
        tblHeader.Controls.Add(lblNotesCaption, 4, 1);
        tblHeader.Controls.Add(txtNotes, 5, 1);
        tblHeader.Dock = DockStyle.Fill;
        tblHeader.Location = new Point(0, 0);
        tblHeader.Name = "tblHeader";
        tblHeader.RowCount = 2;
        tblHeader.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        tblHeader.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        tblHeader.Size = new Size(1432, 104);
        tblHeader.TabIndex = 0;
        // 
        // lblInvoiceNoCaption
        // 
        lblInvoiceNoCaption.Dock = DockStyle.Fill;
        lblInvoiceNoCaption.Font = new Font("Tahoma", 10F);
        lblInvoiceNoCaption.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblInvoiceNoCaption.Location = new Point(1312, 0);
        lblInvoiceNoCaption.Name = "lblInvoiceNoCaption";
        lblInvoiceNoCaption.Size = new Size(114, 42);
        lblInvoiceNoCaption.TabIndex = 0;
        lblInvoiceNoCaption.Text = "رقم المرتجع:";
        lblInvoiceNoCaption.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblInvoiceNo
        // 
        lblInvoiceNo.BackColor = System.Drawing.Color.White;
        lblInvoiceNo.Dock = DockStyle.Fill;
        lblInvoiceNo.Font = new Font("Tahoma", 10F);
        lblInvoiceNo.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblInvoiceNo.Location = new Point(896, 0);
        lblInvoiceNo.Margin = new Padding(4, 6, 4, 6);
        lblInvoiceNo.Name = "lblInvoiceNo";
        lblInvoiceNo.Padding = new Padding(6, 0, 6, 0);
        lblInvoiceNo.Size = new Size(402, 30);
        lblInvoiceNo.TabIndex = 1;
        lblInvoiceNo.Text = "جديد";
        lblInvoiceNo.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblDateCaption
        // 
        lblDateCaption.Dock = DockStyle.Fill;
        lblDateCaption.Font = new Font("Tahoma", 10F);
        lblDateCaption.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblDateCaption.Location = new Point(798, 0);
        lblDateCaption.Name = "lblDateCaption";
        lblDateCaption.Size = new Size(84, 42);
        lblDateCaption.TabIndex = 2;
        lblDateCaption.Text = "التاريخ:";
        lblDateCaption.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // dtpDate
        // 
        dtpDate.BackColor = System.Drawing.Color.White;
        dtpDate.Dock = DockStyle.Fill;
        dtpDate.Font = new Font("Tahoma", 10F);
        dtpDate.Format = DateTimePickerFormat.Short;
        dtpDate.Location = new Point(406, 6);
        dtpDate.Margin = new Padding(4, 6, 4, 6);
        dtpDate.Name = "dtpDate";
        dtpDate.Size = new Size(382, 32);
        dtpDate.TabIndex = 3;
        // 
        // lblStatusCaption
        // 
        lblStatusCaption.Dock = DockStyle.Fill;
        lblStatusCaption.Font = new Font("Tahoma", 10F);
        lblStatusCaption.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblStatusCaption.Location = new Point(308, 0);
        lblStatusCaption.Name = "lblStatusCaption";
        lblStatusCaption.Size = new Size(84, 42);
        lblStatusCaption.TabIndex = 4;
        lblStatusCaption.Text = "الحالة:";
        lblStatusCaption.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblStatus
        // 
        lblStatus.Dock = DockStyle.Fill;
        lblStatus.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblStatus.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
        lblStatus.Location = new Point(3, 0);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(294, 42);
        lblStatus.TabIndex = 5;
        lblStatus.Text = "جديد";
        lblStatus.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblSupplierCaption
        // 
        lblSupplierCaption.Dock = DockStyle.Fill;
        lblSupplierCaption.Font = new Font("Tahoma", 10F);
        lblSupplierCaption.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblSupplierCaption.Location = new Point(1312, 42);
        lblSupplierCaption.Name = "lblSupplierCaption";
        lblSupplierCaption.Size = new Size(114, 62);
        lblSupplierCaption.TabIndex = 6;
        lblSupplierCaption.Text = "المورد *:";
        lblSupplierCaption.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // cmbSupplier
        // 
        cmbSupplier.BackColor = System.Drawing.Color.White;
        cmbSupplier.Dock = DockStyle.Fill;
        cmbSupplier.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbSupplier.FlatStyle = FlatStyle.Flat;
        cmbSupplier.Font = new Font("Tahoma", 10F);
        cmbSupplier.Location = new Point(896, 48);
        cmbSupplier.Margin = new Padding(4, 6, 4, 6);
        cmbSupplier.Name = "cmbSupplier";
        cmbSupplier.Size = new Size(402, 32);
        cmbSupplier.TabIndex = 7;
        // 
        // lblWarehouseCaption
        // 
        lblWarehouseCaption.Dock = DockStyle.Fill;
        lblWarehouseCaption.Font = new Font("Tahoma", 10F);
        lblWarehouseCaption.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblWarehouseCaption.Location = new Point(798, 42);
        lblWarehouseCaption.Name = "lblWarehouseCaption";
        lblWarehouseCaption.Size = new Size(84, 62);
        lblWarehouseCaption.TabIndex = 8;
        lblWarehouseCaption.Text = "المستودع *:";
        lblWarehouseCaption.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // cmbWarehouse
        // 
        cmbWarehouse.BackColor = System.Drawing.Color.White;
        cmbWarehouse.Dock = DockStyle.Fill;
        cmbWarehouse.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbWarehouse.FlatStyle = FlatStyle.Flat;
        cmbWarehouse.Font = new Font("Tahoma", 10F);
        cmbWarehouse.Location = new Point(406, 48);
        cmbWarehouse.Margin = new Padding(4, 6, 4, 6);
        cmbWarehouse.Name = "cmbWarehouse";
        cmbWarehouse.Size = new Size(382, 32);
        cmbWarehouse.TabIndex = 9;
        // 
        // lblNotesCaption
        // 
        lblNotesCaption.Dock = DockStyle.Fill;
        lblNotesCaption.Font = new Font("Tahoma", 10F);
        lblNotesCaption.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblNotesCaption.Location = new Point(308, 42);
        lblNotesCaption.Name = "lblNotesCaption";
        lblNotesCaption.Size = new Size(84, 62);
        lblNotesCaption.TabIndex = 10;
        lblNotesCaption.Text = "ملاحظات:";
        lblNotesCaption.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtNotes
        // 
        txtNotes.BackColor = System.Drawing.Color.White;
        txtNotes.Dock = DockStyle.Fill;
        txtNotes.Font = new Font("Tahoma", 10F);
        txtNotes.Location = new Point(3, 48);
        txtNotes.Margin = new Padding(4, 6, 4, 6);
        txtNotes.Name = "txtNotes";
        txtNotes.PlaceholderText = "ملاحظات الإرجاع...";
        txtNotes.Size = new Size(294, 32);
        txtNotes.TabIndex = 11;
        // 
        // pnlLines
        // 
        pnlLines.Controls.Add(gbLines);
        pnlLines.Dock = DockStyle.Top;
        pnlLines.Location = new Point(0, 154);
        pnlLines.Name = "pnlLines";
        pnlLines.Padding = new Padding(8, 2, 8, 6);
        pnlLines.Size = new Size(1432, 280);
        pnlLines.TabIndex = 2;
        // 
        // gbLines
        // 
        gbLines.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(245), Convert.ToInt32(235));
        gbLines.Controls.Add(dgvItems);
        gbLines.Controls.Add(flpLineBar);
        gbLines.Dock = DockStyle.Fill;
        gbLines.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        gbLines.Location = new Point(8, 2);
        gbLines.Name = "gbLines";
        gbLines.Padding = new Padding(6, 4, 6, 6);
        gbLines.Size = new Size(1416, 272);
        gbLines.TabIndex = 0;
        gbLines.TabStop = false;
        gbLines.Text = "أصناف المرتجع";
        // 
        // dgvItems
        // 
        dgvItems.AllowUserToAddRows = true;
        dgvItems.AllowUserToDeleteRows = false;
        dgvItems.AllowUserToResizeRows = false;
        styleLineAlt.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(245), Convert.ToInt32(252), Convert.ToInt32(245));
        dgvItems.AlternatingRowsDefaultCellStyle = styleLineAlt;
        dgvItems.AutoGenerateColumns = false;
        dgvItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvItems.BackgroundColor = System.Drawing.Color.FromArgb(Convert.ToInt32(220), Convert.ToInt32(240), Convert.ToInt32(220));
        dgvItems.BorderStyle = BorderStyle.None;
        dgvItems.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
        styleLineHeader.Alignment = DataGridViewContentAlignment.MiddleCenter;
        styleLineHeader.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
        styleLineHeader.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        styleLineHeader.ForeColor = System.Drawing.Color.White;
        styleLineHeader.SelectionBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
        styleLineHeader.SelectionForeColor = System.Drawing.Color.White;
        styleLineHeader.WrapMode = DataGridViewTriState.True;
        dgvItems.ColumnHeadersDefaultCellStyle = styleLineHeader;
        dgvItems.ColumnHeadersHeight = 45;
        dgvItems.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        dgvItems.Columns.AddRange(new DataGridViewColumn[] { colCode, colName, colQuantity, colUnitPrice, colLineTotal });
        styleLineCell.Alignment = DataGridViewContentAlignment.MiddleLeft;
        styleLineCell.BackColor = System.Drawing.Color.White;
        styleLineCell.Font = new Font("Tahoma", 10F);
        styleLineCell.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        styleLineCell.SelectionBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(255), Convert.ToInt32(255), Convert.ToInt32(180));
        styleLineCell.SelectionForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        styleLineCell.WrapMode = DataGridViewTriState.False;
        dgvItems.DefaultCellStyle = styleLineCell;
        dgvItems.Dock = DockStyle.Fill;
        dgvItems.EditMode = DataGridViewEditMode.EditOnEnter;
        dgvItems.EnableHeadersVisualStyles = false;
        dgvItems.GridColor = System.Drawing.Color.FromArgb(Convert.ToInt32(205), Convert.ToInt32(225), Convert.ToInt32(205));
        dgvItems.Location = new Point(6, 29);
        dgvItems.MultiSelect = true;
        dgvItems.Name = "dgvItems";
        dgvItems.RowHeadersVisible = false;
        dgvItems.RowHeadersWidth = 62;
        dgvItems.RowTemplate.Height = 32;
        dgvItems.SelectionMode = DataGridViewSelectionMode.CellSelect;
        dgvItems.Size = new Size(1404, 189);
        dgvItems.TabIndex = 0;
        // 
        // colCode
        // 
        colCode.DataPropertyName = "ProductCode";
        colCode.FillWeight = 18F;
        colCode.HeaderText = "الكود / الباركود";
        colCode.MinimumWidth = 120;
        colCode.Name = "colCode";
        colCode.SortMode = DataGridViewColumnSortMode.NotSortable;
        // 
        // colName
        // 
        colName.DataPropertyName = "ProductName";
        colName.FillWeight = 38F;
        colName.HeaderText = "اسم الصنف";
        colName.MinimumWidth = 160;
        colName.Name = "colName";
        colName.ReadOnly = true;
        colName.SortMode = DataGridViewColumnSortMode.NotSortable;
        // 
        // colQuantity
        // 
        colQuantity.DataPropertyName = "Quantity";
        colQuantity.FillWeight = 12F;
        colQuantity.HeaderText = "الكمية";
        colQuantity.MinimumWidth = 70;
        colQuantity.Name = "colQuantity";
        colQuantity.SortMode = DataGridViewColumnSortMode.NotSortable;
        // 
        // colUnitPrice
        // 
        colUnitPrice.DataPropertyName = "UnitPrice";
        colUnitPrice.FillWeight = 15F;
        colUnitPrice.HeaderText = "السعر";
        colUnitPrice.MinimumWidth = 80;
        colUnitPrice.Name = "colUnitPrice";
        colUnitPrice.SortMode = DataGridViewColumnSortMode.NotSortable;
        // 
        // colLineTotal
        // 
        colLineTotal.DataPropertyName = "LineTotal";
        colLineTotal.FillWeight = 17F;
        colLineTotal.HeaderText = "الإجمالي";
        colLineTotal.MinimumWidth = 100;
        colLineTotal.Name = "colLineTotal";
        colLineTotal.ReadOnly = true;
        colLineTotal.SortMode = DataGridViewColumnSortMode.NotSortable;
        // 
        // flpLineBar
        // 
        flpLineBar.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
        flpLineBar.Controls.Add(btnAddItem);
        flpLineBar.Controls.Add(btnRemoveItem);
        flpLineBar.Controls.Add(lblTotals);
        flpLineBar.Dock = DockStyle.Bottom;
        flpLineBar.Location = new Point(6, 218);
        flpLineBar.Name = "flpLineBar";
        flpLineBar.Size = new Size(1404, 48);
        flpLineBar.TabIndex = 1;
        flpLineBar.WrapContents = false;
        // 
        // btnAddItem
        // 
        btnAddItem.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(248), Convert.ToInt32(249), Convert.ToInt32(250));
        btnAddItem.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(Convert.ToInt32(205), Convert.ToInt32(225), Convert.ToInt32(205));
        btnAddItem.FlatStyle = FlatStyle.Flat;
        btnAddItem.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnAddItem.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        btnAddItem.Location = new Point(1234, 4);
        btnAddItem.Margin = new Padding(4, 4, 8, 4);
        btnAddItem.Name = "btnAddItem";
        btnAddItem.Size = new Size(162, 34);
        btnAddItem.TabIndex = 0;
        btnAddItem.Text = "➕ إضافة صنف";
        toolTip.SetToolTip(btnAddItem, "إضافة سطر صنف جديد للمرتجع");
        btnAddItem.UseVisualStyleBackColor = false;
        btnAddItem.Click += btnAddItem_Click;
        // 
        // btnRemoveItem
        // 
        btnRemoveItem.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(108), Convert.ToInt32(117), Convert.ToInt32(125));
        btnRemoveItem.FlatStyle = FlatStyle.Flat;
        btnRemoveItem.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnRemoveItem.ForeColor = System.Drawing.Color.White;
        btnRemoveItem.Location = new Point(1064, 4);
        btnRemoveItem.Margin = new Padding(4, 4, 8, 4);
        btnRemoveItem.Name = "btnRemoveItem";
        btnRemoveItem.Size = new Size(158, 34);
        btnRemoveItem.TabIndex = 1;
        btnRemoveItem.Text = "🗑 حذف السطر";
        toolTip.SetToolTip(btnRemoveItem, "حذف السطر المحدد من شبكة الأصناف");
        btnRemoveItem.UseVisualStyleBackColor = false;
        btnRemoveItem.Click += btnRemoveItem_Click;
        // 
        // lblTotals
        // 
        lblTotals.AutoSize = true;
        lblTotals.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblTotals.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
        lblTotals.Location = new Point(772, 12);
        lblTotals.Margin = new Padding(4, 12, 16, 0);
        lblTotals.Name = "lblTotals";
        lblTotals.Size = new Size(172, 24);
        lblTotals.TabIndex = 2;
        lblTotals.Text = "إجمالي المرتجع: 0.00";
        // 
        // pnlButtons
        // 
        pnlButtons.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
        pnlButtons.Controls.Add(btnNew);
        pnlButtons.Controls.Add(btnSaveDraft);
        pnlButtons.Controls.Add(btnPost);
        pnlButtons.Controls.Add(btnCancelInvoice);
        pnlButtons.Controls.Add(btnRefresh);
        pnlButtons.Controls.Add(btnClose);
        pnlButtons.Dock = DockStyle.Top;
        pnlButtons.Location = new Point(0, 434);
        pnlButtons.Name = "pnlButtons";
        pnlButtons.Padding = new Padding(10);
        pnlButtons.Size = new Size(1432, 60);
        pnlButtons.TabIndex = 3;
        // 
        // btnNew
        // 
        btnNew.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(13), Convert.ToInt32(110), Convert.ToInt32(253));
        btnNew.FlatStyle = FlatStyle.Flat;
        btnNew.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnNew.ForeColor = System.Drawing.Color.White;
        btnNew.Location = new Point(1284, 13);
        btnNew.Margin = new Padding(0, 0, 8, 0);
        btnNew.Name = "btnNew";
        btnNew.Size = new Size(140, 35);
        btnNew.TabIndex = 0;
        btnNew.Text = "🆕 جديد (F2)";
        toolTip.SetToolTip(btnNew, "بدء مرتجع مشتريات جديد — F2");
        btnNew.UseVisualStyleBackColor = false;
        btnNew.Click += btnNew_Click;
        // 
        // btnSaveDraft
        // 
        btnSaveDraft.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(25), Convert.ToInt32(135), Convert.ToInt32(84));
        btnSaveDraft.FlatStyle = FlatStyle.Flat;
        btnSaveDraft.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnSaveDraft.ForeColor = System.Drawing.Color.White;
        btnSaveDraft.Location = new Point(1130, 13);
        btnSaveDraft.Margin = new Padding(0, 0, 8, 0);
        btnSaveDraft.Name = "btnSaveDraft";
        btnSaveDraft.Size = new Size(146, 35);
        btnSaveDraft.TabIndex = 1;
        btnSaveDraft.Text = "💾 حفظ مسودة";
        toolTip.SetToolTip(btnSaveDraft, "حفظ المرتجع كمسودة (لا يمسّ المخزون ولا رصيد المورد)");
        btnSaveDraft.UseVisualStyleBackColor = false;
        btnSaveDraft.Click += btnSaveDraft_Click;
        // 
        // btnPost
        // 
        btnPost.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(23), Convert.ToInt32(162), Convert.ToInt32(184));
        btnPost.FlatStyle = FlatStyle.Flat;
        btnPost.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnPost.ForeColor = System.Drawing.Color.White;
        btnPost.Location = new Point(976, 13);
        btnPost.Margin = new Padding(0, 0, 8, 0);
        btnPost.Name = "btnPost";
        btnPost.Size = new Size(146, 35);
        btnPost.TabIndex = 2;
        btnPost.Text = "✅ ترحيل";
        toolTip.SetToolTip(btnPost, "ترحيل المرتجع وتقليل المخزون وتقليل رصيد المورد");
        btnPost.UseVisualStyleBackColor = false;
        btnPost.Click += btnPost_Click;
        // 
        // btnCancelInvoice
        // 
        btnCancelInvoice.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(220), Convert.ToInt32(53), Convert.ToInt32(69));
        btnCancelInvoice.FlatStyle = FlatStyle.Flat;
        btnCancelInvoice.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnCancelInvoice.ForeColor = System.Drawing.Color.White;
        btnCancelInvoice.Location = new Point(778, 13);
        btnCancelInvoice.Margin = new Padding(0, 0, 8, 0);
        btnCancelInvoice.Name = "btnCancelInvoice";
        btnCancelInvoice.Size = new Size(190, 35);
        btnCancelInvoice.TabIndex = 3;
        btnCancelInvoice.Text = "✖ إلغاء المستند";
        toolTip.SetToolTip(btnCancelInvoice, "إلغاء المرتجع المحدد بعد تأكيد صريح");
        btnCancelInvoice.UseVisualStyleBackColor = false;
        btnCancelInvoice.Click += btnCancelInvoice_Click;
        // 
        // btnRefresh
        // 
        btnRefresh.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(23), Convert.ToInt32(162), Convert.ToInt32(184));
        btnRefresh.FlatStyle = FlatStyle.Flat;
        btnRefresh.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnRefresh.ForeColor = System.Drawing.Color.White;
        btnRefresh.Location = new Point(644, 13);
        btnRefresh.Margin = new Padding(0, 0, 8, 0);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(126, 35);
        btnRefresh.TabIndex = 4;
        btnRefresh.Text = "🔄 تحديث (F5)";
        toolTip.SetToolTip(btnRefresh, "إعادة تحميل كل بيانات الشاشة وإرجاع المحرر لوضع العرض — F5");
        btnRefresh.UseVisualStyleBackColor = false;
        btnRefresh.Click += btnRefresh_Click;
        // 
        // btnClose
        // 
        btnClose.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(108), Convert.ToInt32(117), Convert.ToInt32(125));
        btnClose.FlatStyle = FlatStyle.Flat;
        btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnClose.ForeColor = System.Drawing.Color.White;
        btnClose.Location = new Point(520, 13);
        btnClose.Margin = new Padding(0, 0, 8, 0);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(116, 35);
        btnClose.TabIndex = 5;
        btnClose.Text = "🚪 إغلاق";
        toolTip.SetToolTip(btnClose, "إغلاق الشاشة — Esc");
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += btnClose_Click;
        // 
        // pnlFilters
        // 
        pnlFilters.Controls.Add(gbFilter);
        pnlFilters.Controls.Add(btnToggleFilters);
        pnlFilters.Dock = DockStyle.Top;
        pnlFilters.Location = new Point(0, 494);
        pnlFilters.Margin = new Padding(0);
        pnlFilters.Name = "pnlFilters";
        pnlFilters.Padding = new Padding(6, 4, 6, 4);
        pnlFilters.Size = new Size(1432, 112);
        pnlFilters.TabIndex = 4;
        // 
        // gbFilter
        // 
        gbFilter.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(245), Convert.ToInt32(235));
        gbFilter.Controls.Add(flpFilterRow2);
        gbFilter.Controls.Add(flpFilterRow1);
        gbFilter.Dock = DockStyle.Fill;
        gbFilter.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        gbFilter.Location = new Point(146, 4);
        gbFilter.Margin = new Padding(0);
        gbFilter.Name = "gbFilter";
        gbFilter.Padding = new Padding(8);
        gbFilter.RightToLeft = RightToLeft.Yes;
        gbFilter.Size = new Size(1280, 104);
        gbFilter.TabIndex = 0;
        gbFilter.TabStop = false;
        gbFilter.Text = "بحث وتصفية";
        // 
        // flpFilterRow2
        // 
        flpFilterRow2.Controls.Add(btnRangeToday);
        flpFilterRow2.Controls.Add(btnRangeMonth);
        flpFilterRow2.Controls.Add(btnRange30);
        flpFilterRow2.Controls.Add(chkOnlyCancelled);
        flpFilterRow2.Controls.Add(btnClearFilters);
        flpFilterRow2.Dock = DockStyle.Fill;
        flpFilterRow2.Location = new Point(8, 66);
        flpFilterRow2.Margin = new Padding(0);
        flpFilterRow2.Name = "flpFilterRow2";
        flpFilterRow2.Size = new Size(1264, 30);
        flpFilterRow2.TabIndex = 1;
        flpFilterRow2.WrapContents = false;
        // 
        // btnRangeToday
        // 
        btnRangeToday.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(248), Convert.ToInt32(249), Convert.ToInt32(250));
        btnRangeToday.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(Convert.ToInt32(205), Convert.ToInt32(225), Convert.ToInt32(205));
        btnRangeToday.FlatStyle = FlatStyle.Flat;
        btnRangeToday.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        btnRangeToday.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        btnRangeToday.Location = new Point(1167, 0);
        btnRangeToday.Margin = new Padding(4, 0, 4, 0);
        btnRangeToday.Name = "btnRangeToday";
        btnRangeToday.Size = new Size(93, 30);
        btnRangeToday.TabIndex = 0;
        btnRangeToday.Text = "اليوم";
        btnRangeToday.UseVisualStyleBackColor = false;
        btnRangeToday.Click += btnRangeToday_Click;
        // 
        // btnRangeMonth
        // 
        btnRangeMonth.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(248), Convert.ToInt32(249), Convert.ToInt32(250));
        btnRangeMonth.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(Convert.ToInt32(205), Convert.ToInt32(225), Convert.ToInt32(205));
        btnRangeMonth.FlatStyle = FlatStyle.Flat;
        btnRangeMonth.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        btnRangeMonth.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        btnRangeMonth.Location = new Point(1035, 0);
        btnRangeMonth.Margin = new Padding(4, 0, 4, 0);
        btnRangeMonth.Name = "btnRangeMonth";
        btnRangeMonth.Size = new Size(124, 30);
        btnRangeMonth.TabIndex = 1;
        btnRangeMonth.Text = "هذا الشهر";
        btnRangeMonth.UseVisualStyleBackColor = false;
        btnRangeMonth.Click += btnRangeMonth_Click;
        // 
        // btnRange30
        // 
        btnRange30.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(248), Convert.ToInt32(249), Convert.ToInt32(250));
        btnRange30.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(Convert.ToInt32(205), Convert.ToInt32(225), Convert.ToInt32(205));
        btnRange30.FlatStyle = FlatStyle.Flat;
        btnRange30.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        btnRange30.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        btnRange30.Location = new Point(900, 0);
        btnRange30.Margin = new Padding(4, 0, 4, 0);
        btnRange30.Name = "btnRange30";
        btnRange30.Size = new Size(127, 30);
        btnRange30.TabIndex = 2;
        btnRange30.Text = "آخر 30 يوماً";
        btnRange30.UseVisualStyleBackColor = false;
        btnRange30.Click += btnRange30_Click;
        // 
        // chkOnlyCancelled
        // 
        chkOnlyCancelled.AutoSize = true;
        chkOnlyCancelled.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        chkOnlyCancelled.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        chkOnlyCancelled.Location = new Point(748, 5);
        chkOnlyCancelled.Margin = new Padding(8, 5, 8, 0);
        chkOnlyCancelled.Name = "chkOnlyCancelled";
        chkOnlyCancelled.Size = new Size(140, 26);
        chkOnlyCancelled.TabIndex = 3;
        chkOnlyCancelled.Text = "الملغاة فقط";
        chkOnlyCancelled.UseVisualStyleBackColor = false;
        chkOnlyCancelled.CheckedChanged += chkOnlyCancelled_CheckedChanged;
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
        btnClearFilters.Location = new Point(546, 0);
        btnClearFilters.Margin = new Padding(16, 0, 4, 0);
        btnClearFilters.Name = "btnClearFilters";
        btnClearFilters.Size = new Size(194, 30);
        btnClearFilters.TabIndex = 4;
        btnClearFilters.Text = "🧹 مسح التصفية";
        toolTip.SetToolTip(btnClearFilters, "مسح كل أدوات التصفية (Esc) — لا يعيد تحميل البيانات");
        btnClearFilters.UseVisualStyleBackColor = false;
        btnClearFilters.Click += btnClearFilters_Click;
        // 
        // flpFilterRow1
        // 
        flpFilterRow1.Controls.Add(labelFilterBy);
        flpFilterRow1.Controls.Add(cbFilterBy);
        flpFilterRow1.Controls.Add(txtFilterValue);
        flpFilterRow1.Controls.Add(labelStatus);
        flpFilterRow1.Controls.Add(cbStatus);
        flpFilterRow1.Controls.Add(labelFrom);
        flpFilterRow1.Controls.Add(dtpFrom);
        flpFilterRow1.Controls.Add(labelTo);
        flpFilterRow1.Controls.Add(dtpTo);
        flpFilterRow1.Dock = DockStyle.Top;
        flpFilterRow1.Location = new Point(8, 28);
        flpFilterRow1.Margin = new Padding(0);
        flpFilterRow1.Name = "flpFilterRow1";
        flpFilterRow1.Size = new Size(1264, 38);
        flpFilterRow1.TabIndex = 0;
        flpFilterRow1.WrapContents = false;
        // 
        // labelFilterBy
        // 
        labelFilterBy.AutoSize = true;
        labelFilterBy.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        labelFilterBy.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        labelFilterBy.Location = new Point(1174, 8);
        labelFilterBy.Margin = new Padding(6, 8, 4, 0);
        labelFilterBy.Name = "labelFilterBy";
        labelFilterBy.Size = new Size(86, 22);
        labelFilterBy.TabIndex = 0;
        labelFilterBy.Text = "تصفية بـ:";
        // 
        // cbFilterBy
        // 
        cbFilterBy.BackColor = System.Drawing.Color.White;
        cbFilterBy.DropDownStyle = ComboBoxStyle.DropDownList;
        cbFilterBy.FlatStyle = FlatStyle.Flat;
        cbFilterBy.Font = new Font("Tahoma", 10F);
        cbFilterBy.FormattingEnabled = true;
        cbFilterBy.Items.AddRange(new object[] { "العميل/المورد", "رقم الفاتورة", "البيان", "المبلغ" });
        cbFilterBy.Location = new Point(960, 3);
        cbFilterBy.Margin = new Padding(4, 3, 6, 4);
        cbFilterBy.Name = "cbFilterBy";
        cbFilterBy.Size = new Size(204, 32);
        cbFilterBy.TabIndex = 1;
        cbFilterBy.SelectedIndexChanged += cbFilterBy_SelectedIndexChanged;
        // 
        // txtFilterValue
        // 
        txtFilterValue.BackColor = System.Drawing.Color.White;
        txtFilterValue.Font = new Font("Tahoma", 10F);
        txtFilterValue.Location = new Point(714, 5);
        txtFilterValue.Margin = new Padding(4, 5, 6, 4);
        txtFilterValue.Name = "txtFilterValue";
        txtFilterValue.PlaceholderText = "اكتب قيمة التصفية...";
        txtFilterValue.Size = new Size(236, 32);
        txtFilterValue.TabIndex = 2;
        txtFilterValue.TextChanged += txtFilterValue_TextChanged;
        txtFilterValue.KeyDown += txtFilterValue_KeyDown;
        // 
        // labelStatus
        // 
        labelStatus.AutoSize = true;
        labelStatus.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        labelStatus.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        labelStatus.Location = new Point(637, 8);
        labelStatus.Margin = new Padding(10, 8, 4, 0);
        labelStatus.Name = "labelStatus";
        labelStatus.Size = new Size(67, 22);
        labelStatus.TabIndex = 3;
        labelStatus.Text = "الحالة:";
        // 
        // cbStatus
        // 
        cbStatus.BackColor = System.Drawing.Color.White;
        cbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
        cbStatus.FlatStyle = FlatStyle.Flat;
        cbStatus.Font = new Font("Tahoma", 10F);
        cbStatus.FormattingEnabled = true;
        cbStatus.Location = new Point(487, 3);
        cbStatus.Margin = new Padding(4, 3, 6, 4);
        cbStatus.Name = "cbStatus";
        cbStatus.Size = new Size(140, 32);
        cbStatus.TabIndex = 4;
        cbStatus.SelectedIndexChanged += cbStatus_SelectedIndexChanged;
        // 
        // labelFrom
        // 
        labelFrom.AutoSize = true;
        labelFrom.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        labelFrom.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        labelFrom.Location = new Point(430, 8);
        labelFrom.Margin = new Padding(10, 8, 4, 0);
        labelFrom.Name = "labelFrom";
        labelFrom.Size = new Size(43, 22);
        labelFrom.TabIndex = 5;
        labelFrom.Text = "من:";
        // 
        // dtpFrom
        // 
        dtpFrom.BackColor = System.Drawing.Color.White;
        dtpFrom.Font = new Font("Tahoma", 10F);
        dtpFrom.Format = DateTimePickerFormat.Short;
        dtpFrom.Location = new Point(282, 4);
        dtpFrom.Margin = new Padding(4, 4, 6, 4);
        dtpFrom.Name = "dtpFrom";
        dtpFrom.Size = new Size(138, 32);
        dtpFrom.TabIndex = 6;
        dtpFrom.ValueChanged += dtpFrom_ValueChanged;
        // 
        // labelTo
        // 
        labelTo.AutoSize = true;
        labelTo.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        labelTo.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        labelTo.Location = new Point(225, 8);
        labelTo.Margin = new Padding(10, 8, 4, 0);
        labelTo.Name = "labelTo";
        labelTo.Size = new Size(49, 22);
        labelTo.TabIndex = 7;
        labelTo.Text = "إلى:";
        // 
        // dtpTo
        // 
        dtpTo.BackColor = System.Drawing.Color.White;
        dtpTo.Font = new Font("Tahoma", 10F);
        dtpTo.Format = DateTimePickerFormat.Short;
        dtpTo.Location = new Point(77, 4);
        dtpTo.Margin = new Padding(4, 4, 6, 4);
        dtpTo.Name = "dtpTo";
        dtpTo.Size = new Size(138, 32);
        dtpTo.TabIndex = 8;
        dtpTo.ValueChanged += dtpTo_ValueChanged;
        // 
        // btnToggleFilters
        // 
        btnToggleFilters.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(248), Convert.ToInt32(249), Convert.ToInt32(250));
        btnToggleFilters.Cursor = Cursors.Hand;
        btnToggleFilters.Dock = DockStyle.Left;
        btnToggleFilters.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(Convert.ToInt32(205), Convert.ToInt32(225), Convert.ToInt32(205));
        btnToggleFilters.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(240), Convert.ToInt32(240), Convert.ToInt32(240));
        btnToggleFilters.FlatStyle = FlatStyle.Popup;
        btnToggleFilters.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        btnToggleFilters.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
        btnToggleFilters.Location = new Point(6, 4);
        btnToggleFilters.Name = "btnToggleFilters";
        btnToggleFilters.Size = new Size(140, 104);
        btnToggleFilters.TabIndex = 1;
        btnToggleFilters.Text = "إخفاء التصفية";
        btnToggleFilters.UseVisualStyleBackColor = false;
        // 
        // pnlGrid
        // 
        pnlGrid.Controls.Add(lblNoData);
        pnlGrid.Controls.Add(dgvMain);
        pnlGrid.Dock = DockStyle.Fill;
        pnlGrid.Location = new Point(0, 606);
        pnlGrid.Name = "pnlGrid";
        pnlGrid.Padding = new Padding(0, 10, 0, 0);
        pnlGrid.Size = new Size(1432, 262);
        pnlGrid.TabIndex = 5;
        // 
        // lblNoData
        // 
        lblNoData.Dock = DockStyle.Fill;
        lblNoData.Font = new Font("Tahoma", 12F, FontStyle.Bold);
        lblNoData.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblNoData.Location = new Point(0, 10);
        lblNoData.Name = "lblNoData";
        lblNoData.Size = new Size(1432, 252);
        lblNoData.TabIndex = 1;
        lblNoData.Text = "لا توجد بيانات";
        lblNoData.TextAlign = ContentAlignment.MiddleCenter;
        lblNoData.Visible = false;
        // 
        // dgvMain
        // 
        dgvMain.AllowUserToAddRows = false;
        dgvMain.AllowUserToDeleteRows = false;
        dgvMain.AllowUserToResizeRows = false;
        styleMainAlt.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(245), Convert.ToInt32(252), Convert.ToInt32(245));
        dgvMain.AlternatingRowsDefaultCellStyle = styleMainAlt;
        dgvMain.AutoGenerateColumns = false;
        dgvMain.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvMain.BackgroundColor = System.Drawing.Color.FromArgb(Convert.ToInt32(220), Convert.ToInt32(240), Convert.ToInt32(220));
        dgvMain.BorderStyle = BorderStyle.None;
        dgvMain.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
        styleMainHeader.Alignment = DataGridViewContentAlignment.MiddleCenter;
        styleMainHeader.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
        styleMainHeader.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        styleMainHeader.ForeColor = System.Drawing.Color.White;
        styleMainHeader.SelectionBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
        styleMainHeader.SelectionForeColor = System.Drawing.Color.White;
        styleMainHeader.WrapMode = DataGridViewTriState.True;
        dgvMain.ColumnHeadersDefaultCellStyle = styleMainHeader;
        dgvMain.ColumnHeadersHeight = 45;
        dgvMain.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        dgvMain.Columns.AddRange(new DataGridViewColumn[] { colReturnNo, colReturnDate, colSupplier, colTotal, colStatus });
        dgvMain.ContextMenuStrip = cmsRows;
        styleMainCell.Alignment = DataGridViewContentAlignment.MiddleLeft;
        styleMainCell.BackColor = System.Drawing.Color.White;
        styleMainCell.Font = new Font("Tahoma", 10F);
        styleMainCell.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        styleMainCell.SelectionBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(255), Convert.ToInt32(255), Convert.ToInt32(180));
        styleMainCell.SelectionForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        styleMainCell.WrapMode = DataGridViewTriState.False;
        dgvMain.DefaultCellStyle = styleMainCell;
        dgvMain.Dock = DockStyle.Fill;
        dgvMain.EnableHeadersVisualStyles = false;
        dgvMain.GridColor = System.Drawing.Color.FromArgb(Convert.ToInt32(205), Convert.ToInt32(225), Convert.ToInt32(205));
        dgvMain.Location = new Point(0, 10);
        dgvMain.MultiSelect = false;
        dgvMain.Name = "dgvMain";
        dgvMain.ReadOnly = true;
        dgvMain.RowHeadersVisible = false;
        dgvMain.RowHeadersWidth = 62;
        dgvMain.RowTemplate.Height = 32;
        dgvMain.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvMain.Size = new Size(1432, 252);
        dgvMain.TabIndex = 0;
        dgvMain.CellClick += dgvMain_CellClick;
        // 
        // colReturnNo
        // 
        colReturnNo.DataPropertyName = "ReturnNo";
        colReturnNo.FillWeight = 12F;
        colReturnNo.HeaderText = "رقم المرتجع";
        colReturnNo.MinimumWidth = 100;
        colReturnNo.Name = "colReturnNo";
        colReturnNo.SortMode = DataGridViewColumnSortMode.NotSortable;
        // 
        // colReturnDate
        // 
        colReturnDate.DataPropertyName = "DateText";
        colReturnDate.FillWeight = 14F;
        colReturnDate.HeaderText = "التاريخ";
        colReturnDate.MinimumWidth = 110;
        colReturnDate.Name = "colReturnDate";
        colReturnDate.SortMode = DataGridViewColumnSortMode.NotSortable;
        // 
        // colSupplier
        // 
        colSupplier.DataPropertyName = "SupplierName";
        colSupplier.FillWeight = 36F;
        colSupplier.HeaderText = "المورد";
        colSupplier.MinimumWidth = 160;
        colSupplier.Name = "colSupplier";
        colSupplier.SortMode = DataGridViewColumnSortMode.NotSortable;
        // 
        // colTotal
        // 
        colTotal.DataPropertyName = "TotalText";
        colTotal.FillWeight = 16F;
        colTotal.HeaderText = "الإجمالي";
        colTotal.MinimumWidth = 110;
        colTotal.Name = "colTotal";
        colTotal.SortMode = DataGridViewColumnSortMode.NotSortable;
        // 
        // colStatus
        // 
        colStatus.DataPropertyName = "StatusText";
        colStatus.FillWeight = 14F;
        colStatus.HeaderText = "الحالة";
        colStatus.MinimumWidth = 100;
        colStatus.Name = "colStatus";
        colStatus.SortMode = DataGridViewColumnSortMode.NotSortable;
        // 
        // pnlSummary
        // 
        pnlSummary.Controls.Add(lblSummary);
        pnlSummary.Dock = DockStyle.Bottom;
        pnlSummary.Location = new Point(0, 868);
        pnlSummary.Margin = new Padding(0);
        pnlSummary.Name = "pnlSummary";
        pnlSummary.Size = new Size(1432, 32);
        pnlSummary.TabIndex = 6;
        // 
        // lblSummary
        // 
        lblSummary.Dock = DockStyle.Fill;
        lblSummary.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblSummary.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
        lblSummary.Location = new Point(0, 0);
        lblSummary.Name = "lblSummary";
        lblSummary.Size = new Size(1432, 32);
        lblSummary.TabIndex = 0;
        lblSummary.Text = "المعروض: 0 من 0";
        lblSummary.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // cmsRows
        // 
        cmsRows.Items.AddRange(new ToolStripItem[] { cmsView, cmsRefresh });
        cmsRows.Name = "cmsRows";
        cmsRows.Size = new Size(232, 64);
        cmsRows.Opening += cmsRows_Opening;
        // 
        // cmsView
        // 
        cmsView.Name = "cmsView";
        cmsView.Size = new Size(231, 30);
        cmsView.Text = "👁 عرض المستند";
        cmsView.Click += cmsView_Click;
        // 
        // cmsRefresh
        // 
        cmsRefresh.Name = "cmsRefresh";
        cmsRefresh.Size = new Size(231, 30);
        cmsRefresh.Text = "🔄 تحديث (F5)";
        cmsRefresh.Click += cmsRefresh_Click;
        // 
        // errorProvider
        // 
        errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        errorProvider.ContainerControl = this;
        errorProvider.RightToLeft = true;
        // 
        // frmPurchaseReturnMaster
        // 
        AutoScaleDimensions = new SizeF(11F, 24F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(170), Convert.ToInt32(220), Convert.ToInt32(170));
        ClientSize = new Size(1432, 900);
        Controls.Add(pnlGrid);
        Controls.Add(pnlSummary);
        Controls.Add(pnlFilters);
        Controls.Add(pnlButtons);
        Controls.Add(pnlLines);
        Controls.Add(pnlFields);
        Controls.Add(lblMode);
        Font = new Font("Tahoma", 10F);
        Name = "frmPurchaseReturnMaster";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterParent;
        Text = "مرتجع مشتريات";
        Load += frmPurchaseReturnMaster_Load;
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        pnlFields.ResumeLayout(false);
        pnlFields.PerformLayout();
        tblHeader.ResumeLayout(false);
        tblHeader.PerformLayout();
        pnlLines.ResumeLayout(false);
        gbLines.ResumeLayout(false);
        gbLines.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvItems).EndInit();
        flpLineBar.ResumeLayout(false);
        flpLineBar.PerformLayout();
        pnlButtons.ResumeLayout(false);
        pnlFilters.ResumeLayout(false);
        gbFilter.ResumeLayout(false);
        gbFilter.PerformLayout();
        flpFilterRow2.ResumeLayout(false);
        flpFilterRow2.PerformLayout();
        flpFilterRow1.ResumeLayout(false);
        flpFilterRow1.PerformLayout();
        pnlGrid.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvMain).EndInit();
        pnlSummary.ResumeLayout(false);
        ResumeLayout(false);
    }

    private System.Windows.Forms.Label lblMode;
    private System.Windows.Forms.Panel pnlFields;
    private System.Windows.Forms.TableLayoutPanel tblHeader;
    private System.Windows.Forms.Label lblInvoiceNoCaption;
    private System.Windows.Forms.Label lblInvoiceNo;
    private System.Windows.Forms.Label lblDateCaption;
    private System.Windows.Forms.DateTimePicker dtpDate;
    private System.Windows.Forms.Label lblStatusCaption;
    private System.Windows.Forms.Label lblStatus;
    private System.Windows.Forms.Label lblSupplierCaption;
    private System.Windows.Forms.ComboBox cmbSupplier;
    private System.Windows.Forms.Label lblWarehouseCaption;
    private System.Windows.Forms.ComboBox cmbWarehouse;
    private System.Windows.Forms.Label lblNotesCaption;
    private System.Windows.Forms.TextBox txtNotes;
    private System.Windows.Forms.Panel pnlLines;
    private System.Windows.Forms.GroupBox gbLines;
    private System.Windows.Forms.DataGridView dgvItems;
    private System.Windows.Forms.DataGridViewTextBoxColumn colCode;
    private System.Windows.Forms.DataGridViewTextBoxColumn colName;
    private System.Windows.Forms.DataGridViewTextBoxColumn colQuantity;
    private System.Windows.Forms.DataGridViewTextBoxColumn colUnitPrice;
    private System.Windows.Forms.DataGridViewTextBoxColumn colLineTotal;
    private System.Windows.Forms.FlowLayoutPanel flpLineBar;
    private System.Windows.Forms.Button btnAddItem;
    private System.Windows.Forms.Button btnRemoveItem;
    private System.Windows.Forms.Label lblTotals;
    private System.Windows.Forms.FlowLayoutPanel pnlButtons;
    private System.Windows.Forms.Button btnNew;
    private System.Windows.Forms.Button btnSaveDraft;
    private System.Windows.Forms.Button btnPost;
    private System.Windows.Forms.Button btnCancelInvoice;
    private System.Windows.Forms.Button btnRefresh;
    private System.Windows.Forms.Button btnClose;
    private System.Windows.Forms.Panel pnlFilters;
    private System.Windows.Forms.GroupBox gbFilter;
    private System.Windows.Forms.FlowLayoutPanel flpFilterRow1;
    private System.Windows.Forms.FlowLayoutPanel flpFilterRow2;
    private System.Windows.Forms.Label labelFilterBy;
    private System.Windows.Forms.ComboBox cbFilterBy;
    private System.Windows.Forms.TextBox txtFilterValue;
    private System.Windows.Forms.Label labelStatus;
    private System.Windows.Forms.ComboBox cbStatus;
    private System.Windows.Forms.Label labelFrom;
    private System.Windows.Forms.DateTimePicker dtpFrom;
    private System.Windows.Forms.Label labelTo;
    private System.Windows.Forms.DateTimePicker dtpTo;
    private System.Windows.Forms.Button btnRangeToday;
    private System.Windows.Forms.Button btnRangeMonth;
    private System.Windows.Forms.Button btnRange30;
    private System.Windows.Forms.CheckBox chkOnlyCancelled;
    private System.Windows.Forms.Button btnClearFilters;
    private System.Windows.Forms.Button btnToggleFilters;
    private System.Windows.Forms.Panel pnlGrid;
    private System.Windows.Forms.Label lblNoData;
    private System.Windows.Forms.DataGridView dgvMain;
    private System.Windows.Forms.DataGridViewTextBoxColumn colReturnNo;
    private System.Windows.Forms.DataGridViewTextBoxColumn colReturnDate;
    private System.Windows.Forms.DataGridViewTextBoxColumn colSupplier;
    private System.Windows.Forms.DataGridViewTextBoxColumn colTotal;
    private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
    private System.Windows.Forms.Panel pnlSummary;
    private System.Windows.Forms.Label lblSummary;
    private System.Windows.Forms.ContextMenuStrip cmsRows;
    private System.Windows.Forms.ToolStripMenuItem cmsView;
    private System.Windows.Forms.ToolStripMenuItem cmsRefresh;
    private System.Windows.Forms.ErrorProvider errorProvider;
    private System.Windows.Forms.ToolTip toolTip;
}
