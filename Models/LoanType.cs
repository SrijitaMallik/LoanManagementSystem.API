using System.ComponentModel.DataAnnotations;

namespace LoanManagementSystem.API.Models
{
    public class LoanType
    {
        [Key]
        public int LoanTypeId { get; set; }


        [Required]
        public required string LoanTypeName { get; set; }

        public decimal InterestRate { get; set; }

        public decimal MinAmount { get; set; }
        public decimal MaxAmount { get; set; }

        public int MaxTenureMonths { get; set; }

        public bool IsActive { get; set; }
    }
}
