namespace SalesSystem.Desktop.Screens.StockTransfers.Forms
{
    partial class frmStockTransfer
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
            errorProvider = new System.Windows.Forms.ErrorProvider(components);
            errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            pnlHeader = new Panel();
            tableLayoutPanel1 = new TableLayoutPanel();
            lblTransferNoLabel = new Label();
            lblTransferNo = new Label();
            lblDate = new Label();
            dtpDate = new DateTimePicker();
            lblStatus = new Label();
            lblSource = new Label();
            cmbSource = new ComboBox();
            lblDest = new Label();
            cmbDest = new ComboBox();
            dgvItems = new DataGridView();
            pnlFooter = new Panel();
            txtNotes = new TextBox();
            pnlButtons = new Panel();
            btnClose = new Button();
            btnPost = new Button();
            btnSaveDraft = new Button();
            btnRemoveItem = new Button();
            btnAddItem = new Button();
            pnlHeader.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvItems).BeginInit();
            pnlFooter.SuspendLayout();
            pnlButtons.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.Controls.Add(tableLayoutPanel1);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Padding = new Padding(10);
            pnlHeader.Size = new Size(971, 120);
            pnlHeader.TabIndex = 3;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 6;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34F));
            tableLayoutPanel1.Controls.Add(lblTransferNoLabel, 0, 0);
            tableLayoutPanel1.Controls.Add(lblTransferNo, 1, 0);
            tableLayoutPanel1.Controls.Add(lblDate, 2, 0);
            tableLayoutPanel1.Controls.Add(dtpDate, 3, 0);
            tableLayoutPanel1.Controls.Add(lblSource, 0, 1);
            tableLayoutPanel1.Controls.Add(cmbSource, 1, 1);
            tableLayoutPanel1.Controls.Add(lblDest, 2, 1);
            tableLayoutPanel1.Controls.Add(cmbDest, 3, 1);
            tableLayoutPanel1.Controls.Add(lblStatus, 5, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(10, 10);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new Size(951, 100);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // lblTransferNoLabel
            // 
            lblTransferNoLabel.Location = new Point(854, 0);
            lblTransferNoLabel.Name = "lblTransferNoLabel";
            lblTransferNoLabel.Size = new Size(94, 20);
            lblTransferNoLabel.TabIndex = 0;
            lblTransferNoLabel.Text = "رقم التحويل:";
            // 
            // lblTransferNo
            // 
            lblTransferNo.Location = new Point(748, 0);
            lblTransferNo.Name = "lblTransferNo";
            lblTransferNo.Size = new Size(100, 20);
            lblTransferNo.TabIndex = 1;
            lblTransferNo.Text = "جديد";
            // 
            // lblDate
            // 
            lblDate.Location = new Point(540, 0);
            lblDate.Name = "lblDate";
            lblDate.Size = new Size(94, 20);
            lblDate.TabIndex = 2;
            lblDate.Text = "التاريخ:";
            // 
            // dtpDate
            // 
            dtpDate.Location = new Point(349, 3);
            dtpDate.Name = "dtpDate";
            dtpDate.Size = new Size(185, 36);
            dtpDate.TabIndex = 3;
            // 
            // lblStatus
            // 
            lblStatus.BackColor = Color.MediumPurple;
            lblStatus.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblStatus.ForeColor = Color.White;
            lblStatus.Location = new Point(120, 0);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(100, 20);
            lblStatus.TabIndex = 4;
            lblStatus.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblSource
            // 
            lblSource.Location = new Point(854, 20);
            lblSource.Name = "lblSource";
            lblSource.Size = new Size(94, 23);
            lblSource.TabIndex = 5;
            lblSource.Text = "من مستودع:";
            // 
            // cmbSource
            // 
            cmbSource.Location = new Point(727, 23);
            cmbSource.Name = "cmbSource";
            cmbSource.Size = new Size(121, 37);
            cmbSource.TabIndex = 6;
            // 
            // lblDest
            // 
            lblDest.Location = new Point(540, 20);
            lblDest.Name = "lblDest";
            lblDest.Size = new Size(94, 23);
            lblDest.TabIndex = 7;
            lblDest.Text = "إلى مستودع:";
            // 
            // cmbDest
            // 
            cmbDest.Location = new Point(413, 23);
            cmbDest.Name = "cmbDest";
            cmbDest.Size = new Size(121, 37);
            cmbDest.TabIndex = 8;
            // 
            // dgvItems
            // 
            dgvItems.BackgroundColor = Color.White;
            dgvItems.ColumnHeadersHeight = 34;
            dgvItems.Dock = DockStyle.Fill;
            dgvItems.Location = new Point(0, 120);
            dgvItems.Name = "dgvItems";
            dgvItems.RowHeadersWidth = 62;
            dgvItems.Size = new Size(971, 374);
            dgvItems.TabIndex = 0;
            // 
            // pnlFooter
            // 
            pnlFooter.Controls.Add(txtNotes);
            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Location = new Point(0, 554);
            pnlFooter.Name = "pnlFooter";
            pnlFooter.Padding = new Padding(10);
            pnlFooter.Size = new Size(971, 100);
            pnlFooter.TabIndex = 2;
            // 
            // txtNotes
            // 
            txtNotes.Dock = DockStyle.Fill;
            txtNotes.Location = new Point(10, 10);
            txtNotes.Multiline = true;
            txtNotes.Name = "txtNotes";
            txtNotes.PlaceholderText = "ملاحظات التحويل...";
            txtNotes.Size = new Size(951, 80);
            txtNotes.TabIndex = 0;
            // 
            // pnlButtons
            // 
            pnlButtons.Controls.Add(btnClose);
            pnlButtons.Controls.Add(btnPost);
            pnlButtons.Controls.Add(btnSaveDraft);
            pnlButtons.Controls.Add(btnRemoveItem);
            pnlButtons.Controls.Add(btnAddItem);
            pnlButtons.Dock = DockStyle.Bottom;
            pnlButtons.Location = new Point(0, 494);
            pnlButtons.Name = "pnlButtons";
            pnlButtons.Size = new Size(971, 60);
            pnlButtons.TabIndex = 1;
            // 
            // btnClose
            // 
            btnClose.BackColor = Color.White;
            btnClose.Dock = DockStyle.Left;
            btnClose.FlatAppearance.BorderColor = Color.Black;
            btnClose.FlatAppearance.BorderSize = 2;
            btnClose.FlatStyle = FlatStyle.Popup;
            btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnClose.Location = new Point(240, 0);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(80, 60);
            btnClose.TabIndex = 0;
            btnClose.Text = "إغلاق";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // btnPost
            // 
            btnPost.BackColor = Color.White;
            btnPost.Dock = DockStyle.Left;
            btnPost.FlatAppearance.BorderColor = Color.Black;
            btnPost.FlatAppearance.BorderSize = 2;
            btnPost.FlatStyle = FlatStyle.Popup;
            btnPost.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnPost.Location = new Point(120, 0);
            btnPost.Name = "btnPost";
            btnPost.Size = new Size(120, 60);
            btnPost.TabIndex = 1;
            btnPost.Text = "ترحيل";
            btnPost.UseVisualStyleBackColor = false;
            btnPost.Click += btnPost_Click;
            // 
            // btnSaveDraft
            // 
            btnSaveDraft.BackColor = Color.White;
            btnSaveDraft.Dock = DockStyle.Left;
            btnSaveDraft.FlatAppearance.BorderColor = Color.Black;
            btnSaveDraft.FlatAppearance.BorderSize = 2;
            btnSaveDraft.FlatStyle = FlatStyle.Popup;
            btnSaveDraft.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSaveDraft.Location = new Point(0, 0);
            btnSaveDraft.Name = "btnSaveDraft";
            btnSaveDraft.Size = new Size(120, 60);
            btnSaveDraft.TabIndex = 2;
            btnSaveDraft.Text = "حفظ مسودة";
            btnSaveDraft.UseVisualStyleBackColor = false;
            btnSaveDraft.Click += btnSaveDraft_Click;
            // 
            // btnRemoveItem
            // 
            btnRemoveItem.BackColor = Color.White;
            btnRemoveItem.Dock = DockStyle.Right;
            btnRemoveItem.FlatAppearance.BorderColor = Color.Black;
            btnRemoveItem.FlatAppearance.BorderSize = 2;
            btnRemoveItem.FlatStyle = FlatStyle.Popup;
            btnRemoveItem.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnRemoveItem.Location = new Point(731, 0);
            btnRemoveItem.Name = "btnRemoveItem";
            btnRemoveItem.Size = new Size(120, 60);
            btnRemoveItem.TabIndex = 3;
            btnRemoveItem.Text = "حذف صنف";
            btnRemoveItem.UseVisualStyleBackColor = false;
            btnRemoveItem.Click += btnRemoveItem_Click;
            // 
            // btnAddItem
            // 
            btnAddItem.BackColor = Color.White;
            btnAddItem.Dock = DockStyle.Right;
            btnAddItem.FlatAppearance.BorderColor = Color.Black;
            btnAddItem.FlatAppearance.BorderSize = 2;
            btnAddItem.FlatStyle = FlatStyle.Popup;
            btnAddItem.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAddItem.Location = new Point(851, 0);
            btnAddItem.Name = "btnAddItem";
            btnAddItem.Size = new Size(120, 60);
            btnAddItem.TabIndex = 4;
            btnAddItem.Text = "إضافة صنف";
            btnAddItem.UseVisualStyleBackColor = false;
            btnAddItem.Click += btnAddItem_Click;
            // 
            // frmStockTransfer
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(971, 654);
            Controls.Add(dgvItems);
            Controls.Add(pnlButtons);
            Controls.Add(pnlFooter);
            Controls.Add(pnlHeader);
            Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmStockTransfer";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            pnlHeader.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvItems).EndInit();
            pnlFooter.ResumeLayout(false);
            pnlFooter.PerformLayout();
            pnlButtons.ResumeLayout(false);
            ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlHeader, pnlFooter, pnlButtons;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblTransferNoLabel, lblTransferNo, lblDate, lblStatus, lblSource, lblDest;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.ComboBox cmbSource, cmbDest;
        private System.Windows.Forms.DataGridView dgvItems;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.Button btnAddItem, btnRemoveItem, btnSaveDraft, btnPost, btnClose;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}
