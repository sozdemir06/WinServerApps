using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Accounting.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTenantTaxGroupAndTenantTaxes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TenantTaxGroups",
                schema: "accounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
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
                    table.PrimaryKey("PK_TenantTaxGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TenantTaxGroups_AppTenants_AppTenantId",
                        column: x => x.AppTenantId,
                        principalSchema: "accounting",
                        principalTable: "AppTenants",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TenantTaxGroups_AppTenants_TenantId",
                        column: x => x.TenantId,
                        principalSchema: "accounting",
                        principalTable: "AppTenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TenantTaxes",
                schema: "accounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Rate = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    TenantTaxGroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantTaxes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TenantTaxes_AppTenants_TenantId",
                        column: x => x.TenantId,
                        principalSchema: "accounting",
                        principalTable: "AppTenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TenantTaxes_TenantTaxGroups_TenantTaxGroupId",
                        column: x => x.TenantTaxGroupId,
                        principalSchema: "accounting",
                        principalTable: "TenantTaxGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TenantTaxGroupTranslates",
                schema: "accounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    LanguageId = table.Column<Guid>(type: "uuid", nullable: true),
                    TenantTaxGroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantTaxGroupTranslates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TenantTaxGroupTranslates_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalSchema: "accounting",
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TenantTaxGroupTranslates_TenantTaxGroups_TenantTaxGroupId",
                        column: x => x.TenantTaxGroupId,
                        principalSchema: "accounting",
                        principalTable: "TenantTaxGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TenantTaxTranslates",
                schema: "accounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    LanguageId = table.Column<Guid>(type: "uuid", nullable: true),
                    TenantTaxId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantTaxTranslates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TenantTaxTranslates_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalSchema: "accounting",
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TenantTaxTranslates_TenantTaxes_TenantTaxId",
                        column: x => x.TenantTaxId,
                        principalSchema: "accounting",
                        principalTable: "TenantTaxes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TenantTaxes_TenantId",
                schema: "accounting",
                table: "TenantTaxes",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_TenantTaxes_TenantTaxGroupId",
                schema: "accounting",
                table: "TenantTaxes",
                column: "TenantTaxGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_TenantTaxGroups_AppTenantId",
                schema: "accounting",
                table: "TenantTaxGroups",
                column: "AppTenantId");

            migrationBuilder.CreateIndex(
                name: "IX_TenantTaxGroups_TenantId",
                schema: "accounting",
                table: "TenantTaxGroups",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_TenantTaxGroupTranslates_LanguageId",
                schema: "accounting",
                table: "TenantTaxGroupTranslates",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_TenantTaxGroupTranslates_TenantTaxGroupId",
                schema: "accounting",
                table: "TenantTaxGroupTranslates",
                column: "TenantTaxGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_TenantTaxTranslates_LanguageId",
                schema: "accounting",
                table: "TenantTaxTranslates",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_TenantTaxTranslates_TenantTaxId",
                schema: "accounting",
                table: "TenantTaxTranslates",
                column: "TenantTaxId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TenantTaxGroupTranslates",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "TenantTaxTranslates",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "TenantTaxes",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "TenantTaxGroups",
                schema: "accounting");
        }
    }
}
