namespace SalesSystem.Desktop.Screens.Inventory.Forms
{
    partial class frmInventoryList
    {
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            lblMode = new Label();
            dgvMovements = new DataGridView();
            btnClose = new Button();
            lblRecordsCount = new Label();
            label1 = new Label();
            label2 = new Label();
            txtFilterValue = new TextBox();
            btnSearch = new Button();
            btnRefresh = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvMovements).BeginInit();
            SuspendLayout();
            // 
            // lblMode
            // 
            lblMode.Anchor = AnchorStyles.Top;
            lblMode.AutoSize = true;
            lblMode.BackColor = Color.WhiteSmoke;
            lblMode.FlatStyle = FlatStyle.Flat;
            lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            lblMode.ForeColor = Color.Firebrick;
            lblMode.Location = new Point(667, 20);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(190, 39);
            lblMode.TabIndex = 30;
            lblMode.Text = "\u062D\u0631\u0643\u0627\u062A \u0627\u0644\u0645\u062E\u0632\u0648\u0646";
            // 
            // dgvMovements
            // 
            dgvMovements.AllowUserToAddRows = false;
            dgvMovements.AllowUserToDeleteRows = false;
            dgvMovements.AllowUserToOrderColumns = true;
            dgvMovements.BackgroundColor = SystemColors.Window;
            dgvMovements.BorderStyle = BorderStyle.Fixed3D;
            dgvMovements.ColumnHeadersHeight = 50;
            dgvMovements.Location = new Point(12, 160);
            dgvMovements.MultiSelect = false;
            dgvMovements.Name = "dgvMovements";
            dgvMovements.ReadOnly = true;
            dgvMovements.RowHeadersWidth = 62;
            dgvMovements.RowTemplate.Height = 28;
            dgvMovements.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMovements.Size = new Size(1540, 540);
            dgvMovements.TabIndex = 34;
            dgvMovements.TabStop = false;
            dgvMovements.CellFormatting += dgvMovements_CellFormatting;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnClose.BackColor = Color.White;
            btnClose.DialogResult = DialogResult.Cancel;
            btnClose.FlatStyle = FlatStyle.Popup;
            btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnClose.Location = new Point(12, 710);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(181, 61);
            btnClose.TabIndex = 0;
            btnClose.Text = "\u0625\u063A\u0644\u0627\u0642";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // lblRecordsCount
            // 
            lblRecordsCount.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            lblRecordsCount.AutoSize = true;
            lblRecordsCount.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblRecordsCount.Location = new Point(1380, 720);
            lblRecordsCount.Name = "lblRecordsCount";
            lblRecordsCount.Size = new Size(72, 24);
            lblRecordsCount.TabIndex = 4;
            lblRecordsCount.Text = "[????]";
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label1.Location = new Point(1220, 720);
            label1.Name = "label1";
            label1.Size = new Size(152, 24);
            label1.TabIndex = 3;
            label1.Text = "\u0639\u062F\u062F \u0627\u0644\u062D\u0631\u0643\u0627\u062A:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label2.Location = new Point(21, 115);
            label2.Name = "label2";
            label2.Size = new Size(107, 24);
            label2.TabIndex = 5;
            label2.Text = "\u0628\u062D\u062B \u0639\u0646:";
            // 
            // txtFilterValue
            // 
            txtFilterValue.Font = new Font("Tahoma", 10F);
            txtFilterValue.Location = new Point(145, 111);
            txtFilterValue.Name = "txtFilterValue";
            txtFilterValue.Size = new Size(400, 32);
            txtFilterValue.TabIndex = 2;
            txtFilterValue.KeyPress += txtFilterValue_KeyPress;
            // 
            // btnSearch
            // 
            btnSearch.BackColor = Color.White;
            btnSearch.FlatStyle = FlatStyle.Popup;
            btnSearch.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnSearch.Location = new Point(555, 109);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(100, 36);
            btnSearch.TabIndex = 6;
            btnSearch.Text = "\u0628\u062D\u062B";
            btnSearch.UseVisualStyleBackColor = true;
            btnSearch.Click += btnSearch_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = Color.White;
            btnRefresh.FlatStyle = FlatStyle.Popup;
            btnRefresh.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnRefresh.Location = new Point(665, 109);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(100, 36);
            btnRefresh.TabIndex = 7;
            btnRefresh.Text = "\u062A\u062D\u062F\u064A\u062B";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // frmInventoryList
            // 
            AcceptButton = btnSearch;
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            CancelButton = btnClose;
            ClientSize = new Size(1564, 783);
            Controls.Add(btnRefresh);
            Controls.Add(btnSearch);
            Controls.Add(lblMode);
            Controls.Add(dgvMovements);
            Controls.Add(label2);
            Controls.Add(txtFilterValue);
            Controls.Add(label1);
            Controls.Add(lblRecordsCount);
            Controls.Add(btnClose);
            Font = new Font("Tahoma", 12F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "frmInventoryList";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "\u062D\u0631\u0643\u0627\u062A \u0627\u0644\u0645\u062E\u0632\u0648\u0646";
            Load += frmInventoryList_Load;
            ((System.ComponentModel.ISupportInitialize)dgvMovements).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnClose;
        private Label label1;
        private Label lblRecordsCount;
        private Label label2;
        private TextBox txtFilterValue;
        private Button btnSearch;
        private Button btnRefresh;
        private Label lblMode;
        private DataGridView dgvMovements;
    }
}
