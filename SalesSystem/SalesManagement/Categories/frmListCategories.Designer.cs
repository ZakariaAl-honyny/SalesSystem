namespace SalesManagement.Categories
{
    partial class frmListCategories
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
            dgvCategories = new DataGridView();
            cmsCategories = new ContextMenuStrip(components);
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
            btnAddNewCategory = new Button();
            panel1 = new Panel();
            label3 = new Label();
            pictureBox2 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)dgvCategories).BeginInit();
            cmsCategories.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // dgvCategories
            // 
            dgvCategories.AllowUserToAddRows = false;
            dgvCategories.AllowUserToDeleteRows = false;
            dgvCategories.AllowUserToOrderColumns = true;
            dgvCategories.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvCategories.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvCategories.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvCategories.BackgroundColor = Color.White;
            dgvCategories.BorderStyle = BorderStyle.Fixed3D;
            dgvCategories.ColumnHeadersHeight = 50;
            dgvCategories.ContextMenuStrip = cmsCategories;
            dgvCategories.Location = new Point(8, 284);
            dgvCategories.Margin = new Padding(4);
            dgvCategories.MultiSelect = false;
            dgvCategories.Name = "dgvCategories";
            dgvCategories.ReadOnly = true;
            dgvCategories.RowHeadersWidth = 50;
            dgvCategories.RowTemplate.Height = 28;
            dgvCategories.RowTemplate.Resizable = DataGridViewTriState.True;
            dgvCategories.Size = new Size(1562, 483);
            dgvCategories.TabIndex = 47;
            dgvCategories.TabStop = false;
            dgvCategories.DoubleClick += dgvAllCategories_DoubleClick;
            // 
            // cmsCategories
            // 
            cmsCategories.BackColor = Color.White;
            cmsCategories.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmsCategories.ImageScalingSize = new Size(24, 24);
            cmsCategories.Items.AddRange(new ToolStripItem[] { refresh_toolStripMenuItem1, toolStripMenuItem2, toolStripMenuItem3, editToolStripMenuItem, addNewPeraonToolStripMenuItem, DeletePerson_toolStripMenuItem4, toolStripMenuItem5 });
            cmsCategories.Name = "contextMenuStrip1";
            cmsCategories.RightToLeft = RightToLeft.Yes;
            cmsCategories.Size = new Size(213, 150);
            cmsCategories.Opening += cmsCategories_Opening;
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
            DeletePerson_toolStripMenuItem4.Click += DeleteCategory_toolStripMenuItem4_Click;
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
            lblMode.Location = new Point(615, 166);
            lblMode.Margin = new Padding(4);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(354, 39);
            lblMode.TabIndex = 45;
            lblMode.Text = "شاشة إدراة التصنيفات";
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
            cbFilterBy.Items.AddRange(new object[] { "لا شيئ", "رقم التصنيف", "اسم التصنيف", "تفاصيل التصنيف" });
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
            label1.Size = new Size(154, 24);
            label1.TabIndex = 41;
            label1.Text = "عدد التصنيفات:";
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
            pictureBox1.Image = Properties.Resources.People_512;
            pictureBox1.Location = new Point(714, 8);
            pictureBox1.Margin = new Padding(4);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(150, 150);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 46;
            pictureBox1.TabStop = false;
            // 
            // btnAddNewCategory
            // 
            btnAddNewCategory.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAddNewCategory.BackColor = Color.White;
            btnAddNewCategory.FlatAppearance.BorderColor = Color.Black;
            btnAddNewCategory.FlatAppearance.BorderSize = 2;
            btnAddNewCategory.FlatStyle = FlatStyle.Popup;
            btnAddNewCategory.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAddNewCategory.Image = Properties.Resources.Add_Person_40;
            btnAddNewCategory.Location = new Point(1495, 201);
            btnAddNewCategory.Margin = new Padding(4);
            btnAddNewCategory.Name = "btnAddNewCategory";
            btnAddNewCategory.Size = new Size(75, 75);
            btnAddNewCategory.TabIndex = 0;
            btnAddNewCategory.UseVisualStyleBackColor = false;
            btnAddNewCategory.Click += btnAddNewCategory_Click;
            // 
            // panel1
            // 
            panel1.Location = new Point(18, 422);
            panel1.Margin = new Padding(10, 9, 10, 9);
            panel1.Name = "panel1";
            panel1.Size = new Size(0, 0);
            panel1.TabIndex = 48;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top;
            label3.AutoSize = true;
            label3.BackColor = Color.WhiteSmoke;
            label3.FlatStyle = FlatStyle.Flat;
            label3.Font = new Font("Tahoma", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.Firebrick;
            label3.Location = new Point(612, 166);
            label3.Margin = new Padding(4);
            label3.Name = "label3";
            label3.Size = new Size(354, 39);
            label3.TabIndex = 45;
            label3.Text = "شاشة إدراة التصنيفات";
            // 
            // pictureBox2
            // 
            pictureBox2.Anchor = AnchorStyles.Top;
            pictureBox2.Image = Properties.Resources.People_512;
            pictureBox2.Location = new Point(714, 8);
            pictureBox2.Margin = new Padding(4);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(150, 150);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 46;
            pictureBox2.TabStop = false;
            // 
            // frmListCategories
            // 
            AcceptButton = btnAddNewCategory;
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            CancelButton = btnClose;
            ClientSize = new Size(1578, 844);
            Controls.Add(dgvCategories);
            Controls.Add(btnClose);
            Controls.Add(pictureBox2);
            Controls.Add(label3);
            Controls.Add(pictureBox1);
            Controls.Add(lblMode);
            Controls.Add(txtFilterValue);
            Controls.Add(cbFilterBy);
            Controls.Add(label2);
            Controls.Add(lblRecordsCount);
            Controls.Add(label1);
            Controls.Add(btnAddNewCategory);
            Controls.Add(panel1);
            Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(4, 5, 4, 5);
            Name = "frmListCategories";
            Padding = new Padding(4);
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "شاشة إدراة التصنيفات";
            Load += frmListCategories_Load;
            ((System.ComponentModel.ISupportInitialize)dgvCategories).EndInit();
            cmsCategories.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvCategories;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.TextBox txtFilterValue;
        private System.Windows.Forms.ComboBox cbFilterBy;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblRecordsCount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnAddNewCategory;
        private System.Windows.Forms.ContextMenuStrip cmsCategories;
        private System.Windows.Forms.ToolStripMenuItem refresh_toolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addNewPeraonToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DeletePerson_toolStripMenuItem4;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.Panel panel1;
        private Label label3;
        private PictureBox pictureBox2;
    }
}