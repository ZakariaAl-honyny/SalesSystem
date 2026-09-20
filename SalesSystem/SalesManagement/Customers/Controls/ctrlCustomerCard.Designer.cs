using SalesManagement.People.Controls;

namespace SalesManagement.Customers.Controls
{
    partial class ctrlCustomerCard
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
            groupBox1 = new GroupBox();
            txtCustomerOtherDetails = new TextBox();
            txtCustomerStartDate = new TextBox();
            txtCustomerID = new TextBox();
            pictureBox1 = new PictureBox();
            pictureBox4 = new PictureBox();
            pictureBox3 = new PictureBox();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            ctrlPersonCard1 = new ctrlPersonCard();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.BackColor = Color.WhiteSmoke;
            groupBox1.Controls.Add(txtCustomerOtherDetails);
            groupBox1.Controls.Add(txtCustomerStartDate);
            groupBox1.Controls.Add(txtCustomerID);
            groupBox1.Controls.Add(pictureBox1);
            groupBox1.Controls.Add(pictureBox4);
            groupBox1.Controls.Add(pictureBox3);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(label3);
            groupBox1.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            groupBox1.Location = new Point(10, 421);
            groupBox1.Margin = new Padding(0);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(4);
            groupBox1.Size = new Size(1115, 135);
            groupBox1.TabIndex = 4;
            groupBox1.TabStop = false;
            groupBox1.Text = "بيانات  العميل";
            groupBox1.UseWaitCursor = true;
            // 
            // txtCustomerOtherDetails
            // 
            txtCustomerOtherDetails.BackColor = Color.WhiteSmoke;
            txtCustomerOtherDetails.BorderStyle = BorderStyle.None;
            txtCustomerOtherDetails.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtCustomerOtherDetails.Location = new Point(-80, 66);
            txtCustomerOtherDetails.Margin = new Padding(4);
            txtCustomerOtherDetails.Name = "txtCustomerOtherDetails";
            txtCustomerOtherDetails.Size = new Size(217, 25);
            txtCustomerOtherDetails.TabIndex = 92;
            txtCustomerOtherDetails.Text = "[????]";
            txtCustomerOtherDetails.UseWaitCursor = true;
            // 
            // txtCustomerStartDate
            // 
            txtCustomerStartDate.BackColor = Color.WhiteSmoke;
            txtCustomerStartDate.BorderStyle = BorderStyle.None;
            txtCustomerStartDate.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtCustomerStartDate.Location = new Point(361, 65);
            txtCustomerStartDate.Margin = new Padding(4);
            txtCustomerStartDate.Name = "txtCustomerStartDate";
            txtCustomerStartDate.Size = new Size(191, 25);
            txtCustomerStartDate.TabIndex = 91;
            txtCustomerStartDate.Text = "[????/???/??]";
            txtCustomerStartDate.UseWaitCursor = true;
            // 
            // txtCustomerID
            // 
            txtCustomerID.BackColor = Color.WhiteSmoke;
            txtCustomerID.BorderStyle = BorderStyle.None;
            txtCustomerID.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtCustomerID.Location = new Point(775, 64);
            txtCustomerID.Margin = new Padding(4);
            txtCustomerID.Name = "txtCustomerID";
            txtCustomerID.Size = new Size(137, 25);
            txtCustomerID.TabIndex = 90;
            txtCustomerID.Text = "[????]";
            txtCustomerID.UseWaitCursor = true;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.Question_32;
            pictureBox1.Location = new Point(145, 52);
            pictureBox1.Margin = new Padding(4);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(47, 51);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 89;
            pictureBox1.TabStop = false;
            pictureBox1.UseWaitCursor = true;
            // 
            // pictureBox4
            // 
            pictureBox4.Image = Properties.Resources.User_32__2;
            pictureBox4.Location = new Point(560, 51);
            pictureBox4.Margin = new Padding(4);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new Size(47, 51);
            pictureBox4.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox4.TabIndex = 87;
            pictureBox4.TabStop = false;
            pictureBox4.UseWaitCursor = true;
            // 
            // pictureBox3
            // 
            pictureBox3.Image = Properties.Resources.Number_32;
            pictureBox3.Location = new Point(920, 51);
            pictureBox3.Margin = new Padding(4);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(47, 51);
            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox3.TabIndex = 86;
            pictureBox3.TabStop = false;
            pictureBox3.UseWaitCursor = true;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(200, 66);
            label5.Margin = new Padding(4);
            label5.Name = "label5";
            label5.Size = new Size(153, 24);
            label5.TabIndex = 82;
            label5.Text = "تفاصيل العميل:";
            label5.UseWaitCursor = true;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(615, 65);
            label4.Margin = new Padding(4);
            label4.Name = "label4";
            label4.Size = new Size(146, 24);
            label4.TabIndex = 81;
            label4.Text = "تاريخ الحساب:";
            label4.UseWaitCursor = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(975, 65);
            label3.Margin = new Padding(4);
            label3.Name = "label3";
            label3.Size = new Size(120, 24);
            label3.TabIndex = 80;
            label3.Text = "رقم العميل:";
            label3.UseWaitCursor = true;
            // 
            // ctrlPersonCard1
            // 
            ctrlPersonCard1.BackColor = Color.WhiteSmoke;
            ctrlPersonCard1.Font = new Font("Tahoma", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ctrlPersonCard1.Location = new Point(1, 5);
            ctrlPersonCard1.Margin = new Padding(0);
            ctrlPersonCard1.Name = "ctrlPersonCard1";
            ctrlPersonCard1.Padding = new Padding(8, 7, 8, 7);
            ctrlPersonCard1.RightToLeft = RightToLeft.Yes;
            ctrlPersonCard1.Size = new Size(1128, 416);
            ctrlPersonCard1.TabIndex = 3;
            ctrlPersonCard1.UseWaitCursor = true;
            // 
            // ctrlCustomerCard
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            Controls.Add(groupBox1);
            Controls.Add(ctrlPersonCard1);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(0);
            Name = "ctrlCustomerCard";
            RightToLeft = RightToLeft.Yes;
            Size = new Size(1135, 566);
            UseWaitCursor = true;
            Load += ctrlCustomerCard_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ResumeLayout(false);

        }

        #endregion

        private GroupBox groupBox1;
        private PictureBox pictureBox1;
        private PictureBox pictureBox4;
        private PictureBox pictureBox3;
        private Label label5;
        private Label label4;
        private Label label3;
        private ctrlPersonCard ctrlPersonCard1;
        private TextBox txtCustomerStartDate;
        private TextBox txtCustomerID;
        private TextBox txtCustomerOtherDetails;
    }
}
