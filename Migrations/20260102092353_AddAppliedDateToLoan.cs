using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanManagementSystem.API.Migrations
{
    /// <inheritdoc />
    public partial class AddAppliedDateToLoan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AppliedOn",
                table: "Loans",
                newName: "AppliedDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AppliedDate",
                table: "Loans",
                newName: "AppliedOn");
        }
    }
}
