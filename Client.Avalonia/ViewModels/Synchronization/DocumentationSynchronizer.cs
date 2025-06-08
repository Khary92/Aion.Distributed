using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Contract.DTO;
using MediatR;

namespace Client.Avalonia.ViewModels.Synchronization;

public class DocumentationSynchronizer(IMediator mediator) : IStateSynchronizer<TicketReplayDecorator, string>
{
    private readonly ConcurrentDictionary<Guid, string> _documentationById = new();
    private readonly ConcurrentDictionary<Guid, ConcurrentBag<TicketReplayDecorator>> _ticketDecoratorsById = new();

    public void Register(Guid ticketId, TicketReplayDecorator ticket)
    {
        var decorators = _ticketDecoratorsById.GetOrAdd(ticketId, _ => []);
        decorators.Add(ticket);
    }

    public void SetSynchronizationValue(Guid ticketId, string documentation)
    {
        _documentationById.AddOrUpdate(ticketId, documentation, (_, _) => documentation);
    }

    public async Task FireCommand()
    {
        foreach (var ticketId in _ticketDecoratorsById.Keys)
        {
            if (!_documentationById.TryGetValue(ticketId, out var documentation))
                continue;

            if (!_ticketDecoratorsById.TryGetValue(ticketId, out var decorators))
                continue;

            foreach (var decorator in decorators)
            {
                decorator.Ticket.SynchronizeDocumentation(documentation);
                decorator.DisplayedDocumentation = documentation;
            }

            await mediator.Send(new UpdateTicketDocumentationCommand(ticketId, documentation));
        }
    }
}