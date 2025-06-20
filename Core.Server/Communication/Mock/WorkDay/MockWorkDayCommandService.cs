using Grpc.Core;
using Proto.Command.WorkDays;
using Proto.Notifications.WorkDay;

namespace Core.Server.Communication.Mock.WorkDay;

public class MockWorkDayCommandService(NotificationService.NotificationServiceClient notificationClient)
    : WorkDayCommandProtoService.WorkDayCommandProtoServiceBase
{
    public override async Task<CommandResponse> CreateWorkDay(CreateWorkDayCommandProto request,
        ServerCallContext context)
    {
        Console.WriteLine($"[CreateWorkDay] ID: {request.WorkDayId}, Date: {request.Date}");

        var notification = new WorkDayCreatedNotification
        {
            WorkDayId = request.WorkDayId,
            Date = request.Date
        };

        await notificationClient.SendWorkDayCreatedAsync(notification);

        return new CommandResponse { Success = true };
    }
}