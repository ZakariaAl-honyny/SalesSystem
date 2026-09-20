namespace SalesSystem.Desktop.Screens.Warehouses.Forms
{
    partial class frmListWarehouses
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
            dgvWarehouses = new DataGridView();
            btnAdd = new Button();
            btnEdit = new Button();
            btnClose = new Button();
            lblRecordsCount = new Label();
            label1 = new Label();
            label2 = new Label();
            cbFilterBy = new ComboBox();
            txtFilterValue = new TextBox();
            ((System.ComponentModel.ISupportInitialize)dgvWarehouses).BeginInit();
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
            lblMode.Size = new Size(216, 39);
            lblMode.TabIndex = 30;
            lblMode.Text = "إدارة المستودعات";

            // dgvWarehouses
            dgvWarehouses.AllowUserToAddRows = false;
            dgvWarehouses.AllowUserToDeleteRows = false;
            dgvWarehouses.AllowUserToOrderColumns = true;
            dgvWarehouses.BackgroundColor = SystemColors.Window;
            dgvWarehouses.BorderStyle = BorderStyle.Fixed3D;
            dgvWarehouses.ColumnHeadersHeight = 50;
            dgvWarehouses.Location = new Point(12, 160);
            dgvWarehouses.MultiSelect = false;
            dgvWarehouses.Name = "dgvWarehouses";
            dgvWarehouses.ReadOnly = true;
            dgvWarehouses.RowHeadersWidth = 62;
            dgvWarehouses.RowTemplate.Height = 28;
            dgvWarehouses.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvWarehouses.Size = new Size(1540, 540);
            dgvWarehouses.TabIndex = 34;
            dgvWarehouses.TabStop = false;
            dgvWarehouses.DoubleClick += dgvWarehouses_DoubleClick;

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
            label1.Location = new Point(1220, 720);
            label1.Name = "label1";
            label1.Size = new Size(160, 24);
            label1.TabIndex = 3;
            label1.Text = "عدد المستودعات:";

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
            cbFilterBy.Items.AddRange(new object[] { "لا شيئ", "الاسم", "الهاتف", "العنوان" });
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

            // frmListWarehouses
            AcceptButton = btnAdd;
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            CancelButton = btnClose;
            ClientSize = new Size(1564, 783);
            Controls.Add(lblMode);
            Controls.Add(dgvWarehouses);
            Controls.Add(label2);
            Controls.Add(cbFilterBy);
            Controls.Add(txtFilterValue);
            Controls.Add(label1);
            Controls.Add(lblRecordsCount);
            Controls.Add(btnAdd);
            Controls.Add(btnEdit);
            Controls.Add(btnClose);
            Font = new Font("Tahoma", 12F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "frmListWarehouses";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "إدارة المستودعات";
            Load += frmListWarehouses_Load;
            ((System.ComponentModel.ISupportInitialize)dgvWarehouses).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.DataGridView dgvWarehouses;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblRecordsCount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbFilterBy;
        private System.Windows.Forms.TextBox txtFilterValue;
    }
}
