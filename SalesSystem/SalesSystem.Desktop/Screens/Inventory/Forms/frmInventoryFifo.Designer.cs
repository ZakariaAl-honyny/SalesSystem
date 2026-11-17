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
        components = new System.ComponentModel.Container();
        DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
        tooltip = new ToolTip(components);
        dgvLayers = new DataGridView();
        cmsLayers = new ContextMenuStrip(components);
        miCopyBatch = new ToolStripMenuItem();
        miClose = new ToolStripMenuItem();
        btnClose = new Button();
        pnlTitle = new Panel();
        lblMode = new Label();
        pnlInfo = new Panel();
        gbInfo = new GroupBox();
        tlpInfo = new TableLayoutPanel();
        lblProduct = new Label();
        lblWarehouse = new Label();
        lblMethodBadge = new Label();
        gbGrid = new GroupBox();
        lblEmptyState = new Label();
        pnlFooter = new Panel();
        gbTotals = new GroupBox();
        tlpFooter = new TableLayoutPanel();
        lblTotalQtyCaption = new Label();
        lblTotalQtyValue = new Label();
        lblTotalCostCaption = new Label();
        lblTotalCostValue = new Label();
        lblNote = new Label();
        ((System.ComponentModel.ISupportInitialize)dgvLayers).BeginInit();
        cmsLayers.SuspendLayout();
        pnlTitle.SuspendLayout();
        pnlInfo.SuspendLayout();
        gbInfo.SuspendLayout();
        tlpInfo.SuspendLayout();
        gbGrid.SuspendLayout();
        pnlFooter.SuspendLayout();
        gbTotals.SuspendLayout();
        tlpFooter.SuspendLayout();
        SuspendLayout();
        // 
        // tooltip
        // 
        tooltip.AutoPopDelay = 6000;
        tooltip.InitialDelay = 300;
        tooltip.ReshowDelay = 100;
        // 
        // dgvLayers
        // 
        dgvLayers.AllowUserToAddRows = false;
        dgvLayers.AllowUserToDeleteRows = false;
        dataGridViewCellStyle1.BackColor = Color.FromArgb(248, 249, 250);
        dgvLayers.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
        dgvLayers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvLayers.BackgroundColor = Color.White;
        dgvLayers.BorderStyle = BorderStyle.None;
        dgvLayers.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
        dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
        dataGridViewCellStyle2.BackColor = Color.FromArgb(52, 58, 64);
        dataGridViewCellStyle2.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        dataGridViewCellStyle2.ForeColor = Color.White;
        dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(52, 58, 64);
        dataGridViewCellStyle2.SelectionForeColor = Color.White;
        dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
        dgvLayers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
        dgvLayers.ColumnHeadersHeight = 46;
        dgvLayers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        dgvLayers.ContextMenuStrip = cmsLayers;
        dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle3.BackColor = Color.White;
        dataGridViewCellStyle3.Font = new Font("Tahoma", 10F);
        dataGridViewCellStyle3.ForeColor = Color.FromArgb(33, 37, 41);
        dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(255, 243, 205);
        dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(33, 37, 41);
        dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
        dgvLayers.DefaultCellStyle = dataGridViewCellStyle3;
        dgvLayers.Dock = DockStyle.Fill;
        dgvLayers.EnableHeadersVisualStyles = false;
        dgvLayers.GridColor = Color.FromArgb(222, 226, 230);
        dgvLayers.Location = new Point(8, 33);
        dgvLayers.MultiSelect = false;
        dgvLayers.Name = "dgvLayers";
        dgvLayers.ReadOnly = true;
        dgvLayers.RowHeadersVisible = false;
        dgvLayers.RowHeadersWidth = 62;
        dgvLayers.RowTemplate.Height = 30;
        dgvLayers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvLayers.Size = new Size(935, 323);
        dgvLayers.TabIndex = 0;
        tooltip.SetToolTip(dgvLayers, "الطبقة الأولى هي التالية للصرف");
        dgvLayers.CellFormatting += dgvLayers_CellFormatting;
        dgvLayers.RowPrePaint += dgvLayers_RowPrePaint;
        // 
        // cmsLayers
        // 
        cmsLayers.Font = new Font("Tahoma", 10F);
        cmsLayers.ImageScalingSize = new Size(24, 24);
        cmsLayers.Items.AddRange(new ToolStripItem[] { miCopyBatch, miClose });
        cmsLayers.Name = "cmsLayers";
        cmsLayers.RightToLeft = RightToLeft.Yes;
        cmsLayers.Size = new Size(237, 64);
        // 
        // miCopyBatch
        // 
        miCopyBatch.Name = "miCopyBatch";
        miCopyBatch.Size = new Size(236, 30);
        miCopyBatch.Text = "نسخ رقم التشغيلة";
        miCopyBatch.Click += miCopyBatch_Click;
        // 
        // miClose
        // 
        miClose.Name = "miClose";
        miClose.Size = new Size(236, 30);
        miClose.Text = "إغلاق";
        miClose.Click += miClose_Click;
        // 
        // btnClose
        // 
        btnClose.BackColor = Color.FromArgb(108, 117, 125);
        btnClose.Cursor = Cursors.Hand;
        btnClose.DialogResult = DialogResult.Cancel;
        btnClose.Dock = DockStyle.Fill;
        btnClose.FlatAppearance.BorderSize = 0;
        btnClose.FlatStyle = FlatStyle.Flat;
        btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnClose.ForeColor = Color.White;
        btnClose.Location = new Point(0, 0);
        btnClose.Margin = new Padding(12, 0, 0, 0);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(175, 38);
        btnClose.TabIndex = 4;
        btnClose.Text = "إغلاق";
        tooltip.SetToolTip(btnClose, "إغلاق الشاشة (Esc)");
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += btnClose_Click;
        // 
        // pnlTitle
        // 
        pnlTitle.BackColor = Color.FromArgb(16, 150, 96);
        pnlTitle.Controls.Add(lblMode);
        pnlTitle.Dock = DockStyle.Top;
        pnlTitle.Location = new Point(0, 0);
        pnlTitle.Name = "pnlTitle";
        pnlTitle.Size = new Size(951, 52);
        pnlTitle.TabIndex = 0;
        // 
        // lblMode
        // 
        lblMode.BackColor = Color.FromArgb(16, 150, 96);
        lblMode.Dock = DockStyle.Fill;
        lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold);
        lblMode.ForeColor = Color.White;
        lblMode.Location = new Point(0, 0);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(951, 52);
        lblMode.TabIndex = 0;
        lblMode.Text = "طبقات الصرف FIFO / FEFO";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlInfo
        // 
        pnlInfo.BackColor = Color.White;
        pnlInfo.Controls.Add(gbInfo);
        pnlInfo.Dock = DockStyle.Top;
        pnlInfo.Location = new Point(0, 52);
        pnlInfo.Name = "pnlInfo";
        pnlInfo.Padding = new Padding(8);
        pnlInfo.Size = new Size(951, 84);
        pnlInfo.TabIndex = 1;
        // 
        // gbInfo
        // 
        gbInfo.BackColor = Color.White;
        gbInfo.Controls.Add(tlpInfo);
        gbInfo.Dock = DockStyle.Fill;
        gbInfo.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        gbInfo.ForeColor = Color.FromArgb(33, 37, 41);
        gbInfo.Location = new Point(8, 8);
        gbInfo.Name = "gbInfo";
        gbInfo.Padding = new Padding(6);
        gbInfo.RightToLeft = RightToLeft.Yes;
        gbInfo.Size = new Size(935, 68);
        gbInfo.TabIndex = 0;
        gbInfo.TabStop = false;
        gbInfo.Text = "بيانات الصنف";
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
        tlpInfo.Location = new Point(6, 31);
        tlpInfo.Name = "tlpInfo";
        tlpInfo.RowCount = 1;
        tlpInfo.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlpInfo.Size = new Size(923, 31);
        tlpInfo.TabIndex = 0;
        // 
        // lblProduct
        // 
        lblProduct.Dock = DockStyle.Fill;
        lblProduct.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblProduct.ForeColor = Color.FromArgb(33, 37, 41);
        lblProduct.Location = new Point(612, 0);
        lblProduct.Margin = new Padding(3, 0, 12, 0);
        lblProduct.Name = "lblProduct";
        lblProduct.Size = new Size(308, 31);
        lblProduct.TabIndex = 0;
        lblProduct.Text = "الصنف: زيت نباتي 1 لتر";
        lblProduct.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblWarehouse
        // 
        lblWarehouse.Dock = DockStyle.Fill;
        lblWarehouse.Font = new Font("Tahoma", 10F);
        lblWarehouse.ForeColor = Color.FromArgb(33, 37, 41);
        lblWarehouse.Location = new Point(289, 0);
        lblWarehouse.Margin = new Padding(3, 0, 12, 0);
        lblWarehouse.Name = "lblWarehouse";
        lblWarehouse.Size = new Size(308, 31);
        lblWarehouse.TabIndex = 1;
        lblWarehouse.Text = "المستودع: المستودع الرئيسي";
        lblWarehouse.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblMethodBadge
        // 
        lblMethodBadge.BackColor = Color.FromArgb(16, 150, 96);
        lblMethodBadge.Dock = DockStyle.Fill;
        lblMethodBadge.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblMethodBadge.ForeColor = Color.White;
        lblMethodBadge.Location = new Point(0, 4);
        lblMethodBadge.Margin = new Padding(12, 4, 0, 4);
        lblMethodBadge.Name = "lblMethodBadge";
        lblMethodBadge.Padding = new Padding(8, 2, 8, 2);
        lblMethodBadge.Size = new Size(265, 23);
        lblMethodBadge.TabIndex = 2;
        lblMethodBadge.Text = "FIFO / FEFO";
        lblMethodBadge.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // gbGrid
        // 
        gbGrid.BackColor = Color.White;
        gbGrid.Controls.Add(dgvLayers);
        gbGrid.Controls.Add(lblEmptyState);
        gbGrid.Dock = DockStyle.Fill;
        gbGrid.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        gbGrid.ForeColor = Color.FromArgb(33, 37, 41);
        gbGrid.Location = new Point(0, 136);
        gbGrid.Name = "gbGrid";
        gbGrid.Padding = new Padding(8);
        gbGrid.RightToLeft = RightToLeft.Yes;
        gbGrid.Size = new Size(951, 364);
        gbGrid.TabIndex = 2;
        gbGrid.TabStop = false;
        gbGrid.Text = "طبقات المخزون مرتبة بأولوية الصرف";
        // 
        // lblEmptyState
        // 
        lblEmptyState.BackColor = Color.White;
        lblEmptyState.Dock = DockStyle.Fill;
        lblEmptyState.Font = new Font("Tahoma", 12F, FontStyle.Bold);
        lblEmptyState.ForeColor = Color.FromArgb(33, 37, 41);
        lblEmptyState.Location = new Point(8, 33);
        lblEmptyState.Name = "lblEmptyState";
        lblEmptyState.Size = new Size(935, 323);
        lblEmptyState.TabIndex = 1;
        lblEmptyState.Text = "لا توجد بيانات";
        lblEmptyState.TextAlign = ContentAlignment.MiddleCenter;
        lblEmptyState.Visible = false;
        // 
        // pnlFooter
        // 
        pnlFooter.BackColor = Color.White;
        pnlFooter.Controls.Add(gbTotals);
        pnlFooter.Dock = DockStyle.Bottom;
        pnlFooter.Location = new Point(0, 500);
        pnlFooter.Name = "pnlFooter";
        pnlFooter.Padding = new Padding(8);
        pnlFooter.Size = new Size(951, 120);
        pnlFooter.TabIndex = 3;
        // 
        // gbTotals
        // 
        gbTotals.BackColor = Color.White;
        gbTotals.Controls.Add(tlpFooter);
        gbTotals.Dock = DockStyle.Fill;
        gbTotals.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        gbTotals.ForeColor = Color.FromArgb(33, 37, 41);
        gbTotals.Location = new Point(8, 8);
        gbTotals.Name = "gbTotals";
        gbTotals.Padding = new Padding(6);
        gbTotals.RightToLeft = RightToLeft.Yes;
        gbTotals.Size = new Size(935, 104);
        gbTotals.TabIndex = 0;
        gbTotals.TabStop = false;
        gbTotals.Text = "الإجماليات";
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
        tlpFooter.Location = new Point(6, 31);
        tlpFooter.Name = "tlpFooter";
        tlpFooter.RowCount = 2;
        tlpFooter.RowStyles.Add(new RowStyle(SizeType.Percent, 58F));
        tlpFooter.RowStyles.Add(new RowStyle(SizeType.Percent, 42F));
        tlpFooter.Size = new Size(923, 67);
        tlpFooter.TabIndex = 0;
        // 
        // lblTotalQtyCaption
        // 
        lblTotalQtyCaption.Dock = DockStyle.Fill;
        lblTotalQtyCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblTotalQtyCaption.ForeColor = Color.FromArgb(33, 37, 41);
        lblTotalQtyCaption.Location = new Point(701, 0);
        lblTotalQtyCaption.Margin = new Padding(3, 0, 8, 0);
        lblTotalQtyCaption.Name = "lblTotalQtyCaption";
        lblTotalQtyCaption.Size = new Size(219, 38);
        lblTotalQtyCaption.TabIndex = 0;
        lblTotalQtyCaption.Text = "إجمالي الكمية المتبقية:";
        lblTotalQtyCaption.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblTotalQtyValue
        // 
        lblTotalQtyValue.Dock = DockStyle.Fill;
        lblTotalQtyValue.Font = new Font("Tahoma", 12F, FontStyle.Bold);
        lblTotalQtyValue.ForeColor = Color.FromArgb(33, 37, 41);
        lblTotalQtyValue.Location = new Point(567, 0);
        lblTotalQtyValue.Margin = new Padding(3, 0, 12, 0);
        lblTotalQtyValue.Name = "lblTotalQtyValue";
        lblTotalQtyValue.Size = new Size(123, 38);
        lblTotalQtyValue.TabIndex = 1;
        lblTotalQtyValue.Text = "0.000";
        lblTotalQtyValue.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblTotalCostCaption
        // 
        lblTotalCostCaption.Dock = DockStyle.Fill;
        lblTotalCostCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblTotalCostCaption.ForeColor = Color.FromArgb(33, 37, 41);
        lblTotalCostCaption.Location = new Point(333, 0);
        lblTotalCostCaption.Margin = new Padding(3, 0, 8, 0);
        lblTotalCostCaption.Name = "lblTotalCostCaption";
        lblTotalCostCaption.Size = new Size(219, 38);
        lblTotalCostCaption.TabIndex = 2;
        lblTotalCostCaption.Text = "إجمالي التكلفة:";
        lblTotalCostCaption.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblTotalCostValue
        // 
        lblTotalCostValue.Dock = DockStyle.Fill;
        lblTotalCostValue.Font = new Font("Tahoma", 12F, FontStyle.Bold);
        lblTotalCostValue.ForeColor = Color.FromArgb(33, 37, 41);
        lblTotalCostValue.Location = new Point(199, 0);
        lblTotalCostValue.Margin = new Padding(3, 0, 12, 0);
        lblTotalCostValue.Name = "lblTotalCostValue";
        lblTotalCostValue.Size = new Size(123, 38);
        lblTotalCostValue.TabIndex = 3;
        lblTotalCostValue.Text = "0.00";
        lblTotalCostValue.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblNote
        // 
        tlpFooter.SetColumnSpan(lblNote, 5);
        lblNote.Dock = DockStyle.Fill;
        lblNote.Font = new Font("Tahoma", 9F, FontStyle.Italic);
        lblNote.ForeColor = Color.FromArgb(33, 37, 41);
        lblNote.Location = new Point(3, 38);
        lblNote.Name = "lblNote";
        lblNote.Size = new Size(917, 29);
        lblNote.TabIndex = 5;
        lblNote.Text = "ملاحظة: يُصرف الأقدم أولاً (أو الأقرب صلاحية عند تفعيل FEFO) — التشغيلة التالية للصرف: B-2026-001";
        lblNote.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // frmInventoryFifo
        // 
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.FromArgb(240, 242, 245);
        CancelButton = btnClose;
        ClientSize = new Size(951, 620);
        Controls.Add(gbGrid);
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
        ((System.ComponentModel.ISupportInitialize)dgvLayers).EndInit();
        cmsLayers.ResumeLayout(false);
        pnlTitle.ResumeLayout(false);
        pnlInfo.ResumeLayout(false);
        gbInfo.ResumeLayout(false);
        tlpInfo.ResumeLayout(false);
        gbGrid.ResumeLayout(false);
        pnlFooter.ResumeLayout(false);
        gbTotals.ResumeLayout(false);
        tlpFooter.ResumeLayout(false);
        ResumeLayout(false);
    }

    private ToolTip tooltip;
    private ContextMenuStrip cmsLayers;
    private ToolStripMenuItem miCopyBatch;
    private ToolStripMenuItem miClose;
    private Panel pnlTitle;
    private Label lblMode;
    private Panel pnlInfo;
    private GroupBox gbInfo;
    private TableLayoutPanel tlpInfo;
    private Label lblProduct;
    private Label lblWarehouse;
    private Label lblMethodBadge;
    private GroupBox gbGrid;
    private DataGridView dgvLayers;
    private Label lblEmptyState;
    private Panel pnlFooter;
    private GroupBox gbTotals;
    private TableLayoutPanel tlpFooter;
    private Label lblTotalQtyCaption;
    private Label lblTotalQtyValue;
    private Label lblTotalCostCaption;
    private Label lblTotalCostValue;
    private Button btnClose;
    private Label lblNote;
}
