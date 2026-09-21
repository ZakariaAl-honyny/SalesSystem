namespace SalesSystem.Desktop.Screens.Sales.Forms
{
    partial class frmReceiptPrint
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
            pnlHeader = new Panel();
            lblTitle = new Label();
            pnlFooter = new Panel();
            flpFooter = new FlowLayoutPanel();
            btnPrint = new Button();
            btnClose = new Button();
            pnlBody = new Panel();
            tblCenter = new TableLayoutPanel();
            pnlReceipt = new Panel();
            lblStoreName = new Label();
            lblStoreInfo = new Label();
            lblReceiptTitle = new Label();
            tblInfo = new TableLayoutPanel();
            lblInvoiceNoCaption = new Label();
            lblInvoiceNoValue = new Label();
            lblDateCaption = new Label();
            lblDateValue = new Label();
            lblCustomerCaption = new Label();
            lblCustomerValue = new Label();
            dgvItems = new DataGridView();
            colItem = new DataGridViewTextBoxColumn();
            colQty = new DataGridViewTextBoxColumn();
            colPrice = new DataGridViewTextBoxColumn();
            colTotal = new DataGridViewTextBoxColumn();
            tblSummary = new TableLayoutPanel();
            lblSubTotalCaption = new Label();
            lblSubTotalValue = new Label();
            lblDiscountCaption = new Label();
            lblDiscountValue = new Label();
            lblTaxCaption = new Label();
            lblTaxValue = new Label();
            lblNetTotalCaption = new Label();
            lblNetTotalValue = new Label();
            lblPaidCaption = new Label();
            lblPaidValue = new Label();
            lblRemainingCaption = new Label();
            lblRemainingValue = new Label();
            pnlDashedSeparator = new Panel();
            lblThanks = new Label();
            pnlHeader.SuspendLayout();
            pnlFooter.SuspendLayout();
            flpFooter.SuspendLayout();
            pnlBody.SuspendLayout();
            tblCenter.SuspendLayout();
            pnlReceipt.SuspendLayout();
            tblInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvItems).BeginInit();
            tblSummary.SuspendLayout();
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
            lblTitle.Text = "طباعة وصل مبيعات";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
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
            flpFooter.Controls.Add(btnPrint);
            flpFooter.Controls.Add(btnClose);
            flpFooter.Dock = DockStyle.Fill;
            flpFooter.FlowDirection = FlowDirection.RightToLeft;
            flpFooter.Padding = new Padding(6, 10, 6, 8);
            // 
            // btnPrint
            // 
            btnPrint.BackColor = Color.FromArgb(25, 135, 84);
            btnPrint.FlatStyle = FlatStyle.Popup;
            btnPrint.Font = new Font("Tahoma", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnPrint.ForeColor = Color.White;
            btnPrint.Height = 42;
            btnPrint.Margin = new Padding(0, 0, 6, 0);
            btnPrint.Text = "طباعة";
            btnPrint.Width = 130;
            btnPrint.Click += btnPrint_Click;
            // 
            // btnClose
            // 
            btnClose.BackColor = Color.White;
            btnClose.DialogResult = DialogResult.Cancel;
            btnClose.FlatStyle = FlatStyle.Popup;
            btnClose.Font = new Font("Tahoma", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnClose.ForeColor = Color.FromArgb(33, 43, 54);
            btnClose.Height = 42;
            btnClose.Margin = new Padding(0, 0, 6, 0);
            btnClose.Text = "إغلاق";
            btnClose.Width = 100;
            btnClose.Click += btnClose_Click;
            // 
            // pnlBody
            // 
            pnlBody.BackColor = Color.FromArgb(240, 242, 245);
            pnlBody.Controls.Add(tblCenter);
            pnlBody.Dock = DockStyle.Fill;
            // 
            // tblCenter
            // 
            tblCenter.ColumnCount = 3;
            tblCenter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tblCenter.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            tblCenter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tblCenter.Controls.Add(pnlReceipt, 1, 1);
            tblCenter.Dock = DockStyle.Fill;
            tblCenter.RightToLeft = RightToLeft.Yes;
            tblCenter.RowCount = 3;
            tblCenter.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tblCenter.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tblCenter.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            // 
            // pnlReceipt
            // 
            pnlReceipt.BackColor = Color.White;
            pnlReceipt.BorderStyle = BorderStyle.FixedSingle;
            pnlReceipt.Controls.Add(lblThanks);
            pnlReceipt.Controls.Add(pnlDashedSeparator);
            pnlReceipt.Controls.Add(tblSummary);
            pnlReceipt.Controls.Add(dgvItems);
            pnlReceipt.Controls.Add(tblInfo);
            pnlReceipt.Controls.Add(lblReceiptTitle);
            pnlReceipt.Controls.Add(lblStoreInfo);
            pnlReceipt.Controls.Add(lblStoreName);
            pnlReceipt.Padding = new Padding(8, 8, 8, 8);
            pnlReceipt.RightToLeft = RightToLeft.Yes;
            pnlReceipt.Size = new Size(306, 428);
            // 
            // lblStoreName
            // 
            lblStoreName.Dock = DockStyle.Top;
            lblStoreName.Font = new Font("Tahoma", 12.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblStoreName.ForeColor = Color.FromArgb(33, 43, 54);
            lblStoreName.Height = 26;
            lblStoreName.Text = "مؤسسة النور للتجارة العامة";
            lblStoreName.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblStoreInfo
            // 
            lblStoreInfo.Dock = DockStyle.Top;
            lblStoreInfo.Font = new Font("Tahoma", 7.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblStoreInfo.ForeColor = Color.FromArgb(110, 118, 126);
            lblStoreInfo.Height = 16;
            lblStoreInfo.Text = "صنعاء — شارع الزبيري | هاتف: 01-234567";
            lblStoreInfo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblReceiptTitle
            // 
            lblReceiptTitle.Dock = DockStyle.Top;
            lblReceiptTitle.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblReceiptTitle.ForeColor = Color.FromArgb(25, 135, 84);
            lblReceiptTitle.Height = 26;
            lblReceiptTitle.Text = "وصل بيع";
            lblReceiptTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tblInfo
            // 
            tblInfo.ColumnCount = 2;
            tblInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tblInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tblInfo.Controls.Add(lblInvoiceNoCaption, 0, 0);
            tblInfo.Controls.Add(lblInvoiceNoValue, 1, 0);
            tblInfo.Controls.Add(lblDateCaption, 0, 1);
            tblInfo.Controls.Add(lblDateValue, 1, 1);
            tblInfo.Controls.Add(lblCustomerCaption, 0, 2);
            tblInfo.Controls.Add(lblCustomerValue, 1, 2);
            tblInfo.Dock = DockStyle.Top;
            tblInfo.Height = 60;
            tblInfo.RightToLeft = RightToLeft.Yes;
            tblInfo.RowCount = 3;
            tblInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tblInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tblInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            // 
            // lblInvoiceNoCaption
            // 
            lblInvoiceNoCaption.Dock = DockStyle.Fill;
            lblInvoiceNoCaption.Font = new Font("Tahoma", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblInvoiceNoCaption.ForeColor = Color.FromArgb(110, 118, 126);
            lblInvoiceNoCaption.Text = "رقم الفاتورة";
            lblInvoiceNoCaption.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblInvoiceNoValue
            // 
            lblInvoiceNoValue.Dock = DockStyle.Fill;
            lblInvoiceNoValue.Font = new Font("Tahoma", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblInvoiceNoValue.ForeColor = Color.FromArgb(33, 43, 54);
            lblInvoiceNoValue.Text = "1004";
            lblInvoiceNoValue.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblDateCaption
            // 
            lblDateCaption.Dock = DockStyle.Fill;
            lblDateCaption.Font = new Font("Tahoma", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblDateCaption.ForeColor = Color.FromArgb(110, 118, 126);
            lblDateCaption.Text = "التاريخ";
            lblDateCaption.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblDateValue
            // 
            lblDateValue.Dock = DockStyle.Fill;
            lblDateValue.Font = new Font("Tahoma", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDateValue.ForeColor = Color.FromArgb(33, 43, 54);
            lblDateValue.Text = "21/09/2026";
            lblDateValue.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblCustomerCaption
            // 
            lblCustomerCaption.Dock = DockStyle.Fill;
            lblCustomerCaption.Font = new Font("Tahoma", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblCustomerCaption.ForeColor = Color.FromArgb(110, 118, 126);
            lblCustomerCaption.Text = "العميل";
            lblCustomerCaption.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblCustomerValue
            // 
            lblCustomerValue.Dock = DockStyle.Fill;
            lblCustomerValue.Font = new Font("Tahoma", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCustomerValue.ForeColor = Color.FromArgb(33, 43, 54);
            lblCustomerValue.Text = "عميل نقدي";
            lblCustomerValue.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // dgvItems
            // 
            dgvItems.AllowUserToAddRows = false;
            dgvItems.AllowUserToDeleteRows = false;
            dgvItems.BackgroundColor = Color.White;
            dgvItems.BorderStyle = BorderStyle.None;
            dgvItems.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvItems.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvItems.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                BackColor = Color.FromArgb(33, 43, 54),
                Font = new Font("Tahoma", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0),
                ForeColor = Color.White,
                SelectionBackColor = Color.FromArgb(33, 43, 54),
                SelectionForeColor = Color.White,
                WrapMode = DataGridViewTriState.False
            };
            dgvItems.ColumnHeadersHeight = 26;
            dgvItems.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvItems.Columns.AddRange(new DataGridViewColumn[] { colItem, colQty, colPrice, colTotal });
            dgvItems.DefaultCellStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                BackColor = Color.White,
                Font = new Font("Tahoma", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0),
                ForeColor = Color.FromArgb(33, 43, 54),
                SelectionBackColor = Color.FromArgb(214, 232, 252),
                SelectionForeColor = Color.FromArgb(33, 43, 54),
                WrapMode = DataGridViewTriState.False
            };
            dgvItems.Dock = DockStyle.Top;
            dgvItems.EnableHeadersVisualStyles = false;
            dgvItems.GridColor = Color.FromArgb(224, 228, 233);
            dgvItems.Height = 104;
            dgvItems.MultiSelect = false;
            dgvItems.Name = "dgvItems";
            dgvItems.ReadOnly = true;
            dgvItems.RightToLeft = RightToLeft.Yes;
            dgvItems.RowHeadersVisible = false;
            dgvItems.RowTemplate.Height = 26;
            dgvItems.ScrollBars = ScrollBars.None;
            dgvItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvItems.StandardTab = true;
            // 
            // colItem
            // 
            colItem.DefaultCellStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleRight
            };
            colItem.HeaderText = "الصنف";
            colItem.Name = "colItem";
            colItem.Width = 114;
            // 
            // colQty
            // 
            colQty.HeaderText = "كمية";
            colQty.Name = "colQty";
            colQty.Width = 42;
            // 
            // colPrice
            // 
            colPrice.HeaderText = "السعر";
            colPrice.Name = "colPrice";
            colPrice.Width = 62;
            // 
            // colTotal
            // 
            colTotal.HeaderText = "الإجمالي";
            colTotal.Name = "colTotal";
            colTotal.Width = 70;
            // 
            // tblSummary
            // 
            tblSummary.ColumnCount = 2;
            tblSummary.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tblSummary.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tblSummary.Controls.Add(lblSubTotalCaption, 0, 0);
            tblSummary.Controls.Add(lblSubTotalValue, 1, 0);
            tblSummary.Controls.Add(lblDiscountCaption, 0, 1);
            tblSummary.Controls.Add(lblDiscountValue, 1, 1);
            tblSummary.Controls.Add(lblTaxCaption, 0, 2);
            tblSummary.Controls.Add(lblTaxValue, 1, 2);
            tblSummary.Controls.Add(lblNetTotalCaption, 0, 3);
            tblSummary.Controls.Add(lblNetTotalValue, 1, 3);
            tblSummary.Controls.Add(lblPaidCaption, 0, 4);
            tblSummary.Controls.Add(lblPaidValue, 1, 4);
            tblSummary.Controls.Add(lblRemainingCaption, 0, 5);
            tblSummary.Controls.Add(lblRemainingValue, 1, 5);
            tblSummary.Dock = DockStyle.Top;
            tblSummary.Height = 132;
            tblSummary.RightToLeft = RightToLeft.Yes;
            tblSummary.RowCount = 6;
            tblSummary.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            tblSummary.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            tblSummary.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            tblSummary.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            tblSummary.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            tblSummary.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            // 
            // lblSubTotalCaption
            // 
            lblSubTotalCaption.Dock = DockStyle.Fill;
            lblSubTotalCaption.Font = new Font("Tahoma", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblSubTotalCaption.ForeColor = Color.FromArgb(110, 118, 126);
            lblSubTotalCaption.Text = "المجموع";
            lblSubTotalCaption.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblSubTotalValue
            // 
            lblSubTotalValue.Dock = DockStyle.Fill;
            lblSubTotalValue.Font = new Font("Tahoma", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSubTotalValue.ForeColor = Color.FromArgb(33, 43, 54);
            lblSubTotalValue.Text = "15,300 ريال";
            lblSubTotalValue.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblDiscountCaption
            // 
            lblDiscountCaption.Dock = DockStyle.Fill;
            lblDiscountCaption.Font = new Font("Tahoma", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblDiscountCaption.ForeColor = Color.FromArgb(110, 118, 126);
            lblDiscountCaption.Text = "الخصم";
            lblDiscountCaption.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblDiscountValue
            // 
            lblDiscountValue.Dock = DockStyle.Fill;
            lblDiscountValue.Font = new Font("Tahoma", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDiscountValue.ForeColor = Color.FromArgb(33, 43, 54);
            lblDiscountValue.Text = "500 ريال";
            lblDiscountValue.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTaxCaption
            // 
            lblTaxCaption.Dock = DockStyle.Fill;
            lblTaxCaption.Font = new Font("Tahoma", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTaxCaption.ForeColor = Color.FromArgb(110, 118, 126);
            lblTaxCaption.Text = "الضريبة";
            lblTaxCaption.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblTaxValue
            // 
            lblTaxValue.Dock = DockStyle.Fill;
            lblTaxValue.Font = new Font("Tahoma", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTaxValue.ForeColor = Color.FromArgb(33, 43, 54);
            lblTaxValue.Text = "200 ريال";
            lblTaxValue.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblNetTotalCaption
            // 
            lblNetTotalCaption.BackColor = Color.FromArgb(232, 244, 253);
            lblNetTotalCaption.Dock = DockStyle.Fill;
            lblNetTotalCaption.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNetTotalCaption.ForeColor = Color.FromArgb(33, 43, 54);
            lblNetTotalCaption.Text = "الإجمالي";
            lblNetTotalCaption.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblNetTotalValue
            // 
            lblNetTotalValue.BackColor = Color.FromArgb(232, 244, 253);
            lblNetTotalValue.Dock = DockStyle.Fill;
            lblNetTotalValue.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNetTotalValue.ForeColor = Color.FromArgb(25, 135, 84);
            lblNetTotalValue.Text = "15,000 ريال";
            lblNetTotalValue.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblPaidCaption
            // 
            lblPaidCaption.Dock = DockStyle.Fill;
            lblPaidCaption.Font = new Font("Tahoma", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblPaidCaption.ForeColor = Color.FromArgb(110, 118, 126);
            lblPaidCaption.Text = "المدفوع";
            lblPaidCaption.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblPaidValue
            // 
            lblPaidValue.Dock = DockStyle.Fill;
            lblPaidValue.Font = new Font("Tahoma", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPaidValue.ForeColor = Color.FromArgb(33, 43, 54);
            lblPaidValue.Text = "15,000 ريال";
            lblPaidValue.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblRemainingCaption
            // 
            lblRemainingCaption.Dock = DockStyle.Fill;
            lblRemainingCaption.Font = new Font("Tahoma", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblRemainingCaption.ForeColor = Color.FromArgb(110, 118, 126);
            lblRemainingCaption.Text = "الباقي";
            lblRemainingCaption.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblRemainingValue
            // 
            lblRemainingValue.Dock = DockStyle.Fill;
            lblRemainingValue.Font = new Font("Tahoma", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblRemainingValue.ForeColor = Color.FromArgb(33, 43, 54);
            lblRemainingValue.Text = "0 ريال";
            lblRemainingValue.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pnlDashedSeparator
            // 
            pnlDashedSeparator.BackColor = Color.White;
            pnlDashedSeparator.Dock = DockStyle.Top;
            pnlDashedSeparator.Height = 16;
            pnlDashedSeparator.Paint += pnlDashedSeparator_Paint;
            // 
            // lblThanks
            // 
            lblThanks.Dock = DockStyle.Top;
            lblThanks.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblThanks.ForeColor = Color.FromArgb(13, 60, 120);
            lblThanks.Height = 26;
            lblThanks.Text = "شكراً لتعاملكم معنا";
            lblThanks.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // frmReceiptPrint
            // 
            AcceptButton = btnPrint;
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            CancelButton = btnClose;
            ClientSize = new Size(400, 560);
            Controls.Add(pnlBody);
            Controls.Add(pnlFooter);
            Controls.Add(pnlHeader);
            Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmReceiptPrint";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "طباعة وصل مبيعات";
            pnlHeader.ResumeLayout(false);
            pnlFooter.ResumeLayout(false);
            flpFooter.ResumeLayout(false);
            pnlBody.ResumeLayout(false);
            tblCenter.ResumeLayout(false);
            pnlReceipt.ResumeLayout(false);
            tblInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvItems).EndInit();
            tblSummary.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.FlowLayoutPanel flpFooter;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel pnlBody;
        private System.Windows.Forms.TableLayoutPanel tblCenter;
        private System.Windows.Forms.Panel pnlReceipt;
        private System.Windows.Forms.Label lblStoreName;
        private System.Windows.Forms.Label lblStoreInfo;
        private System.Windows.Forms.Label lblReceiptTitle;
        private System.Windows.Forms.TableLayoutPanel tblInfo;
        private System.Windows.Forms.Label lblInvoiceNoCaption;
        private System.Windows.Forms.Label lblInvoiceNoValue;
        private System.Windows.Forms.Label lblDateCaption;
        private System.Windows.Forms.Label lblDateValue;
        private System.Windows.Forms.Label lblCustomerCaption;
        private System.Windows.Forms.Label lblCustomerValue;
        private System.Windows.Forms.DataGridView dgvItems;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTotal;
        private System.Windows.Forms.TableLayoutPanel tblSummary;
        private System.Windows.Forms.Label lblSubTotalCaption;
        private System.Windows.Forms.Label lblSubTotalValue;
        private System.Windows.Forms.Label lblDiscountCaption;
        private System.Windows.Forms.Label lblDiscountValue;
        private System.Windows.Forms.Label lblTaxCaption;
        private System.Windows.Forms.Label lblTaxValue;
        private System.Windows.Forms.Label lblNetTotalCaption;
        private System.Windows.Forms.Label lblNetTotalValue;
        private System.Windows.Forms.Label lblPaidCaption;
        private System.Windows.Forms.Label lblPaidValue;
        private System.Windows.Forms.Label lblRemainingCaption;
        private System.Windows.Forms.Label lblRemainingValue;
        private System.Windows.Forms.Panel pnlDashedSeparator;
        private System.Windows.Forms.Label lblThanks;
    }
}