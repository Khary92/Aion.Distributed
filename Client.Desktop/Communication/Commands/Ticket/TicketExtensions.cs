using Proto.Command.Tickets;

namespace Client.Desktop.Communication.Commands.Ticket;

public static class TicketExtensions
{
    public static UpdateTicketDocumentationCommandProto ToProto(this ClientUpdateTicketDocumentationCommand command) => new()
    {
        TicketId = command.TicketId.ToString(),
        Documentation = command.Documentation
    };
}