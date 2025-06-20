using Service.Server.CQRS.Commands.Entities.Sprints;
using Service.Server.CQRS.Commands.Entities.Tickets;
using Service.Server.Old.Services.Entities.Sprints;
using Service.Server.Old.Services.Entities.Tickets;

namespace Service.Server.Communication.Sprint.Handlers;

public class AddTicketToActiveSprintCommandHandler(
    ITicketCommandsService ticketCommandsService,
    ITicketRequestsService ticketRequestsService,
    ISprintRequestsService sprintRequestsService,
    ISprintCommandsService sprintCommandsService)
{
    public async Task Handle(AddTicketToActiveSprintCommand command)
    {
        var ticketDto = await ticketRequestsService.GetTicketById(command.TicketId);

        if (ticketDto == null) return;

        var activeSprint = (await sprintRequestsService.GetAll()).FirstOrDefault(s => s.IsActive);

        if (activeSprint == null) return;

        ticketDto.SprintIds.Add(activeSprint.SprintId);
        await ticketCommandsService.UpdateData(new UpdateTicketDataCommand(ticketDto.TicketId, ticketDto.Name,
            ticketDto.BookingNumber, ticketDto.SprintIds));

        await sprintCommandsService.AddTicketToSprint(new AddTicketToSprintCommand(activeSprint.SprintId,
            command.TicketId));
    }
}