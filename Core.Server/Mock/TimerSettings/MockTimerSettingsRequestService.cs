using Grpc.Core;
using Proto.Requests.TimerSettings;

namespace Service.Server.Mock.TimerSettings;

public class MockTimerSettingsRequestService : TimerSettingsRequestService.TimerSettingsRequestServiceBase
{
    public override Task<TimerSettingsProto> GetTimerSettings(GetTimerSettingsRequestProto request,
        ServerCallContext context)
    {
        var response = new TimerSettingsProto
        {
            TimerSettingsId = Guid.NewGuid().ToString(),
            DocumentationSaveInterval = 300,
            SnapshotSaveInterval = 60
        };
        return Task.FromResult(response);
    }

    public override Task<TimerSettingExistsProto> IsTimerSettingExisting(IsTimerSettingExistingRequestProto request,
        ServerCallContext context)
    {
        var response = new TimerSettingExistsProto
        {
            Exists = true
        };
        return Task.FromResult(response);
    }
}