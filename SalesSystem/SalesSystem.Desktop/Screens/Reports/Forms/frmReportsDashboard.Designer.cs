namespace SalesSystem.Desktop.Screens.Reports.Forms
{
    partial class frmReportsDashboard
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
            pnlHeader = new Panel();
            lblMode = new Label();
            flowKpis = new FlowLayoutPanel();
            cardSales = new Panel();
            cardSalesAccent = new Label();
            cardSalesTitle = new Label();
            cardSalesValue = new Label();
            cardSalesSubtitle = new Label();
            cardPurchases = new Panel();
            cardPurchasesAccent = new Label();
            cardPurchasesTitle = new Label();
            cardPurchasesValue = new Label();
            cardPurchasesSubtitle = new Label();
            cardStock = new Panel();
            cardStockAccent = new Label();
            cardStockTitle = new Label();
            cardStockValue = new Label();
            cardStockSubtitle = new Label();
            cardReceivables = new Panel();
            cardReceivablesAccent = new Label();
            cardReceivablesTitle = new Label();
            cardReceivablesValue = new Label();
            cardReceivablesSubtitle = new Label();
            tblBody = new TableLayoutPanel();
            pnlReports = new Panel();
            lblReportsTitle = new Label();
            tvReports = new TreeView();
            pnlPreview = new Panel();
            lblPreviewTitle = new Label();
            lblPreviewPlaceholder = new Label();
            pnlFooter = new Panel();
            btnClose = new Button();
            lblFooterHint = new Label();
            pnlHeader.SuspendLayout();
            flowKpis.SuspendLayout();
            cardSales.SuspendLayout();
            cardPurchases.SuspendLayout();
            cardStock.SuspendLayout();
            cardReceivables.SuspendLayout();
            tblBody.SuspendLayout();
            pnlReports.SuspendLayout();
            pnlPreview.SuspendLayout();
            pnlFooter.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(44, 62, 80);
            pnlHeader.Controls.Add(lblMode);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 58;
            pnlHeader.Name = "pnlHeader";
            pnlHeader.TabIndex = 0;
            // 
            // lblMode
            // 
            lblMode.Dock = DockStyle.Fill;
            lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            lblMode.ForeColor = Color.White;
            lblMode.Name = "lblMode";
            lblMode.TabIndex = 0;
            lblMode.Text = "لوحة التقارير";
            lblMode.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // flowKpis
            // 
            flowKpis.BackColor = Color.WhiteSmoke;
            flowKpis.Controls.Add(cardSales);
            flowKpis.Controls.Add(cardPurchases);
            flowKpis.Controls.Add(cardStock);
            flowKpis.Controls.Add(cardReceivables);
            flowKpis.Dock = DockStyle.Top;
            flowKpis.FlowDirection = FlowDirection.RightToLeft;
            flowKpis.Height = 132;
            flowKpis.Name = "flowKpis";
            flowKpis.Padding = new Padding(10, 8, 10, 8);
            flowKpis.TabIndex = 1;
            // 
            // cardSales
            // 
            cardSales.BackColor = Color.White;
            cardSales.Controls.Add(cardSalesAccent);
            cardSales.Controls.Add(cardSalesTitle);
            cardSales.Controls.Add(cardSalesValue);
            cardSales.Controls.Add(cardSalesSubtitle);
            cardSales.Margin = new Padding(6, 4, 6, 4);
            cardSales.Name = "cardSales";
            cardSales.Padding = new Padding(14);
            cardSales.Size = new Size(250, 100);
            cardSales.TabIndex = 0;
            // 
            // cardSalesAccent
            // 
            cardSalesAccent.BackColor = Color.FromArgb(46, 204, 113);
            cardSalesAccent.Dock = DockStyle.Bottom;
            cardSalesAccent.Height = 5;
            cardSalesAccent.Name = "cardSalesAccent";
            cardSalesAccent.TabIndex = 0;
            // 
            // cardSalesTitle
            // 
            cardSalesTitle.AutoSize = true;
            cardSalesTitle.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            cardSalesTitle.ForeColor = Color.FromArgb(127, 140, 141);
            cardSalesTitle.Location = new Point(14, 10);
            cardSalesTitle.Name = "cardSalesTitle";
            cardSalesTitle.TabIndex = 1;
            cardSalesTitle.Text = "إجمالي المبيعات";
            // 
            // cardSalesValue
            // 
            cardSalesValue.AutoSize = true;
            cardSalesValue.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            cardSalesValue.ForeColor = Color.FromArgb(44, 62, 80);
            cardSalesValue.Location = new Point(14, 36);
            cardSalesValue.Name = "cardSalesValue";
            cardSalesValue.TabIndex = 2;
            cardSalesValue.Text = "0.00";
            // 
            // cardSalesSubtitle
            // 
            cardSalesSubtitle.AutoSize = true;
            cardSalesSubtitle.Font = new Font("Tahoma", 8.5F);
            cardSalesSubtitle.ForeColor = Color.FromArgb(149, 165, 166);
            cardSalesSubtitle.Location = new Point(14, 66);
            cardSalesSubtitle.Name = "cardSalesSubtitle";
            cardSalesSubtitle.TabIndex = 3;
            cardSalesSubtitle.Text = "ريال يمني — حتى تاريخه";
            // 
            // cardPurchases
            // 
            cardPurchases.BackColor = Color.White;
            cardPurchases.Controls.Add(cardPurchasesAccent);
            cardPurchases.Controls.Add(cardPurchasesTitle);
            cardPurchases.Controls.Add(cardPurchasesValue);
            cardPurchases.Controls.Add(cardPurchasesSubtitle);
            cardPurchases.Margin = new Padding(6, 4, 6, 4);
            cardPurchases.Name = "cardPurchases";
            cardPurchases.Padding = new Padding(14);
            cardPurchases.Size = new Size(250, 100);
            cardPurchases.TabIndex = 1;
            // 
            // cardPurchasesAccent
            // 
            cardPurchasesAccent.BackColor = Color.FromArgb(243, 156, 18);
            cardPurchasesAccent.Dock = DockStyle.Bottom;
            cardPurchasesAccent.Height = 5;
            cardPurchasesAccent.Name = "cardPurchasesAccent";
            cardPurchasesAccent.TabIndex = 0;
            // 
            // cardPurchasesTitle
            // 
            cardPurchasesTitle.AutoSize = true;
            cardPurchasesTitle.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            cardPurchasesTitle.ForeColor = Color.FromArgb(127, 140, 141);
            cardPurchasesTitle.Location = new Point(14, 10);
            cardPurchasesTitle.Name = "cardPurchasesTitle";
            cardPurchasesTitle.TabIndex = 1;
            cardPurchasesTitle.Text = "إجمالي المشتريات";
            // 
            // cardPurchasesValue
            // 
            cardPurchasesValue.AutoSize = true;
            cardPurchasesValue.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            cardPurchasesValue.ForeColor = Color.FromArgb(44, 62, 80);
            cardPurchasesValue.Location = new Point(14, 36);
            cardPurchasesValue.Name = "cardPurchasesValue";
            cardPurchasesValue.TabIndex = 2;
            cardPurchasesValue.Text = "0.00";
            // 
            // cardPurchasesSubtitle
            // 
            cardPurchasesSubtitle.AutoSize = true;
            cardPurchasesSubtitle.Font = new Font("Tahoma", 8.5F);
            cardPurchasesSubtitle.ForeColor = Color.FromArgb(149, 165, 166);
            cardPurchasesSubtitle.Location = new Point(14, 66);
            cardPurchasesSubtitle.Name = "cardPurchasesSubtitle";
            cardPurchasesSubtitle.TabIndex = 3;
            cardPurchasesSubtitle.Text = "ريال يمني — حتى تاريخه";
            // 
            // cardStock
            // 
            cardStock.BackColor = Color.White;
            cardStock.Controls.Add(cardStockAccent);
            cardStock.Controls.Add(cardStockTitle);
            cardStock.Controls.Add(cardStockValue);
            cardStock.Controls.Add(cardStockSubtitle);
            cardStock.Margin = new Padding(6, 4, 6, 4);
            cardStock.Name = "cardStock";
            cardStock.Padding = new Padding(14);
            cardStock.Size = new Size(250, 100);
            cardStock.TabIndex = 2;
            // 
            // cardStockAccent
            // 
            cardStockAccent.BackColor = Color.FromArgb(52, 152, 219);
            cardStockAccent.Dock = DockStyle.Bottom;
            cardStockAccent.Height = 5;
            cardStockAccent.Name = "cardStockAccent";
            cardStockAccent.TabIndex = 0;
            // 
            // cardStockTitle
            // 
            cardStockTitle.AutoSize = true;
            cardStockTitle.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            cardStockTitle.ForeColor = Color.FromArgb(127, 140, 141);
            cardStockTitle.Location = new Point(14, 10);
            cardStockTitle.Name = "cardStockTitle";
            cardStockTitle.TabIndex = 1;
            cardStockTitle.Text = "قيمة المخزون";
            // 
            // cardStockValue
            // 
            cardStockValue.AutoSize = true;
            cardStockValue.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            cardStockValue.ForeColor = Color.FromArgb(44, 62, 80);
            cardStockValue.Location = new Point(14, 36);
            cardStockValue.Name = "cardStockValue";
            cardStockValue.TabIndex = 2;
            cardStockValue.Text = "0.00";
            // 
            // cardStockSubtitle
            // 
            cardStockSubtitle.AutoSize = true;
            cardStockSubtitle.Font = new Font("Tahoma", 8.5F);
            cardStockSubtitle.ForeColor = Color.FromArgb(149, 165, 166);
            cardStockSubtitle.Location = new Point(14, 66);
            cardStockSubtitle.Name = "cardStockSubtitle";
            cardStockSubtitle.TabIndex = 3;
            cardStockSubtitle.Text = "ريال يمني — بالتكلفة";
            // 
            // cardReceivables
            // 
            cardReceivables.BackColor = Color.White;
            cardReceivables.Controls.Add(cardReceivablesAccent);
            cardReceivables.Controls.Add(cardReceivablesTitle);
            cardReceivables.Controls.Add(cardReceivablesValue);
            cardReceivables.Controls.Add(cardReceivablesSubtitle);
            cardReceivables.Margin = new Padding(6, 4, 6, 4);
            cardReceivables.Name = "cardReceivables";
            cardReceivables.Padding = new Padding(14);
            cardReceivables.Size = new Size(250, 100);
            cardReceivables.TabIndex = 3;
            // 
            // cardReceivablesAccent
            // 
            cardReceivablesAccent.BackColor = Color.FromArgb(231, 76, 60);
            cardReceivablesAccent.Dock = DockStyle.Bottom;
            cardReceivablesAccent.Height = 5;
            cardReceivablesAccent.Name = "cardReceivablesAccent";
            cardReceivablesAccent.TabIndex = 0;
            // 
            // cardReceivablesTitle
            // 
            cardReceivablesTitle.AutoSize = true;
            cardReceivablesTitle.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            cardReceivablesTitle.ForeColor = Color.FromArgb(127, 140, 141);
            cardReceivablesTitle.Location = new Point(14, 10);
            cardReceivablesTitle.Name = "cardReceivablesTitle";
            cardReceivablesTitle.TabIndex = 1;
            cardReceivablesTitle.Text = "إجمالي الذمم المدينة";
            // 
            // cardReceivablesValue
            // 
            cardReceivablesValue.AutoSize = true;
            cardReceivablesValue.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            cardReceivablesValue.ForeColor = Color.FromArgb(44, 62, 80);
            cardReceivablesValue.Location = new Point(14, 36);
            cardReceivablesValue.Name = "cardReceivablesValue";
            cardReceivablesValue.TabIndex = 2;
            cardReceivablesValue.Text = "0.00";
            // 
            // cardReceivablesSubtitle
            // 
            cardReceivablesSubtitle.AutoSize = true;
            cardReceivablesSubtitle.Font = new Font("Tahoma", 8.5F);
            cardReceivablesSubtitle.ForeColor = Color.FromArgb(149, 165, 166);
            cardReceivablesSubtitle.Location = new Point(14, 66);
            cardReceivablesSubtitle.Name = "cardReceivablesSubtitle";
            cardReceivablesSubtitle.TabIndex = 3;
            cardReceivablesSubtitle.Text = "ريال يمني — أرصدة العملاء";
            // 
            // tblBody
            // 
            tblBody.BackColor = Color.WhiteSmoke;
            tblBody.ColumnCount = 2;
            // In RTL, column 0 renders on the RIGHT (45% preview) and column 1 on the LEFT (55% report list).
            tblBody.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));
            tblBody.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55F));
            tblBody.Controls.Add(pnlPreview, 0, 0);
            tblBody.Controls.Add(pnlReports, 1, 0);
            tblBody.Dock = DockStyle.Fill;
            tblBody.Name = "tblBody";
            tblBody.Padding = new Padding(10);
            tblBody.RightToLeft = RightToLeft.Yes;
            tblBody.RowCount = 1;
            tblBody.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblBody.TabIndex = 2;
            // 
            // pnlReports
            // 
            pnlReports.BackColor = Color.White;
            pnlReports.Controls.Add(tvReports);
            pnlReports.Controls.Add(lblReportsTitle);
            pnlReports.Dock = DockStyle.Fill;
            pnlReports.Name = "pnlReports";
            pnlReports.Padding = new Padding(8, 4, 8, 8);
            pnlReports.TabIndex = 0;
            // 
            // lblReportsTitle
            // 
            lblReportsTitle.BackColor = Color.FromArgb(236, 240, 241);
            lblReportsTitle.Dock = DockStyle.Top;
            lblReportsTitle.Font = new Font("Tahoma", 11F, FontStyle.Bold);
            lblReportsTitle.ForeColor = Color.FromArgb(44, 62, 80);
            lblReportsTitle.Height = 36;
            lblReportsTitle.Name = "lblReportsTitle";
            lblReportsTitle.TabIndex = 0;
            lblReportsTitle.Text = "قائمة التقارير";
            lblReportsTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tvReports
            // 
            tvReports.BorderStyle = BorderStyle.FixedSingle;
            tvReports.Dock = DockStyle.Fill;
            tvReports.Font = new Font("Tahoma", 10F);
            tvReports.FullRowSelect = true;
            tvReports.HideSelection = false;
            tvReports.Indent = 22;
            tvReports.Name = "tvReports";
            tvReports.RightToLeft = RightToLeft.Yes;
            tvReports.TabIndex = 1;
            tvReports.AfterSelect += tvReports_AfterSelect;
            // 
            // pnlPreview
            // 
            pnlPreview.BackColor = Color.White;
            pnlPreview.Controls.Add(lblPreviewPlaceholder);
            pnlPreview.Controls.Add(lblPreviewTitle);
            pnlPreview.Dock = DockStyle.Fill;
            pnlPreview.Name = "pnlPreview";
            pnlPreview.Padding = new Padding(8, 4, 8, 8);
            pnlPreview.TabIndex = 1;
            // 
            // lblPreviewTitle
            // 
            lblPreviewTitle.BackColor = Color.FromArgb(236, 240, 241);
            lblPreviewTitle.Dock = DockStyle.Top;
            lblPreviewTitle.Font = new Font("Tahoma", 11F, FontStyle.Bold);
            lblPreviewTitle.ForeColor = Color.FromArgb(44, 62, 80);
            lblPreviewTitle.Height = 36;
            lblPreviewTitle.Name = "lblPreviewTitle";
            lblPreviewTitle.TabIndex = 0;
            lblPreviewTitle.Text = "معاينة التقرير";
            lblPreviewTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblPreviewPlaceholder
            // 
            lblPreviewPlaceholder.BackColor = Color.FromArgb(245, 246, 250);
            lblPreviewPlaceholder.Dock = DockStyle.Fill;
            lblPreviewPlaceholder.Font = new Font("Tahoma", 11F);
            lblPreviewPlaceholder.ForeColor = Color.FromArgb(127, 140, 141);
            lblPreviewPlaceholder.Name = "lblPreviewPlaceholder";
            lblPreviewPlaceholder.TabIndex = 1;
            lblPreviewPlaceholder.Text = "اختر تقريراً من القائمة لعرضه هنا";
            lblPreviewPlaceholder.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlFooter
            // 
            pnlFooter.BackColor = Color.FromArgb(236, 240, 241);
            pnlFooter.Controls.Add(btnClose);
            pnlFooter.Controls.Add(lblFooterHint);
            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Height = 56;
            pnlFooter.Name = "pnlFooter";
            pnlFooter.Padding = new Padding(8);
            pnlFooter.TabIndex = 3;
            // 
            // btnClose
            // 
            btnClose.BackColor = Color.White;
            btnClose.DialogResult = DialogResult.Cancel;
            btnClose.Dock = DockStyle.Right;
            btnClose.FlatAppearance.BorderColor = Color.Black;
            btnClose.FlatAppearance.BorderSize = 2;
            btnClose.FlatStyle = FlatStyle.Popup;
            btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(120, 40);
            btnClose.TabIndex = 5;
            btnClose.Text = "إغلاق";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;
            // 
            // lblFooterHint
            // 
            lblFooterHint.Dock = DockStyle.Fill;
            lblFooterHint.Font = new Font("Tahoma", 9F, FontStyle.Italic);
            lblFooterHint.ForeColor = Color.Gray;
            lblFooterHint.Name = "lblFooterHint";
            lblFooterHint.TabIndex = 6;
            lblFooterHint.Text = "وضع التجربة (Mock) — بيانات وهمية للعرض فقط، بدون اتصال بالخادم";
            lblFooterHint.TextAlign = ContentAlignment.MiddleRight;
            // 
            // frmReportsDashboard
            // 
            AcceptButton = btnClose;
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            CancelButton = btnClose;
            ClientSize = new Size(1150, 700);
            // Dock order (reverse z-order): last added = topmost.
            // pnlHeader (Top, added last) renders at the very top, flowKpis below it.
            Controls.Add(tblBody);
            Controls.Add(pnlFooter);
            Controls.Add(flowKpis);
            Controls.Add(pnlHeader);
            Font = new Font("Tahoma", 12F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "frmReportsDashboard";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "لوحة التقارير";
            Load += frmReportsDashboard_Load;
            pnlFooter.ResumeLayout(false);
            lblFooterHint.ResumeLayout(false);
            pnlPreview.ResumeLayout(false);
            pnlReports.ResumeLayout(false);
            tblBody.ResumeLayout(false);
            cardReceivables.ResumeLayout(false);
            cardReceivables.PerformLayout();
            cardStock.ResumeLayout(false);
            cardStock.PerformLayout();
            cardPurchases.ResumeLayout(false);
            cardPurchases.PerformLayout();
            cardSales.ResumeLayout(false);
            cardSales.PerformLayout();
            flowKpis.ResumeLayout(false);
            pnlHeader.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel pnlHeader;
        private Label lblMode;
        private FlowLayoutPanel flowKpis;
        private Panel cardSales;
        private Label cardSalesAccent;
        private Label cardSalesTitle;
        private Label cardSalesValue;
        private Label cardSalesSubtitle;
        private Panel cardPurchases;
        private Label cardPurchasesAccent;
        private Label cardPurchasesTitle;
        private Label cardPurchasesValue;
        private Label cardPurchasesSubtitle;
        private Panel cardStock;
        private Label cardStockAccent;
        private Label cardStockTitle;
        private Label cardStockValue;
        private Label cardStockSubtitle;
        private Panel cardReceivables;
        private Label cardReceivablesAccent;
        private Label cardReceivablesTitle;
        private Label cardReceivablesValue;
        private Label cardReceivablesSubtitle;
        private TableLayoutPanel tblBody;
        private Panel pnlReports;
        private Label lblReportsTitle;
        private TreeView tvReports;
        private Panel pnlPreview;
        private Label lblPreviewTitle;
        private Label lblPreviewPlaceholder;
        private Panel pnlFooter;
        private Button btnClose;
        private Label lblFooterHint;
    }
}