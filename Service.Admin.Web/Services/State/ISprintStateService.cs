using Service.Admin.Web.Communication.Records.Notifications;
using Service.Admin.Web.Communication.Records.Wrappers;
using Service.Admin.Web.Models;

namespace Service.Admin.Web.Services.State;

public interface ISprintStateService
{
    IReadOnlyList<SprintWebModel> Sprints { get; }
    event Action? OnStateChanged;
    Task AddSprint(NewSprintMessage sprintMessage);
    Task Apply(WebAddTicketToActiveSprintNotification notification);
    Task Apply(WebSetSprintActiveStatusNotification notification);
    Task Apply(WebSprintDataUpdatedNotification notification);
}