namespace SalesSystem.Desktop.Screens.Purchases.Forms
{
    partial class frmPurchaseDiscount
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
            tblMain = new TableLayoutPanel();
            grpDiscount = new GroupBox();
            tblDiscount = new TableLayoutPanel();
            lblTypeCaption = new Label();
            flpTypes = new FlowLayoutPanel();
            rbAmount = new RadioButton();
            rbPercent = new RadioButton();
            lblValueCaption = new Label();
            tblValueRow = new TableLayoutPanel();
            numDiscount = new NumericUpDown();
            lblUnit = new Label();
            lblRateCaption = new Label();
            cmbTax = new ComboBox();
            grpSummary = new GroupBox();
            tblSummary = new TableLayoutPanel();
            lblSubtotalCaption = new Label();
            lblSubtotalValue = new Label();
            lblDiscountCaption = new Label();
            lblDiscountValue = new Label();
            lblTaxCaption = new Label();
            lblTaxValue = new Label();
            lblNetCaption = new Label();
            lblNetValue = new Label();
            lblNote = new Label();
            pnlFooter = new Panel();
            lblFooterLine = new Label();
            flpFooter = new FlowLayoutPanel();
            btnOk = new Button();
            btnCancel = new Button();
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numDiscount).BeginInit();
            pnlTitle.SuspendLayout();
            pnlBody.SuspendLayout();
            tblMain.SuspendLayout();
            grpDiscount.SuspendLayout();
            tblDiscount.SuspendLayout();
            flpTypes.SuspendLayout();
            tblValueRow.SuspendLayout();
            grpSummary.SuspendLayout();
            tblSummary.SuspendLayout();
            pnlFooter.SuspendLayout();
            flpFooter.SuspendLayout();
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
            pnlTitle.Size = new Size(500, 46);
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
            lblTitle.Size = new Size(500, 46);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "خصم المشتريات";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlBody
            // 
            pnlBody.Controls.Add(tblMain);
            pnlBody.Dock = DockStyle.Fill;
            pnlBody.Location = new Point(0, 46);
            pnlBody.Margin = new Padding(0);
            pnlBody.Name = "pnlBody";
            pnlBody.Padding = new Padding(10, 8, 10, 8);
            pnlBody.Size = new Size(500, 412);
            pnlBody.TabIndex = 1;
            // 
            // tblMain
            // 
            tblMain.ColumnCount = 1;
            tblMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblMain.Controls.Add(grpDiscount, 0, 0);
            tblMain.Controls.Add(grpSummary, 0, 1);
            tblMain.Controls.Add(lblNote, 0, 2);
            tblMain.Dock = DockStyle.Fill;
            tblMain.Location = new Point(10, 8);
            tblMain.Name = "tblMain";
            tblMain.RowCount = 4;
            tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 158F));
            tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 176F));
            tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tblMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblMain.Size = new Size(480, 396);
            tblMain.TabIndex = 0;
            // 
            // grpDiscount
            // 
            grpDiscount.BackColor = Color.FromArgb(235, 245, 235);
            grpDiscount.Controls.Add(tblDiscount);
            grpDiscount.Dock = DockStyle.Fill;
            grpDiscount.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            grpDiscount.ForeColor = Color.FromArgb(14, 82, 52);
            grpDiscount.Location = new Point(3, 3);
            grpDiscount.Name = "grpDiscount";
            grpDiscount.Padding = new Padding(10, 4, 10, 6);
            grpDiscount.Size = new Size(474, 152);
            grpDiscount.TabIndex = 0;
            grpDiscount.TabStop = false;
            grpDiscount.Text = "بيانات الخصم";
            // 
            // tblDiscount
            // 
            tblDiscount.ColumnCount = 2;
            tblDiscount.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tblDiscount.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblDiscount.Controls.Add(lblTypeCaption, 0, 0);
            tblDiscount.Controls.Add(flpTypes, 1, 0);
            tblDiscount.Controls.Add(lblValueCaption, 0, 1);
            tblDiscount.Controls.Add(tblValueRow, 1, 1);
            tblDiscount.Controls.Add(lblRateCaption, 0, 2);
            tblDiscount.Controls.Add(cmbTax, 1, 2);
            tblDiscount.Dock = DockStyle.Fill;
            tblDiscount.Location = new Point(10, 20);
            tblDiscount.Name = "tblDiscount";
            tblDiscount.RowCount = 4;
            tblDiscount.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tblDiscount.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tblDiscount.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tblDiscount.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblDiscount.Size = new Size(454, 126);
            tblDiscount.TabIndex = 0;
            // 
            // lblTypeCaption
            // 
            lblTypeCaption.Dock = DockStyle.Fill;
            lblTypeCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTypeCaption.ForeColor = Color.FromArgb(14, 82, 52);
            lblTypeCaption.Margin = new Padding(0, 0, 8, 0);
            lblTypeCaption.Name = "lblTypeCaption";
            lblTypeCaption.Size = new Size(142, 34);
            lblTypeCaption.TabIndex = 0;
            lblTypeCaption.Text = "نوع الخصم:";
            lblTypeCaption.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // flpTypes
            // 
            flpTypes.Controls.Add(rbAmount);
            flpTypes.Controls.Add(rbPercent);
            flpTypes.Dock = DockStyle.Fill;
            flpTypes.FlowDirection = FlowDirection.RightToLeft;
            flpTypes.Location = new Point(153, 3);
            flpTypes.Name = "flpTypes";
            flpTypes.Size = new Size(298, 28);
            flpTypes.TabIndex = 1;
            flpTypes.WrapContents = false;
            // 
            // rbAmount
            // 
            rbAmount.AutoSize = true;
            rbAmount.BackColor = Color.FromArgb(235, 245, 235);
            rbAmount.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rbAmount.ForeColor = Color.FromArgb(14, 82, 52);
            rbAmount.Location = new Point(214, 3);
            rbAmount.Margin = new Padding(0, 3, 16, 0);
            rbAmount.Name = "rbAmount";
            rbAmount.Size = new Size(58, 21);
            rbAmount.TabIndex = 0;
            rbAmount.Text = "مبلغ";
            rbAmount.UseVisualStyleBackColor = false;
            rbAmount.CheckedChanged += rbDiscountType_CheckedChanged;
            // 
            // rbPercent
            // 
            rbPercent.AutoSize = true;
            rbPercent.BackColor = Color.FromArgb(235, 245, 235);
            rbPercent.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rbPercent.ForeColor = Color.FromArgb(14, 82, 52);
            rbPercent.Location = new Point(128, 3);
            rbPercent.Margin = new Padding(0, 3, 0, 0);
            rbPercent.Name = "rbPercent";
            rbPercent.Size = new Size(86, 21);
            rbPercent.TabIndex = 1;
            rbPercent.Text = "نسبة %";
            rbPercent.UseVisualStyleBackColor = false;
            rbPercent.CheckedChanged += rbDiscountType_CheckedChanged;
            // 
            // lblValueCaption
            // 
            lblValueCaption.Dock = DockStyle.Fill;
            lblValueCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblValueCaption.ForeColor = Color.FromArgb(14, 82, 52);
            lblValueCaption.Margin = new Padding(0, 0, 8, 0);
            lblValueCaption.Name = "lblValueCaption";
            lblValueCaption.Size = new Size(142, 34);
            lblValueCaption.TabIndex = 2;
            lblValueCaption.Text = "قيمة الخصم:";
            lblValueCaption.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // tblValueRow
            // 
            tblValueRow.ColumnCount = 2;
            tblValueRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblValueRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60F));
            tblValueRow.Controls.Add(numDiscount, 0, 0);
            tblValueRow.Controls.Add(lblUnit, 1, 0);
            tblValueRow.Dock = DockStyle.Fill;
            tblValueRow.Location = new Point(153, 40);
            tblValueRow.Name = "tblValueRow";
            tblValueRow.RowCount = 1;
            tblValueRow.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblValueRow.Size = new Size(298, 28);
            tblValueRow.TabIndex = 3;
            // 
            // numDiscount
            // 
            numDiscount.BackColor = Color.White;
            numDiscount.DecimalPlaces = 2;
            numDiscount.Dock = DockStyle.Fill;
            numDiscount.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            numDiscount.Location = new Point(63, 3);
            numDiscount.Maximum = 9999999M;
            numDiscount.Name = "numDiscount";
            numDiscount.Size = new Size(232, 23);
            numDiscount.TabIndex = 0;
            numDiscount.ThousandsSeparator = true;
            numDiscount.ValueChanged += numDiscount_ValueChanged;
            // 
            // lblUnit
            // 
            lblUnit.Dock = DockStyle.Fill;
            lblUnit.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblUnit.ForeColor = Color.FromArgb(14, 82, 52);
            lblUnit.Location = new Point(3, 3);
            lblUnit.Name = "lblUnit";
            lblUnit.Size = new Size(54, 22);
            lblUnit.TabIndex = 1;
            lblUnit.Text = "ريال";
            lblUnit.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblRateCaption
            // 
            lblRateCaption.Dock = DockStyle.Fill;
            lblRateCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblRateCaption.ForeColor = Color.FromArgb(14, 82, 52);
            lblRateCaption.Margin = new Padding(0, 0, 8, 0);
            lblRateCaption.Name = "lblRateCaption";
            lblRateCaption.Size = new Size(142, 34);
            lblRateCaption.TabIndex = 4;
            lblRateCaption.Text = "الضريبة %:";
            lblRateCaption.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cmbTax
            // 
            cmbTax.BackColor = Color.White;
            cmbTax.Dock = DockStyle.Fill;
            cmbTax.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTax.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmbTax.FormattingEnabled = true;
            cmbTax.Location = new Point(153, 77);
            cmbTax.Name = "cmbTax";
            cmbTax.RightToLeft = RightToLeft.Yes;
            cmbTax.Size = new Size(298, 24);
            cmbTax.TabIndex = 5;
            cmbTax.SelectedIndexChanged += cmbTax_SelectedIndexChanged;
            // 
            // grpSummary
            // 
            grpSummary.BackColor = Color.FromArgb(235, 245, 235);
            grpSummary.Controls.Add(tblSummary);
            grpSummary.Dock = DockStyle.Fill;
            grpSummary.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            grpSummary.ForeColor = Color.FromArgb(14, 82, 52);
            grpSummary.Location = new Point(3, 164);
            grpSummary.Name = "grpSummary";
            grpSummary.Padding = new Padding(10, 4, 10, 6);
            grpSummary.Size = new Size(474, 170);
            grpSummary.TabIndex = 1;
            grpSummary.TabStop = false;
            grpSummary.Text = "الإجماليات";
            // 
            // tblSummary
            // 
            tblSummary.ColumnCount = 2;
            tblSummary.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160F));
            tblSummary.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblSummary.Controls.Add(lblSubtotalCaption, 0, 0);
            tblSummary.Controls.Add(lblSubtotalValue, 1, 0);
            tblSummary.Controls.Add(lblDiscountCaption, 0, 1);
            tblSummary.Controls.Add(lblDiscountValue, 1, 1);
            tblSummary.Controls.Add(lblTaxCaption, 0, 2);
            tblSummary.Controls.Add(lblTaxValue, 1, 2);
            tblSummary.Controls.Add(lblNetCaption, 0, 3);
            tblSummary.Controls.Add(lblNetValue, 1, 3);
            tblSummary.Dock = DockStyle.Fill;
            tblSummary.Location = new Point(10, 20);
            tblSummary.Name = "tblSummary";
            tblSummary.RowCount = 5;
            tblSummary.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tblSummary.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tblSummary.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tblSummary.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            tblSummary.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblSummary.Size = new Size(454, 144);
            tblSummary.TabIndex = 0;
            // 
            // lblSubtotalCaption
            // 
            lblSubtotalCaption.Dock = DockStyle.Fill;
            lblSubtotalCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSubtotalCaption.ForeColor = Color.FromArgb(14, 82, 52);
            lblSubtotalCaption.Margin = new Padding(0, 0, 8, 0);
            lblSubtotalCaption.Name = "lblSubtotalCaption";
            lblSubtotalCaption.Size = new Size(152, 30);
            lblSubtotalCaption.TabIndex = 0;
            lblSubtotalCaption.Text = "المجموع الفرعي:";
            lblSubtotalCaption.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblSubtotalValue
            // 
            lblSubtotalValue.Dock = DockStyle.Fill;
            lblSubtotalValue.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSubtotalValue.ForeColor = Color.FromArgb(14, 82, 52);
            lblSubtotalValue.Location = new Point(163, 0);
            lblSubtotalValue.Name = "lblSubtotalValue";
            lblSubtotalValue.Size = new Size(288, 30);
            lblSubtotalValue.TabIndex = 1;
            lblSubtotalValue.Text = "0.00";
            lblSubtotalValue.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblDiscountCaption
            // 
            lblDiscountCaption.Dock = DockStyle.Fill;
            lblDiscountCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDiscountCaption.ForeColor = Color.FromArgb(14, 82, 52);
            lblDiscountCaption.Margin = new Padding(0, 0, 8, 0);
            lblDiscountCaption.Name = "lblDiscountCaption";
            lblDiscountCaption.Size = new Size(152, 30);
            lblDiscountCaption.TabIndex = 2;
            lblDiscountCaption.Text = "الخصم:";
            lblDiscountCaption.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblDiscountValue
            // 
            lblDiscountValue.Dock = DockStyle.Fill;
            lblDiscountValue.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDiscountValue.ForeColor = Color.FromArgb(14, 82, 52);
            lblDiscountValue.Location = new Point(163, 30);
            lblDiscountValue.Name = "lblDiscountValue";
            lblDiscountValue.Size = new Size(288, 30);
            lblDiscountValue.TabIndex = 3;
            lblDiscountValue.Text = "0.00";
            lblDiscountValue.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblTaxCaption
            // 
            lblTaxCaption.Dock = DockStyle.Fill;
            lblTaxCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTaxCaption.ForeColor = Color.FromArgb(14, 82, 52);
            lblTaxCaption.Margin = new Padding(0, 0, 8, 0);
            lblTaxCaption.Name = "lblTaxCaption";
            lblTaxCaption.Size = new Size(152, 30);
            lblTaxCaption.TabIndex = 4;
            lblTaxCaption.Text = "الضريبة:";
            lblTaxCaption.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTaxValue
            // 
            lblTaxValue.Dock = DockStyle.Fill;
            lblTaxValue.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTaxValue.ForeColor = Color.FromArgb(14, 82, 52);
            lblTaxValue.Location = new Point(163, 60);
            lblTaxValue.Name = "lblTaxValue";
            lblTaxValue.Size = new Size(288, 30);
            lblTaxValue.TabIndex = 5;
            lblTaxValue.Text = "0.00";
            lblTaxValue.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblNetCaption
            // 
            lblNetCaption.Dock = DockStyle.Fill;
            lblNetCaption.Font = new Font("Tahoma", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNetCaption.ForeColor = Color.FromArgb(14, 82, 52);
            lblNetCaption.Margin = new Padding(0, 0, 8, 0);
            lblNetCaption.Name = "lblNetCaption";
            lblNetCaption.Size = new Size(152, 38);
            lblNetCaption.TabIndex = 6;
            lblNetCaption.Text = "الصافي النهائي:";
            lblNetCaption.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblNetValue
            // 
            lblNetValue.BackColor = Color.FromArgb(255, 255, 180);
            lblNetValue.Dock = DockStyle.Fill;
            lblNetValue.Font = new Font("Tahoma", 13F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNetValue.ForeColor = Color.FromArgb(14, 82, 52);
            lblNetValue.Location = new Point(163, 90);
            lblNetValue.Name = "lblNetValue";
            lblNetValue.Size = new Size(288, 38);
            lblNetValue.TabIndex = 7;
            lblNetValue.Text = "0.00";
            lblNetValue.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblNote
            // 
            lblNote.BackColor = Color.FromArgb(235, 240, 235);
            lblNote.Dock = DockStyle.Fill;
            lblNote.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblNote.ForeColor = Color.FromArgb(14, 82, 52);
            lblNote.Location = new Point(3, 337);
            lblNote.Name = "lblNote";
            lblNote.Size = new Size(474, 28);
            lblNote.TabIndex = 2;
            lblNote.Text = "الضريبة تُحسب على المجموع بعد الخصم — (وضع تجريبي: بيانات وهمية في الذاكرة)";
            lblNote.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlFooter
            // 
            pnlFooter.BackColor = Color.FromArgb(235, 240, 235);
            pnlFooter.Controls.Add(flpFooter);
            pnlFooter.Controls.Add(lblFooterLine);
            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Location = new Point(0, 458);
            pnlFooter.Margin = new Padding(0);
            pnlFooter.Name = "pnlFooter";
            pnlFooter.Size = new Size(500, 62);
            pnlFooter.TabIndex = 2;
            // 
            // lblFooterLine
            // 
            lblFooterLine.BackColor = Color.FromArgb(205, 225, 205);
            lblFooterLine.Dock = DockStyle.Top;
            lblFooterLine.Location = new Point(0, 0);
            lblFooterLine.Name = "lblFooterLine";
            lblFooterLine.Size = new Size(500, 2);
            lblFooterLine.TabIndex = 0;
            // 
            // flpFooter
            // 
            flpFooter.BackColor = Color.FromArgb(235, 240, 235);
            flpFooter.Controls.Add(btnOk);
            flpFooter.Controls.Add(btnCancel);
            flpFooter.Dock = DockStyle.Fill;
            flpFooter.FlowDirection = FlowDirection.RightToLeft;
            flpFooter.Location = new Point(0, 2);
            flpFooter.Name = "flpFooter";
            flpFooter.Padding = new Padding(10, 8, 10, 6);
            flpFooter.Size = new Size(500, 60);
            flpFooter.TabIndex = 1;
            flpFooter.WrapContents = false;
            // 
            // btnOk
            // 
            btnOk.BackColor = Color.FromArgb(25, 135, 84);
            btnOk.FlatAppearance.BorderSize = 0;
            btnOk.FlatStyle = FlatStyle.Flat;
            btnOk.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnOk.ForeColor = Color.White;
            btnOk.Location = new Point(340, 8);
            btnOk.Margin = new Padding(0, 0, 8, 0);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size(140, 40);
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
            btnCancel.Location = new Point(240, 8);
            btnCancel.Margin = new Padding(0);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(92, 40);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "إلغاء";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // frmPurchaseDiscount
            // 
            AcceptButton = btnOk;
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.FromArgb(170, 220, 170);
            CancelButton = btnCancel;
            ClientSize = new Size(500, 520);
            Controls.Add(pnlBody);
            Controls.Add(pnlFooter);
            Controls.Add(pnlTitle);
            Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmPurchaseDiscount";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterParent;
            Text = "خصم المشتريات";
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ((System.ComponentModel.ISupportInitialize)numDiscount).EndInit();
            pnlTitle.ResumeLayout(false);
            pnlBody.ResumeLayout(false);
            tblMain.ResumeLayout(false);
            grpDiscount.ResumeLayout(false);
            tblDiscount.ResumeLayout(false);
            flpTypes.ResumeLayout(false);
            flpTypes.PerformLayout();
            tblValueRow.ResumeLayout(false);
            grpSummary.ResumeLayout(false);
            tblSummary.ResumeLayout(false);
            pnlFooter.ResumeLayout(false);
            flpFooter.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel pnlTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlBody;
        private System.Windows.Forms.TableLayoutPanel tblMain;
        private System.Windows.Forms.GroupBox grpDiscount;
        private System.Windows.Forms.TableLayoutPanel tblDiscount;
        private System.Windows.Forms.Label lblTypeCaption;
        private System.Windows.Forms.FlowLayoutPanel flpTypes;
        private System.Windows.Forms.RadioButton rbAmount, rbPercent;
        private System.Windows.Forms.Label lblValueCaption;
        private System.Windows.Forms.TableLayoutPanel tblValueRow;
        private System.Windows.Forms.NumericUpDown numDiscount;
        private System.Windows.Forms.Label lblUnit;
        private System.Windows.Forms.Label lblRateCaption;
        private System.Windows.Forms.ComboBox cmbTax;
        private System.Windows.Forms.GroupBox grpSummary;
        private System.Windows.Forms.TableLayoutPanel tblSummary;
        private System.Windows.Forms.Label lblSubtotalCaption, lblSubtotalValue;
        private System.Windows.Forms.Label lblDiscountCaption, lblDiscountValue;
        private System.Windows.Forms.Label lblTaxCaption, lblTaxValue;
        private System.Windows.Forms.Label lblNetCaption, lblNetValue;
        private System.Windows.Forms.Label lblNote;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Label lblFooterLine;
        private System.Windows.Forms.FlowLayoutPanel flpFooter;
        private System.Windows.Forms.Button btnOk, btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ToolTip tipTafqeet;
    }
}
