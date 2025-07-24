using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Threading;
using Client.Desktop.DTO;
using Client.Desktop.Services.Initializer;
using CommunityToolkit.Mvvm.Messaging;
using Proto.Notifications.UseCase;
using ReactiveUI;
using Unit = System.Reactive.Unit;

namespace Client.Desktop.Models.Settings;

public class WorkDaysViewModel : ReactiveObject
{
    private WorkDayDto? _selectedWorkDay;

    public WorkDaysViewModel(IMessenger messenger, WorkDaysModel workDaysModel)
    {
        Model = workDaysModel;
        
        LoadSelectedDateCommand = ReactiveCommand.Create(
            () =>
            {
                Model.SetSelectedWorkday(SelectedWorkDay!);
                messenger.Send(new WorkDaySelectionChangedNotification());
            },
            this.WhenAnyValue(x => x.SelectedWorkDay).Any()
        );

        CreateNewDateCommand = ReactiveCommand.CreateFromTask<DateTimeOffset>(Model.AddWorkDayAsync);
    }
    
    public WorkDaysModel Model { get; }

    public ReactiveCommand<DateTimeOffset, Unit>? CreateNewDateCommand { get; internal set; }
    public ReactiveCommand<Unit, Unit>? LoadSelectedDateCommand { get; internal set; }

    public WorkDayDto? SelectedWorkDay
    {
        get => _selectedWorkDay;
        set => this.RaiseAndSetIfChanged(ref _selectedWorkDay, value);
    }
}