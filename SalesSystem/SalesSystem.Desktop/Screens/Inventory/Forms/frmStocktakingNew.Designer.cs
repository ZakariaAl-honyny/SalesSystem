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
        errorProvider = new ErrorProvider(components);
        lblMode = new Label();
        grpHeader = new GroupBox();
        lblCountNo = new Label();
        txtCountNo = new TextBox();
        lblDate = new Label();
        dtpDate = new DateTimePicker();
        lblWarehouse = new Label();
        cmbWarehouse = new ComboBox();
        grpCommittee = new GroupBox();
        lblCommitteeHead = new Label();
        cmbCommitteeHead = new ComboBox();
        lblCommitteeMember = new Label();
        cmbCommitteeMember = new ComboBox();
        lblStorekeeper = new Label();
        cmbStorekeeper = new ComboBox();
        dgvLines = new DataGridView();
        colProduct = new DataGridViewTextBoxColumn();
        colBarcode = new DataGridViewTextBoxColumn();
        colBookQty = new DataGridViewTextBoxColumn();
        colActualQty = new DataGridViewTextBoxColumn();
        colDifference = new DataGridViewTextBoxColumn();
        grpTotals = new GroupBox();
        lblSurplusLabel = new Label();
        lblSurplusValue = new Label();
        lblDeficitLabel = new Label();
        lblDeficitValue = new Label();
        lblItemCountLabel = new Label();
        lblItemCountValue = new Label();
        btnSaveDraft = new Button();
        btnPost = new Button();
        btnCancel = new Button();
        ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
        grpHeader.SuspendLayout();
        grpCommittee.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvLines).BeginInit();
        grpTotals.SuspendLayout();
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
        lblMode.Size = new Size(820, 39);
        lblMode.TabIndex = 0;
        lblMode.Text = "محضر جرد فعلي";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // grpHeader
        // 
        grpHeader.Controls.Add(lblCountNo);
        grpHeader.Controls.Add(txtCountNo);
        grpHeader.Controls.Add(lblDate);
        grpHeader.Controls.Add(dtpDate);
        grpHeader.Controls.Add(lblWarehouse);
        grpHeader.Controls.Add(cmbWarehouse);
        grpHeader.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        grpHeader.Location = new Point(12, 46);
        grpHeader.Name = "grpHeader";
        grpHeader.RightToLeft = RightToLeft.Yes;
        grpHeader.Size = new Size(796, 86);
        grpHeader.TabIndex = 1;
        grpHeader.TabStop = false;
        grpHeader.Text = "بيانات المحضر";
        // 
        // lblCountNo
        // 
        lblCountNo.AutoSize = true;
        lblCountNo.Font = new Font("Tahoma", 10F);
        lblCountNo.Location = new Point(15, 32);
        lblCountNo.Name = "lblCountNo";
        lblCountNo.Size = new Size(95, 24);
        lblCountNo.TabIndex = 0;
        lblCountNo.Text = "رقم المحضر :";
        // 
        // txtCountNo
        // 
        txtCountNo.Font = new Font("Tahoma", 10F);
        txtCountNo.Location = new Point(110, 29);
        txtCountNo.Name = "txtCountNo";
        txtCountNo.ReadOnly = true;
        txtCountNo.Size = new Size(120, 30);
        txtCountNo.TabIndex = 1;
        txtCountNo.Text = "جديد";
        // 
        // lblDate
        // 
        lblDate.AutoSize = true;
        lblDate.Font = new Font("Tahoma", 10F);
        lblDate.Location = new Point(250, 32);
        lblDate.Name = "lblDate";
        lblDate.Size = new Size(70, 24);
        lblDate.TabIndex = 2;
        lblDate.Text = "التاريخ :";
        // 
        // dtpDate
        // 
        dtpDate.Font = new Font("Tahoma", 10F);
        dtpDate.Format = DateTimePickerFormat.Short;
        dtpDate.Location = new Point(330, 29);
        dtpDate.Name = "dtpDate";
        dtpDate.RightToLeft = RightToLeft.Yes;
        dtpDate.Size = new Size(130, 30);
        dtpDate.TabIndex = 3;
        // 
        // lblWarehouse
        // 
        lblWarehouse.AutoSize = true;
        lblWarehouse.Font = new Font("Tahoma", 10F);
        lblWarehouse.Location = new Point(480, 32);
        lblWarehouse.Name = "lblWarehouse";
        lblWarehouse.Size = new Size(85, 24);
        lblWarehouse.TabIndex = 4;
        lblWarehouse.Text = "المستودع :";
        // 
        // cmbWarehouse
        // 
        cmbWarehouse.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbWarehouse.Font = new Font("Tahoma", 10F);
        cmbWarehouse.FormattingEnabled = true;
        cmbWarehouse.Location = new Point(570, 29);
        cmbWarehouse.Name = "cmbWarehouse";
        cmbWarehouse.Size = new Size(210, 30);
        cmbWarehouse.TabIndex = 5;
        // 
        // grpCommittee
        // 
        grpCommittee.Controls.Add(lblCommitteeHead);
        grpCommittee.Controls.Add(cmbCommitteeHead);
        grpCommittee.Controls.Add(lblCommitteeMember);
        grpCommittee.Controls.Add(cmbCommitteeMember);
        grpCommittee.Controls.Add(lblStorekeeper);
        grpCommittee.Controls.Add(cmbStorekeeper);
        grpCommittee.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        grpCommittee.Location = new Point(12, 140);
        grpCommittee.Name = "grpCommittee";
        grpCommittee.RightToLeft = RightToLeft.Yes;
        grpCommittee.Size = new Size(796, 86);
        grpCommittee.TabIndex = 2;
        grpCommittee.TabStop = false;
        grpCommittee.Text = "لجنة الجرد";
        // 
        // lblCommitteeHead
        // 
        lblCommitteeHead.AutoSize = true;
        lblCommitteeHead.Font = new Font("Tahoma", 10F);
        lblCommitteeHead.Location = new Point(15, 32);
        lblCommitteeHead.Name = "lblCommitteeHead";
        lblCommitteeHead.Size = new Size(93, 24);
        lblCommitteeHead.TabIndex = 0;
        lblCommitteeHead.Text = "رئيس اللجنة :";
        // 
        // cmbCommitteeHead
        // 
        cmbCommitteeHead.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbCommitteeHead.Font = new Font("Tahoma", 10F);
        cmbCommitteeHead.FormattingEnabled = true;
        cmbCommitteeHead.Location = new Point(110, 29);
        cmbCommitteeHead.Name = "cmbCommitteeHead";
        cmbCommitteeHead.Size = new Size(170, 30);
        cmbCommitteeHead.TabIndex = 1;
        // 
        // lblCommitteeMember
        // 
        lblCommitteeMember.AutoSize = true;
        lblCommitteeMember.Font = new Font("Tahoma", 10F);
        lblCommitteeMember.Location = new Point(300, 32);
        lblCommitteeMember.Name = "lblCommitteeMember";
        lblCommitteeMember.Size = new Size(55, 24);
        lblCommitteeMember.TabIndex = 2;
        lblCommitteeMember.Text = "عضو :";
        // 
        // cmbCommitteeMember
        // 
        cmbCommitteeMember.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbCommitteeMember.Font = new Font("Tahoma", 10F);
        cmbCommitteeMember.FormattingEnabled = true;
        cmbCommitteeMember.Location = new Point(370, 29);
        cmbCommitteeMember.Name = "cmbCommitteeMember";
        cmbCommitteeMember.Size = new Size(170, 30);
        cmbCommitteeMember.TabIndex = 3;
        // 
        // lblStorekeeper
        // 
        lblStorekeeper.AutoSize = true;
        lblStorekeeper.Font = new Font("Tahoma", 10F);
        lblStorekeeper.Location = new Point(560, 32);
        lblStorekeeper.Name = "lblStorekeeper";
        lblStorekeeper.Size = new Size(97, 24);
        lblStorekeeper.TabIndex = 4;
        lblStorekeeper.Text = "أمين المخزن :";
        // 
        // cmbStorekeeper
        // 
        cmbStorekeeper.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbStorekeeper.Font = new Font("Tahoma", 10F);
        cmbStorekeeper.FormattingEnabled = true;
        cmbStorekeeper.Location = new Point(650, 29);
        cmbStorekeeper.Name = "cmbStorekeeper";
        cmbStorekeeper.Size = new Size(135, 30);
        cmbStorekeeper.TabIndex = 5;
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
        dgvLines.Columns.AddRange(new DataGridViewColumn[] { colProduct, colBarcode, colBookQty, colActualQty, colDifference });
        dgvLines.DefaultCellStyle.Font = new Font("Tahoma", 10F);
        dgvLines.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
        dgvLines.Location = new Point(12, 234);
        dgvLines.Name = "dgvLines";
        dgvLines.ReadOnly = false;
        dgvLines.RowHeadersVisible = false;
        dgvLines.RowTemplate.Height = 30;
        dgvLines.SelectionMode = DataGridViewSelectionMode.CellSelect;
        dgvLines.Size = new Size(796, 276);
        dgvLines.TabIndex = 3;
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
        colBookQty.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        colBookQty.DefaultCellStyle.Format = "N3";
        colBookQty.FillWeight = 16F;
        colBookQty.HeaderText = "الكمية الدفترية";
        colBookQty.MinimumWidth = 90;
        colBookQty.Name = "colBookQty";
        colBookQty.ReadOnly = true;
        // 
        // colActualQty
        // 
        colActualQty.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        colActualQty.DefaultCellStyle.Format = "N3";
        colActualQty.FillWeight = 17F;
        colActualQty.HeaderText = "الكمية الفعلية";
        colActualQty.MinimumWidth = 90;
        colActualQty.Name = "colActualQty";
        // 
        // colDifference
        // 
        colDifference.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        colDifference.DefaultCellStyle.Format = "N3";
        colDifference.FillWeight = 17F;
        colDifference.HeaderText = "الفرق";
        colDifference.MinimumWidth = 90;
        colDifference.Name = "colDifference";
        colDifference.ReadOnly = true;
        // 
        // grpTotals
        // 
        grpTotals.Controls.Add(lblSurplusLabel);
        grpTotals.Controls.Add(lblSurplusValue);
        grpTotals.Controls.Add(lblDeficitLabel);
        grpTotals.Controls.Add(lblDeficitValue);
        grpTotals.Controls.Add(lblItemCountLabel);
        grpTotals.Controls.Add(lblItemCountValue);
        grpTotals.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        grpTotals.Location = new Point(12, 518);
        grpTotals.Name = "grpTotals";
        grpTotals.RightToLeft = RightToLeft.Yes;
        grpTotals.Size = new Size(796, 52);
        grpTotals.TabIndex = 4;
        grpTotals.TabStop = false;
        grpTotals.Text = "ملخص الجرد";
        // 
        // lblSurplusLabel
        // 
        lblSurplusLabel.AutoSize = true;
        lblSurplusLabel.Font = new Font("Tahoma", 10F);
        lblSurplusLabel.Location = new Point(15, 16);
        lblSurplusLabel.Name = "lblSurplusLabel";
        lblSurplusLabel.Size = new Size(103, 24);
        lblSurplusLabel.TabIndex = 0;
        lblSurplusLabel.Text = "إجمالي الفائض :";
        // 
        // lblSurplusValue
        // 
        lblSurplusValue.AutoSize = true;
        lblSurplusValue.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblSurplusValue.ForeColor = Color.FromArgb(25, 135, 84);
        lblSurplusValue.Location = new Point(120, 16);
        lblSurplusValue.Name = "lblSurplusValue";
        lblSurplusValue.Size = new Size(45, 24);
        lblSurplusValue.TabIndex = 1;
        lblSurplusValue.Text = "0.000";
        // 
        // lblDeficitLabel
        // 
        lblDeficitLabel.AutoSize = true;
        lblDeficitLabel.Font = new Font("Tahoma", 10F);
        lblDeficitLabel.Location = new Point(250, 16);
        lblDeficitLabel.Name = "lblDeficitLabel";
        lblDeficitLabel.Size = new Size(100, 24);
        lblDeficitLabel.TabIndex = 2;
        lblDeficitLabel.Text = "إجمالي العجز :";
        // 
        // lblDeficitValue
        // 
        lblDeficitValue.AutoSize = true;
        lblDeficitValue.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblDeficitValue.ForeColor = Color.FromArgb(220, 53, 69);
        lblDeficitValue.Location = new Point(350, 16);
        lblDeficitValue.Name = "lblDeficitValue";
        lblDeficitValue.Size = new Size(45, 24);
        lblDeficitValue.TabIndex = 3;
        lblDeficitValue.Text = "0.000";
        // 
        // lblItemCountLabel
        // 
        lblItemCountLabel.AutoSize = true;
        lblItemCountLabel.Font = new Font("Tahoma", 10F);
        lblItemCountLabel.Location = new Point(480, 16);
        lblItemCountLabel.Name = "lblItemCountLabel";
        lblItemCountLabel.Size = new Size(99, 24);
        lblItemCountLabel.TabIndex = 4;
        lblItemCountLabel.Text = "عدد الأصناف :";
        // 
        // lblItemCountValue
        // 
        lblItemCountValue.AutoSize = true;
        lblItemCountValue.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblItemCountValue.Location = new Point(580, 16);
        lblItemCountValue.Name = "lblItemCountValue";
        lblItemCountValue.Size = new Size(26, 24);
        lblItemCountValue.TabIndex = 5;
        lblItemCountValue.Text = "0";
        // 
        // btnSaveDraft
        // 
        btnSaveDraft.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnSaveDraft.BackColor = Color.FromArgb(13, 110, 253);
        btnSaveDraft.Cursor = Cursors.Hand;
        btnSaveDraft.FlatAppearance.BorderSize = 0;
        btnSaveDraft.FlatStyle = FlatStyle.Flat;
        btnSaveDraft.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnSaveDraft.ForeColor = Color.White;
        btnSaveDraft.Location = new Point(140, 578);
        btnSaveDraft.Name = "btnSaveDraft";
        btnSaveDraft.Size = new Size(150, 50);
        btnSaveDraft.TabIndex = 7;
        btnSaveDraft.Text = "حفظ كمسودة";
        btnSaveDraft.UseVisualStyleBackColor = false;
        btnSaveDraft.Click += btnSaveDraft_Click;
        // 
        // btnPost
        // 
        btnPost.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnPost.BackColor = Color.FromArgb(25, 135, 84);
        btnPost.Cursor = Cursors.Hand;
        btnPost.FlatAppearance.BorderSize = 0;
        btnPost.FlatStyle = FlatStyle.Flat;
        btnPost.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnPost.ForeColor = Color.White;
        btnPost.Location = new Point(298, 578);
        btnPost.Name = "btnPost";
        btnPost.Size = new Size(170, 50);
        btnPost.TabIndex = 8;
        btnPost.Text = "اعتماد الجرد";
        btnPost.UseVisualStyleBackColor = false;
        btnPost.Click += btnPost_Click;
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
        btnCancel.Location = new Point(12, 578);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(120, 50);
        btnCancel.TabIndex = 6;
        btnCancel.Text = "إلغاء";
        btnCancel.UseVisualStyleBackColor = false;
        btnCancel.Click += btnCancel_Click;
        // 
        // frmStocktakingNew
        // 
        AcceptButton = btnPost;
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.WhiteSmoke;
        CancelButton = btnCancel;
        ClientSize = new Size(820, 640);
        Controls.Add(btnCancel);
        Controls.Add(btnPost);
        Controls.Add(btnSaveDraft);
        Controls.Add(grpTotals);
        Controls.Add(dgvLines);
        Controls.Add(grpCommittee);
        Controls.Add(grpHeader);
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
        grpHeader.ResumeLayout(false);
        grpHeader.PerformLayout();
        grpCommittee.ResumeLayout(false);
        grpCommittee.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvLines).EndInit();
        grpTotals.ResumeLayout(false);
        grpTotals.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    private ErrorProvider errorProvider;
    private Label lblMode;
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
    private Button btnSaveDraft;
    private Button btnPost;
    private Button btnCancel;
}