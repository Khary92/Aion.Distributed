using System.Threading.Tasks;
using Proto.Command.Tickets;

namespace Client.Desktop.Communication.Commands.Tickets;

public interface ITicketCommandSender
{
    Task<bool> Send(CreateTicketCommand command);
    Task<bool> Send(UpdateTicketDataCommand command);
    Task<bool> Send(UpdateTicketDocumentationCommand command);
}