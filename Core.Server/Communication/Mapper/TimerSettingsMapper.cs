using Proto.DTO.TimerSettings;

namespace Service.Server.Communication.Mapper;

public class TimerSettingsMapper : IDtoMapper<TimerSettingsProto, Domain.Entities.TimerSettings>
{
    public Domain.Entities.TimerSettings ToDomain(TimerSettingsProto dto)
    {
        return new Domain.Entities.TimerSettings
        {
            TimerSettingsId = Guid.Parse(dto.TimerSettingsId),
            DocumentationSaveInterval = dto.DocumentationSaveInterval,
            SnapshotSaveInterval = dto.SnapshotSaveInterval
        };
    }

    public TimerSettingsProto ToDto(Domain.Entities.TimerSettings domain)
    {
        return new TimerSettingsProto
        {
            TimerSettingsId = domain.TimerSettingsId.ToString(),
            DocumentationSaveInterval = domain.DocumentationSaveInterval,
            SnapshotSaveInterval = domain.SnapshotSaveInterval
        };
    }
}