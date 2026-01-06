using LoanManagementSystem.API.Data;
using LoanManagementSystem.API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LoanManagementSystem.API.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/admin")]
public class AdminController : ControllerBase
{
    private readonly AppDbContext _context;

    public AdminController(AppDbContext context)
    {
        _context = context;
    }

    // ---------------- PENDING OFFICERS ----------------

    [HttpGet("pending-officers")]
    public async Task<IActionResult> PendingOfficers()
    {
        var officers = await _context.Users
            .Where(x => x.Role == "LoanOfficer" && !x.IsApproved)
            .Select(x => new { x.UserId, x.FullName, x.Email })
            .ToListAsync();

        return Ok(officers);
    }

    [HttpPut("approve-officer/{id}")]
    public async Task<IActionResult> ApproveOfficer(int id)
    {
        var officer = await _context.Users.FindAsync(id);
        if (officer == null) return NotFound();

        officer.IsApproved = true;
        officer.IsActive = true;
        await _context.SaveChangesAsync();

        return Ok(new { message = "Loan Officer Approved" });
    }

    [HttpDelete("reject-officer/{id}")]
    public async Task<IActionResult> RejectOfficer(int id)
    {
        var officer = await _context.Users.FindAsync(id);
        if (officer == null) return NotFound();

        _context.Users.Remove(officer);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Loan Officer Rejected" });
    }


    // ---------------- LOANS ----------------

    [HttpGet("verified-loans")]
    public async Task<IActionResult> VerifiedLoans()
    {
        var loans = await _context.LoanApplications
            .Where(x => x.Status == "Verified")
            .ToListAsync();

        return Ok(loans);
    }

    [HttpPut("approve-loan/{id}")]
    public async Task<IActionResult> ApproveLoan(int id)
    {
        var loan = await _context.LoanApplications.FindAsync(id);
        if (loan == null) return NotFound();

        loan.Status = "Approved";
        await _context.SaveChangesAsync();
        return Ok("Loan Approved");
    }

    [HttpPut("reject-loan/{id}")]
    public async Task<IActionResult> RejectLoan(int id)
    {
        var loan = await _context.LoanApplications.FindAsync(id);
        if (loan == null) return NotFound();

        loan.Status = "Rejected";
        await _context.SaveChangesAsync();
        return Ok("Loan Rejected");
    }

    // ---------------- DASHBOARD REPORTS ----------------

    [HttpGet("reports/loans-by-status")]
    public async Task<IActionResult> LoansByStatus()
    {
        var data = await _context.LoanApplications
            .GroupBy(l => l.Status)
            .Select(g => new { status = g.Key, count = g.Count() })
            .ToListAsync();

        return Ok(data);
    }

    [HttpGet("reports/active-vs-closed")]
    public async Task<IActionResult> ActiveVsClosed()
    {
        var active = await _context.LoanApplications.CountAsync(x => x.Status == "Approved");
        var closed = await _context.LoanApplications.CountAsync(x =>
            x.Status == "Closed" || x.Status == "Paid" || x.Status == "Completed");

        return Ok(new { active, closed });
    }

    [HttpGet("reports/customer-summary")]
    public async Task<IActionResult> CustomerSummary()
    {
        var data = await _context.LoanApplications
            .GroupBy(l => l.CustomerId)
            .Select(g => new
            {
                customerId = g.Key,
                totalLoans = g.Count(),
                activeLoans = g.Count(x => x.Status == "Approved"),
                rejectedLoans = g.Count(x => x.Status == "Rejected"),
                closedLoans = g.Count(x => x.Status == "Closed"),
                totalAmount = g.Sum(x => x.LoanAmount)
            })
            .ToListAsync();

        return Ok(data);
    }
    // ---------------- LOAN TYPES ----------------

    [HttpGet("loan-types")]
    public async Task<IActionResult> GetLoanTypes()
    {
        return Ok(await _context.LoanTypes.Where(x => x.IsActive).ToListAsync());
    }

    [HttpPut("loan-types/{id}")]
    public async Task<IActionResult> LoanType(int id, LoanTypeDTO dto)
    {
        var loan = await _context.LoanTypes.FindAsync(id);
        if (loan == null) return NotFound();

        loan.LoanTypeName = dto.LoanTypeName;
        loan.InterestRate = dto.InterestRate;
        loan.MinAmount = dto.MinAmount;
        loan.MaxAmount = dto.MaxAmount;
        loan.MaxTenureMonths = dto.MaxTenureMonths;

        await _context.SaveChangesAsync();

        return Ok(new { message = "Loan type updated successfully" });
    }


    [HttpGet("loan-types/{id}")]
    public async Task<IActionResult> GetLoanTypeById(int id)
    {
        var loan = await _context.LoanTypes.FindAsync(id);
        if (loan == null) return NotFound();
        return Ok(loan);
    }

}
