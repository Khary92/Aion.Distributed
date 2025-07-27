using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Client.Desktop.Communication.Commands;
using Client.Desktop.Communication.Commands.StatisticsData.Records;
using Client.Desktop.Communication.Notifications.Wrappers;
using Client.Desktop.Communication.Requests;
using Client.Desktop.DataModels;
using Client.Desktop.Presentation.Factories;
using CommunityToolkit.Mvvm.Messaging;
using Proto.Command.StatisticsData;
using Proto.Requests.Tags;
using ReactiveUI;

namespace Client.Desktop.Presentation.Models.TimeTracking.DynamicControls;

public class StatisticsViewModel(
    ICommandSender commandSender,
    IRequestSender requestSender,
    IMessenger messenger,
    ITagCheckBoxViewFactory tagCheckBoxViewFactory) : ReactiveObject
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

        var tagDtos = await requestSender.Send(new GetAllTagsRequestProto());

        foreach (var tagDto in tagDtos) AvailableTags.Add(tagCheckBoxViewFactory.Create(tagDto));

        AvailableTags
            .Where(tvm => StatisticsData!.TagIds.Contains(tvm.Tag!.TagId))
            .ToList()
            .ForEach(tvm => tvm.IsChecked = true);
    }

    public void RegisterMessenger()
    {
        messenger.Register<NewTagMessage>(this, (_, m) => { AvailableTags.Add(tagCheckBoxViewFactory.Create(m.Tag)); });
    }

    public void Update()
    {
        StatisticsData!.TagIds = AvailableTags
            .Where(t => t.IsChecked)
            .Select(t => t.Tag!.TagId)
            .ToList();

        var id = StatisticsData.StatisticsId;

        if (StatisticsData.IsProductivityChanged())
        {
            var changeProductivityCommand = new ClientChangeProductivityCommand
            (
                id,
                StatisticsData.IsProductive,
                StatisticsData.IsNeutral,
                StatisticsData.IsUnproductive
            );
            commandSender.Send(changeProductivityCommand);
        }
        
        
        if (StatisticsData.IsTagsSelectionChanged())
        {
            var tagSelectionCommand = new ClientChangeTagSelectionCommand(id, StatisticsData.TagIds);
            commandSender.Send(tagSelectionCommand);
        }
    }
}