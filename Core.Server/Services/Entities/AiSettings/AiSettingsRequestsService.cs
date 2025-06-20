using Domain.Events.AiSettings;
using Domain.Interfaces;
using Proto.DTO.AiSettings;
using Service.Server.Communication.Mapper;

namespace Service.Server.Old.Services.Entities.AiSettings;

public class AiSettingsRequestsService(
    IEventStore<AiSettingsEvent> aiSettingsEventsStore) : IAiSettingsRequestsService
{
    public async Task<Domain.Entities.AiSettings> Get()
    {
        var settingsList = (await aiSettingsEventsStore.GetAllEventsAsync())
            .GroupBy(e => e.EntityId)
            .OrderBy(e => e.Key)
            .Select(Domain.Entities.AiSettings.Rehydrate).ToList();

        // There is only a single settings entity.
        return settingsList[0];
    }

    public async Task<bool> IsAiSettingsExisting()
    {
        var events = await aiSettingsEventsStore.GetAllEventsAsync();

        // If there are no events. There is no projection as well.
        return events.Count != 0;
    }
}