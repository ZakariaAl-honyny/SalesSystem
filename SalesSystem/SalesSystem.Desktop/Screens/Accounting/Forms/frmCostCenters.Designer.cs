namespace SalesSystem.Desktop.Screens.Accounting.Forms
{
    partial class frmCostCenters
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            components?.Dispose();base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            lblMode = new Label();
            dgvCostCenters = new DataGridView();
            btnAdd = new Button();
            btnEdit = new Button();
            btnDelete = new Button();
            btnClose = new Button();
            lblRecordsCount = new Label();
            label1 = new Label();
            label2 = new Label();
            cbFilterBy = new ComboBox();
            txtFilterValue = new TextBox();
            contextMenuStrip = new ContextMenuStrip(components);
            refreshToolStripMenuItem = new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)dgvCostCenters).BeginInit();
            contextMenuStrip.SuspendLayout();
            SuspendLayout();

            // lblMode
            lblMode.Anchor = AnchorStyles.Top;
            lblMode.AutoSize = true;
            lblMode.BackColor = Color.WhiteSmoke;
            lblMode.FlatStyle = FlatStyle.Flat;
            lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            lblMode.ForeColor = Color.Firebrick;
            lblMode.Location = new Point(667, 20);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(250, 39);
            lblMode.TabIndex = 30;
            lblMode.Text = "إدارة مراكز التكلفة";

            // dgvCostCenters
            dgvCostCenters.AllowUserToAddRows = false;
            dgvCostCenters.AllowUserToDeleteRows = false;
            dgvCostCenters.AllowUserToOrderColumns = true;
            dgvCostCenters.BackgroundColor = SystemColors.Window;
            dgvCostCenters.BorderStyle = BorderStyle.Fixed3D;
            dgvCostCenters.ColumnHeadersHeight = 50;
            dgvCostCenters.ContextMenuStrip = contextMenuStrip;
            dgvCostCenters.Location = new Point(12, 160);
            dgvCostCenters.MultiSelect = false;
            dgvCostCenters.Name = "dgvCostCenters";
            dgvCostCenters.ReadOnly = true;
            dgvCostCenters.RowHeadersWidth = 62;
            dgvCostCenters.RowTemplate.Height = 28;
            dgvCostCenters.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCostCenters.Size = new Size(1540, 540);
            dgvCostCenters.TabIndex = 34;
            dgvCostCenters.TabStop = false;
            dgvCostCenters.DoubleClick += dgvCostCenters_DoubleClick;

            // btnAdd
            btnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAdd.BackColor = Color.White;
            btnAdd.FlatAppearance.BorderColor = Color.Black;
            btnAdd.FlatAppearance.BorderSize = 2;
            btnAdd.FlatStyle = FlatStyle.Popup;
            btnAdd.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnAdd.Location = new Point(1470, 60);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(75, 75);
            btnAdd.TabIndex = 5;
            btnAdd.Text = "+ جديد";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;

            // btnEdit
            btnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnEdit.BackColor = Color.White;
            btnEdit.FlatAppearance.BorderColor = Color.Black;
            btnEdit.FlatAppearance.BorderSize = 2;
            btnEdit.FlatStyle = FlatStyle.Popup;
            btnEdit.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnEdit.Location = new Point(1380, 60);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(75, 75);
            btnEdit.TabIndex = 7;
            btnEdit.Text = "تعديل";
            btnEdit.UseVisualStyleBackColor = true;
            btnEdit.Click += btnEdit_Click;

            // btnDelete
            btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDelete.BackColor = Color.White;
            btnDelete.FlatAppearance.BorderColor = Color.Black;
            btnDelete.FlatAppearance.BorderSize = 2;
            btnDelete.FlatStyle = FlatStyle.Popup;
            btnDelete.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnDelete.Location = new Point(1290, 60);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(75, 75);
            btnDelete.TabIndex = 8;
            btnDelete.Text = "تعطيل";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;

            // btnClose
            btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnClose.BackColor = Color.White;
            btnClose.DialogResult = DialogResult.Cancel;
            btnClose.FlatStyle = FlatStyle.Popup;
            btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnClose.Location = new Point(12, 710);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(181, 61);
            btnClose.TabIndex = 0;
            btnClose.Text = "إغلاق";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;

            // lblRecordsCount
            lblRecordsCount.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            lblRecordsCount.AutoSize = true;
            lblRecordsCount.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblRecordsCount.Location = new Point(1380, 720);
            lblRecordsCount.Name = "lblRecordsCount";
            lblRecordsCount.Size = new Size(72, 24);
            lblRecordsCount.TabIndex = 4;
            lblRecordsCount.Text = "[????]";

            // label1
            label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label1.Location = new Point(1190, 720);
            label1.Name = "label1";
            label1.Size = new Size(190, 24);
            label1.TabIndex = 3;
            label1.Text = "عدد مراكز التكلفة:";

            // label2
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label2.Location = new Point(21, 115);
            label2.Name = "label2";
            label2.Size = new Size(130, 24);
            label2.TabIndex = 5;
            label2.Text = "البحث عن طريق:";

            // cbFilterBy
            cbFilterBy.BackColor = SystemColors.Control;
            cbFilterBy.DropDownStyle = ComboBoxStyle.DropDownList;
            cbFilterBy.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            cbFilterBy.FormattingEnabled = true;
            cbFilterBy.Items.AddRange(new object[] { "لا شيئ", "الكود", "الاسم", "المركز الأب", "المدير" });
            cbFilterBy.Location = new Point(200, 111);
            cbFilterBy.Name = "cbFilterBy";
            cbFilterBy.Size = new Size(260, 32);
            cbFilterBy.TabIndex = 1;
            cbFilterBy.SelectedIndexChanged += cbFilterBy_SelectedIndexChanged;

            // txtFilterValue
            txtFilterValue.Font = new Font("Tahoma", 10F);
            txtFilterValue.Location = new Point(470, 111);
            txtFilterValue.Name = "txtFilterValue";
            txtFilterValue.Size = new Size(500, 32);
            txtFilterValue.TabIndex = 2;
            txtFilterValue.Visible = false;
            txtFilterValue.TextChanged += txtFilterValue_TextChanged;
            txtFilterValue.KeyPress += txtFilterValue_KeyPress;

            // contextMenuStrip
            contextMenuStrip.ImageScalingSize = new Size(24, 24);
            contextMenuStrip.Items.AddRange(new ToolStripItem[] { refreshToolStripMenuItem });
            contextMenuStrip.Name = "contextMenuStrip";
            contextMenuStrip.RightToLeft = RightToLeft.Yes;
            contextMenuStrip.Size = new Size(153, 36);

            // refreshToolStripMenuItem
            refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            refreshToolStripMenuItem.Size = new Size(152, 32);
            refreshToolStripMenuItem.Text = "تحديث";
            refreshToolStripMenuItem.Click += refreshToolStripMenuItem_Click;

            // frmCostCenters
            AcceptButton = btnAdd;
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            CancelButton = btnClose;
            ClientSize = new Size(1564, 783);
            Controls.Add(lblMode);
            Controls.Add(dgvCostCenters);
            Controls.Add(label2);
            Controls.Add(cbFilterBy);
            Controls.Add(txtFilterValue);
            Controls.Add(label1);
            Controls.Add(lblRecordsCount);
            Controls.Add(btnAdd);
            Controls.Add(btnEdit);
            Controls.Add(btnDelete);
            Controls.Add(btnClose);
            Font = new Font("Tahoma", 12F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "frmCostCenters";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "إدارة مراكز التكلفة";
            Load += frmCostCenters_Load;
            ((System.ComponentModel.ISupportInitialize)dgvCostCenters).EndInit();
            contextMenuStrip.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.DataGridView dgvCostCenters;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblRecordsCount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbFilterBy;
        private System.Windows.Forms.TextBox txtFilterValue;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
    }
}
