using Core.Server.Services.Entities.Sprints;
using Domain.Events.Sprints;
using Domain.Interfaces;
using Moq;

namespace Core.Server.Test.Services.Entities.Sprints;

[TestFixture]
[TestOf(typeof(SprintRequestService))]
public class SprintRequestServiceTest
{
    [SetUp]
    public void SetUp()
    {
        _mockEventStore = new Mock<IEventStore<SprintEvent>>();
        _mockEventStore
            .Setup(es => es.GetAllEventsAsync())
            .ReturnsAsync(new List<SprintEvent>());
        _mockEventStore
            .Setup(es => es.GetEventsForAggregateAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new List<SprintEvent>());

        _instance = new SprintRequestService(_mockEventStore.Object);
    }

    private Mock<IEventStore<SprintEvent>> _mockEventStore;
    private SprintRequestService _instance;

    [Test]
    public async Task GetAll_CallsGetAllEventsAsync()
    {
        await _instance.GetAll();

        _mockEventStore.Verify(es => es.GetAllEventsAsync(), Times.Once);
    }

    [Test]
    public async Task GetById_CallsGetEventsForAggregateAsync()
    {
        var id = Guid.NewGuid();

        await _instance.GetById(id);

        _mockEventStore.Verify(es => es.GetEventsForAggregateAsync(id), Times.Once);
    }

    [Test]
    public async Task GetActiveSprint_CallsGetAllEventsAsync()
    {
        await _instance.GetActiveSprint();

        _mockEventStore.Verify(es => es.GetAllEventsAsync(), Times.Once);
    }
}