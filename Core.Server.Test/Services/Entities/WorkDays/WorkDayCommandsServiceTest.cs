using Core.Server.Communication.Records.Commands.Entities.WorkDays;
using Core.Server.Services.Entities.WorkDays;
using Core.Server.Tracing.Tracing.Tracers;
using Core.Server.Translators.Commands.WorkDays;
using Domain.Events.WorkDays;
using Domain.Interfaces;
using Moq;
using WorkDayNotificationService = Core.Server.Communication.Endpoints.WorkDay.WorkDayNotificationService;

namespace Core.Server.Test.Services.Entities.WorkDays;

[TestFixture]
[TestOf(typeof(WorkDayCommandsService))]
public class WorkDayCommandsServiceTest
{
    private Mock<WorkDayNotificationService> _mockNotificationService;
    private Mock<IEventStore<WorkDayEvent>> _mockEventStore;
    private Mock<IWorkDayCommandsToEventTranslator> _mockEventTranslator;
    private Mock<IWorkDayRequestsService> _mockRequestsService;
    private Mock<ITraceCollector> _mockTracer;

    private WorkDayCommandsService _instance;

    [SetUp]
    public void SetUp()
    {
        _mockNotificationService = new Mock<WorkDayNotificationService>();
        _mockEventStore = new Mock<IEventStore<WorkDayEvent>>();
        _mockEventTranslator = new Mock<IWorkDayCommandsToEventTranslator>();
        _mockRequestsService = new Mock<IWorkDayRequestsService>();
        _mockTracer = new Mock<ITraceCollector>()
        {
            DefaultValueProvider = DefaultValueProvider.Mock
        };

        _mockRequestsService
            .Setup(s => s.GetAll())
            .ReturnsAsync(new List<Domain.Entities.WorkDay>());

        _instance = new WorkDayCommandsService(
            _mockEventStore.Object,
            _mockNotificationService.Object,
            _mockEventTranslator.Object,
            _mockRequestsService.Object,
            _mockTracer.Object);
    }

    [Test]
    public async Task Create()
    {
        var cmd = new CreateWorkDayCommand(Guid.NewGuid(), DateTimeOffset.Now, Guid.NewGuid());

        await _instance.Create(cmd);

        _mockEventTranslator.Verify(et => et.ToEvent(cmd), Times.Once);
        _mockEventStore.Verify(es => es.StoreEventAsync(It.IsAny<WorkDayEvent>()), Times.Once);
    }
}