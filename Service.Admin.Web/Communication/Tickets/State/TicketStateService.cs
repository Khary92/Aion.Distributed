using Proto.Requests.Tickets;
using Service.Admin.Tracing;
using Service.Admin.Web.Communication.Tickets.Notifications;
using Service.Admin.Web.DTO;

namespace Service.Admin.Web.Communication.Tickets.State;

public class TicketStateService(ISharedRequestSender requestSender, ITraceCollector tracer) : ITicketStateService
{
    private List<TicketDto> _tickets = new();

    public IReadOnlyList<TicketDto> Tickets => _tickets.AsReadOnly();
    public event Action? OnStateChanged;

    public async Task AddTicket(TicketDto ticket)
    {
        await tracer.Ticket.Create.AggregateReceived(GetType(), ticket.TicketId, ticket.AsTraceAttributes());
        _tickets.Add(ticket);
        await tracer.Ticket.Create.AggregateAdded(GetType(), ticket.TicketId);
        NotifyStateChanged();
    }

    public void Apply(WebTicketDataUpdatedNotification ticketDataUpdatedNotification)
    {
        
        var ticket = _tickets.FirstOrDefault(x => x.TicketId == ticketDataUpdatedNotification.TicketId);

        if (ticket == null)
        {
            return;
        }

        ticket.Apply(ticketDataUpdatedNotification);
        NotifyStateChanged();
    }

    public void Apply(WebTicketDocumentationUpdatedNotification ticketDocumentationUpdatedNotification)
    {
        var ticket = _tickets.FirstOrDefault(x => x.TicketId == ticketDocumentationUpdatedNotification.TicketId);

        if (ticket == null)
        {
            return;
        }

        ticket.Apply(ticketDocumentationUpdatedNotification);
        NotifyStateChanged();
    }

    public async  Task LoadTickets()
    {
        var ticketListProto = await requestSender.Send(new GetAllTicketsRequestProto());
        _tickets = ticketListProto.ToDtoList();
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnStateChanged?.Invoke();
}