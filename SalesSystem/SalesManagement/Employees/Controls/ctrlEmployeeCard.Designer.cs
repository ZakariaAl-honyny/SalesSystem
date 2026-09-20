using SalesManagement.People.Controls;

namespace SalesManagement.Employees.Controls
{
    partial class ctrlEmployeeCard
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
            txtEmployeeState = new TextBox();
            txtEmployeeQualification = new TextBox();
            txtEmployeeID = new TextBox();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            ctrlPersonCard1 = new ctrlPersonCard();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.BackColor = Color.WhiteSmoke;
            groupBox1.Controls.Add(txtEmployeeState);
            groupBox1.Controls.Add(txtEmployeeQualification);
            groupBox1.Controls.Add(txtEmployeeID);
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
            groupBox1.Text = "بيانات  الموظف";
            // 
            // txtEmployeeState
            // 
            txtEmployeeState.BackColor = Color.WhiteSmoke;
            txtEmployeeState.BorderStyle = BorderStyle.None;
            txtEmployeeState.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtEmployeeState.Location = new Point(-37, 65);
            txtEmployeeState.Margin = new Padding(4);
            txtEmployeeState.Name = "txtEmployeeState";
            txtEmployeeState.Size = new Size(217, 25);
            txtEmployeeState.TabIndex = 92;
            txtEmployeeState.Text = "[????]";
            // 
            // txtEmployeeQualification
            // 
            txtEmployeeQualification.BackColor = Color.WhiteSmoke;
            txtEmployeeQualification.BorderStyle = BorderStyle.None;
            txtEmployeeQualification.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtEmployeeQualification.Location = new Point(390, 65);
            txtEmployeeQualification.Margin = new Padding(4);
            txtEmployeeQualification.Name = "txtEmployeeQualification";
            txtEmployeeQualification.Size = new Size(217, 25);
            txtEmployeeQualification.TabIndex = 91;
            txtEmployeeQualification.Text = "[????]";
            // 
            // txtEmployeeID
            // 
            txtEmployeeID.BackColor = Color.WhiteSmoke;
            txtEmployeeID.BorderStyle = BorderStyle.None;
            txtEmployeeID.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtEmployeeID.Location = new Point(830, 64);
            txtEmployeeID.Margin = new Padding(4);
            txtEmployeeID.Name = "txtEmployeeID";
            txtEmployeeID.Size = new Size(137, 25);
            txtEmployeeID.TabIndex = 90;
            txtEmployeeID.Text = "[????]";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(188, 65);
            label5.Margin = new Padding(4);
            label5.Name = "label5";
            label5.Size = new Size(139, 24);
            label5.TabIndex = 82;
            label5.Text = "حالة الموظف:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(615, 65);
            label4.Margin = new Padding(4);
            label4.Name = "label4";
            label4.Size = new Size(152, 24);
            label4.TabIndex = 81;
            label4.Text = "مؤهل الموظف:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(975, 65);
            label3.Margin = new Padding(4);
            label3.Name = "label3";
            label3.Size = new Size(132, 24);
            label3.TabIndex = 80;
            label3.Text = "رقم الموظف:";
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
            ctrlPersonCard1.Size = new Size(1128, 440);
            ctrlPersonCard1.TabIndex = 3;
            // 
            // ctrlEmployeeCard
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            Controls.Add(groupBox1);
            Controls.Add(ctrlPersonCard1);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(0);
            Name = "ctrlEmployeeCard";
            RightToLeft = RightToLeft.Yes;
            Size = new Size(1135, 566);
            Load += ctrlEmployeeCard_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private GroupBox groupBox1;
        private Label label5;
        private Label label4;
        private Label label3;
        private ctrlPersonCard ctrlPersonCard1;
        private TextBox txtEmployeeQualification;
        private TextBox txtEmployeeID;
        private TextBox txtEmployeeState;
    }
}
