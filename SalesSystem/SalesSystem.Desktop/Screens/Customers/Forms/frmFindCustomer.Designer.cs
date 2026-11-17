namespace SalesSystem.Desktop.Screens.Customers.Forms;

partial class frmFindCustomer
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
    //   • ‏RULE-N125: ctrlCustomerCardWithFilter1 هو المراسي Fill ويُضاف أولاً، ثم pnlFooter (Bottom).
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        pnlFooter = new Panel();
        btnSelect = new Button();
        btnClose = new Button();
        ctrlCustomerCardWithFilter1 = new SalesSystem.Desktop.Screens.Customers.Controls.ctrlCustomerCardWithFilter();
        pnlFooter.SuspendLayout();
        SuspendLayout();
        // 
        // pnlFooter
        // 
        pnlFooter.BackColor = Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
        pnlFooter.Controls.Add(btnSelect);
        pnlFooter.Controls.Add(btnClose);
        pnlFooter.Dock = DockStyle.Bottom;
        pnlFooter.Location = new Point(0, 600);
        pnlFooter.Name = "pnlFooter";
        pnlFooter.Size = new Size(940, 60);
        pnlFooter.TabIndex = 1;
        // 
        // btnSelect
        // 
        btnSelect.BackColor = Color.FromArgb(Convert.ToInt32(25), Convert.ToInt32(135), Convert.ToInt32(84));
        btnSelect.Cursor = Cursors.Hand;
        btnSelect.DialogResult = DialogResult.OK;
        btnSelect.Enabled = false;
        btnSelect.FlatAppearance.BorderSize = 0;
        btnSelect.FlatStyle = FlatStyle.Flat;
        btnSelect.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnSelect.ForeColor = Color.White;
        btnSelect.Location = new Point(60, 12);
        btnSelect.Name = "btnSelect";
        btnSelect.Size = new Size(130, 36);
        btnSelect.TabIndex = 0;
        btnSelect.Text = "اختيار";
        btnSelect.UseVisualStyleBackColor = false;
        btnSelect.Click += btnSelect_Click;
        // 
        // btnClose
        // 
        btnClose.BackColor = Color.FromArgb(Convert.ToInt32(108), Convert.ToInt32(117), Convert.ToInt32(125));
        btnClose.Cursor = Cursors.Hand;
        btnClose.DialogResult = DialogResult.Cancel;
        btnClose.FlatAppearance.BorderSize = 0;
        btnClose.FlatStyle = FlatStyle.Flat;
        btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnClose.ForeColor = Color.White;
        btnClose.Location = new Point(205, 12);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(130, 36);
        btnClose.TabIndex = 1;
        btnClose.Text = "إغلاق";
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += btnClose_Click;
        // 
        // ctrlCustomerCardWithFilter1
        // 
        ctrlCustomerCardWithFilter1.Dock = DockStyle.Fill;
        ctrlCustomerCardWithFilter1.Location = new Point(0, 0);
        ctrlCustomerCardWithFilter1.Name = "ctrlCustomerCardWithFilter1";
        ctrlCustomerCardWithFilter1.Size = new Size(940, 600);
        ctrlCustomerCardWithFilter1.TabIndex = 0;
        // 
        // frmFindCustomer
        // 
        AcceptButton = btnSelect;
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.FromArgb(Convert.ToInt32(170), Convert.ToInt32(220), Convert.ToInt32(170));
        CancelButton = btnClose;
        ClientSize = new Size(940, 660);
        Controls.Add(ctrlCustomerCardWithFilter1);
        Controls.Add(pnlFooter);
        Font = new Font("Tahoma", 10F);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        KeyPreview = true;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "frmFindCustomer";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "البحث عن عميل";
        Shown += frmFindCustomer_Shown;
        pnlFooter.ResumeLayout(false);
        ResumeLayout(false);
    }

    private System.Windows.Forms.Panel pnlFooter;
    private System.Windows.Forms.Button btnSelect;
    private System.Windows.Forms.Button btnClose;
    private SalesSystem.Desktop.Screens.Customers.Controls.ctrlCustomerCardWithFilter ctrlCustomerCardWithFilter1;
}
