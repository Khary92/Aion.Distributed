using Grpc.Core;
using Proto.Command.WorkDays;
using Proto.Notifications.WorkDay;

public class WorkDayCommandServiceImpl : WorkDayCommandService.WorkDayCommandServiceBase
{
    private readonly NotificationService.NotificationServiceClient _notificationClient;

    public WorkDayCommandServiceImpl(NotificationService.NotificationServiceClient notificationClient)
    {
        _notificationClient = notificationClient;
    }

    public override async Task<CommandResponse> CreateWorkDay(CreateWorkDayCommand request, ServerCallContext context)
    {
        // Hier würdest du dein Domain-Model persistieren oder speichern
        Console.WriteLine($"[CreateWorkDay] ID: {request.WorkDayId}, Date: {request.Date}");

        // Anschließend sende eine Notification
        var notification = new WorkDayCreatedNotification
        {
            WorkDayId = request.WorkDayId,
            Date = request.Date.ToDateTime().ToString("o") // ISO 8601
        };

        await _notificationClient.SendWorkDayCreatedAsync(notification);

        return new CommandResponse { Success = true };
    }
}