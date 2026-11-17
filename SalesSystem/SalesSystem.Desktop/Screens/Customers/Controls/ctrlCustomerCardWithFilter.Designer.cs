namespace SalesSystem.Desktop.Screens.Customers.Controls;

partial class ctrlCustomerCardWithFilter
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
    //   • ‏RULE-N145: gbFilters وأدواته وزرّاه عناصر دائمة — مُعلنة هنا ولا تُبنى وقت التشغيل.
    //   • ‏RULE-N125: ctrlCustomerCard1 هو المراسي Fill ويُضاف أولاً، وgbFilters (Top) بعده.
    //   • قرار المالك 2026-11-17: صندوق بحث واحد باقتراحات (ComboBox) بدل «تصفية بـ + نص + زر بحث».
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        toolTip1 = new ToolTip(components);
        btnReload = new Button();
        gbFilters = new GroupBox();
        label1 = new Label();
        cbCustomer = new ComboBox();
        btnAddNew = new Button();
        ctrlCustomerCard1 = new ctrlCustomerCard();
        gbFilters.SuspendLayout();
        SuspendLayout();
        // 
        // btnReload
        // 
        btnReload.BackColor = Color.FromArgb(23, 162, 184);
        btnReload.Cursor = Cursors.Hand;
        btnReload.FlatAppearance.BorderSize = 0;
        btnReload.FlatStyle = FlatStyle.Flat;
        btnReload.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnReload.ForeColor = Color.White;
        btnReload.Location = new Point(634, 35);
        btnReload.Name = "btnReload";
        btnReload.Size = new Size(120, 32);
        btnReload.TabIndex = 2;
        btnReload.Text = "تحديث القائمة";
        toolTip1.SetToolTip(btnReload, "إعادة تحميل قائمة العملاء");
        btnReload.UseVisualStyleBackColor = false;
        btnReload.Click += btnReload_Click;
        // 
        // gbFilters
        // 
        gbFilters.BackColor = Color.FromArgb(235, 245, 235);
        gbFilters.Controls.Add(label1);
        gbFilters.Controls.Add(cbCustomer);
        gbFilters.Controls.Add(btnReload);
        gbFilters.Controls.Add(btnAddNew);
        gbFilters.Dock = DockStyle.Top;
        gbFilters.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        gbFilters.ForeColor = Color.FromArgb(14, 82, 52);
        gbFilters.Location = new Point(0, 0);
        gbFilters.Name = "gbFilters";
        gbFilters.Padding = new Padding(8);
        gbFilters.Size = new Size(900, 78);
        gbFilters.TabIndex = 0;
        gbFilters.TabStop = false;
        gbFilters.Text = "بحث وتصفية";
        // 
        // label1
        // 
        label1.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        label1.ForeColor = Color.FromArgb(14, 82, 52);
        label1.Location = new Point(483, 38);
        label1.Name = "label1";
        label1.Size = new Size(120, 26);
        label1.TabIndex = 0;
        label1.Text = "ابحث عن عميل:";
        label1.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // cbCustomer
        // 
        cbCustomer.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        cbCustomer.AutoCompleteSource = AutoCompleteSource.ListItems;
        cbCustomer.BackColor = Color.White;
        cbCustomer.FlatStyle = FlatStyle.Flat;
        cbCustomer.Font = new Font("Tahoma", 10F);
        cbCustomer.ForeColor = Color.FromArgb(33, 37, 41);
        cbCustomer.FormattingEnabled = true;
        cbCustomer.Location = new Point(157, 35);
        cbCustomer.MaxDropDownItems = 20;
        cbCustomer.Name = "cbCustomer";
        cbCustomer.RightToLeft = RightToLeft.Yes;
        cbCustomer.Size = new Size(320, 32);
        cbCustomer.TabIndex = 1;
        cbCustomer.SelectedIndexChanged += cbCustomer_SelectedIndexChanged;
        cbCustomer.KeyDown += cbCustomer_KeyDown;
        // 
        // btnAddNew
        // 
        btnAddNew.BackColor = Color.FromArgb(13, 110, 253);
        btnAddNew.Cursor = Cursors.Hand;
        btnAddNew.FlatAppearance.BorderSize = 0;
        btnAddNew.FlatStyle = FlatStyle.Flat;
        btnAddNew.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnAddNew.ForeColor = Color.White;
        btnAddNew.Location = new Point(764, 35);
        btnAddNew.Name = "btnAddNew";
        btnAddNew.Size = new Size(110, 32);
        btnAddNew.TabIndex = 3;
        btnAddNew.Text = "جديد";
        btnAddNew.UseVisualStyleBackColor = false;
        btnAddNew.Click += btnAddNew_Click;
        // 
        // ctrlCustomerCard1
        // 
        ctrlCustomerCard1.BackColor = Color.FromArgb(170, 220, 170);
        ctrlCustomerCard1.Dock = DockStyle.Fill;
        ctrlCustomerCard1.Font = new Font("Tahoma", 10F);
        ctrlCustomerCard1.Location = new Point(0, 78);
        ctrlCustomerCard1.Name = "ctrlCustomerCard1";
        ctrlCustomerCard1.RightToLeft = RightToLeft.Yes;
        ctrlCustomerCard1.Size = new Size(900, 482);
        ctrlCustomerCard1.TabIndex = 1;
        // 
        // ctrlCustomerCardWithFilter
        // 
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.FromArgb(170, 220, 170);
        Controls.Add(ctrlCustomerCard1);
        Controls.Add(gbFilters);
        Font = new Font("Tahoma", 10F);
        Name = "ctrlCustomerCardWithFilter";
        RightToLeft = RightToLeft.Yes;
        Size = new Size(900, 560);
        gbFilters.ResumeLayout(false);
        ResumeLayout(false);
    }

    private System.Windows.Forms.ToolTip toolTip1;
    private System.Windows.Forms.GroupBox gbFilters;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ComboBox cbCustomer;
    private System.Windows.Forms.Button btnReload;
    private System.Windows.Forms.Button btnAddNew;
    private ctrlCustomerCard ctrlCustomerCard1;
}
