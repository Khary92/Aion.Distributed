using Core.Server.Communication.Records.Commands.Entities.TimerSettings;
using Core.Server.Services.Entities.TimerSettings;
using Core.Server.Tracing.Tracing.Tracers;
using Core.Server.Translators.Commands.TimerSettings;
using Domain.Events.TimerSettings;
using Domain.Interfaces;
using Moq;
using TimerSettingsNotificationService =
    Core.Server.Communication.Endpoints.TimerSettings.TimerSettingsNotificationService;

namespace Core.Server.Test.Services.Entities.TimerSettings;

[TestFixture]
[TestOf(typeof(TimerSettingsCommandsService))]
public class TimerSettingsCommandsServiceTest
{
    private Mock<TimerSettingsNotificationService> _mockNotificationService;
    private Mock<IEventStore<TimerSettingsEvent>> _mockEventStore;
    private Mock<ITimerSettingsCommandsToEventTranslator> _mockEventTranslator;
    private Mock<ITraceCollector> _mockTracer;

    private TimerSettingsCommandsService _instance;

    [SetUp]
    public void SetUp()
    {
        _mockNotificationService = new Mock<TimerSettingsNotificationService>();
        _mockEventStore = new Mock<IEventStore<TimerSettingsEvent>>();
        _mockEventTranslator = new Mock<ITimerSettingsCommandsToEventTranslator>();
        _mockTracer = new Mock<ITraceCollector>()
        {
            DefaultValueProvider = DefaultValueProvider.Mock
        };

        _instance = new TimerSettingsCommandsService(
            _mockNotificationService.Object,
            _mockEventStore.Object,
            _mockEventTranslator.Object,
            _mockTracer.Object);
    }

    [Test]
    public async Task ChangeSnapshotInterval()
    {
        var cmd = new ChangeSnapshotSaveIntervalCommand(
            Guid.NewGuid(),
            5,
            Guid.NewGuid());

        await _instance.ChangeSnapshotInterval(cmd);

        _mockEventTranslator.Verify(et => et.ToEvent(cmd), Times.Once);
        _mockEventStore.Verify(es => es.StoreEventAsync(It.IsAny<TimerSettingsEvent>()), Times.Once);
    }

    [Test]
    public async Task ChangeDocumentationInterval()
    {
        var cmd = new ChangeDocuTimerSaveIntervalCommand(
            Guid.NewGuid(),
            5,
            Guid.NewGuid());

        await _instance.ChangeDocumentationInterval(cmd);

        _mockEventTranslator.Verify(et => et.ToEvent(cmd), Times.Once);
        _mockEventStore.Verify(es => es.StoreEventAsync(It.IsAny<TimerSettingsEvent>()), Times.Once);
    }

    [Test]
    public async Task Create()
    {
        var cmd = new CreateTimerSettingsCommand(
            Guid.NewGuid(),
            10,
            10,
            Guid.NewGuid());

        await _instance.Create(cmd);

        _mockEventTranslator.Verify(et => et.ToEvent(cmd), Times.Once);
        _mockEventStore.Verify(es => es.StoreEventAsync(It.IsAny<TimerSettingsEvent>()), Times.Once);
    }
}