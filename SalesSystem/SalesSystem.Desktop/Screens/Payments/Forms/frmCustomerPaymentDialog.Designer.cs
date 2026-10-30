namespace SalesSystem.Desktop.Screens.Payments.Forms
{
    partial class frmCustomerPaymentDialog
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            components?.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            tableLayoutPanel1 = new TableLayoutPanel();
            lblCustomer = new Label();
            cmbCustomer = new ComboBox();
            lblBalance = new Label();
            lblBalanceVal = new Label();
            lblInvoice = new Label();
            cmbInvoice = new ComboBox();
            lblAmount = new Label();
            numAmount = new NumericUpDown();
            lblDate = new Label();
            dtpDate = new DateTimePicker();
            lblNotes = new Label();
            txtNotes = new TextBox();
            pnlButtons = new FlowLayoutPanel();
            btnCancel = new Button();
            btnSave = new Button();
            errorProvider = new ErrorProvider(components);
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numAmount).BeginInit();
            pnlButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(lblCustomer, 0, 0);
            tableLayoutPanel1.Controls.Add(cmbCustomer, 1, 0);
            tableLayoutPanel1.Controls.Add(lblBalance, 0, 1);
            tableLayoutPanel1.Controls.Add(lblBalanceVal, 1, 1);
            tableLayoutPanel1.Controls.Add(lblInvoice, 0, 2);
            tableLayoutPanel1.Controls.Add(cmbInvoice, 1, 2);
            tableLayoutPanel1.Controls.Add(lblAmount, 0, 3);
            tableLayoutPanel1.Controls.Add(numAmount, 1, 3);
            tableLayoutPanel1.Controls.Add(lblDate, 0, 4);
            tableLayoutPanel1.Controls.Add(dtpDate, 1, 4);
            tableLayoutPanel1.Controls.Add(lblNotes, 0, 5);
            tableLayoutPanel1.Controls.Add(txtNotes, 1, 5);
            tableLayoutPanel1.Dock = DockStyle.Top;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(4);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.Padding = new Padding(12);
            tableLayoutPanel1.RowCount = 6;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new Size(625, 438);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // lblCustomer
            // 
            lblCustomer.Location = new Point(484, 12);
            lblCustomer.Margin = new Padding(4, 0, 4, 0);
            lblCustomer.Name = "lblCustomer";
            lblCustomer.Size = new Size(125, 20);
            lblCustomer.TabIndex = 0;
            lblCustomer.Text = "العميل:";
            // 
            // cmbCustomer
            // 
            cmbCustomer.Dock = DockStyle.Fill;
            cmbCustomer.Location = new Point(16, 16);
            cmbCustomer.Margin = new Padding(4);
            cmbCustomer.Name = "cmbCustomer";
            cmbCustomer.Size = new Size(443, 37);
            cmbCustomer.TabIndex = 1;
            // 
            // lblBalance
            // 
            lblBalance.Location = new Point(484, 32);
            lblBalance.Margin = new Padding(4, 0, 4, 0);
            lblBalance.Name = "lblBalance";
            lblBalance.Size = new Size(125, 20);
            lblBalance.TabIndex = 2;
            lblBalance.Text = "الرصيد الحالي:";
            // 
            // lblBalanceVal
            // 
            lblBalanceVal.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblBalanceVal.ForeColor = Color.Blue;
            lblBalanceVal.Location = new Point(391, 32);
            lblBalanceVal.Margin = new Padding(4, 0, 4, 0);
            lblBalanceVal.Name = "lblBalanceVal";
            lblBalanceVal.Size = new Size(68, 20);
            lblBalanceVal.TabIndex = 3;
            lblBalanceVal.Text = "0.00";
            // 
            // lblInvoice
            // 
            lblInvoice.Location = new Point(484, 52);
            lblInvoice.Margin = new Padding(4, 0, 4, 0);
            lblInvoice.Name = "lblInvoice";
            lblInvoice.Size = new Size(125, 20);
            lblInvoice.TabIndex = 4;
            lblInvoice.Text = "رقم الفاتورة:";
            // 
            // cmbInvoice
            // 
            cmbInvoice.Dock = DockStyle.Fill;
            cmbInvoice.Location = new Point(16, 56);
            cmbInvoice.Margin = new Padding(4);
            cmbInvoice.Name = "cmbInvoice";
            cmbInvoice.Size = new Size(443, 37);
            cmbInvoice.TabIndex = 5;
            // 
            // lblAmount
            // 
            lblAmount.Location = new Point(484, 72);
            lblAmount.Margin = new Padding(4, 0, 4, 0);
            lblAmount.Name = "lblAmount";
            lblAmount.Size = new Size(125, 20);
            lblAmount.TabIndex = 6;
            lblAmount.Text = "المبلغ المدفوع:";
            // 
            // numAmount
            // 
            numAmount.DecimalPlaces = 2;
            numAmount.Dock = DockStyle.Fill;
            numAmount.Location = new Point(16, 76);
            numAmount.Margin = new Padding(4);
            numAmount.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
            numAmount.Name = "numAmount";
            numAmount.Size = new Size(443, 36);
            numAmount.TabIndex = 7;
            // 
            // lblDate
            // 
            lblDate.Location = new Point(484, 92);
            lblDate.Margin = new Padding(4, 0, 4, 0);
            lblDate.Name = "lblDate";
            lblDate.Size = new Size(125, 20);
            lblDate.TabIndex = 8;
            lblDate.Text = "التاريخ:";
            // 
            // dtpDate
            // 
            dtpDate.Dock = DockStyle.Fill;
            dtpDate.Location = new Point(16, 96);
            dtpDate.Margin = new Padding(4);
            dtpDate.Name = "dtpDate";
            dtpDate.Size = new Size(443, 36);
            dtpDate.TabIndex = 9;
            // 
            // lblNotes
            // 
            lblNotes.Location = new Point(484, 112);
            lblNotes.Margin = new Padding(4, 0, 4, 0);
            lblNotes.Name = "lblNotes";
            lblNotes.Size = new Size(125, 29);
            lblNotes.TabIndex = 10;
            lblNotes.Text = "ملاحظات:";
            // 
            // txtNotes
            // 
            txtNotes.Dock = DockStyle.Fill;
            txtNotes.Location = new Point(16, 116);
            txtNotes.Margin = new Padding(4);
            txtNotes.Multiline = true;
            txtNotes.Name = "txtNotes";
            txtNotes.Size = new Size(443, 306);
            txtNotes.TabIndex = 11;
            // 
            // pnlButtons
            // 
            pnlButtons.Controls.Add(btnCancel);
            pnlButtons.Controls.Add(btnSave);
            pnlButtons.Dock = DockStyle.Bottom;
            pnlButtons.FlowDirection = FlowDirection.RightToLeft;
            pnlButtons.Location = new Point(0, 463);
            pnlButtons.Margin = new Padding(4);
            pnlButtons.Name = "pnlButtons";
            pnlButtons.Padding = new Padding(12, 0, 12, 0);
            pnlButtons.Size = new Size(625, 62);
            pnlButtons.TabIndex = 0;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.White;
            btnCancel.FlatAppearance.BorderColor = Color.Black;
            btnCancel.FlatAppearance.BorderSize = 2;
            btnCancel.FlatStyle = FlatStyle.Popup;
            btnCancel.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCancel.Location = new Point(16, 4);
            btnCancel.Margin = new Padding(4);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(100, 44);
            btnCancel.TabIndex = 0;
            btnCancel.Text = "إلغاء";
            btnCancel.UseVisualStyleBackColor = false;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.White;
            btnSave.FlatAppearance.BorderColor = Color.Black;
            btnSave.FlatAppearance.BorderSize = 2;
            btnSave.FlatStyle = FlatStyle.Popup;
            btnSave.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSave.Location = new Point(124, 4);
            btnSave.Margin = new Padding(4);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(125, 44);
            btnSave.TabIndex = 1;
            btnSave.Text = "حفظ السند";
            btnSave.UseVisualStyleBackColor = false;
            // 
            // errorProvider
            // 
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errorProvider.ContainerControl = this;
            // 
            // frmCustomerPaymentDialog
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(625, 525);
            Controls.Add(pnlButtons);
            Controls.Add(tableLayoutPanel1);
            Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmCustomerPaymentDialog";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "سند قبض جديد";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numAmount).EndInit();
            pnlButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
        }

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblCustomer, lblBalance, lblBalanceVal, lblInvoice, lblAmount, lblDate, lblNotes;
        private System.Windows.Forms.ComboBox cmbCustomer, cmbInvoice;
        private System.Windows.Forms.NumericUpDown numAmount;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.FlowLayoutPanel pnlButtons;
        private System.Windows.Forms.Button btnSave, btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}
