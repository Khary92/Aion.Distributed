using System.Threading.Tasks;
using Client.Avalonia.Communication.Commands;
using Grpc.Net.Client;
using Proto.Requests.WorkDays;

namespace Client.Avalonia.Communication.Requests.WorkDays;

public class WorkDayRequestSender : IWorkDayRequestSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.Address);
    private readonly WorkDayRequestService.WorkDayRequestServiceClient _client = new(Channel);

    public async Task<WorkDayListProto> GetAllWorkDays()
    {
        var request = new GetAllWorkDaysRequestProto();
        var response = await _client.GetAllWorkDaysAsync(request);
        return response;
    }

    public async Task<WorkDayProto> GetSelectedWorkDay()
    {
        var request = new GetSelectedWorkDayRequestProto();
        var response = await _client.GetSelectedWorkDayAsync(request);
        return response;
    }

    public async Task<WorkDayProto> GetWorkDayByDate(Google.Protobuf.WellKnownTypes.Timestamp date)
    {
        var request = new GetWorkDayByDateRequestProto { Date = date };
        var response = await _client.GetWorkDayByDateAsync(request);
        return response;
    }
}