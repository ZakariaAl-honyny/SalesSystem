namespace SalesSystem.Desktop.Screens.Dashboard.Controls
{
    partial class ctrlDashboard
    {
        private System.ComponentModel.IContainer components = null;

        #region Component Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            lblMode = new Label();
            lblWelcome = new Label();
            flowCards = new FlowLayoutPanel();

            // ── بطاقات المؤشرات الست — أسماء العناصر محفوظة كما كانت في frmDashboard ──
            cardSales = BuildCard("cardSales", 0, Color.FromArgb(52, 152, 219),
                "إجمالي المبيعات", "0.00",
                "عدد الفواتير اليوم: 0\nهذا الشهر: 0.00", "lblSalesSub",
                out cardSalesAccent, out cardSalesTitle, out cardSalesValue, out lblSalesSub);

            cardPurchases = BuildCard("cardPurchases", 1, Color.FromArgb(231, 76, 60),
                "إجمالي المشتريات", "0.00",
                "هذا الشهر: 0.00", "lblPurchasesSub",
                out cardPurchasesAccent, out cardPurchasesTitle, out cardPurchasesValue, out lblPurchasesSub);

            cardCustomers = BuildCard("cardCustomers", 2, Color.FromArgb(46, 204, 113),
                "عدد العملاء", "0",
                "الموردون النشطون: 0", "lblCustomersSub",
                out cardCustomersAccent, out cardCustomersTitle, out cardCustomersValue, out lblCustomersSub);

            cardLowStock = BuildCard("cardLowStock", 3, Color.FromArgb(241, 196, 15),
                "مخزون منخفض", "0",
                "إجمالي الأصناف: 0", "lblStockSub",
                out cardLowStockAccent, out cardLowStockTitle, out cardLowStockValue, out lblStockSub);

            cardReceivables = BuildCard("cardReceivables", 4, Color.FromArgb(155, 89, 182),
                "المستحق لنا (عملاء)", "0.00",
                "أرصدة العملاء المدينة", "cardReceivablesSub",
                out cardReceivablesAccent, out cardReceivablesTitle, out cardReceivablesValue, out cardReceivablesSub);

            cardPayables = BuildCard("cardPayables", 5, Color.FromArgb(52, 73, 94),
                "المستحق لهم (موردون)", "0.00",
                "أرصدة الموردين الدائنة", "cardPayablesSub",
                out cardPayablesAccent, out cardPayablesTitle, out cardPayablesValue, out cardPayablesSub);

            SuspendLayout();

            // 
            // lblMode — شريط عنوان المكوّن (RULE-N44 / RULE-N53)
            // 
            lblMode.Dock = DockStyle.Top;
            lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            lblMode.Height = 44;
            lblMode.Name = "lblMode";
            lblMode.TabIndex = 0;
            lblMode.Text = "لوحة المعلومات";
            lblMode.TextAlign = ContentAlignment.MiddleCenter;

            // 
            // lblWelcome
            // 
            lblWelcome.Dock = DockStyle.Top;
            lblWelcome.Font = new Font("Tahoma", 14F, FontStyle.Bold);
            lblWelcome.Height = 40;
            lblWelcome.Name = "lblWelcome";
            lblWelcome.Padding = new Padding(16, 0, 16, 0);
            lblWelcome.TabIndex = 1;
            lblWelcome.Text = "أهلاً";
            lblWelcome.TextAlign = ContentAlignment.MiddleCenter;

            // 
            // flowCards
            // 
            flowCards.AutoScroll = true;
            flowCards.Controls.Add(cardSales);
            flowCards.Controls.Add(cardPurchases);
            flowCards.Controls.Add(cardCustomers);
            flowCards.Controls.Add(cardLowStock);
            flowCards.Controls.Add(cardReceivables);
            flowCards.Controls.Add(cardPayables);
            flowCards.Dock = DockStyle.Top;
            flowCards.FlowDirection = FlowDirection.RightToLeft;
            flowCards.Height = 172;
            flowCards.Name = "flowCards";
            flowCards.Padding = new Padding(10);
            flowCards.TabIndex = 2;
            flowCards.WrapContents = false;

            // 
            // ctrlDashboard
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            Controls.Add(flowCards);
            Controls.Add(lblWelcome);
            Controls.Add(lblMode);
            Font = new Font("Tahoma", 10F);
            Name = "ctrlDashboard";
            RightToLeft = RightToLeft.Yes;
            Size = new Size(1200, 420);

            ResumeLayout(false);
        }

        /// <summary>
        /// يبني بطاقة مؤشر واحدة: لوحة + شريط لون سفلي + عنوان + قيمة + سطر ثانوي.
        /// خلفية البطاقة تُترك للثيم الأخضر (Walk في <c>GreenThemeHelper</c>)،
        /// ولون السطر الثانوي يُطبَّق من الشاشة (RULE-N54).
        /// </summary>
        private static Panel BuildCard(string cardName, int tabIndex, Color accentColor,
            string titleText, string valueText, string subText, string subName,
            out Label accentLabel, out Label titleLabel, out Label valueLabel, out Label subLabel)
        {
            var card = new Panel
            {
                BackColor = Color.White,
                Margin = new Padding(8),
                Name = cardName,
                Padding = new Padding(14),
                Size = new Size(244, 132),
                TabIndex = tabIndex
            };

            accentLabel = new Label
            {
                BackColor = accentColor,
                Dock = DockStyle.Bottom,
                Height = 4,
                Name = cardName + "Accent",
                TabIndex = 0
            };

            titleLabel = new Label
            {
                AutoSize = true,
                Font = new Font("Tahoma", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(127, 140, 141),
                Location = new Point(14, 10),
                Name = cardName + "Title",
                TabIndex = 1,
                Text = titleText
            };

            valueLabel = new Label
            {
                AutoSize = true,
                Font = new Font("Tahoma", 18F, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(14, 36),
                Name = cardName + "Value",
                TabIndex = 2,
                Text = valueText
            };

            subLabel = new Label
            {
                AutoSize = true,
                Font = new Font("Tahoma", 9F),
                Location = new Point(14, 86),
                Name = subName,
                TabIndex = 3,
                Text = subText
            };

            card.Controls.Add(subLabel);
            card.Controls.Add(valueLabel);
            card.Controls.Add(titleLabel);
            card.Controls.Add(accentLabel);
            return card;
        }

        #endregion

        private Label lblMode;
        private Label lblWelcome;
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
    }
}
