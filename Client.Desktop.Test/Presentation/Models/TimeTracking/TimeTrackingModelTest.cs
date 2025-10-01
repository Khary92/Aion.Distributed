using Client.Desktop.Communication.Commands.Client.Records;
using Client.Desktop.Communication.Local.LocalEvents.Records;
using Client.Desktop.Communication.Notifications.Client.Records;
using Client.Desktop.Communication.Notifications.Sprint.Records;
using Client.Desktop.Communication.Notifications.Ticket.Records;
using Client.Desktop.Communication.Notifications.Wrappers;
using Client.Desktop.Communication.Requests.Client.Records;
using Client.Desktop.Communication.Requests.Ticket;
using Client.Desktop.DataModels;
using Client.Desktop.Presentation.Models.TimeTracking;
using Moq;

namespace Client.Desktop.Test.Presentation.Models.TimeTracking;

[TestFixture]
[TestOf(typeof(TimeTrackingModel))]
public class TimeTrackingModelTest
{
    [Test]
    public async Task Initialize()
    {
        var initialData = CreateInitialData();
        var timeSlotData = new ClientGetTrackingControlResponse(initialData.InitialStatisticsData,
            initialData.InitialTicket, initialData.InitialTimeSlot);

        var fixture = await TimeTrackingModelProvider.Create([initialData.InitialTicket], [timeSlotData]);

        Assert.That(fixture.Instance, Is.Not.Null);
        Assert.That(fixture.Instance.FilteredTickets, Has.Count.EqualTo(1));
    }

    [Test]
    public async Task ReceiveNewTicketMessage_TicketNotInCurrentSprint()
    {
        var fixture = await TimeTrackingModelProvider.Create([], []);

        var newTicket =
            new TicketClientModel(Guid.NewGuid(), "NewTicketName", "BookingNumber", "ChangeDocumentation", []);

        await fixture.NotificationPublisher.Ticket.Publish(new NewTicketMessage(newTicket, Guid.NewGuid()));

        Assert.That(fixture.Instance.FilteredTickets, Has.Count.EqualTo(0));
    }

    [Test]
    public async Task ReceiveNewTicketMessage()
    {
        var newTicket =
            new TicketClientModel(Guid.NewGuid(), "NewTicketName", "BookingNumber", "ChangeDocumentation", []);
        var fixture = await TimeTrackingModelProvider.Create([newTicket], []);

        await fixture.NotificationPublisher.Ticket.Publish(new NewTicketMessage(newTicket, Guid.NewGuid()));

        Assert.That(fixture.Instance.FilteredTickets, Has.Count.EqualTo(2));
    }

    [Test]
    public async Task ReceiveClientTicketDataUpdatedNotification()
    {
        var initialData = CreateInitialData();
        var fixture = await TimeTrackingModelProvider.Create([initialData.InitialTicket], []);

        var newName = "NewName";
        await fixture.NotificationPublisher.Ticket.Publish(new ClientTicketDataUpdatedNotification(
            initialData.InitialTicket.TicketId, newName,
            "BookingNumber", [], Guid.NewGuid()));

        Assert.That(fixture.Instance.FilteredTickets[0].Name, Is.EqualTo(newName));
    }

    [Test]
    public async Task ReceiveClientTicketAddedToActiveSprintNotification()
    {
        var initialData = CreateInitialData();
        var fixture = await TimeTrackingModelProvider.Create([initialData.InitialTicket], []);
        var newTicketOne =
            new TicketClientModel(Guid.NewGuid(), "NewTicketNameOne", "BookingNumberOne", "ChangeDocumentation", []);
        var newTicketTwo =
            new TicketClientModel(Guid.NewGuid(), "NewTicketNameTwo", "BookingNumberTwo", "ChangeDocumentation", []);
        fixture.RequestSender.Setup(rs => rs.Send(new ClientGetTicketsForCurrentSprintRequest()))
            .ReturnsAsync([newTicketOne, newTicketTwo]);

        await fixture.NotificationPublisher.Sprint.Publish(new ClientTicketAddedToActiveSprintNotification());

        Assert.That(fixture.Instance.FilteredTickets.Count, Is.EqualTo(2));
        Assert.That(fixture.Instance.FilteredTickets[0].TicketId, Is.EqualTo(newTicketOne.TicketId));
        Assert.That(fixture.Instance.FilteredTickets[1].TicketId, Is.EqualTo(newTicketTwo.TicketId));
    }

