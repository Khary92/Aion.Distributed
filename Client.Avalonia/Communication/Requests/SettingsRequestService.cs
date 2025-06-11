using System.Threading.Tasks;
using Client.Avalonia.Communication.Commands;
using Grpc.Net.Client;
using Proto.Requests.Settings;

namespace Client.Avalonia.Communication.Requests;

public class SettingsRequestSender : ISettingsRequestSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempStatic.Address);
    private readonly SettingsRequestService.SettingsRequestServiceClient _client = new(Channel);

    public async Task<SettingsProto> GetSettings()
    {
        var request = new GetSettingsRequestProto();
        var response = await _client.GetSettingsAsync(request);
        return response;
    }

    public async Task<bool> IsExportPathValid()
    {
        var request = new IsExportPathValidRequestProto();
        var response = await _client.IsExportPathValidAsync(request);
        return response.IsValid;
    }
}