using System.Threading.Tasks;
using Proto.Command.TimeSlots;

namespace Client.Desktop.Communication.Commands.TimeSlots;

public interface ITimeSlotCommandSender
{
    Task<bool> Send(CreateTimeSlotCommandProto command);
    Task<bool> Send(AddNoteCommandProto command);
    Task<bool> Send(SetStartTimeCommandProto command);
    Task<bool> Send(SetEndTimeCommandProto command);
}