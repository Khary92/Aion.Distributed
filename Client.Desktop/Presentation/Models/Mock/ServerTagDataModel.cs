using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using Client.Desktop.Communication.Mock;
using Client.Desktop.Communication.Notifications.Tag.Receiver;
using Client.Desktop.Communication.Notifications.Tag.Records;
using Client.Desktop.Communication.Notifications.Wrappers;
using Client.Desktop.DataModels;
using Client.Desktop.Lifecycle.Startup.Tasks.Initialize;
using Client.Desktop.Lifecycle.Startup.Tasks.Streams;
using Proto.Command.Tags;
using Proto.DTO.Tag;
using Proto.DTO.TraceData;
using Proto.Requests.Tags;
using ReactiveUI;
using Service.Proto.Shared.Commands.Tags;
using Service.Proto.Shared.Requests.Tags;

namespace Client.Desktop.Presentation.Models.Mock;

public class ServerTagDataModel(MockDataService mockDataService) : ReactiveObject, IInitializeAsync, ITagCommandSender,
    ITagRequestSender, ILocalTagNotificationPublisher, IStreamClient
{
    private readonly ConcurrentQueue<CreateTagCommandProto> _createQueue = new();

    private readonly TimeSpan _responseDelay = TimeSpan.FromMilliseconds(50);
    private readonly ConcurrentQueue<UpdateTagCommandProto> _updateQueue = new();
    private ObservableCollection<TagClientModel> _tags = [];

    public ObservableCollection<TagClientModel> Tags
    {
        get => _tags;
        set => this.RaiseAndSetIfChanged(ref _tags, value);
    }

    public InitializationType Type => InitializationType.MockModels;

    public Task InitializeAsync()
    {
        Tags.Clear();
        Tags = new ObservableCollection<TagClientModel>(mockDataService.Tags);
        return Task.CompletedTask;
    }

    public event Func<ClientTagUpdatedNotification, Task>? ClientTagUpdatedNotificationReceived;
    public event Func<NewTagMessage, Task>? NewTagMessageNotificationReceived;

    public async Task Publish(NewTagMessage message)
    {
        if (NewTagMessageNotificationReceived is { } handler)
            await handler(message);
    }

    public async Task Publish(ClientTagUpdatedNotification notification)
    {
        if (ClientTagUpdatedNotificationReceived is { } handler)
            await handler(notification);
    }

    public async Task StartListening(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            while (_updateQueue.TryDequeue(out var updateCmd))
            {
                var notification = new ClientTagUpdatedNotification(
                    Guid.Parse(updateCmd.TagId),
                    updateCmd.Name,
                    Guid.Parse(updateCmd.TraceData.TraceId)
                );

                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    var ticket = Tags.FirstOrDefault(t => t.TagId == Guid.Parse(updateCmd.TagId));
                    ticket?.Apply(notification);
                });

                await Task.Delay(_responseDelay, cancellationToken);
                await Publish(notification);
            }

            while (_createQueue.TryDequeue(out var createCmd))
            {
                var notification =
                    new NewTagMessage(new TagClientModel(Guid.Parse(createCmd.TagId), createCmd.Name, false),
                        Guid.Parse(createCmd.TraceData.TraceId));

                await Dispatcher.UIThread.InvokeAsync(() => { Tags.Add(notification.Tag); });

                await Task.Delay(_responseDelay, cancellationToken);
                await Publish(notification);
            }

            if (_updateQueue.IsEmpty && _createQueue.IsEmpty)
                await Task.Delay(50, cancellationToken);
        }
    }


    public Task<bool> Send(CreateTagCommandProto command)
    {
        _createQueue.Enqueue(command);
        return Task.FromResult(true);
    }

    public Task<bool> Send(UpdateTagCommandProto command)
    {
        _updateQueue.Enqueue(command);
        return Task.FromResult(true);
    }

    public Task<TagListProto> Send(GetAllTagsRequestProto request)
    {
        var list = new TagListProto();

        foreach (var tag in Tags)
            list.Tags.Add(new TagProto
            {
                TagId = tag.TagId.ToString(),
                Name = tag.Name,
                IsSelected = tag.IsSelected
            });

        return Task.FromResult(list);
    }

    public Task<TagProto> Send(GetTagByIdRequestProto request)
    {
        var tag = Tags.FirstOrDefault(t => t.TagId == Guid.Parse(request.TagId));

        if (tag == null) return Task.FromResult<TagProto>(null!);

        var result = new TagProto
        {
            TagId = tag.TagId.ToString(),
            Name = tag.Name,
            IsSelected = tag.IsSelected
        };

        return Task.FromResult(result);
    }

    public Task<TagListProto> Send(GetTagsByIdsRequestProto request)
    {
        var list = new TagListProto();

        foreach (var tag in Tags)
        {
            if (!request.TagIds.Contains(tag.TagId.ToString())) continue;

            list.Tags.Add(new TagProto
            {
                TagId = tag.TagId.ToString(),
                Name = tag.Name,
                IsSelected = tag.IsSelected
            });
        }

        return Task.FromResult(list);
    }

    public async Task PersistTagAsync(string tagName, TagClientModel? selectedTag, bool isEditMode)
    {
        if (isEditMode && selectedTag != null)
        {
            var updateCommand = new UpdateTagCommandProto
            {
                TagId = selectedTag.TagId.ToString(),
                Name = tagName,
                TraceData = new TraceDataProto
                {
                    TraceId = Guid.NewGuid().ToString()
                }
            };

            await Send(updateCommand);
            return;
        }

        var createCommand = new CreateTagCommandProto
        {
            TagId = Guid.NewGuid().ToString(),
            Name = tagName,
            TraceData = new TraceDataProto
            {
                TraceId = Guid.NewGuid().ToString()
            }
        };

        await Send(createCommand);
    }
}