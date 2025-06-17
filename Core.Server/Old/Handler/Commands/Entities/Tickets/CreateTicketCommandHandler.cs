using Application.Contract.CQRS.Commands.Entities.Sprints;
using Application.Contract.CQRS.Commands.Entities.Tickets;
using Application.Contract.CQRS.Requests.Sprints;
using Application.Services.Entities.Settings;
using Application.Services.Entities.Tickets;
using MediatR;

namespace Application.Handler.Commands.Entities.Tickets;

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