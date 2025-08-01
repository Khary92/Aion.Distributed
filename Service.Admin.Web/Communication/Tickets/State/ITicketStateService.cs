using Service.Admin.Web.Communication.Tickets.Notifications;
using Service.Admin.Web.Models;

namespace Service.Admin.Web.Communication.Tickets.State;

public interface ITicketStateService
{
    IReadOnlyList<TicketWebModel> Tickets { get; }
    event Action? OnStateChanged;
    Task AddTicket(WebTicketCreatedNotification notification);
    void Apply(WebTicketDataUpdatedNotification notification);
    void Apply(WebTicketDocumentationUpdatedNotification notification);
}