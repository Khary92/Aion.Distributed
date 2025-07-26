using Service.Admin.Web.Models;

namespace Service.Admin.Web.Communication.Tags;

public interface ITagController
{
    TagWebModel? SelectedTag { get; set; }
    string InputTagName { get; set; }
    bool IsEditMode { get; set; }
    Task PersistTag();
    void ToggleTagEditMode();
}