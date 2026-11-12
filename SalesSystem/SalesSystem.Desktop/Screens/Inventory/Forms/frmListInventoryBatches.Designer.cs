using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Inventory.Forms
{
    partial class frmListInventoryBatches
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

            pnlHeader = new Panel();
            lblMode = new Label();
            pnlToolbar = new Panel();
            flpToolbar = new FlowLayoutPanel();
            btnRefresh = new Button();
            btnClose = new Button();
            pnlFilters = new Panel();
            gbFilter = new GroupBox();
            label2 = new Label();
            cbFilterBy = new ComboBox();
            txtFilterValue = new TextBox();
            label1 = new Label();
            cbStatus = new ComboBox();
            label3 = new Label();
            dtpFrom = new DateTimePicker();
            label4 = new Label();
            dtpTo = new DateTimePicker();
            btnRangeToday = new Button();
            btnRange30 = new Button();
            btnRangeMonth = new Button();
            chkOnlyExpiringSoon = new CheckBox();
            chkOnlyExpired = new CheckBox();
            lblProduct = new Label();
            cmbProduct = new ComboBox();
            pnlSummary = new Panel();
            lblSummary = new Label();
            dgvBatches = new DataGridView();
            toolTip1 = new ToolTip(components);

            pnlHeader.SuspendLayout();
            pnlToolbar.SuspendLayout();
            flpToolbar.SuspendLayout();
            pnlFilters.SuspendLayout();
            gbFilter.SuspendLayout();
            pnlSummary.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvBatches).BeginInit();
            SuspendLayout();

            // ── pnlHeader ───────────────────────────────────────────────────
            pnlHeader.BackColor = GreenThemeHelper.HeaderBackground;
            pnlHeader.Controls.Add(lblMode);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Margin = new Padding(0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1564, 56);
            pnlHeader.TabIndex = 0;

            // ── lblMode ─────────────────────────────────────────────────────
            lblMode.BackColor = GreenThemeHelper.HeaderBackground;
            lblMode.Dock = DockStyle.Fill;
            lblMode.Font = new Font("Tahoma", 15F, FontStyle.Bold);
            lblMode.ForeColor = GreenThemeHelper.HeaderForeground;
            lblMode.Location = new Point(0, 0);
            lblMode.Margin = new Padding(0);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(1564, 56);
            lblMode.TabIndex = 0;
            lblMode.Text = "دفعات المخزون";
            lblMode.TextAlign = ContentAlignment.MiddleCenter;

            // ── pnlToolbar ──────────────────────────────────────────────────
            pnlToolbar.BackColor = GreenThemeHelper.BarBackground;
            pnlToolbar.Controls.Add(flpToolbar);
            pnlToolbar.Dock = DockStyle.Top;
            pnlToolbar.Location = new Point(0, 56);
            pnlToolbar.Margin = new Padding(0);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.Size = new Size(1564, 60);
            pnlToolbar.TabIndex = 1;

            // ── flpToolbar ──────────────────────────────────────────────────
            flpToolbar.BackColor = GreenThemeHelper.BarBackground;
            flpToolbar.Controls.Add(btnRefresh);
            flpToolbar.Controls.Add(btnClose);
            flpToolbar.Dock = DockStyle.Fill;
            flpToolbar.Location = new Point(0, 0);
            flpToolbar.Margin = new Padding(0);
            flpToolbar.Name = "flpToolbar";
            flpToolbar.Padding = new Padding(10);
            flpToolbar.Size = new Size(1564, 60);
            flpToolbar.TabIndex = 0;
            flpToolbar.WrapContents = false;

            // ── btnRefresh ──────────────────────────────────────────────────
            btnRefresh.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnRefresh.Location = new Point(1420, 13);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(124, 36);
            btnRefresh.TabIndex = 0;
            btnRefresh.Text = "تحديث";
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Click += btnRefresh_Click;

            // ── btnClose ────────────────────────────────────────────────────
            btnClose.DialogResult = DialogResult.Cancel;
            btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnClose.Location = new Point(1290, 13);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(124, 36);
            btnClose.TabIndex = 1;
            btnClose.Text = "إغلاق";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;

            // ── pnlFilters ──────────────────────────────────────────────────
            pnlFilters.BackColor = GreenThemeHelper.GroupBackground;
            pnlFilters.Controls.Add(gbFilter);
            pnlFilters.Dock = DockStyle.Top;
            pnlFilters.Location = new Point(0, 116);
            pnlFilters.Margin = new Padding(0);
            pnlFilters.Name = "pnlFilters";
            pnlFilters.Padding = new Padding(6, 4, 6, 4);
            pnlFilters.Size = new Size(1564, 112);
            pnlFilters.TabIndex = 2;

            // ── gbFilter ────────────────────────────────────────────────────
            gbFilter.BackColor = GreenThemeHelper.GroupBackground;
            gbFilter.Controls.Add(label2);
            gbFilter.Controls.Add(cbFilterBy);
            gbFilter.Controls.Add(txtFilterValue);
            gbFilter.Controls.Add(label1);
            gbFilter.Controls.Add(cbStatus);
            gbFilter.Controls.Add(label3);
            gbFilter.Controls.Add(dtpFrom);
            gbFilter.Controls.Add(label4);
            gbFilter.Controls.Add(dtpTo);
            gbFilter.Controls.Add(btnRangeToday);
            gbFilter.Controls.Add(btnRange30);
            gbFilter.Controls.Add(btnRangeMonth);
            gbFilter.Controls.Add(chkOnlyExpiringSoon);
            gbFilter.Controls.Add(chkOnlyExpired);
            gbFilter.Controls.Add(lblProduct);
            gbFilter.Controls.Add(cmbProduct);
            gbFilter.Dock = DockStyle.Fill;
            gbFilter.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            gbFilter.Location = new Point(6, 4);
            gbFilter.Margin = new Padding(0);
            gbFilter.Name = "gbFilter";
            gbFilter.Padding = new Padding(8);
            gbFilter.Size = new Size(1552, 104);
            gbFilter.TabIndex = 0;
            gbFilter.TabStop = false;
            gbFilter.Text = "بحث وتصفية";

            // ── label2 «تصفية بـ:» ──────────────────────────────────────────
            label2.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            label2.ForeColor = GreenThemeHelper.TextStrong;
            label2.Location = new Point(1440, 26);
            label2.Margin = new Padding(0);
            label2.Name = "label2";
            label2.Size = new Size(90, 26);
            label2.TabIndex = 0;
            label2.Text = "تصفية بـ:";
            label2.TextAlign = ContentAlignment.MiddleRight;

            // ── cbFilterBy ──────────────────────────────────────────────────
            cbFilterBy.DropDownStyle = ComboBoxStyle.DropDownList;
            cbFilterBy.FlatStyle = FlatStyle.Flat;
            cbFilterBy.Font = new Font("Tahoma", 9F, FontStyle.Regular);
            cbFilterBy.FormattingEnabled = true;
            cbFilterBy.Items.AddRange(new object[] { "الكل", "الصنف", "رقم الدفعة", "المستودع" });
            cbFilterBy.Location = new Point(1270, 23);
            cbFilterBy.Margin = new Padding(0);
            cbFilterBy.Name = "cbFilterBy";
            cbFilterBy.Size = new Size(160, 26);
            cbFilterBy.TabIndex = 1;
            cbFilterBy.SelectedIndexChanged += cbFilterBy_SelectedIndexChanged;

            // ── txtFilterValue ──────────────────────────────────────────────
            txtFilterValue.Font = new Font("Tahoma", 9F, FontStyle.Regular);
            txtFilterValue.Location = new Point(1054, 23);
            txtFilterValue.Margin = new Padding(0);
            txtFilterValue.Name = "txtFilterValue";
            txtFilterValue.Size = new Size(206, 25);
            txtFilterValue.TabIndex = 2;
            txtFilterValue.TextChanged += txtFilterValue_TextChanged;
            txtFilterValue.KeyPress += txtFilterValue_KeyPress;

            // ── label1 «الحالة:» ────────────────────────────────────────────
            label1.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            label1.ForeColor = GreenThemeHelper.TextStrong;
            label1.Location = new Point(966, 26);
            label1.Margin = new Padding(0);
            label1.Name = "label1";
            label1.Size = new Size(78, 26);
            label1.TabIndex = 3;
            label1.Text = "الحالة:";
            label1.TextAlign = ContentAlignment.MiddleRight;

            // ── cbStatus ────────────────────────────────────────────────────
            cbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cbStatus.FlatStyle = FlatStyle.Flat;
            cbStatus.Font = new Font("Tahoma", 9F, FontStyle.Regular);
            cbStatus.FormattingEnabled = true;
            cbStatus.Items.AddRange(new object[] { "الكل", "سارية", "قاربت الانتهاء", "منتهية" });
            cbStatus.Location = new Point(806, 23);
            cbStatus.Margin = new Padding(0);
            cbStatus.Name = "cbStatus";
            cbStatus.Size = new Size(150, 26);
            cbStatus.TabIndex = 4;
            cbStatus.SelectedIndexChanged += cbStatus_SelectedIndexChanged;

            // ── label3 «من:» ────────────────────────────────────────────────
            label3.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            label3.ForeColor = GreenThemeHelper.TextStrong;
            label3.Location = new Point(752, 26);
            label3.Margin = new Padding(0);
            label3.Name = "label3";
            label3.Size = new Size(44, 26);
            label3.TabIndex = 5;
            label3.Text = "من:";
            label3.TextAlign = ContentAlignment.MiddleRight;

            // ── dtpFrom ─────────────────────────────────────────────────────
            dtpFrom.Font = new Font("Tahoma", 9F, FontStyle.Regular);
            dtpFrom.Format = DateTimePickerFormat.Short;
            dtpFrom.Location = new Point(602, 23);
            dtpFrom.Margin = new Padding(0);
            dtpFrom.Name = "dtpFrom";
            dtpFrom.Size = new Size(140, 25);
            dtpFrom.TabIndex = 6;
            dtpFrom.ValueChanged += dtpFrom_ValueChanged;

            // ── label4 «إلى:» ───────────────────────────────────────────────
            label4.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            label4.ForeColor = GreenThemeHelper.TextStrong;
            label4.Location = new Point(548, 26);
            label4.Margin = new Padding(0);
            label4.Name = "label4";
            label4.Size = new Size(44, 26);
            label4.TabIndex = 7;
            label4.Text = "إلى:";
            label4.TextAlign = ContentAlignment.MiddleRight;

            // ── dtpTo ───────────────────────────────────────────────────────
            dtpTo.Font = new Font("Tahoma", 9F, FontStyle.Regular);
            dtpTo.Format = DateTimePickerFormat.Short;
            dtpTo.Location = new Point(398, 23);
            dtpTo.Margin = new Padding(0);
            dtpTo.Name = "dtpTo";
            dtpTo.Size = new Size(140, 25);
            dtpTo.TabIndex = 8;
            dtpTo.ValueChanged += dtpTo_ValueChanged;

            // ── btnRangeToday ───────────────────────────────────────────────
            btnRangeToday.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            btnRangeToday.Location = new Point(1430, 62);
            btnRangeToday.Name = "btnRangeToday";
            btnRangeToday.Size = new Size(100, 30);
            btnRangeToday.TabIndex = 9;
            btnRangeToday.Text = "اليوم";
            btnRangeToday.UseVisualStyleBackColor = false;
            btnRangeToday.Click += btnRangeToday_Click;

            // ── btnRange30 ──────────────────────────────────────────────────
            btnRange30.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            btnRange30.Location = new Point(1296, 62);
            btnRange30.Name = "btnRange30";
            btnRange30.Size = new Size(128, 30);
            btnRange30.TabIndex = 10;
            btnRange30.Text = "آخر 30 يوماً";
            btnRange30.UseVisualStyleBackColor = false;
            btnRange30.Click += btnRange30_Click;

            // ── btnRangeMonth ───────────────────────────────────────────────
            btnRangeMonth.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            btnRangeMonth.Location = new Point(1190, 62);
            btnRangeMonth.Name = "btnRangeMonth";
            btnRangeMonth.Size = new Size(100, 30);
            btnRangeMonth.TabIndex = 11;
            btnRangeMonth.Text = "هذا الشهر";
            btnRangeMonth.UseVisualStyleBackColor = false;
            btnRangeMonth.Click += btnRangeMonth_Click;

            // ── chkOnlyExpiringSoon ─────────────────────────────────────────
            chkOnlyExpiringSoon.Font = new Font("Tahoma", 9F, FontStyle.Regular);
            chkOnlyExpiringSoon.ForeColor = GreenThemeHelper.TextStrong;
            chkOnlyExpiringSoon.Location = new Point(210, 64);
            chkOnlyExpiringSoon.Margin = new Padding(0);
            chkOnlyExpiringSoon.Name = "chkOnlyExpiringSoon";
            chkOnlyExpiringSoon.Size = new Size(210, 26);
            chkOnlyExpiringSoon.TabIndex = 12;
            chkOnlyExpiringSoon.Text = "قاربت الانتهاء فقط";
            chkOnlyExpiringSoon.UseVisualStyleBackColor = false;
            chkOnlyExpiringSoon.CheckedChanged += chkOnlyExpiringSoon_CheckedChanged;

            // ── chkOnlyExpired ──────────────────────────────────────────────
            chkOnlyExpired.Font = new Font("Tahoma", 9F, FontStyle.Regular);
            chkOnlyExpired.ForeColor = GreenThemeHelper.TextStrong;
            chkOnlyExpired.Location = new Point(20, 64);
            chkOnlyExpired.Margin = new Padding(0);
            chkOnlyExpired.Name = "chkOnlyExpired";
            chkOnlyExpired.Size = new Size(180, 26);
            chkOnlyExpired.TabIndex = 13;
            chkOnlyExpired.Text = "المنتهية فقط";
            chkOnlyExpired.UseVisualStyleBackColor = false;
            chkOnlyExpired.CheckedChanged += chkOnlyExpired_CheckedChanged;

            // ── lblProduct «الصنف:» (مرشّح المصدر — يبقى داخل gbFilter وفق RULE-N62) ──
            lblProduct.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            lblProduct.ForeColor = GreenThemeHelper.TextStrong;
            lblProduct.Location = new Point(316, 26);
            lblProduct.Margin = new Padding(0);
            lblProduct.Name = "lblProduct";
            lblProduct.Size = new Size(64, 26);
            lblProduct.TabIndex = 14;
            lblProduct.Text = "الصنف:";
            lblProduct.TextAlign = ContentAlignment.MiddleRight;

            // ── cmbProduct ──────────────────────────────────────────────────
            cmbProduct.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbProduct.FlatStyle = FlatStyle.Flat;
            cmbProduct.Font = new Font("Tahoma", 9F, FontStyle.Regular);
            cmbProduct.FormattingEnabled = true;
            cmbProduct.Location = new Point(60, 23);
            cmbProduct.Margin = new Padding(0);
            cmbProduct.Name = "cmbProduct";
            cmbProduct.Size = new Size(250, 26);
            cmbProduct.TabIndex = 15;
            cmbProduct.SelectedIndexChanged += cmbProduct_SelectedIndexChanged;

            // ── pnlSummary ──────────────────────────────────────────────────
            pnlSummary.BackColor = GreenThemeHelper.BarBackground;
            pnlSummary.Controls.Add(lblSummary);
            pnlSummary.Dock = DockStyle.Bottom;
            pnlSummary.Location = new Point(0, 788);
            pnlSummary.Margin = new Padding(0);
            pnlSummary.Name = "pnlSummary";
            pnlSummary.Padding = new Padding(10, 0, 10, 0);
            pnlSummary.Size = new Size(1564, 32);
            pnlSummary.TabIndex = 4;

            // ── lblSummary ──────────────────────────────────────────────────
            lblSummary.Dock = DockStyle.Fill;
            lblSummary.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            lblSummary.ForeColor = GreenThemeHelper.TextStrong;
            lblSummary.Location = new Point(10, 0);
            lblSummary.Margin = new Padding(0);
            lblSummary.Name = "lblSummary";
            lblSummary.Size = new Size(1544, 32);
            lblSummary.TabIndex = 0;
            lblSummary.Text = "المعروض: 0 من 0 — إجمالي الكمية: 0.000 · منتهية: 0 · قاربت الانتهاء: 0 · إجمالي القيمة: 0.00";
            lblSummary.TextAlign = ContentAlignment.MiddleRight;

            // ── dgvBatches ──────────────────────────────────────────────────
            dgvBatches.AllowUserToAddRows = false;
            dgvBatches.AllowUserToDeleteRows = false;
            dgvBatches.AllowUserToOrderColumns = true;
            dgvBatches.AutoGenerateColumns = false;
            dgvBatches.BackgroundColor = GreenThemeHelper.GridBackground;
            dgvBatches.BorderStyle = BorderStyle.None;
            dgvBatches.ColumnHeadersHeight = 45;
            dgvBatches.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvBatches.Dock = DockStyle.Fill;
            dgvBatches.Location = new Point(0, 228);
            dgvBatches.Margin = new Padding(0);
            dgvBatches.MultiSelect = false;
            dgvBatches.Name = "dgvBatches";
            dgvBatches.ReadOnly = true;
            dgvBatches.RowHeadersVisible = false;
            dgvBatches.RowHeadersWidth = 51;
            dgvBatches.RowTemplate.Height = 32;
            dgvBatches.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBatches.Size = new Size(1564, 560);
            dgvBatches.TabIndex = 3;
            dgvBatches.TabStop = false;
            dgvBatches.CellFormatting += dgvBatches_CellFormatting;

            // ── frmListInventoryBatches ─────────────────────────────────────
            AutoScaleMode = AutoScaleMode.None;
            BackColor = GreenThemeHelper.FormBackground;
            CancelButton = btnClose;
            ClientSize = new Size(1564, 820);
            Controls.Add(dgvBatches);
            Controls.Add(pnlSummary);
            Controls.Add(pnlFilters);
            Controls.Add(pnlToolbar);
            Controls.Add(pnlHeader);
            Font = new Font("Tahoma", 10F, FontStyle.Regular);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            KeyPreview = true;
            MaximizeBox = false;
            MinimumSize = new Size(1200, 700);
            Name = "frmListInventoryBatches";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "دفعات المخزون";
            Load += frmListInventoryBatches_Load;

            pnlHeader.ResumeLayout(false);
            pnlToolbar.ResumeLayout(false);
            flpToolbar.ResumeLayout(false);
            pnlFilters.ResumeLayout(false);
            gbFilter.ResumeLayout(false);
            gbFilter.PerformLayout();
            pnlSummary.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvBatches).EndInit();
            ResumeLayout(false);
        }

        #region Fields
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.Panel pnlToolbar;
        private System.Windows.Forms.FlowLayoutPanel flpToolbar;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel pnlFilters;
        private System.Windows.Forms.GroupBox gbFilter;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbFilterBy;
        private System.Windows.Forms.TextBox txtFilterValue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbStatus;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Button btnRangeToday;
        private System.Windows.Forms.Button btnRange30;
        private System.Windows.Forms.Button btnRangeMonth;
        private System.Windows.Forms.CheckBox chkOnlyExpiringSoon;
        private System.Windows.Forms.CheckBox chkOnlyExpired;
        private System.Windows.Forms.Label lblProduct;
        private System.Windows.Forms.ComboBox cmbProduct;
        private System.Windows.Forms.Panel pnlSummary;
        private System.Windows.Forms.Label lblSummary;
        private System.Windows.Forms.DataGridView dgvBatches;
        private System.Windows.Forms.ToolTip toolTip1;
        #endregion
    }
}
