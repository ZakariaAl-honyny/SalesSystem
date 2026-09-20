using SalesManagement.People.Controls;

namespace SalesManagement.Delegates
{
    partial class frmAddUpdateDelegate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAddUpdateDelegate));
            tcDelegateInfo = new TabControl();
            tpPersonInfo = new TabPage();
            btnNextDelegateInfo = new Button();
            ctrlPersonCardWithFilter1 = new ctrlPersonCardWithFilter();
            btnNext = new Button();
            tpDelegateInfo = new TabPage();
            btnPrevPersonInfo = new Button();
            pictureBox11 = new PictureBox();
            lblDelegateID = new Label();
            label1 = new Label();
            lblTitle = new Label();
            btnClose = new Button();
            btnSave = new Button();
            errorProvider1 = new ErrorProvider(components);
            btnSaveAndClose = new Button();
            tcDelegateInfo.SuspendLayout();
            tpPersonInfo.SuspendLayout();
            tpDelegateInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox11).BeginInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider1).BeginInit();
            SuspendLayout();
            // 
            // tcDelegateInfo
            // 
            tcDelegateInfo.Anchor = AnchorStyles.Top;
            tcDelegateInfo.Controls.Add(tpPersonInfo);
            tcDelegateInfo.Controls.Add(tpDelegateInfo);
            tcDelegateInfo.Font = new Font("Tahoma", 8F);
            tcDelegateInfo.Location = new Point(8, 55);
            tcDelegateInfo.Margin = new Padding(4);
            tcDelegateInfo.Name = "tcDelegateInfo";
            tcDelegateInfo.RightToLeftLayout = true;
            tcDelegateInfo.SelectedIndex = 0;
            tcDelegateInfo.Size = new Size(1143, 656);
            tcDelegateInfo.TabIndex = 0;
            // 
            // tpPersonInfo
            // 
            tpPersonInfo.BackColor = Color.WhiteSmoke;
            tpPersonInfo.Controls.Add(btnNextDelegateInfo);
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
            // btnNextDelegateInfo
            // 
            btnNextDelegateInfo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnNextDelegateInfo.AutoSize = true;
            btnNextDelegateInfo.BackColor = Color.White;
            btnNextDelegateInfo.FlatStyle = FlatStyle.Popup;
            btnNextDelegateInfo.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnNextDelegateInfo.Image = Properties.Resources.Prev_32;
            btnNextDelegateInfo.ImageAlign = ContentAlignment.MiddleRight;
            btnNextDelegateInfo.Location = new Point(9, 553);
            btnNextDelegateInfo.Margin = new Padding(4);
            btnNextDelegateInfo.Name = "btnNextDelegateInfo";
            btnNextDelegateInfo.Size = new Size(351, 63);
            btnNextDelegateInfo.TabIndex = 1;
            btnNextDelegateInfo.Text = "الانتقال إلى أدخل بينات المندوب";
            btnNextDelegateInfo.TextAlign = ContentAlignment.MiddleLeft;
            btnNextDelegateInfo.UseVisualStyleBackColor = false;
            btnNextDelegateInfo.Click += btnNextDelegateInfo_Click;
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
            // tpDelegateInfo
            // 
            tpDelegateInfo.BackColor = Color.WhiteSmoke;
            tpDelegateInfo.Controls.Add(btnPrevPersonInfo);
            tpDelegateInfo.Controls.Add(pictureBox11);
            tpDelegateInfo.Controls.Add(lblDelegateID);
            tpDelegateInfo.Controls.Add(label1);
            tpDelegateInfo.Location = new Point(4, 28);
            tpDelegateInfo.Margin = new Padding(3, 4, 3, 4);
            tpDelegateInfo.Name = "tpDelegateInfo";
            tpDelegateInfo.Padding = new Padding(3, 4, 3, 4);
            tpDelegateInfo.Size = new Size(1135, 624);
            tpDelegateInfo.TabIndex = 1;
            tpDelegateInfo.Text = "بيانات المندوب";
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
            btnPrevPersonInfo.TabIndex = 113;
            btnPrevPersonInfo.Text = "العودة لأختيار شخص أخر";
            btnPrevPersonInfo.TextAlign = ContentAlignment.MiddleRight;
            btnPrevPersonInfo.UseVisualStyleBackColor = false;
            btnPrevPersonInfo.Click += btnPrevPersonInfo_Click;
            // 
            // pictureBox11
            // 
            pictureBox11.Image = Properties.Resources.Number_32;
            pictureBox11.Location = new Point(775, 85);
            pictureBox11.Margin = new Padding(4);
            pictureBox11.Name = "pictureBox11";
            pictureBox11.Size = new Size(47, 51);
            pictureBox11.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox11.TabIndex = 123;
            pictureBox11.TabStop = false;
            // 
            // lblDelegateID
            // 
            lblDelegateID.AutoSize = true;
            lblDelegateID.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblDelegateID.Location = new Point(716, 98);
            lblDelegateID.Margin = new Padding(4);
            lblDelegateID.Name = "lblDelegateID";
            lblDelegateID.Size = new Size(51, 24);
            lblDelegateID.TabIndex = 122;
            lblDelegateID.Text = "N/A";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.FlatStyle = FlatStyle.Flat;
            label1.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label1.Location = new Point(830, 98);
            label1.Margin = new Padding(4);
            label1.Name = "label1";
            label1.Size = new Size(134, 24);
            label1.TabIndex = 121;
            label1.Text = "رقم المندوب:";
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
            lblTitle.Size = new Size(332, 39);
            lblTitle.TabIndex = 30;
            lblTitle.Text = "إضافة / تعديل مندوب";
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
            btnClose.Location = new Point(405, 715);
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
            btnSave.Location = new Point(212, 715);
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
            btnSaveAndClose.Location = new Point(19, 715);
            btnSaveAndClose.Margin = new Padding(4);
            btnSaveAndClose.Name = "btnSaveAndClose";
            btnSaveAndClose.Size = new Size(185, 61);
            btnSaveAndClose.TabIndex = 31;
            btnSaveAndClose.Text = "إضافة وإغلاق";
            btnSaveAndClose.TextAlign = ContentAlignment.MiddleLeft;
            btnSaveAndClose.UseVisualStyleBackColor = false;
            btnSaveAndClose.Click += btnSaveAndClose_Click;
            // 
            // frmAddUpdateDelegate
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
            Controls.Add(tcDelegateInfo);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(3, 4, 3, 4);
            Name = "frmAddUpdateDelegate";
            Padding = new Padding(4);
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "إضافة / تعديل مندوب";
            Activated += frmAddUpdateDelegate_Activated;
            FormClosing += frmAddUpdateDelegate_FormClosing;
            Load += frmAddUpdateDelegate_Load;
            tcDelegateInfo.ResumeLayout(false);
            tpPersonInfo.ResumeLayout(false);
            tpPersonInfo.PerformLayout();
            tpDelegateInfo.ResumeLayout(false);
            tpDelegateInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox11).EndInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider1).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tcDelegateInfo;
        private System.Windows.Forms.TabPage tpPersonInfo;
        private System.Windows.Forms.TabPage tpDelegateInfo;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private ctrlPersonCardWithFilter ctrlPersonCardWithFilter1;
        private System.Windows.Forms.Button btnNextDelegateInfo;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Button btnPrevPersonInfo;
        private System.Windows.Forms.PictureBox pictureBox11;
        private System.Windows.Forms.Label lblDelegateID;
        private System.Windows.Forms.Label label1;
        private Button btnSaveAndClose;
    }
}