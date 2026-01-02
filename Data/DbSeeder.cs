using LoanManagementSystem.API.Models;

namespace LoanManagementSystem.API.Data
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (!context.Users.Any(x => x.Role == "Admin"))
            {
                context.Users.Add(new User
                {
                    FullName = "System Admin",
                    Email = "admin@lms.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                    Role = "Admin",
                    IsApproved = true,
                    IsActive = true
                });

                context.SaveChanges();
            }
        }
    }
}
