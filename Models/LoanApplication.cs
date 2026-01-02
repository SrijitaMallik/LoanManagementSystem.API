using System;
using System.ComponentModel.DataAnnotations;

namespace LoanManagementSystem.API.Models
{
    public class LoanApplication
    {
        [Key]
        public int LoanId { get; set; }

        public int CustomerId { get; set; }
        public int LoanTypeId { get; set; }
        public User Customer { get; set; }
        public LoanType LoanType { get; set; }
        public decimal LoanAmount { get; set; }
        public int TenureMonths { get; set; }

        public decimal MonthlyIncome { get; set; }

        public required string Status { get; set; }

        public DateTime AppliedDate { get; set; } = DateTime.Now;
        public DateTime? ApprovedDate { get; set; }
        public int? ApprovedBy { get; set; }

        public string? Remarks { get; set; }

        public ICollection<EMI> EMIs { get; set; } = new List<EMI>();

    }
}
