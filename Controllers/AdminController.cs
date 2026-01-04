using LoanManagementSystem.API.Data;
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
    public IActionResult PendingOfficers()
    {
        var officers = _context.Users
            .Where(x => x.Role == "LoanOfficer" && !x.IsApproved)
            .Select(x => new { x.UserId, x.FullName, x.Email })
            .ToList();

        return Ok(officers);
    }

    [HttpPut("approve-officer/{id}")]
    public IActionResult ApproveOfficer(int id)
    {
        var officer = _context.Users.Find(id);
        if (officer == null) return NotFound();

        officer.IsApproved = true;
        officer.IsActive = true;
        _context.SaveChanges();
        return Ok("Loan Officer Approved");
    }

    [HttpDelete("reject-officer/{id}")]
    public IActionResult RejectOfficer(int id)
    {
        var officer = _context.Users.Find(id);
        if (officer == null) return NotFound();

        _context.Users.Remove(officer);
        _context.SaveChanges();
        return Ok("Loan Officer Rejected");
    }

    // ---------------- LOANS ----------------

    [HttpGet("verified-loans")]
    public IActionResult VerifiedLoans()
    {
        return Ok(_context.LoanApplications.Where(x => x.Status == "Verified").ToList());
    }

    [HttpPut("approve-loan/{id}")]
    public IActionResult ApproveLoan(int id)
    {
        var loan = _context.LoanApplications.Find(id);
        if (loan == null) return NotFound();

        loan.Status = "Approved";
        _context.SaveChanges();
        return Ok("Loan Approved");
    }

    [HttpPut("reject-loan/{id}")]
    public IActionResult RejectLoan(int id)
    {
        var loan = _context.LoanApplications.Find(id);
        if (loan == null) return NotFound();

        loan.Status = "Rejected";
        _context.SaveChanges();
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
        return Ok(new
        {
            active = await _context.LoanApplications.CountAsync(x => x.Status == "Approved"),
            closed = await _context.LoanApplications.CountAsync(x => x.Status == "Closed")
        });
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
            }).ToListAsync();

        return Ok(data);
    }
}
