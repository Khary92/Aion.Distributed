using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.Desktop.DataModels;
using Client.Proto;
using Grpc.Net.Client;
using Proto.DTO.TimeSlots;
using Proto.Requests.TimeSlots;

namespace Client.Desktop.Communication.Requests.TimeSlots;

public class TimeSlotRequestSender : ITimeSlotRequestSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.ServerAddress);
    private readonly TimeSlotRequestService.TimeSlotRequestServiceClient _client = new(Channel);

    public async Task<TimeSlotClientModel> Send(GetTimeSlotByIdRequestProto request)
    {
        var response = await _client.GetTimeSlotByIdAsync(request);
        return ToDto(response);
    }

    public async Task<List<TimeSlotClientModel>> Send(GetTimeSlotsForWorkDayIdRequestProto request)
    {
        var response = await _client.GetTimeSlotsForWorkDayIdAsync(request);

        return response.TimeSlots.Select(ToDto).ToList();
    }

    private static TimeSlotClientModel ToDto(TimeSlotProto proto)
    {
        var noteIds = proto.NoteIds
            .Select(idStr => Guid.TryParse(idStr, out var guid) ? guid : Guid.Empty)
            .Where(guid => guid != Guid.Empty)
            .ToList();

        return new TimeSlotClientModel(
            Guid.Parse(proto.TimeSlotId),
            Guid.Parse(proto.WorkDayId),
            Guid.Parse(proto.SelectedTicketId),
            proto.StartTime.ToDateTimeOffset(),
            proto.EndTime.ToDateTimeOffset(),
            noteIds,
            proto.IsTimerRunning
        );
    }
}