using Core.Server.Communication.Records.Commands.Entities.Tickets;
using Core.Server.Services.Entities.Tickets;
using Core.Server.Tracing.Tracing.Tracers;
using Core.Server.Translators.Commands.Tickets;
using Domain.Events.Tickets;
using Domain.Interfaces;
using Moq;
using TicketNotificationService = Core.Server.Communication.Endpoints.Ticket.TicketNotificationService;

namespace Core.Server.Test.Services.Entities.Tickets;

[TestFixture]
[TestOf(typeof(TicketCommandsService))]
public class TicketCommandsServiceTest
{
    [SetUp]
    public void SetUp()
    {
        _mockTicketNotificationService = new Mock<TicketNotificationService>();
        _mockEventStore = new Mock<ITicketEventsStore>();
        _mockEventTranslator = new Mock<ITicketCommandsToEventTranslator>();
        _mockTracer = new Mock<ITraceCollector>
        {
            DefaultValueProvider = DefaultValueProvider.Mock
        };

        _instance = new TicketCommandsService(
            _mockEventStore.Object,
            _mockTicketNotificationService.Object,
            _mockEventTranslator.Object,
            _mockTracer.Object);
    }

    private Mock<TicketNotificationService> _mockTicketNotificationService;
    private Mock<ITicketEventsStore> _mockEventStore;
    private Mock<ITicketCommandsToEventTranslator> _mockEventTranslator;
    private Mock<ITraceCollector> _mockTracer;

    private TicketCommandsService _instance;

    [Test]
    public async Task UpdateData()
    {
        var cmd = new UpdateTicketDataCommand(
            Guid.NewGuid(),
            "Ticket 1",
            "BN-123",
            new List<Guid> { Guid.NewGuid() },
            Guid.NewGuid());

        await _instance.UpdateData(cmd);

        _mockEventTranslator.Verify(et => et.ToEvent(cmd), Times.Once);
        _mockEventStore.Verify(es => es.StoreEventAsync(It.IsAny<TicketEvent>()), Times.Once);
    }

    [Test]
    public async Task UpdateDocumentation()
    {
        var cmd = new UpdateTicketDocumentationCommand(
            Guid.NewGuid(),
            "Some docs",
            Guid.NewGuid());

        await _instance.UpdateDocumentation(cmd);

        _mockEventTranslator.Verify(et => et.ToEvent(cmd), Times.Once);
        _mockEventStore.Verify(es => es.StoreEventAsync(It.IsAny<TicketEvent>()), Times.Once);
    }

    [Test]
    public async Task Create()
    {
        var cmd = new CreateTicketCommand(
            Guid.NewGuid(),
            "Ticket 1",
            "BN-123",
            new List<Guid> { Guid.NewGuid() },
            Guid.NewGuid());

        await _instance.Create(cmd);

        _mockEventTranslator.Verify(et => et.ToEvent(cmd), Times.Once);
        _mockEventStore.Verify(es => es.StoreEventAsync(It.IsAny<TicketEvent>()), Times.Once);
    }
}