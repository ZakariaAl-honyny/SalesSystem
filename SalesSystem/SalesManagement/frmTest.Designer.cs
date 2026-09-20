namespace SalesManagement
{
    partial class frmTest
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
            ctrlEmployeeCardWithFilter1 = new SalesManagement.Employees.Controls.ctrlEmployeeCardWithFilter();
            SuspendLayout();
            // 
            // ctrlEmployeeCardWithFilter1
            // 
            ctrlEmployeeCardWithFilter1.BackColor = Color.WhiteSmoke;
            ctrlEmployeeCardWithFilter1.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ctrlEmployeeCardWithFilter1.Location = new Point(-1, 50);
            ctrlEmployeeCardWithFilter1.Margin = new Padding(0);
            ctrlEmployeeCardWithFilter1.Name = "ctrlEmployeeCardWithFilter1";
            ctrlEmployeeCardWithFilter1.RightToLeft = RightToLeft.Yes;
            ctrlEmployeeCardWithFilter1.Size = new Size(1200, 700);
            ctrlEmployeeCardWithFilter1.TabIndex = 0;
            // 
            // frmTest
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1199, 768);
            Controls.Add(ctrlEmployeeCardWithFilter1);
            Name = "frmTest";
            Text = "frmTest";
            ResumeLayout(false);
        }

        #endregion

        private Employees.Controls.ctrlEmployeeCardWithFilter ctrlEmployeeCardWithFilter1;
    }
}