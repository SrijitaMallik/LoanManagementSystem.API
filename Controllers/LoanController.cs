using LoanManagementSystem.API.Data;
using LoanManagementSystem.API.DTOs;
using LoanManagementSystem.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LoanManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/loans")]
    public class LoanController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LoanController(AppDbContext context)
        {
            _context = context;
        }

        // CUSTOMER → APPLY LOAN
        [Authorize(Roles = "Customer")]
        [HttpPost("apply")]
        public async Task<IActionResult> ApplyLoan(LoanApplyDTO dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("UserId not found in token");

            int userId = int.Parse(userIdClaim.Value);

            var loan = new LoanApplication
            {
                CustomerId = userId,
                LoanTypeId = dto.LoanTypeId,
                LoanAmount = dto.LoanAmount,
                TenureMonths = dto.TenureMonths,
                MonthlyIncome = dto.MonthlyIncome,
                Status = "Pending",
                AppliedDate = DateTime.UtcNow
            };

            _context.LoanApplications.Add(loan);
            await _context.SaveChangesAsync();

            return Ok("Loan Applied Successfully");
        }

        // CUSTOMER → VIEW OWN LOANS
        [Authorize(Roles = "Customer")]
        [HttpGet("my-loans")]
        public IActionResult MyLoans()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var loans = _context.LoanApplications
                .Where(l => l.CustomerId == userId)
                .ToList();

            return Ok(loans);
        }
    }
}
