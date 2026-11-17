namespace SalesSystem.Desktop.Screens.Accounting.Controls;

partial class ctrlTreeOfAccounts
{
    private System.ComponentModel.IContainer components = null;

    // ملاحظات المصمّم:
    //   • المصمّم = التشغيل (RULE-N134 / RULE-N140 / RULE-N204): كل لون بصيغة Convert.ToInt32
    //     وبالقيمة الحرفية نفسها التي يطبّقها GreenThemeHelper وقت التشغيل — ولا مرجع لأي عضو
    //     في Helpers/ داخل هذا الملف (RULE-N128).
    //   • هيكل شاشة المرجع (frmAddUpdateSalesInvoice) من أعلى لأسفل، كل كتلة صندوق واحد بعنوان
    //     ولا صندوق داخل صندوق (RULE-N137):
    //       (1) gbCommands «أوامر الحسابات» — شريط CRUD المشترك + صف ثانوي خاص بالشجرة
    //       (2) gbSearch   «بحث وتصفية»
    //       (3) gbTree     «شجرة الحسابات»  (Fill)
    //       (4) pnlSummary «ملخص» (شريط سفلي، سطر واحد)
    //     شريط العنوان ليس هنا: يملكه النموذج الحاضن (frmAccountsTree).
    //   • RULE-N125 (ترتيب الإرساء): gbTree (Fill) يُضاف أولاً، ثم pnlSummary (Bottom)،
    //     ثم gbSearch وgbCommands (Top) — فيُرسي آخر المُضاف (gbCommands) في الأعلى.
    //   • داخل gbCommands: flpSecondary يُضاف أولاً ثم ctrlCrudToolbar1 — فيصعد شريط الأوامر
    //     فوق الصف الثانوي (نفس منطق ctrlDashboard: آخر Top يُضاف يُرسى أولاً).
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
        flpSecondary = new FlowLayoutPanel();
        btnExpandAll = new Button();
        btnCollapseAll = new Button();
        btnDeletePermanent = new Button();
        ctrlCrudToolbar1 = new SalesSystem.Desktop.Controls.Common.ctrlCrudToolbar();
        gbSearch = new GroupBox();
        flpSearch = new FlowLayoutPanel();
        lblSearchBy = new Label();
        cbSearchBy = new ComboBox();
        txtSearch = new TextBox();
        lblSearchHint = new Label();
        gbTree = new GroupBox();
        tvAccounts = new TreeView();
        pnlSummary = new Panel();
        lblSummary = new Label();
        cmsAccounts.SuspendLayout();
        gbCommands.SuspendLayout();
        flpSecondary.SuspendLayout();
        gbSearch.SuspendLayout();
        flpSearch.SuspendLayout();
        gbTree.SuspendLayout();
        pnlSummary.SuspendLayout();
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
        miAddChild.Click += CtrlToolbar_AddRequested;
        // 
        // miEdit
        // 
        miEdit.Name = "miEdit";
        miEdit.Size = new Size(195, 26);
        miEdit.Text = "تعديل";
        miEdit.Click += CtrlToolbar_EditRequested;
        // 
        // miDelete
        // 
        miDelete.Name = "miDelete";
        miDelete.Size = new Size(195, 26);
        miDelete.Text = "حذف";
        miDelete.Click += CtrlToolbar_DeleteRequested;
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
        miRefresh.Click += CtrlToolbar_RefreshRequested;
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
        gbCommands.Controls.Add(flpSecondary);
        gbCommands.Controls.Add(ctrlCrudToolbar1);
        gbCommands.Dock = DockStyle.Top;
        gbCommands.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        gbCommands.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
        gbCommands.Location = new Point(0, 0);
        gbCommands.Name = "gbCommands";
        gbCommands.Padding = new Padding(8, 6, 8, 6);
        gbCommands.Size = new Size(980, 128);
        gbCommands.TabIndex = 0;
        gbCommands.TabStop = false;
        gbCommands.Text = "أوامر الحسابات";
        // 
        // ctrlCrudToolbar1
        // 
        ctrlCrudToolbar1.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(170), Convert.ToInt32(220), Convert.ToInt32(170));
        ctrlCrudToolbar1.Dock = DockStyle.Top;
        ctrlCrudToolbar1.Font = new Font("Tahoma", 10F);
        ctrlCrudToolbar1.Location = new Point(8, 28);
        ctrlCrudToolbar1.Name = "ctrlCrudToolbar1";
        ctrlCrudToolbar1.RightToLeft = RightToLeft.Yes;
        ctrlCrudToolbar1.Size = new Size(964, 46);
        ctrlCrudToolbar1.TabIndex = 0;
        // 
        // flpSecondary
        // 
        flpSecondary.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(245), Convert.ToInt32(235));
        flpSecondary.Controls.Add(btnExpandAll);
        flpSecondary.Controls.Add(btnCollapseAll);
        flpSecondary.Controls.Add(btnDeletePermanent);
        flpSecondary.Dock = DockStyle.Top;
        flpSecondary.FlowDirection = FlowDirection.RightToLeft;
        flpSecondary.Location = new Point(8, 74);
        flpSecondary.Name = "flpSecondary";
        flpSecondary.Size = new Size(964, 46);
        flpSecondary.TabIndex = 1;
        flpSecondary.WrapContents = false;
        // 
        // btnExpandAll
        // 
        btnExpandAll.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(108), Convert.ToInt32(117), Convert.ToInt32(125));
        btnExpandAll.Cursor = Cursors.Hand;
        btnExpandAll.FlatAppearance.BorderSize = 0;
        btnExpandAll.FlatStyle = FlatStyle.Flat;
        btnExpandAll.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnExpandAll.ForeColor = System.Drawing.Color.White;
        btnExpandAll.Location = new Point(840, 3);
        btnExpandAll.Margin = new Padding(4, 3, 0, 3);
        btnExpandAll.Name = "btnExpandAll";
        btnExpandAll.Size = new Size(120, 40);
        btnExpandAll.TabIndex = 0;
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
        btnCollapseAll.Location = new Point(716, 3);
        btnCollapseAll.Margin = new Padding(4, 3, 0, 3);
        btnCollapseAll.Name = "btnCollapseAll";
        btnCollapseAll.Size = new Size(120, 40);
        btnCollapseAll.TabIndex = 1;
        btnCollapseAll.Text = "طيّ الكل";
        toolTip1.SetToolTip(btnCollapseAll, "طيّ كل مجموعات الشجرة");
        btnCollapseAll.UseVisualStyleBackColor = false;
        btnCollapseAll.Click += btnCollapseAll_Click;
        // 
        // btnDeletePermanent
        // 
        btnDeletePermanent.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(220), Convert.ToInt32(53), Convert.ToInt32(69));
        btnDeletePermanent.Cursor = Cursors.Hand;
        btnDeletePermanent.FlatAppearance.BorderSize = 0;
        btnDeletePermanent.FlatStyle = FlatStyle.Flat;
        btnDeletePermanent.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnDeletePermanent.ForeColor = System.Drawing.Color.White;
        btnDeletePermanent.Location = new Point(582, 3);
        btnDeletePermanent.Margin = new Padding(4, 3, 0, 3);
        btnDeletePermanent.Name = "btnDeletePermanent";
        btnDeletePermanent.Size = new Size(130, 40);
        btnDeletePermanent.TabIndex = 2;
        btnDeletePermanent.Text = "حذف نهائي";
        toolTip1.SetToolTip(btnDeletePermanent, "حذف نهائي للحساب المحدد — غير النظامي وبلا حسابات فرعية فقط");
        btnDeletePermanent.UseVisualStyleBackColor = false;
        btnDeletePermanent.Click += btnDeletePermanent_Click;
        // 
        // gbSearch
        // 
        gbSearch.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(245), Convert.ToInt32(235));
        gbSearch.Controls.Add(flpSearch);
        gbSearch.Dock = DockStyle.Top;
        gbSearch.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        gbSearch.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
        gbSearch.Location = new Point(0, 128);
        gbSearch.Name = "gbSearch";
        gbSearch.Padding = new Padding(8, 6, 8, 6);
        gbSearch.Size = new Size(980, 64);
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
        flpSearch.Size = new Size(964, 36);
        flpSearch.TabIndex = 0;
        flpSearch.WrapContents = false;
        // 
        // lblSearchBy
        // 
        lblSearchBy.AutoSize = true;
        lblSearchBy.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblSearchBy.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
        lblSearchBy.Location = new Point(876, 11);
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
        cbSearchBy.Location = new Point(728, 8);
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
        txtSearch.Location = new Point(380, 8);
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
        lblSearchHint.Location = new Point(127, 13);
        lblSearchHint.Margin = new Padding(8, 11, 4, 3);
        lblSearchHint.Name = "lblSearchHint";
        lblSearchHint.Size = new Size(241, 18);
        lblSearchHint.TabIndex = 3;
        lblSearchHint.Text = "اكتب للتصفية الفورية — Enter ينقل التركيز إلى الشجرة";
        lblSearchHint.TextAlign = ContentAlignment.MiddleRight;
        // 
        // gbTree
        // 
        gbTree.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(245), Convert.ToInt32(235));
        gbTree.Controls.Add(tvAccounts);
        gbTree.Dock = DockStyle.Fill;
        gbTree.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        gbTree.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
        gbTree.Location = new Point(0, 192);
        gbTree.Name = "gbTree";
        gbTree.Padding = new Padding(6);
        gbTree.Size = new Size(980, 434);
        gbTree.TabIndex = 2;
        gbTree.TabStop = false;
        gbTree.Text = "شجرة الحسابات";
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
        tvAccounts.Location = new Point(6, 28);
        tvAccounts.Name = "tvAccounts";
        tvAccounts.ShowLines = true;
        tvAccounts.ShowPlusMinus = true;
        tvAccounts.ShowRootLines = true;
        tvAccounts.Size = new Size(968, 400);
        tvAccounts.TabIndex = 0;
        tvAccounts.AfterSelect += tvAccounts_AfterSelect;
        // 
        // pnlSummary
        // 
        pnlSummary.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
        pnlSummary.Controls.Add(lblSummary);
        pnlSummary.Dock = DockStyle.Bottom;
        pnlSummary.Location = new Point(0, 626);
        pnlSummary.Name = "pnlSummary";
        pnlSummary.Size = new Size(980, 34);
        pnlSummary.TabIndex = 3;
        // 
        // lblSummary
        // 
        lblSummary.Dock = DockStyle.Fill;
        lblSummary.Font = new Font("Tahoma", 9F);
        lblSummary.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
        lblSummary.Location = new Point(0, 0);
        lblSummary.Name = "lblSummary";
        lblSummary.Padding = new Padding(8, 0, 8, 0);
        lblSummary.Size = new Size(980, 34);
        lblSummary.TabIndex = 0;
        lblSummary.Text = "عدد الحسابات المعروضة: 0";
        lblSummary.TextAlign = ContentAlignment.MiddleRight;
        // 
        // ctrlTreeOfAccounts
        // 
        AutoScaleMode = AutoScaleMode.None;
        BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(170), Convert.ToInt32(220), Convert.ToInt32(170));
        Controls.Add(gbTree);
        Controls.Add(pnlSummary);
        Controls.Add(gbSearch);
        Controls.Add(gbCommands);
        Font = new Font("Tahoma", 10F);
        Name = "ctrlTreeOfAccounts";
        RightToLeft = RightToLeft.Yes;
        Size = new Size(980, 660);
        cmsAccounts.ResumeLayout(false);
        gbCommands.ResumeLayout(false);
        flpSecondary.ResumeLayout(false);
        gbSearch.ResumeLayout(false);
        flpSearch.ResumeLayout(false);
        flpSearch.PerformLayout();
        gbTree.ResumeLayout(false);
        pnlSummary.ResumeLayout(false);
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
    private System.Windows.Forms.FlowLayoutPanel flpSecondary;
    private System.Windows.Forms.Button btnExpandAll;
    private System.Windows.Forms.Button btnCollapseAll;
    private System.Windows.Forms.Button btnDeletePermanent;
    private SalesSystem.Desktop.Controls.Common.ctrlCrudToolbar ctrlCrudToolbar1;
    private System.Windows.Forms.GroupBox gbSearch;
    private System.Windows.Forms.FlowLayoutPanel flpSearch;
    private System.Windows.Forms.Label lblSearchBy;
    private System.Windows.Forms.ComboBox cbSearchBy;
    private System.Windows.Forms.TextBox txtSearch;
    private System.Windows.Forms.Label lblSearchHint;
    private System.Windows.Forms.GroupBox gbTree;
    private System.Windows.Forms.TreeView tvAccounts;
    private System.Windows.Forms.Panel pnlSummary;
    private System.Windows.Forms.Label lblSummary;
}
