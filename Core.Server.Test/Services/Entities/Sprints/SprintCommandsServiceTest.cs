using Core.Server.Communication.Records.Commands.Entities.Sprints;
using Core.Server.Services.Entities.Sprints;
using Core.Server.Tracing.Tracing.Tracers;
using Core.Server.Translators.Commands.Sprints;
using Domain.Events.Sprints;
using Domain.Interfaces;
using Moq;
using SprintNotificationService = Core.Server.Communication.Endpoints.Sprint.SprintNotificationService;

namespace Core.Server.Test.Services.Entities.Sprints;

[TestFixture]
[TestOf(typeof(SprintCommandsService))]
public class SprintCommandsServiceTest
{
    private Mock<SprintNotificationService> _mockSprintNotificationService;
    private Mock<IEventStore<SprintEvent>> _mockEventStore;
    private Mock<ISprintCommandsToEventTranslator> _mockEventTranslator;
    private Mock<ITraceCollector> _mockTracer;

    private SprintCommandsService _instance;

    [SetUp]
    public void SetUp()
    {
        _mockSprintNotificationService = new Mock<SprintNotificationService>();
        _mockEventStore = new Mock<IEventStore<SprintEvent>>();
        _mockEventTranslator = new Mock<ISprintCommandsToEventTranslator>();
        _mockTracer = new Mock<ITraceCollector>()
        {
            DefaultValueProvider = DefaultValueProvider.Mock
        };

        _instance = new SprintCommandsService(
            _mockEventStore.Object,
            _mockSprintNotificationService.Object,
            _mockEventTranslator.Object,
            _mockTracer.Object);
    }

    [Test]
    public async Task Create()
    {
        var cmd = new CreateSprintCommand(
            Guid.NewGuid(),
            "Sprint 1",
            DateTimeOffset.Now,
            DateTimeOffset.Now.AddDays(14),
            true,
            new List<Guid> { Guid.NewGuid(), Guid.NewGuid() },
            Guid.NewGuid());

        await _instance.Create(cmd);

        _mockEventTranslator.Verify(et => et.ToEvent(cmd), Times.Once);
        _mockEventStore.Verify(es => es.StoreEventAsync(It.IsAny<SprintEvent>()), Times.Once);
    }

    [Test]
    public async Task UpdateSprintData()
    {
        var cmd = new UpdateSprintDataCommand(
            Guid.NewGuid(),
            "Sprint 1 - Updated",
            DateTimeOffset.Now,
            DateTimeOffset.Now.AddDays(21),
            Guid.NewGuid());

        await _instance.UpdateSprintData(cmd);

        _mockEventTranslator.Verify(et => et.ToEvent(cmd), Times.Once);
        _mockEventStore.Verify(es => es.StoreEventAsync(It.IsAny<SprintEvent>()), Times.Once);
    }

    [Test]
    public async Task SetSprintActiveStatus()
    {
        var cmd = new SetSprintActiveStatusCommand(
            Guid.NewGuid(),
            true,
            Guid.NewGuid());

        await _instance.SetSprintActiveStatus(cmd);

        _mockEventTranslator.Verify(et => et.ToEvent(cmd), Times.Once);
        _mockEventStore.Verify(es => es.StoreEventAsync(It.IsAny<SprintEvent>()), Times.Once);
    }

    [Test]
    public async Task AddTicketToSprint()
    {
        var cmd = new AddTicketToSprintCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid());

        await _instance.AddTicketToSprint(cmd);

        _mockEventTranslator.Verify(et => et.ToEvent(cmd), Times.Once);
        _mockEventStore.Verify(es => es.StoreEventAsync(It.IsAny<SprintEvent>()), Times.Once);
    }
}