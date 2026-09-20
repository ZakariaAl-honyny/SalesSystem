namespace SalesManagement.Units
{
    partial class frmListUnits
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            dgvUnits = new DataGridView();
            cmsUnits = new ContextMenuStrip(components);
            refresh_toolStripMenuItem1 = new ToolStripMenuItem();
            toolStripMenuItem2 = new ToolStripSeparator();
            toolStripMenuItem3 = new ToolStripSeparator();
            editToolStripMenuItem = new ToolStripMenuItem();
            addNewPeraonToolStripMenuItem = new ToolStripMenuItem();
            DeletePerson_toolStripMenuItem4 = new ToolStripMenuItem();
            toolStripMenuItem5 = new ToolStripSeparator();
            lblMode = new Label();
            txtFilterValue = new TextBox();
            cbFilterBy = new ComboBox();
            label2 = new Label();
            lblRecordsCount = new Label();
            label1 = new Label();
            btnClose = new Button();
            pictureBox1 = new PictureBox();
            btnAddNewUnit = new Button();
            pnlTop = new Panel();
            pnlFilter = new Panel();
            pnlBottom = new Panel();

            ((System.ComponentModel.ISupportInitialize)dgvUnits).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            cmsUnits.SuspendLayout();
            pnlTop.SuspendLayout();
            pnlFilter.SuspendLayout();
            pnlBottom.SuspendLayout();
            SuspendLayout();

            // ══════════════════════════════════════════════
            //   pnlTop — يحتوي الصورة والعنوان وزر الإضافة
            //   Dock = Top يضمن بقاءه في الأعلى دائماً
            // ══════════════════════════════════════════════
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Height = 220;
            pnlTop.BackColor = Color.WhiteSmoke;
            pnlTop.Padding = new Padding(8);
            pnlTop.Name = "pnlTop";

            // ── الصورة ──
            pictureBox1.Image = Properties.Resources.People_512;
            pictureBox1.Size = new Size(150, 150);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.Anchor = AnchorStyles.Top;
            pictureBox1.Location = new Point(714, 8);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.TabStop = false;

            // ── العنوان ──
            lblMode.Text = "شاشة إدارة الوحدات";
            lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            lblMode.ForeColor = Color.Firebrick;
            lblMode.AutoSize = true;
            lblMode.Anchor = AnchorStyles.Top;
            lblMode.Location = new Point(615, 166);
            lblMode.Name = "lblMode";

            // ── زر الإضافة — Anchor يجعله يلتصق بالأعلى والجهة ──
            btnAddNewUnit.Text = "";
            btnAddNewUnit.Image = Properties.Resources.Add_Person_40;
            btnAddNewUnit.Size = new Size(75, 75);
            btnAddNewUnit.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnAddNewUnit.Location = new Point(8, 80);
            btnAddNewUnit.FlatStyle = FlatStyle.Popup;
            btnAddNewUnit.BackColor = Color.White;
            btnAddNewUnit.FlatAppearance.BorderSize = 2;
            btnAddNewUnit.FlatAppearance.BorderColor = Color.Black;
            btnAddNewUnit.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnAddNewUnit.Name = "btnAddNewUnit";
            btnAddNewUnit.UseVisualStyleBackColor = false;
            btnAddNewUnit.Click += btnAddNewUnit_Click;

            pnlTop.Controls.Add(pictureBox1);
            pnlTop.Controls.Add(lblMode);
            pnlTop.Controls.Add(btnAddNewUnit);

            // ══════════════════════════════════════════════
            //   pnlFilter — شريط الفلتر
            //   Dock = Top يجعله يظهر أسفل pnlTop مباشرة
            // ══════════════════════════════════════════════
            pnlFilter.Dock = DockStyle.Top;
            pnlFilter.Height = 55;
            pnlFilter.BackColor = Color.WhiteSmoke;
            pnlFilter.Padding = new Padding(8, 8, 8, 0);
            pnlFilter.Name = "pnlFilter";

            label2.Text = "البحث عن طريق:";
            label2.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label2.AutoSize = true;
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label2.Location = new Point(1360, 15);
            label2.Name = "label2";

            // DropDownList يمنع الكتابة اليدوية في الـ ComboBox
            cbFilterBy.DropDownStyle = ComboBoxStyle.DropDownList;
            cbFilterBy.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            cbFilterBy.BackColor = SystemColors.Control;
            cbFilterBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cbFilterBy.Location = new Point(990, 12);
            cbFilterBy.Size = new Size(329, 32);
            cbFilterBy.Name = "cbFilterBy";
            cbFilterBy.Items.AddRange(new object[] { "لا شيئ", "رقم الوحدة", "اسم الوحدة", "تفاصيل الوحدة" });
            cbFilterBy.SelectedIndexChanged += cbFilterBy_SelectedIndexChanged;

            txtFilterValue.Font = new Font("Tahoma", 10F);
            txtFilterValue.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtFilterValue.Location = new Point(560, 12);
            txtFilterValue.Size = new Size(403, 32);
            txtFilterValue.Visible = false;
            txtFilterValue.Name = "txtFilterValue";
            txtFilterValue.TextChanged += txtFilter_TextChanged;
            txtFilterValue.KeyPress += txtFilterValue_KeyPress;

            pnlFilter.Controls.Add(label2);
            pnlFilter.Controls.Add(cbFilterBy);
            pnlFilter.Controls.Add(txtFilterValue);

            // ══════════════════════════════════════════════
            //   pnlBottom — شريط السفلي (إغلاق + عداد)
            //   Dock = Bottom يجعله يلتصق بأسفل النموذج
            // ══════════════════════════════════════════════
            pnlBottom.Dock = DockStyle.Bottom;
            pnlBottom.Height = 70;
            pnlBottom.BackColor = Color.WhiteSmoke;
            pnlBottom.Padding = new Padding(8);
            pnlBottom.Name = "pnlBottom";

            btnClose.Text = "إغلاق";
            btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnClose.Image = Properties.Resources.Close_32;
            btnClose.ImageAlign = ContentAlignment.MiddleRight;
            btnClose.Size = new Size(181, 52);
            btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnClose.Location = new Point(8, 8);
            btnClose.FlatStyle = FlatStyle.Popup;
            btnClose.BackColor = Color.White;
            btnClose.DialogResult = DialogResult.Cancel;
            btnClose.Name = "btnClose";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;

            label1.Text = "عدد الوحدات:";
            label1.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label1.AutoSize = true;
            label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label1.Location = new Point(220, 20);
            label1.Name = "label1";

            lblRecordsCount.Text = "0";
            lblRecordsCount.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblRecordsCount.AutoSize = true;
            lblRecordsCount.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblRecordsCount.Location = new Point(380, 20);
            lblRecordsCount.Name = "lblRecordsCount";

            pnlBottom.Controls.Add(btnClose);
            pnlBottom.Controls.Add(label1);
            pnlBottom.Controls.Add(lblRecordsCount);

            // ══════════════════════════════════════════════
            //   dgvUnits
            //   Dock = Fill يملأ المساحة المتبقية تلقائياً
            //   بين pnlTop+pnlFilter من الأعلى و pnlBottom من الأسفل
            // ══════════════════════════════════════════════
            dgvUnits.Dock = DockStyle.Fill;
            dgvUnits.AllowUserToAddRows = false;
            dgvUnits.AllowUserToDeleteRows = false;
            dgvUnits.AllowUserToOrderColumns = true;
            dgvUnits.BackgroundColor = Color.White;
            dgvUnits.BorderStyle = BorderStyle.Fixed3D;
            dgvUnits.ColumnHeadersHeight = 50;
            dgvUnits.ContextMenuStrip = cmsUnits;
            dgvUnits.MultiSelect = false;
            dgvUnits.ReadOnly = true;
            dgvUnits.RowHeadersWidth = 50;
            dgvUnits.RowTemplate.Height = 28;
            dgvUnits.Name = "dgvUnits";
            dgvUnits.TabStop = false;

            // AutoSizeColumnsMode = Fill يوزع الأعمدة لتملأ عرض الـ Grid تلقائياً
            // عند تغيير حجم النموذج تتمدد الأعمدة معه بدون scroll أفقي
            dgvUnits.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUnits.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvUnits.DoubleClick += dgvAllUnits_DoubleClick;

            // ══════════════════════════════════════════════
            //   إضافة الأعمدة مسبقاً
            //   يضمن ظهور الهيدر حتى لو كانت البيانات فارغة
            // ══════════════════════════════════════════════
            dgvUnits.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "UnitID",
                HeaderText = "رقم الوحدة",
                DataPropertyName = "UnitID",
                MinimumWidth = 100,
            });
            dgvUnits.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "UnitName",
                HeaderText = "اسم الوحدة",
                DataPropertyName = "UnitName",
                MinimumWidth = 180,
            });
            dgvUnits.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "UnitDescription",
                HeaderText = "تفاصيل الوحدة",
                DataPropertyName = "UnitDescription",
                MinimumWidth = 250,
            });

            // ── Context Menu ──
            cmsUnits.BackColor = Color.White;
            cmsUnits.Font = new Font("Tahoma", 10F);
            cmsUnits.ImageScalingSize = new Size(24, 24);
            cmsUnits.RightToLeft = RightToLeft.Yes;
            cmsUnits.Name = "cmsUnits";
            cmsUnits.Opening += cmsUnits_Opening;

            refresh_toolStripMenuItem1.Text = "&تحديث";
            refresh_toolStripMenuItem1.Image = Properties.Resources.refresh_40;
            refresh_toolStripMenuItem1.ShortcutKeys = Keys.Control | Keys.R;
            refresh_toolStripMenuItem1.Click += refresh_toolStripMenuItem1_Click;

            editToolStripMenuItem.Text = "تعديل";
            editToolStripMenuItem.Image = Properties.Resources.edit_32;
            editToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.E;
            editToolStripMenuItem.Click += editToolStripMenuItem_Click;

            addNewPeraonToolStripMenuItem.Text = "&إضافة";
            addNewPeraonToolStripMenuItem.Image = Properties.Resources.Add_New_User_72;
            addNewPeraonToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.A;
            addNewPeraonToolStripMenuItem.Click += addNewPeraonToolStripMenuItem_Click;

            DeletePerson_toolStripMenuItem4.Text = "حذف";
            DeletePerson_toolStripMenuItem4.Image = Properties.Resources.Delete_32;
            DeletePerson_toolStripMenuItem4.Click += DeleteUnit_toolStripMenuItem4_Click;

            cmsUnits.Items.AddRange(new ToolStripItem[]
            {
                refresh_toolStripMenuItem1,
                toolStripMenuItem2,
                editToolStripMenuItem,
                addNewPeraonToolStripMenuItem,
                toolStripMenuItem3,
                DeletePerson_toolStripMenuItem4,
                toolStripMenuItem5,
            });

            // ══════════════════════════════════════════════
            //   إعدادات النموذج الرئيسي
            // ══════════════════════════════════════════════
            AcceptButton = btnAddNewUnit;
            CancelButton = btnClose;
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(1200, 750);
            Font = new Font("Tahoma", 12F);
            FormBorderStyle = FormBorderStyle.Sizable;  // قابل للتغيير الحجم
            MinimumSize = new Size(900, 600);       // حد أدنى لمنع التشويه
            Name = "frmListUnits";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "شاشة إدارة الوحدات";
            WindowState = FormWindowState.Maximized; // يفتح بالشاشة الكاملة

            // الترتيب مهم: Bottom أولاً ثم Fill حتى يحجز مكانه
            Controls.Add(dgvUnits);
            Controls.Add(pnlFilter);
            Controls.Add(pnlTop);
            Controls.Add(pnlBottom);

            Load += frmListUnits_Load;

            ((System.ComponentModel.ISupportInitialize)dgvUnits).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            cmsUnits.ResumeLayout(false);
            pnlTop.ResumeLayout(false);
            pnlTop.PerformLayout();
            pnlFilter.ResumeLayout(false);
            pnlFilter.PerformLayout();
            pnlBottom.ResumeLayout(false);
            pnlBottom.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvUnits;
        private PictureBox pictureBox1;
        private Label lblMode;
        private TextBox txtFilterValue;
        private ComboBox cbFilterBy;
        private Label label2;
        private Label lblRecordsCount;
        private Label label1;
        private Button btnClose;
        private Button btnAddNewUnit;
        private ContextMenuStrip cmsUnits;
        private ToolStripMenuItem refresh_toolStripMenuItem1;
        private ToolStripSeparator toolStripMenuItem2;
        private ToolStripSeparator toolStripMenuItem3;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem addNewPeraonToolStripMenuItem;
        private ToolStripMenuItem DeletePerson_toolStripMenuItem4;
        private ToolStripSeparator toolStripMenuItem5;
        private Panel pnlTop;
        private Panel pnlFilter;
        private Panel pnlBottom;
    }
}