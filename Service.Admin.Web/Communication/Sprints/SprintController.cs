using Service.Admin.Tracing;
using Service.Admin.Web.Communication.Sprints.Records;
using Service.Admin.Web.Models;

namespace Service.Admin.Web.Communication.Sprints;

public class SprintController(ISharedCommandSender commandSender, ITraceCollector tracer) : ISprintController
{
    private bool IsEditMode { get; set; }
    public SprintWebModel? SelectedSprint { get; set; }
    public string NewSprintName { get; set; } = string.Empty;
    public DateTimeOffset StartTime { get; set; } = DateTimeOffset.Now;
    public DateTimeOffset EndTime { get; set; } = DateTimeOffset.Now.AddDays(7);

    public bool CanSave => !string.IsNullOrWhiteSpace(NewSprintName) &&
                           StartTime != default &&
                           EndTime != default &&
                           StartTime < EndTime;

    public string EditButtonText => IsEditMode ? "Cancel Edit" : "Edit";

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
        var traceId = Guid.NewGuid();
        await tracer.Sprint.ActiveStatus.StartUseCase(GetType(), traceId);

        if (SelectedSprint == null)
        {
            await tracer.Sprint.ActiveStatus.NoEntitySelected(GetType(), traceId);
            return;
        }

        var command = new WebSetSprintActiveStatusCommand(SelectedSprint.SprintId, true, traceId);

        await tracer.Sprint.ActiveStatus.SendingCommand(GetType(), traceId, command);
        await commandSender.Send(command.ToProto());
    }

    public Task CreateOrUpdateSprint()
    {
        return IsUpdateRequired() ? UpdateSprint() : CreateSprint();
    }

    private void ResetData()
    {
        NewSprintName = string.Empty;
        StartTime = DateTimeOffset.Now;
        EndTime = DateTimeOffset.Now.AddDays(7);
    }

    private bool IsUpdateRequired()
    {
        return IsEditMode && SelectedSprint != null;
    }

    private async Task UpdateSprint()
    {
        var traceId = Guid.NewGuid();
        await tracer.Sprint.Update.StartUseCase(GetType(), traceId);

        if (SelectedSprint == null)
        {
            await tracer.Sprint.Update.NoEntitySelected(GetType(), traceId);
            return;
        }

        var command =
            new WebUpdateSprintDataCommand(SelectedSprint.SprintId, NewSprintName, StartTime, EndTime, traceId);

        await tracer.Sprint.Update.SendingCommand(GetType(), traceId, command);
        await commandSender.Send(command.ToProto());

        IsEditMode = false;
        ResetData();
    }

    private async Task CreateSprint()
    {
        var traceId = Guid.NewGuid();
        await tracer.Sprint.Create.StartUseCase(GetType(), traceId);

        var createCommand =
            new WebCreateSprintCommand(Guid.NewGuid(), NewSprintName, StartTime, EndTime, false, traceId);

        await tracer.Sprint.Create.SendingCommand(GetType(), traceId, createCommand);
        await commandSender.Send(createCommand.ToProto());

        ResetData();
    }
}