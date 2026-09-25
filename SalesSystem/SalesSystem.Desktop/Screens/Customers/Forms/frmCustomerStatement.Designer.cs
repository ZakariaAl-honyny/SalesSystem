namespace SalesSystem.Desktop.Screens.Customers.Forms;

partial class frmCustomerStatement
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
        pnlHeader = new Panel();
        lblMode = new Label();
        pnlFilter = new Panel();
        flowFilter = new FlowLayoutPanel();
        lblCustomerCaption = new Label();
        cmbCustomer = new ComboBox();
        lblCustomerInfo = new Label();
        lblFrom = new Label();
        dtpFrom = new DateTimePicker();
        lblTo = new Label();
        dtpTo = new DateTimePicker();
        btnRefresh = new Button();
        pnlStats = new Panel();
        flowCards = new FlowLayoutPanel();
        cardOpening = new Panel();
        cardOpeningAccent = new Label();
        cardOpeningTitle = new Label();
        cardOpeningValue = new Label();
        cardDebit = new Panel();
        cardDebitAccent = new Label();
        cardDebitTitle = new Label();
        cardDebitValue = new Label();
        cardCredit = new Panel();
        cardCreditAccent = new Label();
        cardCreditTitle = new Label();
        cardCreditValue = new Label();
        cardClosing = new Panel();
        cardClosingAccent = new Label();
        cardClosingTitle = new Label();
        cardClosingValue = new Label();
        lblAgingTitle = new Label();
        dgvAging = new DataGridView();
        pnlFooter = new Panel();
        lblFooterLine = new Label();
        btnClose = new Button();
        lblRecordsLabel = new Label();
        lblRecordsCount = new Label();
        dgvStatement = new DataGridView();
        splitMain = new SplitContainer();
        pnlHeader.SuspendLayout();
        pnlFilter.SuspendLayout();
        flowFilter.SuspendLayout();
        pnlStats.SuspendLayout();
        flowCards.SuspendLayout();
        cardOpening.SuspendLayout();
        cardDebit.SuspendLayout();
        cardCredit.SuspendLayout();
        cardClosing.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvAging).BeginInit();
        pnlFooter.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvStatement).BeginInit();
        ((System.ComponentModel.ISupportInitialize)splitMain).BeginInit();
        splitMain.Panel1.SuspendLayout();
        splitMain.Panel2.SuspendLayout();
        splitMain.SuspendLayout();
        SuspendLayout();
        // 
        // pnlHeader
        // 
        pnlHeader.BackColor = Color.FromArgb(33, 43, 54);
        pnlHeader.Controls.Add(lblMode);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Name = "pnlHeader";
        pnlHeader.Size = new Size(880, 56);
        pnlHeader.TabIndex = 0;
        // 
        // lblMode
        // 
        lblMode.Dock = DockStyle.Fill;
        lblMode.Font = new Font("Tahoma", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblMode.ForeColor = Color.White;
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(880, 56);
        lblMode.TabIndex = 0;
        lblMode.Text = "كشف حساب عميل";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlFilter
        // 
        pnlFilter.BackColor = Color.White;
        pnlFilter.Controls.Add(flowFilter);
        pnlFilter.Dock = DockStyle.Top;
        pnlFilter.Name = "pnlFilter";
        pnlFilter.Size = new Size(880, 54);
        pnlFilter.TabIndex = 1;
        // 
        // flowFilter
        // 
        flowFilter.Controls.Add(lblCustomerCaption);
        flowFilter.Controls.Add(cmbCustomer);
        flowFilter.Controls.Add(lblCustomerInfo);
        flowFilter.Controls.Add(lblFrom);
        flowFilter.Controls.Add(dtpFrom);
        flowFilter.Controls.Add(lblTo);
        flowFilter.Controls.Add(dtpTo);
        flowFilter.Controls.Add(btnRefresh);
        flowFilter.Dock = DockStyle.Fill;
        flowFilter.FlowDirection = FlowDirection.RightToLeft;
        flowFilter.Name = "flowFilter";
        flowFilter.Padding = new Padding(8, 2, 8, 2);
        flowFilter.WrapContents = false;
        // 
        // lblCustomerCaption
        // 
        lblCustomerCaption.AutoSize = true;
        lblCustomerCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblCustomerCaption.ForeColor = Color.FromArgb(52, 73, 94);
        lblCustomerCaption.Location = new Point(3, 15);
        lblCustomerCaption.Margin = new Padding(4, 12, 2, 12);
        lblCustomerCaption.Name = "lblCustomerCaption";
        lblCustomerCaption.Size = new Size(55, 24);
        lblCustomerCaption.TabIndex = 0;
        lblCustomerCaption.Text = "العميل:";
        // 
        // cmbCustomer
        // 
        cmbCustomer.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbCustomer.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        cmbCustomer.Location = new Point(62, 12);
        cmbCustomer.Margin = new Padding(2, 12, 2, 12);
        cmbCustomer.Name = "cmbCustomer";
        cmbCustomer.Size = new Size(220, 32);
        cmbCustomer.TabIndex = 1;
        cmbCustomer.SelectedIndexChanged += cmbCustomer_SelectedIndexChanged;
        // 
        // lblCustomerInfo
        // 
        lblCustomerInfo.AutoSize = true;
        lblCustomerInfo.Font = new Font("Tahoma", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblCustomerInfo.ForeColor = Color.FromArgb(108, 117, 125);
        lblCustomerInfo.Location = new Point(286, 16);
        lblCustomerInfo.Margin = new Padding(2, 14, 8, 14);
        lblCustomerInfo.Name = "lblCustomerInfo";
        lblCustomerInfo.Size = new Size(100, 20);
        lblCustomerInfo.TabIndex = 2;
        lblCustomerInfo.Text = "";
        // 
        // lblFrom
        // 
        lblFrom.AutoSize = true;
        lblFrom.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblFrom.ForeColor = Color.FromArgb(52, 73, 94);
        lblFrom.Location = new Point(396, 15);
        lblFrom.Margin = new Padding(4, 12, 2, 12);
        lblFrom.Name = "lblFrom";
        lblFrom.Size = new Size(42, 24);
        lblFrom.TabIndex = 3;
        lblFrom.Text = "من:";
        // 
        // dtpFrom
        // 
        dtpFrom.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        dtpFrom.Format = DateTimePickerFormat.Short;
        dtpFrom.Location = new Point(442, 12);
        dtpFrom.Margin = new Padding(2, 12, 2, 12);
        dtpFrom.Name = "dtpFrom";
        dtpFrom.Size = new Size(120, 30);
        dtpFrom.TabIndex = 4;
        dtpFrom.ValueChanged += dtpFrom_ValueChanged;
        // 
        // lblTo
        // 
        lblTo.AutoSize = true;
        lblTo.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblTo.ForeColor = Color.FromArgb(52, 73, 94);
        lblTo.Location = new Point(566, 15);
        lblTo.Margin = new Padding(4, 12, 2, 12);
        lblTo.Name = "lblTo";
        lblTo.Size = new Size(40, 24);
        lblTo.TabIndex = 5;
        lblTo.Text = "إلى:";
        // 
        // dtpTo
        // 
        dtpTo.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        dtpTo.Format = DateTimePickerFormat.Short;
        dtpTo.Location = new Point(610, 12);
        dtpTo.Margin = new Padding(2, 12, 2, 12);
        dtpTo.Name = "dtpTo";
        dtpTo.Size = new Size(120, 30);
        dtpTo.TabIndex = 6;
        dtpTo.ValueChanged += dtpTo_ValueChanged;
        // 
        // btnRefresh
        // 
        btnRefresh.BackColor = Color.FromArgb(41, 128, 185);
        btnRefresh.FlatStyle = FlatStyle.Flat;
        btnRefresh.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnRefresh.ForeColor = Color.White;
        btnRefresh.Location = new Point(734, 8);
        btnRefresh.Margin = new Padding(2, 8, 2, 8);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(80, 34);
        btnRefresh.TabIndex = 7;
        btnRefresh.Text = "تحديث";
        btnRefresh.UseVisualStyleBackColor = false;
        btnRefresh.Click += btnRefresh_Click;
        // 
        // pnlStats
        // 
        pnlStats.BackColor = Color.WhiteSmoke;
        pnlStats.Controls.Add(flowCards);
        pnlStats.Dock = DockStyle.Top;
        pnlStats.Name = "pnlStats";
        pnlStats.Size = new Size(880, 100);
        pnlStats.TabIndex = 2;
        // 
        // flowCards
        // 
        flowCards.Controls.Add(cardOpening);
        flowCards.Controls.Add(cardDebit);
        flowCards.Controls.Add(cardCredit);
        flowCards.Controls.Add(cardClosing);
        flowCards.Dock = DockStyle.Fill;
        flowCards.FlowDirection = FlowDirection.RightToLeft;
        flowCards.Name = "flowCards";
        flowCards.Padding = new Padding(8, 6, 8, 6);
        // 
        // cardOpening
        // 
        cardOpening.BackColor = Color.White;
        cardOpening.Controls.Add(cardOpeningAccent);
        cardOpening.Controls.Add(cardOpeningTitle);
        cardOpening.Controls.Add(cardOpeningValue);
        cardOpening.Margin = new Padding(8, 6, 8, 6);
        cardOpening.Name = "cardOpening";
        cardOpening.Padding = new Padding(10, 4, 10, 4);
        cardOpening.Size = new Size(190, 72);
        cardOpening.TabIndex = 0;
        // 
        // cardOpeningAccent
        // 
        cardOpeningAccent.BackColor = Color.FromArgb(149, 165, 166);
        cardOpeningAccent.Dock = DockStyle.Bottom;
        cardOpeningAccent.Height = 3;
        cardOpeningAccent.Name = "cardOpeningAccent";
        cardOpeningAccent.TabIndex = 0;
        // 
        // cardOpeningTitle
        // 
        cardOpeningTitle.AutoSize = true;
        cardOpeningTitle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        cardOpeningTitle.ForeColor = Color.FromArgb(108, 117, 125);
        cardOpeningTitle.Location = new Point(10, 4);
        cardOpeningTitle.Name = "cardOpeningTitle";
        cardOpeningTitle.Size = new Size(85, 18);
        cardOpeningTitle.TabIndex = 1;
        cardOpeningTitle.Text = "الرصيد السابق";
        // 
        // cardOpeningValue
        // 
        cardOpeningValue.AutoSize = true;
        cardOpeningValue.Font = new Font("Tahoma", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
        cardOpeningValue.ForeColor = Color.FromArgb(52, 73, 94);
        cardOpeningValue.Location = new Point(10, 28);
        cardOpeningValue.Name = "cardOpeningValue";
        cardOpeningValue.Size = new Size(65, 29);
        cardOpeningValue.TabIndex = 2;
        cardOpeningValue.Text = "0.00";
        // 
        // cardDebit
        // 
        cardDebit.BackColor = Color.White;
        cardDebit.Controls.Add(cardDebitAccent);
        cardDebit.Controls.Add(cardDebitTitle);
        cardDebit.Controls.Add(cardDebitValue);
        cardDebit.Margin = new Padding(8, 6, 8, 6);
        cardDebit.Name = "cardDebit";
        cardDebit.Padding = new Padding(10, 4, 10, 4);
        cardDebit.Size = new Size(190, 72);
        cardDebit.TabIndex = 1;
        // 
        // cardDebitAccent
        // 
        cardDebitAccent.BackColor = Color.FromArgb(192, 57, 43);
        cardDebitAccent.Dock = DockStyle.Bottom;
        cardDebitAccent.Height = 3;
        cardDebitAccent.Name = "cardDebitAccent";
        cardDebitAccent.TabIndex = 0;
        // 
        // cardDebitTitle
        // 
        cardDebitTitle.AutoSize = true;
        cardDebitTitle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        cardDebitTitle.ForeColor = Color.FromArgb(108, 117, 125);
        cardDebitTitle.Location = new Point(10, 4);
        cardDebitTitle.Name = "cardDebitTitle";
        cardDebitTitle.Size = new Size(98, 18);
        cardDebitTitle.TabIndex = 1;
        cardDebitTitle.Text = "إجمالي المدين";
        // 
        // cardDebitValue
        // 
        cardDebitValue.AutoSize = true;
        cardDebitValue.Font = new Font("Tahoma", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
        cardDebitValue.ForeColor = Color.FromArgb(192, 57, 43);
        cardDebitValue.Location = new Point(10, 28);
        cardDebitValue.Name = "cardDebitValue";
        cardDebitValue.Size = new Size(65, 29);
        cardDebitValue.TabIndex = 2;
        cardDebitValue.Text = "0.00";
        // 
        // cardCredit
        // 
        cardCredit.BackColor = Color.White;
        cardCredit.Controls.Add(cardCreditAccent);
        cardCredit.Controls.Add(cardCreditTitle);
        cardCredit.Controls.Add(cardCreditValue);
        cardCredit.Margin = new Padding(8, 6, 8, 6);
        cardCredit.Name = "cardCredit";
        cardCredit.Padding = new Padding(10, 4, 10, 4);
        cardCredit.Size = new Size(190, 72);
        cardCredit.TabIndex = 2;
        // 
        // cardCreditAccent
        // 
        cardCreditAccent.BackColor = Color.FromArgb(39, 174, 96);
        cardCreditAccent.Dock = DockStyle.Bottom;
        cardCreditAccent.Height = 3;
        cardCreditAccent.Name = "cardCreditAccent";
        cardCreditAccent.TabIndex = 0;
        // 
        // cardCreditTitle
        // 
        cardCreditTitle.AutoSize = true;
        cardCreditTitle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        cardCreditTitle.ForeColor = Color.FromArgb(108, 117, 125);
        cardCreditTitle.Location = new Point(10, 4);
        cardCreditTitle.Name = "cardCreditTitle";
        cardCreditTitle.Size = new Size(98, 18);
        cardCreditTitle.TabIndex = 1;
        cardCreditTitle.Text = "إجمالي الدائن";
        // 
        // cardCreditValue
        // 
        cardCreditValue.AutoSize = true;
        cardCreditValue.Font = new Font("Tahoma", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
        cardCreditValue.ForeColor = Color.FromArgb(39, 174, 96);
        cardCreditValue.Location = new Point(10, 28);
        cardCreditValue.Name = "cardCreditValue";
        cardCreditValue.Size = new Size(65, 29);
        cardCreditValue.TabIndex = 2;
        cardCreditValue.Text = "0.00";
        // 
        // cardClosing
        // 
        cardClosing.BackColor = Color.White;
        cardClosing.Controls.Add(cardClosingAccent);
        cardClosing.Controls.Add(cardClosingTitle);
        cardClosing.Controls.Add(cardClosingValue);
        cardClosing.Margin = new Padding(8, 6, 8, 6);
        cardClosing.Name = "cardClosing";
        cardClosing.Padding = new Padding(10, 4, 10, 4);
        cardClosing.Size = new Size(190, 72);
        cardClosing.TabIndex = 3;
        // 
        // cardClosingAccent
        // 
        cardClosingAccent.BackColor = Color.FromArgb(243, 156, 18);
        cardClosingAccent.Dock = DockStyle.Bottom;
        cardClosingAccent.Height = 3;
        cardClosingAccent.Name = "cardClosingAccent";
        cardClosingAccent.TabIndex = 0;
        // 
        // cardClosingTitle
        // 
        cardClosingTitle.AutoSize = true;
        cardClosingTitle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        cardClosingTitle.ForeColor = Color.FromArgb(108, 117, 125);
        cardClosingTitle.Location = new Point(10, 4);
        cardClosingTitle.Name = "cardClosingTitle";
        cardClosingTitle.Size = new Size(90, 18);
        cardClosingTitle.TabIndex = 1;
        cardClosingTitle.Text = "الرصيد النهائي";
        // 
        // cardClosingValue
        // 
        cardClosingValue.AutoSize = true;
        cardClosingValue.Font = new Font("Tahoma", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
        cardClosingValue.ForeColor = Color.FromArgb(52, 73, 94);
        cardClosingValue.Location = new Point(10, 28);
        cardClosingValue.Name = "cardClosingValue";
        cardClosingValue.Size = new Size(65, 29);
        cardClosingValue.TabIndex = 2;
        cardClosingValue.Text = "0.00";
        // 
        // splitMain
        // 
        splitMain.Dock = DockStyle.Fill;
        splitMain.Name = "splitMain";
        splitMain.Orientation = Orientation.Horizontal;
        splitMain.SplitterDistance = 320;
        splitMain.TabIndex = 3;
        // 
        // splitMain.Panel1
        // 
        splitMain.Panel1.Controls.Add(dgvStatement);
        // 
        // splitMain.Panel2
        // 
        splitMain.Panel2.Controls.Add(dgvAging);
        splitMain.Panel2.Controls.Add(lblAgingTitle);
        splitMain.Panel2MinSize = 120;
        // 
        // lblAgingTitle
        // 
        lblAgingTitle.BackColor = Color.FromArgb(52, 73, 94);
        lblAgingTitle.Dock = DockStyle.Top;
        lblAgingTitle.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblAgingTitle.ForeColor = Color.White;
        lblAgingTitle.Name = "lblAgingTitle";
        lblAgingTitle.Size = new Size(880, 28);
        lblAgingTitle.TabIndex = 0;
        lblAgingTitle.Text = "تحليل أعمار الديون";
        lblAgingTitle.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // dgvAging
        // 
        dgvAging.AllowUserToAddRows = false;
        dgvAging.AllowUserToDeleteRows = false;
        dgvAging.AllowUserToResizeRows = false;
        dgvAging.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvAging.BackgroundColor = Color.White;
        dgvAging.BorderStyle = BorderStyle.None;
        dgvAging.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        dgvAging.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        dgvAging.ColumnHeadersHeight = 36;
        dgvAging.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        dgvAging.Dock = DockStyle.Fill;
        dgvAging.EnableHeadersVisualStyles = false;
        dgvAging.GridColor = Color.FromArgb(222, 226, 230);
        dgvAging.MultiSelect = false;
        dgvAging.Name = "dgvAging";
        dgvAging.ReadOnly = true;
        dgvAging.RowHeadersVisible = false;
        dgvAging.RowTemplate.Height = 28;
        dgvAging.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvAging.TabIndex = 1;
        dgvAging.TabStop = false;
        dgvAging.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(33, 43, 54);
        dgvAging.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        dgvAging.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        dgvAging.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        dgvAging.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(33, 43, 54);
        dgvAging.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
        dgvAging.DefaultCellStyle.BackColor = Color.White;
        dgvAging.DefaultCellStyle.ForeColor = Color.FromArgb(52, 73, 94);
        dgvAging.DefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        dgvAging.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 120, 215);
        dgvAging.DefaultCellStyle.SelectionForeColor = Color.White;
        dgvAging.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);
        dgvAging.CellFormatting += dgvAging_CellFormatting;
        // 
        // dgvStatement
        // 
        dgvStatement.AllowUserToAddRows = false;
        dgvStatement.AllowUserToDeleteRows = false;
        dgvStatement.AllowUserToResizeRows = false;
        dgvStatement.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvStatement.BackgroundColor = Color.White;
        dgvStatement.BorderStyle = BorderStyle.None;
        dgvStatement.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        dgvStatement.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        dgvStatement.ColumnHeadersHeight = 44;
        dgvStatement.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        dgvStatement.Dock = DockStyle.Fill;
        dgvStatement.EnableHeadersVisualStyles = false;
        dgvStatement.GridColor = Color.FromArgb(222, 226, 230);
        dgvStatement.MultiSelect = false;
        dgvStatement.Name = "dgvStatement";
        dgvStatement.ReadOnly = true;
        dgvStatement.RowHeadersVisible = false;
        dgvStatement.RowTemplate.Height = 32;
        dgvStatement.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvStatement.TabIndex = 4;
        dgvStatement.TabStop = false;
        dgvStatement.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(33, 43, 54);
        dgvStatement.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        dgvStatement.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        dgvStatement.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        dgvStatement.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(33, 43, 54);
        dgvStatement.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
        dgvStatement.DefaultCellStyle.BackColor = Color.White;
        dgvStatement.DefaultCellStyle.ForeColor = Color.FromArgb(52, 73, 94);
        dgvStatement.DefaultCellStyle.Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
        dgvStatement.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 120, 215);
        dgvStatement.DefaultCellStyle.SelectionForeColor = Color.White;
        dgvStatement.DefaultCellStyle.Padding = new Padding(6, 0, 6, 0);
        dgvStatement.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);
        dgvStatement.CellFormatting += dgvStatement_CellFormatting;
        // 
        // pnlFooter
        // 
        pnlFooter.BackColor = Color.FromArgb(248, 249, 250);
        pnlFooter.Controls.Add(btnClose);
        pnlFooter.Controls.Add(lblRecordsLabel);
        pnlFooter.Controls.Add(lblRecordsCount);
        pnlFooter.Controls.Add(lblFooterLine);
        pnlFooter.Dock = DockStyle.Bottom;
        pnlFooter.Name = "pnlFooter";
        pnlFooter.Size = new Size(880, 54);
        pnlFooter.TabIndex = 4;
        // 
        // lblFooterLine
        // 
        lblFooterLine.BackColor = Color.FromArgb(222, 226, 230);
        lblFooterLine.Dock = DockStyle.Top;
        lblFooterLine.Height = 2;
        lblFooterLine.Name = "lblFooterLine";
        lblFooterLine.TabIndex = 0;
        // 
        // btnClose
        // 
        btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnClose.BackColor = Color.White;
        btnClose.DialogResult = DialogResult.Cancel;
        btnClose.FlatAppearance.BorderColor = Color.FromArgb(222, 226, 230);
        btnClose.FlatAppearance.BorderSize = 1;
        btnClose.FlatStyle = FlatStyle.Flat;
        btnClose.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnClose.ForeColor = Color.FromArgb(52, 73, 94);
        btnClose.Location = new Point(12, 8);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(110, 38);
        btnClose.TabIndex = 0;
        btnClose.Text = "إغلاق";
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += btnClose_Click;
        // 
        // lblRecordsLabel
        // 
        lblRecordsLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        lblRecordsLabel.AutoSize = true;
        lblRecordsLabel.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblRecordsLabel.ForeColor = Color.FromArgb(108, 117, 125);
        lblRecordsLabel.Location = new Point(330, 15);
        lblRecordsLabel.Margin = new Padding(4, 0, 4, 0);
        lblRecordsLabel.Name = "lblRecordsLabel";
        lblRecordsLabel.Size = new Size(115, 24);
        lblRecordsLabel.TabIndex = 1;
        lblRecordsLabel.Text = "عدد الحركات:";
        // 
        // lblRecordsCount
        // 
        lblRecordsCount.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        lblRecordsCount.AutoSize = true;
        lblRecordsCount.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblRecordsCount.ForeColor = Color.FromArgb(52, 73, 94);
        lblRecordsCount.Location = new Point(500, 15);
        lblRecordsCount.Margin = new Padding(4, 0, 4, 0);
        lblRecordsCount.Name = "lblRecordsCount";
        lblRecordsCount.Size = new Size(30, 24);
        lblRecordsCount.TabIndex = 2;
        lblRecordsCount.Text = "0";
        // 
        // frmCustomerStatement
        // 
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.WhiteSmoke;
        CancelButton = btnClose;
        ClientSize = new Size(880, 640);
        Controls.Add(splitMain);
        Controls.Add(pnlFooter);
        Controls.Add(pnlStats);
        Controls.Add(pnlFilter);
        Controls.Add(pnlHeader);
        Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "frmCustomerStatement";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "كشف حساب عميل";
        Load += frmCustomerStatement_Load;
        pnlHeader.ResumeLayout(false);
        pnlFilter.ResumeLayout(false);
        flowFilter.ResumeLayout(false);
        flowFilter.PerformLayout();
        pnlStats.ResumeLayout(false);
        flowCards.ResumeLayout(false);
        cardOpening.ResumeLayout(false);
        cardOpening.PerformLayout();
        cardDebit.ResumeLayout(false);
        cardDebit.PerformLayout();
        cardCredit.ResumeLayout(false);
        cardCredit.PerformLayout();
        cardClosing.ResumeLayout(false);
        cardClosing.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvAging).EndInit();
        pnlFooter.ResumeLayout(false);
        pnlFooter.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvStatement).EndInit();
        splitMain.Panel1.ResumeLayout(false);
        splitMain.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitMain).EndInit();
        splitMain.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Panel pnlHeader;
    private Label lblMode;
    private Panel pnlFilter;
    private FlowLayoutPanel flowFilter;
    private Label lblCustomerCaption;
    private ComboBox cmbCustomer;
    private Label lblCustomerInfo;
    private Label lblFrom;
    private DateTimePicker dtpFrom;
    private Label lblTo;
    private DateTimePicker dtpTo;
    private Button btnRefresh;
    private Panel pnlStats;
    private FlowLayoutPanel flowCards;
    private Panel cardOpening;
    private Label cardOpeningAccent;
    private Label cardOpeningTitle;
    private Label cardOpeningValue;
    private Panel cardDebit;
    private Label cardDebitAccent;
    private Label cardDebitTitle;
    private Label cardDebitValue;
    private Panel cardCredit;
    private Label cardCreditAccent;
    private Label cardCreditTitle;
    private Label cardCreditValue;
    private Panel cardClosing;
    private Label cardClosingAccent;
    private Label cardClosingTitle;
    private Label cardClosingValue;
    private SplitContainer splitMain;
    private Label lblAgingTitle;
    private DataGridView dgvAging;
    private DataGridView dgvStatement;
    private Panel pnlFooter;
    private Label lblFooterLine;
    private Button btnClose;
    private Label lblRecordsLabel;
    private Label lblRecordsCount;
}
