using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalesSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RemoveProductPriceUniqueConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductPrices_ProductUnit_Date",
                table: "ProductPrices");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPrices_ProductUnit_Date",
                table: "ProductPrices",
                columns: new[] { "ProductUnitId", "EffectiveFrom" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductPrices_ProductUnit_Date",
                table: "ProductPrices");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPrices_ProductUnit_Date",
                table: "ProductPrices",
                columns: new[] { "ProductUnitId", "EffectiveFrom" },
                unique: true);
        }
    }
}

