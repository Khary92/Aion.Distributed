using Application.Contract.CQRS.Commands.Entities.TimeSlots;

namespace Application.Services.Entities.TimeSlots;

public interface ITimeSlotCommandsService
{
    Task SetEndTime(SetEndTimeCommand setEndTimeCommand);
    Task SetStartTime(SetStartTimeCommand setStartTimeCommand);
    Task AddNote(AddNoteCommand addNoteCommand);
    Task Create(CreateTimeSlotCommand createTimeSlotCommand);
}