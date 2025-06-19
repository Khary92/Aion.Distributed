using Grpc.Core;
using Proto.Command.AiSettings;
using Service.Server.CQRS.Commands.Entities.AiSettings;
using Service.Server.Old.Services.Entities.AiSettings;

namespace Service.Server.Communication.AiSettings;

public class AiSettingsCommandsReceiver(
    AiSettingsNotificationServiceImpl aiSettingsNotificationService,
    IAiSettingsCommandsService aiSettingsCommandsService)
    : AiSettingsCommandProtoService.AiSettingsCommandProtoServiceBase
{
    public override async Task<CommandResponse> SendChangeLanguageModel(ChangeLanguageModelCommandProto request,
        ServerCallContext context)
    {
        Console.WriteLine($"[ChangeLanguageModel] ID: {request.AiSettingsId}, ModelPath: {request.LanguageModelPath}");
        await aiSettingsCommandsService.ChangeLanguageModelPath(request.ToCommand());

        try
        {
            await aiSettingsNotificationService.SendNotificationAsync(request.ToNotification());
            return new CommandResponse { Success = true };
        }
        catch (Exception ex)
        {
            await Console.Error.WriteLineAsync($"[Error] ChangeLanguageModel failed: {ex.Message}");
            return new CommandResponse { Success = false };
        }
    }

    public override async Task<CommandResponse> SendChangePrompt(ChangePromptCommandProto request,
        ServerCallContext context)
    {
        Console.WriteLine($"[ChangePrompt] ID: {request.AiSettingsId}, Prompt: {request.Prompt}");
        await aiSettingsCommandsService.ChangePrompt(request.ToCommand());

        try
        {
            await aiSettingsNotificationService.SendNotificationAsync(request.ToNotification());
            return new CommandResponse { Success = true };
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[Error] ChangePrompt failed: {ex.Message}");
            return new CommandResponse { Success = false };
        }
    }

    public override async Task<CommandResponse> SendCreateAiSettings(CreateAiSettingsCommandProto request,
        ServerCallContext context)
    {
        Console.WriteLine(
            $"[CreateAiSettings] ID: {request.AiSettingsId}, Prompt: {request.Prompt}, ModelPath: {request.LanguageModelPath}");
        await aiSettingsCommandsService.Create(request.ToCommand());

        try
        {
            await aiSettingsNotificationService.SendNotificationAsync(request.ToNotification());
            return new CommandResponse { Success = true };
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[Error] CreateAiSettings failed: {ex.Message}");
            return new CommandResponse { Success = false };
        }
    }
}