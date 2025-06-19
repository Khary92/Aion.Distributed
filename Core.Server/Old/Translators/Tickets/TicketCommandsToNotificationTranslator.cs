using Service.Server.CQRS.Commands.Entities.Tickets;

namespace Service.Server.Old.Translators.Tickets;

public class TicketCommandsToNotificationTranslator : ITicketCommandsToNotificationTranslator
{
    public TicketCreatedNotification ToNotification(CreateTicketCommand createTicketCommand)
    {
        return new TicketCreatedNotification(createTicketCommand.TicketId, createTicketCommand.Name,
            createTicketCommand.BookingNumber, createTicketCommand.SprintIds);
    }

    public TicketDataUpdatedNotification ToNotification(UpdateTicketDataCommand updateTicketDataCommand)
    {
        return new TicketDataUpdatedNotification(updateTicketDataCommand.TicketId, updateTicketDataCommand.Name,
            updateTicketDataCommand.BookingNumber, updateTicketDataCommand.SprintIds);
    }

    public TicketDocumentationUpdatedNotification ToNotification(
        UpdateTicketDocumentationCommand updateTicketDocumentationCommand)
    {
        return new TicketDocumentationUpdatedNotification(updateTicketDocumentationCommand.TicketId,
            updateTicketDocumentationCommand.Documentation);
    }
}