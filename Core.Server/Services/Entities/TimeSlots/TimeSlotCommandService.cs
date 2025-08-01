using Core.Server.Communication.Endpoints.TimeSlot;
using Core.Server.Communication.Records.Commands.Entities.TimeSlots;
using Core.Server.Translators.Commands.TimeSlots;
using Domain.Events.TimeSlots;
using Domain.Interfaces;

namespace Core.Server.Services.Entities.TimeSlots;

public class TimeSlotCommandService(
    TimeSlotNotificationService timeSlotNotificationService,
    IEventStore<TimeSlotEvent> timeSlotEventStore,
    ITimeSlotCommandsToEventTranslator eventTranslator) : ITimeSlotCommandsService
{
    public async Task SetEndTime(SetEndTimeCommand command)
    {
        await timeSlotEventStore.StoreEventAsync(eventTranslator.ToEvent(command), command.TraceId);
        await timeSlotNotificationService.SendNotificationAsync(command.ToNotification());
    }

    public async Task SetStartTime(SetStartTimeCommand command)
    {
        await timeSlotEventStore.StoreEventAsync(eventTranslator.ToEvent(command), command.TraceId);
        await timeSlotNotificationService.SendNotificationAsync(command.ToNotification());
    }

    public async Task AddNote(AddNoteCommand command)
    {
        await timeSlotEventStore.StoreEventAsync(eventTranslator.ToEvent(command), command.TraceId);
        await timeSlotNotificationService.SendNotificationAsync(command.ToNotification());
    }

    public async Task Create(CreateTimeSlotCommand command)
    {
        await timeSlotEventStore.StoreEventAsync(eventTranslator.ToEvent(command), command.TraceId);
        await timeSlotNotificationService.SendNotificationAsync(command.ToNotification());
    }
}