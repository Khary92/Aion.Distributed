using Core.Server.Services.Entities.Sprints;
using Core.Server.Services.Entities.Tickets;
using Domain.Entities;
using Domain.Interfaces;
using Moq;

namespace Core.Server.Test.Services.Entities.Tickets;

[TestFixture]
[TestOf(typeof(TicketRequestService))]
public class TicketRequestServiceTest
{
    [SetUp]
    public void SetUp()
    {
        _mockEventStore = new Mock<ITicketEventsStore>();
        _mockEventStore
            .Setup(es => es.GetAllEventsAsync())
            .ReturnsAsync([]);
        _mockEventStore
            .Setup(es => es.GetEventsForAggregateAsync(It.IsAny<Guid>()))
            .ReturnsAsync([]);
        _mockEventStore
            .Setup(es => es.GetTicketDocumentationEventsByTicketId(It.IsAny<Guid>()))
            .ReturnsAsync([]);

        _mockSprintRequests = new Mock<ISprintRequestsService>();

        _instance = new TicketRequestService(
            _mockEventStore.Object,
            _mockSprintRequests.Object);
    }

    private Mock<ITicketEventsStore> _mockEventStore;
    private Mock<ISprintRequestsService> _mockSprintRequests;
    private TicketRequestService _instance;

    [Test]
    public async Task GetAll_CallsGetAllEventsAsync()
    {
        await _instance.GetAll();

        _mockEventStore.Verify(es => es.GetAllEventsAsync(), Times.Once);
    }

    [Test]
    public async Task GetTicketById_CallsGetEventsForAggregateAsync()
    {
        var id = Guid.NewGuid();

        await _instance.GetTicketById(id);

        _mockEventStore.Verify(es => es.GetEventsForAggregateAsync(id), Times.Once);
    }

    [Test]
    public async Task GetTicketsBySprintId_CallsGetAllEventsAsync()
    {
        await _instance.GetTicketsBySprintId(Guid.NewGuid());

        _mockEventStore.Verify(es => es.GetAllEventsAsync(), Times.Once);
    }

    [Test]
    public async Task GetTicketsForCurrentSprint_NoActiveSprint_CallsOnlyGetActiveSprint()
    {
        _mockSprintRequests
            .Setup(s => s.GetActiveSprint())
            .ReturnsAsync((Sprint?)null);

        var result = await _instance.GetTicketsForCurrentSprint();

        Assert.That(result, Is.Empty);
        _mockSprintRequests.Verify(s => s.GetActiveSprint(), Times.Once);
        _mockEventStore.Verify(es => es.GetAllEventsAsync(), Times.Never);
    }

    [Test]
    public async Task GetTicketsForCurrentSprint_WithActiveSprint_CallsGetAllEventsAsync()
    {
        var activeSprint = new Sprint
        {
            SprintId = Guid.NewGuid(),
            Name = "Sprint 1",
            StartDate = DateTimeOffset.Now,
            EndDate = DateTimeOffset.Now.AddDays(7),
            IsActive = true
        };

        _mockSprintRequests
            .Setup(s => s.GetActiveSprint())
            .ReturnsAsync(activeSprint);

        await _instance.GetTicketsForCurrentSprint();

        _mockSprintRequests.Verify(s => s.GetActiveSprint(), Times.Once);
        _mockEventStore.Verify(es => es.GetAllEventsAsync(), Times.Once);
    }

    [Test]
    public async Task GetDocumentationByTicketId_CallsGetTicketDocumentationEventsByTicketId()
    {
        var id = Guid.NewGuid();

        await _instance.GetDocumentationByTicketId(id);

        _mockEventStore.Verify(es => es.GetTicketDocumentationEventsByTicketId(id), Times.Once);
    }
}