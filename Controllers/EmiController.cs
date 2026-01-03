using LoanManagementSystem.API.Data;
using LoanManagementSystem.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LoanManagementSystem.API.Controllers;

[Authorize(Roles = "Customer")]
[ApiController]
[Route("api/emis")]
public class EmiController : ControllerBase
{
    private readonly AppDbContext _context;

    public EmiController(AppDbContext context)
    {
        _context = context;
    }

    // CUSTOMER EMI LIST
    [HttpGet("my-emis")]
    public async Task<IActionResult> GetMyEmis()
    {
        int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var emis = await _context.EmiSchedules
     .Include(e => e.LoanApplication)
         .ThenInclude(l => l.LoanType)
     .Where(e => e.LoanApplication.CustomerId == userId && !e.IsPaid)
     .GroupBy(e => e.LoanApplicationId)
     .Select(g => new
     {
         LoanApplicationId = g.Key,
         LoanType = g.First().LoanApplication.LoanType!.LoanTypeName,
         LoanAmount = g.First().LoanApplication.LoanAmount,
         TenureMonths = g.First().LoanApplication.TenureMonths,
         InterestRate = g.First().LoanApplication.LoanType.InterestRate,
         Emi = g.First().EmiAmount,   // 👈 real EMI
         PendingMonths = g.Count(),
         Status = g.First().LoanApplication.Status
     })
     .ToListAsync();


        return Ok(emis);
    }

    // PAY EMI
    [HttpPut("{loanApplicationId}/pay")]
    public async Task<IActionResult> PayEmi(int loanApplicationId)
    {
        int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var loan = await _context.LoanApplications
            .FirstOrDefaultAsync(l => l.LoanApplicationId == loanApplicationId && l.CustomerId == userId);

        if (loan == null)
            return NotFound("Loan not found");

        if (loan.Status != "Approved")
            return BadRequest("Loan not approved");

        // Get next unpaid EMI
        var nextEmi = await _context.EmiSchedules
            .Where(e => e.LoanApplicationId == loanApplicationId && !e.IsPaid)
            .OrderBy(e => e.MonthNumber)
            .FirstOrDefaultAsync();

        if (nextEmi == null)
            return BadRequest("All EMIs already paid");

        // Pay this EMI
        nextEmi.IsPaid = true;
        _context.Receipts.Add(new Receipt
        {
            LoanApplicationId = loanApplicationId,
            EmiScheduleId = nextEmi.EmiScheduleId,
            PaidAmount = nextEmi.EmiAmount
        });

        // If this was last EMI → close loan
        bool allPaid = !_context.EmiSchedules
            .Any(e => e.LoanApplicationId == loanApplicationId && !e.IsPaid);

        if (allPaid)
            loan.Status = "Closed";

        await _context.SaveChangesAsync();
        return Ok($"EMI for Month {nextEmi.MonthNumber} paid successfully.");
    }
    [HttpGet("{loanApplicationId}/outstanding")]
    public async Task<IActionResult> GetOutstanding(int loanApplicationId)
    {
        int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var emis = await _context.EmiSchedules
            .Where(e => e.LoanApplicationId == loanApplicationId && !e.IsPaid)
            .ToListAsync();

        decimal outstanding = emis.Sum(e => e.EmiAmount);
        return Ok(new { outstanding });
    }
    [HttpGet("{loanApplicationId}/receipts")]
    public async Task<IActionResult> GetReceipts(int loanApplicationId)
    {
        int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var receipts = await _context.Receipts
            .Where(r => r.LoanApplicationId == loanApplicationId)
            .ToListAsync();

        return Ok(receipts);
    }



}
