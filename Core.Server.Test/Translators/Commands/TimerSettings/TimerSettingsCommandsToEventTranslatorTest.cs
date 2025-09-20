using System.Text.Json;
using Core.Server.Communication.Records.Commands.Entities.TimerSettings;
using Core.Server.Translators.Commands.TimerSettings;
using Domain.Events.TimerSettings;

namespace Core.Server.Test.Translators.Commands.TimerSettings;

[TestFixture]
[TestOf(typeof(TimerSettingsCommandsToEventTranslator))]
public class TimerSettingsCommandsToEventTranslatorTest
{
    private TimerSettingsCommandsToEventTranslator _translator;

    [SetUp]
    public void SetUp()
    {
        _translator = new TimerSettingsCommandsToEventTranslator();
    }

    [Test]
    public void ToEvent_ChangeDocuTimerSaveIntervalCommand_BuildsExpectedEvent()
    {
        var timerSettingsId = Guid.NewGuid();
        var interval = 20;
        var cmd = new ChangeDocuTimerSaveIntervalCommand(timerSettingsId, interval, Guid.NewGuid());

        var evt = _translator.ToEvent(cmd);

        Assert.That(evt, Is.Not.Null);
        Assert.That(evt.EventId, Is.Not.EqualTo(Guid.Empty));
        Assert.That(evt.EntityId, Is.EqualTo(timerSettingsId));
        Assert.That(evt.EventType, Is.EqualTo(nameof(DocuIntervalChangedEvent)));
        Assert.That(evt.EventPayload, Is.Not.Null.And.Not.Empty);

        var payload = JsonSerializer.Deserialize<DocuIntervalChangedEvent>(evt.EventPayload);
        Assert.That(payload, Is.Not.Null);
        Assert.That(payload!.TimerSettingsId, Is.EqualTo(timerSettingsId));
        Assert.That(payload.DocumentationSaveInterval, Is.EqualTo(interval));
    }

    [Test]
    public void ToEvent_ChangeSnapshotSaveIntervalCommand_BuildsExpectedEvent()
    {
        var timerSettingsId = Guid.NewGuid();
        var interval = 5;
        var cmd = new ChangeSnapshotSaveIntervalCommand(timerSettingsId, interval, Guid.NewGuid());

        var evt = _translator.ToEvent(cmd);

        Assert.That(evt, Is.Not.Null);
        Assert.That(evt.EventId, Is.Not.EqualTo(Guid.Empty));
        Assert.That(evt.EntityId, Is.EqualTo(timerSettingsId));
        Assert.That(evt.EventType, Is.EqualTo(nameof(SnapshotIntervalChangedEvent)));
        Assert.That(evt.EventPayload, Is.Not.Null.And.Not.Empty);

        var payload = JsonSerializer.Deserialize<SnapshotIntervalChangedEvent>(evt.EventPayload);
        Assert.That(payload, Is.Not.Null);
        Assert.That(payload!.TimerSettingsId, Is.EqualTo(timerSettingsId));
        Assert.That(payload.SnapshotSaveInterval, Is.EqualTo(interval));
    }

    [Test]
    public void ToEvent_CreateTimerSettingsCommand_BuildsExpectedEvent()
    {
        var timerSettingsId = Guid.NewGuid();
        var docuInterval = 15;
        var snapshotInterval = 2;
        var cmd = new CreateTimerSettingsCommand(timerSettingsId, docuInterval, snapshotInterval, Guid.NewGuid());

        var evt = _translator.ToEvent(cmd);

        Assert.That(evt, Is.Not.Null);
        Assert.That(evt.EventId, Is.Not.EqualTo(Guid.Empty));
        Assert.That(evt.EntityId, Is.EqualTo(timerSettingsId));
        Assert.That(evt.EventType, Is.EqualTo(nameof(TimerSettingsCreatedEvent)));
        Assert.That(evt.EventPayload, Is.Not.Null.And.Not.Empty);

        var payload = JsonSerializer.Deserialize<TimerSettingsCreatedEvent>(evt.EventPayload);
        Assert.That(payload, Is.Not.Null);
        Assert.That(payload!.TimerSettingsId, Is.EqualTo(timerSettingsId));
        Assert.That(payload.DocumentationSaveInterval, Is.EqualTo(docuInterval));
        Assert.That(payload.SnapshotSaveInterval, Is.EqualTo(snapshotInterval));
    }
}