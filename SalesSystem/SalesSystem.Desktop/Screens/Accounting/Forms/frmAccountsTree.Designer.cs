namespace SalesSystem.Desktop.Screens.Accounting.Forms;

partial class frmAccountsTree
{
    private System.ComponentModel.IContainer components = null;

    // ملاحظات المصمّم:
    //   • المصمّم = التشغيل (RULE-N134 / RULE-N140 / RULE-N204): كل لون بصيغة Convert.ToInt32
    //     وبالقيمة الحرفية نفسها التي يطبّقها GreenThemeHelper (شريط العنوان 14,165,103 ·
    //     خلفية النموذج 170,220,170 · الزر المحايد 108,117,125).
    //   • RULE-N145: العنوان والشجرة وزر الإغلاق عناصر دائمة مُعلَنة هنا.
    //   • RULE-N125 (ترتيب الإرساء): الشجرة (Fill) تُضاف أولاً، ثم الإغلاق (Bottom)،
    //     ثم العنوان (Top) — فيُرسي العنوان أعلى الشاشة والإغلاق أسفلها.
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        toolTip = new ToolTip(components);
        lblTitle = new Label();
        ctrlTreeOfAccounts1 = new SalesSystem.Desktop.Screens.Accounting.Controls.ctrlTreeOfAccounts();
        btnClose = new Button();
        SuspendLayout();
        // 
        // lblTitle
        // 
        lblTitle.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
        lblTitle.Dock = DockStyle.Top;
        lblTitle.Font = new Font("Tahoma", 14F, FontStyle.Bold);
        lblTitle.ForeColor = System.Drawing.Color.White;
        lblTitle.Location = new Point(0, 0);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(1000, 46);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "(( شجرة دليل الحسابات ))";
        lblTitle.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // ctrlTreeOfAccounts1
        // 
        ctrlTreeOfAccounts1.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(170), Convert.ToInt32(220), Convert.ToInt32(170));
        ctrlTreeOfAccounts1.Dock = DockStyle.Fill;
        ctrlTreeOfAccounts1.Font = new Font("Tahoma", 10F);
        ctrlTreeOfAccounts1.Location = new Point(0, 46);
        ctrlTreeOfAccounts1.Name = "ctrlTreeOfAccounts1";
        ctrlTreeOfAccounts1.RightToLeft = RightToLeft.Yes;
        ctrlTreeOfAccounts1.Size = new Size(1000, 628);
        ctrlTreeOfAccounts1.TabIndex = 1;
        // 
        // btnClose
        // 
        btnClose.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(108), Convert.ToInt32(117), Convert.ToInt32(125));
        btnClose.Cursor = Cursors.Hand;
        btnClose.Dock = DockStyle.Bottom;
        btnClose.FlatAppearance.BorderSize = 0;
        btnClose.FlatStyle = FlatStyle.Flat;
        btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnClose.ForeColor = System.Drawing.Color.White;
        btnClose.Location = new Point(0, 674);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(1000, 46);
        btnClose.TabIndex = 2;
        btnClose.Text = "إغلاق";
        toolTip.SetToolTip(btnClose, "إغلاق الشاشة (Esc)");
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += btnClose_Click;
        // 
        // frmAccountsTree
        // 
        AutoScaleMode = AutoScaleMode.None;
        BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(170), Convert.ToInt32(220), Convert.ToInt32(170));
        CancelButton = btnClose;
        ClientSize = new Size(1000, 720);
        Controls.Add(ctrlTreeOfAccounts1);
        Controls.Add(btnClose);
        Controls.Add(lblTitle);
        Font = new Font("Tahoma", 10F);
        FormBorderStyle = FormBorderStyle.Sizable;
        MinimumSize = new Size(860, 580);
        Name = "frmAccountsTree";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "شجرة دليل الحسابات";
        Load += frmAccountsTree_Load;
        ResumeLayout(false);
    }

    private System.Windows.Forms.ToolTip toolTip;
    private System.Windows.Forms.Label lblTitle;
    private SalesSystem.Desktop.Screens.Accounting.Controls.ctrlTreeOfAccounts ctrlTreeOfAccounts1;
    private System.Windows.Forms.Button btnClose;
}
