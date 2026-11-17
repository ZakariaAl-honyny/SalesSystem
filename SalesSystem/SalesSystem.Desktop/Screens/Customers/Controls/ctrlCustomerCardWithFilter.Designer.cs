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
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        gbFilters = new GroupBox();
        label1 = new Label();
        cbFilterBy = new ComboBox();
        txtFilterValue = new TextBox();
        btnFind = new Button();
        btnAddNew = new Button();
        ctrlCustomerCard1 = new ctrlCustomerCard();
        gbFilters.SuspendLayout();
        SuspendLayout();
        // 
        // gbFilters
        // 
        gbFilters.BackColor = Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(245), Convert.ToInt32(235));
        gbFilters.Controls.Add(label1);
        gbFilters.Controls.Add(cbFilterBy);
        gbFilters.Controls.Add(txtFilterValue);
        gbFilters.Controls.Add(btnFind);
        gbFilters.Controls.Add(btnAddNew);
        gbFilters.Dock = DockStyle.Top;
        gbFilters.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        gbFilters.ForeColor = Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
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
        label1.AutoSize = false;
        label1.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        label1.ForeColor = Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        label1.Location = new Point(45, 24);
        label1.Name = "label1";
        label1.Size = new Size(95, 26);
        label1.TabIndex = 0;
        label1.Text = "تصفية بـ:";
        label1.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // cbFilterBy
        // 
        cbFilterBy.BackColor = Color.White;
        cbFilterBy.DropDownStyle = ComboBoxStyle.DropDownList;
        cbFilterBy.FlatStyle = FlatStyle.Flat;
        cbFilterBy.Font = new Font("Tahoma", 10F);
        cbFilterBy.ForeColor = Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        cbFilterBy.FormattingEnabled = true;
        cbFilterBy.Location = new Point(148, 20);
        cbFilterBy.Name = "cbFilterBy";
        cbFilterBy.Size = new Size(146, 32);
        cbFilterBy.TabIndex = 1;
        cbFilterBy.SelectedIndexChanged += cbFilterBy_SelectedIndexChanged;
        // 
        // txtFilterValue
        // 
        txtFilterValue.BackColor = Color.White;
        txtFilterValue.BorderStyle = BorderStyle.FixedSingle;
        txtFilterValue.Font = new Font("Tahoma", 10F);
        txtFilterValue.ForeColor = Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        txtFilterValue.Location = new Point(304, 22);
        txtFilterValue.Name = "txtFilterValue";
        txtFilterValue.Size = new Size(250, 32);
        txtFilterValue.TabIndex = 2;
        txtFilterValue.KeyDown += txtFilterValue_KeyDown;
        // 
        // btnFind
        // 
        btnFind.BackColor = Color.FromArgb(Convert.ToInt32(23), Convert.ToInt32(162), Convert.ToInt32(184));
        btnFind.Cursor = Cursors.Hand;
        btnFind.FlatAppearance.BorderSize = 0;
        btnFind.FlatStyle = FlatStyle.Flat;
        btnFind.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnFind.ForeColor = Color.White;
        btnFind.Location = new Point(564, 20);
        btnFind.Name = "btnFind";
        btnFind.Size = new Size(100, 32);
        btnFind.TabIndex = 3;
        btnFind.Text = "بحث";
        btnFind.UseVisualStyleBackColor = false;
        btnFind.Click += btnFind_Click;
        // 
        // btnAddNew
        // 
        btnAddNew.BackColor = Color.FromArgb(Convert.ToInt32(13), Convert.ToInt32(110), Convert.ToInt32(253));
        btnAddNew.Cursor = Cursors.Hand;
        btnAddNew.FlatAppearance.BorderSize = 0;
        btnAddNew.FlatStyle = FlatStyle.Flat;
        btnAddNew.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnAddNew.ForeColor = Color.White;
        btnAddNew.Location = new Point(674, 20);
        btnAddNew.Name = "btnAddNew";
        btnAddNew.Size = new Size(100, 32);
        btnAddNew.TabIndex = 4;
        btnAddNew.Text = "جديد";
        btnAddNew.UseVisualStyleBackColor = false;
        btnAddNew.Click += btnAddNew_Click;
        // 
        // ctrlCustomerCard1
        // 
        ctrlCustomerCard1.Dock = DockStyle.Fill;
        ctrlCustomerCard1.Location = new Point(0, 78);
        ctrlCustomerCard1.Name = "ctrlCustomerCard1";
        ctrlCustomerCard1.Size = new Size(900, 482);
        ctrlCustomerCard1.TabIndex = 1;
        // 
        // ctrlCustomerCardWithFilter
        // 
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.FromArgb(Convert.ToInt32(170), Convert.ToInt32(220), Convert.ToInt32(170));
        Controls.Add(ctrlCustomerCard1);
        Controls.Add(gbFilters);
        Font = new Font("Tahoma", 10F);
        Name = "ctrlCustomerCardWithFilter";
        RightToLeft = RightToLeft.Yes;
        Size = new Size(900, 560);
        gbFilters.ResumeLayout(false);
        ResumeLayout(false);
    }

    private System.Windows.Forms.GroupBox gbFilters;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ComboBox cbFilterBy;
    private System.Windows.Forms.TextBox txtFilterValue;
    private System.Windows.Forms.Button btnFind;
    private System.Windows.Forms.Button btnAddNew;
    private ctrlCustomerCard ctrlCustomerCard1;
}
