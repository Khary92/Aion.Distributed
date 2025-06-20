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

    public SettingsEvent ToEvent(ChangeExportPathCommand command)
    {
        throw new NotImplementedException();
    }

    public SettingsEvent ToEvent(ChangeAutomaticTicketAddingToSprintCommand command)
    {
        throw new NotImplementedException();
    }


    private static SettingsEvent CreateDatabaseEvent(string eventName, Guid entityId, string json)
    {
        return new SettingsEvent(Guid.NewGuid(), DateTime.UtcNow, TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow),
            eventName, entityId, json);
    }
}