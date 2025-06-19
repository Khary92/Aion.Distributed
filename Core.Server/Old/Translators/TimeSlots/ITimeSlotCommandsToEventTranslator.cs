using Domain.Events.TimeSlots;
using Service.Server.CQRS.Commands.Entities.TimeSlots;

namespace Service.Server.Old.Translators.TimeSlots;

public interface ITimeSlotCommandsToEventTranslator
{
    TimeSlotEvent ToEvent(CreateTimeSlotCommand createTimeSlotCommand);
    TimeSlotEvent ToEvent(AddNoteCommand addNoteCommand);
    TimeSlotEvent ToEvent(SetStartTimeCommand setStartTimeCommand);
    TimeSlotEvent ToEvent(SetEndTimeCommand setEndTimeCommand);
}