using Core.Server.Services.Entities.NoteTypes;
using Domain.Events.NoteTypes;
using Domain.Interfaces;
using Moq;

namespace Core.Server.Test.Services.Entities.NoteTypes;

[TestFixture]
[TestOf(typeof(NoteTypeRequestsService))]
public class NoteTypeRequestsServiceTest
{
    private Mock<IEventStore<NoteTypeEvent>> _mockEventStore;
    private NoteTypeRequestsService _instance;

    [SetUp]
    public void SetUp()
    {
        _mockEventStore = new Mock<IEventStore<NoteTypeEvent>>();
        _mockEventStore
            .Setup(es => es.GetAllEventsAsync())
            .ReturnsAsync(new List<NoteTypeEvent>());
        _mockEventStore
            .Setup(es => es.GetEventsForAggregateAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new List<NoteTypeEvent>());

        _instance = new NoteTypeRequestsService(_mockEventStore.Object);
    }

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
}