using Core.Server.Communication.Records.Commands.Entities.TimeSlots;

namespace Core.Server.Services.Entities.TimeSlots;

public interface ITimeSlotCommandsService
{
    Task SetEndTime(SetEndTimeCommand command);
    Task SetStartTime(SetStartTimeCommand command);
    Task AddNote(AddNoteCommand command);
    Task Create(CreateTimeSlotCommand command);
}