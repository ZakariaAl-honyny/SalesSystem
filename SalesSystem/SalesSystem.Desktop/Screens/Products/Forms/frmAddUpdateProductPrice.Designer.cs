namespace SalesSystem.Desktop.Products.Forms;

partial class frmAddUpdateProductPrice
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        errorProvider = new ErrorProvider(components);
        tooltip = new ToolTip(components);
        numPrice = new NumericUpDown();
        cmbProductUnit = new ComboBox();
        dtpEffectiveFrom = new DateTimePicker();
        dtpEffectiveTo = new DateTimePicker();
        chkNoEndDate = new CheckBox();
        mainLayout = new TableLayoutPanel();
        lblMode = new Label();
        lblPrice = new Label();
        lblUnit = new Label();
        unitPanel = new TableLayoutPanel();
        lblFrom = new Label();
        lblTo = new Label();
        toPanel = new TableLayoutPanel();
        buttonPanel = new FlowLayoutPanel();
        btnCancel = new Button();
        btnSave = new Button();
        ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numPrice).BeginInit();
        mainLayout.SuspendLayout();
        unitPanel.SuspendLayout();
        toPanel.SuspendLayout();
        buttonPanel.SuspendLayout();
        SuspendLayout();
        // 
        // errorProvider
        // 
        errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        errorProvider.ContainerControl = this;
        // 
        // numPrice
        // 
        numPrice.DecimalPlaces = 2;
        numPrice.Dock = DockStyle.Fill;
        numPrice.Font = new Font("Tahoma", 12F);
        numPrice.Location = new Point(23, 63);
        numPrice.Maximum = new decimal(new int[] { 999999999, 0, 0, 131072 });
        numPrice.Name = "numPrice";
        numPrice.Size = new Size(442, 36);
        numPrice.TabIndex = 2;
        numPrice.ThousandsSeparator = true;
        tooltip.SetToolTip(numPrice, "سعر البيع للوحدة. أدخل السعر النهائي بعد الخصم إن وجد.");
        // 
        // cmbProductUnit
        // 
        cmbProductUnit.Dock = DockStyle.Fill;
        cmbProductUnit.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbProductUnit.Font = new Font("Tahoma", 12F);
        cmbProductUnit.Location = new Point(3, 3);
        cmbProductUnit.Name = "cmbProductUnit";
        cmbProductUnit.Size = new Size(436, 37);
        cmbProductUnit.TabIndex = 0;
        tooltip.SetToolTip(cmbProductUnit, "اختر الوحدة التي ينطبق عليها هذا السعر");
        // 
        // dtpEffectiveFrom
        // 
        dtpEffectiveFrom.Dock = DockStyle.Fill;
        dtpEffectiveFrom.Font = new Font("Tahoma", 12F);
        dtpEffectiveFrom.Format = DateTimePickerFormat.Short;
        dtpEffectiveFrom.Location = new Point(23, 143);
        dtpEffectiveFrom.Name = "dtpEffectiveFrom";
        dtpEffectiveFrom.Size = new Size(442, 36);
        dtpEffectiveFrom.TabIndex = 6;
        tooltip.SetToolTip(dtpEffectiveFrom, "تاريخ بدء سريان السعر");
        // 
        // dtpEffectiveTo
        // 
        dtpEffectiveTo.Dock = DockStyle.Fill;
        dtpEffectiveTo.Font = new Font("Tahoma", 12F);
        dtpEffectiveTo.Format = DateTimePickerFormat.Short;
        dtpEffectiveTo.Location = new Point(113, 3);
        dtpEffectiveTo.Name = "dtpEffectiveTo";
        dtpEffectiveTo.Size = new Size(326, 36);
        dtpEffectiveTo.TabIndex = 0;
        tooltip.SetToolTip(dtpEffectiveTo, "تاريخ انتهاء صلاحية السعر (اختياري)");
        // 
        // chkNoEndDate
        // 
        chkNoEndDate.Dock = DockStyle.Fill;
        chkNoEndDate.Font = new Font("Tahoma", 10F);
        chkNoEndDate.Location = new Point(3, 3);
        chkNoEndDate.Name = "chkNoEndDate";
        chkNoEndDate.Size = new Size(104, 28);
        chkNoEndDate.TabIndex = 1;
        chkNoEndDate.Text = "بدون تاريخ انتهاء";
        chkNoEndDate.TextAlign = ContentAlignment.MiddleRight;
        tooltip.SetToolTip(chkNoEndDate, "فعّل إذا كان السعر ساري المفعول بدون تاريخ انتهاء");
        chkNoEndDate.CheckedChanged += chkNoEndDate_CheckedChanged;
        // 
        // mainLayout
        // 
        mainLayout.BackColor = Color.White;
        mainLayout.ColumnCount = 2;
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        mainLayout.Controls.Add(lblMode, 0, 0);
        mainLayout.Controls.Add(lblPrice, 0, 1);
        mainLayout.Controls.Add(numPrice, 1, 1);
        mainLayout.Controls.Add(lblUnit, 0, 2);
        mainLayout.Controls.Add(unitPanel, 1, 2);
        mainLayout.Controls.Add(lblFrom, 0, 3);
        mainLayout.Controls.Add(dtpEffectiveFrom, 1, 3);
        mainLayout.Controls.Add(lblTo, 0, 4);
        mainLayout.Controls.Add(toPanel, 1, 4);
        mainLayout.Controls.Add(buttonPanel, 0, 5);
        mainLayout.Dock = DockStyle.Fill;
        mainLayout.Location = new Point(0, 0);
        mainLayout.Name = "mainLayout";
        mainLayout.Padding = new Padding(20);
        mainLayout.RowCount = 7;
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        mainLayout.Size = new Size(618, 517);
        mainLayout.TabIndex = 0;
        // 
        // lblMode
        // 
        lblMode.AutoSize = true;
        mainLayout.SetColumnSpan(lblMode, 2);
        lblMode.Font = new Font("Tahoma", 14F, FontStyle.Bold);
        lblMode.ForeColor = Color.Firebrick;
        lblMode.Location = new Point(360, 20);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(235, 34);
        lblMode.TabIndex = 0;
        lblMode.Text = "إضافة سعر جديد";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblPrice
        // 
        lblPrice.Dock = DockStyle.Fill;
        lblPrice.Font = new Font("Tahoma", 12F, FontStyle.Bold);
        lblPrice.Location = new Point(471, 60);
        lblPrice.Name = "lblPrice";
        lblPrice.Size = new Size(124, 40);
        lblPrice.TabIndex = 1;
        lblPrice.Text = "السعر:";
        lblPrice.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblUnit
        // 
        lblUnit.Dock = DockStyle.Fill;
        lblUnit.Font = new Font("Tahoma", 12F, FontStyle.Bold);
        lblUnit.Location = new Point(471, 100);
        lblUnit.Name = "lblUnit";
        lblUnit.Size = new Size(124, 40);
        lblUnit.TabIndex = 3;
        lblUnit.Text = "الوحدة:";
        lblUnit.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // unitPanel
        // 
        unitPanel.ColumnCount = 1;
        unitPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
        unitPanel.Controls.Add(cmbProductUnit, 0, 0);
        unitPanel.Dock = DockStyle.Fill;
        unitPanel.Location = new Point(23, 103);
        unitPanel.Name = "unitPanel";
        unitPanel.RowCount = 1;
        unitPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
        unitPanel.Size = new Size(442, 34);
        unitPanel.TabIndex = 4;
        // 
        // lblFrom
        // 
        lblFrom.Dock = DockStyle.Fill;
        lblFrom.Font = new Font("Tahoma", 12F, FontStyle.Bold);
        lblFrom.Location = new Point(471, 140);
        lblFrom.Name = "lblFrom";
        lblFrom.Size = new Size(124, 40);
        lblFrom.TabIndex = 5;
        lblFrom.Text = "من تاريخ:";
        lblFrom.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblTo
        // 
        lblTo.Dock = DockStyle.Fill;
        lblTo.Font = new Font("Tahoma", 12F, FontStyle.Bold);
        lblTo.Location = new Point(471, 180);
        lblTo.Name = "lblTo";
        lblTo.Size = new Size(124, 40);
        lblTo.TabIndex = 7;
        lblTo.Text = "إلى تاريخ:";
        lblTo.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // toPanel
        // 
        toPanel.ColumnCount = 2;
        toPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        toPanel.ColumnStyles.Add(new ColumnStyle());
        toPanel.Controls.Add(dtpEffectiveTo, 0, 0);
        toPanel.Controls.Add(chkNoEndDate, 1, 0);
        toPanel.Dock = DockStyle.Fill;
        toPanel.Location = new Point(23, 183);
        toPanel.Name = "toPanel";
        toPanel.RowCount = 1;
        toPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
        toPanel.Size = new Size(442, 34);
        toPanel.TabIndex = 8;
        // 
        // buttonPanel
        // 
        mainLayout.SetColumnSpan(buttonPanel, 2);
        buttonPanel.Controls.Add(btnCancel);
        buttonPanel.Controls.Add(btnSave);
        buttonPanel.Dock = DockStyle.Fill;
        buttonPanel.FlowDirection = FlowDirection.RightToLeft;
        buttonPanel.Location = new Point(23, 223);
        buttonPanel.Name = "buttonPanel";
        buttonPanel.Padding = new Padding(0, 15, 0, 0);
        mainLayout.SetRowSpan(buttonPanel, 2);
        buttonPanel.Size = new Size(572, 271);
        buttonPanel.TabIndex = 9;
        // 
        // btnCancel
        // 
        btnCancel.AutoSize = true;
        btnCancel.BackColor = Color.FromArgb(108, 117, 125);
        btnCancel.Cursor = Cursors.Hand;
        btnCancel.FlatAppearance.BorderSize = 0;
        btnCancel.FlatStyle = FlatStyle.Flat;
        btnCancel.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        btnCancel.ForeColor = Color.White;
        btnCancel.Location = new Point(3, 18);
        btnCancel.MinimumSize = new Size(100, 35);
        btnCancel.Name = "btnCancel";
        btnCancel.Padding = new Padding(12, 0, 12, 0);
        btnCancel.Size = new Size(100, 35);
        btnCancel.TabIndex = 1;
        btnCancel.Text = "إلغاء";
        btnCancel.UseVisualStyleBackColor = false;
        btnCancel.Click += btnCancel_Click;
        // 
        // btnSave
        // 
        btnSave.AutoSize = true;
        btnSave.BackColor = Color.FromArgb(25, 135, 84);
        btnSave.Cursor = Cursors.Hand;
        btnSave.FlatAppearance.BorderSize = 0;
        btnSave.FlatStyle = FlatStyle.Flat;
        btnSave.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        btnSave.ForeColor = Color.White;
        btnSave.Location = new Point(109, 18);
        btnSave.MinimumSize = new Size(100, 35);
        btnSave.Name = "btnSave";
        btnSave.Padding = new Padding(12, 0, 12, 0);
        btnSave.Size = new Size(100, 35);
        btnSave.TabIndex = 0;
        btnSave.Text = "حفظ";
        btnSave.UseVisualStyleBackColor = false;
        btnSave.Click += btnSave_Click;
        // 
        // frmAddUpdateProductPrice
        // 
        BackColor = Color.WhiteSmoke;
        ClientSize = new Size(618, 517);
        Controls.Add(mainLayout);
        Font = new Font("Tahoma", 12F);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "frmAddUpdateProductPrice";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterParent;
        Text = "إضافة سعر جديد";
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        ((System.ComponentModel.ISupportInitialize)numPrice).EndInit();
        mainLayout.ResumeLayout(false);
        mainLayout.PerformLayout();
        unitPanel.ResumeLayout(false);
        toPanel.ResumeLayout(false);
        buttonPanel.ResumeLayout(false);
        buttonPanel.PerformLayout();
        ResumeLayout(false);
    }

    private TableLayoutPanel mainLayout;
    private TableLayoutPanel unitPanel;
    private TableLayoutPanel toPanel;
    private FlowLayoutPanel buttonPanel;
    private Label lblMode;
    private ComboBox cmbProductUnit;
    private NumericUpDown numPrice;
    private DateTimePicker dtpEffectiveFrom;
    private DateTimePicker dtpEffectiveTo;
    private CheckBox chkNoEndDate;
    private Button btnSave;
    private Button btnCancel;
    private ErrorProvider errorProvider;
    private ToolTip tooltip;
    private Label lblPrice;
    private Label lblUnit;
    private Label lblFrom;
    private Label lblTo;
}
