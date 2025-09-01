using Core.Server.Communication.Endpoints.WorkDay;
using Core.Server.Communication.Records.Commands.Entities.WorkDays;
using Core.Server.Tracing.Tracing.Tracers;
using Core.Server.Translators.Commands.WorkDays;
using Domain.Events.WorkDays;
using Domain.Interfaces;

namespace Core.Server.Services.Entities.WorkDays;

public class WorkDayCommandsService(
    IEventStore<WorkDayEvent> workDayEventStore,
    WorkDayNotificationService workDayNotificationService,
    IWorkDayCommandsToEventTranslator eventTranslator,
    IWorkDayRequestsService workDayRequestsService,
    ITraceCollector tracer)
    : IWorkDayCommandsService
{
    public async Task Create(CreateWorkDayCommand command)
    {
        var workDays = await workDayRequestsService.GetAll();

        if (workDays.Any(wd => wd.Date.Date == command.Date.Date))
        {
            await tracer.WorkDay.Create.ActionAborted(GetType(), command.TraceId);
            return;
        }

        await workDayEventStore.StoreEventAsync(eventTranslator.ToEvent(command));
        await tracer.WorkDay.Create.EventPersisted(GetType(), command.TraceId, command.ToNotification());
        await tracer.WorkDay.Create.SendingNotification(GetType(), command.TraceId, command.ToNotification());
        await workDayNotificationService.SendNotificationAsync(command.ToNotification());
    }
}