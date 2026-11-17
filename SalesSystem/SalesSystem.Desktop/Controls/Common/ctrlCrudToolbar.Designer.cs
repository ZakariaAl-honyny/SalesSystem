namespace SalesSystem.Desktop.Controls.Common;

partial class ctrlCrudToolbar
{
    private System.ComponentModel.IContainer components = null;

    // ملاحظات المصمّم:
    //   • المصمّم = التشغيل (RULE-N134 / RULE-N140 / RULE-N204): كل لون بصيغة Convert.ToInt32
    //     وبالقيمة الحرفية نفسها التي تحتفظ بها اللوحة وقت التشغيل:
    //     خلفية العنصر = خلفية النموذج (170,220,170) · شريط الأزرار = (235,240,235)
    //     (BarBackground) — فيهبط GreenThemeHelper.Apply على القيمة نفسها بلا تغيير.
    //   • RULE-N145: الأزرار السبعة عناصر دائمة مُعلَنة هنا ولا تُبنى وقت التشغيل.
    //   • خلفية الشريط (235,240,235) لا (235,245,235): المصمّم يجب أن يطابق ما يضبطه
    //     Walk في GreenThemeHelper على الألواح (BarBackground) فلا يظهر فرق وقت التشغيل (RULE-N139).
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        toolTip1 = new ToolTip(components);
        flpButtons = new FlowLayoutPanel();
        btnAdd = new Button();
        btnEdit = new Button();
        btnSave = new Button();
        btnCancelEdit = new Button();
        btnDelete = new Button();
        btnRefresh = new Button();
        btnClose = new Button();
        flpButtons.SuspendLayout();
        SuspendLayout();
        // 
        // flpButtons
        // 
        flpButtons.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
        flpButtons.Controls.Add(btnAdd);
        flpButtons.Controls.Add(btnEdit);
        flpButtons.Controls.Add(btnSave);
        flpButtons.Controls.Add(btnCancelEdit);
        flpButtons.Controls.Add(btnDelete);
        flpButtons.Controls.Add(btnRefresh);
        flpButtons.Controls.Add(btnClose);
        flpButtons.Dock = DockStyle.Fill;
        flpButtons.FlowDirection = FlowDirection.RightToLeft;
        flpButtons.Location = new Point(0, 0);
        flpButtons.Name = "flpButtons";
        flpButtons.Padding = new Padding(2, 0, 2, 0);
        flpButtons.Size = new Size(960, 46);
        flpButtons.TabIndex = 0;
        flpButtons.WrapContents = false;
        // 
        // btnAdd
        // 
        btnAdd.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(25), Convert.ToInt32(135), Convert.ToInt32(84));
        btnAdd.Cursor = Cursors.Hand;
        btnAdd.FlatAppearance.BorderSize = 0;
        btnAdd.FlatStyle = FlatStyle.Flat;
        btnAdd.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnAdd.ForeColor = System.Drawing.Color.White;
        btnAdd.Location = new Point(804, 3);
        btnAdd.Margin = new Padding(4, 3, 0, 3);
        btnAdd.Name = "btnAdd";
        btnAdd.Size = new Size(130, 40);
        btnAdd.TabIndex = 0;
        btnAdd.Text = "+ إضافة";
        toolTip1.SetToolTip(btnAdd, "إضافة سجل جديد (F2)");
        btnAdd.UseVisualStyleBackColor = false;
        btnAdd.Click += btnAdd_Click;
        // 
        // btnEdit
        // 
        btnEdit.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(255), Convert.ToInt32(193), Convert.ToInt32(7));
        btnEdit.Cursor = Cursors.Hand;
        btnEdit.FlatAppearance.BorderSize = 0;
        btnEdit.FlatStyle = FlatStyle.Flat;
        btnEdit.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnEdit.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(30), Convert.ToInt32(41), Convert.ToInt32(59));
        btnEdit.Location = new Point(670, 3);
        btnEdit.Margin = new Padding(4, 3, 0, 3);
        btnEdit.Name = "btnEdit";
        btnEdit.Size = new Size(130, 40);
        btnEdit.TabIndex = 1;
        btnEdit.Text = "تعديل";
        toolTip1.SetToolTip(btnEdit, "تعديل السجل المحدد (F3)");
        btnEdit.UseVisualStyleBackColor = false;
        btnEdit.Click += btnEdit_Click;
        // 
        // btnSave
        // 
        btnSave.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(25), Convert.ToInt32(135), Convert.ToInt32(84));
        btnSave.Cursor = Cursors.Hand;
        btnSave.FlatAppearance.BorderSize = 0;
        btnSave.FlatStyle = FlatStyle.Flat;
        btnSave.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnSave.ForeColor = System.Drawing.Color.White;
        btnSave.Location = new Point(536, 3);
        btnSave.Margin = new Padding(4, 3, 0, 3);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(130, 40);
        btnSave.TabIndex = 2;
        btnSave.Text = "حفظ";
        toolTip1.SetToolTip(btnSave, "حفظ التغييرات في وضع الإضافة/التعديل");
        btnSave.UseVisualStyleBackColor = false;
        btnSave.Visible = false;
        btnSave.Click += btnSave_Click;
        // 
        // btnCancelEdit
        // 
        btnCancelEdit.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(108), Convert.ToInt32(117), Convert.ToInt32(125));
        btnCancelEdit.Cursor = Cursors.Hand;
        btnCancelEdit.FlatAppearance.BorderSize = 0;
        btnCancelEdit.FlatStyle = FlatStyle.Flat;
        btnCancelEdit.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnCancelEdit.ForeColor = System.Drawing.Color.White;
        btnCancelEdit.Location = new Point(402, 3);
        btnCancelEdit.Margin = new Padding(4, 3, 0, 3);
        btnCancelEdit.Name = "btnCancelEdit";
        btnCancelEdit.Size = new Size(130, 40);
        btnCancelEdit.TabIndex = 3;
        btnCancelEdit.Text = "إلغاء التعديل";
        toolTip1.SetToolTip(btnCancelEdit, "إلغاء الإضافة/التعديل والعودة إلى وضع العرض (Esc)");
        btnCancelEdit.UseVisualStyleBackColor = false;
        btnCancelEdit.Click += btnCancelEdit_Click;
        // 
        // btnDelete
        // 
        btnDelete.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(220), Convert.ToInt32(53), Convert.ToInt32(69));
        btnDelete.Cursor = Cursors.Hand;
        btnDelete.FlatAppearance.BorderSize = 0;
        btnDelete.FlatStyle = FlatStyle.Flat;
        btnDelete.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnDelete.ForeColor = System.Drawing.Color.White;
        btnDelete.Location = new Point(268, 3);
        btnDelete.Margin = new Padding(4, 3, 0, 3);
        btnDelete.Name = "btnDelete";
        btnDelete.Size = new Size(130, 40);
        btnDelete.TabIndex = 4;
        btnDelete.Text = "حذف";
        toolTip1.SetToolTip(btnDelete, "حذف/تعطيل السجل المحدد بعد تأكيد");
        btnDelete.UseVisualStyleBackColor = false;
        btnDelete.Click += btnDelete_Click;
        // 
        // btnRefresh
        // 
        btnRefresh.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(108), Convert.ToInt32(117), Convert.ToInt32(125));
        btnRefresh.Cursor = Cursors.Hand;
        btnRefresh.FlatAppearance.BorderSize = 0;
        btnRefresh.FlatStyle = FlatStyle.Flat;
        btnRefresh.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnRefresh.ForeColor = System.Drawing.Color.White;
        btnRefresh.Location = new Point(134, 3);
        btnRefresh.Margin = new Padding(4, 3, 0, 3);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(130, 40);
        btnRefresh.TabIndex = 5;
        btnRefresh.Text = "تحديث بيانات";
        toolTip1.SetToolTip(btnRefresh, "إعادة تحميل البيانات من الخدمة (F5)");
        btnRefresh.UseVisualStyleBackColor = false;
        btnRefresh.Click += btnRefresh_Click;
        // 
        // btnClose
        // 
        btnClose.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(108), Convert.ToInt32(117), Convert.ToInt32(125));
        btnClose.Cursor = Cursors.Hand;
        btnClose.FlatAppearance.BorderSize = 0;
        btnClose.FlatStyle = FlatStyle.Flat;
        btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnClose.ForeColor = System.Drawing.Color.White;
        btnClose.Location = new Point(0, 3);
        btnClose.Margin = new Padding(4, 3, 0, 3);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(130, 40);
        btnClose.TabIndex = 6;
        btnClose.Text = "خروج";
        toolTip1.SetToolTip(btnClose, "إغلاق الشاشة (Esc)");
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += btnClose_Click;
        // 
        // ctrlCrudToolbar
        // 
        AutoScaleMode = AutoScaleMode.None;
        BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(170), Convert.ToInt32(220), Convert.ToInt32(170));
        Controls.Add(flpButtons);
        Font = new Font("Tahoma", 10F);
        Name = "ctrlCrudToolbar";
        RightToLeft = RightToLeft.Yes;
        Size = new Size(960, 46);
        flpButtons.ResumeLayout(false);
        ResumeLayout(false);
    }

    private System.Windows.Forms.ToolTip toolTip1;
    private System.Windows.Forms.FlowLayoutPanel flpButtons;
    private System.Windows.Forms.Button btnAdd;
    private System.Windows.Forms.Button btnEdit;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.Button btnCancelEdit;
    private System.Windows.Forms.Button btnDelete;
    private System.Windows.Forms.Button btnRefresh;
    private System.Windows.Forms.Button btnClose;
}
