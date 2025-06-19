using Proto.DTO.AiSettings;

namespace Service.Server.Communication.Mapper;

public class AiSettingsMapper : IDtoMapper<AiSettingsProto, Domain.Entities.AiSettings>
{
    public Domain.Entities.AiSettings ToDomain(AiSettingsProto dto)
    {
        return new Domain.Entities.AiSettings
        {
            AiSettingsId = Guid.Parse(dto.AiSettingsId),
            Prompt = dto.Prompt,
            LanguageModelPath = dto.LanguageModelPath
        };
    }

    public AiSettingsProto ToDto(Domain.Entities.AiSettings domain)
    {
        return new AiSettingsProto
        {
            AiSettingsId = domain.AiSettingsId.ToString(),
            Prompt = domain.Prompt,
            LanguageModelPath = domain.LanguageModelPath
        };
    }
}