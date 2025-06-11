using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Client.Avalonia.Communication.Commands;
using Client.Avalonia.Communication.Notifications.Tags;
using Client.Avalonia.Communication.Requests;
using CommunityToolkit.Mvvm.Messaging;
using Contract.DTO;
using Contract.Tracing;
using Contract.Tracing.Tracers;
using DynamicData;
using Proto.Command.Tags;
using Proto.Notifications.Tag;
using ReactiveUI;

namespace Client.Avalonia.Models.Data;

public class TagsDataModel(
    ICommandSender commandSender,
    IRequestSender requestSender,
    IMessenger messenger,
    ITracingCollectorProvider tracer) : ReactiveObject
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
            var parsedGuid = Guid.Parse(m.TagId);
            tracer.Tag.Update.NotificationReceived(GetType(), parsedGuid, m);

            var tag = Tags.FirstOrDefault(t => t.TagId == parsedGuid);

            if (tag == null)
            {
                tracer.Tag.Update.NoAggregateFound(GetType(), parsedGuid);
                return;
            }

            tag.Apply(m);
            tracer.Tag.Update.ChangesApplied(GetType(), parsedGuid);
        });
    }

    public async Task InitializeAsync()
    {
        Tags.Clear();
        Tags.AddRange(await requestSender.GetAllTags());
    }

    public async Task PersistTagAsync(string tagName, TagDto? selectedTag, bool isEditMode)
    {
        if (isEditMode && selectedTag != null)
        {
            selectedTag.Name = tagName;

            tracer.Tag.Update.StartUseCase(GetType(), selectedTag.TagId, selectedTag.AsTraceAttributes());

            var updateTagCommand = new UpdateTagCommand
            {
                TagId = selectedTag.TagId.ToString(),
                Name = tagName
            };

            await commandSender.Send(updateTagCommand);

            tracer.Tag.Update.CommandSent(GetType(), selectedTag.TagId, updateTagCommand);
            return;
        }

        var createTagDto = new TagDto(Guid.NewGuid(), tagName, false);

        tracer.Tag.Create.StartUseCase(GetType(), createTagDto.TagId, createTagDto.AsTraceAttributes());

        var createTagCommand = new CreateTagCommand
        {
            TagId = createTagDto.TagId.ToString(),
            Name = tagName
        };
        
        await commandSender.Send(createTagCommand);

        tracer.Tag.Create.CommandSent(GetType(), createTagDto.TagId, createTagCommand);
    }
}