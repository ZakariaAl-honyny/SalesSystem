using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalesSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixSupplierBatchNoType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SupplierBatchNo",
                table: "InventoryBatches",
                type: "int",
                nullable: true,
                comment: "رقم الدفعة من المورد",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldComment: "رقم الدفعة من المورد");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SupplierBatchNo",
                table: "InventoryBatches",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                comment: "رقم الدفعة من المورد",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "رقم الدفعة من المورد");
        }
    }
}

