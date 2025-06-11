using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contract.DTO;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Proto.Requests.WorkDays;
using Proto.Shared;

namespace Client.Avalonia.Communication.Requests.WorkDays;

public class WorkDayRequestSender : IWorkDayRequestSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.Address);
    private readonly WorkDayRequestService.WorkDayRequestServiceClient _client = new(Channel);

    public async Task<List<WorkDayDto>> GetAllWorkDays()
    {
        var request = new GetAllWorkDaysRequestProto();
        var response = await _client.GetAllWorkDaysAsync(request);
        return response.WorkDays.Select(w => ToDto(w)).ToList();
    }

    public async Task<WorkDayDto> GetSelectedWorkDay()
    {
        var request = new GetSelectedWorkDayRequestProto();
        var response = await _client.GetSelectedWorkDayAsync(request);
        return ToDto(response);
    }

    public async Task<WorkDayDto> GetWorkDayByDate(Timestamp date)
    {
        var request = new GetWorkDayByDateRequestProto { Date = date };
        var response = await _client.GetWorkDayByDateAsync(request);
        return ToDto(response);
    }

    private static WorkDayDto ToDto(WorkDayProto proto)
    {
        return new WorkDayDto(Guid.Parse(proto.WorkDayId), proto.Date.ToDateTimeOffset());
    }
}