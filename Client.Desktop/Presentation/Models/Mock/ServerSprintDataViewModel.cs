using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Client.Desktop.DataModels;
using Google.Protobuf.WellKnownTypes;
using Proto.Command.Sprints;
using Proto.DTO.TraceData;
using ReactiveUI;
using Unit = System.Reactive.Unit;

namespace Client.Desktop.Presentation.Models.Mock;

public class ServerSprintDataViewModel : ReactiveObject
{
    private string _editButtonText = string.Empty;

    private DateTimeOffset _endTime = DateTimeOffset.Now.AddDays(7);

    private bool _isEditMode;

    private string _newSprintName = string.Empty;

    private SprintClientModel _selectedSprint = null!;

    private DateTimeOffset _startTime = DateTimeOffset.Now;

    public ServerSprintDataViewModel(ServerSprintDataModel dataModel)
    {
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
    }

    public ServerSprintDataModel DataModel { get; }

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

    public SprintClientModel SelectedSprint
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
        await DataModel.Send(new SetSprintActiveStatusCommandProto()
        {
            SprintId = SelectedSprint!.SprintId.ToString(),
            IsActive = !SelectedSprint.IsActive,
            TraceData = new TraceDataProto()
            {
                TraceId = Guid.NewGuid().ToString()
            }
        });
    }

    private async Task PersistSprint()
    {
        if (IsEditMode)
        {
            var updateSprintDataCommand = new UpdateSprintDataCommandProto()
            {
                SprintId = SelectedSprint.SprintId.ToString(),
                Name = NewSprintName,
                EndTime = Timestamp.FromDateTimeOffset(EndTime),
                StartTime = Timestamp.FromDateTimeOffset(StartTime),
                TraceData = new TraceDataProto()
                {
                    TraceId = Guid.NewGuid().ToString()
                }
            };
            
            await DataModel.Send(updateSprintDataCommand);
            ResetData();
            return;
        }

        var createSprintCommand = new CreateSprintCommandProto()
        {
            SprintId = Guid.NewGuid().ToString(),
            Name = NewSprintName,
            EndTime = Timestamp.FromDateTimeOffset(EndTime),
            StartTime = Timestamp.FromDateTimeOffset(StartTime),
            TraceData = new TraceDataProto()
            {
                TraceId = Guid.NewGuid().ToString()
            }
        };
        
        await DataModel.Send(createSprintCommand);
        ResetData();
    }
}