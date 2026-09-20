namespace SalesManagement.Companies
{
    partial class frmListCompanies
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
            dgvCompanies = new DataGridView();
            cmsCompanies = new ContextMenuStrip(components);
            refresh_toolStripMenuItem1 = new ToolStripMenuItem();
            toolStripMenuItem2 = new ToolStripSeparator();
            toolStripMenuItem3 = new ToolStripSeparator();
            editToolStripMenuItem = new ToolStripMenuItem();
            addNewPeraonToolStripMenuItem = new ToolStripMenuItem();
            DeletePerson_toolStripMenuItem4 = new ToolStripMenuItem();
            toolStripMenuItem5 = new ToolStripSeparator();
            lblMode = new Label();
            txtFilterValue = new TextBox();
            cbFilterBy = new ComboBox();
            label2 = new Label();
            lblRecordsCount = new Label();
            label1 = new Label();
            btnClose = new Button();
            pictureBox1 = new PictureBox();
            btnAddNewCompany = new Button();
            panel1 = new Panel();
            ((System.ComponentModel.ISupportInitialize)dgvCompanies).BeginInit();
            cmsCompanies.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // dgvCompanies
            // 
            dgvCompanies.AllowUserToAddRows = false;
            dgvCompanies.AllowUserToDeleteRows = false;
            dgvCompanies.AllowUserToOrderColumns = true;
            dgvCompanies.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvCompanies.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvCompanies.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvCompanies.BackgroundColor = Color.White;
            dgvCompanies.BorderStyle = BorderStyle.Fixed3D;
            dgvCompanies.ColumnHeadersHeight = 50;
            dgvCompanies.ContextMenuStrip = cmsCompanies;
            dgvCompanies.Location = new Point(8, 284);
            dgvCompanies.Margin = new Padding(4);
            dgvCompanies.MultiSelect = false;
            dgvCompanies.Name = "dgvCompanies";
            dgvCompanies.ReadOnly = true;
            dgvCompanies.RowHeadersWidth = 50;
            dgvCompanies.RowTemplate.Height = 28;
            dgvCompanies.RowTemplate.Resizable = DataGridViewTriState.True;
            dgvCompanies.Size = new Size(1562, 483);
            dgvCompanies.TabIndex = 47;
            dgvCompanies.TabStop = false;
            dgvCompanies.DoubleClick += dgvAllCompanies_DoubleClick;
            // 
            // cmsCompanies
            // 
            cmsCompanies.BackColor = Color.White;
            cmsCompanies.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmsCompanies.ImageScalingSize = new Size(24, 24);
            cmsCompanies.Items.AddRange(new ToolStripItem[] { refresh_toolStripMenuItem1, toolStripMenuItem2, toolStripMenuItem3, editToolStripMenuItem, addNewPeraonToolStripMenuItem, DeletePerson_toolStripMenuItem4, toolStripMenuItem5 });
            cmsCompanies.Name = "contextMenuStrip1";
            cmsCompanies.RightToLeft = RightToLeft.Yes;
            cmsCompanies.Size = new Size(213, 150);
            cmsCompanies.Opening += cmsCompanies_Opening;
            // 
            // refresh_toolStripMenuItem1
            // 
            refresh_toolStripMenuItem1.Image = Properties.Resources.refresh_40;
            refresh_toolStripMenuItem1.Name = "refresh_toolStripMenuItem1";
            refresh_toolStripMenuItem1.ShortcutKeys = Keys.Control | Keys.R;
            refresh_toolStripMenuItem1.Size = new Size(212, 32);
            refresh_toolStripMenuItem1.Text = "&تحديث";
            refresh_toolStripMenuItem1.Click += refresh_toolStripMenuItem1_Click;
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new Size(209, 6);
            // 
            // toolStripMenuItem3
            // 
            toolStripMenuItem3.Name = "toolStripMenuItem3";
            toolStripMenuItem3.Size = new Size(209, 6);
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.Image = Properties.Resources.edit_32;
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.E;
            editToolStripMenuItem.Size = new Size(212, 32);
            editToolStripMenuItem.Text = "تعديل";
            editToolStripMenuItem.Click += editToolStripMenuItem_Click;
            // 
            // addNewPeraonToolStripMenuItem
            // 
            addNewPeraonToolStripMenuItem.Image = Properties.Resources.Add_New_User_72;
            addNewPeraonToolStripMenuItem.Name = "addNewPeraonToolStripMenuItem";
            addNewPeraonToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.A;
            addNewPeraonToolStripMenuItem.Size = new Size(212, 32);
            addNewPeraonToolStripMenuItem.Text = "&إضافة";
            addNewPeraonToolStripMenuItem.Click += addNewPeraonToolStripMenuItem_Click;
            // 
            // DeletePerson_toolStripMenuItem4
            // 
            DeletePerson_toolStripMenuItem4.Image = Properties.Resources.Delete_32;
            DeletePerson_toolStripMenuItem4.Name = "DeletePerson_toolStripMenuItem4";
            DeletePerson_toolStripMenuItem4.Size = new Size(212, 32);
            DeletePerson_toolStripMenuItem4.Text = "حذف";
            DeletePerson_toolStripMenuItem4.Click += DeleteCompany_toolStripMenuItem4_Click;
            // 
            // toolStripMenuItem5
            // 
            toolStripMenuItem5.Name = "toolStripMenuItem5";
            toolStripMenuItem5.Size = new Size(209, 6);
            // 
            // lblMode
            // 
            lblMode.Anchor = AnchorStyles.Top;
            lblMode.AutoSize = true;
            lblMode.BackColor = Color.WhiteSmoke;
            lblMode.FlatStyle = FlatStyle.Flat;
            lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblMode.ForeColor = Color.Firebrick;
            lblMode.Location = new Point(620, 166);
            lblMode.Margin = new Padding(4);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(338, 39);
            lblMode.TabIndex = 45;
            lblMode.Text = "شاشة إدراة الشركات";
            // 
            // txtFilterValue
            // 
            txtFilterValue.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtFilterValue.Location = new Point(524, 244);
            txtFilterValue.Margin = new Padding(4);
            txtFilterValue.Name = "txtFilterValue";
            txtFilterValue.Size = new Size(403, 32);
            txtFilterValue.TabIndex = 2;
            txtFilterValue.Visible = false;
            txtFilterValue.TextChanged += txtFilter_TextChanged;
            txtFilterValue.KeyPress += txtFilterValue_KeyPress;
            // 
            // cbFilterBy
            // 
            cbFilterBy.BackColor = SystemColors.Control;
            cbFilterBy.DropDownStyle = ComboBoxStyle.DropDownList;
            cbFilterBy.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            cbFilterBy.FormattingEnabled = true;
            cbFilterBy.Items.AddRange(new object[] { "لا شيئ", "رقم الشركة", "اسم الشركة", "هاتف الشركة", "عنوان الشركة", "تفاصيل الشركة" });
            cbFilterBy.Location = new Point(187, 244);
            cbFilterBy.Margin = new Padding(4);
            cbFilterBy.Name = "cbFilterBy";
            cbFilterBy.Size = new Size(329, 32);
            cbFilterBy.TabIndex = 1;
            cbFilterBy.SelectedIndexChanged += cbFilterBy_SelectedIndexChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(8, 247);
            label2.Margin = new Padding(4);
            label2.Name = "label2";
            label2.Size = new Size(171, 24);
            label2.TabIndex = 43;
            label2.Text = "البحث عن طريق:";
            // 
            // lblRecordsCount
            // 
            lblRecordsCount.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            lblRecordsCount.AutoSize = true;
            lblRecordsCount.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblRecordsCount.Location = new Point(1498, 775);
            lblRecordsCount.Margin = new Padding(4);
            lblRecordsCount.Name = "lblRecordsCount";
            lblRecordsCount.Size = new Size(72, 24);
            lblRecordsCount.TabIndex = 42;
            lblRecordsCount.Text = "[????]";
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(1341, 775);
            label1.Margin = new Padding(4);
            label1.Name = "label1";
            label1.Size = new Size(143, 24);
            label1.TabIndex = 41;
            label1.Text = "عدد الشركات:";
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnClose.BackColor = Color.White;
            btnClose.DialogResult = DialogResult.Cancel;
            btnClose.FlatStyle = FlatStyle.Popup;
            btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnClose.Image = Properties.Resources.Close_32;
            btnClose.ImageAlign = ContentAlignment.MiddleRight;
            btnClose.Location = new Point(8, 775);
            btnClose.Margin = new Padding(4);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(181, 61);
            btnClose.TabIndex = 3;
            btnClose.Text = "إغلاق";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.Top;
            pictureBox1.Image = Properties.Resources.Local_32;
            pictureBox1.Location = new Point(714, 8);
            pictureBox1.Margin = new Padding(4);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(150, 150);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 46;
            pictureBox1.TabStop = false;
            // 
            // btnAddNewCompany
            // 
            btnAddNewCompany.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAddNewCompany.BackColor = Color.White;
            btnAddNewCompany.FlatAppearance.BorderColor = Color.Black;
            btnAddNewCompany.FlatAppearance.BorderSize = 2;
            btnAddNewCompany.FlatStyle = FlatStyle.Popup;
            btnAddNewCompany.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAddNewCompany.Image = Properties.Resources.Add_Person_40;
            btnAddNewCompany.Location = new Point(1495, 201);
            btnAddNewCompany.Margin = new Padding(4);
            btnAddNewCompany.Name = "btnAddNewCompany";
            btnAddNewCompany.Size = new Size(75, 75);
            btnAddNewCompany.TabIndex = 0;
            btnAddNewCompany.UseVisualStyleBackColor = false;
            btnAddNewCompany.Click += btnAddNewCompany_Click;
            // 
            // panel1
            // 
            panel1.Location = new Point(18, 422);
            panel1.Margin = new Padding(10, 9, 10, 9);
            panel1.Name = "panel1";
            panel1.Size = new Size(0, 0);
            panel1.TabIndex = 48;
            // 
            // frmListCompanies
            // 
            AcceptButton = btnAddNewCompany;
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            CancelButton = btnClose;
            ClientSize = new Size(1578, 844);
            Controls.Add(dgvCompanies);
            Controls.Add(btnClose);
            Controls.Add(pictureBox1);
            Controls.Add(lblMode);
            Controls.Add(txtFilterValue);
            Controls.Add(cbFilterBy);
            Controls.Add(label2);
            Controls.Add(lblRecordsCount);
            Controls.Add(label1);
            Controls.Add(btnAddNewCompany);
            Controls.Add(panel1);
            Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(4, 5, 4, 5);
            Name = "frmListCompanies";
            Padding = new Padding(4);
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "شاشة إدراة الشركات";
            Load += frmListCompanies_Load;
            ((System.ComponentModel.ISupportInitialize)dgvCompanies).EndInit();
            cmsCompanies.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvCompanies;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.TextBox txtFilterValue;
        private System.Windows.Forms.ComboBox cbFilterBy;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblRecordsCount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnAddNewCompany;
        private System.Windows.Forms.ContextMenuStrip cmsCompanies;
        private System.Windows.Forms.ToolStripMenuItem refresh_toolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addNewPeraonToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DeletePerson_toolStripMenuItem4;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.Panel panel1;
    }
}