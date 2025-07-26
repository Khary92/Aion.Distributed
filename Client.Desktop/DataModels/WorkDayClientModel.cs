using System;
using ReactiveUI;

namespace Client.Desktop.DataModels;

public class WorkDayClientModel : ReactiveObject
{
    private readonly Guid _workDayId;

    private DateTimeOffset _date;

    public WorkDayClientModel(Guid workDayId, DateTimeOffset date)
    {
        WorkDayId = workDayId;
        Date = date;
    }

    public Guid WorkDayId
    {
        get => _workDayId;
        private init => this.RaiseAndSetIfChanged(ref _workDayId, value);
    }

    public DateTimeOffset Date
    {
        get => _date;
        set => this.RaiseAndSetIfChanged(ref _date, value);
    }
}