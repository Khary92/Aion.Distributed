using Grpc.Core;
using Proto.Command.Tickets;
using Proto.Notifications.Ticket;

namespace Service.Server.Mock;

public class TicketCommandServiceImpl(TicketNotificationServiceImpl ticketNotificationService)
    : TicketCommandService.TicketCommandServiceBase
{
    public override async Task<CommandResponse> CreateTicket(CreateTicketCommand request, ServerCallContext context)
    {
        Console.WriteLine(
            $"[CreateTicket] ID: {request.TicketId}, Name: {request.Name}, BookingNumber: {request.BookingNumber}");

        await ticketNotificationService.SendNotificationAsync(new TicketNotification()
        {
            TicketCreated = new TicketCreatedNotification()
            {
                TicketId = request.TicketId,
                Name = request.Name,
                BookingNumber = request.BookingNumber
            }
        });

        return new CommandResponse { Success = true };
    }

    public override async Task<CommandResponse> UpdateTicketData(UpdateTicketDataCommand request,
        ServerCallContext context)
    {
        Console.WriteLine(
            $"[UpdateTicketData] ID: {request.TicketId}, Name: {request.Name}, BookingNumber: {request.BookingNumber}");

        await ticketNotificationService.SendNotificationAsync(new TicketNotification()
        {
            TicketDataUpdated = new TicketDataUpdatedNotification()
            {
                TicketId = request.TicketId,
                Name = request.Name,
                BookingNumber = request.BookingNumber
            }
        });

        return new CommandResponse { Success = true };
    }

    public override async Task<CommandResponse> UpdateTicketDocumentation(UpdateTicketDocumentationCommand request,
        ServerCallContext context)
    {
        Console.WriteLine($"[UpdateTicketDocumentation] ID: {request.TicketId}, Doc: {request.Documentation}");
        
        await ticketNotificationService.SendNotificationAsync(new TicketNotification()
        {
            TicketDocumentationUpdated = new TicketDocumentationUpdatedNotification()
            {
                TicketId = request.TicketId,
                Documentation = request.Documentation
            }
        });

        return new CommandResponse { Success = true };
    }
}