using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Client.Desktop.Communication.Local;
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
using DynamicData;
using ReactiveUI;

namespace Client.Desktop.Presentation.Models.Analysis;

public class AnalysisByTagModel(
    IRequestSender requestSender,
    ITraceCollector tracer,
    INotificationPublisherFacade notificationPublisher)
    : ReactiveObject, IInitializeAsync, IEventRegistration
{
    private AnalysisByTagDecorator? _analysisByTag;

    public ObservableCollection<TagClientModel> Tags { get; } = [];

    public AnalysisByTagDecorator? AnalysisByTag
    {
        get => _analysisByTag;
        private set => this.RaiseAndSetIfChanged(ref _analysisByTag, value);
    }

    public void RegisterMessenger()
    {
        notificationPublisher.Tag.NewTagMessageNotificationReceived += HandleNewTagMessage;
        notificationPublisher.Tag.ClientTagUpdatedNotificationReceived += HandleClientTagUpdatedNotification;
    }

    public void UnregisterMessenger()
    {
        notificationPublisher.Tag.NewTagMessageNotificationReceived -= HandleNewTagMessage;
        notificationPublisher.Tag.ClientTagUpdatedNotificationReceived -= HandleClientTagUpdatedNotification;
    }

    public InitializationType Type => InitializationType.Model;

    public async Task InitializeAsync()
    {
        var tagClientModels = await requestSender.Send(new ClientGetAllTagsRequest());

        Tags.Clear();
        Tags.AddRange(tagClientModels);
    }

    private async Task HandleClientTagUpdatedNotification(ClientTagUpdatedNotification message)
    {
        var tag = Tags.FirstOrDefault(t => t.TagId == message.TagId);

        if (tag == null)
        {
            await tracer.Tag.Update.NoAggregateFound(GetType(), message.TraceId);
            return;
        }

        tag.Apply(message);
        await tracer.Tag.Update.ChangesApplied(GetType(), message.TraceId);
    }

    private async Task HandleNewTagMessage(NewTagMessage message)
    {
        Tags.Add(message.Tag);
        await tracer.Tag.Create.AggregateAdded(GetType(), message.TraceId);
    }

    public async Task SetAnalysisForTag(TagClientModel selectedTag)
    {
        var analysisByTagDecorator = await requestSender.Send(new ClientGetTagAnalysisById(selectedTag.TagId));
        AnalysisByTag = analysisByTagDecorator;
    }
}