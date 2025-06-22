using Core.Server.Services.Entities.Settings;
using Grpc.Core;
using Proto.DTO.Settings;
using Proto.Requests.Settings;

namespace Core.Server.Communication.Endpoints.Settings;

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

    public override async Task<SettingsExistsResponseProto> SettingsExists(SettingsExistsRequestProto request,
        ServerCallContext context)
    {
        return new SettingsExistsResponseProto
        {
            Exists = await settingsRequestsService.IsSettingsExisting()
        };
    }
}