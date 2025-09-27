using Proto.Requests.Tickets;
using Service.Admin.Tracing;
using Service.Admin.Web.Communication.Mapper;
using Service.Admin.Web.Communication.Records.Notifications;
using Service.Admin.Web.Communication.Records.Wrappers;
using Service.Admin.Web.Communication.Sender.Common;
using Service.Admin.Web.Models;
using Service.Admin.Web.Services.Startup;

namespace Service.Admin.Web.Services.State;

public class TicketStateService(ISharedRequestSender requestSender, ITraceCollector tracer)
    : ITicketStateService, IInitializeAsync
{
    private List<TicketWebModel> _tickets = [];

    public InitializationType Type => InitializationType.StateService;

    public async Task InitializeComponents()
    {
        var ticketListProto = await requestSender.Send(new GetAllTicketsRequestProto());
        _tickets = ticketListProto.ToWebModelList();
        NotifyStateChanged();
    }

    public IReadOnlyList<TicketWebModel> Tickets => _tickets.AsReadOnly();
    public event Action? OnStateChanged;

    public async Task AddTicket(NewTicketMessage ticketMessage)
    {
        _tickets.Add(ticketMessage.Ticket);

        await tracer.Ticket.Create.AggregateAdded(GetType(), ticketMessage.TraceId);

        NotifyStateChanged();
    }

    public async Task Apply(WebTicketDataUpdatedNotification notification)
    {
        var ticket = _tickets.FirstOrDefault(x => x.TicketId == notification.TicketId);

        if (ticket == null)
        {
            await tracer.Ticket.Update.NoAggregateFound(GetType(), notification.TraceId);
            return;
        }

        ticket.Apply(notification);
        await tracer.Ticket.Update.ChangesApplied(GetType(), notification.TraceId);
        NotifyStateChanged();
    }

    public async Task Apply(WebTicketDocumentationUpdatedNotification notification)
    {
        var ticket = _tickets.FirstOrDefault(x => x.TicketId == notification.TicketId);

        if (ticket == null)
        {
            await tracer.Ticket.ChangeDocumentation.NoAggregateFound(GetType(), notification.TraceId);
            return;
        }

        ticket.Apply(notification);
        await tracer.Ticket.ChangeDocumentation.ChangesApplied(GetType(), notification.TraceId);
        NotifyStateChanged();
    }

    private void NotifyStateChanged()
    {
        OnStateChanged?.Invoke();
    }
}