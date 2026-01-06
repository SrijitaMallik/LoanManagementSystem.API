using System;

namespace LoanManagementSystem.API.Models
{
    public class EmiSchedule
    {
        public int EmiScheduleId { get; set; }   // 🔥 PRIMARY KEY

        public int LoanApplicationId { get; set; }
        public LoanApplication LoanApplication { get; set; }

        public int MonthNumber { get; set; }
        public decimal EmiAmount { get; set; }
        public DateTime DueDate { get; set; }

        public bool IsPaid { get; set; } = false;
        public DateTime? PaidOn { get; set; }
    }

}
