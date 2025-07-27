using System.Threading.Tasks;
using Client.Desktop.Communication.Commands.UseCases.Records;
using Proto.Command.UseCases;

namespace Client.Desktop.Communication.Commands.UseCases;

public interface IUseCaseCommandSender
{
    Task<bool> Send(ClientCreateTimeSlotControlCommand command);
}