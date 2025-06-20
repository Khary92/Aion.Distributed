using Core.Server.Services.Entities.WorkDays;
using Grpc.Core;
using Proto.Command.WorkDays;

namespace Core.Server.Communication.Services.WorkDay;

public class WorkDayCommandReceiver(IWorkDayCommandsService workDayCommandsService)
    : WorkDayCommandProtoService.WorkDayCommandProtoServiceBase
{
    public override async Task<CommandResponse> CreateWorkDay(CreateWorkDayCommandProto request,
        ServerCallContext context)
    {
        Console.WriteLine($"[CreateWorkDay] ID: {request.WorkDayId}, Date: {request.Date}");

        await workDayCommandsService.Create(request.ToCommand());
        return new CommandResponse { Success = true };
    }
}