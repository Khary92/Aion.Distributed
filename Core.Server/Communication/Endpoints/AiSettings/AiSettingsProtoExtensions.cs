using Core.Server.Communication.Records.Commands.Entities.AiSettings;
using Proto.Command.AiSettings;
using Proto.DTO.AiSettings;
using Proto.Notifications.AiSettings;

namespace Core.Server.Communication.Endpoints.AiSettings;

public static class AiSettingsProtoExtensions
{
    public static CreateAiSettingsCommand ToCommand(this CreateAiSettingsCommandProto proto)
    {
        return new CreateAiSettingsCommand(Guid.Parse(proto.AiSettingsId), proto.Prompt, proto.LanguageModelPath);
    }

    public static AiSettingsNotification ToNotification(this CreateAiSettingsCommand proto)
    {
        return new AiSettingsNotification
        {
            AiSettingsCreated = new AiSettingsCreatedNotification
            {
                AiSettingsId = proto.AiSettingsId.ToString(),
                Prompt = proto.Prompt,
                LanguageModelPath = proto.LanguageModelPath
            }
        };
    }


    public static ChangeLanguageModelCommand ToCommand(this ChangeLanguageModelCommandProto proto)
    {
        return new ChangeLanguageModelCommand(Guid.Parse(proto.AiSettingsId), proto.LanguageModelPath);
    }

    public static AiSettingsNotification ToNotification(this ChangeLanguageModelCommand proto)
    {
        return new AiSettingsNotification
        {
            LanguageModelChanged = new LanguageModelChangedNotification
            {
                AiSettingsId = proto.AiSettingsId.ToString(),
                LanguageModelPath = proto.LanguageModelPath
            }
        };
    }

    public static ChangePromptCommand ToCommand(this ChangePromptCommandProto proto)
    {
        return new ChangePromptCommand(Guid.Parse(proto.AiSettingsId), proto.Prompt);
    }

    public static AiSettingsNotification ToNotification(this ChangePromptCommand proto)
    {
        return new AiSettingsNotification
        {
            PromptChanged = new PromptChangedNotification
            {
                AiSettingsId = proto.AiSettingsId.ToString(),
                Prompt = proto.Prompt
            }
        };
    }

    public static AiSettingsProto ToProto(this Domain.Entities.AiSettings domain)
    {
        return new AiSettingsProto
        {
            AiSettingsId = domain.AiSettingsId.ToString(),
            Prompt = domain.Prompt,
            LanguageModelPath = domain.LanguageModelPath
        };
    }
}