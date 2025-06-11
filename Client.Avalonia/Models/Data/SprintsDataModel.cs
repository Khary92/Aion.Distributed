using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Client.Avalonia.Communication.Commands;
using Client.Avalonia.Communication.Notifications.Sprints;
using Client.Avalonia.Communication.Requests;
using CommunityToolkit.Mvvm.Messaging;
using Contract.DTO;
using Contract.Tracing;
using Contract.Tracing.Tracers;
using DynamicData;
using Google.Protobuf.WellKnownTypes;
using Proto.Command.Sprints;
using Proto.Notifications.Sprint;
using ReactiveUI;

namespace Client.Avalonia.Models.Data;

public class SprintsDataModel(
    ICommandSender commandSender,
    IRequestSender requestSender,
    IMessenger messenger)
    //ITracingCollectorProvider tracer)
    : ReactiveObject
{
    public ObservableCollection<SprintDto> Sprints { get; } = [];

    public void RegisterMessenger()
    {
        messenger.Register<NewSprintMessage>(this, (_, m) =>
        {
            //tracer.Sprint.Create.AggregateReceived(GetType(), m.Sprint.SprintId, m.Sprint.AsTraceAttributes());
            Sprints.Add(m.Sprint);
            //tracer.Sprint.Create.AggregateAdded(GetType(), m.Sprint.SprintId);
        });

        messenger.Register<SprintDataUpdatedNotification>(this, (_, m) =>
        {
            var parsedId = Guid.Parse(m.SprintId);
            //tracer.Sprint.Update.NotificationReceived(GetType(), parsedId, m);

            var sprint = Sprints.FirstOrDefault(s => parsedId == s.SprintId);

            if (sprint is null)
            {
                //tracer.Sprint.Update.NoAggregateFound(GetType(), parsedId);
                return;
            }

            sprint.Apply(m);
            //tracer.Sprint.Update.ChangesApplied(GetType(), parsedId);
        });

        messenger.Register<TicketAddedToSprintNotification>(this, (_, m) =>
        {
            var parsedId = Guid.Parse(m.SprintId);
            //tracer.Sprint.AddTicketToSprint.NotificationReceived(GetType(), parsedId, m);

            var sprint = Sprints.FirstOrDefault(s => parsedId == s.SprintId);

            if (sprint is null)
            {
                //tracer.Sprint.AddTicketToSprint.NoAggregateFound(GetType(), parsedId);

                return;
            }

            sprint.Apply(m);
            //tracer.Sprint.AddTicketToSprint.ChangesApplied(GetType(), parsedId);
        });

        messenger.Register<SprintActiveStatusSetNotification>(this, (_, m) =>
        {
            var parsedId = Guid.Parse(m.SprintId);
            //tracer.Sprint.ActiveStatus.NotificationReceived(GetType(), parsedId, m);

            var sprint = Sprints.FirstOrDefault(s => parsedId == s.SprintId);

            if (sprint is null)
            {
                //tracer.Sprint.ActiveStatus.NoAggregateFound(GetType(), parsedId);

                return;
            }

            sprint.Apply(m);
            //tracer.Sprint.ActiveStatus.ChangesApplied(GetType(), parsedId);
        });
    }

    public async Task InitializeAsync()
    {
        Sprints.Clear();
        Sprints.AddRange(await requestSender.GetAllSprints());
    }

    public async Task UpdateSprint(SprintDto sprintDto)
    {
        var updateSprintDataCommand = new UpdateSprintDataCommand
        {
            SprintId = sprintDto.SprintId.ToString(),
            Name = sprintDto.Name,
            StartTime = Timestamp.FromDateTime(sprintDto.StartTime.UtcDateTime),
            EndTime = Timestamp.FromDateTime(sprintDto.EndTime.UtcDateTime)
        };

        await commandSender.Send(updateSprintDataCommand);
        //tracer.Sprint.ActiveStatus.CommandSent(GetType(), sprintDto.SprintId, updateSprintDataCommand);
    }

    public async Task CreateSprint(SprintDto newSprintDto)
    {
        var createSprintCommand = new CreateSprintCommand
        {
            SprintId = newSprintDto.SprintId.ToString(),
            Name = newSprintDto.Name,
            StartTime = Timestamp.FromDateTime(newSprintDto.StartTime.UtcDateTime),
            EndTime = Timestamp.FromDateTime(newSprintDto.EndTime.UtcDateTime),
            IsActive = newSprintDto.IsActive,
        };

        createSprintCommand.TicketIds.AddRange(newSprintDto.TicketIds.Select(id => id.ToString()));

        await commandSender.Send(createSprintCommand);
        //tracer.Sprint.Create.CommandSent(GetType(), newSprintDto.SprintId, createSprintCommand);
    }

    public async Task SetSprintActive(SprintDto selectedSprint)
    {
        var setSprintActiveStatusCommand = new SetSprintActiveStatusCommand
        {
            SprintId = selectedSprint.SprintId.ToString(),
            IsActive = true
        };

        await commandSender.Send(setSprintActiveStatusCommand);
        //tracer.Sprint.ActiveStatus.CommandSent(GetType(), selectedSprint.SprintId, setSprintActiveStatusCommand);

        //TODO This is wrong. Check it.
        //await mediator.Publish(new SprintSelectionChangedNotification());
    }
}