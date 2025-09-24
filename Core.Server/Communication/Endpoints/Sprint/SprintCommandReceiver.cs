using Core.Server.Communication.Endpoints.Sprint.Handlers;
using Core.Server.Services.Entities.Sprints;
using Core.Server.Tracing.Tracing.Tracers;
using Grpc.Core;
using Proto.Command.Sprints;

namespace Core.Server.Communication.Endpoints.Sprint;

public class SprintCommandReceiver(
    ISprintCommandsService sprintsCommandsService,
    IAddTicketToActiveSprintCommandHandler addTicketToActiveSprintCommandHandler,
    ISetSprintActiveStatusCommandHandler setSprintActiveStatusCommandHandler,
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
        await tracer.Sprint.Create.CommandReceived(GetType(), Guid.Parse(request.TraceData.TraceId), request);

        await sprintsCommandsService.Create(request.ToCommand());
        return new CommandResponse { Success = true };
    }

    public override async Task<CommandResponse> SetSprintActiveStatus(SetSprintActiveStatusCommandProto request,
        ServerCallContext context)
    {
        await tracer.Sprint.ActiveStatus.CommandReceived(GetType(), Guid.Parse(request.TraceData.TraceId), request);

        await setSprintActiveStatusCommandHandler.Handle(request.ToCommand());
        return new CommandResponse { Success = true };
    }

    public override async Task<CommandResponse> UpdateSprintData(UpdateSprintDataCommandProto request,
        ServerCallContext context)
    {
        await tracer.Sprint.Update.CommandReceived(GetType(), Guid.Parse(request.TraceData.TraceId), request);

        await sprintsCommandsService.UpdateSprintData(request.ToCommand());
        return new CommandResponse { Success = true };
    }
}