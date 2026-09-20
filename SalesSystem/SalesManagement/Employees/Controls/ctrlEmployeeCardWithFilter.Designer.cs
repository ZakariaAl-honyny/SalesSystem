using SalesManagement.People.Controls;

namespace SalesManagement.Employees.Controls
{
    partial class ctrlEmployeeCardWithFilter
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
            ctrlEmployeeCard1 = new ctrlEmployeeCard();
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
            gbFilters.Location = new Point(9, 5);
            gbFilters.Margin = new Padding(0);
            gbFilters.Name = "gbFilters";
            gbFilters.Padding = new Padding(4);
            gbFilters.Size = new Size(1116, 132);
            gbFilters.TabIndex = 6;
            gbFilters.TabStop = false;
            gbFilters.Text = "مربع البحث";
            // 
            // txtFilterValue
            // 
            txtFilterValue.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtFilterValue.Location = new Point(204, 55);
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
            label2.Location = new Point(871, 58);
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
            btnFind.Location = new Point(121, 33);
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
            btnAddNewPerson.Location = new Point(38, 33);
            btnAddNewPerson.Margin = new Padding(4);
            btnAddNewPerson.Name = "btnAddNewPerson";
            btnAddNewPerson.Size = new Size(75, 75);
            btnAddNewPerson.TabIndex = 3;
            btnAddNewPerson.UseVisualStyleBackColor = true;
            btnAddNewPerson.Click += btnAddNewEmployee_Click;
            // 
            // cbFilterBy
            // 
            cbFilterBy.BackColor = SystemColors.Control;
            cbFilterBy.DropDownStyle = ComboBoxStyle.DropDownList;
            cbFilterBy.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            cbFilterBy.FormattingEnabled = true;
            cbFilterBy.Items.AddRange(new object[] { "الأسم", "رقم الموظف" });
            cbFilterBy.Location = new Point(605, 54);
            cbFilterBy.Margin = new Padding(4);
            cbFilterBy.Name = "cbFilterBy";
            cbFilterBy.Size = new Size(258, 32);
            cbFilterBy.TabIndex = 0;
            cbFilterBy.SelectedIndexChanged += cbFilterBy_SelectedIndexChanged;
            // 
            // ctrlEmployeeCard1
            // 
            ctrlEmployeeCard1.BackColor = Color.WhiteSmoke;
            ctrlEmployeeCard1.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ctrlEmployeeCard1.Location = new Point(0, 137);
            ctrlEmployeeCard1.Margin = new Padding(0);
            ctrlEmployeeCard1.Name = "ctrlEmployeeCard1";
            ctrlEmployeeCard1.RightToLeft = RightToLeft.Yes;
            ctrlEmployeeCard1.Size = new Size(1128, 562);
            ctrlEmployeeCard1.TabIndex = 7;
            // 
            // ctrlEmployeeCardWithFilter
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            Controls.Add(ctrlEmployeeCard1);
            Controls.Add(gbFilters);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(0);
            Name = "ctrlEmployeeCardWithFilter";
            RightToLeft = RightToLeft.Yes;
            Size = new Size(1135, 700);
            Load += ctrlEmployeeCardWithFilter_Load;
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
        private ctrlEmployeeCard ctrlEmployeeCard1;
    }
}
