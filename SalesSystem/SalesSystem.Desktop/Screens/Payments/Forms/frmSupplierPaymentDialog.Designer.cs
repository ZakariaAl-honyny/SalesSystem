namespace SalesSystem.Desktop.Screens.Payments.Forms
{
    partial class frmSupplierPaymentDialog
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
            gbPaymentInfo = new GroupBox();
            tlpInfo = new TableLayoutPanel();
            lblSupplier = new Label();
            cmbSupplier = new ComboBox();
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
            pnlFooter = new Panel();
            flpButtons = new FlowLayoutPanel();
            btnSave = new Button();
            btnCancel = new Button();
            toolTip1 = new ToolTip(components);
            liveTip = new ToolTip(components);
            errorProvider = new ErrorProvider(components);
            gbPaymentInfo.SuspendLayout();
            tlpInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numAmount).BeginInit();
            pnlFooter.SuspendLayout();
            flpButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            // 
            // gbPaymentInfo
            // 
            gbPaymentInfo.BackColor = Color.FromArgb(235, 245, 235);
            gbPaymentInfo.Controls.Add(tlpInfo);
            gbPaymentInfo.Dock = DockStyle.Fill;
            gbPaymentInfo.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            gbPaymentInfo.ForeColor = Color.FromArgb(14, 82, 52);
            gbPaymentInfo.Location = new Point(0, 0);
            gbPaymentInfo.Margin = new Padding(0);
            gbPaymentInfo.Name = "gbPaymentInfo";
            gbPaymentInfo.Padding = new Padding(12, 6, 12, 8);
            gbPaymentInfo.Size = new Size(640, 418);
            gbPaymentInfo.TabIndex = 0;
            gbPaymentInfo.TabStop = false;
            gbPaymentInfo.Text = "بيانات السند";
            // 
            // tlpInfo
            // 
            tlpInfo.BackColor = Color.FromArgb(235, 245, 235);
            tlpInfo.ColumnCount = 2;
            tlpInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 170F));
            tlpInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpInfo.Controls.Add(lblSupplier, 0, 0);
            tlpInfo.Controls.Add(cmbSupplier, 1, 0);
            tlpInfo.Controls.Add(lblBalance, 0, 1);
            tlpInfo.Controls.Add(lblBalanceVal, 1, 1);
            tlpInfo.Controls.Add(lblInvoice, 0, 2);
            tlpInfo.Controls.Add(cmbInvoice, 1, 2);
            tlpInfo.Controls.Add(lblAmount, 0, 3);
            tlpInfo.Controls.Add(numAmount, 1, 3);
            tlpInfo.Controls.Add(lblDate, 0, 4);
            tlpInfo.Controls.Add(dtpDate, 1, 4);
            tlpInfo.Controls.Add(lblNotes, 0, 5);
            tlpInfo.Controls.Add(txtNotes, 1, 5);
            tlpInfo.Dock = DockStyle.Fill;
            tlpInfo.Location = new Point(12, 24);
            tlpInfo.Margin = new Padding(0);
            tlpInfo.Name = "tlpInfo";
            tlpInfo.RowCount = 6;
            tlpInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
            tlpInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
            tlpInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
            tlpInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
            tlpInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
            tlpInfo.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpInfo.Size = new Size(616, 386);
            tlpInfo.TabIndex = 0;
            // 
            // lblSupplier
            // 
            lblSupplier.Dock = DockStyle.Fill;
            lblSupplier.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSupplier.ForeColor = Color.FromArgb(14, 82, 52);
            lblSupplier.Location = new Point(4, 0);
            lblSupplier.Margin = new Padding(4, 0, 4, 0);
            lblSupplier.Name = "lblSupplier";
            lblSupplier.Size = new Size(162, 46);
            lblSupplier.TabIndex = 0;
            lblSupplier.Text = "المورد: *";
            lblSupplier.TextAlign = ContentAlignment.MiddleRight;
            // 
            // cmbSupplier
            // 
            cmbSupplier.BackColor = Color.White;
            cmbSupplier.Dock = DockStyle.Fill;
            cmbSupplier.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSupplier.FlatStyle = FlatStyle.Flat;
            cmbSupplier.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmbSupplier.FormattingEnabled = true;
            cmbSupplier.Location = new Point(174, 8);
            cmbSupplier.Margin = new Padding(4, 8, 4, 6);
            cmbSupplier.Name = "cmbSupplier";
            cmbSupplier.Size = new Size(438, 32);
            cmbSupplier.TabIndex = 1;
            cmbSupplier.SelectedIndexChanged += cmbSupplier_SelectedIndexChanged;
            // 
            // lblBalance
            // 
            lblBalance.Dock = DockStyle.Fill;
            lblBalance.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblBalance.ForeColor = Color.FromArgb(14, 82, 52);
            lblBalance.Location = new Point(4, 46);
            lblBalance.Margin = new Padding(4, 0, 4, 0);
            lblBalance.Name = "lblBalance";
            lblBalance.Size = new Size(162, 46);
            lblBalance.TabIndex = 2;
            lblBalance.Text = "الرصيد الحالي:";
            lblBalance.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblBalanceVal
            // 
            lblBalanceVal.Dock = DockStyle.Fill;
            lblBalanceVal.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblBalanceVal.ForeColor = Color.Blue;
            lblBalanceVal.Location = new Point(174, 46);
            lblBalanceVal.Margin = new Padding(4, 0, 4, 0);
            lblBalanceVal.Name = "lblBalanceVal";
            lblBalanceVal.Size = new Size(438, 46);
            lblBalanceVal.TabIndex = 3;
            lblBalanceVal.Text = "0.00";
            lblBalanceVal.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblInvoice
            // 
            lblInvoice.Dock = DockStyle.Fill;
            lblInvoice.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblInvoice.ForeColor = Color.FromArgb(14, 82, 52);
            lblInvoice.Location = new Point(4, 92);
            lblInvoice.Margin = new Padding(4, 0, 4, 0);
            lblInvoice.Name = "lblInvoice";
            lblInvoice.Size = new Size(162, 46);
            lblInvoice.TabIndex = 4;
            lblInvoice.Text = "رقم الفاتورة:";
            lblInvoice.TextAlign = ContentAlignment.MiddleRight;
            // 
            // cmbInvoice
            // 
            cmbInvoice.BackColor = Color.White;
            cmbInvoice.Dock = DockStyle.Fill;
            cmbInvoice.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbInvoice.FlatStyle = FlatStyle.Flat;
            cmbInvoice.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmbInvoice.FormattingEnabled = true;
            cmbInvoice.Location = new Point(174, 100);
            cmbInvoice.Margin = new Padding(4, 8, 4, 6);
            cmbInvoice.Name = "cmbInvoice";
            cmbInvoice.Size = new Size(438, 32);
            cmbInvoice.TabIndex = 5;
            // 
            // lblAmount
            // 
            lblAmount.Dock = DockStyle.Fill;
            lblAmount.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblAmount.ForeColor = Color.FromArgb(14, 82, 52);
            lblAmount.Location = new Point(4, 138);
            lblAmount.Margin = new Padding(4, 0, 4, 0);
            lblAmount.Name = "lblAmount";
            lblAmount.Size = new Size(162, 46);
            lblAmount.TabIndex = 6;
            lblAmount.Text = "المبلغ المدفوع: *";
            lblAmount.TextAlign = ContentAlignment.MiddleRight;
            // 
            // numAmount
            // 
            numAmount.BackColor = Color.White;
            numAmount.DecimalPlaces = 2;
            numAmount.Dock = DockStyle.Fill;
            numAmount.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            numAmount.Location = new Point(174, 146);
            numAmount.Margin = new Padding(4, 8, 4, 6);
            numAmount.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
            numAmount.Name = "numAmount";
            numAmount.Size = new Size(438, 32);
            numAmount.TabIndex = 7;
            numAmount.TextAlign = HorizontalAlignment.Right;
            numAmount.ThousandsSeparator = true;
            // 
            // lblDate
            // 
            lblDate.Dock = DockStyle.Fill;
            lblDate.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDate.ForeColor = Color.FromArgb(14, 82, 52);
            lblDate.Location = new Point(4, 184);
            lblDate.Margin = new Padding(4, 0, 4, 0);
            lblDate.Name = "lblDate";
            lblDate.Size = new Size(162, 46);
            lblDate.TabIndex = 8;
            lblDate.Text = "التاريخ:";
            lblDate.TextAlign = ContentAlignment.MiddleRight;
            // 
            // dtpDate
            // 
            dtpDate.BackColor = Color.White;
            dtpDate.Dock = DockStyle.Fill;
            dtpDate.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dtpDate.Format = DateTimePickerFormat.Short;
            dtpDate.Location = new Point(174, 192);
            dtpDate.Margin = new Padding(4, 8, 4, 6);
            dtpDate.Name = "dtpDate";
            dtpDate.Size = new Size(438, 32);
            dtpDate.TabIndex = 9;
            // 
            // lblNotes
            // 
            lblNotes.Dock = DockStyle.Fill;
            lblNotes.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNotes.ForeColor = Color.FromArgb(14, 82, 52);
            lblNotes.Location = new Point(4, 230);
            lblNotes.Margin = new Padding(4, 0, 4, 0);
            lblNotes.Name = "lblNotes";
            lblNotes.Size = new Size(162, 156);
            lblNotes.TabIndex = 10;
            lblNotes.Text = "ملاحظات:";
            lblNotes.TextAlign = ContentAlignment.TopRight;
            // 
            // txtNotes
            // 
            txtNotes.BackColor = Color.White;
            txtNotes.Dock = DockStyle.Fill;
            txtNotes.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtNotes.Location = new Point(174, 238);
            txtNotes.Margin = new Padding(4, 8, 4, 8);
            txtNotes.Multiline = true;
            txtNotes.Name = "txtNotes";
            txtNotes.ScrollBars = ScrollBars.Vertical;
            txtNotes.Size = new Size(438, 140);
            txtNotes.TabIndex = 11;
            // 
            // pnlFooter
            // 
            pnlFooter.BackColor = Color.FromArgb(235, 240, 235);
            pnlFooter.Controls.Add(flpButtons);
            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Location = new Point(0, 418);
            pnlFooter.Margin = new Padding(0);
            pnlFooter.Name = "pnlFooter";
            pnlFooter.Size = new Size(640, 74);
            pnlFooter.TabIndex = 1;
            // 
            // flpButtons
            // 
            flpButtons.BackColor = Color.FromArgb(235, 240, 235);
            flpButtons.Controls.Add(btnSave);
            flpButtons.Controls.Add(btnCancel);
            flpButtons.Dock = DockStyle.Fill;
            flpButtons.FlowDirection = FlowDirection.RightToLeft;
            flpButtons.Location = new Point(0, 0);
            flpButtons.Margin = new Padding(0);
            flpButtons.Name = "flpButtons";
            flpButtons.Padding = new Padding(12, 11, 12, 11);
            flpButtons.Size = new Size(640, 74);
            flpButtons.TabIndex = 0;
            flpButtons.WrapContents = false;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(25, 135, 84);
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(486, 11);
            btnSave.Margin = new Padding(4);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(140, 40);
            btnSave.TabIndex = 0;
            btnSave.Text = "حفظ السند (F4)";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.FromArgb(108, 117, 125);
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCancel.ForeColor = Color.White;
            btnCancel.Location = new Point(342, 11);
            btnCancel.Margin = new Padding(4);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(136, 40);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "إلغاء (Esc)";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // toolTip1
            // 
            toolTip1.AutomaticDelay = 300;
            toolTip1.BackColor = Color.White;
            toolTip1.ForeColor = Color.Black;
            // 
            // liveTip
            // 
            liveTip.AutomaticDelay = 300;
            liveTip.BackColor = Color.White;
            liveTip.ForeColor = Color.Black;
            // 
            // errorProvider
            // 
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errorProvider.ContainerControl = this;
            errorProvider.RightToLeft = true;
            // 
            // frmSupplierPaymentDialog
            // 
            AcceptButton = btnSave;
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.FromArgb(170, 220, 170);
            CancelButton = btnCancel;
            ClientSize = new Size(640, 492);
            Controls.Add(gbPaymentInfo);
            Controls.Add(pnlFooter);
            Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            KeyPreview = true;
            Margin = new Padding(4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmSupplierPaymentDialog";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterParent;
            Text = "سند صرف جديد";
            gbPaymentInfo.ResumeLayout(false);
            tlpInfo.ResumeLayout(false);
            tlpInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numAmount).EndInit();
            pnlFooter.ResumeLayout(false);
            flpButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
        }

        private System.Windows.Forms.GroupBox gbPaymentInfo;
        private System.Windows.Forms.TableLayoutPanel tlpInfo;
        private System.Windows.Forms.Label lblSupplier, lblBalance, lblBalanceVal, lblInvoice, lblAmount, lblDate, lblNotes;
        private System.Windows.Forms.ComboBox cmbSupplier, cmbInvoice;
        private System.Windows.Forms.NumericUpDown numAmount;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.FlowLayoutPanel flpButtons;
        private System.Windows.Forms.Button btnSave, btnCancel;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolTip liveTip;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}
