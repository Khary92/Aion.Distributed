using Core.Server.Communication.Records.Commands.Entities.TimeSlots;
using Domain.Events.TimeSlots;

namespace Core.Server.Translators.Commands.TimeSlots;

public interface ITimeSlotCommandsToEventTranslator
{
    TimeSlotEvent ToEvent(CreateTimeSlotCommand createTimeSlotCommand);
    TimeSlotEvent ToEvent(AddNoteCommand addNoteCommand);
    TimeSlotEvent ToEvent(SetStartTimeCommand setStartTimeCommand);
    TimeSlotEvent ToEvent(SetEndTimeCommand setEndTimeCommand);
}