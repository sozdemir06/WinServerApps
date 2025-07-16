using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Accounting.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTaxGroupAndTax : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Taxes",
                schema: "accounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Rate = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Taxes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaxGroups",
                schema: "accounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaxTranslates",
                schema: "accounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    LanguageId = table.Column<Guid>(type: "uuid", nullable: true),
                    TaxId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxTranslates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaxTranslates_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalSchema: "accounting",
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaxTranslates_Taxes_TaxId",
                        column: x => x.TaxId,
                        principalSchema: "accounting",
                        principalTable: "Taxes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaxGroupTranslates",
                schema: "accounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    LanguageId = table.Column<Guid>(type: "uuid", nullable: true),
                    TaxGroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxGroupTranslates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaxGroupTranslates_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalSchema: "accounting",
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaxGroupTranslates_TaxGroups_TaxGroupId",
                        column: x => x.TaxGroupId,
                        principalSchema: "accounting",
                        principalTable: "TaxGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Taxes_IsActive",
                schema: "accounting",
                table: "Taxes",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Taxes_IsDefault",
                schema: "accounting",
                table: "Taxes",
                column: "IsDefault");

            migrationBuilder.CreateIndex(
                name: "IX_Taxes_Rate",
                schema: "accounting",
                table: "Taxes",
                column: "Rate");

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

            migrationBuilder.CreateIndex(
                name: "IX_TaxGroupTranslates_LanguageId",
                schema: "accounting",
                table: "TaxGroupTranslates",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_TaxGroupTranslates_TaxGroupId",
                schema: "accounting",
                table: "TaxGroupTranslates",
                column: "TaxGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_TaxTranslates_LanguageId",
                schema: "accounting",
                table: "TaxTranslates",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_TaxTranslates_TaxId",
                schema: "accounting",
                table: "TaxTranslates",
                column: "TaxId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaxGroupTranslates",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "TaxTranslates",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "TaxGroups",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "Taxes",
                schema: "accounting");
        }
    }
}
