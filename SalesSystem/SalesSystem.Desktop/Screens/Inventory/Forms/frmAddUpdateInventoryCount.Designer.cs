namespace SalesSystem.Desktop.Screens.Inventory.Forms
{
    partial class frmInventoryCountEditor
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            components?.Dispose();base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            errorProvider = new ErrorProvider(components);
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            lblMode = new Label();
            groupHeader = new GroupBox();
            lblCountNo = new Label();
            txtCountNo = new TextBox();
            lblWarehouse = new Label();
            cmbWarehouse = new ComboBox();
            lblNotes = new Label();
            txtNotes = new TextBox();
            groupLines = new GroupBox();
            dgvLines = new DataGridView();
            btnSaveDraft = new Button();
            btnPost = new Button();
            btnClose = new Button();

            groupHeader.SuspendLayout();
            groupLines.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvLines).BeginInit();
            SuspendLayout();

            // lblMode
            lblMode.Anchor = AnchorStyles.Top;
            lblMode.AutoSize = true;
            lblMode.BackColor = Color.WhiteSmoke;
            lblMode.FlatStyle = FlatStyle.Flat;
            lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            lblMode.ForeColor = Color.Firebrick;
            lblMode.Location = new Point(620, 15);
            lblMode.Name = "lblMode"; lblMode.Size = new Size(230, 39);
            lblMode.Text = "جرد مخزون";

            // groupHeader
            groupHeader.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            groupHeader.Location = new Point(12, 65);
            groupHeader.Name = "groupHeader";
            groupHeader.RightToLeft = RightToLeft.Yes;
            groupHeader.Size = new Size(1540, 100);
            groupHeader.TabIndex = 0;
            groupHeader.TabStop = false;
            groupHeader.Text = "معلومات الجرد";

            // lblCountNo
            lblCountNo.AutoSize = true;
            lblCountNo.Font = new Font("Tahoma", 10F);
            lblCountNo.Location = new Point(1370, 30);
            lblCountNo.Name = "lblCountNo"; lblCountNo.Size = new Size(110, 24);
            lblCountNo.Text = "رقم الجرد:";

            // txtCountNo
            txtCountNo.Font = new Font("Tahoma", 10F);
            txtCountNo.Location = new Point(1200, 27);
            txtCountNo.Name = "txtCountNo"; txtCountNo.ReadOnly = true;
            txtCountNo.Size = new Size(150, 32);

            // lblWarehouse
            lblWarehouse.AutoSize = true;
            lblWarehouse.Font = new Font("Tahoma", 10F);
            lblWarehouse.Location = new Point(1070, 30);
            lblWarehouse.Name = "lblWarehouse"; lblWarehouse.Size = new Size(100, 24);
            lblWarehouse.Text = "المستودع:";

            // cmbWarehouse
            cmbWarehouse.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbWarehouse.Font = new Font("Tahoma", 10F);
            cmbWarehouse.Location = new Point(840, 27);
            cmbWarehouse.Name = "cmbWarehouse"; cmbWarehouse.Size = new Size(200, 32);

            // lblNotes
            lblNotes.AutoSize = true;
            lblNotes.Font = new Font("Tahoma", 10F);
            lblNotes.Location = new Point(750, 30);
            lblNotes.Name = "lblNotes"; lblNotes.Size = new Size(80, 24);
            lblNotes.Text = "ملاحظات:";

            // txtNotes
            txtNotes.Font = new Font("Tahoma", 10F);
            txtNotes.Location = new Point(15, 27);
            txtNotes.Name = "txtNotes"; txtNotes.Size = new Size(720, 32);

            // groupLines
            groupLines.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            groupLines.Location = new Point(12, 175);
            groupLines.Name = "groupLines";
            groupLines.RightToLeft = RightToLeft.Yes;
            groupLines.Size = new Size(1540, 490);
            groupLines.TabIndex = 1;
            groupLines.TabStop = false;
            groupLines.Text = "البنود";

            // dgvLines
            dgvLines.AllowUserToAddRows = false;
            dgvLines.AllowUserToDeleteRows = false;
            dgvLines.AllowUserToOrderColumns = true;
            dgvLines.BackgroundColor = SystemColors.Window;
            dgvLines.BorderStyle = BorderStyle.Fixed3D;
            dgvLines.ColumnHeadersHeight = 50;
            dgvLines.Location = new Point(10, 35);
            dgvLines.MultiSelect = false;
            dgvLines.Name = "dgvLines"; dgvLines.ReadOnly = true;
            dgvLines.RowHeadersWidth = 62;
            dgvLines.RowTemplate.Height = 28;
            dgvLines.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLines.Size = new Size(1515, 430);

            // btnSaveDraft
            btnSaveDraft.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnSaveDraft.BackColor = Color.White;
            btnSaveDraft.FlatStyle = FlatStyle.Popup;
            btnSaveDraft.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnSaveDraft.Location = new Point(140, 710);
            btnSaveDraft.Name = "btnSaveDraft"; btnSaveDraft.Size = new Size(140, 50);
            btnSaveDraft.Text = "حفظ مسودة";
            btnSaveDraft.UseVisualStyleBackColor = false;
            btnSaveDraft.Click += btnSaveDraft_Click;

            // btnPost
            btnPost.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnPost.BackColor = Color.White;
            btnPost.FlatStyle = FlatStyle.Popup;
            btnPost.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnPost.Location = new Point(290, 710);
            btnPost.Name = "btnPost"; btnPost.Size = new Size(140, 50);
            btnPost.Text = "ترحيل";
            btnPost.UseVisualStyleBackColor = false;
            btnPost.Click += btnPost_Click;

            // btnClose
            btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnClose.BackColor = Color.White;
            btnClose.DialogResult = DialogResult.Cancel;
            btnClose.FlatStyle = FlatStyle.Popup;
            btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnClose.Location = new Point(12, 710);
            btnClose.Name = "btnClose"; btnClose.Size = new Size(120, 50);
            btnClose.Text = "إغلاق";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;

            // frmInventoryCountEditor
            AcceptButton = btnPost;
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            CancelButton = btnClose;
            ClientSize = new Size(1564, 783);
            Controls.Add(btnPost); Controls.Add(btnSaveDraft);
            Controls.Add(btnClose); Controls.Add(lblMode);
            Controls.Add(groupHeader); Controls.Add(groupLines);
            Font = new Font("Tahoma", 12F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "frmInventoryCountEditor";
            RightToLeft = RightToLeft.Yes; RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "جرد مخزون جديد";
            Load += frmInventoryCountEditor_Load;

            groupHeader.ResumeLayout(false);
            groupHeader.PerformLayout();
            groupLines.ResumeLayout(false);
            groupLines.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvLines).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #region Fields
        private Label lblMode;
        private GroupBox groupHeader;
        private Label lblCountNo; private TextBox txtCountNo;
        private Label lblWarehouse; private ComboBox cmbWarehouse;
        private Label lblNotes; private TextBox txtNotes;
        private GroupBox groupLines;
        private DataGridView dgvLines;
        private Button btnSaveDraft; private Button btnPost; private Button btnClose;
        private ErrorProvider errorProvider;
        #endregion
    }
}
