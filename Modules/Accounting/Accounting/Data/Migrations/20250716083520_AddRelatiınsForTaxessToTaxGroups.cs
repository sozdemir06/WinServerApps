using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Accounting.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRelatiınsForTaxessToTaxGroups : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TaxGroups_IsActive",
                schema: "accounting",
                table: "TaxGroups");

            migrationBuilder.DropIndex(
                name: "IX_TaxGroups_IsDefault",
                schema: "accounting",
                table: "TaxGroups");

            migrationBuilder.AddColumn<Guid>(
                name: "TaxGroupId",
                schema: "accounting",
                table: "Taxes",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Taxes_TaxGroupId",
                schema: "accounting",
                table: "Taxes",
                column: "TaxGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Taxes_TaxGroups_TaxGroupId",
                schema: "accounting",
                table: "Taxes",
                column: "TaxGroupId",
                principalSchema: "accounting",
                principalTable: "TaxGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Taxes_TaxGroups_TaxGroupId",
                schema: "accounting",
                table: "Taxes");

            migrationBuilder.DropIndex(
                name: "IX_Taxes_TaxGroupId",
                schema: "accounting",
                table: "Taxes");

            migrationBuilder.DropColumn(
                name: "TaxGroupId",
                schema: "accounting",
                table: "Taxes");

            migrationBuilder.CreateIndex(
                name: "IX_TaxGroups_IsActive",
                schema: "accounting",
                table: "TaxGroups",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_TaxGroups_IsDefault",
                schema: "accounting",
                table: "TaxGroups",
                column: "IsDefault");
        }
    }
}
