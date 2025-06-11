using System.Threading.Tasks;
using Contract.DTO;
using Grpc.Net.Client;
using Proto.Requests.AiSettings;
using Proto.Shared;

namespace Client.Desktop.Communication.Requests.AiSettings;

public class AiSettingsRequestSender : IAiSettingsRequestSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.Address);
    private readonly AiSettingsRequestService.AiSettingsRequestServiceClient _client = new(Channel);

    public async Task<AiSettingsDto?> Get(string aiSettingsId)
    {
        var request = new GetAiSettingsRequestProto { AiSettingsId = aiSettingsId };
        var response = await _client.GetAiSettingsAsync(request);
        return response.ToDto();
    }
}