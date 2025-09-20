using Core.Server.Services.Entities.WorkDays;
using Domain.Events.WorkDays;
using Domain.Interfaces;
using Moq;

namespace Core.Server.Test.Services.Entities.WorkDays;

[TestFixture]
[TestOf(typeof(WorkDayRequestsService))]
public class WorkDayRequestsServiceTest
{
    private Mock<IEventStore<WorkDayEvent>> _mockEventStore;
    private WorkDayRequestsService _instance;

    [SetUp]
    public void SetUp()
    {
        _mockEventStore = new Mock<IEventStore<WorkDayEvent>>();
        _mockEventStore
            .Setup(es => es.GetAllEventsAsync())
            .ReturnsAsync(new List<WorkDayEvent>());

        _instance = new WorkDayRequestsService(_mockEventStore.Object);
    }

    [Test]
    public async Task GetAll_CallsGetAllEventsAsync()
    {
        await _instance.GetAll();

        _mockEventStore.Verify(es => es.GetAllEventsAsync(), Times.Once);
    }

    [Test]
    public async Task GetById_CallsGetAllEventsAsync_AndFilters()
    {
        await _instance.GetById(Guid.NewGuid());

        _mockEventStore.Verify(es => es.GetAllEventsAsync(), Times.Once);
    }

    [Test]
    public async Task GetWorkDaysInDateRange_CallsGetAllEventsAsync()
    {
        await _instance.GetWorkDaysInDateRange(DateTimeOffset.Now.AddDays(-1), DateTimeOffset.Now);

        _mockEventStore.Verify(es => es.GetAllEventsAsync(), Times.Once);
    }

    [Test]
    public async Task GetWorkDayByDate_CallsGetAllEventsAsync()
    {
        await _instance.GetWorkDayByDate(DateTimeOffset.Now);

        _mockEventStore.Verify(es => es.GetAllEventsAsync(), Times.Once);
    }

    [Test]
    public async Task IsWorkDayExisting_CallsGetAllEventsAsync_Indirectly()
    {
        await _instance.IsWorkDayExisting(DateTimeOffset.Now);

        _mockEventStore.Verify(es => es.GetAllEventsAsync(), Times.Once);
    }
}