using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.Desktop.Communication.Commands;
using Client.Desktop.Communication.Commands.Ticket;
using Client.Desktop.Communication.Notifications.Client.Records;
using Client.Desktop.Communication.Notifications.Ticket.Records;
using Client.Desktop.Lifecycle.Startup.Tasks.Register;
using CommunityToolkit.Mvvm.Messaging;

namespace Client.Desktop.Presentation.Models.Synchronization;

public class DocumentationSynchronizer(IMessenger messenger, ICommandSender commandSender)
    : IMessengerRegistration, IRecipient<ClientTicketDocumentationUpdatedNotification>,
        IRecipient<ClientSaveDocumentationNotification>, IDocumentationSynchronizer
{
    private readonly List<IDocumentationSynchronizationListener> _listeners = [];

    public void Register(IDocumentationSynchronizationListener listener)
    {
        _listeners.Add(listener);
    }

    public void RegisterMessenger()
    {
        messenger.RegisterAll(this);
    }

    public void UnregisterMessenger()
    {
        messenger.UnregisterAll(this);
    }

    public void Synchronize(Guid ticketId, string documentation)
    {
        foreach (var listener in _listeners.Where(listener => listener.Ticket.TicketId == ticketId))
        {
            listener.Ticket.Documentation = documentation;
        }
    }

    public void Receive(ClientTicketDocumentationUpdatedNotification message)
    {
        foreach (var listener in _listeners.Where(listener => listener.Ticket.TicketId == message.TicketId))
        {
            listener.Ticket.Apply(message);
        }
    }

    public void Receive(ClientSaveDocumentationNotification message) => _ = SendCommand(message);

    private async Task SendCommand(ClientSaveDocumentationNotification message)
    {
        var traceId = Guid.NewGuid();

        var dirtyTickets = _listeners
            .Select(l => l.Ticket)
            .GroupBy(t => t.TicketId)
            .Select(g => g.First())
            .Where(t => t.IsDirty)
            .ToList();

        foreach (var ticket in dirtyTickets)
        {
            await commandSender.Send(new ClientUpdateTicketDocumentationCommand(
                ticket.TicketId,
                ticket.Documentation,
                traceId));
        }
    }
}