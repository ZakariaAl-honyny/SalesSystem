namespace SalesSystem.Desktop.Products.Forms
{
    partial class frmProductUnitsList
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
            tooltip = new ToolTip(components);
            cmsUnits = new ContextMenuStrip(components);
            addUnitToolStripMenuItem = new ToolStripMenuItem();
            editUnitToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            refreshToolStripMenuItem = new ToolStripMenuItem();
            lblMode = new Label();
            dgvUnits = new DataGridView();
            btnClose = new Button();
            lblRecordsCount = new Label();
            label1 = new Label();
            label2 = new Label();
            cmbProduct = new ComboBox();
            cmsUnits.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUnits).BeginInit();
            SuspendLayout();

            // cmsUnits
            cmsUnits.BackColor = Color.White;
            cmsUnits.Font = new Font("Tahoma", 10F);
            cmsUnits.ImageScalingSize = new Size(24, 24);
            cmsUnits.Items.AddRange(new ToolStripItem[] { addUnitToolStripMenuItem, editUnitToolStripMenuItem, toolStripSeparator1, refreshToolStripMenuItem });
            cmsUnits.Name = "cmsUnits";
            cmsUnits.RightToLeft = RightToLeft.Yes;
            cmsUnits.Size = new Size(250, 130);
            cmsUnits.Opening += cmsUnits_Opening;

            // addUnitToolStripMenuItem
            addUnitToolStripMenuItem.Name = "addUnitToolStripMenuItem";
            addUnitToolStripMenuItem.Size = new Size(249, 32);
            addUnitToolStripMenuItem.Text = "إضافة وحدة";
            addUnitToolStripMenuItem.Click += addUnitToolStripMenuItem_Click;

            // editUnitToolStripMenuItem
            editUnitToolStripMenuItem.Name = "editUnitToolStripMenuItem";
            editUnitToolStripMenuItem.Size = new Size(249, 32);
            editUnitToolStripMenuItem.Text = "تعديل";
            editUnitToolStripMenuItem.Click += editUnitToolStripMenuItem_Click;

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
            lblMode.Text = "وحدات المنتجات";

            // dgvUnits
            dgvUnits.AllowUserToAddRows = false;
            dgvUnits.AllowUserToDeleteRows = false;
            dgvUnits.AllowUserToOrderColumns = true;
            dgvUnits.BackgroundColor = SystemColors.Window;
            dgvUnits.BorderStyle = BorderStyle.Fixed3D;
            dgvUnits.ColumnHeadersHeight = 50;
            dgvUnits.ContextMenuStrip = cmsUnits;
            dgvUnits.Location = new Point(12, 120);
            dgvUnits.MultiSelect = false;
            dgvUnits.Name = "dgvUnits";
            dgvUnits.ReadOnly = true;
            dgvUnits.RowHeadersWidth = 62;
            dgvUnits.RowTemplate.Height = 28;
            dgvUnits.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUnits.Size = new Size(1540, 580);
            dgvUnits.TabIndex = 34;
            dgvUnits.TabStop = false;
            tooltip.SetToolTip(dgvUnits, "وحدات المنتج الفرعية. انقر بزر الماوس الأيمن للمزيد");

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
            tooltip.SetToolTip(btnClose, "إضافة وحدة فرعية للمنتج");

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
            label1.Size = new Size(142, 24);
            label1.TabIndex = 3;
            label1.Text = "عدد الوحدات:";

            // label2
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label2.Location = new Point(21, 75);
            label2.Name = "label2";
            label2.Size = new Size(86, 24);
            label2.TabIndex = 5;
            label2.Text = "المنتج:";

            // cmbProduct
            cmbProduct.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbProduct.Font = new Font("Tahoma", 10F);
            cmbProduct.FormattingEnabled = true;
            cmbProduct.Location = new Point(110, 72);
            cmbProduct.Name = "cmbProduct";
            cmbProduct.Size = new Size(500, 32);
            cmbProduct.TabIndex = 1;
            tooltip.SetToolTip(cmbProduct, "اختر المنتج لإدارة وحداته الفرعية");

            // frmProductUnitsList
            AcceptButton = btnClose;
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            CancelButton = btnClose;
            ClientSize = new Size(1564, 783);
            Controls.Add(lblMode);
            Controls.Add(dgvUnits);
            Controls.Add(label2);
            Controls.Add(cmbProduct);
            Controls.Add(label1);
            Controls.Add(lblRecordsCount);
            Controls.Add(btnClose);
            Font = new Font("Tahoma", 12F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "frmProductUnitsList";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "وحدات المنتجات";
            Load += frmProductUnitsList_Load;
            cmsUnits.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvUnits).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.ContextMenuStrip cmsUnits;
        private System.Windows.Forms.ToolStripMenuItem addUnitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editUnitToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.DataGridView dgvUnits;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblRecordsCount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbProduct;
        private System.Windows.Forms.ToolTip tooltip;
    }
}
