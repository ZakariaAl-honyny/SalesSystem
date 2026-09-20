namespace SalesSystem.Desktop.Screens.Payments.Forms
{
    partial class frmListSupplierPayments
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
            this.dgvSupplierPayments = new System.Windows.Forms.DataGridView();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.btnAddNew = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblRecordsCountTitle = new System.Windows.Forms.Label();
            this.lblRecordsCount = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.cmsSupplierPayments = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAdd = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSupplierPayments)).BeginInit();
            this.pnlTop.SuspendLayout();
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
            this.lblMode.Location = new System.Drawing.Point(470, 20);
            this.lblMode.Name = "lblMode";
            this.lblMode.Size = new System.Drawing.Size(260, 39);
            this.lblMode.TabIndex = 30;
            this.lblMode.Text = "المدفوعات للموردين";
            // 
            // dgvSupplierPayments
            // 
            this.dgvSupplierPayments.AllowUserToAddRows = false;
            this.dgvSupplierPayments.AllowUserToDeleteRows = false;
            this.dgvSupplierPayments.AllowUserToOrderColumns = true;
            this.dgvSupplierPayments.BackgroundColor = System.Drawing.Color.White;
            this.dgvSupplierPayments.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvSupplierPayments.ColumnHeadersHeight = 50;
            this.dgvSupplierPayments.ContextMenuStrip = this.cmsSupplierPayments;
            this.dgvSupplierPayments.Location = new System.Drawing.Point(12, 160);
            this.dgvSupplierPayments.MultiSelect = false;
            this.dgvSupplierPayments.Name = "dgvSupplierPayments";
            this.dgvSupplierPayments.ReadOnly = true;
            this.dgvSupplierPayments.RowHeadersWidth = 62;
            this.dgvSupplierPayments.RowTemplate.Height = 28;
            this.dgvSupplierPayments.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSupplierPayments.Size = new System.Drawing.Size(1176, 470);
            this.dgvSupplierPayments.TabIndex = 1;
            this.dgvSupplierPayments.TabStop = false;
            // 
            // pnlTop
            // 
            this.pnlTop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlTop.Controls.Add(this.btnAddNew);
            this.pnlTop.Controls.Add(this.btnRefresh);
            this.pnlTop.Controls.Add(this.txtSearch);
            this.pnlTop.Controls.Add(this.lblRecordsCountTitle);
            this.pnlTop.Controls.Add(this.lblRecordsCount);
            this.pnlTop.Location = new System.Drawing.Point(0, 42);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(900, 48);
            this.pnlTop.TabIndex = 2;
            // 
            // btnAddNew
            // 
            this.btnAddNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddNew.BackColor = System.Drawing.Color.White;
            this.btnAddNew.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnAddNew.FlatAppearance.BorderSize = 2;
            this.btnAddNew.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAddNew.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btnAddNew.Location = new System.Drawing.Point(790, 8);
            this.btnAddNew.Name = "btnAddNew";
            this.btnAddNew.Size = new System.Drawing.Size(98, 32);
            this.btnAddNew.TabIndex = 4;
            this.btnAddNew.Text = "+ صرف جديد";
            this.btnAddNew.UseVisualStyleBackColor = false;
            this.btnAddNew.Click += new System.EventHandler(this.btnAddNew_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.BackColor = System.Drawing.Color.White;
            this.btnRefresh.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnRefresh.FlatAppearance.BorderSize = 2;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnRefresh.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.Location = new System.Drawing.Point(686, 8);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(98, 32);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "تحديث";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearch.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtSearch.Location = new System.Drawing.Point(472, 10);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtSearch.Size = new System.Drawing.Size(200, 28);
            this.txtSearch.TabIndex = 2;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // lblRecordsCountTitle
            // 
            this.lblRecordsCountTitle.AutoSize = true;
            this.lblRecordsCountTitle.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblRecordsCountTitle.Location = new System.Drawing.Point(60, 13);
            this.lblRecordsCountTitle.Name = "lblRecordsCountTitle";
            this.lblRecordsCountTitle.Size = new System.Drawing.Size(55, 21);
            this.lblRecordsCountTitle.TabIndex = 1;
            this.lblRecordsCountTitle.Text = "العدد:";
            // 
            // lblRecordsCount
            // 
            this.lblRecordsCount.AutoSize = true;
            this.lblRecordsCount.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblRecordsCount.Location = new System.Drawing.Point(15, 13);
            this.lblRecordsCount.Name = "lblRecordsCount";
            this.lblRecordsCount.Size = new System.Drawing.Size(40, 21);
            this.lblRecordsCount.TabIndex = 0;
            this.lblRecordsCount.Text = "[0]";
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
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "إغلاق";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // cmsSupplierPayments
            // 
            this.cmsSupplierPayments.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmsSupplierPayments.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiRefresh,
            this.toolStripSeparator1,
            this.tsmiEdit,
            this.tsmiAdd});
            this.cmsSupplierPayments.Name = "cmsSupplierPayments";
            this.cmsSupplierPayments.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cmsSupplierPayments.Size = new System.Drawing.Size(161, 88);
            this.cmsSupplierPayments.Opening += new System.ComponentModel.CancelEventHandler(this.cmsSupplierPayments_Opening);
            // 
            // tsmiRefresh
            // 
            this.tsmiRefresh.Name = "tsmiRefresh";
            this.tsmiRefresh.Size = new System.Drawing.Size(160, 26);
            this.tsmiRefresh.Text = "تحديث";
            this.tsmiRefresh.Click += new System.EventHandler(this.tsmiRefresh_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(157, 6);
            // 
            // tsmiEdit
            // 
            this.tsmiEdit.Name = "tsmiEdit";
            this.tsmiEdit.Size = new System.Drawing.Size(160, 26);
            this.tsmiEdit.Text = "عرض السند";
            this.tsmiEdit.Click += new System.EventHandler(this.tsmiEdit_Click);
            // 
            // tsmiAdd
            // 
            this.tsmiAdd.Name = "tsmiAdd";
            this.tsmiAdd.Size = new System.Drawing.Size(160, 26);
            this.tsmiAdd.Text = "صرف جديد";
            this.tsmiAdd.Click += new System.EventHandler(this.tsmiAdd_Click);
            // 
            // frmListSupplierPayments
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(1200, 703);
            this.Controls.Add(this.dgvSupplierPayments);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblMode);
            this.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "frmListSupplierPayments";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "المدفوعات للموردين";
            this.Load += new System.EventHandler(this.frmListSupplierPayments_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSupplierPayments)).EndInit();
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.cmsSupplierPayments.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.DataGridView dgvSupplierPayments;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Button btnAddNew;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblRecordsCountTitle;
        private System.Windows.Forms.Label lblRecordsCount;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ContextMenuStrip cmsSupplierPayments;
        private System.Windows.Forms.ToolStripMenuItem tsmiRefresh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tsmiEdit;
        private System.Windows.Forms.ToolStripMenuItem tsmiAdd;
    }
}
