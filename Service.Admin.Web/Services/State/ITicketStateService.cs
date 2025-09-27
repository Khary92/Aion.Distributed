using Service.Admin.Web.Communication.Records.Notifications;
using Service.Admin.Web.Communication.Records.Wrappers;
using Service.Admin.Web.Models;

namespace Service.Admin.Web.Services.State;

public interface ITicketStateService
{
    IReadOnlyList<TicketWebModel> Tickets { get; }
    event Action? OnStateChanged;
    Task AddTicket(NewTicketMessage ticketMessage);
    Task Apply(WebTicketDataUpdatedNotification notification);
    Task Apply(WebTicketDocumentationUpdatedNotification notification);
}