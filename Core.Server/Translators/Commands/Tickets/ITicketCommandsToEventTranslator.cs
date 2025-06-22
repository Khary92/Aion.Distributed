using Core.Server.Communication.Records.Commands.Entities.Tickets;
using Domain.Events.Tickets;

namespace Core.Server.Translators.Commands.Tickets;

public interface ITicketCommandsToEventTranslator
{
    TicketEvent ToEvent(CreateTicketCommand createTicketCommand);
    TicketEvent ToEvent(UpdateTicketDataCommand updateTicketDataCommand);
    TicketEvent ToEvent(UpdateTicketDocumentationCommand updateTicketDocumentationCommand);
}