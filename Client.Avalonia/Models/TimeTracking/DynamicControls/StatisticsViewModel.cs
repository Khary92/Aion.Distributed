using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Client.Avalonia.Communication.Notifications.Tags;
using Client.Avalonia.Factories;
using CommunityToolkit.Mvvm.Messaging;
using Contract.DTO;
using MediatR;
using ReactiveUI;

namespace Client.Avalonia.ViewModels.TimeTracking.DynamicControls;

public class StatisticsViewModel(
    IMediator mediator,
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

        var tagDtos = await mediator.Send(new GetAllTagsRequest());

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
            mediator.Send(new ChangeProductivityCommand(id,
                StatisticsData.IsProductive,
                StatisticsData.IsNeutral,
                StatisticsData.IsUnproductive));

        if (StatisticsData.IsTagsSelectionChanged())
            mediator.Send(new ChangeTagSelectionCommand(id, StatisticsData.TagIds));
    }
}