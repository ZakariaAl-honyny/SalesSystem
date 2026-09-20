namespace SalesSystem.Desktop.Screens.Returns.Forms
{
    partial class frmSalesReturn
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            components?.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblInvoiceNoLabel = new System.Windows.Forms.Label();
            this.lblInvoiceNo = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblCustomer = new System.Windows.Forms.Label();
            this.cmbCustomer = new System.Windows.Forms.ComboBox();
            this.lblWarehouse = new System.Windows.Forms.Label();
            this.cmbWarehouse = new System.Windows.Forms.ComboBox();
            this.lblReference = new System.Windows.Forms.Label();
            this.txtReference = new System.Windows.Forms.TextBox();
            this.pnlTaxDiscountHeader = new System.Windows.Forms.FlowLayoutPanel();
            this.chkTaxInclusive = new System.Windows.Forms.CheckBox();
            this.lblTaxRate = new System.Windows.Forms.Label();
            this.numTaxRate = new System.Windows.Forms.NumericUpDown();
            this.dgvItems = new System.Windows.Forms.DataGridView();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.lblSubTotal = new System.Windows.Forms.Label();
            this.lblSubTotalVal = new System.Windows.Forms.Label();
            this.lblTaxAmount = new System.Windows.Forms.Label();
            this.lblTaxAmountVal = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.lblTotalVal = new System.Windows.Forms.Label();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnAddItem = new System.Windows.Forms.Button();
            this.btnRemoveItem = new System.Windows.Forms.Button();
            this.btnSaveDraft = new System.Windows.Forms.Button();
            this.btnPost = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            
            this.pnlHeader.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.pnlTaxDiscountHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTaxRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).BeginInit();
            this.pnlFooter.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();

            this.pnlHeader.Controls.Add(this.tableLayoutPanel1);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Height = 150;
            this.pnlHeader.Padding = new System.Windows.Forms.Padding(10);

            this.tableLayoutPanel1.ColumnCount = 6;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34F));
            this.tableLayoutPanel1.Controls.Add(this.lblInvoiceNoLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblInvoiceNo, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblDate, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.dtpDate, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblStatus, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblCustomer, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.cmbCustomer, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblWarehouse, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.cmbWarehouse, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblReference, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtReference, 5, 1);
            this.tableLayoutPanel1.Controls.Add(this.pnlTaxDiscountHeader, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.RowCount = 3;

            this.lblInvoiceNoLabel.Text = "��� �������:";
            this.lblInvoiceNo.Text = "����";
            this.lblDate.Text = "�������:";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblStatus.BackColor = System.Drawing.Color.DarkOrange;
            this.lblStatus.ForeColor = System.Drawing.Color.White;
            this.lblStatus.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblCustomer.Text = "������:";
            this.lblWarehouse.Text = "������ ��������:";
            this.lblReference.Text = "������ ������:";

            this.tableLayoutPanel1.SetColumnSpan(this.pnlTaxDiscountHeader, 6);
            this.pnlTaxDiscountHeader.Controls.Add(this.chkTaxInclusive);
            this.pnlTaxDiscountHeader.Controls.Add(this.lblTaxRate);
            this.pnlTaxDiscountHeader.Controls.Add(this.numTaxRate);
            this.pnlTaxDiscountHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTaxDiscountHeader.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;

            this.chkTaxInclusive.Text = "���� �������";
            this.numTaxRate.Value = 15;

            this.dgvItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvItems.BackgroundColor = System.Drawing.Color.White;

            this.pnlFooter.Controls.Add(this.tableLayoutPanel2);
            this.pnlFooter.Controls.Add(this.txtNotes);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Height = 150;
            this.pnlFooter.Padding = new System.Windows.Forms.Padding(10);

            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.lblSubTotal, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblSubTotalVal, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblTaxAmount, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.lblTaxAmountVal, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.lblTotal, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.lblTotalVal, 1, 2);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.tableLayoutPanel2.Width = 300;

            this.lblSubTotal.Text = "�������� ������:";
            this.lblTaxAmount.Text = "���� �������:";
            this.lblTotal.Text = "������ �������:";
            this.lblTotal.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);

            this.txtNotes.Dock = System.Windows.Forms.DockStyle.Left;
            this.txtNotes.Width = 400;
            this.txtNotes.Multiline = true;
            this.txtNotes.PlaceholderText = "������� �������...";

            this.pnlButtons.Controls.Add(this.btnClose);
            this.pnlButtons.Controls.Add(this.btnPost);
            this.pnlButtons.Controls.Add(this.btnSaveDraft);
            this.pnlButtons.Controls.Add(this.btnRemoveItem);
            this.pnlButtons.Controls.Add(this.btnAddItem);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Height = 60;

            this.btnAddItem.Text = "\u0625\u0636\u0627\u0641\u0629 \u0635\u0646\u0641";
            this.btnAddItem.BackColor = System.Drawing.Color.White;
            this.btnAddItem.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnAddItem.FlatAppearance.BorderSize = 2;
            this.btnAddItem.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAddItem.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemoveItem.Text = "\u062d\u0630\u0641 \u0635\u0646\u0641";
            this.btnRemoveItem.BackColor = System.Drawing.Color.White;
            this.btnRemoveItem.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnRemoveItem.FlatAppearance.BorderSize = 2;
            this.btnRemoveItem.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnRemoveItem.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveDraft.Text = "\u062d\u0641\u0638 \u0645\u0633\u0648\u062f\u0629";
            this.btnSaveDraft.BackColor = System.Drawing.Color.White;
            this.btnSaveDraft.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnSaveDraft.FlatAppearance.BorderSize = 2;
            this.btnSaveDraft.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSaveDraft.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPost.Text = "\u062a\u0631\u062d\u064a\u0644";
            this.btnPost.BackColor = System.Drawing.Color.White;
            this.btnPost.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnPost.FlatAppearance.BorderSize = 2;
            this.btnPost.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnPost.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Text = "\u0625\u063a\u0644\u0627\u0642";
            this.btnClose.BackColor = System.Drawing.Color.White;
            this.btnClose.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnClose.FlatAppearance.BorderSize = 2;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClose.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            this.btnAddItem.Dock = System.Windows.Forms.DockStyle.Right; this.btnAddItem.Width = 120;
            this.btnRemoveItem.Dock = System.Windows.Forms.DockStyle.Right; this.btnRemoveItem.Width = 120;
            this.btnSaveDraft.Dock = System.Windows.Forms.DockStyle.Left; this.btnSaveDraft.Width = 120;
            this.btnPost.Dock = System.Windows.Forms.DockStyle.Left; this.btnPost.Width = 120;
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Left; this.btnClose.Width = 80;

            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1100, 750);
            this.Controls.Add(this.dgvItems);
            this.Controls.Add(this.pnlButtons);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.pnlHeader);
            this.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            this.btnAddItem.Click += new System.EventHandler(this.btnAddItem_Click);
            this.btnRemoveItem.Click += new System.EventHandler(this.btnRemoveItem_Click);
            this.btnSaveDraft.Click += new System.EventHandler(this.btnSaveDraft_Click);
            this.btnPost.Click += new System.EventHandler(this.btnPost_Click);
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            this.chkTaxInclusive.CheckedChanged += (s,e) => CalculateTotals();
            this.numTaxRate.ValueChanged += (s,e) => CalculateTotals();

            this.pnlHeader.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.pnlTaxDiscountHeader.ResumeLayout(false);
            this.pnlTaxDiscountHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTaxRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).EndInit();
            this.pnlFooter.ResumeLayout(false);
            this.pnlFooter.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlHeader, pnlFooter, pnlButtons;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1, tableLayoutPanel2;
        private System.Windows.Forms.FlowLayoutPanel pnlTaxDiscountHeader;
        private System.Windows.Forms.Label lblInvoiceNoLabel, lblInvoiceNo, lblDate, lblStatus, lblCustomer, lblWarehouse, lblReference, lblSubTotal, lblSubTotalVal, lblTaxAmount, lblTaxAmountVal, lblTotal, lblTotalVal, lblTaxRate;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.ComboBox cmbCustomer, cmbWarehouse;
        private System.Windows.Forms.TextBox txtReference, txtNotes;
        private System.Windows.Forms.DataGridView dgvItems;
        private System.Windows.Forms.CheckBox chkTaxInclusive;
        private System.Windows.Forms.NumericUpDown numTaxRate;
        private System.Windows.Forms.Button btnAddItem, btnRemoveItem, btnSaveDraft, btnPost, btnClose;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}
