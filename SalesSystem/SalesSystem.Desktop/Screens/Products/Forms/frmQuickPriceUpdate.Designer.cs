namespace SalesSystem.Desktop.Products.Forms;

partial class frmQuickPriceUpdate
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
        lblMode = new Label();
        pnlBulk = new Panel();
        flowBulk = new FlowLayoutPanel();
        lblOperation = new Label();
        cmbOperation = new ComboBox();
        lblValue = new Label();
        numValue = new NumericUpDown();
        btnApplySelected = new Button();
        btnApplyAll = new Button();
        lblHint = new Label();
        pnlGrid = new Panel();
        dgvPrices = new DataGridView();
        colCode = new DataGridViewTextBoxColumn();
        colName = new DataGridViewTextBoxColumn();
        colUnit = new DataGridViewTextBoxColumn();
        colCurrentPrice = new DataGridViewTextBoxColumn();
        colNewPrice = new DataGridViewTextBoxColumn();
        colCost = new DataGridViewTextBoxColumn();
        colMargin = new DataGridViewTextBoxColumn();
        pnlFooter = new Panel();
        flowFooter = new FlowLayoutPanel();
        btnSave = new Button();
        btnClose = new Button();
        lblStatus = new Label();
        pnlBulk.SuspendLayout();
        flowBulk.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numValue).BeginInit();
        pnlGrid.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvPrices).BeginInit();
        pnlFooter.SuspendLayout();
        flowFooter.SuspendLayout();
        SuspendLayout();
        // 
        // lblMode
        // 
        lblMode.BackColor = Color.WhiteSmoke;
        lblMode.Dock = DockStyle.Top;
        lblMode.FlatStyle = FlatStyle.Flat;
        lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold);
        lblMode.ForeColor = Color.Firebrick;
        lblMode.Location = new Point(0, 0);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(900, 55);
        lblMode.TabIndex = 0;
        lblMode.Text = "تحديث سريع للأسعار";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlBulk
        // 
        pnlBulk.BackColor = Color.White;
        pnlBulk.Controls.Add(flowBulk);
        pnlBulk.Dock = DockStyle.Top;
        pnlBulk.Location = new Point(0, 55);
        pnlBulk.Name = "pnlBulk";
        pnlBulk.Size = new Size(900, 58);
        pnlBulk.TabIndex = 1;
        // 
        // flowBulk
        // 
        flowBulk.Controls.Add(lblOperation);
        flowBulk.Controls.Add(cmbOperation);
        flowBulk.Controls.Add(lblValue);
        flowBulk.Controls.Add(numValue);
        flowBulk.Controls.Add(btnApplySelected);
        flowBulk.Controls.Add(btnApplyAll);
        flowBulk.Controls.Add(lblHint);
        flowBulk.Dock = DockStyle.Fill;
        flowBulk.FlowDirection = FlowDirection.RightToLeft;
        flowBulk.Location = new Point(0, 0);
        flowBulk.Name = "flowBulk";
        flowBulk.Padding = new Padding(10, 4, 10, 4);
        flowBulk.RightToLeft = RightToLeft.Yes;
        flowBulk.Size = new Size(900, 58);
        flowBulk.TabIndex = 0;
        flowBulk.WrapContents = false;
        // 
        // lblOperation
        // 
        lblOperation.AutoSize = true;
        lblOperation.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblOperation.Location = new Point(792, 7);
        lblOperation.Margin = new Padding(2, 13, 4, 13);
        lblOperation.Name = "lblOperation";
        lblOperation.Size = new Size(96, 24);
        lblOperation.TabIndex = 0;
        lblOperation.Text = "العملية :";
        // 
        // cmbOperation
        // 
        cmbOperation.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbOperation.Font = new Font("Tahoma", 10F);
        cmbOperation.FormattingEnabled = true;
        cmbOperation.Items.AddRange(new object[] { "زيادة نسبة ٪", "خصم نسبة ٪", "تحديد سعر ثابت" });
        cmbOperation.Location = new Point(602, 10);
        cmbOperation.Margin = new Padding(4, 10, 6, 10);
        cmbOperation.Name = "cmbOperation";
        cmbOperation.Size = new Size(180, 30);
        cmbOperation.TabIndex = 1;
        cmbOperation.SelectedIndexChanged += cmbOperation_SelectedIndexChanged;
        // 
        // lblValue
        // 
        lblValue.AutoSize = true;
        lblValue.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblValue.Location = new Point(484, 7);
        lblValue.Margin = new Padding(2, 13, 4, 13);
        lblValue.Name = "lblValue";
        lblValue.Size = new Size(108, 24);
        lblValue.TabIndex = 2;
        lblValue.Text = "النسبة (٪):";
        // 
        // numValue
        // 
        numValue.DecimalPlaces = 2;
        numValue.Font = new Font("Tahoma", 10F);
        numValue.Location = new Point(344, 10);
        numValue.Margin = new Padding(4, 10, 6, 10);
        numValue.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
        numValue.Name = "numValue";
        numValue.Size = new Size(130, 30);
        numValue.TabIndex = 3;
        // 
        // btnApplySelected
        // 
        btnApplySelected.BackColor = Color.FromArgb(13, 110, 253);
        btnApplySelected.Cursor = Cursors.Hand;
        btnApplySelected.FlatAppearance.BorderSize = 0;
        btnApplySelected.FlatStyle = FlatStyle.Flat;
        btnApplySelected.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnApplySelected.ForeColor = Color.White;
        btnApplySelected.Location = new Point(186, 6);
        btnApplySelected.Margin = new Padding(8, 6, 4, 6);
        btnApplySelected.Name = "btnApplySelected";
        btnApplySelected.Size = new Size(150, 38);
        btnApplySelected.TabIndex = 4;
        btnApplySelected.Text = "تطبيق على المحدد";
        btnApplySelected.UseVisualStyleBackColor = false;
        btnApplySelected.Click += btnApplySelected_Click;
        // 
        // btnApplyAll
        // 
        btnApplyAll.BackColor = Color.FromArgb(13, 110, 253);
        btnApplyAll.Cursor = Cursors.Hand;
        btnApplyAll.FlatAppearance.BorderSize = 0;
        btnApplyAll.FlatStyle = FlatStyle.Flat;
        btnApplyAll.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnApplyAll.ForeColor = Color.White;
        btnApplyAll.Location = new Point(38, 6);
        btnApplyAll.Margin = new Padding(4, 6, 8, 6);
        btnApplyAll.Name = "btnApplyAll";
        btnApplyAll.Size = new Size(140, 38);
        btnApplyAll.TabIndex = 5;
        btnApplyAll.Text = "تطبيق على الكل";
        btnApplyAll.UseVisualStyleBackColor = false;
        btnApplyAll.Click += btnApplyAll_Click;
        // 
        // lblHint
        // 
        lblHint.AutoSize = true;
        lblHint.Font = new Font("Tahoma", 9F, FontStyle.Italic);
        lblHint.ForeColor = Color.Gray;
        lblHint.Location = new Point(2, 9);
        lblHint.Margin = new Padding(12, 13, 4, 13);
        lblHint.Name = "lblHint";
        lblHint.Size = new Size(19, 24);
        lblHint.TabIndex = 6;
        lblHint.Text = "…";
        // 
        // pnlGrid
        // 
        pnlGrid.BackColor = Color.WhiteSmoke;
        pnlGrid.Controls.Add(dgvPrices);
        pnlGrid.Dock = DockStyle.Fill;
        pnlGrid.Location = new Point(0, 113);
        pnlGrid.Name = "pnlGrid";
        pnlGrid.Padding = new Padding(12, 6, 12, 6);
        pnlGrid.Size = new Size(900, 452);
        pnlGrid.TabIndex = 2;
        // 
        // dgvPrices
        // 
        dgvPrices.AllowUserToAddRows = false;
        dgvPrices.AllowUserToDeleteRows = false;
        dgvPrices.AutoGenerateColumns = false;
        dgvPrices.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvPrices.BackgroundColor = Color.White;
        dgvPrices.BorderStyle = BorderStyle.Fixed3D;
        dgvPrices.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        dgvPrices.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        dgvPrices.ColumnHeadersHeight = 42;
        dgvPrices.Columns.AddRange(new DataGridViewColumn[] { colCode, colName, colUnit, colCurrentPrice, colNewPrice, colCost, colMargin });
        dgvPrices.DefaultCellStyle.Font = new Font("Tahoma", 10F);
        dgvPrices.Dock = DockStyle.Fill;
        dgvPrices.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
        dgvPrices.Location = new Point(12, 6);
        dgvPrices.MultiSelect = true;
        dgvPrices.Name = "dgvPrices";
        dgvPrices.ReadOnly = false;
        dgvPrices.RightToLeft = RightToLeft.Yes;
        dgvPrices.RowHeadersVisible = false;
        dgvPrices.RowTemplate.Height = 32;
        dgvPrices.SelectionMode = DataGridViewSelectionMode.CellSelect;
        dgvPrices.Size = new Size(876, 440);
        dgvPrices.TabIndex = 0;
        dgvPrices.CellEndEdit += dgvPrices_CellEndEdit;
        // 
        // colCode
        // 
        colCode.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        colCode.FillWeight = 10F;
        colCode.HeaderText = "الكود";
        colCode.MinimumWidth = 90;
        colCode.Name = "colCode";
        colCode.ReadOnly = true;
        // 
        // colName
        // 
        colName.FillWeight = 26F;
        colName.HeaderText = "اسم الصنف";
        colName.MinimumWidth = 180;
        colName.Name = "colName";
        colName.ReadOnly = true;
        // 
        // colUnit
        // 
        colUnit.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        colUnit.FillWeight = 8F;
        colUnit.HeaderText = "الوحدة";
        colUnit.MinimumWidth = 70;
        colUnit.Name = "colUnit";
        colUnit.ReadOnly = true;
        // 
        // colCurrentPrice
        // 
        colCurrentPrice.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        colCurrentPrice.DefaultCellStyle.Format = "N2";
        colCurrentPrice.FillWeight = 12F;
        colCurrentPrice.HeaderText = "سعر البيع الحالي";
        colCurrentPrice.MinimumWidth = 110;
        colCurrentPrice.Name = "colCurrentPrice";
        colCurrentPrice.ReadOnly = true;
        // 
        // colNewPrice
        // 
        colNewPrice.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        colNewPrice.DefaultCellStyle.BackColor = Color.FromArgb(255, 253, 210);
        colNewPrice.DefaultCellStyle.Format = "N2";
        colNewPrice.FillWeight = 12F;
        colNewPrice.HeaderText = "السعر الجديد";
        colNewPrice.MinimumWidth = 110;
        colNewPrice.Name = "colNewPrice";
        // 
        // colCost
        // 
        colCost.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        colCost.DefaultCellStyle.Format = "N2";
        colCost.FillWeight = 10F;
        colCost.HeaderText = "التكلفة";
        colCost.MinimumWidth = 90;
        colCost.Name = "colCost";
        colCost.ReadOnly = true;
        // 
        // colMargin
        // 
        colMargin.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        colMargin.DefaultCellStyle.Format = "P1";
        colMargin.FillWeight = 12F;
        colMargin.HeaderText = "هامش الربح";
        colMargin.MinimumWidth = 100;
        colMargin.Name = "colMargin";
        colMargin.ReadOnly = true;
        // 
        // pnlFooter
        // 
        pnlFooter.BackColor = Color.White;
        pnlFooter.Controls.Add(flowFooter);
        pnlFooter.Dock = DockStyle.Bottom;
        pnlFooter.Location = new Point(0, 565);
        pnlFooter.Name = "pnlFooter";
        pnlFooter.Size = new Size(900, 55);
        pnlFooter.TabIndex = 3;
        // 
        // flowFooter
        // 
        flowFooter.Controls.Add(btnSave);
        flowFooter.Controls.Add(btnClose);
        flowFooter.Controls.Add(lblStatus);
        flowFooter.Dock = DockStyle.Fill;
        flowFooter.FlowDirection = FlowDirection.RightToLeft;
        flowFooter.Location = new Point(0, 0);
        flowFooter.Name = "flowFooter";
        flowFooter.Padding = new Padding(12, 6, 12, 6);
        flowFooter.RightToLeft = RightToLeft.Yes;
        flowFooter.Size = new Size(900, 55);
        flowFooter.TabIndex = 0;
        flowFooter.WrapContents = false;
        // 
        // btnSave
        // 
        btnSave.BackColor = Color.FromArgb(25, 135, 84);
        btnSave.Cursor = Cursors.Hand;
        btnSave.FlatAppearance.BorderSize = 0;
        btnSave.FlatStyle = FlatStyle.Flat;
        btnSave.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnSave.ForeColor = Color.White;
        btnSave.Location = new Point(714, 9);
        btnSave.Margin = new Padding(4, 4, 4, 4);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(170, 36);
        btnSave.TabIndex = 0;
        btnSave.Text = "حفظ التغييرات";
        btnSave.UseVisualStyleBackColor = false;
        btnSave.Click += btnSave_Click;
        // 
        // btnClose
        // 
        btnClose.BackColor = Color.FromArgb(108, 117, 125);
        btnClose.Cursor = Cursors.Hand;
        btnClose.DialogResult = DialogResult.Cancel;
        btnClose.FlatAppearance.BorderSize = 0;
        btnClose.FlatStyle = FlatStyle.Flat;
        btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnClose.ForeColor = Color.White;
        btnClose.Location = new Point(606, 9);
        btnClose.Margin = new Padding(4, 4, 8, 4);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(100, 36);
        btnClose.TabIndex = 1;
        btnClose.Text = "إلغاء";
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += btnClose_Click;
        // 
        // lblStatus
        // 
        lblStatus.AutoSize = true;
        lblStatus.Font = new Font("Tahoma", 9F, FontStyle.Italic);
        lblStatus.ForeColor = Color.Gray;
        lblStatus.Location = new Point(470, 9);
        lblStatus.Margin = new Padding(4, 10, 4, 10);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(128, 24);
        lblStatus.TabIndex = 2;
        lblStatus.Text = "[لا توجد تغييرات]";
        // 
        // frmQuickPriceUpdate
        // 
        AcceptButton = btnSave;
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.WhiteSmoke;
        CancelButton = btnClose;
        ClientSize = new Size(900, 620);
        Controls.Add(pnlGrid);
        Controls.Add(pnlBulk);
        Controls.Add(pnlFooter);
        Controls.Add(lblMode);
        Font = new Font("Tahoma", 12F);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "frmQuickPriceUpdate";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "تحديث سريع للأسعار";
        Load += frmQuickPriceUpdate_Load;
        pnlBulk.ResumeLayout(false);
        flowBulk.ResumeLayout(false);
        flowBulk.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numValue).EndInit();
        pnlGrid.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvPrices).EndInit();
        pnlFooter.ResumeLayout(false);
        flowFooter.ResumeLayout(false);
        flowFooter.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    private Label lblMode;
    private Panel pnlBulk;
    private FlowLayoutPanel flowBulk;
    private Label lblOperation;
    private ComboBox cmbOperation;
    private Label lblValue;
    private NumericUpDown numValue;
    private Button btnApplySelected;
    private Button btnApplyAll;
    private Label lblHint;
    private Panel pnlGrid;
    private DataGridView dgvPrices;
    private DataGridViewTextBoxColumn colCode;
    private DataGridViewTextBoxColumn colName;
    private DataGridViewTextBoxColumn colUnit;
    private DataGridViewTextBoxColumn colCurrentPrice;
    private DataGridViewTextBoxColumn colNewPrice;
    private DataGridViewTextBoxColumn colCost;
    private DataGridViewTextBoxColumn colMargin;
    private Panel pnlFooter;
    private FlowLayoutPanel flowFooter;
    private Button btnSave;
    private Button btnClose;
    private Label lblStatus;
}