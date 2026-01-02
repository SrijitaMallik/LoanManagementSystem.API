using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoanManagementSystem.API.Models;

public class Loan
{
    public int LoanId { get; set; }
    public int UserId { get; set; }

    public int LoanTypeId { get; set; }
    public LoanType? LoanType { get; set; }

    public decimal Amount { get; set; }
    public int TenureMonths { get; set; }
    public DateTime AppliedOn { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public bool IsApproved { get; set; }
    public string Status { get; set; } = "Pending";
    public string? Remarks { get; set; }
}
