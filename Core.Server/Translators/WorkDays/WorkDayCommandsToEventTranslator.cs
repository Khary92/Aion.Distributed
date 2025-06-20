using System.Text.Json;
using Domain.Events.WorkDays;
using Service.Server.Communication.CQRS.Commands.Entities.WorkDays;

namespace Service.Server.Translators.WorkDays;

public class WorkDayCommandsToEventTranslator : IWorkDayCommandsToEventTranslator
{
    public WorkDayEvent ToEvent(CreateWorkDayCommand createWorkDayCommand)
    {
        var domainEvent = new WorkDayCreatedEvent(createWorkDayCommand.WorkDayId, createWorkDayCommand.Date);
        return CreateDatabaseEvent(nameof(WorkDayCreatedEvent), createWorkDayCommand.WorkDayId,
            JsonSerializer.Serialize(domainEvent));
    }

    private static WorkDayEvent CreateDatabaseEvent(string eventName, Guid entityId, string json)
    {
        return new WorkDayEvent(Guid.NewGuid(), DateTime.UtcNow, TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow),
            eventName,
            entityId, json);
    }
}