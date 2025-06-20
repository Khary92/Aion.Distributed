using Grpc.Core;
using Proto.Command.WorkDays;
using Service.Server.Old.Services.Entities.WorkDays;

namespace Service.Server.Communication.WorkDay;

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