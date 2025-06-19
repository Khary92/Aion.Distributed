using Proto.DTO.Settings;

namespace Service.Server.Communication.Mapper;

public class SettingsMapper : IDtoMapper<SettingsProto, Domain.Entities.Settings>
{
    public Domain.Entities.Settings ToDomain(SettingsProto dto)
    {
        return new Domain.Entities.Settings
        {
            SettingsId = Guid.Parse(dto.SettingsId),
            ExportPath = dto.ExportPath,
            IsAddNewTicketsToCurrentSprintActive = dto.IsAddNewTicketsToCurrentSprintActive
        };
    }

    public SettingsProto ToDto(Domain.Entities.Settings domain)
    {
        return new SettingsProto
        {
            SettingsId = domain.SettingsId.ToString(),
            ExportPath = domain.ExportPath,
            IsAddNewTicketsToCurrentSprintActive = domain.IsAddNewTicketsToCurrentSprintActive
        };
    }
}