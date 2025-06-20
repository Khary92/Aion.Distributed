using Proto.Command.AiSettings;
using Proto.DTO.AiSettings;
using Proto.Notifications.AiSettings;
using Service.Server.CQRS.Commands.Entities.AiSettings;

namespace Service.Server.Communication.AiSettings;

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

    public static Domain.Entities.AiSettings ToDomain(this AiSettingsProto dto)
    {
        return new Domain.Entities.AiSettings
        {
            AiSettingsId = Guid.Parse(dto.AiSettingsId),
            Prompt = dto.Prompt,
            LanguageModelPath = dto.LanguageModelPath
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