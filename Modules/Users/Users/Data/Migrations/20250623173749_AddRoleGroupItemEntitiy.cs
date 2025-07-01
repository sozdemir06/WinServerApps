using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Users.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRoleGroupItemEntitiy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RoleGroupItems",
                schema: "users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleGroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    AppRoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleGroupItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleGroupItems_AppRoles_AppRoleId",
                        column: x => x.AppRoleId,
                        principalSchema: "users",
                        principalTable: "AppRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleGroupItems_RoleGroups_RoleGroupId",
                        column: x => x.RoleGroupId,
                        principalSchema: "users",
                        principalTable: "RoleGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoleGroupItem_AppRoleId",
                schema: "users",
                table: "RoleGroupItems",
                column: "AppRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleGroupItem_RoleGroupId",
                schema: "users",
                table: "RoleGroupItems",
                column: "RoleGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleGroupItem_RoleGroupId_AppRoleId",
                schema: "users",
                table: "RoleGroupItems",
                columns: new[] { "RoleGroupId", "AppRoleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoleGroupItems_Id",
                schema: "users",
                table: "RoleGroupItems",
                column: "Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleGroupItems",
                schema: "users");
        }
    }
}
