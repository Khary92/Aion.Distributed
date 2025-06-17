using Application.Contract.CQRS.Commands.Entities.Tickets;
using Application.Services.Entities.Tickets;
using MediatR;

namespace Application.Handler.Commands.Entities.Tickets;

public class UpdateTicketDataCommandHandler(ITicketCommandsService ticketCommandsService)
    : IRequestHandler<UpdateTicketDataCommand, Unit>
{
    public async Task<Unit> Handle(UpdateTicketDataCommand command, CancellationToken cancellationToken)
    {
        await ticketCommandsService.UpdateData(command);
        return Unit.Value;
    }
}