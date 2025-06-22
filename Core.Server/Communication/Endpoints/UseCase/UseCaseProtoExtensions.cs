using Core.Server.Communication.Endpoints.StatisticsData;
using Core.Server.Communication.Endpoints.Ticket;
using Core.Server.Communication.Endpoints.TimeSlot;
using Core.Server.Communication.Records.Commands.UseCase.Commands;
using Proto.Notifications.UseCase;
using Proto.Requests.UseCase;

namespace Core.Server.Communication.Endpoints.UseCase;

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