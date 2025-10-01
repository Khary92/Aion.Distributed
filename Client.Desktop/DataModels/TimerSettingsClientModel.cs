using System;
using Client.Desktop.Communication.Notifications.TimerSettings.Records;
using ReactiveUI;

namespace Client.Desktop.DataModels;

public class TimerSettingsClientModel : ReactiveObject
{
    private int _currentDocuSecondsCount;

    private int _currentSnapshotSecondsCount;
    private int _documentationSaveInterval;
    private int _snapshotSaveInterval;
    private Guid _timerSettingsId;

    public TimerSettingsClientModel(Guid timerSettingsId, int documentationSaveInterval, int snapshotSaveInterval)
    {
        TimerSettingsId = timerSettingsId;
        DocumentationSaveInterval = documentationSaveInterval;
        SnapshotSaveInterval = snapshotSaveInterval;
    }

    public Guid TimerSettingsId
    {
        get => _timerSettingsId;
        set => this.RaiseAndSetIfChanged(ref _timerSettingsId, value);
    }

    public int DocumentationSaveInterval
    {
        get => _documentationSaveInterval;
        set => this.RaiseAndSetIfChanged(ref _documentationSaveInterval, value);
    }

    public int SnapshotSaveInterval
    {
        get => _snapshotSaveInterval;
        set => this.RaiseAndSetIfChanged(ref _snapshotSaveInterval, value);
    }

    public bool IsDocuSaveIntervalReached()
    {
        _currentDocuSecondsCount++;

        if (_currentDocuSecondsCount < DocumentationSaveInterval) return false;

        _currentDocuSecondsCount = 0;
        return true;
    }

    public bool IsSnapshotIntervalReached()
    {
        _currentSnapshotSecondsCount++;

        if (_currentSnapshotSecondsCount < SnapshotSaveInterval) return false;

        _currentSnapshotSecondsCount = 0;
        return true;
    }

    public void Apply(ClientSnapshotSaveIntervalChangedNotification notification)
    {
        SnapshotSaveInterval = notification.SaveInterval;
    }


    public void Apply(ClientDocuTimerSaveIntervalChangedNotification notification)
    {
        DocumentationSaveInterval = notification.SaveInterval;
    }
}