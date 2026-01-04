using System.ComponentModel.DataAnnotations;

namespace LoanManagementSystem.API.Models
{
    public class LoanNotification
    {
        [Key]
        public int Id { get; set; }

        public int LoanId { get; set; }          // Kis loan ke liye
        public int UserId { get; set; }          // Kis user ko notify karna
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        // Full message
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public bool IsRead { get; set; } = false;
    }
}
