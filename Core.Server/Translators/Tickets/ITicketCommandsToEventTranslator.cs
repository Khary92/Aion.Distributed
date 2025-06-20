using Domain.Events.Tickets;
using Service.Server.Communication.CQRS.Commands.Entities.Tickets;

namespace Service.Server.Translators.Tickets;

public interface ITicketCommandsToEventTranslator
{
    TicketEvent ToEvent(CreateTicketCommand createTicketCommand);
    TicketEvent ToEvent(UpdateTicketDataCommand updateTicketDataCommand);
    TicketEvent ToEvent(UpdateTicketDocumentationCommand updateTicketDocumentationCommand);
}