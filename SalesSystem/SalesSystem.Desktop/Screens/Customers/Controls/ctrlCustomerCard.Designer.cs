namespace SalesSystem.Desktop.Screens.Customers.Controls;

partial class ctrlCustomerCard
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
    //   • المصمّم = التشغيل (RULE-N134 / RULE-N140 / RULE-N204): كل لون معلن هنا بالقيمة الحرفية
    //     نفسها التي يطبّقها GreenThemeHelper وقت التشغيل، وبصيغة Convert.ToInt32.
    //   • لا يُعلَن أي لون حالة (نشط/معطّل/له حد ائتمان) — لون حالة يُطبَّق وقت التشغيل فقط
    //     (استثناء RULE-N134 الموثَّق في AGENTS.md §2.19 / §2.38).
    //   • ‏UserControl لا يملك RightToLeftLayout (خاصية Form فقط — مقيس بالانعكاس) فالاتجاه
    //     RTL يأتي من RightToLeft + من النموذج الحاضن.
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        toolTip1 = new ToolTip(components);
        gbIdentity = new GroupBox();
        lblIdCaption = new Label();
        lblIdValue = new Label();
        lblNameCaption = new Label();
        lblNameValue = new Label();
        lblPhoneCaption = new Label();
        lblPhoneValue = new Label();
        lblEmailCaption = new Label();
        lblEmailValue = new Label();
        lblAddressCaption = new Label();
        lblAddressValue = new Label();
        lblTaxNumberCaption = new Label();
        lblTaxNumberValue = new Label();
        lblCreditLimitCaption = new Label();
        lblCreditLimitValue = new Label();
        lblBalanceCaption = new Label();
        lblBalanceValue = new Label();
        lblAccountCaption = new Label();
        lblAccountValue = new Label();
        lblNotesCaption = new Label();
        lblNotesValue = new Label();
        gbStatus = new GroupBox();
        lblStatusCaption = new Label();
        lblStatusValue = new Label();
        lblCreditFlagCaption = new Label();
        lblCreditFlagValue = new Label();
        lnkEdit = new LinkLabel();
        gbIdentity.SuspendLayout();
        gbStatus.SuspendLayout();
        SuspendLayout();
        // 
        // gbIdentity
        // 
        gbIdentity.BackColor = Color.FromArgb(235, 245, 235);
        gbIdentity.Controls.Add(lblIdCaption);
        gbIdentity.Controls.Add(lblIdValue);
        gbIdentity.Controls.Add(lblNameCaption);
        gbIdentity.Controls.Add(lblNameValue);
        gbIdentity.Controls.Add(lblPhoneCaption);
        gbIdentity.Controls.Add(lblPhoneValue);
        gbIdentity.Controls.Add(lblEmailCaption);
        gbIdentity.Controls.Add(lblEmailValue);
        gbIdentity.Controls.Add(lblAddressCaption);
        gbIdentity.Controls.Add(lblAddressValue);
        gbIdentity.Controls.Add(lblTaxNumberCaption);
        gbIdentity.Controls.Add(lblTaxNumberValue);
        gbIdentity.Controls.Add(lblCreditLimitCaption);
        gbIdentity.Controls.Add(lblCreditLimitValue);
        gbIdentity.Controls.Add(lblBalanceCaption);
        gbIdentity.Controls.Add(lblBalanceValue);
        gbIdentity.Controls.Add(lblAccountCaption);
        gbIdentity.Controls.Add(lblAccountValue);
        gbIdentity.Controls.Add(lblNotesCaption);
        gbIdentity.Controls.Add(lblNotesValue);
        gbIdentity.Dock = DockStyle.Fill;
        gbIdentity.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        gbIdentity.ForeColor = Color.FromArgb(14, 82, 52);
        gbIdentity.Location = new Point(0, 78);
        gbIdentity.Name = "gbIdentity";
        gbIdentity.Padding = new Padding(8);
        gbIdentity.RightToLeft = RightToLeft.Yes;
        gbIdentity.Size = new Size(900, 392);
        gbIdentity.TabIndex = 0;
        gbIdentity.TabStop = false;
        gbIdentity.Text = "بيانات العميل";
        // 
        // lblIdCaption
        // 
        lblIdCaption.AutoSize = true;
        lblIdCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblIdCaption.ForeColor = Color.FromArgb(14, 82, 52);
        lblIdCaption.Location = new Point(717, 33);
        lblIdCaption.Name = "lblIdCaption";
        lblIdCaption.Size = new Size(113, 24);
        lblIdCaption.TabIndex = 0;
        lblIdCaption.Text = "رقم العميل";
        lblIdCaption.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblIdValue
        // 
        lblIdValue.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblIdValue.ForeColor = Color.FromArgb(33, 37, 41);
        lblIdValue.Location = new Point(471, 33);
        lblIdValue.Name = "lblIdValue";
        lblIdValue.Size = new Size(240, 26);
        lblIdValue.TabIndex = 1;
        lblIdValue.Text = "—";
        lblIdValue.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblNameCaption
        // 
        lblNameCaption.AutoSize = true;
        lblNameCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblNameCaption.ForeColor = Color.FromArgb(14, 82, 52);
        lblNameCaption.Location = new Point(717, 71);
        lblNameCaption.Name = "lblNameCaption";
        lblNameCaption.Size = new Size(67, 24);
        lblNameCaption.TabIndex = 2;
        lblNameCaption.Text = "الاسم";
        lblNameCaption.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblNameValue
        // 
        lblNameValue.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblNameValue.ForeColor = Color.FromArgb(33, 37, 41);
        lblNameValue.Location = new Point(471, 71);
        lblNameValue.Name = "lblNameValue";
        lblNameValue.Size = new Size(240, 26);
        lblNameValue.TabIndex = 3;
        lblNameValue.Text = "غير مُدخل";
        lblNameValue.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblPhoneCaption
        // 
        lblPhoneCaption.AutoSize = true;
        lblPhoneCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblPhoneCaption.ForeColor = Color.FromArgb(14, 82, 52);
        lblPhoneCaption.Location = new Point(717, 109);
        lblPhoneCaption.Name = "lblPhoneCaption";
        lblPhoneCaption.Size = new Size(70, 24);
        lblPhoneCaption.TabIndex = 4;
        lblPhoneCaption.Text = "الهاتف";
        lblPhoneCaption.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblPhoneValue
        // 
        lblPhoneValue.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblPhoneValue.ForeColor = Color.FromArgb(33, 37, 41);
        lblPhoneValue.Location = new Point(471, 109);
        lblPhoneValue.Name = "lblPhoneValue";
        lblPhoneValue.Size = new Size(240, 26);
        lblPhoneValue.TabIndex = 5;
        lblPhoneValue.Text = "غير مُدخل";
        lblPhoneValue.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblEmailCaption
        // 
        lblEmailCaption.AutoSize = true;
        lblEmailCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblEmailCaption.ForeColor = Color.FromArgb(14, 82, 52);
        lblEmailCaption.Location = new Point(717, 147);
        lblEmailCaption.Name = "lblEmailCaption";
        lblEmailCaption.Size = new Size(164, 24);
        lblEmailCaption.TabIndex = 6;
        lblEmailCaption.Text = "البريد الإلكتروني";
        lblEmailCaption.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblEmailValue
        // 
        lblEmailValue.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblEmailValue.ForeColor = Color.FromArgb(33, 37, 41);
        lblEmailValue.Location = new Point(471, 147);
        lblEmailValue.Name = "lblEmailValue";
        lblEmailValue.Size = new Size(240, 26);
        lblEmailValue.TabIndex = 7;
        lblEmailValue.Text = "غير مُدخل";
        lblEmailValue.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblAddressCaption
        // 
        lblAddressCaption.AutoSize = true;
        lblAddressCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblAddressCaption.ForeColor = Color.FromArgb(14, 82, 52);
        lblAddressCaption.Location = new Point(717, 185);
        lblAddressCaption.Name = "lblAddressCaption";
        lblAddressCaption.Size = new Size(76, 24);
        lblAddressCaption.TabIndex = 8;
        lblAddressCaption.Text = "العنوان";
        lblAddressCaption.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblAddressValue
        // 
        lblAddressValue.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblAddressValue.ForeColor = Color.FromArgb(33, 37, 41);
        lblAddressValue.Location = new Point(471, 185);
        lblAddressValue.Name = "lblAddressValue";
        lblAddressValue.Size = new Size(240, 26);
        lblAddressValue.TabIndex = 9;
        lblAddressValue.Text = "غير مُدخل";
        lblAddressValue.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblTaxNumberCaption
        // 
        lblTaxNumberCaption.AutoSize = true;
        lblTaxNumberCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblTaxNumberCaption.ForeColor = Color.FromArgb(14, 82, 52);
        lblTaxNumberCaption.Location = new Point(272, 52);
        lblTaxNumberCaption.Name = "lblTaxNumberCaption";
        lblTaxNumberCaption.Size = new Size(146, 24);
        lblTaxNumberCaption.TabIndex = 10;
        lblTaxNumberCaption.Text = "الرقم الضريبي";
        lblTaxNumberCaption.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblTaxNumberValue
        // 
        lblTaxNumberValue.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblTaxNumberValue.ForeColor = Color.FromArgb(33, 37, 41);
        lblTaxNumberValue.Location = new Point(26, 52);
        lblTaxNumberValue.Name = "lblTaxNumberValue";
        lblTaxNumberValue.Size = new Size(240, 26);
        lblTaxNumberValue.TabIndex = 11;
        lblTaxNumberValue.Text = "غير مُدخل";
        lblTaxNumberValue.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblCreditLimitCaption
        // 
        lblCreditLimitCaption.AutoSize = true;
        lblCreditLimitCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblCreditLimitCaption.ForeColor = Color.FromArgb(14, 82, 52);
        lblCreditLimitCaption.Location = new Point(272, 90);
        lblCreditLimitCaption.Name = "lblCreditLimitCaption";
        lblCreditLimitCaption.Size = new Size(114, 24);
        lblCreditLimitCaption.TabIndex = 12;
        lblCreditLimitCaption.Text = "حد الائتمان";
        lblCreditLimitCaption.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblCreditLimitValue
        // 
        lblCreditLimitValue.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblCreditLimitValue.ForeColor = Color.FromArgb(33, 37, 41);
        lblCreditLimitValue.Location = new Point(26, 90);
        lblCreditLimitValue.Name = "lblCreditLimitValue";
        lblCreditLimitValue.Size = new Size(240, 26);
        lblCreditLimitValue.TabIndex = 13;
        lblCreditLimitValue.Text = "غير مُدخل";
        lblCreditLimitValue.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblBalanceCaption
        // 
        lblBalanceCaption.AutoSize = true;
        lblBalanceCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblBalanceCaption.ForeColor = Color.FromArgb(14, 82, 52);
        lblBalanceCaption.Location = new Point(272, 128);
        lblBalanceCaption.Name = "lblBalanceCaption";
        lblBalanceCaption.Size = new Size(143, 24);
        lblBalanceCaption.TabIndex = 14;
        lblBalanceCaption.Text = "الرصيد الحالي";
        lblBalanceCaption.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblBalanceValue
        // 
        lblBalanceValue.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblBalanceValue.ForeColor = Color.FromArgb(33, 37, 41);
        lblBalanceValue.Location = new Point(26, 128);
        lblBalanceValue.Name = "lblBalanceValue";
        lblBalanceValue.Size = new Size(240, 26);
        lblBalanceValue.TabIndex = 15;
        lblBalanceValue.Text = "غير مُدخل";
        lblBalanceValue.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblAccountCaption
        // 
        lblAccountCaption.AutoSize = true;
        lblAccountCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblAccountCaption.ForeColor = Color.FromArgb(14, 82, 52);
        lblAccountCaption.Location = new Point(272, 166);
        lblAccountCaption.Name = "lblAccountCaption";
        lblAccountCaption.Size = new Size(194, 24);
        lblAccountCaption.TabIndex = 16;
        lblAccountCaption.Text = "الحساب المحاسبي";
        lblAccountCaption.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblAccountValue
        // 
        lblAccountValue.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblAccountValue.ForeColor = Color.FromArgb(33, 37, 41);
        lblAccountValue.Location = new Point(26, 166);
        lblAccountValue.Name = "lblAccountValue";
        lblAccountValue.Size = new Size(240, 26);
        lblAccountValue.TabIndex = 17;
        lblAccountValue.Text = "غير مُدخل";
        lblAccountValue.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblNotesCaption
        // 
        lblNotesCaption.AutoSize = true;
        lblNotesCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblNotesCaption.ForeColor = Color.FromArgb(14, 82, 52);
        lblNotesCaption.Location = new Point(717, 231);
        lblNotesCaption.Name = "lblNotesCaption";
        lblNotesCaption.Size = new Size(95, 24);
        lblNotesCaption.TabIndex = 18;
        lblNotesCaption.Text = "ملاحظات";
        lblNotesCaption.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblNotesValue
        // 
        lblNotesValue.AutoEllipsis = true;
        lblNotesValue.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblNotesValue.ForeColor = Color.FromArgb(33, 37, 41);
        lblNotesValue.Location = new Point(36, 228);
        lblNotesValue.Name = "lblNotesValue";
        lblNotesValue.Size = new Size(675, 26);
        lblNotesValue.TabIndex = 19;
        lblNotesValue.Text = "غير مُدخل";
        lblNotesValue.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // gbStatus
        // 
        gbStatus.BackColor = Color.FromArgb(235, 245, 235);
        gbStatus.Controls.Add(lblStatusCaption);
        gbStatus.Controls.Add(lblStatusValue);
        gbStatus.Controls.Add(lblCreditFlagCaption);
        gbStatus.Controls.Add(lblCreditFlagValue);
        gbStatus.Dock = DockStyle.Top;
        gbStatus.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        gbStatus.ForeColor = Color.FromArgb(14, 82, 52);
        gbStatus.Location = new Point(0, 0);
        gbStatus.Name = "gbStatus";
        gbStatus.Padding = new Padding(8);
        gbStatus.Size = new Size(900, 78);
        gbStatus.TabIndex = 1;
        gbStatus.TabStop = false;
        gbStatus.Text = "الحالة";
        // 
        // lblStatusCaption
        // 
        lblStatusCaption.AutoSize = true;
        lblStatusCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblStatusCaption.ForeColor = Color.FromArgb(14, 82, 52);
        lblStatusCaption.Location = new Point(674, 31);
        lblStatusCaption.Name = "lblStatusCaption";
        lblStatusCaption.Size = new Size(72, 24);
        lblStatusCaption.TabIndex = 0;
        lblStatusCaption.Text = "الحالة:";
        lblStatusCaption.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblStatusValue
        // 
        lblStatusValue.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblStatusValue.Location = new Point(528, 30);
        lblStatusValue.Name = "lblStatusValue";
        lblStatusValue.Size = new Size(140, 26);
        lblStatusValue.TabIndex = 1;
        lblStatusValue.Text = "—";
        lblStatusValue.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblCreditFlagCaption
        // 
        lblCreditFlagCaption.AutoSize = true;
        lblCreditFlagCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblCreditFlagCaption.ForeColor = Color.FromArgb(14, 82, 52);
        lblCreditFlagCaption.Location = new Point(377, 31);
        lblCreditFlagCaption.Name = "lblCreditFlagCaption";
        lblCreditFlagCaption.Size = new Size(133, 24);
        lblCreditFlagCaption.TabIndex = 2;
        lblCreditFlagCaption.Text = "له حد ائتمان:";
        lblCreditFlagCaption.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblCreditFlagValue
        // 
        lblCreditFlagValue.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblCreditFlagValue.Location = new Point(271, 30);
        lblCreditFlagValue.Name = "lblCreditFlagValue";
        lblCreditFlagValue.Size = new Size(100, 26);
        lblCreditFlagValue.TabIndex = 3;
        lblCreditFlagValue.Text = "—";
        lblCreditFlagValue.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lnkEdit
        // 
        lnkEdit.ActiveLinkColor = Color.FromArgb(14, 165, 103);
        lnkEdit.Dock = DockStyle.Bottom;
        lnkEdit.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lnkEdit.LinkColor = Color.FromArgb(14, 82, 52);
        lnkEdit.Location = new Point(0, 470);
        lnkEdit.Name = "lnkEdit";
        lnkEdit.Size = new Size(900, 40);
        lnkEdit.TabIndex = 2;
        lnkEdit.TabStop = true;
        lnkEdit.Text = "تعديل بيانات العميل";
        lnkEdit.TextAlign = ContentAlignment.MiddleCenter;
        lnkEdit.VisitedLinkColor = Color.FromArgb(14, 82, 52);
        lnkEdit.LinkClicked += lnkEdit_LinkClicked;
        // 
        // ctrlCustomerCard
        // 
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.FromArgb(170, 220, 170);
        Controls.Add(gbIdentity);
        Controls.Add(gbStatus);
        Controls.Add(lnkEdit);
        Font = new Font("Tahoma", 10F);
        Name = "ctrlCustomerCard";
        RightToLeft = RightToLeft.Yes;
        Size = new Size(900, 510);
        gbIdentity.ResumeLayout(false);
        gbIdentity.PerformLayout();
        gbStatus.ResumeLayout(false);
        gbStatus.PerformLayout();
        ResumeLayout(false);
    }

    private System.Windows.Forms.ToolTip toolTip1;
    private System.Windows.Forms.GroupBox gbIdentity;
    private System.Windows.Forms.Label lblIdCaption;
    private System.Windows.Forms.Label lblIdValue;
    private System.Windows.Forms.Label lblNameCaption;
    private System.Windows.Forms.Label lblNameValue;
    private System.Windows.Forms.Label lblPhoneCaption;
    private System.Windows.Forms.Label lblPhoneValue;
    private System.Windows.Forms.Label lblEmailCaption;
    private System.Windows.Forms.Label lblEmailValue;
    private System.Windows.Forms.Label lblAddressCaption;
    private System.Windows.Forms.Label lblAddressValue;
    private System.Windows.Forms.Label lblTaxNumberCaption;
    private System.Windows.Forms.Label lblTaxNumberValue;
    private System.Windows.Forms.Label lblCreditLimitCaption;
    private System.Windows.Forms.Label lblCreditLimitValue;
    private System.Windows.Forms.Label lblBalanceCaption;
    private System.Windows.Forms.Label lblBalanceValue;
    private System.Windows.Forms.Label lblAccountCaption;
    private System.Windows.Forms.Label lblAccountValue;
    private System.Windows.Forms.Label lblNotesCaption;
    private System.Windows.Forms.Label lblNotesValue;
    private System.Windows.Forms.GroupBox gbStatus;
    private System.Windows.Forms.Label lblStatusCaption;
    private System.Windows.Forms.Label lblStatusValue;
    private System.Windows.Forms.Label lblCreditFlagCaption;
    private System.Windows.Forms.Label lblCreditFlagValue;
    private System.Windows.Forms.LinkLabel lnkEdit;
}
