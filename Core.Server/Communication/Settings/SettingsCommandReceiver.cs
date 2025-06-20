using Grpc.Core;
using Proto.Command.Settings;
using Service.Server.Old.Services.Entities.Settings;

namespace Service.Server.Communication.Settings;

public class SettingsCommandReceiver(
    SettingsNotificationServiceImpl settingsNotificationService,
    ISettingsCommandsService settingsCommandsService)
    : SettingsCommandProtoService.SettingsCommandProtoServiceBase
{
    public override async Task<CommandResponse> CreateSettings(CreateSettingsCommandProto request,
        ServerCallContext context)
    {
        Console.WriteLine(
            $"[CreateSettings] ID: {request.SettingsId}, ExportPath: {request.ExportPath}, AddNewTicketsActive: {request.IsAddNewTicketsToCurrentSprintActive}");

        await settingsCommandsService.Create(request.ToCommand());
        
        try
        {
            await settingsNotificationService.SendNotificationAsync(request.ToNotification());
            return new CommandResponse { Success = true };
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[Error] CreateSettings failed: {ex.Message}");
            return new CommandResponse { Success = false };
        }
    }

    public override async Task<CommandResponse> ChangeExportPath(ChangeExportPathCommandProto request,
        ServerCallContext context)
    {
        Console.WriteLine($"[UpdateSettings] ID: {request.SettingsId}, ExportPath: {request.ExportPath}");

        await settingsCommandsService.ChangeExportPath(request.ToCommand());
        
        try
        {
            await settingsNotificationService.SendNotificationAsync(request.ToNotification());
            return new CommandResponse { Success = true };
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[Error] ExportPathChanged failed: {ex.Message}");
            return new CommandResponse { Success = false };
        }
    }

    public override async Task<CommandResponse> ChangeAutomaticTicketAdding(
        ChangeAutomaticTicketAddingToSprintCommandProto request, ServerCallContext context)
    {
        Console.WriteLine(
            $"[UpdateSettings] ID: {request.SettingsId}, AddNewTicketsActive: {request.IsAddNewTicketsToCurrentSprintActive}");

        await settingsCommandsService.ChangeAutomaticTicketAddingToSprint(request.ToCommand());

        try
        {
            await settingsNotificationService.SendNotificationAsync(request.ToNotification());
            return new CommandResponse { Success = true };
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[Error] ChangeAutomaticTicketAdding failed: {ex.Message}");
            return new CommandResponse { Success = false };
        }
    }
}