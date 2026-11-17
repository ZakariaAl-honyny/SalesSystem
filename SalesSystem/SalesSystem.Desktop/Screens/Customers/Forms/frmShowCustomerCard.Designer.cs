namespace SalesSystem.Desktop.Screens.Customers.Forms;

partial class frmShowCustomerCard
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    // ملاحظات المصمّم:
    //   • المصمّم = التشغيل (RULE-N134 / RULE-N140 / RULE-N204): كل لون بالقيمة الحرفية نفسها
    //     التي يطبّقها GreenThemeHelper وقت التشغيل، وبصيغة Convert.ToInt32.
    //   • ‏RULE-N125: ctrlCustomerCard1 هو المراسي Fill ويُضاف أولاً، ثم pnlFooter (Bottom)
    //     ثم lblTitle (Top) — فيُرسى الشريط أولاً ويتقلّص الممتد فوقه.
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        lblTitle = new Label();
        pnlFooter = new Panel();
        btnEdit = new Button();
        btnClose = new Button();
        ctrlCustomerCard1 = new SalesSystem.Desktop.Screens.Customers.Controls.ctrlCustomerCard();
        pnlFooter.SuspendLayout();
        SuspendLayout();
        // 
        // lblTitle
        // 
        lblTitle.BackColor = Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
        lblTitle.Dock = DockStyle.Top;
        lblTitle.Font = new Font("Tahoma", 14F, FontStyle.Bold);
        lblTitle.ForeColor = Color.White;
        lblTitle.Location = new Point(0, 0);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(940, 50);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "(( بطاقة العميل ))";
        lblTitle.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlFooter
        // 
        pnlFooter.BackColor = Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
        pnlFooter.Controls.Add(btnEdit);
        pnlFooter.Controls.Add(btnClose);
        pnlFooter.Dock = DockStyle.Bottom;
        pnlFooter.Location = new Point(0, 560);
        pnlFooter.Name = "pnlFooter";
        pnlFooter.Size = new Size(940, 60);
        pnlFooter.TabIndex = 1;
        // 
        // btnEdit
        // 
        btnEdit.BackColor = Color.FromArgb(Convert.ToInt32(255), Convert.ToInt32(193), Convert.ToInt32(7));
        btnEdit.Cursor = Cursors.Hand;
        btnEdit.Enabled = false;
        btnEdit.FlatAppearance.BorderSize = 0;
        btnEdit.FlatStyle = FlatStyle.Flat;
        btnEdit.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnEdit.ForeColor = Color.FromArgb(Convert.ToInt32(30), Convert.ToInt32(41), Convert.ToInt32(59));
        btnEdit.Location = new Point(205, 12);
        btnEdit.Name = "btnEdit";
        btnEdit.Size = new Size(130, 36);
        btnEdit.TabIndex = 1;
        btnEdit.Text = "تعديل";
        btnEdit.UseVisualStyleBackColor = false;
        btnEdit.Click += btnEdit_Click;
        // 
        // btnClose
        // 
        btnClose.BackColor = Color.FromArgb(Convert.ToInt32(108), Convert.ToInt32(117), Convert.ToInt32(125));
        btnClose.Cursor = Cursors.Hand;
        btnClose.FlatAppearance.BorderSize = 0;
        btnClose.FlatStyle = FlatStyle.Flat;
        btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnClose.ForeColor = Color.White;
        btnClose.Location = new Point(60, 12);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(130, 36);
        btnClose.TabIndex = 0;
        btnClose.Text = "إغلاق";
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += btnClose_Click;
        // 
        // ctrlCustomerCard1
        // 
        ctrlCustomerCard1.Dock = DockStyle.Fill;
        ctrlCustomerCard1.Location = new Point(0, 50);
        ctrlCustomerCard1.Name = "ctrlCustomerCard1";
        ctrlCustomerCard1.Size = new Size(940, 510);
        ctrlCustomerCard1.TabIndex = 2;
        // 
        // frmShowCustomerCard
        // 
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.FromArgb(Convert.ToInt32(170), Convert.ToInt32(220), Convert.ToInt32(170));
        ClientSize = new Size(940, 620);
        Controls.Add(ctrlCustomerCard1);
        Controls.Add(pnlFooter);
        Controls.Add(lblTitle);
        Font = new Font("Tahoma", 10F);
        KeyPreview = true;
        MinimumSize = new Size(940, 660);
        Name = "frmShowCustomerCard";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "بطاقة العميل";
        pnlFooter.ResumeLayout(false);
        ResumeLayout(false);
    }

    private System.Windows.Forms.Label lblTitle;
    private System.Windows.Forms.Panel pnlFooter;
    private System.Windows.Forms.Button btnEdit;
    private System.Windows.Forms.Button btnClose;
    private SalesSystem.Desktop.Screens.Customers.Controls.ctrlCustomerCard ctrlCustomerCard1;
}
