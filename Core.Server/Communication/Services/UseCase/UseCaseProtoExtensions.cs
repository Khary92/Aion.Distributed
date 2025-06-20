using Proto.Notifications.UseCase;

namespace Core.Server.Communication.Services.UseCase;

public static class UseCaseProtoExtensions
{
    public static UseCaseNotification ToNotification(Guid timeSlotId, Guid statisticsDatatId, Guid ticketId)
    {
        return new UseCaseNotification
        {
            TimeSlotControlCreated = new TimeSlotControlCreatedNotification
            {
                TimeSlotId = timeSlotId.ToString(),
                StatisticsDataId = statisticsDatatId.ToString(),
                TicketId = ticketId.ToString()
            }
        };
    }
}