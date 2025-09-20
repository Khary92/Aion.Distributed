using Core.Server.Services.Entities.TimeSlots;
using Domain.Events.TimeSlots;
using Domain.Interfaces;
using Moq;

namespace Core.Server.Test.Services.Entities.TimeSlots;

[TestFixture]
[TestOf(typeof(TimeSlotRequestsService))]
public class TimeSlotRequestsServiceTest
{
    private Mock<IEventStore<TimeSlotEvent>> _mockEventStore;
    private TimeSlotRequestsService _instance;

    [SetUp]
    public void SetUp()
    {
        _mockEventStore = new Mock<IEventStore<TimeSlotEvent>>();
        _mockEventStore
            .Setup(es => es.GetAllEventsAsync())
            .ReturnsAsync(new List<TimeSlotEvent>());
        _mockEventStore
            .Setup(es => es.GetEventsForAggregateAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new List<TimeSlotEvent>());

        _instance = new TimeSlotRequestsService(_mockEventStore.Object);
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

    [Test]
    public async Task GetTimeSlotsForWorkDayId_CallsGetAllEventsAsync_Indirectly()
    {
        await _instance.GetTimeSlotsForWorkDayId(Guid.NewGuid());

        _mockEventStore.Verify(es => es.GetAllEventsAsync(), Times.Once);
    }
}