using Domain.Events.TimerSettings;
using Domain.Interfaces;

namespace Core.Server.Services.Entities.TimerSettings;

public class TimerSettingsRequestsService(IEventStore<TimerSettingsEvent> timerSettingsEventStore)
    : ITimerSettingsRequestsService
{
    public async Task<Domain.Entities.TimerSettings> Get()
    {
        var settingsList = (await timerSettingsEventStore.GetAllEventsAsync())
            .GroupBy(e => e.EntityId)
            .OrderBy(e => e.Key)
            .Select(Domain.Entities.TimerSettings.Rehydrate).ToList();

        //There is only one aggregate.
        return settingsList[0];
    }

    public async Task<bool> IsTimerSettingsExisting()
    {
        var events = await timerSettingsEventStore.GetAllEventsAsync();

        //There is only oen aggregate. No events means no that TimerSettings do not exist yet or were deleted.
        return events.Count != 0;
    }
}