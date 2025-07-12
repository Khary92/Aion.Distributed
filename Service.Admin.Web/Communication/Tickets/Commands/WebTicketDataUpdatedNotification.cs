namespace Service.Admin.Web.Communication.Tickets.Commands;

public record WebTicketDataUpdatedNotification(Guid TicketId, string Name, string BookingNumber, List<Guid> SprintIds);