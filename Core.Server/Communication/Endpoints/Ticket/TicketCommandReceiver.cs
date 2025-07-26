using Core.Server.Services.Entities.Tickets;
using Core.Server.Tracing.Tracing.Tracers;
using Grpc.Core;
using Proto.Command.Tickets;

namespace Core.Server.Communication.Endpoints.Ticket;

public class TicketCommandReceiver(ITicketCommandsService ticketCommandsService, ITraceCollector tracer)
    : TicketCommandProtoService.TicketCommandProtoServiceBase
{
    public override async Task<CommandResponse> CreateTicket(CreateTicketCommandProto request,
        ServerCallContext context)
    {
        await tracer.Ticket.Create.CommandReceived(GetType(), Guid.Parse(request.TicketId), request);

        await ticketCommandsService.Create(request.ToCommand());
        return new CommandResponse { Success = true };
    }

    public override async Task<CommandResponse> UpdateTicketData(UpdateTicketDataCommandProto request,
        ServerCallContext context)
    {
        Console.WriteLine(
            $"[UpdateTicketData] ID: {request.TicketId}, Name: {request.Name}, BookingNumber: {request.BookingNumber}");

        await ticketCommandsService.UpdateData(request.ToCommand());
        return new CommandResponse { Success = true };
    }

    public override async Task<CommandResponse> UpdateTicketDocumentation(UpdateTicketDocumentationCommandProto request,
        ServerCallContext context)
    {
        Console.WriteLine($"[UpdateTicketDocumentation] ID: {request.TicketId}, Doc: {request.Documentation}");

        await ticketCommandsService.UpdateDocumentation(request.ToCommand());
        return new CommandResponse { Success = true };
    }
}