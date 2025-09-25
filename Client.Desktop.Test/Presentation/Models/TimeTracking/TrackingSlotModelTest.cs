using Client.Desktop.Communication.Commands.TimeSlots.Records;
using Client.Desktop.Communication.Notifications.Client.Records;
using Client.Desktop.Communication.Notifications.Ticket.Records;
using Client.Desktop.DataModels;
using Client.Desktop.Presentation.Models.TimeTracking;
using CommunityToolkit.Mvvm.Messaging;
using Moq;

namespace Client.Desktop.Test.Presentation.Models.TimeTracking;

[TestFixture]
[TestOf(typeof(TrackingSlotModel))]
public class TrackingSlotModelTest
{
    [Test]
    public async Task Initialize()
    {
        var fixture = await TrackingSlotModelProvider.Create(null);

        Assert.That(fixture.Instance, Is.Not.Null);
    }

    [Test]
    public async Task ToggleTimerState()
    {
        var initialTimeSlot = new TimeSlotClientModel(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
            DateTimeOffset.Now, DateTimeOffset.Now,
            [], false);
        var fixture = await TrackingSlotModelProvider.Create(initialTimeSlot);

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
    public async Task ReceiveClientTicketDocumentationUpdatedNotification()
    {
        var fixture = await TrackingSlotModelProvider.Create(null);

        var newDocumentation = "NewDocumentation";
        var clientTicketDocumentationUpdatedNotification =
            new ClientTicketDocumentationUpdatedNotification(fixture.Ticket.TicketId,
                newDocumentation, Guid.NewGuid());

        fixture.Messenger.Send(clientTicketDocumentationUpdatedNotification);

        Assert.That(fixture.Ticket.Documentation, Is.EqualTo(newDocumentation));
    }

    [Test]
    public async Task ReceiveClientTicketDataUpdatedNotification()
    {
        var fixture = await TrackingSlotModelProvider.Create(null);

        var newName = "New Name from test";
        var clientTicketDataUpdatedNotification =
            new ClientTicketDataUpdatedNotification(fixture.Ticket.TicketId, newName,
                "BookingNumber", [], Guid.NewGuid());

        fixture.Messenger.Send(clientTicketDataUpdatedNotification);

        Assert.That(fixture.Instance.Ticket.Ticket.Name, Is.EqualTo(newName));
    }

    [Test]
    public async Task ReceiveClientCreateSnapshotNotification_TimesChanged()
    {
        var fixture = await TrackingSlotModelProvider.Create(null);
        fixture.TimeSlot.StartTime = fixture.TimeSlot.StartTime.AddHours(-1);
        fixture.TimeSlot.EndTime = fixture.TimeSlot.EndTime.AddHours(1);
        var clientCreateSnapshotNotification = new ClientCreateSnapshotNotification();

        fixture.Messenger.Send(clientCreateSnapshotNotification);

        fixture.EndTimeCache.Verify(ec => ec.Store(It.IsAny<ClientSetEndTimeCommand>()));
        fixture.StartTimeCache.Verify(ec => ec.Store(It.IsAny<ClientSetStartTimeCommand>()));
    }
}