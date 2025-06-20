using Grpc.Core;
using Proto.Command.WorkDays;
using Proto.Notifications.WorkDay;
using Service.Server.Communication.WorkDay;
using Service.Server.Old.Services.Entities.WorkDays;

namespace Service.Server.Communication.Mock.WorkDay;

public class WorkDayCommandReceiver(
    NotificationService.NotificationServiceClient notificationClient,
    IWorkDayCommandsService workDayCommandsService)
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