using Application.Contract.CQRS.Commands.Entities.WorkDays;
using Application.Translators.WorkDays;
using Domain.Events.WorkDays;
using Domain.Interfaces;
using MediatR;

namespace Application.Services.Entities.WorkDays;

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