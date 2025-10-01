using Client.Desktop.Communication.Commands.WorkDays.Records;
using Client.Desktop.Communication.Notifications.Wrappers;
using Client.Desktop.DataModels;
using Client.Desktop.Presentation.Models.Settings;
using Moq;

namespace Client.Desktop.Test.Presentation.Models.Settings;

[TestFixture]
[TestOf(typeof(WorkDaysModel))]
public class WorkDaysModelTest
{
    [Test]
    public async Task Initialise()
    {
        var workDay = new WorkDayClientModel(Guid.NewGuid(), DateTimeOffset.Now);
        var fixture = await WorkDaysModelProvider.Create([workDay], true);

        Assert.That(fixture.Instance, Is.Not.Null);
        Assert.That(fixture.Instance.WorkDays, Has.Count.EqualTo(1));
        fixture.CommandSender.Verify(x => x.Send(It.IsAny<ClientCreateWorkDayCommand>()), Times.Never);
    }

    [Test]
    public async Task InitialiseWorkDayNotExistingYet()
    {
        var fixture = await WorkDaysModelProvider.Create([], false);

        Assert.That(fixture.Instance, Is.Not.Null);
        Assert.That(fixture.Instance.WorkDays, Has.Count.EqualTo(0));
        fixture.CommandSender.Verify(x => x.Send(It.IsAny<ClientCreateWorkDayCommand>()));
    }

    [Test]
    public async Task ReceiveNewWorkDayMessage()
    {
        var fixture = await WorkDaysModelProvider.Create([], false);
        var newWorkDay = new WorkDayClientModel(Guid.NewGuid(), DateTimeOffset.Now);

        await fixture.NotificationPublisher.WorkDay.Publish(new NewWorkDayMessage(newWorkDay, Guid.NewGuid()));

        Assert.That(fixture.Instance.WorkDays, Has.Count.EqualTo(1));
    }

    [Test]
    public async Task SetSelectedWorkday()
    {
        var fixture = await WorkDaysModelProvider.Create([], false);
        var newWorkDay = new WorkDayClientModel(Guid.NewGuid(), DateTimeOffset.Now);

        await fixture.Instance.SetSelectedWorkday(newWorkDay);


        fixture.LocalSettingsService.Verify(x => x.SetSelectedDate(It.IsAny<DateTimeOffset>()));
    }
}