using System.Threading.Tasks;
using Client.Avalonia.Communication.Commands;
using Grpc.Net.Client;
using Proto.Requests.Sprints;

namespace Client.Avalonia.Communication.Requests;

public class SprintRequestSender : ISprintRequestSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempStatic.Address);
    private readonly SprintRequestService.SprintRequestServiceClient _client = new(Channel);

    public async Task<SprintProto> GetActiveSprint()
    {
        var request = new GetActiveSprintRequestProto();
        var response = await _client.GetActiveSprintAsync(request);
        return response;
    }

    public async Task<SprintListProto> GetAllSprints()
    {
        var request = new GetAllSprintsRequestProto();
        var response = await _client.GetAllSprintsAsync(request);
        return response;
    }
}