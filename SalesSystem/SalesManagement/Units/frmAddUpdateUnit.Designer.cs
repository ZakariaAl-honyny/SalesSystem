namespace SalesManagement.Unit
{
    partial class frmAddUpdateUnit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAddUpdateUnit));
            pictureBox1 = new PictureBox();
            txtUnitDescription = new TextBox();
            label4 = new Label();
            txtUnitName = new TextBox();
            txtUnitID = new TextBox();
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
            // txtUnitDescription
            // 
            txtUnitDescription.Anchor = AnchorStyles.Top;
            txtUnitDescription.Location = new Point(217, 358);
            txtUnitDescription.Margin = new Padding(8);
            txtUnitDescription.Multiline = true;
            txtUnitDescription.Name = "txtUnitDescription";
            txtUnitDescription.Size = new Size(571, 150);
            txtUnitDescription.TabIndex = 2;
            txtUnitDescription.KeyPress += txtFilterValue_KeyPress;
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Top;
            label4.AutoSize = true;
            label4.Location = new Point(43, 358);
            label4.Margin = new Padding(4);
            label4.Name = "label4";
            label4.Size = new Size(162, 29);
            label4.TabIndex = 5;
            label4.Text = "تفاصيل الوحدة:";
            label4.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtUnitName
            // 
            txtUnitName.Anchor = AnchorStyles.Top;
            txtUnitName.Location = new Point(217, 306);
            txtUnitName.Margin = new Padding(8);
            txtUnitName.Name = "txtUnitName";
            txtUnitName.Size = new Size(571, 36);
            txtUnitName.TabIndex = 1;
            txtUnitName.KeyPress += txtFilterValue_KeyPress;
            txtUnitName.Validating += _ValidateEmptyTextBox;
            // 
            // txtUnitID
            // 
            txtUnitID.Anchor = AnchorStyles.Top;
            txtUnitID.BackColor = SystemColors.Window;
            txtUnitID.Enabled = false;
            txtUnitID.Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtUnitID.Location = new Point(217, 254);
            txtUnitID.Margin = new Padding(8);
            txtUnitID.Name = "txtUnitID";
            txtUnitID.ReadOnly = true;
            txtUnitID.Size = new Size(571, 36);
            txtUnitID.TabIndex = 0;
            txtUnitID.Text = "0";
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top;
            label3.AutoSize = true;
            label3.Location = new Point(66, 310);
            label3.Margin = new Padding(4);
            label3.Name = "label3";
            label3.Size = new Size(139, 29);
            label3.TabIndex = 2;
            label3.Text = "أسم الوحدة:";
            label3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top;
            label2.AutoSize = true;
            label2.Location = new Point(77, 257);
            label2.Margin = new Padding(4);
            label2.Name = "label2";
            label2.Size = new Size(128, 29);
            label2.TabIndex = 1;
            label2.Text = "رقم الوحدة:";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTitle
            // 
            lblTitle.Anchor = AnchorStyles.Top;
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Tahoma", 20F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTitle.Location = new Point(332, 163);
            lblTitle.Margin = new Padding(4);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(341, 48);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "إضافة / تعديل وحدة";
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
            btnClose.TabIndex = 6;
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
            btnSave.TabIndex = 5;
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
            btnSaveAndClose.TabIndex = 4;
            btnSaveAndClose.Text = "إضافة وإغلاق";
            btnSaveAndClose.TextAlign = ContentAlignment.MiddleLeft;
            btnSaveAndClose.UseVisualStyleBackColor = false;
            btnSaveAndClose.Click += btnSaveAndClose_Click;
            // 
            // frmAddUpdateUnit
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
            Controls.Add(txtUnitName);
            Controls.Add(txtUnitID);
            Controls.Add(txtUnitDescription);
            Controls.Add(label3);
            Controls.Add(label4);
            Controls.Add(label2);
            Controls.Add(lblTitle);
            Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(4, 3, 4, 3);
            Name = "frmAddUpdateUnit";
            Padding = new Padding(4);
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "إضافة / تعديل وحدة";
            FormClosing += frmAddUpdateUnit_FormClosing;
            Load += frmAddUpdateUnit_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox txtUnitDescription;
        private Label label4;
        private TextBox txtUnitName;
        private TextBox txtUnitID;
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