using Domain.Events.WorkDays;
using Domain.Interfaces;
using Service.Server.Communication.CQRS.Commands.Entities.WorkDays;
using Service.Server.Communication.Services.WorkDay;
using Service.Server.Translators.WorkDays;

namespace Service.Server.Services.Entities.WorkDays;

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