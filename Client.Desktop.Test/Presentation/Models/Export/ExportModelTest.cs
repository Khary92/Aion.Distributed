using System.Collections.ObjectModel;
using Client.Desktop.Communication.Notifications.Wrappers;
using Client.Desktop.DataModels;
using Client.Desktop.Presentation.Models.Export;
using CommunityToolkit.Mvvm.Messaging;
using Moq;

namespace Client.Desktop.Test.Presentation.Models.Export;

[TestFixture]
[TestOf(typeof(ExportModel))]
public class ExportModelTest
{
    [Test]
    public async Task Initialize()
    {
        var workDay = new WorkDayClientModel(Guid.NewGuid(), DateTimeOffset.Now);
        var fixture = await ExportModelProvider.Create([workDay]);

        Assert.That(fixture.Instance.WorkDays, Has.Count.EqualTo(1));
    }

    [Test]
    public async Task ReceiveNewWorkDayMessage()
    {
        var workDay = new WorkDayClientModel(Guid.NewGuid(), DateTimeOffset.Now);
        var fixture = await ExportModelProvider.Create([workDay]);
        var newWorkDay = new WorkDayClientModel(Guid.NewGuid(), DateTimeOffset.Now);

        fixture.Messenger.Send(new NewWorkDayMessage(newWorkDay, Guid.NewGuid()));

        Assert.That(fixture.Instance.WorkDays, Has.Count.EqualTo(2));
    }

    [Test]
    public async Task ExportSuccessful()
    {
        var workDay = new WorkDayClientModel(Guid.NewGuid(), DateTimeOffset.Now);
        var fixture = await ExportModelProvider.Create([workDay]);
        fixture.LocalSettingsService.Setup(ls => ls.IsExportPathValid()).Returns(true);
        fixture.ExportService.Setup(es => es.ExportToFile(It.IsAny<Collection<WorkDayClientModel>>()))
            .Returns(Task.FromResult(true));

        var isExportedSuccessful = await fixture.Instance.ExportFileAsync();

        Assert.That(fixture.Instance.WorkDays, Has.Count.EqualTo(1));
        Assert.That(isExportedSuccessful, Is.True);
    }

    [Test]
    public async Task ExportPathInvalid()
    {
        var workDay = new WorkDayClientModel(Guid.NewGuid(), DateTimeOffset.Now);
        var fixture = await ExportModelProvider.Create([workDay]);
        fixture.ExportService.Setup(es => es.ExportToFile(It.IsAny<Collection<WorkDayClientModel>>()))
            .Returns(Task.FromResult(true));

        var isExportedSuccessful = await fixture.Instance.ExportFileAsync();

        Assert.That(fixture.Instance.WorkDays, Has.Count.EqualTo(1));
        Assert.That(isExportedSuccessful, Is.False);
    }

    [Test]
    public async Task ExportWritingFileFailed()
    {
        var workDay = new WorkDayClientModel(Guid.NewGuid(), DateTimeOffset.Now);
        var fixture = await ExportModelProvider.Create([workDay]);
        fixture.LocalSettingsService.Setup(ls => ls.IsExportPathValid()).Returns(true);

        var isExportedSuccessful = await fixture.Instance.ExportFileAsync();

        Assert.That(fixture.Instance.WorkDays, Has.Count.EqualTo(1));
        Assert.That(isExportedSuccessful, Is.False);
    }
}