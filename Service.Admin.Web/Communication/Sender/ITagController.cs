using Service.Admin.Web.Models;

namespace Service.Admin.Web.Communication.Sender;

public interface ITagController
{
    TagWebModel? SelectedTag { get; set; }
    string InputTagName { get; set; }
    bool IsEditMode { get; }
    Task PersistTag();
    void ToggleTagEditMode();
}