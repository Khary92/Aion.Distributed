using System;
using System.Threading.Tasks;
using Client.Avalonia.Communication.Notifications.TimerSettings;
using CommunityToolkit.Mvvm.Messaging;
using Contract.CQRS.Notifications.Entities.TimerSettings;
using Contract.DTO;
using Contract.Tracing;
using Contract.Tracing.Tracers;
using MediatR;
using ReactiveUI;

namespace Client.Avalonia.ViewModels.Settings;

public class TimerSettingsModel(
    IMediator mediator,
    IMessenger messenger,
    ITracingCollectorProvider tracer) : ReactiveObject
{
    private TimerSettingsDto? _timerSettingsDto;

    public TimerSettingsDto? TimerSettingsDto
    {
        get => _timerSettingsDto;
        private set => this.RaiseAndSetIfChanged(ref _timerSettingsDto, value);
    }

    public async Task Initialize()
    {
        if (await mediator.Send(new IsTimerSettingExistingRequest()))
        {
            _timerSettingsDto = await mediator.Send(new GetTimerSettingsRequest());
            return;
        }

        await mediator.Send(new CreateTimerSettingsCommand(Guid.NewGuid(), 30, 30));
    }

    public void RegisterMessenger()
    {
        messenger.Register<NewTimerSettingsMessage>(this, (_, m) =>
        {
            tracer.TimerSettings.Create.AggregateReceived(GetType(), m.TimerSettingsDto.TimerSettingsId,
                m.TimerSettingsDto.AsTraceAttributes());

            TimerSettingsDto = m.TimerSettingsDto;

            tracer.TimerSettings.Create.AggregateAdded(GetType(), m.TimerSettingsDto.TimerSettingsId);
        });

        messenger.Register<DocuTimerSaveIntervalChangedNotification>(this, (_, m) =>
        {
            tracer.TimerSettings.ChangeDocuTimerInterval.NotificationReceived(GetType(), m.TimerSettingsId, m);

            TimerSettingsDto?.Apply(m);

            tracer.TimerSettings.ChangeDocuTimerInterval.ChangesApplied(GetType(), m.TimerSettingsId);
        });

        messenger.Register<SnapshotSaveIntervalChangedNotification>(this, (_, m) =>
        {
            tracer.TimerSettings.ChangeSnapshotInterval.NotificationReceived(GetType(), m.TimerSettingsId, m);

            TimerSettingsDto?.Apply(m);

            tracer.TimerSettings.ChangeSnapshotInterval.ChangesApplied(GetType(), m.TimerSettingsId);
        });
    }

    public async Task Save()
    {
        if (TimerSettingsDto == null) return;

        await CheckSnapShotIntervalChanged();
        await CheckDocuIntervalChanged();
    }

    private async Task CheckDocuIntervalChanged()
    {
        if (!TimerSettingsDto!.IsDocuIntervalChanged())
        {
            tracer.TimerSettings.ChangeDocuTimerInterval.PropertyNotChanged(GetType(), TimerSettingsDto.TimerSettingsId,
                TimerSettingsDto.AsTraceAttributes());
            return;
        }

        var changeDocuTimerSaveIntervalCommand = new ChangeDocuTimerSaveIntervalCommand(
            TimerSettingsDto.TimerSettingsId,
            TimerSettingsDto.DocumentationSaveInterval);
        await mediator.Send(changeDocuTimerSaveIntervalCommand);

        tracer.TimerSettings.ChangeDocuTimerInterval.CommandSent(GetType(), TimerSettingsDto.TimerSettingsId,
            changeDocuTimerSaveIntervalCommand);
    }

    private async Task CheckSnapShotIntervalChanged()
    {
        if (!TimerSettingsDto!.IsSnapshotIntervalChanged())
        {
            tracer.TimerSettings.ChangeSnapshotInterval.PropertyNotChanged(GetType(), TimerSettingsDto.TimerSettingsId,
                TimerSettingsDto.AsTraceAttributes());
            return;
        }

        var changeSnapshotSaveIntervalCommand = new ChangeSnapshotSaveIntervalCommand(TimerSettingsDto.TimerSettingsId,
            TimerSettingsDto.SnapshotSaveInterval);
        await mediator.Send(changeSnapshotSaveIntervalCommand);

        tracer.TimerSettings.ChangeSnapshotInterval.CommandSent(GetType(), TimerSettingsDto.TimerSettingsId,
            changeSnapshotSaveIntervalCommand);
    }
}