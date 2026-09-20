using SalesManagement.People.Controls;

namespace SalesManagement.Suppliers
{
    partial class frmAddUpdateSupplier
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAddUpdateSupplier));
            tcSupplierInfo = new TabControl();
            tpPersonInfo = new TabPage();
            btnNextSupplierInfo = new Button();
            ctrlPersonCardWithFilter1 = new ctrlPersonCardWithFilter();
            btnNext = new Button();
            tpSupplierInfo = new TabPage();
            txtOwnerPhone = new TextBox();
            label5 = new Label();
            pictureBox3 = new PictureBox();
            txtOwnerName = new TextBox();
            label3 = new Label();
            pictureBox2 = new PictureBox();
            txtManagerPhone = new TextBox();
            label2 = new Label();
            pictureBox1 = new PictureBox();
            txtManagerName = new TextBox();
            label4 = new Label();
            pictureBox4 = new PictureBox();
            btnPrevPersonInfo = new Button();
            pictureBox11 = new PictureBox();
            lblSupplierID = new Label();
            label1 = new Label();
            lblTitle = new Label();
            btnClose = new Button();
            btnSave = new Button();
            errorProvider1 = new ErrorProvider(components);
            btnSaveAndClose = new Button();
            tcSupplierInfo.SuspendLayout();
            tpPersonInfo.SuspendLayout();
            tpSupplierInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox11).BeginInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider1).BeginInit();
            SuspendLayout();
            // 
            // tcSupplierInfo
            // 
            tcSupplierInfo.Anchor = AnchorStyles.Top;
            tcSupplierInfo.Controls.Add(tpPersonInfo);
            tcSupplierInfo.Controls.Add(tpSupplierInfo);
            tcSupplierInfo.Font = new Font("Tahoma", 8F);
            tcSupplierInfo.Location = new Point(8, 55);
            tcSupplierInfo.Margin = new Padding(4);
            tcSupplierInfo.Name = "tcSupplierInfo";
            tcSupplierInfo.RightToLeftLayout = true;
            tcSupplierInfo.SelectedIndex = 0;
            tcSupplierInfo.Size = new Size(1143, 656);
            tcSupplierInfo.TabIndex = 0;
            // 
            // tpPersonInfo
            // 
            tpPersonInfo.BackColor = Color.WhiteSmoke;
            tpPersonInfo.Controls.Add(btnNextSupplierInfo);
            tpPersonInfo.Controls.Add(ctrlPersonCardWithFilter1);
            tpPersonInfo.Controls.Add(btnNext);
            tpPersonInfo.Location = new Point(4, 28);
            tpPersonInfo.Margin = new Padding(3, 4, 3, 4);
            tpPersonInfo.Name = "tpPersonInfo";
            tpPersonInfo.Padding = new Padding(3, 4, 3, 4);
            tpPersonInfo.Size = new Size(1135, 624);
            tpPersonInfo.TabIndex = 0;
            tpPersonInfo.Text = "بيانات الشخص";
            // 
            // btnNextSupplierInfo
            // 
            btnNextSupplierInfo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnNextSupplierInfo.AutoSize = true;
            btnNextSupplierInfo.BackColor = Color.White;
            btnNextSupplierInfo.FlatStyle = FlatStyle.Popup;
            btnNextSupplierInfo.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnNextSupplierInfo.Image = Properties.Resources.Prev_32;
            btnNextSupplierInfo.ImageAlign = ContentAlignment.MiddleRight;
            btnNextSupplierInfo.Location = new Point(9, 553);
            btnNextSupplierInfo.Margin = new Padding(4);
            btnNextSupplierInfo.Name = "btnNextSupplierInfo";
            btnNextSupplierInfo.Size = new Size(351, 63);
            btnNextSupplierInfo.TabIndex = 1;
            btnNextSupplierInfo.Text = "الانتقال إلى أدخل بينات الالمورد";
            btnNextSupplierInfo.TextAlign = ContentAlignment.MiddleLeft;
            btnNextSupplierInfo.UseVisualStyleBackColor = false;
            btnNextSupplierInfo.Click += btnNextSupplierInfo_Click;
            // 
            // ctrlPersonCardWithFilter1
            // 
            ctrlPersonCardWithFilter1.BackColor = Color.WhiteSmoke;
            ctrlPersonCardWithFilter1.Font = new Font("Tahoma", 8F);
            ctrlPersonCardWithFilter1.Location = new Point(0, 0);
            ctrlPersonCardWithFilter1.Margin = new Padding(0);
            ctrlPersonCardWithFilter1.Name = "ctrlPersonCardWithFilter1";
            ctrlPersonCardWithFilter1.RightToLeft = RightToLeft.Yes;
            ctrlPersonCardWithFilter1.Size = new Size(1130, 549);
            ctrlPersonCardWithFilter1.TabIndex = 0;
            ctrlPersonCardWithFilter1.OnPersonSelected += ctrlPersonCardWithFilter1_OnPersonSelected;
            // 
            // btnNext
            // 
            btnNext.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnNext.AutoSize = true;
            btnNext.BackColor = Color.White;
            btnNext.FlatStyle = FlatStyle.Flat;
            btnNext.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnNext.Image = Properties.Resources.Logo_Final;
            btnNext.ImageAlign = ContentAlignment.MiddleRight;
            btnNext.Location = new Point(7327, 3871);
            btnNext.Margin = new Padding(3, 4, 3, 4);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(1342, 1063);
            btnNext.TabIndex = 33;
            btnNext.Text = "Next";
            btnNext.UseVisualStyleBackColor = false;
            // 
            // tpSupplierInfo
            // 
            tpSupplierInfo.BackColor = Color.WhiteSmoke;
            tpSupplierInfo.Controls.Add(txtOwnerPhone);
            tpSupplierInfo.Controls.Add(label5);
            tpSupplierInfo.Controls.Add(pictureBox3);
            tpSupplierInfo.Controls.Add(txtOwnerName);
            tpSupplierInfo.Controls.Add(label3);
            tpSupplierInfo.Controls.Add(pictureBox2);
            tpSupplierInfo.Controls.Add(txtManagerPhone);
            tpSupplierInfo.Controls.Add(label2);
            tpSupplierInfo.Controls.Add(pictureBox1);
            tpSupplierInfo.Controls.Add(txtManagerName);
            tpSupplierInfo.Controls.Add(label4);
            tpSupplierInfo.Controls.Add(pictureBox4);
            tpSupplierInfo.Controls.Add(btnPrevPersonInfo);
            tpSupplierInfo.Controls.Add(pictureBox11);
            tpSupplierInfo.Controls.Add(lblSupplierID);
            tpSupplierInfo.Controls.Add(label1);
            tpSupplierInfo.Location = new Point(4, 28);
            tpSupplierInfo.Margin = new Padding(3, 4, 3, 4);
            tpSupplierInfo.Name = "tpSupplierInfo";
            tpSupplierInfo.Padding = new Padding(3, 4, 3, 4);
            tpSupplierInfo.Size = new Size(1135, 624);
            tpSupplierInfo.TabIndex = 1;
            tpSupplierInfo.Text = "بيانات المورد";
            // 
            // txtOwnerPhone
            // 
            txtOwnerPhone.Font = new Font("Tahoma", 10F);
            txtOwnerPhone.Location = new Point(203, 196);
            txtOwnerPhone.Margin = new Padding(4);
            txtOwnerPhone.Name = "txtOwnerPhone";
            txtOwnerPhone.Size = new Size(567, 32);
            txtOwnerPhone.TabIndex = 1;
            txtOwnerPhone.KeyPress += txtFilterValue_KeyPress;
            txtOwnerPhone.Validating += _ValidateEmptyTextBox;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label5.Location = new Point(833, 200);
            label5.Margin = new Padding(4);
            label5.Name = "label5";
            label5.Size = new Size(132, 24);
            label5.TabIndex = 140;
            label5.Text = "هاتف المالك:";
            // 
            // pictureBox3
            // 
            pictureBox3.Image = Properties.Resources.Phone_32;
            pictureBox3.Location = new Point(778, 187);
            pictureBox3.Margin = new Padding(4);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(47, 51);
            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox3.TabIndex = 141;
            pictureBox3.TabStop = false;
            // 
            // txtOwnerName
            // 
            txtOwnerName.Font = new Font("Tahoma", 10F);
            txtOwnerName.Location = new Point(203, 138);
            txtOwnerName.Margin = new Padding(4);
            txtOwnerName.Name = "txtOwnerName";
            txtOwnerName.Size = new Size(567, 32);
            txtOwnerName.TabIndex = 0;
            txtOwnerName.KeyPress += txtFilterValue_KeyPress;
            txtOwnerName.Validating += _ValidateEmptyTextBox;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label3.Location = new Point(833, 142);
            label3.Margin = new Padding(4);
            label3.Name = "label3";
            label3.Size = new Size(124, 24);
            label3.TabIndex = 137;
            label3.Text = "أسم المالك:";
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Properties.Resources.Person_32;
            pictureBox2.Location = new Point(778, 129);
            pictureBox2.Margin = new Padding(4);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(47, 51);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 138;
            pictureBox2.TabStop = false;
            // 
            // txtManagerPhone
            // 
            txtManagerPhone.Font = new Font("Tahoma", 10F);
            txtManagerPhone.Location = new Point(203, 312);
            txtManagerPhone.Margin = new Padding(4);
            txtManagerPhone.Name = "txtManagerPhone";
            txtManagerPhone.Size = new Size(567, 32);
            txtManagerPhone.TabIndex = 3;
            txtManagerPhone.KeyPress += txtFilterValue_KeyPress;
            txtManagerPhone.Validating += _ValidateEmptyTextBox;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label2.Location = new Point(833, 316);
            label2.Margin = new Padding(4);
            label2.Name = "label2";
            label2.Size = new Size(129, 24);
            label2.TabIndex = 134;
            label2.Text = "هاتف المدير:";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.Phone_32;
            pictureBox1.Location = new Point(778, 303);
            pictureBox1.Margin = new Padding(4);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(47, 51);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 135;
            pictureBox1.TabStop = false;
            // 
            // txtManagerName
            // 
            txtManagerName.Font = new Font("Tahoma", 10F);
            txtManagerName.Location = new Point(203, 254);
            txtManagerName.Margin = new Padding(4);
            txtManagerName.Name = "txtManagerName";
            txtManagerName.Size = new Size(567, 32);
            txtManagerName.TabIndex = 2;
            txtManagerName.KeyPress += txtFilterValue_KeyPress;
            txtManagerName.Validating += _ValidateEmptyTextBox;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label4.Location = new Point(833, 258);
            label4.Margin = new Padding(4);
            label4.Name = "label4";
            label4.Size = new Size(121, 24);
            label4.TabIndex = 131;
            label4.Text = "أسم المدير:";
            label4.Click += label4_Click;
            // 
            // pictureBox4
            // 
            pictureBox4.Image = Properties.Resources.Person_32;
            pictureBox4.Location = new Point(778, 245);
            pictureBox4.Margin = new Padding(4);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new Size(47, 51);
            pictureBox4.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox4.TabIndex = 132;
            pictureBox4.TabStop = false;
            // 
            // btnPrevPersonInfo
            // 
            btnPrevPersonInfo.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnPrevPersonInfo.AutoSize = true;
            btnPrevPersonInfo.BackColor = Color.White;
            btnPrevPersonInfo.FlatStyle = FlatStyle.Popup;
            btnPrevPersonInfo.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnPrevPersonInfo.Image = Properties.Resources.Next_32;
            btnPrevPersonInfo.ImageAlign = ContentAlignment.MiddleLeft;
            btnPrevPersonInfo.Location = new Point(830, 553);
            btnPrevPersonInfo.Margin = new Padding(4);
            btnPrevPersonInfo.Name = "btnPrevPersonInfo";
            btnPrevPersonInfo.Size = new Size(298, 63);
            btnPrevPersonInfo.TabIndex = 4;
            btnPrevPersonInfo.Text = "العودة لأختيار شخص أخر";
            btnPrevPersonInfo.TextAlign = ContentAlignment.MiddleRight;
            btnPrevPersonInfo.UseVisualStyleBackColor = false;
            btnPrevPersonInfo.Click += btnPrevPersonInfo_Click;
            // 
            // pictureBox11
            // 
            pictureBox11.Image = Properties.Resources.Number_32;
            pictureBox11.Location = new Point(778, 71);
            pictureBox11.Margin = new Padding(4);
            pictureBox11.Name = "pictureBox11";
            pictureBox11.Size = new Size(47, 51);
            pictureBox11.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox11.TabIndex = 123;
            pictureBox11.TabStop = false;
            // 
            // lblSupplierID
            // 
            lblSupplierID.AutoSize = true;
            lblSupplierID.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblSupplierID.Location = new Point(719, 84);
            lblSupplierID.Margin = new Padding(4);
            lblSupplierID.Name = "lblSupplierID";
            lblSupplierID.Size = new Size(51, 24);
            lblSupplierID.TabIndex = 122;
            lblSupplierID.Text = "N/A";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.FlatStyle = FlatStyle.Flat;
            label1.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label1.Location = new Point(833, 84);
            label1.Margin = new Padding(4);
            label1.Name = "label1";
            label1.Size = new Size(115, 24);
            label1.TabIndex = 121;
            label1.Text = "رقم المورد:";
            // 
            // lblTitle
            // 
            lblTitle.Anchor = AnchorStyles.Top;
            lblTitle.AutoSize = true;
            lblTitle.BackColor = Color.WhiteSmoke;
            lblTitle.FlatStyle = FlatStyle.Flat;
            lblTitle.Font = new Font("Tahoma", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.ForeColor = Color.Firebrick;
            lblTitle.Location = new Point(413, 8);
            lblTitle.Margin = new Padding(4);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(305, 39);
            lblTitle.TabIndex = 30;
            lblTitle.Text = "إضافة / تعديل مورد";
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Top;
            btnClose.AutoSize = true;
            btnClose.BackColor = Color.White;
            btnClose.DialogResult = DialogResult.Cancel;
            btnClose.FlatStyle = FlatStyle.Popup;
            btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnClose.Image = (Image)resources.GetObject("btnClose.Image");
            btnClose.ImageAlign = ContentAlignment.MiddleRight;
            btnClose.Location = new Point(403, 715);
            btnClose.Margin = new Padding(4);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(185, 61);
            btnClose.TabIndex = 7;
            btnClose.Text = "إغلاق";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Top;
            btnSave.AutoSize = true;
            btnSave.BackColor = Color.White;
            btnSave.Enabled = false;
            btnSave.FlatStyle = FlatStyle.Popup;
            btnSave.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnSave.Image = (Image)resources.GetObject("btnSave.Image");
            btnSave.ImageAlign = ContentAlignment.MiddleRight;
            btnSave.Location = new Point(210, 715);
            btnSave.Margin = new Padding(4);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(185, 61);
            btnSave.TabIndex = 6;
            btnSave.Text = "إضافة";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // errorProvider1
            // 
            errorProvider1.ContainerControl = this;
            // 
            // btnSaveAndClose
            // 
            btnSaveAndClose.Anchor = AnchorStyles.Top;
            btnSaveAndClose.BackColor = Color.White;
            btnSaveAndClose.Enabled = false;
            btnSaveAndClose.FlatStyle = FlatStyle.Popup;
            btnSaveAndClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnSaveAndClose.Image = Properties.Resources.Save_32;
            btnSaveAndClose.ImageAlign = ContentAlignment.MiddleRight;
            btnSaveAndClose.Location = new Point(17, 715);
            btnSaveAndClose.Margin = new Padding(4);
            btnSaveAndClose.Name = "btnSaveAndClose";
            btnSaveAndClose.Size = new Size(185, 61);
            btnSaveAndClose.TabIndex = 5;
            btnSaveAndClose.Text = "إضافة وإغلاق";
            btnSaveAndClose.TextAlign = ContentAlignment.MiddleLeft;
            btnSaveAndClose.UseVisualStyleBackColor = false;
            btnSaveAndClose.Click += btnSaveAndClose_Click;
            // 
            // frmAddUpdateSupplier
            // 
            AutoScaleMode = AutoScaleMode.None;
            AutoValidate = AutoValidate.EnableAllowFocusChange;
            BackColor = Color.WhiteSmoke;
            CancelButton = btnClose;
            ClientSize = new Size(1159, 789);
            Controls.Add(btnSaveAndClose);
            Controls.Add(btnClose);
            Controls.Add(btnSave);
            Controls.Add(lblTitle);
            Controls.Add(tcSupplierInfo);
            Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(3, 4, 3, 4);
            Name = "frmAddUpdateSupplier";
            Padding = new Padding(4);
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "إضافة / تعديل مورد";
            Activated += frmAddUpdateSupplier_Activated;
            FormClosing += frmAddUpdateSupplier_FormClosing;
            Load += frmAddUpdateSupplier_Load;
            tcSupplierInfo.ResumeLayout(false);
            tpPersonInfo.ResumeLayout(false);
            tpPersonInfo.PerformLayout();
            tpSupplierInfo.ResumeLayout(false);
            tpSupplierInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox11).EndInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider1).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tcSupplierInfo;
        private System.Windows.Forms.TabPage tpPersonInfo;
        private System.Windows.Forms.TabPage tpSupplierInfo;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private ctrlPersonCardWithFilter ctrlPersonCardWithFilter1;
        private System.Windows.Forms.Button btnNextSupplierInfo;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Button btnPrevPersonInfo;
        private System.Windows.Forms.PictureBox pictureBox11;
        private System.Windows.Forms.Label lblSupplierID;
        private System.Windows.Forms.Label label1;
        private TextBox txtManagerName;
        private Label label4;
        private PictureBox pictureBox4;
        private TextBox txtOwnerPhone;
        private Label label5;
        private PictureBox pictureBox3;
        private TextBox txtOwnerName;
        private Label label3;
        private PictureBox pictureBox2;
        private TextBox txtManagerPhone;
        private Label label2;
        private PictureBox pictureBox1;
        private Button btnSaveAndClose;
    }
}