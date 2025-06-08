using Contract.CQRS.Commands.Entities.AiSettings;
using Proto.Command.AiSettings;

namespace Contract.Proto.Converter;

public static class AiSettingsCommandConverter
{
    public static ChangeLanguageModelProtoCommand ToProto(this ChangeLanguageModelCommand command)
    {
        return new ChangeLanguageModelProtoCommand
        {
            AiSettingsId = command.AiSettingsId.ToString(),
            LanguageModelPath = command.LanguageModelPath
        };
    }

    public static ChangePromptProtoCommand ToProto(this ChangePromptCommand command)
    {
        return new ChangePromptProtoCommand
        {
            AiSettingsId = command.AiSettingsId.ToString(),
            Prompt = command.Prompt
        };
    }

    public static CreateAiSettingsProtoCommand ToProto(this CreateAiSettingsCommand command)
    {
        return new CreateAiSettingsProtoCommand
        {
            AiSettingsId = command.AiSettingsId.ToString(),
            Prompt = command.Prompt,
            LanguageModelPath = command.LanguageModelPath
        };
    }

    public static ChangeLanguageModelCommand ToDomain(this ChangeLanguageModelProtoCommand proto)
    {
        return new ChangeLanguageModelCommand(
            Guid.Parse(proto.AiSettingsId),
            proto.LanguageModelPath
        );
    }

    public static ChangePromptCommand ToDomain(this ChangePromptProtoCommand proto)
    {
        return new ChangePromptCommand(
            Guid.Parse(proto.AiSettingsId),
            proto.Prompt
        );
    }

    public static CreateAiSettingsCommand ToDomain(this CreateAiSettingsProtoCommand proto)
    {
        return new CreateAiSettingsCommand(
            Guid.Parse(proto.AiSettingsId),
            proto.Prompt,
            proto.LanguageModelPath
        );
    }
}