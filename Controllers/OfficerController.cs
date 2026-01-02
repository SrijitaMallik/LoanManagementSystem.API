using LoanManagementSystem.API.Data;
using LoanManagementSystem.API.DTOs;
using LoanManagementSystem.API.Models;
using LoanManagementSystem.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LoanManagementSystem.API.Controllers
{
    [Authorize(Roles = "LoanOfficer")]
    [ApiController]
    [Route("api/officer")]
    public class OfficerController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly EmiCalculatorService _emiService;

        public OfficerController(AppDbContext context, EmiCalculatorService emiService)
        {
            _context = context;
            _emiService = emiService;
        }

        [HttpGet("pending-loans")]
        public IActionResult GetPendingLoans()
        {
            var loans = _context.LoanApplications
                .Include(l => l.LoanType)
                .Where(l => l.Status == "Pending")
                .ToList();

            return Ok(loans);
        }

        [Authorize(Roles = "LoanOfficer")]
        [HttpPut("verify/{id}")]
        public async Task<IActionResult> VerifyLoan(int id, [FromBody] VerifyLoanDto dto)
        {
            var loan = await _context.LoanApplications
                .Include(l => l.LoanType)
                .FirstOrDefaultAsync(l => l.LoanId == id);

            if (loan == null)
                return NotFound("Loan not found");

            if (loan.Status != "Pending")
                return BadRequest("Loan already processed");

            if (!dto.IsApproved)
            {
                if (string.IsNullOrWhiteSpace(dto.Remarks))
                    return BadRequest("Remarks are required when rejecting a loan");

                loan.Status = "Rejected";
                loan.Remarks = dto.Remarks;

                await _context.SaveChangesAsync();
                return Ok("Loan Rejected");
            }

            loan.Status = "Approved";
            loan.ApprovedDate = DateTime.UtcNow;
            loan.Remarks = dto.Remarks;

            var emiAmount = _emiService.CalculateEMI(
                loan.LoanAmount,
                loan.LoanType.InterestRate,
                loan.TenureMonths
            );

            for (int i = 1; i <= loan.TenureMonths; i++)
            {
                _context.EMIs.Add(new EMI
                {
                    LoanId = loan.LoanId,
                    InstallmentNumber = i,
                    DueDate = DateTime.UtcNow.AddMonths(i),
                    EMIAmount = emiAmount,
                    IsPaid = false
                });
            }

            loan.Status = "Active";
            await _context.SaveChangesAsync();

            return Ok("Loan Approved & EMI Schedule Generated");
        }
    }
}
