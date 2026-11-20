namespace SalesSystem.Desktop.Screens.Dashboard.Controls
{
    partial class ctrlDashboard
    {
        private System.ComponentModel.IContainer components = null;

        #region Component Designer generated code

        // ملاحظات المصمّم (RULE-N134 / RULE-N140 / RULE-N145):
        //   • كل عنصر دائم في اللوحة (العنوان · صناديق المجموعة · البطاقات · التسميات · زر التحديث)
        //     معلَن هنا بقيم حرفية مطابقة لثوابت GreenThemeHelper — فلا يظهر عنصر وقت التشغيل فقط
        //     ولا يوجد مرجع لأي عضو في Helpers/ داخل هذا الملف (RULE-N128).
        //   • ترتيب الإرساء (RULE-N125): يُضاف العنصر المراسٍ Fill أولاً ثم العناصر العلوية من الأسفل
        //     إلى الأعلى، فيُرسي آخر عنصر مُضاف (lblMode) أولاً ويأخذ أعلى الشاشة.
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            toolTip1 = new ToolTip(components);
            lblMode = new Label();
            gbCommands = new GroupBox();
            lblPeriod = new Label();
            btnRefresh = new Button();
            lblWelcome = new Label();
            gbIndicators = new GroupBox();
            flowCards = new FlowLayoutPanel();
            cardSales = new Panel();
            lblSalesSub = new Label();
            cardSalesValue = new Label();
            cardSalesTitle = new Label();
            cardSalesAccent = new Label();
            cardPurchases = new Panel();
            lblPurchasesSub = new Label();
            cardPurchasesValue = new Label();
            cardPurchasesTitle = new Label();
            cardPurchasesAccent = new Label();
            cardCustomers = new Panel();
            lblCustomersSub = new Label();
            cardCustomersValue = new Label();
            cardCustomersTitle = new Label();
            cardCustomersAccent = new Label();
            cardLowStock = new Panel();
            lblStockSub = new Label();
            cardLowStockValue = new Label();
            cardLowStockTitle = new Label();
            cardLowStockAccent = new Label();
            cardReceivables = new Panel();
            cardReceivablesSub = new Label();
            cardReceivablesValue = new Label();
            cardReceivablesTitle = new Label();
            cardReceivablesAccent = new Label();
            cardPayables = new Panel();
            cardPayablesSub = new Label();
            cardPayablesValue = new Label();
            cardPayablesTitle = new Label();
            cardPayablesAccent = new Label();
            gbDetails = new GroupBox();
            lblDetails = new Label();
            lblDataStatus = new Label();
            gbCommands.SuspendLayout();
            gbIndicators.SuspendLayout();
            flowCards.SuspendLayout();
            cardSales.SuspendLayout();
            cardPurchases.SuspendLayout();
            cardCustomers.SuspendLayout();
            cardLowStock.SuspendLayout();
            cardReceivables.SuspendLayout();
            cardPayables.SuspendLayout();
            gbDetails.SuspendLayout();
            SuspendLayout();
            // 
            // lblMode
            // 
            lblMode.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
            lblMode.Dock = DockStyle.Top;
            lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            lblMode.ForeColor = System.Drawing.Color.White;
            lblMode.Location = new Point(0, 0);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(1200, 44);
            lblMode.TabIndex = 0;
            lblMode.Text = "ملخص الأداء اليوم";
            lblMode.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // gbCommands
            // 
            gbCommands.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(245), Convert.ToInt32(235));
            gbCommands.Controls.Add(lblPeriod);
            gbCommands.Controls.Add(btnRefresh);
            gbCommands.Controls.Add(lblWelcome);
            gbCommands.Dock = DockStyle.Top;
            gbCommands.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            gbCommands.Location = new Point(0, 44);
            gbCommands.Name = "gbCommands";
            gbCommands.Padding = new Padding(8, 6, 8, 6);
            gbCommands.Size = new Size(1200, 74);
            gbCommands.TabIndex = 1;
            gbCommands.TabStop = false;
            gbCommands.Text = "أوامر اللوحة";
            // 
            // lblPeriod
            // 
            lblPeriod.Dock = DockStyle.Fill;
            lblPeriod.Font = new Font("Tahoma", 9F);
            lblPeriod.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
            lblPeriod.Location = new Point(158, 31);
            lblPeriod.Name = "lblPeriod";
            lblPeriod.Size = new Size(694, 37);
            lblPeriod.TabIndex = 0;
            lblPeriod.Text = "الفترة: اليوم — والشهر الجاري";
            lblPeriod.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(23), Convert.ToInt32(162), Convert.ToInt32(184));
            btnRefresh.Cursor = Cursors.Hand;
            btnRefresh.Dock = DockStyle.Left;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnRefresh.ForeColor = System.Drawing.Color.White;
            btnRefresh.Location = new Point(8, 31);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(150, 37);
            btnRefresh.TabIndex = 1;
            btnRefresh.Text = "تحديث (F5)";
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // lblWelcome
            // 
            lblWelcome.Dock = DockStyle.Right;
            lblWelcome.Font = new Font("Tahoma", 12F, FontStyle.Bold);
            lblWelcome.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
            lblWelcome.Location = new Point(852, 31);
            lblWelcome.Name = "lblWelcome";
            lblWelcome.Padding = new Padding(8, 0, 8, 0);
            lblWelcome.Size = new Size(340, 37);
            lblWelcome.TabIndex = 2;
            lblWelcome.Text = "أهلاً";
            lblWelcome.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // gbIndicators
            // 
            gbIndicators.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(245), Convert.ToInt32(235));
            gbIndicators.Controls.Add(flowCards);
            gbIndicators.Dock = DockStyle.Top;
            gbIndicators.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            gbIndicators.Location = new Point(0, 118);
            gbIndicators.Name = "gbIndicators";
            gbIndicators.Padding = new Padding(6, 4, 6, 6);
            gbIndicators.Size = new Size(1200, 192);
            gbIndicators.TabIndex = 2;
            gbIndicators.TabStop = false;
            gbIndicators.Text = "مؤشرات اليوم";
            // 
            // flowCards
            // 
            flowCards.AutoScroll = true;
            flowCards.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
            flowCards.Controls.Add(cardSales);
            flowCards.Controls.Add(cardPurchases);
            flowCards.Controls.Add(cardCustomers);
            flowCards.Controls.Add(cardLowStock);
            flowCards.Controls.Add(cardReceivables);
            flowCards.Controls.Add(cardPayables);
            flowCards.Dock = DockStyle.Fill;
            flowCards.FlowDirection = FlowDirection.LeftToRight;
            flowCards.Location = new Point(6, 29);
            flowCards.Name = "flowCards";
            flowCards.Padding = new Padding(4);
            flowCards.Size = new Size(1188, 157);
            flowCards.TabIndex = 0;
            flowCards.WrapContents = false;
            // 
            // cardSales
            // 
            cardSales.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
            cardSales.Controls.Add(lblSalesSub);
            cardSales.Controls.Add(cardSalesValue);
            cardSales.Controls.Add(cardSalesTitle);
            cardSales.Controls.Add(cardSalesAccent);
            cardSales.Location = new Point(10, 10);
            cardSales.Margin = new Padding(6);
            cardSales.Name = "cardSales";
            cardSales.Size = new Size(182, 128);
            cardSales.TabIndex = 0;
            // 
            // lblSalesSub
            // 
            lblSalesSub.AutoSize = true;
            lblSalesSub.Font = new Font("Tahoma", 9F);
            lblSalesSub.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
            lblSalesSub.Location = new Point(12, 76);
            lblSalesSub.MaximumSize = new Size(158, 0);
            lblSalesSub.Name = "lblSalesSub";
            lblSalesSub.Size = new Size(156, 44);
            lblSalesSub.TabIndex = 2;
            lblSalesSub.Text = "عدد الفواتير اليوم: 0\r\nهذا الشهر: 0.00";
            // 
            // cardSalesValue
            // 
            cardSalesValue.AutoSize = true;
            cardSalesValue.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            cardSalesValue.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            cardSalesValue.Location = new Point(12, 36);
            cardSalesValue.Name = "cardSalesValue";
            cardSalesValue.Size = new Size(87, 39);
            cardSalesValue.TabIndex = 1;
            cardSalesValue.Text = "0.00";
            // 
            // cardSalesTitle
            // 
            cardSalesTitle.AutoSize = true;
            cardSalesTitle.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            cardSalesTitle.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            cardSalesTitle.Location = new Point(12, 10);
            cardSalesTitle.Name = "cardSalesTitle";
            cardSalesTitle.Size = new Size(167, 24);
            cardSalesTitle.TabIndex = 0;
            cardSalesTitle.Text = "إجمالي المبيعات";
            // 
            // cardSalesAccent
            // 
            cardSalesAccent.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
            cardSalesAccent.Dock = DockStyle.Bottom;
            cardSalesAccent.Location = new Point(0, 124);
            cardSalesAccent.Name = "cardSalesAccent";
            cardSalesAccent.Size = new Size(182, 4);
            cardSalesAccent.TabIndex = 3;
            // 
            // cardPurchases
            // 
            cardPurchases.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
            cardPurchases.Controls.Add(lblPurchasesSub);
            cardPurchases.Controls.Add(cardPurchasesValue);
            cardPurchases.Controls.Add(cardPurchasesTitle);
            cardPurchases.Controls.Add(cardPurchasesAccent);
            cardPurchases.Location = new Point(204, 10);
            cardPurchases.Margin = new Padding(6);
            cardPurchases.Name = "cardPurchases";
            cardPurchases.Size = new Size(182, 128);
            cardPurchases.TabIndex = 1;
            // 
            // lblPurchasesSub
            // 
            lblPurchasesSub.AutoSize = true;
            lblPurchasesSub.Font = new Font("Tahoma", 9F);
            lblPurchasesSub.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
            lblPurchasesSub.Location = new Point(12, 76);
            lblPurchasesSub.MaximumSize = new Size(158, 0);
            lblPurchasesSub.Name = "lblPurchasesSub";
            lblPurchasesSub.Size = new Size(135, 22);
            lblPurchasesSub.TabIndex = 2;
            lblPurchasesSub.Text = "هذا الشهر: 0.00";
            // 
            // cardPurchasesValue
            // 
            cardPurchasesValue.AutoSize = true;
            cardPurchasesValue.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            cardPurchasesValue.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            cardPurchasesValue.Location = new Point(12, 36);
            cardPurchasesValue.Name = "cardPurchasesValue";
            cardPurchasesValue.Size = new Size(87, 39);
            cardPurchasesValue.TabIndex = 1;
            cardPurchasesValue.Text = "0.00";
            // 
            // cardPurchasesTitle
            // 
            cardPurchasesTitle.AutoSize = true;
            cardPurchasesTitle.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            cardPurchasesTitle.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            cardPurchasesTitle.Location = new Point(12, 10);
            cardPurchasesTitle.Name = "cardPurchasesTitle";
            cardPurchasesTitle.Size = new Size(187, 24);
            cardPurchasesTitle.TabIndex = 0;
            cardPurchasesTitle.Text = "إجمالي المشتريات";
            // 
            // cardPurchasesAccent
            // 
            cardPurchasesAccent.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
            cardPurchasesAccent.Dock = DockStyle.Bottom;
            cardPurchasesAccent.Location = new Point(0, 124);
            cardPurchasesAccent.Name = "cardPurchasesAccent";
            cardPurchasesAccent.Size = new Size(182, 4);
            cardPurchasesAccent.TabIndex = 3;
            // 
            // cardCustomers
            // 
            cardCustomers.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
            cardCustomers.Controls.Add(lblCustomersSub);
            cardCustomers.Controls.Add(cardCustomersValue);
            cardCustomers.Controls.Add(cardCustomersTitle);
            cardCustomers.Controls.Add(cardCustomersAccent);
            cardCustomers.Location = new Point(398, 10);
            cardCustomers.Margin = new Padding(6);
            cardCustomers.Name = "cardCustomers";
            cardCustomers.Size = new Size(182, 128);
            cardCustomers.TabIndex = 2;
            // 
            // lblCustomersSub
            // 
            lblCustomersSub.AutoSize = true;
            lblCustomersSub.Font = new Font("Tahoma", 9F);
            lblCustomersSub.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
            lblCustomersSub.Location = new Point(12, 76);
            lblCustomersSub.MaximumSize = new Size(158, 0);
            lblCustomersSub.Name = "lblCustomersSub";
            lblCustomersSub.Size = new Size(158, 44);
            lblCustomersSub.TabIndex = 2;
            lblCustomersSub.Text = "الموردون النشطون: 0";
            // 
            // cardCustomersValue
            // 
            cardCustomersValue.AutoSize = true;
            cardCustomersValue.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            cardCustomersValue.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            cardCustomersValue.Location = new Point(12, 36);
            cardCustomersValue.Name = "cardCustomersValue";
            cardCustomersValue.Size = new Size(37, 39);
            cardCustomersValue.TabIndex = 1;
            cardCustomersValue.Text = "0";
            // 
            // cardCustomersTitle
            // 
            cardCustomersTitle.AutoSize = true;
            cardCustomersTitle.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            cardCustomersTitle.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            cardCustomersTitle.Location = new Point(12, 10);
            cardCustomersTitle.Name = "cardCustomersTitle";
            cardCustomersTitle.Size = new Size(114, 24);
            cardCustomersTitle.TabIndex = 0;
            cardCustomersTitle.Text = "عدد العملاء";
            // 
            // cardCustomersAccent
            // 
            cardCustomersAccent.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
            cardCustomersAccent.Dock = DockStyle.Bottom;
            cardCustomersAccent.Location = new Point(0, 124);
            cardCustomersAccent.Name = "cardCustomersAccent";
            cardCustomersAccent.Size = new Size(182, 4);
            cardCustomersAccent.TabIndex = 3;
            // 
            // cardLowStock
            // 
            cardLowStock.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
            cardLowStock.Controls.Add(lblStockSub);
            cardLowStock.Controls.Add(cardLowStockValue);
            cardLowStock.Controls.Add(cardLowStockTitle);
            cardLowStock.Controls.Add(cardLowStockAccent);
            cardLowStock.Location = new Point(592, 10);
            cardLowStock.Margin = new Padding(6);
            cardLowStock.Name = "cardLowStock";
            cardLowStock.Size = new Size(182, 128);
            cardLowStock.TabIndex = 3;
            // 
            // lblStockSub
            // 
            lblStockSub.AutoSize = true;
            lblStockSub.Font = new Font("Tahoma", 9F);
            lblStockSub.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
            lblStockSub.Location = new Point(12, 76);
            lblStockSub.MaximumSize = new Size(158, 0);
            lblStockSub.Name = "lblStockSub";
            lblStockSub.Size = new Size(149, 22);
            lblStockSub.TabIndex = 2;
            lblStockSub.Text = "إجمالي الأصناف: 0";
            // 
            // cardLowStockValue
            // 
            cardLowStockValue.AutoSize = true;
            cardLowStockValue.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            cardLowStockValue.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            cardLowStockValue.Location = new Point(12, 36);
            cardLowStockValue.Name = "cardLowStockValue";
            cardLowStockValue.Size = new Size(37, 39);
            cardLowStockValue.TabIndex = 1;
            cardLowStockValue.Text = "0";
            // 
            // cardLowStockTitle
            // 
            cardLowStockTitle.AutoSize = true;
            cardLowStockTitle.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            cardLowStockTitle.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            cardLowStockTitle.Location = new Point(12, 10);
            cardLowStockTitle.Name = "cardLowStockTitle";
            cardLowStockTitle.Size = new Size(156, 24);
            cardLowStockTitle.TabIndex = 0;
            cardLowStockTitle.Text = "مخزون منخفض";
            // 
            // cardLowStockAccent
            // 
            cardLowStockAccent.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
            cardLowStockAccent.Dock = DockStyle.Bottom;
            cardLowStockAccent.Location = new Point(0, 124);
            cardLowStockAccent.Name = "cardLowStockAccent";
            cardLowStockAccent.Size = new Size(182, 4);
            cardLowStockAccent.TabIndex = 3;
            // 
            // cardReceivables
            // 
            cardReceivables.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
            cardReceivables.Controls.Add(cardReceivablesSub);
            cardReceivables.Controls.Add(cardReceivablesValue);
            cardReceivables.Controls.Add(cardReceivablesTitle);
            cardReceivables.Controls.Add(cardReceivablesAccent);
            cardReceivables.Location = new Point(786, 10);
            cardReceivables.Margin = new Padding(6);
            cardReceivables.Name = "cardReceivables";
            cardReceivables.Size = new Size(182, 128);
            cardReceivables.TabIndex = 4;
            // 
            // cardReceivablesSub
            // 
            cardReceivablesSub.AutoSize = true;
            cardReceivablesSub.Font = new Font("Tahoma", 9F);
            cardReceivablesSub.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            cardReceivablesSub.Location = new Point(12, 76);
            cardReceivablesSub.MaximumSize = new Size(158, 0);
            cardReceivablesSub.Name = "cardReceivablesSub";
            cardReceivablesSub.Size = new Size(114, 44);
            cardReceivablesSub.TabIndex = 2;
            cardReceivablesSub.Text = "أرصدة العملاء المدينة";
            // 
            // cardReceivablesValue
            // 
            cardReceivablesValue.AutoSize = true;
            cardReceivablesValue.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            cardReceivablesValue.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            cardReceivablesValue.Location = new Point(12, 36);
            cardReceivablesValue.Name = "cardReceivablesValue";
            cardReceivablesValue.Size = new Size(87, 39);
            cardReceivablesValue.TabIndex = 1;
            cardReceivablesValue.Text = "0.00";
            // 
            // cardReceivablesTitle
            // 
            cardReceivablesTitle.AutoSize = true;
            cardReceivablesTitle.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            cardReceivablesTitle.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            cardReceivablesTitle.Location = new Point(12, 10);
            cardReceivablesTitle.Name = "cardReceivablesTitle";
            cardReceivablesTitle.Size = new Size(205, 24);
            cardReceivablesTitle.TabIndex = 0;
            cardReceivablesTitle.Text = "المستحق لنا (عملاء)";
            // 
            // cardReceivablesAccent
            // 
            cardReceivablesAccent.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
            cardReceivablesAccent.Dock = DockStyle.Bottom;
            cardReceivablesAccent.Location = new Point(0, 124);
            cardReceivablesAccent.Name = "cardReceivablesAccent";
            cardReceivablesAccent.Size = new Size(182, 4);
            cardReceivablesAccent.TabIndex = 3;
            // 
            // cardPayables
            // 
            cardPayables.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
            cardPayables.Controls.Add(cardPayablesSub);
            cardPayables.Controls.Add(cardPayablesValue);
            cardPayables.Controls.Add(cardPayablesTitle);
            cardPayables.Controls.Add(cardPayablesAccent);
            cardPayables.Location = new Point(980, 10);
            cardPayables.Margin = new Padding(6);
            cardPayables.Name = "cardPayables";
            cardPayables.Size = new Size(182, 128);
            cardPayables.TabIndex = 5;
            // 
            // cardPayablesSub
            // 
            cardPayablesSub.AutoSize = true;
            cardPayablesSub.Font = new Font("Tahoma", 9F);
            cardPayablesSub.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            cardPayablesSub.Location = new Point(12, 76);
            cardPayablesSub.MaximumSize = new Size(158, 0);
            cardPayablesSub.Name = "cardPayablesSub";
            cardPayablesSub.Size = new Size(127, 44);
            cardPayablesSub.TabIndex = 2;
            cardPayablesSub.Text = "أرصدة الموردين الدائنة";
            // 
            // cardPayablesValue
            // 
            cardPayablesValue.AutoSize = true;
            cardPayablesValue.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            cardPayablesValue.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            cardPayablesValue.Location = new Point(12, 36);
            cardPayablesValue.Name = "cardPayablesValue";
            cardPayablesValue.Size = new Size(87, 39);
            cardPayablesValue.TabIndex = 1;
            cardPayablesValue.Text = "0.00";
            // 
            // cardPayablesTitle
            // 
            cardPayablesTitle.AutoSize = true;
            cardPayablesTitle.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            cardPayablesTitle.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            cardPayablesTitle.Location = new Point(12, 10);
            cardPayablesTitle.Name = "cardPayablesTitle";
            cardPayablesTitle.Size = new Size(237, 24);
            cardPayablesTitle.TabIndex = 0;
            cardPayablesTitle.Text = "المستحق لهم (موردون)";
            // 
            // cardPayablesAccent
            // 
            cardPayablesAccent.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
            cardPayablesAccent.Dock = DockStyle.Bottom;
            cardPayablesAccent.Location = new Point(0, 124);
            cardPayablesAccent.Name = "cardPayablesAccent";
            cardPayablesAccent.Size = new Size(182, 4);
            cardPayablesAccent.TabIndex = 3;
            // 
            // gbDetails
            // 
            gbDetails.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(245), Convert.ToInt32(235));
            gbDetails.Controls.Add(lblDetails);
            gbDetails.Controls.Add(lblDataStatus);
            gbDetails.Dock = DockStyle.Fill;
            gbDetails.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            gbDetails.Location = new Point(0, 310);
            gbDetails.Name = "gbDetails";
            gbDetails.Padding = new Padding(10, 8, 10, 8);
            gbDetails.Size = new Size(1200, 250);
            gbDetails.TabIndex = 3;
            gbDetails.TabStop = false;
            gbDetails.Text = "ملخص الأرقام";
            // 
            // lblDetails
            // 
            lblDetails.Dock = DockStyle.Fill;
            lblDetails.Font = new Font("Tahoma", 11F);
            lblDetails.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
            lblDetails.Location = new Point(10, 33);
            lblDetails.Name = "lblDetails";
            lblDetails.Padding = new Padding(12, 8, 12, 8);
            lblDetails.Size = new Size(1180, 181);
            lblDetails.TabIndex = 0;
            lblDetails.Text = "جارٍ تحميل بيانات اللوحة…";
            lblDetails.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblDataStatus
            // 
            lblDataStatus.Dock = DockStyle.Bottom;
            lblDataStatus.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            lblDataStatus.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
            lblDataStatus.Location = new Point(10, 214);
            lblDataStatus.Name = "lblDataStatus";
            lblDataStatus.Size = new Size(1180, 28);
            lblDataStatus.TabIndex = 1;
            lblDataStatus.Text = "جارٍ التحميل…";
            lblDataStatus.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // ctrlDashboard
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(170), Convert.ToInt32(220), Convert.ToInt32(170));
            Controls.Add(gbDetails);
            Controls.Add(gbIndicators);
            Controls.Add(gbCommands);
            Controls.Add(lblMode);
            Font = new Font("Tahoma", 10F);
            Name = "ctrlDashboard";
            RightToLeft = RightToLeft.Yes;
            Size = new Size(1200, 560);
            gbCommands.ResumeLayout(false);
            gbIndicators.ResumeLayout(false);
            flowCards.ResumeLayout(false);
            cardSales.ResumeLayout(false);
            cardSales.PerformLayout();
            cardPurchases.ResumeLayout(false);
            cardPurchases.PerformLayout();
            cardCustomers.ResumeLayout(false);
            cardCustomers.PerformLayout();
            cardLowStock.ResumeLayout(false);
            cardLowStock.PerformLayout();
            cardReceivables.ResumeLayout(false);
            cardReceivables.PerformLayout();
            cardPayables.ResumeLayout(false);
            cardPayables.PerformLayout();
            gbDetails.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private ToolTip toolTip1;
        private Label lblMode;

        private GroupBox gbCommands;
        private Label lblPeriod;
        private Button btnRefresh;
        private Label lblWelcome;

        private GroupBox gbIndicators;
        private FlowLayoutPanel flowCards;

        private Panel cardSales;
        private Label cardSalesAccent;
        private Label cardSalesTitle;
        private Label cardSalesValue;
        private Label lblSalesSub;

        private Panel cardPurchases;
        private Label cardPurchasesAccent;
        private Label cardPurchasesTitle;
        private Label cardPurchasesValue;
        private Label lblPurchasesSub;

        private Panel cardCustomers;
        private Label cardCustomersAccent;
        private Label cardCustomersTitle;
        private Label cardCustomersValue;
        private Label lblCustomersSub;

        private Panel cardLowStock;
        private Label cardLowStockAccent;
        private Label cardLowStockTitle;
        private Label cardLowStockValue;
        private Label lblStockSub;

        private Panel cardReceivables;
        private Label cardReceivablesAccent;
        private Label cardReceivablesTitle;
        private Label cardReceivablesValue;
        private Label cardReceivablesSub;

        private Panel cardPayables;
        private Label cardPayablesAccent;
        private Label cardPayablesTitle;
        private Label cardPayablesValue;
        private Label cardPayablesSub;

        private GroupBox gbDetails;
        private Label lblDetails;
        private Label lblDataStatus;
    }
}
