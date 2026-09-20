namespace SalesSystem.Desktop.Screens.Products.Forms;

partial class frmPriceHistory
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
        lblMode = new Label();
        dgvPrices = new DataGridView();
        btnAddPrice = new Button();
        btnClose = new Button();
        lblRecordsCount = new Label();
        label1 = new Label();
        cmbUnit = new ComboBox();
        label2 = new Label();
        tooltip = new ToolTip(components);
        ((System.ComponentModel.ISupportInitialize)dgvPrices).BeginInit();
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
        lblMode.Size = new Size(900, 39);
        lblMode.TabIndex = 30;
        lblMode.Text = "ط³ط¬ظ„ ط£ط³ط¹ط§ط± ط§ظ„ظ…ظ†طھط¬";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // dgvPrices
        // 
        dgvPrices.AllowUserToAddRows = false;
        dgvPrices.AllowUserToDeleteRows = false;
        dgvPrices.AllowUserToOrderColumns = true;
        dgvPrices.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvPrices.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvPrices.BackgroundColor = Color.White;
        dgvPrices.BorderStyle = BorderStyle.Fixed3D;
        dgvPrices.ColumnHeadersHeight = 40;
        dgvPrices.Location = new Point(12, 120);
        dgvPrices.MultiSelect = false;
        dgvPrices.Name = "dgvPrices";
        dgvPrices.ReadOnly = true;
        dgvPrices.RowHeadersVisible = false;
        dgvPrices.RowHeadersWidth = 62;
        dgvPrices.RowTemplate.Height = 28;
        dgvPrices.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvPrices.Size = new Size(876, 400);
        dgvPrices.TabIndex = 34;
        dgvPrices.TabStop = false;
        // 
        // btnAddPrice
        // 
        btnAddPrice.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnAddPrice.BackColor = Color.White;
        btnAddPrice.FlatAppearance.BorderColor = Color.Black;
        btnAddPrice.FlatAppearance.BorderSize = 2;
        btnAddPrice.FlatStyle = FlatStyle.Popup;
        btnAddPrice.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnAddPrice.ForeColor = Color.FromArgb(25, 135, 84);
        btnAddPrice.Location = new Point(739, 79);
        btnAddPrice.Name = "btnAddPrice";
        btnAddPrice.Size = new Size(149, 35);
        btnAddPrice.TabIndex = 5;
        btnAddPrice.Text = "+ ط³ط¹ط± ط¬ط¯ظٹط¯";
        tooltip.SetToolTip(btnAddPrice, "ط¥ط¶ط§ظپط© ط³ط¹ط± ط¨ظٹط¹ ط¬ط¯ظٹط¯ ظ„ظ„ظˆط­ط¯ط© ط§ظ„ظ…ط­ط¯ط¯ط©");
        btnAddPrice.UseVisualStyleBackColor = true;
        btnAddPrice.Click += btnAddPrice_Click;
        // 
        // btnClose
        // 
        btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnClose.BackColor = Color.White;
        btnClose.DialogResult = DialogResult.Cancel;
        btnClose.FlatAppearance.BorderColor = Color.Black;
        btnClose.FlatAppearance.BorderSize = 2;
        btnClose.FlatStyle = FlatStyle.Popup;
        btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnClose.ForeColor = Color.Black;
        btnClose.Location = new Point(12, 526);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(181, 61);
        btnClose.TabIndex = 0;
        btnClose.Text = "ط¥ط؛ظ„ط§ظ‚";
        tooltip.SetToolTip(btnClose, "ط¥ط؛ظ„ط§ظ‚ ظ†ط§ظپط°ط© ط³ط¬ظ„ ط§ظ„ط£ط³ط¹ط§ط±");
        btnClose.UseVisualStyleBackColor = true;
        btnClose.Click += btnClose_Click;
        // 
        // lblRecordsCount
        // 
        lblRecordsCount.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        lblRecordsCount.AutoSize = true;
        lblRecordsCount.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblRecordsCount.Location = new Point(838, 526);
        lblRecordsCount.Name = "lblRecordsCount";
        lblRecordsCount.Size = new Size(41, 24);
        lblRecordsCount.TabIndex = 4;
        lblRecordsCount.Text = "[0]";
        // 
        // label1
        // 
        label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        label1.AutoSize = true;
        label1.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        label1.Location = new Point(702, 526);
        label1.Name = "label1";
        label1.Size = new Size(130, 24);
        label1.TabIndex = 3;
        label1.Text = "ط¹ط¯ط¯ ط§ظ„ط£ط³ط¹ط§ط±:";
        // 
        // cmbUnit
        // 
        cmbUnit.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbUnit.Font = new Font("Tahoma", 10F);
        cmbUnit.FormattingEnabled = true;
        cmbUnit.Location = new Point(97, 82);
        cmbUnit.Name = "cmbUnit";
        cmbUnit.Size = new Size(300, 32);
        cmbUnit.TabIndex = 1;
        tooltip.SetToolTip(cmbUnit, "طھطµظپظٹط© ط§ظ„ط£ط³ط¹ط§ط± ط­ط³ط¨ ط§ظ„ظˆط­ط¯ط© ط§ظ„ظ…ط­ط¯ط¯ط©");
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        label2.Location = new Point(12, 86);
        label2.Name = "label2";
        label2.Size = new Size(79, 24);
        label2.TabIndex = 6;
        label2.Text = "ط§ظ„ظˆط­ط¯ط©:";
        // 
        // frmPriceHistory
        // 
        AcceptButton = btnAddPrice;
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.WhiteSmoke;
        CancelButton = btnClose;
        ClientSize = new Size(900, 613);
        Controls.Add(btnAddPrice);
        Controls.Add(btnClose);
        Controls.Add(lblRecordsCount);
        Controls.Add(label1);
        Controls.Add(cmbUnit);
        Controls.Add(label2);
        Controls.Add(dgvPrices);
        Controls.Add(lblMode);
        Font = new Font("Tahoma", 12F);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "frmPriceHistory";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterParent;
        Text = "ط³ط¬ظ„ ط£ط³ط¹ط§ط± ط§ظ„ظ…ظ†طھط¬";
        Load += frmPriceHistory_Load;
        ((System.ComponentModel.ISupportInitialize)dgvPrices).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    private Label lblMode;
    private DataGridView dgvPrices;
    private Button btnAddPrice;
    private Button btnClose;
    private Label lblRecordsCount;
    private Label label1;
    private ComboBox cmbUnit;
    private Label label2;
    private ToolTip tooltip;
}

