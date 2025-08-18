using Core.Server.Communication.Endpoints.Sprint.Handlers;
using Core.Server.Services.Entities.Sprints;
using Core.Server.Tracing.Tracing.Tracers;
using Grpc.Core;
using Proto.Command.Sprints;

namespace Core.Server.Communication.Endpoints.Sprint;

public class SprintCommandReceiver(
    ISprintCommandsService sprintsCommandsService,
    AddTicketToActiveSprintCommandHandler addTicketToActiveSprintCommandHandler,
    SetSprintActiveStatusCommandHandler setSprintActiveStatusCommandHandler, 
    ITraceCollector tracer)
    : SprintCommandProtoService.SprintCommandProtoServiceBase
{
    public override async Task<CommandResponse> AddTicketToActiveSprint(AddTicketToActiveSprintCommandProto request,
        ServerCallContext context)
    {
        var command = request.ToCommand();
        await tracer.Sprint.AddTicketToSprint.CommandReceived(GetType(), command.TraceId, command);

        await addTicketToActiveSprintCommandHandler.Handle(command);
        return new CommandResponse { Success = true };
    }
    
    public override async Task<CommandResponse> CreateSprint(CreateSprintCommandProto request,
        ServerCallContext context)
    {
        Console.WriteLine(
            $"[CreateSprint] SprintID: {request.SprintId}, Name: {request.Name}, Start: {request.StartTime}, End: {request.EndTime}, IsActive: {request.IsActive}, Tickets: {string.Join(", ", request.TicketIds)}");

        await sprintsCommandsService.Create(request.ToCommand());
        return new CommandResponse { Success = true };
    }

    public override async Task<CommandResponse> SetSprintActiveStatus(SetSprintActiveStatusCommandProto request,
        ServerCallContext context)
    {
        Console.WriteLine($"[SetSprintActiveStatus] SprintID: {request.SprintId}, IsActive: {request.IsActive}");

        await setSprintActiveStatusCommandHandler.Handle(request.ToCommand());
        return new CommandResponse { Success = true };
    }

    public override async Task<CommandResponse> UpdateSprintData(UpdateSprintDataCommandProto request,
        ServerCallContext context)
    {
        Console.WriteLine(
            $"[UpdateSprintData] SprintID: {request.SprintId}, Name: {request.Name}, Start: {request.StartTime}, End: {request.EndTime}");

        await sprintsCommandsService.UpdateSprintData(request.ToCommand());
        return new CommandResponse { Success = true };
    }
}