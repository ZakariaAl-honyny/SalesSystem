namespace SalesSystem.Desktop.Screens.Accounting.Forms
{
    partial class frmAccounting
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            components?.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            cmsGrids = new ContextMenuStrip(components);
            refreshToolStripMenuItem = new ToolStripMenuItem();
            pnlHeader = new Panel();
            lblMode = new Label();
            lblMockBadge = new Label();
            tabMain = new TabControl();
            tpAccounts = new TabPage();
            pnlAccountsFilter = new Panel();
            lblAccountsSearch = new Label();
            cbFilterBy = new ComboBox();
            txtAccountsSearch = new TextBox();
            lblAccountsCount = new Label();
            dgvAccounts = new DataGridView();
            tpJournalEntries = new TabPage();
            pnlJournalFilter = new Panel();
            lblJvSearch = new Label();
            txtJvSearch = new TextBox();
            lblFrom = new Label();
            dtpFrom = new DateTimePicker();
            lblTo = new Label();
            dtpTo = new DateTimePicker();
            lblJvCount = new Label();
            dgvJournalEntries = new DataGridView();
            pnlTotals = new Panel();
            lblTotalDebit = new Label();
            lblTotalDebitVal = new Label();
            lblTotalCredit = new Label();
            lblTotalCreditVal = new Label();
            lblBalanceStatus = new Label();
            tpCostCenters = new TabPage();
            pnlCostFilter = new Panel();
            lblCostCaption = new Label();
            lblCostCount = new Label();
            dgvCostCenters = new DataGridView();
            tpFiscalPeriods = new TabPage();
            pnlPeriodsFilter = new Panel();
            lblPeriodsCaption = new Label();
            lblPeriodsCount = new Label();
            dgvFiscalPeriods = new DataGridView();
            pnlButtons = new Panel();
            lblFooterHint = new Label();
            btnClose = new Button();
            cmsGrids.SuspendLayout();
            pnlHeader.SuspendLayout();
            tabMain.SuspendLayout();
            tpAccounts.SuspendLayout();
            pnlAccountsFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAccounts).BeginInit();
            tpJournalEntries.SuspendLayout();
            pnlJournalFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvJournalEntries).BeginInit();
            pnlTotals.SuspendLayout();
            tpCostCenters.SuspendLayout();
            pnlCostFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCostCenters).BeginInit();
            tpFiscalPeriods.SuspendLayout();
            pnlPeriodsFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvFiscalPeriods).BeginInit();
            pnlButtons.SuspendLayout();
            SuspendLayout();

            // cmsGrids
            cmsGrids.BackColor = Color.White;
            cmsGrids.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmsGrids.ImageScalingSize = new Size(24, 24);
            cmsGrids.Items.AddRange(new ToolStripItem[] { refreshToolStripMenuItem });
            cmsGrids.Name = "cmsGrids";
            cmsGrids.RightToLeft = RightToLeft.Yes;
            cmsGrids.Size = new Size(200, 36);

            // refreshToolStripMenuItem
            refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            refreshToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.R;
            refreshToolStripMenuItem.Size = new Size(199, 30);
            refreshToolStripMenuItem.Text = "&تحديث البيانات";
            refreshToolStripMenuItem.Click += refreshToolStripMenuItem_Click;

            // pnlHeader
            pnlHeader.BackColor = Color.FromArgb(52, 73, 94);
            pnlHeader.Controls.Add(lblMode);
            pnlHeader.Controls.Add(lblMockBadge);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1150, 50);
            pnlHeader.TabIndex = 0;

            // lblMode
            lblMode.BackColor = Color.Transparent;
            lblMode.Dock = DockStyle.Fill;
            lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblMode.ForeColor = Color.White;
            lblMode.Location = new Point(0, 0);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(1150, 50);
            lblMode.TabIndex = 1;
            lblMode.Text = "الحسابات";
            lblMode.TextAlign = ContentAlignment.MiddleCenter;

            // lblMockBadge
            lblMockBadge.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblMockBadge.AutoSize = true;
            lblMockBadge.BackColor = Color.Transparent;
            lblMockBadge.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            lblMockBadge.ForeColor = Color.FromArgb(241, 196, 15);
            lblMockBadge.Location = new Point(940, 14);
            lblMockBadge.Name = "lblMockBadge";
            lblMockBadge.Size = new Size(175, 22);
            lblMockBadge.TabIndex = 2;
            lblMockBadge.Text = "بيانات تجريبية (Mock)";

            // tabMain
            tabMain.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabMain.Controls.Add(tpAccounts);
            tabMain.Controls.Add(tpJournalEntries);
            tabMain.Controls.Add(tpCostCenters);
            tabMain.Controls.Add(tpFiscalPeriods);
            tabMain.Font = new Font("Tahoma", 10F);
            tabMain.Location = new Point(10, 58);
            tabMain.Name = "tabMain";
            tabMain.RightToLeft = RightToLeft.Yes;
            tabMain.RightToLeftLayout = true;
            tabMain.SelectedIndex = 0;
            tabMain.Size = new Size(1130, 586);
            tabMain.TabIndex = 1;

            // tpAccounts
            tpAccounts.BackColor = Color.White;
            tpAccounts.Controls.Add(dgvAccounts);
            tpAccounts.Controls.Add(pnlAccountsFilter);
            tpAccounts.Location = new Point(4, 34);
            tpAccounts.Name = "tpAccounts";
            tpAccounts.Padding = new Padding(3);
            tpAccounts.Size = new Size(1122, 548);
            tpAccounts.TabIndex = 0;
            tpAccounts.Text = "دليل الحسابات";

            // pnlAccountsFilter
            pnlAccountsFilter.BackColor = Color.White;
            pnlAccountsFilter.Controls.Add(lblAccountsSearch);
            pnlAccountsFilter.Controls.Add(cbFilterBy);
            pnlAccountsFilter.Controls.Add(txtAccountsSearch);
            pnlAccountsFilter.Controls.Add(lblAccountsCount);
            pnlAccountsFilter.Dock = DockStyle.Top;
            pnlAccountsFilter.Location = new Point(3, 3);
            pnlAccountsFilter.Name = "pnlAccountsFilter";
            pnlAccountsFilter.Size = new Size(1116, 48);
            pnlAccountsFilter.TabIndex = 1;

            // lblAccountsSearch
            lblAccountsSearch.AutoSize = true;
            lblAccountsSearch.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblAccountsSearch.Location = new Point(12, 12);
            lblAccountsSearch.Name = "lblAccountsSearch";
            lblAccountsSearch.Size = new Size(44, 24);
            lblAccountsSearch.TabIndex = 0;
            lblAccountsSearch.Text = "بحث:";

            // cbFilterBy
            cbFilterBy.DropDownStyle = ComboBoxStyle.DropDownList;
            cbFilterBy.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            cbFilterBy.FormattingEnabled = true;
            cbFilterBy.Items.AddRange(new object[] { "بالاسم", "بالكود" });
            cbFilterBy.Location = new Point(70, 8);
            cbFilterBy.Name = "cbFilterBy";
            cbFilterBy.Size = new Size(140, 32);
            cbFilterBy.TabIndex = 1;
            cbFilterBy.SelectedIndex = 0;
            cbFilterBy.SelectedIndexChanged += cbFilterBy_SelectedIndexChanged;

            // txtAccountsSearch
            txtAccountsSearch.Font = new Font("Tahoma", 10F);
            txtAccountsSearch.Location = new Point(220, 8);
            txtAccountsSearch.Name = "txtAccountsSearch";
            txtAccountsSearch.Size = new Size(360, 32);
            txtAccountsSearch.TabIndex = 2;
            txtAccountsSearch.TextChanged += txtAccountsSearch_TextChanged;

            // lblAccountsCount
            lblAccountsCount.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblAccountsCount.AutoSize = true;
            lblAccountsCount.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblAccountsCount.Location = new Point(1040, 12);
            lblAccountsCount.Name = "lblAccountsCount";
            lblAccountsCount.Size = new Size(52, 24);
            lblAccountsCount.TabIndex = 3;
            lblAccountsCount.Text = "[0]";

            // dgvAccounts
            dgvAccounts.AllowUserToAddRows = false;
            dgvAccounts.AllowUserToDeleteRows = false;
            dgvAccounts.AllowUserToOrderColumns = true;
            dgvAccounts.BackgroundColor = SystemColors.Window;
            dgvAccounts.BorderStyle = BorderStyle.Fixed3D;
            dgvAccounts.ColumnHeadersHeight = 50;
            dgvAccounts.ContextMenuStrip = cmsGrids;
            dgvAccounts.Dock = DockStyle.Fill;
            dgvAccounts.Location = new Point(3, 51);
            dgvAccounts.MultiSelect = false;
            dgvAccounts.Name = "dgvAccounts";
            dgvAccounts.ReadOnly = true;
            dgvAccounts.RowHeadersWidth = 50;
            dgvAccounts.RowTemplate.Height = 28;
            dgvAccounts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAccounts.Size = new Size(1116, 494);
            dgvAccounts.TabIndex = 0;
            dgvAccounts.TabStop = false;

            // tpJournalEntries
            tpJournalEntries.BackColor = Color.White;
            tpJournalEntries.Controls.Add(dgvJournalEntries);
            tpJournalEntries.Controls.Add(pnlTotals);
            tpJournalEntries.Controls.Add(pnlJournalFilter);
            tpJournalEntries.Location = new Point(4, 34);
            tpJournalEntries.Name = "tpJournalEntries";
            tpJournalEntries.Padding = new Padding(3);
            tpJournalEntries.Size = new Size(1122, 548);
            tpJournalEntries.TabIndex = 1;
            tpJournalEntries.Text = "قيود اليومية";

            // pnlJournalFilter
            pnlJournalFilter.BackColor = Color.White;
            pnlJournalFilter.Controls.Add(lblJvSearch);
            pnlJournalFilter.Controls.Add(txtJvSearch);
            pnlJournalFilter.Controls.Add(lblFrom);
            pnlJournalFilter.Controls.Add(dtpFrom);
            pnlJournalFilter.Controls.Add(lblTo);
            pnlJournalFilter.Controls.Add(dtpTo);
            pnlJournalFilter.Controls.Add(lblJvCount);
            pnlJournalFilter.Dock = DockStyle.Top;
            pnlJournalFilter.Location = new Point(3, 3);
            pnlJournalFilter.Name = "pnlJournalFilter";
            pnlJournalFilter.Size = new Size(1116, 48);
            pnlJournalFilter.TabIndex = 2;

            // lblJvSearch
            lblJvSearch.AutoSize = true;
            lblJvSearch.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblJvSearch.Location = new Point(12, 12);
            lblJvSearch.Name = "lblJvSearch";
            lblJvSearch.Size = new Size(44, 24);
            lblJvSearch.TabIndex = 0;
            lblJvSearch.Text = "بحث:";

            // txtJvSearch
            txtJvSearch.Font = new Font("Tahoma", 10F);
            txtJvSearch.Location = new Point(70, 8);
            txtJvSearch.Name = "txtJvSearch";
            txtJvSearch.Size = new Size(300, 32);
            txtJvSearch.TabIndex = 1;
            txtJvSearch.TextChanged += txtJvSearch_TextChanged;

            // lblFrom
            lblFrom.AutoSize = true;
            lblFrom.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblFrom.Location = new Point(400, 12);
            lblFrom.Name = "lblFrom";
            lblFrom.Size = new Size(42, 24);
            lblFrom.TabIndex = 2;
            lblFrom.Text = "من:";

            // dtpFrom
            dtpFrom.CustomFormat = "yyyy/MM/dd";
            dtpFrom.Font = new Font("Tahoma", 10F);
            dtpFrom.Format = DateTimePickerFormat.Custom;
            dtpFrom.Location = new Point(440, 8);
            dtpFrom.Name = "dtpFrom";
            dtpFrom.Size = new Size(150, 32);
            dtpFrom.TabIndex = 3;
            dtpFrom.Value = new DateTime(2026, 1, 1);
            dtpFrom.ValueChanged += dtpFrom_ValueChanged;

            // lblTo
            lblTo.AutoSize = true;
            lblTo.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblTo.Location = new Point(620, 12);
            lblTo.Name = "lblTo";
            lblTo.Size = new Size(40, 24);
            lblTo.TabIndex = 4;
            lblTo.Text = "إلى:";

            // dtpTo
            dtpTo.CustomFormat = "yyyy/MM/dd";
            dtpTo.Font = new Font("Tahoma", 10F);
            dtpTo.Format = DateTimePickerFormat.Custom;
            dtpTo.Location = new Point(660, 8);
            dtpTo.Name = "dtpTo";
            dtpTo.Size = new Size(150, 32);
            dtpTo.TabIndex = 5;
            dtpTo.Value = new DateTime(2026, 12, 31);
            dtpTo.ValueChanged += dtpTo_ValueChanged;

            // lblJvCount
            lblJvCount.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblJvCount.AutoSize = true;
            lblJvCount.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblJvCount.Location = new Point(1040, 12);
            lblJvCount.Name = "lblJvCount";
            lblJvCount.Size = new Size(52, 24);
            lblJvCount.TabIndex = 6;
            lblJvCount.Text = "[0]";

            // dgvJournalEntries
            dgvJournalEntries.AllowUserToAddRows = false;
            dgvJournalEntries.AllowUserToDeleteRows = false;
            dgvJournalEntries.AllowUserToOrderColumns = true;
            dgvJournalEntries.BackgroundColor = SystemColors.Window;
            dgvJournalEntries.BorderStyle = BorderStyle.Fixed3D;
            dgvJournalEntries.ColumnHeadersHeight = 50;
            dgvJournalEntries.ContextMenuStrip = cmsGrids;
            dgvJournalEntries.Dock = DockStyle.Fill;
            dgvJournalEntries.Location = new Point(3, 51);
            dgvJournalEntries.MultiSelect = false;
            dgvJournalEntries.Name = "dgvJournalEntries";
            dgvJournalEntries.ReadOnly = true;
            dgvJournalEntries.RowHeadersWidth = 50;
            dgvJournalEntries.RowTemplate.Height = 28;
            dgvJournalEntries.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvJournalEntries.Size = new Size(1116, 448);
            dgvJournalEntries.TabIndex = 0;
            dgvJournalEntries.TabStop = false;

            // pnlTotals
            pnlTotals.BackColor = Color.FromArgb(240, 242, 245);
            pnlTotals.Controls.Add(lblTotalDebit);
            pnlTotals.Controls.Add(lblTotalDebitVal);
            pnlTotals.Controls.Add(lblTotalCredit);
            pnlTotals.Controls.Add(lblTotalCreditVal);
            pnlTotals.Controls.Add(lblBalanceStatus);
            pnlTotals.Dock = DockStyle.Bottom;
            pnlTotals.Location = new Point(3, 499);
            pnlTotals.Name = "pnlTotals";
            pnlTotals.Size = new Size(1116, 46);
            pnlTotals.TabIndex = 1;

            // lblTotalDebit
            lblTotalDebit.AutoSize = true;
            lblTotalDebit.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblTotalDebit.Location = new Point(20, 11);
            lblTotalDebit.Name = "lblTotalDebit";
            lblTotalDebit.Size = new Size(121, 24);
            lblTotalDebit.TabIndex = 0;
            lblTotalDebit.Text = "مجموع المدين:";

            // lblTotalDebitVal
            lblTotalDebitVal.AutoSize = true;
            lblTotalDebitVal.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblTotalDebitVal.ForeColor = Color.FromArgb(30, 58, 138);
            lblTotalDebitVal.Location = new Point(150, 11);
            lblTotalDebitVal.Name = "lblTotalDebitVal";
            lblTotalDebitVal.Size = new Size(52, 24);
            lblTotalDebitVal.TabIndex = 1;
            lblTotalDebitVal.Text = "0.00";

            // lblTotalCredit
            lblTotalCredit.AutoSize = true;
            lblTotalCredit.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblTotalCredit.Location = new Point(330, 11);
            lblTotalCredit.Name = "lblTotalCredit";
            lblTotalCredit.Size = new Size(120, 24);
            lblTotalCredit.TabIndex = 2;
            lblTotalCredit.Text = "مجموع الدائن:";

            // lblTotalCreditVal
            lblTotalCreditVal.AutoSize = true;
            lblTotalCreditVal.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblTotalCreditVal.ForeColor = Color.FromArgb(0, 100, 0);
            lblTotalCreditVal.Location = new Point(460, 11);
            lblTotalCreditVal.Name = "lblTotalCreditVal";
            lblTotalCreditVal.Size = new Size(52, 24);
            lblTotalCreditVal.TabIndex = 3;
            lblTotalCreditVal.Text = "0.00";

            // lblBalanceStatus
            lblBalanceStatus.AutoSize = true;
            lblBalanceStatus.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblBalanceStatus.ForeColor = Color.Green;
            lblBalanceStatus.Location = new Point(680, 11);
            lblBalanceStatus.Name = "lblBalanceStatus";
            lblBalanceStatus.Size = new Size(86, 24);
            lblBalanceStatus.TabIndex = 4;
            lblBalanceStatus.Text = "متوازن ✓";

            // tpCostCenters
            tpCostCenters.BackColor = Color.White;
            tpCostCenters.Controls.Add(dgvCostCenters);
            tpCostCenters.Controls.Add(pnlCostFilter);
            tpCostCenters.Location = new Point(4, 34);
            tpCostCenters.Name = "tpCostCenters";
            tpCostCenters.Padding = new Padding(3);
            tpCostCenters.Size = new Size(1122, 548);
            tpCostCenters.TabIndex = 2;
            tpCostCenters.Text = "مراكز التكلفة";

            // pnlCostFilter
            pnlCostFilter.BackColor = Color.White;
            pnlCostFilter.Controls.Add(lblCostCaption);
            pnlCostFilter.Controls.Add(lblCostCount);
            pnlCostFilter.Dock = DockStyle.Top;
            pnlCostFilter.Location = new Point(3, 3);
            pnlCostFilter.Name = "pnlCostFilter";
            pnlCostFilter.Size = new Size(1116, 40);
            pnlCostFilter.TabIndex = 1;

            // lblCostCaption
            lblCostCaption.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblCostCaption.AutoSize = true;
            lblCostCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblCostCaption.Location = new Point(880, 10);
            lblCostCaption.Name = "lblCostCaption";
            lblCostCaption.Size = new Size(152, 24);
            lblCostCaption.TabIndex = 0;
            lblCostCaption.Text = "عدد مراكز التكلفة:";

            // lblCostCount
            lblCostCount.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblCostCount.AutoSize = true;
            lblCostCount.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblCostCount.Location = new Point(1040, 10);
            lblCostCount.Name = "lblCostCount";
            lblCostCount.Size = new Size(52, 24);
            lblCostCount.TabIndex = 1;
            lblCostCount.Text = "[0]";

            // dgvCostCenters
            dgvCostCenters.AllowUserToAddRows = false;
            dgvCostCenters.AllowUserToDeleteRows = false;
            dgvCostCenters.AllowUserToOrderColumns = true;
            dgvCostCenters.BackgroundColor = SystemColors.Window;
            dgvCostCenters.BorderStyle = BorderStyle.Fixed3D;
            dgvCostCenters.ColumnHeadersHeight = 50;
            dgvCostCenters.ContextMenuStrip = cmsGrids;
            dgvCostCenters.Dock = DockStyle.Fill;
            dgvCostCenters.Location = new Point(3, 43);
            dgvCostCenters.MultiSelect = false;
            dgvCostCenters.Name = "dgvCostCenters";
            dgvCostCenters.ReadOnly = true;
            dgvCostCenters.RowHeadersWidth = 50;
            dgvCostCenters.RowTemplate.Height = 28;
            dgvCostCenters.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCostCenters.Size = new Size(1116, 502);
            dgvCostCenters.TabIndex = 0;
            dgvCostCenters.TabStop = false;

            // tpFiscalPeriods
            tpFiscalPeriods.BackColor = Color.White;
            tpFiscalPeriods.Controls.Add(dgvFiscalPeriods);
            tpFiscalPeriods.Controls.Add(pnlPeriodsFilter);
            tpFiscalPeriods.Location = new Point(4, 34);
            tpFiscalPeriods.Name = "tpFiscalPeriods";
            tpFiscalPeriods.Padding = new Padding(3);
            tpFiscalPeriods.Size = new Size(1122, 548);
            tpFiscalPeriods.TabIndex = 3;
            tpFiscalPeriods.Text = "الفترات المالية";

            // pnlPeriodsFilter
            pnlPeriodsFilter.BackColor = Color.White;
            pnlPeriodsFilter.Controls.Add(lblPeriodsCaption);
            pnlPeriodsFilter.Controls.Add(lblPeriodsCount);
            pnlPeriodsFilter.Dock = DockStyle.Top;
            pnlPeriodsFilter.Location = new Point(3, 3);
            pnlPeriodsFilter.Name = "pnlPeriodsFilter";
            pnlPeriodsFilter.Size = new Size(1116, 40);
            pnlPeriodsFilter.TabIndex = 1;

            // lblPeriodsCaption
            lblPeriodsCaption.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblPeriodsCaption.AutoSize = true;
            lblPeriodsCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblPeriodsCaption.Location = new Point(850, 10);
            lblPeriodsCaption.Name = "lblPeriodsCaption";
            lblPeriodsCaption.Size = new Size(182, 24);
            lblPeriodsCaption.TabIndex = 0;
            lblPeriodsCaption.Text = "عدد الفترات المالية:";

            // lblPeriodsCount
            lblPeriodsCount.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblPeriodsCount.AutoSize = true;
            lblPeriodsCount.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblPeriodsCount.Location = new Point(1040, 10);
            lblPeriodsCount.Name = "lblPeriodsCount";
            lblPeriodsCount.Size = new Size(52, 24);
            lblPeriodsCount.TabIndex = 1;
            lblPeriodsCount.Text = "[0]";

            // dgvFiscalPeriods
            dgvFiscalPeriods.AllowUserToAddRows = false;
            dgvFiscalPeriods.AllowUserToDeleteRows = false;
            dgvFiscalPeriods.AllowUserToOrderColumns = true;
            dgvFiscalPeriods.BackgroundColor = SystemColors.Window;
            dgvFiscalPeriods.BorderStyle = BorderStyle.Fixed3D;
            dgvFiscalPeriods.ColumnHeadersHeight = 50;
            dgvFiscalPeriods.ContextMenuStrip = cmsGrids;
            dgvFiscalPeriods.Dock = DockStyle.Fill;
            dgvFiscalPeriods.Location = new Point(3, 43);
            dgvFiscalPeriods.MultiSelect = false;
            dgvFiscalPeriods.Name = "dgvFiscalPeriods";
            dgvFiscalPeriods.ReadOnly = true;
            dgvFiscalPeriods.RowHeadersWidth = 50;
            dgvFiscalPeriods.RowTemplate.Height = 28;
            dgvFiscalPeriods.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvFiscalPeriods.Size = new Size(1116, 502);
            dgvFiscalPeriods.TabIndex = 0;
            dgvFiscalPeriods.TabStop = false;

            // pnlButtons
            pnlButtons.BackColor = Color.White;
            pnlButtons.Controls.Add(btnClose);
            pnlButtons.Controls.Add(lblFooterHint);
            pnlButtons.Dock = DockStyle.Bottom;
            pnlButtons.Location = new Point(0, 648);
            pnlButtons.Name = "pnlButtons";
            pnlButtons.Size = new Size(1150, 52);
            pnlButtons.TabIndex = 2;

            // lblFooterHint
            lblFooterHint.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            lblFooterHint.AutoSize = true;
            lblFooterHint.Font = new Font("Tahoma", 9F);
            lblFooterHint.ForeColor = Color.Gray;
            lblFooterHint.Location = new Point(760, 16);
            lblFooterHint.Name = "lblFooterHint";
            lblFooterHint.Size = new Size(378, 22);
            lblFooterHint.TabIndex = 1;
            lblFooterHint.Text = "نموذج تجريبي للواجهة — بيانات Mock محلية (بدون اتصال بقاعدة البيانات)";

            // btnClose
            btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnClose.BackColor = Color.White;
            btnClose.DialogResult = DialogResult.Cancel;
            btnClose.FlatStyle = FlatStyle.Popup;
            btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnClose.Location = new Point(12, 6);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(140, 40);
            btnClose.TabIndex = 0;
            btnClose.Text = "إغلاق";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;

            // frmAccounting
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            CancelButton = btnClose;
            ClientSize = new Size(1150, 700);
            Controls.Add(pnlButtons);
            Controls.Add(tabMain);
            Controls.Add(pnlHeader);
            Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "frmAccounting";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "الحسابات";
            Load += frmAccounting_Load;
            pnlButtons.ResumeLayout(false);
            pnlButtons.PerformLayout();
            tpFiscalPeriods.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvFiscalPeriods).EndInit();
            pnlPeriodsFilter.ResumeLayout(false);
            pnlPeriodsFilter.PerformLayout();
            tpCostCenters.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvCostCenters).EndInit();
            pnlCostFilter.ResumeLayout(false);
            pnlCostFilter.PerformLayout();
            pnlTotals.ResumeLayout(false);
            pnlTotals.PerformLayout();
            tpJournalEntries.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvJournalEntries).EndInit();
            pnlJournalFilter.ResumeLayout(false);
            pnlJournalFilter.PerformLayout();
            tpAccounts.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvAccounts).EndInit();
            pnlAccountsFilter.ResumeLayout(false);
            pnlAccountsFilter.PerformLayout();
            tabMain.ResumeLayout(false);
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            cmsGrids.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip cmsGrids;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.Label lblMockBadge;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tpAccounts;
        private System.Windows.Forms.Panel pnlAccountsFilter;
        private System.Windows.Forms.Label lblAccountsSearch;
        private System.Windows.Forms.ComboBox cbFilterBy;
        private System.Windows.Forms.TextBox txtAccountsSearch;
        private System.Windows.Forms.Label lblAccountsCount;
        private System.Windows.Forms.DataGridView dgvAccounts;
        private System.Windows.Forms.TabPage tpJournalEntries;
        private System.Windows.Forms.Panel pnlJournalFilter;
        private System.Windows.Forms.Label lblJvSearch;
        private System.Windows.Forms.TextBox txtJvSearch;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label lblJvCount;
        private System.Windows.Forms.DataGridView dgvJournalEntries;
        private System.Windows.Forms.Panel pnlTotals;
        private System.Windows.Forms.Label lblTotalDebit;
        private System.Windows.Forms.Label lblTotalDebitVal;
        private System.Windows.Forms.Label lblTotalCredit;
        private System.Windows.Forms.Label lblTotalCreditVal;
        private System.Windows.Forms.Label lblBalanceStatus;
        private System.Windows.Forms.TabPage tpCostCenters;
        private System.Windows.Forms.Panel pnlCostFilter;
        private System.Windows.Forms.Label lblCostCaption;
        private System.Windows.Forms.Label lblCostCount;
        private System.Windows.Forms.DataGridView dgvCostCenters;
        private System.Windows.Forms.TabPage tpFiscalPeriods;
        private System.Windows.Forms.Panel pnlPeriodsFilter;
        private System.Windows.Forms.Label lblPeriodsCaption;
        private System.Windows.Forms.Label lblPeriodsCount;
        private System.Windows.Forms.DataGridView dgvFiscalPeriods;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Label lblFooterHint;
        private System.Windows.Forms.Button btnClose;
    }
}