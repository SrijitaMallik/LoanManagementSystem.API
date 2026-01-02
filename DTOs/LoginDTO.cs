using System.ComponentModel.DataAnnotations;

namespace LoanManagementSystem.API.DTOs
{
    public class LoginDTO
    {
        [Required]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}
