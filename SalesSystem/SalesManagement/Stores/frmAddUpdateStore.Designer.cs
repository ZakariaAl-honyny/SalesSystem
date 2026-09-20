namespace SalesManagement.Stores
{
    partial class frmAddUpdateStore
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAddUpdateStore));
            pictureBox1 = new PictureBox();
            txtStoreDetails = new TextBox();
            label4 = new Label();
            txtStoreName = new TextBox();
            txtStoreID = new TextBox();
            label3 = new Label();
            label2 = new Label();
            lblTitle = new Label();
            errorProvider1 = new ErrorProvider(components);
            txtStorePhone = new TextBox();
            label1 = new Label();
            txtStoreAddress = new TextBox();
            label5 = new Label();
            label6 = new Label();
            cbxEmployees = new ComboBox();
            btnSaveAndClose = new Button();
            btnSave = new Button();
            btnClose = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.Top;
            pictureBox1.Location = new Point(434, 6);
            pictureBox1.Margin = new Padding(4);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(155, 150);
            pictureBox1.TabIndex = 6;
            pictureBox1.TabStop = false;
            // 
            // txtStoreDetails
            // 
            txtStoreDetails.Anchor = AnchorStyles.Top;
            txtStoreDetails.Location = new Point(226, 473);
            txtStoreDetails.Margin = new Padding(4);
            txtStoreDetails.Multiline = true;
            txtStoreDetails.Name = "txtStoreDetails";
            txtStoreDetails.PasswordChar = '*';
            txtStoreDetails.Size = new Size(571, 100);
            txtStoreDetails.TabIndex = 4;
            txtStoreDetails.UseSystemPasswordChar = true;
            txtStoreDetails.KeyPress += txtFilterValue_KeyPress;
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Top;
            label4.AutoSize = true;
            label4.Location = new Point(50, 473);
            label4.Margin = new Padding(4);
            label4.Name = "label4";
            label4.Size = new Size(168, 29);
            label4.TabIndex = 5;
            label4.Text = "تفاصيل المخزن:";
            label4.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtStoreName
            // 
            txtStoreName.Anchor = AnchorStyles.Top;
            txtStoreName.Location = new Point(226, 277);
            txtStoreName.Margin = new Padding(4);
            txtStoreName.Name = "txtStoreName";
            txtStoreName.Size = new Size(571, 36);
            txtStoreName.TabIndex = 1;
            txtStoreName.KeyPress += txtFilterValue_KeyPress;
            txtStoreName.Validating += _ValidateEmptyTextBox;
            // 
            // txtStoreID
            // 
            txtStoreID.Anchor = AnchorStyles.Top;
            txtStoreID.BackColor = SystemColors.Window;
            txtStoreID.Enabled = false;
            txtStoreID.Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtStoreID.Location = new Point(226, 233);
            txtStoreID.Margin = new Padding(4);
            txtStoreID.Name = "txtStoreID";
            txtStoreID.ReadOnly = true;
            txtStoreID.Size = new Size(571, 36);
            txtStoreID.TabIndex = 0;
            txtStoreID.Text = "0";
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top;
            label3.AutoSize = true;
            label3.Location = new Point(73, 281);
            label3.Margin = new Padding(4);
            label3.Name = "label3";
            label3.Size = new Size(145, 29);
            label3.TabIndex = 2;
            label3.Text = "أسم المخزن:";
            label3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top;
            label2.AutoSize = true;
            label2.Location = new Point(84, 236);
            label2.Margin = new Padding(4);
            label2.Name = "label2";
            label2.Size = new Size(134, 29);
            label2.TabIndex = 1;
            label2.Text = "رقم المخزن:";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTitle
            // 
            lblTitle.Anchor = AnchorStyles.Top;
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Tahoma", 20F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTitle.Location = new Point(338, 164);
            lblTitle.Margin = new Padding(4);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(347, 48);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "إضافة / تعديل مخزن";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // errorProvider1
            // 
            errorProvider1.ContainerControl = this;
            // 
            // txtStorePhone
            // 
            txtStorePhone.Anchor = AnchorStyles.Top;
            txtStorePhone.Location = new Point(226, 321);
            txtStorePhone.Margin = new Padding(4);
            txtStorePhone.Name = "txtStorePhone";
            txtStorePhone.Size = new Size(571, 36);
            txtStorePhone.TabIndex = 2;
            txtStorePhone.KeyPress += txtFilterValue_KeyPress;
            txtStorePhone.Validating += _ValidateEmptyTextBox;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top;
            label1.AutoSize = true;
            label1.Location = new Point(24, 325);
            label1.Margin = new Padding(4);
            label1.Name = "label1";
            label1.Size = new Size(194, 29);
            label1.TabIndex = 8;
            label1.Text = "رقم هاتف المخزن:";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtStoreAddress
            // 
            txtStoreAddress.Anchor = AnchorStyles.Top;
            txtStoreAddress.Location = new Point(226, 365);
            txtStoreAddress.Margin = new Padding(4);
            txtStoreAddress.Multiline = true;
            txtStoreAddress.Name = "txtStoreAddress";
            txtStoreAddress.Size = new Size(571, 100);
            txtStoreAddress.TabIndex = 3;
            txtStoreAddress.KeyPress += txtFilterValue_KeyPress;
            txtStoreAddress.Validating += _ValidateEmptyTextBox;
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Top;
            label5.AutoSize = true;
            label5.Location = new Point(64, 365);
            label5.Margin = new Padding(4);
            label5.Name = "label5";
            label5.Size = new Size(154, 29);
            label5.TabIndex = 10;
            label5.Text = "عنوان المخزن:";
            label5.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            label6.Anchor = AnchorStyles.Top;
            label6.AutoSize = true;
            label6.Location = new Point(77, 584);
            label6.Margin = new Padding(4);
            label6.Name = "label6";
            label6.Size = new Size(141, 29);
            label6.TabIndex = 12;
            label6.Text = "مدير المخزن:";
            label6.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // cbxEmployees
            // 
            cbxEmployees.Anchor = AnchorStyles.Top;
            cbxEmployees.DropDownStyle = ComboBoxStyle.DropDownList;
            cbxEmployees.FormattingEnabled = true;
            cbxEmployees.Items.AddRange(new object[] { "أختر أحد الموظفين" });
            cbxEmployees.Location = new Point(226, 581);
            cbxEmployees.Margin = new Padding(4);
            cbxEmployees.Name = "cbxEmployees";
            cbxEmployees.Size = new Size(571, 37);
            cbxEmployees.TabIndex = 5;
            cbxEmployees.KeyPress += txtFilterValue_KeyPress;
            // 
            // btnSaveAndClose
            // 
            btnSaveAndClose.Anchor = AnchorStyles.Top;
            btnSaveAndClose.BackColor = Color.White;
            btnSaveAndClose.FlatStyle = FlatStyle.Popup;
            btnSaveAndClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnSaveAndClose.Image = Properties.Resources.Save_32;
            btnSaveAndClose.ImageAlign = ContentAlignment.MiddleRight;
            btnSaveAndClose.Location = new Point(226, 626);
            btnSaveAndClose.Margin = new Padding(4);
            btnSaveAndClose.Name = "btnSaveAndClose";
            btnSaveAndClose.Size = new Size(185, 61);
            btnSaveAndClose.TabIndex = 6;
            btnSaveAndClose.Text = "إضافة وإغلاق";
            btnSaveAndClose.TextAlign = ContentAlignment.MiddleLeft;
            btnSaveAndClose.UseVisualStyleBackColor = false;
            btnSaveAndClose.Click += btnSaveAndClose_Click;
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Top;
            btnSave.BackColor = Color.White;
            btnSave.FlatStyle = FlatStyle.Popup;
            btnSave.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnSave.Image = Properties.Resources.Save_32;
            btnSave.ImageAlign = ContentAlignment.MiddleRight;
            btnSave.Location = new Point(419, 626);
            btnSave.Margin = new Padding(4);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(185, 61);
            btnSave.TabIndex = 7;
            btnSave.Text = "إضافة";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Top;
            btnClose.BackColor = Color.White;
            btnClose.DialogResult = DialogResult.Cancel;
            btnClose.FlatStyle = FlatStyle.Popup;
            btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnClose.Image = (Image)resources.GetObject("btnClose.Image");
            btnClose.ImageAlign = ContentAlignment.MiddleRight;
            btnClose.Location = new Point(612, 626);
            btnClose.Margin = new Padding(4);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(185, 61);
            btnClose.TabIndex = 8;
            btnClose.Text = "إغلاق";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // frmAddUpdateStore
            // 
            AcceptButton = btnSave;
            AutoScaleMode = AutoScaleMode.None;
            AutoValidate = AutoValidate.EnableAllowFocusChange;
            CancelButton = btnClose;
            ClientSize = new Size(985, 714);
            Controls.Add(btnSaveAndClose);
            Controls.Add(btnSave);
            Controls.Add(btnClose);
            Controls.Add(cbxEmployees);
            Controls.Add(label6);
            Controls.Add(txtStoreAddress);
            Controls.Add(label5);
            Controls.Add(txtStorePhone);
            Controls.Add(label1);
            Controls.Add(pictureBox1);
            Controls.Add(txtStoreName);
            Controls.Add(txtStoreID);
            Controls.Add(txtStoreDetails);
            Controls.Add(label3);
            Controls.Add(label4);
            Controls.Add(label2);
            Controls.Add(lblTitle);
            Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            KeyPreview = true;
            Margin = new Padding(4, 3, 4, 3);
            Name = "frmAddUpdateStore";
            Padding = new Padding(4);
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "إضافة / تعديل مخزن";
            FormClosing += frmAddUpdateStore_FormClosing;
            Load += frmAddUpdateStore_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox txtStoreDetails;
        private Label label4;
        private TextBox txtStoreName;
        private TextBox txtStoreID;
        private Label label3;
        private Label label2;
        private Label lblTitle;
        private PictureBox pictureBox1;
        private ErrorProvider errorProvider1;
        private TextBox txtStoreAddress;
        private Label label5;
        private TextBox txtStorePhone;
        private Label label1;
        private ComboBox cbxEmployees;
        private Label label6;
        private Button btnSaveAndClose;
        private Button btnSave;
        private Button btnClose;
    }
}