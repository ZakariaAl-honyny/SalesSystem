namespace SalesSystem.Desktop.Screens.Sales.Forms
{
    partial class frmSalesProfitBreakdown
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            pnlHeader = new Panel();
            lblTitle = new Label();
            pnlBody = new Panel();
            dgvProfit = new DataGridView();
            colItem = new DataGridViewTextBoxColumn();
            colQty = new DataGridViewTextBoxColumn();
            colCost = new DataGridViewTextBoxColumn();
            colPrice = new DataGridViewTextBoxColumn();
            colProfit = new DataGridViewTextBoxColumn();
            colMargin = new DataGridViewTextBoxColumn();
            flpCards = new FlowLayoutPanel();
            flpCustomer = new FlowLayoutPanel();
            lblCustomerCaption = new Label();
            lblCustomerValue = new Label();
            lblPaymentCaption = new Label();
            lblPaymentValue = new Label();
            pnlFooter = new Panel();
            flpFooter = new FlowLayoutPanel();
            lblTotalSalesCaption = new Label();
            lblTotalSalesVal = new Label();
            lblTotalCostCaption = new Label();
            lblTotalCostVal = new Label();
            lblTotalProfitCaption = new Label();
            lblTotalProfitVal = new Label();
            btnClose = new Button();
            tip = new ToolTip(components);
            pnlHeader.SuspendLayout();
            pnlBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvProfit).BeginInit();
            pnlFooter.SuspendLayout();
            flpFooter.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(33, 43, 54);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 56;
            // 
            // lblTitle
            // 
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.Font = new Font("Tahoma", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.ForeColor = Color.White;
            lblTitle.Text = "توزيع الأرباح";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlBody
            // 
            pnlBody.BackColor = Color.WhiteSmoke;
            pnlBody.Controls.Add(dgvProfit);
            pnlBody.Controls.Add(flpCards);
            pnlBody.Controls.Add(flpCustomer);
            pnlBody.Dock = DockStyle.Fill;
            pnlBody.Padding = new Padding(12, 10, 12, 10);
            // 
            // dgvProfit
            // 
            dgvProfit.AllowUserToAddRows = false;
            dgvProfit.AllowUserToDeleteRows = false;
            dgvProfit.AllowUserToResizeRows = false;
            dgvProfit.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(248, 249, 250)
            };
            dgvProfit.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProfit.BackgroundColor = Color.White;
            dgvProfit.BorderStyle = BorderStyle.None;
            dgvProfit.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvProfit.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvProfit.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                BackColor = Color.FromArgb(52, 73, 94),
                Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0),
                ForeColor = Color.White,
                Padding = new Padding(0, 4, 0, 4),
                SelectionBackColor = Color.FromArgb(52, 73, 94),
                SelectionForeColor = Color.White,
                WrapMode = DataGridViewTriState.True
            };
            dgvProfit.ColumnHeadersHeight = 45;
            dgvProfit.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvProfit.Columns.AddRange(new DataGridViewColumn[] { colItem, colQty, colCost, colPrice, colProfit, colMargin });
            dgvProfit.DefaultCellStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                BackColor = Color.White,
                Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0),
                ForeColor = Color.FromArgb(33, 37, 41),
                SelectionBackColor = Color.FromArgb(232, 244, 253),
                SelectionForeColor = Color.FromArgb(33, 37, 41)
            };
            dgvProfit.Dock = DockStyle.Fill;
            dgvProfit.EnableHeadersVisualStyles = false;
            dgvProfit.GridColor = Color.FromArgb(222, 226, 230);
            dgvProfit.MultiSelect = false;
            dgvProfit.Name = "dgvProfit";
            dgvProfit.ReadOnly = true;
            dgvProfit.RowHeadersVisible = false;
            dgvProfit.RowTemplate.Height = 34;
            dgvProfit.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProfit.TabStop = false;
            dgvProfit.CellFormatting += dgvProfit_CellFormatting;
            // 
            // colItem
            // 
            colItem.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colItem.DataPropertyName = "Item";
            colItem.FillWeight = 34F;
            colItem.HeaderText = "الصنف / الوحدة";
            colItem.MinimumWidth = 190;
            colItem.Name = "colItem";
            colItem.ReadOnly = true;
            // 
            // colQty
            // 
            colQty.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colQty.DataPropertyName = "Qty";
            colQty.DefaultCellStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                Format = "0.###"
            };
            colQty.FillWeight = 11F;
            colQty.HeaderText = "الكمية";
            colQty.MinimumWidth = 55;
            colQty.Name = "colQty";
            colQty.ReadOnly = true;
            // 
            // colCost
            // 
            colCost.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colCost.DataPropertyName = "Cost";
            colCost.DefaultCellStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                Format = "N2"
            };
            colCost.FillWeight = 14F;
            colCost.HeaderText = "التكلفة";
            colCost.MinimumWidth = 75;
            colCost.Name = "colCost";
            colCost.ReadOnly = true;
            // 
            // colPrice
            // 
            colPrice.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colPrice.DataPropertyName = "Price";
            colPrice.DefaultCellStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                Format = "N2"
            };
            colPrice.FillWeight = 14F;
            colPrice.HeaderText = "سعر البيع";
            colPrice.MinimumWidth = 75;
            colPrice.Name = "colPrice";
            colPrice.ReadOnly = true;
            // 
            // colProfit
            // 
            colProfit.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colProfit.DataPropertyName = "Profit";
            colProfit.DefaultCellStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0),
                ForeColor = Color.FromArgb(21, 87, 36),
                Format = "N2"
            };
            colProfit.FillWeight = 14F;
            colProfit.HeaderText = "ربح الصنف";
            colProfit.MinimumWidth = 75;
            colProfit.Name = "colProfit";
            colProfit.ReadOnly = true;
            // 
            // colMargin
            // 
            colMargin.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colMargin.DataPropertyName = "Margin";
            colMargin.DefaultCellStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0),
                ForeColor = Color.FromArgb(13, 110, 253)
            };
            colMargin.FillWeight = 13F;
            colMargin.HeaderText = "الهامش %";
            colMargin.MinimumWidth = 70;
            colMargin.Name = "colMargin";
            colMargin.ReadOnly = true;
            // 
            // flpCards
            // 
            flpCards.Dock = DockStyle.Top;
            flpCards.FlowDirection = FlowDirection.RightToLeft;
            flpCards.Height = 100;
            flpCards.Padding = new Padding(0, 4, 0, 0);
            // 
            // flpCustomer
            // 
            flpCustomer.BackColor = Color.White;
            flpCustomer.Controls.Add(lblCustomerCaption);
            flpCustomer.Controls.Add(lblCustomerValue);
            flpCustomer.Controls.Add(lblPaymentCaption);
            flpCustomer.Controls.Add(lblPaymentValue);
            flpCustomer.Dock = DockStyle.Top;
            flpCustomer.FlowDirection = FlowDirection.RightToLeft;
            flpCustomer.Height = 44;
            flpCustomer.Padding = new Padding(8, 0, 8, 0);
            // 
            // lblCustomerCaption
            // 
            lblCustomerCaption.AutoSize = true;
            lblCustomerCaption.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCustomerCaption.ForeColor = Color.FromArgb(100, 110, 120);
            lblCustomerCaption.Margin = new Padding(0, 12, 4, 0);
            lblCustomerCaption.Text = "العميل:";
            // 
            // lblCustomerValue
            // 
            lblCustomerValue.AutoSize = true;
            lblCustomerValue.Font = new Font("Tahoma", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCustomerValue.ForeColor = Color.FromArgb(33, 43, 54);
            lblCustomerValue.Margin = new Padding(14, 12, 4, 0);
            lblCustomerValue.Text = "شركة النور التجارية";
            // 
            // lblPaymentCaption
            // 
            lblPaymentCaption.AutoSize = true;
            lblPaymentCaption.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPaymentCaption.ForeColor = Color.FromArgb(100, 110, 120);
            lblPaymentCaption.Margin = new Padding(14, 12, 4, 0);
            lblPaymentCaption.Text = "نوع الدفع:";
            // 
            // lblPaymentValue
            // 
            lblPaymentValue.AutoSize = true;
            lblPaymentValue.Font = new Font("Tahoma", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPaymentValue.ForeColor = Color.FromArgb(21, 87, 36);
            lblPaymentValue.Margin = new Padding(14, 12, 4, 0);
            lblPaymentValue.Text = "نقدي فوري";
            // 
            // pnlFooter
            // 
            pnlFooter.BackColor = Color.FromArgb(248, 249, 250);
            pnlFooter.Controls.Add(flpFooter);
            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Height = 64;
            // 
            // flpFooter
            // 
            flpFooter.Controls.Add(lblTotalSalesCaption);
            flpFooter.Controls.Add(lblTotalSalesVal);
            flpFooter.Controls.Add(lblTotalCostCaption);
            flpFooter.Controls.Add(lblTotalCostVal);
            flpFooter.Controls.Add(lblTotalProfitCaption);
            flpFooter.Controls.Add(lblTotalProfitVal);
            flpFooter.Controls.Add(btnClose);
            flpFooter.Dock = DockStyle.Fill;
            flpFooter.FlowDirection = FlowDirection.RightToLeft;
            flpFooter.Padding = new Padding(6, 11, 6, 11);
            // 
            // lblTotalSalesCaption
            // 
            lblTotalSalesCaption.Height = 42;
            lblTotalSalesCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalSalesCaption.ForeColor = Color.FromArgb(100, 110, 120);
            lblTotalSalesCaption.Margin = new Padding(0, 0, 4, 0);
            lblTotalSalesCaption.Text = "إجمالي المبيعات:";
            lblTotalSalesCaption.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTotalSalesVal
            // 
            lblTotalSalesVal.Height = 42;
            lblTotalSalesVal.Font = new Font("Tahoma", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalSalesVal.ForeColor = Color.FromArgb(13, 110, 253);
            lblTotalSalesVal.Margin = new Padding(10, 0, 4, 0);
            lblTotalSalesVal.Text = "0.00";
            lblTotalSalesVal.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTotalCostCaption
            // 
            lblTotalCostCaption.Height = 42;
            lblTotalCostCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalCostCaption.ForeColor = Color.FromArgb(100, 110, 120);
            lblTotalCostCaption.Margin = new Padding(10, 0, 4, 0);
            lblTotalCostCaption.Text = "إجمالي التكلفة:";
            lblTotalCostCaption.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTotalCostVal
            // 
            lblTotalCostVal.Height = 42;
            lblTotalCostVal.Font = new Font("Tahoma", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalCostVal.ForeColor = Color.FromArgb(108, 117, 125);
            lblTotalCostVal.Margin = new Padding(10, 0, 4, 0);
            lblTotalCostVal.Text = "0.00";
            lblTotalCostVal.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTotalProfitCaption
            // 
            lblTotalProfitCaption.Height = 42;
            lblTotalProfitCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalProfitCaption.ForeColor = Color.FromArgb(100, 110, 120);
            lblTotalProfitCaption.Margin = new Padding(10, 0, 4, 0);
            lblTotalProfitCaption.Text = "صافي الربح:";
            lblTotalProfitCaption.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTotalProfitVal
            // 
            lblTotalProfitVal.Height = 42;
            lblTotalProfitVal.Font = new Font("Tahoma", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalProfitVal.ForeColor = Color.FromArgb(21, 87, 36);
            lblTotalProfitVal.Margin = new Padding(10, 0, 4, 0);
            lblTotalProfitVal.Text = "0.00";
            lblTotalProfitVal.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnClose
            // 
            btnClose.BackColor = Color.White;
            btnClose.Cursor = Cursors.Hand;
            btnClose.DialogResult = DialogResult.Cancel;
            btnClose.FlatStyle = FlatStyle.Popup;
            btnClose.Font = new Font("Tahoma", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnClose.ForeColor = Color.FromArgb(33, 43, 54);
            btnClose.Height = 42;
            btnClose.Margin = new Padding(16, 0, 6, 0);
            btnClose.Text = "إغلاق";
            btnClose.Width = 100;
            btnClose.Click += btnClose_Click;
            tip.SetToolTip(btnClose, "إغلاق نافذة توزيع الأرباح");
            // 
            // tip
            // 
            tip.AutomaticDelay = 400;
            tip.ToolTipIcon = ToolTipIcon.Info;
            tip.ToolTipTitle = "توزيع الأرباح";
            // 
            // frmSalesProfitBreakdown
            // 
            AcceptButton = btnClose;
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            CancelButton = btnClose;
            ClientSize = new Size(720, 600);
            Controls.Add(pnlBody);
            Controls.Add(pnlFooter);
            Controls.Add(pnlHeader);
            Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmSalesProfitBreakdown";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "توزيع الأرباح";
            pnlHeader.ResumeLayout(false);
            pnlBody.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvProfit).EndInit();
            pnlFooter.ResumeLayout(false);
            flpFooter.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlBody;
        private System.Windows.Forms.DataGridView dgvProfit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCost;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProfit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMargin;
        private System.Windows.Forms.FlowLayoutPanel flpCards;
        private System.Windows.Forms.FlowLayoutPanel flpCustomer;
        private System.Windows.Forms.Label lblCustomerCaption;
        private System.Windows.Forms.Label lblCustomerValue;
        private System.Windows.Forms.Label lblPaymentCaption;
        private System.Windows.Forms.Label lblPaymentValue;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.FlowLayoutPanel flpFooter;
        private System.Windows.Forms.Label lblTotalSalesCaption;
        private System.Windows.Forms.Label lblTotalSalesVal;
        private System.Windows.Forms.Label lblTotalCostCaption;
        private System.Windows.Forms.Label lblTotalCostVal;
        private System.Windows.Forms.Label lblTotalProfitCaption;
        private System.Windows.Forms.Label lblTotalProfitVal;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ToolTip tip;
    }
}