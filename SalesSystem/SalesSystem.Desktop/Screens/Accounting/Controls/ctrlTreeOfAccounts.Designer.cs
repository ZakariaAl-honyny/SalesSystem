namespace SalesSystem.Desktop.Screens.Accounting.Controls;

partial class ctrlTreeOfAccounts
{
    private System.ComponentModel.IContainer components = null;

    // ملاحظات المصمّم:
    //   • المصمّم = التشغيل (RULE-N134 / RULE-N140 / RULE-N204): كل لون بصيغة Convert.ToInt32
    //     وبالقيمة الحرفية نفسها التي يطبّقها GreenThemeHelper وقت التشغيل — ولا مرجع لأي عضو
    //     في Helpers/ داخل هذا الملف (RULE-N128).
    //   • العنصر **عرض شجري فقط**: صندوق واحد بعنوان «شجرة الحسابات» يحتوي TreeView يملؤه.
    //     لا أزرار ولا تصفية ولا شريط ملخص — الأوامر ملك الشاشة الحاضنة (frmAccounts).
    private void InitializeComponent()
    {
        gbTree = new GroupBox();
        tvAccounts = new TreeView();
        gbTree.SuspendLayout();
        SuspendLayout();
        // 
        // gbTree
        // 
        gbTree.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(245), Convert.ToInt32(235));
        gbTree.Controls.Add(tvAccounts);
        gbTree.Dock = DockStyle.Fill;
        gbTree.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        gbTree.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
        gbTree.Location = new Point(0, 0);
        gbTree.Name = "gbTree";
        gbTree.Padding = new Padding(6);
        gbTree.Size = new Size(356, 350);
        gbTree.TabIndex = 0;
        gbTree.TabStop = false;
        gbTree.Text = "شجرة الحسابات";
        // 
        // tvAccounts
        // 
        tvAccounts.BackColor = System.Drawing.Color.White;
        tvAccounts.BorderStyle = BorderStyle.FixedSingle;
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
        tvAccounts.Size = new Size(344, 316);
        tvAccounts.TabIndex = 0;
        tvAccounts.AfterSelect += tvAccounts_AfterSelect;
        // 
        // ctrlTreeOfAccounts
        // 
        AutoScaleMode = AutoScaleMode.None;
        BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(170), Convert.ToInt32(220), Convert.ToInt32(170));
        Controls.Add(gbTree);
        Font = new Font("Tahoma", 10F);
        Name = "ctrlTreeOfAccounts";
        RightToLeft = RightToLeft.Yes;
        Size = new Size(356, 350);
        gbTree.ResumeLayout(false);
        ResumeLayout(false);
    }

    private System.Windows.Forms.GroupBox gbTree;
    private System.Windows.Forms.TreeView tvAccounts;
}
