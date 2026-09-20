namespace SalesManagement.Items
{
    partial class frmAddUpdateItem
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAddUpdateItem));
            label1 = new Label();
            lblItemID = new Label();
            lblMode = new Label();
            errorProvider1 = new ErrorProvider(components);
            pictureBox11 = new PictureBox();
            openFileDialog1 = new OpenFileDialog();
            pictureBox5 = new PictureBox();
            pictureBox7 = new PictureBox();
            pictureBox8 = new PictureBox();
            pictureBox4 = new PictureBox();
            pictureBox3 = new PictureBox();
            pictureBox2 = new PictureBox();
            pbItemImage = new PictureBox();
            label10 = new Label();
            txtItemName = new TextBox();
            label13 = new Label();
            txtItemCode = new TextBox();
            label2 = new Label();
            btnClose = new Button();
            label12 = new Label();
            label9 = new Label();
            txtMinmumQty = new TextBox();
            llRemoveImage = new LinkLabel();
            llOpenFileDialog = new LinkLabel();
            label5 = new Label();
            cbxUnits = new ComboBox();
            btnSave = new Button();
            txtBuyPrice = new TextBox();
            label3 = new Label();
            pictureBox1 = new PictureBox();
            pictureBox12 = new PictureBox();
            pictureBox13 = new PictureBox();
            label7 = new Label();
            label6 = new Label();
            cbxCompanies = new ComboBox();
            cbxCategories = new ComboBox();
            cbxStores = new ComboBox();
            chbxIsActiveSale = new CheckBox();
            chbxIsActiveBuy = new CheckBox();
            chbxPreventFraction = new CheckBox();
            chbxPreventDiscount = new CheckBox();
            label4 = new Label();
            label8 = new Label();
            chkStopProfitRate = new CheckBox();
            txtSalePrice = new TextBox();
            nudProfitRate = new NumericUpDown();
            btnSaveAndClose = new Button();
            ((System.ComponentModel.ISupportInitialize)errorProvider1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox11).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox7).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox8).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbItemImage).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox12).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox13).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudProfitRate).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top;
            label1.AutoSize = true;
            label1.FlatStyle = FlatStyle.Flat;
            label1.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label1.Location = new Point(97, 86);
            label1.Margin = new Padding(4);
            label1.Name = "label1";
            label1.Size = new Size(120, 24);
            label1.TabIndex = 0;
            label1.Text = "رقم الصنف:";
            // 
            // lblItemID
            // 
            lblItemID.Anchor = AnchorStyles.Top;
            lblItemID.AutoSize = true;
            lblItemID.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblItemID.Location = new Point(284, 86);
            lblItemID.Margin = new Padding(4);
            lblItemID.Name = "lblItemID";
            lblItemID.Size = new Size(51, 24);
            lblItemID.TabIndex = 7;
            lblItemID.Text = "N/A";
            // 
            // lblMode
            // 
            lblMode.Anchor = AnchorStyles.Top;
            lblMode.AutoSize = true;
            lblMode.BackColor = Color.WhiteSmoke;
            lblMode.FlatStyle = FlatStyle.Flat;
            lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblMode.ForeColor = Color.Firebrick;
            lblMode.Location = new Point(517, 8);
            lblMode.Margin = new Padding(4);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(397, 39);
            lblMode.TabIndex = 29;
            lblMode.Text = "إضافة وتعديل بيانات صنف";
            // 
            // errorProvider1
            // 
            errorProvider1.ContainerControl = this;
            // 
            // pictureBox11
            // 
            pictureBox11.Anchor = AnchorStyles.Top;
            pictureBox11.Image = Properties.Resources.Number_32;
            pictureBox11.Location = new Point(225, 73);
            pictureBox11.Margin = new Padding(4);
            pictureBox11.Name = "pictureBox11";
            pictureBox11.Size = new Size(47, 51);
            pictureBox11.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox11.TabIndex = 78;
            pictureBox11.TabStop = false;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // pictureBox5
            // 
            pictureBox5.Anchor = AnchorStyles.Top;
            pictureBox5.Image = Properties.Resources.Address_32;
            pictureBox5.Location = new Point(712, 427);
            pictureBox5.Margin = new Padding(4);
            pictureBox5.Name = "pictureBox5";
            pictureBox5.Size = new Size(47, 51);
            pictureBox5.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox5.TabIndex = 112;
            pictureBox5.TabStop = false;
            // 
            // pictureBox7
            // 
            pictureBox7.Anchor = AnchorStyles.Top;
            pictureBox7.Image = Properties.Resources.Email_32;
            pictureBox7.Location = new Point(712, 368);
            pictureBox7.Margin = new Padding(4);
            pictureBox7.Name = "pictureBox7";
            pictureBox7.Size = new Size(47, 51);
            pictureBox7.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox7.TabIndex = 113;
            pictureBox7.TabStop = false;
            // 
            // pictureBox8
            // 
            pictureBox8.Anchor = AnchorStyles.Top;
            pictureBox8.Image = Properties.Resources.Country_32;
            pictureBox8.Location = new Point(225, 132);
            pictureBox8.Margin = new Padding(4);
            pictureBox8.Name = "pictureBox8";
            pictureBox8.Size = new Size(47, 51);
            pictureBox8.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox8.TabIndex = 115;
            pictureBox8.TabStop = false;
            // 
            // pictureBox4
            // 
            pictureBox4.Anchor = AnchorStyles.Top;
            pictureBox4.Image = Properties.Resources.Man_32;
            pictureBox4.Location = new Point(712, 191);
            pictureBox4.Margin = new Padding(4);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new Size(47, 51);
            pictureBox4.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox4.TabIndex = 111;
            pictureBox4.TabStop = false;
            // 
            // pictureBox3
            // 
            pictureBox3.Anchor = AnchorStyles.Top;
            pictureBox3.Image = Properties.Resources.Number_32;
            pictureBox3.Location = new Point(225, 427);
            pictureBox3.Margin = new Padding(4);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(47, 51);
            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox3.TabIndex = 110;
            pictureBox3.TabStop = false;
            // 
            // pictureBox2
            // 
            pictureBox2.Anchor = AnchorStyles.Top;
            pictureBox2.Image = Properties.Resources.Person_32;
            pictureBox2.Location = new Point(225, 191);
            pictureBox2.Margin = new Padding(4);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(47, 51);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 109;
            pictureBox2.TabStop = false;
            // 
            // pbItemImage
            // 
            pbItemImage.Anchor = AnchorStyles.Top;
            pbItemImage.BorderStyle = BorderStyle.FixedSingle;
            pbItemImage.Image = Properties.Resources.Male_512;
            pbItemImage.Location = new Point(1107, 132);
            pbItemImage.Margin = new Padding(4);
            pbItemImage.Name = "pbItemImage";
            pbItemImage.Size = new Size(250, 250);
            pbItemImage.SizeMode = PictureBoxSizeMode.Zoom;
            pbItemImage.TabIndex = 108;
            pbItemImage.TabStop = false;
            // 
            // label10
            // 
            label10.Anchor = AnchorStyles.Top;
            label10.AutoSize = true;
            label10.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label10.Location = new Point(577, 381);
            label10.Margin = new Padding(4);
            label10.Name = "label10";
            label10.Size = new Size(127, 24);
            label10.TabIndex = 102;
            label10.Text = "سعر الشراء:";
            // 
            // txtItemName
            // 
            txtItemName.Anchor = AnchorStyles.Top;
            txtItemName.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            txtItemName.Location = new Point(282, 200);
            txtItemName.Margin = new Padding(4);
            txtItemName.Name = "txtItemName";
            txtItemName.Size = new Size(261, 32);
            txtItemName.TabIndex = 79;
            txtItemName.KeyPress += txtItemName_KeyPress;
            txtItemName.Validating += _ValidateEmptyTextBox;
            // 
            // label13
            // 
            label13.Anchor = AnchorStyles.Top;
            label13.AutoSize = true;
            label13.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label13.Location = new Point(575, 204);
            label13.Margin = new Padding(4);
            label13.Name = "label13";
            label13.Size = new Size(129, 24);
            label13.TabIndex = 106;
            label13.Text = "حد النواقص:";
            // 
            // txtItemCode
            // 
            txtItemCode.Anchor = AnchorStyles.Top;
            txtItemCode.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            txtItemCode.Location = new Point(280, 141);
            txtItemCode.Margin = new Padding(4);
            txtItemCode.Name = "txtItemCode";
            txtItemCode.Size = new Size(748, 32);
            txtItemCode.TabIndex = 0;
            txtItemCode.KeyPress += txtAllowOnlyNumber_KeyPress;
            txtItemCode.Validating += _ValidateEmptyTextBox;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top;
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label2.Location = new Point(89, 204);
            label2.Margin = new Padding(4);
            label2.Name = "label2";
            label2.Size = new Size(128, 24);
            label2.TabIndex = 105;
            label2.Text = "أسم الصنف:";
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Top;
            btnClose.BackColor = Color.White;
            btnClose.DialogResult = DialogResult.Cancel;
            btnClose.FlatStyle = FlatStyle.Popup;
            btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnClose.Image = (Image)resources.GetObject("btnClose.Image");
            btnClose.ImageAlign = ContentAlignment.MiddleRight;
            btnClose.Location = new Point(611, 552);
            btnClose.Margin = new Padding(4);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(185, 61);
            btnClose.TabIndex = 94;
            btnClose.Text = "إغلاق";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // label12
            // 
            label12.Anchor = AnchorStyles.Top;
            label12.AutoSize = true;
            label12.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label12.Location = new Point(103, 145);
            label12.Margin = new Padding(4);
            label12.Name = "label12";
            label12.Size = new Size(114, 24);
            label12.TabIndex = 104;
            label12.Text = "كود المنتج:";
            // 
            // label9
            // 
            label9.Anchor = AnchorStyles.Top;
            label9.AutoSize = true;
            label9.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label9.Location = new Point(594, 440);
            label9.Margin = new Padding(4);
            label9.Name = "label9";
            label9.Size = new Size(110, 24);
            label9.TabIndex = 101;
            label9.Text = "سعر البيع:";
            // 
            // txtMinmumQty
            // 
            txtMinmumQty.Anchor = AnchorStyles.Top;
            txtMinmumQty.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            txtMinmumQty.Location = new Point(767, 200);
            txtMinmumQty.Margin = new Padding(4);
            txtMinmumQty.Name = "txtMinmumQty";
            txtMinmumQty.Size = new Size(261, 32);
            txtMinmumQty.TabIndex = 87;
            txtMinmumQty.KeyPress += txtAllowOnlyNumber_KeyPress;
            txtMinmumQty.Validating += _ValidateEmptyTextBox;
            // 
            // llRemoveImage
            // 
            llRemoveImage.Anchor = AnchorStyles.Top;
            llRemoveImage.AutoSize = true;
            llRemoveImage.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            llRemoveImage.Location = new Point(1169, 422);
            llRemoveImage.Margin = new Padding(4);
            llRemoveImage.Name = "llRemoveImage";
            llRemoveImage.Size = new Size(127, 24);
            llRemoveImage.TabIndex = 92;
            llRemoveImage.TabStop = true;
            llRemoveImage.Text = "خذف الصورة";
            llRemoveImage.Visible = false;
            llRemoveImage.LinkClicked += llRemoveImage_LinkClicked;
            // 
            // llOpenFileDialog
            // 
            llOpenFileDialog.Anchor = AnchorStyles.Top;
            llOpenFileDialog.AutoSize = true;
            llOpenFileDialog.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            llOpenFileDialog.Location = new Point(1180, 390);
            llOpenFileDialog.Margin = new Padding(4);
            llOpenFileDialog.Name = "llOpenFileDialog";
            llOpenFileDialog.Size = new Size(105, 24);
            llOpenFileDialog.TabIndex = 91;
            llOpenFileDialog.TabStop = true;
            llOpenFileDialog.Text = "أختر صورة";
            llOpenFileDialog.LinkClicked += llOpenFileDialog_LinkClicked;
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Top;
            label5.AutoSize = true;
            label5.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label5.Location = new Point(45, 263);
            label5.Margin = new Padding(4);
            label5.Name = "label5";
            label5.Size = new Size(172, 24);
            label5.TabIndex = 97;
            label5.Text = "الشركة المصنعة:";
            label5.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // cbxUnits
            // 
            cbxUnits.Anchor = AnchorStyles.Top;
            cbxUnits.BackColor = SystemColors.Control;
            cbxUnits.DropDownStyle = ComboBoxStyle.DropDownList;
            cbxUnits.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            cbxUnits.FormattingEnabled = true;
            cbxUnits.Location = new Point(280, 436);
            cbxUnits.Margin = new Padding(4);
            cbxUnits.Name = "cbxUnits";
            cbxUnits.Size = new Size(261, 32);
            cbxUnits.TabIndex = 89;
            cbxUnits.Validating += _ValidateEmptyComboBox;
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Top;
            btnSave.BackColor = Color.White;
            btnSave.FlatStyle = FlatStyle.Popup;
            btnSave.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnSave.Image = (Image)resources.GetObject("btnSave.Image");
            btnSave.ImageAlign = ContentAlignment.MiddleRight;
            btnSave.Location = new Point(418, 552);
            btnSave.Margin = new Padding(4);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(185, 61);
            btnSave.TabIndex = 93;
            btnSave.Text = "إضافة";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // txtBuyPrice
            // 
            txtBuyPrice.Anchor = AnchorStyles.Top;
            txtBuyPrice.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            txtBuyPrice.Location = new Point(767, 377);
            txtBuyPrice.Margin = new Padding(4);
            txtBuyPrice.Name = "txtBuyPrice";
            txtBuyPrice.Size = new Size(261, 32);
            txtBuyPrice.TabIndex = 83;
            txtBuyPrice.TextChanged += txtBuyPrice_TextChanged;
            txtBuyPrice.KeyPress += txtAllowOnlyNumber_KeyPress;
            txtBuyPrice.Validating += _ValidateEmptyTextBox;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top;
            label3.AutoSize = true;
            label3.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label3.Location = new Point(102, 440);
            label3.Margin = new Padding(4);
            label3.Name = "label3";
            label3.Size = new Size(115, 24);
            label3.TabIndex = 95;
            label3.Text = "وحدة البيع:";
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.Top;
            pictureBox1.Image = Properties.Resources.Person_32;
            pictureBox1.Location = new Point(225, 250);
            pictureBox1.Margin = new Padding(4);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(47, 51);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 118;
            pictureBox1.TabStop = false;
            // 
            // pictureBox12
            // 
            pictureBox12.Anchor = AnchorStyles.Top;
            pictureBox12.Image = Properties.Resources.Person_32;
            pictureBox12.Location = new Point(225, 309);
            pictureBox12.Margin = new Padding(4);
            pictureBox12.Name = "pictureBox12";
            pictureBox12.Size = new Size(47, 51);
            pictureBox12.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox12.TabIndex = 119;
            pictureBox12.TabStop = false;
            // 
            // pictureBox13
            // 
            pictureBox13.Anchor = AnchorStyles.Top;
            pictureBox13.Image = Properties.Resources.Person_32;
            pictureBox13.Location = new Point(225, 368);
            pictureBox13.Margin = new Padding(4);
            pictureBox13.Name = "pictureBox13";
            pictureBox13.Size = new Size(47, 51);
            pictureBox13.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox13.TabIndex = 120;
            pictureBox13.TabStop = false;
            // 
            // label7
            // 
            label7.Anchor = AnchorStyles.Top;
            label7.AutoSize = true;
            label7.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label7.Location = new Point(77, 381);
            label7.Margin = new Padding(4);
            label7.Name = "label7";
            label7.Size = new Size(140, 24);
            label7.TabIndex = 99;
            label7.Text = "مكان التخزين:";
            // 
            // label6
            // 
            label6.Anchor = AnchorStyles.Top;
            label6.AutoSize = true;
            label6.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label6.Location = new Point(106, 322);
            label6.Margin = new Padding(4);
            label6.Name = "label6";
            label6.Size = new Size(111, 24);
            label6.TabIndex = 98;
            label6.Text = "التصنيفات:";
            // 
            // cbxCompanies
            // 
            cbxCompanies.Anchor = AnchorStyles.Top;
            cbxCompanies.BackColor = SystemColors.Control;
            cbxCompanies.DropDownStyle = ComboBoxStyle.DropDownList;
            cbxCompanies.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            cbxCompanies.FormattingEnabled = true;
            cbxCompanies.Location = new Point(280, 259);
            cbxCompanies.Margin = new Padding(4);
            cbxCompanies.Name = "cbxCompanies";
            cbxCompanies.Size = new Size(261, 32);
            cbxCompanies.TabIndex = 122;
            // 
            // cbxCategories
            // 
            cbxCategories.Anchor = AnchorStyles.Top;
            cbxCategories.BackColor = SystemColors.Control;
            cbxCategories.DropDownStyle = ComboBoxStyle.DropDownList;
            cbxCategories.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            cbxCategories.FormattingEnabled = true;
            cbxCategories.Location = new Point(280, 318);
            cbxCategories.Margin = new Padding(4);
            cbxCategories.Name = "cbxCategories";
            cbxCategories.Size = new Size(261, 32);
            cbxCategories.TabIndex = 123;
            cbxCategories.Validating += _ValidateEmptyComboBox;
            // 
            // cbxStores
            // 
            cbxStores.Anchor = AnchorStyles.Top;
            cbxStores.BackColor = SystemColors.Control;
            cbxStores.DropDownStyle = ComboBoxStyle.DropDownList;
            cbxStores.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            cbxStores.FormattingEnabled = true;
            cbxStores.Location = new Point(280, 377);
            cbxStores.Margin = new Padding(4);
            cbxStores.Name = "cbxStores";
            cbxStores.Size = new Size(261, 32);
            cbxStores.TabIndex = 124;
            cbxStores.Validating += _ValidateEmptyComboBox;
            // 
            // chbxIsActiveSale
            // 
            chbxIsActiveSale.Anchor = AnchorStyles.Top;
            chbxIsActiveSale.AutoSize = true;
            chbxIsActiveSale.Checked = true;
            chbxIsActiveSale.CheckState = CheckState.Checked;
            chbxIsActiveSale.Location = new Point(575, 318);
            chbxIsActiveSale.Margin = new Padding(4);
            chbxIsActiveSale.Name = "chbxIsActiveSale";
            chbxIsActiveSale.Size = new Size(137, 33);
            chbxIsActiveSale.TabIndex = 125;
            chbxIsActiveSale.Text = "فعال للبيع";
            chbxIsActiveSale.UseVisualStyleBackColor = true;
            // 
            // chbxIsActiveBuy
            // 
            chbxIsActiveBuy.Anchor = AnchorStyles.Top;
            chbxIsActiveBuy.AutoSize = true;
            chbxIsActiveBuy.Checked = true;
            chbxIsActiveBuy.CheckState = CheckState.Checked;
            chbxIsActiveBuy.Location = new Point(575, 259);
            chbxIsActiveBuy.Margin = new Padding(4);
            chbxIsActiveBuy.Name = "chbxIsActiveBuy";
            chbxIsActiveBuy.Size = new Size(157, 33);
            chbxIsActiveBuy.TabIndex = 126;
            chbxIsActiveBuy.Text = "فعال للشراء";
            chbxIsActiveBuy.UseVisualStyleBackColor = true;
            // 
            // chbxPreventFraction
            // 
            chbxPreventFraction.Anchor = AnchorStyles.Top;
            chbxPreventFraction.AutoSize = true;
            chbxPreventFraction.Checked = true;
            chbxPreventFraction.CheckState = CheckState.Checked;
            chbxPreventFraction.Location = new Point(799, 259);
            chbxPreventFraction.Margin = new Padding(4);
            chbxPreventFraction.Name = "chbxPreventFraction";
            chbxPreventFraction.Size = new Size(211, 33);
            chbxPreventFraction.TabIndex = 127;
            chbxPreventFraction.Text = "منع البيع بالكسور";
            chbxPreventFraction.UseVisualStyleBackColor = true;
            // 
            // chbxPreventDiscount
            // 
            chbxPreventDiscount.Anchor = AnchorStyles.Top;
            chbxPreventDiscount.AutoSize = true;
            chbxPreventDiscount.Checked = true;
            chbxPreventDiscount.CheckState = CheckState.Checked;
            chbxPreventDiscount.Location = new Point(799, 318);
            chbxPreventDiscount.Margin = new Padding(4);
            chbxPreventDiscount.Name = "chbxPreventDiscount";
            chbxPreventDiscount.Size = new Size(144, 33);
            chbxPreventDiscount.TabIndex = 128;
            chbxPreventDiscount.Text = "منع الخصم";
            chbxPreventDiscount.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Top;
            label4.AutoSize = true;
            label4.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label4.Location = new Point(831, 476);
            label4.Margin = new Padding(4);
            label4.Name = "label4";
            label4.Size = new Size(133, 24);
            label4.TabIndex = 129;
            label4.Text = "هامش الربح:";
            // 
            // label8
            // 
            label8.Anchor = AnchorStyles.Top;
            label8.AutoSize = true;
            label8.Font = new Font("Tahoma", 10F);
            label8.Location = new Point(934, 514);
            label8.Margin = new Padding(0, 4, 0, 4);
            label8.Name = "label8";
            label8.Size = new Size(30, 24);
            label8.TabIndex = 131;
            label8.Text = "%";
            label8.Click += label8_Click;
            // 
            // chkStopProfitRate
            // 
            chkStopProfitRate.Anchor = AnchorStyles.Top;
            chkStopProfitRate.AutoSize = true;
            chkStopProfitRate.Location = new Point(851, 552);
            chkStopProfitRate.Margin = new Padding(4);
            chkStopProfitRate.Name = "chkStopProfitRate";
            chkStopProfitRate.Size = new Size(92, 33);
            chkStopProfitRate.TabIndex = 132;
            chkStopProfitRate.Text = "إيقاف";
            chkStopProfitRate.UseVisualStyleBackColor = true;
            chkStopProfitRate.CheckedChanged += chbxStopProfitRate_CheckedChanged;
            // 
            // txtSalePrice
            // 
            txtSalePrice.Anchor = AnchorStyles.Top;
            txtSalePrice.Enabled = false;
            txtSalePrice.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            txtSalePrice.Location = new Point(767, 436);
            txtSalePrice.Margin = new Padding(4);
            txtSalePrice.Name = "txtSalePrice";
            txtSalePrice.Size = new Size(261, 32);
            txtSalePrice.TabIndex = 133;
            txtSalePrice.KeyPress += txtAllowOnlyNumber_KeyPress;
            txtSalePrice.Validating += _ValidateEmptyTextBox;
            // 
            // nudProfitRate
            // 
            nudProfitRate.Anchor = AnchorStyles.Top;
            nudProfitRate.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            nudProfitRate.Location = new Point(861, 508);
            nudProfitRate.Margin = new Padding(4);
            nudProfitRate.Name = "nudProfitRate";
            nudProfitRate.ReadOnly = true;
            nudProfitRate.Size = new Size(72, 36);
            nudProfitRate.TabIndex = 134;
            nudProfitRate.TextAlign = HorizontalAlignment.Center;
            nudProfitRate.UpDownAlign = LeftRightAlignment.Left;
            nudProfitRate.Value = new decimal(new int[] { 20, 0, 0, 0 });
            nudProfitRate.ValueChanged += nudProfitRate_ValueChanged;
            nudProfitRate.MouseDown += nudProfitRate_MouseDown;
            nudProfitRate.MouseUp += nudProfitRate_MouseUp;
            // 
            // btnSaveAndClose
            // 
            btnSaveAndClose.Anchor = AnchorStyles.Top;
            btnSaveAndClose.BackColor = Color.White;
            btnSaveAndClose.FlatStyle = FlatStyle.Popup;
            btnSaveAndClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnSaveAndClose.Image = Properties.Resources.Save_32;
            btnSaveAndClose.ImageAlign = ContentAlignment.MiddleRight;
            btnSaveAndClose.Location = new Point(225, 552);
            btnSaveAndClose.Margin = new Padding(4);
            btnSaveAndClose.Name = "btnSaveAndClose";
            btnSaveAndClose.Size = new Size(185, 61);
            btnSaveAndClose.TabIndex = 135;
            btnSaveAndClose.Text = "إضافة وإغلاق";
            btnSaveAndClose.TextAlign = ContentAlignment.MiddleLeft;
            btnSaveAndClose.UseVisualStyleBackColor = false;
            btnSaveAndClose.Click += btnSaveAndClose_Click;
            // 
            // frmAddUpdateItem
            // 
            AcceptButton = btnSave;
            AutoScaleMode = AutoScaleMode.None;
            AutoValidate = AutoValidate.EnableAllowFocusChange;
            BackColor = Color.WhiteSmoke;
            CancelButton = btnClose;
            ClientSize = new Size(1413, 642);
            Controls.Add(btnSaveAndClose);
            Controls.Add(nudProfitRate);
            Controls.Add(txtSalePrice);
            Controls.Add(chkStopProfitRate);
            Controls.Add(label8);
            Controls.Add(label4);
            Controls.Add(chbxPreventDiscount);
            Controls.Add(chbxPreventFraction);
            Controls.Add(chbxIsActiveBuy);
            Controls.Add(chbxIsActiveSale);
            Controls.Add(cbxStores);
            Controls.Add(cbxCategories);
            Controls.Add(cbxCompanies);
            Controls.Add(pictureBox13);
            Controls.Add(pictureBox12);
            Controls.Add(pictureBox1);
            Controls.Add(pictureBox5);
            Controls.Add(pictureBox7);
            Controls.Add(pictureBox8);
            Controls.Add(pictureBox4);
            Controls.Add(pictureBox3);
            Controls.Add(pictureBox2);
            Controls.Add(pbItemImage);
            Controls.Add(label10);
            Controls.Add(txtItemName);
            Controls.Add(label13);
            Controls.Add(txtItemCode);
            Controls.Add(label2);
            Controls.Add(btnClose);
            Controls.Add(label12);
            Controls.Add(label9);
            Controls.Add(txtMinmumQty);
            Controls.Add(llRemoveImage);
            Controls.Add(llOpenFileDialog);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(cbxUnits);
            Controls.Add(btnSave);
            Controls.Add(txtBuyPrice);
            Controls.Add(label3);
            Controls.Add(pictureBox11);
            Controls.Add(lblMode);
            Controls.Add(lblItemID);
            Controls.Add(label1);
            Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(3, 4, 3, 4);
            Name = "frmAddUpdateItem";
            Padding = new Padding(4);
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "إضافة وتعديل بيانات صنف";
            FormClosing += frmAddUpdateItem_FormClosing;
            Load += frmAddUpdateItem_Load;
            ((System.ComponentModel.ISupportInitialize)errorProvider1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox11).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox7).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox8).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbItemImage).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox12).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox13).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudProfitRate).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblItemID;
        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.PictureBox pictureBox11;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private PictureBox pictureBox5;
        private PictureBox pictureBox7;
        private PictureBox pictureBox8;
        private PictureBox pictureBox4;
        private PictureBox pictureBox3;
        private PictureBox pictureBox2;
        private PictureBox pbItemImage;
        private Label label10;
        private TextBox txtItemName;
        private Label label13;
        private TextBox txtItemCode;
        private Label label2;
        private Button btnClose;
        private Label label12;
        private Label label9;
        private TextBox txtMinmumQty;
        private LinkLabel llRemoveImage;
        private LinkLabel llOpenFileDialog;
        private Label label5;
        private ComboBox cbxUnits;
        private Button btnSave;
        private TextBox txtBuyPrice;
        private Label label3;
        private PictureBox pictureBox13;
        private PictureBox pictureBox12;
        private PictureBox pictureBox1;
        private Label label7;
        private Label label6;
        private ComboBox cbxCompanies;
        private CheckBox chbxPreventDiscount;
        private CheckBox chbxPreventFraction;
        private CheckBox chbxIsActiveBuy;
        private CheckBox chbxIsActiveSale;
        private ComboBox cbxStores;
        private ComboBox cbxCategories;
        private Label label8;
        private Label label4;
        private CheckBox chkStopProfitRate;
        private TextBox txtSalePrice;
        private NumericUpDown nudProfitRate;
        private Button btnSaveAndClose;
    }
}