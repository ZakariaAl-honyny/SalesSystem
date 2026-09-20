using SalesSystem.Desktop.People.Controls;

namespace SalesSystem.Desktop.People.Forms
{
    partial class frmShowPersonInfo
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
            components?.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblMode = new Label();
            btnClose = new Button();
            ctrlPersonCard1 = new ctrlPersonCard();
            SuspendLayout();
            // 
            // lblMode
            // 
            lblMode.AutoSize = true;
            lblMode.BackColor = Color.WhiteSmoke;
            lblMode.FlatStyle = FlatStyle.Flat;
            lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblMode.ForeColor = Color.Firebrick;
            lblMode.Location = new Point(395, 8);
            lblMode.Margin = new Padding(4);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(358, 39);
            lblMode.TabIndex = 33;
            lblMode.Text = "تفاصيل بيانات الشخص";
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnClose.BackColor = Color.White;
            btnClose.DialogResult = DialogResult.Cancel;
            btnClose.FlatStyle = FlatStyle.Popup;
            btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnClose.ImageAlign = ContentAlignment.MiddleRight;
            btnClose.Location = new Point(18, 476);
            btnClose.Margin = new Padding(4);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(181, 61);
            btnClose.TabIndex = 35;
            btnClose.Text = "إغلاق";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // ctrlPersonCard1
            // 
            ctrlPersonCard1.BackColor = Color.WhiteSmoke;
            ctrlPersonCard1.Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ctrlPersonCard1.Location = new Point(8, 55);
            ctrlPersonCard1.Margin = new Padding(4);
            ctrlPersonCard1.Name = "ctrlPersonCard1";
            ctrlPersonCard1.Padding = new Padding(8, 7, 8, 7);
            ctrlPersonCard1.RightToLeft = RightToLeft.Yes;
            ctrlPersonCard1.Size = new Size(1133, 414);
            ctrlPersonCard1.TabIndex = 34;
            // 
            // frmShowPersonInfo
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            CancelButton = btnClose;
            ClientSize = new Size(1149, 545);
            Controls.Add(btnClose);
            Controls.Add(ctrlPersonCard1);
            Controls.Add(lblMode);
            Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(4, 5, 4, 5);
            MaximizeBox = false;
            Name = "frmShowPersonInfo";
            Padding = new Padding(4);
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "تفاصيل بيانات الشخص";
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private ctrlPersonCard ctrlPersonCard1;
        private System.Windows.Forms.Label lblMode;
    }
}
