using System;
using System.Collections.Generic;
using System.Linq;
using Client.Desktop.Communication.Commands.StatisticsData.Records;
using ReactiveUI;

namespace Client.Desktop.DataModels;

public sealed class StatisticsDataClientModel : ReactiveObject
{
    private readonly Guid _statisticsId;
    private readonly Guid _timeSlotId;
    private bool _isNeutral;
    private bool _isProductive;
    private bool _isUnproductive;

    private PreviousProductivityState _previousProductivityState;
    private PreviousTagsState _previousTagsState;
    private List<Guid> _tagIds = [];

    public StatisticsDataClientModel(Guid statisticsId, Guid timeSlotId, List<Guid> tagIds, bool isProductive,
        bool isNeutral,
        bool isUnproductive)
    {
        StatisticsId = statisticsId;
        TimeSlotId = timeSlotId;
        TagIds = tagIds;
        IsProductive = isProductive;
        IsNeutral = isNeutral;
        IsUnproductive = isUnproductive;

        _previousProductivityState = new PreviousProductivityState(_isProductive, _isNeutral, _isUnproductive);
        _previousTagsState = new PreviousTagsState(_tagIds.ToList());
    }

    public bool IsProductive
    {
        get => _isProductive;
        set => this.RaiseAndSetIfChanged(ref _isProductive, value);
    }

    public bool IsNeutral
    {
        get => _isNeutral;
        set => this.RaiseAndSetIfChanged(ref _isNeutral, value);
    }

    public bool IsUnproductive
    {
        get => _isUnproductive;
        set => this.RaiseAndSetIfChanged(ref _isUnproductive, value);
    }

    public Guid StatisticsId
    {
        get => _statisticsId;
        private init => this.RaiseAndSetIfChanged(ref _statisticsId, value);
    }

    public Guid TimeSlotId
    {
        get => _timeSlotId;
        private init => this.RaiseAndSetIfChanged(ref _timeSlotId, value);
    }

    public List<Guid> TagIds
    {
        get => _tagIds;
        set => this.RaiseAndSetIfChanged(ref _tagIds, value);
    }


    public bool IsProductivityChanged()
    {
        if (_previousProductivityState.IsNeutral == _isNeutral &&
            _previousProductivityState.IsProductive == _isProductive &&
            _previousProductivityState.IsUnproductive == _isUnproductive) return false;

        _previousProductivityState =
            new PreviousProductivityState(_isProductive, _isNeutral, _isUnproductive);

        return true;
    }

    public bool IsTagsSelectionChanged()
    {
        if (_previousTagsState.TagIds.SequenceEqual(_tagIds)) return false;

        _previousTagsState = new PreviousTagsState(_tagIds);
        return true;
    }

    private record PreviousProductivityState(bool IsProductive, bool IsNeutral, bool IsUnproductive);

    private record PreviousTagsState(List<Guid> TagIds);

    public void Apply(ClientChangeTagSelectionCommand notification)
    {
        TagIds = notification.SelectedTagIds;
    }
}