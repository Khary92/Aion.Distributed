using System;
using System.Threading.Tasks;
using Client.Desktop.DTO;
using Grpc.Net.Client;
using Proto.Client;
using Proto.Requests.Settings;

namespace Client.Desktop.Communication.Requests.Settings;

public class SettingsRequestSender : ISettingsRequestSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.ServerAddress);
    private readonly SettingsRequestService.SettingsRequestServiceClient _client = new(Channel);

    public async Task<SettingsDto> Send(GetSettingsRequestProto request)
    {
        var response = await _client.GetSettingsAsync(request);
        return new SettingsDto(Guid.Parse(response.SettingsId), response.ExportPath,
            response.IsAddNewTicketsToCurrentSprintActive);
    }

    public async Task<bool> Send(IsExportPathValidRequestProto request)
    {
        var response = await _client.IsExportPathValidAsync(request);
        return response.IsValid;
    }

    public async Task<bool> Send(SettingsExistsRequestProto request)
    {
        var response = await _client.SettingsExistsAsync(request);
        return response.Exists;
    }
}