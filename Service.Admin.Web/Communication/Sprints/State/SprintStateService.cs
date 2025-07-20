using Proto.Requests.NoteTypes;
using Proto.Requests.Sprints;
using Service.Admin.Web.Communication.Sprints.Notifications;
using Service.Admin.Web.DTO;

namespace Service.Admin.Web.Communication.Sprints.State;

public class SprintStateService(ISharedRequestSender requestSender) : ISprintStateService
{
    private List<SprintDto> _sprints = new();
    public IReadOnlyList<SprintDto> Sprints => _sprints.AsReadOnly();

    public event Action? OnStateChanged;

    public Task AddSprint(SprintDto sprint)
    {
        _sprints.Add(sprint);
        return Task.CompletedTask;
    }

    public void Apply(WebAddTicketToActiveSprintNotification notification)
    {
        var currentSprint = _sprints.FirstOrDefault(s => s.IsActive);

        if (currentSprint == null)
        {
            return;
        }

        currentSprint.Apply(notification);
    }

    public void Apply(WebAddTicketToSprintNotification notification)
    {
        var sprint = _sprints.FirstOrDefault(s => s.SprintId == notification.SprintId);

        if (sprint == null)
        {
            return;
        }

        sprint.Apply(notification);
    }

    public void Apply(WebSetSprintActiveStatusNotification notification)
    {
        var sprint = _sprints.FirstOrDefault(s => s.SprintId == notification.SprintId);

        if (sprint == null)
        {
            return;
        }

        sprint.Apply(notification);
    }

    public void Apply(WebSprintDataUpdatedNotification notification)
    {
        var sprint = _sprints.FirstOrDefault(s => s.SprintId == notification.SprintId);

        if (sprint == null)
        {
            return;
        }

        sprint.Apply(notification);
    }

    public async Task LoadSprints()
    {
        var sprintListProto = await requestSender.Send(new GetAllSprintsRequestProto());
        _sprints = sprintListProto.ToDtoList();
    }
}