using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Users.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRoleGroupAndRoleGroupTranslateAddDomainToDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleGroupTranslatate_Languages_LanguageId",
                schema: "users",
                table: "RoleGroupTranslatate");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleGroupTranslatate_RoleGroup_RoleGroupId",
                schema: "users",
                table: "RoleGroupTranslatate");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoleGroupTranslatate",
                schema: "users",
                table: "RoleGroupTranslatate");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoleGroup",
                schema: "users",
                table: "RoleGroup");

            migrationBuilder.RenameTable(
                name: "RoleGroupTranslatate",
                schema: "users",
                newName: "RoleGroupTranslatates",
                newSchema: "users");

            migrationBuilder.RenameTable(
                name: "RoleGroup",
                schema: "users",
                newName: "RoleGroups",
                newSchema: "users");

            migrationBuilder.RenameIndex(
                name: "IX_RoleGroupTranslatate_Id",
                schema: "users",
                table: "RoleGroupTranslatates",
                newName: "IX_RoleGroupTranslatates_Id");

            migrationBuilder.RenameIndex(
                name: "IX_RoleGroup_Id",
                schema: "users",
                table: "RoleGroups",
                newName: "IX_RoleGroups_Id");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "users",
                table: "RoleGroups",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoleGroupTranslatates",
                schema: "users",
                table: "RoleGroupTranslatates",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoleGroups",
                schema: "users",
                table: "RoleGroups",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleGroupTranslatates_Languages_LanguageId",
                schema: "users",
                table: "RoleGroupTranslatates",
                column: "LanguageId",
                principalSchema: "users",
                principalTable: "Languages",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleGroupTranslatates_RoleGroups_RoleGroupId",
                schema: "users",
                table: "RoleGroupTranslatates",
                column: "RoleGroupId",
                principalSchema: "users",
                principalTable: "RoleGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleGroupTranslatates_Languages_LanguageId",
                schema: "users",
                table: "RoleGroupTranslatates");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleGroupTranslatates_RoleGroups_RoleGroupId",
                schema: "users",
                table: "RoleGroupTranslatates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoleGroupTranslatates",
                schema: "users",
                table: "RoleGroupTranslatates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoleGroups",
                schema: "users",
                table: "RoleGroups");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "users",
                table: "RoleGroups");

            migrationBuilder.RenameTable(
                name: "RoleGroupTranslatates",
                schema: "users",
                newName: "RoleGroupTranslatate",
                newSchema: "users");

            migrationBuilder.RenameTable(
                name: "RoleGroups",
                schema: "users",
                newName: "RoleGroup",
                newSchema: "users");

            migrationBuilder.RenameIndex(
                name: "IX_RoleGroupTranslatates_Id",
                schema: "users",
                table: "RoleGroupTranslatate",
                newName: "IX_RoleGroupTranslatate_Id");

            migrationBuilder.RenameIndex(
                name: "IX_RoleGroups_Id",
                schema: "users",
                table: "RoleGroup",
                newName: "IX_RoleGroup_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoleGroupTranslatate",
                schema: "users",
                table: "RoleGroupTranslatate",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoleGroup",
                schema: "users",
                table: "RoleGroup",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleGroupTranslatate_Languages_LanguageId",
                schema: "users",
                table: "RoleGroupTranslatate",
                column: "LanguageId",
                principalSchema: "users",
                principalTable: "Languages",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleGroupTranslatate_RoleGroup_RoleGroupId",
                schema: "users",
                table: "RoleGroupTranslatate",
                column: "RoleGroupId",
                principalSchema: "users",
                principalTable: "RoleGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
