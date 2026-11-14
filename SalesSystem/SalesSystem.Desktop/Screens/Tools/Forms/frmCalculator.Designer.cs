namespace SalesSystem.Desktop.Screens.Tools.Forms
{
    partial class frmCalculator
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            components?.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            lblTitle = new Label();
            txtDisplay = new TextBox();
            tblButtons = new TableLayoutPanel();
            btnClear = new Button();
            btnBackspace = new Button();
            btnDivide = new Button();
            btnMultiply = new Button();
            btn7 = new Button();
            btn8 = new Button();
            btn9 = new Button();
            btnSubtract = new Button();
            btn4 = new Button();
            btn5 = new Button();
            btn6 = new Button();
            btnAdd = new Button();
            btn1 = new Button();
            btn2 = new Button();
            btn3 = new Button();
            btnEquals = new Button();
            btn0 = new Button();
            btnDot = new Button();
            btnClose = new Button();
            tblButtons.SuspendLayout();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.BackColor = Color.FromArgb(14, 165, 103);
            lblTitle.Dock = DockStyle.Top;
            lblTitle.Font = new Font("Tahoma", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(0, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(320, 44);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "آلة حاسبة";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtDisplay
            // 
            txtDisplay.BackColor = Color.White;
            txtDisplay.Dock = DockStyle.Top;
            txtDisplay.Font = new Font("Tahoma", 22F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtDisplay.ForeColor = Color.FromArgb(33, 37, 41);
            txtDisplay.Location = new Point(0, 44);
            txtDisplay.Name = "txtDisplay";
            txtDisplay.ReadOnly = true;
            txtDisplay.RightToLeft = RightToLeft.No;
            txtDisplay.Size = new Size(320, 62);
            txtDisplay.TabIndex = 1;
            txtDisplay.TabStop = false;
            txtDisplay.Text = "0";
            txtDisplay.TextAlign = HorizontalAlignment.Right;
            // 
            // tblButtons
            // 
            tblButtons.BackColor = Color.FromArgb(235, 240, 235);
            tblButtons.ColumnCount = 4;
            tblButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tblButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tblButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tblButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tblButtons.Dock = DockStyle.Fill;
            tblButtons.Location = new Point(0, 106);
            tblButtons.Name = "tblButtons";
            tblButtons.Padding = new Padding(8);
            tblButtons.RightToLeft = RightToLeft.No;
            tblButtons.RowCount = 5;
            tblButtons.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tblButtons.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tblButtons.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tblButtons.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tblButtons.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tblButtons.Size = new Size(320, 304);
            tblButtons.TabIndex = 2;
            // 
            // btnClear
            // 
            btnClear.BackColor = Color.FromArgb(108, 117, 125);
            btnClear.Dock = DockStyle.Fill;
            btnClear.FlatAppearance.BorderColor = Color.FromArgb(205, 225, 205);
            btnClear.FlatAppearance.BorderSize = 1;
            btnClear.FlatStyle = FlatStyle.Flat;
            btnClear.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnClear.ForeColor = Color.White;
            btnClear.Margin = new Padding(4);
            btnClear.Name = "btnClear";
            btnClear.TabIndex = 10;
            btnClear.Text = "مسح";
            btnClear.UseVisualStyleBackColor = false;
            btnClear.Click += Clear_Click;
            // 
            // btnBackspace
            // 
            btnBackspace.BackColor = Color.FromArgb(235, 245, 235);
            btnBackspace.Dock = DockStyle.Fill;
            btnBackspace.FlatAppearance.BorderColor = Color.FromArgb(205, 225, 205);
            btnBackspace.FlatAppearance.BorderSize = 1;
            btnBackspace.FlatStyle = FlatStyle.Flat;
            btnBackspace.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnBackspace.ForeColor = Color.FromArgb(33, 37, 41);
            btnBackspace.Margin = new Padding(4);
            btnBackspace.Name = "btnBackspace";
            btnBackspace.TabIndex = 11;
            btnBackspace.Text = "⌫";
            btnBackspace.UseVisualStyleBackColor = false;
            btnBackspace.Click += Backspace_Click;
            // 
            // btnDivide
            // 
            btnDivide.BackColor = Color.FromArgb(23, 162, 184);
            btnDivide.Dock = DockStyle.Fill;
            btnDivide.FlatAppearance.BorderColor = Color.FromArgb(205, 225, 205);
            btnDivide.FlatAppearance.BorderSize = 1;
            btnDivide.FlatStyle = FlatStyle.Flat;
            btnDivide.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDivide.ForeColor = Color.White;
            btnDivide.Margin = new Padding(4);
            btnDivide.Name = "btnDivide";
            btnDivide.TabIndex = 12;
            btnDivide.Text = "÷";
            btnDivide.UseVisualStyleBackColor = false;
            btnDivide.Click += Operator_Click;
            // 
            // btnMultiply
            // 
            btnMultiply.BackColor = Color.FromArgb(23, 162, 184);
            btnMultiply.Dock = DockStyle.Fill;
            btnMultiply.FlatAppearance.BorderColor = Color.FromArgb(205, 225, 205);
            btnMultiply.FlatAppearance.BorderSize = 1;
            btnMultiply.FlatStyle = FlatStyle.Flat;
            btnMultiply.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnMultiply.ForeColor = Color.White;
            btnMultiply.Margin = new Padding(4);
            btnMultiply.Name = "btnMultiply";
            btnMultiply.TabIndex = 13;
            btnMultiply.Text = "×";
            btnMultiply.UseVisualStyleBackColor = false;
            btnMultiply.Click += Operator_Click;
            // 
            // btn7
            // 
            btn7.BackColor = Color.White;
            btn7.Dock = DockStyle.Fill;
            btn7.FlatAppearance.BorderColor = Color.FromArgb(205, 225, 205);
            btn7.FlatAppearance.BorderSize = 1;
            btn7.FlatStyle = FlatStyle.Flat;
            btn7.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn7.ForeColor = Color.FromArgb(33, 37, 41);
            btn7.Margin = new Padding(4);
            btn7.Name = "btn7";
            btn7.TabIndex = 14;
            btn7.Text = "7";
            btn7.UseVisualStyleBackColor = false;
            btn7.Click += Digit_Click;
            // 
            // btn8
            // 
            btn8.BackColor = Color.White;
            btn8.Dock = DockStyle.Fill;
            btn8.FlatAppearance.BorderColor = Color.FromArgb(205, 225, 205);
            btn8.FlatAppearance.BorderSize = 1;
            btn8.FlatStyle = FlatStyle.Flat;
            btn8.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn8.ForeColor = Color.FromArgb(33, 37, 41);
            btn8.Margin = new Padding(4);
            btn8.Name = "btn8";
            btn8.TabIndex = 15;
            btn8.Text = "8";
            btn8.UseVisualStyleBackColor = false;
            btn8.Click += Digit_Click;
            // 
            // btn9
            // 
            btn9.BackColor = Color.White;
            btn9.Dock = DockStyle.Fill;
            btn9.FlatAppearance.BorderColor = Color.FromArgb(205, 225, 205);
            btn9.FlatAppearance.BorderSize = 1;
            btn9.FlatStyle = FlatStyle.Flat;
            btn9.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn9.ForeColor = Color.FromArgb(33, 37, 41);
            btn9.Margin = new Padding(4);
            btn9.Name = "btn9";
            btn9.TabIndex = 16;
            btn9.Text = "9";
            btn9.UseVisualStyleBackColor = false;
            btn9.Click += Digit_Click;
            // 
            // btnSubtract
            // 
            btnSubtract.BackColor = Color.FromArgb(23, 162, 184);
            btnSubtract.Dock = DockStyle.Fill;
            btnSubtract.FlatAppearance.BorderColor = Color.FromArgb(205, 225, 205);
            btnSubtract.FlatAppearance.BorderSize = 1;
            btnSubtract.FlatStyle = FlatStyle.Flat;
            btnSubtract.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSubtract.ForeColor = Color.White;
            btnSubtract.Margin = new Padding(4);
            btnSubtract.Name = "btnSubtract";
            btnSubtract.TabIndex = 17;
            btnSubtract.Text = "−";
            btnSubtract.UseVisualStyleBackColor = false;
            btnSubtract.Click += Operator_Click;
            // 
            // btn4
            // 
            btn4.BackColor = Color.White;
            btn4.Dock = DockStyle.Fill;
            btn4.FlatAppearance.BorderColor = Color.FromArgb(205, 225, 205);
            btn4.FlatAppearance.BorderSize = 1;
            btn4.FlatStyle = FlatStyle.Flat;
            btn4.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn4.ForeColor = Color.FromArgb(33, 37, 41);
            btn4.Margin = new Padding(4);
            btn4.Name = "btn4";
            btn4.TabIndex = 18;
            btn4.Text = "4";
            btn4.UseVisualStyleBackColor = false;
            btn4.Click += Digit_Click;
            // 
            // btn5
            // 
            btn5.BackColor = Color.White;
            btn5.Dock = DockStyle.Fill;
            btn5.FlatAppearance.BorderColor = Color.FromArgb(205, 225, 205);
            btn5.FlatAppearance.BorderSize = 1;
            btn5.FlatStyle = FlatStyle.Flat;
            btn5.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn5.ForeColor = Color.FromArgb(33, 37, 41);
            btn5.Margin = new Padding(4);
            btn5.Name = "btn5";
            btn5.TabIndex = 19;
            btn5.Text = "5";
            btn5.UseVisualStyleBackColor = false;
            btn5.Click += Digit_Click;
            // 
            // btn6
            // 
            btn6.BackColor = Color.White;
            btn6.Dock = DockStyle.Fill;
            btn6.FlatAppearance.BorderColor = Color.FromArgb(205, 225, 205);
            btn6.FlatAppearance.BorderSize = 1;
            btn6.FlatStyle = FlatStyle.Flat;
            btn6.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn6.ForeColor = Color.FromArgb(33, 37, 41);
            btn6.Margin = new Padding(4);
            btn6.Name = "btn6";
            btn6.TabIndex = 20;
            btn6.Text = "6";
            btn6.UseVisualStyleBackColor = false;
            btn6.Click += Digit_Click;
            // 
            // btnAdd
            // 
            btnAdd.BackColor = Color.FromArgb(23, 162, 184);
            btnAdd.Dock = DockStyle.Fill;
            btnAdd.FlatAppearance.BorderColor = Color.FromArgb(205, 225, 205);
            btnAdd.FlatAppearance.BorderSize = 1;
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAdd.ForeColor = Color.White;
            btnAdd.Margin = new Padding(4);
            btnAdd.Name = "btnAdd";
            btnAdd.TabIndex = 21;
            btnAdd.Text = "+";
            btnAdd.UseVisualStyleBackColor = false;
            btnAdd.Click += Operator_Click;
            // 
            // btn1
            // 
            btn1.BackColor = Color.White;
            btn1.Dock = DockStyle.Fill;
            btn1.FlatAppearance.BorderColor = Color.FromArgb(205, 225, 205);
            btn1.FlatAppearance.BorderSize = 1;
            btn1.FlatStyle = FlatStyle.Flat;
            btn1.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn1.ForeColor = Color.FromArgb(33, 37, 41);
            btn1.Margin = new Padding(4);
            btn1.Name = "btn1";
            btn1.TabIndex = 22;
            btn1.Text = "1";
            btn1.UseVisualStyleBackColor = false;
            btn1.Click += Digit_Click;
            // 
            // btn2
            // 
            btn2.BackColor = Color.White;
            btn2.Dock = DockStyle.Fill;
            btn2.FlatAppearance.BorderColor = Color.FromArgb(205, 225, 205);
            btn2.FlatAppearance.BorderSize = 1;
            btn2.FlatStyle = FlatStyle.Flat;
            btn2.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn2.ForeColor = Color.FromArgb(33, 37, 41);
            btn2.Margin = new Padding(4);
            btn2.Name = "btn2";
            btn2.TabIndex = 23;
            btn2.Text = "2";
            btn2.UseVisualStyleBackColor = false;
            btn2.Click += Digit_Click;
            // 
            // btn3
            // 
            btn3.BackColor = Color.White;
            btn3.Dock = DockStyle.Fill;
            btn3.FlatAppearance.BorderColor = Color.FromArgb(205, 225, 205);
            btn3.FlatAppearance.BorderSize = 1;
            btn3.FlatStyle = FlatStyle.Flat;
            btn3.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn3.ForeColor = Color.FromArgb(33, 37, 41);
            btn3.Margin = new Padding(4);
            btn3.Name = "btn3";
            btn3.TabIndex = 24;
            btn3.Text = "3";
            btn3.UseVisualStyleBackColor = false;
            btn3.Click += Digit_Click;
            // 
            // btnEquals
            // 
            btnEquals.BackColor = Color.FromArgb(14, 165, 103);
            btnEquals.Dock = DockStyle.Fill;
            btnEquals.FlatAppearance.BorderColor = Color.FromArgb(205, 225, 205);
            btnEquals.FlatAppearance.BorderSize = 1;
            btnEquals.FlatStyle = FlatStyle.Flat;
            btnEquals.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnEquals.ForeColor = Color.White;
            btnEquals.Margin = new Padding(4);
            btnEquals.Name = "btnEquals";
            btnEquals.TabIndex = 25;
            btnEquals.Text = "=";
            btnEquals.UseVisualStyleBackColor = false;
            btnEquals.Click += Equals_Click;
            // 
            // btn0
            // 
            btn0.BackColor = Color.White;
            btn0.Dock = DockStyle.Fill;
            btn0.FlatAppearance.BorderColor = Color.FromArgb(205, 225, 205);
            btn0.FlatAppearance.BorderSize = 1;
            btn0.FlatStyle = FlatStyle.Flat;
            btn0.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn0.ForeColor = Color.FromArgb(33, 37, 41);
            btn0.Margin = new Padding(4);
            btn0.Name = "btn0";
            btn0.TabIndex = 26;
            btn0.Text = "0";
            btn0.UseVisualStyleBackColor = false;
            btn0.Click += Digit_Click;
            // 
            // btnDot
            // 
            btnDot.BackColor = Color.White;
            btnDot.Dock = DockStyle.Fill;
            btnDot.FlatAppearance.BorderColor = Color.FromArgb(205, 225, 205);
            btnDot.FlatAppearance.BorderSize = 1;
            btnDot.FlatStyle = FlatStyle.Flat;
            btnDot.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDot.ForeColor = Color.FromArgb(33, 37, 41);
            btnDot.Margin = new Padding(4);
            btnDot.Name = "btnDot";
            btnDot.TabIndex = 27;
            btnDot.Text = ".";
            btnDot.UseVisualStyleBackColor = false;
            btnDot.Click += Dot_Click;
            // 
            // btnClose
            // 
            btnClose.BackColor = Color.FromArgb(108, 117, 125);
            btnClose.Dock = DockStyle.Bottom;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnClose.ForeColor = Color.White;
            btnClose.Location = new Point(0, 410);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(320, 38);
            btnClose.TabIndex = 28;
            btnClose.Text = "إغلاق (Esc)";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // tblButtons - ترتيب الأزرار
            // 
            tblButtons.Controls.Add(btnClear, 0, 0);
            tblButtons.Controls.Add(btnBackspace, 1, 0);
            tblButtons.Controls.Add(btnDivide, 2, 0);
            tblButtons.Controls.Add(btnMultiply, 3, 0);
            tblButtons.Controls.Add(btn7, 0, 1);
            tblButtons.Controls.Add(btn8, 1, 1);
            tblButtons.Controls.Add(btn9, 2, 1);
            tblButtons.Controls.Add(btnSubtract, 3, 1);
            tblButtons.Controls.Add(btn4, 0, 2);
            tblButtons.Controls.Add(btn5, 1, 2);
            tblButtons.Controls.Add(btn6, 2, 2);
            tblButtons.Controls.Add(btnAdd, 3, 2);
            tblButtons.Controls.Add(btn1, 0, 3);
            tblButtons.Controls.Add(btn2, 1, 3);
            tblButtons.Controls.Add(btn3, 2, 3);
            tblButtons.Controls.Add(btnEquals, 3, 3);
            tblButtons.Controls.Add(btn0, 0, 4);
            tblButtons.SetColumnSpan(btn0, 2);
            tblButtons.Controls.Add(btnDot, 2, 4);
            // 
            // frmCalculator
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.FromArgb(170, 220, 170);
            CancelButton = btnClose;
            ClientSize = new Size(320, 448);
            Controls.Add(tblButtons);
            Controls.Add(btnClose);
            Controls.Add(txtDisplay);
            Controls.Add(lblTitle);
            Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmCalculator";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "آلة حاسبة";
            KeyDown += frmCalculator_KeyDown;
            tblButtons.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox txtDisplay;
        private System.Windows.Forms.TableLayoutPanel tblButtons;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnBackspace;
        private System.Windows.Forms.Button btnDivide;
        private System.Windows.Forms.Button btnMultiply;
        private System.Windows.Forms.Button btn7;
        private System.Windows.Forms.Button btn8;
        private System.Windows.Forms.Button btn9;
        private System.Windows.Forms.Button btnSubtract;
        private System.Windows.Forms.Button btn4;
        private System.Windows.Forms.Button btn5;
        private System.Windows.Forms.Button btn6;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btn1;
        private System.Windows.Forms.Button btn2;
        private System.Windows.Forms.Button btn3;
        private System.Windows.Forms.Button btnEquals;
        private System.Windows.Forms.Button btn0;
        private System.Windows.Forms.Button btnDot;
        private System.Windows.Forms.Button btnClose;
    }
}
