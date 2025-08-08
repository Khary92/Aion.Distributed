using Proto.Requests.Sprints;
using Service.Admin.Tracing;
using Service.Admin.Web.Communication.Sprints.Notifications;
using Service.Admin.Web.Communication.Wrappers;
using Service.Admin.Web.Models;
using Service.Admin.Web.Services;

namespace Service.Admin.Web.Communication.Sprints.State;

public class SprintStateService(ISharedRequestSender requestSender, ITraceCollector tracer)
    : ISprintStateService, IInitializeAsync
{
    private List<SprintWebModel> _sprints = new();
    public IReadOnlyList<SprintWebModel> Sprints => _sprints.AsReadOnly();

    public event Action? OnStateChanged;

    public async Task AddSprint(NewSprintMessage sprintMessage)
    {
        _sprints.Add(sprintMessage.Sprint);
        await tracer.Sprint.Create.AggregateAdded(GetType(), sprintMessage.TraceId);
        NotifyStateChanged();
    }

    public async Task Apply(WebAddTicketToActiveSprintNotification notification)
    {
        var currentSprint = _sprints.FirstOrDefault(s => s.IsActive);

        if (currentSprint == null)
        {
            await tracer.Sprint.AddTicketToSprint.NoAggregateFound(GetType(), notification.TraceId);
            return;
        }

        currentSprint.Apply(notification);
        await tracer.Sprint.AddTicketToSprint.ChangesApplied(GetType(), notification.TraceId);
        NotifyStateChanged();
    }

    public async Task Apply(WebAddTicketToSprintNotification notification)
    {
        var sprint = _sprints.FirstOrDefault(s => s.SprintId == notification.SprintId);

        if (sprint == null)
        {
            await tracer.Sprint.AddTicketToSprint.NoAggregateFound(GetType(), notification.TraceId);
            return;
        }

        sprint.Apply(notification);
        await tracer.Sprint.AddTicketToSprint.ChangesApplied(GetType(), notification.TraceId);
        NotifyStateChanged();
    }

    public async Task Apply(WebSetSprintActiveStatusNotification notification)
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

        await tracer.Sprint.ActiveStatus.ChangesApplied(GetType(), notification.TraceId);
        NotifyStateChanged();
    }

    public async Task Apply(WebSprintDataUpdatedNotification notification)
    {
        var sprint = _sprints.FirstOrDefault(s => s.SprintId == notification.SprintId);

        if (sprint == null)
        {
            await tracer.Sprint.Update.NoAggregateFound(GetType(), notification.TraceId);
            return;
        }

        sprint.Apply(notification);
        await tracer.Sprint.Update.ChangesApplied(GetType(), notification.TraceId);
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
        _sprints = sprintListProto.ToWebModelList();
        NotifyStateChanged();
    }
}