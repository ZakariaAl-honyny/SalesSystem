namespace SalesSystem.Desktop.Screens.Purchases.Forms
{
    partial class frmSupplierStatement
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
            components = new System.ComponentModel.Container();
            toolTip1 = new ToolTip(components);
            tipTafqeet = new ToolTip(components);
            cmsStatement = new ContextMenuStrip(components);
            tsmiDetails = new ToolStripMenuItem();
            tsmiExport = new ToolStripMenuItem();
            tsmiRefresh = new ToolStripMenuItem();
            pnlHeader = new Panel();
            lblMode = new Label();
            lblMockBadge = new Label();
            pnlToolbar = new Panel();
            flpToolbar = new FlowLayoutPanel();
            btnRefresh = new Button();
            btnExport = new Button();
            btnClose = new Button();
            pnlFilters = new Panel();
            gbFilter = new GroupBox();
            label2 = new Label();
            cbFilterBy = new ComboBox();
            txtFilterValue = new TextBox();
            label1 = new Label();
            dtpFrom = new DateTimePicker();
            label3 = new Label();
            dtpTo = new DateTimePicker();
            btnRangeToday = new Button();
            btnRangeMonth = new Button();
            btnRange30 = new Button();
            chkOnlyDue = new CheckBox();
            btnClearFilters = new Button();
            lblSupplierCaption = new Label();
            lblSupplierValue = new Label();
            btnToggleFilters = new Button();
            pnlSummary = new Panel();
            lblSummary = new Label();
            lblClosingCaption = new Label();
            txtClosingBalance = new TextBox();
            pnlGrid = new Panel();
            dgvStatement = new DataGridView();
            emptyState = new Label();
            cmsStatement.SuspendLayout();
            pnlHeader.SuspendLayout();
            pnlToolbar.SuspendLayout();
            flpToolbar.SuspendLayout();
            pnlFilters.SuspendLayout();
            gbFilter.SuspendLayout();
            pnlSummary.SuspendLayout();
            pnlGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvStatement).BeginInit();
            SuspendLayout();
            // 
            // cmsStatement
            // 
            cmsStatement.BackColor = Color.White;
            cmsStatement.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmsStatement.ImageScalingSize = new Size(24, 24);
            cmsStatement.Items.AddRange(new ToolStripItem[] { tsmiDetails, tsmiExport, tsmiRefresh });
            cmsStatement.Name = "cmsStatement";
            cmsStatement.RightToLeft = RightToLeft.Yes;
            cmsStatement.ShowImageMargin = false;
            cmsStatement.Size = new Size(230, 88);
            // 
            // tsmiDetails
            // 
            tsmiDetails.Name = "tsmiDetails";
            tsmiDetails.Size = new Size(229, 28);
            tsmiDetails.Text = "تفاصيل الحركة";
            tsmiDetails.Click += tsmiDetails_Click;
            // 
            // tsmiExport
            // 
            tsmiExport.Name = "tsmiExport";
            tsmiExport.Size = new Size(229, 28);
            tsmiExport.Text = "تصدير الكشف إلى Excel";
            tsmiExport.Click += tsmiExport_Click;
            // 
            // tsmiRefresh
            // 
            tsmiRefresh.Name = "tsmiRefresh";
            tsmiRefresh.Size = new Size(229, 28);
            tsmiRefresh.Text = "تحديث الكشف (F5)";
            tsmiRefresh.Click += tsmiRefresh_Click;
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(14, 165, 103);
            pnlHeader.Controls.Add(lblMode);
            pnlHeader.Controls.Add(lblMockBadge);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Margin = new Padding(0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1180, 56);
            pnlHeader.TabIndex = 0;
            // 
            // lblMode
            // 
            lblMode.BackColor = Color.FromArgb(14, 165, 103);
            lblMode.Dock = DockStyle.Fill;
            lblMode.Font = new Font("Tahoma", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblMode.ForeColor = Color.White;
            lblMode.Location = new Point(0, 0);
            lblMode.Margin = new Padding(0);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(940, 56);
            lblMode.TabIndex = 0;
            lblMode.Text = "كشف حساب مورد";
            lblMode.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblMockBadge
            // 
            lblMockBadge.BackColor = Color.FromArgb(14, 165, 103);
            lblMockBadge.Dock = DockStyle.Right;
            lblMockBadge.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblMockBadge.ForeColor = Color.White;
            lblMockBadge.Location = new Point(940, 0);
            lblMockBadge.Margin = new Padding(0);
            lblMockBadge.Name = "lblMockBadge";
            lblMockBadge.Size = new Size(240, 56);
            lblMockBadge.TabIndex = 1;
            lblMockBadge.Text = "وضع التجربة (Mock) — بيانات وهمية";
            lblMockBadge.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlToolbar
            // 
            pnlToolbar.BackColor = Color.FromArgb(235, 240, 235);
            pnlToolbar.Controls.Add(flpToolbar);
            pnlToolbar.Dock = DockStyle.Top;
            pnlToolbar.Location = new Point(0, 56);
            pnlToolbar.Margin = new Padding(0);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.Size = new Size(1180, 64);
            pnlToolbar.TabIndex = 1;
            // 
            // flpToolbar
            // 
            flpToolbar.BackColor = Color.FromArgb(235, 240, 235);
            flpToolbar.Controls.Add(btnRefresh);
            flpToolbar.Controls.Add(btnExport);
            flpToolbar.Controls.Add(btnClose);
            flpToolbar.Dock = DockStyle.Fill;
            flpToolbar.FlowDirection = FlowDirection.RightToLeft;
            flpToolbar.Location = new Point(0, 0);
            flpToolbar.Margin = new Padding(0);
            flpToolbar.Name = "flpToolbar";
            flpToolbar.Padding = new Padding(8, 13, 8, 13);
            flpToolbar.Size = new Size(1180, 64);
            flpToolbar.TabIndex = 0;
            flpToolbar.WrapContents = false;
            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = Color.FromArgb(13, 110, 253);
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Location = new Point(1034, 13);
            btnRefresh.Margin = new Padding(4);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(134, 38);
            btnRefresh.TabIndex = 0;
            btnRefresh.Text = "تحديث الكشف";
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // btnExport
            // 
            btnExport.BackColor = Color.FromArgb(108, 117, 125);
            btnExport.FlatAppearance.BorderSize = 0;
            btnExport.FlatStyle = FlatStyle.Flat;
            btnExport.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnExport.ForeColor = Color.White;
            btnExport.Location = new Point(892, 13);
            btnExport.Margin = new Padding(4);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(134, 38);
            btnExport.TabIndex = 1;
            btnExport.Text = "تصدير Excel";
            btnExport.UseVisualStyleBackColor = false;
            btnExport.Click += btnExport_Click;
            // 
            // btnClose
            // 
            btnClose.BackColor = Color.FromArgb(248, 249, 250);
            btnClose.FlatAppearance.BorderColor = Color.FromArgb(222, 226, 230);
            btnClose.FlatAppearance.BorderSize = 1;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnClose.ForeColor = Color.FromArgb(33, 37, 41);
            btnClose.Location = new Point(774, 13);
            btnClose.Margin = new Padding(4);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(110, 38);
            btnClose.TabIndex = 2;
            btnClose.Text = "إغلاق";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // pnlFilters
            // 
            pnlFilters.BackColor = Color.FromArgb(235, 240, 235);
            pnlFilters.Controls.Add(gbFilter);
            pnlFilters.Controls.Add(btnToggleFilters);
            pnlFilters.Dock = DockStyle.Top;
            pnlFilters.Location = new Point(0, 120);
            pnlFilters.Margin = new Padding(0);
            pnlFilters.Name = "pnlFilters";
            pnlFilters.Padding = new Padding(6, 4, 6, 4);
            pnlFilters.Size = new Size(1180, 112);
            pnlFilters.TabIndex = 2;
            // 
            // gbFilter
            // 
            gbFilter.BackColor = Color.FromArgb(235, 245, 235);
            gbFilter.Controls.Add(label2);
            gbFilter.Controls.Add(cbFilterBy);
            gbFilter.Controls.Add(txtFilterValue);
            gbFilter.Controls.Add(label1);
            gbFilter.Controls.Add(dtpFrom);
            gbFilter.Controls.Add(label3);
            gbFilter.Controls.Add(dtpTo);
            gbFilter.Controls.Add(btnRangeToday);
            gbFilter.Controls.Add(btnRangeMonth);
            gbFilter.Controls.Add(btnRange30);
            gbFilter.Controls.Add(chkOnlyDue);
            gbFilter.Controls.Add(lblSupplierCaption);
            gbFilter.Controls.Add(lblSupplierValue);
            gbFilter.Controls.Add(btnClearFilters);
            gbFilter.Dock = DockStyle.Fill;
            gbFilter.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            gbFilter.ForeColor = Color.FromArgb(14, 82, 52);
            gbFilter.Location = new Point(146, 4);
            gbFilter.Margin = new Padding(0);
            gbFilter.Name = "gbFilter";
            gbFilter.Padding = new Padding(10);
            gbFilter.RightToLeft = RightToLeft.Yes;
            gbFilter.Size = new Size(1028, 104);
            gbFilter.TabIndex = 0;
            gbFilter.TabStop = false;
            gbFilter.Text = "بحث وتصفية";
            // 
            // label2
            // 
            label2.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.FromArgb(14, 82, 52);
            label2.Location = new Point(926, 25);
            label2.Margin = new Padding(0);
            label2.Name = "label2";
            label2.Size = new Size(90, 22);
            label2.TabIndex = 0;
            label2.Text = "تصفية بـ:";
            label2.TextAlign = ContentAlignment.MiddleRight;
            // 
            // cbFilterBy
            // 
            cbFilterBy.BackColor = Color.White;
            cbFilterBy.DropDownStyle = ComboBoxStyle.DropDownList;
            cbFilterBy.FlatStyle = FlatStyle.Flat;
            cbFilterBy.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cbFilterBy.FormattingEnabled = true;
            cbFilterBy.Items.AddRange(new object[] { "النوع", "رقم المرجع" });
            cbFilterBy.Location = new Point(786, 22);
            cbFilterBy.Margin = new Padding(0);
            cbFilterBy.Name = "cbFilterBy";
            cbFilterBy.Size = new Size(135, 26);
            cbFilterBy.TabIndex = 1;
            cbFilterBy.SelectedIndexChanged += cbFilterBy_SelectedIndexChanged;
            // 
            // txtFilterValue
            // 
            txtFilterValue.BackColor = Color.White;
            txtFilterValue.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtFilterValue.Location = new Point(556, 22);
            txtFilterValue.Margin = new Padding(0);
            txtFilterValue.Name = "txtFilterValue";
            txtFilterValue.PlaceholderText = "اكتب قيمة التصفية...";
            txtFilterValue.RightToLeft = RightToLeft.Yes;
            txtFilterValue.Size = new Size(225, 26);
            txtFilterValue.TabIndex = 2;
            txtFilterValue.TextChanged += txtFilterValue_TextChanged;
            // 
            // label1
            // 
            label1.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.FromArgb(14, 82, 52);
            label1.Location = new Point(496, 25);
            label1.Margin = new Padding(0);
            label1.Name = "label1";
            label1.Size = new Size(52, 22);
            label1.TabIndex = 3;
            label1.Text = "من:";
            label1.TextAlign = ContentAlignment.MiddleRight;
            // 
            // dtpFrom
            // 
            dtpFrom.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dtpFrom.Format = DateTimePickerFormat.Short;
            dtpFrom.Location = new Point(361, 22);
            dtpFrom.Margin = new Padding(0);
            dtpFrom.Name = "dtpFrom";
            dtpFrom.Size = new Size(130, 26);
            dtpFrom.TabIndex = 4;
            dtpFrom.ValueChanged += dtpFrom_ValueChanged;
            // 
            // label3
            // 
            label3.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.FromArgb(14, 82, 52);
            label3.Location = new Point(306, 25);
            label3.Margin = new Padding(0);
            label3.Name = "label3";
            label3.Size = new Size(50, 22);
            label3.TabIndex = 5;
            label3.Text = "إلى:";
            label3.TextAlign = ContentAlignment.MiddleRight;
            // 
            // dtpTo
            // 
            dtpTo.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dtpTo.Format = DateTimePickerFormat.Short;
            dtpTo.Location = new Point(171, 22);
            dtpTo.Margin = new Padding(0);
            dtpTo.Name = "dtpTo";
            dtpTo.Size = new Size(130, 26);
            dtpTo.TabIndex = 6;
            dtpTo.ValueChanged += dtpTo_ValueChanged;
            // 
            // btnRangeToday
            // 
            btnRangeToday.BackColor = Color.FromArgb(248, 249, 250);
            btnRangeToday.FlatAppearance.BorderColor = Color.FromArgb(222, 226, 230);
            btnRangeToday.FlatAppearance.BorderSize = 1;
            btnRangeToday.FlatStyle = FlatStyle.Flat;
            btnRangeToday.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnRangeToday.ForeColor = Color.FromArgb(33, 37, 41);
            btnRangeToday.Location = new Point(926, 62);
            btnRangeToday.Margin = new Padding(0);
            btnRangeToday.Name = "btnRangeToday";
            btnRangeToday.Size = new Size(100, 30);
            btnRangeToday.TabIndex = 7;
            btnRangeToday.Text = "اليوم";
            btnRangeToday.UseVisualStyleBackColor = false;
            btnRangeToday.Click += btnRangeToday_Click;
            // 
            // btnRangeMonth
            // 
            btnRangeMonth.BackColor = Color.FromArgb(248, 249, 250);
            btnRangeMonth.FlatAppearance.BorderColor = Color.FromArgb(222, 226, 230);
            btnRangeMonth.FlatAppearance.BorderSize = 1;
            btnRangeMonth.FlatStyle = FlatStyle.Flat;
            btnRangeMonth.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnRangeMonth.ForeColor = Color.FromArgb(33, 37, 41);
            btnRangeMonth.Location = new Point(818, 62);
            btnRangeMonth.Margin = new Padding(0);
            btnRangeMonth.Name = "btnRangeMonth";
            btnRangeMonth.Size = new Size(103, 30);
            btnRangeMonth.TabIndex = 8;
            btnRangeMonth.Text = "هذا الشهر";
            btnRangeMonth.UseVisualStyleBackColor = false;
            btnRangeMonth.Click += btnRangeMonth_Click;
            // 
            // btnRange30
            // 
            btnRange30.BackColor = Color.FromArgb(248, 249, 250);
            btnRange30.FlatAppearance.BorderColor = Color.FromArgb(222, 226, 230);
            btnRange30.FlatAppearance.BorderSize = 1;
            btnRange30.FlatStyle = FlatStyle.Flat;
            btnRange30.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnRange30.ForeColor = Color.FromArgb(33, 37, 41);
            btnRange30.Location = new Point(690, 62);
            btnRange30.Margin = new Padding(0);
            btnRange30.Name = "btnRange30";
            btnRange30.Size = new Size(123, 30);
            btnRange30.TabIndex = 9;
            btnRange30.Text = "آخر 30 يوماً";
            btnRange30.UseVisualStyleBackColor = false;
            btnRange30.Click += btnRange30_Click;
            // 
            // chkOnlyDue
            // 
            chkOnlyDue.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            chkOnlyDue.ForeColor = Color.FromArgb(14, 82, 52);
            chkOnlyDue.Location = new Point(430, 64);
            chkOnlyDue.Margin = new Padding(0);
            chkOnlyDue.Name = "chkOnlyDue";
            chkOnlyDue.Size = new Size(250, 26);
            chkOnlyDue.TabIndex = 10;
            chkOnlyDue.Text = "إظهار الحركات المستحقة فقط";
            chkOnlyDue.UseVisualStyleBackColor = false;
            chkOnlyDue.CheckedChanged += chkOnlyDue_CheckedChanged;
            // 
            // lblSupplierCaption
            // 
            lblSupplierCaption.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSupplierCaption.ForeColor = Color.FromArgb(14, 82, 52);
            lblSupplierCaption.Location = new Point(300, 64);
            lblSupplierCaption.Margin = new Padding(0);
            lblSupplierCaption.Name = "lblSupplierCaption";
            lblSupplierCaption.Size = new Size(70, 26);
            lblSupplierCaption.TabIndex = 11;
            lblSupplierCaption.Text = "المورد:";
            lblSupplierCaption.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblSupplierValue
            // 
            lblSupplierValue.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSupplierValue.ForeColor = Color.FromArgb(14, 82, 52);
            lblSupplierValue.Location = new Point(60, 64);
            lblSupplierValue.Margin = new Padding(0);
            lblSupplierValue.Name = "lblSupplierValue";
            lblSupplierValue.Size = new Size(235, 26);
            lblSupplierValue.TabIndex = 12;
            lblSupplierValue.Text = "شركة النور للتجارة";
            lblSupplierValue.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // btnClearFilters
            // 
            // RULE-N148: «مسح التصفية» — داخل gbFilter، قيم حرفية مطابقة لما يُطبَّق وقت التشغيل (RULE-N134).
            // الموضع: الخانة الحرة الوحيدة في صندوق التصفية — صف التصفية الأول بجوار dtpTo بفاصل 8px،
            // لأن الصف الثاني مشغول بالكامل بعرض المورد + مربع الاختيار + أزرار النطاقات، بلا تحريك أي عنصر قائم.
            btnClearFilters.BackColor = Color.FromArgb(108, 117, 125);
            btnClearFilters.Cursor = Cursors.Hand;
            btnClearFilters.Enabled = false;
            btnClearFilters.FlatAppearance.BorderSize = 0;
            btnClearFilters.FlatStyle = FlatStyle.Flat;
            btnClearFilters.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnClearFilters.ForeColor = Color.White;
            btnClearFilters.Location = new Point(18, 19);
            btnClearFilters.Name = "btnClearFilters";
            btnClearFilters.Size = new Size(145, 32);
            btnClearFilters.TabIndex = 13;
            btnClearFilters.Text = "🧹 مسح التصفية";
            toolTip1.SetToolTip(btnClearFilters, "مسح كل أدوات التصفية (Esc) — لا يعيد تحميل البيانات");
            btnClearFilters.UseVisualStyleBackColor = false;
            btnClearFilters.Click += btnClearFilters_Click;
            // 
            // btnToggleFilters
            // 
            // RULE-N145: مُعلَن في المصمّم بقيم حرفية مطابقة لما يُطبَّق وقت التشغيل.
            // السلوك (النقرة · F9 · التلميح · الحالة المحفوظة) يُوصِّله FilterPanelHelper.Attach.
            btnToggleFilters.BackColor = Color.FromArgb(248, 249, 250);
            btnToggleFilters.Cursor = Cursors.Hand;
            btnToggleFilters.Dock = DockStyle.Left;
            btnToggleFilters.FlatAppearance.BorderColor = Color.FromArgb(222, 226, 230);
            btnToggleFilters.FlatAppearance.BorderSize = 1;
            btnToggleFilters.FlatAppearance.MouseOverBackColor = Color.FromArgb(240, 240, 240);
            btnToggleFilters.FlatStyle = FlatStyle.Popup;
            btnToggleFilters.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnToggleFilters.ForeColor = Color.FromArgb(33, 37, 41);
            btnToggleFilters.Location = new Point(6, 4);
            btnToggleFilters.Name = "btnToggleFilters";
            btnToggleFilters.Size = new Size(140, 104);
            btnToggleFilters.TabIndex = 1;
            btnToggleFilters.Text = "إخفاء التصفية";
            btnToggleFilters.UseVisualStyleBackColor = false;
            // 
            // pnlSummary
            // 
            pnlSummary.BackColor = Color.FromArgb(235, 240, 235);
            pnlSummary.Controls.Add(lblSummary);
            pnlSummary.Controls.Add(lblClosingCaption);
            pnlSummary.Controls.Add(txtClosingBalance);
            pnlSummary.Dock = DockStyle.Bottom;
            pnlSummary.Location = new Point(0, 684);
            pnlSummary.Margin = new Padding(0);
            pnlSummary.Name = "pnlSummary";
            pnlSummary.Padding = new Padding(10, 2, 10, 2);
            pnlSummary.Size = new Size(1180, 32);
            pnlSummary.TabIndex = 3;
            // 
            // lblSummary
            // 
            lblSummary.Dock = DockStyle.Fill;
            lblSummary.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSummary.ForeColor = Color.FromArgb(14, 82, 52);
            lblSummary.Location = new Point(10, 2);
            lblSummary.Margin = new Padding(0);
            lblSummary.Name = "lblSummary";
            lblSummary.Size = new Size(820, 28);
            lblSummary.TabIndex = 0;
            lblSummary.Text = "عدد الحركات: 0 · مدين: 0.00 · دائن: 0.00";
            lblSummary.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblClosingCaption
            // 
            lblClosingCaption.Dock = DockStyle.Left;
            lblClosingCaption.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblClosingCaption.ForeColor = Color.FromArgb(14, 82, 52);
            lblClosingCaption.Location = new Point(830, 2);
            lblClosingCaption.Margin = new Padding(0);
            lblClosingCaption.Name = "lblClosingCaption";
            lblClosingCaption.Size = new Size(70, 28);
            lblClosingCaption.TabIndex = 1;
            lblClosingCaption.Text = "الرصيد:";
            lblClosingCaption.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtClosingBalance
            // 
            txtClosingBalance.BackColor = Color.White;
            txtClosingBalance.BorderStyle = BorderStyle.FixedSingle;
            txtClosingBalance.Dock = DockStyle.Left;
            txtClosingBalance.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtClosingBalance.ForeColor = Color.FromArgb(14, 82, 52);
            txtClosingBalance.Location = new Point(900, 2);
            txtClosingBalance.Margin = new Padding(0);
            txtClosingBalance.Name = "txtClosingBalance";
            txtClosingBalance.ReadOnly = true;
            txtClosingBalance.Size = new Size(170, 26);
            txtClosingBalance.TabIndex = 2;
            txtClosingBalance.Text = "0.00";
            txtClosingBalance.TextAlign = HorizontalAlignment.Center;
            // 
            // pnlGrid
            // 
            pnlGrid.BackColor = Color.FromArgb(235, 240, 235);
            pnlGrid.Controls.Add(emptyState);
            pnlGrid.Controls.Add(dgvStatement);
            pnlGrid.Dock = DockStyle.Fill;
            pnlGrid.Location = new Point(0, 232);
            pnlGrid.Margin = new Padding(0);
            pnlGrid.Name = "pnlGrid";
            pnlGrid.Size = new Size(1180, 452);
            pnlGrid.TabIndex = 4;
            // 
            // dgvStatement
            // 
            dgvStatement.AllowUserToAddRows = false;
            dgvStatement.AllowUserToDeleteRows = false;
            dgvStatement.AllowUserToResizeRows = false;
            dgvStatement.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 252, 245);
            dgvStatement.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvStatement.BackgroundColor = Color.FromArgb(220, 240, 220);
            dgvStatement.BorderStyle = BorderStyle.None;
            dgvStatement.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(14, 165, 103);
            dgvStatement.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvStatement.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(14, 165, 103);
            dgvStatement.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            dgvStatement.ColumnHeadersHeight = 45;
            dgvStatement.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvStatement.ContextMenuStrip = cmsStatement;
            dgvStatement.DefaultCellStyle.BackColor = Color.White;
            dgvStatement.DefaultCellStyle.ForeColor = Color.FromArgb(33, 37, 41);
            dgvStatement.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 255, 180);
            dgvStatement.DefaultCellStyle.SelectionForeColor = Color.FromArgb(33, 37, 41);
            dgvStatement.Dock = DockStyle.Fill;
            dgvStatement.EnableHeadersVisualStyles = false;
            dgvStatement.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dgvStatement.GridColor = Color.FromArgb(205, 225, 205);
            dgvStatement.Location = new Point(0, 0);
            dgvStatement.Margin = new Padding(0);
            dgvStatement.MultiSelect = false;
            dgvStatement.Name = "dgvStatement";
            dgvStatement.ReadOnly = true;
            dgvStatement.RowHeadersVisible = false;
            dgvStatement.RowTemplate.Height = 32;
            dgvStatement.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvStatement.Size = new Size(1180, 452);
            dgvStatement.TabIndex = 0;
            dgvStatement.TabStop = false;
            dgvStatement.CellDoubleClick += dgvStatement_CellDoubleClick;
            dgvStatement.CellFormatting += dgvStatement_CellFormatting;
            // 
            // emptyState
            // 
            emptyState.BackColor = Color.Transparent;
            emptyState.Dock = DockStyle.Fill;
            emptyState.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            emptyState.ForeColor = Color.FromArgb(14, 82, 52);
            emptyState.Location = new Point(0, 0);
            emptyState.Margin = new Padding(0);
            emptyState.Name = "emptyState";
            emptyState.Size = new Size(1180, 452);
            emptyState.TabIndex = 1;
            emptyState.Text = "لا توجد حركات مطابقة للمعايير الحالية";
            emptyState.TextAlign = ContentAlignment.MiddleCenter;
            emptyState.Visible = false;
            // 
            // frmSupplierStatement
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.FromArgb(170, 220, 170);
            ClientSize = new Size(1180, 716);
            Controls.Add(pnlGrid);
            Controls.Add(pnlSummary);
            Controls.Add(pnlFilters);
            Controls.Add(pnlToolbar);
            Controls.Add(pnlHeader);
            Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmSupplierStatement";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "كشف حساب مورد";
            Load += frmSupplierStatement_Load;
            cmsStatement.ResumeLayout(false);
            pnlHeader.ResumeLayout(false);
            pnlToolbar.ResumeLayout(false);
            flpToolbar.ResumeLayout(false);
            pnlFilters.ResumeLayout(false);
            gbFilter.ResumeLayout(false);
            gbFilter.PerformLayout();
            pnlSummary.ResumeLayout(false);
            pnlSummary.PerformLayout();
            pnlGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvStatement).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel pnlHeader;
        private Label lblMode;
        private Label lblMockBadge;
        private Panel pnlToolbar;
        private FlowLayoutPanel flpToolbar;
        private Button btnRefresh;
        private Button btnExport;
        private Button btnClose;
        private Panel pnlFilters;
        private GroupBox gbFilter;
        private Label label1;
        private Label label2;
        private Label label3;
        private ComboBox cbFilterBy;
        private TextBox txtFilterValue;
        private DateTimePicker dtpFrom;
        private DateTimePicker dtpTo;
        private Button btnRangeToday;
        private Button btnRangeMonth;
        private Button btnRange30;
        private CheckBox chkOnlyDue;
        private Button btnClearFilters;
        private Label lblSupplierCaption;
        private Label lblSupplierValue;
        private Button btnToggleFilters;
        private Panel pnlSummary;
        private Label lblSummary;
        private Label lblClosingCaption;
        private TextBox txtClosingBalance;
        private Panel pnlGrid;
        private DataGridView dgvStatement;
        private Label emptyState;
        private ContextMenuStrip cmsStatement;
        private ToolStripMenuItem tsmiDetails;
        private ToolStripMenuItem tsmiExport;
        private ToolStripMenuItem tsmiRefresh;
        private ToolTip toolTip1;
        private ToolTip tipTafqeet;
    }
}
