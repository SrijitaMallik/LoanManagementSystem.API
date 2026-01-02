using LoanManagementSystem.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LoanManagementSystem.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<LoanType> LoanTypes { get; set; }
        public DbSet<LoanApplication> LoanApplications { get; set; }
        public DbSet<EMI> EMIs => Set<EMI>();

        public DbSet<Loan> Loans { get; set; }

        public DbSet<Payment> Payments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LoanType>()
                .Property(x => x.InterestRate).HasPrecision(10, 2);

            modelBuilder.Entity<LoanType>()
                .Property(x => x.MinAmount).HasPrecision(18, 2);

            modelBuilder.Entity<LoanType>()
                .Property(x => x.MaxAmount).HasPrecision(18, 2);

            modelBuilder.Entity<LoanApplication>()
                .Property(x => x.LoanAmount).HasPrecision(18, 2);

            modelBuilder.Entity<LoanApplication>()
                .Property(x => x.MonthlyIncome).HasPrecision(18, 2);

            modelBuilder.Entity<EMI>()
                .Property(x => x.EMIAmount).HasPrecision(18, 2);

            modelBuilder.Entity<Payment>()
                .Property(x => x.PaidAmount).HasPrecision(18, 2);

            base.OnModelCreating(modelBuilder);
        }
    }
}
