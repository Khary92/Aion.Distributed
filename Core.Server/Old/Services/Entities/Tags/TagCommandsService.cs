using Application.Contract.CQRS.Commands.Entities.Tags;
using Application.Translators.Tags;
using Domain.Events.Tags;
using Domain.Interfaces;
using MediatR;

namespace Application.Services.Entities.Tags;

public class TagCommandsService(
    IEventStore<TagEvent> tagEventStore,
    IMediator mediator,
    ITagCommandsToEventTranslator eventTranslator,
    ITagCommandsToNotificationTranslator notificationTranslator) : ITagCommandsService
{
    public async Task Update(UpdateTagCommand updateTagCommand)
    {
        await tagEventStore.StoreEventAsync(eventTranslator.ToEvent(updateTagCommand));
        await mediator.Publish(notificationTranslator.ToNotification(updateTagCommand));
    }

    public async Task Create(CreateTagCommand createTagCommand)
    {
        await tagEventStore.StoreEventAsync(eventTranslator.ToEvent(createTagCommand));
        await mediator.Publish(notificationTranslator.ToNotification(createTagCommand));
    }
}