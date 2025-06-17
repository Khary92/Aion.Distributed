using Application.Contract.DTO;
using Application.Mapper;
using Domain.Events.Settings;
using Domain.Interfaces;

namespace Application.Services.Entities.Settings;

public class SettingsRequestsService(
    IEventStore<SettingsEvent> settingsEventsStore,
    IDtoMapper<SettingsDto, Domain.Entities.Settings> settingsMapper)
    : ISettingsRequestsService
{
    public async Task<SettingsDto?> Get()
    {
        var settingsList = (await settingsEventsStore.GetAllEventsAsync())
            .GroupBy(e => e.EntityId)
            .OrderBy(e => e.Key)
            .Select(Domain.Entities.Settings.Rehydrate).ToList();

        return settingsList.Count != 0 ? settingsMapper.ToDto(settingsList[0]) : null;
    }
}