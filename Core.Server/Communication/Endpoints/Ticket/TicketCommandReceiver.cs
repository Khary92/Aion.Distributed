using Core.Server.Services.Entities.Tickets;
using Core.Server.Tracing.Tracing.Tracers;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Proto.Command.Tickets;

namespace Core.Server.Communication.Endpoints.Ticket;

[Authorize]
public class TicketCommandReceiver(ITicketCommandsService ticketCommandsService, ITraceCollector tracer)
    : TicketCommandProtoService.TicketCommandProtoServiceBase
{
    public override async Task<CommandResponse> CreateTicket(CreateTicketCommandProto request,
        ServerCallContext context)
    {
        await tracer.Ticket.Create.CommandReceived(GetType(), Guid.Parse(request.TraceData.TraceId), request);

        await ticketCommandsService.Create(request.ToCommand());
        return new CommandResponse { Success = true };
    }

    public override async Task<CommandResponse> UpdateTicketData(UpdateTicketDataCommandProto request,
        ServerCallContext context)
    {
        await tracer.Ticket.Update.CommandReceived(GetType(), Guid.Parse(request.TraceData.TraceId), request);

        await ticketCommandsService.UpdateData(request.ToCommand());
        return new CommandResponse { Success = true };
    }

    public override async Task<CommandResponse> UpdateTicketDocumentation(UpdateTicketDocumentationCommandProto request,
        ServerCallContext context)
    {
        await tracer.Ticket.ChangeDocumentation.CommandReceived(GetType(), Guid.Parse(request.TraceData.TraceId),
            request);

        await ticketCommandsService.UpdateDocumentation(request.ToCommand());
        return new CommandResponse { Success = true };
    }
}