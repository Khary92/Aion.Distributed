using System.Threading.Tasks;
using Proto.Command.UseCases;

namespace Client.Avalonia.Communication.Commands;

public interface IUseCaseCommandSender
{
    Task<bool> Send(CreateTimeSlotControlCommand command);
    Task<bool> Send(LoadTimeSlotControlCommand command);
}