using Core.Server.Communication.Records.Commands.Entities.TimeSlots;
using Core.Server.Services.Entities.TimeSlots;
using Core.Server.Tracing.Tracing.Tracers;
using Core.Server.Translators.Commands.TimeSlots;
using Domain.Events.TimeSlots;
using Domain.Interfaces;
using Moq;
using TimeSlotNotificationService = Core.Server.Communication.Endpoints.TimeSlot.TimeSlotNotificationService;

namespace Core.Server.Test.Services.Entities.TimeSlots;

[TestFixture]
[TestOf(typeof(TimeSlotCommandService))]
public class TimeSlotCommandServiceTest
{
    private Mock<TimeSlotNotificationService> _mockNotificationService;
    private Mock<IEventStore<TimeSlotEvent>> _mockEventStore;
    private Mock<ITimeSlotCommandsToEventTranslator> _mockEventTranslator;
    private Mock<ITraceCollector> _mockTracer;

    private TimeSlotCommandService _instance;

    [SetUp]
    public void SetUp()
    {
        _mockNotificationService = new Mock<TimeSlotNotificationService>();
        _mockEventStore = new Mock<IEventStore<TimeSlotEvent>>();
        _mockEventTranslator = new Mock<ITimeSlotCommandsToEventTranslator>();
        _mockTracer = new Mock<ITraceCollector>()
        {
            DefaultValueProvider = DefaultValueProvider.Mock
        };

        _instance = new TimeSlotCommandService(
            _mockNotificationService.Object,
            _mockEventStore.Object,
            _mockEventTranslator.Object,
            _mockTracer.Object);
    }

    [Test]
    public async Task SetEndTime()
    {
        var cmd = new SetEndTimeCommand(Guid.NewGuid(), DateTimeOffset.Now, Guid.NewGuid());

        await _instance.SetEndTime(cmd);

        _mockEventTranslator.Verify(et => et.ToEvent(cmd), Times.Once);
        _mockEventStore.Verify(es => es.StoreEventAsync(It.IsAny<TimeSlotEvent>()), Times.Once);
    }

    [Test]
    public async Task SetStartTime()
    {
        var cmd = new SetStartTimeCommand(Guid.NewGuid(), DateTimeOffset.Now, Guid.NewGuid());

        await _instance.SetStartTime(cmd);

        _mockEventTranslator.Verify(et => et.ToEvent(cmd), Times.Once);
        _mockEventStore.Verify(es => es.StoreEventAsync(It.IsAny<TimeSlotEvent>()), Times.Once);
    }

    [Test]
    public async Task AddNote()
    {
        var cmd = new AddNoteCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

        await _instance.AddNote(cmd);

        _mockEventTranslator.Verify(et => et.ToEvent(cmd), Times.Once);
        _mockEventStore.Verify(es => es.StoreEventAsync(It.IsAny<TimeSlotEvent>()), Times.Once);
    }

    [Test]
    public async Task Create()
    {
        var cmd = new CreateTimeSlotCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            DateTimeOffset.Now,
            DateTimeOffset.Now.AddHours(1),
            true,
            Guid.NewGuid());

        await _instance.Create(cmd);

        _mockEventTranslator.Verify(et => et.ToEvent(cmd), Times.Once);
        _mockEventStore.Verify(es => es.StoreEventAsync(It.IsAny<TimeSlotEvent>()), Times.Once);
    }
}