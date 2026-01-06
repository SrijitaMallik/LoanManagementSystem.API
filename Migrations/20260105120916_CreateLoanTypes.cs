using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanManagementSystem.API.Migrations
{
    /// <inheritdoc />
    public partial class CreateLoanTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoanApplications_LoanTypes_LoanTypeId",
                table: "LoanApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_Loans_LoanTypes_LoanTypeId",
                table: "Loans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LoanTypes",
                table: "LoanTypes");

            migrationBuilder.RenameTable(
                name: "LoanTypes",
                newName: "LoanType");

            migrationBuilder.AlterColumn<string>(
                name: "LoanTypeName",
                table: "LoanType",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LoanType",
                table: "LoanType",
                column: "LoanTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_LoanApplications_LoanType_LoanTypeId",
                table: "LoanApplications",
                column: "LoanTypeId",
                principalTable: "LoanType",
                principalColumn: "LoanTypeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_LoanType_LoanTypeId",
                table: "Loans",
                column: "LoanTypeId",
                principalTable: "LoanType",
                principalColumn: "LoanTypeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoanApplications_LoanType_LoanTypeId",
                table: "LoanApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_Loans_LoanType_LoanTypeId",
                table: "Loans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LoanType",
                table: "LoanType");

            migrationBuilder.RenameTable(
                name: "LoanType",
                newName: "LoanTypes");

            migrationBuilder.AlterColumn<string>(
                name: "LoanTypeName",
                table: "LoanTypes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddPrimaryKey(
                name: "PK_LoanTypes",
                table: "LoanTypes",
                column: "LoanTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_LoanApplications_LoanTypes_LoanTypeId",
                table: "LoanApplications",
                column: "LoanTypeId",
                principalTable: "LoanTypes",
                principalColumn: "LoanTypeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_LoanTypes_LoanTypeId",
                table: "Loans",
                column: "LoanTypeId",
                principalTable: "LoanTypes",
                principalColumn: "LoanTypeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
