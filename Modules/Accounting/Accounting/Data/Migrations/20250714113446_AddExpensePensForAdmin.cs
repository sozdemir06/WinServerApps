using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Accounting.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddExpensePensForAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExpensePens",
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
                    table.PrimaryKey("PK_ExpensePens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExpensePenTranslates",
                schema: "accounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    LanguageId = table.Column<Guid>(type: "uuid", nullable: true),
                    ExpensePenId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpensePenTranslates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpensePenTranslates_ExpensePens_ExpensePenId",
                        column: x => x.ExpensePenId,
                        principalSchema: "accounting",
                        principalTable: "ExpensePens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExpensePenTranslates_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalSchema: "accounting",
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExpensePens_IsActive",
                schema: "accounting",
                table: "ExpensePens",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_ExpensePens_IsDefault",
                schema: "accounting",
                table: "ExpensePens",
                column: "IsDefault");

            migrationBuilder.CreateIndex(
                name: "IX_ExpensePenTranslates_ExpensePenId",
                schema: "accounting",
                table: "ExpensePenTranslates",
                column: "ExpensePenId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpensePenTranslates_LanguageId",
                schema: "accounting",
                table: "ExpensePenTranslates",
                column: "LanguageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExpensePenTranslates",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "ExpensePens",
                schema: "accounting");
        }
    }
}
