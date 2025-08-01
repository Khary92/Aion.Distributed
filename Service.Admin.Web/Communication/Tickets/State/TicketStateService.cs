using Proto.Requests.Tickets;
using Service.Admin.Tracing;
using Service.Admin.Web.Communication.Tickets.Notifications;
using Service.Admin.Web.Models;
using Service.Admin.Web.Services;

namespace Service.Admin.Web.Communication.Tickets.State;

public class TicketStateService(ISharedRequestSender requestSender, ITraceCollector tracer)
    : ITicketStateService, IInitializeAsync
{
    private List<TicketWebModel> _tickets = new();

    public IReadOnlyList<TicketWebModel> Tickets => _tickets.AsReadOnly();
    public event Action? OnStateChanged;

    public async Task AddTicket(WebTicketCreatedNotification notification)
    {
        var ticket = notification.ToWebModel();
        
        await tracer.Ticket.Create.AggregateReceived(GetType(), notification.TraceId,
            ticket.AsTraceAttributes());
        
        _tickets.Add(ticket);
        
        await tracer.Ticket.Create.AggregateAdded(GetType(), notification.TraceId);
        
        NotifyStateChanged();
    }

    public void Apply(WebTicketDataUpdatedNotification ticketDataUpdatedNotification)
    {
        var ticket = _tickets.FirstOrDefault(x => x.TicketId == ticketDataUpdatedNotification.TicketId);

        if (ticket == null) return;

        ticket.Apply(ticketDataUpdatedNotification);
        NotifyStateChanged();
    }

    public void Apply(WebTicketDocumentationUpdatedNotification ticketDocumentationUpdatedNotification)
    {
        var ticket = _tickets.FirstOrDefault(x => x.TicketId == ticketDocumentationUpdatedNotification.TicketId);

        if (ticket == null) return;

        ticket.Apply(ticketDocumentationUpdatedNotification);
        NotifyStateChanged();
    }

    private void NotifyStateChanged()
    {
        OnStateChanged?.Invoke();
    }

    public InitializationType Type => InitializationType.StateService;

    public async Task InitializeComponents()
    {
        var ticketListProto = await requestSender.Send(new GetAllTicketsRequestProto());
        _tickets = ticketListProto.ToDtoList();
        NotifyStateChanged();
    }
}