using Domain.Events.TimeSlots;
using Service.Server.Communication.CQRS.Commands.Entities.TimeSlots;

namespace Service.Server.Translators.TimeSlots;

public interface ITimeSlotCommandsToEventTranslator
{
    TimeSlotEvent ToEvent(CreateTimeSlotCommand createTimeSlotCommand);
    TimeSlotEvent ToEvent(AddNoteCommand addNoteCommand);
    TimeSlotEvent ToEvent(SetStartTimeCommand setStartTimeCommand);
    TimeSlotEvent ToEvent(SetEndTimeCommand setEndTimeCommand);
}