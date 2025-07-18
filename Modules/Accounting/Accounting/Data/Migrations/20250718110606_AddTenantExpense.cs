using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Accounting.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTenantExpense : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TenantExpensePens",
                schema: "accounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    AppTenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantExpensePens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TenantExpensePens_AppTenants_AppTenantId",
                        column: x => x.AppTenantId,
                        principalSchema: "accounting",
                        principalTable: "AppTenants",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TenantExpensePenTranslates",
                schema: "accounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    LanguageId = table.Column<Guid>(type: "uuid", nullable: true),
                    TenantExpensePenId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantExpensePenTranslates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TenantExpensePenTranslates_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalSchema: "accounting",
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TenantExpensePenTranslates_TenantExpensePens_TenantExpenseP~",
                        column: x => x.TenantExpensePenId,
                        principalSchema: "accounting",
                        principalTable: "TenantExpensePens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TenantExpensePens_AppTenantId",
                schema: "accounting",
                table: "TenantExpensePens",
                column: "AppTenantId");

            migrationBuilder.CreateIndex(
                name: "IX_TenantExpensePenTranslates_LanguageId",
                schema: "accounting",
                table: "TenantExpensePenTranslates",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_TenantExpensePenTranslates_TenantExpensePenId",
                schema: "accounting",
                table: "TenantExpensePenTranslates",
                column: "TenantExpensePenId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TenantExpensePenTranslates",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "TenantExpensePens",
                schema: "accounting");
        }
    }
}
