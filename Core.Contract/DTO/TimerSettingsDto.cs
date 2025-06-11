using Proto.Notifications.TimerSettings;
using ReactiveUI;

namespace Contract.DTO;

public class TimerSettingsDto : ReactiveObject
{
    private int _documentationSaveInterval;
    private int _previousDocumentationInterval;
    private int _previousSnapshotInterval;
    private int _snapshotSaveInterval;

    public TimerSettingsDto(Guid timerSettingsId, int documentationSaveInterval, int snapshotSaveInterval)
    {
        TimerSettingsId = timerSettingsId;
        _documentationSaveInterval = documentationSaveInterval;
        _snapshotSaveInterval = snapshotSaveInterval;

        _previousSnapshotInterval = _documentationSaveInterval;
        _previousDocumentationInterval = _snapshotSaveInterval;
    }

    public Guid TimerSettingsId { get; }

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

    public void Apply(DocuTimerSaveIntervalChangedNotification notification)
    {
        DocumentationSaveInterval = notification.DocuTimerSaveInterval;
    }

    public void Apply(SnapshotSaveIntervalChangedNotification notification)
    {
        SnapshotSaveInterval = notification.SnapshotSaveInterval;
    }

    public bool IsSnapshotIntervalChanged()
    {
        if (_snapshotSaveInterval == _previousSnapshotInterval) return false;

        _previousSnapshotInterval = _snapshotSaveInterval;
        return true;
    }

    public bool IsDocuIntervalChanged()
    {
        if (_documentationSaveInterval == _previousDocumentationInterval) return false;

        _previousDocumentationInterval = _documentationSaveInterval;
        return true;
    }
}