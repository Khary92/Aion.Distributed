using System.Threading.Tasks;
using Proto.DTO.TimerSettings;
using Proto.Requests.TimerSettings;
using Service.Proto.Shared.Requests.TimerSettings;

namespace Client.Desktop.Communication.Mock.Requests;

public class MockTimerSettingsRequestSender(MockDataService mockDataService) : ITimerSettingsRequestSender
{
    public Task<TimerSettingsProto> Send(GetTimerSettingsRequestProto request)
    {
        var result = new TimerSettingsProto()
        {
            TimerSettingsId = mockDataService.TimerSettings.TimerSettingsId.ToString(),
            DocumentationSaveInterval = mockDataService.TimerSettings.DocumentationSaveInterval,
            SnapshotSaveInterval = mockDataService.TimerSettings.SnapshotSaveInterval
        };
        
        return Task.FromResult(result);
    }

    public Task<bool> Send(IsTimerSettingExistingRequestProto request)
    {
        return Task.FromResult(mockDataService.IsTimerSettingsExisting);
    }
}