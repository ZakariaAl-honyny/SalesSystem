namespace SalesManagement.Delegates
{
    partial class frmListDelegates
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
            dgvDelegates = new DataGridView();
            cmsDelegates = new ContextMenuStrip(components);
            refresh_toolStripMenuItem1 = new ToolStripMenuItem();
            toolStripMenuItem2 = new ToolStripSeparator();
            showDetailesToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem3 = new ToolStripSeparator();
            editToolStripMenuItem = new ToolStripMenuItem();
            addNewPeraonToolStripMenuItem = new ToolStripMenuItem();
            DeletePerson_toolStripMenuItem4 = new ToolStripMenuItem();
            toolStripMenuItem5 = new ToolStripSeparator();
            sendEmailToolStripMenuItem = new ToolStripMenuItem();
            callPhoneToolStripMenuItem = new ToolStripMenuItem();
            lblMode = new Label();
            txtFilterValue = new TextBox();
            cbFilterBy = new ComboBox();
            label2 = new Label();
            lblRecordsCount = new Label();
            label1 = new Label();
            btnClose = new Button();
            pictureBox1 = new PictureBox();
            btnAddNewDelegate = new Button();
            panel1 = new Panel();
            ((System.ComponentModel.ISupportInitialize)dgvDelegates).BeginInit();
            cmsDelegates.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // dgvDelegates
            // 
            dgvDelegates.AllowUserToAddRows = false;
            dgvDelegates.AllowUserToDeleteRows = false;
            dgvDelegates.AllowUserToOrderColumns = true;
            dgvDelegates.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvDelegates.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvDelegates.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvDelegates.BackgroundColor = Color.White;
            dgvDelegates.BorderStyle = BorderStyle.Fixed3D;
            dgvDelegates.ColumnHeadersHeight = 50;
            dgvDelegates.ContextMenuStrip = cmsDelegates;
            dgvDelegates.Location = new Point(8, 284);
            dgvDelegates.Margin = new Padding(4);
            dgvDelegates.Name = "dgvDelegates";
            dgvDelegates.ReadOnly = true;
            dgvDelegates.RowHeadersWidth = 50;
            dgvDelegates.RowTemplate.Height = 28;
            dgvDelegates.RowTemplate.Resizable = DataGridViewTriState.True;
            dgvDelegates.Size = new Size(1562, 483);
            dgvDelegates.TabIndex = 47;
            dgvDelegates.TabStop = false;
            dgvDelegates.DoubleClick += dgvAllDelegates_DoubleClick;
            // 
            // cmsDelegates
            // 
            cmsDelegates.BackColor = Color.White;
            cmsDelegates.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmsDelegates.ImageScalingSize = new Size(24, 24);
            cmsDelegates.Items.AddRange(new ToolStripItem[] { refresh_toolStripMenuItem1, toolStripMenuItem2, showDetailesToolStripMenuItem, toolStripMenuItem3, editToolStripMenuItem, addNewPeraonToolStripMenuItem, DeletePerson_toolStripMenuItem4, toolStripMenuItem5, sendEmailToolStripMenuItem, callPhoneToolStripMenuItem });
            cmsDelegates.Name = "contextMenuStrip1";
            cmsDelegates.RightToLeft = RightToLeft.Yes;
            cmsDelegates.Size = new Size(323, 246);
            cmsDelegates.Opening += cmsDelegates_Opening;
            // 
            // refresh_toolStripMenuItem1
            // 
            refresh_toolStripMenuItem1.Image = Properties.Resources.refresh_40;
            refresh_toolStripMenuItem1.Name = "refresh_toolStripMenuItem1";
            refresh_toolStripMenuItem1.ShortcutKeys = Keys.Control | Keys.R;
            refresh_toolStripMenuItem1.Size = new Size(322, 32);
            refresh_toolStripMenuItem1.Text = "&تحديث";
            refresh_toolStripMenuItem1.Click += refresh_toolStripMenuItem1_Click;
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new Size(319, 6);
            // 
            // showDetailesToolStripMenuItem
            // 
            showDetailesToolStripMenuItem.Image = Properties.Resources.PersonDetails_32;
            showDetailesToolStripMenuItem.Name = "showDetailesToolStripMenuItem";
            showDetailesToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.P;
            showDetailesToolStripMenuItem.Size = new Size(322, 32);
            showDetailesToolStripMenuItem.Text = "&عرض بيانات المندوب";
            showDetailesToolStripMenuItem.Click += showDetailesToolStripMenuItem_Click;
            // 
            // toolStripMenuItem3
            // 
            toolStripMenuItem3.Name = "toolStripMenuItem3";
            toolStripMenuItem3.Size = new Size(319, 6);
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.Image = Properties.Resources.edit_32;
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.E;
            editToolStripMenuItem.Size = new Size(322, 32);
            editToolStripMenuItem.Text = "تعديل";
            editToolStripMenuItem.Click += editToolStripMenuItem_Click;
            // 
            // addNewPeraonToolStripMenuItem
            // 
            addNewPeraonToolStripMenuItem.Image = Properties.Resources.Add_New_User_72;
            addNewPeraonToolStripMenuItem.Name = "addNewPeraonToolStripMenuItem";
            addNewPeraonToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.A;
            addNewPeraonToolStripMenuItem.Size = new Size(322, 32);
            addNewPeraonToolStripMenuItem.Text = "&إضافة";
            addNewPeraonToolStripMenuItem.Click += addNewPeraonToolStripMenuItem_Click;
            // 
            // DeletePerson_toolStripMenuItem4
            // 
            DeletePerson_toolStripMenuItem4.Image = Properties.Resources.Delete_32;
            DeletePerson_toolStripMenuItem4.Name = "DeletePerson_toolStripMenuItem4";
            DeletePerson_toolStripMenuItem4.Size = new Size(322, 32);
            DeletePerson_toolStripMenuItem4.Text = "حذف";
            DeletePerson_toolStripMenuItem4.Click += DeleteDelegate_toolStripMenuItem4_Click;
            // 
            // toolStripMenuItem5
            // 
            toolStripMenuItem5.Name = "toolStripMenuItem5";
            toolStripMenuItem5.Size = new Size(319, 6);
            // 
            // sendEmailToolStripMenuItem
            // 
            sendEmailToolStripMenuItem.Image = Properties.Resources.send_email_32;
            sendEmailToolStripMenuItem.Name = "sendEmailToolStripMenuItem";
            sendEmailToolStripMenuItem.Size = new Size(322, 32);
            sendEmailToolStripMenuItem.Text = "&Send Email";
            sendEmailToolStripMenuItem.Click += sendEmailToolStripMenuItem_Click;
            // 
            // callPhoneToolStripMenuItem
            // 
            callPhoneToolStripMenuItem.Image = Properties.Resources.call_32;
            callPhoneToolStripMenuItem.Name = "callPhoneToolStripMenuItem";
            callPhoneToolStripMenuItem.Size = new Size(322, 32);
            callPhoneToolStripMenuItem.Text = "&Call Phone";
            callPhoneToolStripMenuItem.Click += callPhoneToolStripMenuItem_Click;
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
            lblMode.Size = new Size(345, 39);
            lblMode.TabIndex = 45;
            lblMode.Text = "شاشة إدراة المندوبين";
            lblMode.Click += lblMode_Click;
            // 
            // txtFilterValue
            // 
            txtFilterValue.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtFilterValue.Location = new Point(524, 244);
            txtFilterValue.Margin = new Padding(4);
            txtFilterValue.Name = "txtFilterValue";
            txtFilterValue.Size = new Size(403, 32);
            txtFilterValue.TabIndex = 40;
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
            cbFilterBy.Items.AddRange(new object[] { "لا شيئ", "رقم المندوب", "اسم المندوب" });
            cbFilterBy.Location = new Point(187, 244);
            cbFilterBy.Margin = new Padding(4);
            cbFilterBy.Name = "cbFilterBy";
            cbFilterBy.Size = new Size(329, 32);
            cbFilterBy.TabIndex = 36;
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
            label1.Size = new Size(149, 24);
            label1.TabIndex = 41;
            label1.Text = "عدد المندوبين:";
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
            btnClose.TabIndex = 35;
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
            // btnAddNewDelegate
            // 
            btnAddNewDelegate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAddNewDelegate.BackColor = Color.White;
            btnAddNewDelegate.FlatAppearance.BorderColor = Color.Black;
            btnAddNewDelegate.FlatAppearance.BorderSize = 2;
            btnAddNewDelegate.FlatStyle = FlatStyle.Popup;
            btnAddNewDelegate.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAddNewDelegate.Image = Properties.Resources.Add_Person_40;
            btnAddNewDelegate.Location = new Point(1495, 201);
            btnAddNewDelegate.Margin = new Padding(4);
            btnAddNewDelegate.Name = "btnAddNewDelegate";
            btnAddNewDelegate.Size = new Size(75, 75);
            btnAddNewDelegate.TabIndex = 44;
            btnAddNewDelegate.UseVisualStyleBackColor = false;
            btnAddNewDelegate.Click += btnAddNewDelegate_Click;
            // 
            // panel1
            // 
            panel1.Location = new Point(18, 422);
            panel1.Margin = new Padding(10, 9, 10, 9);
            panel1.Name = "panel1";
            panel1.Size = new Size(0, 0);
            panel1.TabIndex = 48;
            // 
            // frmListDelegates
            // 
            AcceptButton = btnAddNewDelegate;
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            CancelButton = btnClose;
            ClientSize = new Size(1578, 844);
            Controls.Add(dgvDelegates);
            Controls.Add(btnClose);
            Controls.Add(pictureBox1);
            Controls.Add(lblMode);
            Controls.Add(txtFilterValue);
            Controls.Add(cbFilterBy);
            Controls.Add(label2);
            Controls.Add(lblRecordsCount);
            Controls.Add(label1);
            Controls.Add(btnAddNewDelegate);
            Controls.Add(panel1);
            Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(4, 5, 4, 5);
            Name = "frmListDelegates";
            Padding = new Padding(4);
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "شاشة إدراة المندوبين";
            Load += frmListDelegates_Load;
            ((System.ComponentModel.ISupportInitialize)dgvDelegates).EndInit();
            cmsDelegates.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvDelegates;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.TextBox txtFilterValue;
        private System.Windows.Forms.ComboBox cbFilterBy;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblRecordsCount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnAddNewDelegate;
        private System.Windows.Forms.ContextMenuStrip cmsDelegates;
        private System.Windows.Forms.ToolStripMenuItem refresh_toolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem showDetailesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addNewPeraonToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DeletePerson_toolStripMenuItem4;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem sendEmailToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem callPhoneToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
    }
}