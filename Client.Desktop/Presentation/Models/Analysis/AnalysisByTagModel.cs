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

public class AnalysisByTagModel(IMessenger messenger, IRequestSender requestSender, ITraceCollector tracer)
    : ReactiveObject, IInitializeAsync, IMessengerRegistration, IRecipient<NewTagMessage>,
        IRecipient<ClientTagUpdatedNotification>
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
        messenger.RegisterAll(this);
    }

    public void UnregisterMessenger()
    {
        messenger.UnregisterAll(this);
    }

    public void Receive(ClientTagUpdatedNotification message)
    {
        var tag = Tags.FirstOrDefault(t => t.TagId == message.TagId);

        if (tag == null)
        {
            _ = tracer.Tag.Update.NoAggregateFound(GetType(), message.TraceId);
            return;
        }

        tag.Name = message.Name;
        _ = tracer.Tag.Update.ChangesApplied(GetType(), message.TraceId);
    }

    public void Receive(NewTagMessage message)
    {
        Tags.Add(message.Tag);
        _ = tracer.Tag.Create.AggregateAdded(GetType(), message.TraceId);
    }

    public async Task SetAnalysisForTag(TagClientModel selectedTag)
    {
        AnalysisByTag = await requestSender.Send(new ClientGetTagAnalysisById(selectedTag.TagId));
    }
}