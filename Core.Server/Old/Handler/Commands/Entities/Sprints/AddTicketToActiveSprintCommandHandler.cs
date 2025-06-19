using Service.Server.CQRS.Commands.Entities.Sprints;
using Service.Server.CQRS.Commands.Entities.Tickets;
using Service.Server.Old.Services.Entities.Sprints;
using Service.Server.Old.Services.Entities.Tickets;

namespace Service.Server.Old.Handler.Commands.Entities.Sprints;

public class AddTicketToActiveSprintCommandHandler(
    ITicketCommandsService ticketCommandsService,
    ITicketRequestsService ticketRequestsService,
    ISprintRequestsService sprintRequestsService,
    ISprintCommandsService sprintCommandsService)
    : IRequestHandler<AddTicketToActiveSprintCommand, Unit>
{
    public async Task<Unit> Handle(AddTicketToActiveSprintCommand command, CancellationToken cancellationToken)
    {
        var ticketDto = await ticketRequestsService.GetTicketAsync(command.TicketId);

        if (ticketDto == null) return Unit.Value;

        var activeSprint = (await sprintRequestsService.GetAll()).FirstOrDefault(s => s.IsActive);

        if (activeSprint == null) return Unit.Value;

        ticketDto.SprintIds.Add(activeSprint.SprintId);
        await ticketCommandsService.UpdateData(new UpdateTicketDataCommand(ticketDto.TicketId, ticketDto.Name,
            ticketDto.BookingNumber, ticketDto.SprintIds));

        await sprintCommandsService.AddTicketToSprint(new AddTicketToSprintCommand(activeSprint.SprintId,
            command.TicketId));

        return Unit.Value;
    }
}