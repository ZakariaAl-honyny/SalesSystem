namespace SalesSystem.Desktop.Screens.Returns.Forms;

partial class frmSalesReturnMaster
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

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        DataGridViewCellStyle cellStyleItemsAlternate = new DataGridViewCellStyle();
        DataGridViewCellStyle cellStyleItemsHeader = new DataGridViewCellStyle();
        DataGridViewCellStyle cellStyleItemsDefault = new DataGridViewCellStyle();
        DataGridViewCellStyle cellStyleMainAlternate = new DataGridViewCellStyle();
        DataGridViewCellStyle cellStyleMainHeader = new DataGridViewCellStyle();
        DataGridViewCellStyle cellStyleMainDefault = new DataGridViewCellStyle();
        cmsRows = new ContextMenuStrip(components);
        cmsView = new ToolStripMenuItem();
        cmsPost = new ToolStripMenuItem();
        cmsCancel = new ToolStripMenuItem();
        cmsSeparator = new ToolStripSeparator();
        cmsRefresh = new ToolStripMenuItem();
        lblMode = new Label();
        pnlFields = new Panel();
        gbDocument = new GroupBox();
        lblReturnNo = new Label();
        txtReturnNo = new TextBox();
        lblDate = new Label();
        dtpDate = new DateTimePicker();
        lblCustomer = new Label();
        lblCustomerReq = new Label();
        cmbCustomer = new ComboBox();
        lblWarehouse = new Label();
        lblWarehouseReq = new Label();
        cmbWarehouse = new ComboBox();
        lblStatusCaption = new Label();
        lblStatusValue = new Label();
        lblNotes = new Label();
        txtNotes = new TextBox();
        pnlLines = new Panel();
        gbLines = new GroupBox();
        dgvItems = new DataGridView();
        colBarcode = new DataGridViewTextBoxColumn();
        colProductName = new DataGridViewTextBoxColumn();
        colQuantity = new DataGridViewTextBoxColumn();
        colUnitPrice = new DataGridViewTextBoxColumn();
        colLineTotal = new DataGridViewTextBoxColumn();
        pnlLineTools = new FlowLayoutPanel();
        btnAddItem = new Button();
        btnRemoveItem = new Button();
        lblTotals = new Label();
        pnlButtons = new FlowLayoutPanel();
        btnNew = new Button();
        btnSaveDraft = new Button();
        btnPost = new Button();
        btnCancelDocument = new Button();
        btnRefresh = new Button();
        btnClose = new Button();
        pnlFilters = new Panel();
        gbFilter = new GroupBox();
        lblFilterBy = new Label();
        cbFilterBy = new ComboBox();
        txtFilterValue = new TextBox();
        lblStatusFilter = new Label();
        cbStatus = new ComboBox();
        chkOnlyCancelled = new CheckBox();
        lblFrom = new Label();
        dtpFrom = new DateTimePicker();
        lblTo = new Label();
        dtpTo = new DateTimePicker();
        btnRangeToday = new Button();
        btnRangeMonth = new Button();
        btnRange30 = new Button();
        btnClearFilters = new Button();
        btnToggleFilters = new Button();
        pnlGrid = new Panel();
        dgvMain = new DataGridView();
        colId = new DataGridViewTextBoxColumn();
        colReturnNo = new DataGridViewTextBoxColumn();
        colReturnDate = new DataGridViewTextBoxColumn();
        colCustomerName = new DataGridViewTextBoxColumn();
        colTotalAmount = new DataGridViewTextBoxColumn();
        colStatusName = new DataGridViewTextBoxColumn();
        lblEmpty = new Label();
        pnlSummary = new Panel();
        lblSummary = new Label();
        toolTip = new ToolTip(components);
        errorProvider = new ErrorProvider(components);
        cmsRows.SuspendLayout();
        pnlFields.SuspendLayout();
        gbDocument.SuspendLayout();
        pnlLines.SuspendLayout();
        gbLines.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvItems).BeginInit();
        pnlLineTools.SuspendLayout();
        pnlButtons.SuspendLayout();
        pnlFilters.SuspendLayout();
        gbFilter.SuspendLayout();
        pnlGrid.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvMain).BeginInit();
        pnlSummary.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
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
        lblMode.Text = "(( مرتجعات المبيعات ))";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlFields
        // 
        pnlFields.Controls.Add(gbDocument);
        pnlFields.Dock = DockStyle.Top;
        pnlFields.Location = new Point(0, 50);
        pnlFields.Name = "pnlFields";
        pnlFields.Padding = new Padding(6, 4, 6, 4);
        pnlFields.Size = new Size(1432, 150);
        pnlFields.TabIndex = 1;
        // 
        // gbDocument
        // 
        gbDocument.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(245), Convert.ToInt32(235));
        gbDocument.Controls.Add(lblReturnNo);
        gbDocument.Controls.Add(txtReturnNo);
        gbDocument.Controls.Add(lblDate);
        gbDocument.Controls.Add(dtpDate);
        gbDocument.Controls.Add(lblCustomer);
        gbDocument.Controls.Add(lblCustomerReq);
        gbDocument.Controls.Add(cmbCustomer);
        gbDocument.Controls.Add(lblWarehouse);
        gbDocument.Controls.Add(lblWarehouseReq);
        gbDocument.Controls.Add(cmbWarehouse);
        gbDocument.Controls.Add(lblStatusCaption);
        gbDocument.Controls.Add(lblStatusValue);
        gbDocument.Controls.Add(lblNotes);
        gbDocument.Controls.Add(txtNotes);
        gbDocument.Dock = DockStyle.Fill;
        gbDocument.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        gbDocument.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        gbDocument.Location = new Point(6, 4);
        gbDocument.Name = "gbDocument";
        gbDocument.Padding = new Padding(10, 4, 10, 4);
        gbDocument.Size = new Size(1420, 142);
        gbDocument.TabIndex = 0;
        gbDocument.TabStop = false;
        gbDocument.Text = "بيانات المرتجع";
        // 
        // lblReturnNo
        // 
        lblReturnNo.AutoSize = true;
        lblReturnNo.Location = new Point(1250, 26);
        lblReturnNo.Name = "lblReturnNo";
        lblReturnNo.Size = new Size(106, 24);
        lblReturnNo.TabIndex = 0;
        lblReturnNo.Text = "رقم المرتجع";
        // 
        // txtReturnNo
        // 
        txtReturnNo.BackColor = System.Drawing.Color.White;
        txtReturnNo.Location = new Point(1246, 52);
        txtReturnNo.Name = "txtReturnNo";
        txtReturnNo.ReadOnly = true;
        txtReturnNo.Size = new Size(124, 32);
        txtReturnNo.TabIndex = 0;
        txtReturnNo.TabStop = false;
        // 
        // lblDate
        // 
        lblDate.AutoSize = true;
        lblDate.Location = new Point(1080, 26);
        lblDate.Name = "lblDate";
        lblDate.Size = new Size(68, 24);
        lblDate.TabIndex = 0;
        lblDate.Text = "التاريخ";
        // 
        // dtpDate
        // 
        dtpDate.BackColor = System.Drawing.Color.White;
        dtpDate.Font = new Font("Tahoma", 10F);
        dtpDate.Format = DateTimePickerFormat.Short;
        dtpDate.Location = new Point(1076, 52);
        dtpDate.Name = "dtpDate";
        dtpDate.RightToLeft = RightToLeft.Yes;
        dtpDate.Size = new Size(156, 32);
        dtpDate.TabIndex = 0;
        // 
        // lblCustomer
        // 
        lblCustomer.AutoSize = true;
        lblCustomer.Location = new Point(912, 26);
        lblCustomer.Name = "lblCustomer";
        lblCustomer.Size = new Size(64, 24);
        lblCustomer.TabIndex = 0;
        lblCustomer.Text = "العميل";
        // 
        // lblCustomerReq
        // 
        lblCustomerReq.AutoSize = true;
        lblCustomerReq.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblCustomerReq.ForeColor = System.Drawing.Color.Red;
        lblCustomerReq.Location = new Point(894, 26);
        lblCustomerReq.Name = "lblCustomerReq";
        lblCustomerReq.Size = new Size(23, 24);
        lblCustomerReq.TabIndex = 0;
        lblCustomerReq.Text = "*";
        // 
        // cmbCustomer
        // 
        cmbCustomer.BackColor = System.Drawing.Color.White;
        cmbCustomer.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbCustomer.FlatStyle = FlatStyle.Flat;
        cmbCustomer.Font = new Font("Tahoma", 10F);
        cmbCustomer.Location = new Point(908, 52);
        cmbCustomer.Name = "cmbCustomer";
        cmbCustomer.Size = new Size(168, 32);
        cmbCustomer.TabIndex = 1;
        // 
        // lblWarehouse
        // 
        lblWarehouse.AutoSize = true;
        lblWarehouse.Location = new Point(742, 26);
        lblWarehouse.Name = "lblWarehouse";
        lblWarehouse.Size = new Size(80, 24);
        lblWarehouse.TabIndex = 0;
        lblWarehouse.Text = "المستودع";
        // 
        // lblWarehouseReq
        // 
        lblWarehouseReq.AutoSize = true;
        lblWarehouseReq.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblWarehouseReq.ForeColor = System.Drawing.Color.Red;
        lblWarehouseReq.Location = new Point(724, 26);
        lblWarehouseReq.Name = "lblWarehouseReq";
        lblWarehouseReq.Size = new Size(23, 24);
        lblWarehouseReq.TabIndex = 0;
        lblWarehouseReq.Text = "*";
        // 
        // cmbWarehouse
        // 
        cmbWarehouse.BackColor = System.Drawing.Color.White;
        cmbWarehouse.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbWarehouse.FlatStyle = FlatStyle.Flat;
        cmbWarehouse.Font = new Font("Tahoma", 10F);
        cmbWarehouse.Location = new Point(738, 52);
        cmbWarehouse.Name = "cmbWarehouse";
        cmbWarehouse.Size = new Size(170, 32);
        cmbWarehouse.TabIndex = 2;
        // 
        // lblStatusCaption
        // 
        lblStatusCaption.AutoSize = true;
        lblStatusCaption.Location = new Point(566, 26);
        lblStatusCaption.Name = "lblStatusCaption";
        lblStatusCaption.Size = new Size(112, 24);
        lblStatusCaption.TabIndex = 0;
        lblStatusCaption.Text = "حالة المستند";
        // 
        // lblStatusValue
        // 
        lblStatusValue.BackColor = System.Drawing.Color.White;
        lblStatusValue.BorderStyle = BorderStyle.FixedSingle;
        lblStatusValue.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblStatusValue.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
        lblStatusValue.Location = new Point(556, 52);
        lblStatusValue.Name = "lblStatusValue";
        lblStatusValue.Size = new Size(180, 32);
        lblStatusValue.TabIndex = 0;
        lblStatusValue.Text = "—";
        lblStatusValue.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblNotes
        // 
        lblNotes.Location = new Point(1230, 96);
        lblNotes.Name = "lblNotes";
        lblNotes.Size = new Size(130, 26);
        lblNotes.TabIndex = 0;
        lblNotes.Text = "البيان / ملاحظات";
        lblNotes.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtNotes
        // 
        txtNotes.BackColor = System.Drawing.Color.White;
        txtNotes.Font = new Font("Tahoma", 10F);
        txtNotes.Location = new Point(700, 94);
        txtNotes.Multiline = true;
        txtNotes.Name = "txtNotes";
        txtNotes.RightToLeft = RightToLeft.Yes;
        txtNotes.ScrollBars = ScrollBars.Vertical;
        txtNotes.Size = new Size(520, 40);
        txtNotes.TabIndex = 3;
        // 
        // pnlLines
        // 
        pnlLines.Controls.Add(gbLines);
        pnlLines.Dock = DockStyle.Top;
        pnlLines.Location = new Point(0, 200);
        pnlLines.Name = "pnlLines";
        pnlLines.Padding = new Padding(6, 4, 6, 6);
        pnlLines.Size = new Size(1432, 260);
        pnlLines.TabIndex = 2;
        // 
        // gbLines
        // 
        gbLines.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(245), Convert.ToInt32(235));
        gbLines.Controls.Add(dgvItems);
        gbLines.Controls.Add(pnlLineTools);
        gbLines.Dock = DockStyle.Fill;
        gbLines.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        gbLines.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        gbLines.Location = new Point(6, 4);
        gbLines.Name = "gbLines";
        gbLines.Padding = new Padding(6, 4, 6, 6);
        gbLines.Size = new Size(1420, 250);
        gbLines.TabIndex = 0;
        gbLines.TabStop = false;
        gbLines.Text = "أصناف المرتجع";
        // 
        // pnlLineTools
        // 
        pnlLineTools.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
        pnlLineTools.Controls.Add(btnAddItem);
        pnlLineTools.Controls.Add(btnRemoveItem);
        pnlLineTools.Controls.Add(lblTotals);
        pnlLineTools.Dock = DockStyle.Top;
        pnlLineTools.Location = new Point(6, 29);
        pnlLineTools.Name = "pnlLineTools";
        pnlLineTools.Padding = new Padding(0, 2, 0, 2);
        pnlLineTools.Size = new Size(1408, 40);
        pnlLineTools.TabIndex = 1;
        pnlLineTools.WrapContents = false;
        // 
        // btnAddItem
        // 
        btnAddItem.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(248), Convert.ToInt32(249), Convert.ToInt32(250));
        btnAddItem.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(Convert.ToInt32(205), Convert.ToInt32(225), Convert.ToInt32(205));
        btnAddItem.FlatStyle = FlatStyle.Flat;
        btnAddItem.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnAddItem.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(30), Convert.ToInt32(41), Convert.ToInt32(59));
        btnAddItem.Location = new Point(1238, 2);
        btnAddItem.Margin = new Padding(0, 0, 8, 0);
        btnAddItem.Name = "btnAddItem";
        btnAddItem.Size = new Size(170, 32);
        btnAddItem.TabIndex = 0;
        btnAddItem.Text = "➕ إضافة صنف";
        btnAddItem.UseVisualStyleBackColor = false;
        btnAddItem.Click += btnAddItem_Click;
        // 
        // btnRemoveItem
        // 
        btnRemoveItem.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(108), Convert.ToInt32(117), Convert.ToInt32(125));
        btnRemoveItem.FlatStyle = FlatStyle.Flat;
        btnRemoveItem.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnRemoveItem.ForeColor = System.Drawing.Color.White;
        btnRemoveItem.Location = new Point(1060, 2);
        btnRemoveItem.Margin = new Padding(0, 0, 8, 0);
        btnRemoveItem.Name = "btnRemoveItem";
        btnRemoveItem.Size = new Size(170, 32);
        btnRemoveItem.TabIndex = 1;
        btnRemoveItem.Text = "🗑 حذف صنف";
        btnRemoveItem.UseVisualStyleBackColor = false;
        btnRemoveItem.Click += btnRemoveItem_Click;
        // 
        // lblTotals
        // 
        lblTotals.BackColor = System.Drawing.Color.White;
        lblTotals.BorderStyle = BorderStyle.FixedSingle;
        lblTotals.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblTotals.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
        lblTotals.Location = new Point(752, 2);
        lblTotals.Margin = new Padding(0, 0, 8, 0);
        lblTotals.Name = "lblTotals";
        lblTotals.Size = new Size(300, 32);
        lblTotals.TabIndex = 2;
        lblTotals.Text = "الإجمالي: 0.00";
        lblTotals.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // dgvItems
        // 
        dgvItems.AllowUserToAddRows = true;
        dgvItems.AllowUserToDeleteRows = false;
        cellStyleItemsAlternate.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(245), Convert.ToInt32(252), Convert.ToInt32(245));
        dgvItems.AlternatingRowsDefaultCellStyle = cellStyleItemsAlternate;
        dgvItems.AutoGenerateColumns = false;
        dgvItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvItems.BackgroundColor = System.Drawing.Color.FromArgb(Convert.ToInt32(220), Convert.ToInt32(240), Convert.ToInt32(220));
        dgvItems.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
        cellStyleItemsHeader.Alignment = DataGridViewContentAlignment.MiddleCenter;
        cellStyleItemsHeader.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
        cellStyleItemsHeader.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        cellStyleItemsHeader.ForeColor = System.Drawing.Color.White;
        cellStyleItemsHeader.SelectionBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
        cellStyleItemsHeader.SelectionForeColor = System.Drawing.Color.White;
        cellStyleItemsHeader.WrapMode = DataGridViewTriState.True;
        dgvItems.ColumnHeadersDefaultCellStyle = cellStyleItemsHeader;
        dgvItems.ColumnHeadersHeight = 45;
        dgvItems.Columns.AddRange(new DataGridViewColumn[] { colBarcode, colProductName, colQuantity, colUnitPrice, colLineTotal });
        cellStyleItemsDefault.Alignment = DataGridViewContentAlignment.MiddleCenter;
        cellStyleItemsDefault.BackColor = System.Drawing.Color.White;
        cellStyleItemsDefault.Font = new Font("Tahoma", 10F);
        cellStyleItemsDefault.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        cellStyleItemsDefault.SelectionBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(255), Convert.ToInt32(255), Convert.ToInt32(180));
        cellStyleItemsDefault.SelectionForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        cellStyleItemsDefault.WrapMode = DataGridViewTriState.False;
        dgvItems.DefaultCellStyle = cellStyleItemsDefault;
        dgvItems.Dock = DockStyle.Fill;
        dgvItems.EnableHeadersVisualStyles = false;
        dgvItems.GridColor = System.Drawing.Color.FromArgb(Convert.ToInt32(205), Convert.ToInt32(225), Convert.ToInt32(205));
        dgvItems.Location = new Point(6, 69);
        dgvItems.MultiSelect = false;
        dgvItems.Name = "dgvItems";
        dgvItems.RowHeadersVisible = false;
        dgvItems.RowHeadersWidth = 62;
        dgvItems.RowTemplate.Height = 32;
        dgvItems.SelectionMode = DataGridViewSelectionMode.CellSelect;
        dgvItems.Size = new Size(1408, 175);
        dgvItems.TabIndex = 0;
        // 
        // colBarcode
        // 
        colBarcode.DataPropertyName = "ProductCode";
        colBarcode.FillWeight = 22F;
        colBarcode.HeaderText = "الكود/الباركود";
        colBarcode.MinimumWidth = 120;
        colBarcode.Name = "colBarcode";
        // 
        // colProductName
        // 
        colProductName.DataPropertyName = "ProductName";
        colProductName.FillWeight = 36F;
        colProductName.HeaderText = "اسم الصنف";
        colProductName.MinimumWidth = 150;
        colProductName.Name = "colProductName";
        colProductName.ReadOnly = true;
        // 
        // colQuantity
        // 
        colQuantity.DataPropertyName = "Quantity";
        colQuantity.FillWeight = 12F;
        colQuantity.HeaderText = "الكمية";
        colQuantity.MinimumWidth = 90;
        colQuantity.Name = "colQuantity";
        // 
        // colUnitPrice
        // 
        colUnitPrice.DataPropertyName = "UnitPrice";
        colUnitPrice.FillWeight = 14F;
        colUnitPrice.HeaderText = "السعر";
        colUnitPrice.MinimumWidth = 100;
        colUnitPrice.Name = "colUnitPrice";
        // 
        // colLineTotal
        // 
        colLineTotal.DataPropertyName = "LineTotal";
        colLineTotal.FillWeight = 16F;
        colLineTotal.HeaderText = "الإجمالي";
        colLineTotal.MinimumWidth = 110;
        colLineTotal.Name = "colLineTotal";
        colLineTotal.ReadOnly = true;
        // 
        // pnlButtons
        // 
        pnlButtons.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
        pnlButtons.Controls.Add(btnNew);
        pnlButtons.Controls.Add(btnSaveDraft);
        pnlButtons.Controls.Add(btnPost);
        pnlButtons.Controls.Add(btnCancelDocument);
        pnlButtons.Controls.Add(btnRefresh);
        pnlButtons.Controls.Add(btnClose);
        pnlButtons.Dock = DockStyle.Top;
        pnlButtons.Location = new Point(0, 460);
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
        btnNew.Location = new Point(1267, 13);
        btnNew.Name = "btnNew";
        btnNew.Size = new Size(142, 35);
        btnNew.TabIndex = 0;
        btnNew.Text = "🆕 جديد";
        btnNew.UseVisualStyleBackColor = false;
        btnNew.Click += btnNew_Click;
        // 
        // btnSaveDraft
        // 
        btnSaveDraft.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(25), Convert.ToInt32(135), Convert.ToInt32(84));
        btnSaveDraft.FlatStyle = FlatStyle.Flat;
        btnSaveDraft.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnSaveDraft.ForeColor = System.Drawing.Color.White;
        btnSaveDraft.Location = new Point(1095, 13);
        btnSaveDraft.Name = "btnSaveDraft";
        btnSaveDraft.Size = new Size(160, 35);
        btnSaveDraft.TabIndex = 1;
        btnSaveDraft.Text = "💾 حفظ مسودة";
        btnSaveDraft.UseVisualStyleBackColor = false;
        btnSaveDraft.Click += btnSaveDraft_Click;
        // 
        // btnPost
        // 
        btnPost.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(23), Convert.ToInt32(162), Convert.ToInt32(184));
        btnPost.FlatStyle = FlatStyle.Flat;
        btnPost.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnPost.ForeColor = System.Drawing.Color.White;
        btnPost.Location = new Point(943, 13);
        btnPost.Name = "btnPost";
        btnPost.Size = new Size(142, 35);
        btnPost.TabIndex = 2;
        btnPost.Text = "📤 ترحيل";
        btnPost.UseVisualStyleBackColor = false;
        btnPost.Click += btnPost_Click;
        // 
        // btnCancelDocument
        // 
        btnCancelDocument.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(220), Convert.ToInt32(53), Convert.ToInt32(69));
        btnCancelDocument.FlatStyle = FlatStyle.Flat;
        btnCancelDocument.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnCancelDocument.ForeColor = System.Drawing.Color.White;
        btnCancelDocument.Location = new Point(759, 13);
        btnCancelDocument.Name = "btnCancelDocument";
        btnCancelDocument.Size = new Size(174, 35);
        btnCancelDocument.TabIndex = 3;
        btnCancelDocument.Text = "🚫 إلغاء المستند";
        btnCancelDocument.UseVisualStyleBackColor = false;
        btnCancelDocument.Click += btnCancelDocument_Click;
        // 
        // btnRefresh
        // 
        btnRefresh.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(23), Convert.ToInt32(162), Convert.ToInt32(184));
        btnRefresh.FlatStyle = FlatStyle.Flat;
        btnRefresh.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnRefresh.ForeColor = System.Drawing.Color.White;
        btnRefresh.Location = new Point(619, 13);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(130, 35);
        btnRefresh.TabIndex = 4;
        btnRefresh.Text = "🔄 تحديث";
        btnRefresh.UseVisualStyleBackColor = false;
        btnRefresh.Click += btnRefresh_Click;
        // 
        // btnClose
        // 
        btnClose.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(248), Convert.ToInt32(249), Convert.ToInt32(250));
        btnClose.FlatStyle = FlatStyle.Flat;
        btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnClose.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(30), Convert.ToInt32(41), Convert.ToInt32(59));
        btnClose.Location = new Point(479, 13);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(130, 35);
        btnClose.TabIndex = 5;
        btnClose.TabStop = false;
        btnClose.Text = "🚪 إغلاق";
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += btnClose_Click;
        // 
        // pnlFilters
        // 
        pnlFilters.Controls.Add(gbFilter);
        pnlFilters.Controls.Add(btnToggleFilters);
        pnlFilters.Dock = DockStyle.Top;
        pnlFilters.Location = new Point(0, 520);
        pnlFilters.Margin = new Padding(0);
        pnlFilters.Name = "pnlFilters";
        pnlFilters.Padding = new Padding(6, 4, 6, 4);
        pnlFilters.Size = new Size(1432, 112);
        pnlFilters.TabIndex = 4;
        // 
        // gbFilter
        // 
        gbFilter.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(245), Convert.ToInt32(235));
        gbFilter.Controls.Add(lblFilterBy);
        gbFilter.Controls.Add(cbFilterBy);
        gbFilter.Controls.Add(txtFilterValue);
        gbFilter.Controls.Add(lblStatusFilter);
        gbFilter.Controls.Add(cbStatus);
        gbFilter.Controls.Add(chkOnlyCancelled);
        gbFilter.Controls.Add(lblFrom);
        gbFilter.Controls.Add(dtpFrom);
        gbFilter.Controls.Add(lblTo);
        gbFilter.Controls.Add(dtpTo);
        gbFilter.Controls.Add(btnRangeToday);
        gbFilter.Controls.Add(btnRangeMonth);
        gbFilter.Controls.Add(btnRange30);
        gbFilter.Controls.Add(btnClearFilters);
        gbFilter.Dock = DockStyle.Fill;
        gbFilter.Font = new Font("Tahoma", 9F);
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
        // lblFilterBy
        // 
        lblFilterBy.AutoSize = true;
        lblFilterBy.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblFilterBy.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblFilterBy.Location = new Point(1196, 30);
        lblFilterBy.Name = "lblFilterBy";
        lblFilterBy.Size = new Size(76, 22);
        lblFilterBy.TabIndex = 0;
        lblFilterBy.Text = "تصفية بـ:";
        // 
        // cbFilterBy
        // 
        cbFilterBy.BackColor = System.Drawing.Color.White;
        cbFilterBy.DropDownStyle = ComboBoxStyle.DropDownList;
        cbFilterBy.FlatStyle = FlatStyle.Flat;
        cbFilterBy.Font = new Font("Tahoma", 9F);
        cbFilterBy.Items.AddRange(new object[] { "العميل", "رقم المرتجع", "البيان", "المبلغ" });
        cbFilterBy.Location = new Point(1040, 28);
        cbFilterBy.Name = "cbFilterBy";
        cbFilterBy.Size = new Size(150, 30);
        cbFilterBy.TabIndex = 1;
        cbFilterBy.SelectedIndexChanged += cbFilterBy_SelectedIndexChanged;
        // 
        // txtFilterValue
        // 
        txtFilterValue.BackColor = System.Drawing.Color.White;
        txtFilterValue.Font = new Font("Tahoma", 9F);
        txtFilterValue.Location = new Point(800, 28);
        txtFilterValue.Name = "txtFilterValue";
        txtFilterValue.PlaceholderText = "اكتب قيمة التصفية...";
        txtFilterValue.RightToLeft = RightToLeft.Yes;
        txtFilterValue.Size = new Size(230, 29);
        txtFilterValue.TabIndex = 2;
        txtFilterValue.TextChanged += txtFilterValue_TextChanged;
        txtFilterValue.KeyDown += txtFilterValue_KeyDown;
        // 
        // lblStatusFilter
        // 
        lblStatusFilter.AutoSize = true;
        lblStatusFilter.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblStatusFilter.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblStatusFilter.Location = new Point(706, 30);
        lblStatusFilter.Name = "lblStatusFilter";
        lblStatusFilter.Size = new Size(60, 22);
        lblStatusFilter.TabIndex = 0;
        lblStatusFilter.Text = "الحالة:";
        // 
        // cbStatus
        // 
        cbStatus.BackColor = System.Drawing.Color.White;
        cbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
        cbStatus.FlatStyle = FlatStyle.Flat;
        cbStatus.Font = new Font("Tahoma", 9F);
        cbStatus.Location = new Point(560, 28);
        cbStatus.Name = "cbStatus";
        cbStatus.Size = new Size(140, 30);
        cbStatus.TabIndex = 3;
        cbStatus.SelectedIndexChanged += cbStatus_SelectedIndexChanged;
        // 
        // chkOnlyCancelled
        // 
        chkOnlyCancelled.AutoSize = true;
        chkOnlyCancelled.FlatStyle = FlatStyle.Flat;
        chkOnlyCancelled.Font = new Font("Tahoma", 9F);
        chkOnlyCancelled.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        chkOnlyCancelled.Location = new Point(380, 30);
        chkOnlyCancelled.Name = "chkOnlyCancelled";
        chkOnlyCancelled.Size = new Size(150, 26);
        chkOnlyCancelled.TabIndex = 4;
        chkOnlyCancelled.Text = "الملغاة فقط";
        chkOnlyCancelled.CheckedChanged += chkOnlyCancelled_CheckedChanged;
        // 
        // lblFrom
        // 
        lblFrom.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblFrom.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblFrom.Location = new Point(1230, 68);
        lblFrom.Name = "lblFrom";
        lblFrom.Size = new Size(48, 26);
        lblFrom.TabIndex = 0;
        lblFrom.Text = "من:";
        lblFrom.TextAlign = ContentAlignment.MiddleRight;
        // 
        // dtpFrom
        // 
        dtpFrom.BackColor = System.Drawing.Color.White;
        dtpFrom.Font = new Font("Tahoma", 9F);
        dtpFrom.Format = DateTimePickerFormat.Short;
        dtpFrom.Location = new Point(1096, 66);
        dtpFrom.Name = "dtpFrom";
        dtpFrom.Size = new Size(128, 29);
        dtpFrom.TabIndex = 5;
        dtpFrom.ValueChanged += dtpFrom_ValueChanged;
        // 
        // lblTo
        // 
        lblTo.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblTo.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblTo.Location = new Point(1048, 68);
        lblTo.Name = "lblTo";
        lblTo.Size = new Size(48, 26);
        lblTo.TabIndex = 0;
        lblTo.Text = "إلى:";
        lblTo.TextAlign = ContentAlignment.MiddleRight;
        // 
        // dtpTo
        // 
        dtpTo.BackColor = System.Drawing.Color.White;
        dtpTo.Font = new Font("Tahoma", 9F);
        dtpTo.Format = DateTimePickerFormat.Short;
        dtpTo.Location = new Point(916, 66);
        dtpTo.Name = "dtpTo";
        dtpTo.Size = new Size(128, 29);
        dtpTo.TabIndex = 6;
        dtpTo.ValueChanged += dtpTo_ValueChanged;
        // 
        // btnRangeToday
        // 
        btnRangeToday.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(248), Convert.ToInt32(249), Convert.ToInt32(250));
        btnRangeToday.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(Convert.ToInt32(205), Convert.ToInt32(225), Convert.ToInt32(205));
        btnRangeToday.FlatStyle = FlatStyle.Flat;
        btnRangeToday.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        btnRangeToday.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(30), Convert.ToInt32(41), Convert.ToInt32(59));
        btnRangeToday.Location = new Point(806, 66);
        btnRangeToday.Name = "btnRangeToday";
        btnRangeToday.Size = new Size(100, 30);
        btnRangeToday.TabIndex = 7;
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
        btnRangeMonth.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(30), Convert.ToInt32(41), Convert.ToInt32(59));
        btnRangeMonth.Location = new Point(686, 66);
        btnRangeMonth.Name = "btnRangeMonth";
        btnRangeMonth.Size = new Size(110, 30);
        btnRangeMonth.TabIndex = 8;
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
        btnRange30.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(30), Convert.ToInt32(41), Convert.ToInt32(59));
        btnRange30.Location = new Point(551, 66);
        btnRange30.Name = "btnRange30";
        btnRange30.Size = new Size(125, 30);
        btnRange30.TabIndex = 9;
        btnRange30.Text = "آخر 30 يوماً";
        btnRange30.UseVisualStyleBackColor = false;
        btnRange30.Click += btnRange30_Click;
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
        btnClearFilters.Location = new Point(340, 66);
        btnClearFilters.Name = "btnClearFilters";
        btnClearFilters.Size = new Size(200, 32);
        btnClearFilters.TabIndex = 10;
        btnClearFilters.Text = "\U0001f9f9 مسح التصفية";
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
        btnToggleFilters.FlatStyle = FlatStyle.Popup;
        btnToggleFilters.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        btnToggleFilters.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
        btnToggleFilters.Location = new Point(6, 4);
        btnToggleFilters.Name = "btnToggleFilters";
        btnToggleFilters.Size = new Size(140, 104);
        btnToggleFilters.TabIndex = 11;
        btnToggleFilters.Text = "إخفاء التصفية";
        btnToggleFilters.UseVisualStyleBackColor = false;
        // 
        // pnlGrid
        // 
        pnlGrid.Controls.Add(dgvMain);
        pnlGrid.Controls.Add(lblEmpty);
        pnlGrid.Dock = DockStyle.Fill;
        pnlGrid.Location = new Point(0, 632);
        pnlGrid.Margin = new Padding(0);
        pnlGrid.Name = "pnlGrid";
        pnlGrid.Padding = new Padding(0, 10, 0, 0);
        pnlGrid.Size = new Size(1432, 236);
        pnlGrid.TabIndex = 5;
        // 
        // dgvMain
        // 
        dgvMain.AllowUserToAddRows = false;
        dgvMain.AllowUserToDeleteRows = false;
        cellStyleMainAlternate.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(245), Convert.ToInt32(252), Convert.ToInt32(245));
        dgvMain.AlternatingRowsDefaultCellStyle = cellStyleMainAlternate;
        dgvMain.AutoGenerateColumns = false;
        dgvMain.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvMain.BackgroundColor = System.Drawing.Color.FromArgb(Convert.ToInt32(220), Convert.ToInt32(240), Convert.ToInt32(220));
        dgvMain.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
        cellStyleMainHeader.Alignment = DataGridViewContentAlignment.MiddleLeft;
        cellStyleMainHeader.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
        cellStyleMainHeader.Font = new Font("Tahoma", 10F);
        cellStyleMainHeader.ForeColor = System.Drawing.Color.White;
        cellStyleMainHeader.SelectionBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
        cellStyleMainHeader.SelectionForeColor = System.Drawing.Color.White;
        cellStyleMainHeader.WrapMode = DataGridViewTriState.True;
        dgvMain.ColumnHeadersDefaultCellStyle = cellStyleMainHeader;
        dgvMain.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvMain.Columns.AddRange(new DataGridViewColumn[] { colId, colReturnNo, colReturnDate, colCustomerName, colTotalAmount, colStatusName });
        dgvMain.ContextMenuStrip = cmsRows;
        cellStyleMainDefault.Alignment = DataGridViewContentAlignment.MiddleLeft;
        cellStyleMainDefault.BackColor = System.Drawing.Color.White;
        cellStyleMainDefault.Font = new Font("Tahoma", 10F);
        cellStyleMainDefault.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        cellStyleMainDefault.SelectionBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(255), Convert.ToInt32(255), Convert.ToInt32(180));
        cellStyleMainDefault.SelectionForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        cellStyleMainDefault.WrapMode = DataGridViewTriState.False;
        dgvMain.DefaultCellStyle = cellStyleMainDefault;
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
        dgvMain.Size = new Size(1432, 226);
        dgvMain.TabIndex = 0;
        dgvMain.CellClick += dgvMain_CellClick;
        dgvMain.DoubleClick += dgvMain_DoubleClick;
        // 
        // colId
        // 
        colId.DataPropertyName = "Id";
        colId.HeaderText = "الرقم";
        colId.MinimumWidth = 60;
        colId.Name = "colId";
        colId.ReadOnly = true;
        colId.Visible = false;
        colId.Width = 60;
        // 
        // colReturnNo
        // 
        colReturnNo.DataPropertyName = "ReturnNo";
        colReturnNo.FillWeight = 12F;
        colReturnNo.HeaderText = "رقم المرتجع";
        colReturnNo.MinimumWidth = 100;
        colReturnNo.Name = "colReturnNo";
        colReturnNo.ReadOnly = true;
        // 
        // colReturnDate
        // 
        colReturnDate.DataPropertyName = "ReturnDateText";
        colReturnDate.FillWeight = 14F;
        colReturnDate.HeaderText = "التاريخ";
        colReturnDate.MinimumWidth = 110;
        colReturnDate.Name = "colReturnDate";
        colReturnDate.ReadOnly = true;
        // 
        // colCustomerName
        // 
        colCustomerName.DataPropertyName = "CustomerName";
        colCustomerName.FillWeight = 32F;
        colCustomerName.HeaderText = "العميل";
        colCustomerName.MinimumWidth = 150;
        colCustomerName.Name = "colCustomerName";
        colCustomerName.ReadOnly = true;
        // 
        // colTotalAmount
        // 
        colTotalAmount.DataPropertyName = "TotalAmountText";
        colTotalAmount.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        colTotalAmount.FillWeight = 14F;
        colTotalAmount.HeaderText = "الإجمالي";
        colTotalAmount.MinimumWidth = 110;
        colTotalAmount.Name = "colTotalAmount";
        colTotalAmount.ReadOnly = true;
        // 
        // colStatusName
        // 
        colStatusName.DataPropertyName = "StatusText";
        colStatusName.FillWeight = 12F;
        colStatusName.HeaderText = "الحالة";
        colStatusName.MinimumWidth = 100;
        colStatusName.Name = "colStatusName";
        colStatusName.ReadOnly = true;
        // 
        // lblEmpty
        // 
        lblEmpty.BackColor = System.Drawing.Color.Transparent;
        lblEmpty.Dock = DockStyle.Fill;
        lblEmpty.Font = new Font("Tahoma", 12F, FontStyle.Bold);
        lblEmpty.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblEmpty.Location = new Point(0, 10);
        lblEmpty.Name = "lblEmpty";
        lblEmpty.Size = new Size(1432, 226);
        lblEmpty.TabIndex = 1;
        lblEmpty.Text = "لا توجد بيانات للعرض";
        lblEmpty.TextAlign = ContentAlignment.MiddleCenter;
        lblEmpty.Visible = false;
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
        lblSummary.Text = "المعروض: 0 من 0 — الإجمالي: 0.00";
        lblSummary.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // cmsRows
        // 
        // RULE-N23: قائمة سياق على الشبكة — كل عنصر يستدعي نفس زر الشاشة (بلا منطق مكرر).
        cmsRows.Items.AddRange(new ToolStripItem[] { cmsView, cmsPost, cmsCancel, cmsSeparator, cmsRefresh });
        cmsRows.Name = "cmsRows";
        cmsRows.RightToLeft = RightToLeft.Yes;
        cmsRows.Size = new Size(240, 134);
        cmsRows.Opening += cmsRows_Opening;
        // 
        // cmsView
        // 
        cmsView.Name = "cmsView";
        cmsView.Size = new Size(239, 30);
        cmsView.Text = "👁 عرض في المحرر";
        cmsView.Click += cmsView_Click;
        // 
        // cmsPost
        // 
        cmsPost.Name = "cmsPost";
        cmsPost.Size = new Size(239, 30);
        cmsPost.Text = "📤 ترحيل";
        cmsPost.Click += cmsPost_Click;
        // 
        // cmsCancel
        // 
        cmsCancel.Name = "cmsCancel";
        cmsCancel.Size = new Size(239, 30);
        cmsCancel.Text = "🚫 إلغاء المستند";
        cmsCancel.Click += cmsCancel_Click;
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
        // toolTip
        // 
        toolTip.AutomaticDelay = 300;
        toolTip.BackColor = System.Drawing.Color.White;
        toolTip.ForeColor = System.Drawing.Color.Black;
        // 
        // errorProvider
        // 
        errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        errorProvider.ContainerControl = this;
        errorProvider.RightToLeft = true;
        // 
        // frmSalesReturnMaster
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
        Name = "frmSalesReturnMaster";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterParent;
        Text = "مرتجعات المبيعات";
        Load += frmSalesReturnMaster_Load;
        cmsRows.ResumeLayout(false);
        pnlFields.ResumeLayout(false);
        gbDocument.ResumeLayout(false);
        gbDocument.PerformLayout();
        pnlLines.ResumeLayout(false);
        gbLines.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvItems).EndInit();
        pnlLineTools.ResumeLayout(false);
        pnlButtons.ResumeLayout(false);
        pnlFilters.ResumeLayout(false);
        gbFilter.ResumeLayout(false);
        gbFilter.PerformLayout();
        pnlGrid.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvMain).EndInit();
        pnlSummary.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private System.Windows.Forms.Label lblMode;
    private System.Windows.Forms.Panel pnlFields;
    private System.Windows.Forms.GroupBox gbDocument;
    private System.Windows.Forms.Label lblReturnNo;
    private System.Windows.Forms.TextBox txtReturnNo;
    private System.Windows.Forms.Label lblDate;
    private System.Windows.Forms.DateTimePicker dtpDate;
    private System.Windows.Forms.Label lblCustomer;
    private System.Windows.Forms.Label lblCustomerReq;
    private System.Windows.Forms.ComboBox cmbCustomer;
    private System.Windows.Forms.Label lblWarehouse;
    private System.Windows.Forms.Label lblWarehouseReq;
    private System.Windows.Forms.ComboBox cmbWarehouse;
    private System.Windows.Forms.Label lblStatusCaption;
    private System.Windows.Forms.Label lblStatusValue;
    private System.Windows.Forms.Label lblNotes;
    private System.Windows.Forms.TextBox txtNotes;
    private System.Windows.Forms.Panel pnlLines;
    private System.Windows.Forms.GroupBox gbLines;
    private System.Windows.Forms.DataGridView dgvItems;
    private System.Windows.Forms.DataGridViewTextBoxColumn colBarcode;
    private System.Windows.Forms.DataGridViewTextBoxColumn colProductName;
    private System.Windows.Forms.DataGridViewTextBoxColumn colQuantity;
    private System.Windows.Forms.DataGridViewTextBoxColumn colUnitPrice;
    private System.Windows.Forms.DataGridViewTextBoxColumn colLineTotal;
    private System.Windows.Forms.FlowLayoutPanel pnlLineTools;
    private System.Windows.Forms.Button btnAddItem;
    private System.Windows.Forms.Button btnRemoveItem;
    private System.Windows.Forms.Label lblTotals;
    private System.Windows.Forms.FlowLayoutPanel pnlButtons;
    private System.Windows.Forms.Button btnNew;
    private System.Windows.Forms.Button btnSaveDraft;
    private System.Windows.Forms.Button btnPost;
    private System.Windows.Forms.Button btnCancelDocument;
    private System.Windows.Forms.Button btnRefresh;
    private System.Windows.Forms.Button btnClose;
    private System.Windows.Forms.Panel pnlFilters;
    private System.Windows.Forms.GroupBox gbFilter;
    private System.Windows.Forms.Label lblFilterBy;
    private System.Windows.Forms.ComboBox cbFilterBy;
    private System.Windows.Forms.TextBox txtFilterValue;
    private System.Windows.Forms.Label lblStatusFilter;
    private System.Windows.Forms.ComboBox cbStatus;
    private System.Windows.Forms.CheckBox chkOnlyCancelled;
    private System.Windows.Forms.Label lblFrom;
    private System.Windows.Forms.DateTimePicker dtpFrom;
    private System.Windows.Forms.Label lblTo;
    private System.Windows.Forms.DateTimePicker dtpTo;
    private System.Windows.Forms.Button btnRangeToday;
    private System.Windows.Forms.Button btnRangeMonth;
    private System.Windows.Forms.Button btnRange30;
    private System.Windows.Forms.Button btnClearFilters;
    private System.Windows.Forms.Button btnToggleFilters;
    private System.Windows.Forms.Panel pnlGrid;
    private System.Windows.Forms.DataGridView dgvMain;
    private System.Windows.Forms.DataGridViewTextBoxColumn colId;
    private System.Windows.Forms.DataGridViewTextBoxColumn colReturnNo;
    private System.Windows.Forms.DataGridViewTextBoxColumn colReturnDate;
    private System.Windows.Forms.DataGridViewTextBoxColumn colCustomerName;
    private System.Windows.Forms.DataGridViewTextBoxColumn colTotalAmount;
    private System.Windows.Forms.DataGridViewTextBoxColumn colStatusName;
    private System.Windows.Forms.Label lblEmpty;
    private System.Windows.Forms.Panel pnlSummary;
    private System.Windows.Forms.Label lblSummary;
    private System.Windows.Forms.ContextMenuStrip cmsRows;
    private System.Windows.Forms.ToolStripMenuItem cmsView;
    private System.Windows.Forms.ToolStripMenuItem cmsPost;
    private System.Windows.Forms.ToolStripMenuItem cmsCancel;
    private System.Windows.Forms.ToolStripSeparator cmsSeparator;
    private System.Windows.Forms.ToolStripMenuItem cmsRefresh;
    private System.Windows.Forms.ToolTip toolTip;
    private System.Windows.Forms.ErrorProvider errorProvider;
}
