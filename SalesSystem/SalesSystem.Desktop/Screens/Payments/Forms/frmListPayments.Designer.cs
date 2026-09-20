namespace SalesSystem.Desktop.Screens.Payments.Forms
{
    partial class frmListPayments
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            components?.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblMode = new System.Windows.Forms.Label();
            this.tabPayments = new System.Windows.Forms.TabControl();
            this.tpCustomerPayments = new System.Windows.Forms.TabPage();
            this.dgvCustomerPayments = new System.Windows.Forms.DataGridView();
            this.pnlCustomerTop = new System.Windows.Forms.Panel();
            this.btnAddCustomerPayment = new System.Windows.Forms.Button();
            this.lblCustomerCount = new System.Windows.Forms.Label();
            this.lblCustomerCountTitle = new System.Windows.Forms.Label();
            this.tpSupplierPayments = new System.Windows.Forms.TabPage();
            this.dgvSupplierPayments = new System.Windows.Forms.DataGridView();
            this.pnlSupplierTop = new System.Windows.Forms.Panel();
            this.btnAddSupplierPayment = new System.Windows.Forms.Button();
            this.lblSupplierCount = new System.Windows.Forms.Label();
            this.lblSupplierCountTitle = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.cmsCustomerPayments = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiCustomerRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiCustomerEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCustomerAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsSupplierPayments = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiSupplierRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiSupplierEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSupplierAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPayments.SuspendLayout();
            this.tpCustomerPayments.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomerPayments)).BeginInit();
            this.pnlCustomerTop.SuspendLayout();
            this.tpSupplierPayments.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSupplierPayments)).BeginInit();
            this.pnlSupplierTop.SuspendLayout();
            this.cmsCustomerPayments.SuspendLayout();
            this.cmsSupplierPayments.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblMode
            // 
            this.lblMode.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblMode.AutoSize = true;
            this.lblMode.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblMode.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold);
            this.lblMode.ForeColor = System.Drawing.Color.Firebrick;
            this.lblMode.Location = new System.Drawing.Point(505, 20);
            this.lblMode.Name = "lblMode";
            this.lblMode.Size = new System.Drawing.Size(310, 39);
            this.lblMode.TabIndex = 30;
            this.lblMode.Text = "إدارة المقبوضات والمدفوعات";
            // 
            // tabPayments
            // 
            this.tabPayments.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabPayments.Controls.Add(this.tpCustomerPayments);
            this.tabPayments.Controls.Add(this.tpSupplierPayments);
            this.tabPayments.Font = new System.Drawing.Font("Tahoma", 9F);
            this.tabPayments.Location = new System.Drawing.Point(12, 50);
            this.tabPayments.Name = "tabPayments";
            this.tabPayments.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tabPayments.RightToLeftLayout = true;
            this.tabPayments.SelectedIndex = 0;
            this.tabPayments.Size = new System.Drawing.Size(1040, 530);
            this.tabPayments.TabIndex = 1;
            // 
            // tpCustomerPayments
            // 
            this.tpCustomerPayments.BackColor = System.Drawing.Color.White;
            this.tpCustomerPayments.Controls.Add(this.dgvCustomerPayments);
            this.tpCustomerPayments.Controls.Add(this.pnlCustomerTop);
            this.tpCustomerPayments.Location = new System.Drawing.Point(4, 28);
            this.tpCustomerPayments.Name = "tpCustomerPayments";
            this.tpCustomerPayments.Padding = new System.Windows.Forms.Padding(3);
            this.tpCustomerPayments.Size = new System.Drawing.Size(1032, 498);
            this.tpCustomerPayments.TabIndex = 0;
            this.tpCustomerPayments.Text = "\u0645\u0642\u0628\u0648\u0636\u0627\u062a \u0627\u0644\u0639\u0645\u0644\u0627\u0621";
            // 
            // dgvCustomerPayments
            // 
            this.dgvCustomerPayments.AllowUserToAddRows = false;
            this.dgvCustomerPayments.AllowUserToDeleteRows = false;
            this.dgvCustomerPayments.AllowUserToOrderColumns = true;
            this.dgvCustomerPayments.AllowUserToResizeRows = false;
            this.dgvCustomerPayments.BackgroundColor = System.Drawing.Color.White;
            this.dgvCustomerPayments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvCustomerPayments.ColumnHeadersHeight = 50;
            this.dgvCustomerPayments.ContextMenuStrip = this.cmsCustomerPayments;
            this.dgvCustomerPayments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCustomerPayments.Location = new System.Drawing.Point(3, 43);
            this.dgvCustomerPayments.MultiSelect = false;
            this.dgvCustomerPayments.Name = "dgvCustomerPayments";
            this.dgvCustomerPayments.ReadOnly = true;
            this.dgvCustomerPayments.RowHeadersWidth = 51;
            this.dgvCustomerPayments.RowTemplate.Height = 28;
            this.dgvCustomerPayments.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCustomerPayments.Size = new System.Drawing.Size(1026, 452);
            this.dgvCustomerPayments.TabIndex = 1;
            // 
            // pnlCustomerTop
            // 
            this.pnlCustomerTop.Controls.Add(this.btnAddCustomerPayment);
            this.pnlCustomerTop.Controls.Add(this.lblCustomerCount);
            this.pnlCustomerTop.Controls.Add(this.lblCustomerCountTitle);
            this.pnlCustomerTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCustomerTop.Height = 40;
            this.pnlCustomerTop.Name = "pnlCustomerTop";
            this.pnlCustomerTop.TabIndex = 0;
            // 
            // btnAddCustomerPayment
            // 
            this.btnAddCustomerPayment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddCustomerPayment.Location = new System.Drawing.Point(920, 5);
            this.btnAddCustomerPayment.Name = "btnAddCustomerPayment";
            this.btnAddCustomerPayment.Size = new System.Drawing.Size(106, 30);
            this.btnAddCustomerPayment.TabIndex = 2;
            this.btnAddCustomerPayment.Text = "\u0642\u0628\u0636 \u062c\u062f\u064a\u062f";
            this.btnAddCustomerPayment.UseVisualStyleBackColor = true;
            this.btnAddCustomerPayment.Click += new System.EventHandler(this.btnAddCustomerPayment_Click);
            // 
            // lblCustomerCount
            // 
            this.lblCustomerCount.AutoSize = true;
            this.lblCustomerCount.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblCustomerCount.Location = new System.Drawing.Point(50, 10);
            this.lblCustomerCount.Name = "lblCustomerCount";
            this.lblCustomerCount.Size = new System.Drawing.Size(22, 18);
            this.lblCustomerCount.TabIndex = 1;
            this.lblCustomerCount.Text = "0";
            // 
            // lblCustomerCountTitle
            // 
            this.lblCustomerCountTitle.AutoSize = true;
            this.lblCustomerCountTitle.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblCustomerCountTitle.Location = new System.Drawing.Point(10, 10);
            this.lblCustomerCountTitle.Name = "lblCustomerCountTitle";
            this.lblCustomerCountTitle.Size = new System.Drawing.Size(40, 18);
            this.lblCustomerCountTitle.TabIndex = 0;
            this.lblCustomerCountTitle.Text = "\u0627\u0644\u0639\u062f\u062f:";
            // 
            // tpSupplierPayments
            // 
            this.tpSupplierPayments.BackColor = System.Drawing.Color.White;
            this.tpSupplierPayments.Controls.Add(this.dgvSupplierPayments);
            this.tpSupplierPayments.Controls.Add(this.pnlSupplierTop);
            this.tpSupplierPayments.Location = new System.Drawing.Point(4, 28);
            this.tpSupplierPayments.Name = "tpSupplierPayments";
            this.tpSupplierPayments.Padding = new System.Windows.Forms.Padding(3);
            this.tpSupplierPayments.Size = new System.Drawing.Size(1032, 498);
            this.tpSupplierPayments.TabIndex = 1;
            this.tpSupplierPayments.Text = "\u0645\u062f\u0641\u0648\u0639\u0627\u062a \u0627\u0644\u0645\u0648\u0631\u062f\u064a\u0646";
            // 
            // dgvSupplierPayments
            // 
            this.dgvSupplierPayments.AllowUserToAddRows = false;
            this.dgvSupplierPayments.AllowUserToDeleteRows = false;
            this.dgvSupplierPayments.AllowUserToOrderColumns = true;
            this.dgvSupplierPayments.AllowUserToResizeRows = false;
            this.dgvSupplierPayments.BackgroundColor = System.Drawing.Color.White;
            this.dgvSupplierPayments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvSupplierPayments.ColumnHeadersHeight = 50;
            this.dgvSupplierPayments.ContextMenuStrip = this.cmsSupplierPayments;
            this.dgvSupplierPayments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSupplierPayments.Location = new System.Drawing.Point(3, 43);
            this.dgvSupplierPayments.MultiSelect = false;
            this.dgvSupplierPayments.Name = "dgvSupplierPayments";
            this.dgvSupplierPayments.ReadOnly = true;
            this.dgvSupplierPayments.RowHeadersWidth = 51;
            this.dgvSupplierPayments.RowTemplate.Height = 28;
            this.dgvSupplierPayments.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSupplierPayments.Size = new System.Drawing.Size(1026, 452);
            this.dgvSupplierPayments.TabIndex = 1;
            // 
            // pnlSupplierTop
            // 
            this.pnlSupplierTop.Controls.Add(this.btnAddSupplierPayment);
            this.pnlSupplierTop.Controls.Add(this.lblSupplierCount);
            this.pnlSupplierTop.Controls.Add(this.lblSupplierCountTitle);
            this.pnlSupplierTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSupplierTop.Height = 40;
            this.pnlSupplierTop.Name = "pnlSupplierTop";
            this.pnlSupplierTop.TabIndex = 0;
            // 
            // btnAddSupplierPayment
            // 
            this.btnAddSupplierPayment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddSupplierPayment.Location = new System.Drawing.Point(920, 5);
            this.btnAddSupplierPayment.Name = "btnAddSupplierPayment";
            this.btnAddSupplierPayment.Size = new System.Drawing.Size(106, 30);
            this.btnAddSupplierPayment.TabIndex = 2;
            this.btnAddSupplierPayment.Text = "\u0635\u0631\u0641 \u062c\u062f\u064a\u062f";
            this.btnAddSupplierPayment.UseVisualStyleBackColor = true;
            this.btnAddSupplierPayment.Click += new System.EventHandler(this.btnAddSupplierPayment_Click);
            // 
            // lblSupplierCount
            // 
            this.lblSupplierCount.AutoSize = true;
            this.lblSupplierCount.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblSupplierCount.Location = new System.Drawing.Point(50, 10);
            this.lblSupplierCount.Name = "lblSupplierCount";
            this.lblSupplierCount.Size = new System.Drawing.Size(22, 18);
            this.lblSupplierCount.TabIndex = 1;
            this.lblSupplierCount.Text = "0";
            // 
            // lblSupplierCountTitle
            // 
            this.lblSupplierCountTitle.AutoSize = true;
            this.lblSupplierCountTitle.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblSupplierCountTitle.Location = new System.Drawing.Point(10, 10);
            this.lblSupplierCountTitle.Name = "lblSupplierCountTitle";
            this.lblSupplierCountTitle.Size = new System.Drawing.Size(40, 18);
            this.lblSupplierCountTitle.TabIndex = 0;
            this.lblSupplierCountTitle.Text = "\u0627\u0644\u0639\u062f\u062f:";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.BackColor = System.Drawing.Color.White;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClose.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btnClose.Location = new System.Drawing.Point(12, 630);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(181, 61);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "إغلاق";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // cmsCustomerPayments
            // 
            this.cmsCustomerPayments.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmsCustomerPayments.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiCustomerRefresh,
            this.toolStripSeparator1,
            this.tsmiCustomerEdit,
            this.tsmiCustomerAdd});
            this.cmsCustomerPayments.Name = "cmsCustomerPayments";
            this.cmsCustomerPayments.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cmsCustomerPayments.Size = new System.Drawing.Size(161, 88);
            this.cmsCustomerPayments.Opening += new System.ComponentModel.CancelEventHandler(this.cmsCustomerPayments_Opening);
            // 
            // tsmiCustomerRefresh
            // 
            this.tsmiCustomerRefresh.Name = "tsmiCustomerRefresh";
            this.tsmiCustomerRefresh.Size = new System.Drawing.Size(160, 26);
            this.tsmiCustomerRefresh.Text = "\u062a\u062d\u062f\u064a\u062b";
            this.tsmiCustomerRefresh.Click += new System.EventHandler(this.tsmiCustomerRefresh_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(157, 6);
            // 
            // tsmiCustomerEdit
            // 
            this.tsmiCustomerEdit.Name = "tsmiCustomerEdit";
            this.tsmiCustomerEdit.Size = new System.Drawing.Size(160, 26);
            this.tsmiCustomerEdit.Text = "\u0641\u062a\u062d \u0627\u0644\u0633\u0646\u062f";
            this.tsmiCustomerEdit.Click += new System.EventHandler(this.tsmiCustomerEdit_Click);
            // 
            // tsmiCustomerAdd
            // 
            this.tsmiCustomerAdd.Name = "tsmiCustomerAdd";
            this.tsmiCustomerAdd.Size = new System.Drawing.Size(160, 26);
            this.tsmiCustomerAdd.Text = "\u0642\u0628\u0636 \u062c\u062f\u064a\u062f";
            this.tsmiCustomerAdd.Click += new System.EventHandler(this.tsmiCustomerAdd_Click);
            // 
            // cmsSupplierPayments
            // 
            this.cmsSupplierPayments.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmsSupplierPayments.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiSupplierRefresh,
            this.toolStripSeparator2,
            this.tsmiSupplierEdit,
            this.tsmiSupplierAdd});
            this.cmsSupplierPayments.Name = "cmsSupplierPayments";
            this.cmsSupplierPayments.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cmsSupplierPayments.Size = new System.Drawing.Size(161, 88);
            this.cmsSupplierPayments.Opening += new System.ComponentModel.CancelEventHandler(this.cmsSupplierPayments_Opening);
            // 
            // tsmiSupplierRefresh
            // 
            this.tsmiSupplierRefresh.Name = "tsmiSupplierRefresh";
            this.tsmiSupplierRefresh.Size = new System.Drawing.Size(160, 26);
            this.tsmiSupplierRefresh.Text = "\u062a\u062d\u062f\u064a\u062b";
            this.tsmiSupplierRefresh.Click += new System.EventHandler(this.tsmiSupplierRefresh_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(157, 6);
            // 
            // tsmiSupplierEdit
            // 
            this.tsmiSupplierEdit.Name = "tsmiSupplierEdit";
            this.tsmiSupplierEdit.Size = new System.Drawing.Size(160, 26);
            this.tsmiSupplierEdit.Text = "\u0641\u062a\u062d \u0627\u0644\u0633\u0646\u062f";
            this.tsmiSupplierEdit.Click += new System.EventHandler(this.tsmiSupplierEdit_Click);
            // 
            // tsmiSupplierAdd
            // 
            this.tsmiSupplierAdd.Name = "tsmiSupplierAdd";
            this.tsmiSupplierAdd.Size = new System.Drawing.Size(160, 26);
            this.tsmiSupplierAdd.Text = "\u0635\u0631\u0641 \u062c\u062f\u064a\u062f";
            this.tsmiSupplierAdd.Click += new System.EventHandler(this.tsmiSupplierAdd_Click);
            // 
            // frmListPayments
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(1200, 703);
            this.Controls.Add(this.tabPayments);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblMode);
            this.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "frmListPayments";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "إدارة المقبوضات والمدفوعات";
            this.Load += new System.EventHandler(this.frmListPayments_Load);
            this.tabPayments.ResumeLayout(false);
            this.tpCustomerPayments.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomerPayments)).EndInit();
            this.pnlCustomerTop.ResumeLayout(false);
            this.pnlCustomerTop.PerformLayout();
            this.tpSupplierPayments.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSupplierPayments)).EndInit();
            this.pnlSupplierTop.ResumeLayout(false);
            this.pnlSupplierTop.PerformLayout();
            this.cmsCustomerPayments.ResumeLayout(false);
            this.cmsSupplierPayments.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.TabControl tabPayments;
        private System.Windows.Forms.TabPage tpCustomerPayments;
        private System.Windows.Forms.TabPage tpSupplierPayments;
        private System.Windows.Forms.DataGridView dgvCustomerPayments;
        private System.Windows.Forms.Panel pnlCustomerTop;
        private System.Windows.Forms.Button btnAddCustomerPayment;
        private System.Windows.Forms.Label lblCustomerCount;
        private System.Windows.Forms.Label lblCustomerCountTitle;
        private System.Windows.Forms.DataGridView dgvSupplierPayments;
        private System.Windows.Forms.Panel pnlSupplierTop;
        private System.Windows.Forms.Button btnAddSupplierPayment;
        private System.Windows.Forms.Label lblSupplierCount;
        private System.Windows.Forms.Label lblSupplierCountTitle;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ContextMenuStrip cmsCustomerPayments;
        private System.Windows.Forms.ToolStripMenuItem tsmiCustomerRefresh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tsmiCustomerEdit;
        private System.Windows.Forms.ToolStripMenuItem tsmiCustomerAdd;
        private System.Windows.Forms.ContextMenuStrip cmsSupplierPayments;
        private System.Windows.Forms.ToolStripMenuItem tsmiSupplierRefresh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem tsmiSupplierEdit;
        private System.Windows.Forms.ToolStripMenuItem tsmiSupplierAdd;
    }
}
