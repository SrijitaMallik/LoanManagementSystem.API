using LoanManagementSystem.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
}
