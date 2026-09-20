namespace SalesSystem.Desktop.Screens.Dashboard.Forms
{
    partial class frmDashboard
    {
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            lblWelcome = new Label();
            lblMode = new Label();
            flowCards = new FlowLayoutPanel();
            cardSales = new Panel();
            cardSalesAccent = new Label();
            cardSalesTitle = new Label();
            cardSalesValue = new Label();
            cardPurchases = new Panel();
            cardPurchasesAccent = new Label();
            cardPurchasesTitle = new Label();
            cardPurchasesValue = new Label();
            cardCustomers = new Panel();
            cardCustomersAccent = new Label();
            cardCustomersTitle = new Label();
            cardCustomersValue = new Label();
            cardLowStock = new Panel();
            cardLowStockAccent = new Label();
            cardLowStockTitle = new Label();
            cardLowStockValue = new Label();
            cardReceivables = new Panel();
            cardReceivablesAccent = new Label();
            cardReceivablesTitle = new Label();
            cardReceivablesValue = new Label();
            cardPayables = new Panel();
            cardPayablesAccent = new Label();
            cardPayablesTitle = new Label();
            cardPayablesValue = new Label();
            btnClose = new Button();
            flowCards.SuspendLayout();
            cardSales.SuspendLayout();
            cardPurchases.SuspendLayout();
            cardCustomers.SuspendLayout();
            cardLowStock.SuspendLayout();
            cardReceivables.SuspendLayout();
            cardPayables.SuspendLayout();
            SuspendLayout();
            // 
            // lblWelcome
            // 
            lblWelcome.AutoSize = true;
            lblWelcome.Font = new Font("Tahoma", 14F, FontStyle.Bold);
            lblWelcome.ForeColor = Color.FromArgb(52, 73, 94);
            lblWelcome.Location = new Point(20, 15);
            lblWelcome.Name = "lblWelcome";
            lblWelcome.Size = new Size(128, 34);
            lblWelcome.TabIndex = 0;
            lblWelcome.Text = "\u0623\u0647\u0644\u0627\u064B";
            // 
            // lblMode
            // 
            lblMode.Anchor = AnchorStyles.Top;
            lblMode.AutoSize = true;
            lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            lblMode.ForeColor = Color.Firebrick;
            lblMode.Location = new Point(667, 12);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(161, 39);
            lblMode.TabIndex = 1;
            lblMode.Text = "\u0644\u0648\u062D\u0629 \u0627\u0644\u0645\u0639\u0644\u0648\u0645\u0627\u062A";
            // 
            // flowCards
            // 
            flowCards.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            flowCards.Controls.Add(cardSales);
            flowCards.Controls.Add(cardPurchases);
            flowCards.Controls.Add(cardCustomers);
            flowCards.Controls.Add(cardLowStock);
            flowCards.Controls.Add(cardReceivables);
            flowCards.Controls.Add(cardPayables);
            flowCards.FlowDirection = FlowDirection.RightToLeft;
            flowCards.Location = new Point(12, 60);
            flowCards.Name = "flowCards";
            flowCards.Padding = new Padding(10);
            flowCards.Size = new Size(1540, 130);
            flowCards.TabIndex = 2;
            // 
            // cardSales
            // 
            cardSales.BackColor = Color.White;
            cardSales.Controls.Add(cardSalesAccent);
            cardSales.Controls.Add(cardSalesTitle);
            cardSales.Controls.Add(cardSalesValue);
            cardSales.Margin = new Padding(10);
            cardSales.Name = "cardSales";
            cardSales.Padding = new Padding(15);
            cardSales.Size = new Size(220, 100);
            cardSales.TabIndex = 0;
            // 
            // cardSalesAccent
            // 
            cardSalesAccent.BackColor = Color.FromArgb(52, 152, 219);
            cardSalesAccent.Dock = DockStyle.Bottom;
            cardSalesAccent.Height = 4;
            cardSalesAccent.Name = "cardSalesAccent";
            cardSalesAccent.TabIndex = 0;
            // 
            // cardSalesTitle
            // 
            cardSalesTitle.AutoSize = true;
            cardSalesTitle.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            cardSalesTitle.ForeColor = Color.FromArgb(127, 140, 141);
            cardSalesTitle.Location = new Point(15, 10);
            cardSalesTitle.Name = "cardSalesTitle";
            cardSalesTitle.Size = new Size(147, 24);
            cardSalesTitle.TabIndex = 1;
            cardSalesTitle.Text = "\u0625\u062C\u0645\u0627\u0644\u064A \u0627\u0644\u0645\u0628\u064A\u0639\u0627\u062A";
            // 
            // cardSalesValue
            // 
            cardSalesValue.AutoSize = true;
            cardSalesValue.Font = new Font("Tahoma", 20F, FontStyle.Bold);
            cardSalesValue.ForeColor = Color.FromArgb(44, 62, 80);
            cardSalesValue.Location = new Point(15, 40);
            cardSalesValue.Name = "cardSalesValue";
            cardSalesValue.Size = new Size(108, 48);
            cardSalesValue.TabIndex = 2;
            cardSalesValue.Text = "0.00";
            // 
            // cardPurchases
            // 
            cardPurchases.BackColor = Color.White;
            cardPurchases.Controls.Add(cardPurchasesAccent);
            cardPurchases.Controls.Add(cardPurchasesTitle);
            cardPurchases.Controls.Add(cardPurchasesValue);
            cardPurchases.Margin = new Padding(10);
            cardPurchases.Name = "cardPurchases";
            cardPurchases.Padding = new Padding(15);
            cardPurchases.Size = new Size(220, 100);
            cardPurchases.TabIndex = 1;
            // 
            // cardPurchasesAccent
            // 
            cardPurchasesAccent.BackColor = Color.FromArgb(231, 76, 60);
            cardPurchasesAccent.Dock = DockStyle.Bottom;
            cardPurchasesAccent.Height = 4;
            cardPurchasesAccent.Name = "cardPurchasesAccent";
            cardPurchasesAccent.TabIndex = 0;
            // 
            // cardPurchasesTitle
            // 
            cardPurchasesTitle.AutoSize = true;
            cardPurchasesTitle.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            cardPurchasesTitle.ForeColor = Color.FromArgb(127, 140, 141);
            cardPurchasesTitle.Location = new Point(15, 10);
            cardPurchasesTitle.Name = "cardPurchasesTitle";
            cardPurchasesTitle.Size = new Size(161, 24);
            cardPurchasesTitle.TabIndex = 1;
            cardPurchasesTitle.Text = "\u0625\u062C\u0645\u0627\u0644\u064A \u0627\u0644\u0645\u0634\u062A\u0631\u064A\u0627\u062A";
            // 
            // cardPurchasesValue
            // 
            cardPurchasesValue.AutoSize = true;
            cardPurchasesValue.Font = new Font("Tahoma", 20F, FontStyle.Bold);
            cardPurchasesValue.ForeColor = Color.FromArgb(44, 62, 80);
            cardPurchasesValue.Location = new Point(15, 40);
            cardPurchasesValue.Name = "cardPurchasesValue";
            cardPurchasesValue.Size = new Size(108, 48);
            cardPurchasesValue.TabIndex = 2;
            cardPurchasesValue.Text = "0.00";
            // 
            // cardCustomers
            // 
            cardCustomers.BackColor = Color.White;
            cardCustomers.Controls.Add(cardCustomersAccent);
            cardCustomers.Controls.Add(cardCustomersTitle);
            cardCustomers.Controls.Add(cardCustomersValue);
            cardCustomers.Margin = new Padding(10);
            cardCustomers.Name = "cardCustomers";
            cardCustomers.Padding = new Padding(15);
            cardCustomers.Size = new Size(220, 100);
            cardCustomers.TabIndex = 2;
            // 
            // cardCustomersAccent
            // 
            cardCustomersAccent.BackColor = Color.FromArgb(46, 204, 113);
            cardCustomersAccent.Dock = DockStyle.Bottom;
            cardCustomersAccent.Height = 4;
            cardCustomersAccent.Name = "cardCustomersAccent";
            cardCustomersAccent.TabIndex = 0;
            // 
            // cardCustomersTitle
            // 
            cardCustomersTitle.AutoSize = true;
            cardCustomersTitle.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            cardCustomersTitle.ForeColor = Color.FromArgb(127, 140, 141);
            cardCustomersTitle.Location = new Point(15, 10);
            cardCustomersTitle.Name = "cardCustomersTitle";
            cardCustomersTitle.Size = new Size(132, 24);
            cardCustomersTitle.TabIndex = 1;
            cardCustomersTitle.Text = "\u0639\u062F\u062F \u0627\u0644\u0639\u0645\u0644\u0627\u0621";
            // 
            // cardCustomersValue
            // 
            cardCustomersValue.AutoSize = true;
            cardCustomersValue.Font = new Font("Tahoma", 20F, FontStyle.Bold);
            cardCustomersValue.ForeColor = Color.FromArgb(44, 62, 80);
            cardCustomersValue.Location = new Point(15, 40);
            cardCustomersValue.Name = "cardCustomersValue";
            cardCustomersValue.Size = new Size(44, 48);
            cardCustomersValue.TabIndex = 2;
            cardCustomersValue.Text = "0";
            // 
            // cardLowStock
            // 
            cardLowStock.BackColor = Color.White;
            cardLowStock.Controls.Add(cardLowStockAccent);
            cardLowStock.Controls.Add(cardLowStockTitle);
            cardLowStock.Controls.Add(cardLowStockValue);
            cardLowStock.Margin = new Padding(10);
            cardLowStock.Name = "cardLowStock";
            cardLowStock.Padding = new Padding(15);
            cardLowStock.Size = new Size(220, 100);
            cardLowStock.TabIndex = 3;
            // 
            // cardLowStockAccent
            // 
            cardLowStockAccent.BackColor = Color.FromArgb(241, 196, 15);
            cardLowStockAccent.Dock = DockStyle.Bottom;
            cardLowStockAccent.Height = 4;
            cardLowStockAccent.Name = "cardLowStockAccent";
            cardLowStockAccent.TabIndex = 0;
            // 
            // cardLowStockTitle
            // 
            cardLowStockTitle.AutoSize = true;
            cardLowStockTitle.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            cardLowStockTitle.ForeColor = Color.FromArgb(127, 140, 141);
            cardLowStockTitle.Location = new Point(15, 10);
            cardLowStockTitle.Name = "cardLowStockTitle";
            cardLowStockTitle.Size = new Size(137, 24);
            cardLowStockTitle.TabIndex = 1;
            cardLowStockTitle.Text = "\u0645\u062E\u0632\u0648\u0646 \u0645\u0646\u062E\u0641\u0636";
            // 
            // cardLowStockValue
            // 
            cardLowStockValue.AutoSize = true;
            cardLowStockValue.Font = new Font("Tahoma", 20F, FontStyle.Bold);
            cardLowStockValue.ForeColor = Color.FromArgb(44, 62, 80);
            cardLowStockValue.Location = new Point(15, 40);
            cardLowStockValue.Name = "cardLowStockValue";
            cardLowStockValue.Size = new Size(44, 48);
            cardLowStockValue.TabIndex = 2;
            cardLowStockValue.Text = "0";
            // 
            // cardReceivables
            // 
            cardReceivables.BackColor = Color.White;
            cardReceivables.Controls.Add(cardReceivablesAccent);
            cardReceivables.Controls.Add(cardReceivablesTitle);
            cardReceivables.Controls.Add(cardReceivablesValue);
            cardReceivables.Margin = new Padding(10);
            cardReceivables.Name = "cardReceivables";
            cardReceivables.Padding = new Padding(15);
            cardReceivables.Size = new Size(220, 100);
            cardReceivables.TabIndex = 4;
            // 
            // cardReceivablesAccent
            // 
            cardReceivablesAccent.BackColor = Color.FromArgb(155, 89, 182);
            cardReceivablesAccent.Dock = DockStyle.Bottom;
            cardReceivablesAccent.Height = 4;
            cardReceivablesAccent.Name = "cardReceivablesAccent";
            cardReceivablesAccent.TabIndex = 0;
            // 
            // cardReceivablesTitle
            // 
            cardReceivablesTitle.AutoSize = true;
            cardReceivablesTitle.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            cardReceivablesTitle.ForeColor = Color.FromArgb(127, 140, 141);
            cardReceivablesTitle.Location = new Point(15, 10);
            cardReceivablesTitle.Name = "cardReceivablesTitle";
            cardReceivablesTitle.Size = new Size(175, 48);
            cardReceivablesTitle.TabIndex = 1;
            cardReceivablesTitle.Text = "\u0627\u0644\u0645\u0628\u0627\u0644\u063A \u0627\u0644\u0645\u0633\u062A\u062D\u0642\u0629\r\n(\u0644\u0646\u0627)";
            // 
            // cardReceivablesValue
            // 
            cardReceivablesValue.AutoSize = true;
            cardReceivablesValue.Font = new Font("Tahoma", 20F, FontStyle.Bold);
            cardReceivablesValue.ForeColor = Color.FromArgb(44, 62, 80);
            cardReceivablesValue.Location = new Point(15, 55);
            cardReceivablesValue.Name = "cardReceivablesValue";
            cardReceivablesValue.Size = new Size(108, 48);
            cardReceivablesValue.TabIndex = 2;
            cardReceivablesValue.Text = "0.00";
            // 
            // cardPayables
            // 
            cardPayables.BackColor = Color.White;
            cardPayables.Controls.Add(cardPayablesAccent);
            cardPayables.Controls.Add(cardPayablesTitle);
            cardPayables.Controls.Add(cardPayablesValue);
            cardPayables.Margin = new Padding(10);
            cardPayables.Name = "cardPayables";
            cardPayables.Padding = new Padding(15);
            cardPayables.Size = new Size(220, 100);
            cardPayables.TabIndex = 5;
            // 
            // cardPayablesAccent
            // 
            cardPayablesAccent.BackColor = Color.FromArgb(52, 73, 94);
            cardPayablesAccent.Dock = DockStyle.Bottom;
            cardPayablesAccent.Height = 4;
            cardPayablesAccent.Name = "cardPayablesAccent";
            cardPayablesAccent.TabIndex = 0;
            // 
            // cardPayablesTitle
            // 
            cardPayablesTitle.AutoSize = true;
            cardPayablesTitle.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            cardPayablesTitle.ForeColor = Color.FromArgb(127, 140, 141);
            cardPayablesTitle.Location = new Point(15, 10);
            cardPayablesTitle.Name = "cardPayablesTitle";
            cardPayablesTitle.Size = new Size(175, 48);
            cardPayablesTitle.TabIndex = 1;
            cardPayablesTitle.Text = "\u0627\u0644\u0645\u0628\u0627\u0644\u063A \u0627\u0644\u0645\u0633\u062A\u062D\u0642\u0629\r\n(\u0644\u0647\u0645)";
            // 
            // cardPayablesValue
            // 
            cardPayablesValue.AutoSize = true;
            cardPayablesValue.Font = new Font("Tahoma", 20F, FontStyle.Bold);
            cardPayablesValue.ForeColor = Color.FromArgb(44, 62, 80);
            cardPayablesValue.Location = new Point(15, 55);
            cardPayablesValue.Name = "cardPayablesValue";
            cardPayablesValue.Size = new Size(108, 48);
            cardPayablesValue.TabIndex = 2;
            cardPayablesValue.Text = "0.00";
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnClose.BackColor = Color.White;
            btnClose.DialogResult = DialogResult.Cancel;
            btnClose.FlatStyle = FlatStyle.Popup;
            btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnClose.Location = new Point(12, 710);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(181, 61);
            btnClose.TabIndex = 10;
            btnClose.Text = "\u0625\u063A\u0644\u0627\u0642";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // frmDashboard
            // 
            AcceptButton = btnClose;
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            CancelButton = btnClose;
            ClientSize = new Size(1564, 783);
            Controls.Add(btnClose);
            Controls.Add(flowCards);
            Controls.Add(lblMode);
            Controls.Add(lblWelcome);
            Font = new Font("Tahoma", 12F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "frmDashboard";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "\u0644\u0648\u062D\u0629 \u0627\u0644\u0645\u0639\u0644\u0648\u0645\u0627\u062A";
            Load += frmDashboard_Load;
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
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblMode;
        private Label lblWelcome;
        private FlowLayoutPanel flowCards;
        private Panel cardSales;
        private Label cardSalesAccent;
        private Label cardSalesTitle;
        private Label cardSalesValue;
        private Panel cardPurchases;
        private Label cardPurchasesAccent;
        private Label cardPurchasesTitle;
        private Label cardPurchasesValue;
        private Panel cardCustomers;
        private Label cardCustomersAccent;
        private Label cardCustomersTitle;
        private Label cardCustomersValue;
        private Panel cardLowStock;
        private Label cardLowStockAccent;
        private Label cardLowStockTitle;
        private Label cardLowStockValue;
        private Panel cardReceivables;
        private Label cardReceivablesAccent;
        private Label cardReceivablesTitle;
        private Label cardReceivablesValue;
        private Panel cardPayables;
        private Label cardPayablesAccent;
        private Label cardPayablesTitle;
        private Label cardPayablesValue;
        private Button btnClose;
    }
}
