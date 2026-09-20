namespace SalesManagement.Expenses
{
    partial class frmListExpenses
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
            dgvExpenses = new DataGridView();
            cmsExpenses = new ContextMenuStrip(components);
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
            btnAddNewExpense = new Button();
            panel1 = new Panel();
            ((System.ComponentModel.ISupportInitialize)dgvExpenses).BeginInit();
            cmsExpenses.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // dgvExpenses
            // 
            dgvExpenses.AllowUserToAddRows = false;
            dgvExpenses.AllowUserToDeleteRows = false;
            dgvExpenses.AllowUserToOrderColumns = true;
            dgvExpenses.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvExpenses.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvExpenses.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvExpenses.BackgroundColor = Color.White;
            dgvExpenses.BorderStyle = BorderStyle.Fixed3D;
            dgvExpenses.ColumnHeadersHeight = 50;
            dgvExpenses.ContextMenuStrip = cmsExpenses;
            dgvExpenses.Location = new Point(8, 284);
            dgvExpenses.Margin = new Padding(4);
            dgvExpenses.MultiSelect = false;
            dgvExpenses.Name = "dgvExpenses";
            dgvExpenses.ReadOnly = true;
            dgvExpenses.RowHeadersWidth = 50;
            dgvExpenses.RowTemplate.Height = 28;
            dgvExpenses.RowTemplate.Resizable = DataGridViewTriState.True;
            dgvExpenses.Size = new Size(1562, 483);
            dgvExpenses.TabIndex = 47;
            dgvExpenses.TabStop = false;
            dgvExpenses.DoubleClick += dgvAllExpenses_DoubleClick;
            // 
            // cmsExpenses
            // 
            cmsExpenses.BackColor = Color.White;
            cmsExpenses.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmsExpenses.ImageScalingSize = new Size(24, 24);
            cmsExpenses.Items.AddRange(new ToolStripItem[] { refresh_toolStripMenuItem1, toolStripMenuItem2, toolStripMenuItem3, editToolStripMenuItem, addNewPeraonToolStripMenuItem, DeletePerson_toolStripMenuItem4, toolStripMenuItem5 });
            cmsExpenses.Name = "contextMenuStrip1";
            cmsExpenses.RightToLeft = RightToLeft.Yes;
            cmsExpenses.Size = new Size(213, 150);
            cmsExpenses.Opening += cmsExpenses_Opening;
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
            DeletePerson_toolStripMenuItem4.Click += DeleteExpense_toolStripMenuItem4_Click;
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
            lblMode.Location = new Point(604, 166);
            lblMode.Margin = new Padding(4);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(371, 39);
            lblMode.TabIndex = 45;
            lblMode.Text = "شاشة إدراة المصروفات";
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
            cbFilterBy.Items.AddRange(new object[] { "لا شيئ", "رقم المصروف", "اسم المصروف", "تفاصيل المصروف" });
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
            label1.Size = new Size(164, 24);
            label1.TabIndex = 41;
            label1.Text = "عدد المصروفات:";
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
            // btnAddNewExpense
            // 
            btnAddNewExpense.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAddNewExpense.BackColor = Color.White;
            btnAddNewExpense.FlatAppearance.BorderColor = Color.Black;
            btnAddNewExpense.FlatAppearance.BorderSize = 2;
            btnAddNewExpense.FlatStyle = FlatStyle.Popup;
            btnAddNewExpense.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAddNewExpense.Image = Properties.Resources.Add_Person_40;
            btnAddNewExpense.Location = new Point(1495, 201);
            btnAddNewExpense.Margin = new Padding(4);
            btnAddNewExpense.Name = "btnAddNewExpense";
            btnAddNewExpense.Size = new Size(75, 75);
            btnAddNewExpense.TabIndex = 0;
            btnAddNewExpense.UseVisualStyleBackColor = false;
            btnAddNewExpense.Click += btnAddNewExpense_Click;
            // 
            // panel1
            // 
            panel1.Location = new Point(18, 422);
            panel1.Margin = new Padding(10, 9, 10, 9);
            panel1.Name = "panel1";
            panel1.Size = new Size(0, 0);
            panel1.TabIndex = 48;
            // 
            // frmListExpenses
            // 
            AcceptButton = btnAddNewExpense;
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            CancelButton = btnClose;
            ClientSize = new Size(1578, 844);
            Controls.Add(dgvExpenses);
            Controls.Add(btnClose);
            Controls.Add(pictureBox1);
            Controls.Add(lblMode);
            Controls.Add(txtFilterValue);
            Controls.Add(cbFilterBy);
            Controls.Add(label2);
            Controls.Add(lblRecordsCount);
            Controls.Add(label1);
            Controls.Add(btnAddNewExpense);
            Controls.Add(panel1);
            Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(4, 5, 4, 5);
            Name = "frmListExpenses";
            Padding = new Padding(4);
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "شاشة إدراة المصروفات";
            Load += frmListExpenses_Load;
            ((System.ComponentModel.ISupportInitialize)dgvExpenses).EndInit();
            cmsExpenses.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvExpenses;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.TextBox txtFilterValue;
        private System.Windows.Forms.ComboBox cbFilterBy;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblRecordsCount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnAddNewExpense;
        private System.Windows.Forms.ContextMenuStrip cmsExpenses;
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