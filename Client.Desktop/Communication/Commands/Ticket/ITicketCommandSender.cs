using System.Threading.Tasks;

namespace Client.Desktop.Communication.Commands.Ticket;

public interface ITicketCommandSender
{
    Task<bool> Send(ClientUpdateTicketDocumentationCommand command);
}