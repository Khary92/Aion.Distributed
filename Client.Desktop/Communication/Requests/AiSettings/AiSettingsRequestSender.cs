using System.Threading.Tasks;
using Contract.DTO;
using Grpc.Net.Client;
using Proto.Requests.AiSettings;
using Proto.Shared;

namespace Client.Desktop.Communication.Requests.AiSettings;

public class AiSettingsRequestSender : IAiSettingsRequestSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.ServerAddress);
    private readonly AiSettingsRequestService.AiSettingsRequestServiceClient _client = new(Channel);

    public async Task<AiSettingsDto?> GetAiSettings()
    {
        var request = new GetAiSettingsRequestProto();
        var response = await _client.GetAiSettingsAsync(request);
        return response.ToDto();
    }

    public async Task<bool> IsAiSettingsExisting()
    {
        var request = new AiSettingExistsRequestProto();
        var response = await _client.AiSettingsExistsAsync(request);
        return response.Exists;
    }
}