using System.ComponentModel.DataAnnotations;

namespace LoanManagementSystem.API.DTOs
{
    public class RegisterDTO
    {
        [Required]
        public required string FullName { get; set; }

        [Required]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }

        [Required]
        public required string Role { get; set; }
    }
}
