using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Client.Desktop.Converter;
using Client.Desktop.DTO;
using Client.Tracing.Tracing.Tracers;
using ReactiveUI;
using Unit = System.Reactive.Unit;

namespace Client.Desktop.Models.Data;

public class SprintsDataViewModel : ReactiveObject
{
    private readonly ITraceCollector _tracer;
    private string _editButtonText = string.Empty;

    private DateTimeOffset _endTime = DateTimeOffset.Now.AddDays(7);

    private bool _isEditMode;

    private string _newSprintName = string.Empty;

    private SprintDto _selectedSprint = null!;

    private DateTimeOffset _startTime = DateTimeOffset.Now;

    public SprintsDataViewModel(SprintsDataModel dataModel, ITraceCollector tracer)
    {
        _tracer = tracer;
        DataModel = dataModel;

        CreateSprintCommand = ReactiveCommand.CreateFromTask(PersistSprint,
            this.WhenAnyValue(
                x => x.NewSprintName,
                x => x.StartTime,
                x => x.EndTime,
                (name, start, end) =>
                    !string.IsNullOrWhiteSpace(name) &&
                    start != default &&
                    end != default &&
                    start < end
            ));

        EditSprintCommand = ReactiveCommand.Create(ToggleTagEditMode);

        SetSprintActiveCommand = ReactiveCommand.CreateFromTask(SetSelectedSprintActive,
            this.WhenAnyValue(x => x.SelectedSprint).Any()
        );

        IsEditMode = false;

        DataModel.RegisterMessenger();
        DataModel.InitializeAsync().ConfigureAwait(false);
    }

    public SprintsDataModel DataModel { get; }

    public string NewSprintName
    {
        get => _newSprintName;
        set => this.RaiseAndSetIfChanged(ref _newSprintName, value);
    }

    public DateTimeOffset StartTime
    {
        get => _startTime;
        set => this.RaiseAndSetIfChanged(ref _startTime, value);
    }

    public DateTimeOffset EndTime
    {
        get => _endTime;
        set => this.RaiseAndSetIfChanged(ref _endTime, value);
    }

    public SprintDto SelectedSprint
    {
        get => _selectedSprint;
        set => this.RaiseAndSetIfChanged(ref _selectedSprint, value);
    }

    private bool IsEditMode
    {
        get => _isEditMode;
        set
        {
            this.RaiseAndSetIfChanged(ref _isEditMode, value);
            EditButtonText = _isEditMode ? "Cancel Edit" : "Edit";
        }
    }

    public string EditButtonText
    {
        get => _editButtonText;
        private set => this.RaiseAndSetIfChanged(ref _editButtonText, value);
    }

    public ReactiveCommand<Unit, Unit> CreateSprintCommand { get; }
    public ReactiveCommand<Unit, Unit> EditSprintCommand { get; }
    public ReactiveCommand<Unit, Unit> SetSprintActiveCommand { get; }

    private void ResetData()
    {
        NewSprintName = string.Empty;
        StartTime = DateTimeOffset.Now;
        EndTime = DateTimeOffset.Now.AddDays(7);
    }

    private void ToggleTagEditMode()
    {
        IsEditMode = !IsEditMode;
        ResetData();

        if (!IsEditMode) return;
        NewSprintName = SelectedSprint.Name;
        StartTime = SelectedSprint.StartTime;
        EndTime = SelectedSprint.EndTime;
    }

    private async Task SetSelectedSprintActive()
    {
        DataModel.Sprints
            .Where(s => s.SprintId != SelectedSprint.SprintId)
            .ToList()
            .ForEach(s => s.IsActive = false);

        await _tracer.Sprint.ActiveStatus.StartUseCase(GetType(), SelectedSprint.SprintId,
            SelectedSprint.AsTraceAttributes());

        await DataModel.SetSprintActive(SelectedSprint);
    }

    private async Task PersistSprint()
    {
        if (IsEditMode)
        {
            var updateSprintDto = new SprintDto(SelectedSprint.SprintId, NewSprintName, SelectedSprint.IsActive,
                StartTime, EndTime, SelectedSprint.TicketIds);

            await _tracer.Sprint.Update.StartUseCase(GetType(), updateSprintDto.SprintId,
                updateSprintDto.AsTraceAttributes());
            
            await DataModel.UpdateSprint(updateSprintDto);

            IsEditMode = false;
            ResetData();

            return;
        }

        var createSprintDto = new SprintDto(Guid.NewGuid(), NewSprintName, false, StartTime, EndTime, []);
        await _tracer.Sprint.Create.StartUseCase(GetType(), createSprintDto.SprintId, createSprintDto.AsTraceAttributes());
        await DataModel.CreateSprint(createSprintDto);

        ResetData();
    }
}