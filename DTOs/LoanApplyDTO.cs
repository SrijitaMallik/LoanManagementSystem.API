namespace LoanManagementSystem.API.DTOs
{
    public class LoanApplyDTO
    {
        public int LoanTypeId { get; set; }
        public decimal LoanAmount { get; set; }
        public int TenureMonths { get; set; }
        public decimal MonthlyIncome { get; set; }
    }
}
