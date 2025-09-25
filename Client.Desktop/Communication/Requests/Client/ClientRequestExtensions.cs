using System.Collections.Generic;
using System.Linq;
using Client.Desktop.Communication.Notifications;
using Client.Desktop.Communication.Requests.Client.Records;
using Google.Protobuf.WellKnownTypes;
using Proto.DTO.Client;
using Proto.DTO.TraceData;
using Proto.Requests.Client;

namespace Client.Desktop.Communication.Requests.Client;

public static class ClientRequestExtensions
{
    public static GetTrackingControlDataRequestProto ToProto(this ClientGetTrackingControlDataRequest request)
    {
        return new GetTrackingControlDataRequestProto
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