using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.Desktop.DTO;
using Client.Desktop.Proto;
using Grpc.Net.Client;
using Proto.DTO.TimerSettings;
using Proto.Requests.WorkDays;

namespace Client.Desktop.Communication.Requests.WorkDays;

public class WorkDayRequestSender : IWorkDayRequestSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.ServerAddress);
    private readonly WorkDayRequestService.WorkDayRequestServiceClient _client = new(Channel);

    public async Task<List<WorkDayDto>> Send(GetAllWorkDaysRequestProto request)
    {
        var response = await _client.GetAllWorkDaysAsync(request);
        return response.WorkDays.Select(ToDto).ToList();
    }

    public async Task<WorkDayDto> Send(GetSelectedWorkDayRequestProto request)
    {
        var response = await _client.GetSelectedWorkDayAsync(request);
        return ToDto(response);
    }

    public async Task<WorkDayDto> Send(GetWorkDayByDateRequestProto request)
    {
        var response = await _client.GetWorkDayByDateAsync(request);
        return ToDto(response);
    }

    public async Task<bool> Send(IsWorkDayExistingRequestProto request)
    {
        var response = await _client.IsWorkDayExistingAsync(request);
        return response.Exists;
    }

    private static WorkDayDto ToDto(WorkDayProto proto)
    {
        return new WorkDayDto(Guid.Parse(proto.WorkDayId), proto.Date.ToDateTimeOffset());
    }
}