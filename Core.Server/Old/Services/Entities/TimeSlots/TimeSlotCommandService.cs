using Application.Contract.CQRS.Commands.Entities.TimeSlots;
using Application.Translators.TimeSlots;
using Domain.Events.TimeSlots;
using Domain.Interfaces;

namespace Application.Services.Entities.TimeSlots;

public class TimeSlotCommandService(
    IEventStore<TimeSlotEvent> timeSlotEventStore,
    ITimeSlotCommandsToEventTranslator eventTranslator) : ITimeSlotCommandsService
{
    public async Task SetEndTime(SetEndTimeCommand setEndTimeCommand)
    {
        await timeSlotEventStore.StoreEventAsync(eventTranslator.ToEvent(setEndTimeCommand));
    }

    public async Task SetStartTime(SetStartTimeCommand setStartTimeCommand)
    {
        await timeSlotEventStore.StoreEventAsync(eventTranslator.ToEvent(setStartTimeCommand));
    }

    public async Task AddNote(AddNoteCommand addNoteCommand)
    {
        await timeSlotEventStore.StoreEventAsync(eventTranslator.ToEvent(addNoteCommand));
    }

    public async Task Create(CreateTimeSlotCommand createTimeSlotCommand)
    {
        await timeSlotEventStore.StoreEventAsync(eventTranslator.ToEvent(createTimeSlotCommand));
    }
}