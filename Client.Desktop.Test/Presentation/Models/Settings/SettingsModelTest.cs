using Client.Desktop.Presentation.Models.Settings;
using Client.Desktop.Services.LocalSettings.Commands;
using CommunityToolkit.Mvvm.Messaging;
using Moq;

namespace Client.Desktop.Test.Presentation.Models.Settings;

[TestFixture]
[TestOf(typeof(SettingsModel))]
public class SettingsModelTest
{
    [Test]
    public void Initialize()
    {
        var fixture = SettingsModelProvider.Create();

        Assert.That(fixture.Instance.Settings, Is.Not.Null);
    }

    [Test]
    public void ReceiveExportPathSetNotification()
    {
        var fixture = SettingsModelProvider.Create();
        var newExportPath = "New export path";

        fixture.Messenger.Send(new ExportPathSetNotification(newExportPath));

        Assert.That(fixture.Instance.Settings!.ExportPath, Is.EqualTo(newExportPath));
    }

    [Test]
    public void ReceiveWorkDaySelectedNotification()
    {
        var fixture = SettingsModelProvider.Create();
        var newDate = DateTimeOffset.Now.AddDays(3);

        fixture.Messenger.Send(new WorkDaySelectedNotification(newDate));

        Assert.That(fixture.Instance.Settings!.SelectedDate, Is.EqualTo(newDate));
    }

    [Test]
    public void SendSetExportPath()
    {
        var fixture = SettingsModelProvider.Create();

        fixture.Instance.SetExportPath();

        fixture.LocalSettingsCommandSender.Verify(x => x.Send(It.IsAny<SetExportPathCommand>()), Times.Once);
    }
}