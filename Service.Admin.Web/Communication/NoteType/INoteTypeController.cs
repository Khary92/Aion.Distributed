using Service.Admin.Web.Models;

namespace Service.Admin.Web.Communication.NoteType;

public interface INoteTypeController
{
    string InputName { get; set; }
    string InputColor { get; set; }
    NoteTypeWebModel? SelectedNoteType { get; set; }
    bool CanSave { get; }
    string EditButtonText { get; }
    void ToggleEditMode();
    Task CreateOrUpdate();
}