using Grpc.Core;
using Proto.Command.WorkDays;
using Proto.Notifications.WorkDay;

namespace Service.Server.Mock.WorkDay;

public class MockWorkDayCommandService(NotificationService.NotificationServiceClient notificationClient)
    : WorkDayCommandService.WorkDayCommandServiceBase
{
    public override async Task<CommandResponse> CreateWorkDay(CreateWorkDayCommand request, ServerCallContext context)
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