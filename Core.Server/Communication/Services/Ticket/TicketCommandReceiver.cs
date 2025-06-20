using Grpc.Core;
using Proto.Command.Tickets;
using Service.Server.Services.Entities.Tickets;

namespace Service.Server.Communication.Services.Ticket;

public class TicketCommandReceiver(ITicketCommandsService ticketCommandsService)
    : TicketCommandProtoService.TicketCommandProtoServiceBase
{
    public override async Task<CommandResponse> CreateTicket(CreateTicketCommandProto request,
        ServerCallContext context)
    {
        Console.WriteLine(
            $"[CreateTicket] ID: {request.TicketId}, Name: {request.Name}, BookingNumber: {request.BookingNumber}");

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