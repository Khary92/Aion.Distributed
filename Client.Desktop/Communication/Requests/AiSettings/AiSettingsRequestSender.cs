using System.Threading.Tasks;
using Client.Desktop.DTO;
using Client.Desktop.Proto;
using Grpc.Net.Client;
using Proto.Requests.AiSettings;

namespace Client.Desktop.Communication.Requests.AiSettings;

public class AiSettingsRequestSender : IAiSettingsRequestSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.ServerAddress);
    private readonly AiSettingsRequestService.AiSettingsRequestServiceClient _client = new(Channel);

    public async Task<AiSettingsDto> Send(GetAiSettingsRequestProto request)
    {
        var response = await _client.GetAiSettingsAsync(request);
        return response.ToDto();
    }

    public async Task<bool> Send(AiSettingExistsRequestProto request)
    {
        var response = await _client.AiSettingsExistsAsync(request);
        return response.Exists;
    }
}