using Core.Server.Services.Entities.TimerSettings;
using Domain.Events.TimerSettings;
using Domain.Interfaces;
using Moq;

namespace Core.Server.Test.Services.Entities.TimerSettings;

[TestFixture]
[TestOf(typeof(TimerSettingsRequestsService))]
public class TimerSettingsRequestsServiceTest
{
    private Mock<IEventStore<TimerSettingsEvent>> _mockEventStore;
    private TimerSettingsRequestsService _instance;

    [SetUp]
    public void SetUp()
    {
        _mockEventStore = new Mock<IEventStore<TimerSettingsEvent>>();
        _mockEventStore
            .Setup(es => es.GetAllEventsAsync())
            .ReturnsAsync(new List<TimerSettingsEvent>());

        _instance = new TimerSettingsRequestsService(_mockEventStore.Object);
    }

    [Test]
    public void Get_CallsGetAllEventsAsync_AndThrowsWhenNoEvents()
    {
        Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await _instance.Get());
        _mockEventStore.Verify(es => es.GetAllEventsAsync(), Times.Once);
    }

    [Test]
    public async Task IsTimerSettingsExisting_CallsGetAllEventsAsync()
    {
        await _instance.IsTimerSettingsExisting();
        _mockEventStore.Verify(es => es.GetAllEventsAsync(), Times.Once);
    }
}