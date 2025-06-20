using Google.Protobuf.WellKnownTypes;
using Proto.Command.UseCases;
using Proto.DTO.TimeSlots;
using Proto.Notifications.UseCase;
using Service.Server.CQRS.Commands.UseCase;
using Service.Server.CQRS.Commands.UseCase.Commands;

namespace Service.Server.Communication.UseCase;

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