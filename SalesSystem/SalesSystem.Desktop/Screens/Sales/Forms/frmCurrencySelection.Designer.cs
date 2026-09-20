namespace SalesSystem.Desktop.Screens.Sales.Forms
{
    partial class frmCurrencySelection
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
            pnlHeader = new Panel();
            lblTitle = new Label();
            pnlFooter = new Panel();
            flpFooter = new FlowLayoutPanel();
            btnOk = new Button();
            btnCancel = new Button();
            pnlBody = new Panel();
            lblValidation = new Label();
            lblNote = new Label();
            pnlRate = new Panel();
            tblRate = new TableLayoutPanel();
            lblRateLabel = new Label();
            numExchangeRate = new NumericUpDown();
            lblRateRange = new Label();
            flpRestore = new FlowLayoutPanel();
            btnRestoreRate = new Button();
            lblRateTitle = new Label();
            flpCurrencies = new FlowLayoutPanel();
            tip = new ToolTip(components);
            pnlHeader.SuspendLayout();
            pnlFooter.SuspendLayout();
            flpFooter.SuspendLayout();
            pnlBody.SuspendLayout();
            pnlRate.SuspendLayout();
            tblRate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numExchangeRate).BeginInit();
            flpRestore.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(33, 43, 54);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 56;
            // 
            // lblTitle
            // 
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.Font = new Font("Tahoma", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.ForeColor = Color.White;
            lblTitle.Text = "اختيار العملة";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlFooter
            // 
            pnlFooter.BackColor = Color.FromArgb(248, 249, 250);
            pnlFooter.Controls.Add(flpFooter);
            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Height = 64;
            // 
            // flpFooter
            // 
            flpFooter.Controls.Add(btnOk);
            flpFooter.Controls.Add(btnCancel);
            flpFooter.Dock = DockStyle.Fill;
            flpFooter.FlowDirection = FlowDirection.RightToLeft;
            flpFooter.Padding = new Padding(6, 10, 6, 8);
            // 
            // btnOk
            // 
            btnOk.BackColor = Color.FromArgb(25, 135, 84);
            btnOk.FlatStyle = FlatStyle.Popup;
            btnOk.Font = new Font("Tahoma", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnOk.ForeColor = Color.White;
            btnOk.Height = 42;
            btnOk.Margin = new Padding(0, 0, 6, 0);
            btnOk.Text = "تأكيد واعتماد";
            btnOk.Width = 170;
            btnOk.Click += btnOk_Click;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.White;
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.FlatStyle = FlatStyle.Popup;
            btnCancel.Font = new Font("Tahoma", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCancel.ForeColor = Color.FromArgb(33, 43, 54);
            btnCancel.Height = 42;
            btnCancel.Margin = new Padding(0, 0, 6, 0);
            btnCancel.Text = "إلغاء";
            btnCancel.Width = 100;
            btnCancel.Click += btnCancel_Click;
            // 
            // pnlBody
            // 
            pnlBody.BackColor = Color.WhiteSmoke;
            pnlBody.Controls.Add(lblValidation);
            pnlBody.Controls.Add(lblNote);
            pnlBody.Controls.Add(pnlRate);
            pnlBody.Controls.Add(flpCurrencies);
            pnlBody.Dock = DockStyle.Fill;
            pnlBody.Padding = new Padding(12, 10, 12, 10);
            // 
            // lblValidation
            // 
            lblValidation.BackColor = Color.FromArgb(253, 237, 237);
            lblValidation.Dock = DockStyle.Top;
            lblValidation.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblValidation.ForeColor = Color.FromArgb(220, 53, 69);
            lblValidation.Height = 38;
            lblValidation.Padding = new Padding(10, 6, 10, 6);
            lblValidation.Text = "تنبيه: سعر الصرف خارج النطاق المسموح.";
            lblValidation.TextAlign = ContentAlignment.MiddleCenter;
            lblValidation.Visible = false;
            // 
            // lblNote
            // 
            lblNote.BackColor = Color.FromArgb(232, 244, 253);
            lblNote.Dock = DockStyle.Top;
            lblNote.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblNote.ForeColor = Color.FromArgb(13, 60, 120);
            lblNote.Height = 50;
            lblNote.Padding = new Padding(10, 8, 10, 8);
            lblNote.Text = "سيتم تقييد مبالغ الفاتورة بالعملة المحددة وتحويل المعادل المحلي بالريال اليمني.";
            lblNote.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlRate
            // 
            pnlRate.BackColor = Color.White;
            pnlRate.Controls.Add(tblRate);
            pnlRate.Controls.Add(lblRateTitle);
            pnlRate.Dock = DockStyle.Top;
            pnlRate.Height = 128;
            pnlRate.Padding = new Padding(10, 6, 10, 6);
            pnlRate.Visible = false;
            // 
            // tblRate
            // 
            tblRate.ColumnCount = 2;
            tblRate.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
            tblRate.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblRate.Controls.Add(lblRateLabel, 0, 0);
            tblRate.Controls.Add(numExchangeRate, 1, 0);
            tblRate.Controls.Add(lblRateRange, 0, 1);
            tblRate.SetColumnSpan(lblRateRange, 2);
            tblRate.Controls.Add(flpRestore, 0, 2);
            tblRate.SetColumnSpan(flpRestore, 2);
            tblRate.Dock = DockStyle.Top;
            tblRate.Height = 90;
            tblRate.RowCount = 3;
            tblRate.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            tblRate.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            tblRate.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            // 
            // lblRateLabel
            // 
            lblRateLabel.Dock = DockStyle.Fill;
            lblRateLabel.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblRateLabel.ForeColor = Color.FromArgb(80, 90, 100);
            lblRateLabel.Text = "سعر الصرف:";
            lblRateLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // numExchangeRate
            // 
            numExchangeRate.DecimalPlaces = 2;
            numExchangeRate.Dock = DockStyle.Fill;
            numExchangeRate.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            numExchangeRate.Maximum = 99999;
            numExchangeRate.Minimum = 0.01m;
            numExchangeRate.TextAlign = HorizontalAlignment.Center;
            numExchangeRate.ThousandsSeparator = true;
            numExchangeRate.Value = 250;
            numExchangeRate.ValueChanged += numExchangeRate_ValueChanged;
            // 
            // lblRateRange
            // 
            lblRateRange.Dock = DockStyle.Fill;
            lblRateRange.Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblRateRange.ForeColor = Color.FromArgb(100, 110, 120);
            lblRateRange.Text = "النطاق المسموح: —";
            lblRateRange.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // flpRestore
            // 
            flpRestore.Controls.Add(btnRestoreRate);
            flpRestore.Dock = DockStyle.Fill;
            flpRestore.FlowDirection = FlowDirection.RightToLeft;
            flpRestore.Padding = new Padding(0, 2, 0, 2);
            // 
            // btnRestoreRate
            // 
            btnRestoreRate.BackColor = Color.FromArgb(13, 110, 253);
            btnRestoreRate.FlatStyle = FlatStyle.Popup;
            btnRestoreRate.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnRestoreRate.ForeColor = Color.White;
            btnRestoreRate.Height = 26;
            btnRestoreRate.Margin = new Padding(0, 0, 6, 0);
            btnRestoreRate.Text = "استعادة السعر الرسمي";
            btnRestoreRate.Width = 180;
            btnRestoreRate.Click += btnRestoreRate_Click;
            // 
            // lblRateTitle
            // 
            lblRateTitle.Dock = DockStyle.Top;
            lblRateTitle.Font = new Font("Tahoma", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblRateTitle.ForeColor = Color.FromArgb(33, 43, 54);
            lblRateTitle.Height = 24;
            lblRateTitle.Text = "سعر الصرف";
            lblRateTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // flpCurrencies
            // 
            flpCurrencies.Dock = DockStyle.Top;
            flpCurrencies.FlowDirection = FlowDirection.RightToLeft;
            flpCurrencies.Height = 84;
            flpCurrencies.Padding = new Padding(0, 0, 0, 2);
            // 
            // tip
            // 
            tip.AutomaticDelay = 400;
            tip.ToolTipIcon = ToolTipIcon.Info;
            tip.ToolTipTitle = "سعر الصرف";
            // 
            // frmCurrencySelection
            // 
            AcceptButton = btnOk;
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            CancelButton = btnCancel;
            ClientSize = new Size(460, 520);
            Controls.Add(pnlBody);
            Controls.Add(pnlFooter);
            Controls.Add(pnlHeader);
            Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmCurrencySelection";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "اختيار العملة";
            pnlHeader.ResumeLayout(false);
            pnlFooter.ResumeLayout(false);
            flpFooter.ResumeLayout(false);
            pnlBody.ResumeLayout(false);
            pnlRate.ResumeLayout(false);
            tblRate.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)numExchangeRate).EndInit();
            flpRestore.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.FlowLayoutPanel flpFooter;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel pnlBody;
        private System.Windows.Forms.Label lblValidation;
        private System.Windows.Forms.Label lblNote;
        private System.Windows.Forms.Panel pnlRate;
        private System.Windows.Forms.TableLayoutPanel tblRate;
        private System.Windows.Forms.Label lblRateLabel;
        private System.Windows.Forms.NumericUpDown numExchangeRate;
        private System.Windows.Forms.Label lblRateRange;
        private System.Windows.Forms.FlowLayoutPanel flpRestore;
        private System.Windows.Forms.Button btnRestoreRate;
        private System.Windows.Forms.Label lblRateTitle;
        private System.Windows.Forms.FlowLayoutPanel flpCurrencies;
        private System.Windows.Forms.ToolTip tip;
    }
}