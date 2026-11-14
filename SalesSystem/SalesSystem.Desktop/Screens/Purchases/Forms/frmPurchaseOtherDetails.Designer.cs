namespace SalesSystem.Desktop.Screens.Purchases.Forms
{
    partial class frmPurchaseOtherDetails
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
            toolTip = new ToolTip(components);
            tipTafqeet = new ToolTip(components);
            pnlTitle = new Panel();
            lblTitle = new Label();
            pnlBody = new Panel();
            tblMain = new TableLayoutPanel();
            gbCosts = new GroupBox();
            tblCosts = new TableLayoutPanel();
            lblShipping = new Label();
            numShipping = new NumericUpDown();
            lblCustoms = new Label();
            numCustoms = new NumericUpDown();
            lblOther = new Label();
            tblOtherRow = new TableLayoutPanel();
            numOther = new NumericUpDown();
            lblStatementCaption = new Label();
            txtStatement = new TextBox();
            gbTotal = new GroupBox();
            tblTotal = new TableLayoutPanel();
            lblTotalCaption = new Label();
            lblTotalValue = new Label();
            gbNotes = new GroupBox();
            txtNotes = new TextBox();
            lblNote = new Label();
            pnlFooter = new Panel();
            lblFooterLine = new Label();
            flpFooter = new FlowLayoutPanel();
            btnOk = new Button();
            btnCancel = new Button();
            pnlTitle.SuspendLayout();
            pnlBody.SuspendLayout();
            tblMain.SuspendLayout();
            gbCosts.SuspendLayout();
            tblCosts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numShipping).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numCustoms).BeginInit();
            tblOtherRow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numOther).BeginInit();
            gbTotal.SuspendLayout();
            tblTotal.SuspendLayout();
            gbNotes.SuspendLayout();
            pnlFooter.SuspendLayout();
            flpFooter.SuspendLayout();
            SuspendLayout();
            // 
            // pnlTitle
            // 
            pnlTitle.BackColor = Color.FromArgb(14, 165, 103);
            pnlTitle.Controls.Add(lblTitle);
            pnlTitle.Dock = DockStyle.Top;
            pnlTitle.Location = new Point(0, 0);
            pnlTitle.Margin = new Padding(0);
            pnlTitle.Name = "pnlTitle";
            pnlTitle.Size = new Size(540, 46);
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
            lblTitle.Size = new Size(540, 46);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "تفاصيل وتكاليف إضافية";
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
            pnlBody.Size = new Size(540, 492);
            pnlBody.TabIndex = 1;
            // 
            // tblMain
            // 
            tblMain.ColumnCount = 1;
            tblMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblMain.Controls.Add(gbCosts, 0, 0);
            tblMain.Controls.Add(gbTotal, 0, 1);
            tblMain.Controls.Add(gbNotes, 0, 2);
            tblMain.Controls.Add(lblNote, 0, 3);
            tblMain.Dock = DockStyle.Fill;
            tblMain.Location = new Point(10, 8);
            tblMain.Name = "tblMain";
            tblMain.RowCount = 5;
            tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 156F));
            tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 58F));
            tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 180F));
            tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
            tblMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblMain.Size = new Size(520, 476);
            tblMain.TabIndex = 0;
            // 
            // gbCosts
            // 
            gbCosts.BackColor = Color.FromArgb(235, 245, 235);
            gbCosts.Controls.Add(tblCosts);
            gbCosts.Dock = DockStyle.Fill;
            gbCosts.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            gbCosts.ForeColor = Color.FromArgb(14, 82, 52);
            gbCosts.Location = new Point(3, 3);
            gbCosts.Name = "gbCosts";
            gbCosts.Padding = new Padding(10, 4, 10, 6);
            gbCosts.Size = new Size(514, 150);
            gbCosts.TabIndex = 0;
            gbCosts.TabStop = false;
            gbCosts.Text = "التكاليف الإضافية";
            // 
            // tblCosts
            // 
            tblCosts.ColumnCount = 2;
            tblCosts.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tblCosts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblCosts.Controls.Add(lblShipping, 0, 0);
            tblCosts.Controls.Add(numShipping, 1, 0);
            tblCosts.Controls.Add(lblCustoms, 0, 1);
            tblCosts.Controls.Add(numCustoms, 1, 1);
            tblCosts.Controls.Add(lblOther, 0, 2);
            tblCosts.Controls.Add(tblOtherRow, 1, 2);
            tblCosts.Dock = DockStyle.Fill;
            tblCosts.Location = new Point(10, 20);
            tblCosts.Name = "tblCosts";
            tblCosts.RowCount = 4;
            tblCosts.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tblCosts.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tblCosts.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tblCosts.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblCosts.Size = new Size(494, 124);
            tblCosts.TabIndex = 0;
            // 
            // lblShipping
            // 
            lblShipping.Dock = DockStyle.Fill;
            lblShipping.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblShipping.ForeColor = Color.FromArgb(14, 82, 52);
            lblShipping.Margin = new Padding(0, 0, 8, 0);
            lblShipping.Name = "lblShipping";
            lblShipping.Size = new Size(142, 34);
            lblShipping.TabIndex = 0;
            lblShipping.Text = "تكلفة الشحن:";
            lblShipping.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // numShipping
            // 
            numShipping.BackColor = Color.White;
            numShipping.DecimalPlaces = 2;
            numShipping.Dock = DockStyle.Fill;
            numShipping.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            numShipping.Location = new Point(153, 3);
            numShipping.Maximum = 9999999M;
            numShipping.Name = "numShipping";
            numShipping.Size = new Size(338, 23);
            numShipping.TabIndex = 1;
            numShipping.ThousandsSeparator = true;
            numShipping.ValueChanged += numCost_ValueChanged;
            // 
            // lblCustoms
            // 
            lblCustoms.Dock = DockStyle.Fill;
            lblCustoms.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCustoms.ForeColor = Color.FromArgb(14, 82, 52);
            lblCustoms.Margin = new Padding(0, 0, 8, 0);
            lblCustoms.Name = "lblCustoms";
            lblCustoms.Size = new Size(142, 34);
            lblCustoms.TabIndex = 2;
            lblCustoms.Text = "رسوم الجمارك:";
            lblCustoms.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // numCustoms
            // 
            numCustoms.BackColor = Color.White;
            numCustoms.DecimalPlaces = 2;
            numCustoms.Dock = DockStyle.Fill;
            numCustoms.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            numCustoms.Location = new Point(153, 37);
            numCustoms.Maximum = 9999999M;
            numCustoms.Name = "numCustoms";
            numCustoms.Size = new Size(338, 23);
            numCustoms.TabIndex = 3;
            numCustoms.ThousandsSeparator = true;
            numCustoms.ValueChanged += numCost_ValueChanged;
            // 
            // lblOther
            // 
            lblOther.Dock = DockStyle.Fill;
            lblOther.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblOther.ForeColor = Color.FromArgb(14, 82, 52);
            lblOther.Margin = new Padding(0, 0, 8, 0);
            lblOther.Name = "lblOther";
            lblOther.Size = new Size(142, 34);
            lblOther.TabIndex = 4;
            lblOther.Text = "تكاليف أخرى:";
            lblOther.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // tblOtherRow
            // 
            tblOtherRow.ColumnCount = 3;
            tblOtherRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
            tblOtherRow.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            tblOtherRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblOtherRow.Controls.Add(numOther, 0, 0);
            tblOtherRow.Controls.Add(lblStatementCaption, 1, 0);
            tblOtherRow.Controls.Add(txtStatement, 2, 0);
            tblOtherRow.Dock = DockStyle.Fill;
            tblOtherRow.Location = new Point(153, 71);
            tblOtherRow.Name = "tblOtherRow";
            tblOtherRow.RowCount = 1;
            tblOtherRow.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblOtherRow.Size = new Size(338, 28);
            tblOtherRow.TabIndex = 5;
            // 
            // numOther
            // 
            numOther.BackColor = Color.White;
            numOther.DecimalPlaces = 2;
            numOther.Dock = DockStyle.Fill;
            numOther.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            numOther.Location = new Point(3, 3);
            numOther.Maximum = 9999999M;
            numOther.Name = "numOther";
            numOther.Size = new Size(124, 23);
            numOther.TabIndex = 0;
            numOther.ThousandsSeparator = true;
            numOther.ValueChanged += numCost_ValueChanged;
            // 
            // lblStatementCaption
            // 
            lblStatementCaption.Dock = DockStyle.Fill;
            lblStatementCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblStatementCaption.ForeColor = Color.FromArgb(14, 82, 52);
            lblStatementCaption.Margin = new Padding(8, 0, 8, 0);
            lblStatementCaption.Name = "lblStatementCaption";
            lblStatementCaption.Size = new Size(44, 28);
            lblStatementCaption.TabIndex = 1;
            lblStatementCaption.Text = "بيان:";
            lblStatementCaption.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtStatement
            // 
            txtStatement.BackColor = Color.White;
            txtStatement.BorderStyle = BorderStyle.FixedSingle;
            txtStatement.Dock = DockStyle.Fill;
            txtStatement.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtStatement.Location = new Point(133, 3);
            txtStatement.Name = "txtStatement";
            txtStatement.PlaceholderText = "مثال: تغليف، تحميل، رسوم تخليص...";
            txtStatement.Size = new Size(202, 23);
            txtStatement.TabIndex = 2;
            // 
            // gbTotal
            // 
            gbTotal.BackColor = Color.FromArgb(235, 245, 235);
            gbTotal.Controls.Add(tblTotal);
            gbTotal.Dock = DockStyle.Fill;
            gbTotal.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            gbTotal.ForeColor = Color.FromArgb(14, 82, 52);
            gbTotal.Location = new Point(3, 159);
            gbTotal.Name = "gbTotal";
            gbTotal.Padding = new Padding(10, 4, 10, 4);
            gbTotal.Size = new Size(514, 52);
            gbTotal.TabIndex = 1;
            gbTotal.TabStop = false;
            gbTotal.Text = "الإجماليات";
            // 
            // tblTotal
            // 
            tblTotal.ColumnCount = 2;
            tblTotal.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 190F));
            tblTotal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblTotal.Controls.Add(lblTotalCaption, 0, 0);
            tblTotal.Controls.Add(lblTotalValue, 1, 0);
            tblTotal.Dock = DockStyle.Fill;
            tblTotal.Location = new Point(10, 20);
            tblTotal.Name = "tblTotal";
            tblTotal.RowCount = 1;
            tblTotal.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblTotal.Size = new Size(494, 28);
            tblTotal.TabIndex = 0;
            // 
            // lblTotalCaption
            // 
            lblTotalCaption.Dock = DockStyle.Fill;
            lblTotalCaption.Font = new Font("Tahoma", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalCaption.ForeColor = Color.FromArgb(14, 82, 52);
            lblTotalCaption.Margin = new Padding(0, 0, 8, 0);
            lblTotalCaption.Name = "lblTotalCaption";
            lblTotalCaption.Size = new Size(182, 28);
            lblTotalCaption.TabIndex = 0;
            lblTotalCaption.Text = "إجمالي التكاليف الإضافية:";
            lblTotalCaption.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTotalValue
            // 
            lblTotalValue.BackColor = Color.FromArgb(255, 255, 180);
            lblTotalValue.Dock = DockStyle.Fill;
            lblTotalValue.Font = new Font("Tahoma", 13F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalValue.ForeColor = Color.FromArgb(14, 82, 52);
            lblTotalValue.Location = new Point(193, 0);
            lblTotalValue.Name = "lblTotalValue";
            lblTotalValue.Size = new Size(298, 28);
            lblTotalValue.TabIndex = 1;
            lblTotalValue.Text = "0.00";
            lblTotalValue.TextAlign = ContentAlignment.MiddleRight;
            // 
            // gbNotes
            // 
            gbNotes.BackColor = Color.FromArgb(235, 245, 235);
            gbNotes.Controls.Add(txtNotes);
            gbNotes.Dock = DockStyle.Fill;
            gbNotes.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            gbNotes.ForeColor = Color.FromArgb(14, 82, 52);
            gbNotes.Location = new Point(3, 217);
            gbNotes.Name = "gbNotes";
            gbNotes.Padding = new Padding(10, 4, 10, 8);
            gbNotes.Size = new Size(514, 174);
            gbNotes.TabIndex = 2;
            gbNotes.TabStop = false;
            gbNotes.Text = "ملاحظات فاتورة الشراء";
            // 
            // txtNotes
            // 
            txtNotes.AcceptsReturn = true;
            txtNotes.BackColor = Color.White;
            txtNotes.BorderStyle = BorderStyle.FixedSingle;
            txtNotes.Dock = DockStyle.Fill;
            txtNotes.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtNotes.Location = new Point(10, 20);
            txtNotes.Multiline = true;
            txtNotes.Name = "txtNotes";
            txtNotes.PlaceholderText = "اكتب أي ملاحظات إضافية على فاتورة الشراء...";
            txtNotes.ScrollBars = ScrollBars.Vertical;
            txtNotes.Size = new Size(494, 146);
            txtNotes.TabIndex = 0;
            // 
            // lblNote
            // 
            lblNote.BackColor = Color.FromArgb(235, 240, 235);
            lblNote.Dock = DockStyle.Fill;
            lblNote.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblNote.ForeColor = Color.FromArgb(14, 82, 52);
            lblNote.Location = new Point(3, 394);
            lblNote.Name = "lblNote";
            lblNote.Size = new Size(514, 20);
            lblNote.TabIndex = 3;
            lblNote.Text = "التكاليف الإضافية توزّع تلقائياً على بنود الفاتورة (Landed Cost)";
            lblNote.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlFooter
            // 
            pnlFooter.BackColor = Color.FromArgb(235, 240, 235);
            pnlFooter.Controls.Add(flpFooter);
            pnlFooter.Controls.Add(lblFooterLine);
            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Location = new Point(0, 538);
            pnlFooter.Margin = new Padding(0);
            pnlFooter.Name = "pnlFooter";
            pnlFooter.Size = new Size(540, 62);
            pnlFooter.TabIndex = 2;
            // 
            // lblFooterLine
            // 
            lblFooterLine.BackColor = Color.FromArgb(205, 225, 205);
            lblFooterLine.Dock = DockStyle.Top;
            lblFooterLine.Location = new Point(0, 0);
            lblFooterLine.Name = "lblFooterLine";
            lblFooterLine.Size = new Size(540, 2);
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
            flpFooter.Size = new Size(540, 60);
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
            btnOk.Location = new Point(380, 8);
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
            btnCancel.Location = new Point(280, 8);
            btnCancel.Margin = new Padding(0);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(92, 40);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "إلغاء";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // frmPurchaseOtherDetails
            // 
            AcceptButton = btnOk;
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.FromArgb(170, 220, 170);
            CancelButton = btnCancel;
            ClientSize = new Size(540, 600);
            Controls.Add(pnlBody);
            Controls.Add(pnlFooter);
            Controls.Add(pnlTitle);
            Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmPurchaseOtherDetails";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterParent;
            Text = "تفاصيل وتكاليف إضافية";
            pnlTitle.ResumeLayout(false);
            pnlBody.ResumeLayout(false);
            tblMain.ResumeLayout(false);
            gbCosts.ResumeLayout(false);
            tblCosts.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)numShipping).EndInit();
            ((System.ComponentModel.ISupportInitialize)numCustoms).EndInit();
            tblOtherRow.ResumeLayout(false);
            tblOtherRow.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numOther).EndInit();
            gbTotal.ResumeLayout(false);
            tblTotal.ResumeLayout(false);
            gbNotes.ResumeLayout(false);
            gbNotes.PerformLayout();
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
        private System.Windows.Forms.GroupBox gbCosts;
        private System.Windows.Forms.TableLayoutPanel tblCosts;
        private System.Windows.Forms.Label lblShipping;
        private System.Windows.Forms.NumericUpDown numShipping;
        private System.Windows.Forms.Label lblCustoms;
        private System.Windows.Forms.NumericUpDown numCustoms;
        private System.Windows.Forms.Label lblOther;
        private System.Windows.Forms.TableLayoutPanel tblOtherRow;
        private System.Windows.Forms.NumericUpDown numOther;
        private System.Windows.Forms.Label lblStatementCaption;
        private System.Windows.Forms.TextBox txtStatement;
        private System.Windows.Forms.GroupBox gbTotal;
        private System.Windows.Forms.TableLayoutPanel tblTotal;
        private System.Windows.Forms.Label lblTotalCaption;
        private System.Windows.Forms.Label lblTotalValue;
        private System.Windows.Forms.GroupBox gbNotes;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.Label lblNote;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Label lblFooterLine;
        private System.Windows.Forms.FlowLayoutPanel flpFooter;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ToolTip tipTafqeet;
    }
}
