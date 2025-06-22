using Core.Server.Communication.Endpoints.WorkDay;
using Core.Server.Communication.Records.Commands.Entities.WorkDays;
using Core.Server.Translators.Commands.WorkDays;
using Domain.Events.WorkDays;
using Domain.Interfaces;

namespace Core.Server.Services.Entities.WorkDays;

public class WorkDayCommandsService(
    IEventStore<WorkDayEvent> workDayEventStore,
    WorkDayNotificationService workDayNotificationService,
    IWorkDayCommandsToEventTranslator eventTranslator,
    IWorkDayRequestsService workDayRequestsService)
    : IWorkDayCommandsService
{
    public async Task Create(CreateWorkDayCommand createWorkDayCommand)
    {
        var workDays = await workDayRequestsService.GetAll();

        if (workDays.Any(wd => wd.Date.Date == createWorkDayCommand.Date.Date)) return;

        await workDayEventStore.StoreEventAsync(eventTranslator.ToEvent(createWorkDayCommand));
        await workDayNotificationService.SendNotificationAsync(createWorkDayCommand.ToNotification());
    }
}