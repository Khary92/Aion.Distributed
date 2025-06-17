using Application.Contract.DTO;
using Application.Mapper;
using Domain.Events.TimerSettings;
using Domain.Interfaces;

namespace Application.Services.Entities.TimerSettings;

public class TimerSettingsRequestsService(
    IEventStore<TimerSettingsEvent> timerSettingsEventStore,
    IDtoMapper<TimerSettingsDto, Domain.Entities.TimerSettings> settingsMapper) : ITimerSettingsRequestsService
{
    public async Task<TimerSettingsDto?> Get()
    {
        var settingsList = (await timerSettingsEventStore.GetAllEventsAsync())
            .GroupBy(e => e.EntityId)
            .OrderBy(e => e.Key)
            .Select(Domain.Entities.TimerSettings.Rehydrate).ToList();

        return settingsList.Count != 0 ? settingsMapper.ToDto(settingsList[0]) : null;
    }
}