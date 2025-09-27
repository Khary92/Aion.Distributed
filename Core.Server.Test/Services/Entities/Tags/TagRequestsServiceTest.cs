using Core.Server.Services.Entities.Tags;
using Domain.Events.Tags;
using Domain.Interfaces;
using Moq;

namespace Core.Server.Test.Services.Entities.Tags;

[TestFixture]
[TestOf(typeof(TagRequestsService))]
public class TagRequestsServiceTest
{
    [SetUp]
    public void SetUp()
    {
        _mockEventStore = new Mock<IEventStore<TagEvent>>();
        _mockEventStore
            .Setup(es => es.GetAllEventsAsync())
            .ReturnsAsync(new List<TagEvent>());
        _mockEventStore
            .Setup(es => es.GetEventsForAggregateAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new List<TagEvent>());

        _instance = new TagRequestsService(_mockEventStore.Object);
    }

    private Mock<IEventStore<TagEvent>> _mockEventStore;
    private TagRequestsService _instance;

    [Test]
    public async Task GetAll_CallsGetAllEventsAsync()
    {
        await _instance.GetAll();

        _mockEventStore.Verify(es => es.GetAllEventsAsync(), Times.Once);
    }

    [Test]
    public async Task GetTagById_CallsGetEventsForAggregateAsync()
    {
        var id = Guid.NewGuid();

        await _instance.GetTagById(id);

        _mockEventStore.Verify(es => es.GetEventsForAggregateAsync(id), Times.Once);
    }

    [Test]
    public async Task GetTagsByTagIds_CallsGetAllEventsAsync_Indirectly()
    {
        await _instance.GetTagsByTagIds(new List<Guid> { Guid.NewGuid() });

        _mockEventStore.Verify(es => es.GetAllEventsAsync(), Times.Once);
    }
}