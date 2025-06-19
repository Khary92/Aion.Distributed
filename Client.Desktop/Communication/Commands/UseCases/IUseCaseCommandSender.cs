using System.Threading.Tasks;
using Proto.Command.UseCases;

namespace Client.Desktop.Communication.Commands.UseCases;

public interface IUseCaseCommandSender
{
    Task<bool> Send(CreateTimeSlotControlCommandProto command);
    Task<bool> Send(LoadTimeSlotControlCommandProto command);
}