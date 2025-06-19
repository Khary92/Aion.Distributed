using Proto.Command.AiSettings;
using Proto.Notifications.AiSettings;

namespace Service.Server.CQRS.Commands.Entities.AiSettings;

public static class AiSettingsProtoExtensions
{
    public static CreateAiSettingsCommand ToCommand(this CreateAiSettingsCommandProto proto) =>
        new(Guid.Parse(proto.AiSettingsId), proto.Prompt, proto.LanguageModelPath);
    
    public static AiSettingsNotification ToNotification(this CreateAiSettingsCommandProto proto) =>
        new()
        {
            AiSettingsCreated = new AiSettingsCreatedNotification()
            {
                AiSettingsId = proto.AiSettingsId,
                Prompt = proto.Prompt,
                LanguageModelPath = proto.LanguageModelPath
            }
        };

    public static ChangeLanguageModelCommand ToCommand(this ChangeLanguageModelCommandProto proto) =>
        new(Guid.Parse(proto.AiSettingsId), proto.LanguageModelPath);

    public static AiSettingsNotification ToNotification(this ChangeLanguageModelCommandProto proto) =>
        new()
        {
            LanguageModelChanged = new LanguageModelChangedNotification
            {
                AiSettingsId = proto.AiSettingsId,
                LanguageModelPath = proto.LanguageModelPath
            }
        };
    
    public static ChangePromptCommand ToCommand(this ChangePromptCommandProto proto) =>
        new(Guid.Parse(proto.AiSettingsId), proto.Prompt);
    
    public static AiSettingsNotification ToNotification(this ChangePromptCommandProto proto) =>
        new()
        {
            PromptChanged = new PromptChangedNotification
            {
                AiSettingsId = proto.AiSettingsId,
                Prompt = proto.Prompt
            }
        };
}