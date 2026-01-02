using System;
using System.ComponentModel.DataAnnotations;

namespace LoanManagementSystem.API.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }


        [Required, MaxLength(100)]
        public required string FullName { get; set; }

        [Required, EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string PasswordHash { get; set; }

        [Required]
        public required string Role { get; set; }

        public bool IsActive { get; set; }
        public bool IsApproved { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
