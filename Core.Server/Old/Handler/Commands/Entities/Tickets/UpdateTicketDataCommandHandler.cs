using Service.Server.CQRS.Commands.Entities.Tickets;
using Service.Server.Old.Services.Entities.Tickets;

namespace Service.Server.Old.Handler.Commands.Entities.Tickets;

public class UpdateTicketDataCommandHandler(ITicketCommandsService ticketCommandsService)
    : IRequestHandler<UpdateTicketDataCommand, Unit>
{
    public async Task<Unit> Handle(UpdateTicketDataCommand command, CancellationToken cancellationToken)
    {
        await ticketCommandsService.UpdateData(command);
        return Unit.Value;
    }
}