using SalesManagement.People.Controls;

namespace SalesManagement.Customers
{
    partial class frmAddUpdateCustomer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAddUpdateCustomer));
            tcCustomerInfo = new TabControl();
            tpPersonInfo = new TabPage();
            btnNextCustomerInfo = new Button();
            ctrlPersonCardWithFilter1 = new ctrlPersonCardWithFilter();
            btnNext = new Button();
            tpCustomerInfo = new TabPage();
            dtpStartDate = new DateTimePicker();
            txtOtherDetails = new TextBox();
            label5 = new Label();
            pictureBox3 = new PictureBox();
            label3 = new Label();
            pictureBox2 = new PictureBox();
            btnPrevPersonInfo = new Button();
            pictureBox11 = new PictureBox();
            lblCustomerID = new Label();
            label1 = new Label();
            lblTitle = new Label();
            btnClose = new Button();
            btnSave = new Button();
            errorProvider1 = new ErrorProvider(components);
            btnSaveAndClose = new Button();
            tcCustomerInfo.SuspendLayout();
            tpPersonInfo.SuspendLayout();
            tpCustomerInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox11).BeginInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider1).BeginInit();
            SuspendLayout();
            // 
            // tcCustomerInfo
            // 
            tcCustomerInfo.Anchor = AnchorStyles.Top;
            tcCustomerInfo.Controls.Add(tpPersonInfo);
            tcCustomerInfo.Controls.Add(tpCustomerInfo);
            tcCustomerInfo.Font = new Font("Tahoma", 8F);
            tcCustomerInfo.Location = new Point(8, 55);
            tcCustomerInfo.Margin = new Padding(4);
            tcCustomerInfo.Name = "tcCustomerInfo";
            tcCustomerInfo.RightToLeftLayout = true;
            tcCustomerInfo.SelectedIndex = 0;
            tcCustomerInfo.Size = new Size(1143, 656);
            tcCustomerInfo.TabIndex = 0;
            // 
            // tpPersonInfo
            // 
            tpPersonInfo.BackColor = Color.WhiteSmoke;
            tpPersonInfo.Controls.Add(btnNextCustomerInfo);
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
            // btnNextCustomerInfo
            // 
            btnNextCustomerInfo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnNextCustomerInfo.AutoSize = true;
            btnNextCustomerInfo.BackColor = Color.White;
            btnNextCustomerInfo.FlatStyle = FlatStyle.Popup;
            btnNextCustomerInfo.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnNextCustomerInfo.Image = Properties.Resources.Prev_32;
            btnNextCustomerInfo.ImageAlign = ContentAlignment.MiddleRight;
            btnNextCustomerInfo.Location = new Point(9, 553);
            btnNextCustomerInfo.Margin = new Padding(4);
            btnNextCustomerInfo.Name = "btnNextCustomerInfo";
            btnNextCustomerInfo.Size = new Size(351, 63);
            btnNextCustomerInfo.TabIndex = 1;
            btnNextCustomerInfo.Text = "الانتقال إلى أدخل بينات الموظف";
            btnNextCustomerInfo.TextAlign = ContentAlignment.MiddleLeft;
            btnNextCustomerInfo.UseVisualStyleBackColor = false;
            btnNextCustomerInfo.Click += btnNextCustomerInfo_Click;
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
            ctrlPersonCardWithFilter1.Load += ctrlPersonCardWithFilter1_Load;
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
            // tpCustomerInfo
            // 
            tpCustomerInfo.BackColor = Color.WhiteSmoke;
            tpCustomerInfo.Controls.Add(dtpStartDate);
            tpCustomerInfo.Controls.Add(txtOtherDetails);
            tpCustomerInfo.Controls.Add(label5);
            tpCustomerInfo.Controls.Add(pictureBox3);
            tpCustomerInfo.Controls.Add(label3);
            tpCustomerInfo.Controls.Add(pictureBox2);
            tpCustomerInfo.Controls.Add(btnPrevPersonInfo);
            tpCustomerInfo.Controls.Add(pictureBox11);
            tpCustomerInfo.Controls.Add(lblCustomerID);
            tpCustomerInfo.Controls.Add(label1);
            tpCustomerInfo.Location = new Point(4, 28);
            tpCustomerInfo.Margin = new Padding(3, 4, 3, 4);
            tpCustomerInfo.Name = "tpCustomerInfo";
            tpCustomerInfo.Padding = new Padding(3, 4, 3, 4);
            tpCustomerInfo.Size = new Size(1135, 624);
            tpCustomerInfo.TabIndex = 1;
            tpCustomerInfo.Text = "بيانات عميل";
            // 
            // dtpStartDate
            // 
            dtpStartDate.CustomFormat = "dd/MM/yyyy";
            dtpStartDate.Font = new Font("Tahoma", 8F, FontStyle.Bold);
            dtpStartDate.Format = DateTimePickerFormat.Custom;
            dtpStartDate.Location = new Point(356, 134);
            dtpStartDate.Margin = new Padding(4);
            dtpStartDate.Name = "dtpStartDate";
            dtpStartDate.Size = new Size(500, 27);
            dtpStartDate.TabIndex = 142;
            // 
            // txtOtherDetails
            // 
            txtOtherDetails.Font = new Font("Tahoma", 10F);
            txtOtherDetails.Location = new Point(356, 181);
            txtOtherDetails.Margin = new Padding(4);
            txtOtherDetails.Multiline = true;
            txtOtherDetails.Name = "txtOtherDetails";
            txtOtherDetails.Size = new Size(500, 150);
            txtOtherDetails.TabIndex = 139;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label5.Location = new Point(919, 194);
            label5.Margin = new Padding(4);
            label5.Name = "label5";
            label5.Size = new Size(153, 24);
            label5.TabIndex = 140;
            label5.Text = "تفاصيل العميل:";
            // 
            // pictureBox3
            // 
            pictureBox3.Image = Properties.Resources.List_32;
            pictureBox3.Location = new Point(864, 181);
            pictureBox3.Margin = new Padding(4);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(47, 51);
            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox3.TabIndex = 141;
            pictureBox3.TabStop = false;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label3.Location = new Point(919, 136);
            label3.Margin = new Padding(4);
            label3.Name = "label3";
            label3.Size = new Size(110, 24);
            label3.TabIndex = 137;
            label3.Text = "تاريخ البدء:";
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Properties.Resources.Calendar_32;
            pictureBox2.Location = new Point(864, 123);
            pictureBox2.Margin = new Padding(4);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(47, 51);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 138;
            pictureBox2.TabStop = false;
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
            pictureBox11.Location = new Point(864, 65);
            pictureBox11.Margin = new Padding(4);
            pictureBox11.Name = "pictureBox11";
            pictureBox11.Size = new Size(47, 51);
            pictureBox11.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox11.TabIndex = 123;
            pictureBox11.TabStop = false;
            // 
            // lblCustomerID
            // 
            lblCustomerID.AutoSize = true;
            lblCustomerID.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblCustomerID.Location = new Point(805, 78);
            lblCustomerID.Margin = new Padding(4);
            lblCustomerID.Name = "lblCustomerID";
            lblCustomerID.Size = new Size(51, 24);
            lblCustomerID.TabIndex = 122;
            lblCustomerID.Text = "N/A";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.FlatStyle = FlatStyle.Flat;
            label1.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label1.Location = new Point(919, 78);
            label1.Margin = new Padding(4);
            label1.Name = "label1";
            label1.Size = new Size(120, 24);
            label1.TabIndex = 121;
            label1.Text = "رقم العميل:";
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
            lblTitle.Size = new Size(317, 39);
            lblTitle.TabIndex = 30;
            lblTitle.Text = "إضافة / تعديل عميل";
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
            btnClose.Location = new Point(398, 716);
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
            btnSave.Location = new Point(205, 716);
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
            btnSaveAndClose.Location = new Point(12, 715);
            btnSaveAndClose.Margin = new Padding(4);
            btnSaveAndClose.Name = "btnSaveAndClose";
            btnSaveAndClose.Size = new Size(185, 61);
            btnSaveAndClose.TabIndex = 5;
            btnSaveAndClose.Text = "إضافة وإغلاق";
            btnSaveAndClose.TextAlign = ContentAlignment.MiddleLeft;
            btnSaveAndClose.UseVisualStyleBackColor = false;
            btnSaveAndClose.Click += btnSaveAndClose_Click;
            // 
            // frmAddUpdateCustomer
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
            Controls.Add(tcCustomerInfo);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(3, 4, 3, 4);
            Name = "frmAddUpdateCustomer";
            Padding = new Padding(4);
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "إضافة / تعديل عميل";
            Activated += frmAddUpdateCustomer_Activated;
            FormClosing += frmAddUpdateCustomer_FormClosing;
            Load += frmAddUpdateCustomer_Load;
            tcCustomerInfo.ResumeLayout(false);
            tpPersonInfo.ResumeLayout(false);
            tpPersonInfo.PerformLayout();
            tpCustomerInfo.ResumeLayout(false);
            tpCustomerInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox11).EndInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider1).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tcCustomerInfo;
        private System.Windows.Forms.TabPage tpPersonInfo;
        private System.Windows.Forms.TabPage tpCustomerInfo;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private ctrlPersonCardWithFilter ctrlPersonCardWithFilter1;
        private System.Windows.Forms.Button btnNextCustomerInfo;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Button btnPrevPersonInfo;
        private System.Windows.Forms.PictureBox pictureBox11;
        private System.Windows.Forms.Label lblCustomerID;
        private System.Windows.Forms.Label label1;
        private TextBox txtOtherDetails;
        private Label label5;
        private PictureBox pictureBox3;
        private Label label3;
        private PictureBox pictureBox2;
        private DateTimePicker dtpStartDate;
        private Button btnSaveAndClose;
    }
}