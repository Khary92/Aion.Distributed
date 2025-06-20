using Service.Server.CQRS.Commands.Entities.Sprints;
using Service.Server.CQRS.Commands.Entities.Tickets;
using Service.Server.CQRS.Requests.Sprints;
using Service.Server.Old.Services.Entities.Settings;
using Service.Server.Old.Services.Entities.Sprints;
using Service.Server.Old.Services.Entities.Tickets;

namespace Service.Server.Old.Handler.Commands.Entities.Tickets;

public class CreateTicketCommandHandler(
    ITicketCommandsService ticketCommandsService,
    ISettingsRequestsService settingsRequestsService,
    ISprintRequestsService sprintsRequestsService)
{
    public async Task Handle(CreateTicketCommand command)
    {
        var config = await settingsRequestsService.Get();

        if (config.IsAddNewTicketsToCurrentSprintActive)
        {
            var activeSprint = await sprintsRequestsService.GetActiveSprint(); 

            if (activeSprint != null)
            {
                await mediator.Send(new AddTicketToSprintCommand(activeSprint.SprintId, command.TicketId),
                    cancellationToken);
                command.SprintIds.Add(activeSprint.SprintId);
            }
        }

        await ticketCommandsService.Create(command);
    }
}