namespace SalesSystem.Desktop.Suppliers.Forms
{
    partial class frmSupplierSelection
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
            components = new System.ComponentModel.Container();
            cmsRows = new ContextMenuStrip(components);
            cmsSelect = new ToolStripMenuItem();
            cmsSeparator = new ToolStripSeparator();
            cmsRefresh = new ToolStripMenuItem();
            txtSearch = new TextBox();
            dgvSuppliers = new DataGridView();
            btnSelect = new Button();
            btnCancel = new Button();
            lblNoData = new Label();
            dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dgvSuppliers).BeginInit();
            SuspendLayout();
            // 
            // txtSearch
            // 
            txtSearch.BackColor = System.Drawing.Color.White;
            txtSearch.BorderStyle = BorderStyle.FixedSingle;
            txtSearch.Font = new Font("Tahoma", 10F);
            txtSearch.Location = new Point(12, 14);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(400, 30);
            txtSearch.TabIndex = 0;
            txtSearch.TextChanged += txtSearch_TextChanged;
            txtSearch.KeyDown += txtSearch_KeyDown;
            // 
            // lblNoData
            // 
            lblNoData.AutoSize = true;
            lblNoData.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblNoData.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            lblNoData.Location = new Point(420, 20);
            lblNoData.Name = "lblNoData";
            lblNoData.Size = new Size(130, 24);
            lblNoData.TabIndex = 4;
            lblNoData.Text = "لا توجد بيانات";
            lblNoData.Visible = false;
            // 
            // dgvSuppliers
            // 
            dgvSuppliers.AllowUserToAddRows = false;
            dgvSuppliers.AllowUserToDeleteRows = false;
            dgvSuppliers.AllowUserToResizeRows = false;
            dgvSuppliers.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(248), Convert.ToInt32(249), Convert.ToInt32(250));
            dgvSuppliers.AutoGenerateColumns = false;
            dgvSuppliers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvSuppliers.BackgroundColor = System.Drawing.Color.White;
            dgvSuppliers.BorderStyle = BorderStyle.None;
            dgvSuppliers.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvSuppliers.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvSuppliers.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(52), Convert.ToInt32(58), Convert.ToInt32(64));
            dgvSuppliers.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            dgvSuppliers.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dgvSuppliers.ColumnHeadersDefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(52), Convert.ToInt32(58), Convert.ToInt32(64));
            dgvSuppliers.ColumnHeadersDefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            dgvSuppliers.ColumnHeadersHeight = 45;
            dgvSuppliers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvSuppliers.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn1, dataGridViewTextBoxColumn2, dataGridViewTextBoxColumn3, dataGridViewTextBoxColumn4 });
            dgvSuppliers.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            dgvSuppliers.DefaultCellStyle.Font = new Font("Tahoma", 10F);
            dgvSuppliers.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            dgvSuppliers.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(255), Convert.ToInt32(243), Convert.ToInt32(205));
            dgvSuppliers.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            dgvSuppliers.EnableHeadersVisualStyles = false;
            dgvSuppliers.GridColor = System.Drawing.Color.FromArgb(Convert.ToInt32(222), Convert.ToInt32(226), Convert.ToInt32(230));
            dgvSuppliers.Location = new Point(12, 50);
            dgvSuppliers.MultiSelect = false;
            dgvSuppliers.Name = "dgvSuppliers";
            dgvSuppliers.ContextMenuStrip = cmsRows;
            dgvSuppliers.ReadOnly = true;
            dgvSuppliers.RowHeadersVisible = false;
            dgvSuppliers.RowHeadersWidth = 62;
            dgvSuppliers.RowTemplate.Height = 32;
            dgvSuppliers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSuppliers.Size = new Size(676, 380);
            dgvSuppliers.TabIndex = 1;
            dgvSuppliers.TabStop = false;
            dgvSuppliers.SelectionChanged += dgvSuppliers_SelectionChanged;
            dgvSuppliers.DoubleClick += dgvSuppliers_DoubleClick;
            dgvSuppliers.KeyDown += dgvSuppliers_KeyDown;
            // 
            // btnSelect
            // 
            btnSelect.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(40), Convert.ToInt32(167), Convert.ToInt32(69));
            btnSelect.Cursor = Cursors.Hand;
            btnSelect.FlatAppearance.BorderSize = 0;
            btnSelect.FlatStyle = FlatStyle.Flat;
            btnSelect.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnSelect.ForeColor = System.Drawing.Color.White;
            btnSelect.Location = new Point(570, 440);
            btnSelect.MinimumSize = new Size(100, 35);
            btnSelect.Name = "btnSelect";
            btnSelect.Size = new Size(118, 38);
            btnSelect.TabIndex = 2;
            btnSelect.Text = "تحديد";
            btnSelect.UseVisualStyleBackColor = false;
            btnSelect.Click += btnSelect_Click;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(108), Convert.ToInt32(117), Convert.ToInt32(125));
            btnCancel.Cursor = Cursors.Hand;
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnCancel.ForeColor = System.Drawing.Color.White;
            btnCancel.Location = new Point(440, 440);
            btnCancel.MinimumSize = new Size(100, 35);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(118, 38);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "إلغاء";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // cmsRows
            // 
            // RULE-N23: قائمة سياق على الشبكة — «اختيار» يستدعي نفس زر الشاشة (بلا منطق مكرر).
            cmsRows.Items.AddRange(new ToolStripItem[] { cmsSelect, cmsSeparator, cmsRefresh });
            cmsRows.Name = "cmsRows";
            cmsRows.Size = new Size(240, 104);
            cmsRows.Opening += cmsRows_Opening;
            // 
            // cmsSelect
            // 
            cmsSelect.Name = "cmsSelect";
            cmsSelect.Size = new Size(239, 30);
            cmsSelect.Text = "✔ اختيار";
            cmsSelect.Click += cmsSelect_Click;
            // 
            // cmsSeparator
            // 
            cmsSeparator.Name = "cmsSeparator";
            cmsSeparator.Size = new Size(236, 6);
            // 
            // cmsRefresh
            // 
            cmsRefresh.Name = "cmsRefresh";
            cmsRefresh.Size = new Size(239, 30);
            cmsRefresh.Text = "🔄 تحديث (F5)";
            cmsRefresh.Click += cmsRefresh_Click;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewTextBoxColumn1.DataPropertyName = "Id";
            dataGridViewTextBoxColumn1.FillWeight = 15F;
            dataGridViewTextBoxColumn1.HeaderText = "رقم المورد";
            dataGridViewTextBoxColumn1.MinimumWidth = 8;
            dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewTextBoxColumn2.DataPropertyName = "Name";
            dataGridViewTextBoxColumn2.FillWeight = 40F;
            dataGridViewTextBoxColumn2.HeaderText = "اسم المورد";
            dataGridViewTextBoxColumn2.MinimumWidth = 8;
            dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            dataGridViewTextBoxColumn3.DataPropertyName = "Phone";
            dataGridViewTextBoxColumn3.FillWeight = 20F;
            dataGridViewTextBoxColumn3.HeaderText = "رقم الهاتف";
            dataGridViewTextBoxColumn3.MinimumWidth = 8;
            dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            dataGridViewTextBoxColumn4.DataPropertyName = "Email";
            dataGridViewTextBoxColumn4.FillWeight = 25F;
            dataGridViewTextBoxColumn4.HeaderText = "البريد الإلكتروني";
            dataGridViewTextBoxColumn4.MinimumWidth = 8;
            dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // frmSupplierSelection
            // 
            AcceptButton = btnSelect;
            AutoScaleMode = AutoScaleMode.None;
            BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(240), Convert.ToInt32(242), Convert.ToInt32(245));
            CancelButton = btnCancel;
            ClientSize = new Size(700, 490);
            Controls.Add(lblNoData);
            Controls.Add(btnCancel);
            Controls.Add(btnSelect);
            Controls.Add(dgvSuppliers);
            Controls.Add(txtSearch);
            Font = new Font("Tahoma", 12F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmSupplierSelection";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "اختيار مورد";
            Load += frmSupplierSelection_Load;
            ((System.ComponentModel.ISupportInitialize)dgvSuppliers).EndInit();
            cmsRows.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.DataGridView dgvSuppliers;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblNoData;
        private System.Windows.Forms.ContextMenuStrip cmsRows;
        private System.Windows.Forms.ToolStripMenuItem cmsSelect;
        private System.Windows.Forms.ToolStripSeparator cmsSeparator;
        private System.Windows.Forms.ToolStripMenuItem cmsRefresh;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
    }
}
