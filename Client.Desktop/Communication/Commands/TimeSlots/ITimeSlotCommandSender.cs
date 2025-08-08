using System.Threading.Tasks;
using Client.Desktop.Communication.Commands.TimeSlots.Records;

namespace Client.Desktop.Communication.Commands.TimeSlots;

public interface ITimeSlotCommandSender
{
    Task<bool> Send(ClientSetStartTimeCommand command);
    Task<bool> Send(ClientSetEndTimeCommand command);
}