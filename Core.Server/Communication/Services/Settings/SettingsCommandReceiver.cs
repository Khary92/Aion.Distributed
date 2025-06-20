using Grpc.Core;
using Proto.Command.Settings;
using Service.Server.Services.Entities.Settings;

namespace Service.Server.Communication.Services.Settings;

public class SettingsCommandReceiver(ISettingsCommandsService settingsCommandsService)
    : SettingsCommandProtoService.SettingsCommandProtoServiceBase
{
    public override async Task<CommandResponse> CreateSettings(CreateSettingsCommandProto request,
        ServerCallContext context)
    {
        Console.WriteLine(
            $"[CreateSettings] ID: {request.SettingsId}, ExportPath: {request.ExportPath}, AddNewTicketsActive: {request.IsAddNewTicketsToCurrentSprintActive}");

        await settingsCommandsService.Create(request.ToCommand());
        return new CommandResponse { Success = true };
    }

    public override async Task<CommandResponse> ChangeExportPath(ChangeExportPathCommandProto request,
        ServerCallContext context)
    {
        Console.WriteLine($"[UpdateSettings] ID: {request.SettingsId}, ExportPath: {request.ExportPath}");

        await settingsCommandsService.ChangeExportPath(request.ToCommand());
        return new CommandResponse { Success = true };
    }

    public override async Task<CommandResponse> ChangeAutomaticTicketAdding(
        ChangeAutomaticTicketAddingToSprintCommandProto request, ServerCallContext context)
    {
        Console.WriteLine(
            $"[UpdateSettings] ID: {request.SettingsId}, AddNewTicketsActive: {request.IsAddNewTicketsToCurrentSprintActive}");

        await settingsCommandsService.ChangeAutomaticTicketAddingToSprint(request.ToCommand());
        return new CommandResponse { Success = true };
    }
}