using System.Text.Json;
using Core.Server.Communication.Records.Commands.Entities.Tickets;
using Core.Server.Translators.Commands.Tickets;
using Domain.Events.Tickets;
using NUnit.Framework.Legacy;

namespace Core.Server.Test.Translators.Commands.Tickets;

[TestFixture]
[TestOf(typeof(TicketCommandsToEventTranslator))]
public class TicketCommandsToEventTranslatorTest
{
    private TicketCommandsToEventTranslator _translator;

    [SetUp]
    public void SetUp()
    {
        _translator = new TicketCommandsToEventTranslator();
    }

    [Test]
    public void ToEvent_CreateTicketCommand_BuildsExpectedTicketEvent()
    {
        var ticketId = Guid.NewGuid();
        var sprintIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
        var cmd = new CreateTicketCommand(ticketId, "T-1", "BN-001", sprintIds, Guid.NewGuid());

        var evt = _translator.ToEvent(cmd);

        Assert.That(evt, Is.Not.Null);
        Assert.That(evt.EventId, Is.Not.EqualTo(Guid.Empty));
        Assert.That(evt.EntityId, Is.EqualTo(ticketId));
        Assert.That(evt.EventType, Is.EqualTo(nameof(TicketCreatedEvent)));
        Assert.That(evt.EventPayload, Is.Not.Null.And.Not.Empty);

        var payload = JsonSerializer.Deserialize<TicketCreatedEvent>(evt.EventPayload);
        Assert.That(payload, Is.Not.Null);
        Assert.That(payload!.TicketId, Is.EqualTo(ticketId));
        Assert.That(payload.Name, Is.EqualTo("T-1"));
        Assert.That(payload.BookingNumber, Is.EqualTo("BN-001"));
        Assert.That(payload.Documentation, Is.EqualTo(string.Empty));
        Assert.That(payload.SprintIds, Is.EquivalentTo(sprintIds));
    }

    [Test]
    public void ToEvent_UpdateTicketDataCommand_BuildsExpectedTicketEvent()
    {
        var ticketId = Guid.NewGuid();
        var sprintIds = new List<Guid> { Guid.NewGuid() };
        var cmd = new UpdateTicketDataCommand(ticketId, "T-1 Updated", "BN-999", sprintIds, Guid.NewGuid());

        var evt = _translator.ToEvent(cmd);

        Assert.That(evt, Is.Not.Null);
        Assert.That(evt.EventId, Is.Not.EqualTo(Guid.Empty));
        Assert.That(evt.EntityId, Is.EqualTo(ticketId));
        Assert.That(evt.EventType, Is.EqualTo(nameof(TicketDataUpdatedEvent)));
        Assert.That(evt.EventPayload, Is.Not.Null.And.Not.Empty);

        var payload = JsonSerializer.Deserialize<TicketDataUpdatedEvent>(evt.EventPayload);
        Assert.That(payload, Is.Not.Null);
        Assert.That(payload!.TicketId, Is.EqualTo(ticketId));
        Assert.That(payload.Name, Is.EqualTo("T-1 Updated"));
        Assert.That(payload.BookingNumber, Is.EqualTo("BN-999"));
        Assert.That(payload.SprintIds, Is.EquivalentTo(sprintIds));
    }

    [Test]
    public void ToEvent_UpdateTicketDocumentationCommand_BuildsExpectedTicketEvent()
    {
        var ticketId = Guid.NewGuid();
        var cmd = new UpdateTicketDocumentationCommand(ticketId, "Some docs", Guid.NewGuid());

        var evt = _translator.ToEvent(cmd);

        Assert.That(evt, Is.Not.Null);
        Assert.That(evt.EventId, Is.Not.EqualTo(Guid.Empty));
        Assert.That(evt.EntityId, Is.EqualTo(ticketId));
        Assert.That(evt.EventType, Is.EqualTo(nameof(TicketDocumentationChangedEvent)));
        Assert.That(evt.EventPayload, Is.Not.Null.And.Not.Empty);

        var payload = JsonSerializer.Deserialize<TicketDocumentationChangedEvent>(evt.EventPayload);
        Assert.That(payload, Is.Not.Null);
        Assert.That(payload!.TicketId, Is.EqualTo(ticketId));
        Assert.That(payload.Documentation, Is.EqualTo("Some docs"));
    }
}