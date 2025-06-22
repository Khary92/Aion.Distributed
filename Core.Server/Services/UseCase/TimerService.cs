using System.Timers;
using Core.Server.Services.Entities.TimerSettings;
using Domain.Entities;
using Proto.Notifications.UseCase;
using Timer = System.Timers.Timer;
using UseCaseNotificationService = Core.Server.Communication.Endpoints.UseCase.UseCaseNotificationService;

namespace Core.Server.Services.UseCase;

public class TimerService
{
    private readonly Timer _timer;
    private readonly ITimerSettingsRequestsService _timerSettingsRequestsService;
    private readonly UseCaseNotificationService _useCaseNotificationService;
    private int _docuSeconds;
    private int _snapshotSeconds;

    private TimerSettings? _timerSettings;

    public TimerService(ITimerSettingsRequestsService timerSettingsRequestsService,
        UseCaseNotificationService useCaseNotificationService)
    {
        _timerSettingsRequestsService = timerSettingsRequestsService;
        _useCaseNotificationService = useCaseNotificationService;

        _timer = new Timer(1000);
        _timer.Elapsed += OnTick;
        _timer.AutoReset = true;

        _ = InitializeAndStart();
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
            _ = _useCaseNotificationService.SendNotificationAsync(new UseCaseNotification
            {
                CreateSnapshot = new CreateSnapshotNotification()
            });
            _snapshotSeconds = 0;
        }

        if (_docuSeconds >= _timerSettings.DocumentationSaveInterval)
        {
            _ = _useCaseNotificationService.SendNotificationAsync(new UseCaseNotification
            {
                SaveDocumentation = new SaveDocumentationNotification()
            });

            _docuSeconds = 0;
        }
    }
}