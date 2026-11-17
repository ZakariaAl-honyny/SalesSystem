namespace SalesSystem.Desktop.Screens.Accounting.Forms;

partial class frmAccounts
{
    private System.ComponentModel.IContainer components = null;

    // RULE-N30: تنظيف المكوّنات (cmsGrid · toolTip · errorProvider) المملوكة للحاوية.
    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    // ملاحظات المصمّم:
    //   • المصمّم = التشغيل (RULE-N134 / RULE-N140 / RULE-N204): كل لون بصيغة Convert.ToInt32
    //     وبالقيمة الحيّة للوحة (الجيل الرابع — الأخضر): خلفية النموذج (170,220,170) ·
    //     شريط العنوان (14,165,103) · رؤوس الشبكة (14,165,103) · التحديد (255,255,180) ·
    //     الأسطر المتبادلة (245,252,245) · خطوط الشبكة (205,225,205) · النص المميّز (14,82,52).
    //     ولا مرجع لأي عضو في Helpers/ داخل هذا الملف (RULE-N128).
    //   • TASK-011 (قرار المالك): هيكل الشاشة **مطابق حرفياً** لتخطيط frmProductMaster —
    //     من أعلى لأسفل، كل كتلة بلا صندوق داخل صندوق (RULE-N137):
    //       (1) lblMode       «شريط العنوان»  — Label على مستوى النموذج (Dock=Top · 1432×50 ·
    //                          Tahoma 14 Bold)
    //       (2) pnlFields     «بيانات الحساب» — Panel (Dock=Top · 1432×244) يحمل حقول الحساب
    //                          الثلاثة أعمدة (يمين ← وسط ← يسار) + سطر صحّة البيانات
    //       (3) pnlButtons    «أوامر الحسابات» — FlowLayoutPanel (Dock=Top · 1432×66) يحمل
    //                          ctrlCrudToolbar المشترك (يميناً) + «حذف نهائي» (يساره)
    //       (4) pnlFilters    «بحث وتصفية»    — gbFilter (Fill) + زر الطيّ (140×104)
    //       (5) pnlBody       المنطقة الوسطى  — Panel (Dock=Fill) = pnlTreeHost (Left 360) |
    //                          pnlGrid (Fill · «بيانات الحسابات»)
    //       (6) pnlSummary    «الإجماليات»    — Panel (Dock=Bottom · 1432×32) + lblSummary (Fill)
    //   • RULE-N125 (ترتيب الإرساء): تُرتَّب الاستدعاءات Fill أولاً ثم Bottom ثم Top
    //     (الداخلي ← الخارجي) — لأن آخر ما يُضاف يُرسى أولاً:
    //       pnlBody(Fill) · pnlSummary(Bottom) · pnlFilters(Top) · pnlButtons(Top) ·
    //       pnlFields(Top) · lblMode(Top)  ← النتيجة: العنوان أعلى، ثم الحقول، ثم الأوامر،
    //       ثم التصفية، ثم [الشجرة | الشبكة]، ثم الملخص في القاع.
    //     وداخل pnlBody: pnlGrid يُضاف أولاً ثم pnlTreeHost ⇒ الشجرة تُرسى أولاً على اليسار
    //     (360) وpnlGrid يأخذ الباقي (RULE-N125).
    //   • pnlFields: Padding(10,6,10,4) · بلا BackColor صريح (Ambient مثل pnlFields في
    //     frmProductMaster — فيرث خلفية النموذج، RULE-N51a/RULE-N139) · RightToLeft=Yes.
    //     الأعمدة (من اليمين إلى اليسار، تسمية فوق الحقل، صف ≈ 52 بكسل):
    //       يمين (x=1150 · 270): الحساب الأب · اسم الحساب · الاسم الأجنبي · رقم الحساب
    //       وسط  (x=770  · 270): نوع الحساب · الحساب الختامي · نوع الحساب (رئيسي/فرعي) ·
    //                            [حساب نظامي | يسمح بمركز تكلفة] في صف واحد
    //       يسار (x=390  · 270): وصف · ملاحظات (متعدد الأسطر)
    //       صف سفلي: lblValidationHint (Dock=Bottom · 26 · Tahoma 9 · (176,42,55))
    //     والتثبيت: **كل** تسمية وحقل داخل pnlFields = Anchor Top|Right (بعرض ثابت يتبع الحد
    //     الأيمن) — لأن التخطيط كله مرساة يمينية في RTL (شريط فارغ يساراً كما في المرجع)،
    //     وTop|Left|Right على عمود عند x=1150 يُنتج عرضاً **سالباً** عند التصغير إلى
    //     MinimumSize(1100) (1100 − 12 − 1150 = −62) ⇒ Top|Right يحفظ الأعمدة والفجوات
    //     نفسها في كل عرض بلا تداخل وبلا عرض سالب.
    //   • RULE-N145/RULE-N183: errorProvider مُعلَن هنا (BlinkStyle = NeverBlink · ContainerControl = this)
    //     ويتبنّاه RequiredFieldHelper مع قاعدة «اسم الحساب مطلوب».
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
        lblMode = new Label();
        pnlFields = new Panel();
        lblParent = new Label();
        cbParent = new ComboBox();
        lblNameAr = new Label();
        txtNameAr = new TextBox();
        lblNameEn = new Label();
        txtNameEn = new TextBox();
        lblAccountCode = new Label();
        txtAccountCode = new TextBox();
        lblNature = new Label();
        cbNature = new ComboBox();
        lblNormalBalance = new Label();
        cbNormalBalance = new ComboBox();
        lblAccountType = new Label();
        cbAccountType = new ComboBox();
        chkIsSystem = new CheckBox();
        chkAllowCostCenter = new CheckBox();
        lblDescription = new Label();
        txtDescription = new TextBox();
        lblNotes = new Label();
        txtNotes = new TextBox();
        lblValidationHint = new Label();
        pnlButtons = new FlowLayoutPanel();
        ctrlCrudToolbar1 = new SalesSystem.Desktop.Controls.Common.ctrlCrudToolbar();
        btnPermanentDelete = new Button();
        pnlFilters = new Panel();
        gbFilter = new GroupBox();
        lblFilterBy = new Label();
        cbFilterBy = new ComboBox();
        txtFilterValue = new TextBox();
        lblNatureFilter = new Label();
        cbNatureFilter = new ComboBox();
        chkShowInactive = new CheckBox();
        chkOnlyLeaf = new CheckBox();
        chkOnlySystem = new CheckBox();
        chkOnlyParents = new CheckBox();
        btnClearFilters = new Button();
        btnToggleFilters = new Button();
        pnlBody = new Panel();
        pnlGrid = new GroupBox();
        lblEmptyState = new Label();
        dgvAccounts = new DataGridView();
        cmsGrid = new ContextMenuStrip(components);
        cmsEdit = new ToolStripMenuItem();
        cmsDelete = new ToolStripMenuItem();
        cmsRefresh = new ToolStripMenuItem();
        pnlTreeHost = new Panel();
        ctrlTreeOfAccounts1 = new SalesSystem.Desktop.Screens.Accounting.Controls.ctrlTreeOfAccounts();
        pnlSummary = new Panel();
        lblSummary = new Label();
        toolTip = new ToolTip(components);
        errorProvider = new ErrorProvider(components);
        pnlFields.SuspendLayout();
        pnlButtons.SuspendLayout();
        pnlFilters.SuspendLayout();
        gbFilter.SuspendLayout();
        pnlBody.SuspendLayout();
        pnlGrid.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvAccounts).BeginInit();
        cmsGrid.SuspendLayout();
        pnlTreeHost.SuspendLayout();
        pnlSummary.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
        SuspendLayout();
        // 
        // lblMode
        // 
        lblMode.BackColor = Color.FromArgb(14, 165, 103);
        lblMode.Dock = DockStyle.Top;
        lblMode.Font = new Font("Tahoma", 14F, FontStyle.Bold);
        lblMode.ForeColor = Color.White;
        lblMode.Location = new Point(0, 0);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(1432, 50);
        lblMode.TabIndex = 0;
        lblMode.Text = "دليل الحسابات — وضع العرض";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlFields
        // 
        pnlFields.Controls.Add(lblParent);
        pnlFields.Controls.Add(cbParent);
        pnlFields.Controls.Add(lblNameAr);
        pnlFields.Controls.Add(txtNameAr);
        pnlFields.Controls.Add(lblNameEn);
        pnlFields.Controls.Add(txtNameEn);
        pnlFields.Controls.Add(lblAccountCode);
        pnlFields.Controls.Add(txtAccountCode);
        pnlFields.Controls.Add(lblNature);
        pnlFields.Controls.Add(cbNature);
        pnlFields.Controls.Add(lblNormalBalance);
        pnlFields.Controls.Add(cbNormalBalance);
        pnlFields.Controls.Add(lblAccountType);
        pnlFields.Controls.Add(cbAccountType);
        pnlFields.Controls.Add(chkIsSystem);
        pnlFields.Controls.Add(chkAllowCostCenter);
        pnlFields.Controls.Add(lblDescription);
        pnlFields.Controls.Add(txtDescription);
        pnlFields.Controls.Add(lblNotes);
        pnlFields.Controls.Add(txtNotes);
        pnlFields.Controls.Add(lblValidationHint);
        pnlFields.Dock = DockStyle.Top;
        pnlFields.Location = new Point(0, 50);
        pnlFields.Name = "pnlFields";
        pnlFields.Padding = new Padding(10, 6, 10, 4);
        pnlFields.RightToLeft = RightToLeft.Yes;
        pnlFields.Size = new Size(1432, 244);
        pnlFields.TabIndex = 1;
        // 
        // lblParent
        // 
        lblParent.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblParent.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblParent.ForeColor = Color.FromArgb(14, 82, 52);
        lblParent.Location = new Point(1150, 6);
        lblParent.Name = "lblParent";
        lblParent.Size = new Size(270, 20);
        lblParent.TabIndex = 0;
        lblParent.Text = "الحساب الأب";
        lblParent.TextAlign = ContentAlignment.MiddleRight;
        // 
        // cbParent
        // 
        cbParent.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        cbParent.BackColor = Color.White;
        cbParent.DropDownStyle = ComboBoxStyle.DropDownList;
        cbParent.Font = new Font("Tahoma", 10F);
        cbParent.ForeColor = Color.FromArgb(33, 37, 41);
        cbParent.Location = new Point(1150, 26);
        cbParent.Name = "cbParent";
        cbParent.Size = new Size(270, 32);
        cbParent.TabIndex = 0;
        toolTip.SetToolTip(cbParent, "الحساب الأب — تُعرض الحسابات ذات الفروع النشطة فقط، ولا يظهر الحساب نفسه ولا فروعه");
        cbParent.SelectedIndexChanged += cbParent_SelectedIndexChanged;
        // 
        // lblNameAr
        // 
        lblNameAr.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblNameAr.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblNameAr.ForeColor = Color.FromArgb(14, 82, 52);
        lblNameAr.Location = new Point(1150, 58);
        lblNameAr.Name = "lblNameAr";
        lblNameAr.Size = new Size(270, 20);
        lblNameAr.TabIndex = 2;
        lblNameAr.Text = "اسم الحساب *";
        lblNameAr.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtNameAr
        // 
        txtNameAr.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        txtNameAr.BackColor = Color.White;
        txtNameAr.BorderStyle = BorderStyle.FixedSingle;
        txtNameAr.Font = new Font("Tahoma", 10F);
        txtNameAr.ForeColor = Color.FromArgb(33, 37, 41);
        txtNameAr.Location = new Point(1150, 78);
        txtNameAr.Name = "txtNameAr";
        txtNameAr.Size = new Size(270, 32);
        txtNameAr.TabIndex = 1;
        toolTip.SetToolTip(txtNameAr, "الاسم العربي للحساب — حقل مطلوب");
        // 
        // lblNameEn
        // 
        lblNameEn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblNameEn.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblNameEn.ForeColor = Color.FromArgb(14, 82, 52);
        lblNameEn.Location = new Point(1150, 110);
        lblNameEn.Name = "lblNameEn";
        lblNameEn.Size = new Size(270, 20);
        lblNameEn.TabIndex = 3;
        lblNameEn.Text = "الاسم الأجنبي";
        lblNameEn.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtNameEn
        // 
        txtNameEn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        txtNameEn.BackColor = Color.White;
        txtNameEn.BorderStyle = BorderStyle.FixedSingle;
        txtNameEn.Font = new Font("Tahoma", 10F);
        txtNameEn.ForeColor = Color.FromArgb(33, 37, 41);
        txtNameEn.Location = new Point(1150, 130);
        txtNameEn.Name = "txtNameEn";
        txtNameEn.Size = new Size(270, 32);
        txtNameEn.TabIndex = 2;
        // 
        // lblAccountCode
        // 
        lblAccountCode.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblAccountCode.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblAccountCode.ForeColor = Color.FromArgb(14, 82, 52);
        lblAccountCode.Location = new Point(1150, 162);
        lblAccountCode.Name = "lblAccountCode";
        lblAccountCode.Size = new Size(270, 20);
        lblAccountCode.TabIndex = 4;
        lblAccountCode.Text = "رقم الحساب";
        lblAccountCode.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtAccountCode
        // 
        txtAccountCode.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        txtAccountCode.BackColor = Color.White;
        txtAccountCode.BorderStyle = BorderStyle.FixedSingle;
        txtAccountCode.Font = new Font("Tahoma", 10F);
        txtAccountCode.ForeColor = Color.FromArgb(33, 37, 41);
        txtAccountCode.Location = new Point(1150, 182);
        txtAccountCode.Name = "txtAccountCode";
        txtAccountCode.ReadOnly = true;
        txtAccountCode.Size = new Size(270, 32);
        txtAccountCode.TabIndex = 3;
        txtAccountCode.TabStop = false;
        toolTip.SetToolTip(txtAccountCode, "رمز الحساب يُولّده النظام ولا يُعدَّل يدوياً — ويُعرض هنا كود المعاينة في وضع الإضافة");
        // 
        // lblNature
        // 
        lblNature.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblNature.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblNature.ForeColor = Color.FromArgb(14, 82, 52);
        lblNature.Location = new Point(770, 6);
        lblNature.Name = "lblNature";
        lblNature.Size = new Size(270, 20);
        lblNature.TabIndex = 5;
        lblNature.Text = "نوع الحساب";
        lblNature.TextAlign = ContentAlignment.MiddleRight;
        // 
        // cbNature
        // 
        cbNature.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        cbNature.BackColor = Color.White;
        cbNature.DropDownStyle = ComboBoxStyle.DropDownList;
        cbNature.Font = new Font("Tahoma", 10F);
        cbNature.ForeColor = Color.FromArgb(33, 37, 41);
        cbNature.Location = new Point(770, 26);
        cbNature.Name = "cbNature";
        cbNature.Size = new Size(270, 32);
        cbNature.TabIndex = 4;
        toolTip.SetToolTip(cbNature, "نوع الحساب: أصول · خصوم · حقوق ملكية · إيرادات · مصروفات");
        // 
        // lblNormalBalance
        // 
        lblNormalBalance.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblNormalBalance.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblNormalBalance.ForeColor = Color.FromArgb(14, 82, 52);
        lblNormalBalance.Location = new Point(770, 58);
        lblNormalBalance.Name = "lblNormalBalance";
        lblNormalBalance.Size = new Size(270, 20);
        lblNormalBalance.TabIndex = 6;
        lblNormalBalance.Text = "الحساب الختامي";
        lblNormalBalance.TextAlign = ContentAlignment.MiddleRight;
        // 
        // cbNormalBalance
        // 
        cbNormalBalance.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        cbNormalBalance.BackColor = Color.White;
        cbNormalBalance.DropDownStyle = ComboBoxStyle.DropDownList;
        cbNormalBalance.Font = new Font("Tahoma", 10F);
        cbNormalBalance.ForeColor = Color.FromArgb(33, 37, 41);
        cbNormalBalance.Location = new Point(770, 78);
        cbNormalBalance.Name = "cbNormalBalance";
        cbNormalBalance.Size = new Size(270, 32);
        cbNormalBalance.TabIndex = 5;
        toolTip.SetToolTip(cbNormalBalance, "طبيعة الرصيد — «تلقائي» تُشتق من نوع الحساب؛ اختر «مدين» أو «دائن» للحسابات المضادة");
        // 
        // lblAccountType
        // 
        lblAccountType.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblAccountType.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblAccountType.ForeColor = Color.FromArgb(14, 82, 52);
        lblAccountType.Location = new Point(770, 110);
        lblAccountType.Name = "lblAccountType";
        lblAccountType.Size = new Size(270, 20);
        lblAccountType.TabIndex = 7;
        lblAccountType.Text = "نوع الحساب";
        lblAccountType.TextAlign = ContentAlignment.MiddleRight;
        // 
        // cbAccountType
        // 
        cbAccountType.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        cbAccountType.BackColor = Color.White;
        cbAccountType.DropDownStyle = ComboBoxStyle.DropDownList;
        cbAccountType.Font = new Font("Tahoma", 10F);
        cbAccountType.ForeColor = Color.FromArgb(33, 37, 41);
        cbAccountType.Location = new Point(770, 130);
        cbAccountType.Name = "cbAccountType";
        cbAccountType.Size = new Size(270, 32);
        cbAccountType.TabIndex = 6;
        toolTip.SetToolTip(cbAccountType, "رئيسي = مجموعة يُفرَّع منها · فرعي = حساب حركة نهائي يُرحَّل عليه");
        cbAccountType.SelectedIndexChanged += cbAccountType_SelectedIndexChanged;
        // 
        // chkIsSystem
        // 
        chkIsSystem.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        chkIsSystem.Font = new Font("Tahoma", 10F);
        chkIsSystem.ForeColor = Color.FromArgb(33, 37, 41);
        chkIsSystem.Location = new Point(930, 182);
        chkIsSystem.Name = "chkIsSystem";
        chkIsSystem.Size = new Size(110, 28);
        chkIsSystem.TabIndex = 7;
        chkIsSystem.Text = "حساب نظامي";
        toolTip.SetToolTip(chkIsSystem, "الحسابات النظامية تُنشأ من بذور النظام ومزامنته فقط — العلم للعرض ولا يُغيَّر من الشاشة");
        chkIsSystem.UseVisualStyleBackColor = false;
        // 
        // chkAllowCostCenter
        // 
        chkAllowCostCenter.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        chkAllowCostCenter.Font = new Font("Tahoma", 10F);
        chkAllowCostCenter.ForeColor = Color.FromArgb(33, 37, 41);
        chkAllowCostCenter.Location = new Point(770, 182);
        chkAllowCostCenter.Name = "chkAllowCostCenter";
        chkAllowCostCenter.Size = new Size(150, 28);
        chkAllowCostCenter.TabIndex = 8;
        chkAllowCostCenter.Text = "يسمح بمركز تكلفة";
        toolTip.SetToolTip(chkAllowCostCenter, "يسمح بمركز التكلفة عند ترحيل القيود على هذا الحساب");
        chkAllowCostCenter.UseVisualStyleBackColor = false;
        // 
        // lblDescription
        // 
        lblDescription.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblDescription.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblDescription.ForeColor = Color.FromArgb(14, 82, 52);
        lblDescription.Location = new Point(390, 6);
        lblDescription.Name = "lblDescription";
        lblDescription.Size = new Size(270, 20);
        lblDescription.TabIndex = 8;
        lblDescription.Text = "وصف";
        lblDescription.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtDescription
        // 
        txtDescription.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        txtDescription.BackColor = Color.White;
        txtDescription.BorderStyle = BorderStyle.FixedSingle;
        txtDescription.Font = new Font("Tahoma", 10F);
        txtDescription.ForeColor = Color.FromArgb(33, 37, 41);
        txtDescription.Location = new Point(390, 26);
        txtDescription.Multiline = true;
        txtDescription.Name = "txtDescription";
        txtDescription.ScrollBars = ScrollBars.Vertical;
        txtDescription.Size = new Size(270, 56);
        txtDescription.TabIndex = 9;
        // 
        // lblNotes
        // 
        lblNotes.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblNotes.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblNotes.ForeColor = Color.FromArgb(14, 82, 52);
        lblNotes.Location = new Point(390, 94);
        lblNotes.Name = "lblNotes";
        lblNotes.Size = new Size(270, 20);
        lblNotes.TabIndex = 9;
        lblNotes.Text = "ملاحظات";
        lblNotes.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtNotes
        // 
        txtNotes.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        txtNotes.BackColor = Color.White;
        txtNotes.BorderStyle = BorderStyle.FixedSingle;
        txtNotes.Font = new Font("Tahoma", 10F);
        txtNotes.ForeColor = Color.FromArgb(33, 37, 41);
        txtNotes.Location = new Point(390, 114);
        txtNotes.Multiline = true;
        txtNotes.Name = "txtNotes";
        txtNotes.ScrollBars = ScrollBars.Vertical;
        txtNotes.Size = new Size(270, 56);
        txtNotes.TabIndex = 10;
        // 
        // lblValidationHint
        // 
        lblValidationHint.Dock = DockStyle.Bottom;
        lblValidationHint.Font = new Font("Tahoma", 9F);
        lblValidationHint.ForeColor = Color.FromArgb(176, 42, 55);
        lblValidationHint.Location = new Point(10, 214);
        lblValidationHint.Name = "lblValidationHint";
        lblValidationHint.Padding = new Padding(8, 0, 8, 0);
        lblValidationHint.Size = new Size(1412, 26);
        lblValidationHint.TabIndex = 10;
        lblValidationHint.TextAlign = ContentAlignment.MiddleRight;
        // 
        // pnlButtons
        // 
        pnlButtons.BackColor = Color.FromArgb(235, 240, 235);
        pnlButtons.Controls.Add(ctrlCrudToolbar1);
        pnlButtons.Controls.Add(btnPermanentDelete);
        pnlButtons.Dock = DockStyle.Top;
        pnlButtons.Location = new Point(0, 294);
        pnlButtons.Name = "pnlButtons";
        pnlButtons.Padding = new Padding(10);
        pnlButtons.Size = new Size(1432, 66);
        pnlButtons.TabIndex = 2;
        pnlButtons.WrapContents = false;
        // 
        // ctrlCrudToolbar1
        // 
        ctrlCrudToolbar1.BackColor = Color.FromArgb(170, 220, 170);
        ctrlCrudToolbar1.Font = new Font("Tahoma", 10F);
        ctrlCrudToolbar1.Location = new Point(512, 17);
        ctrlCrudToolbar1.Margin = new Padding(0, 7, 6, 7);
        ctrlCrudToolbar1.Name = "ctrlCrudToolbar1";
        ctrlCrudToolbar1.RightToLeft = RightToLeft.Yes;
        ctrlCrudToolbar1.Size = new Size(900, 46);
        ctrlCrudToolbar1.TabIndex = 0;
        // 
        // btnPermanentDelete
        // 
        btnPermanentDelete.BackColor = Color.FromArgb(220, 53, 69);
        btnPermanentDelete.Cursor = Cursors.Hand;
        btnPermanentDelete.FlatAppearance.BorderSize = 0;
        btnPermanentDelete.FlatStyle = FlatStyle.Flat;
        btnPermanentDelete.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnPermanentDelete.ForeColor = Color.White;
        btnPermanentDelete.Location = new Point(360, 17);
        btnPermanentDelete.Margin = new Padding(4, 7, 0, 0);
        btnPermanentDelete.Name = "btnPermanentDelete";
        btnPermanentDelete.Size = new Size(142, 35);
        btnPermanentDelete.TabIndex = 1;
        btnPermanentDelete.Text = "حذف نهائي";
        toolTip.SetToolTip(btnPermanentDelete, "حذف الحساب المحدد نهائياً — غير النظامي فقط، وبعد تأكيد صريح");
        btnPermanentDelete.UseVisualStyleBackColor = false;
        btnPermanentDelete.Click += btnPermanentDelete_Click;
        // 
        // pnlFilters
        // 
        pnlFilters.Controls.Add(gbFilter);
        pnlFilters.Controls.Add(btnToggleFilters);
        pnlFilters.Dock = DockStyle.Top;
        pnlFilters.Location = new Point(0, 360);
        pnlFilters.Margin = new Padding(0);
        pnlFilters.Name = "pnlFilters";
        pnlFilters.Padding = new Padding(6, 4, 6, 4);
        pnlFilters.Size = new Size(1432, 112);
        pnlFilters.TabIndex = 3;
        // 
        // gbFilter
        // 
        gbFilter.Controls.Add(lblFilterBy);
        gbFilter.Controls.Add(cbFilterBy);
        gbFilter.Controls.Add(txtFilterValue);
        gbFilter.Controls.Add(lblNatureFilter);
        gbFilter.Controls.Add(cbNatureFilter);
        gbFilter.Controls.Add(chkShowInactive);
        gbFilter.Controls.Add(chkOnlyLeaf);
        gbFilter.Controls.Add(chkOnlySystem);
        gbFilter.Controls.Add(chkOnlyParents);
        gbFilter.Controls.Add(btnClearFilters);
        gbFilter.Dock = DockStyle.Fill;
        gbFilter.Font = new Font("Tahoma", 10F);
        gbFilter.Location = new Point(146, 4);
        gbFilter.Margin = new Padding(0);
        gbFilter.Name = "gbFilter";
        gbFilter.Padding = new Padding(8);
        gbFilter.RightToLeft = RightToLeft.Yes;
        gbFilter.Size = new Size(1280, 104);
        gbFilter.TabIndex = 0;
        gbFilter.TabStop = false;
        gbFilter.Text = "بحث وتصفية";
        // 
        // lblFilterBy
        // 
        lblFilterBy.AutoSize = true;
        lblFilterBy.Location = new Point(993, 24);
        lblFilterBy.Name = "lblFilterBy";
        lblFilterBy.Size = new Size(83, 24);
        lblFilterBy.TabIndex = 0;
        lblFilterBy.Text = "تصفية بـ:";
        // 
        // cbFilterBy
        // 
        cbFilterBy.BackColor = Color.White;
        cbFilterBy.DropDownStyle = ComboBoxStyle.DropDownList;
        cbFilterBy.Font = new Font("Tahoma", 10F);
        cbFilterBy.Location = new Point(786, 21);
        cbFilterBy.Name = "cbFilterBy";
        cbFilterBy.Size = new Size(200, 32);
        cbFilterBy.TabIndex = 1;
        cbFilterBy.SelectedIndexChanged += cbFilterBy_SelectedIndexChanged;
        // 
        // txtFilterValue
        // 
        txtFilterValue.BackColor = Color.White;
        txtFilterValue.Font = new Font("Tahoma", 10F);
        txtFilterValue.Location = new Point(540, 21);
        txtFilterValue.Name = "txtFilterValue";
        txtFilterValue.PlaceholderText = "اكتب نص البحث...";
        txtFilterValue.Size = new Size(240, 32);
        txtFilterValue.TabIndex = 2;
        toolTip.SetToolTip(txtFilterValue, "بحث فوري — Enter يطبّق وEsc يمسح كل التصفية");
        txtFilterValue.TextChanged += txtFilterValue_TextChanged;
        txtFilterValue.KeyDown += txtFilterValue_KeyDown;
        // 
        // lblNatureFilter
        // 
        lblNatureFilter.AutoSize = true;
        lblNatureFilter.Location = new Point(478, 25);
        lblNatureFilter.Name = "lblNatureFilter";
        lblNatureFilter.Size = new Size(56, 24);
        lblNatureFilter.TabIndex = 3;
        lblNatureFilter.Text = "النوع:";
        // 
        // cbNatureFilter
        // 
        cbNatureFilter.BackColor = Color.White;
        cbNatureFilter.DropDownStyle = ComboBoxStyle.DropDownList;
        cbNatureFilter.Font = new Font("Tahoma", 10F);
        cbNatureFilter.Location = new Point(272, 21);
        cbNatureFilter.Name = "cbNatureFilter";
        cbNatureFilter.Size = new Size(200, 32);
        cbNatureFilter.TabIndex = 4;
        cbNatureFilter.SelectedIndexChanged += cbNatureFilter_SelectedIndexChanged;
        // 
        // chkShowInactive
        // 
        chkShowInactive.AutoSize = true;
        chkShowInactive.Location = new Point(163, 61);
        chkShowInactive.Name = "chkShowInactive";
        chkShowInactive.Size = new Size(170, 28);
        chkShowInactive.TabIndex = 5;
        chkShowInactive.Text = "إظهار غير النشط";
        chkShowInactive.UseVisualStyleBackColor = false;
        chkShowInactive.CheckedChanged += chkShowInactive_CheckedChanged;
        // 
        // chkOnlyLeaf
        // 
        chkOnlyLeaf.AutoSize = true;
        chkOnlyLeaf.Location = new Point(339, 61);
        chkOnlyLeaf.Name = "chkOnlyLeaf";
        chkOnlyLeaf.Size = new Size(219, 28);
        chkOnlyLeaf.TabIndex = 6;
        chkOnlyLeaf.Text = "الحسابات الفرعية فقط";
        chkOnlyLeaf.UseVisualStyleBackColor = false;
        chkOnlyLeaf.CheckedChanged += chkOnlyLeaf_CheckedChanged;
        // 
        // chkOnlySystem
        // 
        chkOnlySystem.AutoSize = true;
        chkOnlySystem.Location = new Point(564, 61);
        chkOnlySystem.Name = "chkOnlySystem";
        chkOnlySystem.Size = new Size(198, 28);
        chkOnlySystem.TabIndex = 7;
        chkOnlySystem.Text = "حسابات النظام فقط";
        chkOnlySystem.UseVisualStyleBackColor = false;
        chkOnlySystem.CheckedChanged += chkOnlySystem_CheckedChanged;
        // 
        // chkOnlyParents
        // 
        chkOnlyParents.AutoSize = true;
        chkOnlyParents.Location = new Point(768, 61);
        chkOnlyParents.Name = "chkOnlyParents";
        chkOnlyParents.Size = new Size(231, 28);
        chkOnlyParents.TabIndex = 8;
        chkOnlyParents.Text = "الحسابات الرئيسية فقط";
        chkOnlyParents.UseVisualStyleBackColor = false;
        chkOnlyParents.CheckedChanged += chkOnlyParents_CheckedChanged;
        // 
        // btnClearFilters
        // 
        btnClearFilters.BackColor = Color.FromArgb(108, 117, 125);
        btnClearFilters.Cursor = Cursors.Hand;
        btnClearFilters.Enabled = false;
        btnClearFilters.FlatAppearance.BorderSize = 0;
        btnClearFilters.FlatStyle = FlatStyle.Flat;
        btnClearFilters.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        btnClearFilters.ForeColor = Color.White;
        btnClearFilters.Location = new Point(77, 21);
        btnClearFilters.Name = "btnClearFilters";
        btnClearFilters.Size = new Size(184, 32);
        btnClearFilters.TabIndex = 21;
        btnClearFilters.Text = "\U0001f9f9 مسح التصفية";
        toolTip.SetToolTip(btnClearFilters, "مسح كل أدوات التصفية (Esc) — لا يعيد تحميل البيانات");
        btnClearFilters.UseVisualStyleBackColor = false;
        btnClearFilters.Click += btnClearFilters_Click;
        // 
        // btnToggleFilters
        // 
        btnToggleFilters.BackColor = Color.FromArgb(248, 249, 250);
        btnToggleFilters.Cursor = Cursors.Hand;
        btnToggleFilters.Dock = DockStyle.Left;
        btnToggleFilters.FlatAppearance.BorderColor = Color.FromArgb(205, 225, 205);
        btnToggleFilters.FlatAppearance.MouseOverBackColor = Color.FromArgb(240, 240, 240);
        btnToggleFilters.FlatStyle = FlatStyle.Popup;
        btnToggleFilters.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        btnToggleFilters.ForeColor = Color.FromArgb(14, 82, 52);
        btnToggleFilters.Location = new Point(6, 4);
        btnToggleFilters.Name = "btnToggleFilters";
        btnToggleFilters.Size = new Size(140, 104);
        btnToggleFilters.TabIndex = 20;
        btnToggleFilters.Text = "إخفاء التصفية";
        btnToggleFilters.UseVisualStyleBackColor = false;
        // 
        // pnlBody
        // 
        pnlBody.BackColor = Color.FromArgb(235, 240, 235);
        pnlBody.Controls.Add(pnlGrid);
        pnlBody.Controls.Add(pnlTreeHost);
        pnlBody.Dock = DockStyle.Fill;
        pnlBody.Location = new Point(0, 472);
        pnlBody.Name = "pnlBody";
        pnlBody.Size = new Size(1432, 396);
        pnlBody.TabIndex = 4;
        // 
        // pnlGrid
        // 
        pnlGrid.BackColor = Color.White;
        pnlGrid.Controls.Add(lblEmptyState);
        pnlGrid.Controls.Add(dgvAccounts);
        pnlGrid.Dock = DockStyle.Fill;
        pnlGrid.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        pnlGrid.Location = new Point(360, 0);
        pnlGrid.Name = "pnlGrid";
        pnlGrid.Padding = new Padding(8);
        pnlGrid.Size = new Size(1072, 396);
        pnlGrid.TabIndex = 0;
        pnlGrid.TabStop = false;
        pnlGrid.Text = "بيانات الحسابات";
        // 
        // lblEmptyState
        // 
        lblEmptyState.BackColor = Color.White;
        lblEmptyState.Dock = DockStyle.Fill;
        lblEmptyState.Font = new Font("Tahoma", 12F, FontStyle.Bold);
        lblEmptyState.ForeColor = Color.FromArgb(14, 82, 52);
        lblEmptyState.Location = new Point(8, 33);
        lblEmptyState.Name = "lblEmptyState";
        lblEmptyState.Size = new Size(1056, 355);
        lblEmptyState.TabIndex = 1;
        lblEmptyState.Text = "لا توجد بيانات";
        lblEmptyState.TextAlign = ContentAlignment.MiddleCenter;
        lblEmptyState.Visible = false;
        // 
        // dgvAccounts
        // 
        dgvAccounts.AllowUserToAddRows = false;
        dgvAccounts.AllowUserToDeleteRows = false;
        dgvAccounts.AllowUserToOrderColumns = true;
        dataGridViewCellStyle1.BackColor = Color.FromArgb(245, 252, 245);
        dgvAccounts.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
        dgvAccounts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvAccounts.BackgroundColor = Color.FromArgb(220, 240, 220);
        dgvAccounts.BorderStyle = BorderStyle.None;
        dgvAccounts.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
        dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
        dataGridViewCellStyle2.BackColor = Color.FromArgb(14, 165, 103);
        dataGridViewCellStyle2.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        dataGridViewCellStyle2.ForeColor = Color.White;
        dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(14, 165, 103);
        dataGridViewCellStyle2.SelectionForeColor = Color.White;
        dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
        dgvAccounts.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
        dgvAccounts.ColumnHeadersHeight = 45;
        dgvAccounts.ContextMenuStrip = cmsGrid;
        dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle3.BackColor = Color.White;
        dataGridViewCellStyle3.Font = new Font("Tahoma", 10F);
        dataGridViewCellStyle3.ForeColor = Color.FromArgb(33, 37, 41);
        dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(255, 255, 180);
        dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(33, 37, 41);
        dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
        dgvAccounts.DefaultCellStyle = dataGridViewCellStyle3;
        dgvAccounts.Dock = DockStyle.Fill;
        dgvAccounts.EnableHeadersVisualStyles = false;
        dgvAccounts.GridColor = Color.FromArgb(205, 225, 205);
        dgvAccounts.Location = new Point(8, 33);
        dgvAccounts.MultiSelect = false;
        dgvAccounts.Name = "dgvAccounts";
        dgvAccounts.ReadOnly = true;
        dgvAccounts.RowHeadersVisible = false;
        dgvAccounts.RowHeadersWidth = 62;
        dgvAccounts.RowTemplate.Height = 32;
        dgvAccounts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvAccounts.Size = new Size(1056, 355);
        dgvAccounts.TabIndex = 0;
        dgvAccounts.CellDoubleClick += dgvAccounts_CellDoubleClick;
        dgvAccounts.CellFormatting += dgvAccounts_CellFormatting;
        dgvAccounts.SelectionChanged += dgvAccounts_SelectionChanged;
        // 
        // cmsGrid
        // 
        cmsGrid.Font = new Font("Tahoma", 10F);
        cmsGrid.ImageScalingSize = new Size(24, 24);
        cmsGrid.Items.AddRange(new ToolStripItem[] { cmsEdit, cmsDelete, cmsRefresh });
        cmsGrid.Name = "cmsGrid";
        cmsGrid.RightToLeft = RightToLeft.Yes;
        cmsGrid.Size = new Size(137, 94);
        // 
        // cmsEdit
        // 
        cmsEdit.Name = "cmsEdit";
        cmsEdit.Size = new Size(136, 30);
        cmsEdit.Text = "تعديل";
        cmsEdit.Click += cmsEdit_Click;
        // 
        // cmsDelete
        // 
        cmsDelete.Name = "cmsDelete";
        cmsDelete.Size = new Size(136, 30);
        cmsDelete.Text = "تعطيل";
        cmsDelete.Click += cmsDelete_Click;
        // 
        // cmsRefresh
        // 
        cmsRefresh.Name = "cmsRefresh";
        cmsRefresh.Size = new Size(136, 30);
        cmsRefresh.Text = "تحديث";
        cmsRefresh.Click += cmsRefresh_Click;
        // 
        // pnlTreeHost
        // 
        pnlTreeHost.BackColor = Color.FromArgb(235, 240, 235);
        pnlTreeHost.Controls.Add(ctrlTreeOfAccounts1);
        pnlTreeHost.Dock = DockStyle.Left;
        pnlTreeHost.Location = new Point(0, 0);
        pnlTreeHost.Name = "pnlTreeHost";
        pnlTreeHost.Padding = new Padding(0, 0, 4, 0);
        pnlTreeHost.Size = new Size(360, 396);
        pnlTreeHost.TabIndex = 1;
        // 
        // ctrlTreeOfAccounts1
        // 
        ctrlTreeOfAccounts1.BackColor = Color.FromArgb(170, 220, 170);
        ctrlTreeOfAccounts1.Dock = DockStyle.Fill;
        ctrlTreeOfAccounts1.Font = new Font("Tahoma", 10F);
        ctrlTreeOfAccounts1.Location = new Point(0, 0);
        ctrlTreeOfAccounts1.Name = "ctrlTreeOfAccounts1";
        ctrlTreeOfAccounts1.RightToLeft = RightToLeft.Yes;
        ctrlTreeOfAccounts1.Size = new Size(356, 396);
        ctrlTreeOfAccounts1.TabIndex = 0;
        // 
        // pnlSummary
        // 
        pnlSummary.Controls.Add(lblSummary);
        pnlSummary.Dock = DockStyle.Bottom;
        pnlSummary.Location = new Point(0, 868);
        pnlSummary.Margin = new Padding(0);
        pnlSummary.Name = "pnlSummary";
        pnlSummary.Size = new Size(1432, 32);
        pnlSummary.TabIndex = 5;
        // 
        // lblSummary
        // 
        lblSummary.Dock = DockStyle.Fill;
        lblSummary.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblSummary.ForeColor = Color.FromArgb(14, 82, 52);
        lblSummary.Location = new Point(0, 0);
        lblSummary.Name = "lblSummary";
        lblSummary.Size = new Size(1432, 32);
        lblSummary.TabIndex = 0;
        lblSummary.Text = "المعروض: 0 من 0";
        lblSummary.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // errorProvider
        // 
        errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        errorProvider.ContainerControl = this;
        errorProvider.RightToLeft = true;
        // 
        // frmAccounts
        // 
        AutoScaleDimensions = new SizeF(11F, 24F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(170, 220, 170);
        ClientSize = new Size(1432, 900);
        Controls.Add(pnlBody);
        Controls.Add(pnlSummary);
        Controls.Add(pnlFilters);
        Controls.Add(pnlButtons);
        Controls.Add(pnlFields);
        Controls.Add(lblMode);
        Font = new Font("Tahoma", 10F);
        MinimumSize = new Size(1100, 760);
        Name = "frmAccounts";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterParent;
        Text = "دليل الحسابات";
        Load += frmAccounts_Load;
        pnlFields.ResumeLayout(false);
        pnlFields.PerformLayout();
        pnlButtons.ResumeLayout(false);
        pnlFilters.ResumeLayout(false);
        gbFilter.ResumeLayout(false);
        gbFilter.PerformLayout();
        pnlBody.ResumeLayout(false);
        pnlGrid.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvAccounts).EndInit();
        cmsGrid.ResumeLayout(false);
        pnlTreeHost.ResumeLayout(false);
        pnlSummary.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        ResumeLayout(false);
    }

    private System.Windows.Forms.Label lblMode;
    private System.Windows.Forms.Panel pnlFields;
    private System.Windows.Forms.Label lblParent;
    private System.Windows.Forms.ComboBox cbParent;
    private System.Windows.Forms.Label lblNameAr;
    private System.Windows.Forms.TextBox txtNameAr;
    private System.Windows.Forms.Label lblNameEn;
    private System.Windows.Forms.TextBox txtNameEn;
    private System.Windows.Forms.Label lblAccountCode;
    private System.Windows.Forms.TextBox txtAccountCode;
    private System.Windows.Forms.Label lblNature;
    private System.Windows.Forms.ComboBox cbNature;
    private System.Windows.Forms.Label lblNormalBalance;
    private System.Windows.Forms.ComboBox cbNormalBalance;
    private System.Windows.Forms.Label lblAccountType;
    private System.Windows.Forms.ComboBox cbAccountType;
    private System.Windows.Forms.CheckBox chkIsSystem;
    private System.Windows.Forms.CheckBox chkAllowCostCenter;
    private System.Windows.Forms.Label lblDescription;
    private System.Windows.Forms.TextBox txtDescription;
    private System.Windows.Forms.Label lblNotes;
    private System.Windows.Forms.TextBox txtNotes;
    private System.Windows.Forms.Label lblValidationHint;
    private System.Windows.Forms.FlowLayoutPanel pnlButtons;
    private SalesSystem.Desktop.Controls.Common.ctrlCrudToolbar ctrlCrudToolbar1;
    private System.Windows.Forms.Button btnPermanentDelete;
    private System.Windows.Forms.Panel pnlFilters;
    private System.Windows.Forms.GroupBox gbFilter;
    private System.Windows.Forms.Button btnToggleFilters;
    private System.Windows.Forms.Button btnClearFilters;
    private System.Windows.Forms.Label lblFilterBy;
    private System.Windows.Forms.ComboBox cbFilterBy;
    private System.Windows.Forms.TextBox txtFilterValue;
    private System.Windows.Forms.Label lblNatureFilter;
    private System.Windows.Forms.ComboBox cbNatureFilter;
    private System.Windows.Forms.CheckBox chkShowInactive;
    private System.Windows.Forms.CheckBox chkOnlyLeaf;
    private System.Windows.Forms.CheckBox chkOnlySystem;
    private System.Windows.Forms.CheckBox chkOnlyParents;
    private System.Windows.Forms.Panel pnlBody;
    private System.Windows.Forms.Panel pnlTreeHost;
    private SalesSystem.Desktop.Screens.Accounting.Controls.ctrlTreeOfAccounts ctrlTreeOfAccounts1;
    private System.Windows.Forms.GroupBox pnlGrid;
    private System.Windows.Forms.Label lblEmptyState;
    private System.Windows.Forms.DataGridView dgvAccounts;
    private System.Windows.Forms.DataGridViewTextBoxColumn colCode;
    private System.Windows.Forms.DataGridViewTextBoxColumn colNameAr;
    private System.Windows.Forms.DataGridViewTextBoxColumn colNameEn;
    private System.Windows.Forms.DataGridViewTextBoxColumn colNature;
    private System.Windows.Forms.DataGridViewTextBoxColumn colLevel;
    private System.Windows.Forms.DataGridViewTextBoxColumn colParent;
    private System.Windows.Forms.DataGridViewTextBoxColumn colTreePath;
    private System.Windows.Forms.DataGridViewTextBoxColumn colBalance;
    private System.Windows.Forms.DataGridViewTextBoxColumn colFinancialStatement;
    private System.Windows.Forms.DataGridViewTextBoxColumn colCategory;
    private System.Windows.Forms.DataGridViewTextBoxColumn colIsLeaf;
    private System.Windows.Forms.DataGridViewTextBoxColumn colIsSystem;
    private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
    private System.Windows.Forms.Panel pnlSummary;
    private System.Windows.Forms.Label lblSummary;
    private System.Windows.Forms.ContextMenuStrip cmsGrid;
    private System.Windows.Forms.ToolStripMenuItem cmsEdit;
    private System.Windows.Forms.ToolStripMenuItem cmsDelete;
    private System.Windows.Forms.ToolStripMenuItem cmsRefresh;
    private System.Windows.Forms.ToolTip toolTip;
    private System.Windows.Forms.ErrorProvider errorProvider;
}
