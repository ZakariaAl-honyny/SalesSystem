namespace SalesSystem.Desktop.Screens.Purchases.Forms
{
    partial class frmPurchaseItem
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
            errorProvider = new ErrorProvider(components);
            pnlTitle = new Panel();
            lblTitle = new Label();
            pnlBody = new Panel();
            tblFields = new TableLayoutPanel();
            lblProduct = new Label();
            cmbProduct = new ComboBox();
            lblBarcodeCaption = new Label();
            lblBarcodeValue = new Label();
            lblUnit = new Label();
            cmbUnit = new ComboBox();
            lblFactorCaption = new Label();
            lblConversionFactor = new Label();
            lblQuantity = new Label();
            numQuantity = new NumericUpDown();
            lblCost = new Label();
            numCost = new NumericUpDown();
            lblDiscount = new Label();
            numDiscount = new NumericUpDown();
            lblBatch = new Label();
            txtBatch = new TextBox();
            lblExpiry = new Label();
            flpExpiry = new FlowLayoutPanel();
            chkExpiry = new CheckBox();
            dtpExpiry = new DateTimePicker();
            lblTotal = new Label();
            lblTotalValue = new Label();
            pnlFooter = new Panel();
            flpButtons = new FlowLayoutPanel();
            btnOk = new Button();
            btnCancel = new Button();
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numQuantity).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numCost).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numDiscount).BeginInit();
            pnlTitle.SuspendLayout();
            pnlBody.SuspendLayout();
            tblFields.SuspendLayout();
            flpExpiry.SuspendLayout();
            pnlFooter.SuspendLayout();
            flpButtons.SuspendLayout();
            SuspendLayout();
            // 
            // errorProvider
            // 
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errorProvider.ContainerControl = this;
            // 
            // pnlTitle
            // 
            pnlTitle.Controls.Add(lblTitle);
            pnlTitle.Dock = DockStyle.Top;
            pnlTitle.Name = "pnlTitle";
            pnlTitle.Size = new Size(520, 50);
            pnlTitle.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.BackColor = Color.Firebrick;
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.Font = new Font("Tahoma", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.ForeColor = Color.White;
            lblTitle.Name = "lblTitle";
            lblTitle.Text = "صنف مشتريات";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlBody
            // 
            pnlBody.Controls.Add(tblFields);
            pnlBody.Dock = DockStyle.Fill;
            pnlBody.Name = "pnlBody";
            pnlBody.Padding = new Padding(14, 10, 14, 10);
            pnlBody.TabIndex = 1;
            // 
            // tblFields
            // 
            tblFields.ColumnCount = 2;
            tblFields.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tblFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblFields.Controls.Add(lblProduct, 0, 0);
            tblFields.Controls.Add(cmbProduct, 1, 0);
            tblFields.Controls.Add(lblBarcodeCaption, 0, 1);
            tblFields.Controls.Add(lblBarcodeValue, 1, 1);
            tblFields.Controls.Add(lblUnit, 0, 2);
            tblFields.Controls.Add(cmbUnit, 1, 2);
            tblFields.Controls.Add(lblFactorCaption, 0, 3);
            tblFields.Controls.Add(lblConversionFactor, 1, 3);
            tblFields.Controls.Add(lblQuantity, 0, 4);
            tblFields.Controls.Add(numQuantity, 1, 4);
            tblFields.Controls.Add(lblCost, 0, 5);
            tblFields.Controls.Add(numCost, 1, 5);
            tblFields.Controls.Add(lblDiscount, 0, 6);
            tblFields.Controls.Add(numDiscount, 1, 6);
            tblFields.Controls.Add(lblBatch, 0, 7);
            tblFields.Controls.Add(txtBatch, 1, 7);
            tblFields.Controls.Add(lblExpiry, 0, 8);
            tblFields.Controls.Add(flpExpiry, 1, 8);
            tblFields.Controls.Add(lblTotal, 0, 9);
            tblFields.Controls.Add(lblTotalValue, 1, 9);
            tblFields.Dock = DockStyle.Fill;
            tblFields.Name = "tblFields";
            tblFields.RowCount = 10;
            tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            // 
            // lblProduct
            // 
            lblProduct.Dock = DockStyle.Fill;
            lblProduct.Margin = new Padding(0, 0, 8, 0);
            lblProduct.Name = "lblProduct";
            lblProduct.Text = "الصنف:";
            lblProduct.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cmbProduct
            // 
            cmbProduct.Dock = DockStyle.Fill;
            cmbProduct.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbProduct.Name = "cmbProduct";
            cmbProduct.SelectedIndexChanged += cmbProduct_SelectedIndexChanged;
            // 
            // lblBarcodeCaption
            // 
            lblBarcodeCaption.Dock = DockStyle.Fill;
            lblBarcodeCaption.Margin = new Padding(0, 0, 8, 0);
            lblBarcodeCaption.Name = "lblBarcodeCaption";
            lblBarcodeCaption.Text = "الباركود:";
            lblBarcodeCaption.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblBarcodeValue
            // 
            lblBarcodeValue.Dock = DockStyle.Fill;
            lblBarcodeValue.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblBarcodeValue.ForeColor = Color.Navy;
            lblBarcodeValue.Name = "lblBarcodeValue";
            lblBarcodeValue.Text = "—";
            lblBarcodeValue.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblUnit
            // 
            lblUnit.Dock = DockStyle.Fill;
            lblUnit.Margin = new Padding(0, 0, 8, 0);
            lblUnit.Name = "lblUnit";
            lblUnit.Text = "الوحدة:";
            lblUnit.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cmbUnit
            // 
            cmbUnit.Dock = DockStyle.Fill;
            cmbUnit.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbUnit.Name = "cmbUnit";
            cmbUnit.SelectedIndexChanged += cmbUnit_SelectedIndexChanged;
            // 
            // lblFactorCaption
            // 
            lblFactorCaption.Dock = DockStyle.Fill;
            lblFactorCaption.Margin = new Padding(0, 0, 8, 0);
            lblFactorCaption.Name = "lblFactorCaption";
            lblFactorCaption.Text = "معامل التحويل:";
            lblFactorCaption.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblConversionFactor
            // 
            lblConversionFactor.Dock = DockStyle.Fill;
            lblConversionFactor.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblConversionFactor.ForeColor = Color.ForestGreen;
            lblConversionFactor.Name = "lblConversionFactor";
            lblConversionFactor.Text = "1";
            lblConversionFactor.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblQuantity
            // 
            lblQuantity.Dock = DockStyle.Fill;
            lblQuantity.Margin = new Padding(0, 0, 8, 0);
            lblQuantity.Name = "lblQuantity";
            lblQuantity.Text = "الكمية:";
            lblQuantity.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // numQuantity
            // 
            numQuantity.DecimalPlaces = 3;
            numQuantity.Dock = DockStyle.Fill;
            numQuantity.Increment = 0.5M;
            numQuantity.Maximum = 999999.999M;
            numQuantity.Name = "numQuantity";
            numQuantity.ThousandsSeparator = true;
            numQuantity.ValueChanged += num_ValueChanged;
            // 
            // lblCost
            // 
            lblCost.Dock = DockStyle.Fill;
            lblCost.Margin = new Padding(0, 0, 8, 0);
            lblCost.Name = "lblCost";
            lblCost.Text = "التكلفة:";
            lblCost.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // numCost
            // 
            numCost.DecimalPlaces = 2;
            numCost.Dock = DockStyle.Fill;
            numCost.Increment = 10M;
            numCost.Maximum = 99999999.99M;
            numCost.Name = "numCost";
            numCost.ThousandsSeparator = true;
            numCost.ValueChanged += num_ValueChanged;
            // 
            // lblDiscount
            // 
            lblDiscount.Dock = DockStyle.Fill;
            lblDiscount.Margin = new Padding(0, 0, 8, 0);
            lblDiscount.Name = "lblDiscount";
            lblDiscount.Text = "الخصم:";
            lblDiscount.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // numDiscount
            // 
            numDiscount.DecimalPlaces = 2;
            numDiscount.Dock = DockStyle.Fill;
            numDiscount.Increment = 5M;
            numDiscount.Maximum = 99999999.99M;
            numDiscount.Name = "numDiscount";
            numDiscount.ThousandsSeparator = true;
            numDiscount.ValueChanged += num_ValueChanged;
            // 
            // lblBatch
            // 
            lblBatch.Dock = DockStyle.Fill;
            lblBatch.Margin = new Padding(0, 0, 8, 0);
            lblBatch.Name = "lblBatch";
            lblBatch.Text = "رقم التشغيلة:";
            lblBatch.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtBatch
            // 
            txtBatch.Dock = DockStyle.Fill;
            txtBatch.Name = "txtBatch";
            txtBatch.PlaceholderText = "مثال: B-2026-001";
            // 
            // lblExpiry
            // 
            lblExpiry.Dock = DockStyle.Fill;
            lblExpiry.Margin = new Padding(0, 0, 8, 0);
            lblExpiry.Name = "lblExpiry";
            lblExpiry.Text = "تاريخ الصلاحية:";
            lblExpiry.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // flpExpiry
            // 
            flpExpiry.Controls.Add(chkExpiry);
            flpExpiry.Controls.Add(dtpExpiry);
            flpExpiry.Dock = DockStyle.Fill;
            flpExpiry.FlowDirection = FlowDirection.RightToLeft;
            flpExpiry.Name = "flpExpiry";
            flpExpiry.WrapContents = false;
            // 
            // chkExpiry
            // 
            chkExpiry.AutoSize = true;
            chkExpiry.CheckedChanged += chkExpiry_CheckedChanged;
            chkExpiry.Margin = new Padding(0, 9, 12, 0);
            chkExpiry.Name = "chkExpiry";
            chkExpiry.Text = "له تاريخ صلاحية";
            // 
            // dtpExpiry
            // 
            dtpExpiry.Enabled = false;
            dtpExpiry.Format = DateTimePickerFormat.Short;
            dtpExpiry.MinDate = DateTime.Today;
            dtpExpiry.Name = "dtpExpiry";
            dtpExpiry.Size = new Size(170, 36);
            dtpExpiry.Value = DateTime.Today.AddMonths(6);
            // 
            // lblTotal
            // 
            lblTotal.Dock = DockStyle.Fill;
            lblTotal.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotal.Margin = new Padding(0, 0, 8, 0);
            lblTotal.Name = "lblTotal";
            lblTotal.Text = "الإجمالي:";
            lblTotal.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTotalValue
            // 
            lblTotalValue.Dock = DockStyle.Fill;
            lblTotalValue.Font = new Font("Tahoma", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalValue.ForeColor = Color.Blue;
            lblTotalValue.Name = "lblTotalValue";
            lblTotalValue.Text = "0.00";
            lblTotalValue.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pnlFooter
            // 
            pnlFooter.Controls.Add(flpButtons);
            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Name = "pnlFooter";
            pnlFooter.Padding = new Padding(14, 8, 14, 8);
            pnlFooter.Size = new Size(520, 60);
            pnlFooter.TabIndex = 2;
            // 
            // flpButtons
            // 
            flpButtons.Controls.Add(btnOk);
            flpButtons.Controls.Add(btnCancel);
            flpButtons.Dock = DockStyle.Fill;
            flpButtons.FlowDirection = FlowDirection.RightToLeft;
            flpButtons.Name = "flpButtons";
            flpButtons.WrapContents = false;
            // 
            // btnOk
            // 
            btnOk.BackColor = Color.White;
            btnOk.FlatAppearance.BorderColor = Color.Black;
            btnOk.FlatAppearance.BorderSize = 2;
            btnOk.FlatStyle = FlatStyle.Popup;
            btnOk.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnOk.Margin = new Padding(0, 0, 10, 0);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size(130, 40);
            btnOk.TabIndex = 0;
            btnOk.Text = "موافق";
            btnOk.UseVisualStyleBackColor = false;
            btnOk.Click += btnOk_Click;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.White;
            btnCancel.FlatAppearance.BorderColor = Color.Black;
            btnCancel.FlatAppearance.BorderSize = 2;
            btnCancel.FlatStyle = FlatStyle.Popup;
            btnCancel.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(130, 40);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "إلغاء";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // frmPurchaseItem
            // 
            AcceptButton = btnOk;
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            CancelButton = btnCancel;
            ClientSize = new Size(520, 560);
            Controls.Add(pnlBody);
            Controls.Add(pnlFooter);
            Controls.Add(pnlTitle);
            Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmPurchaseItem";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ((System.ComponentModel.ISupportInitialize)numQuantity).EndInit();
            ((System.ComponentModel.ISupportInitialize)numCost).EndInit();
            ((System.ComponentModel.ISupportInitialize)numDiscount).EndInit();
            pnlTitle.ResumeLayout(false);
            pnlBody.ResumeLayout(false);
            tblFields.ResumeLayout(false);
            flpExpiry.ResumeLayout(false);
            flpExpiry.PerformLayout();
            pnlFooter.ResumeLayout(false);
            flpButtons.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.Panel pnlTitle, pnlBody, pnlFooter;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TableLayoutPanel tblFields;
        private System.Windows.Forms.Label lblProduct, lblBarcodeCaption, lblBarcodeValue, lblUnit, lblFactorCaption, lblConversionFactor, lblQuantity, lblCost, lblDiscount, lblBatch, lblExpiry, lblTotal, lblTotalValue;
        private System.Windows.Forms.ComboBox cmbProduct, cmbUnit;
        private System.Windows.Forms.NumericUpDown numQuantity, numCost, numDiscount;
        private System.Windows.Forms.TextBox txtBatch;
        private System.Windows.Forms.FlowLayoutPanel flpExpiry, flpButtons;
        private System.Windows.Forms.CheckBox chkExpiry;
        private System.Windows.Forms.DateTimePicker dtpExpiry;
        private System.Windows.Forms.Button btnOk, btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}