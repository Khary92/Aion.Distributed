using Core.Server.Communication.Records.Commands.Entities.StatisticsData;
using Core.Server.Services.Entities.StatisticsData;
using Core.Server.Tracing.Tracing.Tracers;
using Core.Server.Translators.Commands.StatisticsData;
using Domain.Events.StatisticsData;
using Domain.Interfaces;
using Moq;
using StatisticsDataNotificationService =
    Core.Server.Communication.Endpoints.StatisticsData.StatisticsDataNotificationService;

namespace Core.Server.Test.Services.Entities.StatisticsData;

[TestFixture]
[TestOf(typeof(StatisticsDataCommandsService))]
public class StatisticsDataCommandsServiceTest
{
    [SetUp]
    public void SetUp()
    {
        _mockStatisticsDataNotificationService = new Mock<StatisticsDataNotificationService>();
        _mockEventStore = new Mock<IEventStore<StatisticsDataEvent>>();
        _mockEventTranslator = new Mock<IStatisticsDataCommandsToEventTranslator>();
        _mockTracer = new Mock<ITraceCollector>
        {
            DefaultValueProvider = DefaultValueProvider.Mock
        };

        _instance = new StatisticsDataCommandsService(
            _mockStatisticsDataNotificationService.Object,
            _mockEventStore.Object,
            _mockEventTranslator.Object,
            _mockTracer.Object);
    }

    private Mock<StatisticsDataNotificationService> _mockStatisticsDataNotificationService;
    private Mock<IEventStore<StatisticsDataEvent>> _mockEventStore;
    private Mock<IStatisticsDataCommandsToEventTranslator> _mockEventTranslator;
    private Mock<ITraceCollector> _mockTracer;

    private StatisticsDataCommandsService _instance;

    [Test]
    public async Task ChangeTagSelection()
    {
        var cmd = new ChangeTagSelectionCommand(
            Guid.NewGuid(),
            new List<Guid> { Guid.NewGuid(), Guid.NewGuid() },
            Guid.NewGuid());

        await _instance.ChangeTagSelection(cmd);

        _mockEventTranslator.Verify(et => et.ToEvent(cmd), Times.Once);
        _mockEventStore.Verify(es => es.StoreEventAsync(It.IsAny<StatisticsDataEvent>()), Times.Once);
    }

    [Test]
    public async Task ChangeProductivity()
    {
        var cmd = new ChangeProductivityCommand(
            Guid.NewGuid(),
            true,
            false,
            false,
            Guid.NewGuid());

        await _instance.ChangeProductivity(cmd);

        _mockEventTranslator.Verify(et => et.ToEvent(cmd), Times.Once);
        _mockEventStore.Verify(es => es.StoreEventAsync(It.IsAny<StatisticsDataEvent>()), Times.Once);
    }

    [Test]
    public async Task Create()
    {
        var cmd = new CreateStatisticsDataCommand(
            Guid.NewGuid(),
            true,
            false,
            false,
            new List<Guid> { Guid.NewGuid() },
            Guid.NewGuid(),
            Guid.NewGuid());

        await _instance.Create(cmd);

        _mockEventTranslator.Verify(et => et.ToEvent(cmd), Times.Once);
        _mockEventStore.Verify(es => es.StoreEventAsync(It.IsAny<StatisticsDataEvent>()), Times.Once);
    }
}