using System;
using System.Threading.Tasks;
using Client.Desktop.Communication.Notifications.UseCase.Records;
using Client.Desktop.DataModels;
using Client.Desktop.Presentation.Factories;
using Client.Desktop.Presentation.Models.TimeTracking.DynamicControls;
using Client.Desktop.Presentation.Views.Custom;
using CommunityToolkit.Mvvm.Messaging;
using ReactiveUI;
using Unit = System.Reactive.Unit;

namespace Client.Desktop.Presentation.Models.TimeTracking;

public class TimeSlotViewModel : ReactiveObject
{
    private readonly INoteStreamViewModelFactory _noteStreamViewModelFactory;
    private readonly IStatisticsViewModelFactory _statisticsViewModelFactory;
    private object _currentView = null!;
    private string _elapsedTimeRepresentation = null!;
    private string _timerButtonText = null!;

    private ViewTimer _viewTimer = null!;

    public TimeSlotViewModel(TimeSlotModel model,
        IStatisticsViewModelFactory statisticsViewModelFactory,
        INoteStreamViewModelFactory noteStreamViewModelFactory,
        IMessenger messenger)
    {
        _statisticsViewModelFactory = statisticsViewModelFactory;
        _noteStreamViewModelFactory = noteStreamViewModelFactory;
        Model = model;

        messenger.Register<ClientCreateSnapshotNotification>(this, async void (_, _) =>
        {
            if (StatisticsViewModel == null) return;

            await StatisticsViewModel.Update();
        });

        ToggleTimerCommand = ReactiveCommand.Create(UpdateTimerState);
        SwitchToDocumentationViewCommand = ReactiveCommand.Create(SwitchToNotestreamView);
        SwitchToStatisticsViewCommand = ReactiveCommand.Create(SwitchToStatisticsView);
        StartReplayCommand = ReactiveCommand.CreateFromTask(StartReplayMode);
        PreviousStateCommand = ReactiveCommand.Create(PreviousState);
        NextStateCommand = ReactiveCommand.Create(NextState);
        AddNoteCommand = ReactiveCommand.CreateFromTask(AddNoteHotkeyFired);
    }

    public required NoteStreamViewModel NoteStreamViewModel { get; set; }
    public required StatisticsViewModel StatisticsViewModel { get; set; }

    public TimeSlotModel Model { get; }

    public ReactiveCommand<Unit, Unit> ToggleTimerCommand { get; }
    public ReactiveCommand<Unit, Unit> SwitchToDocumentationViewCommand { get; }
    public ReactiveCommand<Unit, Unit> SwitchToStatisticsViewCommand { get; }
    public ReactiveCommand<Unit, Unit> AddNoteCommand { get; }
    public ReactiveCommand<Unit, Unit> StartReplayCommand { get; }
    public ReactiveCommand<Unit, Unit> PreviousStateCommand { get; }
    public ReactiveCommand<Unit, Unit> NextStateCommand { get; }

    public object CurrentView
    {
        get => _currentView;
        set => this.RaiseAndSetIfChanged(ref _currentView, value);
    }

    public string TimerButtonText
    {
        get => _timerButtonText;
        private set => this.RaiseAndSetIfChanged(ref _timerButtonText, value);
    }

    public string ElapsedTimeRepresentation
    {
        get => _elapsedTimeRepresentation;
        set => this.RaiseAndSetIfChanged(ref _elapsedTimeRepresentation, value);
    }

    public void CreateSubViewModels(Guid ticketId, Guid timeSlotId, StatisticsDataClientModel statisticsDataClientModel)
    {
        NoteStreamViewModel =
            _noteStreamViewModelFactory.Create(timeSlotId, ticketId);
        StatisticsViewModel = _statisticsViewModelFactory.Create(statisticsDataClientModel);

        SwitchToNotestreamView();
    }

    private async Task StartReplayMode()
    {
        await Model.ToggleReplayMode();
    }

    private void NextState()
    {
        if (!Model.TicketReplayDecorator.IsReplayMode) return;

        Model.TicketReplayDecorator.Next();
    }


    private void PreviousState()
    {
        if (!Model.TicketReplayDecorator.IsReplayMode) return;

        Model.TicketReplayDecorator.Previous();
    }

    private void UpdateTimerState()
    {
        Model.ToggleTimerState();
        SetTimerText();
    }

    private async Task AddNoteHotkeyFired()
    {
        if (CurrentView is not NoteStreamViewModel documentationViewModel) return;

        await documentationViewModel.AddNoteControl();
    }

    private void SwitchToNotestreamView()
    {
        CurrentView = NoteStreamViewModel;
    }

    private void SwitchToStatisticsView()
    {
        CurrentView = StatisticsViewModel;
    }

    public void InitializeViewTimer(ViewTimer timer)
    {
        SetTimerText();
        ElapsedTimeRepresentation = Model.TimeSlot.GetElapsedTime();

        _viewTimer = timer;
        _viewTimer.Tick += OnTimerTick;

        _viewTimer.Start();
    }

    private void SetTimerText()
    {
        TimerButtonText = Model.TimeSlot.IsTimerRunning ? "Stop" : "Start";
    }

    private void OnTimerTick(object? sender, EventArgs e)
    {
        if (!Model.TimeSlot.IsTimerRunning) return;

        Model.TimeSlot.EndTime = DateTimeOffset.Now;
        ElapsedTimeRepresentation = Model.TimeSlot.GetElapsedTime();
    }
}