namespace SalesSystem.Desktop.Screens.Roles.Forms
{
    partial class frmListRoles
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
            components = new System.ComponentModel.Container();
            lblMode = new Label();
            cbFilterBy = new ComboBox();
            txtFilterValue = new TextBox();
            dgvRoles = new DataGridView();
            cmsRoles = new ContextMenuStrip(components);
            addNewToolStripMenuItem = new ToolStripMenuItem();
            editToolStripMenuItem = new ToolStripMenuItem();
            deleteToolStripMenuItem = new ToolStripMenuItem();
            refreshToolStripMenuItem = new ToolStripMenuItem();
            btnAdd = new Button();
            btnClose = new Button();
            lblRecordsCount = new Label();
            label2 = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvRoles).BeginInit();
            cmsRoles.SuspendLayout();
            SuspendLayout();
            // 
            // lblMode
            // 
            lblMode.Anchor = AnchorStyles.Top;
            lblMode.AutoSize = true;
            lblMode.BackColor = Color.WhiteSmoke;
            lblMode.FlatStyle = FlatStyle.Flat;
            lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            lblMode.ForeColor = Color.Firebrick;
            lblMode.Location = new Point(492, 20);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(216, 39);
            lblMode.TabIndex = 30;
            lblMode.Text = "إدارة الأدوار والصلاحيات";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label2.Location = new Point(21, 115);
            label2.Name = "label2";
            label2.Size = new Size(130, 24);
            label2.TabIndex = 6;
            label2.Text = "البحث عن طريق:";
            // 
            // cbFilterBy
            // 
            cbFilterBy.DropDownStyle = ComboBoxStyle.DropDownList;
            cbFilterBy.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            cbFilterBy.FormattingEnabled = true;
            cbFilterBy.Items.AddRange(new object[] { "لا شيئ", "رقم الدور", "اسم الدور" });
            cbFilterBy.Location = new Point(200, 111);
            cbFilterBy.Name = "cbFilterBy";
            cbFilterBy.Size = new Size(260, 32);
            cbFilterBy.TabIndex = 1;
            cbFilterBy.SelectedIndexChanged += cbFilterBy_SelectedIndexChanged;
            // 
            // txtFilterValue
            // 
            txtFilterValue.Font = new Font("Tahoma", 10F);
            txtFilterValue.Location = new Point(470, 111);
            txtFilterValue.Name = "txtFilterValue";
            txtFilterValue.Size = new Size(500, 32);
            txtFilterValue.TabIndex = 2;
            txtFilterValue.Visible = false;
            txtFilterValue.TextChanged += txtFilterValue_TextChanged;
            txtFilterValue.KeyPress += txtFilterValue_KeyPress;
            // 
            // dgvRoles
            // 
            dgvRoles.AllowUserToAddRows = false;
            dgvRoles.AllowUserToDeleteRows = false;
            dgvRoles.AllowUserToOrderColumns = true;
            dgvRoles.BackgroundColor = SystemColors.Window;
            dgvRoles.BorderStyle = BorderStyle.Fixed3D;
            dgvRoles.ColumnHeadersHeight = 50;
            dgvRoles.ContextMenuStrip = cmsRoles;
            dgvRoles.Location = new Point(12, 160);
            dgvRoles.MultiSelect = false;
            dgvRoles.Name = "dgvRoles";
            dgvRoles.ReadOnly = true;
            dgvRoles.RowHeadersWidth = 62;
            dgvRoles.RowTemplate.Height = 28;
            dgvRoles.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRoles.Size = new Size(1176, 480);
            dgvRoles.TabIndex = 3;
            dgvRoles.TabStop = false;
            dgvRoles.DoubleClick += dgvRoles_DoubleClick;
            // 
            // cmsRoles
            // 
            cmsRoles.ImageScalingSize = new Size(20, 20);
            cmsRoles.Items.AddRange(new ToolStripItem[] { addNewToolStripMenuItem, editToolStripMenuItem, deleteToolStripMenuItem, refreshToolStripMenuItem });
            cmsRoles.Name = "cmsRoles";
            cmsRoles.RightToLeft = RightToLeft.Yes;
            cmsRoles.Size = new Size(215, 132);
            cmsRoles.Opening += cmsRoles_Opening;
            // 
            // addNewToolStripMenuItem
            // 
            addNewToolStripMenuItem.Name = "addNewToolStripMenuItem";
            addNewToolStripMenuItem.Size = new Size(214, 32);
            addNewToolStripMenuItem.Text = "إضافة دور جديد";
            addNewToolStripMenuItem.Click += addNewToolStripMenuItem_Click;
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new Size(214, 32);
            editToolStripMenuItem.Text = "تعديل الدور";
            editToolStripMenuItem.Click += editToolStripMenuItem_Click;
            // 
            // deleteToolStripMenuItem
            // 
            deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            deleteToolStripMenuItem.Size = new Size(214, 32);
            deleteToolStripMenuItem.Text = "إلغاء تنشيط الدور";
            deleteToolStripMenuItem.Click += deleteToolStripMenuItem_Click;
            // 
            // refreshToolStripMenuItem
            // 
            refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            refreshToolStripMenuItem.Size = new Size(214, 32);
            refreshToolStripMenuItem.Text = "تحديث";
            refreshToolStripMenuItem.Click += refreshToolStripMenuItem_Click;
            // 
            // btnAdd
            // 
            btnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAdd.BackColor = Color.White;
            btnAdd.FlatAppearance.BorderColor = Color.Black;
            btnAdd.FlatAppearance.BorderSize = 2;
            btnAdd.FlatStyle = FlatStyle.Popup;
            btnAdd.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnAdd.Location = new Point(1103, 80);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(75, 75);
            btnAdd.TabIndex = 4;
            btnAdd.Text = "+ جديد";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnClose.BackColor = Color.White;
            btnClose.DialogResult = DialogResult.Cancel;
            btnClose.FlatStyle = FlatStyle.Popup;
            btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnClose.Location = new Point(12, 650);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(181, 61);
            btnClose.TabIndex = 0;
            btnClose.Text = "إغلاق";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // lblRecordsCount
            // 
            lblRecordsCount.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            lblRecordsCount.AutoSize = true;
            lblRecordsCount.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblRecordsCount.Location = new Point(1100, 665);
            lblRecordsCount.Name = "lblRecordsCount";
            lblRecordsCount.Size = new Size(52, 24);
            lblRecordsCount.TabIndex = 7;
            lblRecordsCount.Text = "[0]";
            // 
            // frmListRoles
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            CancelButton = btnClose;
            ClientSize = new Size(1200, 723);
            Controls.Add(lblRecordsCount);
            Controls.Add(label2);
            Controls.Add(btnClose);
            Controls.Add(btnAdd);
            Controls.Add(dgvRoles);
            Controls.Add(txtFilterValue);
            Controls.Add(cbFilterBy);
            Controls.Add(lblMode);
            Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "frmListRoles";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "إدارة الأدوار والصلاحيات";
            Load += frmListRoles_Load;
            ((System.ComponentModel.ISupportInitialize)dgvRoles).EndInit();
            cmsRoles.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.ComboBox cbFilterBy;
        private System.Windows.Forms.TextBox txtFilterValue;
        private System.Windows.Forms.DataGridView dgvRoles;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblRecordsCount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ContextMenuStrip cmsRoles;
        private System.Windows.Forms.ToolStripMenuItem addNewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
    }
}
