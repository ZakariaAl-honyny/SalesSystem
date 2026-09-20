using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalesSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AlignInventoryTransactionLineSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BatchNo",
                table: "InventoryTransactionLines");

            migrationBuilder.DropColumn(
                name: "ExpiryDate",
                table: "InventoryTransactionLines");

            migrationBuilder.DropColumn(
                name: "WarehouseId",
                table: "InventoryTransactionLines");

            migrationBuilder.AddColumn<int>(
                name: "BatchId",
                table: "InventoryTransactionLines",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "InventoryTransactionLines",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalCost",
                table: "InventoryTransactionLines",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m,
                comment: "التكلفة الإجمالية");

            migrationBuilder.AlterColumn<int>(
                name: "BatchNo",
                table: "InventoryBatches",
                type: "int",
                nullable: false,
                comment: "رقم الدفعة",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldComment: "رقم الدفعة (nvarchar 50)");

            migrationBuilder.CreateIndex(
                name: "IX_InvTxLines_BatchId",
                table: "InventoryTransactionLines",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_InvTxLines_ProductId",
                table: "InventoryTransactionLines",
                column: "ProductId");

            migrationBuilder.AddCheckConstraint(
                name: "CHK_InvTxLines_TotalCost_NonNegative",
                table: "InventoryTransactionLines",
                sql: "[TotalCost] >= 0");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryTransactionLines_InventoryBatches_BatchId",
                table: "InventoryTransactionLines",
                column: "BatchId",
                principalTable: "InventoryBatches",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryTransactionLines_Products_ProductId",
                table: "InventoryTransactionLines",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryTransactionLines_InventoryBatches_BatchId",
                table: "InventoryTransactionLines");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryTransactionLines_Products_ProductId",
                table: "InventoryTransactionLines");

            migrationBuilder.DropIndex(
                name: "IX_InvTxLines_BatchId",
                table: "InventoryTransactionLines");

            migrationBuilder.DropIndex(
                name: "IX_InvTxLines_ProductId",
                table: "InventoryTransactionLines");

            migrationBuilder.DropCheckConstraint(
                name: "CHK_InvTxLines_TotalCost_NonNegative",
                table: "InventoryTransactionLines");

            migrationBuilder.DropColumn(
                name: "BatchId",
                table: "InventoryTransactionLines");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "InventoryTransactionLines");

            migrationBuilder.DropColumn(
                name: "TotalCost",
                table: "InventoryTransactionLines");

            migrationBuilder.AddColumn<string>(
                name: "BatchNo",
                table: "InventoryTransactionLines",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                comment: "رقم الدفعة");

            migrationBuilder.AddColumn<DateOnly>(
                name: "ExpiryDate",
                table: "InventoryTransactionLines",
                type: "date",
                nullable: true,
                comment: "تاريخ انتهاء الصلاحية");

            migrationBuilder.AddColumn<short>(
                name: "WarehouseId",
                table: "InventoryTransactionLines",
                type: "smallint",
                nullable: true,
                comment: "معرف المستودع (إذا كان مختلفاً عن المستودع الرئيسي)");

            migrationBuilder.AlterColumn<string>(
                name: "BatchNo",
                table: "InventoryBatches",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                comment: "رقم الدفعة (nvarchar 50)",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "رقم الدفعة");
        }
    }
}

