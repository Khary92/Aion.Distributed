using Grpc.Core;
using Proto.Command.TimerSettings;
using Proto.Notifications.TimerSettings;

namespace Service.Server.Mock.TimerSettings;

public class MockTimerSettingsCommandService(TimerSettingsNotificationServiceImpl notificationService)
    : TimerSettingsCommandService.TimerSettingsCommandServiceBase
{
    public override async Task<CommandResponse> CreateTimerSettings(CreateTimerSettingsCommand request, ServerCallContext context)
    {
        Console.WriteLine($"[CreateTimerSettings] ID: {request.TimerSettingsId}, DocuInterval: {request.DocumentationSaveInterval}, SnapshotInterval: {request.SnapshotSaveInterval}");

        try
        {
            await notificationService.SendNotificationAsync(new TimerSettingsNotification
            {
                TimerSettingsCreated = new TimerSettingsCreatedNotification
                {
                    TimerSettingsId = request.TimerSettingsId,
                    DocumentationSaveInterval = request.DocumentationSaveInterval,
                    SnapshotSaveInterval = request.SnapshotSaveInterval
                }
            });

            return new CommandResponse { Success = true };
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[Error] CreateTimerSettings failed: {ex.Message}");
            return new CommandResponse { Success = false };
        }
    }

    public override async Task<CommandResponse> ChangeDocuTimerSaveInterval(ChangeDocuTimerSaveIntervalCommand request, ServerCallContext context)
    {
        Console.WriteLine($"[ChangeDocuTimerSaveInterval] ID: {request.TimerSettingsId}, NewInterval: {request.DocuTimerSaveInterval}");

        try
        {
            await notificationService.SendNotificationAsync(new TimerSettingsNotification
            {
                DocuTimerSaveIntervalChanged = new DocuTimerSaveIntervalChangedNotification
                {
                    TimerSettingsId = request.TimerSettingsId,
                    DocuTimerSaveInterval = request.DocuTimerSaveInterval
                }
            });

            return new CommandResponse { Success = true };
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[Error] ChangeDocuTimerSaveInterval failed: {ex.Message}");
            return new CommandResponse { Success = false };
        }
    }

    public override async Task<CommandResponse> ChangeSnapshotSaveInterval(ChangeSnapshotSaveIntervalCommand request, ServerCallContext context)
    {
        Console.WriteLine($"[ChangeSnapshotSaveInterval] ID: {request.TimerSettingsId}, NewInterval: {request.SnapshotSaveInterval}");

        try
        {
            await notificationService.SendNotificationAsync(new TimerSettingsNotification
            {
                SnapshotSaveIntervalChanged = new SnapshotSaveIntervalChangedNotification
                {
                    TimerSettingsId = request.TimerSettingsId,
                    SnapshotSaveInterval = request.SnapshotSaveInterval
                }
            });

            return new CommandResponse { Success = true };
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[Error] ChangeSnapshotSaveInterval failed: {ex.Message}");
            return new CommandResponse { Success = false };
        }
    }
}
