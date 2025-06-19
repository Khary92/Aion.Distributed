using Domain.Events.WorkDays;
using Domain.Interfaces;
using Service.Server.CQRS.Commands.Entities.WorkDays;
using Service.Server.Old.Translators.WorkDays;

namespace Service.Server.Old.Services.Entities.WorkDays;

public class WorkDayCommandsService(
    IEventStore<WorkDayEvent> workDayEventStore,
    IMediator mediator,
    IWorkDayCommandsToEventTranslator eventTranslator,
    IWorkDayCommandsToNotificationTranslator notificationTranslator)
    : IWorkDayCommandsService
{
    public async Task Create(CreateWorkDayCommand createWorkDayCommand)
    {
        await workDayEventStore.StoreEventAsync(eventTranslator.ToEvent(createWorkDayCommand));
        await mediator.Publish(notificationTranslator.ToNotification(createWorkDayCommand));
    }
}