using Grpc.Net.Client;
using Proto.DTO.Sprint;
using Proto.Requests.Sprints;

namespace Service.Proto.Shared.Requests.Sprints;

public class SprintRequestSender : ISprintRequestSender
{
    private readonly SprintProtoRequestService.SprintProtoRequestServiceClient _client;
    
    public SprintRequestSender(string address)
    {
        var channel = GrpcChannel.ForAddress(address);
        _client = new(channel);
    }
    
    public async Task<SprintProto?> Send(GetActiveSprintRequestProto request)
    {
        var response = await _client.GetActiveSprintAsync(request);
        return response.SprintId == string.Empty ? null : response;
    }

    public async Task<SprintListProto> Send(GetAllSprintsRequestProto request)
    {
        return await _client.GetAllSprintsAsync(request);
    }
}