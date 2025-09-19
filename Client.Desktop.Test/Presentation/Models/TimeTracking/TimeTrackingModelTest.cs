using Client.Desktop.DataModels;
using Client.Desktop.Presentation.Models.TimeTracking;

namespace Client.Desktop.Test.Presentation.Models.TimeTracking;

[TestFixture]
[TestOf(typeof(TimeTrackingModel))]
public class TimeTrackingModelTest
{
    [Test]
    public async Task Initialize()
    {
        var ticket = new TicketClientModel(Guid.NewGuid(), "name", "bookingNumber", "documentation", []);
        var fixture = await TimeTrackingModelProvider.Create([ticket]);

        Assert.That(fixture.Instance, Is.Not.Null);
        Assert.That(fixture.Instance.FilteredTickets, Has.Count.EqualTo(1));
    }

    [Test]
    public async Task Foo()
    {
        var ticket = new TicketClientModel(Guid.NewGuid(), "name", "bookingNumber", "documentation", []);
        var fixture = await TimeTrackingModelProvider.Create([ticket]);
    }
}