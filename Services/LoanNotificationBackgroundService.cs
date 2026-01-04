using LoanManagementSystem.API.Data;
using LoanManagementSystem.API.Models;

namespace LoanManagementSystem.API.Services
{
    public class LoanNotificationBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public LoanNotificationBackgroundService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Loan Notification Service STARTED");

            await foreach (var evt in LoanNotificationQueue.Channel.Reader.ReadAllAsync(stoppingToken))
            {
                using var scope = _scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                db.LoanNotifications.Add(new LoanNotification
                {
                    LoanId = evt.LoanId,
                    UserId = evt.UserId,
                    Title = evt.Title,
                    Message = evt.Message
                });

                await db.SaveChangesAsync();

                Console.WriteLine($"NOTIFICATION => {evt.Title} for Loan {evt.LoanId}");
            }
        }
    }
}
