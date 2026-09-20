namespace SalesSystem.Desktop.Screens.Sales.Forms
{
    partial class frmListSales
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
            dgvSales = new DataGridView();
            btnAdd = new Button();
            btnView = new Button();
            btnPost = new Button();
            btnCancel = new Button();
            btnClose = new Button();
            label1 = new Label();
            lblRecordsCount = new Label();
            label2 = new Label();
            cbFilterBy = new ComboBox();
            txtFilterValue = new TextBox();
            label3 = new Label();
            dtpFrom = new DateTimePicker();
            label4 = new Label();
            dtpTo = new DateTimePicker();
            label5 = new Label();
            cbStatus = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)dgvSales).BeginInit();
            SuspendLayout();
            // 
            // lblMode
            // 
            lblMode.Anchor = AnchorStyles.Top;
            lblMode.AutoSize = true;
            lblMode.BackColor = Color.WhiteSmoke;
            lblMode.FlatStyle = FlatStyle.Flat;
            lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblMode.ForeColor = Color.Firebrick;
            lblMode.Location = new Point(631, 50);
            lblMode.Margin = new Padding(4);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(235, 39);
            lblMode.TabIndex = 30;
            lblMode.Text = "فواتير البيع";
            // 
            // dgvSales
            // 
            dgvSales.AllowUserToAddRows = false;
            dgvSales.AllowUserToDeleteRows = false;
            dgvSales.AllowUserToOrderColumns = true;
            dgvSales.BackgroundColor = SystemColors.Window;
            dgvSales.BorderStyle = BorderStyle.Fixed3D;
            dgvSales.ColumnHeadersHeight = 50;
            dgvSales.Location = new Point(-2, 280);
            dgvSales.Margin = new Padding(0, 4, 0, 4);
            dgvSales.Name = "dgvSales";
            dgvSales.ReadOnly = true;
            dgvSales.RowHeadersWidth = 62;
            dgvSales.RowTemplate.Height = 28;
            dgvSales.Size = new Size(1580, 483);
            dgvSales.TabIndex = 34;
            dgvSales.TabStop = false;
            dgvSales.DoubleClick += dgvSales_DoubleClick;
            // 
            // btnAdd
            // 
            btnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAdd.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnAdd.BackColor = Color.White;
            btnAdd.FlatAppearance.BorderColor = Color.Black;
            btnAdd.FlatAppearance.BorderSize = 2;
            btnAdd.FlatStyle = FlatStyle.Popup;
            btnAdd.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAdd.Location = new Point(1490, 180);
            btnAdd.Margin = new Padding(4);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(90, 40);
            btnAdd.TabIndex = 5;
            btnAdd.Text = "فاتورة جديدة";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnView
            // 
            btnView.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnView.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnView.BackColor = Color.White;
            btnView.FlatAppearance.BorderColor = Color.Black;
            btnView.FlatAppearance.BorderSize = 2;
            btnView.FlatStyle = FlatStyle.Popup;
            btnView.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnView.Location = new Point(1390, 180);
            btnView.Margin = new Padding(4);
            btnView.Name = "btnView";
            btnView.Size = new Size(90, 40);
            btnView.TabIndex = 6;
            btnView.Text = "عرض / تعديل";
            btnView.UseVisualStyleBackColor = true;
            btnView.Click += btnView_Click;
            // 
            // btnPost
            // 
            btnPost.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnPost.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnPost.BackColor = Color.White;
            btnPost.FlatAppearance.BorderColor = Color.Black;
            btnPost.FlatAppearance.BorderSize = 2;
            btnPost.FlatStyle = FlatStyle.Popup;
            btnPost.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnPost.Location = new Point(1290, 180);
            btnPost.Margin = new Padding(4);
            btnPost.Name = "btnPost";
            btnPost.Size = new Size(90, 40);
            btnPost.TabIndex = 7;
            btnPost.Text = "ترحيل";
            btnPost.UseVisualStyleBackColor = true;
            btnPost.Click += btnPost_Click;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCancel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnCancel.BackColor = Color.White;
            btnCancel.FlatAppearance.BorderColor = Color.Black;
            btnCancel.FlatAppearance.BorderSize = 2;
            btnCancel.FlatStyle = FlatStyle.Popup;
            btnCancel.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCancel.ForeColor = Color.Red;
            btnCancel.Location = new Point(1190, 180);
            btnCancel.Margin = new Padding(4);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(90, 40);
            btnCancel.TabIndex = 8;
            btnCancel.Text = "إلغاء";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnClose.BackColor = Color.White;
            btnClose.DialogResult = DialogResult.Cancel;
            btnClose.FlatStyle = FlatStyle.Popup;
            btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnClose.ImageAlign = ContentAlignment.MiddleRight;
            btnClose.Location = new Point(11, 771);
            btnClose.Margin = new Padding(4);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(181, 61);
            btnClose.TabIndex = 0;
            btnClose.Text = "إغلاق";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(1307, 771);
            label1.Margin = new Padding(4);
            label1.Name = "label1";
            label1.Size = new Size(155, 24);
            label1.TabIndex = 3;
            label1.Text = "عدد الفواتير:";
            // 
            // lblRecordsCount
            // 
            lblRecordsCount.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            lblRecordsCount.AutoSize = true;
            lblRecordsCount.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblRecordsCount.Location = new Point(1467, 771);
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
            label2.Location = new Point(21, 244);
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
            cbFilterBy.Items.AddRange(new object[] { "لا شيئ", "رقم الفاتورة", "اسم العميل" });
            cbFilterBy.Location = new Point(200, 240);
            cbFilterBy.Margin = new Padding(4);
            cbFilterBy.Name = "cbFilterBy";
            cbFilterBy.Size = new Size(326, 32);
            cbFilterBy.TabIndex = 1;
            cbFilterBy.SelectedIndexChanged += cbFilterBy_SelectedIndexChanged;
            // 
            // txtFilterValue
            // 
            txtFilterValue.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtFilterValue.Location = new Point(534, 240);
            txtFilterValue.Margin = new Padding(4);
            txtFilterValue.Name = "txtFilterValue";
            txtFilterValue.Size = new Size(200, 32);
            txtFilterValue.TabIndex = 2;
            txtFilterValue.Visible = false;
            txtFilterValue.TextChanged += txtFilterValue_TextChanged;
            txtFilterValue.KeyPress += txtFilterValue_KeyPress;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(784, 244);
            label3.Margin = new Padding(4);
            label3.Name = "label3";
            label3.Size = new Size(42, 24);
            label3.TabIndex = 9;
            label3.Text = "من:";
            // 
            // dtpFrom
            // 
            dtpFrom.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dtpFrom.Format = DateTimePickerFormat.Short;
            dtpFrom.Location = new Point(830, 240);
            dtpFrom.Margin = new Padding(4);
            dtpFrom.Name = "dtpFrom";
            dtpFrom.Size = new Size(150, 32);
            dtpFrom.TabIndex = 10;
            dtpFrom.ValueChanged += dtpFrom_ValueChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(1000, 244);
            label4.Margin = new Padding(4);
            label4.Name = "label4";
            label4.Size = new Size(40, 24);
            label4.TabIndex = 11;
            label4.Text = "إلى:";
            // 
            // dtpTo
            // 
            dtpTo.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dtpTo.Format = DateTimePickerFormat.Short;
            dtpTo.Location = new Point(1046, 240);
            dtpTo.Margin = new Padding(4);
            dtpTo.Name = "dtpTo";
            dtpTo.Size = new Size(150, 32);
            dtpTo.TabIndex = 12;
            dtpTo.ValueChanged += dtpTo_ValueChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(1220, 244);
            label5.Margin = new Padding(4);
            label5.Name = "label5";
            label5.Size = new Size(68, 24);
            label5.TabIndex = 13;
            label5.Text = "الحالة:";
            // 
            // cbStatus
            // 
            cbStatus.BackColor = SystemColors.Control;
            cbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cbStatus.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            cbStatus.FormattingEnabled = true;
            cbStatus.Items.AddRange(new object[] { "الكل", "مسودة", "مرحل", "ملغي" });
            cbStatus.Location = new Point(1294, 240);
            cbStatus.Margin = new Padding(4);
            cbStatus.Name = "cbStatus";
            cbStatus.Size = new Size(150, 32);
            cbStatus.TabIndex = 14;
            cbStatus.SelectedIndexChanged += cbStatus_SelectedIndexChanged;
            // 
            // frmListSales
            // 
            AcceptButton = btnAdd;
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            CancelButton = btnClose;
            ClientSize = new Size(1578, 844);
            Controls.Add(cbStatus);
            Controls.Add(label5);
            Controls.Add(dtpTo);
            Controls.Add(label4);
            Controls.Add(dtpFrom);
            Controls.Add(label3);
            Controls.Add(txtFilterValue);
            Controls.Add(cbFilterBy);
            Controls.Add(label2);
            Controls.Add(lblRecordsCount);
            Controls.Add(label1);
            Controls.Add(btnCancel);
            Controls.Add(btnPost);
            Controls.Add(btnView);
            Controls.Add(btnAdd);
            Controls.Add(dgvSales);
            Controls.Add(lblMode);
            Controls.Add(btnClose);
            Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "frmListSales";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "فواتير البيع";
            Load += frmListSales_Load;
            ((System.ComponentModel.ISupportInitialize)dgvSales).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.DataGridView dgvSales;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.Button btnPost;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblRecordsCount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbFilterBy;
        private System.Windows.Forms.TextBox txtFilterValue;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbStatus;
    }
}
