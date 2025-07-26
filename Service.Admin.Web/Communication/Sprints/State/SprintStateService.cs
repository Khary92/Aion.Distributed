using Proto.Requests.Sprints;
using Service.Admin.Web.Communication.Sprints.Notifications;
using Service.Admin.Web.Models;

namespace Service.Admin.Web.Communication.Sprints.State;

public class SprintStateService(ISharedRequestSender requestSender) : ISprintStateService
{
    private List<SprintWebModel> _sprints = new();
    public IReadOnlyList<SprintWebModel> Sprints => _sprints.AsReadOnly();

    public event Action? OnStateChanged;

    public Task AddSprint(SprintWebModel sprint)
    {
        _sprints.Add(sprint);
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

    public async Task LoadSprints()
    {
        var sprintListProto = await requestSender.Send(new GetAllSprintsRequestProto());
        _sprints = sprintListProto.ToDtoList();
        NotifyStateChanged();
    }

    private void NotifyStateChanged()
    {
        OnStateChanged?.Invoke();
    }
}