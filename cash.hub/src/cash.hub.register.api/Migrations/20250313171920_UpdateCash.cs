using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cash.hub.register.api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Balance",
                table: "CashRegisters",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Balance",
                table: "CashRegisters");
        }
    }
}
