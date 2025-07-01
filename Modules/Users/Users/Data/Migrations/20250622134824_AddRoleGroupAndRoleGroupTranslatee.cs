using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Users.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRoleGroupAndRoleGroupTranslatee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RoleGroup",
                schema: "users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleGroupTranslatate",
                schema: "users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    LanguageId = table.Column<Guid>(type: "uuid", nullable: true),
                    RoleGroupId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleGroupTranslatate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleGroupTranslatate_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalSchema: "users",
                        principalTable: "Languages",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RoleGroupTranslatate_RoleGroup_RoleGroupId",
                        column: x => x.RoleGroupId,
                        principalSchema: "users",
                        principalTable: "RoleGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoleGroup_Id",
                schema: "users",
                table: "RoleGroup",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoleGroupTranslatate_Id",
                schema: "users",
                table: "RoleGroupTranslatate",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoleGroupTranslatate_LanguageId",
                schema: "users",
                table: "RoleGroupTranslatate",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleGroupTranslatate_RoleGroupId_LanguageId",
                schema: "users",
                table: "RoleGroupTranslatate",
                columns: new[] { "RoleGroupId", "LanguageId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleGroupTranslatate",
                schema: "users");

            migrationBuilder.DropTable(
                name: "RoleGroup",
                schema: "users");
        }
    }
}
