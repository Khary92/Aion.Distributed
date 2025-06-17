using System.Timers;
using Application.Contract.DTO;
using Application.Contract.Notifications.Entities.TimerSettings;
using Application.Contract.Notifications.UseCase;
using Application.Services.Entities.TimerSettings;
using MediatR;
using Timer = System.Timers.Timer;

namespace Application.Services.UseCase;

public class TimerService :
    INotificationHandler<TimerSettingsCreatedNotification>,
    INotificationHandler<DocuTimerSaveIntervalChangedNotification>,
    INotificationHandler<SnapshotSaveIntervalChangedNotification>
{
    private readonly IMediator _mediator;
    private readonly Timer _timer;
    private readonly ITimerSettingsRequestsService _timerSettingsRequestsService;
    private int _docuSeconds;
    private int _snapshotSeconds;

    private TimerSettingsDto? _timerSettings;

    public TimerService(IMediator mediator, ITimerSettingsRequestsService timerSettingsRequestsService)
    {
        _mediator = mediator;
        _timerSettingsRequestsService = timerSettingsRequestsService;

        _timer = new Timer(1000);
        _timer.Elapsed += OnTick;
        _timer.AutoReset = true;

        _ = InitializeAndStart();
    }

    public Task Handle(DocuTimerSaveIntervalChangedNotification notification, CancellationToken cancellationToken)
    {
        _timerSettings!.Apply(notification);
        return Task.CompletedTask;
    }

    public Task Handle(SnapshotSaveIntervalChangedNotification notification, CancellationToken cancellationToken)
    {
        _timerSettings!.Apply(notification);
        return Task.CompletedTask;
    }

    public Task Handle(TimerSettingsCreatedNotification notification, CancellationToken cancellationToken)
    {
        _timerSettings = new TimerSettingsDto(notification.TimerSettingsId, notification.DocumentationSaveInterval,
            notification.SnapshotSaveInterval);
        return Task.CompletedTask;
    }

    private async Task InitializeAndStart()
    {
        _timerSettings = await _timerSettingsRequestsService.Get();
        _timer.Start();
    }

    private void OnTick(object? sender, ElapsedEventArgs e)
    {
        if (_timerSettings == null) return;

        _snapshotSeconds++;
        _docuSeconds++;

        if (_snapshotSeconds >= _timerSettings.SnapshotSaveInterval)
        {
            _ = _mediator.Publish(new CreateSnapshotNotification());
            _snapshotSeconds = 0;
        }

        if (_docuSeconds >= _timerSettings.DocumentationSaveInterval)
        {
            _ = _mediator.Publish(new SaveDocumentationNotification());
            _docuSeconds = 0;
        }
    }
}