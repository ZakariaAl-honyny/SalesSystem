'use client';

import React, { useState } from 'react';
import { 
  FileCode2, 
  Copy, 
  Check, 
  Database, 
  Layers, 
  Layout, 
  Server, 
  FileText, 
  Sparkles,
  ExternalLink
} from 'lucide-react';

interface WinFormsArchitectureModalProps {
  isOpen: boolean;
  onClose: () => void;
  currency: string;
}

export const WinFormsArchitectureModal: React.FC<WinFormsArchitectureModalProps> = ({
  isOpen,
  onClose,
  currency,
}) => {
  const [copiedSection, setCopiedSection] = useState<string | null>(null);
  const [activeLayer, setActiveLayer] = useState<'ENTITIES' | 'DAL' | 'BLL' | 'WINFORMS_UI' | 'MENU_STRIP' | 'SQL_SCHEMA' | 'SYSTEM_PROMPT'>('ENTITIES');

  if (!isOpen) return null;

  const copyToClipboard = (text: string, id: string) => {
    navigator.clipboard.writeText(text);
    setCopiedSection(id);
    setTimeout(() => setCopiedSection(null), 2500);
  };

  const csharpEntitiesCode = `// ==========================================
// 1. Entities & DTOs Layer (Core Domain Models)
// Project: YemenPOS.Core.Entities
// ==========================================

namespace YemenPOS.Core.Entities
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Barcode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Unit { get; set; } = "حبة";
        public decimal CostPrice { get; set; } // YER
        public decimal SalePrice { get; set; } // YER
        public decimal MinSalePrice { get; set; }
        public int CurrentStock { get; set; }
        public int MinStockAlert { get; set; }
        public string BatchNo { get; set; } = string.Empty;
        public DateTime? ExpiryDate { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class InvoiceHeader
    {
        public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; } = string.Empty;
        public string InvoiceType { get; set; } = "SALE"; // SALE, PURCHASE, RETURN
        public DateTime InvoiceDate { get; set; } = DateTime.Now;
        public int CustomerOrSupplierId { get; set; }
        public string PaymentMethod { get; set; } = "CASH"; // CASH, KURAYMI, CREDIT, JEB
        public decimal Subtotal { get; set; }
        public decimal DiscountTotal { get; set; }
        public decimal TaxTotal { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal RemainingAmount { get; set; }
        public int CashierUserId { get; set; }
        public int ShiftNumber { get; set; }
        public string Notes { get; set; } = string.Empty;
        public List<InvoiceDetail> Items { get; set; } = new List<InvoiceDetail>();
    }

    public class InvoiceDetail
    {
        public int DetailId { get; set; }
        public int InvoiceId { get; set; }
        public int ProductId { get; set; }
        public string UnitName { get; set; } = "حبة";
        public decimal UnitFactor { get; set; } = 1.0m;
        public int Quantity { get; set; }
        public decimal UnitCost { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal LineTotal { get; set; }
    }
}`;

  const csharpDalCode = `// ==========================================
// 2. Data Access Layer (DAL)
// Project: YemenPOS.DataAccess
// Tech: ADO.NET / Dapper with SQL Server
// ==========================================

using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;
using YemenPOS.Core.Entities;

namespace YemenPOS.DataAccess.Repositories
{
    public class InvoiceRepository
    {
        private readonly string _connectionString;

        public InvoiceRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<int> SaveSalesInvoiceAsync(InvoiceHeader invoice)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();

            try
            {
                // 1. Insert Header
                string sqlHeader = @"
                    INSERT INTO Invoices (InvoiceNo, InvoiceType, InvoiceDate, CustomerId, PaymentMethod, 
                                          Subtotal, DiscountTotal, GrandTotal, PaidAmount, RemainingAmount, CashierUserId, ShiftNumber)
                    VALUES (@InvoiceNo, @InvoiceType, @InvoiceDate, @CustomerOrSupplierId, @PaymentMethod, 
                            @Subtotal, @DiscountTotal, @GrandTotal, @PaidAmount, @RemainingAmount, @CashierUserId, @ShiftNumber);
                    SELECT CAST(SCOPE_IDENTITY() as int);";

                int invoiceId = await connection.ExecuteScalarAsync<int>(sqlHeader, invoice, transaction);

                // 2. Insert Details & Update Stock Quantity
                foreach (var item in invoice.Items)
                {
                    item.InvoiceId = invoiceId;
                    string sqlDetail = @"
                        INSERT INTO InvoiceDetails (InvoiceId, ProductId, UnitName, Quantity, UnitCost, UnitPrice, Discount, LineTotal)
                        VALUES (@InvoiceId, @ProductId, @UnitName, @Quantity, @UnitCost, @UnitPrice, @Discount, @LineTotal);
                        
                        -- Deduct inventory
                        UPDATE Products 
                        SET CurrentStock = CurrentStock - (@Quantity * @UnitFactor)
                        WHERE ProductId = @ProductId;";

                    await connection.ExecuteAsync(sqlDetail, item, transaction);
                }

                // 3. Update Customer Balance if Credit Sale (ذمة)
                if (invoice.PaymentMethod == "CREDIT" && invoice.RemainingAmount > 0)
                {
                    string sqlCustomer = @"
                        UPDATE Customers 
                        SET CurrentBalance = CurrentBalance + @RemainingAmount 
                        WHERE CustomerId輳 = @CustomerOrSupplierId;";
                    await connection.ExecuteAsync(sqlCustomer, new { invoice.RemainingAmount, invoice.CustomerOrSupplierId }, transaction);
                }

                transaction.Commit();
                return invoiceId;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}`;

  const csharpBllCode = `// ==========================================
// 3. Business Logic Layer (BLL)
// Project: YemenPOS.BusinessLogic
// Enforces: Validation, Credit Limits, Stock Availability
// ==========================================

using YemenPOS.Core.Entities;
using YemenPOS.DataAccess.Repositories;

namespace YemenPOS.BusinessLogic.Services
{
    public class POSService
    {
        private readonly InvoiceRepository _invoiceRepo;
        private readonly CustomerRepository _customerRepo;
        private readonly ProductRepository _productRepo;

        public POSService(InvoiceRepository invoiceRepo, CustomerRepository customerRepo, ProductRepository productRepo)
        {
            _invoiceRepo輳 = invoiceRepo;
            _customerRepo = customerRepo;
            _productRepo = productRepo;
        }

        public async Task<(bool Success, string Message, int InvoiceId)> ProcessSaleCheckoutAsync(InvoiceHeader invoice)
        {
            if (invoice.Items == null || invoice.Items.Count == 0)
                return (false, "لا توجد أصناف في الفاتورة لإتمام البيع.", 0);

            // 1. Check Credit Customer Limit if Credit
            if (invoice.PaymentMethod == "CREDIT")
            {
                var customer = await _customerRepo.GetByIdAsync(invoice.CustomerOrSupplierId);
                if (customer == null)
                    return (false, "العميل المحدد غير موجود في قاعدة البيانات.", 0);

                if (customer.CreditLimit > 0 && (customer.CurrentBalance + invoice.GrandTotal) > customer.CreditLimit)
                {
                    return (false, $"تجاوز سقف الائتمان! سقف العميل: {customer.CreditLimit:N0} ريال يمني", 0);
                }
            }

            // 2. Validate Stock Availability
            foreach (var item in invoice.Items)
            {
                var product = await _productRepo.GetByIdAsync(item.ProductId);
                if (product.CurrentStock < item.Quantity)
                {
                    return (false, $"الكمية غير متوفرة في المستودع للصنف: {product.Name} (المتوفر: {product.CurrentStock})", 0);
                }
            }

            // 3. Save via DAL
            int newId = await _invoiceRepo.SaveSalesInvoiceAsync(invoice);
            return (true, "تم حفظ الفاتورة وترحيل القيد وتحديث المخزون بنجاح.", newId);
        }
    }
}`;

  const winformsUiCode = `// ==========================================
// 4. WinForms Presentation Layer
// Form: frmPOS.cs (C# Windows Forms Desktop)
// Features: Keyboard Hotkeys [F1..F12], DataGridView Editing, Barcode Reader
// ==========================================

using System.Windows.Forms;
using YemenPOS.BusinessLogic.Services;
using YemenPOS.Core.Entities;

namespace YemenPOS.WinFormsApp.Forms
{
    public partial class frmPOS : Form
    {
        private readonly POSService _posService;
        private List<InvoiceDetail> _cartItems = new List<InvoiceDetail>();

        public frmPOS(POSService posService)
        {
            InitializeComponent();
            _posService = posService;
            this.KeyPreview = true; // Essential for global hotkeys
            this.KeyDown += FrmPOS_KeyDown;
        }

        private void FrmPOS_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F1: // New Bill
                    btnNewBill.PerformClick();
                    break;
                case Keys.F2: // Focus Barcode
                    txtBarcode.Focus();
                    txtBarcode.SelectAll();
                    break;
                case Keys.F3: // Checkout & Print
                    btnCheckout.PerformClick();
                    break;
                case Keys.F4: // Hold Bill
                    btnHoldBill.PerformClick();
                    break;
                case Keys.F9: // Calculator
                    OpenCalculator();
                    break;
                case Keys.Delete: // Remove selected item from grid
                    RemoveSelectedGridItem();
                    break;
            }
        }

        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(txtBarcode.Text))
            {
                e.Handled = true;
                e.SuppressKeyPress輳 = true;
                AddProductToGridByBarcode(txtBarcode.Text.Trim());
                txtBarcode.Clear();
            }
        }

        private void RecalculateTotals()
        {
            decimal subtotal = _cartItems.Sum(i => i.LineTotal);
            decimal discount = numDiscount.Value;
            decimal grandTotal = Math.Max(0, subtotal - discount);

            lblGrandTotal.Text = $"{grandTotal:N0} ريال يمني";
            dgvInvoiceItems.DataSource = null;
            dgvInvoiceItems.DataSource = _cartItems;
        }
    }
}`;

  const winformsMenuStripCode = `// ==========================================
// 4.2 WinForms MenuStrip, ToolStrip & StatusStrip Designer Code
// File: frmMain.Designer.cs & frmMain.cs
// Demonstrates: Mnemonic Keys (&File), Shortcuts (Ctrl+N, F1..F10), Nested Submenus, Status Panels
// ==========================================

using System;
using System.Windows.Forms;
using System.Drawing;

namespace YemenPOS.WinFormsApp.Forms
{
    public partial class frmMain : Form
    {
        // 1. Declare WinForms Menu & Toolbar Controls
        private MenuStrip menuStripMain;
        private ToolStripMenuItem mnuFile;
        private ToolStripMenuItem mnuFileNewSale;
        private ToolStripMenuItem mnuFileHoldSale;
        private ToolStripMenuItem mnuFileRecallSale;
        private ToolStripMenuItem mnuFileExportSubmenu;
        private ToolStripMenuItem mnuExportExcel;
        private ToolStripMenuItem mnuExportCSV;
        private ToolStripSeparator mnuSep1;
        private ToolStripMenuItem mnuFileExit;

        private ToolStripMenuItem mnuSales;
        private ToolStripMenuItem mnuSalesPOS;
        private ToolStripMenuItem mnuSalesReturns;
        private ToolStripMenuItem mnuSalesCustomersSub;
        private ToolStripMenuItem mnuCustList;
        private ToolStripMenuItem mnuCustStatement;

        private ToolStripMenuItem mnuInventory;
        private ToolStripMenuItem mnuInvProducts;
        private ToolStripMenuItem mnuInvAlerts;

        private ToolStripMenuItem mnuAccounting;
        private ToolStripMenuItem mnuAccChart;
        private ToolStripMenuItem mnuAccJournal;

        private ToolStripMenuItem mnuReports;
        private ToolStripMenuItem mnuRepDashboard;
        private ToolStripMenuItem mnuRepZReport;

        // ToolStrip
        private ToolStrip toolStripMain;
        private ToolStripButton btnToolNewSale;
        private ToolStripButton btnToolHoldSale;
        private ToolStripButton btnToolProducts;
        private ToolStripButton btnToolCalc;
        private ToolStripSeparator toolStripSep1;
        private ToolStripLabel lblCashDrawer;

        // StatusStrip
        private StatusStrip statusStripBottom;
        private ToolStripStatusLabel lblStatusReady;
        private ToolStripStatusLabel lblStatusUser;
        private ToolStripStatusLabel lblStatusShift;
        private ToolStripStatusLabel lblStatusSpring;
        private ToolStripStatusLabel lblStatusCapsLock;
        private ToolStripStatusLabel lblStatusDb;
        private ToolStripStatusLabel lblStatusClock;

        private void InitializeWinFormsComponents()
        {
            this.SuspendLayout();

            // ==========================================
            // A. MenuStrip Configuration (Mnemonics & Shortcuts)
            // ==========================================
            this.menuStripMain = new MenuStrip();
            this.menuStripMain.BackColor = Color.FromArgb(11, 31, 54);
            this.menuStripMain.ForeColor = Color.White;
            this.menuStripMain.Dock = DockStyle.Top;

            // 1. File Menu (&File => Alt + F)
            this.mnuFile = new ToolStripMenuItem("&ملف");
            
            this.mnuFileNewSale = new ToolStripMenuItem("فاتورة مبيعات جديدة", null, (s, e) => OpenPOSNewSale())
            {
                ShortcutKeys = Keys.F1,
                ShowShortcutKeys = true
            };

            this.mnuFileHoldSale = new ToolStripMenuItem("تعليق الفاتورة الحالية", null, (s, e) => HoldActiveSale())
            {
                ShortcutKeys = Keys.F4,
                ShowShortcutKeys = true
            };

            this.mnuFileRecallSale = new ToolStripMenuItem("استرجاع الفواتير المعلقة", null, (s, e) => OpenHeldSalesModal())
            {
                ShortcutKeys = Keys.Control | Keys.H,
                ShowShortcutKeys = true
            };

            // Cascading Submenu: Data Export
            this.mnuFileExportSubmenu = new ToolStripMenuItem("تصدير واستيراد البيانات");
            this.mnuExportExcel = new ToolStripMenuItem("تصدير دليل الحسابات (Excel)", null, (s, e) => ExportToExcel())
            {
                ShortcutKeys = Keys.Control | Keys.Shift | Keys.E
            };
            this.mnuExportCSV = new ToolStripMenuItem("تصدير قائمة المنتجات (CSV)", null, (s, e) => ExportToCSV());
            this.mnuFileExportSubmenu.DropDownItems.AddRange(new ToolStripItem[] { this.mnuExportExcel, this.mnuExportCSV });

            this.mnuSep1 = new ToolStripSeparator();
            this.mnuFileExit = new ToolStripMenuItem("خروج من النظام", null, (s, e) => this.Close())
            {
                ShortcutKeys = Keys.Alt | Keys.F4
            };

            this.mnuFile.DropDownItems.AddRange(new ToolStripItem[] {
                this.mnuFileNewSale,
                this.mnuFileHoldSale,
                this.mnuFileRecallSale,
                new ToolStripSeparator(),
                this.mnuFileExportSubmenu,
                this.mnuSep1,
                this.mnuFileExit
            });

            // 2. Sales Menu (&Sales => Alt + S)
            this.mnuSales = new ToolStripMenuItem("الم&بيعات");
            this.mnuSalesPOS = new ToolStripMenuItem("شاشة نقطة البيع السريعة", null, (s, e) => OpenPOSModule())
            {
                ShortcutKeys = Keys.F1
            };
            this.mnuSalesReturns = new ToolStripMenuItem("مردودات ومبيعات المرتجع", null, (s, e) => OpenReturnsModule())
            {
                ShortcutKeys = Keys.Control | Keys.R
            };
            
            // Nested Customer Submenu
            this.mnuSalesCustomersSub = new ToolStripMenuItem("العملاء والذمم المدينة");
            this.mnuCustList = new ToolStripMenuItem("دليل العملاء وسقوف الائتمان", null, (s, e) => OpenCustomersList());
            this.mnuCustStatement = new ToolStripMenuItem("كشف حساب عميل تفصيلي", null, (s, e) => OpenCustomerStatement());
            this.mnuSalesCustomersSub.DropDownItems.AddRange(new ToolStripItem[] { this.mnuCustList, this.mnuCustStatement });

            this.mnuSales.DropDownItems.AddRange(new ToolStripItem[] {
                this.mnuSalesPOS,
                this.mnuSalesReturns,
                new ToolStripSeparator(),
                this.mnuSalesCustomersSub
            });

            // 3. Inventory Menu (&Inventory => Alt + I)
            this.mnuInventory = new ToolStripMenuItem("الم&خازن");
            this.mnuInvProducts = new ToolStripMenuItem("بطاقة الأصناف والمخزون", null, (s, e) => OpenInventoryModule())
            {
                ShortcutKeys = Keys.F5
            };
            this.mnuInvAlerts = new ToolStripMenuItem("تنبيهات نواقص المخزون والصلاحيات", null, (s, e) => OpenStockAlerts());
            this.mnuInventory.DropDownItems.AddRange(new ToolStripItem[] { this.mnuInvProducts, this.mnuInvAlerts });

            // 4. Accounting Menu (&Accounts => Alt + A)
            this.mnuAccounting = new ToolStripMenuItem("الح&سابات");
            this.mnuAccChart = new ToolStripMenuItem("دليل الحسابات الشجري اليمني", null, (s, e) => OpenChartOfAccounts())
            {
                ShortcutKeys = Keys.F7
            };
            this.mnuAccJournal = new ToolStripMenuItem("سجل قيود اليومية العامة", null, (s, e) => OpenJournalEntries());
            this.mnuAccounting.DropDownItems.AddRange(new ToolStripItem[] { this.mnuAccChart, this.mnuAccJournal });

            // 5. Reports Menu (&Reports => Alt + R)
            this.mnuReports = new ToolStripMenuItem("ال&تقارير");
            this.mnuRepDashboard = new ToolStripMenuItem("لوحة المؤشرات العامة P&L", null, (s, e) => OpenReportsModule())
            {
                ShortcutKeys = Keys.F8
            };
            this.mnuRepZReport = new ToolStripMenuItem("تقرير إقفال الوردية وجرد الصندوق (Z-Report)", null, (s, e) => OpenZReport());
            this.mnuReports.DropDownItems.AddRange(new ToolStripItem[] { this.mnuRepDashboard, this.mnuRepZReport });

            // Add all top-level menus to MenuStrip
            this.menuStripMain.Items.AddRange(new ToolStripItem[] {
                this.mnuFile,
                this.mnuSales,
                this.mnuInventory,
                this.mnuAccounting,
                this.mnuReports
            });

            // ==========================================
            // B. ToolStrip Configuration
            // ==========================================
            this.toolStripMain = new ToolStrip();
            this.toolStripMain.BackColor = Color.FromArgb(19, 47, 78);
            this.toolStripMain.GripStyle = ToolStripGripStyle.Hidden;

            this.btnToolNewSale = new ToolStripButton("فاتورة جديدة [F1]", null, (s, e) => OpenPOSNewSale());
            this.btnToolHoldSale = new ToolStripButton("المعلقات [F4]", null, (s, e) => OpenHeldSalesModal());
            this.btnToolProducts = new ToolStripButton("المخزون [F5]", null, (s, e) => OpenInventoryModule());
            this.btnToolCalc = new ToolStripButton("الحاسبة [F9]", null, (s, e) => OpenCalculator());
            
            this.lblCashDrawer = new ToolStripLabel("نقدية الصندوق: 245,000 ريال يمني")
            {
                Alignment = ToolStripItemAlignment.Right,
                ForeColor = Color.LightGreen,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };

            this.toolStripMain.Items.AddRange(new ToolStripItem[] {
                this.btnToolNewSale,
                this.btnToolHoldSale,
                new ToolStripSeparator(),
                this.btnToolProducts,
                this.btnToolCalc,
                this.lblCashDrawer
            });

            // ==========================================
            // C. StatusStrip Configuration
            // ==========================================
            this.statusStripBottom = new StatusStrip();
            this.statusStripBottom.BackColor = Color.FromArgb(11, 27, 45);

            this.lblStatusReady = new ToolStripStatusLabel("● جاهز (Ready)") { ForeColor = Color.LimeGreen };
            this.lblStatusUser = new ToolStripStatusLabel("المستخدم: أحمد صالح") { BorderSides = ToolStripStatusLabelBorderSides.Left };
            this.lblStatusShift = new ToolStripStatusLabel("الوردية: #104") { BorderSides = ToolStripStatusLabelBorderSides.Left };
            this.lblStatusSpring = new ToolStripStatusLabel() { Spring = true }; // pushes items to the right
            this.lblStatusCapsLock = new ToolStripStatusLabel("NUM") { BorderSides = ToolStripStatusLabelBorderSides.All };
            this.lblStatusDb = new ToolStripStatusLabel("SQL Server: متصل") { ForeColor = Color.SkyBlue };
            this.lblStatusClock = new ToolStripStatusLabel(DateTime.Now.ToLongTimeString());

            this.statusStripBottom.Items.AddRange(new ToolStripItem[] {
                this.lblStatusReady,
                this.lblStatusUser,
                this.lblStatusShift,
                this.lblStatusSpring,
                this.lblStatusCapsLock,
                this.lblStatusDb,
                this.lblStatusClock
            });

            // Add to Form
            this.MainMenuStrip = this.menuStripMain;
            this.Controls.Add(this.toolStripMain);
            this.Controls.Add(this.menuStripMain);
            this.Controls.Add(this.statusStripBottom);

            this.RightToLeft = RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}`;

  const sqlSchemaCode = `-- ==========================================
-- 5. SQL Server Database Schema (YemenPOS_DB)
-- Currency: Yemeni Rial (YER)
-- ==========================================

CREATE DATABASE YemenPOS_DB;
GO
USE YemenPOS_DB;
GO

-- 1. Products Table
CREATE TABLE Products (
    ProductId INT IDENTITY(1,1) PRIMARY KEY,
    Code NVARCHAR(50) UNIQUE NOT NULL,
    Barcode NVARCHAR(100) UNIQUE NOT NULL,
    Name NVARCHAR(250) NOT NULL,
    Category NVARCHAR(100) NOT NULL,
    Unit NVARCHAR(50) DEFAULT N'حبة',
    CostPrice DECIMAL(18,2) NOT NULL DEFAULT 0.00,
    SalePrice DECIMAL(18,2) NOT NULL DEFAULT 0.00,
    MinSalePrice DECIMAL(18,2) DEFAULT 0.00,
    CurrentStock INT NOT NULL DEFAULT 0,
    MinStockAlert INT NOT NULL DEFAULT 10,
    BatchNo NVARCHAR(100) NULL,
    ExpiryDate DATE NULL,
    IsActive BIT DEFAULT 1
);

-- 2. Customers Table
CREATE TABLE Customers (
    CustomerId INT IDENTITY(1,1) PRIMARY KEY,
    CustomerCode NVARCHAR(50) UNIQUE NOT NULL,
    Name NVARCHAR(250) NOT NULL,
    Phone NVARCHAR(50) NULL,
    Address NVARCHAR(300) NULL,
    CreditLimit DECIMAL(18,2) DEFAULT 0.00,
    CurrentBalance DECIMAL(18,2) DEFAULT 0.00,
    AccountCode NVARCHAR(50) NOT NULL
);

-- 3. Invoices Header Table
CREATE TABLE Invoices (
    InvoiceId INT IDENTITY(1,1) PRIMARY KEY,
    InvoiceNo NVARCHAR(50) UNIQUE NOT NULL,
    InvoiceType NVARCHAR(20) NOT NULL DEFAULT 'SALE',
    InvoiceDate DATETIME NOT NULL DEFAULT GETDATE(),
    CustomerId INT NOT NULL,
    PaymentMethod NVARCHAR(50) NOT NULL DEFAULT 'CASH',
    Subtotal DECIMAL(18,2) NOT NULL,
    DiscountTotal DECIMAL(18,2) DEFAULT 0.00,
    GrandTotal DECIMAL(18,2) NOT NULL,
    PaidAmount DECIMAL(18,2) NOT NULL,
    RemainingAmount DECIMAL(18,2) DEFAULT 0.00,
    CashierUserId INT NOT NULL,
    ShiftNumber INT NOT NULL,
    Notes NVARCHAR(500) NULL,
    FOREIGN KEY (CustomerId) REFERENCES Customers(CustomerId)
);

-- 4. Invoice Details Table
CREATE TABLE InvoiceDetails (
    DetailId INT IDENTITY(1,1) PRIMARY KEY,
    InvoiceId INT NOT NULL,
    ProductId INT NOT NULL,
    UnitName NVARCHAR(50) NOT NULL,
    UnitFactor DECIMAL(10,2) DEFAULT 1.0,
    Quantity INT NOT NULL,
    UnitCost DECIMAL(18,2) NOT NULL,
    UnitPrice DECIMAL(18,2) NOT NULL,
    Discount DECIMAL(18,2) DEFAULT 0.00,
    LineTotal DECIMAL(18,2) NOT NULL,
    FOREIGN KEY (InvoiceId) REFERENCES Invoices(InvoiceId),
    FOREIGN KEY (ProductId) REFERENCES Products(ProductId)
);`;

  const systemPromptTemplate = `📋 الموجه التأسيسي لمشروع الـ POS & Accounting (Google AI Studio System Prompt):

أنت مهندس برمجيات ومحلل نظم أول متخصص في بناء أنظمة إدارة المبيعات والمحاسبة (POS & Accounting ERP Systems) لبيئة سطح المكتب C# WinForms بنمط N-Layer Architecture وقواعد بيانات SQL Server.

السياق والعملة:
- السوق المستهدف: السوق اليمني.
- العملة الأساسية: ريال يمني (YER).
- معمارية النظام: N-Layer (Entities -> DAL / Dapper -> BLL -> WinForms UI -> SQL Server).
- واجهة الديسك توب: لوحة مفاتيح سريعة (F1: جديد، F2: باركود، F3: دفع وطباعة، F4: تعليق، F9: حاسبة)، MenuStrip كامل مع Alt+Key وأسهم التنقل والقوائم المتداخلة.

الوحدات الأساسية المغطاة:
1. شاشة نقطة البيع السريعة POS مع دعم الدفع النقدي وحوالات الكريمي والمحافظ الإلكترونية والآجل.
2. إدارة المخزون والدفعات Batches والصلاحيات وتعدد الوحدات (حبة، باكت، كرتون).
3. فواتير المشتريات والتوريد وحسابات الموردين.
4. قيود اليومية ودليل الحسابات الشجري اليمني وسندات القبض والصرف.
5. التقارير المالية، الأرباح P&L، وإقفال الوردية وجرد الصندوق Z-Report.
6. كود تصميم WinForms MenuStrip, ToolStrip, StatusStrip مع مفاتيح الاختصار المباشرة.`;

  const getActiveCode = () => {
    switch (activeLayer) {
      case 'ENTITIES':
        return csharpEntitiesCode;
      case 'DAL':
        return csharpDalCode;
      case 'BLL':
        return csharpBllCode;
      case 'WINFORMS_UI':
        return winformsUiCode;
      case 'MENU_STRIP':
        return winformsMenuStripCode;
      case 'SQL_SCHEMA':
        return sqlSchemaCode;
      case 'SYSTEM_PROMPT':
        return systemPromptTemplate;
      default:
        return csharpEntitiesCode;
    }
  };

  return (
    <div className="fixed inset-0 bg-slate-950/70 backdrop-blur-xs flex items-center justify-center z-50 p-3 select-none">
      <div className="bg-white rounded-xl shadow-2xl border border-slate-300 w-full max-w-5xl h-[88vh] flex flex-col overflow-hidden animate-in fade-in zoom-in-95 duration-150">
        
        {/* Header */}
        <div className="bg-white border-b border-slate-200 px-4 py-3.5 flex items-center justify-between text-slate-800 shadow-2xs">
          <div className="flex items-center gap-2.5">
            <div className="w-8 h-8 rounded-lg bg-purple-50 border border-purple-200 flex items-center justify-center text-purple-700 shadow-xs">
              <Layers className="w-4 h-4 text-purple-700" />
            </div>
            <div>
              <h3 className="font-bold text-sm text-slate-900 leading-none">
                مخطط معمارية C# WinForms N-Layer وتوليد الكود البرمجي
              </h3>
              <p className="text-[11px] text-slate-500 mt-0.5">
                تحويل وتوثيق كافة شاشات الـ MVP الحالية إلى كود C# وSQL Server جاهز للتطبيق الفعلي
              </p>
            </div>
          </div>

          <button
            onClick={onClose}
            className="text-slate-400 hover:text-slate-700 p-1.5 hover:bg-slate-100 rounded-lg text-sm font-bold cursor-pointer transition"
          >
            ✕
          </button>
        </div>

        {/* Layer Tabs Navigation */}
        <div className="bg-slate-100 px-3 py-2 flex items-center justify-between gap-1 overflow-x-auto text-xs border-b border-slate-200">
          <div className="flex items-center gap-1">
            <button
              onClick={() => setActiveLayer('ENTITIES')}
              className={`px-3 py-1.5 rounded-lg font-semibold transition cursor-pointer flex items-center gap-1.5 ${
                activeLayer === 'ENTITIES'
                  ? 'bg-purple-600 text-white shadow-xs'
                  : 'text-slate-700 hover:bg-slate-200'
              }`}
            >
              <FileCode2 className="w-3.5 h-3.5" />
              <span>1. Entities & DTOs</span>
            </button>

            <button
              onClick={() => setActiveLayer('DAL')}
              className={`px-3 py-1.5 rounded-lg font-semibold transition cursor-pointer flex items-center gap-1.5 ${
                activeLayer === 'DAL'
                  ? 'bg-purple-600 text-white shadow-xs'
                  : 'text-slate-700 hover:bg-slate-200'
              }`}
            >
              <Database className="w-3.5 h-3.5" />
              <span>2. DAL (Dapper / ADO)</span>
            </button>

            <button
              onClick={() => setActiveLayer('BLL')}
              className={`px-3 py-1.5 rounded-lg font-semibold transition cursor-pointer flex items-center gap-1.5 ${
                activeLayer === 'BLL'
                  ? 'bg-purple-600 text-white shadow-xs'
                  : 'text-slate-700 hover:bg-slate-200'
              }`}
            >
              <Server className="w-3.5 h-3.5" />
              <span>3. BLL (Business Rules)</span>
            </button>

            <button
              onClick={() => setActiveLayer('WINFORMS_UI')}
              className={`px-3 py-1.5 rounded-lg font-semibold transition cursor-pointer flex items-center gap-1.5 ${
                activeLayer === 'WINFORMS_UI'
                  ? 'bg-purple-600 text-white shadow-xs'
                  : 'text-slate-700 hover:bg-slate-200'
              }`}
            >
              <Layout className="w-3.5 h-3.5" />
              <span>4. WinForms Form Events</span>
            </button>

            <button
              onClick={() => setActiveLayer('MENU_STRIP')}
              className={`px-3 py-1.5 rounded-lg font-semibold transition cursor-pointer flex items-center gap-1.5 ${
                activeLayer === 'MENU_STRIP'
                  ? 'bg-purple-600 text-white shadow-xs'
                  : 'text-slate-700 hover:bg-slate-200'
              }`}
            >
              <Layers className="w-3.5 h-3.5" />
              <span>4.2 MenuStrip & ToolStrip</span>
            </button>

            <button
              onClick={() => setActiveLayer('SQL_SCHEMA')}
              className={`px-3 py-1.5 rounded-lg font-semibold transition cursor-pointer flex items-center gap-1.5 ${
                activeLayer === 'SQL_SCHEMA'
                  ? 'bg-purple-600 text-white shadow-xs'
                  : 'text-slate-700 hover:bg-slate-200'
              }`}
            >
              <Database className="w-3.5 h-3.5" />
              <span>5. SQL Server Schema</span>
            </button>

            <button
              onClick={() => setActiveLayer('SYSTEM_PROMPT')}
              className={`px-3 py-1.5 rounded-lg font-semibold transition cursor-pointer flex items-center gap-1.5 ${
                activeLayer === 'SYSTEM_PROMPT'
                  ? 'bg-emerald-600 text-white shadow-xs'
                  : 'text-emerald-700 bg-emerald-50 hover:bg-emerald-100 border border-emerald-200'
              }`}
            >
              <Sparkles className="w-3.5 h-3.5" />
              <span>نسخ System Prompt لـ AI Studio</span>
            </button>
          </div>

          <button
            onClick={() => copyToClipboard(getActiveCode(), activeLayer)}
            className="bg-purple-600 hover:bg-purple-500 text-white px-3 py-1.5 rounded font-bold flex items-center gap-1.5 cursor-pointer shadow transition"
          >
            {copiedSection === activeLayer ? (
              <>
                <Check className="w-4 h-4 text-emerald-300" />
                <span>تم النسخ للحافظة!</span>
              </>
            ) : (
              <>
                <Copy className="w-4 h-4" />
                <span>نسخ الكود بالكامل</span>
              </>
            )}
          </button>
        </div>

        {/* Code Content Box */}
        <div className="flex-1 p-3 bg-slate-900 overflow-auto font-mono text-xs text-emerald-400 leading-relaxed text-left" dir="ltr">
          <pre className="whitespace-pre-wrap">{getActiveCode()}</pre>
        </div>

        {/* Footer */}
        <div className="bg-slate-100 px-4 py-2.5 border-t border-slate-300 flex items-center justify-between text-xs">
          <div className="text-slate-600">
            <span>الهدف: تحليل وتثبيت متطلبات العمل (Business Logic & UI) قبل الانتقال للـ Desktop C# WinForms</span>
          </div>
          <button
            onClick={onClose}
            className="bg-slate-800 hover:bg-slate-700 text-white font-semibold px-4 py-1.5 rounded cursor-pointer"
          >
            إغلاق النافذة
          </button>
        </div>
      </div>
    </div>
  );
};