    [Test]
    public async Task ReceiveClientSprintSelectionChangedNotification()
    {
        var initialData = CreateInitialData();
        var fixture = await TimeTrackingModelProvider.Create([initialData.InitialTicket], []);
        var newTicketOne =
            new TicketClientModel(Guid.NewGuid(), "NewTicketNameOne", "BookingNumberOne", "ChangeDocumentation",
                [fixture.CurrentSprintId]);
        var newTicketTwo =
            new TicketClientModel(Guid.NewGuid(), "NewTicketNameTwo", "BookingNumberTwo", "ChangeDocumentation",
                [fixture.CurrentSprintId]);
        fixture.RequestSender.Setup(rs => rs.Send(new ClientGetTicketsForCurrentSprintRequest()))
            .ReturnsAsync([newTicketOne, newTicketTwo]);

        await fixture.NotificationPublisher.Client.Publish(new ClientSprintSelectionChangedNotification());

        Assert.That(fixture.Instance.FilteredTickets.Count, Is.EqualTo(2));
        Assert.That(fixture.Instance.FilteredTickets[0].TicketId, Is.EqualTo(newTicketOne.TicketId));
        Assert.That(fixture.Instance.FilteredTickets[1].TicketId, Is.EqualTo(newTicketTwo.TicketId));
    }

    [Test]
    public async Task ReceiveClientTimeSlotControlCreatedNotification()
    {
        var initialData = CreateInitialData();
        var fixture = await TimeTrackingModelProvider.Create([initialData.InitialTicket], []);

        await fixture.NotificationPublisher.Client.Publish(new ClientTrackingControlCreatedNotification(
            initialData.InitialStatisticsData,
            initialData.InitialTicket, initialData.InitialTimeSlot, Guid.NewGuid()));

        Assert.That(fixture.Instance.TimeSlotViewModels, Has.Count.EqualTo(1));
    }

    [Test]
    public async Task CreateNewTimeSlotViewModel()
    {
        var initialData = CreateInitialData();
        var fixture = await TimeTrackingModelProvider.Create([initialData.InitialTicket], []);
        fixture.Instance.SelectedTicket = initialData.InitialTicket;

        await fixture.Instance.CreateNewTimeSlotViewModel();

        fixture.CommandSender.Verify(cs => cs.Send(It.IsAny<ClientCreateTrackingControlCommand>()));
    }

    [Test]
    public async Task ToggleViewModels()
    {
        var firstData = CreateInitialData();
        var secondData = CreateInitialData();
        var fixture = await TimeTrackingModelProvider.Create([], [
            new ClientGetTrackingControlResponse(firstData.InitialStatisticsData, firstData.InitialTicket,
                firstData.InitialTimeSlot),
            new ClientGetTrackingControlResponse(secondData.InitialStatisticsData, secondData.InitialTicket,
                secondData.InitialTimeSlot)
        ]);

        fixture.Instance.ToggleNextViewModel();

        Assert.That(fixture.Instance.CurrentViewModelIndex, Is.EqualTo(1));

        fixture.Instance.TogglePreviousViewModel();

        Assert.That(fixture.Instance.CurrentViewModelIndex, Is.EqualTo(0));
    }


    private static (StatisticsDataClientModel InitialStatisticsData, TicketClientModel InitialTicket,
        TimeSlotClientModel InitialTimeSlot) CreateInitialData()
    {
        var statisticsData =
            new StatisticsDataClientModel(Guid.NewGuid(), Guid.NewGuid(), new List<Guid>(), true, false, false);
        var timeSlot = new TimeSlotClientModel(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.UtcNow,
            DateTimeOffset.UtcNow, new List<Guid>(), false);
        var ticket = new TicketClientModel(Guid.NewGuid(), "name", "bookingNumber", "documentation", []);

        return (statisticsData, ticket, timeSlot);
    }
}