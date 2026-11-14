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
        tooltip = new ToolTip(components);
        cmsLines = new ContextMenuStrip(components);
        miSave = new ToolStripMenuItem();
        miDeleteRow = new ToolStripMenuItem();
        lblMode = new Label();
        gbToolbar = new GroupBox();
        btnSave = new Button();
        btnCancel = new Button();
        grpHeader = new GroupBox();
        lblReference = new Label();
        txtReference = new TextBox();
        lblVehiclePlate = new Label();
        txtVehiclePlate = new TextBox();
        lblDriverName = new Label();
        txtDriverName = new TextBox();
        lblToWarehouse = new Label();
        cmbToWarehouse = new ComboBox();
        lblFromWarehouse = new Label();
        cmbFromWarehouse = new ComboBox();
        lblDate = new Label();
        dtpDate = new DateTimePicker();
        lblTransferNo = new Label();
        txtTransferNo = new TextBox();
        gbItems = new GroupBox();
        dgvLines = new DataGridView();
        colProduct = new DataGridViewComboBoxColumn();
        colUnit = new DataGridViewComboBoxColumn();
        colQuantity = new DataGridViewTextBoxColumn();
        colBatchNo = new DataGridViewTextBoxColumn();
        colExpiryDate = new DataGridViewTextBoxColumn();
        pnlFooter = new Panel();
        lblNote = new Label();
        cmsLines.SuspendLayout();
        gbToolbar.SuspendLayout();
        grpHeader.SuspendLayout();
        gbItems.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvLines).BeginInit();
        pnlFooter.SuspendLayout();
        SuspendLayout();
        // 
        // tooltip
        // 
        tooltip.AutoPopDelay = 6000;
        tooltip.InitialDelay = 300;
        tooltip.ReshowDelay = 100;
        // 
        // cmsLines
        // 
        cmsLines.Font = new Font("Tahoma", 10F);
        cmsLines.Items.AddRange(new ToolStripItem[] { miSave, miDeleteRow });
        cmsLines.Name = "cmsLines";
        cmsLines.RightToLeft = RightToLeft.Yes;
        cmsLines.Size = new Size(220, 52);
        // 
        // miSave
        // 
        miSave.Name = "miSave";
        miSave.Size = new Size(219, 24);
        miSave.Text = "حفظ السند (F4)";
        miSave.Click += miSave_Click;
        // 
        // miDeleteRow
        // 
        miDeleteRow.Name = "miDeleteRow";
        miDeleteRow.Size = new Size(219, 24);
        miDeleteRow.Text = "حذف السطر المحدد";
        miDeleteRow.Click += miDeleteRow_Click;
        // 
        // lblMode
        // 
        lblMode.BackColor = Color.FromArgb(14, 165, 103);
        lblMode.Dock = DockStyle.Top;
        lblMode.FlatStyle = FlatStyle.Flat;
        lblMode.Font = new Font("Tahoma", 14F, FontStyle.Bold);
        lblMode.ForeColor = Color.White;
        lblMode.Location = new Point(0, 0);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(880, 42);
        lblMode.TabIndex = 0;
        lblMode.Text = "سند تحويل مخزني";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // gbToolbar
        // 
        gbToolbar.BackColor = Color.FromArgb(235, 245, 235);
        gbToolbar.Controls.Add(btnCancel);
        gbToolbar.Controls.Add(btnSave);
        gbToolbar.Dock = DockStyle.Top;
        gbToolbar.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        gbToolbar.ForeColor = Color.FromArgb(14, 82, 52);
        gbToolbar.Location = new Point(0, 42);
        gbToolbar.Name = "gbToolbar";
        gbToolbar.Padding = new Padding(8);
        gbToolbar.RightToLeft = RightToLeft.Yes;
        gbToolbar.Size = new Size(880, 68);
        gbToolbar.TabIndex = 1;
        gbToolbar.TabStop = false;
        gbToolbar.Text = "أوامر السند";
        // 
        // btnSave
        // 
        btnSave.BackColor = Color.FromArgb(25, 135, 84);
        btnSave.Cursor = Cursors.Hand;
        btnSave.FlatAppearance.BorderSize = 0;
        btnSave.FlatStyle = FlatStyle.Flat;
        btnSave.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnSave.ForeColor = Color.White;
        btnSave.Location = new Point(12, 16);
        btnSave.MinimumSize = new Size(100, 34);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(150, 36);
        btnSave.TabIndex = 0;
        btnSave.Text = "حفظ السند";
        btnSave.UseVisualStyleBackColor = false;
        btnSave.Click += btnSave_Click;
        tooltip.SetToolTip(btnSave, "حفظ سند التحويل (F4)");
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
        btnCancel.Location = new Point(172, 16);
        btnCancel.MinimumSize = new Size(100, 34);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(140, 36);
        btnCancel.TabIndex = 1;
        btnCancel.Text = "إلغاء";
        btnCancel.UseVisualStyleBackColor = false;
        btnCancel.Click += btnCancel_Click;
        tooltip.SetToolTip(btnCancel, "إلغاء وإغلاق النافذة (Esc)");
        // 
        // grpHeader
        // 
        grpHeader.BackColor = Color.FromArgb(235, 245, 235);
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
        grpHeader.Dock = DockStyle.Top;
        grpHeader.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        grpHeader.ForeColor = Color.FromArgb(14, 82, 52);
        grpHeader.Location = new Point(0, 110);
        grpHeader.Name = "grpHeader";
        grpHeader.RightToLeft = RightToLeft.Yes;
        grpHeader.Size = new Size(880, 176);
        grpHeader.TabIndex = 2;
        grpHeader.TabStop = false;
        grpHeader.Text = "بيانات السند";
        // 
        // lblTransferNo
        // 
        lblTransferNo.AutoSize = true;
        lblTransferNo.Font = new Font("Tahoma", 10F);
        lblTransferNo.ForeColor = Color.FromArgb(33, 37, 41);
        lblTransferNo.Location = new Point(15, 32);
        lblTransferNo.Name = "lblTransferNo";
        lblTransferNo.Size = new Size(100, 21);
        lblTransferNo.TabIndex = 0;
        lblTransferNo.Text = "رقم السند :";
        // 
        // txtTransferNo
        // 
        txtTransferNo.BackColor = Color.White;
        txtTransferNo.Font = new Font("Tahoma", 10F);
        txtTransferNo.ForeColor = Color.FromArgb(33, 37, 41);
        txtTransferNo.Location = new Point(130, 28);
        txtTransferNo.Name = "txtTransferNo";
        txtTransferNo.ReadOnly = true;
        txtTransferNo.Size = new Size(140, 27);
        txtTransferNo.TabIndex = 1;
        txtTransferNo.Text = "جديد";
        txtTransferNo.TextAlign = HorizontalAlignment.Center;
        // 
        // lblDate
        // 
        lblDate.AutoSize = true;
        lblDate.Font = new Font("Tahoma", 10F);
        lblDate.ForeColor = Color.FromArgb(33, 37, 41);
        lblDate.Location = new Point(300, 32);
        lblDate.Name = "lblDate";
        lblDate.Size = new Size(72, 21);
        lblDate.TabIndex = 2;
        lblDate.Text = "التاريخ :";
        // 
        // dtpDate
        // 
        dtpDate.Font = new Font("Tahoma", 10F);
        dtpDate.Format = DateTimePickerFormat.Short;
        dtpDate.Location = new Point(380, 28);
        dtpDate.Name = "dtpDate";
        dtpDate.RightToLeft = RightToLeft.Yes;
        dtpDate.Size = new Size(150, 27);
        dtpDate.TabIndex = 3;
        // 
        // lblFromWarehouse
        // 
        lblFromWarehouse.AutoSize = true;
        lblFromWarehouse.Font = new Font("Tahoma", 10F);
        lblFromWarehouse.ForeColor = Color.FromArgb(33, 37, 41);
        lblFromWarehouse.Location = new Point(560, 32);
        lblFromWarehouse.Name = "lblFromWarehouse";
        lblFromWarehouse.Size = new Size(113, 21);
        lblFromWarehouse.TabIndex = 4;
        lblFromWarehouse.Text = "من مستودع :";
        // 
        // cmbFromWarehouse
        // 
        cmbFromWarehouse.BackColor = Color.White;
        cmbFromWarehouse.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbFromWarehouse.FlatStyle = FlatStyle.Flat;
        cmbFromWarehouse.Font = new Font("Tahoma", 10F);
        cmbFromWarehouse.ForeColor = Color.FromArgb(33, 37, 41);
        cmbFromWarehouse.FormattingEnabled = true;
        cmbFromWarehouse.Location = new Point(690, 28);
        cmbFromWarehouse.Name = "cmbFromWarehouse";
        cmbFromWarehouse.Size = new Size(170, 29);
        cmbFromWarehouse.TabIndex = 5;
        tooltip.SetToolTip(cmbFromWarehouse, "المستودع الذي تُخصم منه الكميات");
        // 
        // lblToWarehouse
        // 
        lblToWarehouse.AutoSize = true;
        lblToWarehouse.Font = new Font("Tahoma", 10F);
        lblToWarehouse.ForeColor = Color.FromArgb(33, 37, 41);
        lblToWarehouse.Location = new Point(15, 78);
        lblToWarehouse.Name = "lblToWarehouse";
        lblToWarehouse.Size = new Size(113, 21);
        lblToWarehouse.TabIndex = 6;
        lblToWarehouse.Text = "إلى مستودع :";
        // 
        // cmbToWarehouse
        // 
        cmbToWarehouse.BackColor = Color.White;
        cmbToWarehouse.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbToWarehouse.FlatStyle = FlatStyle.Flat;
        cmbToWarehouse.Font = new Font("Tahoma", 10F);
        cmbToWarehouse.ForeColor = Color.FromArgb(33, 37, 41);
        cmbToWarehouse.FormattingEnabled = true;
        cmbToWarehouse.Location = new Point(140, 74);
        cmbToWarehouse.Name = "cmbToWarehouse";
        cmbToWarehouse.Size = new Size(170, 29);
        cmbToWarehouse.TabIndex = 7;
        tooltip.SetToolTip(cmbToWarehouse, "المستودع الذي تُضاف إليه الكميات عند الاستلام");
        // 
        // lblDriverName
        // 
        lblDriverName.AutoSize = true;
        lblDriverName.Font = new Font("Tahoma", 10F);
        lblDriverName.ForeColor = Color.FromArgb(33, 37, 41);
        lblDriverName.Location = new Point(340, 78);
        lblDriverName.Name = "lblDriverName";
        lblDriverName.Size = new Size(107, 21);
        lblDriverName.TabIndex = 8;
        lblDriverName.Text = "اسم السائق :";
        // 
        // txtDriverName
        // 
        txtDriverName.BackColor = Color.White;
        txtDriverName.Font = new Font("Tahoma", 10F);
        txtDriverName.ForeColor = Color.FromArgb(33, 37, 41);
        txtDriverName.Location = new Point(460, 74);
        txtDriverName.Name = "txtDriverName";
        txtDriverName.Size = new Size(400, 27);
        txtDriverName.TabIndex = 9;
        // 
        // lblVehiclePlate
        // 
        lblVehiclePlate.AutoSize = true;
        lblVehiclePlate.Font = new Font("Tahoma", 10F);
        lblVehiclePlate.ForeColor = Color.FromArgb(33, 37, 41);
        lblVehiclePlate.Location = new Point(15, 124);
        lblVehiclePlate.Name = "lblVehiclePlate";
        lblVehiclePlate.Size = new Size(121, 21);
        lblVehiclePlate.TabIndex = 10;
        lblVehiclePlate.Text = "لوحة المركبة :";
        // 
        // txtVehiclePlate
        // 
        txtVehiclePlate.BackColor = Color.White;
        txtVehiclePlate.Font = new Font("Tahoma", 10F);
        txtVehiclePlate.ForeColor = Color.FromArgb(33, 37, 41);
        txtVehiclePlate.Location = new Point(140, 120);
        txtVehiclePlate.Name = "txtVehiclePlate";
        txtVehiclePlate.Size = new Size(180, 27);
        txtVehiclePlate.TabIndex = 11;
        // 
        // lblReference
        // 
        lblReference.AutoSize = true;
        lblReference.Font = new Font("Tahoma", 10F);
        lblReference.ForeColor = Color.FromArgb(33, 37, 41);
        lblReference.Location = new Point(360, 124);
        lblReference.Name = "lblReference";
        lblReference.Size = new Size(77, 21);
        lblReference.TabIndex = 12;
        lblReference.Text = "المرجع :";
        // 
        // txtReference
        // 
        txtReference.BackColor = Color.White;
        txtReference.Font = new Font("Tahoma", 10F);
        txtReference.ForeColor = Color.FromArgb(33, 37, 41);
        txtReference.Location = new Point(450, 120);
        txtReference.Name = "txtReference";
        txtReference.Size = new Size(410, 27);
        txtReference.TabIndex = 13;
        // 
        // gbItems
        // 
        gbItems.BackColor = Color.FromArgb(235, 245, 235);
        gbItems.Controls.Add(dgvLines);
        gbItems.Dock = DockStyle.Fill;
        gbItems.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        gbItems.ForeColor = Color.FromArgb(14, 82, 52);
        gbItems.Location = new Point(0, 286);
        gbItems.Name = "gbItems";
        gbItems.Padding = new Padding(8);
        gbItems.RightToLeft = RightToLeft.Yes;
        gbItems.Size = new Size(880, 368);
        gbItems.TabIndex = 3;
        gbItems.TabStop = false;
        gbItems.Text = "الأصناف المحوّلة";
        // 
        // dgvLines
        // 
        dgvLines.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 252, 245);
        dgvLines.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvLines.BackgroundColor = Color.FromArgb(220, 240, 220);
        dgvLines.BorderStyle = BorderStyle.None;
        dgvLines.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
        dgvLines.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        dgvLines.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(14, 165, 103);
        dgvLines.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        dgvLines.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        dgvLines.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(14, 165, 103);
        dgvLines.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
        dgvLines.ColumnHeadersHeight = 42;
        dgvLines.Columns.AddRange(new DataGridViewColumn[] { colProduct, colUnit, colQuantity, colBatchNo, colExpiryDate });
        dgvLines.ContextMenuStrip = cmsLines;
        dgvLines.DefaultCellStyle.BackColor = Color.White;
        dgvLines.DefaultCellStyle.Font = new Font("Tahoma", 10F);
        dgvLines.DefaultCellStyle.ForeColor = Color.FromArgb(33, 37, 41);
        dgvLines.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 255, 180);
        dgvLines.DefaultCellStyle.SelectionForeColor = Color.FromArgb(33, 37, 41);
        dgvLines.Dock = DockStyle.Fill;
        dgvLines.EnableHeadersVisualStyles = false;
        dgvLines.GridColor = Color.FromArgb(205, 225, 205);
        dgvLines.Location = new Point(8, 24);
        dgvLines.Name = "dgvLines";
        dgvLines.RowHeadersVisible = false;
        dgvLines.RowTemplate.Height = 30;
        dgvLines.SelectionMode = DataGridViewSelectionMode.CellSelect;
        dgvLines.Size = new Size(864, 336);
        dgvLines.TabIndex = 0;
        dgvLines.CellEndEdit += dgvLines_CellEndEdit;
        dgvLines.UserAddedRow += dgvLines_UserAddedRow;
        dgvLines.UserDeletedRow += dgvLines_UserDeletedRow;
        tooltip.SetToolTip(dgvLines, "أدخل كمية كل صنف — الأسطر بكمية صفر لا تُحوَّل");
        // 
        // colProduct
        // 
        colProduct.FillWeight = 30F;
        colProduct.FlatStyle = FlatStyle.Flat;
        colProduct.HeaderText = "الصنف";
        colProduct.MinimumWidth = 180;
        colProduct.Name = "colProduct";
        // 
        // colUnit
        // 
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
        // pnlFooter
        // 
        pnlFooter.BackColor = Color.FromArgb(235, 240, 235);
        pnlFooter.Controls.Add(lblNote);
        pnlFooter.Dock = DockStyle.Bottom;
        pnlFooter.Location = new Point(0, 654);
        pnlFooter.Name = "pnlFooter";
        pnlFooter.Padding = new Padding(8);
        pnlFooter.Size = new Size(880, 46);
        pnlFooter.TabIndex = 4;
        // 
        // lblNote
        // 
        lblNote.BackColor = Color.FromArgb(235, 245, 235);
        lblNote.BorderStyle = BorderStyle.FixedSingle;
        lblNote.Dock = DockStyle.Fill;
        lblNote.Font = new Font("Tahoma", 10F);
        lblNote.ForeColor = Color.FromArgb(14, 82, 52);
        lblNote.Location = new Point(8, 8);
        lblNote.Name = "lblNote";
        lblNote.Size = new Size(864, 30);
        lblNote.TabIndex = 0;
        lblNote.Text = "ملاحظة: الإرسال يخصم فوراً من مستودع المصدر ويضاف عند الاستلام";
        lblNote.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // frmWarehouseTransferNew
        // 
        AcceptButton = btnSave;
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.FromArgb(170, 220, 170);
        CancelButton = btnCancel;
        ClientSize = new Size(880, 700);
        Controls.Add(gbItems);
        Controls.Add(pnlFooter);
        Controls.Add(grpHeader);
        Controls.Add(gbToolbar);
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
        cmsLines.ResumeLayout(false);
        gbToolbar.ResumeLayout(false);
        grpHeader.ResumeLayout(false);
        grpHeader.PerformLayout();
        gbItems.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvLines).EndInit();
        pnlFooter.ResumeLayout(false);
        ResumeLayout(false);
    }

    private ToolTip tooltip;
    private ContextMenuStrip cmsLines;
    private ToolStripMenuItem miSave;
    private ToolStripMenuItem miDeleteRow;
    private Label lblMode;
    private GroupBox gbToolbar;
    private Button btnSave;
    private Button btnCancel;
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
    private GroupBox gbItems;
    private DataGridView dgvLines;
    private DataGridViewComboBoxColumn colProduct;
    private DataGridViewComboBoxColumn colUnit;
    private DataGridViewTextBoxColumn colQuantity;
    private DataGridViewTextBoxColumn colBatchNo;
    private DataGridViewTextBoxColumn colExpiryDate;
    private Panel pnlFooter;
    private Label lblNote;
}
