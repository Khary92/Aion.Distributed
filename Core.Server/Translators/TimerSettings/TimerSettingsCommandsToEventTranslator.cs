using System.Text.Json;
using Domain.Events.TimerSettings;
using Service.Server.Communication.CQRS.Commands.Entities.TimerSettings;

namespace Service.Server.Translators.TimerSettings;

public class TimerSettingsCommandsToEventTranslator : ITimerSettingsCommandsToEventTranslator
{
    public TimerSettingsEvent ToEvent(ChangeDocuTimerSaveIntervalCommand command)
    {
        var domainEvent = new DocuIntervalChangedEvent(command.TimerSettingsId, command.DocuTimerSaveInterval);

        return CreateDatabaseEvent(nameof(DocuIntervalChangedEvent), command.TimerSettingsId,
            JsonSerializer.Serialize(domainEvent));
    }

    public TimerSettingsEvent ToEvent(ChangeSnapshotSaveIntervalCommand command)
    {
        var domainEvent = new SnapshotIntervalChangedEvent(command.TimerSettingsId, command.SnapshotSaveInterval);

        return CreateDatabaseEvent(nameof(SnapshotIntervalChangedEvent), command.TimerSettingsId,
            JsonSerializer.Serialize(domainEvent));
    }

    public TimerSettingsEvent ToEvent(CreateTimerSettingsCommand command)
    {
        var domainEvent = new CreateTimerSettingsCommand(command.TimerSettingsId, command.DocumentationSaveInterval,
            command.SnapshotSaveInterval);

        return CreateDatabaseEvent(nameof(TimerSettingsCreatedEvent), command.TimerSettingsId,
            JsonSerializer.Serialize(domainEvent));
    }

    private static TimerSettingsEvent CreateDatabaseEvent(string eventName, Guid entityId, string json)
    {
        return new TimerSettingsEvent(Guid.NewGuid(), DateTime.UtcNow, TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow),
            eventName, entityId, json);
    }
}