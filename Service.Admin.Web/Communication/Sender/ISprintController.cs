using Service.Admin.Web.Models;

namespace Service.Admin.Web.Communication.Sender;

public interface ISprintController
{
    SprintWebModel? SelectedSprint { get; set; }
    string NewSprintName { get; set; }
    DateTimeOffset StartTime { get; set; }
    DateTimeOffset EndTime { get; set; }
    bool CanSave { get; }
    string EditButtonText { get; }
    void ToggleEditMode();
    Task SetSelectedSprintActive();
    Task CreateOrUpdateSprint();
}