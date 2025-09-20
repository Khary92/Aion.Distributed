using System.Text.Json;
using Core.Server.Communication.Records.Commands.Entities.WorkDays;
using Core.Server.Translators.Commands.WorkDays;
using Domain.Events.WorkDays;

namespace Core.Server.Test.Translators.Commands.WorkDays;

[TestFixture]
[TestOf(typeof(WorkDayCommandsToEventTranslator))]
public class WorkDayCommandsToEventTranslatorTest
{
    private WorkDayCommandsToEventTranslator _translator;

    [SetUp]
    public void SetUp()
    {
        _translator = new WorkDayCommandsToEventTranslator();
    }

    [Test]
    public void ToEvent_CreateWorkDayCommand_BuildsExpectedWorkDayEvent()
    {
        var workDayId = Guid.NewGuid();
        var date = DateTimeOffset.UtcNow.Date;
        var cmd = new CreateWorkDayCommand(workDayId, date, Guid.NewGuid());

        var evt = _translator.ToEvent(cmd);

        Assert.That(evt, Is.Not.Null);
        Assert.That(evt.EventId, Is.Not.EqualTo(Guid.Empty));
        Assert.That(evt.EntityId, Is.EqualTo(workDayId));
        Assert.That(evt.EventType, Is.EqualTo(nameof(WorkDayCreatedEvent)));
        Assert.That(evt.EventPayload, Is.Not.Null.And.Not.Empty);

        var payload = JsonSerializer.Deserialize<WorkDayCreatedEvent>(evt.EventPayload);
        Assert.That(payload, Is.Not.Null);
        Assert.That(payload!.WorkDayId, Is.EqualTo(workDayId));
    }
}