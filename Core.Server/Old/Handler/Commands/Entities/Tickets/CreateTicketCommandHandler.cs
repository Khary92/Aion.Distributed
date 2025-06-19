using Service.Server.CQRS.Commands.Entities.Sprints;
using Service.Server.CQRS.Commands.Entities.Tickets;
using Service.Server.CQRS.Requests.Sprints;
using Service.Server.Old.Services.Entities.Settings;
using Service.Server.Old.Services.Entities.Tickets;

namespace Service.Server.Old.Handler.Commands.Entities.Tickets;

public class CreateTicketCommandHandler(
    ITicketCommandsService ticketCommandsService,
    ISettingsRequestsService settingsRequestsService,
    IMediator mediator)
    : IRequestHandler<CreateTicketCommand, Unit>
{
    public async Task<Unit> Handle(CreateTicketCommand command, CancellationToken cancellationToken)
    {
        var config = await settingsRequestsService.Get();

        if (config!.IsAddNewTicketsToCurrentSprintActive)
        {
            var activeSprint = await mediator.Send(new GetActiveSprintRequest(), cancellationToken);

            if (activeSprint != null)
            {
                await mediator.Send(new AddTicketToSprintCommand(activeSprint.SprintId, command.TicketId),
                    cancellationToken);
                command.SprintIds.Add(activeSprint.SprintId);
            }
        }

        await ticketCommandsService.Create(command);
        return Unit.Value;
    }
}