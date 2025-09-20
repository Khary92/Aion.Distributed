using Core.Server.Services.Entities.StatisticsData;
using Domain.Events.StatisticsData;
using Domain.Interfaces;
using Moq;

namespace Core.Server.Test.Services.Entities.StatisticsData;

[TestFixture]
[TestOf(typeof(StatisticsDataRequestsService))]
public class StatisticsDataRequestsServiceTest
{
    private Mock<IEventStore<StatisticsDataEvent>> _mockEventStore;
    private StatisticsDataRequestsService _instance;

    [SetUp]
    public void SetUp()
    {
        _mockEventStore = new Mock<IEventStore<StatisticsDataEvent>>();
        _mockEventStore
            .Setup(es => es.GetAllEventsAsync())
            .ReturnsAsync(new List<StatisticsDataEvent>());

        _instance = new StatisticsDataRequestsService(_mockEventStore.Object);
    }

    [Test]
    public async Task GetAll_CallsGetAllEventsAsync()
    {
        await _instance.GetAll();

        _mockEventStore.Verify(es => es.GetAllEventsAsync(), Times.Once);
    }

    [Test]
    public void GetStatisticsDataByTimeSlotId_CallsGetAllEventsAsync()
    {
        Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await _instance.GetStatisticsDataByTimeSlotId(Guid.NewGuid()));

        _mockEventStore.Verify(es => es.GetAllEventsAsync(), Times.Once);
    }
}