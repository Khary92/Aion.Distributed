using Service.Admin.Web.Communication.Tickets.Notifications;
using Service.Admin.Web.DTO;

namespace Service.Admin.Web.Communication.Tickets.State;

public interface ITicketStateService
{
    IReadOnlyList<TicketDto> Tickets { get; }
    event Action? OnStateChanged;
    Task AddTicket(TicketDto ticket);
    void Apply(WebTicketDataUpdatedNotification ticketDataUpdatedNotification);
    void Apply(WebTicketDocumentationUpdatedNotification ticketDocumentationUpdatedNotification);
    Task LoadTickets();
}