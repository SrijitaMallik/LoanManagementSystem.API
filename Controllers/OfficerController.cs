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

        // Get all pending loan applications
        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingLoans()
        {
            var loans = await _context.LoanApplications
                .Include(l => l.LoanType)
                .Where(l => l.Status == "Pending")
                .ToListAsync();

            return Ok(loans);
        }

        // Verify / Approve / Reject loan + EMI generation
        [HttpPut("verify/{id}")]
        public async Task<IActionResult> VerifyLoan(int id, [FromBody] VerifyLoanDTO dto)
        {
            var loan = await _context.LoanApplications
                .Include(l => l.LoanType)
                .FirstOrDefaultAsync(l => l.LoanApplicationId == id);

            if (loan == null)
                return NotFound("Loan not found");

            loan.IsVerified = true;
            loan.VerificationRemarks = dto.Remarks;
            loan.Status = dto.IsApproved ? "Approved" : "Rejected";

            if (dto.IsApproved)
            {
                var emi = _emiService.CalculateEmi(
                    loan.LoanAmount,
                    loan.TenureMonths,
                    loan.LoanType.InterestRate
                );

                for (int i = 1; i <= loan.TenureMonths; i++)
                {
                    _context.EmiSchedules.Add(new EmiSchedule
                    {
                        LoanApplicationId = loan.LoanApplicationId,
                        MonthNumber = i,
                        EmiAmount = emi,
                        DueDate = DateTime.Now.AddMonths(i)
                    });

                }
            }


            await _context.SaveChangesAsync();
            return Ok(loan);
        }
    }
}
