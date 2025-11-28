using System.Threading.Tasks;
using Client.Desktop.DataModels;
using Proto.Requests.TimerSettings;
using ITimerSettingsRequestSender = Client.Desktop.Communication.Requests.Timer.ITimerSettingsRequestSender;

namespace Client.Desktop.Communication.Mock.Requests;

public class MockTimerSettingsRequestSender(MockDataService mockDataService) : ITimerSettingsRequestSender
{
    public Task<TimerSettingsClientModel> Send(GetTimerSettingsRequestProto request)
    {
        var result = new TimerSettingsClientModel(mockDataService.TimerSettings.TimerSettingsId,
            mockDataService.TimerSettings.DocumentationSaveInterval,
            mockDataService.TimerSettings.SnapshotSaveInterval);

        return Task.FromResult(result);
    }

    public Task<bool> Send(IsTimerSettingExistingRequestProto request)
    {
        return Task.FromResult(mockDataService.IsTimerSettingsExisting);
    }
}