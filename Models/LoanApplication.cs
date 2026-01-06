using LoanManagementSystem.API.Models;

public class LoanApplication
{
    public int LoanApplicationId { get; set; }
    public int CustomerId { get; set; }
    public int LoanTypeId { get; set; }
    public User Customer { get; set; }
    public LoanType LoanType { get; set; }   // ADD

    public decimal LoanAmount { get; set; }
    public int TenureMonths { get; set; }
    public decimal MonthlyIncome { get; set; }

    public string Status { get; set; } = "Pending";
    public bool IsVerified { get; set; }
    public string? VerificationRemarks { get; set; }

    public decimal EmiAmount { get; set; }
    public decimal OutstandingAmount { get; set; }
    public DateTime? VerifiedOn { get; set; }

}
