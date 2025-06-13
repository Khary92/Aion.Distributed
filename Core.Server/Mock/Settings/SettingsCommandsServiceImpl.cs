using System;
using System.Threading.Tasks;
using Grpc.Core;
using Proto.Command.Settings;
using Proto.Notifications.Settings;

namespace Service.Server.Mock;

public class SettingsCommandServiceImpl(SettingsNotificationServiceImpl settingsNotificationService)
    : SettingsCommandService.SettingsCommandServiceBase
{
    public override async Task<CommandResponse> CreateSettings(CreateSettingsCommand request, ServerCallContext context)
    {
        Console.WriteLine($"[CreateSettings] ID: {request.SettingsId}, ExportPath: {request.ExportPath}, AddNewTicketsActive: {request.IsAddNewTicketsToCurrentSprintActive}");

        try
        {
            await settingsNotificationService.SendNotificationAsync(new SettingsNotification
            {
                SettingsCreated = new SettingsCreatedNotification
                {
                    SettingsId = request.SettingsId,
                    ExportPath = request.ExportPath,
                    IsAddNewTicketsToCurrentSprintActive = request.IsAddNewTicketsToCurrentSprintActive
                }
            });

            return new CommandResponse { Success = true };
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[Error] CreateSettings failed: {ex.Message}");
            return new CommandResponse { Success = false };
        }
    }

    public override async Task<CommandResponse> UpdateSettings(UpdateSettingsCommand request, ServerCallContext context)
    {
        Console.WriteLine($"[UpdateSettings] ID: {request.SettingsId}, ExportPath: {request.ExportPath}, AddNewTicketsActive: {request.IsAddNewTicketsToCurrentSprintActive}");

        try
        {
            await settingsNotificationService.SendNotificationAsync(new SettingsNotification
            {
                SettingsUpdated = new SettingsUpdatedNotification
                {
                    SettingsId = request.SettingsId,
                    ExportPath = request.ExportPath,
                    IsAddNewTicketsToCurrentSprintActive = request.IsAddNewTicketsToCurrentSprintActive
                }
            });

            return new CommandResponse { Success = true };
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[Error] UpdateSettings failed: {ex.Message}");
            return new CommandResponse { Success = false };
        }
    }
}
