using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Customers.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixCityCountryRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Countries_CountryId1",
                schema: "customers",
                table: "Cities");

            migrationBuilder.DropIndex(
                name: "IX_Cities_CountryId1",
                schema: "customers",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "CountryId1",
                schema: "customers",
                table: "Cities");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                schema: "customers",
                table: "Countries",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                comment: "Country type (e.g., sovereign state)",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Tld",
                schema: "customers",
                table: "Countries",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true,
                comment: "Top-level domain",
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Subregion",
                schema: "customers",
                table: "Countries",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                comment: "Geographic subregion",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Region",
                schema: "customers",
                table: "Countries",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                comment: "Geographic region",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneCode",
                schema: "customers",
                table: "Countries",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true,
                comment: "International dialing code",
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NumericCode",
                schema: "customers",
                table: "Countries",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                comment: "ISO 3166-1 numeric code",
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Native",
                schema: "customers",
                table: "Countries",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                comment: "Native name",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Nationality",
                schema: "customers",
                table: "Countries",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                comment: "Nationality name",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "customers",
                table: "Countries",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                comment: "Country name",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                schema: "customers",
                table: "Countries",
                type: "numeric(11,8)",
                precision: 11,
                scale: 8,
                nullable: true,
                comment: "Geographic longitude",
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                schema: "customers",
                table: "Countries",
                type: "numeric(10,8)",
                precision: 10,
                scale: 8,
                nullable: true,
                comment: "Geographic latitude",
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Iso3",
                schema: "customers",
                table: "Countries",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                comment: "ISO 3166-1 alpha-3 code",
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Iso2",
                schema: "customers",
                table: "Countries",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                comment: "ISO 3166-1 alpha-2 code",
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                schema: "customers",
                table: "Countries",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "Soft delete flag",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "EmojiHtml",
                schema: "customers",
                table: "Countries",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                comment: "HTML code for flag emoji",
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Emoji",
                schema: "customers",
                table: "Countries",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true,
                comment: "Country flag emoji",
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CurrencySymbol",
                schema: "customers",
                table: "Countries",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true,
                comment: "Currency symbol",
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CurrencyName",
                schema: "customers",
                table: "Countries",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                comment: "Currency name",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Currency",
                schema: "customers",
                table: "Countries",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                comment: "Currency code",
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CountryCode",
                schema: "customers",
                table: "Countries",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                comment: "Country code (e.g., US, GB)",
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Capital",
                schema: "customers",
                table: "Countries",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                comment: "Capital city name",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                schema: "customers",
                table: "Cities",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "Soft delete flag",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Type",
                schema: "customers",
                table: "Countries",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true,
                oldComment: "Country type (e.g., sovereign state)");

            migrationBuilder.AlterColumn<string>(
                name: "Tld",
                schema: "customers",
                table: "Countries",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10,
                oldNullable: true,
                oldComment: "Top-level domain");

            migrationBuilder.AlterColumn<string>(
                name: "Subregion",
                schema: "customers",
                table: "Countries",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true,
                oldComment: "Geographic subregion");

            migrationBuilder.AlterColumn<string>(
                name: "Region",
                schema: "customers",
                table: "Countries",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true,
                oldComment: "Geographic region");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneCode",
                schema: "customers",
                table: "Countries",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10,
                oldNullable: true,
                oldComment: "International dialing code");

            migrationBuilder.AlterColumn<string>(
                name: "NumericCode",
                schema: "customers",
                table: "Countries",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true,
                oldComment: "ISO 3166-1 numeric code");

            migrationBuilder.AlterColumn<string>(
                name: "Native",
                schema: "customers",
                table: "Countries",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldComment: "Native name");

            migrationBuilder.AlterColumn<string>(
                name: "Nationality",
                schema: "customers",
                table: "Countries",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true,
                oldComment: "Nationality name");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "customers",
                table: "Countries",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldComment: "Country name");

            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                schema: "customers",
                table: "Countries",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(11,8)",
                oldPrecision: 11,
                oldScale: 8,
                oldNullable: true,
                oldComment: "Geographic longitude");

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                schema: "customers",
                table: "Countries",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(10,8)",
                oldPrecision: 10,
                oldScale: 8,
                oldNullable: true,
                oldComment: "Geographic latitude");

            migrationBuilder.AlterColumn<string>(
                name: "Iso3",
                schema: "customers",
                table: "Countries",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true,
                oldComment: "ISO 3166-1 alpha-3 code");

            migrationBuilder.AlterColumn<string>(
                name: "Iso2",
                schema: "customers",
                table: "Countries",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true,
                oldComment: "ISO 3166-1 alpha-2 code");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                schema: "customers",
                table: "Countries",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false,
                oldComment: "Soft delete flag");

            migrationBuilder.AlterColumn<string>(
                name: "EmojiHtml",
                schema: "customers",
                table: "Countries",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true,
                oldComment: "HTML code for flag emoji");

            migrationBuilder.AlterColumn<string>(
                name: "Emoji",
                schema: "customers",
                table: "Countries",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10,
                oldNullable: true,
                oldComment: "Country flag emoji");

            migrationBuilder.AlterColumn<string>(
                name: "CurrencySymbol",
                schema: "customers",
                table: "Countries",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10,
                oldNullable: true,
                oldComment: "Currency symbol");

            migrationBuilder.AlterColumn<string>(
                name: "CurrencyName",
                schema: "customers",
                table: "Countries",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true,
                oldComment: "Currency name");

            migrationBuilder.AlterColumn<string>(
                name: "Currency",
                schema: "customers",
                table: "Countries",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true,
                oldComment: "Currency code");

            migrationBuilder.AlterColumn<string>(
                name: "CountryCode",
                schema: "customers",
                table: "Countries",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldComment: "Country code (e.g., US, GB)");

            migrationBuilder.AlterColumn<string>(
                name: "Capital",
                schema: "customers",
                table: "Countries",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldComment: "Capital city name");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                schema: "customers",
                table: "Cities",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false,
                oldComment: "Soft delete flag");

            migrationBuilder.AddColumn<long>(
                name: "CountryId1",
                schema: "customers",
                table: "Cities",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cities_CountryId1",
                schema: "customers",
                table: "Cities",
                column: "CountryId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Countries_CountryId1",
                schema: "customers",
                table: "Cities",
                column: "CountryId1",
                principalSchema: "customers",
                principalTable: "Countries",
                principalColumn: "Id");
        }
    }
}
