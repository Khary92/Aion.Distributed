using System.Threading.Tasks;
using Client.Avalonia.Communication.Commands;
using Grpc.Net.Client;
using Proto.Requests.AiSettings;

namespace Client.Avalonia.Communication.Requests.AiSettings;

public class AiSettingsRequestSender : IAiSettingsRequestSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.Address);
    private readonly AiSettingsRequestService.AiSettingsRequestServiceClient _client = new(Channel);

    public async Task<AiSettingsProto?> Get(string aiSettingsId)
    {
        var request = new GetAiSettingsRequestProto { AiSettingsId = aiSettingsId };
        var response = await _client.GetAiSettingsAsync(request);
        return response;
    }
}