using Proto.Command.NoteTypes;
using Service.Admin.Tracing;
using Service.Admin.Web.DTO;

namespace Service.Admin.Web.Communication.NoteType;

public class NoteTypeController(ISharedCommandSender commandSender, ITraceCollector tracer) : INoteTypeController
{
    public string InputName { get; set; } = string.Empty;
    public string InputColor { get; set; } = "#000000";
    public NoteTypeDto? SelectedNoteType { get; set; }

    public bool IsEditMode { get; set; }

    public bool CanSave => !string.IsNullOrWhiteSpace(InputName) &&
                           !string.IsNullOrWhiteSpace(InputColor);

    public string EditButtonText => IsEditMode ? "Cancel Edit" : "Edit";

    private void ResetData()
    {
        InputName = string.Empty;
        InputColor = "#000000";
    }

    public void ToggleEditMode()
    {
        IsEditMode = !IsEditMode;
        ResetData();

        if (!IsEditMode || SelectedNoteType is null) return;
        InputName = SelectedNoteType.Name;
        InputColor = SelectedNoteType.Color;
    }

    public async Task PersistNoteType()
    {
        if (IsEditMode && SelectedNoteType != null)
        {
            var originalName = SelectedNoteType.Name;
            var originalColor = SelectedNoteType.Color;

            if (InputName != originalName)
            {
                await tracer.NoteType.ChangeName.StartUseCase(GetType(), SelectedNoteType.NoteTypeId,
                    SelectedNoteType);

                var nameCommand = new ChangeNoteTypeNameCommandProto
                {
                    NoteTypeId = SelectedNoteType.NoteTypeId.ToString(),
                    Name = InputName
                };

                await commandSender.Send(nameCommand);
                await tracer.NoteType.ChangeName.CommandSent(GetType(), SelectedNoteType.NoteTypeId, nameCommand);
            }

            if (InputColor != originalColor)
            {
                await tracer.NoteType.ChangeColor.StartUseCase(GetType(), SelectedNoteType.NoteTypeId,
                    SelectedNoteType);

                var colorCommand = new ChangeNoteTypeColorCommandProto
                {
                    NoteTypeId = SelectedNoteType.NoteTypeId.ToString(),
                    Color = InputColor
                };

                await commandSender.Send(colorCommand);
                await tracer.NoteType.ChangeColor.CommandSent(GetType(), SelectedNoteType.NoteTypeId, colorCommand);
            }

            IsEditMode = false;
            ResetData();
            return;
        }

        var noteTypeId = Guid.NewGuid();

        var createCommand = new CreateNoteTypeCommandProto
        {
            NoteTypeId = noteTypeId.ToString(),
            Name = InputName,
            Color = InputColor
        };

        await tracer.NoteType.Create.StartUseCase(GetType(), noteTypeId, createCommand.ToString());
        await commandSender.Send(createCommand);
        await tracer.NoteType.Create.CommandSent(GetType(), noteTypeId, createCommand);

        ResetData();
    }
}