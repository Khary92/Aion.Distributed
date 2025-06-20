using Grpc.Core;
using Proto.Command.Sprints;
using Service.Server.Communication.Sprint.Handlers;
using Service.Server.Old.Services.Entities.Sprints;

namespace Service.Server.Communication.Sprint;

public class SprintCommandReceiver(ISprintCommandsService sprintsCommandsService,
    AddTicketToActiveSprintCommandHandler addTicketToActiveSprintCommandHandler,
    AddTicketToSprintCommandHandler addTicketToSprintCommandHandler,
    SetSprintActiveStatusCommandHandler setSprintActiveStatusCommandHandler)
    : SprintCommandProtoService.SprintCommandProtoServiceBase
{
    public override async Task<CommandResponse> AddTicketToActiveSprint(AddTicketToActiveSprintCommandProto request,
        ServerCallContext context)
    {
        Console.WriteLine($"[AddTicketToActiveSprint] TicketID: {request.TicketId}");

        await addTicketToActiveSprintCommandHandler.Handle(request.ToCommand());
        return new CommandResponse { Success = true };
    }

    public override async Task<CommandResponse> AddTicketToSprint(AddTicketToSprintCommandProto request,
        ServerCallContext context)
    {
        Console.WriteLine($"[AddTicketToSprint] SprintID: {request.SprintId}, TicketID: {request.TicketId}");

        await addTicketToSprintCommandHandler.Handle(request.ToCommand());
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