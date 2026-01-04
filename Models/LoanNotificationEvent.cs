namespace LoanManagementSystem.API.Models
{
    public class LoanNotificationEvent
    {
        public int LoanId { get; set; }
        public int UserId { get; set; }     // Jisko notify karna hai
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

    }
}
