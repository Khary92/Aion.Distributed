using Grpc.Core;
using Proto.Command.TimerSettings;
using Service.Server.Old.Services.Entities.TimerSettings;

namespace Service.Server.Communication.TimerSettings;

public class TimerSettingsCommandReceiver(ITimerSettingsCommandsService timerSettingsCommandsService)
    : TimerSettingsCommandProtoService.TimerSettingsCommandProtoServiceBase
{
    public override async Task<CommandResponse> CreateTimerSettings(CreateTimerSettingsCommandProto request,
        ServerCallContext context)
    {
        Console.WriteLine(
            $"[CreateTimerSettings] ID: {request.TimerSettingsId}, DocuInterval: {request.DocumentationSaveInterval}, SnapshotInterval: {request.SnapshotSaveInterval}");

        await timerSettingsCommandsService.Create(request.ToCommand());
        return new CommandResponse { Success = true };
    }

    public override async Task<CommandResponse> ChangeDocuTimerSaveInterval(
        ChangeDocuTimerSaveIntervalCommandProto request, ServerCallContext context)
    {
        Console.WriteLine(
            $"[ChangeDocuTimerSaveInterval] ID: {request.TimerSettingsId}, NewInterval: {request.DocuTimerSaveInterval}");

        await timerSettingsCommandsService.ChangeDocumentationInterval(request.ToCommand());
        return new CommandResponse { Success = true };
    }

    public override async Task<CommandResponse> ChangeSnapshotSaveInterval(
        ChangeSnapshotSaveIntervalCommandProto request, ServerCallContext context)
    {
        Console.WriteLine(
            $"[ChangeSnapshotSaveInterval] ID: {request.TimerSettingsId}, NewInterval: {request.SnapshotSaveInterval}");

        await timerSettingsCommandsService.ChangeSnapshotInterval(request.ToCommand());
        return new CommandResponse { Success = true };
    }
}