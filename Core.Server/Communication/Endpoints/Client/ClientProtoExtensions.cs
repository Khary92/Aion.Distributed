using Core.Server.Communication.Endpoints.StatisticsData;
using Core.Server.Communication.Endpoints.Ticket;
using Core.Server.Communication.Endpoints.TimeSlot;
using Core.Server.Communication.Records.Commands.UseCase.Commands;
using Proto.DTO.Client;
using Proto.DTO.TraceData;
using Proto.Notifications.Client;
using Proto.Requests.Client;

namespace Core.Server.Communication.Endpoints.Client;

public static class ClientProtoExtensions
{
    public static ClientNotification ToNotification(Domain.Entities.TimeSlot timeSlot,
        Domain.Entities.StatisticsData statisticsData, Domain.Entities.Ticket ticket, Guid traceId)
    {
        return new ClientNotification
        {
            TimeSlotControlCreated = new TrackingControlCreatedNotification
            {
                TimeSlotControlData = new TrackingControlDataProto
                {
                    TicketProto = ticket.ToProto(),
                    StatisticsDataProto = statisticsData.ToProto(),
                    TimeSlotProto = timeSlot.ToProto()
                },
                TraceData = new TraceDataProto
                {
                    TraceId = traceId.ToString()
                }
            }
        };
    }

    public static GetTrackingControlDataForDateRequest ToRequest(this GetTrackingControlDataRequestProto proto)
    {
        return new GetTrackingControlDataForDateRequest(proto.Date.ToDateTimeOffset(),
            Guid.Parse(proto.TraceData.TraceId));
    }
}