using Grpc.Core;
using Proto.Command.AiSettings;
using Proto.Notifications.AiSettings;

namespace Service.Server.Mock.AiSettings;

public class MockAiSettingsCommandService(AiSettingsNotificationServiceImpl aiSettingsNotificationService)
    : Proto.Command.AiSettings.AiSettingsCommandService.AiSettingsCommandServiceBase
{
    public override async Task<CommandResponse> SendChangeLanguageModel(ChangeLanguageModelCommand request, ServerCallContext context)
    {
        Console.WriteLine($"[ChangeLanguageModel] ID: {request.AiSettingsId}, ModelPath: {request.LanguageModelPath}");

        try
        {
            await aiSettingsNotificationService.SendNotificationAsync(new AiSettingsNotification
            {
                LanguageModelChanged = new LanguageModelChangedNotification
                {
                    AiSettingsId = request.AiSettingsId,
                    LanguageModelPath = request.LanguageModelPath
                }
            });

            return new CommandResponse { Success = true };
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[Error] ChangeLanguageModel failed: {ex.Message}");
            return new CommandResponse { Success = false };
        }
    }

    public override async Task<CommandResponse> SendChangePrompt(ChangePromptCommand request, ServerCallContext context)
    {
        Console.WriteLine($"[ChangePrompt] ID: {request.AiSettingsId}, Prompt: {request.Prompt}");

        try
        {
            await aiSettingsNotificationService.SendNotificationAsync(new AiSettingsNotification
            {
                PromptChanged = new PromptChangedNotification
                {
                    AiSettingsId = request.AiSettingsId,
                    Prompt = request.Prompt
                }
            });

            return new CommandResponse { Success = true };
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[Error] ChangePrompt failed: {ex.Message}");
            return new CommandResponse { Success = false };
        }
    }

    public override async Task<CommandResponse> SendCreateAiSettings(CreateAiSettingsCommand request, ServerCallContext context)
    {
        Console.WriteLine($"[CreateAiSettings] ID: {request.AiSettingsId}, Prompt: {request.Prompt}, ModelPath: {request.LanguageModelPath}");

        try
        {
            await aiSettingsNotificationService.SendNotificationAsync(new AiSettingsNotification
            {
                AiSettingsCreated = new AiSettingsCreatedNotification
                {
                    AiSettingsId = request.AiSettingsId,
                    Prompt = request.Prompt,
                    LanguageModelPath = request.LanguageModelPath
                }
            });

            return new CommandResponse { Success = true };
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[Error] CreateAiSettings failed: {ex.Message}");
            return new CommandResponse { Success = false };
        }
    }
}
