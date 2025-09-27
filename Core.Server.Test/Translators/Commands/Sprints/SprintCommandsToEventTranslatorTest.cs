using System.Text.Json;
using Core.Server.Communication.Records.Commands.Entities.Sprints;
using Core.Server.Translators.Commands.Sprints;
using Domain.Events.Sprints;

namespace Core.Server.Test.Translators.Commands.Sprints;

[TestFixture]
[TestOf(typeof(SprintCommandsToEventTranslator))]
public class SprintCommandsToEventTranslatorTest
{
    [SetUp]
    public void SetUp()
    {
        _translator = new SprintCommandsToEventTranslator();
    }

    private SprintCommandsToEventTranslator _translator;

    [Test]
    public void ToEvent_CreateSprintCommand_BuildsExpectedSprintEvent()
    {
        var sprintId = Guid.NewGuid();
        var start = DateTimeOffset.UtcNow;
        var end = start.AddDays(14);
        var ticketIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
        var cmd = new CreateSprintCommand(sprintId, "Sprint 1", start, end, true, ticketIds, Guid.NewGuid());

        var evt = _translator.ToEvent(cmd);

        Assert.That(evt, Is.Not.Null);
        Assert.That(evt.EventId, Is.Not.EqualTo(Guid.Empty));
        Assert.That(evt.EntityId, Is.EqualTo(sprintId));
        Assert.That(evt.EventType, Is.EqualTo(nameof(SprintCreatedEvent)));
        Assert.That(evt.EventPayload, Is.Not.Null.And.Not.Empty);

        var payload = JsonSerializer.Deserialize<SprintCreatedEvent>(evt.EventPayload);
        Assert.That(payload, Is.Not.Null);
        Assert.That(payload!.SprintId, Is.EqualTo(sprintId));
        Assert.That(payload.Name, Is.EqualTo("Sprint 1"));
        Assert.That(payload.StartDate, Is.EqualTo(start));
        Assert.That(payload.EndDate, Is.EqualTo(end));
        Assert.That(payload.IsActive, Is.True);
        Assert.That(payload.TicketIds, Is.EquivalentTo(ticketIds));
    }

    [Test]
    public void ToEvent_UpdateSprintDataCommand_BuildsExpectedSprintEvent()
    {
        var sprintId = Guid.NewGuid();
        var start = DateTimeOffset.UtcNow;
        var end = start.AddDays(21);
        var cmd = new UpdateSprintDataCommand(sprintId, "Sprint 1 - Updated", start, end, Guid.NewGuid());

        var evt = _translator.ToEvent(cmd);

        Assert.That(evt, Is.Not.Null);
        Assert.That(evt.EventId, Is.Not.EqualTo(Guid.Empty));
        Assert.That(evt.EntityId, Is.EqualTo(sprintId));
        Assert.That(evt.EventType, Is.EqualTo(nameof(SprintDataUpdatedEvent)));
        Assert.That(evt.EventPayload, Is.Not.Null.And.Not.Empty);

        var payload = JsonSerializer.Deserialize<SprintDataUpdatedEvent>(evt.EventPayload);
        Assert.That(payload, Is.Not.Null);
        Assert.That(payload!.SprintId, Is.EqualTo(sprintId));
        Assert.That(payload.Name, Is.EqualTo("Sprint 1 - Updated"));
        Assert.That(payload.StartDate, Is.EqualTo(start));
        Assert.That(payload.EndDate, Is.EqualTo(end));
    }

    [Test]
    public void ToEvent_SetSprintActiveStatusCommand_BuildsExpectedSprintEvent()
    {
        var sprintId = Guid.NewGuid();
        var cmd = new SetSprintActiveStatusCommand(sprintId, true, Guid.NewGuid());

        var evt = _translator.ToEvent(cmd);

        Assert.That(evt, Is.Not.Null);
        Assert.That(evt.EventId, Is.Not.EqualTo(Guid.Empty));
        Assert.That(evt.EntityId, Is.EqualTo(sprintId));
        Assert.That(evt.EventType, Is.EqualTo(nameof(SprintActiveStatusChangedEvent)));
        Assert.That(evt.EventPayload, Is.Not.Null.And.Not.Empty);

        var payload = JsonSerializer.Deserialize<SprintActiveStatusChangedEvent>(evt.EventPayload);
        Assert.That(payload, Is.Not.Null);
        Assert.That(payload!.SprintId, Is.EqualTo(sprintId));
        Assert.That(payload.IsActive, Is.True);
    }

    [Test]
    public void ToEvent_AddTicketToSprintCommand_BuildsExpectedSprintEvent()
    {
        var sprintId = Guid.NewGuid();
        var ticketId = Guid.NewGuid();
        var cmd = new AddTicketToSprintCommand(sprintId, ticketId, Guid.NewGuid());

        var evt = _translator.ToEvent(cmd);

        Assert.That(evt, Is.Not.Null);
        Assert.That(evt.EventId, Is.Not.EqualTo(Guid.Empty));
        Assert.That(evt.EntityId, Is.EqualTo(sprintId));
        Assert.That(evt.EventType, Is.EqualTo(nameof(TicketAddedToSprintEvent)));
        Assert.That(evt.EventPayload, Is.Not.Null.And.Not.Empty);

        var payload = JsonSerializer.Deserialize<TicketAddedToSprintEvent>(evt.EventPayload);
        Assert.That(payload, Is.Not.Null);
        Assert.That(payload!.SprintId, Is.EqualTo(sprintId));
        Assert.That(payload.TicketId, Is.EqualTo(ticketId));
    }
}