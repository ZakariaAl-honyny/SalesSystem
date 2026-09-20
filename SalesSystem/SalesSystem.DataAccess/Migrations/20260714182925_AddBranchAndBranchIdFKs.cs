using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalesSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddBranchAndBranchIdFKs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "BranchId",
                table: "WarehouseTransfers",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "BranchId",
                table: "Warehouses",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "BranchId",
                table: "Suppliers",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "BranchId",
                table: "SupplierPayments",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "BranchId",
                table: "SalesReturns",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "BranchId",
                table: "SalesInvoices",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "BranchId",
                table: "ReceiptVouchers",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "BranchId",
                table: "PurchaseReturns",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "BranchId",
                table: "PurchaseInvoices",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "BranchId",
                table: "Products",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "BranchId",
                table: "ProductCategories",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "BranchId",
                table: "PaymentVouchers",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "BranchId",
                table: "JournalEntries",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "BranchId",
                table: "InventoryCounts",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "BranchId",
                table: "InventoryAdjustments",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "BranchId",
                table: "Expenses",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "BranchId",
                table: "Customers",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "BranchId",
                table: "CustomerReceipts",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "BranchId",
                table: "CashBoxes",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "BranchId",
                table: "Banks",
                type: "smallint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseTransfers_BranchId",
                table: "WarehouseTransfers",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_BranchId",
                table: "Warehouses",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_BranchId",
                table: "Suppliers",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierPayments_BranchId",
                table: "SupplierPayments",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesReturns_BranchId",
                table: "SalesReturns",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesInvoices_BranchId",
                table: "SalesInvoices",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptVouchers_BranchId",
                table: "ReceiptVouchers",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseReturns_BranchId",
                table: "PurchaseReturns",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseInvoices_BranchId",
                table: "PurchaseInvoices",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_BranchId",
                table: "Products",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_BranchId",
                table: "ProductCategories",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentVouchers_BranchId",
                table: "PaymentVouchers",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntries_BranchId",
                table: "JournalEntries",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryCounts_BranchId",
                table: "InventoryCounts",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryAdjustments_BranchId",
                table: "InventoryAdjustments",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_BranchId",
                table: "Expenses",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_BranchId",
                table: "Customers",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerReceipts_BranchId",
                table: "CustomerReceipts",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_CashBoxes_BranchId",
                table: "CashBoxes",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Banks_BranchId",
                table: "Banks",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_Name",
                table: "Branches",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Banks_Branches_BranchId",
                table: "Banks",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CashBoxes_Branches_BranchId",
                table: "CashBoxes",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerReceipts_Branches_BranchId",
                table: "CustomerReceipts",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Branches_BranchId",
                table: "Customers",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Branches_BranchId",
                table: "Expenses",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryAdjustments_Branches_BranchId",
                table: "InventoryAdjustments",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryCounts_Branches_BranchId",
                table: "InventoryCounts",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JournalEntries_Branches_BranchId",
                table: "JournalEntries",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentVouchers_Branches_BranchId",
                table: "PaymentVouchers",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategories_Branches_BranchId",
                table: "ProductCategories",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Branches_BranchId",
                table: "Products",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseInvoices_Branches_BranchId",
                table: "PurchaseInvoices",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseReturns_Branches_BranchId",
                table: "PurchaseReturns",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReceiptVouchers_Branches_BranchId",
                table: "ReceiptVouchers",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SalesInvoices_Branches_BranchId",
                table: "SalesInvoices",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SalesReturns_Branches_BranchId",
                table: "SalesReturns",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SupplierPayments_Branches_BranchId",
                table: "SupplierPayments",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Suppliers_Branches_BranchId",
                table: "Suppliers",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Warehouses_Branches_BranchId",
                table: "Warehouses",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseTransfers_Branches_BranchId",
                table: "WarehouseTransfers",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Banks_Branches_BranchId",
                table: "Banks");

            migrationBuilder.DropForeignKey(
                name: "FK_CashBoxes_Branches_BranchId",
                table: "CashBoxes");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerReceipts_Branches_BranchId",
                table: "CustomerReceipts");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Branches_BranchId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Branches_BranchId",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryAdjustments_Branches_BranchId",
                table: "InventoryAdjustments");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryCounts_Branches_BranchId",
                table: "InventoryCounts");

            migrationBuilder.DropForeignKey(
                name: "FK_JournalEntries_Branches_BranchId",
                table: "JournalEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentVouchers_Branches_BranchId",
                table: "PaymentVouchers");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategories_Branches_BranchId",
                table: "ProductCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Branches_BranchId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseInvoices_Branches_BranchId",
                table: "PurchaseInvoices");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseReturns_Branches_BranchId",
                table: "PurchaseReturns");

            migrationBuilder.DropForeignKey(
                name: "FK_ReceiptVouchers_Branches_BranchId",
                table: "ReceiptVouchers");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesInvoices_Branches_BranchId",
                table: "SalesInvoices");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesReturns_Branches_BranchId",
                table: "SalesReturns");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplierPayments_Branches_BranchId",
                table: "SupplierPayments");

            migrationBuilder.DropForeignKey(
                name: "FK_Suppliers_Branches_BranchId",
                table: "Suppliers");

            migrationBuilder.DropForeignKey(
                name: "FK_Warehouses_Branches_BranchId",
                table: "Warehouses");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseTransfers_Branches_BranchId",
                table: "WarehouseTransfers");

            migrationBuilder.DropTable(
                name: "Branches");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseTransfers_BranchId",
                table: "WarehouseTransfers");

            migrationBuilder.DropIndex(
                name: "IX_Warehouses_BranchId",
                table: "Warehouses");

            migrationBuilder.DropIndex(
                name: "IX_Suppliers_BranchId",
                table: "Suppliers");

            migrationBuilder.DropIndex(
                name: "IX_SupplierPayments_BranchId",
                table: "SupplierPayments");

            migrationBuilder.DropIndex(
                name: "IX_SalesReturns_BranchId",
                table: "SalesReturns");

            migrationBuilder.DropIndex(
                name: "IX_SalesInvoices_BranchId",
                table: "SalesInvoices");

            migrationBuilder.DropIndex(
                name: "IX_ReceiptVouchers_BranchId",
                table: "ReceiptVouchers");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseReturns_BranchId",
                table: "PurchaseReturns");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseInvoices_BranchId",
                table: "PurchaseInvoices");

            migrationBuilder.DropIndex(
                name: "IX_Products_BranchId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_ProductCategories_BranchId",
                table: "ProductCategories");

            migrationBuilder.DropIndex(
                name: "IX_PaymentVouchers_BranchId",
                table: "PaymentVouchers");

            migrationBuilder.DropIndex(
                name: "IX_JournalEntries_BranchId",
                table: "JournalEntries");

            migrationBuilder.DropIndex(
                name: "IX_InventoryCounts_BranchId",
                table: "InventoryCounts");

            migrationBuilder.DropIndex(
                name: "IX_InventoryAdjustments_BranchId",
                table: "InventoryAdjustments");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_BranchId",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_Customers_BranchId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_CustomerReceipts_BranchId",
                table: "CustomerReceipts");

            migrationBuilder.DropIndex(
                name: "IX_CashBoxes_BranchId",
                table: "CashBoxes");

            migrationBuilder.DropIndex(
                name: "IX_Banks_BranchId",
                table: "Banks");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "WarehouseTransfers");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "SupplierPayments");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "SalesReturns");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "SalesInvoices");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "ReceiptVouchers");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "PurchaseReturns");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "PurchaseInvoices");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "ProductCategories");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "PaymentVouchers");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "JournalEntries");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "InventoryCounts");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "InventoryAdjustments");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "CustomerReceipts");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "CashBoxes");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Banks");
        }
    }
}
