using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Customers.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixDistrictIdSequence : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Fix the sequence for Districts table to start after the highest existing ID
            migrationBuilder.Sql(@"
                SELECT setval(pg_get_serial_sequence('customers.""Districts""', 'Id'), 
                    COALESCE((SELECT MAX(""Id"") FROM customers.""Districts""), 0) + 1, false);
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // No down migration needed for sequence fix
        }
    }
}
