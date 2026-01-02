using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoanManagementSystem.API.Models
{
    public class LoanApplication
    {
        public int LoanApplicationId { get; set; }

        public int CustomerId { get; set; }
        public User Customer { get; set; }

        public int LoanTypeId { get; set; }
        public LoanType LoanType { get; set; }

        public decimal LoanAmount { get; set; }
        public int TenureMonths { get; set; }
        public decimal MonthlyIncome { get; set; }

        public string Status { get; set; } = "Pending";

        // 🔥 Missing fields added
        public bool IsVerified { get; set; } = false;
        public string? VerificationRemarks { get; set; }
        public decimal EmiAmount { get; set; } = 0;
    }
}

