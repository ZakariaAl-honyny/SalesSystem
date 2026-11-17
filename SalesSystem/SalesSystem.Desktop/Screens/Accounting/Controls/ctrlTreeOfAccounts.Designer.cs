namespace SalesSystem.Desktop.Screens.Accounting.Controls;

partial class ctrlTreeOfAccounts
{
    private System.ComponentModel.IContainer components = null;

    // ملاحظات المصمّم:
    //   • المصمّم = التشغيل (RULE-N134 / RULE-N140 / RULE-N204): كل لون بصيغة Convert.ToInt32
    //     وبالقيمة الحرفية نفسها التي يطبّقها GreenThemeHelper وقت التشغيل — ولا مرجع لأي عضو
    //     في Helpers/ داخل هذا الملف (RULE-N128).
    //   • RULE-N145: كل زر وشريط وحقل وقائمة دائمة مُعلَنة هنا — لا يُبنى عنصر دائم وقت التشغيل.
    //   • RULE-N125 (ترتيب الإرساء): tvAccounts (Fill) يُضاف أولاً، ثم pnlStatus (Bottom)،
    //     ثم gbSearch وgbCommands (Top) — فيُرسي آخر المُضاف (gbCommands) في الأعلى.
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        toolTip1 = new ToolTip(components);
        searchTimer = new System.Windows.Forms.Timer(components);
        cmsAccounts = new ContextMenuStrip(components);
        miAddChild = new ToolStripMenuItem();
        miEdit = new ToolStripMenuItem();
        miDelete = new ToolStripMenuItem();
        miDeletePermanent = new ToolStripMenuItem();
        miSeparator1 = new ToolStripSeparator();
        miRefresh = new ToolStripMenuItem();
        miExpandAll = new ToolStripMenuItem();
        miCollapseAll = new ToolStripMenuItem();
        gbCommands = new GroupBox();
        flpCommands = new FlowLayoutPanel();
        btnAddChild = new Button();
        btnEdit = new Button();
        btnDelete = new Button();
        btnDeletePermanent = new Button();
        btnRefresh = new Button();
        btnExpandAll = new Button();
        btnCollapseAll = new Button();
        gbSearch = new GroupBox();
        flpSearch = new FlowLayoutPanel();
        lblSearchBy = new Label();
        cbSearchBy = new ComboBox();
        txtSearch = new TextBox();
        lblSearchHint = new Label();
        tvAccounts = new TreeView();
        pnlStatus = new Panel();
        lblStatus = new Label();
        cmsAccounts.SuspendLayout();
        gbCommands.SuspendLayout();
        flpCommands.SuspendLayout();
        gbSearch.SuspendLayout();
        flpSearch.SuspendLayout();
        pnlStatus.SuspendLayout();
        SuspendLayout();
        // 
        // toolTip1
        // 
        toolTip1.AutoPopDelay = 8000;
        toolTip1.InitialDelay = 300;
        toolTip1.ReshowDelay = 100;
        // 
        // searchTimer
        // 
        searchTimer.Interval = 250;
        searchTimer.Tick += searchTimer_Tick;
        // 
        // cmsAccounts
        // 
        cmsAccounts.BackColor = System.Drawing.Color.White;
        cmsAccounts.Font = new Font("Tahoma", 10F);
        cmsAccounts.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        cmsAccounts.Items.AddRange(new ToolStripItem[] { miAddChild, miEdit, miDelete, miDeletePermanent, miSeparator1, miRefresh, miExpandAll, miCollapseAll });
        cmsAccounts.Name = "cmsAccounts";
        cmsAccounts.RightToLeft = RightToLeft.Yes;
        cmsAccounts.ShowCheckMargin = false;
        cmsAccounts.ShowImageMargin = false;
        cmsAccounts.Size = new Size(196, 214);
        // 
        // miAddChild
        // 
        miAddChild.Name = "miAddChild";
        miAddChild.Size = new Size(195, 26);
        miAddChild.Text = "إضافة حساب فرعي";
        miAddChild.Click += btnAddChild_Click;
        // 
        // miEdit
        // 
        miEdit.Name = "miEdit";
        miEdit.Size = new Size(195, 26);
        miEdit.Text = "تعديل";
        miEdit.Click += btnEdit_Click;
        // 
        // miDelete
        // 
        miDelete.Name = "miDelete";
        miDelete.Size = new Size(195, 26);
        miDelete.Text = "حذف";
        miDelete.Click += btnDelete_Click;
        // 
        // miDeletePermanent
        // 
        miDeletePermanent.Name = "miDeletePermanent";
        miDeletePermanent.Size = new Size(195, 26);
        miDeletePermanent.Text = "حذف نهائي";
        miDeletePermanent.Click += btnDeletePermanent_Click;
        // 
        // miSeparator1
        // 
        miSeparator1.Name = "miSeparator1";
        miSeparator1.Size = new Size(192, 6);
        // 
        // miRefresh
        // 
        miRefresh.Name = "miRefresh";
        miRefresh.Size = new Size(195, 26);
        miRefresh.Text = "تحديث";
        miRefresh.Click += btnRefresh_Click;
        // 
        // miExpandAll
        // 
        miExpandAll.Name = "miExpandAll";
        miExpandAll.Size = new Size(195, 26);
        miExpandAll.Text = "توسيع الكل";
        miExpandAll.Click += btnExpandAll_Click;
        // 
        // miCollapseAll
        // 
        miCollapseAll.Name = "miCollapseAll";
        miCollapseAll.Size = new Size(195, 26);
        miCollapseAll.Text = "طيّ الكل";
        miCollapseAll.Click += btnCollapseAll_Click;
        // 
        // gbCommands
        // 
        gbCommands.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(245), Convert.ToInt32(235));
        gbCommands.Controls.Add(flpCommands);
        gbCommands.Dock = DockStyle.Top;
        gbCommands.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        gbCommands.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
        gbCommands.Location = new Point(0, 0);
        gbCommands.Name = "gbCommands";
        gbCommands.Padding = new Padding(8, 6, 8, 6);
        gbCommands.Size = new Size(940, 64);
        gbCommands.TabIndex = 0;
        gbCommands.TabStop = false;
        gbCommands.Text = "أوامر الحسابات";
        // 
        // flpCommands
        // 
        flpCommands.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(245), Convert.ToInt32(235));
        flpCommands.Controls.Add(btnAddChild);
        flpCommands.Controls.Add(btnEdit);
        flpCommands.Controls.Add(btnDelete);
        flpCommands.Controls.Add(btnDeletePermanent);
        flpCommands.Controls.Add(btnRefresh);
        flpCommands.Controls.Add(btnExpandAll);
        flpCommands.Controls.Add(btnCollapseAll);
        flpCommands.Dock = DockStyle.Fill;
        flpCommands.FlowDirection = FlowDirection.RightToLeft;
        flpCommands.Location = new Point(8, 22);
        flpCommands.Name = "flpCommands";
        flpCommands.Size = new Size(924, 36);
        flpCommands.TabIndex = 0;
        flpCommands.WrapContents = false;
        // 
        // btnAddChild
        // 
        btnAddChild.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(25), Convert.ToInt32(135), Convert.ToInt32(84));
        btnAddChild.Cursor = Cursors.Hand;
        btnAddChild.FlatAppearance.BorderSize = 0;
        btnAddChild.FlatStyle = FlatStyle.Flat;
        btnAddChild.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnAddChild.ForeColor = System.Drawing.Color.White;
        btnAddChild.Location = new Point(770, 2);
        btnAddChild.Margin = new Padding(4, 2, 4, 2);
        btnAddChild.Name = "btnAddChild";
        btnAddChild.Size = new Size(150, 34);
        btnAddChild.TabIndex = 0;
        btnAddChild.Text = "إضافة حساب فرعي";
        toolTip1.SetToolTip(btnAddChild, "إضافة حساب فرعي تحت الحساب المحدد (F2 — تفتح شاشة دليل الحسابات)");
        btnAddChild.UseVisualStyleBackColor = false;
        btnAddChild.Click += btnAddChild_Click;
        // 
        // btnEdit
        // 
        btnEdit.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(255), Convert.ToInt32(193), Convert.ToInt32(7));
        btnEdit.Cursor = Cursors.Hand;
        btnEdit.FlatAppearance.BorderSize = 0;
        btnEdit.FlatStyle = FlatStyle.Flat;
        btnEdit.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnEdit.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(30), Convert.ToInt32(41), Convert.ToInt32(59));
        btnEdit.Location = new Point(666, 2);
        btnEdit.Margin = new Padding(4, 2, 4, 2);
        btnEdit.Name = "btnEdit";
        btnEdit.Size = new Size(100, 34);
        btnEdit.TabIndex = 1;
        btnEdit.Text = "تعديل";
        toolTip1.SetToolTip(btnEdit, "تعديل الحساب المحدد (تفتح شاشة دليل الحسابات)");
        btnEdit.UseVisualStyleBackColor = false;
        btnEdit.Click += btnEdit_Click;
        // 
        // btnDelete
        // 
        btnDelete.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(220), Convert.ToInt32(53), Convert.ToInt32(69));
        btnDelete.Cursor = Cursors.Hand;
        btnDelete.FlatAppearance.BorderSize = 0;
        btnDelete.FlatStyle = FlatStyle.Flat;
        btnDelete.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnDelete.ForeColor = System.Drawing.Color.White;
        btnDelete.Location = new Point(562, 2);
        btnDelete.Margin = new Padding(4, 2, 4, 2);
        btnDelete.Name = "btnDelete";
        btnDelete.Size = new Size(100, 34);
        btnDelete.TabIndex = 2;
        btnDelete.Text = "حذف";
        toolTip1.SetToolTip(btnDelete, "تعطيل الحساب المحدد بعد تأكيد");
        btnDelete.UseVisualStyleBackColor = false;
        btnDelete.Click += btnDelete_Click;
        // 
        // btnDeletePermanent
        // 
        btnDeletePermanent.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(220), Convert.ToInt32(53), Convert.ToInt32(69));
        btnDeletePermanent.Cursor = Cursors.Hand;
        btnDeletePermanent.FlatAppearance.BorderSize = 0;
        btnDeletePermanent.FlatStyle = FlatStyle.Flat;
        btnDeletePermanent.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnDeletePermanent.ForeColor = System.Drawing.Color.White;
        btnDeletePermanent.Location = new Point(438, 2);
        btnDeletePermanent.Margin = new Padding(4, 2, 4, 2);
        btnDeletePermanent.Name = "btnDeletePermanent";
        btnDeletePermanent.Size = new Size(120, 34);
        btnDeletePermanent.TabIndex = 3;
        btnDeletePermanent.Text = "حذف نهائي";
        toolTip1.SetToolTip(btnDeletePermanent, "حذف نهائي للحساب المحدد — غير النظامي وبلا حسابات فرعية فقط");
        btnDeletePermanent.UseVisualStyleBackColor = false;
        btnDeletePermanent.Click += btnDeletePermanent_Click;
        // 
        // btnRefresh
        // 
        btnRefresh.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(23), Convert.ToInt32(162), Convert.ToInt32(184));
        btnRefresh.Cursor = Cursors.Hand;
        btnRefresh.FlatAppearance.BorderSize = 0;
        btnRefresh.FlatStyle = FlatStyle.Flat;
        btnRefresh.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnRefresh.ForeColor = System.Drawing.Color.White;
        btnRefresh.Location = new Point(324, 2);
        btnRefresh.Margin = new Padding(4, 2, 4, 2);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(110, 34);
        btnRefresh.TabIndex = 4;
        btnRefresh.Text = "تحديث";
        toolTip1.SetToolTip(btnRefresh, "إعادة تحميل شجرة الحسابات (F5)");
        btnRefresh.UseVisualStyleBackColor = false;
        btnRefresh.Click += btnRefresh_Click;
        // 
        // btnExpandAll
        // 
        btnExpandAll.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(108), Convert.ToInt32(117), Convert.ToInt32(125));
        btnExpandAll.Cursor = Cursors.Hand;
        btnExpandAll.FlatAppearance.BorderSize = 0;
        btnExpandAll.FlatStyle = FlatStyle.Flat;
        btnExpandAll.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnExpandAll.ForeColor = System.Drawing.Color.White;
        btnExpandAll.Location = new Point(200, 2);
        btnExpandAll.Margin = new Padding(4, 2, 4, 2);
        btnExpandAll.Name = "btnExpandAll";
        btnExpandAll.Size = new Size(120, 34);
        btnExpandAll.TabIndex = 5;
        btnExpandAll.Text = "توسيع الكل";
        toolTip1.SetToolTip(btnExpandAll, "توسيع كل مجموعات الشجرة");
        btnExpandAll.UseVisualStyleBackColor = false;
        btnExpandAll.Click += btnExpandAll_Click;
        // 
        // btnCollapseAll
        // 
        btnCollapseAll.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(108), Convert.ToInt32(117), Convert.ToInt32(125));
        btnCollapseAll.Cursor = Cursors.Hand;
        btnCollapseAll.FlatAppearance.BorderSize = 0;
        btnCollapseAll.FlatStyle = FlatStyle.Flat;
        btnCollapseAll.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnCollapseAll.ForeColor = System.Drawing.Color.White;
        btnCollapseAll.Location = new Point(76, 2);
        btnCollapseAll.Margin = new Padding(4, 2, 4, 2);
        btnCollapseAll.Name = "btnCollapseAll";
        btnCollapseAll.Size = new Size(120, 34);
        btnCollapseAll.TabIndex = 6;
        btnCollapseAll.Text = "طيّ الكل";
        toolTip1.SetToolTip(btnCollapseAll, "طيّ كل مجموعات الشجرة");
        btnCollapseAll.UseVisualStyleBackColor = false;
        btnCollapseAll.Click += btnCollapseAll_Click;
        // 
        // gbSearch
        // 
        gbSearch.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(245), Convert.ToInt32(235));
        gbSearch.Controls.Add(flpSearch);
        gbSearch.Dock = DockStyle.Top;
        gbSearch.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        gbSearch.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
        gbSearch.Location = new Point(0, 64);
        gbSearch.Name = "gbSearch";
        gbSearch.Padding = new Padding(8, 6, 8, 6);
        gbSearch.Size = new Size(940, 64);
        gbSearch.TabIndex = 1;
        gbSearch.TabStop = false;
        gbSearch.Text = "بحث وتصفية";
        // 
        // flpSearch
        // 
        flpSearch.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(245), Convert.ToInt32(235));
        flpSearch.Controls.Add(lblSearchBy);
        flpSearch.Controls.Add(cbSearchBy);
        flpSearch.Controls.Add(txtSearch);
        flpSearch.Controls.Add(lblSearchHint);
        flpSearch.Dock = DockStyle.Fill;
        flpSearch.FlowDirection = FlowDirection.RightToLeft;
        flpSearch.Location = new Point(8, 22);
        flpSearch.Name = "flpSearch";
        flpSearch.Size = new Size(924, 36);
        flpSearch.TabIndex = 0;
        flpSearch.WrapContents = false;
        // 
        // lblSearchBy
        // 
        lblSearchBy.AutoSize = true;
        lblSearchBy.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblSearchBy.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
        lblSearchBy.Location = new Point(836, 11);
        lblSearchBy.Margin = new Padding(8, 9, 4, 3);
        lblSearchBy.Name = "lblSearchBy";
        lblSearchBy.Size = new Size(76, 20);
        lblSearchBy.TabIndex = 0;
        lblSearchBy.Text = "تصفية بـ:";
        lblSearchBy.TextAlign = ContentAlignment.MiddleRight;
        // 
        // cbSearchBy
        // 
        cbSearchBy.BackColor = System.Drawing.Color.White;
        cbSearchBy.DropDownStyle = ComboBoxStyle.DropDownList;
        cbSearchBy.FlatStyle = FlatStyle.Flat;
        cbSearchBy.Font = new Font("Tahoma", 10F);
        cbSearchBy.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        cbSearchBy.FormattingEnabled = true;
        cbSearchBy.Location = new Point(688, 8);
        cbSearchBy.Margin = new Padding(4, 6, 4, 3);
        cbSearchBy.Name = "cbSearchBy";
        cbSearchBy.RightToLeft = RightToLeft.Yes;
        cbSearchBy.Size = new Size(140, 28);
        cbSearchBy.TabIndex = 1;
        cbSearchBy.SelectedIndexChanged += cbSearchBy_SelectedIndexChanged;
        // 
        // txtSearch
        // 
        txtSearch.BackColor = System.Drawing.Color.White;
        txtSearch.BorderStyle = BorderStyle.FixedSingle;
        txtSearch.Font = new Font("Tahoma", 10F);
        txtSearch.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        txtSearch.Location = new Point(340, 8);
        txtSearch.Margin = new Padding(4, 7, 4, 3);
        txtSearch.Name = "txtSearch";
        txtSearch.RightToLeft = RightToLeft.Yes;
        txtSearch.Size = new Size(340, 27);
        txtSearch.TabIndex = 2;
        txtSearch.TextChanged += txtSearch_TextChanged;
        // 
        // lblSearchHint
        // 
        lblSearchHint.AutoSize = true;
        lblSearchHint.Font = new Font("Tahoma", 9F);
        lblSearchHint.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
        lblSearchHint.Location = new Point(87, 13);
        lblSearchHint.Margin = new Padding(8, 11, 4, 3);
        lblSearchHint.Name = "lblSearchHint";
        lblSearchHint.Size = new Size(241, 18);
        lblSearchHint.TabIndex = 3;
        lblSearchHint.Text = "اكتب للتصفية الفورية — Enter ينقل التركيز إلى الشجرة";
        lblSearchHint.TextAlign = ContentAlignment.MiddleRight;
        // 
        // tvAccounts
        // 
        tvAccounts.BackColor = System.Drawing.Color.White;
        tvAccounts.BorderStyle = BorderStyle.FixedSingle;
        tvAccounts.ContextMenuStrip = cmsAccounts;
        tvAccounts.Dock = DockStyle.Fill;
        tvAccounts.Font = new Font("Tahoma", 10F);
        tvAccounts.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        tvAccounts.FullRowSelect = true;
        tvAccounts.HideSelection = false;
        tvAccounts.ItemHeight = 26;
        tvAccounts.Location = new Point(0, 128);
        tvAccounts.Name = "tvAccounts";
        tvAccounts.ShowLines = true;
        tvAccounts.ShowRootLines = true;
        tvAccounts.Size = new Size(940, 482);
        tvAccounts.TabIndex = 2;
        tvAccounts.AfterSelect += tvAccounts_AfterSelect;
        // 
        // pnlStatus
        // 
        pnlStatus.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(245), Convert.ToInt32(235));
        pnlStatus.Controls.Add(lblStatus);
        pnlStatus.Dock = DockStyle.Bottom;
        pnlStatus.Location = new Point(0, 610);
        pnlStatus.Name = "pnlStatus";
        pnlStatus.Size = new Size(940, 30);
        pnlStatus.TabIndex = 3;
        // 
        // lblStatus
        // 
        lblStatus.Dock = DockStyle.Fill;
        lblStatus.Font = new Font("Tahoma", 9F);
        lblStatus.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
        lblStatus.Location = new Point(0, 0);
        lblStatus.Name = "lblStatus";
        lblStatus.Padding = new Padding(8, 0, 8, 0);
        lblStatus.Size = new Size(940, 30);
        lblStatus.TabIndex = 0;
        lblStatus.Text = "عدد الحسابات المعروضة: 0";
        lblStatus.TextAlign = ContentAlignment.MiddleRight;
        // 
        // ctrlTreeOfAccounts
        // 
        AutoScaleMode = AutoScaleMode.None;
        BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(170), Convert.ToInt32(220), Convert.ToInt32(170));
        Controls.Add(tvAccounts);
        Controls.Add(pnlStatus);
        Controls.Add(gbSearch);
        Controls.Add(gbCommands);
        Font = new Font("Tahoma", 10F);
        Name = "ctrlTreeOfAccounts";
        RightToLeft = RightToLeft.Yes;
        Size = new Size(940, 640);
        cmsAccounts.ResumeLayout(false);
        gbCommands.ResumeLayout(false);
        flpCommands.ResumeLayout(false);
        gbSearch.ResumeLayout(false);
        flpSearch.ResumeLayout(false);
        flpSearch.PerformLayout();
        pnlStatus.ResumeLayout(false);
        ResumeLayout(false);
    }

    private System.Windows.Forms.ToolTip toolTip1;
    private System.Windows.Forms.Timer searchTimer;
    private System.Windows.Forms.ContextMenuStrip cmsAccounts;
    private System.Windows.Forms.ToolStripMenuItem miAddChild;
    private System.Windows.Forms.ToolStripMenuItem miEdit;
    private System.Windows.Forms.ToolStripMenuItem miDelete;
    private System.Windows.Forms.ToolStripMenuItem miDeletePermanent;
    private System.Windows.Forms.ToolStripSeparator miSeparator1;
    private System.Windows.Forms.ToolStripMenuItem miRefresh;
    private System.Windows.Forms.ToolStripMenuItem miExpandAll;
    private System.Windows.Forms.ToolStripMenuItem miCollapseAll;
    private System.Windows.Forms.GroupBox gbCommands;
    private System.Windows.Forms.FlowLayoutPanel flpCommands;
    private System.Windows.Forms.Button btnAddChild;
    private System.Windows.Forms.Button btnEdit;
    private System.Windows.Forms.Button btnDelete;
    private System.Windows.Forms.Button btnDeletePermanent;
    private System.Windows.Forms.Button btnRefresh;
    private System.Windows.Forms.Button btnExpandAll;
    private System.Windows.Forms.Button btnCollapseAll;
    private System.Windows.Forms.GroupBox gbSearch;
    private System.Windows.Forms.FlowLayoutPanel flpSearch;
    private System.Windows.Forms.Label lblSearchBy;
    private System.Windows.Forms.ComboBox cbSearchBy;
    private System.Windows.Forms.TextBox txtSearch;
    private System.Windows.Forms.Label lblSearchHint;
    private System.Windows.Forms.TreeView tvAccounts;
    private System.Windows.Forms.Panel pnlStatus;
    private System.Windows.Forms.Label lblStatus;
}
