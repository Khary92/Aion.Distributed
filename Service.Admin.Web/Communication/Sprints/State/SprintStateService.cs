using Proto.Requests.Sprints;
using Service.Admin.Web.Communication.Sprints.Notifications;
using Service.Admin.Web.Communication.Wrappers;
using Service.Admin.Web.Models;
using Service.Admin.Web.Services;

namespace Service.Admin.Web.Communication.Sprints.State;

public class SprintStateService(ISharedRequestSender requestSender) : ISprintStateService, IInitializeAsync
{
    private List<SprintWebModel> _sprints = new();
    public IReadOnlyList<SprintWebModel> Sprints => _sprints.AsReadOnly();

    public event Action? OnStateChanged;

    public Task AddSprint(NewSprintMessage sprintMessage)
    {
        _sprints.Add(sprintMessage.Sprint);
        NotifyStateChanged();
        return Task.CompletedTask;
    }

    public void Apply(WebAddTicketToActiveSprintNotification notification)
    {
        var currentSprint = _sprints.FirstOrDefault(s => s.IsActive);

        if (currentSprint == null) return;

        currentSprint.Apply(notification);
        NotifyStateChanged();
    }

    public void Apply(WebAddTicketToSprintNotification notification)
    {
        var sprint = _sprints.FirstOrDefault(s => s.SprintId == notification.SprintId);

        if (sprint == null) return;

        sprint.Apply(notification);
        NotifyStateChanged();
    }

    public void Apply(WebSetSprintActiveStatusNotification notification)
    {
        foreach (var loadedSprint in _sprints)
        {
            if (loadedSprint.SprintId == notification.SprintId)
            {
                loadedSprint.IsActive = true;
                continue;
            }

            loadedSprint.IsActive = false;
        }

        NotifyStateChanged();
    }

    public void Apply(WebSprintDataUpdatedNotification notification)
    {
        var sprint = _sprints.FirstOrDefault(s => s.SprintId == notification.SprintId);

        if (sprint == null) return;

        sprint.Apply(notification);
        NotifyStateChanged();
    }
    
    private void NotifyStateChanged()
    {
        OnStateChanged?.Invoke();
    }

    public InitializationType Type => InitializationType.StateService;

    public async Task InitializeComponents()
    {
        var sprintListProto = await requestSender.Send(new GetAllSprintsRequestProto());
        _sprints = sprintListProto.ToDtoList();
        NotifyStateChanged();
    }
}