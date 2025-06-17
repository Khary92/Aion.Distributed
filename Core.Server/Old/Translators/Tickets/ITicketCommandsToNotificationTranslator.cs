using Application.Contract.CQRS.Commands.Entities.Tickets;
using Application.Contract.Notifications.Entities.Tickets;

namespace Application.Translators.Tickets;

public interface ITicketCommandsToNotificationTranslator
{
    TicketCreatedNotification ToNotification(CreateTicketCommand createTicketCommand);
    TicketDataUpdatedNotification ToNotification(UpdateTicketDataCommand updateTicketDataCommand);

    TicketDocumentationUpdatedNotification ToNotification(
        UpdateTicketDocumentationCommand updateTicketDocumentationCommand);
}