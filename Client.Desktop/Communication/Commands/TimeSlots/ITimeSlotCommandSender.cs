using System.Threading.Tasks;
using Client.Desktop.Communication.Commands.TimeSlots.Records;
using Proto.Command.TimeSlots;

namespace Client.Desktop.Communication.Commands.TimeSlots;

public interface ITimeSlotCommandSender
{
    Task<bool> Send(ClientCreateTimeSlotCommand command);
    Task<bool> Send(ClientAddNoteCommand command);
    Task<bool> Send(ClientSetStartTimeCommand command);
    Task<bool> Send(ClientSetEndTimeCommand command);
}