using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Customers.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDistrictRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Districts_Cities_StateId",
                schema: "customers",
                table: "Districts");

            migrationBuilder.DropIndex(
                name: "IX_Districts_CountryId_StateId_Name",
                schema: "customers",
                table: "Districts");

            migrationBuilder.DropIndex(
                name: "IX_Districts_StateId",
                schema: "customers",
                table: "Districts");

            migrationBuilder.DropColumn(
                name: "StateId",
                schema: "customers",
                table: "Districts");

            migrationBuilder.AddColumn<string>(
                name: "WikiDataId",
                schema: "customers",
                table: "Cities",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Districts_CountryId_CityId_Name",
                schema: "customers",
                table: "Districts",
                columns: new[] { "CountryId", "CityId", "Name" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Districts_CountryId_CityId_Name",
                schema: "customers",
                table: "Districts");

            migrationBuilder.DropColumn(
                name: "WikiDataId",
                schema: "customers",
                table: "Cities");

            migrationBuilder.AddColumn<long>(
                name: "StateId",
                schema: "customers",
                table: "Districts",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Districts_CountryId_StateId_Name",
                schema: "customers",
                table: "Districts",
                columns: new[] { "CountryId", "StateId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_Districts_StateId",
                schema: "customers",
                table: "Districts",
                column: "StateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Districts_Cities_StateId",
                schema: "customers",
                table: "Districts",
                column: "StateId",
                principalSchema: "customers",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
