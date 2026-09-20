namespace SalesSystem.Desktop.Screens.Reports.Forms;

partial class frmLowStockReport
{
    private System.ComponentModel.IContainer components = null;
    private System.Windows.Forms.Button btnClose;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        btnClose = new System.Windows.Forms.Button();
        SuspendLayout();
        // 
        // btnClose
        // 
        btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnClose.BackColor = Color.White;
        btnClose.DialogResult = DialogResult.Cancel;
        btnClose.FlatStyle = FlatStyle.Popup;
        btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnClose.Location = new Point(12, 580);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(140, 50);
        btnClose.TabIndex = 0;
        btnClose.Text = "\u0625\u063a\u0644\u0627\u0642";
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += new System.EventHandler(this.btnClose_Click);
        // 
        // frmLowStockReport
        // 
        AcceptButton = btnClose;
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.WhiteSmoke;
        CancelButton = btnClose;
        ClientSize = new Size(1200, 650);
        Font = new Font("Tahoma", 12F);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        Name = "frmLowStockReport";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "\u062a\u0646\u0628\u064a\u0647 \u0627\u0644\u0645\u062e\u0632\u0648\u0646 \u0627\u0644\u0645\u0646\u062e\u0641\u0636";
        ResumeLayout(false);
        PerformLayout();
    }
}
