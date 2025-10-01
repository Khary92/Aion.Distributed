using System;
using System.Threading.Tasks;
using Client.Desktop.Communication.Local.LocalEvents.Publisher;
using Client.Desktop.DataModels;
using Client.Desktop.Presentation.Factories;
using Client.Desktop.Presentation.Models.TimeTracking.DynamicControls;
using Client.Desktop.Presentation.Views.Custom;
using ReactiveUI;
using Unit = System.Reactive.Unit;

namespace Client.Desktop.Presentation.Models.TimeTracking;

public class TrackingSlotViewModel : ReactiveObject
{
    private readonly IClientTimerNotificationPublisher _clientTimerNotificationPublisher;
    private readonly INoteStreamViewModelFactory _noteStreamViewModelFactory;
    private readonly IStatisticsViewModelFactory _statisticsViewModelFactory;
    private object _currentView = null!;
    private string _elapsedTimeRepresentation = null!;
    private string _timerButtonText = null!;

    private ViewTimer _viewTimer = null!;

    public TrackingSlotViewModel(TrackingSlotModel model,
        IStatisticsViewModelFactory statisticsViewModelFactory,
        INoteStreamViewModelFactory noteStreamViewModelFactory,
        IClientTimerNotificationPublisher clientTimerNotificationPublisher)
    {
        _statisticsViewModelFactory = statisticsViewModelFactory;
        _noteStreamViewModelFactory = noteStreamViewModelFactory;
        _clientTimerNotificationPublisher = clientTimerNotificationPublisher;
        Model = model;

        ToggleTimerCommand = ReactiveCommand.Create(UpdateTimerState);
        SwitchToDocumentationViewCommand = ReactiveCommand.Create(SwitchToNoteStreamView);
        SwitchToStatisticsViewCommand = ReactiveCommand.Create(SwitchToStatisticsView);
        StartReplayCommand = ReactiveCommand.CreateFromTask(StartReplayMode);
        PreviousStateCommand = ReactiveCommand.Create(PreviousState);
        NextStateCommand = ReactiveCommand.Create(NextState);
        AddNoteCommand = ReactiveCommand.CreateFromTask(AddNoteHotkeyFired);
    }

    public required NoteStreamViewModel NoteStreamViewModel { get; set; }
    public required StatisticsViewModel StatisticsViewModel { get; set; }

    public TrackingSlotModel Model { get; }

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

    public async Task CreateSubViewModels(Guid ticketId, Guid timeSlotId,
        StatisticsDataClientModel statisticsDataClientModel)
    {
        NoteStreamViewModel =
            await _noteStreamViewModelFactory.Create(timeSlotId, ticketId);
        StatisticsViewModel = _statisticsViewModelFactory.Create(statisticsDataClientModel);

        SwitchToNoteStreamView();
    }

    private async Task StartReplayMode()
    {
        await Model.ToggleReplayMode();
    }

    private void NextState()
    {
        //TODO
    }


    private void PreviousState()
    {
        //TODO
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

    private void SwitchToNoteStreamView()
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
        _ = HandleTimerTick();
    }

    private Task HandleTimerTick()
    {
        if (!Model.TimeSlot.IsTimerRunning) return Task.CompletedTask;

        Model.TimeSlot.EndTime = DateTimeOffset.Now;
        ElapsedTimeRepresentation = Model.TimeSlot.GetElapsedTime();
        return Task.CompletedTask;
    }
}