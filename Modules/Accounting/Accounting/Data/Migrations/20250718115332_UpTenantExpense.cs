using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Accounting.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpTenantExpense : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TenantExpensePens_AppTenants_AppTenantId",
                schema: "accounting",
                table: "TenantExpensePens");

            migrationBuilder.DropIndex(
                name: "IX_TenantExpensePens_AppTenantId",
                schema: "accounting",
                table: "TenantExpensePens");

            migrationBuilder.DropColumn(
                name: "AppTenantId",
                schema: "accounting",
                table: "TenantExpensePens");

            migrationBuilder.CreateIndex(
                name: "IX_TenantExpensePens_TenantId",
                schema: "accounting",
                table: "TenantExpensePens",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_TenantExpensePens_AppTenants_TenantId",
                schema: "accounting",
                table: "TenantExpensePens",
                column: "TenantId",
                principalSchema: "accounting",
                principalTable: "AppTenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TenantExpensePens_AppTenants_TenantId",
                schema: "accounting",
                table: "TenantExpensePens");

            migrationBuilder.DropIndex(
                name: "IX_TenantExpensePens_TenantId",
                schema: "accounting",
                table: "TenantExpensePens");

            migrationBuilder.AddColumn<Guid>(
                name: "AppTenantId",
                schema: "accounting",
                table: "TenantExpensePens",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TenantExpensePens_AppTenantId",
                schema: "accounting",
                table: "TenantExpensePens",
                column: "AppTenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_TenantExpensePens_AppTenants_AppTenantId",
                schema: "accounting",
                table: "TenantExpensePens",
                column: "AppTenantId",
                principalSchema: "accounting",
                principalTable: "AppTenants",
                principalColumn: "Id");
        }
    }
}
