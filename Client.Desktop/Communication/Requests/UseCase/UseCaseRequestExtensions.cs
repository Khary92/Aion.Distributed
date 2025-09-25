using System.Collections.Generic;
using System.Linq;
using Client.Desktop.Communication.Notifications;
using Client.Desktop.Communication.Requests.UseCase.Records;
using Google.Protobuf.WellKnownTypes;
using Proto.DTO.TraceData;
using Proto.Requests.UseCase;

namespace Client.Desktop.Communication.Requests.UseCase;

public static class UseCaseRequestExtensions
{
    public static GetTimeSlotControlDataRequestProto ToProto(this ClientGetTimeSlotControlDataRequest request)
    {
        return new GetTimeSlotControlDataRequestProto
        {
            Date = Timestamp.FromDateTimeOffset(request.Date),
            TraceData = new TraceDataProto
            {
                TraceId = request.TraceId.ToString()
            }
        };
    }


    public static List<ClientGetTrackingControlResponse> ToResponseDataList(this TrackingControlDataListProto proto)
    {
        return proto.TimeSlotControlData.Select(ToResponseData).ToList();
    }

    private static ClientGetTrackingControlResponse ToResponseData(this TrackingControlDataProto proto)
    {
        return new ClientGetTrackingControlResponse(
            proto.StatisticsDataProto.ToClientModel(), proto.TicketProto.ToClientModel(),
            proto.TimeSlotProto.ToClientModel());
    }
}