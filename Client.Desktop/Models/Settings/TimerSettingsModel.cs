using System;
using System.Threading.Tasks;
using Client.Desktop.Communication.Commands;
using Client.Desktop.Communication.NotificationWrappers;
using Client.Desktop.Communication.Requests;
using Client.Desktop.Converter;
using Client.Desktop.DTO;
using Client.Tracing.Tracing.Tracers;
using CommunityToolkit.Mvvm.Messaging;
using Proto.Command.TimerSettings;
using Proto.Notifications.TimerSettings;
using Proto.Requests.TimerSettings;
using ReactiveUI;

namespace Client.Desktop.Models.Settings;

public class TimerSettingsModel(
    ICommandSender commandSender,
    IRequestSender requestSender,
    IMessenger messenger,
    ITraceCollector tracer) : ReactiveObject
{
    private TimerSettingsDto? _timerSettingsDto;

    public TimerSettingsDto? TimerSettingsDto
    {
        get => _timerSettingsDto;
        private set => this.RaiseAndSetIfChanged(ref _timerSettingsDto, value);
    }

    public async Task Initialize()
    {
        if (await requestSender.Send(new IsTimerSettingExistingRequestProto()))
        {
            TimerSettingsDto = await requestSender.Send(new GetTimerSettingsRequestProto());
            return;
        }

        var crateTimerSettingsCommand = new CreateTimerSettingsCommandProto
        {
            TimerSettingsId = Guid.NewGuid().ToString(),
            DocumentationSaveInterval = 30,
            SnapshotSaveInterval = 30
        };

        await commandSender.Send(crateTimerSettingsCommand);
    }

    public void RegisterMessenger()
    {
        messenger.Register<NewTimerSettingsMessage>(this, async (_, m) =>
        {
            await tracer.TimerSettings.Create.AggregateReceived(GetType(), m.TimerSettingsDto.TimerSettingsId,
                m.TimerSettingsDto.AsTraceAttributes());

            TimerSettingsDto = m.TimerSettingsDto;

            await tracer.TimerSettings.Create.AggregateAdded(GetType(), m.TimerSettingsDto.TimerSettingsId);
        });

        messenger.Register<DocuTimerSaveIntervalChangedNotification>(this, async (_, m) =>
        {
            var parsedId = Guid.Parse(m.TimerSettingsId);
            await tracer.TimerSettings.ChangeDocuTimerInterval.NotificationReceived(GetType(), parsedId, m);

            TimerSettingsDto?.Apply(m);

            await tracer.TimerSettings.ChangeDocuTimerInterval.ChangesApplied(GetType(), parsedId);
        });

        messenger.Register<SnapshotSaveIntervalChangedNotification>(this, async (_, m) =>
        {
            var parsedId = Guid.Parse(m.TimerSettingsId);
            await tracer.TimerSettings.ChangeSnapshotInterval.NotificationReceived(GetType(), parsedId, m);

            TimerSettingsDto?.Apply(m);

            await tracer.TimerSettings.ChangeSnapshotInterval.ChangesApplied(GetType(), parsedId);
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
            await tracer.TimerSettings.ChangeDocuTimerInterval.PropertyNotChanged(GetType(),
                TimerSettingsDto.TimerSettingsId,
                TimerSettingsDto.AsTraceAttributes());
            return;
        }

        var changeDocuTimerSaveIntervalCommand = new ChangeDocuTimerSaveIntervalCommandProto
        {
            TimerSettingsId = TimerSettingsDto.TimerSettingsId.ToString(),
            DocuTimerSaveInterval = TimerSettingsDto.DocumentationSaveInterval
        };

        await commandSender.Send(changeDocuTimerSaveIntervalCommand);

        await tracer.TimerSettings.ChangeDocuTimerInterval.CommandSent(GetType(), TimerSettingsDto.TimerSettingsId,
            changeDocuTimerSaveIntervalCommand);
    }

    private async Task CheckSnapShotIntervalChanged()
    {
        if (!TimerSettingsDto!.IsSnapshotIntervalChanged())
        {
            await tracer.TimerSettings.ChangeSnapshotInterval.PropertyNotChanged(GetType(),
                TimerSettingsDto.TimerSettingsId,
                TimerSettingsDto.AsTraceAttributes());
            return;
        }

        var changeSnapshotSaveIntervalCommand = new ChangeSnapshotSaveIntervalCommandProto
        {
            TimerSettingsId = TimerSettingsDto.TimerSettingsId.ToString(),
            SnapshotSaveInterval = TimerSettingsDto.SnapshotSaveInterval
        };

        await commandSender.Send(changeSnapshotSaveIntervalCommand);

        await tracer.TimerSettings.ChangeSnapshotInterval.CommandSent(GetType(), TimerSettingsDto.TimerSettingsId,
            changeSnapshotSaveIntervalCommand);
    }
}