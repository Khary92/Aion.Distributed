using System.Threading.Tasks;
using Client.Avalonia.Communication.Commands;
using Grpc.Net.Client;
using Proto.Requests.TimeSlots;

namespace Client.Avalonia.Communication.Requests.TimeSlots;

public class TimeSlotRequestSender : ITimeSlotRequestSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.Address);
    private readonly TimeSlotRequestService.TimeSlotRequestServiceClient _client = new(Channel);

    public async Task<TimeSlotProto> GetTimeSlotById(string timeSlotId)
    {
        var request = new GetTimeSlotByIdRequestProto { TimeSlotId = timeSlotId };
        var response = await _client.GetTimeSlotByIdAsync(request);
        return response;
    }

    public async Task<TimeSlotListProto> GetTimeSlotsForWorkDayId(string workDayId)
    {
        var request = new GetTimeSlotsForWorkDayIdRequestProto { WorkDayId = workDayId };
        var response = await _client.GetTimeSlotsForWorkDayIdAsync(request);
        return response;
    }
}