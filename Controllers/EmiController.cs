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
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var emis = await _context.EMIs
            .Include(e => e.Loan)
            .Where(e => e.Loan.UserId == userId)
            .Select(e => new
            {
                e.EMIId,
                e.InstallmentNumber,
                e.EMIAmount,
                e.DueDate,
                e.IsPaid
            })
            .ToListAsync();

        return Ok(emis);
    }

    // PAY EMI
    [HttpPut("{id}/pay")]
    public async Task<IActionResult> PayEmi(int id)
    {
        var emi = await _context.EMIs.FindAsync(id);
        if (emi == null || emi.IsPaid) return BadRequest("Invalid EMI");

        emi.IsPaid = true;
        emi.PaidDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return Ok("EMI Paid Successfully");
    }
}
