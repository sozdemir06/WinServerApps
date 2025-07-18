﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Users.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRoleGroupAndTranslatesss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleGroupTranslatates_Languages_LanguageId",
                schema: "users",
                table: "RoleGroupTranslatates");

            migrationBuilder.AlterColumn<Guid>(
                name: "RoleGroupId",
                schema: "users",
                table: "RoleGroupTranslatates",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "LanguageId",
                schema: "users",
                table: "RoleGroupTranslatates",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleGroupTranslatates_Languages_LanguageId",
                schema: "users",
                table: "RoleGroupTranslatates",
                column: "LanguageId",
                principalSchema: "users",
                principalTable: "Languages",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleGroupTranslatates_Languages_LanguageId",
                schema: "users",
                table: "RoleGroupTranslatates");

            migrationBuilder.AlterColumn<Guid>(
                name: "RoleGroupId",
                schema: "users",
                table: "RoleGroupTranslatates",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "LanguageId",
                schema: "users",
                table: "RoleGroupTranslatates",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleGroupTranslatates_Languages_LanguageId",
                schema: "users",
                table: "RoleGroupTranslatates",
                column: "LanguageId",
                principalSchema: "users",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
