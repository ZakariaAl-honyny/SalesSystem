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
            lblTitle.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(16), Convert.ToInt32(150), Convert.ToInt32(96));
            lblTitle.Dock = DockStyle.Top;
            lblTitle.Font = new Font("Tahoma", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.ForeColor = System.Drawing.Color.White;
            lblTitle.Location = new Point(0, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(320, 44);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "آلة حاسبة";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtDisplay
            // 
            txtDisplay.BackColor = System.Drawing.Color.White;
            txtDisplay.Dock = DockStyle.Top;
            txtDisplay.Font = new Font("Tahoma", 22F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtDisplay.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
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
            tblButtons.BackColor = System.Drawing.Color.White;
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
            btnClear.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(108), Convert.ToInt32(117), Convert.ToInt32(125));
            btnClear.Dock = DockStyle.Fill;
            btnClear.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(Convert.ToInt32(222), Convert.ToInt32(226), Convert.ToInt32(230));
            btnClear.FlatAppearance.BorderSize = 1;
            btnClear.FlatStyle = FlatStyle.Flat;
            btnClear.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnClear.ForeColor = System.Drawing.Color.White;
            btnClear.Margin = new Padding(4);
            btnClear.Name = "btnClear";
            btnClear.TabIndex = 10;
            btnClear.Text = "مسح";
            btnClear.UseVisualStyleBackColor = false;
            btnClear.Click += Clear_Click;
            // 
            // btnBackspace
            // 
            btnBackspace.BackColor = System.Drawing.Color.White;
            btnBackspace.Dock = DockStyle.Fill;
            btnBackspace.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(Convert.ToInt32(222), Convert.ToInt32(226), Convert.ToInt32(230));
            btnBackspace.FlatAppearance.BorderSize = 1;
            btnBackspace.FlatStyle = FlatStyle.Flat;
            btnBackspace.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnBackspace.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            btnBackspace.Margin = new Padding(4);
            btnBackspace.Name = "btnBackspace";
            btnBackspace.TabIndex = 11;
            btnBackspace.Text = "⌫";
            btnBackspace.UseVisualStyleBackColor = false;
            btnBackspace.Click += Backspace_Click;
            // 
            // btnDivide
            // 
            btnDivide.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(23), Convert.ToInt32(162), Convert.ToInt32(184));
            btnDivide.Dock = DockStyle.Fill;
            btnDivide.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(Convert.ToInt32(222), Convert.ToInt32(226), Convert.ToInt32(230));
            btnDivide.FlatAppearance.BorderSize = 1;
            btnDivide.FlatStyle = FlatStyle.Flat;
            btnDivide.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDivide.ForeColor = System.Drawing.Color.White;
            btnDivide.Margin = new Padding(4);
            btnDivide.Name = "btnDivide";
            btnDivide.TabIndex = 12;
            btnDivide.Text = "÷";
            btnDivide.UseVisualStyleBackColor = false;
            btnDivide.Click += Operator_Click;
            // 
            // btnMultiply
            // 
            btnMultiply.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(23), Convert.ToInt32(162), Convert.ToInt32(184));
            btnMultiply.Dock = DockStyle.Fill;
            btnMultiply.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(Convert.ToInt32(222), Convert.ToInt32(226), Convert.ToInt32(230));
            btnMultiply.FlatAppearance.BorderSize = 1;
            btnMultiply.FlatStyle = FlatStyle.Flat;
            btnMultiply.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnMultiply.ForeColor = System.Drawing.Color.White;
            btnMultiply.Margin = new Padding(4);
            btnMultiply.Name = "btnMultiply";
            btnMultiply.TabIndex = 13;
            btnMultiply.Text = "×";
            btnMultiply.UseVisualStyleBackColor = false;
            btnMultiply.Click += Operator_Click;
            // 
            // btn7
            // 
            btn7.BackColor = System.Drawing.Color.White;
            btn7.Dock = DockStyle.Fill;
            btn7.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(Convert.ToInt32(222), Convert.ToInt32(226), Convert.ToInt32(230));
            btn7.FlatAppearance.BorderSize = 1;
            btn7.FlatStyle = FlatStyle.Flat;
            btn7.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn7.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            btn7.Margin = new Padding(4);
            btn7.Name = "btn7";
            btn7.TabIndex = 14;
            btn7.Text = "7";
            btn7.UseVisualStyleBackColor = false;
            btn7.Click += Digit_Click;
            // 
            // btn8
            // 
            btn8.BackColor = System.Drawing.Color.White;
            btn8.Dock = DockStyle.Fill;
            btn8.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(Convert.ToInt32(222), Convert.ToInt32(226), Convert.ToInt32(230));
            btn8.FlatAppearance.BorderSize = 1;
            btn8.FlatStyle = FlatStyle.Flat;
            btn8.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn8.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            btn8.Margin = new Padding(4);
            btn8.Name = "btn8";
            btn8.TabIndex = 15;
            btn8.Text = "8";
            btn8.UseVisualStyleBackColor = false;
            btn8.Click += Digit_Click;
            // 
            // btn9
            // 
            btn9.BackColor = System.Drawing.Color.White;
            btn9.Dock = DockStyle.Fill;
            btn9.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(Convert.ToInt32(222), Convert.ToInt32(226), Convert.ToInt32(230));
            btn9.FlatAppearance.BorderSize = 1;
            btn9.FlatStyle = FlatStyle.Flat;
            btn9.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn9.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            btn9.Margin = new Padding(4);
            btn9.Name = "btn9";
            btn9.TabIndex = 16;
            btn9.Text = "9";
            btn9.UseVisualStyleBackColor = false;
            btn9.Click += Digit_Click;
            // 
            // btnSubtract
            // 
            btnSubtract.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(23), Convert.ToInt32(162), Convert.ToInt32(184));
            btnSubtract.Dock = DockStyle.Fill;
            btnSubtract.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(Convert.ToInt32(222), Convert.ToInt32(226), Convert.ToInt32(230));
            btnSubtract.FlatAppearance.BorderSize = 1;
            btnSubtract.FlatStyle = FlatStyle.Flat;
            btnSubtract.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSubtract.ForeColor = System.Drawing.Color.White;
            btnSubtract.Margin = new Padding(4);
            btnSubtract.Name = "btnSubtract";
            btnSubtract.TabIndex = 17;
            btnSubtract.Text = "−";
            btnSubtract.UseVisualStyleBackColor = false;
            btnSubtract.Click += Operator_Click;
            // 
            // btn4
            // 
            btn4.BackColor = System.Drawing.Color.White;
            btn4.Dock = DockStyle.Fill;
            btn4.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(Convert.ToInt32(222), Convert.ToInt32(226), Convert.ToInt32(230));
            btn4.FlatAppearance.BorderSize = 1;
            btn4.FlatStyle = FlatStyle.Flat;
            btn4.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn4.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            btn4.Margin = new Padding(4);
            btn4.Name = "btn4";
            btn4.TabIndex = 18;
            btn4.Text = "4";
            btn4.UseVisualStyleBackColor = false;
            btn4.Click += Digit_Click;
            // 
            // btn5
            // 
            btn5.BackColor = System.Drawing.Color.White;
            btn5.Dock = DockStyle.Fill;
            btn5.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(Convert.ToInt32(222), Convert.ToInt32(226), Convert.ToInt32(230));
            btn5.FlatAppearance.BorderSize = 1;
            btn5.FlatStyle = FlatStyle.Flat;
            btn5.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn5.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            btn5.Margin = new Padding(4);
            btn5.Name = "btn5";
            btn5.TabIndex = 19;
            btn5.Text = "5";
            btn5.UseVisualStyleBackColor = false;
            btn5.Click += Digit_Click;
            // 
            // btn6
            // 
            btn6.BackColor = System.Drawing.Color.White;
            btn6.Dock = DockStyle.Fill;
            btn6.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(Convert.ToInt32(222), Convert.ToInt32(226), Convert.ToInt32(230));
            btn6.FlatAppearance.BorderSize = 1;
            btn6.FlatStyle = FlatStyle.Flat;
            btn6.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn6.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            btn6.Margin = new Padding(4);
            btn6.Name = "btn6";
            btn6.TabIndex = 20;
            btn6.Text = "6";
            btn6.UseVisualStyleBackColor = false;
            btn6.Click += Digit_Click;
            // 
            // btnAdd
            // 
            btnAdd.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(23), Convert.ToInt32(162), Convert.ToInt32(184));
            btnAdd.Dock = DockStyle.Fill;
            btnAdd.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(Convert.ToInt32(222), Convert.ToInt32(226), Convert.ToInt32(230));
            btnAdd.FlatAppearance.BorderSize = 1;
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAdd.ForeColor = System.Drawing.Color.White;
            btnAdd.Margin = new Padding(4);
            btnAdd.Name = "btnAdd";
            btnAdd.TabIndex = 21;
            btnAdd.Text = "+";
            btnAdd.UseVisualStyleBackColor = false;
            btnAdd.Click += Operator_Click;
            // 
            // btn1
            // 
            btn1.BackColor = System.Drawing.Color.White;
            btn1.Dock = DockStyle.Fill;
            btn1.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(Convert.ToInt32(222), Convert.ToInt32(226), Convert.ToInt32(230));
            btn1.FlatAppearance.BorderSize = 1;
            btn1.FlatStyle = FlatStyle.Flat;
            btn1.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn1.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            btn1.Margin = new Padding(4);
            btn1.Name = "btn1";
            btn1.TabIndex = 22;
            btn1.Text = "1";
            btn1.UseVisualStyleBackColor = false;
            btn1.Click += Digit_Click;
            // 
            // btn2
            // 
            btn2.BackColor = System.Drawing.Color.White;
            btn2.Dock = DockStyle.Fill;
            btn2.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(Convert.ToInt32(222), Convert.ToInt32(226), Convert.ToInt32(230));
            btn2.FlatAppearance.BorderSize = 1;
            btn2.FlatStyle = FlatStyle.Flat;
            btn2.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn2.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            btn2.Margin = new Padding(4);
            btn2.Name = "btn2";
            btn2.TabIndex = 23;
            btn2.Text = "2";
            btn2.UseVisualStyleBackColor = false;
            btn2.Click += Digit_Click;
            // 
            // btn3
            // 
            btn3.BackColor = System.Drawing.Color.White;
            btn3.Dock = DockStyle.Fill;
            btn3.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(Convert.ToInt32(222), Convert.ToInt32(226), Convert.ToInt32(230));
            btn3.FlatAppearance.BorderSize = 1;
            btn3.FlatStyle = FlatStyle.Flat;
            btn3.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn3.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            btn3.Margin = new Padding(4);
            btn3.Name = "btn3";
            btn3.TabIndex = 24;
            btn3.Text = "3";
            btn3.UseVisualStyleBackColor = false;
            btn3.Click += Digit_Click;
            // 
            // btnEquals
            // 
            btnEquals.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(16), Convert.ToInt32(150), Convert.ToInt32(96));
            btnEquals.Dock = DockStyle.Fill;
            btnEquals.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(Convert.ToInt32(222), Convert.ToInt32(226), Convert.ToInt32(230));
            btnEquals.FlatAppearance.BorderSize = 1;
            btnEquals.FlatStyle = FlatStyle.Flat;
            btnEquals.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnEquals.ForeColor = System.Drawing.Color.White;
            btnEquals.Margin = new Padding(4);
            btnEquals.Name = "btnEquals";
            btnEquals.TabIndex = 25;
            btnEquals.Text = "=";
            btnEquals.UseVisualStyleBackColor = false;
            btnEquals.Click += Equals_Click;
            // 
            // btn0
            // 
            btn0.BackColor = System.Drawing.Color.White;
            btn0.Dock = DockStyle.Fill;
            btn0.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(Convert.ToInt32(222), Convert.ToInt32(226), Convert.ToInt32(230));
            btn0.FlatAppearance.BorderSize = 1;
            btn0.FlatStyle = FlatStyle.Flat;
            btn0.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn0.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            btn0.Margin = new Padding(4);
            btn0.Name = "btn0";
            btn0.TabIndex = 26;
            btn0.Text = "0";
            btn0.UseVisualStyleBackColor = false;
            btn0.Click += Digit_Click;
            // 
            // btnDot
            // 
            btnDot.BackColor = System.Drawing.Color.White;
            btnDot.Dock = DockStyle.Fill;
            btnDot.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(Convert.ToInt32(222), Convert.ToInt32(226), Convert.ToInt32(230));
            btnDot.FlatAppearance.BorderSize = 1;
            btnDot.FlatStyle = FlatStyle.Flat;
            btnDot.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDot.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            btnDot.Margin = new Padding(4);
            btnDot.Name = "btnDot";
            btnDot.TabIndex = 27;
            btnDot.Text = ".";
            btnDot.UseVisualStyleBackColor = false;
            btnDot.Click += Dot_Click;
            // 
            // btnClose
            // 
            btnClose.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(108), Convert.ToInt32(117), Convert.ToInt32(125));
            btnClose.Dock = DockStyle.Bottom;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnClose.ForeColor = System.Drawing.Color.White;
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
            BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(240), Convert.ToInt32(242), Convert.ToInt32(245));
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
