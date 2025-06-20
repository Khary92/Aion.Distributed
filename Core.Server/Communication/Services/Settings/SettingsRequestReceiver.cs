using Grpc.Core;
using Proto.DTO.Settings;
using Proto.Requests.Settings;
using Service.Server.Services.Entities.Settings;

namespace Service.Server.Communication.Services.Settings;

public class SettingsRequestReceiver(ISettingsRequestsService settingsRequestsService)
    : SettingsRequestService.SettingsRequestServiceBase
{
    public override async Task<SettingsProto> GetSettings(GetSettingsRequestProto request, ServerCallContext context)
    {
        var settings = await settingsRequestsService.Get();
        return settings.ToProto();
    }

    public override async Task<IsExportPathValidResponseProto> IsExportPathValid(IsExportPathValidRequestProto request,
        ServerCallContext context)
    {
        return new IsExportPathValidResponseProto
        {
            IsValid = await settingsRequestsService.IsSettingsExisting()
        };
    }

    public override Task<SettingsExistsResponseProto> SettingsExists(SettingsExistsRequestProto request,
        ServerCallContext context)
    {
        var response = new SettingsExistsResponseProto
        {
            Exists = true
        };

        return Task.FromResult(response);
    }
}