using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Client.Avalonia.Communication.NotificationProcessors.Messages;
using CommunityToolkit.Mvvm.Messaging;
using Contract.CQRS.Notifications.Entities.Tags;
using Contract.DTO;
using Contract.Tracing;
using Contract.Tracing.Tracers;
using MediatR;
using ReactiveUI;

namespace Client.Avalonia.ViewModels.Data;

public class TagsDataModel(IMediator mediator, IMessenger messenger, ITracingCollectorProvider tracer) : ReactiveObject
{
    public ObservableCollection<TagDto> Tags { get; } = [];

    public void RegisterMessenger()
    {
        messenger.Register<NewTagMessage>(this, (_, m) =>
        {
            tracer.Tag.Create.AggregateReceived(GetType(), m.Tag.TagId, m.Tag.AsTraceAttributes());
            Tags.Add(m.Tag);
            tracer.Tag.Create.AggregateAdded(GetType(), m.Tag.TagId);
        });

        messenger.Register<TagUpdatedNotification>(this, (_, m) =>
        {
            tracer.Tag.Update.NotificationReceived(GetType(), m.TagId, m);

            var tag = Tags.FirstOrDefault(t => t.TagId == m.TagId);

            if (tag == null)
            {
                tracer.Tag.Update.NoAggregateFound(GetType(), m.TagId);
                return;
            }

            tag.Apply(m);
            tracer.Tag.Update.ChangesApplied(GetType(), m.TagId);
        });
    }

    public async Task InitializeAsync()
    {
        Tags.Clear();
        Tags.AddRange(await mediator.Send(new GetAllTagsRequest()));
    }

    public async Task PersistTagAsync(string tagName, TagDto? selectedTag, bool isEditMode)
    {
        if (isEditMode && selectedTag != null)
        {
            selectedTag.Name = tagName;

            tracer.Tag.Update.StartUseCase(GetType(), selectedTag.TagId, selectedTag.AsTraceAttributes());

            var updateTagCommand = new UpdateTagCommand(selectedTag.TagId, tagName);
            await mediator.Send(updateTagCommand);

            tracer.Tag.Update.CommandSent(GetType(), selectedTag.TagId, updateTagCommand);
            return;
        }

        var createTagDto = new TagDto(Guid.NewGuid(), tagName, false);

        tracer.Tag.Create.StartUseCase(GetType(), createTagDto.TagId, createTagDto.AsTraceAttributes());

        var createTagCommand = new CreateTagCommand(Guid.NewGuid(), tagName);
        await mediator.Send(createTagCommand);

        tracer.Tag.Create.CommandSent(GetType(), createTagDto.TagId, createTagCommand);
    }
}