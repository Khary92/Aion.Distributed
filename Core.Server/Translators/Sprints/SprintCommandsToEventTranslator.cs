using System.Text.Json;
using Domain.Events.Sprints;
using Service.Server.Communication.CQRS.Commands.Entities.Sprints;

namespace Service.Server.Translators.Sprints;

public class SprintCommandsToEventTranslator : ISprintCommandsToEventTranslator
{
    public SprintEvent ToEvent(CreateSprintCommand createSprintCommand)
    {
        var domainEvent = new SprintCreatedEvent(createSprintCommand.SprintId, createSprintCommand.Name,
            createSprintCommand.StartTime, createSprintCommand.EndTime, createSprintCommand.IsActive,
            createSprintCommand.TicketIds);

        return CreateDatabaseEvent(nameof(SprintCreatedEvent), createSprintCommand.SprintId,
            JsonSerializer.Serialize(domainEvent));
    }

    public SprintEvent ToEvent(UpdateSprintDataCommand updateSprintDataCommand)
    {
        var domainEvent = new SprintDataUpdatedEvent(updateSprintDataCommand.SprintId, updateSprintDataCommand.Name,
            updateSprintDataCommand.StartTime, updateSprintDataCommand.EndTime);

        return CreateDatabaseEvent(nameof(SprintDataUpdatedEvent), updateSprintDataCommand.SprintId,
            JsonSerializer.Serialize(domainEvent));
    }

    public SprintEvent ToEvent(SetSprintActiveStatusCommand setSprintActiveStatusCommand)
    {
        var domainEvent = new SprintActiveStatusChangedEvent(setSprintActiveStatusCommand.SprintId,
            setSprintActiveStatusCommand.IsActive);

        return CreateDatabaseEvent(nameof(SprintActiveStatusChangedEvent), setSprintActiveStatusCommand.SprintId,
            JsonSerializer.Serialize(domainEvent));
    }

    public SprintEvent ToEvent(AddTicketToSprintCommand addTicketToSprintCommand)
    {
        var domainEvent = new TicketAddedToSprintEvent(addTicketToSprintCommand.SprintId,
            addTicketToSprintCommand.TicketId);

        return CreateDatabaseEvent(nameof(TicketAddedToSprintEvent), addTicketToSprintCommand.SprintId,
            JsonSerializer.Serialize(domainEvent));
    }

    private static SprintEvent CreateDatabaseEvent(string eventName, Guid entityId, string json)
    {
        return new SprintEvent(Guid.NewGuid(), DateTime.UtcNow, TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow),
            eventName,
            entityId, json);
    }
}