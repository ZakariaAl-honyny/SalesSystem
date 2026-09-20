namespace SalesManagement.Revenues
{
    partial class frmAddUpdateRevenue
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAddUpdateRevenue));
            pictureBox1 = new PictureBox();
            txtRevenueDescription = new TextBox();
            label4 = new Label();
            txtRevenueName = new TextBox();
            txtRevenueID = new TextBox();
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
            // txtRevenueDescription
            // 
            txtRevenueDescription.Anchor = AnchorStyles.Top;
            txtRevenueDescription.Location = new Point(217, 358);
            txtRevenueDescription.Margin = new Padding(8);
            txtRevenueDescription.Multiline = true;
            txtRevenueDescription.Name = "txtRevenueDescription";
            txtRevenueDescription.PasswordChar = '*';
            txtRevenueDescription.Size = new Size(571, 150);
            txtRevenueDescription.TabIndex = 2;
            txtRevenueDescription.UseSystemPasswordChar = true;
            txtRevenueDescription.KeyPress += txtFilterValue_KeyPress;
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Top;
            label4.AutoSize = true;
            label4.Location = new Point(52, 358);
            label4.Margin = new Padding(4);
            label4.Name = "label4";
            label4.Size = new Size(153, 29);
            label4.TabIndex = 5;
            label4.Text = "تفاصيل الايراد:";
            label4.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtRevenueName
            // 
            txtRevenueName.Anchor = AnchorStyles.Top;
            txtRevenueName.Location = new Point(217, 306);
            txtRevenueName.Margin = new Padding(8);
            txtRevenueName.Name = "txtRevenueName";
            txtRevenueName.Size = new Size(571, 36);
            txtRevenueName.TabIndex = 1;
            txtRevenueName.KeyPress += txtFilterValue_KeyPress;
            txtRevenueName.Validating += _ValidateEmptyTextBox;
            // 
            // txtRevenueID
            // 
            txtRevenueID.Anchor = AnchorStyles.Top;
            txtRevenueID.BackColor = SystemColors.Window;
            txtRevenueID.Enabled = false;
            txtRevenueID.Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtRevenueID.Location = new Point(217, 254);
            txtRevenueID.Margin = new Padding(8);
            txtRevenueID.Name = "txtRevenueID";
            txtRevenueID.ReadOnly = true;
            txtRevenueID.Size = new Size(571, 36);
            txtRevenueID.TabIndex = 0;
            txtRevenueID.Text = "0";
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top;
            label3.AutoSize = true;
            label3.Location = new Point(75, 309);
            label3.Margin = new Padding(4);
            label3.Name = "label3";
            label3.Size = new Size(130, 29);
            label3.TabIndex = 2;
            label3.Text = "أسم الايراد:";
            label3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top;
            label2.AutoSize = true;
            label2.Location = new Point(86, 257);
            label2.Margin = new Padding(4);
            label2.Name = "label2";
            label2.Size = new Size(119, 29);
            label2.TabIndex = 1;
            label2.Text = "رقم الايراد:";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTitle
            // 
            lblTitle.Anchor = AnchorStyles.Top;
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Tahoma", 20F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTitle.Location = new Point(340, 163);
            lblTitle.Margin = new Padding(4);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(324, 48);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "إضافة / تعديل ايراد";
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
            btnClose.Location = new Point(603, 520);
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
            btnSave.ForeColor = SystemColors.ControlText;
            btnSave.Image = (Image)resources.GetObject("btnSave.Image");
            btnSave.ImageAlign = ContentAlignment.MiddleRight;
            btnSave.Location = new Point(410, 520);
            btnSave.Margin = new Padding(4);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(185, 61);
            btnSave.TabIndex = 134;
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
            btnSaveAndClose.Location = new Point(217, 520);
            btnSaveAndClose.Margin = new Padding(4);
            btnSaveAndClose.Name = "btnSaveAndClose";
            btnSaveAndClose.Size = new Size(185, 61);
            btnSaveAndClose.TabIndex = 136;
            btnSaveAndClose.Text = "إضافة وإغلاق";
            btnSaveAndClose.TextAlign = ContentAlignment.MiddleLeft;
            btnSaveAndClose.UseVisualStyleBackColor = false;
            btnSaveAndClose.Click += btnSaveAndClose_Click;
            // 
            // frmAddUpdateRevenue
            // 
            AcceptButton = btnSave;
            AutoScaleMode = AutoScaleMode.None;
            AutoValidate = AutoValidate.EnableAllowFocusChange;
            CancelButton = btnClose;
            ClientSize = new Size(985, 612);
            Controls.Add(btnSaveAndClose);
            Controls.Add(btnClose);
            Controls.Add(btnSave);
            Controls.Add(pictureBox1);
            Controls.Add(txtRevenueName);
            Controls.Add(txtRevenueID);
            Controls.Add(txtRevenueDescription);
            Controls.Add(label3);
            Controls.Add(label4);
            Controls.Add(label2);
            Controls.Add(lblTitle);
            Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            KeyPreview = true;
            Margin = new Padding(4, 3, 4, 3);
            Name = "frmAddUpdateRevenue";
            Padding = new Padding(4);
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "إضافة / تعديل ايراد";
            FormClosing += frmAddUpdateRevenue_FormClosing;
            Load += frmAddUpdateRevenue_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox txtRevenueDescription;
        private Label label4;
        private TextBox txtRevenueName;
        private TextBox txtRevenueID;
        private Label label3;
        private Label label2;
        private Label lblTitle;
        private PictureBox pictureBox1;
        private ErrorProvider errorProvider1;
        private Button btnClose;
        private Button btnSave;
        private Button btnSaveAndClose;
    }
}