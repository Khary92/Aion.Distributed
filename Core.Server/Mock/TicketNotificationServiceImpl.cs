using Grpc.Core;
using Proto.Notifications.Ticket;

public class TicketNotificationServiceImpl : TicketNotificationService.TicketNotificationServiceBase
{
    public override async Task SubscribeTicketNotifications(SubscribeRequest request,
        IServerStreamWriter<TicketNotification> responseStream,
        ServerCallContext context)
    {
        while (!context.CancellationToken.IsCancellationRequested)
        {
            var notification = new TicketNotification
            {
                TicketCreated = new TicketCreatedNotification
                {
                    TicketId = Guid.NewGuid().ToString(),
                    Name = "Test Ticket",
                    BookingNumber = "BN-123",
                    SprintIds = { Guid.NewGuid().ToString() }
                }
            };

            await responseStream.WriteAsync(notification);
            await Task.Delay(100);
        }
    }
}