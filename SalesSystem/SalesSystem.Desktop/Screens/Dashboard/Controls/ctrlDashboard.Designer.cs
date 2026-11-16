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
            // lblMode — عنوان المكوّن (RULE-N53)
            // 
            lblMode.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(16), Convert.ToInt32(150), Convert.ToInt32(96));
            lblMode.Dock = DockStyle.Top;
            lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            lblMode.ForeColor = System.Drawing.Color.White;
            lblMode.Height = 44;
            lblMode.Name = "lblMode";
            lblMode.TabIndex = 0;
            lblMode.Text = "ملخص الأداء اليوم";
            lblMode.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // gbCommands — الكتلة (2) من RULE-N135: أوامر/فترة
            // 
            gbCommands.BackColor = System.Drawing.Color.White;
            gbCommands.Controls.Add(lblPeriod);
            gbCommands.Controls.Add(btnRefresh);
            gbCommands.Controls.Add(lblWelcome);
            gbCommands.Dock = DockStyle.Top;
            gbCommands.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            gbCommands.Height = 74;
            gbCommands.Name = "gbCommands";
            gbCommands.Padding = new Padding(8, 6, 8, 6);
            gbCommands.TabIndex = 1;
            gbCommands.TabStop = false;
            gbCommands.Text = "أوامر اللوحة";
            // 
            // lblPeriod — الفترة التي تخصّها الأرقام
            // 
            lblPeriod.Dock = DockStyle.Fill;
            lblPeriod.Font = new Font("Tahoma", 9F);
            lblPeriod.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            lblPeriod.Name = "lblPeriod";
            lblPeriod.TabIndex = 0;
            lblPeriod.Text = "الفترة: اليوم — والشهر الجاري";
            lblPeriod.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnRefresh — تحديث الأرقام يدوياً
            // 
            btnRefresh.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(23), Convert.ToInt32(162), Convert.ToInt32(184));
            btnRefresh.Cursor = Cursors.Hand;
            btnRefresh.Dock = DockStyle.Left;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnRefresh.ForeColor = System.Drawing.Color.White;
            btnRefresh.Name = "btnRefresh";
            btnRefresh.TabIndex = 1;
            btnRefresh.Text = "تحديث (F5)";
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Width = 150;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // lblWelcome
            // 
            lblWelcome.Dock = DockStyle.Right;
            lblWelcome.Font = new Font("Tahoma", 12F, FontStyle.Bold);
            lblWelcome.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            lblWelcome.Name = "lblWelcome";
            lblWelcome.Padding = new Padding(8, 0, 8, 0);
            lblWelcome.TabIndex = 2;
            lblWelcome.Text = "أهلاً";
            lblWelcome.TextAlign = ContentAlignment.MiddleCenter;
            lblWelcome.Width = 340;
            // 
            // gbIndicators — الكتلة (3) من RULE-N135: المؤشرات (البطاقات)
            // 
            gbIndicators.BackColor = System.Drawing.Color.White;
            gbIndicators.Controls.Add(flowCards);
            gbIndicators.Dock = DockStyle.Top;
            gbIndicators.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            gbIndicators.Height = 192;
            gbIndicators.Name = "gbIndicators";
            gbIndicators.Padding = new Padding(6, 4, 6, 6);
            gbIndicators.TabIndex = 2;
            gbIndicators.TabStop = false;
            gbIndicators.Text = "مؤشرات اليوم";
            // 
            // flowCards
            // 
            flowCards.AutoScroll = true;
            flowCards.BackColor = System.Drawing.Color.White;
            flowCards.Controls.Add(cardSales);
            flowCards.Controls.Add(cardPurchases);
            flowCards.Controls.Add(cardCustomers);
            flowCards.Controls.Add(cardLowStock);
            flowCards.Controls.Add(cardReceivables);
            flowCards.Controls.Add(cardPayables);
            flowCards.Dock = DockStyle.Fill;
            flowCards.FlowDirection = FlowDirection.RightToLeft;
            flowCards.Name = "flowCards";
            flowCards.Padding = new Padding(4);
            flowCards.TabIndex = 0;
            flowCards.WrapContents = false;
            // 
            // cardSales
            // 
            cardSales.BackColor = System.Drawing.Color.White;
            cardSales.Controls.Add(lblSalesSub);
            cardSales.Controls.Add(cardSalesValue);
            cardSales.Controls.Add(cardSalesTitle);
            cardSales.Controls.Add(cardSalesAccent);
            cardSales.Margin = new Padding(6);
            cardSales.Name = "cardSales";
            cardSales.Size = new Size(182, 128);
            cardSales.TabIndex = 0;
            // 
            // cardSalesAccent
            // 
            cardSalesAccent.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(16), Convert.ToInt32(150), Convert.ToInt32(96));
            cardSalesAccent.Dock = DockStyle.Bottom;
            cardSalesAccent.Height = 4;
            cardSalesAccent.Name = "cardSalesAccent";
            cardSalesAccent.TabIndex = 3;
            // 
            // cardSalesTitle
            // 
            cardSalesTitle.AutoSize = true;
            cardSalesTitle.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            cardSalesTitle.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            cardSalesTitle.Location = new Point(12, 10);
            cardSalesTitle.Name = "cardSalesTitle";
            cardSalesTitle.TabIndex = 0;
            cardSalesTitle.Text = "إجمالي المبيعات";
            // 
            // cardSalesValue
            // 
            cardSalesValue.AutoSize = true;
            cardSalesValue.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            cardSalesValue.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            cardSalesValue.Location = new Point(12, 36);
            cardSalesValue.Name = "cardSalesValue";
            cardSalesValue.TabIndex = 1;
            cardSalesValue.Text = "0.00";
            // 
            // lblSalesSub
            // 
            lblSalesSub.AutoSize = true;
            lblSalesSub.Font = new Font("Tahoma", 9F);
            lblSalesSub.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            lblSalesSub.Location = new Point(12, 76);
            lblSalesSub.MaximumSize = new Size(158, 0);
            lblSalesSub.Name = "lblSalesSub";
            lblSalesSub.TabIndex = 2;
            lblSalesSub.Text = "عدد الفواتير اليوم: 0\r\nهذا الشهر: 0.00";
            // 
            // cardPurchases
            // 
            cardPurchases.BackColor = System.Drawing.Color.White;
            cardPurchases.Controls.Add(lblPurchasesSub);
            cardPurchases.Controls.Add(cardPurchasesValue);
            cardPurchases.Controls.Add(cardPurchasesTitle);
            cardPurchases.Controls.Add(cardPurchasesAccent);
            cardPurchases.Margin = new Padding(6);
            cardPurchases.Name = "cardPurchases";
            cardPurchases.Size = new Size(182, 128);
            cardPurchases.TabIndex = 1;
            // 
            // cardPurchasesAccent
            // 
            cardPurchasesAccent.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(16), Convert.ToInt32(150), Convert.ToInt32(96));
            cardPurchasesAccent.Dock = DockStyle.Bottom;
            cardPurchasesAccent.Height = 4;
            cardPurchasesAccent.Name = "cardPurchasesAccent";
            cardPurchasesAccent.TabIndex = 3;
            // 
            // cardPurchasesTitle
            // 
            cardPurchasesTitle.AutoSize = true;
            cardPurchasesTitle.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            cardPurchasesTitle.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            cardPurchasesTitle.Location = new Point(12, 10);
            cardPurchasesTitle.Name = "cardPurchasesTitle";
            cardPurchasesTitle.TabIndex = 0;
            cardPurchasesTitle.Text = "إجمالي المشتريات";
            // 
            // cardPurchasesValue
            // 
            cardPurchasesValue.AutoSize = true;
            cardPurchasesValue.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            cardPurchasesValue.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            cardPurchasesValue.Location = new Point(12, 36);
            cardPurchasesValue.Name = "cardPurchasesValue";
            cardPurchasesValue.TabIndex = 1;
            cardPurchasesValue.Text = "0.00";
            // 
            // lblPurchasesSub
            // 
            lblPurchasesSub.AutoSize = true;
            lblPurchasesSub.Font = new Font("Tahoma", 9F);
            lblPurchasesSub.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            lblPurchasesSub.Location = new Point(12, 76);
            lblPurchasesSub.MaximumSize = new Size(158, 0);
            lblPurchasesSub.Name = "lblPurchasesSub";
            lblPurchasesSub.TabIndex = 2;
            lblPurchasesSub.Text = "هذا الشهر: 0.00";
            // 
            // cardCustomers
            // 
            cardCustomers.BackColor = System.Drawing.Color.White;
            cardCustomers.Controls.Add(lblCustomersSub);
            cardCustomers.Controls.Add(cardCustomersValue);
            cardCustomers.Controls.Add(cardCustomersTitle);
            cardCustomers.Controls.Add(cardCustomersAccent);
            cardCustomers.Margin = new Padding(6);
            cardCustomers.Name = "cardCustomers";
            cardCustomers.Size = new Size(182, 128);
            cardCustomers.TabIndex = 2;
            // 
            // cardCustomersAccent
            // 
            cardCustomersAccent.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(16), Convert.ToInt32(150), Convert.ToInt32(96));
            cardCustomersAccent.Dock = DockStyle.Bottom;
            cardCustomersAccent.Height = 4;
            cardCustomersAccent.Name = "cardCustomersAccent";
            cardCustomersAccent.TabIndex = 3;
            // 
            // cardCustomersTitle
            // 
            cardCustomersTitle.AutoSize = true;
            cardCustomersTitle.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            cardCustomersTitle.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            cardCustomersTitle.Location = new Point(12, 10);
            cardCustomersTitle.Name = "cardCustomersTitle";
            cardCustomersTitle.TabIndex = 0;
            cardCustomersTitle.Text = "عدد العملاء";
            // 
            // cardCustomersValue
            // 
            cardCustomersValue.AutoSize = true;
            cardCustomersValue.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            cardCustomersValue.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            cardCustomersValue.Location = new Point(12, 36);
            cardCustomersValue.Name = "cardCustomersValue";
            cardCustomersValue.TabIndex = 1;
            cardCustomersValue.Text = "0";
            // 
            // lblCustomersSub
            // 
            lblCustomersSub.AutoSize = true;
            lblCustomersSub.Font = new Font("Tahoma", 9F);
            lblCustomersSub.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            lblCustomersSub.Location = new Point(12, 76);
            lblCustomersSub.MaximumSize = new Size(158, 0);
            lblCustomersSub.Name = "lblCustomersSub";
            lblCustomersSub.TabIndex = 2;
            lblCustomersSub.Text = "الموردون النشطون: 0";
            // 
            // cardLowStock
            // 
            cardLowStock.BackColor = System.Drawing.Color.White;
            cardLowStock.Controls.Add(lblStockSub);
            cardLowStock.Controls.Add(cardLowStockValue);
            cardLowStock.Controls.Add(cardLowStockTitle);
            cardLowStock.Controls.Add(cardLowStockAccent);
            cardLowStock.Margin = new Padding(6);
            cardLowStock.Name = "cardLowStock";
            cardLowStock.Size = new Size(182, 128);
            cardLowStock.TabIndex = 3;
            // 
            // cardLowStockAccent
            // 
            cardLowStockAccent.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(16), Convert.ToInt32(150), Convert.ToInt32(96));
            cardLowStockAccent.Dock = DockStyle.Bottom;
            cardLowStockAccent.Height = 4;
            cardLowStockAccent.Name = "cardLowStockAccent";
            cardLowStockAccent.TabIndex = 3;
            // 
            // cardLowStockTitle
            // 
            cardLowStockTitle.AutoSize = true;
            cardLowStockTitle.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            cardLowStockTitle.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            cardLowStockTitle.Location = new Point(12, 10);
            cardLowStockTitle.Name = "cardLowStockTitle";
            cardLowStockTitle.TabIndex = 0;
            cardLowStockTitle.Text = "مخزون منخفض";
            // 
            // cardLowStockValue
            // 
            cardLowStockValue.AutoSize = true;
            cardLowStockValue.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            cardLowStockValue.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            cardLowStockValue.Location = new Point(12, 36);
            cardLowStockValue.Name = "cardLowStockValue";
            cardLowStockValue.TabIndex = 1;
            cardLowStockValue.Text = "0";
            // 
            // lblStockSub
            // 
            lblStockSub.AutoSize = true;
            lblStockSub.Font = new Font("Tahoma", 9F);
            lblStockSub.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            lblStockSub.Location = new Point(12, 76);
            lblStockSub.MaximumSize = new Size(158, 0);
            lblStockSub.Name = "lblStockSub";
            lblStockSub.TabIndex = 2;
            lblStockSub.Text = "إجمالي الأصناف: 0";
            // 
            // cardReceivables
            // 
            cardReceivables.BackColor = System.Drawing.Color.White;
            cardReceivables.Controls.Add(cardReceivablesSub);
            cardReceivables.Controls.Add(cardReceivablesValue);
            cardReceivables.Controls.Add(cardReceivablesTitle);
            cardReceivables.Controls.Add(cardReceivablesAccent);
            cardReceivables.Margin = new Padding(6);
            cardReceivables.Name = "cardReceivables";
            cardReceivables.Size = new Size(182, 128);
            cardReceivables.TabIndex = 4;
            // 
            // cardReceivablesAccent
            // 
            cardReceivablesAccent.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(16), Convert.ToInt32(150), Convert.ToInt32(96));
            cardReceivablesAccent.Dock = DockStyle.Bottom;
            cardReceivablesAccent.Height = 4;
            cardReceivablesAccent.Name = "cardReceivablesAccent";
            cardReceivablesAccent.TabIndex = 3;
            // 
            // cardReceivablesTitle
            // 
            cardReceivablesTitle.AutoSize = true;
            cardReceivablesTitle.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            cardReceivablesTitle.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            cardReceivablesTitle.Location = new Point(12, 10);
            cardReceivablesTitle.Name = "cardReceivablesTitle";
            cardReceivablesTitle.TabIndex = 0;
            cardReceivablesTitle.Text = "المستحق لنا (عملاء)";
            // 
            // cardReceivablesValue
            // 
            cardReceivablesValue.AutoSize = true;
            cardReceivablesValue.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            cardReceivablesValue.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            cardReceivablesValue.Location = new Point(12, 36);
            cardReceivablesValue.Name = "cardReceivablesValue";
            cardReceivablesValue.TabIndex = 1;
            cardReceivablesValue.Text = "0.00";
            // 
            // cardReceivablesSub
            // 
            cardReceivablesSub.AutoSize = true;
            cardReceivablesSub.Font = new Font("Tahoma", 9F);
            cardReceivablesSub.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            cardReceivablesSub.Location = new Point(12, 76);
            cardReceivablesSub.MaximumSize = new Size(158, 0);
            cardReceivablesSub.Name = "cardReceivablesSub";
            cardReceivablesSub.TabIndex = 2;
            cardReceivablesSub.Text = "أرصدة العملاء المدينة";
            // 
            // cardPayables
            // 
            cardPayables.BackColor = System.Drawing.Color.White;
            cardPayables.Controls.Add(cardPayablesSub);
            cardPayables.Controls.Add(cardPayablesValue);
            cardPayables.Controls.Add(cardPayablesTitle);
            cardPayables.Controls.Add(cardPayablesAccent);
            cardPayables.Margin = new Padding(6);
            cardPayables.Name = "cardPayables";
            cardPayables.Size = new Size(182, 128);
            cardPayables.TabIndex = 5;
            // 
            // cardPayablesAccent
            // 
            cardPayablesAccent.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(16), Convert.ToInt32(150), Convert.ToInt32(96));
            cardPayablesAccent.Dock = DockStyle.Bottom;
            cardPayablesAccent.Height = 4;
            cardPayablesAccent.Name = "cardPayablesAccent";
            cardPayablesAccent.TabIndex = 3;
            // 
            // cardPayablesTitle
            // 
            cardPayablesTitle.AutoSize = true;
            cardPayablesTitle.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            cardPayablesTitle.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            cardPayablesTitle.Location = new Point(12, 10);
            cardPayablesTitle.Name = "cardPayablesTitle";
            cardPayablesTitle.TabIndex = 0;
            cardPayablesTitle.Text = "المستحق لهم (موردون)";
            // 
            // cardPayablesValue
            // 
            cardPayablesValue.AutoSize = true;
            cardPayablesValue.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            cardPayablesValue.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            cardPayablesValue.Location = new Point(12, 36);
            cardPayablesValue.Name = "cardPayablesValue";
            cardPayablesValue.TabIndex = 1;
            cardPayablesValue.Text = "0.00";
            // 
            // cardPayablesSub
            // 
            cardPayablesSub.AutoSize = true;
            cardPayablesSub.Font = new Font("Tahoma", 9F);
            cardPayablesSub.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            cardPayablesSub.Location = new Point(12, 76);
            cardPayablesSub.MaximumSize = new Size(158, 0);
            cardPayablesSub.Name = "cardPayablesSub";
            cardPayablesSub.TabIndex = 2;
            cardPayablesSub.Text = "أرصدة الموردين الدائنة";
            // 
            // gbDetails — الكتلة (4) من RULE-N135: التفاصيل/الإجماليات
            // 
            gbDetails.BackColor = System.Drawing.Color.White;
            gbDetails.Controls.Add(lblDetails);
            gbDetails.Controls.Add(lblDataStatus);
            gbDetails.Dock = DockStyle.Fill;
            gbDetails.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            gbDetails.Name = "gbDetails";
            gbDetails.Padding = new Padding(10, 8, 10, 8);
            gbDetails.TabIndex = 3;
            gbDetails.TabStop = false;
            gbDetails.Text = "ملخص الأرقام";
            // 
            // lblDetails
            // 
            lblDetails.Dock = DockStyle.Fill;
            lblDetails.Font = new Font("Tahoma", 11F);
            lblDetails.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            lblDetails.Name = "lblDetails";
            lblDetails.Padding = new Padding(12, 8, 12, 8);
            lblDetails.TabIndex = 0;
            lblDetails.Text = "جارٍ تحميل بيانات اللوحة…";
            lblDetails.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblDataStatus — حالة العرض: تحميل / لا توجد بيانات / آخر تحديث
            // 
            lblDataStatus.Dock = DockStyle.Bottom;
            lblDataStatus.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            lblDataStatus.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            lblDataStatus.Height = 28;
            lblDataStatus.Name = "lblDataStatus";
            lblDataStatus.TabIndex = 1;
            lblDataStatus.Text = "جارٍ التحميل…";
            lblDataStatus.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // ctrlDashboard
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(240), Convert.ToInt32(242), Convert.ToInt32(245));
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
