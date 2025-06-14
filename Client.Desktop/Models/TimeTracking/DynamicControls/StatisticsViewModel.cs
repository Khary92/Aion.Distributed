using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Client.Desktop.Communication.Commands;
using Client.Desktop.Communication.Notifications.Tags;
using Client.Desktop.Communication.Requests;
using Client.Desktop.Factories;
using CommunityToolkit.Mvvm.Messaging;
using Contract.DTO;
using Proto.Command.StatisticsData;
using Proto.Requests.Tags;
using ReactiveUI;

namespace Client.Desktop.Models.TimeTracking.DynamicControls;

public class StatisticsViewModel(
    ICommandSender commandSender,
    IRequestSender requestSender,
    IMessenger messenger,
    ITagCheckBoxViewFactory tagCheckBoxViewFactory) : ReactiveObject
{
    private ObservableCollection<TagCheckBoxViewModel> _availableTags = [];
    private StatisticsDataDto? _statisticsData;

    private Guid _viewId;

    public StatisticsDataDto? StatisticsData
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
            var changeProductivityCommand = new ChangeProductivityCommand
            {
                StatisticsDataId = id.ToString(),
                IsProductive = StatisticsData.IsProductive,
                IsNeutral = StatisticsData.IsNeutral,
                IsUnproductive = StatisticsData.IsUnproductive
            };
            commandSender.Send(changeProductivityCommand);
        }


        if (StatisticsData.IsTagsSelectionChanged())
        {
            var cmd = new ChangeTagSelectionCommand
            {
                StatisticsDataId = id.ToString()
            };
            cmd.SelectedTagIds.AddRange(StatisticsData.TagIds.Select(t => t.ToString()));

            commandSender.Send(cmd);
        }
    }
}