using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Client.Desktop.Communication.Notifications.Tag.Records;
using Client.Desktop.Communication.Notifications.Wrappers;
using Client.Desktop.Communication.Requests;
using Client.Desktop.Communication.Requests.Analysis.Records;
using Client.Desktop.Communication.Requests.Tag;
using Client.Desktop.DataModels;
using Client.Desktop.DataModels.Decorators;
using Client.Desktop.Lifecycle.Startup.Tasks.Initialize;
using Client.Desktop.Lifecycle.Startup.Tasks.Register;
using Client.Tracing.Tracing.Tracers;
using CommunityToolkit.Mvvm.Messaging;
using DynamicData;
using ReactiveUI;

namespace Client.Desktop.Presentation.Models.Analysis;

public class AnalysisByTagModel(IRequestSender requestSender, IMessenger messenger, ITraceCollector tracer)
    : ReactiveObject, IInitializeAsync, IRegisterMessenger
{
    private AnalysisByTagDecorator? _analysisByTag;

    public ObservableCollection<TagClientModel> Tags { get; } = [];

    public AnalysisByTagDecorator? AnalysisByTag
    {
        get => _analysisByTag;
        private set => this.RaiseAndSetIfChanged(ref _analysisByTag, value);
    }

    public InitializationType Type => InitializationType.Model;

    public async Task InitializeAsync()
    {
        Tags.Clear();
        Tags.AddRange(await requestSender.Send(new ClientGetAllTagsRequest()));
    }

    public void RegisterMessenger()
    {
        messenger.Register<NewTagMessage>(this, async void (_, message) =>
        {
            Tags.Add(message.Tag);
            await tracer.Tag.Create.AggregateAdded(GetType(), message.TraceId);
        });

        messenger.Register<ClientTagUpdatedNotification>(this, async void (_, notification) =>
        {
            var tag = Tags.FirstOrDefault(t => t.TagId == notification.TagId);

            if (tag == null)
            {
                await tracer.Tag.Update.NoAggregateFound(GetType(), notification.TraceId);
                return;
            }

            tag.Name = notification.Name;
            await tracer.Tag.Update.ChangesApplied(GetType(), notification.TraceId);
        });
    }

    public async Task SetAnalysisForTag(TagClientModel selectedTag)
    {
        AnalysisByTag = await requestSender.Send(new ClientGetTagAnalysisById(selectedTag.TagId));
    }
}