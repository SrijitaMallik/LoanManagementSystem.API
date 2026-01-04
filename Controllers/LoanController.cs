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

        // Apply Loan
        [HttpPost("apply")]
        public async Task<IActionResult> ApplyLoan(LoanApplyDTO dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

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
            await LoanNotificationQueue.Channel.Writer.WriteAsync(new LoanNotificationEvent
            {
                LoanId = application.LoanApplicationId,
                UserId = userId,
                Title = "Loan Applied",
                Message = "Your loan application has been submitted successfully and is under review."
            });


            return Ok("Loan applied successfully");
        }

        // Get my loan applications
        [HttpGet("my-applications")]
        public async Task<IActionResult> GetMyLoans()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var loans = await _context.LoanApplications
                .Include(x => x.LoanType)
                .Where(x => x.CustomerId == userId)
                .ToListAsync();

            return Ok(loans);
        }
    }
}
