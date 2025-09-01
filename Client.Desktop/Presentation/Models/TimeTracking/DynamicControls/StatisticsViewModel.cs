using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Client.Desktop.Communication.Commands;
using Client.Desktop.Communication.Commands.StatisticsData.Records;
using Client.Desktop.Communication.Notifications.StatisticsData.Records;
using Client.Desktop.Communication.Notifications.Tag.Records;
using Client.Desktop.Communication.Notifications.Wrappers;
using Client.Desktop.Communication.Requests;
using Client.Desktop.Communication.Requests.Tag;
using Client.Desktop.DataModels;
using Client.Desktop.Presentation.Factories;
using Client.Tracing.Tracing.Tracers;
using CommunityToolkit.Mvvm.Messaging;
using ReactiveUI;

namespace Client.Desktop.Presentation.Models.TimeTracking.DynamicControls;

public class StatisticsViewModel(
    ICommandSender commandSender,
    IRequestSender requestSender,
    IMessenger messenger,
    ITagCheckBoxViewFactory tagCheckBoxViewFactory,
    ITraceCollector tracer) : ReactiveObject
{
    private ObservableCollection<TagCheckBoxViewModel> _availableTags = [];
    private StatisticsDataClientModel? _statisticsData;

    private Guid _viewId;

    public StatisticsDataClientModel? StatisticsData
    {
        get => _statisticsData;
        set => this.RaiseAndSetIfChanged(ref _statisticsData, value);
    }

    public Guid ViewId
    {
        get => _viewId;
        set => this.RaiseAndSetIfChanged(ref _viewId, value);
    }

    public ObservableCollection<TagCheckBoxViewModel> AvailableTags
    {
        get => _availableTags;
        set => this.RaiseAndSetIfChanged(ref _availableTags, value);
    }

    public async Task Initialize()
    {
        AvailableTags.Clear();

        var tagClientModels = await requestSender.Send(new ClientGetAllTagsRequest(Guid.NewGuid()));

        foreach (var tagDto in tagClientModels) AvailableTags.Add(tagCheckBoxViewFactory.Create(tagDto));

        AvailableTags
            .Where(tvm => StatisticsData!.TagIds.Contains(tvm.Tag!.TagId))
            .ToList()
            .ForEach(tvm => tvm.IsChecked = true);
    }

    public void RegisterMessenger()
    {
        messenger.Register<NewTagMessage>(this, async void (_, message) =>
        {
            AvailableTags.Add(tagCheckBoxViewFactory.Create(message.Tag));
            await tracer.Tag.Create.AggregateAdded(GetType(), message.TraceId);
        });
        
        messenger.Register<ClientTagUpdatedNotification>(this, async void (_, notification) =>
        {
            var tagViewModel = AvailableTags.FirstOrDefault(t => t.Tag!.TagId == notification.TagId);

            if (tagViewModel?.Tag == null)
            {
                await tracer.Tag.Update.NoAggregateFound(GetType(), notification.TraceId);
                return;
            }
            
            tagViewModel.Tag.Apply(notification);
            await tracer.Tag.Update.ChangesApplied(GetType(), notification.TraceId);
        });
        
        messenger.Register<ClientChangeProductivityNotification>(this, async void (_, notification) =>
        {
            if (StatisticsData?.StatisticsId != notification.StatisticsDataId)
            {
                await tracer.Statistics.ChangeTagSelection.WrongModel(GetType(), notification.TraceId);
                return;
            }
            
            StatisticsData!.Apply(notification);
            await tracer.Statistics.ChangeTagSelection.ChangesApplied(GetType(), notification.TraceId);
        });
    }

    public async Task Update()
    {
        StatisticsData!.TagIds = AvailableTags
            .Where(t => t.IsChecked)
            .Select(t => t.Tag!.TagId)
            .ToList();

        if (StatisticsData.IsProductivityChanged())
        {
            var traceId = Guid.NewGuid();
            await tracer.Statistics.ChangeProductivity.StartUseCase(GetType(), traceId);

            var changeProductivityCommand = new ClientChangeProductivityCommand
            (
                StatisticsData.StatisticsId,
                StatisticsData.IsProductive,
                StatisticsData.IsNeutral,
                StatisticsData.IsUnproductive,
                traceId
            );

            await tracer.Statistics.ChangeProductivity.SendingCommand(GetType(), traceId, changeProductivityCommand);
            await commandSender.Send(changeProductivityCommand);
        }


        if (StatisticsData.IsTagsSelectionChanged())
        {
            var traceId = Guid.NewGuid();
            await tracer.Statistics.ChangeTagSelection.StartUseCase(GetType(), traceId);

            var tagSelectionCommand =
                new ClientChangeTagSelectionCommand(StatisticsData.StatisticsId, StatisticsData.TagIds, traceId);

            await tracer.Statistics.ChangeTagSelection.SendingCommand(GetType(), traceId, tagSelectionCommand);
            await commandSender.Send(tagSelectionCommand);
        }
    }
}