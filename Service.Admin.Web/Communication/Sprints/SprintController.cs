using Google.Protobuf.WellKnownTypes;
using Proto.Command.Sprints;
using Service.Admin.Tracing;
using Service.Admin.Web.Models;
using Service.Admin.Web.Pages;

namespace Service.Admin.Web.Communication.Sprints;

public class SprintController(ISharedCommandSender commandSender, ITraceCollector tracer) : ISprintController
{
    public SprintWebModel? SelectedSprint { get; set; }
    public string NewSprintName { get; set; } = string.Empty;
    public DateTimeOffset StartTime { get; set; } = DateTimeOffset.Now;
    public DateTimeOffset EndTime { get; set; } = DateTimeOffset.Now.AddDays(7);
    private bool IsEditMode { get; set; }

    public bool CanSave => !string.IsNullOrWhiteSpace(NewSprintName) &&
                           StartTime != default &&
                           EndTime != default &&
                           StartTime < EndTime;

    public string EditButtonText => IsEditMode ? "Cancel Edit" : "Edit";

    private void ResetData()
    {
        NewSprintName = string.Empty;
        StartTime = DateTimeOffset.Now;
        EndTime = DateTimeOffset.Now.AddDays(7);
    }

    public void ToggleEditMode()
    {
        IsEditMode = !IsEditMode;
        ResetData();

        if (!IsEditMode || SelectedSprint is null) return;
        NewSprintName = SelectedSprint.Name;
        StartTime = SelectedSprint.StartTime;
        EndTime = SelectedSprint.EndTime;
    }

    public async Task SetSelectedSprintActive()
    {
        if (SelectedSprint == null)
            return;

        await tracer.Sprint.ActiveStatus.StartUseCase(GetType(), SelectedSprint.SprintId,
            SelectedSprint.AsTraceAttributes());

        var command = new SetSprintActiveStatusCommandProto
        {
            SprintId = SelectedSprint.SprintId.ToString(),
            IsActive = true
        };

        await commandSender.Send(command);
        await tracer.Sprint.ActiveStatus.CommandSent(GetType(), SelectedSprint.SprintId, command);
    }

    public async Task PersistSprint()
    {
        if (IsEditMode && SelectedSprint != null)
        {
            var updateSprintDto = new SprintWebModel(SelectedSprint.SprintId, NewSprintName,
                SelectedSprint.IsActive, StartTime, EndTime, SelectedSprint.TicketIds);

            await tracer.Sprint.Update.StartUseCase(GetType(), updateSprintDto.SprintId,
                updateSprintDto.AsTraceAttributes());

            var command = new UpdateSprintDataCommandProto
            {
                SprintId = updateSprintDto.SprintId.ToString(),
                Name = updateSprintDto.Name,
                StartTime = Timestamp.FromDateTime(updateSprintDto.StartTime.UtcDateTime),
                EndTime = Timestamp.FromDateTime(updateSprintDto.EndTime.UtcDateTime)
            };

            await commandSender.Send(command);
            await tracer.Sprint.ActiveStatus.CommandSent(GetType(), updateSprintDto.SprintId, command);

            IsEditMode = false;
            ResetData();
            return;
        }

        var createSprintDto = new SprintWebModel(Guid.NewGuid(), NewSprintName, false, StartTime, EndTime, []);

        await tracer.Sprint.Create.StartUseCase(GetType(), createSprintDto.SprintId,
            createSprintDto.AsTraceAttributes());

        var createCommand = new CreateSprintCommandProto
        {
            SprintId = createSprintDto.SprintId.ToString(),
            Name = createSprintDto.Name,
            StartTime = Timestamp.FromDateTime(createSprintDto.StartTime.UtcDateTime),
            EndTime = Timestamp.FromDateTime(createSprintDto.EndTime.UtcDateTime),
            IsActive = createSprintDto.IsActive
        };

        await commandSender.Send(createCommand);
        await tracer.Sprint.Create.CommandSent(GetType(), createSprintDto.SprintId, createCommand);

        ResetData();
    }
}