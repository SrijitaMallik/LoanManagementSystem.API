using System.Threading.Channels;

namespace LoanManagementSystem.API.Models
{
    public static class LoanNotificationQueue
    {
        public static Channel<LoanNotificationEvent> Channel =
            System.Threading.Channels.Channel.CreateUnbounded<LoanNotificationEvent>();
    }
}
