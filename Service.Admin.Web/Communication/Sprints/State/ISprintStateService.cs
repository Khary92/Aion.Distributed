using Service.Admin.Web.Communication.Sprints.Notifications;
using Service.Admin.Web.Communication.Wrappers;
using Service.Admin.Web.Models;

namespace Service.Admin.Web.Communication.Sprints.State;

public interface ISprintStateService
{
    IReadOnlyList<SprintWebModel> Sprints { get; }
    event Action? OnStateChanged;
    Task AddSprint(NewSprintMessage sprintMessage);
    void Apply(WebAddTicketToActiveSprintNotification notification);
    void Apply(WebAddTicketToSprintNotification notification);
    void Apply(WebSetSprintActiveStatusNotification notification);
    void Apply(WebSprintDataUpdatedNotification notification);
}