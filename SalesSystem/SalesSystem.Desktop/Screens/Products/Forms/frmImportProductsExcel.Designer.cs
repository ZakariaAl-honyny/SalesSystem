namespace SalesSystem.Desktop.Screens.Products.Forms;

partial class frmImportProductsExcel
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

    private void InitializeComponent()
    {
        this.pnlTop = new System.Windows.Forms.Panel();
        this.btnSelectFile = new System.Windows.Forms.Button();
        this.btnDownloadTemplate = new System.Windows.Forms.Button();
        this.btnGenerateBarcodes = new System.Windows.Forms.Button();
        this.dgvPreview = new System.Windows.Forms.DataGridView();
        this.pnlBottom = new System.Windows.Forms.Panel();
        this.lblCount = new System.Windows.Forms.Label();
        this.btnSave = new System.Windows.Forms.Button();
        this.pnlTop.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.dgvPreview)).BeginInit();
        this.pnlBottom.SuspendLayout();
        this.SuspendLayout();
        // 
        // pnlTop
        // 
        this.pnlTop.Controls.Add(this.btnSelectFile);
        this.pnlTop.Controls.Add(this.btnDownloadTemplate);
        this.pnlTop.Controls.Add(this.btnGenerateBarcodes);
        this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
        this.pnlTop.Location = new System.Drawing.Point(0, 0);
        this.pnlTop.Name = "pnlTop";
        this.pnlTop.Size = new System.Drawing.Size(984, 60);
        this.pnlTop.TabIndex = 0;
        // 
        // btnSelectFile
        // 
        this.btnSelectFile.BackColor = System.Drawing.Color.MediumSeaGreen;
        this.btnSelectFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.btnSelectFile.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
        this.btnSelectFile.ForeColor = System.Drawing.Color.White;
        this.btnSelectFile.Location = new System.Drawing.Point(820, 12);
        this.btnSelectFile.Name = "btnSelectFile";
        this.btnSelectFile.Size = new System.Drawing.Size(150, 35);
        this.btnSelectFile.TabIndex = 0;
        this.btnSelectFile.Text = "اختر ملف إكسل";
        this.btnSelectFile.UseVisualStyleBackColor = false;
        this.btnSelectFile.Click += new System.EventHandler(this.btnSelectFile_Click);
        // 
        // btnDownloadTemplate
        // 
        this.btnDownloadTemplate.BackColor = System.Drawing.Color.SteelBlue;
        this.btnDownloadTemplate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.btnDownloadTemplate.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
        this.btnDownloadTemplate.ForeColor = System.Drawing.Color.White;
        this.btnDownloadTemplate.Location = new System.Drawing.Point(660, 12);
        this.btnDownloadTemplate.Name = "btnDownloadTemplate";
        this.btnDownloadTemplate.Size = new System.Drawing.Size(150, 35);
        this.btnDownloadTemplate.TabIndex = 1;
        this.btnDownloadTemplate.Text = "تحميل نموذج";
        this.btnDownloadTemplate.UseVisualStyleBackColor = false;
        this.btnDownloadTemplate.Click += new System.EventHandler(this.btnDownloadTemplate_Click);
        // 
        // btnGenerateBarcodes
        // 
        this.btnGenerateBarcodes.BackColor = System.Drawing.Color.DarkOrange;
        this.btnGenerateBarcodes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.btnGenerateBarcodes.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
        this.btnGenerateBarcodes.ForeColor = System.Drawing.Color.White;
        this.btnGenerateBarcodes.Location = new System.Drawing.Point(400, 12);
        this.btnGenerateBarcodes.Name = "btnGenerateBarcodes";
        this.btnGenerateBarcodes.Size = new System.Drawing.Size(250, 35);
        this.btnGenerateBarcodes.TabIndex = 2;
        this.btnGenerateBarcodes.Text = "توليد باركود للأصناف الفارغة";
        this.btnGenerateBarcodes.UseVisualStyleBackColor = false;
        this.btnGenerateBarcodes.Click += new System.EventHandler(this.btnGenerateBarcodes_Click);
        // 
        // dgvPreview
        // 
        this.dgvPreview.AllowUserToAddRows = false;
        this.dgvPreview.AllowUserToDeleteRows = false;
        this.dgvPreview.BackgroundColor = System.Drawing.Color.White;
        this.dgvPreview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.dgvPreview.Dock = System.Windows.Forms.DockStyle.Fill;
        this.dgvPreview.Location = new System.Drawing.Point(0, 60);
        this.dgvPreview.Name = "dgvPreview";
        this.dgvPreview.RowTemplate.Height = 28;
        this.dgvPreview.Size = new System.Drawing.Size(984, 441);
        this.dgvPreview.TabIndex = 1;
        this.dgvPreview.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPreview_CellValueChanged);
        // 
        // pnlBottom
        // 
        this.pnlBottom.Controls.Add(this.lblCount);
        this.pnlBottom.Controls.Add(this.btnSave);
        this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
        this.pnlBottom.Location = new System.Drawing.Point(0, 501);
        this.pnlBottom.Name = "pnlBottom";
        this.pnlBottom.Size = new System.Drawing.Size(984, 60);
        this.pnlBottom.TabIndex = 2;
        // 
        // lblCount
        // 
        this.lblCount.AutoSize = true;
        this.lblCount.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
        this.lblCount.Location = new System.Drawing.Point(820, 20);
        this.lblCount.Name = "lblCount";
        this.lblCount.Size = new System.Drawing.Size(120, 24);
        this.lblCount.TabIndex = 1;
        this.lblCount.Text = "عدد الأصناف: 0";
        // 
        // btnSave
        // 
        this.btnSave.BackColor = System.Drawing.Color.SteelBlue;
        this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.btnSave.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
        this.btnSave.ForeColor = System.Drawing.Color.White;
        this.btnSave.Location = new System.Drawing.Point(12, 12);
        this.btnSave.Name = "btnSave";
        this.btnSave.Size = new System.Drawing.Size(200, 40);
        this.btnSave.TabIndex = 0;
        this.btnSave.Text = "اعتماد وحفظ الأصناف";
        this.btnSave.UseVisualStyleBackColor = false;
        this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
        // 
        // frmImportProductsExcel
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.BackColor = System.Drawing.Color.WhiteSmoke;
        this.ClientSize = new System.Drawing.Size(984, 561);
        this.Controls.Add(this.dgvPreview);
        this.Controls.Add(this.pnlBottom);
        this.Controls.Add(this.pnlTop);
        this.Font = new System.Drawing.Font("Tahoma", 10F);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.Name = "frmImportProductsExcel";
        this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
        this.RightToLeftLayout = true;
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "استيراد المنتجات من Excel";
        this.pnlTop.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.dgvPreview)).EndInit();
        this.pnlBottom.ResumeLayout(false);
        this.pnlBottom.PerformLayout();
        this.ResumeLayout(false);
    }

    private System.Windows.Forms.Panel pnlTop;
    private System.Windows.Forms.Button btnSelectFile;
    private System.Windows.Forms.Button btnDownloadTemplate;
    private System.Windows.Forms.Button btnGenerateBarcodes;
    private System.Windows.Forms.DataGridView dgvPreview;
    private System.Windows.Forms.Panel pnlBottom;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.Label lblCount;
}
