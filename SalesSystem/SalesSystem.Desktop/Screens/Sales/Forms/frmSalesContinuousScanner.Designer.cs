namespace SalesSystem.Desktop.Screens.Sales.Forms
{
    partial class frmSalesContinuousScanner
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
            pnlHeader = new Panel();
            tblHeader = new TableLayoutPanel();
            lblTitle = new Label();
            lblHeaderHint = new Label();
            pnlScanner = new Panel();
            pnlLastScan = new Panel();
            lblLastScanInfo = new Label();
            lblLastScanTitle = new Label();
            lblScanError = new Label();
            pnlScanBox = new Panel();
            txtBarcode = new TextBox();
            lblScanCaption = new Label();
            pnlDemo = new Panel();
            flpDemo = new FlowLayoutPanel();
            lblDemoCaption = new Label();
            btnDemo1 = new Button();
            btnDemo2 = new Button();
            btnDemo3 = new Button();
            pnlCart = new Panel();
            dgvCart = new DataGridView();
            pnlFooter = new Panel();
            tblFooter = new TableLayoutPanel();
            pnlTotals = new Panel();
            flpTotals = new FlowLayoutPanel();
            lblSubTotalCaption = new Label();
            lblSubTotalVal = new Label();
            lblNetTotalCaption = new Label();
            lblNetTotalVal = new Label();
            flpFooterButtons = new FlowLayoutPanel();
            btnSave = new Button();
            btnClose = new Button();
            pnlHeader.SuspendLayout();
            tblHeader.SuspendLayout();
            pnlScanner.SuspendLayout();
            pnlLastScan.SuspendLayout();
            pnlScanBox.SuspendLayout();
            pnlDemo.SuspendLayout();
            flpDemo.SuspendLayout();
            pnlCart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCart).BeginInit();
            pnlFooter.SuspendLayout();
            tblFooter.SuspendLayout();
            pnlTotals.SuspendLayout();
            flpTotals.SuspendLayout();
            flpFooterButtons.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(33, 43, 54);
            pnlHeader.Controls.Add(tblHeader);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 64;
            // 
            // tblHeader
            // 
            tblHeader.ColumnCount = 3;
            tblHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 380F));
            tblHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 240F));
            tblHeader.Controls.Add(lblTitle, 0, 0);
            tblHeader.Controls.Add(lblHeaderHint, 2, 0);
            tblHeader.Dock = DockStyle.Fill;
            tblHeader.RowCount = 1;
            tblHeader.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            // 
            // lblTitle
            // 
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.Font = new Font("Tahoma", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.ForeColor = Color.White;
            lblTitle.Text = "مسح متواصل — فاتورة مبيعات";
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblHeaderHint
            // 
            lblHeaderHint.Dock = DockStyle.Fill;
            lblHeaderHint.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblHeaderHint.ForeColor = Color.FromArgb(150, 170, 190);
            lblHeaderHint.Text = "F2 = تركيز المسح   |   Delete = حذف سطر   |   Esc = إغلاق";
            lblHeaderHint.TextAlign = ContentAlignment.MiddleRight;
            // 
            // pnlScanner
            // 
            pnlScanner.BackColor = Color.White;
            pnlScanner.Controls.Add(pnlLastScan);
            pnlScanner.Controls.Add(lblScanError);
            pnlScanner.Controls.Add(pnlScanBox);
            pnlScanner.Controls.Add(lblScanCaption);
            pnlScanner.Dock = DockStyle.Top;
            pnlScanner.Height = 190;
            pnlScanner.Padding = new Padding(12, 6, 12, 6);
            // 
            // pnlLastScan
            // 
            pnlLastScan.BackColor = Color.FromArgb(211, 240, 222);
            pnlLastScan.BorderStyle = BorderStyle.FixedSingle;
            pnlLastScan.Controls.Add(lblLastScanInfo);
            pnlLastScan.Controls.Add(lblLastScanTitle);
            pnlLastScan.Dock = DockStyle.Fill;
            pnlLastScan.Padding = new Padding(10, 4, 10, 4);
            // 
            // lblLastScanInfo
            // 
            lblLastScanInfo.Dock = DockStyle.Fill;
            lblLastScanInfo.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblLastScanInfo.ForeColor = Color.FromArgb(21, 87, 36);
            lblLastScanInfo.Text = "—";
            lblLastScanInfo.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblLastScanTitle
            // 
            lblLastScanTitle.Dock = DockStyle.Top;
            lblLastScanTitle.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblLastScanTitle.ForeColor = Color.FromArgb(21, 87, 36);
            lblLastScanTitle.Height = 22;
            lblLastScanTitle.Text = "آخر مسح ناجح";
            lblLastScanTitle.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblScanError
            // 
            lblScanError.Dock = DockStyle.Top;
            lblScanError.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblScanError.ForeColor = Color.FromArgb(220, 53, 69);
            lblScanError.Height = 26;
            lblScanError.Text = "الباركود غير معروف";
            lblScanError.TextAlign = ContentAlignment.MiddleRight;
            lblScanError.Visible = false;
            // 
            // pnlScanBox
            // 
            pnlScanBox.BackColor = Color.FromArgb(25, 135, 84);
            pnlScanBox.BorderStyle = BorderStyle.FixedSingle;
            pnlScanBox.Controls.Add(txtBarcode);
            pnlScanBox.Dock = DockStyle.Top;
            pnlScanBox.Height = 60;
            pnlScanBox.Padding = new Padding(4);
            // 
            // txtBarcode
            // 
            txtBarcode.BackColor = Color.White;
            txtBarcode.BorderStyle = BorderStyle.None;
            txtBarcode.Dock = DockStyle.Fill;
            txtBarcode.Font = new Font("Tahoma", 22F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtBarcode.ForeColor = Color.FromArgb(33, 43, 54);
            txtBarcode.PlaceholderText = "امسح الباركود ثم اضغط Enter";
            txtBarcode.KeyDown += txtBarcode_KeyDown;
            // 
            // lblScanCaption
            // 
            lblScanCaption.Dock = DockStyle.Top;
            lblScanCaption.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblScanCaption.ForeColor = Color.FromArgb(80, 90, 100);
            lblScanCaption.Height = 24;
            lblScanCaption.Text = "منفذ المسح بالليزر (Laser Scanner) — الكميات تُجمع تلقائياً لكل مسحة";
            lblScanCaption.TextAlign = ContentAlignment.MiddleRight;
            // 
            // pnlDemo
            // 
            pnlDemo.BackColor = Color.FromArgb(248, 249, 250);
            pnlDemo.Controls.Add(flpDemo);
            pnlDemo.Dock = DockStyle.Top;
            pnlDemo.Height = 58;
            pnlDemo.Padding = new Padding(12, 8, 12, 8);
            // 
            // flpDemo
            // 
            flpDemo.Controls.Add(lblDemoCaption);
            flpDemo.Controls.Add(btnDemo1);
            flpDemo.Controls.Add(btnDemo2);
            flpDemo.Controls.Add(btnDemo3);
            flpDemo.Dock = DockStyle.Fill;
            flpDemo.FlowDirection = FlowDirection.RightToLeft;
            flpDemo.WrapContents = false;
            // 
            // lblDemoCaption
            // 
            lblDemoCaption.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDemoCaption.ForeColor = Color.FromArgb(80, 90, 100);
            lblDemoCaption.Height = 36;
            lblDemoCaption.Margin = new Padding(0, 0, 12, 0);
            lblDemoCaption.Text = "مسح تجريبي سريع:";
            lblDemoCaption.TextAlign = ContentAlignment.MiddleRight;
            // 
            // btnDemo1
            // 
            btnDemo1.BackColor = Color.White;
            btnDemo1.Cursor = Cursors.Hand;
            btnDemo1.FlatStyle = FlatStyle.Popup;
            btnDemo1.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDemo1.ForeColor = Color.FromArgb(33, 43, 54);
            btnDemo1.Height = 36;
            btnDemo1.Margin = new Padding(0, 0, 6, 0);
            btnDemo1.Tag = "102030";
            btnDemo1.Text = "102030 — شاي ليبتون";
            btnDemo1.Width = 150;
            btnDemo1.Click += btnDemo_Click;
            // 
            // btnDemo2
            // 
            btnDemo2.BackColor = Color.White;
            btnDemo2.Cursor = Cursors.Hand;
            btnDemo2.FlatStyle = FlatStyle.Popup;
            btnDemo2.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDemo2.ForeColor = Color.FromArgb(33, 43, 54);
            btnDemo2.Height = 36;
            btnDemo2.Margin = new Padding(0, 0, 6, 0);
            btnDemo2.Tag = "102031";
            btnDemo2.Text = "102031 — زيت نباتي";
            btnDemo2.Width = 150;
            btnDemo2.Click += btnDemo_Click;
            // 
            // btnDemo3
            // 
            btnDemo3.BackColor = Color.White;
            btnDemo3.Cursor = Cursors.Hand;
            btnDemo3.FlatStyle = FlatStyle.Popup;
            btnDemo3.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDemo3.ForeColor = Color.FromArgb(33, 43, 54);
            btnDemo3.Height = 36;
            btnDemo3.Margin = new Padding(0, 0, 6, 0);
            btnDemo3.Tag = "102032";
            btnDemo3.Text = "102032 — أرز بسمتي";
            btnDemo3.Width = 150;
            btnDemo3.Click += btnDemo_Click;
            // 
            // pnlCart
            // 
            pnlCart.BackColor = Color.White;
            pnlCart.Controls.Add(dgvCart);
            pnlCart.Dock = DockStyle.Fill;
            pnlCart.Padding = new Padding(8);
            // 
            // dgvCart
            // 
            dgvCart.AllowUserToAddRows = false;
            dgvCart.AllowUserToDeleteRows = false;
            dgvCart.BackgroundColor = Color.White;
            dgvCart.BorderStyle = BorderStyle.Fixed3D;
            dgvCart.ColumnHeadersHeight = 42;
            dgvCart.Dock = DockStyle.Fill;
            dgvCart.MultiSelect = false;
            dgvCart.ReadOnly = true;
            dgvCart.RowHeadersVisible = false;
            dgvCart.RowTemplate.Height = 34;
            dgvCart.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            // 
            // pnlFooter
            // 
            pnlFooter.BackColor = Color.FromArgb(248, 249, 250);
            pnlFooter.Controls.Add(tblFooter);
            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Height = 68;
            pnlFooter.Padding = new Padding(10, 8, 10, 8);
            // 
            // tblFooter
            // 
            tblFooter.ColumnCount = 2;
            tblFooter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblFooter.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300F));
            tblFooter.Controls.Add(pnlTotals, 0, 0);
            tblFooter.Controls.Add(flpFooterButtons, 1, 0);
            tblFooter.Dock = DockStyle.Fill;
            tblFooter.RowCount = 1;
            tblFooter.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            // 
            // pnlTotals
            // 
            pnlTotals.Controls.Add(flpTotals);
            pnlTotals.Dock = DockStyle.Fill;
            // 
            // flpTotals
            // 
            flpTotals.Controls.Add(lblSubTotalCaption);
            flpTotals.Controls.Add(lblSubTotalVal);
            flpTotals.Controls.Add(lblNetTotalCaption);
            flpTotals.Controls.Add(lblNetTotalVal);
            flpTotals.Dock = DockStyle.Fill;
            flpTotals.FlowDirection = FlowDirection.RightToLeft;
            flpTotals.Padding = new Padding(0, 2, 0, 0);
            // 
            // lblSubTotalCaption
            // 
            lblSubTotalCaption.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSubTotalCaption.ForeColor = Color.FromArgb(80, 90, 100);
            lblSubTotalCaption.Height = 44;
            lblSubTotalCaption.Margin = new Padding(0, 0, 10, 0);
            lblSubTotalCaption.Text = "المجموع:";
            lblSubTotalCaption.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblSubTotalVal
            // 
            lblSubTotalVal.Font = new Font("Tahoma", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSubTotalVal.ForeColor = Color.FromArgb(33, 43, 54);
            lblSubTotalVal.Height = 44;
            lblSubTotalVal.Margin = new Padding(0, 0, 24, 0);
            lblSubTotalVal.Text = "0.00";
            lblSubTotalVal.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblNetTotalCaption
            // 
            lblNetTotalCaption.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNetTotalCaption.ForeColor = Color.FromArgb(21, 87, 36);
            lblNetTotalCaption.Height = 44;
            lblNetTotalCaption.Margin = new Padding(0, 0, 10, 0);
            lblNetTotalCaption.Text = "الصافي:";
            lblNetTotalCaption.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblNetTotalVal
            // 
            lblNetTotalVal.Font = new Font("Tahoma", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNetTotalVal.ForeColor = Color.FromArgb(25, 135, 84);
            lblNetTotalVal.Height = 44;
            lblNetTotalVal.Text = "0.00";
            lblNetTotalVal.TextAlign = ContentAlignment.MiddleRight;
            // 
            // flpFooterButtons
            // 
            flpFooterButtons.Controls.Add(btnSave);
            flpFooterButtons.Controls.Add(btnClose);
            flpFooterButtons.Dock = DockStyle.Fill;
            flpFooterButtons.FlowDirection = FlowDirection.RightToLeft;
            flpFooterButtons.Padding = new Padding(0, 4, 0, 0);
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(25, 135, 84);
            btnSave.Cursor = Cursors.Hand;
            btnSave.FlatStyle = FlatStyle.Popup;
            btnSave.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSave.ForeColor = Color.White;
            btnSave.Height = 44;
            btnSave.Margin = new Padding(0, 0, 8, 0);
            btnSave.Text = "حفظ واعتماد [F9]";
            btnSave.Width = 170;
            btnSave.Click += btnSave_Click;
            // 
            // btnClose
            // 
            btnClose.BackColor = Color.White;
            btnClose.Cursor = Cursors.Hand;
            btnClose.DialogResult = DialogResult.Cancel;
            btnClose.FlatStyle = FlatStyle.Popup;
            btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnClose.ForeColor = Color.FromArgb(33, 43, 54);
            btnClose.Height = 44;
            btnClose.Text = "إغلاق [Esc]";
            btnClose.Width = 110;
            btnClose.Click += btnClose_Click;
            // 
            pnlHeader.ResumeLayout(false);
            tblHeader.ResumeLayout(false);
            pnlScanner.ResumeLayout(false);
            pnlLastScan.ResumeLayout(false);
            pnlScanBox.ResumeLayout(false);
            pnlScanBox.PerformLayout();
            pnlDemo.ResumeLayout(false);
            flpDemo.ResumeLayout(false);
            pnlCart.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvCart).EndInit();
            pnlFooter.ResumeLayout(false);
            tblFooter.ResumeLayout(false);
            pnlTotals.ResumeLayout(false);
            flpTotals.ResumeLayout(false);
            flpFooterButtons.ResumeLayout(false);
            // 
            // frmSalesContinuousScanner
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            CancelButton = btnClose;
            ClientSize = new Size(900, 640);
            Controls.Add(pnlCart);
            Controls.Add(pnlFooter);
            Controls.Add(pnlDemo);
            Controls.Add(pnlScanner);
            Controls.Add(pnlHeader);
            Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmSalesContinuousScanner";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "مسح متواصل — فاتورة مبيعات";
            Shown += frmSalesContinuousScanner_Shown;
            KeyDown += frmSalesContinuousScanner_KeyDown;
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.TableLayoutPanel tblHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblHeaderHint;
        private System.Windows.Forms.Panel pnlScanner;
        private System.Windows.Forms.Panel pnlLastScan;
        private System.Windows.Forms.Label lblLastScanInfo;
        private System.Windows.Forms.Label lblLastScanTitle;
        private System.Windows.Forms.Label lblScanError;
        private System.Windows.Forms.Panel pnlScanBox;
        private System.Windows.Forms.TextBox txtBarcode;
        private System.Windows.Forms.Label lblScanCaption;
        private System.Windows.Forms.Panel pnlDemo;
        private System.Windows.Forms.FlowLayoutPanel flpDemo;
        private System.Windows.Forms.Label lblDemoCaption;
        private System.Windows.Forms.Button btnDemo1;
        private System.Windows.Forms.Button btnDemo2;
        private System.Windows.Forms.Button btnDemo3;
        private System.Windows.Forms.Panel pnlCart;
        private System.Windows.Forms.DataGridView dgvCart;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.TableLayoutPanel tblFooter;
        private System.Windows.Forms.Panel pnlTotals;
        private System.Windows.Forms.FlowLayoutPanel flpTotals;
        private System.Windows.Forms.Label lblSubTotalCaption;
        private System.Windows.Forms.Label lblSubTotalVal;
        private System.Windows.Forms.Label lblNetTotalCaption;
        private System.Windows.Forms.Label lblNetTotalVal;
        private System.Windows.Forms.FlowLayoutPanel flpFooterButtons;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
    }
}