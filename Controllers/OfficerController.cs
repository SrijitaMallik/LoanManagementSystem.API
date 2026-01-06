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
loan.VerifiedOn = DateTime.Now;   // ✅ YEH LINE ADD KARO

            if (dto.IsApproved)
            {
                var emi = _emiService.CalculateEmi(
                    loan.LoanAmount,
                    loan.TenureMonths,
                    loan.LoanType.InterestRate
                );

                loan.OutstandingAmount = loan.LoanAmount; // 🔥 IMPORTANT

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

                await LoanNotificationQueue.Channel.Writer.WriteAsync(new LoanNotificationEvent
                {
                    LoanId = loan.LoanApplicationId,
                    UserId = loan.CustomerId,
                    Title = "Loan Approved",
                    Message = "Congratulations! Your loan has been approved."
                });
            }
            else
            {
                await LoanNotificationQueue.Channel.Writer.WriteAsync(new LoanNotificationEvent
                {
                    LoanId = loan.LoanApplicationId,
                    UserId = loan.CustomerId,
                    Title = "Loan Rejected",
                    Message = "Sorry, your loan application has been rejected."
                });
            }

            await _context.SaveChangesAsync();
            return Ok(loan);
        }
        [HttpGet("stats")]
        public IActionResult GetStats()
        {
            var total = _context.LoanApplications.Count();
            var pending = _context.LoanApplications.Count(x => x.Status == "Pending");
            var active = _context.LoanApplications.Count(x => x.Status == "Approved");
            var rejected = _context.LoanApplications.Count(x => x.Status == "Rejected");

            return Ok(new { total, pending, active, rejected });
        }
      [HttpGet("loan-history")]
public async Task<IActionResult> LoanHistory()
{
    var data = await _context.LoanApplications
        .Include(x => x.Customer)
        .Include(x => x.LoanType)
        .Where(x => x.Status == "Approved" || x.Status == "Rejected")
        .Select(x => new {
            customerName = x.Customer.FullName,
            loanType = x.LoanType.LoanTypeName,
            amount = x.LoanAmount,
            emi = x.EmiAmount,
            tenure = x.TenureMonths,
            status = x.Status,
            verifiedOn = x.VerifiedOn
        })
        .ToListAsync();

    return Ok(data);
}

    }
}
