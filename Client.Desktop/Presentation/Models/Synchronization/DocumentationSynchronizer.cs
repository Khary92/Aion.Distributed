using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.Desktop.Communication.Commands;
using Client.Desktop.Communication.Commands.Ticket;
using Client.Desktop.Communication.Local;
using Client.Desktop.Communication.Local.LocalEvents.Publisher;
using Client.Desktop.Communication.Local.LocalEvents.Records;
using Client.Desktop.Communication.Notifications.Ticket.Records;
using Client.Desktop.Lifecycle.Startup.Tasks.Register;
using Client.Tracing.Tracing.Tracers;

namespace Client.Desktop.Presentation.Models.Synchronization;

public class DocumentationSynchronizer(
    ICommandSender commandSender,
    ITraceCollector tracer,
    INotificationPublisherFacade notificationPublisher,
    IClientTimerNotificationPublisher clientTimerNotificationPublisher)
    : IDocumentationSynchronizer, IMessengerRegistration
{
    private readonly List<IDocumentationSynchronizationListener> _listeners = [];

    public void Register(IDocumentationSynchronizationListener listener)
    {
        _listeners.Add(listener);
    }

    public void Synchronize(Guid ticketId, string documentation)
    {
        foreach (var listener in _listeners.Where(listener => listener.Ticket.TicketId == ticketId))
            listener.Ticket.Documentation = documentation;
    }

    public void RegisterMessenger()
    {
        notificationPublisher.Ticket.TicketDocumentationUpdatedNotificationReceived +=
            HandleClientTicketDocumentationUpdatedNotification;
        clientTimerNotificationPublisher.ClientSaveDocumentationNotificationReceived +=
            HandleClientSaveDocumentationNotification;
    }

    public void UnregisterMessenger()
    {
        notificationPublisher.Ticket.TicketDocumentationUpdatedNotificationReceived -=
            HandleClientTicketDocumentationUpdatedNotification;
        clientTimerNotificationPublisher.ClientSaveDocumentationNotificationReceived -=
            HandleClientSaveDocumentationNotification;
    }

    private async Task HandleClientSaveDocumentationNotification(ClientSaveDocumentationNotification message)
    {
        var traceId = Guid.NewGuid();
        await tracer.Ticket.ChangeDocumentation.StartUseCase(GetType(), traceId);

        var dirtyTickets = _listeners
            .Select(l => l.Ticket)
            .GroupBy(t => t.TicketId)
            .Select(g => g.First())
            .Where(t => t.IsDirty)
            .ToList();

        if (dirtyTickets.Count == 0)
        {
            await tracer.Ticket.ChangeDocumentation.ActionAborted(GetType(), traceId);
            return;
        }

        foreach (var ticket in dirtyTickets)
        {
            var clientUpdateTicketDocumentationCommand = new ClientUpdateTicketDocumentationCommand(
                ticket.TicketId,
                ticket.Documentation,
                traceId);

            await tracer.Ticket.ChangeDocumentation.SendingCommand(GetType(), traceId,
                clientUpdateTicketDocumentationCommand);
            await commandSender.Send(clientUpdateTicketDocumentationCommand);
        }
    }

    private Task HandleClientTicketDocumentationUpdatedNotification(
        ClientTicketDocumentationUpdatedNotification message)
    {
        foreach (var listener in _listeners.Where(listener => listener.Ticket.TicketId == message.TicketId)) ;
        return Task.CompletedTask;
    }
}