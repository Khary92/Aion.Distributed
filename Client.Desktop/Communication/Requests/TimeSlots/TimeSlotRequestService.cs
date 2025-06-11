using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contract.DTO;
using Grpc.Net.Client;
using Proto.Requests.TimeSlots;
using Proto.Shared;

namespace Client.Desktop.Communication.Requests.TimeSlots;

public class TimeSlotRequestSender : ITimeSlotRequestSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.Address);
    private readonly TimeSlotRequestService.TimeSlotRequestServiceClient _client = new(Channel);

    public async Task<TimeSlotDto> GetTimeSlotById(string timeSlotId)
    {
        var request = new GetTimeSlotByIdRequestProto { TimeSlotId = timeSlotId };
        var response = await _client.GetTimeSlotByIdAsync(request);
        return ToDto(response);
    }

    public async Task<List<TimeSlotDto>> GetTimeSlotsForWorkDayId(string workDayId)
    {
        var request = new GetTimeSlotsForWorkDayIdRequestProto { WorkDayId = workDayId };
        var response = await _client.GetTimeSlotsForWorkDayIdAsync(request);

        return response.TimeSlots.Select(ToDto).ToList();
    }

    private static TimeSlotDto ToDto(TimeSlotProto proto)
    {
        var noteIds = proto.NoteIds
            .Select(idStr => Guid.TryParse(idStr, out var guid) ? guid : Guid.Empty)
            .Where(guid => guid != Guid.Empty)
            .ToList();

        return new TimeSlotDto(
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