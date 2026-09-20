namespace SalesSystem.Desktop.Screens.Sales.Forms
{
    partial class frmSalesTouchGrid
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
            lblMode = new Label();
            lblEscHint = new Label();
            tblMain = new TableLayoutPanel();
            pnlCatalog = new Panel();
            flpProducts = new FlowLayoutPanel();
            flpCategories = new FlowLayoutPanel();
            pnlCart = new Panel();
            flpCart = new FlowLayoutPanel();
            pnlCartHeader = new Panel();
            tblCartHeader = new TableLayoutPanel();
            lblCartTitle = new Label();
            lblCartCount = new Label();
            pnlCartFooter = new Panel();
            tblTotalCard = new TableLayoutPanel();
            lblTotalCaption = new Label();
            lblTotalVal = new Label();
            btnSave = new Button();
            pnlHeader.SuspendLayout();
            tblHeader.SuspendLayout();
            tblMain.SuspendLayout();
            pnlCatalog.SuspendLayout();
            pnlCart.SuspendLayout();
            pnlCartHeader.SuspendLayout();
            tblCartHeader.SuspendLayout();
            pnlCartFooter.SuspendLayout();
            tblTotalCard.SuspendLayout();
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
            tblHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 240F));
            tblHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160F));
            tblHeader.Controls.Add(lblTitle, 0, 0);
            tblHeader.Controls.Add(lblMode, 1, 0);
            tblHeader.Controls.Add(lblEscHint, 2, 0);
            tblHeader.Dock = DockStyle.Fill;
            tblHeader.RowCount = 1;
            tblHeader.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            // 
            // lblTitle
            // 
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.Font = new Font("Tahoma", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.ForeColor = Color.White;
            lblTitle.Text = "شبكة لمس — فاتورة بيع";
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblMode
            // 
            lblMode.Dock = DockStyle.Fill;
            lblMode.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblMode.ForeColor = Color.FromArgb(180, 200, 220);
            lblMode.Text = "وضع تجريبي (Mock) — بدون خادم";
            lblMode.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblEscHint
            // 
            lblEscHint.Dock = DockStyle.Fill;
            lblEscHint.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblEscHint.ForeColor = Color.FromArgb(150, 170, 190);
            lblEscHint.Text = "Esc = إغلاق";
            lblEscHint.TextAlign = ContentAlignment.MiddleRight;
            // 
            // tblMain
            // 
            tblMain.ColumnCount = 2;
            tblMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55F));
            tblMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));
            tblMain.Controls.Add(pnlCatalog, 0, 0);
            tblMain.Controls.Add(pnlCart, 1, 0);
            tblMain.Dock = DockStyle.Fill;
            tblMain.RowCount = 1;
            tblMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            // 
            // pnlCatalog
            // 
            pnlCatalog.BackColor = Color.White;
            pnlCatalog.Controls.Add(flpProducts);
            pnlCatalog.Controls.Add(flpCategories);
            pnlCatalog.Dock = DockStyle.Fill;
            pnlCatalog.Padding = new Padding(8);
            // 
            // flpProducts
            // 
            flpProducts.AutoScroll = true;
            flpProducts.BackColor = Color.White;
            flpProducts.Dock = DockStyle.Fill;
            flpProducts.FlowDirection = FlowDirection.LeftToRight;
            flpProducts.Padding = new Padding(4);
            flpProducts.WrapContents = true;
            // 
            // flpCategories
            // 
            flpCategories.Dock = DockStyle.Top;
            flpCategories.FlowDirection = FlowDirection.RightToLeft;
            flpCategories.Height = 48;
            flpCategories.Padding = new Padding(0, 8, 0, 6);
            // 
            // pnlCart
            // 
            pnlCart.BackColor = Color.White;
            pnlCart.Controls.Add(flpCart);
            pnlCart.Controls.Add(pnlCartHeader);
            pnlCart.Controls.Add(pnlCartFooter);
            pnlCart.Dock = DockStyle.Fill;
            // 
            // flpCart
            // 
            flpCart.AutoScroll = true;
            flpCart.BackColor = Color.WhiteSmoke;
            flpCart.Dock = DockStyle.Fill;
            flpCart.FlowDirection = FlowDirection.TopDown;
            flpCart.Padding = new Padding(8);
            flpCart.WrapContents = false;
            flpCart.Resize += flpCart_Resize;
            // 
            // pnlCartHeader
            // 
            pnlCartHeader.BackColor = Color.FromArgb(233, 236, 239);
            pnlCartHeader.Controls.Add(tblCartHeader);
            pnlCartHeader.Dock = DockStyle.Top;
            pnlCartHeader.Height = 44;
            // 
            // tblCartHeader
            // 
            tblCartHeader.ColumnCount = 2;
            tblCartHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblCartHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
            tblCartHeader.Controls.Add(lblCartTitle, 0, 0);
            tblCartHeader.Controls.Add(lblCartCount, 1, 0);
            tblCartHeader.Dock = DockStyle.Fill;
            tblCartHeader.RowCount = 1;
            tblCartHeader.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            // 
            // lblCartTitle
            // 
            lblCartTitle.Dock = DockStyle.Fill;
            lblCartTitle.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCartTitle.ForeColor = Color.FromArgb(33, 43, 54);
            lblCartTitle.Text = "سلة الفاتورة";
            lblCartTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblCartCount
            // 
            lblCartCount.Dock = DockStyle.Fill;
            lblCartCount.Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblCartCount.ForeColor = Color.FromArgb(80, 90, 100);
            lblCartCount.Text = "لا توجد أصناف";
            lblCartCount.TextAlign = ContentAlignment.MiddleRight;
            // 
            // pnlCartFooter
            // 
            pnlCartFooter.BackColor = Color.FromArgb(248, 249, 250);
            pnlCartFooter.Controls.Add(tblTotalCard);
            pnlCartFooter.Controls.Add(btnSave);
            pnlCartFooter.Dock = DockStyle.Bottom;
            pnlCartFooter.Height = 146;
            pnlCartFooter.Padding = new Padding(8, 8, 8, 8);
            // 
            // tblTotalCard
            // 
            tblTotalCard.BackColor = Color.FromArgb(211, 240, 222);
            tblTotalCard.ColumnCount = 2;
            tblTotalCard.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
            tblTotalCard.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblTotalCard.Controls.Add(lblTotalCaption, 0, 0);
            tblTotalCard.Controls.Add(lblTotalVal, 1, 0);
            tblTotalCard.Dock = DockStyle.Top;
            tblTotalCard.Height = 56;
            tblTotalCard.Padding = new Padding(10, 0, 10, 0);
            tblTotalCard.RowCount = 1;
            tblTotalCard.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            // 
            // lblTotalCaption
            // 
            lblTotalCaption.Dock = DockStyle.Fill;
            lblTotalCaption.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalCaption.ForeColor = Color.FromArgb(21, 87, 36);
            lblTotalCaption.Text = "الإجمالي:";
            lblTotalCaption.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTotalVal
            // 
            lblTotalVal.Dock = DockStyle.Fill;
            lblTotalVal.Font = new Font("Tahoma", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalVal.ForeColor = Color.FromArgb(21, 87, 36);
            lblTotalVal.Text = "0.00";
            lblTotalVal.TextAlign = ContentAlignment.MiddleRight;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(25, 135, 84);
            btnSave.Cursor = Cursors.Hand;
            btnSave.Dock = DockStyle.Bottom;
            btnSave.Enabled = false;
            btnSave.FlatStyle = FlatStyle.Popup;
            btnSave.Font = new Font("Tahoma", 13F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSave.ForeColor = Color.White;
            btnSave.Height = 62;
            btnSave.Text = "حفظ باللمس";
            btnSave.Click += btnSave_Click;
            // 
            // frmSalesTouchGrid
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(1100, 680);
            Controls.Add(tblMain);
            Controls.Add(pnlHeader);
            Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmSalesTouchGrid";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "شبكة لمس — فاتورة بيع";
            KeyDown += frmSalesTouchGrid_KeyDown;
            pnlHeader.ResumeLayout(false);
            tblHeader.ResumeLayout(false);
            tblMain.ResumeLayout(false);
            pnlCatalog.ResumeLayout(false);
            pnlCart.ResumeLayout(false);
            pnlCartHeader.ResumeLayout(false);
            tblCartHeader.ResumeLayout(false);
            pnlCartFooter.ResumeLayout(false);
            tblTotalCard.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.TableLayoutPanel tblHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.Label lblEscHint;
        private System.Windows.Forms.TableLayoutPanel tblMain;
        private System.Windows.Forms.Panel pnlCatalog;
        private System.Windows.Forms.FlowLayoutPanel flpProducts;
        private System.Windows.Forms.FlowLayoutPanel flpCategories;
        private System.Windows.Forms.Panel pnlCart;
        private System.Windows.Forms.FlowLayoutPanel flpCart;
        private System.Windows.Forms.Panel pnlCartHeader;
        private System.Windows.Forms.TableLayoutPanel tblCartHeader;
        private System.Windows.Forms.Label lblCartTitle;
        private System.Windows.Forms.Label lblCartCount;
        private System.Windows.Forms.Panel pnlCartFooter;
        private System.Windows.Forms.TableLayoutPanel tblTotalCard;
        private System.Windows.Forms.Label lblTotalCaption;
        private System.Windows.Forms.Label lblTotalVal;
        private System.Windows.Forms.Button btnSave;
    }
}