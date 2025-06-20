using Core.Server.Communication.CQRS.Commands.Entities.Tickets;
using Domain.Events.Tickets;

namespace Core.Server.Translators.Tickets;

public interface ITicketCommandsToEventTranslator
{
    TicketEvent ToEvent(CreateTicketCommand createTicketCommand);
    TicketEvent ToEvent(UpdateTicketDataCommand updateTicketDataCommand);
    TicketEvent ToEvent(UpdateTicketDocumentationCommand updateTicketDocumentationCommand);
}