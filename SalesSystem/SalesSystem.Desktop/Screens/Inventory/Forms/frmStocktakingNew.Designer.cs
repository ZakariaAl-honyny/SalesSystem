namespace SalesSystem.Desktop.Screens.Inventory.Forms;

partial class frmStocktakingNew
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
        DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
        errorProvider = new ErrorProvider(components);
        tooltip = new ToolTip(components);
        btnSaveDraft = new Button();
        btnPost = new Button();
        btnCancel = new Button();
        cmbWarehouse = new ComboBox();
        dgvLines = new DataGridView();
        colProduct = new DataGridViewTextBoxColumn();
        colBarcode = new DataGridViewTextBoxColumn();
        colBookQty = new DataGridViewTextBoxColumn();
        colActualQty = new DataGridViewTextBoxColumn();
        colDifference = new DataGridViewTextBoxColumn();
        cmsLines = new ContextMenuStrip(components);
        miPost = new ToolStripMenuItem();
        miDeleteRow = new ToolStripMenuItem();
        lblMode = new Label();
        gbToolbar = new GroupBox();
        grpHeader = new GroupBox();
        lblWarehouse = new Label();
        dtpDate = new DateTimePicker();
        lblDate = new Label();
        txtCountNo = new TextBox();
        lblCountNo = new Label();
        grpCommittee = new GroupBox();
        cmbStorekeeper = new ComboBox();
        lblStorekeeper = new Label();
        cmbCommitteeMember = new ComboBox();
        lblCommitteeMember = new Label();
        cmbCommitteeHead = new ComboBox();
        lblCommitteeHead = new Label();
        gbItems = new GroupBox();
        grpTotals = new GroupBox();
        lblItemCountValue = new Label();
        lblItemCountLabel = new Label();
        lblDeficitValue = new Label();
        lblDeficitLabel = new Label();
        lblSurplusValue = new Label();
        lblSurplusLabel = new Label();
        ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvLines).BeginInit();
        cmsLines.SuspendLayout();
        gbToolbar.SuspendLayout();
        grpHeader.SuspendLayout();
        grpCommittee.SuspendLayout();
        gbItems.SuspendLayout();
        grpTotals.SuspendLayout();
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
        // btnSaveDraft
        // 
        btnSaveDraft.BackColor = Color.FromArgb(13, 110, 253);
        btnSaveDraft.Cursor = Cursors.Hand;
        btnSaveDraft.FlatAppearance.BorderSize = 0;
        btnSaveDraft.FlatStyle = FlatStyle.Flat;
        btnSaveDraft.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnSaveDraft.ForeColor = Color.White;
        btnSaveDraft.Location = new Point(710, 26);
        btnSaveDraft.MinimumSize = new Size(100, 34);
        btnSaveDraft.Name = "btnSaveDraft";
        btnSaveDraft.Size = new Size(150, 36);
        btnSaveDraft.TabIndex = 0;
        btnSaveDraft.Text = "حفظ كمسودة";
        tooltip.SetToolTip(btnSaveDraft, "حفظ المحضر كمسودة دون اعتماده");
        btnSaveDraft.UseVisualStyleBackColor = false;
        btnSaveDraft.Click += btnSaveDraft_Click;
        // 
        // btnPost
        // 
        btnPost.BackColor = Color.FromArgb(25, 135, 84);
        btnPost.Cursor = Cursors.Hand;
        btnPost.FlatAppearance.BorderSize = 0;
        btnPost.FlatStyle = FlatStyle.Flat;
        btnPost.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnPost.ForeColor = Color.White;
        btnPost.Location = new Point(534, 26);
        btnPost.MinimumSize = new Size(100, 34);
        btnPost.Name = "btnPost";
        btnPost.Size = new Size(170, 36);
        btnPost.TabIndex = 1;
        btnPost.Text = "اعتماد الجرد";
        tooltip.SetToolTip(btnPost, "اعتماد محضر الجرد وترحيل الفروق (F4)");
        btnPost.UseVisualStyleBackColor = false;
        btnPost.Click += btnPost_Click;
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
        btnCancel.Location = new Point(388, 26);
        btnCancel.MinimumSize = new Size(100, 34);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(140, 36);
        btnCancel.TabIndex = 2;
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
        cmbWarehouse.Location = new Point(678, 35);
        cmbWarehouse.Name = "cmbWarehouse";
        cmbWarehouse.Size = new Size(200, 32);
        cmbWarehouse.TabIndex = 5;
        tooltip.SetToolTip(cmbWarehouse, "المستودع الذي يُجرى عليه الجرد الفعلي");
        // 
        // dgvLines
        // 
        dataGridViewCellStyle1.BackColor = Color.FromArgb(248, 249, 250);
        dgvLines.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
        dgvLines.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvLines.BackgroundColor = Color.White;
        dgvLines.BorderStyle = BorderStyle.None;
        dgvLines.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
        dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
        dataGridViewCellStyle2.BackColor = Color.FromArgb(52, 58, 64);
        dataGridViewCellStyle2.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        dataGridViewCellStyle2.ForeColor = Color.White;
        dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(52, 58, 64);
        dataGridViewCellStyle2.SelectionForeColor = Color.White;
        dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
        dgvLines.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
        dgvLines.ColumnHeadersHeight = 42;
        dgvLines.Columns.AddRange(new DataGridViewColumn[] { colProduct, colBarcode, colBookQty, colActualQty, colDifference });
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
        dgvLines.Location = new Point(8, 33);
        dgvLines.Name = "dgvLines";
        dgvLines.RowHeadersVisible = false;
        dgvLines.RowHeadersWidth = 62;
        dgvLines.RowTemplate.Height = 30;
        dgvLines.SelectionMode = DataGridViewSelectionMode.CellSelect;
        dgvLines.Size = new Size(996, 309);
        dgvLines.TabIndex = 0;
        tooltip.SetToolTip(dgvLines, "أدخل الكمية الفعلية لكل صنف — يُحسب الفرق تلقائياً");
        dgvLines.CellEndEdit += dgvLines_CellEndEdit;
        dgvLines.UserDeletedRow += dgvLines_UserDeletedRow;
        // 
        // colProduct
        // 
        colProduct.FillWeight = 34F;
        colProduct.HeaderText = "الصنف";
        colProduct.MinimumWidth = 150;
        colProduct.Name = "colProduct";
        // 
        // colBarcode
        // 
        colBarcode.FillWeight = 16F;
        colBarcode.HeaderText = "الباركود";
        colBarcode.MinimumWidth = 100;
        colBarcode.Name = "colBarcode";
        // 
        // colBookQty
        // 
        dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleRight;
        dataGridViewCellStyle3.Format = "N3";
        colBookQty.DefaultCellStyle = dataGridViewCellStyle3;
        colBookQty.FillWeight = 16F;
        colBookQty.HeaderText = "الكمية الدفترية";
        colBookQty.MinimumWidth = 90;
        colBookQty.Name = "colBookQty";
        colBookQty.ReadOnly = true;
        // 
        // colActualQty
        // 
        dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleRight;
        dataGridViewCellStyle4.Format = "N3";
        colActualQty.DefaultCellStyle = dataGridViewCellStyle4;
        colActualQty.FillWeight = 17F;
        colActualQty.HeaderText = "الكمية الفعلية";
        colActualQty.MinimumWidth = 90;
        colActualQty.Name = "colActualQty";
        // 
        // colDifference
        // 
        dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleRight;
        dataGridViewCellStyle5.Format = "N3";
        colDifference.DefaultCellStyle = dataGridViewCellStyle5;
        colDifference.FillWeight = 17F;
        colDifference.HeaderText = "الفرق";
        colDifference.MinimumWidth = 90;
        colDifference.Name = "colDifference";
        colDifference.ReadOnly = true;
        // 
        // cmsLines
        // 
        cmsLines.Font = new Font("Tahoma", 10F);
        cmsLines.ImageScalingSize = new Size(24, 24);
        cmsLines.Items.AddRange(new ToolStripItem[] { miPost, miDeleteRow });
        cmsLines.Name = "cmsLines";
        cmsLines.RightToLeft = RightToLeft.Yes;
        cmsLines.Size = new Size(245, 64);
        // 
        // miPost
        // 
        miPost.Name = "miPost";
        miPost.Size = new Size(244, 30);
        miPost.Text = "اعتماد الجرد (F4)";
        miPost.Click += miPost_Click;
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
        lblMode.BackColor = Color.FromArgb(16, 150, 96);
        lblMode.Dock = DockStyle.Top;
        lblMode.FlatStyle = FlatStyle.Flat;
        lblMode.Font = new Font("Tahoma", 14F, FontStyle.Bold);
        lblMode.ForeColor = Color.White;
        lblMode.Location = new Point(0, 0);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(1012, 42);
        lblMode.TabIndex = 0;
        lblMode.Text = "محضر جرد فعلي";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // gbToolbar
        // 
        gbToolbar.BackColor = Color.White;
        gbToolbar.Controls.Add(btnCancel);
        gbToolbar.Controls.Add(btnPost);
        gbToolbar.Controls.Add(btnSaveDraft);
        gbToolbar.Dock = DockStyle.Top;
        gbToolbar.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        gbToolbar.ForeColor = Color.FromArgb(33, 37, 41);
        gbToolbar.Location = new Point(0, 42);
        gbToolbar.Name = "gbToolbar";
        gbToolbar.Padding = new Padding(8);
        gbToolbar.RightToLeft = RightToLeft.Yes;
        gbToolbar.Size = new Size(1012, 68);
        gbToolbar.TabIndex = 1;
        gbToolbar.TabStop = false;
        gbToolbar.Text = "أوامر المحضر";
        // 
        // grpHeader
        // 
        grpHeader.BackColor = Color.White;
        grpHeader.Controls.Add(cmbWarehouse);
        grpHeader.Controls.Add(lblWarehouse);
        grpHeader.Controls.Add(dtpDate);
        grpHeader.Controls.Add(lblDate);
        grpHeader.Controls.Add(txtCountNo);
        grpHeader.Controls.Add(lblCountNo);
        grpHeader.Dock = DockStyle.Top;
        grpHeader.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        grpHeader.ForeColor = Color.FromArgb(33, 37, 41);
        grpHeader.Location = new Point(0, 110);
        grpHeader.Name = "grpHeader";
        grpHeader.RightToLeft = RightToLeft.Yes;
        grpHeader.Size = new Size(1012, 86);
        grpHeader.TabIndex = 2;
        grpHeader.TabStop = false;
        grpHeader.Text = "بيانات المحضر";
        // 
        // lblWarehouse
        // 
        lblWarehouse.AutoSize = true;
        lblWarehouse.Font = new Font("Tahoma", 10F);
        lblWarehouse.ForeColor = Color.FromArgb(33, 37, 41);
        lblWarehouse.Location = new Point(884, 39);
        lblWarehouse.Name = "lblWarehouse";
        lblWarehouse.Size = new Size(104, 24);
        lblWarehouse.TabIndex = 4;
        lblWarehouse.Text = "المستودع :";
        // 
        // dtpDate
        // 
        dtpDate.Font = new Font("Tahoma", 10F);
        dtpDate.Format = DateTimePickerFormat.Short;
        dtpDate.Location = new Point(378, 35);
        dtpDate.Name = "dtpDate";
        dtpDate.RightToLeft = RightToLeft.Yes;
        dtpDate.Size = new Size(150, 32);
        dtpDate.TabIndex = 3;
        // 
        // lblDate
        // 
        lblDate.AutoSize = true;
        lblDate.Font = new Font("Tahoma", 10F);
        lblDate.ForeColor = Color.FromArgb(33, 37, 41);
        lblDate.Location = new Point(534, 39);
        lblDate.Name = "lblDate";
        lblDate.Size = new Size(74, 24);
        lblDate.TabIndex = 2;
        lblDate.Text = "التاريخ :";
        // 
        // txtCountNo
        // 
        txtCountNo.BackColor = Color.White;
        txtCountNo.Font = new Font("Tahoma", 10F);
        txtCountNo.ForeColor = Color.FromArgb(33, 37, 41);
        txtCountNo.Location = new Point(106, 31);
        txtCountNo.Name = "txtCountNo";
        txtCountNo.ReadOnly = true;
        txtCountNo.Size = new Size(130, 32);
        txtCountNo.TabIndex = 1;
        txtCountNo.Text = "جديد";
        txtCountNo.TextAlign = HorizontalAlignment.Center;
        // 
        // lblCountNo
        // 
        lblCountNo.AutoSize = true;
        lblCountNo.Font = new Font("Tahoma", 10F);
        lblCountNo.ForeColor = Color.FromArgb(33, 37, 41);
        lblCountNo.Location = new Point(242, 39);
        lblCountNo.Name = "lblCountNo";
        lblCountNo.Size = new Size(118, 24);
        lblCountNo.TabIndex = 0;
        lblCountNo.Text = "رقم المحضر :";
        // 
        // grpCommittee
        // 
        grpCommittee.BackColor = Color.White;
        grpCommittee.Controls.Add(cmbStorekeeper);
        grpCommittee.Controls.Add(lblStorekeeper);
        grpCommittee.Controls.Add(cmbCommitteeMember);
        grpCommittee.Controls.Add(lblCommitteeMember);
        grpCommittee.Controls.Add(cmbCommitteeHead);
        grpCommittee.Controls.Add(lblCommitteeHead);
        grpCommittee.Dock = DockStyle.Top;
        grpCommittee.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        grpCommittee.ForeColor = Color.FromArgb(33, 37, 41);
        grpCommittee.Location = new Point(0, 196);
        grpCommittee.Name = "grpCommittee";
        grpCommittee.RightToLeft = RightToLeft.Yes;
        grpCommittee.Size = new Size(1012, 86);
        grpCommittee.TabIndex = 3;
        grpCommittee.TabStop = false;
        grpCommittee.Text = "لجنة الجرد";
        // 
        // cmbStorekeeper
        // 
        cmbStorekeeper.BackColor = Color.White;
        cmbStorekeeper.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbStorekeeper.FlatStyle = FlatStyle.Flat;
        cmbStorekeeper.Font = new Font("Tahoma", 10F);
        cmbStorekeeper.ForeColor = Color.FromArgb(33, 37, 41);
        cmbStorekeeper.FormattingEnabled = true;
        cmbStorekeeper.Location = new Point(680, 26);
        cmbStorekeeper.Name = "cmbStorekeeper";
        cmbStorekeeper.Size = new Size(180, 32);
        cmbStorekeeper.TabIndex = 5;
        // 
        // lblStorekeeper
        // 
        lblStorekeeper.AutoSize = true;
        lblStorekeeper.Font = new Font("Tahoma", 10F);
        lblStorekeeper.ForeColor = Color.FromArgb(33, 37, 41);
        lblStorekeeper.Location = new Point(864, 30);
        lblStorekeeper.Name = "lblStorekeeper";
        lblStorekeeper.Size = new Size(124, 24);
        lblStorekeeper.TabIndex = 4;
        lblStorekeeper.Text = "أمين المخزن :";
        // 
        // cmbCommitteeMember
        // 
        cmbCommitteeMember.BackColor = Color.White;
        cmbCommitteeMember.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbCommitteeMember.FlatStyle = FlatStyle.Flat;
        cmbCommitteeMember.Font = new Font("Tahoma", 10F);
        cmbCommitteeMember.ForeColor = Color.FromArgb(33, 37, 41);
        cmbCommitteeMember.FormattingEnabled = true;
        cmbCommitteeMember.Location = new Point(380, 26);
        cmbCommitteeMember.Name = "cmbCommitteeMember";
        cmbCommitteeMember.Size = new Size(170, 32);
        cmbCommitteeMember.TabIndex = 3;
        // 
        // lblCommitteeMember
        // 
        lblCommitteeMember.AutoSize = true;
        lblCommitteeMember.Font = new Font("Tahoma", 10F);
        lblCommitteeMember.ForeColor = Color.FromArgb(33, 37, 41);
        lblCommitteeMember.Location = new Point(584, 30);
        lblCommitteeMember.Name = "lblCommitteeMember";
        lblCommitteeMember.Size = new Size(59, 24);
        lblCommitteeMember.TabIndex = 2;
        lblCommitteeMember.Text = "عضو :";
        // 
        // cmbCommitteeHead
        // 
        cmbCommitteeHead.BackColor = Color.White;
        cmbCommitteeHead.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbCommitteeHead.FlatStyle = FlatStyle.Flat;
        cmbCommitteeHead.Font = new Font("Tahoma", 10F);
        cmbCommitteeHead.ForeColor = Color.FromArgb(33, 37, 41);
        cmbCommitteeHead.FormattingEnabled = true;
        cmbCommitteeHead.Location = new Point(66, 26);
        cmbCommitteeHead.Name = "cmbCommitteeHead";
        cmbCommitteeHead.Size = new Size(170, 32);
        cmbCommitteeHead.TabIndex = 1;
        // 
        // lblCommitteeHead
        // 
        lblCommitteeHead.AutoSize = true;
        lblCommitteeHead.Font = new Font("Tahoma", 10F);
        lblCommitteeHead.ForeColor = Color.FromArgb(33, 37, 41);
        lblCommitteeHead.Location = new Point(235, 30);
        lblCommitteeHead.Name = "lblCommitteeHead";
        lblCommitteeHead.Size = new Size(125, 24);
        lblCommitteeHead.TabIndex = 0;
        lblCommitteeHead.Text = "رئيس اللجنة :";
        // 
        // gbItems
        // 
        gbItems.BackColor = Color.White;
        gbItems.Controls.Add(dgvLines);
        gbItems.Dock = DockStyle.Fill;
        gbItems.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        gbItems.ForeColor = Color.FromArgb(33, 37, 41);
        gbItems.Location = new Point(0, 282);
        gbItems.Name = "gbItems";
        gbItems.Padding = new Padding(8);
        gbItems.RightToLeft = RightToLeft.Yes;
        gbItems.Size = new Size(1012, 350);
        gbItems.TabIndex = 4;
        gbItems.TabStop = false;
        gbItems.Text = "أصناف الجرد — أدخل الكمية الفعلية";
        // 
        // grpTotals
        // 
        grpTotals.BackColor = Color.White;
        grpTotals.Controls.Add(lblItemCountValue);
        grpTotals.Controls.Add(lblItemCountLabel);
        grpTotals.Controls.Add(lblDeficitValue);
        grpTotals.Controls.Add(lblDeficitLabel);
        grpTotals.Controls.Add(lblSurplusValue);
        grpTotals.Controls.Add(lblSurplusLabel);
        grpTotals.Dock = DockStyle.Bottom;
        grpTotals.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        grpTotals.ForeColor = Color.FromArgb(33, 37, 41);
        grpTotals.Location = new Point(0, 632);
        grpTotals.Name = "grpTotals";
        grpTotals.RightToLeft = RightToLeft.Yes;
        grpTotals.Size = new Size(1012, 68);
        grpTotals.TabIndex = 5;
        grpTotals.TabStop = false;
        grpTotals.Text = "ملخص الجرد";
        // 
        // lblItemCountValue
        // 
        lblItemCountValue.AutoSize = true;
        lblItemCountValue.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblItemCountValue.ForeColor = Color.FromArgb(33, 37, 41);
        lblItemCountValue.Location = new Point(646, 35);
        lblItemCountValue.Name = "lblItemCountValue";
        lblItemCountValue.Size = new Size(23, 24);
        lblItemCountValue.TabIndex = 5;
        lblItemCountValue.Text = "0";
        // 
        // lblItemCountLabel
        // 
        lblItemCountLabel.AutoSize = true;
        lblItemCountLabel.Font = new Font("Tahoma", 10F);
        lblItemCountLabel.ForeColor = Color.FromArgb(33, 37, 41);
        lblItemCountLabel.Location = new Point(737, 30);
        lblItemCountLabel.Name = "lblItemCountLabel";
        lblItemCountLabel.Size = new Size(123, 24);
        lblItemCountLabel.TabIndex = 4;
        lblItemCountLabel.Text = "عدد الأصناف :";
        // 
        // lblDeficitValue
        // 
        lblDeficitValue.AutoSize = true;
        lblDeficitValue.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblDeficitValue.ForeColor = Color.FromArgb(220, 53, 69);
        lblDeficitValue.Location = new Point(380, 30);
        lblDeficitValue.Name = "lblDeficitValue";
        lblDeficitValue.Size = new Size(68, 24);
        lblDeficitValue.TabIndex = 3;
        lblDeficitValue.Text = "0.000";
        // 
        // lblDeficitLabel
        // 
        lblDeficitLabel.AutoSize = true;
        lblDeficitLabel.Font = new Font("Tahoma", 10F);
        lblDeficitLabel.ForeColor = Color.FromArgb(33, 37, 41);
        lblDeficitLabel.Location = new Point(454, 28);
        lblDeficitLabel.Name = "lblDeficitLabel";
        lblDeficitLabel.Size = new Size(134, 24);
        lblDeficitLabel.TabIndex = 2;
        lblDeficitLabel.Text = "إجمالي العجز :";
        // 
        // lblSurplusValue
        // 
        lblSurplusValue.AutoSize = true;
        lblSurplusValue.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblSurplusValue.ForeColor = Color.FromArgb(25, 135, 84);
        lblSurplusValue.Location = new Point(130, 30);
        lblSurplusValue.Name = "lblSurplusValue";
        lblSurplusValue.Size = new Size(68, 24);
        lblSurplusValue.TabIndex = 1;
        lblSurplusValue.Text = "0.000";
        // 
        // lblSurplusLabel
        // 
        lblSurplusLabel.AutoSize = true;
        lblSurplusLabel.Font = new Font("Tahoma", 10F);
        lblSurplusLabel.ForeColor = Color.FromArgb(33, 37, 41);
        lblSurplusLabel.Location = new Point(204, 30);
        lblSurplusLabel.Name = "lblSurplusLabel";
        lblSurplusLabel.Size = new Size(147, 24);
        lblSurplusLabel.TabIndex = 0;
        lblSurplusLabel.Text = "إجمالي الفائض :";
        // 
        // frmStocktakingNew
        // 
        AcceptButton = btnPost;
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.FromArgb(240, 242, 245);
        CancelButton = btnCancel;
        ClientSize = new Size(1012, 700);
        Controls.Add(gbItems);
        Controls.Add(grpTotals);
        Controls.Add(grpCommittee);
        Controls.Add(grpHeader);
        Controls.Add(gbToolbar);
        Controls.Add(lblMode);
        Font = new Font("Tahoma", 12F);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "frmStocktakingNew";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "محضر جرد فعلي";
        Load += frmStocktakingNew_Load;
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvLines).EndInit();
        cmsLines.ResumeLayout(false);
        gbToolbar.ResumeLayout(false);
        grpHeader.ResumeLayout(false);
        grpHeader.PerformLayout();
        grpCommittee.ResumeLayout(false);
        grpCommittee.PerformLayout();
        gbItems.ResumeLayout(false);
        grpTotals.ResumeLayout(false);
        grpTotals.PerformLayout();
        ResumeLayout(false);
    }

    private ErrorProvider errorProvider;
    private ToolTip tooltip;
    private ContextMenuStrip cmsLines;
    private ToolStripMenuItem miDeleteRow;
    private ToolStripMenuItem miPost;
    private Label lblMode;
    private GroupBox gbToolbar;
    private Button btnSaveDraft;
    private Button btnPost;
    private Button btnCancel;
    private GroupBox grpHeader;
    private Label lblCountNo;
    private TextBox txtCountNo;
    private Label lblDate;
    private DateTimePicker dtpDate;
    private Label lblWarehouse;
    private ComboBox cmbWarehouse;
    private GroupBox grpCommittee;
    private Label lblCommitteeHead;
    private ComboBox cmbCommitteeHead;
    private Label lblCommitteeMember;
    private ComboBox cmbCommitteeMember;
    private Label lblStorekeeper;
    private ComboBox cmbStorekeeper;
    private GroupBox gbItems;
    private DataGridView dgvLines;
    private DataGridViewTextBoxColumn colProduct;
    private DataGridViewTextBoxColumn colBarcode;
    private DataGridViewTextBoxColumn colBookQty;
    private DataGridViewTextBoxColumn colActualQty;
    private DataGridViewTextBoxColumn colDifference;
    private GroupBox grpTotals;
    private Label lblSurplusLabel;
    private Label lblSurplusValue;
    private Label lblDeficitLabel;
    private Label lblDeficitValue;
    private Label lblItemCountLabel;
    private Label lblItemCountValue;
}
