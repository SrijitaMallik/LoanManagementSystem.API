using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoanManagementSystem.API.Models
{
    public class LoanType
    {
        [Key]
        public int LoanTypeId { get; set; }

        [Required]
        [MaxLength(100)]
        public string LoanTypeName { get; set; } = string.Empty;

        [Column(TypeName = "decimal(10,2)")]
        public decimal InterestRate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal MinAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal MaxAmount { get; set; }

        public int MaxTenureMonths { get; set; }
        public bool IsActive { get; set; } = true;

    }
}
