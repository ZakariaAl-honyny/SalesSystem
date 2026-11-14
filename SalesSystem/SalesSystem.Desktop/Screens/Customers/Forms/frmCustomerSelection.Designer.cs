namespace SalesSystem.Desktop.Screens.Customers.Forms
{
    partial class frmCustomerSelection
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            components?.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            // RULE-N134: ألوان المصمّم = ثوابت GreenThemeHelper حرفياً (المصمّم = وقت التشغيل).
            this.components = new System.ComponentModel.Container();
            this.cmsRows = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsSelect = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.cmsRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.dgvCustomers = new System.Windows.Forms.DataGridView();
            this.btnSelect = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblNoData = new System.Windows.Forms.Label();
            this.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomers)).BeginInit();

            // txtSearch
            this.txtSearch.BackColor = System.Drawing.Color.White;
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearch.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular);
            this.txtSearch.Location = new System.Drawing.Point(12, 14);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(400, 30);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);

            // lblNoData
            this.lblNoData.AutoSize = true;
            this.lblNoData.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblNoData.ForeColor = System.Drawing.Color.FromArgb(14, 82, 52);
            this.lblNoData.Location = new System.Drawing.Point(420, 20);
            this.lblNoData.Name = "lblNoData";
            this.lblNoData.Size = new System.Drawing.Size(130, 24);
            this.lblNoData.TabIndex = 4;
            this.lblNoData.Text = "لا توجد بيانات";
            this.lblNoData.Visible = false;

            // dgvCustomers
            this.dgvCustomers.AllowUserToAddRows = false;
            this.dgvCustomers.AllowUserToDeleteRows = false;
            this.dgvCustomers.AllowUserToResizeRows = false;
            this.dgvCustomers.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(245, 252, 245);
            this.dgvCustomers.AutoGenerateColumns = false;
            this.dgvCustomers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCustomers.BackgroundColor = System.Drawing.Color.FromArgb(220, 240, 220);
            this.dgvCustomers.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvCustomers.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvCustomers.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvCustomers.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(14, 165, 103);
            this.dgvCustomers.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.dgvCustomers.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            this.dgvCustomers.ColumnHeadersDefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(14, 165, 103);
            this.dgvCustomers.ColumnHeadersDefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            this.dgvCustomers.ColumnHeadersHeight = 45;
            this.dgvCustomers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvCustomers.Columns.Add("Id", "\u0631\u0642\u0645 \u0627\u0644\u0639\u0645\u064a\u0644");
            this.dgvCustomers.Columns.Add("Name", "\u0627\u0633\u0645 \u0627\u0644\u0639\u0645\u064a\u0644");
            this.dgvCustomers.Columns.Add("Phone", "\u0631\u0642\u0645 \u0627\u0644\u0647\u0627\u062a\u0641");
            this.dgvCustomers.Columns.Add("Email", "\u0627\u0644\u0628\u0631\u064a\u062f \u0627\u0644\u0625\u0644\u0643\u062a\u0631\u0648\u0646\u064a");
            this.dgvCustomers.Columns["Id"].DataPropertyName = "Id";
            this.dgvCustomers.Columns["Id"].FillWeight = 15F;
            this.dgvCustomers.Columns["Name"].DataPropertyName = "Name";
            this.dgvCustomers.Columns["Name"].FillWeight = 40F;
            this.dgvCustomers.Columns["Phone"].DataPropertyName = "Phone";
            this.dgvCustomers.Columns["Phone"].FillWeight = 20F;
            this.dgvCustomers.Columns["Email"].DataPropertyName = "Email";
            this.dgvCustomers.Columns["Email"].FillWeight = 25F;
            this.dgvCustomers.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.dgvCustomers.DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 10F);
            this.dgvCustomers.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(33, 37, 41);
            this.dgvCustomers.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(255, 255, 180);
            this.dgvCustomers.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(33, 37, 41);
            this.dgvCustomers.EnableHeadersVisualStyles = false;
            this.dgvCustomers.GridColor = System.Drawing.Color.FromArgb(205, 225, 205);
            this.dgvCustomers.Location = new System.Drawing.Point(12, 50);
            this.dgvCustomers.MultiSelect = false;
            this.dgvCustomers.Name = "dgvCustomers";
            this.dgvCustomers.ContextMenuStrip = this.cmsRows;
            this.dgvCustomers.ReadOnly = true;
            this.dgvCustomers.RowHeadersVisible = false;
            this.dgvCustomers.RowHeadersWidth = 62;
            this.dgvCustomers.RowTemplate.Height = 32;
            this.dgvCustomers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCustomers.Size = new System.Drawing.Size(676, 380);
            this.dgvCustomers.TabIndex = 1;
            this.dgvCustomers.TabStop = false;
            this.dgvCustomers.SelectionChanged += new System.EventHandler(this.dgvCustomers_SelectionChanged);
            this.dgvCustomers.DoubleClick += new System.EventHandler(this.dgvCustomers_DoubleClick);
            this.dgvCustomers.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvCustomers_KeyDown);

            // btnSelect
            this.btnSelect.BackColor = System.Drawing.Color.FromArgb(40, 167, 69);
            this.btnSelect.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSelect.FlatAppearance.BorderSize = 0;
            this.btnSelect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelect.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btnSelect.ForeColor = System.Drawing.Color.White;
            this.btnSelect.Location = new System.Drawing.Point(570, 440);
            this.btnSelect.MinimumSize = new System.Drawing.Size(100, 35);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(118, 38);
            this.btnSelect.TabIndex = 2;
            this.btnSelect.Text = "\u062a\u062d\u062f\u064a\u062f";
            this.btnSelect.UseVisualStyleBackColor = false;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);

            // btnCancel
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(108, 117, 125);
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(440, 440);
            this.btnCancel.MinimumSize = new System.Drawing.Size(100, 35);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(118, 38);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "\u0625\u0644\u063a\u0627\u0621";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

            // cmsRows
            // RULE-N23: قائمة سياق على الشبكة — «اختيار» يستدعي نفس زر الشاشة (بلا منطق مكرر).
            this.cmsRows.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.cmsSelect, this.cmsSeparator, this.cmsRefresh });
            this.cmsRows.Name = "cmsRows";
            this.cmsRows.Size = new System.Drawing.Size(240, 104);
            this.cmsRows.Opening += new System.ComponentModel.CancelEventHandler(this.cmsRows_Opening);

            // cmsSelect
            this.cmsSelect.Name = "cmsSelect";
            this.cmsSelect.Size = new System.Drawing.Size(239, 30);
            this.cmsSelect.Text = "✔ اختيار";
            this.cmsSelect.Click += new System.EventHandler(this.cmsSelect_Click);

            // cmsSeparator
            this.cmsSeparator.Name = "cmsSeparator";
            this.cmsSeparator.Size = new System.Drawing.Size(236, 6);

            // cmsRefresh
            this.cmsRefresh.Name = "cmsRefresh";
            this.cmsRefresh.Size = new System.Drawing.Size(239, 30);
            this.cmsRefresh.Text = "🔄 تحديث (F5)";
            this.cmsRefresh.Click += new System.EventHandler(this.cmsRefresh_Click);

            // frmCustomerSelection
            this.AcceptButton = this.btnSelect;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(170, 220, 170);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(700, 490);
            this.Controls.Add(this.lblNoData);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.dgvCustomers);
            this.Controls.Add(this.txtSearch);
            this.Font = new System.Drawing.Font("Tahoma", 12F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCustomerSelection";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "\u0627\u062e\u062a\u064a\u0627\u0631 \u0639\u0645\u064a\u0644";
            this.Load += new System.EventHandler(this.frmCustomerSelection_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomers)).EndInit();
            this.cmsRows.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.DataGridView dgvCustomers;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblNoData;
        private System.Windows.Forms.ContextMenuStrip cmsRows;
        private System.Windows.Forms.ToolStripMenuItem cmsSelect;
        private System.Windows.Forms.ToolStripSeparator cmsSeparator;
        private System.Windows.Forms.ToolStripMenuItem cmsRefresh;
    }
}
