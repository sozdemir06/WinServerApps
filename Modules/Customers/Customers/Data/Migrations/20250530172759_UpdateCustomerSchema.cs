using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Customers.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCustomerSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Countries_CountryCode",
                schema: "customers",
                table: "Countries");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Countries_CountryCode",
                schema: "customers",
                table: "Countries",
                column: "CountryCode",
                unique: true);
        }
    }
}
