using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanManagementSystem.API.Migrations
{
    /// <inheritdoc />
    public partial class AddLoanNavigation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AppliedDate",
                table: "Loans",
                newName: "AppliedOn");

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedDate",
                table: "Loans",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Loans_LoanTypeId",
                table: "Loans",
                column: "LoanTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EMIs_LoanId",
                table: "EMIs",
                column: "LoanId");

            migrationBuilder.AddForeignKey(
                name: "FK_EMIs_Loans_LoanId",
                table: "EMIs",
                column: "LoanId",
                principalTable: "Loans",
                principalColumn: "LoanId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_LoanTypes_LoanTypeId",
                table: "Loans",
                column: "LoanTypeId",
                principalTable: "LoanTypes",
                principalColumn: "LoanTypeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EMIs_Loans_LoanId",
                table: "EMIs");

            migrationBuilder.DropForeignKey(
                name: "FK_Loans_LoanTypes_LoanTypeId",
                table: "Loans");

            migrationBuilder.DropIndex(
                name: "IX_Loans_LoanTypeId",
                table: "Loans");

            migrationBuilder.DropIndex(
                name: "IX_EMIs_LoanId",
                table: "EMIs");

            migrationBuilder.DropColumn(
                name: "ApprovedDate",
                table: "Loans");

            migrationBuilder.RenameColumn(
                name: "AppliedOn",
                table: "Loans",
                newName: "AppliedDate");
        }
    }
}
