namespace SalesSystem.Desktop.Screens.Accounting.Forms
{
    partial class frmAddUpdateJournalEntry
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
            lblMode = new Label();
            label1 = new Label();
            lblEntryId = new Label();
            label2 = new Label();
            txtEntryNumber = new TextBox();
            label3 = new Label();
            dtpEntryDate = new DateTimePicker();
            label4 = new Label();
            txtDescription = new TextBox();
            label5 = new Label();
            cmbEntryType = new ComboBox();
            label6 = new Label();
            txtReferenceNumber = new TextBox();
            dgvLines = new DataGridView();
            btnAddLine = new Button();
            btnRemoveLine = new Button();
            label7 = new Label();
            lblTotalDebit = new Label();
            label8 = new Label();
            lblTotalCredit = new Label();
            lblBalanceStatus = new Label();
            btnSave = new Button();
            btnClose = new Button();
            errorProvider = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)dgvLines).BeginInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();

            // lblMode
            lblMode.AutoSize = true;
            lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            lblMode.ForeColor = Color.FromArgb(64, 64, 64);
            lblMode.Location = new Point(14, 14);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(200, 33);
            lblMode.TabIndex = 0;
            lblMode.Text = "إضافة قيد يومي جديد";
            lblMode.TextAlign = ContentAlignment.MiddleCenter;

            // label1
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 10F);
            label1.Location = new Point(860, 60);
            label1.Name = "label1";
            label1.Size = new Size(84, 21);
            label1.TabIndex = 1;
            label1.Text = "رقم القيد:";

            // lblEntryId
            lblEntryId.AutoSize = true;
            lblEntryId.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblEntryId.ForeColor = Color.FromArgb(64, 64, 64);
            lblEntryId.Location = new Point(820, 60);
            lblEntryId.Name = "lblEntryId";
            lblEntryId.Size = new Size(34, 21);
            lblEntryId.TabIndex = 2;
            lblEntryId.Text = "---";

            // label2
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 10F);
            label2.Location = new Point(680, 60);
            label2.Name = "label2";
            label2.Size = new Size(51, 21);
            label2.TabIndex = 3;
            label2.Text = "الرقم:";

            // txtEntryNumber
            txtEntryNumber.Font = new Font("Tahoma", 10F);
            txtEntryNumber.Location = new Point(540, 57);
            txtEntryNumber.Name = "txtEntryNumber";
            txtEntryNumber.ReadOnly = true;
            txtEntryNumber.RightToLeft = RightToLeft.Yes;
            txtEntryNumber.Size = new Size(134, 28);
            txtEntryNumber.TabIndex = 4;
            txtEntryNumber.Text = "(تلقائي)";

            // label3
            label3.AutoSize = true;
            label3.Font = new Font("Tahoma", 10F);
            label3.Location = new Point(430, 60);
            label3.Name = "label3";
            label3.Size = new Size(52, 21);
            label3.TabIndex = 5;
            label3.Text = "التاريخ:";

            // dtpEntryDate
            dtpEntryDate.Font = new Font("Tahoma", 10F);
            dtpEntryDate.Format = DateTimePickerFormat.Short;
            dtpEntryDate.Location = new Point(288, 57);
            dtpEntryDate.Name = "dtpEntryDate";
            dtpEntryDate.RightToLeft = RightToLeft.Yes;
            dtpEntryDate.Size = new Size(136, 28);
            dtpEntryDate.TabIndex = 6;

            // label4
            label4.AutoSize = true;
            label4.Font = new Font("Tahoma", 10F);
            label4.Location = new Point(860, 100);
            label4.Name = "label4";
            label4.Size = new Size(46, 21);
            label4.TabIndex = 7;
            label4.Text = "البيان:";

            // txtDescription
            txtDescription.Font = new Font("Tahoma", 10F);
            txtDescription.Location = new Point(288, 97);
            txtDescription.Multiline = true;
            txtDescription.Name = "txtDescription";
            txtDescription.RightToLeft = RightToLeft.Yes;
            txtDescription.Size = new Size(564, 60);
            txtDescription.TabIndex = 8;
            txtDescription.Validating += txtDescription_Validating;

            // label5
            label5.AutoSize = true;
            label5.Font = new Font("Tahoma", 10F);
            label5.Location = new Point(860, 180);
            label5.Name = "label5";
            label5.Size = new Size(72, 21);
            label5.TabIndex = 9;
            label5.Text = "نوع القيد:";

            // cmbEntryType
            cmbEntryType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbEntryType.Font = new Font("Tahoma", 10F);
            cmbEntryType.FormattingEnabled = true;
            cmbEntryType.Location = new Point(700, 177);
            cmbEntryType.Name = "cmbEntryType";
            cmbEntryType.RightToLeft = RightToLeft.Yes;
            cmbEntryType.Size = new Size(152, 28);
            cmbEntryType.TabIndex = 10;

            // label6
            label6.AutoSize = true;
            label6.Font = new Font("Tahoma", 10F);
            label6.Location = new Point(470, 180);
            label6.Name = "label6";
            label6.Size = new Size(108, 21);
            label6.TabIndex = 11;
            label6.Text = "رقم المرجع:";

            // txtReferenceNumber
            txtReferenceNumber.Font = new Font("Tahoma", 10F);
            txtReferenceNumber.Location = new Point(288, 177);
            txtReferenceNumber.Name = "txtReferenceNumber";
            txtReferenceNumber.RightToLeft = RightToLeft.Yes;
            txtReferenceNumber.Size = new Size(174, 28);
            txtReferenceNumber.TabIndex = 12;

            // dgvLines
            dgvLines.AllowUserToAddRows = false;
            dgvLines.AllowUserToDeleteRows = false;
            dgvLines.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvLines.BackgroundColor = Color.White;
            dgvLines.BorderStyle = BorderStyle.None;
            dgvLines.ColumnHeadersHeight = 50;
            dgvLines.Location = new Point(12, 220);
            dgvLines.MultiSelect = false;
            dgvLines.Name = "dgvLines";
            dgvLines.ReadOnly = true;
            dgvLines.RightToLeft = RightToLeft.Yes;
            dgvLines.RowHeadersWidth = 40;
            dgvLines.RowTemplate.Height = 28;
            dgvLines.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLines.Size = new Size(960, 280);
            dgvLines.TabIndex = 13;

            // btnAddLine
            btnAddLine.BackColor = Color.White;
            btnAddLine.FlatStyle = FlatStyle.Popup;
            btnAddLine.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAddLine.Location = new Point(140, 512);
            btnAddLine.Name = "btnAddLine";
            btnAddLine.Size = new Size(120, 36);
            btnAddLine.TabIndex = 14;
            btnAddLine.Text = "إضافة بند";
            btnAddLine.UseVisualStyleBackColor = false;
            btnAddLine.Click += btnAddLine_Click;

            // btnRemoveLine
            btnRemoveLine.BackColor = Color.White;
            btnRemoveLine.FlatStyle = FlatStyle.Popup;
            btnRemoveLine.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnRemoveLine.Location = new Point(12, 512);
            btnRemoveLine.Name = "btnRemoveLine";
            btnRemoveLine.Size = new Size(120, 36);
            btnRemoveLine.TabIndex = 15;
            btnRemoveLine.Text = "حذف بند";
            btnRemoveLine.UseVisualStyleBackColor = false;
            btnRemoveLine.Click += btnRemoveLine_Click;

            // label7
            label7.AutoSize = true;
            label7.Font = new Font("Tahoma", 11F, FontStyle.Bold);
            label7.Location = new Point(700, 520);
            label7.Name = "label7";
            label7.Size = new Size(85, 22);
            label7.TabIndex = 16;
            label7.Text = "مجموع المدين:";

            // lblTotalDebit
            lblTotalDebit.AutoSize = true;
            lblTotalDebit.Font = new Font("Tahoma", 11F, FontStyle.Bold);
            lblTotalDebit.ForeColor = Color.FromArgb(0, 100, 0);
            lblTotalDebit.Location = new Point(630, 520);
            lblTotalDebit.Name = "lblTotalDebit";
            lblTotalDebit.Size = new Size(36, 22);
            lblTotalDebit.TabIndex = 17;
            lblTotalDebit.Text = "0.00";

            // label8
            label8.AutoSize = true;
            label8.Font = new Font("Tahoma", 11F, FontStyle.Bold);
            label8.Location = new Point(460, 520);
            label8.Name = "label8";
            label8.Size = new Size(87, 22);
            label8.TabIndex = 18;
            label8.Text = "مجموع الدائن:";

            // lblTotalCredit
            lblTotalCredit.AutoSize = true;
            lblTotalCredit.Font = new Font("Tahoma", 11F, FontStyle.Bold);
            lblTotalCredit.ForeColor = Color.FromArgb(0, 100, 0);
            lblTotalCredit.Location = new Point(390, 520);
            lblTotalCredit.Name = "lblTotalCredit";
            lblTotalCredit.Size = new Size(36, 22);
            lblTotalCredit.TabIndex = 19;
            lblTotalCredit.Text = "0.00";

            // lblBalanceStatus
            lblBalanceStatus.AutoSize = true;
            lblBalanceStatus.Font = new Font("Tahoma", 11F, FontStyle.Bold);
            lblBalanceStatus.ForeColor = Color.Green;
            lblBalanceStatus.Location = new Point(260, 520);
            lblBalanceStatus.Name = "lblBalanceStatus";
            lblBalanceStatus.Size = new Size(80, 22);
            lblBalanceStatus.TabIndex = 20;
            lblBalanceStatus.Text = "✓ متوازن";

            // btnSave
            btnSave.BackColor = Color.White;
            btnSave.FlatStyle = FlatStyle.Popup;
            btnSave.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSave.Location = new Point(820, 556);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(152, 40);
            btnSave.TabIndex = 21;
            btnSave.Text = "حفظ القيد";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;

            // btnClose
            btnClose.BackColor = Color.White;
            btnClose.FlatStyle = FlatStyle.Popup;
            btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnClose.Location = new Point(12, 556);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(80, 40);
            btnClose.TabIndex = 22;
            btnClose.Text = "إلغاء";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;

            // errorProvider
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errorProvider.RightToLeft = true;

            // frmJournalEntryEditor
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(984, 608);
            Controls.Add(btnClose);
            Controls.Add(btnSave);
            Controls.Add(lblBalanceStatus);
            Controls.Add(lblTotalCredit);
            Controls.Add(label8);
            Controls.Add(lblTotalDebit);
            Controls.Add(label7);
            Controls.Add(btnRemoveLine);
            Controls.Add(btnAddLine);
            Controls.Add(dgvLines);
            Controls.Add(txtReferenceNumber);
            Controls.Add(label6);
            Controls.Add(cmbEntryType);
            Controls.Add(label5);
            Controls.Add(txtDescription);
            Controls.Add(label4);
            Controls.Add(dtpEntryDate);
            Controls.Add(label3);
            Controls.Add(txtEntryNumber);
            Controls.Add(label2);
            Controls.Add(lblEntryId);
            Controls.Add(label1);
            Controls.Add(lblMode);
            MaximizeBox = false;
            MinimizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Name = "frmJournalEntryEditor";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "إضافة قيد يومي جديد";
            Load += frmJournalEntryEditor_Load;
            ((System.ComponentModel.ISupportInitialize)dgvLines).EndInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblEntryId;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtEntryNumber;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpEntryDate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbEntryType;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtReferenceNumber;
        private System.Windows.Forms.DataGridView dgvLines;
        private System.Windows.Forms.Button btnAddLine;
        private System.Windows.Forms.Button btnRemoveLine;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblTotalDebit;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblTotalCredit;
        private System.Windows.Forms.Label lblBalanceStatus;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}
