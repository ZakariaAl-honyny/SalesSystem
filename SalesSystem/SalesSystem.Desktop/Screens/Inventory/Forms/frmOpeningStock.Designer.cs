namespace SalesSystem.Desktop.Screens.Inventory.Forms;

partial class frmOpeningStock
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
        DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
        errorProvider = new ErrorProvider(components);
        tooltip = new ToolTip(components);
        btnAddProduct = new Button();
        btnRemoveProduct = new Button();
        btnSave = new Button();
        btnCancel = new Button();
        cmbWarehouse = new ComboBox();
        dtpDate = new DateTimePicker();
        dgvLines = new DataGridView();
        cmsLines = new ContextMenuStrip(components);
        miAddProduct = new ToolStripMenuItem();
        miRemoveLine = new ToolStripMenuItem();
        txtTotalCost = new TextBox();
        liveTip = new ToolTip(components);
        lblMode = new Label();
        gbToolbar = new GroupBox();
        gbInfo = new GroupBox();
        lblDate = new Label();
        lblWarehouse = new Label();
        gbItems = new GroupBox();
        lblProductsHeader = new Label();
        gbTotals = new GroupBox();
        lblTotalCostCaption = new Label();
        lblItemCountValue = new Label();
        lblItemCountCaption = new Label();
        ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvLines).BeginInit();
        cmsLines.SuspendLayout();
        gbToolbar.SuspendLayout();
        gbInfo.SuspendLayout();
        gbItems.SuspendLayout();
        gbTotals.SuspendLayout();
        SuspendLayout();
        // 
        // errorProvider
        // 
        errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        errorProvider.ContainerControl = this;
        // 
        // tooltip
        // 
        tooltip.AutoPopDelay = 6000;
        tooltip.InitialDelay = 300;
        tooltip.ReshowDelay = 100;
        // 
        // btnAddProduct
        // 
        btnAddProduct.BackColor = Color.FromArgb(13, 110, 253);
        btnAddProduct.Cursor = Cursors.Hand;
        btnAddProduct.FlatAppearance.BorderSize = 0;
        btnAddProduct.FlatStyle = FlatStyle.Flat;
        btnAddProduct.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnAddProduct.ForeColor = Color.White;
        btnAddProduct.Location = new Point(819, 26);
        btnAddProduct.MinimumSize = new Size(100, 34);
        btnAddProduct.Name = "btnAddProduct";
        btnAddProduct.Size = new Size(170, 36);
        btnAddProduct.TabIndex = 0;
        btnAddProduct.Text = "+ إضافة منتج";
        tooltip.SetToolTip(btnAddProduct, "إضافة منتج جديد للقائمة (F2)");
        btnAddProduct.UseVisualStyleBackColor = false;
        btnAddProduct.Click += btnAddProduct_Click;
        // 
        // btnRemoveProduct
        // 
        btnRemoveProduct.BackColor = Color.FromArgb(220, 53, 69);
        btnRemoveProduct.Cursor = Cursors.Hand;
        btnRemoveProduct.FlatAppearance.BorderSize = 0;
        btnRemoveProduct.FlatStyle = FlatStyle.Flat;
        btnRemoveProduct.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnRemoveProduct.ForeColor = Color.White;
        btnRemoveProduct.Location = new Point(673, 26);
        btnRemoveProduct.MinimumSize = new Size(100, 34);
        btnRemoveProduct.Name = "btnRemoveProduct";
        btnRemoveProduct.Size = new Size(140, 36);
        btnRemoveProduct.TabIndex = 1;
        btnRemoveProduct.Text = "حذف السطر";
        tooltip.SetToolTip(btnRemoveProduct, "حذف المنتج المحدد من القائمة");
        btnRemoveProduct.UseVisualStyleBackColor = false;
        btnRemoveProduct.Click += btnRemoveProduct_Click;
        // 
        // btnSave
        // 
        btnSave.BackColor = Color.FromArgb(25, 135, 84);
        btnSave.Cursor = Cursors.Hand;
        btnSave.FlatAppearance.BorderSize = 0;
        btnSave.FlatStyle = FlatStyle.Flat;
        btnSave.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnSave.ForeColor = Color.White;
        btnSave.Location = new Point(517, 26);
        btnSave.MinimumSize = new Size(100, 34);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(150, 36);
        btnSave.TabIndex = 2;
        btnSave.Text = "حفظ وترحيل";
        tooltip.SetToolTip(btnSave, "حفظ وترحيل الأرصدة الافتتاحية (F4)");
        btnSave.UseVisualStyleBackColor = false;
        btnSave.Click += btnSave_Click;
        // 
        // btnCancel
        // 
        btnCancel.BackColor = Color.FromArgb(108, 117, 125);
        btnCancel.Cursor = Cursors.Hand;
        btnCancel.DialogResult = DialogResult.Cancel;
        btnCancel.FlatAppearance.BorderSize = 0;
        btnCancel.FlatStyle = FlatStyle.Flat;
        btnCancel.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnCancel.ForeColor = Color.White;
        btnCancel.Location = new Point(371, 26);
        btnCancel.MinimumSize = new Size(100, 34);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(140, 36);
        btnCancel.TabIndex = 3;
        btnCancel.Text = "إلغاء";
        tooltip.SetToolTip(btnCancel, "إلغاء وإغلاق النافذة (Esc)");
        btnCancel.UseVisualStyleBackColor = false;
        btnCancel.Click += btnCancel_Click;
        // 
        // cmbWarehouse
        // 
        cmbWarehouse.BackColor = Color.White;
        cmbWarehouse.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbWarehouse.FlatStyle = FlatStyle.Flat;
        cmbWarehouse.Font = new Font("Tahoma", 10F);
        cmbWarehouse.ForeColor = Color.FromArgb(33, 37, 41);
        cmbWarehouse.FormattingEnabled = true;
        cmbWarehouse.Location = new Point(110, 26);
        cmbWarehouse.Name = "cmbWarehouse";
        cmbWarehouse.Size = new Size(280, 32);
        cmbWarehouse.TabIndex = 1;
        tooltip.SetToolTip(cmbWarehouse, "اختر المخزن الذي ستضاف إليه الأرصدة");
        // 
        // dtpDate
        // 
        dtpDate.Font = new Font("Tahoma", 10F);
        dtpDate.Format = DateTimePickerFormat.Short;
        dtpDate.Location = new Point(505, 26);
        dtpDate.Name = "dtpDate";
        dtpDate.RightToLeft = RightToLeft.Yes;
        dtpDate.Size = new Size(280, 32);
        dtpDate.TabIndex = 3;
        tooltip.SetToolTip(dtpDate, "تاريخ الجرد الافتتاحي");
        // 
        // dgvLines
        // 
        dgvLines.AllowUserToAddRows = false;
        dgvLines.AllowUserToDeleteRows = false;
        dataGridViewCellStyle4.BackColor = Color.FromArgb(248, 249, 250);
        dgvLines.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
        dgvLines.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvLines.BackgroundColor = Color.White;
        dgvLines.BorderStyle = BorderStyle.None;
        dgvLines.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
        dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleCenter;
        dataGridViewCellStyle5.BackColor = Color.FromArgb(52, 58, 64);
        dataGridViewCellStyle5.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        dataGridViewCellStyle5.ForeColor = Color.White;
        dataGridViewCellStyle5.SelectionBackColor = Color.FromArgb(52, 58, 64);
        dataGridViewCellStyle5.SelectionForeColor = Color.White;
        dataGridViewCellStyle5.WrapMode = DataGridViewTriState.True;
        dgvLines.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
        dgvLines.ColumnHeadersHeight = 42;
        dgvLines.ContextMenuStrip = cmsLines;
        dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle6.BackColor = Color.White;
        dataGridViewCellStyle6.Font = new Font("Tahoma", 10F);
        dataGridViewCellStyle6.ForeColor = Color.FromArgb(33, 37, 41);
        dataGridViewCellStyle6.SelectionBackColor = Color.FromArgb(255, 243, 205);
        dataGridViewCellStyle6.SelectionForeColor = Color.FromArgb(33, 37, 41);
        dataGridViewCellStyle6.WrapMode = DataGridViewTriState.False;
        dgvLines.DefaultCellStyle = dataGridViewCellStyle6;
        dgvLines.Dock = DockStyle.Fill;
        dgvLines.EnableHeadersVisualStyles = false;
        dgvLines.GridColor = Color.FromArgb(222, 226, 230);
        dgvLines.Location = new Point(8, 59);
        dgvLines.Name = "dgvLines";
        dgvLines.RowHeadersVisible = false;
        dgvLines.RowHeadersWidth = 62;
        dgvLines.RowTemplate.Height = 30;
        dgvLines.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvLines.Size = new Size(984, 325);
        dgvLines.TabIndex = 1;
        tooltip.SetToolTip(dgvLines, "أدخل الكميات وتكاليف الوحدة للمنتجات. اختر الوحدة المناسبة من القائمة المنسدلة.");
        dgvLines.CellValueChanged += dgvLines_CellValueChanged;
        dgvLines.CurrentCellDirtyStateChanged += dgvLines_CurrentCellDirtyStateChanged;
        dgvLines.EditingControlShowing += dgvLines_EditingControlShowing;
        // 
        // cmsLines
        // 
        cmsLines.Font = new Font("Tahoma", 10F);
        cmsLines.ImageScalingSize = new Size(24, 24);
        cmsLines.Items.AddRange(new ToolStripItem[] { miAddProduct, miRemoveLine });
        cmsLines.Name = "cmsLines";
        cmsLines.RightToLeft = RightToLeft.Yes;
        cmsLines.Size = new Size(245, 64);
        // 
        // miAddProduct
        // 
        miAddProduct.Name = "miAddProduct";
        miAddProduct.Size = new Size(244, 30);
        miAddProduct.Text = "إضافة منتج (F2)";
        miAddProduct.Click += miAddProduct_Click;
        // 
        // miRemoveLine
        // 
        miRemoveLine.Name = "miRemoveLine";
        miRemoveLine.Size = new Size(244, 30);
        miRemoveLine.Text = "حذف السطر المحدد";
        miRemoveLine.Click += miRemoveLine_Click;
        // 
        // txtTotalCost
        // 
        txtTotalCost.BackColor = Color.White;
        txtTotalCost.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        txtTotalCost.ForeColor = Color.FromArgb(33, 37, 41);
        txtTotalCost.Location = new Point(466, 26);
        txtTotalCost.Name = "txtTotalCost";
        txtTotalCost.ReadOnly = true;
        txtTotalCost.Size = new Size(200, 32);
        txtTotalCost.TabIndex = 3;
        txtTotalCost.Text = "0.00";
        txtTotalCost.TextAlign = HorizontalAlignment.Center;
        tooltip.SetToolTip(txtTotalCost, "إجمالي تكلفة الأرصدة الافتتاحية (اقترب بالمؤشر لعرض التفقيط)");
        // 
        // liveTip
        // 
        liveTip.AutoPopDelay = 4000;
        liveTip.InitialDelay = 200;
        liveTip.ReshowDelay = 100;
        // 
        // lblMode
        // 
        lblMode.BackColor = Color.FromArgb(16, 150, 96);
        lblMode.Dock = DockStyle.Top;
        lblMode.FlatStyle = FlatStyle.Flat;
        lblMode.Font = new Font("Tahoma", 14F, FontStyle.Bold);
        lblMode.ForeColor = Color.White;
        lblMode.Location = new Point(0, 0);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(1000, 42);
        lblMode.TabIndex = 0;
        lblMode.Text = "بضاعة أول المدة";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // gbToolbar
        // 
        gbToolbar.BackColor = Color.White;
        gbToolbar.Controls.Add(btnCancel);
        gbToolbar.Controls.Add(btnSave);
        gbToolbar.Controls.Add(btnRemoveProduct);
        gbToolbar.Controls.Add(btnAddProduct);
        gbToolbar.Dock = DockStyle.Top;
        gbToolbar.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        gbToolbar.ForeColor = Color.FromArgb(33, 37, 41);
        gbToolbar.Location = new Point(0, 42);
        gbToolbar.Name = "gbToolbar";
        gbToolbar.Padding = new Padding(8);
        gbToolbar.RightToLeft = RightToLeft.Yes;
        gbToolbar.Size = new Size(1000, 68);
        gbToolbar.TabIndex = 1;
        gbToolbar.TabStop = false;
        gbToolbar.Text = "أوامر المستند";
        // 
        // gbInfo
        // 
        gbInfo.BackColor = Color.White;
        gbInfo.Controls.Add(dtpDate);
        gbInfo.Controls.Add(lblDate);
        gbInfo.Controls.Add(cmbWarehouse);
        gbInfo.Controls.Add(lblWarehouse);
        gbInfo.Dock = DockStyle.Top;
        gbInfo.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        gbInfo.ForeColor = Color.FromArgb(33, 37, 41);
        gbInfo.Location = new Point(0, 110);
        gbInfo.Name = "gbInfo";
        gbInfo.Padding = new Padding(8);
        gbInfo.RightToLeft = RightToLeft.Yes;
        gbInfo.Size = new Size(1000, 86);
        gbInfo.TabIndex = 2;
        gbInfo.TabStop = false;
        gbInfo.Text = "بيانات المستند";
        // 
        // lblDate
        // 
        lblDate.AutoSize = true;
        lblDate.Font = new Font("Tahoma", 10F);
        lblDate.ForeColor = Color.FromArgb(33, 37, 41);
        lblDate.Location = new Point(791, 30);
        lblDate.Name = "lblDate";
        lblDate.Size = new Size(68, 24);
        lblDate.TabIndex = 2;
        lblDate.Text = "التاريخ:";
        // 
        // lblWarehouse
        // 
        lblWarehouse.AutoSize = true;
        lblWarehouse.Font = new Font("Tahoma", 10F);
        lblWarehouse.ForeColor = Color.FromArgb(33, 37, 41);
        lblWarehouse.Location = new Point(396, 30);
        lblWarehouse.Name = "lblWarehouse";
        lblWarehouse.Size = new Size(75, 24);
        lblWarehouse.TabIndex = 0;
        lblWarehouse.Text = "المخزن:";
        // 
        // gbItems
        // 
        gbItems.BackColor = Color.White;
        gbItems.Controls.Add(dgvLines);
        gbItems.Controls.Add(lblProductsHeader);
        gbItems.Dock = DockStyle.Fill;
        gbItems.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        gbItems.ForeColor = Color.FromArgb(33, 37, 41);
        gbItems.Location = new Point(0, 196);
        gbItems.Name = "gbItems";
        gbItems.Padding = new Padding(8);
        gbItems.RightToLeft = RightToLeft.Yes;
        gbItems.Size = new Size(1000, 392);
        gbItems.TabIndex = 3;
        gbItems.TabStop = false;
        gbItems.Text = "الأصناف";
        // 
        // lblProductsHeader
        // 
        lblProductsHeader.Dock = DockStyle.Top;
        lblProductsHeader.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblProductsHeader.ForeColor = Color.FromArgb(33, 37, 41);
        lblProductsHeader.Location = new Point(8, 33);
        lblProductsHeader.Name = "lblProductsHeader";
        lblProductsHeader.Size = new Size(984, 26);
        lblProductsHeader.TabIndex = 0;
        lblProductsHeader.Text = "أرصدة المنتجات — أدخل الكمية وسعر التكلفة لكل صنف، والوحدة من القائمة المنسدلة";
        lblProductsHeader.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // gbTotals
        // 
        gbTotals.BackColor = Color.White;
        gbTotals.Controls.Add(txtTotalCost);
        gbTotals.Controls.Add(lblTotalCostCaption);
        gbTotals.Controls.Add(lblItemCountValue);
        gbTotals.Controls.Add(lblItemCountCaption);
        gbTotals.Dock = DockStyle.Bottom;
        gbTotals.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        gbTotals.ForeColor = Color.FromArgb(33, 37, 41);
        gbTotals.Location = new Point(0, 588);
        gbTotals.Name = "gbTotals";
        gbTotals.Padding = new Padding(8);
        gbTotals.RightToLeft = RightToLeft.Yes;
        gbTotals.Size = new Size(1000, 72);
        gbTotals.TabIndex = 4;
        gbTotals.TabStop = false;
        gbTotals.Text = "الإجماليات";
        // 
        // lblTotalCostCaption
        // 
        lblTotalCostCaption.AutoSize = true;
        lblTotalCostCaption.Font = new Font("Tahoma", 10F);
        lblTotalCostCaption.ForeColor = Color.FromArgb(33, 37, 41);
        lblTotalCostCaption.Location = new Point(671, 30);
        lblTotalCostCaption.Name = "lblTotalCostCaption";
        lblTotalCostCaption.Size = new Size(142, 24);
        lblTotalCostCaption.TabIndex = 2;
        lblTotalCostCaption.Text = "إجمالي التكلفة:";
        // 
        // lblItemCountValue
        // 
        lblItemCountValue.AutoSize = true;
        lblItemCountValue.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblItemCountValue.ForeColor = Color.FromArgb(33, 37, 41);
        lblItemCountValue.Location = new Point(258, 30);
        lblItemCountValue.Name = "lblItemCountValue";
        lblItemCountValue.Size = new Size(23, 24);
        lblItemCountValue.TabIndex = 1;
        lblItemCountValue.Text = "0";
        // 
        // lblItemCountCaption
        // 
        lblItemCountCaption.AutoSize = true;
        lblItemCountCaption.Font = new Font("Tahoma", 10F);
        lblItemCountCaption.ForeColor = Color.FromArgb(33, 37, 41);
        lblItemCountCaption.Location = new Point(287, 30);
        lblItemCountCaption.Name = "lblItemCountCaption";
        lblItemCountCaption.Size = new Size(117, 24);
        lblItemCountCaption.TabIndex = 0;
        lblItemCountCaption.Text = "عدد الأصناف:";
        // 
        // frmOpeningStock
        // 
        AcceptButton = btnSave;
        AutoScaleMode = AutoScaleMode.None;
        AutoValidate = AutoValidate.EnableAllowFocusChange;
        BackColor = Color.FromArgb(240, 242, 245);
        CancelButton = btnCancel;
        ClientSize = new Size(1000, 660);
        Controls.Add(gbItems);
        Controls.Add(gbTotals);
        Controls.Add(gbInfo);
        Controls.Add(gbToolbar);
        Controls.Add(lblMode);
        Font = new Font("Tahoma", 12F);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "frmOpeningStock";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "بضاعة أول المدة";
        Load += frmOpeningStock_Load;
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvLines).EndInit();
        cmsLines.ResumeLayout(false);
        gbToolbar.ResumeLayout(false);
        gbInfo.ResumeLayout(false);
        gbInfo.PerformLayout();
        gbItems.ResumeLayout(false);
        gbTotals.ResumeLayout(false);
        gbTotals.PerformLayout();
        ResumeLayout(false);
    }

    private ErrorProvider errorProvider;
    private ToolTip tooltip;
    private ToolTip liveTip;
    private ContextMenuStrip cmsLines;
    private ToolStripMenuItem miAddProduct;
    private ToolStripMenuItem miRemoveLine;
    private Label lblMode;
    private GroupBox gbToolbar;
    private Button btnAddProduct;
    private Button btnRemoveProduct;
    private Button btnSave;
    private Button btnCancel;
    private GroupBox gbInfo;
    private Label lblWarehouse;
    private ComboBox cmbWarehouse;
    private Label lblDate;
    private DateTimePicker dtpDate;
    private GroupBox gbItems;
    private Label lblProductsHeader;
    private DataGridView dgvLines;
    private DataGridViewTextBoxColumn colProductName;
    private DataGridViewComboBoxColumn colUnit;
    private DataGridViewTextBoxColumn colQuantity;
    private DataGridViewTextBoxColumn colUnitCost;
    private DataGridViewTextBoxColumn colTotalCost;
    private GroupBox gbTotals;
    private Label lblItemCountCaption;
    private Label lblItemCountValue;
    private Label lblTotalCostCaption;
    private TextBox txtTotalCost;
}
