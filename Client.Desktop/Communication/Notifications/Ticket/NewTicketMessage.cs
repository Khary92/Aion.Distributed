using Contract.DTO;

namespace Client.Desktop.Communication.Notifications.Ticket;

public record NewTicketMessage(TicketDto Ticket);