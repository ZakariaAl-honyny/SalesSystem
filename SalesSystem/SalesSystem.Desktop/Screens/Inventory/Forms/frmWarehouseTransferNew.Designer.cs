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
        DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle12 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle11 = new DataGridViewCellStyle();
        tooltip = new ToolTip(components);
        btnSave = new Button();
        btnPost = new Button();
        btnCancel = new Button();
        cmbFromWarehouse = new ComboBox();
        cmbToWarehouse = new ComboBox();
        dgvLines = new DataGridView();
        colProduct = new DataGridViewComboBoxColumn();
        colUnit = new DataGridViewComboBoxColumn();
        colQuantity = new DataGridViewTextBoxColumn();
        colBatchNo = new DataGridViewTextBoxColumn();
        colExpiryDate = new DataGridViewTextBoxColumn();
        cmsLines = new ContextMenuStrip(components);
        miSave = new ToolStripMenuItem();
        miDeleteRow = new ToolStripMenuItem();
        lblMode = new Label();
        gbToolbar = new GroupBox();
        grpHeader = new GroupBox();
        lblReference = new Label();
        txtReference = new TextBox();
        lblVehiclePlate = new Label();
        txtVehiclePlate = new TextBox();
        lblDriverName = new Label();
        txtDriverName = new TextBox();
        lblToWarehouse = new Label();
        lblFromWarehouse = new Label();
        lblDate = new Label();
        dtpDate = new DateTimePicker();
        lblTransferNo = new Label();
        txtTransferNo = new TextBox();
        gbItems = new GroupBox();
        pnlFooter = new Panel();
        lblNote = new Label();
        ((System.ComponentModel.ISupportInitialize)dgvLines).BeginInit();
        cmsLines.SuspendLayout();
        gbToolbar.SuspendLayout();
        grpHeader.SuspendLayout();
        gbItems.SuspendLayout();
        pnlFooter.SuspendLayout();
        SuspendLayout();
        // 
        // tooltip
        // 
        tooltip.AutoPopDelay = 6000;
        tooltip.InitialDelay = 300;
        tooltip.ReshowDelay = 100;
        // 
        // btnSave
        // 
        btnSave.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(25), Convert.ToInt32(135), Convert.ToInt32(84));
        btnSave.Cursor = Cursors.Hand;
        btnSave.FlatAppearance.BorderSize = 0;
        btnSave.FlatStyle = FlatStyle.Flat;
        btnSave.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnSave.ForeColor = System.Drawing.Color.White;
        btnSave.Location = new Point(710, 26);
        btnSave.MinimumSize = new Size(100, 34);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(150, 36);
        btnSave.TabIndex = 0;
        btnSave.Text = "حفظ السند";
        tooltip.SetToolTip(btnSave, "حفظ سند التحويل كمسودة دون تحريك رصيد (F4)");
        btnSave.UseVisualStyleBackColor = false;
        btnSave.Click += btnSave_Click;
        // 
        // btnPost
        // 
        btnPost.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(25), Convert.ToInt32(135), Convert.ToInt32(84));
        btnPost.Cursor = Cursors.Hand;
        btnPost.FlatAppearance.BorderSize = 0;
        btnPost.FlatStyle = FlatStyle.Flat;
        btnPost.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnPost.ForeColor = System.Drawing.Color.White;
        btnPost.Location = new Point(554, 26);
        btnPost.MinimumSize = new Size(100, 34);
        btnPost.Name = "btnPost";
        btnPost.Size = new Size(150, 36);
        btnPost.TabIndex = 1;
        btnPost.Text = "ترحيل";
        tooltip.SetToolTip(btnPost, "ترحيل سند التحويل وخصم الكميات من المستودع المصدر (F4)");
        btnPost.UseVisualStyleBackColor = false;
        btnPost.Click += btnPost_Click;
        // 
        // btnCancel
        // 
        btnCancel.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(108), Convert.ToInt32(117), Convert.ToInt32(125));
        btnCancel.Cursor = Cursors.Hand;
        btnCancel.DialogResult = DialogResult.Cancel;
        btnCancel.FlatAppearance.BorderSize = 0;
        btnCancel.FlatStyle = FlatStyle.Flat;
        btnCancel.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnCancel.ForeColor = System.Drawing.Color.White;
        btnCancel.Location = new Point(408, 26);
        btnCancel.MinimumSize = new Size(100, 34);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(140, 36);
        btnCancel.TabIndex = 2;
        btnCancel.Text = "إلغاء";
        tooltip.SetToolTip(btnCancel, "إلغاء وإغلاق النافذة (Esc)");
        btnCancel.UseVisualStyleBackColor = false;
        btnCancel.Click += btnCancel_Click;
        // 
        // cmbFromWarehouse
        // 
        cmbFromWarehouse.BackColor = System.Drawing.Color.White;
        cmbFromWarehouse.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbFromWarehouse.FlatStyle = FlatStyle.Flat;
        cmbFromWarehouse.Font = new Font("Tahoma", 10F);
        cmbFromWarehouse.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        cmbFromWarehouse.FormattingEnabled = true;
        cmbFromWarehouse.Location = new Point(555, 33);
        cmbFromWarehouse.Name = "cmbFromWarehouse";
        cmbFromWarehouse.Size = new Size(170, 32);
        cmbFromWarehouse.TabIndex = 5;
        tooltip.SetToolTip(cmbFromWarehouse, "المستودع الذي تُخصم منه الكميات");
        // 
        // cmbToWarehouse
        // 
        cmbToWarehouse.BackColor = System.Drawing.Color.White;
        cmbToWarehouse.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbToWarehouse.FlatStyle = FlatStyle.Flat;
        cmbToWarehouse.Font = new Font("Tahoma", 10F);
        cmbToWarehouse.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        cmbToWarehouse.FormattingEnabled = true;
        cmbToWarehouse.Location = new Point(12, 75);
        cmbToWarehouse.Name = "cmbToWarehouse";
        cmbToWarehouse.Size = new Size(170, 32);
        cmbToWarehouse.TabIndex = 7;
        tooltip.SetToolTip(cmbToWarehouse, "المستودع الذي تُضاف إليه الكميات عند الاستلام");
        // 
        // dgvLines
        // 
        dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(245), Convert.ToInt32(252), Convert.ToInt32(245));
        dgvLines.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
        dgvLines.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvLines.BackgroundColor = System.Drawing.Color.FromArgb(Convert.ToInt32(220), Convert.ToInt32(240), Convert.ToInt32(220));
        dgvLines.BorderStyle = BorderStyle.None;
        dgvLines.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
        dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
        dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
        dataGridViewCellStyle2.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
        dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
        dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
        dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
        dgvLines.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
        dgvLines.ColumnHeadersHeight = 42;
        dgvLines.Columns.AddRange(new DataGridViewColumn[] { colProduct, colUnit, colQuantity, colBatchNo, colExpiryDate });
        dgvLines.ContextMenuStrip = cmsLines;
        dataGridViewCellStyle12.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle12.BackColor = System.Drawing.Color.White;
        dataGridViewCellStyle12.Font = new Font("Tahoma", 10F);
        dataGridViewCellStyle12.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        dataGridViewCellStyle12.SelectionBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(255), Convert.ToInt32(255), Convert.ToInt32(180));
        dataGridViewCellStyle12.SelectionForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        dataGridViewCellStyle12.WrapMode = DataGridViewTriState.False;
        dgvLines.DefaultCellStyle = dataGridViewCellStyle12;
        dgvLines.Dock = DockStyle.Fill;
        dgvLines.EnableHeadersVisualStyles = false;
        dgvLines.GridColor = System.Drawing.Color.FromArgb(Convert.ToInt32(205), Convert.ToInt32(225), Convert.ToInt32(205));
        dgvLines.Location = new Point(8, 33);
        dgvLines.Name = "dgvLines";
        dgvLines.RowHeadersVisible = false;
        dgvLines.RowHeadersWidth = 62;
        dgvLines.RowTemplate.Height = 30;
        dgvLines.SelectionMode = DataGridViewSelectionMode.CellSelect;
        dgvLines.Size = new Size(864, 327);
        dgvLines.TabIndex = 0;
        tooltip.SetToolTip(dgvLines, "أدخل كمية كل صنف — الأسطر بكمية صفر لا تُحوَّل");
        dgvLines.CellEndEdit += dgvLines_CellEndEdit;
        dgvLines.UserAddedRow += dgvLines_UserAddedRow;
        dgvLines.UserDeletedRow += dgvLines_UserDeletedRow;
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
        dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleRight;
        dataGridViewCellStyle5.Format = "N3";
        colQuantity.DefaultCellStyle = dataGridViewCellStyle5;
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
        dataGridViewCellStyle11.Alignment = DataGridViewContentAlignment.MiddleCenter;
        dataGridViewCellStyle11.Format = "d";
        colExpiryDate.DefaultCellStyle = dataGridViewCellStyle11;
        colExpiryDate.FillWeight = 14F;
        colExpiryDate.HeaderText = "تاريخ الصلاحية";
        colExpiryDate.MinimumWidth = 100;
        colExpiryDate.Name = "colExpiryDate";
        // 
        // cmsLines
        // 
        cmsLines.Font = new Font("Tahoma", 10F);
        cmsLines.ImageScalingSize = new Size(24, 24);
        cmsLines.Items.AddRange(new ToolStripItem[] { miSave, miDeleteRow });
        cmsLines.Name = "cmsLines";
        cmsLines.RightToLeft = RightToLeft.Yes;
        cmsLines.Size = new Size(245, 64);
        // 
        // miSave
        // 
        miSave.Name = "miSave";
        miSave.Size = new Size(244, 30);
        miSave.Text = "حفظ السند (F4)";
        miSave.Click += miSave_Click;
        // 
        // miDeleteRow
        // 
        miDeleteRow.Name = "miDeleteRow";
        miDeleteRow.Size = new Size(244, 30);
        miDeleteRow.Text = "حذف السطر المحدد";
        miDeleteRow.Click += miDeleteRow_Click;
        // 
        // lblMode
        // 
        lblMode.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
        lblMode.Dock = DockStyle.Top;
        lblMode.FlatStyle = FlatStyle.Flat;
        lblMode.Font = new Font("Tahoma", 14F, FontStyle.Bold);
        lblMode.ForeColor = System.Drawing.Color.White;
        lblMode.Location = new Point(0, 0);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(880, 42);
        lblMode.TabIndex = 0;
        lblMode.Text = "سند تحويل مخزني";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // gbToolbar
        // 
        gbToolbar.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(245), Convert.ToInt32(235));
        gbToolbar.Controls.Add(btnPost);
        gbToolbar.Controls.Add(btnCancel);
        gbToolbar.Controls.Add(btnSave);
        gbToolbar.Dock = DockStyle.Top;
        gbToolbar.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        gbToolbar.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        gbToolbar.Location = new Point(0, 42);
        gbToolbar.Name = "gbToolbar";
        gbToolbar.Padding = new Padding(8);
        gbToolbar.RightToLeft = RightToLeft.Yes;
        gbToolbar.Size = new Size(880, 68);
        gbToolbar.TabIndex = 1;
        gbToolbar.TabStop = false;
        gbToolbar.Text = "أوامر السند";
        // 
        // grpHeader
        // 
        grpHeader.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(245), Convert.ToInt32(235));
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
        grpHeader.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        grpHeader.Location = new Point(0, 110);
        grpHeader.Name = "grpHeader";
        grpHeader.RightToLeft = RightToLeft.Yes;
        grpHeader.Size = new Size(880, 176);
        grpHeader.TabIndex = 2;
        grpHeader.TabStop = false;
        grpHeader.Text = "بيانات السند";
        // 
        // lblReference
        // 
        lblReference.AutoSize = true;
        lblReference.Font = new Font("Tahoma", 10F);
        lblReference.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblReference.Location = new Point(728, 132);
        lblReference.Name = "lblReference";
        lblReference.Size = new Size(79, 24);
        lblReference.TabIndex = 12;
        lblReference.Text = "المرجع :";
        // 
        // txtReference
        // 
        txtReference.BackColor = System.Drawing.Color.White;
        txtReference.Font = new Font("Tahoma", 10F);
        txtReference.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        txtReference.Location = new Point(321, 129);
        txtReference.Name = "txtReference";
        txtReference.Size = new Size(400, 32);
        txtReference.TabIndex = 13;
        // 
        // lblVehiclePlate
        // 
        lblVehiclePlate.AutoSize = true;
        lblVehiclePlate.Font = new Font("Tahoma", 10F);
        lblVehiclePlate.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblVehiclePlate.Location = new Point(203, 128);
        lblVehiclePlate.Name = "lblVehiclePlate";
        lblVehiclePlate.Size = new Size(127, 24);
        lblVehiclePlate.TabIndex = 10;
        lblVehiclePlate.Text = "لوحة المركبة :";
        // 
        // txtVehiclePlate
        // 
        txtVehiclePlate.BackColor = System.Drawing.Color.White;
        txtVehiclePlate.Font = new Font("Tahoma", 10F);
        txtVehiclePlate.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        txtVehiclePlate.Location = new Point(23, 124);
        txtVehiclePlate.Name = "txtVehiclePlate";
        txtVehiclePlate.Size = new Size(180, 32);
        txtVehiclePlate.TabIndex = 11;
        // 
        // lblDriverName
        // 
        lblDriverName.AutoSize = true;
        lblDriverName.Font = new Font("Tahoma", 10F);
        lblDriverName.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblDriverName.Location = new Point(728, 78);
        lblDriverName.Name = "lblDriverName";
        lblDriverName.Size = new Size(130, 24);
        lblDriverName.TabIndex = 8;
        lblDriverName.Text = "اسم السائق :";
        lblDriverName.Click += lblDriverName_Click;
        // 
        // txtDriverName
        // 
        txtDriverName.BackColor = System.Drawing.Color.White;
        txtDriverName.Font = new Font("Tahoma", 10F);
        txtDriverName.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        txtDriverName.Location = new Point(321, 75);
        txtDriverName.Name = "txtDriverName";
        txtDriverName.Size = new Size(400, 32);
        txtDriverName.TabIndex = 9;
        // 
        // lblToWarehouse
        // 
        lblToWarehouse.AutoSize = true;
        lblToWarehouse.Font = new Font("Tahoma", 10F);
        lblToWarehouse.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblToWarehouse.Location = new Point(160, 83);
        lblToWarehouse.Name = "lblToWarehouse";
        lblToWarehouse.Size = new Size(128, 24);
        lblToWarehouse.TabIndex = 6;
        lblToWarehouse.Text = "إلى مستودع :";
        // 
        // lblFromWarehouse
        // 
        lblFromWarehouse.AutoSize = true;
        lblFromWarehouse.Font = new Font("Tahoma", 10F);
        lblFromWarehouse.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblFromWarehouse.Location = new Point(725, 37);
        lblFromWarehouse.Name = "lblFromWarehouse";
        lblFromWarehouse.Size = new Size(122, 24);
        lblFromWarehouse.TabIndex = 4;
        lblFromWarehouse.Text = "من مستودع :";
        // 
        // lblDate
        // 
        lblDate.AutoSize = true;
        lblDate.Font = new Font("Tahoma", 10F);
        lblDate.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblDate.Location = new Point(481, 37);
        lblDate.Name = "lblDate";
        lblDate.Size = new Size(74, 24);
        lblDate.TabIndex = 2;
        lblDate.Text = "التاريخ :";
        // 
        // dtpDate
        // 
        dtpDate.BackColor = System.Drawing.Color.White;
        dtpDate.Font = new Font("Tahoma", 10F);
        dtpDate.Format = DateTimePickerFormat.Short;
        dtpDate.Location = new Point(311, 33);
        dtpDate.Name = "dtpDate";
        dtpDate.RightToLeft = RightToLeft.Yes;
        dtpDate.Size = new Size(170, 32);
        dtpDate.TabIndex = 3;
        // 
        // lblTransferNo
        // 
        lblTransferNo.AutoSize = true;
        lblTransferNo.Font = new Font("Tahoma", 10F);
        lblTransferNo.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblTransferNo.Location = new Point(203, 37);
        lblTransferNo.Name = "lblTransferNo";
        lblTransferNo.Size = new Size(108, 24);
        lblTransferNo.TabIndex = 0;
        lblTransferNo.Text = "رقم السند :";
        // 
        // txtTransferNo
        // 
        txtTransferNo.BackColor = System.Drawing.Color.White;
        txtTransferNo.Font = new Font("Tahoma", 10F);
        txtTransferNo.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        txtTransferNo.Location = new Point(33, 33);
        txtTransferNo.Name = "txtTransferNo";
        txtTransferNo.ReadOnly = true;
        txtTransferNo.Size = new Size(170, 32);
        txtTransferNo.TabIndex = 1;
        txtTransferNo.Text = "جديد";
        txtTransferNo.TextAlign = HorizontalAlignment.Center;
        // 
        // gbItems
        // 
        gbItems.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(245), Convert.ToInt32(235));
        gbItems.Controls.Add(dgvLines);
        gbItems.Dock = DockStyle.Fill;
        gbItems.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        gbItems.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        gbItems.Location = new Point(0, 286);
        gbItems.Name = "gbItems";
        gbItems.Padding = new Padding(8);
        gbItems.RightToLeft = RightToLeft.Yes;
        gbItems.Size = new Size(880, 368);
        gbItems.TabIndex = 3;
        gbItems.TabStop = false;
        gbItems.Text = "الأصناف المحوّلة";
        // 
        // pnlFooter
        // 
        pnlFooter.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
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
        lblNote.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
        lblNote.BorderStyle = BorderStyle.FixedSingle;
        lblNote.Dock = DockStyle.Fill;
        lblNote.Font = new Font("Tahoma", 10F);
        lblNote.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
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
        BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(170), Convert.ToInt32(220), Convert.ToInt32(170));
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
        ((System.ComponentModel.ISupportInitialize)dgvLines).EndInit();
        cmsLines.ResumeLayout(false);
        gbToolbar.ResumeLayout(false);
        grpHeader.ResumeLayout(false);
        grpHeader.PerformLayout();
        gbItems.ResumeLayout(false);
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
    private Button btnPost;
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
