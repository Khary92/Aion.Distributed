using Application.Contract.CQRS.Commands.Entities.Tickets;
using Application.Services.Entities.Tickets;
using MediatR;

namespace Application.Handler.Commands.Entities.Tickets;

public class UpdateTicketDocumentationCommandHandler(ITicketCommandsService ticketCommandsService)
    : IRequestHandler<UpdateTicketDocumentationCommand, Unit>
{
    public async Task<Unit> Handle(UpdateTicketDocumentationCommand command, CancellationToken cancellationToken)
    {
        await ticketCommandsService.UpdateDocumentation(command);
        return Unit.Value;
    }
}