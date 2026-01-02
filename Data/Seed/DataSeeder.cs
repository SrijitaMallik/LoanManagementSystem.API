using LoanManagementSystem.API.Data;
using LoanManagementSystem.API.Models;
using BCrypt.Net;

public static class DataSeeder
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
        }

        if (!context.LoanTypes.Any())
        {
            context.LoanTypes.AddRange(
                new LoanType { LoanTypeName = "Personal Loan", InterestRate = 12, MinAmount = 50000, MaxAmount = 500000, MaxTenureMonths = 36, IsActive = true },
                new LoanType { LoanTypeName = "Education Loan", InterestRate = 9, MinAmount = 100000, MaxAmount = 2000000, MaxTenureMonths = 60, IsActive = true },
                new LoanType { LoanTypeName = "Home Loan", InterestRate = 7, MinAmount = 500000, MaxAmount = 50000000, MaxTenureMonths = 240, IsActive = true }
            );
        }

        context.SaveChanges();
    }
}
