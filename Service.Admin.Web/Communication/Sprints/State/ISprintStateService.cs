using Service.Admin.Web.Communication.Sprints.Notifications;
using Service.Admin.Web.DTO;

namespace Service.Admin.Web.Communication.Sprints.State;

public interface ISprintStateService
{
    IReadOnlyList<SprintDto> Sprints { get; }
    event Action? OnStateChanged;
    Task AddSprint(SprintDto sprint);
    void Apply(WebAddTicketToActiveSprintNotification notification);
    void Apply(WebAddTicketToSprintNotification notification);
    void Apply(WebSetSprintActiveStatusNotification notification);
    void Apply(WebSprintDataUpdatedNotification notification);
    Task LoadSprints();
}