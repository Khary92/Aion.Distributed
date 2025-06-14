using Grpc.Core;
using Proto.Requests.Settings;

public class SettingsRequestServiceImpl : SettingsRequestService.SettingsRequestServiceBase
{
    public override Task<SettingsProto> GetSettings(GetSettingsRequestProto request, ServerCallContext context)
    {
        var settings = new SettingsProto
        {
            SettingsId = "settings-001",
            ExportPath = "/var/export",
            IsAddNewTicketsToCurrentSprintActive = true
        };

        return Task.FromResult(settings);
    }

    public override Task<IsExportPathValidResponseProto> IsExportPathValid(IsExportPathValidRequestProto request,
        ServerCallContext context)
    {
        var response = new IsExportPathValidResponseProto
        {
            IsValid = true
        };

        return Task.FromResult(response);
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