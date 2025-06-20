using Proto.Notifications.UseCase;

namespace Service.Server.Communication.Services.UseCase;

public static class UseCaseProtoExtensions
{
    public static UseCaseNotification ToNotification(Guid timeSlotId, Guid statisticsDatatId, Guid ticketId) =>
        new()
        {
            TimeSlotControlCreated = new TimeSlotControlCreatedNotification
            {
                TimeSlotId = timeSlotId.ToString(),
                StatisticsDataId = statisticsDatatId.ToString(),
                TicketId = ticketId.ToString()
            }
        };
}