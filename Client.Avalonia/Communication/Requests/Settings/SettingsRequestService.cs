using System;
using System.Threading.Tasks;
using Contract.DTO;
using Grpc.Net.Client;
using Proto.Requests.Settings;
using Proto.Shared;

namespace Client.Avalonia.Communication.Requests.Settings;

public class SettingsRequestSender : ISettingsRequestSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.Address);
    private readonly SettingsRequestService.SettingsRequestServiceClient _client = new(Channel);

    public async Task<SettingsDto> GetSettings()
    {
        var request = new GetSettingsRequestProto();
        var response = await _client.GetSettingsAsync(request);
        return new SettingsDto(Guid.Parse(response.SettingsId), response.ExportPath,
            response.IsAddNewTicketsToCurrentSprintActive);
    }

    public async Task<bool> IsExportPathValid()
    {
        var request = new IsExportPathValidRequestProto();
        var response = await _client.IsExportPathValidAsync(request);
        return response.IsValid;
    }
}