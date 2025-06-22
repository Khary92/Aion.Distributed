using Core.Server.Communication.CQRS.Commands.UseCase.Commands;
using Core.Server.Communication.Services.StatisticsData;
using Core.Server.Communication.Services.Ticket;
using Core.Server.Communication.Services.TimeSlot;
using Proto.Notifications.UseCase;
using Proto.Requests.UseCase;

namespace Core.Server.Communication.Services.UseCase;

public static class UseCaseProtoExtensions
{
    public static UseCaseNotification ToNotification(Domain.Entities.TimeSlot timeSlot,
        Domain.Entities.StatisticsData statisticsData, Domain.Entities.Ticket ticket)
    {
        return new UseCaseNotification
        {
            TimeSlotControlCreated = new TimeSlotControlCreatedNotification
            {
                TimeSlotControlData = new TimeSlotControlDataProto
                {
                    TicketProto = ticket.ToProto(),
                    StatisticsDataProto = statisticsData.ToProto(),
                    TimeSlotProto = timeSlot.ToProto()
                }
            }
        };
    }

    public static GetTimeSlotControlDataForDateRequest ToRequest(
        this GetTimeSlotControlDataRequestProto proto)
    {
        return new GetTimeSlotControlDataForDateRequest(proto.Date.ToDateTimeOffset());
    }
}