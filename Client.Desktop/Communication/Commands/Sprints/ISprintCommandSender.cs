using System.Threading.Tasks;
using Proto.Command.Sprints;

namespace Client.Desktop.Communication.Commands.Sprints;

public interface ISprintCommandSender
{
    Task<bool> Send(CreateSprintCommandProto command);
    Task<bool> Send(AddTicketToActiveSprintCommandProto command);
    Task<bool> Send(AddTicketToSprintCommandProto command);
    Task<bool> Send(SetSprintActiveStatusCommandProto command);
    Task<bool> Send(UpdateSprintDataCommandProto command);
}