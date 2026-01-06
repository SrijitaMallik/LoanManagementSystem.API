using LoanManagementSystem.API.Data;
using LoanManagementSystem.API.DTOs;
using LoanManagementSystem.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LoanManagementSystem.API.Controllers
{
    [Authorize(Roles = "Customer")]
    [ApiController]
    [Route("api/loans")]
    public class LoanController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LoanController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("apply")]
        public async Task<IActionResult> ApplyLoan(LoanApplyDTO dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            // ✅ Check LoanType exists
            var loanType = await _context.LoanTypes
                .FirstOrDefaultAsync(x => x.LoanTypeId == dto.LoanTypeId);

            if (loanType == null)
                return BadRequest("Invalid Loan Type");

            var application = new LoanApplication
            {
                CustomerId = userId,
                LoanTypeId = dto.LoanTypeId,
                LoanAmount = dto.LoanAmount,
                TenureMonths = dto.TenureMonths,
                MonthlyIncome = dto.MonthlyIncome,
                Status = "Pending"
            };

            _context.LoanApplications.Add(application);
            await _context.SaveChangesAsync();

            await LoanNotificationQueue.Channel.Writer.WriteAsync(
                new LoanNotificationEvent
                {
                    LoanId = application.LoanApplicationId,
                    UserId = userId,
                    Title = "Loan Applied",
                    Message = "Your loan application has been submitted successfully and is under review."
                });

            return Ok(new { message = "Loan applied successfully" });

        }

        [HttpGet("my-applications")]
        public async Task<IActionResult> GetMyLoans()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var loans = await _context.LoanApplications
    .Include(x => x.LoanType)
    .Where(x => x.CustomerId == userId)
    .Select(x => new
    {
        x.LoanApplicationId,
        x.Status,
        LoanTypeName = x.LoanType.LoanTypeName,
        x.LoanAmount,
        x.TenureMonths,
        EmiAmount = x.EmiAmount
    })
    .ToListAsync();
            

            return Ok(loans);
        }
        [HttpPost("pay-emi/{loanId}")]
        public async Task<IActionResult> PayEmi(int loanId, decimal amount)
        {
            var loan = await _context.LoanApplications.FindAsync(loanId);
            if (loan == null) return NotFound("Loan not found");

            if (loan.Status != "Approved")
                return BadRequest("Loan is not active");

            loan.OutstandingAmount -= amount;

            if (loan.OutstandingAmount <= 0)
            {
                loan.OutstandingAmount = 0;
                loan.Status = "Closed";   // 🔥 AUTO CLOSE
            }

            await _context.SaveChangesAsync();
            return Ok(loan);
        }
        [HttpGet("my-active-loans")]
        public async Task<IActionResult> MyActiveLoans()
        {
            var uid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var loans = await _context.LoanApplications
                .Where(l => l.CustomerId == uid && l.Status != "Closed")
                .ToListAsync();

            return Ok(loans);
        }
        [HttpGet("my-closed-loans")]
        public async Task<IActionResult> MyClosedLoans()
        {
            var uid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var loans = await _context.LoanApplications
                .Where(l => l.CustomerId == uid && l.Status == "Closed")
                .ToListAsync();

            return Ok(loans);
        }


    }
}