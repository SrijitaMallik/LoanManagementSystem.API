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

    [HttpGet("pending-officers")]
    public IActionResult PendingOfficers()
    {
        var officers = _context.Users
            .Where(x => x.Role == "LoanOfficer" && x.IsApproved == false)
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

    [HttpPut("reject-officer/{id}")]
    public IActionResult RejectOfficer(int id)
    {
        var officer = _context.Users.Find(id);
        if (officer == null) return NotFound();

        _context.Users.Remove(officer);
        _context.SaveChanges();
        return Ok("Loan Officer Rejected");
    }

    [HttpGet("verified-loans")]
    public IActionResult VerifiedLoans()
    {
        var loans = _context.Loans.Where(x => x.Status == "Verified").ToList();
        return Ok(loans);
    }

    [HttpPut("approve-loan/{id}")]
    public IActionResult ApproveLoan(int id)
    {
        var loan = _context.Loans.Find(id);
        if (loan == null) return NotFound();

        loan.Status = "Approved";
        _context.SaveChanges();
        return Ok("Loan Approved");
    }

    [HttpPut("reject-loan/{id}")]
    public IActionResult RejectLoan(int id)
    {
        var loan = _context.Loans.Find(id);
        if (loan == null) return NotFound();

        loan.Status = "Rejected";
        _context.SaveChanges();
        return Ok("Loan Rejected");
    }
    [Authorize(Roles = "Admin")]
    [HttpGet("reports/loans-by-status")]
    public async Task<IActionResult> LoansByStatus()
    {
        var data = await _context.LoanApplications
            .GroupBy(l => l.Status)
            .Select(g => new
            {
                status = g.Key,
                count = g.Count()
            }).ToListAsync();

        return Ok(data);
    }
    [Authorize(Roles = "Admin")]
    [HttpGet("reports/active-vs-closed")]
    public async Task<IActionResult> ActiveVsClosed()
    {
        var active = await _context.LoanApplications.CountAsync(l => l.Status == "Approved");
        var closed = await _context.LoanApplications.CountAsync(l => l.Status == "Closed");

        return Ok(new { active, closed });
    }
    [Authorize(Roles = "Admin")]
    [HttpGet("reports/emi-overdue")]
    public async Task<IActionResult> EmiOverdue()
    {
        var overdue = await _context.EmiSchedules
            .Include(e => e.LoanApplication)
            .Where(e => !e.IsPaid && e.DueDate < DateTime.Now)
            .Select(e => new
            {
                e.LoanApplicationId,
                e.MonthNumber,
                e.EmiAmount,
                e.DueDate
            }).ToListAsync();

        return Ok(overdue);
    }

    [Authorize(Roles = "Admin")]
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
                rejectedLoans=g.Count(x=>x.Status=="Rejected"),
                closedLoans = g.Count(x => x.Status == "Closed"),
                totalAmount = g.Sum(x => x.LoanAmount)
            }).ToListAsync();

        return Ok(data);
    }


}
