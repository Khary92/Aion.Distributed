using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Client.Desktop.Communication.Commands;
using Client.Desktop.Communication.Commands.Ticket;
using Client.Desktop.DataModels.Decorators.Replays;
using Client.Tracing.Tracing.Tracers;

namespace Client.Desktop.Presentation.Models.Synchronization;

public class DocumentationSynchronizer(ICommandSender commandSender, ITraceCollector tracer)
    : IStateSynchronizer<TicketReplayDecorator, string>
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

    public async Task FireCommand(Guid traceId)
    {
        // TODO horrible implementation. Felt cute. Might fix later
        var commandTicketId = Guid.Empty;
        var newDocumentation = string.Empty;

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

            commandTicketId = ticketId;
            newDocumentation = documentation;
        }

        await commandSender.Send(
            new ClientUpdateTicketDocumentationCommand(commandTicketId, newDocumentation, traceId));
    }
}