using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Customers.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixCityIdSequence : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Fix the sequence for Cities table to start after the highest existing ID
            migrationBuilder.Sql(@"
                SELECT setval(pg_get_serial_sequence('customers.""Cities""', 'Id'), 
                    COALESCE((SELECT MAX(""Id"") FROM customers.""Cities""), 0) + 1, false);
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // No down migration needed for sequence fix
        }
    }
}
