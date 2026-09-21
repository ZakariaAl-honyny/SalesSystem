namespace SalesSystem.Desktop.Screens.Accounting.Forms
{
    partial class frmAccountingAddCostCenter
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            components?.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            lblTitle = new Label();
            grpDetails = new GroupBox();
            chkIsActive = new CheckBox();
            cmbManager = new ComboBox();
            lblManagerLabel = new Label();
            nudBudget = new NumericUpDown();
            lblBudgetLabel = new Label();
            txtCode = new TextBox();
            lblCodeLabel = new Label();
            txtCostCenterName = new TextBox();
            lblNameLabel = new Label();
            btnSave = new Button();
            btnCancel = new Button();
            errorProvider = new ErrorProvider(components);
            grpDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudBudget).BeginInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.BackColor = Color.FromArgb(33, 43, 54);
            lblTitle.Dock = DockStyle.Top;
            lblTitle.Font = new Font("Tahoma", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(0, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(648, 48);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "إضافة مركز تكلفة";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // grpDetails
            // 
            grpDetails.Controls.Add(chkIsActive);
            grpDetails.Controls.Add(cmbManager);
            grpDetails.Controls.Add(lblManagerLabel);
            grpDetails.Controls.Add(nudBudget);
            grpDetails.Controls.Add(lblBudgetLabel);
            grpDetails.Controls.Add(txtCode);
            grpDetails.Controls.Add(lblCodeLabel);
            grpDetails.Controls.Add(txtCostCenterName);
            grpDetails.Controls.Add(lblNameLabel);
            grpDetails.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            grpDetails.Location = new Point(13, 51);
            grpDetails.Name = "grpDetails";
            grpDetails.RightToLeft = RightToLeft.Yes;
            grpDetails.Size = new Size(623, 300);
            grpDetails.TabIndex = 1;
            grpDetails.TabStop = false;
            grpDetails.Text = "بيانات مركز التكلفة";
            // 
            // chkIsActive
            // 
            chkIsActive.AutoSize = true;
            chkIsActive.Checked = true;
            chkIsActive.CheckState = CheckState.Checked;
            chkIsActive.Font = new Font("Tahoma", 10F);
            chkIsActive.Location = new Point(12, 204);
            chkIsActive.Name = "chkIsActive";
            chkIsActive.RightToLeft = RightToLeft.Yes;
            chkIsActive.Size = new Size(77, 28);
            chkIsActive.TabIndex = 8;
            chkIsActive.Text = "نشط";
            chkIsActive.UseVisualStyleBackColor = true;
            // 
            // cmbManager
            // 
            cmbManager.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbManager.Font = new Font("Tahoma", 10F);
            cmbManager.FormattingEnabled = true;
            cmbManager.Location = new Point(12, 160);
            cmbManager.Name = "cmbManager";
            cmbManager.RightToLeft = RightToLeft.Yes;
            cmbManager.Size = new Size(356, 32);
            cmbManager.TabIndex = 7;
            // 
            // lblManagerLabel
            // 
            lblManagerLabel.AutoSize = true;
            lblManagerLabel.Font = new Font("Tahoma", 10F);
            lblManagerLabel.Location = new Point(380, 162);
            lblManagerLabel.Name = "lblManagerLabel";
            lblManagerLabel.Size = new Size(91, 24);
            lblManagerLabel.TabIndex = 6;
            lblManagerLabel.Text = "المسؤول:";
            // 
            // nudBudget
            // 
            nudBudget.DecimalPlaces = 2;
            nudBudget.Font = new Font("Tahoma", 10F);
            nudBudget.Location = new Point(12, 116);
            nudBudget.Maximum = new decimal(new int[] { 999999999, 0, 0, 0 });
            nudBudget.Name = "nudBudget";
            nudBudget.RightToLeft = RightToLeft.Yes;
            nudBudget.Size = new Size(356, 32);
            nudBudget.TabIndex = 5;
            nudBudget.TextAlign = HorizontalAlignment.Right;
            nudBudget.ThousandsSeparator = true;
            // 
            // lblBudgetLabel
            // 
            lblBudgetLabel.AutoSize = true;
            lblBudgetLabel.Font = new Font("Tahoma", 10F);
            lblBudgetLabel.Location = new Point(380, 118);
            lblBudgetLabel.Name = "lblBudgetLabel";
            lblBudgetLabel.Size = new Size(78, 24);
            lblBudgetLabel.TabIndex = 4;
            lblBudgetLabel.Text = "الموازنة:";
            // 
            // txtCode
            // 
            txtCode.BackColor = Color.Gainsboro;
            txtCode.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtCode.ForeColor = Color.Firebrick;
            txtCode.Location = new Point(12, 72);
            txtCode.Name = "txtCode";
            txtCode.ReadOnly = true;
            txtCode.RightToLeft = RightToLeft.Yes;
            txtCode.Size = new Size(356, 32);
            txtCode.TabIndex = 3;
            txtCode.TabStop = false;
            txtCode.Text = "CC-05";
            // 
            // lblCodeLabel
            // 
            lblCodeLabel.AutoSize = true;
            lblCodeLabel.Font = new Font("Tahoma", 10F);
            lblCodeLabel.Location = new Point(380, 74);
            lblCodeLabel.Name = "lblCodeLabel";
            lblCodeLabel.Size = new Size(57, 24);
            lblCodeLabel.TabIndex = 2;
            lblCodeLabel.Text = "الكود:";
            // 
            // txtCostCenterName
            // 
            txtCostCenterName.Font = new Font("Tahoma", 10F);
            txtCostCenterName.Location = new Point(12, 28);
            txtCostCenterName.Name = "txtCostCenterName";
            txtCostCenterName.RightToLeft = RightToLeft.Yes;
            txtCostCenterName.Size = new Size(356, 32);
            txtCostCenterName.TabIndex = 1;
            // 
            // lblNameLabel
            // 
            lblNameLabel.AutoSize = true;
            lblNameLabel.Font = new Font("Tahoma", 10F);
            lblNameLabel.Location = new Point(380, 30);
            lblNameLabel.Name = "lblNameLabel";
            lblNameLabel.Size = new Size(239, 24);
            lblNameLabel.TabIndex = 0;
            lblNameLabel.Text = "اسم مركز التكلفة (مطلوب):";
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.White;
            btnSave.FlatStyle = FlatStyle.Popup;
            btnSave.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSave.Location = new Point(338, 372);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(150, 44);
            btnSave.TabIndex = 9;
            btnSave.Text = "حفظ";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.White;
            btnCancel.FlatStyle = FlatStyle.Popup;
            btnCancel.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCancel.Location = new Point(12, 372);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(90, 44);
            btnCancel.TabIndex = 10;
            btnCancel.Text = "إلغاء";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // errorProvider
            // 
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errorProvider.ContainerControl = this;
            errorProvider.RightToLeft = true;
            // 
            // frmAccountingAddCostCenter
            // 
            AcceptButton = btnSave;
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            CancelButton = btnCancel;
            ClientSize = new Size(648, 517);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(grpDetails);
            Controls.Add(lblTitle);
            Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmAccountingAddCostCenter";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "إضافة مركز تكلفة";
            grpDetails.ResumeLayout(false);
            grpDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudBudget).EndInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.GroupBox grpDetails;
        private System.Windows.Forms.Label lblNameLabel;
        private System.Windows.Forms.TextBox txtCostCenterName;
        private System.Windows.Forms.Label lblCodeLabel;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Label lblBudgetLabel;
        private System.Windows.Forms.NumericUpDown nudBudget;
        private System.Windows.Forms.Label lblManagerLabel;
        private System.Windows.Forms.ComboBox cmbManager;
        private System.Windows.Forms.CheckBox chkIsActive;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}