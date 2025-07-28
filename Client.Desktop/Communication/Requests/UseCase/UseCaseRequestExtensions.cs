using System.Collections.Generic;
using System.Linq;
using Client.Desktop.Communication.Notifications;
using Client.Desktop.Communication.Requests.UseCase.Records;
using Google.Protobuf.WellKnownTypes;
using Proto.Requests.UseCase;

namespace Client.Desktop.Communication.Requests.UseCase;

public static class UseCaseRequestExtensions
{
    public static GetTimeSlotControlDataRequestProto ToProto(this ClientGetTimeSlotControlDataRequest request) => new()
    {
        Date = Timestamp.FromDateTimeOffset(request.Date)
    };

    public static List<ClientGetTimeSlotControlResponse> ToResponseDataList(this TimeSlotControlDataListProto proto) =>
        proto.TimeSlotControlData.Select(ToResponseData).ToList();

    private static ClientGetTimeSlotControlResponse ToResponseData(this TimeSlotControlDataProto proto) => new(
        proto.StatisticsDataProto.ToClientModel(), proto.TicketProto.ToClientModel(),
        proto.TimeSlotProto.ToClientModel());
}