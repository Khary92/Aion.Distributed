using Client.Desktop.Communication.Commands.TimeSlots.Records;
using Client.Desktop.Communication.Notifications.Ticket.Records;
using Client.Desktop.Communication.Notifications.UseCase.Records;
using Client.Desktop.DataModels;
using Client.Desktop.Presentation.Models.TimeTracking;
using CommunityToolkit.Mvvm.Messaging;
using Moq;

namespace Client.Desktop.Test.Presentation.Models.TimeTracking;

[TestFixture]
[TestOf(typeof(TimeSlotModel))]
public class TimeSlotModelTest
{
    [Test]
    public async Task Initialize()
    {
        var fixture = await TimeSlotModelProvider.Create(null);

        Assert.That(fixture.Instance, Is.Not.Null);
    }

    [Test]
    public async Task ToggleTimerState()
    {
        var initialTimeSlot = new TimeSlotClientModel(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
            DateTimeOffset.Now, DateTimeOffset.Now,
            [], false);
        var fixture = await TimeSlotModelProvider.Create(initialTimeSlot);

        var before = fixture.Instance.TimeSlot.IsTimerRunning;
        fixture.Instance.ToggleTimerState();
        var after = fixture.Instance.TimeSlot.IsTimerRunning;

        Assert.That(before, Is.Not.EqualTo(after));

        before = fixture.Instance.TimeSlot.IsTimerRunning;
        fixture.Instance.ToggleTimerState();
        after = fixture.Instance.TimeSlot.IsTimerRunning;

        Assert.That(before, Is.Not.EqualTo(after));
    }

    [Test]
    public async Task ToggleReplayMode()
    {
        var fixture = await TimeSlotModelProvider.Create(null);

        await fixture.Instance.ToggleReplayMode();

        Assert.That(fixture.Instance.TicketReplayDecorator.IsReplayMode, Is.True);
        Assert.That(fixture.Instance.TicketReplayDecorator.DisplayedDocumentation, Is.Not.EqualTo(string.Empty));

        await fixture.Instance.ToggleReplayMode();

        Assert.That(fixture.Instance.TicketReplayDecorator.IsReplayMode, Is.False);
        Assert.That(fixture.Instance.TicketReplayDecorator.DisplayedDocumentation,
            Is.EqualTo(fixture.TicketReplayDecorator.Ticket.Documentation));
    }

    [Test]
    public async Task ReceiveClientTicketDocumentationUpdatedNotification()
    {
        var fixture = await TimeSlotModelProvider.Create(null);

        var newDocumentation = "NewDocumentation";
        var clientTicketDocumentationUpdatedNotification =
            new ClientTicketDocumentationUpdatedNotification(fixture.TicketReplayDecorator.Ticket.TicketId,
                newDocumentation, Guid.NewGuid());

        fixture.Messenger.Send(clientTicketDocumentationUpdatedNotification);

        Assert.That(fixture.Instance.TicketReplayDecorator.DisplayedDocumentation, Is.EqualTo(newDocumentation));
    }

    [Test]
    public async Task ReceiveClientTicketDataUpdatedNotification()
    {
        var fixture = await TimeSlotModelProvider.Create(null);

        var newName = "New Name from test";
        var clientTicketDataUpdatedNotification =
            new ClientTicketDataUpdatedNotification(fixture.TicketReplayDecorator.Ticket.TicketId, newName,
                "BookingNumber", [], Guid.NewGuid());

        fixture.Messenger.Send(clientTicketDataUpdatedNotification);

        Assert.That(fixture.Instance.TicketReplayDecorator.Ticket.Name, Is.EqualTo(newName));
    }

    [Test]
    public async Task ReceiveClientSaveDocumentationNotification_TimesChanged()
    {
        var fixture = await TimeSlotModelProvider.Create(null);
        fixture.TimeSlot.StartTime = fixture.TimeSlot.StartTime.AddHours(-1);
        fixture.TimeSlot.EndTime = fixture.TimeSlot.EndTime.AddHours(1);
        var clientSaveDocumentationNotification = new ClientSaveDocumentationNotification();

        fixture.Messenger.Send(clientSaveDocumentationNotification);

        fixture.EndTimeCache.Verify(ec => ec.Store(It.IsAny<ClientSetEndTimeCommand>()));
        fixture.StartTimeCache.Verify(ec => ec.Store(It.IsAny<ClientSetStartTimeCommand>()));
 }

    [Test]
    public async Task ReceiveClientSaveDocumentationNotification_DocumentationNotChanged()
    {
        var fixture = await TimeSlotModelProvider.Create(null);
        var clientSaveDocumentationNotification = new ClientSaveDocumentationNotification();

        fixture.Messenger.Send(clientSaveDocumentationNotification);

        fixture.DocumentationSynchronizer.Verify(ds => ds.SetSynchronizationValue(It.IsAny<Guid>(), It.IsAny<string>()),
            Times.Never);
        fixture.DocumentationSynchronizer.Verify(ds => ds.FireCommand(It.IsAny<Guid>()), Times.Never);
    }

    [Test]
    public async Task ReceiveClientSaveDocumentationNotification_DocumentationChanged()
    {
        var fixture = await TimeSlotModelProvider.Create(null);
        var clientSaveDocumentationNotification = new ClientSaveDocumentationNotification();

        fixture.TicketReplayDecorator.Ticket.Documentation = "New Documentation from test";
        fixture.Messenger.Send(clientSaveDocumentationNotification);

        fixture.DocumentationSynchronizer.Verify(ds => ds.SetSynchronizationValue(It.IsAny<Guid>(), It.IsAny<string>()));
        fixture.DocumentationSynchronizer.Verify(ds => ds.FireCommand(It.IsAny<Guid>()));
    }
}