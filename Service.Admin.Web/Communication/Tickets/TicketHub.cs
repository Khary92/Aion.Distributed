using Microsoft.AspNetCore.SignalR;
using Service.Admin.Web.Communication.Tickets.Notifications;
using Service.Admin.Web.DTO;

namespace Service.Admin.Web.Communication.Tickets;

public class TicketHub : Hub
{
    public static class Notifications
    {
        public const string TicketCreated = nameof(ReceiveTicketCreated);
        public const string TicketDataUpdated = nameof(ReceiveTicketDataUpdate);
        public const string TicketDocumentationUpdated = nameof(ReceiveTicketDocumentationUpdated);
    }

    public async Task ReceiveTicketCreated(TicketDto ticket)
        => await Clients.All.SendAsync(Notifications.TicketCreated, ticket);

    public async Task ReceiveTicketDataUpdate(WebTicketDataUpdatedNotification command)
        => await Clients.All.SendAsync(Notifications.TicketDataUpdated, command);

    public async Task ReceiveTicketDocumentationUpdated(WebTicketDocumentationUpdatedNotification command)
        => await Clients.All.SendAsync(Notifications.TicketDocumentationUpdated, command);
}