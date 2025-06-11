using System.Threading.Tasks;
using Proto.Command.TimeSlots;

namespace Client.Avalonia.Communication.Commands.TimeSlots;

public interface ITimeSlotCommandSender
{
    Task<bool> Send(CreateTimeSlotCommand command);
    Task<bool> Send(AddNoteCommand command);
    Task<bool> Send(SetStartTimeCommand command);
    Task<bool> Send(SetEndTimeCommand command);
}