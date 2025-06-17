using Application.Contract.DTO;
using Application.Mapper;
using Domain.Events.AiSettings;
using Domain.Interfaces;

namespace Application.Services.Entities.AiSettings;

public class AiSettingsRequestsService(
    IEventStore<AiSettingsEvent> aiSettingsEventsStore,
    IDtoMapper<AiSettingsDto, Domain.Entities.AiSettings> configMapper) : IAiSettingsRequestsService
{
    public async Task<AiSettingsDto?> Get()
    {
        var settingsList = (await aiSettingsEventsStore.GetAllEventsAsync())
            .GroupBy(e => e.EntityId)
            .OrderBy(e => e.Key)
            .Select(Domain.Entities.AiSettings.Rehydrate).ToList();

        return settingsList.Count != 0 ? configMapper.ToDto(settingsList[0]) : null;
    }
}