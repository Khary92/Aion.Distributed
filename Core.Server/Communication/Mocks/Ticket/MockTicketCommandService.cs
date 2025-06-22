using Grpc.Core;
using Proto.Command.Tickets;
using Proto.Notifications.Ticket;
using TicketNotificationService = Core.Server.Communication.Endpoints.Ticket.TicketNotificationService;

namespace Core.Server.Communication.Mocks.Ticket;

public class MockTicketCommandService(TicketNotificationService ticketNotificationService)
    : TicketCommandProtoService.TicketCommandProtoServiceBase
{
    public override async Task<CommandResponse> CreateTicket(CreateTicketCommandProto request,
        ServerCallContext context)
    {
        Console.WriteLine(
            $"[CreateTicket] ID: {request.TicketId}, Name: {request.Name}, BookingNumber: {request.BookingNumber}");

        await ticketNotificationService.SendNotificationAsync(new TicketNotification
        {
            TicketCreated = new TicketCreatedNotification
            {
                TicketId = request.TicketId,
                Name = request.Name,
                BookingNumber = request.BookingNumber
            }
        });

        return new CommandResponse { Success = true };
    }

    public override async Task<CommandResponse> UpdateTicketData(UpdateTicketDataCommandProto request,
        ServerCallContext context)
    {
        Console.WriteLine(
            $"[UpdateTicketData] ID: {request.TicketId}, Name: {request.Name}, BookingNumber: {request.BookingNumber}");

        await ticketNotificationService.SendNotificationAsync(new TicketNotification
        {
            TicketDataUpdated = new TicketDataUpdatedNotification
            {
                TicketId = request.TicketId,
                Name = request.Name,
                BookingNumber = request.BookingNumber
            }
        });

        return new CommandResponse { Success = true };
    }

    public override async Task<CommandResponse> UpdateTicketDocumentation(UpdateTicketDocumentationCommandProto request,
        ServerCallContext context)
    {
        Console.WriteLine($"[UpdateTicketDocumentation] ID: {request.TicketId}, Doc: {request.Documentation}");

        await ticketNotificationService.SendNotificationAsync(new TicketNotification
        {
            TicketDocumentationUpdated = new TicketDocumentationUpdatedNotification
            {
                TicketId = request.TicketId,
                Documentation = request.Documentation
            }
        });

        return new CommandResponse { Success = true };
    }
}