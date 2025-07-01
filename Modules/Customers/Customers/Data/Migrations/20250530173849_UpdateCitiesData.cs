using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Customers.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCitiesData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Districts_Cities_StateId",
                schema: "customers",
                table: "Districts");

            migrationBuilder.DropForeignKey(
                name: "FK_Districts_Countries_CountryId",
                schema: "customers",
                table: "Districts");

            migrationBuilder.DropIndex(
                name: "IX_Districts_CountryId_StateId_Name",
                schema: "customers",
                table: "Districts");

            migrationBuilder.DropIndex(
                name: "IX_Cities_CountryId_Name",
                schema: "customers",
                table: "Cities");

            migrationBuilder.CreateIndex(
                name: "IX_Districts_CountryId_StateId_Name",
                schema: "customers",
                table: "Districts",
                columns: new[] { "CountryId", "StateId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_Cities_CountryId_Name",
                schema: "customers",
                table: "Cities",
                columns: new[] { "CountryId", "Name" });

            migrationBuilder.AddForeignKey(
                name: "FK_Districts_Cities_StateId",
                schema: "customers",
                table: "Districts",
                column: "StateId",
                principalSchema: "customers",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Districts_Countries_CountryId",
                schema: "customers",
                table: "Districts",
                column: "CountryId",
                principalSchema: "customers",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Districts_Cities_StateId",
                schema: "customers",
                table: "Districts");

            migrationBuilder.DropForeignKey(
                name: "FK_Districts_Countries_CountryId",
                schema: "customers",
                table: "Districts");

            migrationBuilder.DropIndex(
                name: "IX_Districts_CountryId_StateId_Name",
                schema: "customers",
                table: "Districts");

            migrationBuilder.DropIndex(
                name: "IX_Cities_CountryId_Name",
                schema: "customers",
                table: "Cities");

            migrationBuilder.CreateIndex(
                name: "IX_Districts_CountryId_StateId_Name",
                schema: "customers",
                table: "Districts",
                columns: new[] { "CountryId", "StateId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cities_CountryId_Name",
                schema: "customers",
                table: "Cities",
                columns: new[] { "CountryId", "Name" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Districts_Cities_StateId",
                schema: "customers",
                table: "Districts",
                column: "StateId",
                principalSchema: "customers",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Districts_Countries_CountryId",
                schema: "customers",
                table: "Districts",
                column: "CountryId",
                principalSchema: "customers",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
