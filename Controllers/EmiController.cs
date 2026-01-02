using LoanManagementSystem.API.Data;
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

        var emis = await _context.LoanApplications
            .Include(l => l.LoanType)
            .Where(l => l.CustomerId == userId && l.Status == "Approved")
            .Select(l => new
            {
                l.LoanApplicationId,
                LoanType = l.LoanType!.LoanTypeName,
                l.LoanAmount,
                l.TenureMonths,
                InterestRate = l.LoanType.InterestRate,
                Emi = l.EmiAmount,
                l.Status
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
            return BadRequest("Loan not approved yet");

        // EMI paid → Close loan
        loan.Status = "Closed";

        await _context.SaveChangesAsync();
        return Ok("EMI Paid Successfully. Loan Closed.");
    }
}
