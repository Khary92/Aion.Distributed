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
    public async Task Create(CreateWorkDayCommand command)
    {
        var workDays = await workDayRequestsService.GetAll();

        //TODO trace this
        if (workDays.Any(wd => wd.Date.Date == command.Date.Date)) return;

        await workDayEventStore.StoreEventAsync(eventTranslator.ToEvent(command), command.TraceId);
        await workDayNotificationService.SendNotificationAsync(command.ToNotification());
    }
}