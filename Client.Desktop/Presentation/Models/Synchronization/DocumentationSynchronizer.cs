using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Client.Desktop.Communication.Commands;
using Client.Desktop.Communication.Commands.Ticket;
using Client.Desktop.DataModels.Decorators.Replays;

namespace Client.Desktop.Presentation.Models.Synchronization;

public class DocumentationSynchronizer(ICommandSender commandSender)
    : IStateSynchronizer<TicketReplayDecorator, string>
{
    private readonly ConcurrentDictionary<Guid, string> _documentationById = new();

    private readonly ConcurrentDictionary<Guid, ConcurrentDictionary<TicketReplayDecorator, byte>>
        _ticketDecoratorsById = new();

    private readonly ConcurrentDictionary<Guid, byte> _dirtyTickets = new();

    public void Register(Guid ticketId, TicketReplayDecorator ticket)
    {
        var decorators =
            _ticketDecoratorsById.GetOrAdd(ticketId, _ => new ConcurrentDictionary<TicketReplayDecorator, byte>());
        decorators.TryAdd(ticket, 0);
    }


    public void SetSynchronizationValue(Guid ticketId, string documentation)
    {
        string? currentDocumentation = null;

        if (_documentationById.TryGetValue(ticketId, out var existing))
        {
            currentDocumentation = existing;
        }
        else if (_ticketDecoratorsById.TryGetValue(ticketId, out var existingDecorators))
        {
            foreach (var dec in existingDecorators.Keys)
            {
                currentDocumentation = dec.DisplayedDocumentation;
                break;
            }
        }

        if (string.Equals(currentDocumentation, documentation, StringComparison.Ordinal))
        {
            return;
        }

        _documentationById.AddOrUpdate(ticketId, documentation, (_, _) => documentation);

        _dirtyTickets.TryAdd(ticketId, 0);

        if (!_ticketDecoratorsById.TryGetValue(ticketId, out var decorators)) return;

        foreach (var decorator in decorators.Keys)
        {
            if (string.Equals(decorator.DisplayedDocumentation, documentation, StringComparison.Ordinal)) continue;

            decorator.DisplayedDocumentation = documentation;
        }
    }

    public async Task FireCommand(Guid traceId)
    {
        foreach (var ticketId in _dirtyTickets.Keys)
        {
            if (!_documentationById.TryGetValue(ticketId, out var documentation))
                continue;

            if (_ticketDecoratorsById.TryGetValue(ticketId, out var decorators))
            {
                foreach (var decorator in decorators.Keys)
                {
                    if (!string.Equals(decorator.DisplayedDocumentation, documentation, StringComparison.Ordinal))
                    {
                        decorator.DisplayedDocumentation = documentation;
                    }

                    decorator.Ticket.SynchronizeDocumentation(documentation);
                }
            }

            await commandSender.Send(
                new ClientUpdateTicketDocumentationCommand(ticketId, documentation, traceId));

            _dirtyTickets.TryRemove(ticketId, out _);
        }
    }
    
    public bool IsTicketDirty(Guid ticketId) => _dirtyTickets.ContainsKey(ticketId);

    public ConcurrentDictionary<TicketReplayDecorator, byte> GetDecoratorsById(Guid ticketId)
    {
        return _ticketDecoratorsById.TryGetValue(ticketId, out var decorators)
            ? decorators
            : new ConcurrentDictionary<TicketReplayDecorator, byte>();
    }

    public void RemoveTicket(Guid ticketId)
    {
        _ticketDecoratorsById.TryRemove(ticketId, out _);
        _documentationById.TryRemove(ticketId, out _);
        _dirtyTickets.TryRemove(ticketId, out _);
    }
}