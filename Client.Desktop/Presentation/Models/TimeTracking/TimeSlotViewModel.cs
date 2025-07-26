using System;
using System.Threading.Tasks;
using Client.Desktop.Presentation.Factories;
using Client.Desktop.Presentation.Models.TimeTracking.DynamicControls;
using Client.Desktop.Presentation.Views.Custom;
using CommunityToolkit.Mvvm.Messaging;
using Proto.Notifications.UseCase;
using ReactiveUI;
using Unit = System.Reactive.Unit;

namespace Client.Desktop.Presentation.Models.TimeTracking;

public class TimeSlotViewModel : ReactiveObject
{
    private readonly Guid _viewId;

    private object _currentView = null!;
    private string _elapsedTimeRepresentation = null!;
    private string _timerButtonText = null!;

    private ViewTimer _viewTimer = null!;

    public TimeSlotViewModel(TimeSlotModel model,
        IStatisticsViewModelFactory statisticsViewModelFactory,
        INoteStreamViewModelFactory noteStreamViewModelFactory,
        IMessenger messenger)
    {
        Model = model;

        messenger.Register<CreateSnapshotNotification>(this, (_, _) =>
        {
            if (StatisticsViewModel == null) return;

            StatisticsViewModel.Update();
        });

        ViewId = Guid.NewGuid();
        NoteStreamViewModel = noteStreamViewModelFactory.Create(ViewId);
        StatisticsViewModel = statisticsViewModelFactory.Create();

        ToggleTimerCommand = ReactiveCommand.Create(UpdateTimerState);
        SwitchToDocumentationViewCommand = ReactiveCommand.Create(SwitchToDocumentationView);
        SwitchToStatisticsViewCommand = ReactiveCommand.Create(SwitchToStatisticsView);
        StartReplayCommand = ReactiveCommand.CreateFromTask(StartReplayMode);
        PreviousStateCommand = ReactiveCommand.Create(PreviousState);
        NextStateCommand = ReactiveCommand.Create(NextState);
        AddNoteCommand = ReactiveCommand.CreateFromTask(AddNoteHotkeyFired);

        Model.RegisterMessenger();

        SwitchToDocumentationView();
    }

    public NoteStreamViewModel NoteStreamViewModel { get; }
    public StatisticsViewModel StatisticsViewModel { get; }

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

    private Guid ViewId
    {
        get => _viewId;
        init => this.RaiseAndSetIfChanged(ref _viewId, value);
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

    private void SwitchToDocumentationView()
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