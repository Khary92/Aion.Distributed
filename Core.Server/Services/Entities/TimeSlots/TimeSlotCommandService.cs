using Core.Server.Communication.Endpoints.TimeSlot;
using Core.Server.Communication.Records.Commands.Entities.TimeSlots;
using Core.Server.Tracing.Tracing.Tracers;
using Core.Server.Translators.Commands.TimeSlots;
using Domain.Events.TimeSlots;
using Domain.Interfaces;

namespace Core.Server.Services.Entities.TimeSlots;

public class TimeSlotCommandService(
    TimeSlotNotificationService timeSlotNotificationService,
    IEventStore<TimeSlotEvent> timeSlotEventStore,
    ITimeSlotCommandsToEventTranslator eventTranslator,
    ITraceCollector tracer) : ITimeSlotCommandsService
{
    public async Task SetEndTime(SetEndTimeCommand command)
    {
        await timeSlotEventStore.StoreEventAsync(eventTranslator.ToEvent(command));

        var notification = command.ToNotification();
        await tracer.TimeSlot.SetEndTime.EventPersisted(GetType(), command.TraceId, notification.EndTimeSet);

        await tracer.TimeSlot.SetEndTime.SendingNotification(GetType(), command.TraceId,
            notification.EndTimeSet);
        await timeSlotNotificationService.SendNotificationAsync(command.ToNotification());
    }

    public async Task SetStartTime(SetStartTimeCommand command)
    {
        await timeSlotEventStore.StoreEventAsync(eventTranslator.ToEvent(command));

        var notification = command.ToNotification();
        await tracer.TimeSlot.SetStartTime.EventPersisted(GetType(), command.TraceId,
            notification.StartTimeSet);

        await tracer.TimeSlot.SetStartTime.SendingNotification(GetType(), command.TraceId,
            notification.StartTimeSet);
        await timeSlotNotificationService.SendNotificationAsync(command.ToNotification());
    }

    public async Task AddNote(AddNoteCommand command)
    {
        await timeSlotEventStore.StoreEventAsync(eventTranslator.ToEvent(command));

        var notification = command.ToNotification();
        await tracer.TimeSlot.AddNote.EventPersisted(GetType(), command.TraceId,
            notification.NoteAddedToTimeSlot);

        await tracer.TimeSlot.AddNote.SendingNotification(GetType(), command.TraceId,
            notification.NoteAddedToTimeSlot);
        await timeSlotNotificationService.SendNotificationAsync(command.ToNotification());
    }

    public async Task Create(CreateTimeSlotCommand command)
    {
        await timeSlotEventStore.StoreEventAsync(eventTranslator.ToEvent(command));

        var notification = command.ToNotification();
        await tracer.TimeSlot.Create.EventPersisted(GetType(), command.TraceId, notification.TimeSlotCreated);

        await tracer.TimeSlot.Create.SendingNotification(GetType(), command.TraceId,
            notification.TimeSlotCreated);
        await timeSlotNotificationService.SendNotificationAsync(command.ToNotification());
    }
}