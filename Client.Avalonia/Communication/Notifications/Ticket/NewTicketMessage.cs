using Contract.DTO;

namespace Client.Avalonia.Communication.Notifications.Ticket;

public record NewTicketMessage(TicketDto Ticket);