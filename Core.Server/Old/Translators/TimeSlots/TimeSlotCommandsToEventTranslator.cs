using System.Text.Json;
using Domain.Events.TimeSlots;
using Service.Server.CQRS.Commands.Entities.TimeSlots;

namespace Service.Server.Old.Translators.TimeSlots;

public class TimeSlotCommandsToEventTranslator : ITimeSlotCommandsToEventTranslator
{
    public TimeSlotEvent ToEvent(CreateTimeSlotCommand createTimeSlotCommand)
    {
        var domainEvent = new TimeSlotCreatedEvent(createTimeSlotCommand.TimeSlotId,
            createTimeSlotCommand.SelectedTicketId, createTimeSlotCommand.WorkDayId, createTimeSlotCommand.StartTime,
            createTimeSlotCommand.EndTime, createTimeSlotCommand.IsTimerRunning, []);

        return CreateDatabaseEvent(nameof(TimeSlotCreatedEvent), createTimeSlotCommand.TimeSlotId,
            JsonSerializer.Serialize(domainEvent));
    }

    public TimeSlotEvent ToEvent(AddNoteCommand addNoteCommand)
    {
        var domainEvent = new NoteAddedEvent(addNoteCommand.TimeSlotId, addNoteCommand.NoteId);

        return CreateDatabaseEvent(nameof(NoteAddedEvent), addNoteCommand.TimeSlotId,
            JsonSerializer.Serialize(domainEvent));
    }

    public TimeSlotEvent ToEvent(SetStartTimeCommand setStartTimeCommand)
    {
        var domainEvent = new StartTimeSetEvent(setStartTimeCommand.TimeSlotId, setStartTimeCommand.Time);

        return CreateDatabaseEvent(nameof(StartTimeSetEvent), setStartTimeCommand.TimeSlotId,
            JsonSerializer.Serialize(domainEvent));
    }

    public TimeSlotEvent ToEvent(SetEndTimeCommand setEndTimeCommand)
    {
        var domainEvent = new EndTimeSetEvent(setEndTimeCommand.TimeSlotId, setEndTimeCommand.Time);

        return CreateDatabaseEvent(nameof(EndTimeSetEvent), setEndTimeCommand.TimeSlotId,
            JsonSerializer.Serialize(domainEvent));
    }

    private static TimeSlotEvent CreateDatabaseEvent(string eventName, Guid entityId, string json)
    {
        return new TimeSlotEvent(Guid.NewGuid(), DateTime.UtcNow, TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow),
            eventName,
            entityId, json);
    }
}