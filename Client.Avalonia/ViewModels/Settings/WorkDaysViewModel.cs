using System;
using System.Reactive.Linq;
using Contract.CQRS.Notifications.UseCase;
using Contract.DTO;
using MediatR;
using ReactiveUI;
using Unit = System.Reactive.Unit;

namespace Client.Avalonia.ViewModels.Settings;

public class WorkDaysViewModel : ReactiveObject
{
    private WorkDayDto _selectedWorkDay = null!;

    public WorkDaysViewModel(IMediator mediator, WorkDaysModel workDaysModel)
    {
        Model = workDaysModel;

        LoadSelectedDateCommand = ReactiveCommand.Create(
            () =>
            {
                workDaysModel.SetSelectedWorkday(SelectedWorkDay);
                mediator.Publish(new WorkDaySelectionChangedNotification());
            },
            this.WhenAnyValue(x => x.SelectedWorkDay).Any()
        );

        CreateNewDateCommand = ReactiveCommand.CreateFromTask<DateTimeOffset>(workDaysModel.AddWorkDayAsync);

        Model.RegisterMessenger();
        Model.InitializeAsync().ConfigureAwait(false);
    }

    public WorkDaysModel Model { get; }

    public ReactiveCommand<DateTimeOffset, Unit> CreateNewDateCommand { get; }

    public WorkDayDto SelectedWorkDay
    {
        get => _selectedWorkDay;
        set => this.RaiseAndSetIfChanged(ref _selectedWorkDay, value);
    }

    public ReactiveCommand<Unit, Unit> LoadSelectedDateCommand { get; }
}