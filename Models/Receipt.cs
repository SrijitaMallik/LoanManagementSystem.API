using System;

namespace LoanManagementSystem.API.Models
{
    public class Receipt
    {
        public int ReceiptId { get; set; }
        public int LoanApplicationId { get; set; }
        public int EmiScheduleId { get; set; }
        public decimal PaidAmount { get; set; }
        public DateTime PaidOn { get; set; } = DateTime.Now;
    }
}
