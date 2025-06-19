using System.Text.Json;
using Domain.Events.Settings;
using Service.Server.CQRS.Commands.Entities.Settings;

namespace Service.Server.Old.Translators.Settings;

public class SettingsCommandsToEventTranslator : ISettingsCommandsToEventTranslator
{
    public SettingsEvent ToEvent(CreateSettingsCommand command)
    {
        var domainEvent = new SettingsCreatedEvent(command.SettingsId, command.ExportPath,
            command.IsAddNewTicketsToCurrentSprintActive);

        return CreateDatabaseEvent(nameof(SettingsCreatedEvent), command.SettingsId,
            JsonSerializer.Serialize(domainEvent));
    }

    public SettingsEvent ToEvent(UpdateSettingsCommand command)
    {
        var domainEvent = new SettingsUpdatedEvent(command.SettingsId, command.ExportPath,
            command.IsAddNewTicketsToCurrentSprintActive);

        return CreateDatabaseEvent(nameof(SettingsUpdatedEvent), command.SettingsId,
            JsonSerializer.Serialize(domainEvent));
    }

    private static SettingsEvent CreateDatabaseEvent(string eventName, Guid entityId, string json)
    {
        return new SettingsEvent(Guid.NewGuid(), DateTime.UtcNow, TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow),
            eventName, entityId, json);
    }
}