namespace SalesManagement.People.Controls
{
    partial class ctrlPersonCardWithFilter
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            errorProvider1 = new ErrorProvider(components);
            gbFilters = new GroupBox();
            txtFilterValue = new TextBox();
            label2 = new Label();
            btnFind = new Button();
            btnAddNewPerson = new Button();
            cbFilterBy = new ComboBox();
            ctrlPersonCard1 = new ctrlPersonCard();
            ((System.ComponentModel.ISupportInitialize)errorProvider1).BeginInit();
            gbFilters.SuspendLayout();
            SuspendLayout();
            // 
            // errorProvider1
            // 
            errorProvider1.ContainerControl = this;
            // 
            // gbFilters
            // 
            gbFilters.Controls.Add(txtFilterValue);
            gbFilters.Controls.Add(label2);
            gbFilters.Controls.Add(btnFind);
            gbFilters.Controls.Add(btnAddNewPerson);
            gbFilters.Controls.Add(cbFilterBy);
            gbFilters.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            gbFilters.Location = new Point(8, 5);
            gbFilters.Margin = new Padding(0);
            gbFilters.Name = "gbFilters";
            gbFilters.Padding = new Padding(4);
            gbFilters.Size = new Size(1115, 124);
            gbFilters.TabIndex = 6;
            gbFilters.TabStop = false;
            gbFilters.Text = "مربع البحث";
            // 
            // txtFilterValue
            // 
            txtFilterValue.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtFilterValue.Location = new Point(230, 51);
            txtFilterValue.Margin = new Padding(4);
            txtFilterValue.Name = "txtFilterValue";
            txtFilterValue.Size = new Size(393, 32);
            txtFilterValue.TabIndex = 1;
            txtFilterValue.KeyPress += txtFilterValue_KeyPress;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(897, 54);
            label2.Margin = new Padding(4);
            label2.Name = "label2";
            label2.Size = new Size(163, 24);
            label2.TabIndex = 10;
            label2.Text = "أبحث عن طريق:";
            // 
            // btnFind
            // 
            btnFind.FlatStyle = FlatStyle.Popup;
            btnFind.Image = Properties.Resources.SearchPerson;
            btnFind.Location = new Point(147, 29);
            btnFind.Margin = new Padding(4);
            btnFind.Name = "btnFind";
            btnFind.Size = new Size(75, 75);
            btnFind.TabIndex = 2;
            btnFind.UseVisualStyleBackColor = true;
            btnFind.Click += btnFind_Click;
            // 
            // btnAddNewPerson
            // 
            btnAddNewPerson.FlatStyle = FlatStyle.Popup;
            btnAddNewPerson.Image = Properties.Resources.AddPerson_32;
            btnAddNewPerson.Location = new Point(64, 29);
            btnAddNewPerson.Margin = new Padding(4);
            btnAddNewPerson.Name = "btnAddNewPerson";
            btnAddNewPerson.Size = new Size(75, 75);
            btnAddNewPerson.TabIndex = 3;
            btnAddNewPerson.UseVisualStyleBackColor = true;
            btnAddNewPerson.Click += btnAddNewPerson_Click;
            // 
            // cbFilterBy
            // 
            cbFilterBy.BackColor = SystemColors.Control;
            cbFilterBy.DropDownStyle = ComboBoxStyle.DropDownList;
            cbFilterBy.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            cbFilterBy.FormattingEnabled = true;
            cbFilterBy.Items.AddRange(new object[] { "الأسم", "رقم الشخص" });
            cbFilterBy.Location = new Point(631, 51);
            cbFilterBy.Margin = new Padding(4);
            cbFilterBy.Name = "cbFilterBy";
            cbFilterBy.Size = new Size(258, 32);
            cbFilterBy.TabIndex = 0;
            cbFilterBy.SelectedIndexChanged += cbFilterBy_SelectedIndexChanged;
            // 
            // ctrlPersonCard1
            // 
            ctrlPersonCard1.BackColor = Color.WhiteSmoke;
            ctrlPersonCard1.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ctrlPersonCard1.Location = new Point(0, 129);
            ctrlPersonCard1.Margin = new Padding(0);
            ctrlPersonCard1.Name = "ctrlPersonCard1";
            ctrlPersonCard1.Padding = new Padding(6);
            ctrlPersonCard1.RightToLeft = RightToLeft.Yes;
            ctrlPersonCard1.Size = new Size(1124, 421);
            ctrlPersonCard1.TabIndex = 7;
            // 
            // ctrlPersonCardWithFilter
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            Controls.Add(ctrlPersonCard1);
            Controls.Add(gbFilters);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(0);
            Name = "ctrlPersonCardWithFilter";
            RightToLeft = RightToLeft.Yes;
            Size = new Size(1131, 553);
            Load += ctrlPersonCardWithFilter_Load;
            ((System.ComponentModel.ISupportInitialize)errorProvider1).EndInit();
            gbFilters.ResumeLayout(false);
            gbFilters.PerformLayout();
            ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private GroupBox gbFilters;
        private TextBox txtFilterValue;
        private Label label2;
        private Button btnFind;
        private Button btnAddNewPerson;
        private ComboBox cbFilterBy;
        private ctrlPersonCard ctrlPersonCard1;
    }
}
