using Application.Contract.CQRS.Commands.Entities.Sprints;
using Application.Contract.CQRS.Commands.Entities.Tickets;
using Application.Services.Entities.Sprints;
using Application.Services.Entities.Tickets;
using MediatR;

namespace Application.Handler.Commands.Entities.Sprints;

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