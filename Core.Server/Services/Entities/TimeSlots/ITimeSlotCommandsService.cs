using Service.Server.Communication.CQRS.Commands.Entities.TimeSlots;

namespace Service.Server.Services.Entities.TimeSlots;

public interface ITimeSlotCommandsService
{
    Task SetEndTime(SetEndTimeCommand setEndTimeCommand);
    Task SetStartTime(SetStartTimeCommand setStartTimeCommand);
    Task AddNote(AddNoteCommand addNoteCommand);
    Task Create(CreateTimeSlotCommand createTimeSlotCommand);
}