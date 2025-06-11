using System.Threading.Tasks;
using Proto.Command.Sprints;

namespace Client.Avalonia.Communication.Commands.Sprints;

public interface ISprintCommandSender
{
    Task<bool> Send(CreateSprintCommand command);
    Task<bool> Send(AddTicketToActiveSprintCommand command);
    Task<bool> Send(AddTicketToSprintCommand command);
    Task<bool> Send(SetSprintActiveStatusCommand command);
    Task<bool> Send(UpdateSprintDataCommand command);
}