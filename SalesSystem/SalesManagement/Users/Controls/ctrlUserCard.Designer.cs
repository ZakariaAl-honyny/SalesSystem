using SalesManagement.People.Controls;

namespace SalesManagement.Users.Controls
{
    partial class ctrlUserCard
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ctrlEmployeeCard1 = new SalesManagement.Employees.Controls.ctrlEmployeeCard();
            groupBox2 = new GroupBox();
            txtIsActive = new TextBox();
            txtUserName = new TextBox();
            txtUserID = new TextBox();
            pictureBox1 = new PictureBox();
            pictureBox2 = new PictureBox();
            pictureBox5 = new PictureBox();
            label1 = new Label();
            label2 = new Label();
            label6 = new Label();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).BeginInit();
            SuspendLayout();
            // 
            // ctrlEmployeeCard1
            // 
            ctrlEmployeeCard1.BackColor = Color.WhiteSmoke;
            ctrlEmployeeCard1.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ctrlEmployeeCard1.Location = new Point(0, 0);
            ctrlEmployeeCard1.Margin = new Padding(0);
            ctrlEmployeeCard1.Name = "ctrlEmployeeCard1";
            ctrlEmployeeCard1.RightToLeft = RightToLeft.Yes;
            ctrlEmployeeCard1.Size = new Size(1130, 559);
            ctrlEmployeeCard1.TabIndex = 3;
            ctrlEmployeeCard1.UseWaitCursor = true;
            // 
            // groupBox2
            // 
            groupBox2.BackColor = Color.WhiteSmoke;
            groupBox2.Controls.Add(txtIsActive);
            groupBox2.Controls.Add(txtUserName);
            groupBox2.Controls.Add(txtUserID);
            groupBox2.Controls.Add(pictureBox1);
            groupBox2.Controls.Add(pictureBox2);
            groupBox2.Controls.Add(pictureBox5);
            groupBox2.Controls.Add(label1);
            groupBox2.Controls.Add(label2);
            groupBox2.Controls.Add(label6);
            groupBox2.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            groupBox2.Location = new Point(10, 559);
            groupBox2.Margin = new Padding(0);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(4);
            groupBox2.Size = new Size(1115, 138);
            groupBox2.TabIndex = 5;
            groupBox2.TabStop = false;
            groupBox2.Text = "بيانات  المستخدم";
            groupBox2.UseWaitCursor = true;
            // 
            // txtIsActive
            // 
            txtIsActive.BackColor = Color.WhiteSmoke;
            txtIsActive.BorderStyle = BorderStyle.None;
            txtIsActive.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtIsActive.Location = new Point(0, 68);
            txtIsActive.Margin = new Padding(4);
            txtIsActive.Name = "txtIsActive";
            txtIsActive.Size = new Size(118, 25);
            txtIsActive.TabIndex = 94;
            txtIsActive.Text = "[????]";
            txtIsActive.UseWaitCursor = true;
            // 
            // txtUserName
            // 
            txtUserName.BackColor = Color.WhiteSmoke;
            txtUserName.BorderStyle = BorderStyle.None;
            txtUserName.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtUserName.Location = new Point(352, 68);
            txtUserName.Margin = new Padding(4);
            txtUserName.Name = "txtUserName";
            txtUserName.Size = new Size(186, 25);
            txtUserName.TabIndex = 93;
            txtUserName.Text = "[????]";
            txtUserName.UseWaitCursor = true;
            // 
            // txtUserID
            // 
            txtUserID.BackColor = Color.WhiteSmoke;
            txtUserID.BorderStyle = BorderStyle.None;
            txtUserID.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtUserID.Location = new Point(773, 67);
            txtUserID.Margin = new Padding(4);
            txtUserID.Name = "txtUserID";
            txtUserID.Size = new Size(115, 25);
            txtUserID.TabIndex = 92;
            txtUserID.Text = "[????]";
            txtUserID.UseWaitCursor = true;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.Question_32;
            pictureBox1.Location = new Point(126, 55);
            pictureBox1.Margin = new Padding(4);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(47, 51);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 89;
            pictureBox1.TabStop = false;
            pictureBox1.UseWaitCursor = true;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Properties.Resources.User_32__2;
            pictureBox2.Location = new Point(546, 55);
            pictureBox2.Margin = new Padding(4);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(47, 51);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 87;
            pictureBox2.TabStop = false;
            pictureBox2.UseWaitCursor = true;
            // 
            // pictureBox5
            // 
            pictureBox5.Image = Properties.Resources.Number_32;
            pictureBox5.Location = new Point(896, 54);
            pictureBox5.Margin = new Padding(4);
            pictureBox5.Name = "pictureBox5";
            pictureBox5.Size = new Size(47, 51);
            pictureBox5.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox5.TabIndex = 86;
            pictureBox5.TabStop = false;
            pictureBox5.UseWaitCursor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(181, 68);
            label1.Margin = new Padding(4);
            label1.Name = "label1";
            label1.Size = new Size(142, 24);
            label1.TabIndex = 82;
            label1.Text = "حالة الحساب:";
            label1.UseWaitCursor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(601, 68);
            label2.Margin = new Padding(4);
            label2.Name = "label2";
            label2.Size = new Size(164, 24);
            label2.TabIndex = 81;
            label2.Text = "أسم المستخدم:";
            label2.UseWaitCursor = true;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.Location = new Point(951, 67);
            label6.Margin = new Padding(4);
            label6.Name = "label6";
            label6.Size = new Size(156, 24);
            label6.TabIndex = 80;
            label6.Text = "رقم المستخدم:";
            label6.UseWaitCursor = true;
            // 
            // ctrlUserCard
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            Controls.Add(groupBox2);
            Controls.Add(ctrlEmployeeCard1);
            Margin = new Padding(0);
            Name = "ctrlUserCard";
            RightToLeft = RightToLeft.Yes;
            Size = new Size(1132, 704);
            Load += ctrlUserCard_Load;
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).EndInit();
            ResumeLayout(false);

        }

        #endregion
        private Employees.Controls.ctrlEmployeeCard ctrlEmployeeCard1;
        private GroupBox groupBox2;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private PictureBox pictureBox5;
        private Label label1;
        private Label label2;
        private Label label6;
        private TextBox txtIsActive;
        private TextBox txtUserName;
        private TextBox txtUserID;
    }
}
