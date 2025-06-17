using Application.Contract.DTO;
using Domain.Entities;

namespace Application.Mapper;

public class TimerSettingsMapper : IDtoMapper<TimerSettingsDto, TimerSettings>
{
    public TimerSettings ToDomain(TimerSettingsDto dto)
    {
        return new TimerSettings
        {
            TimerSettingsId = dto.TimerSettingsId,
            DocumentationSaveInterval = dto.DocumentationSaveInterval,
            SnapshotSaveInterval = dto.SnapshotSaveInterval
        };
    }

    public TimerSettingsDto ToDto(TimerSettings domain)
    {
        return new TimerSettingsDto(domain.TimerSettingsId, domain.DocumentationSaveInterval,
            domain.SnapshotSaveInterval);
    }
}