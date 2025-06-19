using Domain.Events.AiSettings;
using Domain.Interfaces;
using Proto.DTO.AiSettings;
using Service.Server.Communication.Mapper;

namespace Service.Server.Old.Services.Entities.AiSettings;

public class AiSettingsRequestsService(
    IEventStore<AiSettingsEvent> aiSettingsEventsStore,
    IDtoMapper<AiSettingsProto, Domain.Entities.AiSettings> configMapper) : IAiSettingsRequestsService
{
    public async Task<AiSettingsProto?> Get()
    {
        var settingsList = (await aiSettingsEventsStore.GetAllEventsAsync())
            .GroupBy(e => e.EntityId)
            .OrderBy(e => e.Key)
            .Select(Domain.Entities.AiSettings.Rehydrate).ToList();

        return settingsList.Count != 0 ? configMapper.ToDto(settingsList[0]) : null;
    }
}