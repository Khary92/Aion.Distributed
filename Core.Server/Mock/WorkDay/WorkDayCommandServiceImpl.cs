using Grpc.Core;
using Proto.Command.WorkDays;
using Proto.Notifications.WorkDay;

namespace Service.Server.Mock.WorkDay;

public class WorkDayCommandServiceImpl : WorkDayCommandService.WorkDayCommandServiceBase
{
    private readonly NotificationService.NotificationServiceClient _notificationClient;

    public WorkDayCommandServiceImpl(NotificationService.NotificationServiceClient notificationClient)
    {
        _notificationClient = notificationClient;
    }

    public override async Task<CommandResponse> CreateWorkDay(CreateWorkDayCommand request, ServerCallContext context)
    {
        Console.WriteLine($"[CreateWorkDay] ID: {request.WorkDayId}, Date: {request.Date}");

        var notification = new WorkDayCreatedNotification
        {
            WorkDayId = request.WorkDayId,
            Date = request.Date
        };

        await _notificationClient.SendWorkDayCreatedAsync(notification);

        return new CommandResponse { Success = true };
    }
}