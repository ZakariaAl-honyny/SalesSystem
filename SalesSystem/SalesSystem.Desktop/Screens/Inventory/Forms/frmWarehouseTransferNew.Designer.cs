namespace SalesSystem.Desktop.Screens.Inventory.Forms;

partial class frmWarehouseTransferNew
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
        grpHeader = new GroupBox();
        lblTransferNo = new Label();
        txtTransferNo = new TextBox();
        lblDate = new Label();
        dtpDate = new DateTimePicker();
        lblFromWarehouse = new Label();
        cmbFromWarehouse = new ComboBox();
        lblToWarehouse = new Label();
        cmbToWarehouse = new ComboBox();
        lblDriverName = new Label();
        txtDriverName = new TextBox();
        lblVehiclePlate = new Label();
        txtVehiclePlate = new TextBox();
        lblReference = new Label();
        txtReference = new TextBox();
        dgvLines = new DataGridView();
        colProduct = new DataGridViewComboBoxColumn();
        colUnit = new DataGridViewComboBoxColumn();
        colQuantity = new DataGridViewTextBoxColumn();
        colBatchNo = new DataGridViewTextBoxColumn();
        colExpiryDate = new DataGridViewTextBoxColumn();
        lblNote = new Label();
        btnSave = new Button();
        btnCancel = new Button();
        grpHeader.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvLines).BeginInit();
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
        lblMode.Size = new Size(760, 39);
        lblMode.TabIndex = 0;
        lblMode.Text = "سند تحويل مخزني";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // grpHeader
        // 
        grpHeader.Controls.Add(lblReference);
        grpHeader.Controls.Add(txtReference);
        grpHeader.Controls.Add(lblVehiclePlate);
        grpHeader.Controls.Add(txtVehiclePlate);
        grpHeader.Controls.Add(lblDriverName);
        grpHeader.Controls.Add(txtDriverName);
        grpHeader.Controls.Add(lblToWarehouse);
        grpHeader.Controls.Add(cmbToWarehouse);
        grpHeader.Controls.Add(lblFromWarehouse);
        grpHeader.Controls.Add(cmbFromWarehouse);
        grpHeader.Controls.Add(lblDate);
        grpHeader.Controls.Add(dtpDate);
        grpHeader.Controls.Add(lblTransferNo);
        grpHeader.Controls.Add(txtTransferNo);
        grpHeader.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        grpHeader.Location = new Point(12, 46);
        grpHeader.Name = "grpHeader";
        grpHeader.RightToLeft = RightToLeft.Yes;
        grpHeader.Size = new Size(736, 165);
        grpHeader.TabIndex = 1;
        grpHeader.TabStop = false;
        grpHeader.Text = "بيانات السند";
        // 
        // lblTransferNo
        // 
        lblTransferNo.AutoSize = true;
        lblTransferNo.Font = new Font("Tahoma", 10F);
        lblTransferNo.Location = new Point(15, 28);
        lblTransferNo.Name = "lblTransferNo";
        lblTransferNo.Size = new Size(89, 24);
        lblTransferNo.TabIndex = 0;
        lblTransferNo.Text = "رقم السند :";
        // 
        // txtTransferNo
        // 
        txtTransferNo.Font = new Font("Tahoma", 10F);
        txtTransferNo.Location = new Point(110, 25);
        txtTransferNo.Name = "txtTransferNo";
        txtTransferNo.ReadOnly = true;
        txtTransferNo.Size = new Size(130, 30);
        txtTransferNo.TabIndex = 1;
        txtTransferNo.Text = "جديد";
        // 
        // lblDate
        // 
        lblDate.AutoSize = true;
        lblDate.Font = new Font("Tahoma", 10F);
        lblDate.Location = new Point(260, 28);
        lblDate.Name = "lblDate";
        lblDate.Size = new Size(70, 24);
        lblDate.TabIndex = 2;
        lblDate.Text = "التاريخ :";
        // 
        // dtpDate
        // 
        dtpDate.Font = new Font("Tahoma", 10F);
        dtpDate.Format = DateTimePickerFormat.Short;
        dtpDate.Location = new Point(340, 25);
        dtpDate.Name = "dtpDate";
        dtpDate.RightToLeft = RightToLeft.Yes;
        dtpDate.Size = new Size(130, 30);
        dtpDate.TabIndex = 3;
        // 
        // lblFromWarehouse
        // 
        lblFromWarehouse.AutoSize = true;
        lblFromWarehouse.Font = new Font("Tahoma", 10F);
        lblFromWarehouse.Location = new Point(490, 28);
        lblFromWarehouse.Name = "lblFromWarehouse";
        lblFromWarehouse.Size = new Size(94, 24);
        lblFromWarehouse.TabIndex = 4;
        lblFromWarehouse.Text = "من مستودع :";
        // 
        // cmbFromWarehouse
        // 
        cmbFromWarehouse.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbFromWarehouse.Font = new Font("Tahoma", 10F);
        cmbFromWarehouse.FormattingEnabled = true;
        cmbFromWarehouse.Location = new Point(580, 25);
        cmbFromWarehouse.Name = "cmbFromWarehouse";
        cmbFromWarehouse.Size = new Size(140, 30);
        cmbFromWarehouse.TabIndex = 5;
        // 
        // lblToWarehouse
        // 
        lblToWarehouse.AutoSize = true;
        lblToWarehouse.Font = new Font("Tahoma", 10F);
        lblToWarehouse.Location = new Point(15, 72);
        lblToWarehouse.Name = "lblToWarehouse";
        lblToWarehouse.Size = new Size(90, 24);
        lblToWarehouse.TabIndex = 6;
        lblToWarehouse.Text = "إلى مستودع :";
        // 
        // cmbToWarehouse
        // 
        cmbToWarehouse.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbToWarehouse.Font = new Font("Tahoma", 10F);
        cmbToWarehouse.FormattingEnabled = true;
        cmbToWarehouse.Location = new Point(110, 69);
        cmbToWarehouse.Name = "cmbToWarehouse";
        cmbToWarehouse.Size = new Size(140, 30);
        cmbToWarehouse.TabIndex = 7;
        // 
        // lblDriverName
        // 
        lblDriverName.AutoSize = true;
        lblDriverName.Font = new Font("Tahoma", 10F);
        lblDriverName.Location = new Point(270, 72);
        lblDriverName.Name = "lblDriverName";
        lblDriverName.Size = new Size(95, 24);
        lblDriverName.TabIndex = 8;
        lblDriverName.Text = "اسم السائق :";
        // 
        // txtDriverName
        // 
        txtDriverName.Font = new Font("Tahoma", 10F);
        txtDriverName.Location = new Point(365, 69);
        txtDriverName.Name = "txtDriverName";
        txtDriverName.Size = new Size(355, 30);
        txtDriverName.TabIndex = 9;
        // 
        // lblVehiclePlate
        // 
        lblVehiclePlate.AutoSize = true;
        lblVehiclePlate.Font = new Font("Tahoma", 10F);
        lblVehiclePlate.Location = new Point(15, 116);
        lblVehiclePlate.Name = "lblVehiclePlate";
        lblVehiclePlate.Size = new Size(120, 24);
        lblVehiclePlate.TabIndex = 10;
        lblVehiclePlate.Text = "لوحة المركبة :";
        // 
        // txtVehiclePlate
        // 
        txtVehiclePlate.Font = new Font("Tahoma", 10F);
        txtVehiclePlate.Location = new Point(135, 113);
        txtVehiclePlate.Name = "txtVehiclePlate";
        txtVehiclePlate.Size = new Size(205, 30);
        txtVehiclePlate.TabIndex = 11;
        // 
        // lblReference
        // 
        lblReference.AutoSize = true;
        lblReference.Font = new Font("Tahoma", 10F);
        lblReference.Location = new Point(360, 116);
        lblReference.Name = "lblReference";
        lblReference.Size = new Size(70, 24);
        lblReference.TabIndex = 12;
        lblReference.Text = "المرجع :";
        // 
        // txtReference
        // 
        txtReference.Font = new Font("Tahoma", 10F);
        txtReference.Location = new Point(430, 113);
        txtReference.Name = "txtReference";
        txtReference.Size = new Size(290, 30);
        txtReference.TabIndex = 13;
        // 
        // dgvLines
        // 
        dgvLines.AllowUserToAddRows = true;
        dgvLines.AllowUserToDeleteRows = true;
        dgvLines.AutoGenerateColumns = false;
        dgvLines.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvLines.BackgroundColor = Color.White;
        dgvLines.BorderStyle = BorderStyle.Fixed3D;
        dgvLines.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        dgvLines.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        dgvLines.ColumnHeadersHeight = 40;
        dgvLines.Columns.AddRange(new DataGridViewColumn[] { colProduct, colUnit, colQuantity, colBatchNo, colExpiryDate });
        dgvLines.DefaultCellStyle.Font = new Font("Tahoma", 10F);
        dgvLines.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
        dgvLines.Location = new Point(12, 219);
        dgvLines.Name = "dgvLines";
        dgvLines.ReadOnly = false;
        dgvLines.RowHeadersVisible = false;
        dgvLines.RowTemplate.Height = 30;
        dgvLines.SelectionMode = DataGridViewSelectionMode.CellSelect;
        dgvLines.Size = new Size(736, 262);
        dgvLines.TabIndex = 2;
        dgvLines.CellEndEdit += dgvLines_CellEndEdit;
        dgvLines.UserAddedRow += dgvLines_UserAddedRow;
        dgvLines.UserDeletedRow += dgvLines_UserDeletedRow;
        // 
        // colProduct
        // 
        colProduct.DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton;
        colProduct.FillWeight = 30F;
        colProduct.FlatStyle = FlatStyle.Flat;
        colProduct.HeaderText = "الصنف";
        colProduct.MinimumWidth = 180;
        colProduct.Name = "colProduct";
        // 
        // colUnit
        // 
        colUnit.DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton;
        colUnit.FillWeight = 12F;
        colUnit.FlatStyle = FlatStyle.Flat;
        colUnit.HeaderText = "الوحدة";
        colUnit.MinimumWidth = 80;
        colUnit.Name = "colUnit";
        // 
        // colQuantity
        // 
        colQuantity.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        colQuantity.DefaultCellStyle.Format = "N3";
        colQuantity.FillWeight = 15F;
        colQuantity.HeaderText = "الكمية";
        colQuantity.MinimumWidth = 90;
        colQuantity.Name = "colQuantity";
        // 
        // colBatchNo
        // 
        colBatchNo.FillWeight = 16F;
        colBatchNo.HeaderText = "رقم التشغيلة";
        colBatchNo.MinimumWidth = 110;
        colBatchNo.Name = "colBatchNo";
        // 
        // colExpiryDate
        // 
        colExpiryDate.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        colExpiryDate.DefaultCellStyle.Format = "d";
        colExpiryDate.FillWeight = 14F;
        colExpiryDate.HeaderText = "تاريخ الصلاحية";
        colExpiryDate.MinimumWidth = 100;
        colExpiryDate.Name = "colExpiryDate";
        // 
        // lblNote
        // 
        lblNote.BackColor = Color.FromArgb(255, 243, 205);
        lblNote.BorderStyle = BorderStyle.FixedSingle;
        lblNote.Font = new Font("Tahoma", 10F);
        lblNote.ForeColor = Color.FromArgb(146, 64, 14);
        lblNote.Location = new Point(12, 489);
        lblNote.Name = "lblNote";
        lblNote.Size = new Size(736, 32);
        lblNote.TabIndex = 3;
        lblNote.Text = "ملاحظة: الإرسال يخصم فوراً من مستودع المصدر ويضاف عند الاستلام";
        lblNote.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // btnSave
        // 
        btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnSave.BackColor = Color.FromArgb(25, 135, 84);
        btnSave.Cursor = Cursors.Hand;
        btnSave.FlatAppearance.BorderSize = 0;
        btnSave.FlatStyle = FlatStyle.Flat;
        btnSave.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnSave.ForeColor = Color.White;
        btnSave.Location = new Point(140, 538);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(150, 50);
        btnSave.TabIndex = 5;
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
        btnCancel.Location = new Point(12, 538);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(120, 50);
        btnCancel.TabIndex = 4;
        btnCancel.Text = "إلغاء";
        btnCancel.UseVisualStyleBackColor = false;
        btnCancel.Click += btnCancel_Click;
        // 
        // frmWarehouseTransferNew
        // 
        AcceptButton = btnSave;
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.WhiteSmoke;
        CancelButton = btnCancel;
        ClientSize = new Size(760, 600);
        Controls.Add(btnCancel);
        Controls.Add(btnSave);
        Controls.Add(lblNote);
        Controls.Add(dgvLines);
        Controls.Add(grpHeader);
        Controls.Add(lblMode);
        Font = new Font("Tahoma", 12F);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "frmWarehouseTransferNew";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "سند تحويل مخزني";
        Load += frmWarehouseTransferNew_Load;
        grpHeader.ResumeLayout(false);
        grpHeader.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvLines).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    private Label lblMode;
    private GroupBox grpHeader;
    private Label lblTransferNo;
    private TextBox txtTransferNo;
    private Label lblDate;
    private DateTimePicker dtpDate;
    private Label lblFromWarehouse;
    private ComboBox cmbFromWarehouse;
    private Label lblToWarehouse;
    private ComboBox cmbToWarehouse;
    private Label lblDriverName;
    private TextBox txtDriverName;
    private Label lblVehiclePlate;
    private TextBox txtVehiclePlate;
    private Label lblReference;
    private TextBox txtReference;
    private DataGridView dgvLines;
    private DataGridViewComboBoxColumn colProduct;
    private DataGridViewComboBoxColumn colUnit;
    private DataGridViewTextBoxColumn colQuantity;
    private DataGridViewTextBoxColumn colBatchNo;
    private DataGridViewTextBoxColumn colExpiryDate;
    private Label lblNote;
    private Button btnSave;
    private Button btnCancel;
}