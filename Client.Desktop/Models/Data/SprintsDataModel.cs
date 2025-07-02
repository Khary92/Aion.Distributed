using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Client.Desktop.Communication.Commands;
using Client.Desktop.Communication.NotificationWrappers;
using Client.Desktop.Communication.Requests;
using Client.Desktop.Converter;
using Client.Desktop.DTO;
using Client.Tracing.Tracing.Tracers;
using CommunityToolkit.Mvvm.Messaging;
using DynamicData;
using Google.Protobuf.WellKnownTypes;
using Proto.Command.Sprints;
using Proto.Notifications.Sprint;
using Proto.Requests.Sprints;
using ReactiveUI;

namespace Client.Desktop.Models.Data;

public class SprintsDataModel(
    ICommandSender commandSender,
    IRequestSender requestSender,
    IMessenger messenger,
    ITraceCollector tracer)
    : ReactiveObject
{
    public ObservableCollection<SprintDto> Sprints { get; } = [];

    public void RegisterMessenger()
    {
        messenger.Register<NewSprintMessage>(this, async (_, m) =>
        {
            await tracer.Sprint.Create.AggregateReceived(GetType(), m.Sprint.SprintId, m.Sprint.AsTraceAttributes());
            Sprints.Add(m.Sprint);
            await tracer.Sprint.Create.AggregateAdded(GetType(), m.Sprint.SprintId);
        });

        messenger.Register<SprintDataUpdatedNotification>(this, async (_, m) =>
        {
            var parsedId = Guid.Parse(m.SprintId);
            await tracer.Sprint.Update.NotificationReceived(GetType(), parsedId, m);

            var sprint = Sprints.FirstOrDefault(s => parsedId == s.SprintId);

            if (sprint is null)
            {
                await tracer.Sprint.Update.NoAggregateFound(GetType(), parsedId);
                return;
            }
                
            sprint.Apply(m);
            await tracer.Sprint.Update.ChangesApplied(GetType(), parsedId);
        });

        messenger.Register<TicketAddedToSprintNotification>(this, async (_, m) =>
        {
            var parsedId = Guid.Parse(m.SprintId);
            await tracer.Sprint.AddTicketToSprint.NotificationReceived(GetType(), parsedId, m);

            var sprint = Sprints.FirstOrDefault(s => parsedId == s.SprintId);

            if (sprint is null)
            {
                await tracer.Sprint.AddTicketToSprint.NoAggregateFound(GetType(), parsedId);
                return;
            }

            sprint.Apply(m);
            await tracer.Sprint.AddTicketToSprint.ChangesApplied(GetType(), parsedId);
        });

        messenger.Register<SprintActiveStatusSetNotification>(this, async (_, m) =>
        {
            var parsedId = Guid.Parse(m.SprintId);
            await tracer.Sprint.ActiveStatus.NotificationReceived(GetType(), parsedId, m);

            var sprint = Sprints.FirstOrDefault(s => parsedId == s.SprintId);

            if (sprint is null) {
                await tracer.Sprint.ActiveStatus.NoAggregateFound(GetType(), parsedId);
                return;
            }
            
            sprint.Apply(m);
            await tracer.Sprint.ActiveStatus.ChangesApplied(GetType(), parsedId);
        });
    }

    public async Task InitializeAsync()
    {
        Sprints.Clear();
        Sprints.AddRange(await requestSender.Send(new GetAllSprintsRequestProto()));
    }

    public async Task UpdateSprint(SprintDto sprintDto)
    {
        var updateSprintDataCommand = new UpdateSprintDataCommandProto
        {
            SprintId = sprintDto.SprintId.ToString(),
            Name = sprintDto.Name,
            StartTime = Timestamp.FromDateTime(sprintDto.StartTime.UtcDateTime),
            EndTime = Timestamp.FromDateTime(sprintDto.EndTime.UtcDateTime)
        };

        await commandSender.Send(updateSprintDataCommand);
        await tracer.Sprint.ActiveStatus.CommandSent(GetType(), sprintDto.SprintId, updateSprintDataCommand);
    }

    public async Task CreateSprint(SprintDto newSprintDto)
    {
        var createSprintCommand = new CreateSprintCommandProto
        {
            SprintId = newSprintDto.SprintId.ToString(),
            Name = newSprintDto.Name,
            StartTime = Timestamp.FromDateTime(newSprintDto.StartTime.UtcDateTime),
            EndTime = Timestamp.FromDateTime(newSprintDto.EndTime.UtcDateTime),
            IsActive = newSprintDto.IsActive
        };

        createSprintCommand.TicketIds.AddRange(newSprintDto.TicketIds.Select(id => id.ToString()));

        await commandSender.Send(createSprintCommand);
        await tracer.Sprint.Create.CommandSent(GetType(), newSprintDto.SprintId, createSprintCommand);
    }

    public async Task SetSprintActive(SprintDto selectedSprint)
    {
        var setSprintActiveStatusCommand = new SetSprintActiveStatusCommandProto
        {
            SprintId = selectedSprint.SprintId.ToString(),
            IsActive = true
        };

        await commandSender.Send(setSprintActiveStatusCommand);
        await tracer.Sprint.ActiveStatus.CommandSent(GetType(), selectedSprint.SprintId, setSprintActiveStatusCommand);

        //TODO This is wrong. Check it.
        //await mediator.Publish(new SprintSelectionChangedNotification());
    }
}