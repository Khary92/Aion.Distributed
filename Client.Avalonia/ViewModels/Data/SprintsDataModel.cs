using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Client.Avalonia.Communication.Notifications.Sprints;
using CommunityToolkit.Mvvm.Messaging;
using Contract.CQRS.Notifications.Entities.Sprints;
using Contract.CQRS.Notifications.UseCase;
using Contract.DTO;
using Contract.Tracing;
using Contract.Tracing.Tracers;
using MediatR;
using ReactiveUI;

namespace Client.Avalonia.ViewModels.Data;

public class SprintsDataModel(IMediator mediator, IMessenger messenger, ITracingCollectorProvider tracer)
    : ReactiveObject
{
    public ObservableCollection<SprintDto> Sprints { get; } = [];

    public void RegisterMessenger()
    {
        messenger.Register<NewSprintMessage>(this, (_, m) =>
        {
            tracer.Sprint.Create.AggregateReceived(GetType(), m.Sprint.SprintId, m.Sprint.AsTraceAttributes());
            Sprints.Add(m.Sprint);
            tracer.Sprint.Create.AggregateAdded(GetType(), m.Sprint.SprintId);
        });

        messenger.Register<SprintDataUpdatedNotification>(this, (_, m) =>
        {
            tracer.Sprint.Update.NotificationReceived(GetType(), m.SprintId, m);

            var sprint = Sprints.FirstOrDefault(s => m.SprintId == s.SprintId);

            if (sprint is null)
            {
                tracer.Sprint.Update.NoAggregateFound(GetType(), m.SprintId);
                return;
            }

            sprint.Apply(m);
            tracer.Sprint.Update.ChangesApplied(GetType(), m.SprintId);
        });

        messenger.Register<TicketAddedToSprintNotification>(this, (_, m) =>
        {
            tracer.Sprint.AddTicketToSprint.NotificationReceived(GetType(), m.SprintId, m);

            var sprint = Sprints.FirstOrDefault(s => m.SprintId == s.SprintId);

            if (sprint is null)
            {
                tracer.Sprint.AddTicketToSprint.NoAggregateFound(GetType(), m.SprintId);

                return;
            }

            sprint.Apply(m);
            tracer.Sprint.AddTicketToSprint.ChangesApplied(GetType(), m.SprintId);
        });

        messenger.Register<SprintActiveStatusSetNotification>(this, (_, m) =>
        {
            tracer.Sprint.ActiveStatus.NotificationReceived(GetType(), m.SprintId, m);

            var sprint = Sprints.FirstOrDefault(s => m.SprintId == s.SprintId);

            if (sprint is null)
            {
                tracer.Sprint.ActiveStatus.NoAggregateFound(GetType(), m.SprintId);

                return;
            }

            sprint.Apply(m);
            tracer.Sprint.ActiveStatus.ChangesApplied(GetType(), m.SprintId);
        });
    }

    public async Task InitializeAsync()
    {
        Sprints.Clear();
        Sprints.AddRange(await mediator.Send(new GetAllSprintsRequest()));
    }

    public async Task UpdateSprint(SprintDto sprintDto)
    {
        var updateSprintDataCommand = new UpdateSprintDataCommand(sprintDto.SprintId,
            sprintDto.Name,
            sprintDto.StartTime,
            sprintDto.EndTime);

        await mediator.Send(updateSprintDataCommand);
        tracer.Sprint.ActiveStatus.CommandSent(GetType(), sprintDto.SprintId, updateSprintDataCommand);
    }

    public async Task CreateSprint(SprintDto newSprintDto)
    {
        var createSprintCommand = new CreateSprintCommand(newSprintDto.SprintId,
            newSprintDto.Name,
            newSprintDto.StartTime,
            newSprintDto.EndTime,
            newSprintDto.IsActive,
            newSprintDto.TicketIds);

        await mediator.Send(createSprintCommand);
        tracer.Sprint.Create.CommandSent(GetType(), newSprintDto.SprintId, createSprintCommand);
    }

    public async Task SetSprintActive(SprintDto selectedSprint)
    {
        var setSprintActiveStatusCommand = new SetSprintActiveStatusCommand(selectedSprint.SprintId, true);
        await mediator.Send(setSprintActiveStatusCommand);
        tracer.Sprint.ActiveStatus.CommandSent(GetType(), selectedSprint.SprintId, setSprintActiveStatusCommand);

        //TODO This is probably wrong. Check it.
        await mediator.Publish(new SprintSelectionChangedNotification());
    }
}