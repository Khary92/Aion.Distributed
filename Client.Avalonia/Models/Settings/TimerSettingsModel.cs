using System;
using System.Threading.Tasks;
using Client.Avalonia.Communication.Commands;
using Client.Avalonia.Communication.Notifications.TimerSettings;
using Client.Avalonia.Communication.Requests;
using CommunityToolkit.Mvvm.Messaging;
using Contract.DTO;
using Contract.Tracing;
using Contract.Tracing.Tracers;
using Proto.Command.TimerSettings;
using Proto.Notifications.TimerSettings;
using ReactiveUI;

namespace Client.Avalonia.Models.Settings;

public class TimerSettingsModel(
    ICommandSender commandSender,
    IRequestSender requestSender,
    IMessenger messenger) : ReactiveObject
{
    private TimerSettingsDto? _timerSettingsDto;

    public TimerSettingsDto? TimerSettingsDto
    {
        get => _timerSettingsDto;
        private set => this.RaiseAndSetIfChanged(ref _timerSettingsDto, value);
    }

    public async Task Initialize()
    {
        if (await requestSender.IsTimerSettingExisting())
        {
            _timerSettingsDto = await requestSender.GetTimerSettings();
            return;
        }

        var crateTimerSettingsCommand = new CreateTimerSettingsCommand
        {
            TimerSettingsId = Guid.NewGuid().ToString(),
            DocumentationSaveInterval = 30,
            SnapshotSaveInterval = 30
        };

        await commandSender.Send(crateTimerSettingsCommand);
    }

    public void RegisterMessenger()
    {
        messenger.Register<NewTimerSettingsMessage>(this, (_, m) =>
        {
            //tracer.TimerSettings.Create.AggregateReceived(GetType(), m.TimerSettingsDto.TimerSettingsId,
            //    m.TimerSettingsDto.AsTraceAttributes());

            TimerSettingsDto = m.TimerSettingsDto;

            //tracer.TimerSettings.Create.AggregateAdded(GetType(), m.TimerSettingsDto.TimerSettingsId);
        });

        messenger.Register<DocuTimerSaveIntervalChangedNotification>(this, (_, m) =>
        {
            var parsedId = Guid.Parse(m.TimerSettingsId);
            //tracer.TimerSettings.ChangeDocuTimerInterval.NotificationReceived(GetType(), parsedId, m);

            TimerSettingsDto?.Apply(m);

            //tracer.TimerSettings.ChangeDocuTimerInterval.ChangesApplied(GetType(), parsedId);
        });

        messenger.Register<SnapshotSaveIntervalChangedNotification>(this, (_, m) =>
        {
            var parsedId = Guid.Parse(m.TimerSettingsId);
            //tracer.TimerSettings.ChangeSnapshotInterval.NotificationReceived(GetType(), parsedId, m);

            TimerSettingsDto?.Apply(m);

            //tracer.TimerSettings.ChangeSnapshotInterval.ChangesApplied(GetType(), parsedId);
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
            //tracer.TimerSettings.ChangeDocuTimerInterval.PropertyNotChanged(GetType(), TimerSettingsDto.TimerSettingsId,
                //TimerSettingsDto.AsTraceAttributes());
            return;
        }

        var changeDocuTimerSaveIntervalCommand = new ChangeDocuTimerSaveIntervalCommand
        {
            TimerSettingsId = TimerSettingsDto.TimerSettingsId.ToString(),
            DocuTimerSaveInterval = TimerSettingsDto.DocumentationSaveInterval
        };

        await commandSender.Send(changeDocuTimerSaveIntervalCommand);

        //tracer.TimerSettings.ChangeDocuTimerInterval.CommandSent(GetType(), TimerSettingsDto.TimerSettingsId,
            //changeDocuTimerSaveIntervalCommand);
    }

    private async Task CheckSnapShotIntervalChanged()
    {
        if (!TimerSettingsDto!.IsSnapshotIntervalChanged())
        {
            //tracer.TimerSettings.ChangeSnapshotInterval.PropertyNotChanged(GetType(), TimerSettingsDto.TimerSettingsId,
                //TimerSettingsDto.AsTraceAttributes());
            return;
        }

        var changeSnapshotSaveIntervalCommand = new ChangeSnapshotSaveIntervalCommand
        {
            TimerSettingsId = TimerSettingsDto.TimerSettingsId.ToString(),
            SnapshotSaveInterval = TimerSettingsDto.SnapshotSaveInterval
        };

        await commandSender.Send(changeSnapshotSaveIntervalCommand);

        //tracer.TimerSettings.ChangeSnapshotInterval.CommandSent(GetType(), TimerSettingsDto.TimerSettingsId,
         //   changeSnapshotSaveIntervalCommand);
    }
}