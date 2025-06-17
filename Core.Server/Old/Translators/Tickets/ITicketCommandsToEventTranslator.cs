using Application.Contract.CQRS.Commands.Entities.Tickets;
using Domain.Events.Tickets;

namespace Application.Translators.Tickets;

public interface ITicketCommandsToEventTranslator
{
    TicketEvent ToEvent(CreateTicketCommand createTicketCommand);
    TicketEvent ToEvent(UpdateTicketDataCommand updateTicketDataCommand);
    TicketEvent ToEvent(UpdateTicketDocumentationCommand updateTicketDocumentationCommand);
}