using Service.Server.CQRS.Commands.Entities.Tickets;
using Service.Server.Old.Services.Entities.Tickets;

namespace Service.Server.Old.Handler.Commands.Entities.Tickets;

public class UpdateTicketDocumentationCommandHandler(ITicketCommandsService ticketCommandsService)
    : IRequestHandler<UpdateTicketDocumentationCommand, Unit>
{
    public async Task<Unit> Handle(UpdateTicketDocumentationCommand command, CancellationToken cancellationToken)
    {
        await ticketCommandsService.UpdateDocumentation(command);
        return Unit.Value;
    }
}