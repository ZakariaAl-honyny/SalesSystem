namespace SalesSystem.Desktop.Screens.Inventory.Forms;

partial class frmInventoryFifo
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            components?.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        pnlTitle = new Panel();
        lblMode = new Label();
        pnlInfo = new Panel();
        tlpInfo = new TableLayoutPanel();
        lblProduct = new Label();
        lblWarehouse = new Label();
        lblMethodBadge = new Label();
        dgvLayers = new DataGridView();
        pnlFooter = new Panel();
        tlpFooter = new TableLayoutPanel();
        lblTotalQtyCaption = new Label();
        lblTotalQtyValue = new Label();
        lblTotalCostCaption = new Label();
        lblTotalCostValue = new Label();
        btnClose = new Button();
        lblNote = new Label();
        pnlTitle.SuspendLayout();
        pnlInfo.SuspendLayout();
        tlpInfo.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvLayers).BeginInit();
        pnlFooter.SuspendLayout();
        tlpFooter.SuspendLayout();
        SuspendLayout();
        // 
        // pnlTitle
        // 
        pnlTitle.BackColor = Color.Firebrick;
        pnlTitle.Controls.Add(lblMode);
        pnlTitle.Dock = DockStyle.Top;
        pnlTitle.Location = new Point(0, 0);
        pnlTitle.Name = "pnlTitle";
        pnlTitle.Size = new Size(700, 52);
        pnlTitle.TabIndex = 3;
        // 
        // lblMode
        // 
        lblMode.Dock = DockStyle.Fill;
        lblMode.Font = new Font("Tahoma", 18F, FontStyle.Bold);
        lblMode.ForeColor = Color.White;
        lblMode.Location = new Point(0, 0);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(700, 52);
        lblMode.TabIndex = 0;
        lblMode.Text = "طبقات FIFO/FEFO";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlInfo
        // 
        pnlInfo.BackColor = Color.White;
        pnlInfo.Controls.Add(tlpInfo);
        pnlInfo.Dock = DockStyle.Top;
        pnlInfo.Location = new Point(0, 52);
        pnlInfo.Name = "pnlInfo";
        pnlInfo.Padding = new Padding(12, 8, 12, 8);
        pnlInfo.Size = new Size(700, 64);
        pnlInfo.TabIndex = 4;
        // 
        // tlpInfo
        // 
        tlpInfo.ColumnCount = 3;
        tlpInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
        tlpInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
        tlpInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
        tlpInfo.Controls.Add(lblProduct, 0, 0);
        tlpInfo.Controls.Add(lblWarehouse, 1, 0);
        tlpInfo.Controls.Add(lblMethodBadge, 2, 0);
        tlpInfo.Dock = DockStyle.Fill;
        tlpInfo.Location = new Point(12, 8);
        tlpInfo.Name = "tlpInfo";
        tlpInfo.RowCount = 1;
        tlpInfo.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlpInfo.Size = new Size(676, 48);
        tlpInfo.TabIndex = 0;
        // 
        // lblProduct
        // 
        lblProduct.Dock = DockStyle.Fill;
        lblProduct.Font = new Font("Tahoma", 11F, FontStyle.Bold);
        lblProduct.Location = new Point(3, 0);
        lblProduct.Margin = new Padding(3, 0, 16, 0);
        lblProduct.Name = "lblProduct";
        lblProduct.Size = new Size(218, 48);
        lblProduct.TabIndex = 0;
        lblProduct.Text = "الصنف: زيت نباتي 1 لتر";
        lblProduct.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblWarehouse
        // 
        lblWarehouse.Dock = DockStyle.Fill;
        lblWarehouse.Font = new Font("Tahoma", 11F);
        lblWarehouse.Location = new Point(244, 0);
        lblWarehouse.Margin = new Padding(3, 0, 16, 0);
        lblWarehouse.Name = "lblWarehouse";
        lblWarehouse.Size = new Size(218, 48);
        lblWarehouse.TabIndex = 1;
        lblWarehouse.Text = "المستودع: المستودع الرئيسي";
        lblWarehouse.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblMethodBadge
        // 
        lblMethodBadge.BackColor = Color.SeaGreen;
        lblMethodBadge.Dock = DockStyle.Fill;
        lblMethodBadge.Font = new Font("Tahoma", 11F, FontStyle.Bold);
        lblMethodBadge.ForeColor = Color.White;
        lblMethodBadge.Location = new Point(485, 0);
        lblMethodBadge.Margin = new Padding(16, 8, 0, 8);
        lblMethodBadge.Name = "lblMethodBadge";
        lblMethodBadge.Padding = new Padding(8, 2, 8, 2);
        lblMethodBadge.Size = new Size(191, 48);
        lblMethodBadge.TabIndex = 2;
        lblMethodBadge.Text = "FIFO / FEFO";
        lblMethodBadge.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // dgvLayers
        // 
        dgvLayers.AllowUserToAddRows = false;
        dgvLayers.AllowUserToDeleteRows = false;
        dgvLayers.AllowUserToOrderColumns = false;
        dgvLayers.AutoGenerateColumns = false;
        dgvLayers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvLayers.BackgroundColor = SystemColors.Window;
        dgvLayers.BorderStyle = BorderStyle.Fixed3D;
        dgvLayers.ColumnHeadersHeight = 50;
        dgvLayers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        dgvLayers.Dock = DockStyle.Fill;
        dgvLayers.Font = new Font("Tahoma", 10F);
        dgvLayers.Location = new Point(0, 116);
        dgvLayers.MultiSelect = false;
        dgvLayers.Name = "dgvLayers";
        dgvLayers.ReadOnly = true;
        dgvLayers.RowHeadersWidth = 62;
        dgvLayers.RowTemplate.Height = 28;
        dgvLayers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvLayers.Size = new Size(700, 336);
        dgvLayers.TabIndex = 2;
        dgvLayers.TabStop = false;
        dgvLayers.CellFormatting += dgvLayers_CellFormatting;
        dgvLayers.RowPrePaint += dgvLayers_RowPrePaint;
        // 
        // pnlFooter
        // 
        pnlFooter.BackColor = Color.White;
        pnlFooter.Controls.Add(tlpFooter);
        pnlFooter.Dock = DockStyle.Bottom;
        pnlFooter.Location = new Point(0, 452);
        pnlFooter.Name = "pnlFooter";
        pnlFooter.Padding = new Padding(12, 8, 12, 8);
        pnlFooter.Size = new Size(700, 108);
        pnlFooter.TabIndex = 5;
        // 
        // tlpFooter
        // 
        tlpFooter.ColumnCount = 5;
        tlpFooter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        tlpFooter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
        tlpFooter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        tlpFooter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
        tlpFooter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
        tlpFooter.Controls.Add(lblTotalQtyCaption, 0, 0);
        tlpFooter.Controls.Add(lblTotalQtyValue, 1, 0);
        tlpFooter.Controls.Add(lblTotalCostCaption, 2, 0);
        tlpFooter.Controls.Add(lblTotalCostValue, 3, 0);
        tlpFooter.Controls.Add(btnClose, 4, 0);
        tlpFooter.Controls.Add(lblNote, 0, 1);
        tlpFooter.Dock = DockStyle.Fill;
        tlpFooter.Location = new Point(12, 8);
        tlpFooter.Name = "tlpFooter";
        tlpFooter.RowCount = 2;
        tlpFooter.RowStyles.Add(new RowStyle(SizeType.Percent, 55F));
        tlpFooter.RowStyles.Add(new RowStyle(SizeType.Percent, 45F));
        tlpFooter.Size = new Size(676, 92);
        tlpFooter.TabIndex = 0;
        tlpFooter.SetColumnSpan(lblNote, 5);
        // 
        // lblTotalQtyCaption
        // 
        lblTotalQtyCaption.Dock = DockStyle.Fill;
        lblTotalQtyCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblTotalQtyCaption.Location = new Point(3, 0);
        lblTotalQtyCaption.Margin = new Padding(3, 0, 8, 0);
        lblTotalQtyCaption.Name = "lblTotalQtyCaption";
        lblTotalQtyCaption.Size = new Size(163, 50);
        lblTotalQtyCaption.TabIndex = 0;
        lblTotalQtyCaption.Text = "إجمالي الكمية المتبقية:";
        lblTotalQtyCaption.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblTotalQtyValue
        // 
        lblTotalQtyValue.Dock = DockStyle.Fill;
        lblTotalQtyValue.Font = new Font("Tahoma", 12F, FontStyle.Bold);
        lblTotalQtyValue.ForeColor = Color.SeaGreen;
        lblTotalQtyValue.Location = new Point(177, 0);
        lblTotalQtyValue.Margin = new Padding(3, 0, 16, 0);
        lblTotalQtyValue.Name = "lblTotalQtyValue";
        lblTotalQtyValue.Size = new Size(85, 50);
        lblTotalQtyValue.TabIndex = 1;
        lblTotalQtyValue.Text = "325.000";
        lblTotalQtyValue.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblTotalCostCaption
        // 
        lblTotalCostCaption.Dock = DockStyle.Fill;
        lblTotalCostCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblTotalCostCaption.Location = new Point(283, 0);
        lblTotalCostCaption.Margin = new Padding(3, 0, 8, 0);
        lblTotalCostCaption.Name = "lblTotalCostCaption";
        lblTotalCostCaption.Size = new Size(163, 50);
        lblTotalCostCaption.TabIndex = 2;
        lblTotalCostCaption.Text = "إجمالي التكلفة:";
        lblTotalCostCaption.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblTotalCostValue
        // 
        lblTotalCostValue.Dock = DockStyle.Fill;
        lblTotalCostValue.Font = new Font("Tahoma", 12F, FontStyle.Bold);
        lblTotalCostValue.ForeColor = Color.SeaGreen;
        lblTotalCostValue.Location = new Point(457, 0);
        lblTotalCostValue.Margin = new Padding(3, 0, 16, 0);
        lblTotalCostValue.Name = "lblTotalCostValue";
        lblTotalCostValue.Size = new Size(85, 50);
        lblTotalCostValue.TabIndex = 3;
        lblTotalCostValue.Text = "1,527.25";
        lblTotalCostValue.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // btnClose
        // 
        btnClose.BackColor = Color.White;
        btnClose.DialogResult = DialogResult.Cancel;
        btnClose.Dock = DockStyle.Fill;
        btnClose.FlatAppearance.BorderColor = Color.Black;
        btnClose.FlatAppearance.BorderSize = 2;
        btnClose.FlatStyle = FlatStyle.Popup;
        btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnClose.Location = new Point(565, 3);
        btnClose.Margin = new Padding(16, 0, 0, 0);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(108, 44);
        btnClose.TabIndex = 4;
        btnClose.Text = "إغلاق";
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += btnClose_Click;
        // 
        // lblNote
        // 
        lblNote.Dock = DockStyle.Fill;
        lblNote.Font = new Font("Tahoma", 9F, FontStyle.Italic);
        lblNote.ForeColor = Color.FromArgb(96, 96, 96);
        lblNote.Location = new Point(3, 55);
        lblNote.Name = "lblNote";
        lblNote.Size = new Size(670, 37);
        lblNote.TabIndex = 5;
        lblNote.Text = "ملاحظة: يُصرف الأقدم أولاً (أو الأقرب صلاحية عند تفعيل FEFO) — التشغيلة التالية للصرف: B-2026-001";
        lblNote.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // frmInventoryFifo
        // 
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.WhiteSmoke;
        CancelButton = btnClose;
        ClientSize = new Size(700, 560);
        Controls.Add(dgvLayers);
        Controls.Add(pnlFooter);
        Controls.Add(pnlInfo);
        Controls.Add(pnlTitle);
        Font = new Font("Tahoma", 12F);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "frmInventoryFifo";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "طبقات FIFO/FEFO";
        Load += frmInventoryFifo_Load;
        pnlTitle.ResumeLayout(false);
        pnlInfo.ResumeLayout(false);
        tlpInfo.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvLayers).EndInit();
        pnlFooter.ResumeLayout(false);
        tlpFooter.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    private Panel pnlTitle;
    private Label lblMode;
    private Panel pnlInfo;
    private TableLayoutPanel tlpInfo;
    private Label lblProduct;
    private Label lblWarehouse;
    private Label lblMethodBadge;
    private DataGridView dgvLayers;
    private Panel pnlFooter;
    private TableLayoutPanel tlpFooter;
    private Label lblTotalQtyCaption;
    private Label lblTotalQtyValue;
    private Label lblTotalCostCaption;
    private Label lblTotalCostValue;
    private Button btnClose;
    private Label lblNote;
}