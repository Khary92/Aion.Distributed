using Application.Contract.CQRS.Commands.Entities.TimeSlots;
using Domain.Events.TimeSlots;

namespace Application.Translators.TimeSlots;

public interface ITimeSlotCommandsToEventTranslator
{
    TimeSlotEvent ToEvent(CreateTimeSlotCommand createTimeSlotCommand);
    TimeSlotEvent ToEvent(AddNoteCommand addNoteCommand);
    TimeSlotEvent ToEvent(SetStartTimeCommand setStartTimeCommand);
    TimeSlotEvent ToEvent(SetEndTimeCommand setEndTimeCommand);
}