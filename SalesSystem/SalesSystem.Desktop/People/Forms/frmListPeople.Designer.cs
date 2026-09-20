namespace SalesSystem.Desktop.People.Forms
{
    partial class frmListPeople
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
            components?.Dispose();
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
            cmsPeople = new ContextMenuStrip(components);
            toolStripMenuItem1 = new ToolStripMenuItem();
            toolStripMenuItem2 = new ToolStripSeparator();
            showDetailesToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem3 = new ToolStripSeparator();
            editToolStripMenuItem = new ToolStripMenuItem();
            addNewPeraonToolStripMenuItem = new ToolStripMenuItem();
            DeletePerson_toolStripMenuItem4 = new ToolStripMenuItem();
            toolStripMenuItem5 = new ToolStripSeparator();
            sendEmailToolStripMenuItem = new ToolStripMenuItem();
            callPhoneToolStripMenuItem = new ToolStripMenuItem();
            label1 = new Label();
            lblRecordsCount = new Label();
            label2 = new Label();
            cbFilterBy = new ComboBox();
            txtFilterValue = new TextBox();
            lblMode = new Label();
            dgvPeople = new DataGridView();
            btnAddNewPerson = new Button();
            btnClose = new Button();
            cmsPeople.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPeople).BeginInit();
            SuspendLayout();
            // 
            // cmsPeople
            // 
            cmsPeople.BackColor = Color.White;
            cmsPeople.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmsPeople.ImageScalingSize = new Size(24, 24);
            cmsPeople.Items.AddRange(new ToolStripItem[] { toolStripMenuItem1, toolStripMenuItem2, showDetailesToolStripMenuItem, toolStripMenuItem3, editToolStripMenuItem, addNewPeraonToolStripMenuItem, DeletePerson_toolStripMenuItem4, toolStripMenuItem5, sendEmailToolStripMenuItem, callPhoneToolStripMenuItem });
            cmsPeople.Name = "contextMenuStrip1";
            cmsPeople.RightToLeft = RightToLeft.Yes;
            cmsPeople.Size = new Size(319, 232);
            cmsPeople.Opening += cmsPeople_Opening;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.ShortcutKeys = Keys.Control | Keys.R;
            toolStripMenuItem1.Size = new Size(318, 30);
            toolStripMenuItem1.Text = "&تحديث";
            toolStripMenuItem1.Click += refreshToolStripMenuItem_Click;
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new Size(315, 6);
            // 
            // showDetailesToolStripMenuItem
            // 
            showDetailesToolStripMenuItem.Name = "showDetailesToolStripMenuItem";
            showDetailesToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.P;
            showDetailesToolStripMenuItem.Size = new Size(318, 30);
            showDetailesToolStripMenuItem.Text = "&عرض بيانات الشخص";
            showDetailesToolStripMenuItem.Click += showDetailsToolStripMenuItem_Click;
            // 
            // toolStripMenuItem3
            // 
            toolStripMenuItem3.Name = "toolStripMenuItem3";
            toolStripMenuItem3.Size = new Size(315, 6);
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new Size(318, 30);
            editToolStripMenuItem.Text = "تعديل";
            editToolStripMenuItem.Click += editToolStripMenuItem_Click;
            // 
            // addNewPeraonToolStripMenuItem
            // 
            addNewPeraonToolStripMenuItem.Name = "addNewPeraonToolStripMenuItem";
            addNewPeraonToolStripMenuItem.Size = new Size(318, 30);
            addNewPeraonToolStripMenuItem.Text = "&إضافة";
            addNewPeraonToolStripMenuItem.Click += addNewPersonToolStripMenuItem_Click;
            // 
            // DeletePerson_toolStripMenuItem4
            // 
            DeletePerson_toolStripMenuItem4.Name = "DeletePerson_toolStripMenuItem4";
            DeletePerson_toolStripMenuItem4.Size = new Size(318, 30);
            DeletePerson_toolStripMenuItem4.Text = "حذف";
            DeletePerson_toolStripMenuItem4.Click += DeletePerson_toolStripMenuItem_Click;
            // 
            // toolStripMenuItem5
            // 
            toolStripMenuItem5.Name = "toolStripMenuItem5";
            toolStripMenuItem5.Size = new Size(315, 6);
            // 
            // sendEmailToolStripMenuItem
            // 
            sendEmailToolStripMenuItem.Name = "sendEmailToolStripMenuItem";
            sendEmailToolStripMenuItem.Size = new Size(318, 30);
            sendEmailToolStripMenuItem.Text = "&Send Email";
            sendEmailToolStripMenuItem.Click += sendEmailToolStripMenuItem_Click;
            // 
            // callPhoneToolStripMenuItem
            // 
            callPhoneToolStripMenuItem.Name = "callPhoneToolStripMenuItem";
            callPhoneToolStripMenuItem.Size = new Size(318, 30);
            callPhoneToolStripMenuItem.Text = "&Call Phone";
            callPhoneToolStripMenuItem.Click += callPhoneToolStripMenuItem_Click;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(1309, 644);
            label1.Margin = new Padding(4);
            label1.Name = "label1";
            label1.Size = new Size(152, 24);
            label1.TabIndex = 3;
            label1.Text = "عدد الأشخاص:";
            // 
            // lblRecordsCount
            // 
            lblRecordsCount.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            lblRecordsCount.AutoSize = true;
            lblRecordsCount.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblRecordsCount.Location = new Point(1469, 644);
            lblRecordsCount.Margin = new Padding(4);
            lblRecordsCount.Name = "lblRecordsCount";
            lblRecordsCount.Size = new Size(72, 24);
            lblRecordsCount.TabIndex = 4;
            lblRecordsCount.Text = "[????]";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(23, 113);
            label2.Margin = new Padding(4);
            label2.Name = "label2";
            label2.Size = new Size(171, 24);
            label2.TabIndex = 5;
            label2.Text = "البحث عن طريق:";
            // 
            // cbFilterBy
            // 
            cbFilterBy.BackColor = SystemColors.Control;
            cbFilterBy.DropDownStyle = ComboBoxStyle.DropDownList;
            cbFilterBy.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            cbFilterBy.FormattingEnabled = true;
            cbFilterBy.Items.AddRange(new object[] { "لا شيئ", "رقم الشخص", "رقم الوطني", "الأسم الاول", "الأسم الثاني", "الأسم الثالث", "الأسم الاخير - اللقب", "الجنسية", "الجنس", "رقم الهاتف", "الأيميل" });
            cbFilterBy.Location = new Point(202, 109);
            cbFilterBy.Margin = new Padding(4);
            cbFilterBy.Name = "cbFilterBy";
            cbFilterBy.Size = new Size(326, 32);
            cbFilterBy.TabIndex = 1;
            cbFilterBy.SelectedIndexChanged += cbFilterBy_SelectedIndexChanged;
            // 
            // txtFilterValue
            // 
            txtFilterValue.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtFilterValue.Location = new Point(536, 109);
            txtFilterValue.Margin = new Padding(4);
            txtFilterValue.Name = "txtFilterValue";
            txtFilterValue.Size = new Size(563, 32);
            txtFilterValue.TabIndex = 2;
            txtFilterValue.TextChanged += txtFilterValue_TextChanged;
            txtFilterValue.KeyPress += txtFilterValue_KeyPress;
            // 
            // lblMode
            // 
            lblMode.Anchor = AnchorStyles.Top;
            lblMode.AutoSize = true;
            lblMode.BackColor = Color.WhiteSmoke;
            lblMode.FlatStyle = FlatStyle.Flat;
            lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblMode.ForeColor = Color.Firebrick;
            lblMode.Location = new Point(669, 31);
            lblMode.Margin = new Padding(4);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(242, 39);
            lblMode.TabIndex = 30;
            lblMode.Text = "إدراة الأشخاص";
            // 
            // dgvPeople
            // 
            dgvPeople.AllowUserToAddRows = false;
            dgvPeople.AllowUserToDeleteRows = false;
            dgvPeople.AllowUserToOrderColumns = true;
            dgvPeople.BackgroundColor = SystemColors.Window;
            dgvPeople.BorderStyle = BorderStyle.Fixed3D;
            dgvPeople.ColumnHeadersHeight = 50;
            dgvPeople.ContextMenuStrip = cmsPeople;
            dgvPeople.Location = new Point(0, 149);
            dgvPeople.Margin = new Padding(0, 4, 0, 4);
            dgvPeople.Name = "dgvPeople";
            dgvPeople.ReadOnly = true;
            dgvPeople.RowHeadersWidth = 62;
            dgvPeople.RowTemplate.Height = 28;
            dgvPeople.Size = new Size(1580, 483);
            dgvPeople.TabIndex = 34;
            dgvPeople.TabStop = false;
            dgvPeople.DoubleClick += dgvAllPeople_DoubleClick;
            // 
            // btnAddNewPerson
            // 
            btnAddNewPerson.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAddNewPerson.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnAddNewPerson.BackColor = Color.White;
            btnAddNewPerson.FlatAppearance.BorderColor = Color.Black;
            btnAddNewPerson.FlatAppearance.BorderSize = 2;
            btnAddNewPerson.FlatStyle = FlatStyle.Popup;
            btnAddNewPerson.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAddNewPerson.Location = new Point(1492, 66);
            btnAddNewPerson.Margin = new Padding(4);
            btnAddNewPerson.Name = "btnAddNewPerson";
            btnAddNewPerson.Size = new Size(75, 75);
            btnAddNewPerson.TabIndex = 5;
            btnAddNewPerson.Text = "+ جديد";
            btnAddNewPerson.UseVisualStyleBackColor = true;
            btnAddNewPerson.Click += btnAddNewPerson_Click;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnClose.BackColor = Color.White;
            btnClose.DialogResult = DialogResult.Cancel;
            btnClose.FlatStyle = FlatStyle.Popup;
            btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnClose.ImageAlign = ContentAlignment.MiddleRight;
            btnClose.Location = new Point(13, 644);
            btnClose.Margin = new Padding(4);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(181, 61);
            btnClose.TabIndex = 0;
            btnClose.Text = "إغلاق";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // frmListPeople
            // 
            AcceptButton = btnAddNewPerson;
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            CancelButton = btnClose;
            ClientSize = new Size(1578, 718);
            Controls.Add(cbFilterBy);
            Controls.Add(dgvPeople);
            Controls.Add(lblMode);
            Controls.Add(txtFilterValue);
            Controls.Add(label2);
            Controls.Add(lblRecordsCount);
            Controls.Add(label1);
            Controls.Add(btnClose);
            Controls.Add(btnAddNewPerson);
            Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "frmListPeople";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "إدراة الأشخاص";
            Load += frmManagePeople_Load;
            cmsPeople.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvPeople).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnAddNewPerson;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblRecordsCount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbFilterBy;
        private System.Windows.Forms.ContextMenuStrip cmsPeople;
        private System.Windows.Forms.ToolStripMenuItem showDetailesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addNewPeraonToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sendEmailToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem callPhoneToolStripMenuItem;
        private System.Windows.Forms.TextBox txtFilterValue;
        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem DeletePerson_toolStripMenuItem4;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.DataGridView dgvPeople;
    }
}
