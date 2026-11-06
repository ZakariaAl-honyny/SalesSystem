using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalesSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddProductInfoFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "ExpiryAlertDays",
                table: "Products",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ForeignName",
                table: "Products",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationInStore",
                table: "Products",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpiryAlertDays",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ForeignName",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "LocationInStore",
                table: "Products");
        }
    }
}
