namespace SalesSystem.Desktop.Products.Forms
{
    partial class frmProductPrices
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
            cmsPrices = new ContextMenuStrip(components);
            editPriceToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            refreshToolStripMenuItem = new ToolStripMenuItem();
            lblMode = new Label();
            dgvProducts = new DataGridView();
            btnClose = new Button();
            lblRecordsCount = new Label();
            label1 = new Label();
            label2 = new Label();
            txtFilterValue = new TextBox();
            cmsPrices.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvProducts).BeginInit();
            SuspendLayout();

            // cmsPrices
            cmsPrices.BackColor = Color.White;
            cmsPrices.Font = new Font("Tahoma", 10F);
            cmsPrices.ImageScalingSize = new Size(24, 24);
            cmsPrices.Items.AddRange(new ToolStripItem[] { editPriceToolStripMenuItem, toolStripSeparator1, refreshToolStripMenuItem });
            cmsPrices.Name = "cmsPrices";
            cmsPrices.RightToLeft = RightToLeft.Yes;
            cmsPrices.Size = new Size(250, 92);
            cmsPrices.Opening += cmsPrices_Opening;

            // editPriceToolStripMenuItem
            editPriceToolStripMenuItem.Name = "editPriceToolStripMenuItem";
            editPriceToolStripMenuItem.Size = new Size(249, 32);
            editPriceToolStripMenuItem.Text = "تعديل السعر";
            editPriceToolStripMenuItem.Click += editPriceToolStripMenuItem_Click;

            // toolStripSeparator1
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(246, 6);

            // refreshToolStripMenuItem
            refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            refreshToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.R;
            refreshToolStripMenuItem.Size = new Size(249, 32);
            refreshToolStripMenuItem.Text = "تحديث";
            refreshToolStripMenuItem.Click += refreshToolStripMenuItem_Click;

            // lblMode
            lblMode.Anchor = AnchorStyles.Top;
            lblMode.AutoSize = true;
            lblMode.BackColor = Color.WhiteSmoke;
            lblMode.FlatStyle = FlatStyle.Flat;
            lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            lblMode.ForeColor = Color.Firebrick;
            lblMode.Location = new Point(667, 20);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(186, 39);
            lblMode.TabIndex = 30;
            lblMode.Text = "أسعار المنتجات";

            // dgvProducts
            dgvProducts.AllowUserToAddRows = false;
            dgvProducts.AllowUserToDeleteRows = false;
            dgvProducts.AllowUserToOrderColumns = true;
            dgvProducts.BackgroundColor = SystemColors.Window;
            dgvProducts.BorderStyle = BorderStyle.Fixed3D;
            dgvProducts.ColumnHeadersHeight = 50;
            dgvProducts.ContextMenuStrip = cmsPrices;
            dgvProducts.Location = new Point(12, 120);
            dgvProducts.MultiSelect = false;
            dgvProducts.Name = "dgvProducts";
            dgvProducts.ReadOnly = true;
            dgvProducts.RowHeadersWidth = 62;
            dgvProducts.RowTemplate.Height = 28;
            dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProducts.Size = new Size(1540, 580);
            dgvProducts.TabIndex = 34;
            dgvProducts.TabStop = false;

            // btnClose
            btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnClose.BackColor = Color.White;
            btnClose.DialogResult = DialogResult.Cancel;
            btnClose.FlatAppearance.BorderColor = Color.Black;
            btnClose.FlatAppearance.BorderSize = 2;
            btnClose.FlatStyle = FlatStyle.Popup;
            btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnClose.Location = new Point(12, 710);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(181, 61);
            btnClose.TabIndex = 0;
            btnClose.Text = "إغلاق";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;

            // lblRecordsCount
            lblRecordsCount.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            lblRecordsCount.AutoSize = true;
            lblRecordsCount.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblRecordsCount.Location = new Point(1380, 720);
            lblRecordsCount.Name = "lblRecordsCount";
            lblRecordsCount.Size = new Size(72, 24);
            lblRecordsCount.TabIndex = 4;
            lblRecordsCount.Text = "[????]";

            // label1
            label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label1.Location = new Point(1220, 720);
            label1.Name = "label1";
            label1.Size = new Size(152, 24);
            label1.TabIndex = 3;
            label1.Text = "عدد المنتجات:";

            // label2
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label2.Location = new Point(21, 75);
            label2.Name = "label2";
            label2.Size = new Size(77, 24);
            label2.TabIndex = 5;
            label2.Text = "بحث:";

            // txtFilterValue
            txtFilterValue.Font = new Font("Tahoma", 10F);
            txtFilterValue.Location = new Point(110, 72);
            txtFilterValue.Name = "txtFilterValue";
            txtFilterValue.Size = new Size(500, 32);
            txtFilterValue.TabIndex = 2;
            txtFilterValue.TextChanged += txtFilterValue_TextChanged;

            // frmProductPrices
            AcceptButton = btnClose;
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            CancelButton = btnClose;
            ClientSize = new Size(1564, 783);
            Controls.Add(lblMode);
            Controls.Add(dgvProducts);
            Controls.Add(label2);
            Controls.Add(txtFilterValue);
            Controls.Add(label1);
            Controls.Add(lblRecordsCount);
            Controls.Add(btnClose);
            Font = new Font("Tahoma", 12F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "frmProductPrices";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "أسعار المنتجات";
            Load += frmProductPrices_Load;
            cmsPrices.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvProducts).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.ContextMenuStrip cmsPrices;
        private System.Windows.Forms.ToolStripMenuItem editPriceToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.DataGridView dgvProducts;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblRecordsCount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtFilterValue;
    }
}
