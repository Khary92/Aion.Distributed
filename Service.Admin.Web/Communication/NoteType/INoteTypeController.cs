using Service.Admin.Web.DTO;

namespace Service.Admin.Web.Communication.NoteType;

public interface INoteTypeController
{
    string InputName { get; set; }
    string InputColor { get; set; }
    NoteTypeDto? SelectedNoteType { get; set; }
    bool IsEditMode { get; set; }
    bool CanSave { get; }
    string EditButtonText { get; }
    void ToggleEditMode();
    Task PersistNoteType();
}