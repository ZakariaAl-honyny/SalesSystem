namespace SalesSystem.Desktop.Screens.Sales.Forms
{
    partial class frmSalesInlineGrid
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
            cmsLines = new ContextMenuStrip(components);
            miEditQty = new ToolStripMenuItem();
            miAddDiscount = new ToolStripMenuItem();
            miDeleteLine = new ToolStripMenuItem();
            miInsertBelow = new ToolStripMenuItem();
            pnlTitle = new Panel();
            tblTitle = new TableLayoutPanel();
            lblTitle = new Label();
            lblMode = new Label();
            pnlHeader = new Panel();
            tblHeader = new TableLayoutPanel();
            lblInvoiceNo = new Label();
            lblCustomer = new Label();
            lblPaymentType = new Label();
            lblNotes = new Label();
            txtInvoiceNo = new TextBox();
            tblCustomer = new TableLayoutPanel();
            cbCustomer = new ComboBox();
            lblBalance = new Label();
            cbPaymentType = new ComboBox();
            txtNotes = new TextBox();
            pnlFooter = new Panel();
            tblFooter = new TableLayoutPanel();
            tblSummary = new TableLayoutPanel();
            lblLinesCaption = new Label();
            lblGrossCaption = new Label();
            lblDiscountCaption = new Label();
            lblNetCaption = new Label();
            lblLinesValue = new Label();
            lblGrossValue = new Label();
            lblDiscountValue = new Label();
            lblNetValue = new Label();
            flpButtons = new FlowLayoutPanel();
            btnSave = new Button();
            btnClose = new Button();
            dgvLines = new DataGridView();
            cmsLines.SuspendLayout();
            pnlTitle.SuspendLayout();
            tblTitle.SuspendLayout();
            pnlHeader.SuspendLayout();
            tblHeader.SuspendLayout();
            tblCustomer.SuspendLayout();
            pnlFooter.SuspendLayout();
            tblFooter.SuspendLayout();
            tblSummary.SuspendLayout();
            flpButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvLines).BeginInit();
            SuspendLayout();
            // 
            // cmsLines
            // 
            cmsLines.Items.AddRange(new ToolStripItem[] { miEditQty, miAddDiscount, miDeleteLine, miInsertBelow });
            cmsLines.Name = "cmsLines";
            cmsLines.Opening += cmsLines_Opening;
            // 
            // miEditQty
            // 
            miEditQty.Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            miEditQty.Name = "miEditQty";
            miEditQty.ShortcutKeyDisplayString = "Enter";
            miEditQty.Size = new Size(210, 24);
            miEditQty.Text = "تعديل كمية السطر (Enter)";
            miEditQty.Click += miEditQty_Click;
            // 
            // miAddDiscount
            // 
            miAddDiscount.Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            miAddDiscount.Name = "miAddDiscount";
            miAddDiscount.Size = new Size(210, 24);
            miAddDiscount.Text = "إضافة خصم مباشر";
            miAddDiscount.Click += miAddDiscount_Click;
            // 
            // miDeleteLine
            // 
            miDeleteLine.Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            miDeleteLine.Name = "miDeleteLine";
            miDeleteLine.ShortcutKeyDisplayString = "Delete";
            miDeleteLine.Size = new Size(210, 24);
            miDeleteLine.Text = "حذف السطر (Delete)";
            miDeleteLine.Click += miDeleteLine_Click;
            // 
            // miInsertBelow
            // 
            miInsertBelow.Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            miInsertBelow.Name = "miInsertBelow";
            miInsertBelow.Size = new Size(210, 24);
            miInsertBelow.Text = "إدراج سطر جديد أسفل";
            miInsertBelow.Click += miInsertBelow_Click;
            // 
            // pnlTitle
            // 
            pnlTitle.BackColor = Color.FromArgb(33, 43, 54);
            pnlTitle.Controls.Add(tblTitle);
            pnlTitle.Dock = DockStyle.Top;
            pnlTitle.Height = 56;
            // 
            // tblTitle
            // 
            tblTitle.ColumnCount = 2;
            tblTitle.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblTitle.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 230F));
            tblTitle.Controls.Add(lblTitle, 0, 0);
            tblTitle.Controls.Add(lblMode, 1, 0);
            tblTitle.Dock = DockStyle.Fill;
            tblTitle.Padding = new Padding(16, 0, 16, 0);
            tblTitle.RowCount = 1;
            tblTitle.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            // 
            // lblTitle
            // 
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.Font = new Font("Tahoma", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.ForeColor = Color.White;
            lblTitle.Text = "تعديل داخلي — فاتورة بيع";
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblMode
            // 
            lblMode.Dock = DockStyle.Fill;
            lblMode.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblMode.ForeColor = Color.FromArgb(150, 170, 190);
            lblMode.Text = "وضع تجريبي (Mock) — بدون خادم";
            lblMode.TextAlign = ContentAlignment.MiddleRight;
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(248, 249, 250);
            pnlHeader.Controls.Add(tblHeader);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 88;
            pnlHeader.Padding = new Padding(12, 8, 12, 8);
            // 
            // tblHeader
            // 
            tblHeader.ColumnCount = 4;
            tblHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
            tblHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 340F));
            tblHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tblHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblHeader.Controls.Add(lblInvoiceNo, 0, 0);
            tblHeader.Controls.Add(lblCustomer, 1, 0);
            tblHeader.Controls.Add(lblPaymentType, 2, 0);
            tblHeader.Controls.Add(lblNotes, 3, 0);
            tblHeader.Controls.Add(txtInvoiceNo, 0, 1);
            tblHeader.Controls.Add(tblCustomer, 1, 1);
            tblHeader.Controls.Add(cbPaymentType, 2, 1);
            tblHeader.Controls.Add(txtNotes, 3, 1);
            tblHeader.Dock = DockStyle.Fill;
            tblHeader.RowCount = 2;
            tblHeader.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
            tblHeader.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            // 
            // lblInvoiceNo
            // 
            lblInvoiceNo.Dock = DockStyle.Fill;
            lblInvoiceNo.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblInvoiceNo.ForeColor = Color.FromArgb(108, 117, 125);
            lblInvoiceNo.Text = "رقم الفاتورة:";
            lblInvoiceNo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblCustomer
            // 
            lblCustomer.Dock = DockStyle.Fill;
            lblCustomer.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCustomer.ForeColor = Color.FromArgb(108, 117, 125);
            lblCustomer.Text = "العميل:";
            lblCustomer.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblPaymentType
            // 
            lblPaymentType.Dock = DockStyle.Fill;
            lblPaymentType.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPaymentType.ForeColor = Color.FromArgb(108, 117, 125);
            lblPaymentType.Text = "نوع الدفع:";
            lblPaymentType.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblNotes
            // 
            lblNotes.Dock = DockStyle.Fill;
            lblNotes.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNotes.ForeColor = Color.FromArgb(108, 117, 125);
            lblNotes.Text = "ملاحظات الفاتورة:";
            lblNotes.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtInvoiceNo
            // 
            txtInvoiceNo.BackColor = Color.FromArgb(233, 236, 239);
            txtInvoiceNo.Dock = DockStyle.Fill;
            txtInvoiceNo.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtInvoiceNo.ReadOnly = true;
            txtInvoiceNo.Text = "1024";
            txtInvoiceNo.TextAlign = HorizontalAlignment.Center;
            // 
            // tblCustomer
            // 
            tblCustomer.ColumnCount = 2;
            tblCustomer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblCustomer.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160F));
            tblCustomer.Controls.Add(cbCustomer, 0, 0);
            tblCustomer.Controls.Add(lblBalance, 1, 0);
            tblCustomer.Dock = DockStyle.Fill;
            tblCustomer.RowCount = 1;
            tblCustomer.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            // 
            // cbCustomer
            // 
            cbCustomer.Dock = DockStyle.Fill;
            cbCustomer.DropDownStyle = ComboBoxStyle.DropDownList;
            cbCustomer.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cbCustomer.FormattingEnabled = true;
            cbCustomer.SelectedIndexChanged += cbCustomer_SelectedIndexChanged;
            // 
            // lblBalance
            // 
            lblBalance.BackColor = Color.FromArgb(233, 236, 239);
            lblBalance.Dock = DockStyle.Fill;
            lblBalance.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblBalance.ForeColor = Color.FromArgb(80, 90, 100);
            lblBalance.Margin = new Padding(10, 0, 0, 0);
            lblBalance.Text = "الرصيد: 0.00";
            lblBalance.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // cbPaymentType
            // 
            cbPaymentType.Dock = DockStyle.Fill;
            cbPaymentType.DropDownStyle = ComboBoxStyle.DropDownList;
            cbPaymentType.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cbPaymentType.FormattingEnabled = true;
            cbPaymentType.Items.AddRange(new object[] { "نقدي", "آجل" });
            // 
            // txtNotes
            // 
            txtNotes.Dock = DockStyle.Fill;
            txtNotes.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtNotes.PlaceholderText = "ملاحظات عامة على الفاتورة...";
            // 
            // pnlFooter
            // 
            pnlFooter.BackColor = Color.FromArgb(248, 249, 250);
            pnlFooter.Controls.Add(tblFooter);
            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Height = 112;
            pnlFooter.Padding = new Padding(12, 8, 12, 8);
            // 
            // tblFooter
            // 
            tblFooter.ColumnCount = 1;
            tblFooter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblFooter.Controls.Add(tblSummary, 0, 0);
            tblFooter.Controls.Add(flpButtons, 0, 1);
            tblFooter.Dock = DockStyle.Fill;
            tblFooter.RowCount = 2;
            tblFooter.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
            tblFooter.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            // 
            // tblSummary
            // 
            tblSummary.ColumnCount = 4;
            tblSummary.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tblSummary.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tblSummary.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tblSummary.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tblSummary.Controls.Add(lblLinesCaption, 0, 0);
            tblSummary.Controls.Add(lblGrossCaption, 1, 0);
            tblSummary.Controls.Add(lblDiscountCaption, 2, 0);
            tblSummary.Controls.Add(lblNetCaption, 3, 0);
            tblSummary.Controls.Add(lblLinesValue, 0, 1);
            tblSummary.Controls.Add(lblGrossValue, 1, 1);
            tblSummary.Controls.Add(lblDiscountValue, 2, 1);
            tblSummary.Controls.Add(lblNetValue, 3, 1);
            tblSummary.Dock = DockStyle.Fill;
            tblSummary.RowCount = 2;
            tblSummary.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tblSummary.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            // 
            // lblLinesCaption
            // 
            lblLinesCaption.Dock = DockStyle.Fill;
            lblLinesCaption.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblLinesCaption.ForeColor = Color.FromArgb(108, 117, 125);
            lblLinesCaption.Text = "عدد الأسطر";
            lblLinesCaption.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblGrossCaption
            // 
            lblGrossCaption.Dock = DockStyle.Fill;
            lblGrossCaption.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblGrossCaption.ForeColor = Color.FromArgb(108, 117, 125);
            lblGrossCaption.Text = "الإجمالي المبدئي";
            lblGrossCaption.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblDiscountCaption
            // 
            lblDiscountCaption.Dock = DockStyle.Fill;
            lblDiscountCaption.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblDiscountCaption.ForeColor = Color.FromArgb(108, 117, 125);
            lblDiscountCaption.Text = "إجمالي الخصم";
            lblDiscountCaption.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblNetCaption
            // 
            lblNetCaption.Dock = DockStyle.Fill;
            lblNetCaption.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblNetCaption.ForeColor = Color.FromArgb(108, 117, 125);
            lblNetCaption.Text = "الصافي النهائي";
            lblNetCaption.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblLinesValue
            // 
            lblLinesValue.Dock = DockStyle.Fill;
            lblLinesValue.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblLinesValue.ForeColor = Color.FromArgb(33, 43, 54);
            lblLinesValue.Text = "0";
            lblLinesValue.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblGrossValue
            // 
            lblGrossValue.Dock = DockStyle.Fill;
            lblGrossValue.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblGrossValue.ForeColor = Color.FromArgb(33, 43, 54);
            lblGrossValue.Text = "0.00";
            lblGrossValue.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblDiscountValue
            // 
            lblDiscountValue.Dock = DockStyle.Fill;
            lblDiscountValue.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDiscountValue.ForeColor = Color.FromArgb(220, 53, 69);
            lblDiscountValue.Text = "0.00";
            lblDiscountValue.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblNetValue
            // 
            lblNetValue.Dock = DockStyle.Fill;
            lblNetValue.Font = new Font("Tahoma", 13F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNetValue.ForeColor = Color.FromArgb(21, 87, 36);
            lblNetValue.Text = "0.00";
            lblNetValue.TextAlign = ContentAlignment.MiddleRight;
            // 
            // flpButtons
            // 
            flpButtons.Controls.Add(btnSave);
            flpButtons.Controls.Add(btnClose);
            flpButtons.Dock = DockStyle.Fill;
            flpButtons.FlowDirection = FlowDirection.RightToLeft;
            flpButtons.Padding = new Padding(0, 6, 0, 0);
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(25, 135, 84);
            btnSave.Cursor = Cursors.Hand;
            btnSave.FlatStyle = FlatStyle.Popup;
            btnSave.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSave.ForeColor = Color.White;
            btnSave.Height = 42;
            btnSave.Margin = new Padding(0, 0, 8, 0);
            btnSave.Text = "اعتماد الفاتورة";
            btnSave.Width = 190;
            btnSave.Click += btnSave_Click;
            // 
            // btnClose
            // 
            btnClose.BackColor = Color.FromArgb(233, 236, 239);
            btnClose.Cursor = Cursors.Hand;
            btnClose.FlatStyle = FlatStyle.Popup;
            btnClose.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnClose.ForeColor = Color.FromArgb(33, 43, 54);
            btnClose.Height = 42;
            btnClose.Margin = new Padding(0, 0, 8, 0);
            btnClose.Text = "إغلاق";
            btnClose.Width = 130;
            btnClose.Click += btnClose_Click;
            // 
            // dgvLines
            // 
            dgvLines.AllowUserToAddRows = true;
            dgvLines.AllowUserToDeleteRows = false;
            dgvLines.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvLines.BackgroundColor = Color.White;
            dgvLines.BorderStyle = BorderStyle.None;
            dgvLines.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvLines.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                BackColor = Color.FromArgb(233, 236, 239),
                Font = new Font("Tahoma", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 0),
                ForeColor = Color.FromArgb(33, 43, 54),
                SelectionBackColor = Color.FromArgb(233, 236, 239),
                SelectionForeColor = Color.FromArgb(33, 43, 54),
                WrapMode = DataGridViewTriState.False
            };
            dgvLines.ColumnHeadersHeight = 40;
            dgvLines.ContextMenuStrip = cmsLines;
            dgvLines.DefaultCellStyle = new DataGridViewCellStyle
            {
                Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0),
                Padding = new Padding(4),
                SelectionBackColor = Color.FromArgb(13, 110, 253),
                SelectionForeColor = Color.White
            };
            dgvLines.Dock = DockStyle.Fill;
            dgvLines.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
            dgvLines.EnableHeadersVisualStyles = false;
            dgvLines.GridColor = Color.FromArgb(222, 226, 230);
            dgvLines.MultiSelect = false;
            dgvLines.RightToLeft = RightToLeft.Yes;
            dgvLines.RowHeadersVisible = false;
            dgvLines.RowTemplate.Height = 34;
            dgvLines.ScrollBars = ScrollBars.Both;
            dgvLines.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvLines.CellValueChanged += dgvLines_CellValueChanged;
            dgvLines.CellMouseDown += dgvLines_CellMouseDown;
            dgvLines.DefaultValuesNeeded += dgvLines_DefaultValuesNeeded;
            dgvLines.KeyDown += dgvLines_KeyDown;
            // 
            // frmSalesInlineGrid
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(1000, 660);
            Controls.Add(dgvLines);
            Controls.Add(pnlFooter);
            Controls.Add(pnlHeader);
            Controls.Add(pnlTitle);
            Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmSalesInlineGrid";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "تعديل داخلي — فاتورة بيع";
            KeyDown += frmSalesInlineGrid_KeyDown;
            cmsLines.ResumeLayout(false);
            pnlTitle.ResumeLayout(false);
            tblTitle.ResumeLayout(false);
            pnlHeader.ResumeLayout(false);
            tblHeader.ResumeLayout(false);
            tblCustomer.ResumeLayout(false);
            pnlFooter.ResumeLayout(false);
            tblFooter.ResumeLayout(false);
            tblSummary.ResumeLayout(false);
            flpButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvLines).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip cmsLines;
        private System.Windows.Forms.ToolStripMenuItem miEditQty;
        private System.Windows.Forms.ToolStripMenuItem miAddDiscount;
        private System.Windows.Forms.ToolStripMenuItem miDeleteLine;
        private System.Windows.Forms.ToolStripMenuItem miInsertBelow;
        private System.Windows.Forms.Panel pnlTitle;
        private System.Windows.Forms.TableLayoutPanel tblTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.TableLayoutPanel tblHeader;
        private System.Windows.Forms.Label lblInvoiceNo;
        private System.Windows.Forms.Label lblCustomer;
        private System.Windows.Forms.Label lblPaymentType;
        private System.Windows.Forms.Label lblNotes;
        private System.Windows.Forms.TextBox txtInvoiceNo;
        private System.Windows.Forms.TableLayoutPanel tblCustomer;
        private System.Windows.Forms.ComboBox cbCustomer;
        private System.Windows.Forms.Label lblBalance;
        private System.Windows.Forms.ComboBox cbPaymentType;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.TableLayoutPanel tblFooter;
        private System.Windows.Forms.TableLayoutPanel tblSummary;
        private System.Windows.Forms.Label lblLinesCaption;
        private System.Windows.Forms.Label lblGrossCaption;
        private System.Windows.Forms.Label lblDiscountCaption;
        private System.Windows.Forms.Label lblNetCaption;
        private System.Windows.Forms.Label lblLinesValue;
        private System.Windows.Forms.Label lblGrossValue;
        private System.Windows.Forms.Label lblDiscountValue;
        private System.Windows.Forms.Label lblNetValue;
        private System.Windows.Forms.FlowLayoutPanel flpButtons;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridView dgvLines;
    }
}