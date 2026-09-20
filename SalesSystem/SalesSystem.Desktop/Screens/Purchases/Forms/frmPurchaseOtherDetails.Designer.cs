namespace SalesSystem.Desktop.Screens.Purchases.Forms;

partial class frmPurchaseOtherDetails
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

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        pnlTitle = new Panel();
        lblTitle = new Label();
        pnlBody = new Panel();
        tblMain = new TableLayoutPanel();
        grpCosts = new GroupBox();
        tblCosts = new TableLayoutPanel();
        lblShipping = new Label();
        numShipping = new NumericUpDown();
        lblCustoms = new Label();
        numCustoms = new NumericUpDown();
        lblOther = new Label();
        tblOtherRow = new TableLayoutPanel();
        numOther = new NumericUpDown();
        lblStatementCaption = new Label();
        txtStatement = new TextBox();
        pnlTotal = new Panel();
        tblTotal = new TableLayoutPanel();
        lblTotalCaption = new Label();
        lblTotalValue = new Label();
        pnlNotes = new Panel();
        lblNotesCaption = new Label();
        txtNotes = new TextBox();
        lblNote = new Label();
        pnlFooter = new Panel();
        lblFooterLine = new Label();
        flpFooter = new FlowLayoutPanel();
        btnOk = new Button();
        btnCancel = new Button();
        pnlTitle.SuspendLayout();
        pnlBody.SuspendLayout();
        tblMain.SuspendLayout();
        grpCosts.SuspendLayout();
        tblCosts.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numShipping).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numCustoms).BeginInit();
        tblOtherRow.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numOther).BeginInit();
        pnlTotal.SuspendLayout();
        tblTotal.SuspendLayout();
        pnlNotes.SuspendLayout();
        pnlFooter.SuspendLayout();
        flpFooter.SuspendLayout();
        SuspendLayout();
        // 
        // pnlTitle
        // 
        pnlTitle.BackColor = Color.Firebrick;
        pnlTitle.Controls.Add(lblTitle);
        pnlTitle.Dock = DockStyle.Top;
        pnlTitle.Name = "pnlTitle";
        pnlTitle.Size = new Size(520, 46);
        pnlTitle.TabIndex = 0;
        // 
        // lblTitle
        // 
        lblTitle.Dock = DockStyle.Fill;
        lblTitle.Font = new Font("Tahoma", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblTitle.ForeColor = Color.White;
        lblTitle.Name = "lblTitle";
        lblTitle.Text = "تفاصيل وتكاليف إضافية";
        lblTitle.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlBody
        // 
        pnlBody.BackColor = Color.WhiteSmoke;
        pnlBody.Controls.Add(tblMain);
        pnlBody.Dock = DockStyle.Fill;
        pnlBody.Name = "pnlBody";
        pnlBody.Padding = new Padding(10, 8, 10, 8);
        pnlBody.TabIndex = 1;
        // 
        // tblMain
        // 
        tblMain.ColumnCount = 1;
        tblMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tblMain.Controls.Add(grpCosts, 0, 0);
        tblMain.Controls.Add(pnlTotal, 0, 1);
        tblMain.Controls.Add(pnlNotes, 0, 2);
        tblMain.Controls.Add(lblNote, 0, 3);
        tblMain.Dock = DockStyle.Fill;
        tblMain.Name = "tblMain";
        tblMain.RowCount = 5;
        tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 150F));
        tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
        tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 132F));
        tblMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 62F));
        tblMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        // 
        // grpCosts
        // 
        grpCosts.BackColor = Color.White;
        grpCosts.Controls.Add(tblCosts);
        grpCosts.Dock = DockStyle.Fill;
        grpCosts.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        grpCosts.ForeColor = Color.FromArgb(33, 43, 54);
        grpCosts.Name = "grpCosts";
        grpCosts.Padding = new Padding(10, 4, 10, 6);
        grpCosts.Text = "التكاليف الإضافية";
        // 
        // tblCosts
        // 
        tblCosts.ColumnCount = 2;
        tblCosts.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
        tblCosts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tblCosts.Controls.Add(lblShipping, 0, 0);
        tblCosts.Controls.Add(numShipping, 1, 0);
        tblCosts.Controls.Add(lblCustoms, 0, 1);
        tblCosts.Controls.Add(numCustoms, 1, 1);
        tblCosts.Controls.Add(lblOther, 0, 2);
        tblCosts.Controls.Add(tblOtherRow, 1, 2);
        tblCosts.Dock = DockStyle.Fill;
        tblCosts.Name = "tblCosts";
        tblCosts.RowCount = 4;
        tblCosts.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
        tblCosts.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
        tblCosts.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
        tblCosts.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        // 
        // lblShipping
        // 
        lblShipping.Dock = DockStyle.Fill;
        lblShipping.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblShipping.ForeColor = Color.FromArgb(80, 90, 100);
        lblShipping.Name = "lblShipping";
        lblShipping.Text = "تكلفة الشحن:";
        lblShipping.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // numShipping
        // 
        numShipping.DecimalPlaces = 2;
        numShipping.Dock = DockStyle.Fill;
        numShipping.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        numShipping.Maximum = 9999999;
        numShipping.Minimum = 0;
        numShipping.Name = "numShipping";
        numShipping.ThousandsSeparator = true;
        numShipping.ValueChanged += numCost_ValueChanged;
        // 
        // lblCustoms
        // 
        lblCustoms.Dock = DockStyle.Fill;
        lblCustoms.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblCustoms.ForeColor = Color.FromArgb(80, 90, 100);
        lblCustoms.Name = "lblCustoms";
        lblCustoms.Text = "رسوم الجمارك:";
        lblCustoms.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // numCustoms
        // 
        numCustoms.DecimalPlaces = 2;
        numCustoms.Dock = DockStyle.Fill;
        numCustoms.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        numCustoms.Maximum = 9999999;
        numCustoms.Minimum = 0;
        numCustoms.Name = "numCustoms";
        numCustoms.ThousandsSeparator = true;
        numCustoms.ValueChanged += numCost_ValueChanged;
        // 
        // lblOther
        // 
        lblOther.Dock = DockStyle.Fill;
        lblOther.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblOther.ForeColor = Color.FromArgb(80, 90, 100);
        lblOther.Name = "lblOther";
        lblOther.Text = "تكاليف أخرى:";
        lblOther.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // tblOtherRow
        // 
        tblOtherRow.ColumnCount = 3;
        tblOtherRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
        tblOtherRow.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        tblOtherRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tblOtherRow.Controls.Add(numOther, 0, 0);
        tblOtherRow.Controls.Add(lblStatementCaption, 1, 0);
        tblOtherRow.Controls.Add(txtStatement, 2, 0);
        tblOtherRow.Dock = DockStyle.Fill;
        tblOtherRow.Name = "tblOtherRow";
        tblOtherRow.RowCount = 1;
        tblOtherRow.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        // 
        // numOther
        // 
        numOther.DecimalPlaces = 2;
        numOther.Dock = DockStyle.Fill;
        numOther.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        numOther.Maximum = 9999999;
        numOther.Minimum = 0;
        numOther.Name = "numOther";
        numOther.ThousandsSeparator = true;
        numOther.ValueChanged += numCost_ValueChanged;
        // 
        // lblStatementCaption
        // 
        lblStatementCaption.Dock = DockStyle.Fill;
        lblStatementCaption.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblStatementCaption.ForeColor = Color.FromArgb(80, 90, 100);
        lblStatementCaption.Margin = new Padding(8, 0, 8, 0);
        lblStatementCaption.Name = "lblStatementCaption";
        lblStatementCaption.Text = "بيان:";
        lblStatementCaption.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // txtStatement
        // 
        txtStatement.BorderStyle = BorderStyle.FixedSingle;
        txtStatement.Dock = DockStyle.Fill;
        txtStatement.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        txtStatement.Name = "txtStatement";
        txtStatement.PlaceholderText = "مثال: تغليف، تحميل، رسوم تخليص...";
        // 
        // pnlTotal
        // 
        pnlTotal.BackColor = Color.FromArgb(232, 244, 253);
        pnlTotal.Controls.Add(tblTotal);
        pnlTotal.Dock = DockStyle.Fill;
        pnlTotal.Name = "pnlTotal";
        pnlTotal.Padding = new Padding(10, 4, 10, 4);
        // 
        // tblTotal
        // 
        tblTotal.ColumnCount = 2;
        tblTotal.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 190F));
        tblTotal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tblTotal.Controls.Add(lblTotalCaption, 0, 0);
        tblTotal.Controls.Add(lblTotalValue, 1, 0);
        tblTotal.Dock = DockStyle.Fill;
        tblTotal.Name = "tblTotal";
        tblTotal.RowCount = 1;
        tblTotal.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        // 
        // lblTotalCaption
        // 
        lblTotalCaption.Dock = DockStyle.Fill;
        lblTotalCaption.Font = new Font("Tahoma", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblTotalCaption.ForeColor = Color.FromArgb(33, 43, 54);
        lblTotalCaption.Name = "lblTotalCaption";
        lblTotalCaption.Text = "إجمالي التكاليف الإضافية:";
        lblTotalCaption.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblTotalValue
        // 
        lblTotalValue.Dock = DockStyle.Fill;
        lblTotalValue.Font = new Font("Tahoma", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblTotalValue.ForeColor = Color.FromArgb(21, 87, 36);
        lblTotalValue.Name = "lblTotalValue";
        lblTotalValue.Text = "0.00";
        lblTotalValue.TextAlign = ContentAlignment.MiddleRight;
        // 
        // pnlNotes
        // 
        pnlNotes.BackColor = Color.White;
        pnlNotes.Controls.Add(lblNotesCaption);
        pnlNotes.Controls.Add(txtNotes);
        pnlNotes.Dock = DockStyle.Fill;
        pnlNotes.Name = "pnlNotes";
        pnlNotes.Padding = new Padding(10, 4, 10, 4);
        // 
        // lblNotesCaption
        // 
        lblNotesCaption.Dock = DockStyle.Top;
        lblNotesCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblNotesCaption.ForeColor = Color.FromArgb(108, 117, 125);
        lblNotesCaption.Height = 26;
        lblNotesCaption.Name = "lblNotesCaption";
        lblNotesCaption.Text = "ملاحظات:";
        lblNotesCaption.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtNotes
        // 
        txtNotes.AcceptsReturn = true;
        txtNotes.BorderStyle = BorderStyle.FixedSingle;
        txtNotes.Dock = DockStyle.Fill;
        txtNotes.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        txtNotes.Multiline = true;
        txtNotes.Name = "txtNotes";
        txtNotes.PlaceholderText = "اكتب أي ملاحظات إضافية على فاتورة الشراء...";
        txtNotes.ScrollBars = ScrollBars.Vertical;
        // 
        // lblNote
        // 
        lblNote.BackColor = Color.FromArgb(255, 249, 231);
        lblNote.Dock = DockStyle.Fill;
        lblNote.Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblNote.ForeColor = Color.FromArgb(133, 100, 4);
        lblNote.Name = "lblNote";
        lblNote.Text = "التكاليف الإضافية توزّع تلقائياً على بنود الفاتورة (Landed Cost)";
        lblNote.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlFooter
        // 
        pnlFooter.BackColor = Color.FromArgb(248, 249, 250);
        pnlFooter.Controls.Add(flpFooter);
        pnlFooter.Controls.Add(lblFooterLine);
        pnlFooter.Dock = DockStyle.Bottom;
        pnlFooter.Name = "pnlFooter";
        pnlFooter.Size = new Size(520, 60);
        pnlFooter.TabIndex = 3;
        // 
        // lblFooterLine
        // 
        lblFooterLine.BackColor = Color.FromArgb(222, 226, 230);
        lblFooterLine.Dock = DockStyle.Top;
        lblFooterLine.Height = 1;
        lblFooterLine.Name = "lblFooterLine";
        // 
        // flpFooter
        // 
        flpFooter.Controls.Add(btnOk);
        flpFooter.Controls.Add(btnCancel);
        flpFooter.Dock = DockStyle.Fill;
        flpFooter.FlowDirection = FlowDirection.RightToLeft;
        flpFooter.Name = "flpFooter";
        flpFooter.Padding = new Padding(8, 8, 8, 6);
        // 
        // btnOk
        // 
        btnOk.BackColor = Color.FromArgb(25, 135, 84);
        btnOk.FlatStyle = FlatStyle.Popup;
        btnOk.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnOk.ForeColor = Color.White;
        btnOk.Height = 40;
        btnOk.Margin = new Padding(0, 0, 8, 0);
        btnOk.Name = "btnOk";
        btnOk.Size = new Size(150, 40);
        btnOk.TabIndex = 0;
        btnOk.Text = "موافق";
        btnOk.UseVisualStyleBackColor = false;
        btnOk.Click += btnOk_Click;
        // 
        // btnCancel
        // 
        btnCancel.BackColor = Color.White;
        btnCancel.DialogResult = DialogResult.Cancel;
        btnCancel.FlatStyle = FlatStyle.Popup;
        btnCancel.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnCancel.ForeColor = Color.FromArgb(33, 43, 54);
        btnCancel.Height = 40;
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(90, 40);
        btnCancel.TabIndex = 1;
        btnCancel.Text = "إلغاء";
        btnCancel.UseVisualStyleBackColor = false;
        btnCancel.Click += btnCancel_Click;
        // 
        // frmPurchaseOtherDetails
        // 
        AcceptButton = btnOk;
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.WhiteSmoke;
        CancelButton = btnCancel;
        ClientSize = new Size(520, 560);
        Controls.Add(pnlBody);
        Controls.Add(pnlFooter);
        Controls.Add(pnlTitle);
        Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "frmPurchaseOtherDetails";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "تفاصيل وتكاليف إضافية";
        pnlTitle.ResumeLayout(false);
        pnlBody.ResumeLayout(false);
        tblMain.ResumeLayout(false);
        grpCosts.ResumeLayout(false);
        tblCosts.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)numShipping).EndInit();
        ((System.ComponentModel.ISupportInitialize)numCustoms).EndInit();
        tblOtherRow.ResumeLayout(false);
        tblOtherRow.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numOther).EndInit();
        pnlTotal.ResumeLayout(false);
        tblTotal.ResumeLayout(false);
        pnlNotes.ResumeLayout(false);
        pnlFooter.ResumeLayout(false);
        flpFooter.ResumeLayout(false);
        ResumeLayout(false);
    }

    #endregion

    private Panel pnlTitle;
    private Label lblTitle;
    private Panel pnlBody;
    private TableLayoutPanel tblMain;
    private GroupBox grpCosts;
    private TableLayoutPanel tblCosts;
    private Label lblShipping;
    private NumericUpDown numShipping;
    private Label lblCustoms;
    private NumericUpDown numCustoms;
    private Label lblOther;
    private TableLayoutPanel tblOtherRow;
    private NumericUpDown numOther;
    private Label lblStatementCaption;
    private TextBox txtStatement;
    private Panel pnlTotal;
    private TableLayoutPanel tblTotal;
    private Label lblTotalCaption;
    private Label lblTotalValue;
    private Panel pnlNotes;
    private Label lblNotesCaption;
    private TextBox txtNotes;
    private Label lblNote;
    private Panel pnlFooter;
    private Label lblFooterLine;
    private FlowLayoutPanel flpFooter;
    private Button btnOk;
    private Button btnCancel;
}