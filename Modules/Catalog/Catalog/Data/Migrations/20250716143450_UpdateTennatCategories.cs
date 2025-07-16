using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catalog.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTennatCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TenantCategory_AppTenants_TenantId",
                schema: "catalog",
                table: "TenantCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_TenantCategory_TenantCategory_ParentId",
                schema: "catalog",
                table: "TenantCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_TenantCategoryTranslate_Languages_LanguageId",
                schema: "catalog",
                table: "TenantCategoryTranslate");

            migrationBuilder.DropForeignKey(
                name: "FK_TenantCategoryTranslate_TenantCategory_CategoryId",
                schema: "catalog",
                table: "TenantCategoryTranslate");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TenantCategoryTranslate",
                schema: "catalog",
                table: "TenantCategoryTranslate");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TenantCategory",
                schema: "catalog",
                table: "TenantCategory");

            migrationBuilder.RenameTable(
                name: "TenantCategoryTranslate",
                schema: "catalog",
                newName: "TenantCategoryTranslates",
                newSchema: "catalog");

            migrationBuilder.RenameTable(
                name: "TenantCategory",
                schema: "catalog",
                newName: "TenantCategories",
                newSchema: "catalog");

            migrationBuilder.RenameIndex(
                name: "IX_TenantCategoryTranslate_LanguageId",
                schema: "catalog",
                table: "TenantCategoryTranslates",
                newName: "IX_TenantCategoryTranslates_LanguageId");

            migrationBuilder.RenameIndex(
                name: "IX_TenantCategoryTranslate_CategoryId",
                schema: "catalog",
                table: "TenantCategoryTranslates",
                newName: "IX_TenantCategoryTranslates_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_TenantCategory_TenantId",
                schema: "catalog",
                table: "TenantCategories",
                newName: "IX_TenantCategories_TenantId");

            migrationBuilder.RenameIndex(
                name: "IX_TenantCategory_ParentId",
                schema: "catalog",
                table: "TenantCategories",
                newName: "IX_TenantCategories_ParentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TenantCategoryTranslates",
                schema: "catalog",
                table: "TenantCategoryTranslates",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TenantCategories",
                schema: "catalog",
                table: "TenantCategories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TenantCategories_AppTenants_TenantId",
                schema: "catalog",
                table: "TenantCategories",
                column: "TenantId",
                principalSchema: "catalog",
                principalTable: "AppTenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TenantCategories_TenantCategories_ParentId",
                schema: "catalog",
                table: "TenantCategories",
                column: "ParentId",
                principalSchema: "catalog",
                principalTable: "TenantCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TenantCategoryTranslates_Languages_LanguageId",
                schema: "catalog",
                table: "TenantCategoryTranslates",
                column: "LanguageId",
                principalSchema: "catalog",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TenantCategoryTranslates_TenantCategories_CategoryId",
                schema: "catalog",
                table: "TenantCategoryTranslates",
                column: "CategoryId",
                principalSchema: "catalog",
                principalTable: "TenantCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TenantCategories_AppTenants_TenantId",
                schema: "catalog",
                table: "TenantCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_TenantCategories_TenantCategories_ParentId",
                schema: "catalog",
                table: "TenantCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_TenantCategoryTranslates_Languages_LanguageId",
                schema: "catalog",
                table: "TenantCategoryTranslates");

            migrationBuilder.DropForeignKey(
                name: "FK_TenantCategoryTranslates_TenantCategories_CategoryId",
                schema: "catalog",
                table: "TenantCategoryTranslates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TenantCategoryTranslates",
                schema: "catalog",
                table: "TenantCategoryTranslates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TenantCategories",
                schema: "catalog",
                table: "TenantCategories");

            migrationBuilder.RenameTable(
                name: "TenantCategoryTranslates",
                schema: "catalog",
                newName: "TenantCategoryTranslate",
                newSchema: "catalog");

            migrationBuilder.RenameTable(
                name: "TenantCategories",
                schema: "catalog",
                newName: "TenantCategory",
                newSchema: "catalog");

            migrationBuilder.RenameIndex(
                name: "IX_TenantCategoryTranslates_LanguageId",
                schema: "catalog",
                table: "TenantCategoryTranslate",
                newName: "IX_TenantCategoryTranslate_LanguageId");

            migrationBuilder.RenameIndex(
                name: "IX_TenantCategoryTranslates_CategoryId",
                schema: "catalog",
                table: "TenantCategoryTranslate",
                newName: "IX_TenantCategoryTranslate_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_TenantCategories_TenantId",
                schema: "catalog",
                table: "TenantCategory",
                newName: "IX_TenantCategory_TenantId");

            migrationBuilder.RenameIndex(
                name: "IX_TenantCategories_ParentId",
                schema: "catalog",
                table: "TenantCategory",
                newName: "IX_TenantCategory_ParentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TenantCategoryTranslate",
                schema: "catalog",
                table: "TenantCategoryTranslate",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TenantCategory",
                schema: "catalog",
                table: "TenantCategory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TenantCategory_AppTenants_TenantId",
                schema: "catalog",
                table: "TenantCategory",
                column: "TenantId",
                principalSchema: "catalog",
                principalTable: "AppTenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TenantCategory_TenantCategory_ParentId",
                schema: "catalog",
                table: "TenantCategory",
                column: "ParentId",
                principalSchema: "catalog",
                principalTable: "TenantCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TenantCategoryTranslate_Languages_LanguageId",
                schema: "catalog",
                table: "TenantCategoryTranslate",
                column: "LanguageId",
                principalSchema: "catalog",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TenantCategoryTranslate_TenantCategory_CategoryId",
                schema: "catalog",
                table: "TenantCategoryTranslate",
                column: "CategoryId",
                principalSchema: "catalog",
                principalTable: "TenantCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
