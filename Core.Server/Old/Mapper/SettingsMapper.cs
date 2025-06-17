using Application.Contract.DTO;
using Domain.Entities;

namespace Application.Mapper;

public class SettingsMapper : IDtoMapper<SettingsDto, Settings>
{
    public Settings ToDomain(SettingsDto dto)
    {
        return new Settings
        {
            SettingsId = dto.SettingsId,
            ExportPath = dto.ExportPath,
            IsAddNewTicketsToCurrentSprintActive = dto.IsAddNewTicketsToCurrentSprintActive
        };
    }

    public SettingsDto ToDto(Settings domain)
    {
        return new SettingsDto(domain.SettingsId, domain.ExportPath, domain.IsAddNewTicketsToCurrentSprintActive);
    }
}