using SalesManagement.People.Controls;

namespace SalesManagement.Delegates.Controls
{
    partial class ctrlDelegateCard
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
            txtDelegateID = new TextBox();
            pictureBox3 = new PictureBox();
            label3 = new Label();
            ctrlPersonCard1 = new ctrlPersonCard();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.BackColor = Color.WhiteSmoke;
            groupBox1.Controls.Add(txtDelegateID);
            groupBox1.Controls.Add(pictureBox3);
            groupBox1.Controls.Add(label3);
            groupBox1.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            groupBox1.Location = new Point(10, 421);
            groupBox1.Margin = new Padding(0);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(4);
            groupBox1.Size = new Size(1115, 110);
            groupBox1.TabIndex = 4;
            groupBox1.TabStop = false;
            groupBox1.Text = "بيانات  المندوب";
            groupBox1.UseWaitCursor = true;
            // 
            // txtDelegateID
            // 
            txtDelegateID.BackColor = Color.WhiteSmoke;
            txtDelegateID.BorderStyle = BorderStyle.None;
            txtDelegateID.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtDelegateID.Location = new Point(708, 50);
            txtDelegateID.Margin = new Padding(4);
            txtDelegateID.Name = "txtDelegateID";
            txtDelegateID.Size = new Size(103, 25);
            txtDelegateID.TabIndex = 90;
            txtDelegateID.Text = "[????]";
            txtDelegateID.UseWaitCursor = true;
            // 
            // pictureBox3
            // 
            pictureBox3.Image = Properties.Resources.Number_32;
            pictureBox3.Location = new Point(819, 37);
            pictureBox3.Margin = new Padding(4);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(47, 51);
            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox3.TabIndex = 86;
            pictureBox3.TabStop = false;
            pictureBox3.UseWaitCursor = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(874, 50);
            label3.Margin = new Padding(4);
            label3.Name = "label3";
            label3.Size = new Size(134, 24);
            label3.TabIndex = 80;
            label3.Text = "رقم المندوب:";
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
            // ctrlDelegateCard
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            Controls.Add(groupBox1);
            Controls.Add(ctrlPersonCard1);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(0);
            Name = "ctrlDelegateCard";
            RightToLeft = RightToLeft.Yes;
            Size = new Size(1135, 541);
            UseWaitCursor = true;
            Load += ctrlDelegateCard_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ResumeLayout(false);

        }

        #endregion

        private GroupBox groupBox1;
        private PictureBox pictureBox3;
        private Label label3;
        private ctrlPersonCard ctrlPersonCard1;
        private TextBox txtDelegateID;
    }
}
