using Proto.Command.TimerSettings;
using Service.Admin.Web.Communication.TimerSettings.State;
using Service.Admin.Web.Services;

namespace Service.Admin.Web.Communication.TimerSettings;

public class TimerSettingsController(
    ITimerSettingsStateService timerSettingsStateService,
    ISharedCommandSender commandSender) : IInitializeAsync, ITimerSettingsController
{
    private int _previousSnapshotSaveInterval;
    private int _previousDocumentationSaveInterval;

    public async Task SaveSettingsAsync()
    {
        if (_previousSnapshotSaveInterval != timerSettingsStateService.TimerSettings.SnapshotSaveInterval)
        {
            await commandSender.Send(new ChangeSnapshotSaveIntervalCommandProto
            {
                TimerSettingsId = timerSettingsStateService.TimerSettings.TimerSettingsId.ToString(),
                SnapshotSaveInterval = timerSettingsStateService.TimerSettings.SnapshotSaveInterval
            });

            _previousSnapshotSaveInterval = timerSettingsStateService.TimerSettings.SnapshotSaveInterval;
        }

        if (_previousDocumentationSaveInterval != timerSettingsStateService.TimerSettings.DocumentationSaveInterval)
        {
            await commandSender.Send(new ChangeDocuTimerSaveIntervalCommandProto
            {
                TimerSettingsId = timerSettingsStateService.TimerSettings.TimerSettingsId.ToString(),
                DocuTimerSaveInterval = timerSettingsStateService.TimerSettings.DocumentationSaveInterval
            });

            _previousDocumentationSaveInterval = timerSettingsStateService.TimerSettings.DocumentationSaveInterval;
        }
    }

    public InitializationType Type => InitializationType.Controller;
    
    public Task InitializeComponents()
    {
        _previousSnapshotSaveInterval = timerSettingsStateService.TimerSettings.SnapshotSaveInterval;
        _previousDocumentationSaveInterval = timerSettingsStateService.TimerSettings.DocumentationSaveInterval;
        return Task.CompletedTask;
    }
}