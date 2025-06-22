using Core.Server.Services.Entities.AiSettings;
using Grpc.Core;
using Proto.Command.AiSettings;

namespace Core.Server.Communication.Endpoints.AiSettings;

public class AiSettingsCommandReceiver(
    IAiSettingsCommandsService aiSettingsCommandsService)
    : AiSettingsCommandProtoService.AiSettingsCommandProtoServiceBase
{
    public override async Task<CommandResponse> SendChangeLanguageModel(ChangeLanguageModelCommandProto request,
        ServerCallContext context)
    {
        Console.WriteLine($"[ChangeLanguageModel] ID: {request.AiSettingsId}, ModelPath: {request.LanguageModelPath}");
        await aiSettingsCommandsService.ChangeLanguageModelPath(request.ToCommand());
        return new CommandResponse { Success = true };
    }

    public override async Task<CommandResponse> SendChangePrompt(ChangePromptCommandProto request,
        ServerCallContext context)
    {
        Console.WriteLine($"[ChangePrompt] ID: {request.AiSettingsId}, Prompt: {request.Prompt}");
        await aiSettingsCommandsService.ChangePrompt(request.ToCommand());
        return new CommandResponse { Success = true };
    }

    public override async Task<CommandResponse> SendCreateAiSettings(CreateAiSettingsCommandProto request,
        ServerCallContext context)
    {
        Console.WriteLine(
            $"[CreateAiSettings] ID: {request.AiSettingsId}, Prompt: {request.Prompt}, ModelPath: {request.LanguageModelPath}");
        await aiSettingsCommandsService.Create(request.ToCommand());
        return new CommandResponse { Success = true };
    }
}