using System.Text.Json;
using Core.Server.Communication.Records.Commands.Entities.WorkDays;
using Domain.Events.WorkDays;

namespace Core.Server.Translators.Commands.WorkDays;

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