using Domain.Events.Settings;
using Domain.Interfaces;
using Service.Server.Communication.Mapper;

namespace Service.Server.Old.Services.Entities.Settings;

public class SettingsRequestsService(
    IEventStore<SettingsEvent> settingsEventsStore)
    : ISettingsRequestsService
{
    public async Task<Domain.Entities.Settings> Get()
    {
        var settingsList = (await settingsEventsStore.GetAllEventsAsync())
            .GroupBy(e => e.EntityId)
            .OrderBy(e => e.Key)
            .Select(Domain.Entities.Settings.Rehydrate).ToList();

        //There is only one settings aggregate. 
        return settingsList[0];
    }

    public async Task<bool> IsSettingsExisting()
    {
        var events = await settingsEventsStore.GetAllEventsAsync();
        
        //There is only one settings aggregate. If there are no events, then there is no aggregate.
        return events.Count != 0;
    }
}