using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanManagementSystem.API.Migrations
{
    /// <inheritdoc />
    public partial class AddLoanTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LoanApplicationLoanId",
                table: "EMIs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EMIs_LoanApplicationLoanId",
                table: "EMIs",
                column: "LoanApplicationLoanId");

            migrationBuilder.AddForeignKey(
                name: "FK_EMIs_LoanApplications_LoanApplicationLoanId",
                table: "EMIs",
                column: "LoanApplicationLoanId",
                principalTable: "LoanApplications",
                principalColumn: "LoanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EMIs_LoanApplications_LoanApplicationLoanId",
                table: "EMIs");

            migrationBuilder.DropIndex(
                name: "IX_EMIs_LoanApplicationLoanId",
                table: "EMIs");

            migrationBuilder.DropColumn(
                name: "LoanApplicationLoanId",
                table: "EMIs");
        }
    }
}
