using Application.Contract.DTO;
using Domain.Entities;

namespace Application.Mapper;

public class AiSettingsMapper : IDtoMapper<AiSettingsDto, AiSettings>
{
    public AiSettings ToDomain(AiSettingsDto dto)
    {
        return new AiSettings
        {
            AiSettingsId = dto.AiSettingsId,
            Prompt = dto.Prompt,
            LanguageModelPath = dto.LanguageModelPath
        };
    }

    public AiSettingsDto ToDto(AiSettings domain)
    {
        return new AiSettingsDto(domain.AiSettingsId, domain.LanguageModelPath, domain.Prompt);
    }
}