namespace SalesSystem.Desktop.Screens.Payments.Forms
{
    partial class frmAddUpdateSupplierPayment
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            components?.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            lblMode = new Label();
            lblSupplier = new Label();
            cmbSupplier = new ComboBox();
            lblAmount = new Label();
            numAmount = new NumericUpDown();
            lblPaymentMethod = new Label();
            cmbPaymentMethod = new ComboBox();
            lblNotes = new Label();
            txtNotes = new TextBox();
            btnSave = new Button();
            btnClose = new Button();
            ((System.ComponentModel.ISupportInitialize)numAmount).BeginInit();
            SuspendLayout();
            // 
            // lblMode
            // 
            lblMode.Anchor = AnchorStyles.Top;
            lblMode.AutoSize = true;
            lblMode.BackColor = Color.WhiteSmoke;
        lblMode.FlatStyle = FlatStyle.Flat;
        lblMode.Font = new Font("Tahoma", 14F, FontStyle.Bold);
        lblMode.ForeColor = Color.Firebrick;
        lblMode.Location = new Point(180, 15);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(200, 34);
            lblMode.TabIndex = 30;
            lblMode.Text = "سند صرف جديد";
            // 
            // lblSupplier
            // 
            lblSupplier.AutoSize = true;
            lblSupplier.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblSupplier.Location = new Point(50, 75);
            lblSupplier.Name = "lblSupplier";
            lblSupplier.Size = new Size(74, 24);
            lblSupplier.TabIndex = 0;
            lblSupplier.Text = "المورد:";
            // 
            // cmbSupplier
            // 
            cmbSupplier.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSupplier.Font = new Font("Tahoma", 10F);
            cmbSupplier.FormattingEnabled = true;
            cmbSupplier.Location = new Point(157, 72);
            cmbSupplier.Name = "cmbSupplier";
            cmbSupplier.RightToLeft = RightToLeft.Yes;
            cmbSupplier.Size = new Size(310, 32);
            cmbSupplier.TabIndex = 1;
            // 
            // lblAmount
            // 
            lblAmount.AutoSize = true;
            lblAmount.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblAmount.Location = new Point(51, 120);
            lblAmount.Name = "lblAmount";
            lblAmount.Size = new Size(73, 24);
            lblAmount.TabIndex = 2;
            lblAmount.Text = "المبلغ:";
            // 
            // numAmount
            // 
            numAmount.DecimalPlaces = 2;
            numAmount.Font = new Font("Tahoma", 10F);
            numAmount.Location = new Point(157, 118);
            numAmount.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
            numAmount.Name = "numAmount";
            numAmount.Size = new Size(310, 32);
            numAmount.TabIndex = 3;
            // 
            // lblPaymentMethod
            // 
            lblPaymentMethod.AutoSize = true;
            lblPaymentMethod.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblPaymentMethod.Location = new Point(22, 165);
            lblPaymentMethod.Name = "lblPaymentMethod";
            lblPaymentMethod.Size = new Size(130, 24);
            lblPaymentMethod.TabIndex = 4;
            lblPaymentMethod.Text = "طريقة الدفع:";
            // 
            // cmbPaymentMethod
            // 
            cmbPaymentMethod.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPaymentMethod.Font = new Font("Tahoma", 10F);
            cmbPaymentMethod.FormattingEnabled = true;
            cmbPaymentMethod.Items.AddRange(new object[] { "نقدي", "آجل", "مختلط" });
            cmbPaymentMethod.Location = new Point(157, 162);
            cmbPaymentMethod.Name = "cmbPaymentMethod";
            cmbPaymentMethod.RightToLeft = RightToLeft.Yes;
            cmbPaymentMethod.Size = new Size(310, 32);
            cmbPaymentMethod.TabIndex = 5;
            // 
            // lblNotes
            // 
            lblNotes.AutoSize = true;
            lblNotes.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblNotes.Location = new Point(53, 210);
            lblNotes.Name = "lblNotes";
            lblNotes.Size = new Size(69, 24);
            lblNotes.TabIndex = 6;
            lblNotes.Text = "البيان:";
            // 
            // txtNotes
            // 
            txtNotes.Font = new Font("Tahoma", 10F);
            txtNotes.Location = new Point(157, 207);
            txtNotes.Multiline = true;
            txtNotes.Name = "txtNotes";
            txtNotes.RightToLeft = RightToLeft.Yes;
            txtNotes.Size = new Size(310, 80);
            txtNotes.TabIndex = 7;
            // 
        // btnSave
        // 
        btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnSave.BackColor = Color.White;
        btnSave.FlatAppearance.BorderColor = Color.Black;
        btnSave.FlatAppearance.BorderSize = 2;
        btnSave.FlatStyle = FlatStyle.Popup;
        btnSave.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnSave.Location = new Point(430, 320);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(120, 45);
        btnSave.TabIndex = 8;
        btnSave.Text = "حفظ";
        btnSave.UseVisualStyleBackColor = false;
        btnSave.Click += btnSave_Click;
        // 
        // btnClose
        // 
        btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnClose.BackColor = Color.White;
        btnClose.DialogResult = DialogResult.Cancel;
        btnClose.FlatAppearance.BorderColor = Color.Black;
        btnClose.FlatAppearance.BorderSize = 2;
        btnClose.FlatStyle = FlatStyle.Popup;
        btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnClose.Location = new Point(12, 320);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(120, 45);
        btnClose.TabIndex = 9;
        btnClose.Text = "إلغاء";
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += btnClose_Click;
        // 
        // frmAddUpdateSupplierPayment
        // 
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.WhiteSmoke;
        CancelButton = btnClose;
        ClientSize = new Size(600, 400);
        Controls.Add(btnClose);
        Controls.Add(btnSave);
        Controls.Add(txtNotes);
        Controls.Add(lblNotes);
        Controls.Add(cmbPaymentMethod);
        Controls.Add(lblPaymentMethod);
        Controls.Add(numAmount);
        Controls.Add(lblAmount);
        Controls.Add(cmbSupplier);
        Controls.Add(lblSupplier);
        Controls.Add(lblMode);
        Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "frmAddUpdateSupplierPayment";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "سند صرف جديد";
            Load += frmSupplierPaymentEditor_Load;
            ((System.ComponentModel.ISupportInitialize)numAmount).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.Label lblSupplier;
        private System.Windows.Forms.ComboBox cmbSupplier;
        private System.Windows.Forms.Label lblAmount;
        private System.Windows.Forms.NumericUpDown numAmount;
        private System.Windows.Forms.Label lblPaymentMethod;
        private System.Windows.Forms.ComboBox cmbPaymentMethod;
        private System.Windows.Forms.Label lblNotes;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
    }
}
