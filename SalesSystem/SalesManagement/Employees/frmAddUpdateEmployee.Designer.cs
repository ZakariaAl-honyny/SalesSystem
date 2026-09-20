using SalesManagement.People.Controls;

namespace SalesManagement.Employees
{
    partial class frmAddUpdateEmployee
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAddUpdateEmployee));
            tcEmployeeInfo = new TabControl();
            tpPersonInfo = new TabPage();
            btnNextEmployeeInfo = new Button();
            ctrlPersonCardWithFilter1 = new ctrlPersonCardWithFilter();
            btnNext = new Button();
            tpEmployeeInfo = new TabPage();
            cbxEmployeeState = new ComboBox();
            cbxEmployeeQualification = new ComboBox();
            btnPrevPersonInfo = new Button();
            pictureBox11 = new PictureBox();
            lblEmployeeID = new Label();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            pictureBox3 = new PictureBox();
            pictureBox2 = new PictureBox();
            lblTitle = new Label();
            btnClose = new Button();
            btnSave = new Button();
            errorProvider1 = new ErrorProvider(components);
            btnSaveAndClose = new Button();
            tcEmployeeInfo.SuspendLayout();
            tpPersonInfo.SuspendLayout();
            tpEmployeeInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox11).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider1).BeginInit();
            SuspendLayout();
            // 
            // tcEmployeeInfo
            // 
            tcEmployeeInfo.Anchor = AnchorStyles.Top;
            tcEmployeeInfo.Controls.Add(tpPersonInfo);
            tcEmployeeInfo.Controls.Add(tpEmployeeInfo);
            tcEmployeeInfo.Font = new Font("Tahoma", 8F);
            tcEmployeeInfo.Location = new Point(8, 55);
            tcEmployeeInfo.Margin = new Padding(4);
            tcEmployeeInfo.Name = "tcEmployeeInfo";
            tcEmployeeInfo.RightToLeftLayout = true;
            tcEmployeeInfo.SelectedIndex = 0;
            tcEmployeeInfo.Size = new Size(1143, 656);
            tcEmployeeInfo.TabIndex = 0;
            // 
            // tpPersonInfo
            // 
            tpPersonInfo.BackColor = Color.WhiteSmoke;
            tpPersonInfo.Controls.Add(btnNextEmployeeInfo);
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
            // btnNextEmployeeInfo
            // 
            btnNextEmployeeInfo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnNextEmployeeInfo.AutoSize = true;
            btnNextEmployeeInfo.BackColor = Color.White;
            btnNextEmployeeInfo.FlatStyle = FlatStyle.Popup;
            btnNextEmployeeInfo.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnNextEmployeeInfo.Image = Properties.Resources.Prev_32;
            btnNextEmployeeInfo.ImageAlign = ContentAlignment.MiddleRight;
            btnNextEmployeeInfo.Location = new Point(9, 553);
            btnNextEmployeeInfo.Margin = new Padding(4);
            btnNextEmployeeInfo.Name = "btnNextEmployeeInfo";
            btnNextEmployeeInfo.Size = new Size(351, 63);
            btnNextEmployeeInfo.TabIndex = 1;
            btnNextEmployeeInfo.Text = "الانتقال إلى أدخل بينات الموظف";
            btnNextEmployeeInfo.TextAlign = ContentAlignment.MiddleLeft;
            btnNextEmployeeInfo.UseVisualStyleBackColor = false;
            btnNextEmployeeInfo.Click += btnNextEmployeeInfo_Click;
            // 
            // ctrlPersonCardWithFilter1
            // 
            ctrlPersonCardWithFilter1.BackColor = Color.WhiteSmoke;
            ctrlPersonCardWithFilter1.Font = new Font("Tahoma", 8F);
            ctrlPersonCardWithFilter1.Location = new Point(0, 0);
            ctrlPersonCardWithFilter1.Margin = new Padding(4, 5, 4, 5);
            ctrlPersonCardWithFilter1.Name = "ctrlPersonCardWithFilter1";
            ctrlPersonCardWithFilter1.RightToLeft = RightToLeft.Yes;
            ctrlPersonCardWithFilter1.Size = new Size(1130, 557);
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
            // tpEmployeeInfo
            // 
            tpEmployeeInfo.BackColor = Color.WhiteSmoke;
            tpEmployeeInfo.Controls.Add(cbxEmployeeState);
            tpEmployeeInfo.Controls.Add(cbxEmployeeQualification);
            tpEmployeeInfo.Controls.Add(btnPrevPersonInfo);
            tpEmployeeInfo.Controls.Add(pictureBox11);
            tpEmployeeInfo.Controls.Add(lblEmployeeID);
            tpEmployeeInfo.Controls.Add(label1);
            tpEmployeeInfo.Controls.Add(label2);
            tpEmployeeInfo.Controls.Add(label3);
            tpEmployeeInfo.Controls.Add(pictureBox3);
            tpEmployeeInfo.Controls.Add(pictureBox2);
            tpEmployeeInfo.Location = new Point(4, 28);
            tpEmployeeInfo.Margin = new Padding(3, 4, 3, 4);
            tpEmployeeInfo.Name = "tpEmployeeInfo";
            tpEmployeeInfo.Padding = new Padding(3, 4, 3, 4);
            tpEmployeeInfo.Size = new Size(1135, 624);
            tpEmployeeInfo.TabIndex = 1;
            tpEmployeeInfo.Text = "بيانات الموظف";
            // 
            // cbxEmployeeState
            // 
            cbxEmployeeState.DropDownStyle = ComboBoxStyle.DropDownList;
            cbxEmployeeState.Font = new Font("Tahoma", 12F);
            cbxEmployeeState.FormattingEnabled = true;
            cbxEmployeeState.Items.AddRange(new object[] { "أعزب", "متزوج", "أرمل", "مطلق" });
            cbxEmployeeState.Location = new Point(443, 196);
            cbxEmployeeState.Margin = new Padding(4);
            cbxEmployeeState.Name = "cbxEmployeeState";
            cbxEmployeeState.Size = new Size(327, 37);
            cbxEmployeeState.TabIndex = 125;
            cbxEmployeeState.Validating += _ValidateEmptyComboBox;
            // 
            // cbxEmployeeQualification
            // 
            cbxEmployeeQualification.DropDownStyle = ComboBoxStyle.DropDownList;
            cbxEmployeeQualification.Font = new Font("Tahoma", 12F);
            cbxEmployeeQualification.FormattingEnabled = true;
            cbxEmployeeQualification.Items.AddRange(new object[] { "لا يوجد", "بكالوريوس", "دبلوم", "ثانوية عامة", "تاسع أساسي" });
            cbxEmployeeQualification.Location = new Point(443, 137);
            cbxEmployeeQualification.Margin = new Padding(4);
            cbxEmployeeQualification.Name = "cbxEmployeeQualification";
            cbxEmployeeQualification.Size = new Size(327, 37);
            cbxEmployeeQualification.TabIndex = 124;
            cbxEmployeeQualification.Validating += _ValidateEmptyComboBox;
            // 
            // btnPrevPersonInfo
            // 
            btnPrevPersonInfo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnPrevPersonInfo.BackColor = Color.White;
            btnPrevPersonInfo.FlatStyle = FlatStyle.Popup;
            btnPrevPersonInfo.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnPrevPersonInfo.Image = Properties.Resources.Next_32;
            btnPrevPersonInfo.ImageAlign = ContentAlignment.MiddleLeft;
            btnPrevPersonInfo.Location = new Point(830, 553);
            btnPrevPersonInfo.Margin = new Padding(4);
            btnPrevPersonInfo.Name = "btnPrevPersonInfo";
            btnPrevPersonInfo.Size = new Size(298, 63);
            btnPrevPersonInfo.TabIndex = 113;
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
            // lblEmployeeID
            // 
            lblEmployeeID.AutoSize = true;
            lblEmployeeID.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblEmployeeID.Location = new Point(719, 84);
            lblEmployeeID.Margin = new Padding(4);
            lblEmployeeID.Name = "lblEmployeeID";
            lblEmployeeID.Size = new Size(51, 24);
            lblEmployeeID.TabIndex = 122;
            lblEmployeeID.Text = "N/A";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.FlatStyle = FlatStyle.Flat;
            label1.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label1.Location = new Point(833, 84);
            label1.Margin = new Padding(4);
            label1.Name = "label1";
            label1.Size = new Size(208, 24);
            label1.TabIndex = 121;
            label1.Text = "رقم المعرف للموظف:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label2.Location = new Point(833, 143);
            label2.Margin = new Padding(4);
            label2.Name = "label2";
            label2.Size = new Size(152, 24);
            label2.TabIndex = 116;
            label2.Text = "مؤهل الموظف:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label3.Location = new Point(833, 202);
            label3.Margin = new Padding(4);
            label3.Name = "label3";
            label3.Size = new Size(139, 24);
            label3.TabIndex = 114;
            label3.Text = "حالة الموظف:";
            // 
            // pictureBox3
            // 
            pictureBox3.Image = Properties.Resources.Question_32;
            pictureBox3.Location = new Point(778, 189);
            pictureBox3.Margin = new Padding(4);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(47, 51);
            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox3.TabIndex = 119;
            pictureBox3.TabStop = false;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Properties.Resources.User_32__2;
            pictureBox2.Location = new Point(778, 130);
            pictureBox2.Margin = new Padding(4);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(47, 51);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 118;
            pictureBox2.TabStop = false;
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
            lblTitle.Size = new Size(333, 39);
            lblTitle.TabIndex = 30;
            lblTitle.Text = "إضافة / تعديل موظف";
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
            btnClose.Location = new Point(398, 715);
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
            btnSave.AutoSize = true;
            btnSave.BackColor = Color.White;
            btnSave.Enabled = false;
            btnSave.FlatStyle = FlatStyle.Popup;
            btnSave.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnSave.Image = (Image)resources.GetObject("btnSave.Image");
            btnSave.ImageAlign = ContentAlignment.MiddleRight;
            btnSave.Location = new Point(205, 715);
            btnSave.Margin = new Padding(4);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(185, 61);
            btnSave.TabIndex = 5;
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
            btnSaveAndClose.Location = new Point(12, 715);
            btnSaveAndClose.Margin = new Padding(4);
            btnSaveAndClose.Name = "btnSaveAndClose";
            btnSaveAndClose.Size = new Size(185, 61);
            btnSaveAndClose.TabIndex = 158;
            btnSaveAndClose.Text = "إضافة وإغلاق";
            btnSaveAndClose.TextAlign = ContentAlignment.MiddleLeft;
            btnSaveAndClose.UseVisualStyleBackColor = false;
            btnSaveAndClose.Click += btnSaveAndClose_Click;
            // 
            // frmAddUpdateEmployee
            // 
            AutoScaleMode = AutoScaleMode.None;
            AutoValidate = AutoValidate.EnableAllowFocusChange;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(1159, 789);
            Controls.Add(btnSaveAndClose);
            Controls.Add(btnClose);
            Controls.Add(btnSave);
            Controls.Add(lblTitle);
            Controls.Add(tcEmployeeInfo);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(3, 4, 3, 4);
            MaximizeBox = false;
            Name = "frmAddUpdateEmployee";
            Padding = new Padding(4);
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "إضافة / تعديل موظف";
            Activated += frmAddUpdateEmployee_Activated;
            FormClosing += frmAddUpdateEmployee_FormClosing;
            Load += frmAddUpdateEmployee_Load;
            tcEmployeeInfo.ResumeLayout(false);
            tpPersonInfo.ResumeLayout(false);
            tpPersonInfo.PerformLayout();
            tpEmployeeInfo.ResumeLayout(false);
            tpEmployeeInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox11).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider1).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tcEmployeeInfo;
        private System.Windows.Forms.TabPage tpPersonInfo;
        private System.Windows.Forms.TabPage tpEmployeeInfo;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private ctrlPersonCardWithFilter ctrlPersonCardWithFilter1;
        private System.Windows.Forms.Button btnNextEmployeeInfo;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Button btnPrevPersonInfo;
        private System.Windows.Forms.PictureBox pictureBox11;
        private System.Windows.Forms.Label lblEmployeeID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox2;
        private ComboBox cbxEmployeeState;
        private ComboBox cbxEmployeeQualification;
        private Button btnSaveAndClose;
    }
}