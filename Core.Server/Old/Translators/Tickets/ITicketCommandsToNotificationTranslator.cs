using Service.Server.CQRS.Commands.Entities.Tickets;

namespace Service.Server.Old.Translators.Tickets;

public interface ITicketCommandsToNotificationTranslator
{
    TicketCreatedNotification ToNotification(CreateTicketCommand createTicketCommand);
    TicketDataUpdatedNotification ToNotification(UpdateTicketDataCommand updateTicketDataCommand);

    TicketDocumentationUpdatedNotification ToNotification(
        UpdateTicketDocumentationCommand updateTicketDocumentationCommand);
}