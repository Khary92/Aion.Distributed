using System.Text.Json;
using Core.Server.Communication.Records.Commands.Entities.StatisticsData;
using Core.Server.Translators.Commands.StatisticsData;
using Domain.Events.StatisticsData;

namespace Core.Server.Test.Translators.Commands.StatisticsData;

[TestFixture]
[TestOf(typeof(StatisticsDataCommandsToEventTranslator))]
public class StatisticsDataCommandsToEventTranslatorTest
{
    [SetUp]
    public void SetUp()
    {
        _translator = new StatisticsDataCommandsToEventTranslator();
    }

    private StatisticsDataCommandsToEventTranslator _translator;

    [Test]
    public void ToEvent_CreateStatisticsDataCommand_BuildsExpectedEvent()
    {
        var statisticsDataId = Guid.NewGuid();
        var timeSlotId = Guid.NewGuid();
        var tagIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
        var cmd = new CreateStatisticsDataCommand(
            statisticsDataId,
            true,
            false,
            false,
            tagIds,
            timeSlotId,
            Guid.NewGuid());

        var evt = _translator.ToEvent(cmd);

        Assert.That(evt, Is.Not.Null);
        Assert.That(evt.EventId, Is.Not.EqualTo(Guid.Empty));
        Assert.That(evt.EntityId, Is.EqualTo(statisticsDataId));
        Assert.That(evt.EventType, Is.EqualTo(nameof(StatisticsDataCreatedEvent)));
        Assert.That(evt.EventPayload, Is.Not.Null.And.Not.Empty);

        var payload = JsonSerializer.Deserialize<StatisticsDataCreatedEvent>(evt.EventPayload);
        Assert.That(payload, Is.Not.Null);
        Assert.That(payload!.StatisticsDataId, Is.EqualTo(statisticsDataId));
        Assert.That(payload.IsProductive, Is.True);
        Assert.That(payload.IsNeutral, Is.False);
        Assert.That(payload.IsUnproductive, Is.False);
        Assert.That(payload.TagIds, Is.EquivalentTo(tagIds));
        Assert.That(payload.TimeSlotId, Is.EqualTo(timeSlotId));
    }

    [Test]
    public void ToEvent_ChangeProductivityCommand_BuildsExpectedEvent()
    {
        var statisticsDataId = Guid.NewGuid();
        var cmd = new ChangeProductivityCommand(
            statisticsDataId,
            true,
            true,
            true,
            Guid.NewGuid());

        var evt = _translator.ToEvent(cmd);

        Assert.That(evt, Is.Not.Null);
        Assert.That(evt.EventId, Is.Not.EqualTo(Guid.Empty));
        Assert.That(evt.EntityId, Is.EqualTo(statisticsDataId));
        Assert.That(evt.EventType, Is.EqualTo(nameof(ProductivityChangedEvent)));
        Assert.That(evt.EventPayload, Is.Not.Null.And.Not.Empty);

        var payload = JsonSerializer.Deserialize<ProductivityChangedEvent>(evt.EventPayload);
        Assert.That(payload, Is.Not.Null);
        Assert.That(payload!.StatisticsDataId, Is.EqualTo(statisticsDataId));
        Assert.That(payload.IsProductive, Is.True);
        Assert.That(payload.IsNeutral, Is.True);
        Assert.That(payload.IsUnproductive, Is.True);
    }

    [Test]
    public void ToEvent_ChangeTagSelectionCommand_BuildsExpectedEvent()
    {
        var statisticsDataId = Guid.NewGuid();
        var selectedTags = new List<Guid> { Guid.NewGuid() };
        var cmd = new ChangeTagSelectionCommand(
            statisticsDataId,
            selectedTags,
            Guid.NewGuid());

        var evt = _translator.ToEvent(cmd);

        Assert.That(evt, Is.Not.Null);
        Assert.That(evt.EventId, Is.Not.EqualTo(Guid.Empty));
        Assert.That(evt.EntityId, Is.EqualTo(statisticsDataId));
        Assert.That(evt.EventType, Is.EqualTo(nameof(TagSelectionChangedEvent)));
        Assert.That(evt.EventPayload, Is.Not.Null.And.Not.Empty);

        var payload = JsonSerializer.Deserialize<TagSelectionChangedEvent>(evt.EventPayload);
        Assert.That(payload, Is.Not.Null);
        Assert.That(payload!.StatisticsId, Is.EqualTo(statisticsDataId));
        Assert.That(payload.SelectedTagIds, Is.EquivalentTo(selectedTags));
    }
}