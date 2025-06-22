using Core.Server.Communication.Records.Commands.Entities.Sprints;
using Core.Server.Communication.Records.Commands.Entities.Tickets;
using Core.Server.Services.Entities.Sprints;
using Core.Server.Services.Entities.Tickets;

namespace Core.Server.Communication.Endpoints.Sprint.Handlers;

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