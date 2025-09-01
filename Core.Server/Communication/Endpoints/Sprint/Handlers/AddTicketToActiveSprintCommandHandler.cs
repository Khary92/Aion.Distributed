using Core.Server.Communication.Records.Commands.Entities.Sprints;
using Core.Server.Communication.Records.Commands.Entities.Tickets;
using Core.Server.Services.Entities.Sprints;
using Core.Server.Services.Entities.Tickets;
using Core.Server.Tracing.Tracing.Tracers;

namespace Core.Server.Communication.Endpoints.Sprint.Handlers;

public class AddTicketToActiveSprintCommandHandler(
    ITicketCommandsService ticketCommandsService,
    ITicketRequestsService ticketRequestsService,
    ISprintRequestsService sprintRequestsService,
    ISprintCommandsService sprintCommandsService,
    ITraceCollector tracer)
{
    public async Task Handle(AddTicketToActiveSprintCommand command)
    {
        var ticketDto = await ticketRequestsService.GetTicketById(command.TicketId);

        if (ticketDto == null)
        {
            await tracer.Sprint.AddTicketToSprint.TicketNotFound(GetType(), command.TraceId);
            return;
        }

        var activeSprint = await sprintRequestsService.GetActiveSprint();

        if (activeSprint == null)
        {
            await tracer.Sprint.AddTicketToSprint.NoActiveSprint(GetType(), command.TraceId);
            return;
        }

        if (ticketDto.SprintIds.Contains(activeSprint.SprintId))
        {
            await tracer.Sprint.AddTicketToSprint.TicketIsAlreadyInSprint(GetType(), command.TraceId);
            return;
        }
        
        ticketDto.SprintIds.Add(activeSprint.SprintId);
        await ticketCommandsService.UpdateData(new UpdateTicketDataCommand(ticketDto.TicketId, ticketDto.Name,
            ticketDto.BookingNumber, ticketDto.SprintIds, command.TraceId));
        
        await tracer.Sprint.AddTicketToSprint.AddedSprintIdToTicket(GetType(), command.TraceId,
            activeSprint.SprintId);

        await sprintCommandsService.AddTicketToSprint(new AddTicketToSprintCommand(activeSprint.SprintId,
            command.TicketId, command.TraceId));
    }
}