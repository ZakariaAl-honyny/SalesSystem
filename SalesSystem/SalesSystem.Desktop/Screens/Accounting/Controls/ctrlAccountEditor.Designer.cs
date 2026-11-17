namespace SalesSystem.Desktop.Screens.Accounting.Controls;

partial class ctrlAccountEditor
{
    private System.ComponentModel.IContainer components = null;

    // ملاحظات المصمّم:
    //   • المصمّم = التشغيل (RULE-N134 / RULE-N140 / RULE-N204): كل لون بصيغة Convert.ToInt32
    //     وبالقيمة الحرفية نفسها التي تحتفظ بها اللوحة الحيّة وقت التشغيل:
    //       خلفية العنصر   = خلفية النموذج (170,220,170)  ← يفرضها GreenThemeHelper.Apply(Control)
    //       صندوق البيانات = صناديق المجموعات (235,245,235) ← GroupBoxColor
    //       نص التسميات    = النص المميّز (14,82,52)        ← TextStrongColor
    //       نص الحقول      = نص خلايا الشبكة (33,37,41)     ← GridCellTextColor
    //       زر الحفظ        = (25,135,84) · زر الإلغاء = (108,117,125)
    //       شريط الأسفل    = أشرطة الأدوات (235,240,235) ← BarBackgroundColor
    //       سطر صحّة البيانات = لون الخطر (176,42,55)       ← GreenThemeHelper.StatusDanger
    //     ولا مرجع لأي عضو في Helpers/ داخل هذا الملف (RULE-N128).
    //   • RULE-N145: كل عنصر دائم (حقول · أزرار · تسميات · سطر التنبيه) مُعلَن هنا
    //     ولا يُبنى وقت التشغيل.
    //   • RULE-N137: صندوق واحد بعنوان «بيانات الحساب» + شريط سفلي بلا صندوق
    //     (السطر السفلي مستثنى من التجميع) — ولا صندوق داخل صندوق.
    //   • RULE-N125 (ترتيب الإرساء): gbAccountData (Fill) يُضاف أولاً ثم pnlActions (Bottom).
    //   • RULE-N183: errorProvider مُعلَن هنا ويتبنّاه RequiredFieldHelper في OnLoad.
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        toolTip = new ToolTip(components);
        errorProvider = new ErrorProvider(components);
        gbAccountData = new GroupBox();
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
        chkIsLeaf = new CheckBox();
        chkIsSystem = new CheckBox();
        chkAllowCostCenter = new CheckBox();
        lblDescription = new Label();
        txtDescription = new TextBox();
        lblNotes = new Label();
        txtNotes = new TextBox();
        lblDerivedCaption = new Label();
        lblDerivedLevel = new Label();
        txtDerivedLevel = new TextBox();
        lblDerivedFinancialStatement = new Label();
        txtDerivedFinancialStatement = new TextBox();
        lblDerivedIsSystem = new Label();
        txtDerivedIsSystem = new TextBox();
        lblDerivedAudit = new Label();
        txtDerivedAudit = new TextBox();
        pnlActions = new Panel();
        lblValidationHint = new Label();
        flpActions = new FlowLayoutPanel();
        btnSave = new Button();
        btnCancel = new Button();
        ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
        gbAccountData.SuspendLayout();
        pnlActions.SuspendLayout();
        flpActions.SuspendLayout();
        SuspendLayout();
        // 
        // gbAccountData
        // 
        gbAccountData.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(245), Convert.ToInt32(235));
        gbAccountData.Controls.Add(lblParent);
        gbAccountData.Controls.Add(cbParent);
        gbAccountData.Controls.Add(lblNameAr);
        gbAccountData.Controls.Add(txtNameAr);
        gbAccountData.Controls.Add(lblNameEn);
        gbAccountData.Controls.Add(txtNameEn);
        gbAccountData.Controls.Add(lblAccountCode);
        gbAccountData.Controls.Add(txtAccountCode);
        gbAccountData.Controls.Add(lblNature);
        gbAccountData.Controls.Add(cbNature);
        gbAccountData.Controls.Add(lblNormalBalance);
        gbAccountData.Controls.Add(cbNormalBalance);
        gbAccountData.Controls.Add(chkIsLeaf);
        gbAccountData.Controls.Add(chkIsSystem);
        gbAccountData.Controls.Add(chkAllowCostCenter);
        gbAccountData.Controls.Add(lblDescription);
        gbAccountData.Controls.Add(txtDescription);
        gbAccountData.Controls.Add(lblNotes);
        gbAccountData.Controls.Add(txtNotes);
        gbAccountData.Controls.Add(lblDerivedCaption);
        gbAccountData.Controls.Add(lblDerivedLevel);
        gbAccountData.Controls.Add(txtDerivedLevel);
        gbAccountData.Controls.Add(lblDerivedFinancialStatement);
        gbAccountData.Controls.Add(txtDerivedFinancialStatement);
        gbAccountData.Controls.Add(lblDerivedIsSystem);
        gbAccountData.Controls.Add(txtDerivedIsSystem);
        gbAccountData.Controls.Add(lblDerivedAudit);
        gbAccountData.Controls.Add(txtDerivedAudit);
        gbAccountData.Dock = DockStyle.Fill;
        gbAccountData.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        gbAccountData.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
        gbAccountData.Location = new Point(0, 0);
        gbAccountData.Name = "gbAccountData";
        gbAccountData.Size = new Size(460, 616);
        gbAccountData.TabIndex = 1;
        gbAccountData.TabStop = false;
        gbAccountData.Text = "بيانات الحساب";
        // 
        // lblParent
        // 
        lblParent.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblParent.AutoSize = false;
        lblParent.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblParent.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
        lblParent.Location = new Point(316, 30);
        lblParent.Name = "lblParent";
        lblParent.Size = new Size(130, 22);
        lblParent.TabIndex = 0;
        lblParent.Text = "الحساب الأب";
        lblParent.TextAlign = ContentAlignment.MiddleRight;
        // 
        // cbParent
        // 
        cbParent.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        cbParent.BackColor = System.Drawing.Color.White;
        cbParent.DropDownStyle = ComboBoxStyle.DropDownList;
        cbParent.Font = new Font("Tahoma", 10F);
        cbParent.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        cbParent.Location = new Point(12, 26);
        cbParent.Name = "cbParent";
        cbParent.Size = new Size(296, 32);
        cbParent.TabIndex = 1;
        toolTip.SetToolTip(cbParent, "الحساب الأب — تُعرض الحسابات ذات الفروع النشطة فقط، ولا يظهر الحساب نفسه ولا فروعه");
        cbParent.SelectedIndexChanged += cbParent_SelectedIndexChanged;
        // 
        // lblNameAr
        // 
        lblNameAr.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblNameAr.AutoSize = false;
        lblNameAr.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblNameAr.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
        lblNameAr.Location = new Point(316, 68);
        lblNameAr.Name = "lblNameAr";
        lblNameAr.Size = new Size(130, 22);
        lblNameAr.TabIndex = 2;
        lblNameAr.Text = "اسم الحساب *";
        lblNameAr.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtNameAr
        // 
        txtNameAr.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtNameAr.BackColor = System.Drawing.Color.White;
        txtNameAr.BorderStyle = BorderStyle.FixedSingle;
        txtNameAr.Font = new Font("Tahoma", 10F);
        txtNameAr.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        txtNameAr.Location = new Point(12, 64);
        txtNameAr.Name = "txtNameAr";
        txtNameAr.Size = new Size(296, 28);
        txtNameAr.TabIndex = 2;
        toolTip.SetToolTip(txtNameAr, "الاسم العربي للحساب — حقل مطلوب");
        // 
        // lblNameEn
        // 
        lblNameEn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblNameEn.AutoSize = false;
        lblNameEn.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblNameEn.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
        lblNameEn.Location = new Point(316, 106);
        lblNameEn.Name = "lblNameEn";
        lblNameEn.Size = new Size(130, 22);
        lblNameEn.TabIndex = 3;
        lblNameEn.Text = "الاسم الأجنبي";
        lblNameEn.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtNameEn
        // 
        txtNameEn.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtNameEn.BackColor = System.Drawing.Color.White;
        txtNameEn.BorderStyle = BorderStyle.FixedSingle;
        txtNameEn.Font = new Font("Tahoma", 10F);
        txtNameEn.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        txtNameEn.Location = new Point(12, 102);
        txtNameEn.Name = "txtNameEn";
        txtNameEn.Size = new Size(296, 28);
        txtNameEn.TabIndex = 3;
        // 
        // lblAccountCode
        // 
        lblAccountCode.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblAccountCode.AutoSize = false;
        lblAccountCode.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblAccountCode.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
        lblAccountCode.Location = new Point(316, 144);
        lblAccountCode.Name = "lblAccountCode";
        lblAccountCode.Size = new Size(130, 22);
        lblAccountCode.TabIndex = 4;
        lblAccountCode.Text = "رقم الحساب";
        lblAccountCode.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtAccountCode
        // 
        txtAccountCode.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtAccountCode.BackColor = System.Drawing.Color.White;
        txtAccountCode.BorderStyle = BorderStyle.FixedSingle;
        txtAccountCode.Font = new Font("Tahoma", 10F);
        txtAccountCode.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        txtAccountCode.Location = new Point(12, 140);
        txtAccountCode.Name = "txtAccountCode";
        txtAccountCode.ReadOnly = true;
        txtAccountCode.Size = new Size(296, 28);
        txtAccountCode.TabIndex = 4;
        txtAccountCode.TabStop = false;
        toolTip.SetToolTip(txtAccountCode, "رمز الحساب يُولّده النظام ولا يُعدَّل يدوياً — ويُعرض هنا كود المعاينة في وضع الإضافة");
        // 
        // lblNature
        // 
        lblNature.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblNature.AutoSize = false;
        lblNature.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblNature.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
        lblNature.Location = new Point(316, 182);
        lblNature.Name = "lblNature";
        lblNature.Size = new Size(130, 22);
        lblNature.TabIndex = 5;
        lblNature.Text = "نوع الحساب";
        lblNature.TextAlign = ContentAlignment.MiddleRight;
        // 
        // cbNature
        // 
        cbNature.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        cbNature.BackColor = System.Drawing.Color.White;
        cbNature.DropDownStyle = ComboBoxStyle.DropDownList;
        cbNature.Font = new Font("Tahoma", 10F);
        cbNature.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        cbNature.Location = new Point(12, 178);
        cbNature.Name = "cbNature";
        cbNature.Size = new Size(296, 32);
        cbNature.TabIndex = 5;
        toolTip.SetToolTip(cbNature, "نوع الحساب: أصول · خصوم · حقوق ملكية · إيرادات · مصروفات");
        cbNature.SelectedIndexChanged += cbNature_SelectedIndexChanged;
        // 
        // lblNormalBalance
        // 
        lblNormalBalance.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblNormalBalance.AutoSize = false;
        lblNormalBalance.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblNormalBalance.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
        lblNormalBalance.Location = new Point(316, 220);
        lblNormalBalance.Name = "lblNormalBalance";
        lblNormalBalance.Size = new Size(130, 22);
        lblNormalBalance.TabIndex = 6;
        lblNormalBalance.Text = "الحساب الختامي";
        lblNormalBalance.TextAlign = ContentAlignment.MiddleRight;
        // 
        // cbNormalBalance
        // 
        cbNormalBalance.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        cbNormalBalance.BackColor = System.Drawing.Color.White;
        cbNormalBalance.DropDownStyle = ComboBoxStyle.DropDownList;
        cbNormalBalance.Font = new Font("Tahoma", 10F);
        cbNormalBalance.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        cbNormalBalance.Location = new Point(12, 216);
        cbNormalBalance.Name = "cbNormalBalance";
        cbNormalBalance.Size = new Size(296, 32);
        cbNormalBalance.TabIndex = 6;
        toolTip.SetToolTip(cbNormalBalance, "طبيعة الرصيد — «تلقائي» تُشتق من نوع الحساب؛ اختر «مدين» أو «دائن» للحسابات المضادة");
        // 
        // chkIsLeaf
        // 
        chkIsLeaf.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        chkIsLeaf.AutoSize = true;
        chkIsLeaf.Font = new Font("Tahoma", 10F);
        chkIsLeaf.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        chkIsLeaf.Location = new Point(234, 254);
        chkIsLeaf.Name = "chkIsLeaf";
        chkIsLeaf.Size = new Size(212, 28);
        chkIsLeaf.TabIndex = 7;
        chkIsLeaf.Text = "حساب ورقى (قابل للترحيل)";
        chkIsLeaf.UseVisualStyleBackColor = false;
        toolTip.SetToolTip(chkIsLeaf, "حساب ورقى قابل للترحيل — لا يحمل حسابات فرعية، ولا يُحوَّل إلى ورقى وهو يحمل فروعاً");
        // 
        // chkIsSystem
        // 
        chkIsSystem.AutoSize = true;
        chkIsSystem.Font = new Font("Tahoma", 10F);
        chkIsSystem.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        chkIsSystem.Location = new Point(12, 254);
        chkIsSystem.Name = "chkIsSystem";
        chkIsSystem.Size = new Size(140, 28);
        chkIsSystem.TabIndex = 8;
        chkIsSystem.Text = "حساب نظامي";
        chkIsSystem.UseVisualStyleBackColor = false;
        toolTip.SetToolTip(chkIsSystem, "الحسابات النظامية تُنشأ من بذور النظام ومزامنته فقط — العلم للعرض ولا يُغيَّر من الشاشة");
        // 
        // chkAllowCostCenter
        // 
        chkAllowCostCenter.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        chkAllowCostCenter.AutoSize = true;
        chkAllowCostCenter.Font = new Font("Tahoma", 10F);
        chkAllowCostCenter.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        chkAllowCostCenter.Location = new Point(234, 284);
        chkAllowCostCenter.Name = "chkAllowCostCenter";
        chkAllowCostCenter.Size = new Size(212, 28);
        chkAllowCostCenter.TabIndex = 9;
        chkAllowCostCenter.Text = "يسمح بمركز تكلفة";
        chkAllowCostCenter.UseVisualStyleBackColor = false;
        toolTip.SetToolTip(chkAllowCostCenter, "يسمح بمركز التكلفة عند ترحيل القيود على هذا الحساب");
        // 
        // lblDescription
        // 
        lblDescription.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblDescription.AutoSize = false;
        lblDescription.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblDescription.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
        lblDescription.Location = new Point(316, 320);
        lblDescription.Name = "lblDescription";
        lblDescription.Size = new Size(130, 22);
        lblDescription.TabIndex = 10;
        lblDescription.Text = "وصف";
        lblDescription.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtDescription
        // 
        txtDescription.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtDescription.BackColor = System.Drawing.Color.White;
        txtDescription.BorderStyle = BorderStyle.FixedSingle;
        txtDescription.Font = new Font("Tahoma", 10F);
        txtDescription.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        txtDescription.Location = new Point(12, 316);
        txtDescription.Multiline = true;
        txtDescription.Name = "txtDescription";
        txtDescription.ScrollBars = ScrollBars.Vertical;
        txtDescription.Size = new Size(296, 56);
        txtDescription.TabIndex = 10;
        // 
        // lblNotes
        // 
        lblNotes.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblNotes.AutoSize = false;
        lblNotes.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblNotes.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
        lblNotes.Location = new Point(316, 384);
        lblNotes.Name = "lblNotes";
        lblNotes.Size = new Size(130, 22);
        lblNotes.TabIndex = 11;
        lblNotes.Text = "ملاحظات";
        lblNotes.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtNotes
        // 
        txtNotes.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtNotes.BackColor = System.Drawing.Color.White;
        txtNotes.BorderStyle = BorderStyle.FixedSingle;
        txtNotes.Font = new Font("Tahoma", 10F);
        txtNotes.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        txtNotes.Location = new Point(12, 380);
        txtNotes.Multiline = true;
        txtNotes.Name = "txtNotes";
        txtNotes.ScrollBars = ScrollBars.Vertical;
        txtNotes.Size = new Size(296, 56);
        txtNotes.TabIndex = 11;
        // 
        // lblDerivedCaption
        // 
        lblDerivedCaption.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        lblDerivedCaption.AutoSize = false;
        lblDerivedCaption.BorderStyle = BorderStyle.Fixed3D;
        lblDerivedCaption.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblDerivedCaption.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
        lblDerivedCaption.Location = new Point(12, 446);
        lblDerivedCaption.Name = "lblDerivedCaption";
        lblDerivedCaption.Size = new Size(434, 22);
        lblDerivedCaption.TabIndex = 12;
        lblDerivedCaption.Text = "محسوب تلقائياً بواسطة النظام";
        lblDerivedCaption.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblDerivedLevel
        // 
        lblDerivedLevel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblDerivedLevel.AutoSize = false;
        lblDerivedLevel.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblDerivedLevel.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
        lblDerivedLevel.Location = new Point(286, 478);
        lblDerivedLevel.Name = "lblDerivedLevel";
        lblDerivedLevel.Size = new Size(160, 22);
        lblDerivedLevel.TabIndex = 13;
        lblDerivedLevel.Text = "المستوى";
        lblDerivedLevel.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtDerivedLevel
        // 
        txtDerivedLevel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtDerivedLevel.BackColor = System.Drawing.Color.White;
        txtDerivedLevel.BorderStyle = BorderStyle.FixedSingle;
        txtDerivedLevel.Font = new Font("Tahoma", 10F);
        txtDerivedLevel.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        txtDerivedLevel.Location = new Point(12, 474);
        txtDerivedLevel.Name = "txtDerivedLevel";
        txtDerivedLevel.ReadOnly = true;
        txtDerivedLevel.Size = new Size(266, 28);
        txtDerivedLevel.TabIndex = 14;
        txtDerivedLevel.TabStop = false;
        toolTip.SetToolTip(txtDerivedLevel, "مستوى الحساب يحدّده النظام: مستوى الأب + 1، والحساب الرئيسي = مستوى 1");
        // 
        // lblDerivedFinancialStatement
        // 
        lblDerivedFinancialStatement.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblDerivedFinancialStatement.AutoSize = false;
        lblDerivedFinancialStatement.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblDerivedFinancialStatement.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
        lblDerivedFinancialStatement.Location = new Point(286, 512);
        lblDerivedFinancialStatement.Name = "lblDerivedFinancialStatement";
        lblDerivedFinancialStatement.Size = new Size(160, 22);
        lblDerivedFinancialStatement.TabIndex = 15;
        lblDerivedFinancialStatement.Text = "القائمة المالية";
        lblDerivedFinancialStatement.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtDerivedFinancialStatement
        // 
        txtDerivedFinancialStatement.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtDerivedFinancialStatement.BackColor = System.Drawing.Color.White;
        txtDerivedFinancialStatement.BorderStyle = BorderStyle.FixedSingle;
        txtDerivedFinancialStatement.Font = new Font("Tahoma", 10F);
        txtDerivedFinancialStatement.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        txtDerivedFinancialStatement.Location = new Point(12, 508);
        txtDerivedFinancialStatement.Name = "txtDerivedFinancialStatement";
        txtDerivedFinancialStatement.ReadOnly = true;
        txtDerivedFinancialStatement.Size = new Size(266, 28);
        txtDerivedFinancialStatement.TabIndex = 16;
        txtDerivedFinancialStatement.TabStop = false;
        toolTip.SetToolTip(txtDerivedFinancialStatement, "القائمة المالية تُشتق من نوع الحساب: أصول/خصوم/حقوق ملكية ⇒ الميزانية العمومية · إيرادات/مصروفات ⇒ قائمة الدخل");
        // 
        // lblDerivedIsSystem
        // 
        lblDerivedIsSystem.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblDerivedIsSystem.AutoSize = false;
        lblDerivedIsSystem.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblDerivedIsSystem.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
        lblDerivedIsSystem.Location = new Point(286, 546);
        lblDerivedIsSystem.Name = "lblDerivedIsSystem";
        lblDerivedIsSystem.Size = new Size(160, 22);
        lblDerivedIsSystem.TabIndex = 17;
        lblDerivedIsSystem.Text = "حساب نظامي";
        lblDerivedIsSystem.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtDerivedIsSystem
        // 
        txtDerivedIsSystem.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtDerivedIsSystem.BackColor = System.Drawing.Color.White;
        txtDerivedIsSystem.BorderStyle = BorderStyle.FixedSingle;
        txtDerivedIsSystem.Font = new Font("Tahoma", 10F);
        txtDerivedIsSystem.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        txtDerivedIsSystem.Location = new Point(12, 542);
        txtDerivedIsSystem.Name = "txtDerivedIsSystem";
        txtDerivedIsSystem.ReadOnly = true;
        txtDerivedIsSystem.Size = new Size(266, 28);
        txtDerivedIsSystem.TabIndex = 18;
        txtDerivedIsSystem.TabStop = false;
        toolTip.SetToolTip(txtDerivedIsSystem, "الحسابات النظامية تُنشأ من بذور النظام ومزامنته فقط — ولا يُنشأ حساب نظامي من الشاشة");
        // 
        // lblDerivedAudit
        // 
        lblDerivedAudit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblDerivedAudit.AutoSize = false;
        lblDerivedAudit.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblDerivedAudit.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
        lblDerivedAudit.Location = new Point(286, 580);
        lblDerivedAudit.Name = "lblDerivedAudit";
        lblDerivedAudit.Size = new Size(160, 22);
        lblDerivedAudit.TabIndex = 19;
        lblDerivedAudit.Text = "تاريخ الإنشاء ومُنشئه";
        lblDerivedAudit.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtDerivedAudit
        // 
        txtDerivedAudit.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtDerivedAudit.BackColor = System.Drawing.Color.White;
        txtDerivedAudit.BorderStyle = BorderStyle.FixedSingle;
        txtDerivedAudit.Enabled = false;
        txtDerivedAudit.Font = new Font("Tahoma", 10F);
        txtDerivedAudit.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        txtDerivedAudit.Location = new Point(12, 576);
        txtDerivedAudit.Name = "txtDerivedAudit";
        txtDerivedAudit.ReadOnly = true;
        txtDerivedAudit.Size = new Size(266, 28);
        txtDerivedAudit.TabIndex = 20;
        txtDerivedAudit.TabStop = false;
        toolTip.SetToolTip(txtDerivedAudit, "يُسجَّل تلقائياً عند الحفظ من جلسة المستخدم");
        // 
        // pnlActions
        // 
        pnlActions.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
        pnlActions.Controls.Add(lblValidationHint);
        pnlActions.Controls.Add(flpActions);
        pnlActions.Dock = DockStyle.Bottom;
        pnlActions.Location = new Point(0, 616);
        pnlActions.Name = "pnlActions";
        pnlActions.Size = new Size(460, 84);
        pnlActions.TabIndex = 0;
        // 
        // lblValidationHint
        // 
        lblValidationHint.Dock = DockStyle.Top;
        lblValidationHint.Font = new Font("Tahoma", 9F);
        lblValidationHint.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(176), Convert.ToInt32(42), Convert.ToInt32(55));
        lblValidationHint.Location = new Point(0, 0);
        lblValidationHint.Name = "lblValidationHint";
        lblValidationHint.Padding = new Padding(8, 0, 8, 0);
        lblValidationHint.Size = new Size(460, 30);
        lblValidationHint.TabIndex = 0;
        lblValidationHint.TextAlign = ContentAlignment.MiddleRight;
        // 
        // flpActions
        // 
        flpActions.Controls.Add(btnSave);
        flpActions.Controls.Add(btnCancel);
        flpActions.Dock = DockStyle.Bottom;
        flpActions.FlowDirection = FlowDirection.RightToLeft;
        flpActions.Location = new Point(0, 30);
        flpActions.Name = "flpActions";
        flpActions.Padding = new Padding(6, 7, 6, 7);
        flpActions.Size = new Size(460, 54);
        flpActions.TabIndex = 1;
        flpActions.WrapContents = false;
        // 
        // btnSave
        // 
        btnSave.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(25), Convert.ToInt32(135), Convert.ToInt32(84));
        btnSave.Cursor = Cursors.Hand;
        btnSave.FlatAppearance.BorderSize = 0;
        btnSave.FlatStyle = FlatStyle.Flat;
        btnSave.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnSave.ForeColor = System.Drawing.Color.White;
        btnSave.Location = new Point(334, 7);
        btnSave.Margin = new Padding(4, 0, 0, 0);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(120, 38);
        btnSave.TabIndex = 0;
        btnSave.Text = "حفظ";
        toolTip.SetToolTip(btnSave, "حفظ الحساب (Ctrl+S في الشاشة الحاضنة)");
        btnSave.UseVisualStyleBackColor = false;
        btnSave.Click += btnSave_Click;
        // 
        // btnCancel
        // 
        btnCancel.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(108), Convert.ToInt32(117), Convert.ToInt32(125));
        btnCancel.Cursor = Cursors.Hand;
        btnCancel.FlatAppearance.BorderSize = 0;
        btnCancel.FlatStyle = FlatStyle.Flat;
        btnCancel.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnCancel.ForeColor = System.Drawing.Color.White;
        btnCancel.Location = new Point(210, 7);
        btnCancel.Margin = new Padding(4, 0, 0, 0);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(120, 38);
        btnCancel.TabIndex = 1;
        btnCancel.Text = "إلغاء";
        toolTip.SetToolTip(btnCancel, "إلغاء والعودة إلى وضع العرض (Esc)");
        btnCancel.UseVisualStyleBackColor = false;
        btnCancel.Click += btnCancel_Click;
        // 
        // errorProvider
        // 
        errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        errorProvider.ContainerControl = this;
        errorProvider.RightToLeft = true;
        // 
        // ctrlAccountEditor
        // 
        AutoScaleMode = AutoScaleMode.None;
        BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(170), Convert.ToInt32(220), Convert.ToInt32(170));
        Controls.Add(gbAccountData);
        Controls.Add(pnlActions);
        Font = new Font("Tahoma", 10F);
        Name = "ctrlAccountEditor";
        MinimumSize = new Size(340, 700);
        RightToLeft = RightToLeft.Yes;
        Size = new Size(460, 700);
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        gbAccountData.ResumeLayout(false);
        gbAccountData.PerformLayout();
        pnlActions.ResumeLayout(false);
        flpActions.ResumeLayout(false);
        ResumeLayout(false);
    }

    private System.Windows.Forms.ToolTip toolTip;
    private System.Windows.Forms.ErrorProvider errorProvider;
    private System.Windows.Forms.GroupBox gbAccountData;
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
    private System.Windows.Forms.CheckBox chkIsLeaf;
    private System.Windows.Forms.CheckBox chkIsSystem;
    private System.Windows.Forms.CheckBox chkAllowCostCenter;
    private System.Windows.Forms.Label lblDescription;
    private System.Windows.Forms.TextBox txtDescription;
    private System.Windows.Forms.Label lblNotes;
    private System.Windows.Forms.TextBox txtNotes;
    private System.Windows.Forms.Label lblDerivedCaption;
    private System.Windows.Forms.Label lblDerivedLevel;
    private System.Windows.Forms.TextBox txtDerivedLevel;
    private System.Windows.Forms.Label lblDerivedFinancialStatement;
    private System.Windows.Forms.TextBox txtDerivedFinancialStatement;
    private System.Windows.Forms.Label lblDerivedIsSystem;
    private System.Windows.Forms.TextBox txtDerivedIsSystem;
    private System.Windows.Forms.Label lblDerivedAudit;
    private System.Windows.Forms.TextBox txtDerivedAudit;
    private System.Windows.Forms.Panel pnlActions;
    private System.Windows.Forms.Label lblValidationHint;
    private System.Windows.Forms.FlowLayoutPanel flpActions;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.Button btnCancel;
}
