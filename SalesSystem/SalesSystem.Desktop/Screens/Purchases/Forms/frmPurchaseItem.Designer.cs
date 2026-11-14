namespace SalesSystem.Desktop.Screens.Purchases.Forms
{
    partial class frmPurchaseItem
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
            errorProvider = new ErrorProvider(components);
            toolTip = new ToolTip(components);
            tipTafqeet = new ToolTip(components);
            pnlTitle = new Panel();
            lblTitle = new Label();
            pnlBody = new Panel();
            gbItemData = new GroupBox();
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
            gbItemData.SuspendLayout();
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
            pnlTitle.BackColor = Color.FromArgb(14, 165, 103);
            pnlTitle.Controls.Add(lblTitle);
            pnlTitle.Dock = DockStyle.Top;
            pnlTitle.Location = new Point(0, 0);
            pnlTitle.Margin = new Padding(0);
            pnlTitle.Name = "pnlTitle";
            pnlTitle.Size = new Size(560, 50);
            pnlTitle.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.BackColor = Color.FromArgb(14, 165, 103);
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.Font = new Font("Tahoma", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(0, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(560, 50);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "صنف مشتريات";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlBody
            // 
            pnlBody.Controls.Add(gbItemData);
            pnlBody.Dock = DockStyle.Fill;
            pnlBody.Location = new Point(0, 50);
            pnlBody.Margin = new Padding(0);
            pnlBody.Name = "pnlBody";
            pnlBody.Padding = new Padding(12, 10, 12, 10);
            pnlBody.Size = new Size(560, 488);
            pnlBody.TabIndex = 1;
            // 
            // gbItemData
            // 
            gbItemData.BackColor = Color.FromArgb(235, 245, 235);
            gbItemData.Controls.Add(tblFields);
            gbItemData.Dock = DockStyle.Fill;
            gbItemData.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            gbItemData.ForeColor = Color.FromArgb(14, 82, 52);
            gbItemData.Location = new Point(12, 10);
            gbItemData.Name = "gbItemData";
            gbItemData.Padding = new Padding(12, 6, 12, 8);
            gbItemData.Size = new Size(536, 468);
            gbItemData.TabIndex = 0;
            gbItemData.TabStop = false;
            gbItemData.Text = "بيانات صنف الشراء";
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
            tblFields.Location = new Point(12, 22);
            tblFields.Name = "tblFields";
            tblFields.RowCount = 10;
            tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            tblFields.Size = new Size(512, 438);
            tblFields.TabIndex = 0;
            // 
            // lblProduct
            // 
            lblProduct.Dock = DockStyle.Fill;
            lblProduct.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblProduct.ForeColor = Color.FromArgb(14, 82, 52);
            lblProduct.Margin = new Padding(0, 0, 8, 0);
            lblProduct.Name = "lblProduct";
            lblProduct.Size = new Size(142, 38);
            lblProduct.TabIndex = 0;
            lblProduct.Text = "الصنف:";
            lblProduct.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cmbProduct
            // 
            cmbProduct.BackColor = Color.White;
            cmbProduct.Dock = DockStyle.Fill;
            cmbProduct.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbProduct.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmbProduct.FormattingEnabled = true;
            cmbProduct.Location = new Point(153, 3);
            cmbProduct.Name = "cmbProduct";
            cmbProduct.Size = new Size(356, 24);
            cmbProduct.TabIndex = 1;
            cmbProduct.SelectedIndexChanged += cmbProduct_SelectedIndexChanged;
            // 
            // lblBarcodeCaption
            // 
            lblBarcodeCaption.Dock = DockStyle.Fill;
            lblBarcodeCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblBarcodeCaption.ForeColor = Color.FromArgb(14, 82, 52);
            lblBarcodeCaption.Margin = new Padding(0, 0, 8, 0);
            lblBarcodeCaption.Name = "lblBarcodeCaption";
            lblBarcodeCaption.Size = new Size(142, 38);
            lblBarcodeCaption.TabIndex = 2;
            lblBarcodeCaption.Text = "الباركود:";
            lblBarcodeCaption.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblBarcodeValue
            // 
            lblBarcodeValue.Dock = DockStyle.Fill;
            lblBarcodeValue.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblBarcodeValue.ForeColor = Color.FromArgb(14, 82, 52);
            lblBarcodeValue.Location = new Point(153, 41);
            lblBarcodeValue.Name = "lblBarcodeValue";
            lblBarcodeValue.Size = new Size(356, 38);
            lblBarcodeValue.TabIndex = 3;
            lblBarcodeValue.Text = "—";
            lblBarcodeValue.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblUnit
            // 
            lblUnit.Dock = DockStyle.Fill;
            lblUnit.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblUnit.ForeColor = Color.FromArgb(14, 82, 52);
            lblUnit.Margin = new Padding(0, 0, 8, 0);
            lblUnit.Name = "lblUnit";
            lblUnit.Size = new Size(142, 38);
            lblUnit.TabIndex = 4;
            lblUnit.Text = "الوحدة:";
            lblUnit.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cmbUnit
            // 
            cmbUnit.BackColor = Color.White;
            cmbUnit.Dock = DockStyle.Fill;
            cmbUnit.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbUnit.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmbUnit.FormattingEnabled = true;
            cmbUnit.Location = new Point(153, 79);
            cmbUnit.Name = "cmbUnit";
            cmbUnit.Size = new Size(356, 24);
            cmbUnit.TabIndex = 5;
            cmbUnit.SelectedIndexChanged += cmbUnit_SelectedIndexChanged;
            // 
            // lblFactorCaption
            // 
            lblFactorCaption.Dock = DockStyle.Fill;
            lblFactorCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblFactorCaption.ForeColor = Color.FromArgb(14, 82, 52);
            lblFactorCaption.Margin = new Padding(0, 0, 8, 0);
            lblFactorCaption.Name = "lblFactorCaption";
            lblFactorCaption.Size = new Size(142, 38);
            lblFactorCaption.TabIndex = 6;
            lblFactorCaption.Text = "معامل التحويل:";
            lblFactorCaption.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblConversionFactor
            // 
            lblConversionFactor.Dock = DockStyle.Fill;
            lblConversionFactor.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblConversionFactor.ForeColor = Color.FromArgb(14, 82, 52);
            lblConversionFactor.Location = new Point(153, 117);
            lblConversionFactor.Name = "lblConversionFactor";
            lblConversionFactor.Size = new Size(356, 38);
            lblConversionFactor.TabIndex = 7;
            lblConversionFactor.Text = "1";
            lblConversionFactor.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblQuantity
            // 
            lblQuantity.Dock = DockStyle.Fill;
            lblQuantity.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblQuantity.ForeColor = Color.FromArgb(14, 82, 52);
            lblQuantity.Margin = new Padding(0, 0, 8, 0);
            lblQuantity.Name = "lblQuantity";
            lblQuantity.Size = new Size(142, 38);
            lblQuantity.TabIndex = 8;
            lblQuantity.Text = "الكمية:";
            lblQuantity.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // numQuantity
            // 
            numQuantity.BackColor = Color.White;
            numQuantity.DecimalPlaces = 3;
            numQuantity.Dock = DockStyle.Fill;
            numQuantity.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            numQuantity.Increment = 0.5M;
            numQuantity.Location = new Point(153, 155);
            numQuantity.Maximum = 999999.999M;
            numQuantity.Name = "numQuantity";
            numQuantity.Size = new Size(356, 23);
            numQuantity.TabIndex = 9;
            numQuantity.ThousandsSeparator = true;
            numQuantity.ValueChanged += num_ValueChanged;
            // 
            // lblCost
            // 
            lblCost.Dock = DockStyle.Fill;
            lblCost.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCost.ForeColor = Color.FromArgb(14, 82, 52);
            lblCost.Margin = new Padding(0, 0, 8, 0);
            lblCost.Name = "lblCost";
            lblCost.Size = new Size(142, 38);
            lblCost.TabIndex = 10;
            lblCost.Text = "التكلفة:";
            lblCost.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // numCost
            // 
            numCost.BackColor = Color.White;
            numCost.DecimalPlaces = 2;
            numCost.Dock = DockStyle.Fill;
            numCost.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            numCost.Increment = 10M;
            numCost.Location = new Point(153, 193);
            numCost.Maximum = 99999999.99M;
            numCost.Name = "numCost";
            numCost.Size = new Size(356, 23);
            numCost.TabIndex = 11;
            numCost.ThousandsSeparator = true;
            numCost.ValueChanged += num_ValueChanged;
            // 
            // lblDiscount
            // 
            lblDiscount.Dock = DockStyle.Fill;
            lblDiscount.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDiscount.ForeColor = Color.FromArgb(14, 82, 52);
            lblDiscount.Margin = new Padding(0, 0, 8, 0);
            lblDiscount.Name = "lblDiscount";
            lblDiscount.Size = new Size(142, 38);
            lblDiscount.TabIndex = 12;
            lblDiscount.Text = "الخصم:";
            lblDiscount.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // numDiscount
            // 
            numDiscount.BackColor = Color.White;
            numDiscount.DecimalPlaces = 2;
            numDiscount.Dock = DockStyle.Fill;
            numDiscount.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            numDiscount.Increment = 5M;
            numDiscount.Location = new Point(153, 231);
            numDiscount.Maximum = 99999999.99M;
            numDiscount.Name = "numDiscount";
            numDiscount.Size = new Size(356, 23);
            numDiscount.TabIndex = 13;
            numDiscount.ThousandsSeparator = true;
            numDiscount.ValueChanged += num_ValueChanged;
            // 
            // lblBatch
            // 
            lblBatch.Dock = DockStyle.Fill;
            lblBatch.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblBatch.ForeColor = Color.FromArgb(14, 82, 52);
            lblBatch.Margin = new Padding(0, 0, 8, 0);
            lblBatch.Name = "lblBatch";
            lblBatch.Size = new Size(142, 38);
            lblBatch.TabIndex = 14;
            lblBatch.Text = "رقم التشغيلة:";
            lblBatch.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtBatch
            // 
            txtBatch.BackColor = Color.White;
            txtBatch.Dock = DockStyle.Fill;
            txtBatch.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtBatch.Location = new Point(153, 269);
            txtBatch.Name = "txtBatch";
            txtBatch.PlaceholderText = "مثال: B-2026-001";
            txtBatch.Size = new Size(356, 23);
            txtBatch.TabIndex = 15;
            // 
            // lblExpiry
            // 
            lblExpiry.Dock = DockStyle.Fill;
            lblExpiry.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblExpiry.ForeColor = Color.FromArgb(14, 82, 52);
            lblExpiry.Margin = new Padding(0, 0, 8, 0);
            lblExpiry.Name = "lblExpiry";
            lblExpiry.Size = new Size(142, 38);
            lblExpiry.TabIndex = 16;
            lblExpiry.Text = "تاريخ الصلاحية:";
            lblExpiry.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // flpExpiry
            // 
            flpExpiry.Controls.Add(chkExpiry);
            flpExpiry.Controls.Add(dtpExpiry);
            flpExpiry.Dock = DockStyle.Fill;
            flpExpiry.FlowDirection = FlowDirection.RightToLeft;
            flpExpiry.Location = new Point(153, 307);
            flpExpiry.Name = "flpExpiry";
            flpExpiry.Size = new Size(356, 38);
            flpExpiry.TabIndex = 17;
            flpExpiry.WrapContents = false;
            // 
            // chkExpiry
            // 
            chkExpiry.AutoSize = true;
            chkExpiry.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            chkExpiry.ForeColor = Color.FromArgb(14, 82, 52);
            chkExpiry.Margin = new Padding(0, 9, 12, 0);
            chkExpiry.Name = "chkExpiry";
            chkExpiry.Size = new Size(105, 21);
            chkExpiry.TabIndex = 0;
            chkExpiry.Text = "له تاريخ صلاحية";
            chkExpiry.UseVisualStyleBackColor = false;
            chkExpiry.CheckedChanged += chkExpiry_CheckedChanged;
            // 
            // dtpExpiry
            // 
            dtpExpiry.Enabled = false;
            dtpExpiry.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dtpExpiry.Format = DateTimePickerFormat.Short;
            dtpExpiry.Location = new Point(183, 5);
            dtpExpiry.MinDate = DateTime.Today;
            dtpExpiry.Name = "dtpExpiry";
            dtpExpiry.Size = new Size(170, 24);
            dtpExpiry.TabIndex = 1;
            dtpExpiry.Value = DateTime.Today.AddMonths(6);
            // 
            // lblTotal
            // 
            lblTotal.Dock = DockStyle.Fill;
            lblTotal.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotal.ForeColor = Color.FromArgb(14, 82, 52);
            lblTotal.Margin = new Padding(0, 0, 8, 0);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(142, 38);
            lblTotal.TabIndex = 18;
            lblTotal.Text = "الإجمالي:";
            lblTotal.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTotalValue
            // 
            lblTotalValue.BackColor = Color.FromArgb(255, 255, 180);
            lblTotalValue.Dock = DockStyle.Fill;
            lblTotalValue.Font = new Font("Tahoma", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalValue.ForeColor = Color.FromArgb(14, 82, 52);
            lblTotalValue.Location = new Point(153, 345);
            lblTotalValue.Name = "lblTotalValue";
            lblTotalValue.Size = new Size(356, 38);
            lblTotalValue.TabIndex = 19;
            lblTotalValue.Text = "0.00";
            lblTotalValue.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pnlFooter
            // 
            pnlFooter.BackColor = Color.FromArgb(235, 240, 235);
            pnlFooter.Controls.Add(flpButtons);
            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Location = new Point(0, 538);
            pnlFooter.Margin = new Padding(0);
            pnlFooter.Name = "pnlFooter";
            pnlFooter.Padding = new Padding(14, 10, 14, 10);
            pnlFooter.Size = new Size(560, 62);
            pnlFooter.TabIndex = 2;
            // 
            // flpButtons
            // 
            flpButtons.Controls.Add(btnOk);
            flpButtons.Controls.Add(btnCancel);
            flpButtons.Dock = DockStyle.Fill;
            flpButtons.FlowDirection = FlowDirection.RightToLeft;
            flpButtons.Location = new Point(14, 10);
            flpButtons.Name = "flpButtons";
            flpButtons.Size = new Size(532, 42);
            flpButtons.TabIndex = 0;
            flpButtons.WrapContents = false;
            // 
            // btnOk
            // 
            btnOk.BackColor = Color.FromArgb(25, 135, 84);
            btnOk.FlatAppearance.BorderSize = 0;
            btnOk.FlatStyle = FlatStyle.Flat;
            btnOk.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnOk.ForeColor = Color.White;
            btnOk.Location = new Point(402, 0);
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
            btnCancel.BackColor = Color.FromArgb(248, 249, 250);
            btnCancel.FlatAppearance.BorderColor = Color.FromArgb(222, 226, 230);
            btnCancel.FlatAppearance.BorderSize = 1;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCancel.ForeColor = Color.FromArgb(33, 37, 41);
            btnCancel.Location = new Point(295, 0);
            btnCancel.Margin = new Padding(0);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(107, 40);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "إلغاء";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // frmPurchaseItem
            // 
            AcceptButton = btnOk;
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.FromArgb(170, 220, 170);
            CancelButton = btnCancel;
            ClientSize = new Size(560, 600);
            Controls.Add(pnlBody);
            Controls.Add(pnlFooter);
            Controls.Add(pnlTitle);
            Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmPurchaseItem";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterParent;
            Text = "صنف مشتريات";
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ((System.ComponentModel.ISupportInitialize)numQuantity).EndInit();
            ((System.ComponentModel.ISupportInitialize)numCost).EndInit();
            ((System.ComponentModel.ISupportInitialize)numDiscount).EndInit();
            pnlTitle.ResumeLayout(false);
            pnlBody.ResumeLayout(false);
            gbItemData.ResumeLayout(false);
            tblFields.ResumeLayout(false);
            tblFields.PerformLayout();
            flpExpiry.ResumeLayout(false);
            flpExpiry.PerformLayout();
            pnlFooter.ResumeLayout(false);
            flpButtons.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel pnlTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlBody;
        private System.Windows.Forms.GroupBox gbItemData;
        private System.Windows.Forms.TableLayoutPanel tblFields;
        private System.Windows.Forms.Label lblProduct, lblBarcodeCaption, lblBarcodeValue, lblUnit, lblFactorCaption, lblConversionFactor;
        private System.Windows.Forms.Label lblQuantity, lblCost, lblDiscount, lblBatch, lblExpiry, lblTotal, lblTotalValue;
        private System.Windows.Forms.ComboBox cmbProduct, cmbUnit;
        private System.Windows.Forms.NumericUpDown numQuantity, numCost, numDiscount;
        private System.Windows.Forms.TextBox txtBatch;
        private System.Windows.Forms.FlowLayoutPanel flpExpiry, flpButtons;
        private System.Windows.Forms.CheckBox chkExpiry;
        private System.Windows.Forms.DateTimePicker dtpExpiry;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Button btnOk, btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ToolTip tipTafqeet;
    }
}
