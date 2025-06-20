using Grpc.Core;
using Proto.Command.Settings;
using Proto.Notifications.Settings;
using Service.Server.Communication.Settings;
using SettingsNotificationService = Service.Server.Communication.Settings.SettingsNotificationService;

namespace Service.Server.Communication.Mock.Settings;

public class MockSettingsCommandService(SettingsNotificationService settingsNotificationService)
    : SettingsCommandProtoService.SettingsCommandProtoServiceBase
{
    public override async Task<CommandResponse> CreateSettings(CreateSettingsCommandProto request, ServerCallContext context)
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

    public override async Task<CommandResponse> ChangeExportPath(ChangeExportPathCommandProto request, ServerCallContext context)
    {
        Console.WriteLine($"[UpdateSettings] ID: {request.SettingsId}, ExportPath: {request.ExportPath}");

        try
        {
            await settingsNotificationService.SendNotificationAsync(new SettingsNotification
            {
                ExportPathChanged = new ExportPathChangedNotification()
                {
                    SettingsId = request.SettingsId,
                    ExportPath = request.ExportPath,
                }
            });

            return new CommandResponse { Success = true };
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[Error] ExportPathChanged failed: {ex.Message}");
            return new CommandResponse { Success = false };
        }
    }
    
    public override async Task<CommandResponse> ChangeAutomaticTicketAdding(ChangeAutomaticTicketAddingToSprintCommandProto request, ServerCallContext context)
    {
        Console.WriteLine($"[UpdateSettings] ID: {request.SettingsId}, AddNewTicketsActive: {request.IsAddNewTicketsToCurrentSprintActive}");

        try
        {
            await settingsNotificationService.SendNotificationAsync(new SettingsNotification
            {
                AutomaticTicketAddingChanged = new AutomaticTicketAddingToSprintChangedNotification()
                {
                    SettingsId = request.SettingsId,
                    IsAddNewTicketsToCurrentSprintActive = request.IsAddNewTicketsToCurrentSprintActive
                }
            });

            return new CommandResponse { Success = true };
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[Error] ChangeAutomaticTicketAdding failed: {ex.Message}");
            return new CommandResponse { Success = false };
        }
    }
}
