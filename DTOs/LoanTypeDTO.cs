namespace LoanManagementSystem.API.DTOs
{
    public class LoanTypeDTO

    {
        public int LoanTypeId { get; set; }
        public string LoanTypeName { get; set; } = null!;
        public decimal InterestRate { get; set; }
        public decimal MinAmount { get; set; }
        public decimal MaxAmount { get; set; }
        public int MaxTenureMonths { get; set; }
    }
}
