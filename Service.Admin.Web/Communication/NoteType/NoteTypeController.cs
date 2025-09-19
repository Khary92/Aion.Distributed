using Service.Admin.Tracing;
using Service.Admin.Web.Communication.NoteType.Records.Commands;
using Service.Admin.Web.Models;

namespace Service.Admin.Web.Communication.NoteType;

public class NoteTypeController(ISharedCommandSender commandSender, ITraceCollector tracer) : INoteTypeController
{
    public bool IsEditMode { get; set; }
    public string InputName { get; set; } = string.Empty;
    public string InputColor { get; set; } = "#000000";
    public NoteTypeWebModel? SelectedNoteType { get; set; }

    public bool CanSave => !string.IsNullOrWhiteSpace(InputName) &&
                           !string.IsNullOrWhiteSpace(InputColor);

    public string EditButtonText => IsEditMode ? "Cancel Edit" : "Edit";

    public void ToggleEditMode()
    {
        IsEditMode = !IsEditMode;
        ResetData();

        if (!IsEditMode || SelectedNoteType is null) return;
        InputName = SelectedNoteType.Name;
        InputColor = SelectedNoteType.Color;
    }

    public Task CreateOrUpdate()
    {
        return IsUpdateRequired() ? UpdateNoteType() : CreateNoteType();
    }

    private void ResetData()
    {
        InputName = string.Empty;
        InputColor = "#000000";
    }

    private async Task UpdateNoteType()
    {
        if (InputName != SelectedNoteType!.Name)
        {
            var traceId = Guid.NewGuid();

            await tracer.NoteType.ChangeName.StartUseCase(GetType(), traceId);

            var nameCommand = new WebChangeNoteTypeNameCommand(SelectedNoteType.NoteTypeId, InputName, traceId);

            await tracer.NoteType.ChangeName.SendingCommand(GetType(), traceId, nameCommand);
            await commandSender.Send(nameCommand.ToProto());
        }

        if (InputColor != SelectedNoteType.Color)
        {
            var traceId = Guid.NewGuid();

            await tracer.NoteType.ChangeColor.StartUseCase(GetType(), traceId);

            var colorCommand = new WebChangeNoteTypeColorCommand(SelectedNoteType.NoteTypeId, InputColor, traceId);

            await tracer.NoteType.ChangeColor.SendingCommand(GetType(), traceId, colorCommand);
            await commandSender.Send(colorCommand.ToProto());
        }

        IsEditMode = false;
        ResetData();
    }

    private async Task CreateNoteType()
    {
        var traceId = Guid.NewGuid();
        var createCommand = new WebCreateNoteTypeCommand(Guid.NewGuid(), InputName, InputColor, traceId);

        await tracer.NoteType.Create.StartUseCase(GetType(), traceId, createCommand.ToString());

        await tracer.NoteType.Create.SendingCommand(GetType(), traceId, createCommand);
        await commandSender.Send(createCommand.ToProto());

        ResetData();
    }

    private bool IsUpdateRequired()
    {
        return IsEditMode && SelectedNoteType != null;
    }
}