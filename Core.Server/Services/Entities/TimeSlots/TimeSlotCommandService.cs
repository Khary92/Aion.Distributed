using Core.Server.Communication.CQRS.Commands.Entities.TimeSlots;
using Core.Server.Communication.Services.TimeSlot;
using Core.Server.Translators.TimeSlots;
using Domain.Events.TimeSlots;
using Domain.Interfaces;

namespace Core.Server.Services.Entities.TimeSlots;

public class TimeSlotCommandService(
    TimeSlotNotificationService timeSlotNotificationService,
    IEventStore<TimeSlotEvent> timeSlotEventStore,
    ITimeSlotCommandsToEventTranslator eventTranslator) : ITimeSlotCommandsService
{
    public async Task SetEndTime(SetEndTimeCommand setEndTimeCommand)
    {
        await timeSlotEventStore.StoreEventAsync(eventTranslator.ToEvent(setEndTimeCommand));
        await timeSlotNotificationService.SendNotificationAsync(setEndTimeCommand.ToNotification());
    }

    public async Task SetStartTime(SetStartTimeCommand setStartTimeCommand)
    {
        await timeSlotEventStore.StoreEventAsync(eventTranslator.ToEvent(setStartTimeCommand));
        await timeSlotNotificationService.SendNotificationAsync(setStartTimeCommand.ToNotification());
    }

    public async Task AddNote(AddNoteCommand addNoteCommand)
    {
        await timeSlotEventStore.StoreEventAsync(eventTranslator.ToEvent(addNoteCommand));
        await timeSlotNotificationService.SendNotificationAsync(addNoteCommand.ToNotification());
    }

    public async Task Create(CreateTimeSlotCommand createTimeSlotCommand)
    {
        await timeSlotEventStore.StoreEventAsync(eventTranslator.ToEvent(createTimeSlotCommand));
        await timeSlotNotificationService.SendNotificationAsync(createTimeSlotCommand.ToNotification());
    }
}