using System;
using System.ComponentModel.DataAnnotations;

namespace LoanManagementSystem.API.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }

        public int EMIId { get; set; }

        public decimal PaidAmount { get; set; }
        public DateTime PaymentDate { get; set; }

        public required string PaymentMode { get; set; }
    }
}
