namespace SalesManagement.Categories
{
    partial class frmAddUpdateCategory
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAddUpdateCategory));
            pictureBox1 = new PictureBox();
            txtCategoryDescription = new TextBox();
            label4 = new Label();
            txtCategoryName = new TextBox();
            txtCategoryID = new TextBox();
            label3 = new Label();
            label2 = new Label();
            lblTitle = new Label();
            errorProvider1 = new ErrorProvider(components);
            btnClose = new Button();
            btnSave = new Button();
            btnSaveAndClose = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.Top;
            pictureBox1.Location = new Point(425, 10);
            pictureBox1.Margin = new Padding(4);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(155, 150);
            pictureBox1.TabIndex = 6;
            pictureBox1.TabStop = false;
            // 
            // txtCategoryDescription
            // 
            txtCategoryDescription.Anchor = AnchorStyles.Top;
            txtCategoryDescription.Location = new Point(219, 361);
            txtCategoryDescription.Margin = new Padding(8);
            txtCategoryDescription.Multiline = true;
            txtCategoryDescription.Name = "txtCategoryDescription";
            txtCategoryDescription.PasswordChar = '*';
            txtCategoryDescription.Size = new Size(567, 150);
            txtCategoryDescription.TabIndex = 2;
            txtCategoryDescription.UseSystemPasswordChar = true;
            txtCategoryDescription.KeyPress += txtFilterValue_KeyPress;
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Top;
            label4.AutoSize = true;
            label4.Location = new Point(32, 361);
            label4.Margin = new Padding(4);
            label4.Name = "label4";
            label4.Size = new Size(175, 29);
            label4.TabIndex = 5;
            label4.Text = "تفاصيل التصنيف:";
            label4.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtCategoryName
            // 
            txtCategoryName.Anchor = AnchorStyles.Top;
            txtCategoryName.Location = new Point(219, 309);
            txtCategoryName.Margin = new Padding(8);
            txtCategoryName.Name = "txtCategoryName";
            txtCategoryName.Size = new Size(567, 36);
            txtCategoryName.TabIndex = 1;
            txtCategoryName.KeyPress += txtFilterValue_KeyPress;
            txtCategoryName.Validating += _ValidateEmptyTextBox;
            // 
            // txtCategoryID
            // 
            txtCategoryID.Anchor = AnchorStyles.Top;
            txtCategoryID.BackColor = SystemColors.Window;
            txtCategoryID.Enabled = false;
            txtCategoryID.Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtCategoryID.Location = new Point(219, 257);
            txtCategoryID.Margin = new Padding(8);
            txtCategoryID.Name = "txtCategoryID";
            txtCategoryID.ReadOnly = true;
            txtCategoryID.Size = new Size(567, 36);
            txtCategoryID.TabIndex = 0;
            txtCategoryID.Text = "0";
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top;
            label3.AutoSize = true;
            label3.Location = new Point(55, 313);
            label3.Margin = new Padding(4);
            label3.Name = "label3";
            label3.Size = new Size(152, 29);
            label3.TabIndex = 2;
            label3.Text = "أسم التصنيف:";
            label3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top;
            label2.AutoSize = true;
            label2.Location = new Point(66, 261);
            label2.Margin = new Padding(4);
            label2.Name = "label2";
            label2.Size = new Size(141, 29);
            label2.TabIndex = 1;
            label2.Text = "رقم التصنيف:";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTitle
            // 
            lblTitle.Anchor = AnchorStyles.Top;
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Tahoma", 20F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTitle.Location = new Point(321, 166);
            lblTitle.Margin = new Padding(4);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(363, 48);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "إضافة / تعديل تصنيف";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // errorProvider1
            // 
            errorProvider1.ContainerControl = this;
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
            btnClose.Location = new Point(601, 523);
            btnClose.Margin = new Padding(4);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(185, 61);
            btnClose.TabIndex = 135;
            btnClose.Text = "إغلاق";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Top;
            btnSave.BackColor = Color.White;
            btnSave.FlatStyle = FlatStyle.Popup;
            btnSave.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnSave.Image = Properties.Resources.Save_32;
            btnSave.ImageAlign = ContentAlignment.MiddleRight;
            btnSave.Location = new Point(412, 523);
            btnSave.Margin = new Padding(4);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(185, 61);
            btnSave.TabIndex = 136;
            btnSave.Text = "إضافة";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // btnSaveAndClose
            // 
            btnSaveAndClose.Anchor = AnchorStyles.Top;
            btnSaveAndClose.BackColor = Color.White;
            btnSaveAndClose.FlatStyle = FlatStyle.Popup;
            btnSaveAndClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnSaveAndClose.Image = Properties.Resources.Save_32;
            btnSaveAndClose.ImageAlign = ContentAlignment.MiddleRight;
            btnSaveAndClose.Location = new Point(219, 523);
            btnSaveAndClose.Margin = new Padding(4);
            btnSaveAndClose.Name = "btnSaveAndClose";
            btnSaveAndClose.Size = new Size(185, 61);
            btnSaveAndClose.TabIndex = 137;
            btnSaveAndClose.Text = "إضافة وإغلاق";
            btnSaveAndClose.TextAlign = ContentAlignment.MiddleLeft;
            btnSaveAndClose.UseVisualStyleBackColor = false;
            btnSaveAndClose.Click += btnSaveAndClose_Click;
            // 
            // frmAddUpdateCategory
            // 
            AcceptButton = btnSave;
            AutoScaleMode = AutoScaleMode.None;
            AutoValidate = AutoValidate.EnableAllowFocusChange;
            CancelButton = btnClose;
            ClientSize = new Size(985, 612);
            Controls.Add(btnSaveAndClose);
            Controls.Add(btnSave);
            Controls.Add(btnClose);
            Controls.Add(pictureBox1);
            Controls.Add(txtCategoryName);
            Controls.Add(txtCategoryID);
            Controls.Add(txtCategoryDescription);
            Controls.Add(label3);
            Controls.Add(label4);
            Controls.Add(label2);
            Controls.Add(lblTitle);
            Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(4, 3, 4, 3);
            Name = "frmAddUpdateCategory";
            Padding = new Padding(4);
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "إضافة / تعديل تصنيف";
            FormClosing += frmAddUpdateCategory_FormClosing;
            Load += frmAddUpdateCategory_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox txtCategoryDescription;
        private Label label4;
        private Button btnClose;
        private Button btnSave1;
        private TextBox txtCategoryName;
        private TextBox txtCategoryID;
        private Label label3;
        private Label label2;
        private Label lblTitle;
        private PictureBox pictureBox1;
        private ErrorProvider errorProvider1;
        private Button button1;
        private Button btnSave;
        private Button btnSaveAndClose;
        //private Button btnSave1;
    }
}