using Core.Server.Communication.CQRS.Commands.Entities.TimeSlots;
using Domain.Events.TimeSlots;

namespace Core.Server.Translators.TimeSlots;

public interface ITimeSlotCommandsToEventTranslator
{
    TimeSlotEvent ToEvent(CreateTimeSlotCommand createTimeSlotCommand);
    TimeSlotEvent ToEvent(AddNoteCommand addNoteCommand);
    TimeSlotEvent ToEvent(SetStartTimeCommand setStartTimeCommand);
    TimeSlotEvent ToEvent(SetEndTimeCommand setEndTimeCommand);
}