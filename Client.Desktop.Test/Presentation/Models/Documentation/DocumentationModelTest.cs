using Client.Desktop.Communication.Notifications.Note.Records;
using Client.Desktop.Communication.Notifications.NoteType.Records;
using Client.Desktop.Communication.Notifications.Ticket.Records;
using Client.Desktop.Communication.Notifications.Wrappers;
using Client.Desktop.DataModels;
using Client.Desktop.Presentation.Models.Documentation;

namespace Client.Desktop.Test.Presentation.Models.Documentation;

[TestFixture]
[TestOf(typeof(DocumentationModel))]
public class DocumentationModelTest
{
    [Test]
    public async Task Initialize()
    {
        var initialData = CreateInitialData();
        var fixture =
            await DocumentationModelProvider.Create(initialData.Notes, initialData.NoteTypes, initialData.Tickets);

        Assert.That(fixture.Instance.AllNoteTypes, Has.Count.EqualTo(1));
        Assert.That(fixture.Instance.AllTickets, Has.Count.EqualTo(1));
        Assert.That(fixture.Instance.AllNotesByTicket, Has.Count.EqualTo(1));
    }

    [Test]
    public async Task ReceiveNewNoteMessage()
    {
        var initialData = CreateInitialData();
        var fixture =
            await DocumentationModelProvider.Create(initialData.Notes, initialData.NoteTypes, initialData.Tickets);

        var newNote = new NoteClientModel(Guid.NewGuid(), "NewNoteName", Guid.NewGuid(), Guid.NewGuid(),
            DateTimeOffset.Now);

        await fixture.NotificationPublisher.Note.Publish(new NewNoteMessage(newNote, Guid.NewGuid()));

        Assert.That(fixture.Instance.AllNoteTypes, Has.Count.EqualTo(1));
        Assert.That(fixture.Instance.AllTickets, Has.Count.EqualTo(1));
        Assert.That(fixture.Instance.AllNotesByTicket, Has.Count.EqualTo(2));
    }

    [Test]
    public async Task ReceiveClientNoteUpdatedNotification()
    {
        var initialData = CreateInitialData();
        var fixture =
            await DocumentationModelProvider.Create(initialData.Notes, initialData.NoteTypes, initialData.Tickets);
        var newNoteText = "UpdatedNoteText";
        var noteUpdatedNotification = new ClientNoteUpdatedNotification(initialData.NoteId, newNoteText,
            Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

        await fixture.NotificationPublisher.Note.Publish(noteUpdatedNotification);

        Assert.That(fixture.Instance.AllNoteTypes, Has.Count.EqualTo(1));
        Assert.That(fixture.Instance.AllTickets, Has.Count.EqualTo(1));
        Assert.That(fixture.Instance.AllNotesByTicket, Has.Count.EqualTo(1));
        Assert.That(fixture.Instance.AllNotesByTicket.First().Note.Text, Is.EqualTo(newNoteText));
    }

    [Test]
    public async Task ReceiveNewNoteTypeMessage()
    {
        var initialData = CreateInitialData();
        var fixture =
            await DocumentationModelProvider.Create(initialData.Notes, initialData.NoteTypes, initialData.Tickets);
        var newNoteType = new NoteTypeClientModel(Guid.NewGuid(), "NewNoteType", "Color");

        await fixture.NotificationPublisher.NoteType.Publish(new NewNoteTypeMessage(newNoteType, Guid.NewGuid()));

        Assert.That(fixture.Instance.AllNoteTypes, Has.Count.EqualTo(2));
        Assert.That(fixture.Instance.AllTickets, Has.Count.EqualTo(1));
        Assert.That(fixture.Instance.AllNotesByTicket, Has.Count.EqualTo(1));
    }

    [Test]
    public async Task ReceiveClientNoteTypeNameChangedNotification()
    {
        var initialData = CreateInitialData();
        var fixture =
            await DocumentationModelProvider.Create(initialData.Notes, initialData.NoteTypes, initialData.Tickets);
        var newNoteTypeName = "NewNoteTypeName";
        var clientNoteTypeNameChangedNotification =
            new ClientNoteTypeNameChangedNotification(initialData.NoteTypeId, newNoteTypeName, Guid.NewGuid());

        await fixture.NotificationPublisher.NoteType.Publish(clientNoteTypeNameChangedNotification);

        Assert.That(fixture.Instance.AllNoteTypes.First().NoteType.Name, Is.EqualTo(newNoteTypeName));
    }

    [Test]
    public async Task ReceiveClientNoteTypeColorChangedNotification()
    {
        var initialData = CreateInitialData();
        var fixture =
            await DocumentationModelProvider.Create(initialData.Notes, initialData.NoteTypes, initialData.Tickets);
        var newNoteTypeColor = "A new color";
        var noteUpdatedNotification =
            new ClientNoteTypeColorChangedNotification(initialData.NoteTypeId, newNoteTypeColor, Guid.NewGuid());

        await fixture.NotificationPublisher.NoteType.Publish(noteUpdatedNotification);

        Assert.That(fixture.Instance.AllNoteTypes.First().NoteType.Color, Is.EqualTo(newNoteTypeColor));
    }

    [Test]
    public async Task ReceiveNewTicketMessage()
    {
        var initialData = CreateInitialData();
        var fixture =
            await DocumentationModelProvider.Create(initialData.Notes, initialData.NoteTypes, initialData.Tickets);
        var noteUpdatedNotification =
            new TicketClientModel(Guid.NewGuid(), "NewTicketName", "BookingNumber", "ChangeDocumentation", []);

        await fixture.NotificationPublisher.Ticket.Publish(
            new NewTicketMessage(noteUpdatedNotification, Guid.NewGuid()));

        Assert.That(fixture.Instance.AllNoteTypes, Has.Count.EqualTo(1));
        Assert.That(fixture.Instance.AllTickets, Has.Count.EqualTo(2));
        Assert.That(fixture.Instance.AllNotesByTicket, Has.Count.EqualTo(1));
    }

    [Test]
    public async Task ReceiveClientTicketDataUpdatedNotification()
    {
        var initialData = CreateInitialData();
        var fixture =
            await DocumentationModelProvider.Create(initialData.Notes, initialData.NoteTypes, initialData.Tickets);
        var newTicketName = "NewTicketName";
        var clientTicketDataUpdatedNotification = new ClientTicketDataUpdatedNotification(initialData.TicketId,
            newTicketName,
            "BookingNumber", [], Guid.NewGuid());

        await fixture.NotificationPublisher.Ticket.Publish(clientTicketDataUpdatedNotification);

        Assert.That(fixture.Instance.AllNoteTypes, Has.Count.EqualTo(1));
        Assert.That(fixture.Instance.AllTickets, Has.Count.EqualTo(1));
        Assert.That(fixture.Instance.AllNotesByTicket, Has.Count.EqualTo(1));
        Assert.That(fixture.Instance.AllTickets.First().Name, Is.EqualTo(newTicketName));
    }


    [Test]
    public async Task ClientNoteUpdatedNotification_WithUnknownId_DoesNotChangeCounts()
    {
        var initialData = CreateInitialData();
        var fixture =
            await DocumentationModelProvider.Create(initialData.Notes, initialData.NoteTypes, initialData.Tickets);

        var notification = new ClientNoteUpdatedNotification(Guid.NewGuid(), "Irrelevant",
            Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

        await fixture.NotificationPublisher.Note.Publish(notification);

        Assert.That(fixture.Instance.AllNotesByTicket.Count, Is.EqualTo(1));
        Assert.That(initialData.NoteTypes, Has.Count.EqualTo(fixture.Instance.AllNoteTypes.Count));
        Assert.That(initialData.Tickets, Has.Count.EqualTo(fixture.Instance.AllTickets.Count));
    }

    [Test]
    public async Task ClientNoteTypeNameChangedNotification_WithUnknownId_DoesNotChangeCounts()
    {
        var initialData = CreateInitialData();
        var fixture =
            await DocumentationModelProvider.Create(initialData.Notes, initialData.NoteTypes, initialData.Tickets);

        var beforeNotes = fixture.Instance.AllNotesByTicket.Count;
        var beforeTypes = fixture.Instance.AllNoteTypes.Count;
        var beforeTickets = fixture.Instance.AllTickets.Count;

        var notification =
            new ClientNoteTypeNameChangedNotification(Guid.NewGuid(), "NewName", Guid.NewGuid());

        await fixture.NotificationPublisher.NoteType.Publish(notification);

        Assert.That(fixture.Instance.AllNotesByTicket.Count, Is.EqualTo(beforeNotes));
        Assert.That(fixture.Instance.AllNoteTypes.Count, Is.EqualTo(beforeTypes));
        Assert.That(fixture.Instance.AllTickets.Count, Is.EqualTo(beforeTickets));
    }

    [Test]
    public async Task ClientTicketDataUpdatedNotification_WithUnknownId_DoesNotChangeCounts()
    {
        var initialData = CreateInitialData();
        var fixture =
            await DocumentationModelProvider.Create(initialData.Notes, initialData.NoteTypes, initialData.Tickets);

        var beforeNotes = fixture.Instance.AllNotesByTicket.Count;
        var beforeTypes = fixture.Instance.AllNoteTypes.Count;
        var beforeTickets = fixture.Instance.AllTickets.Count;

        var notification = new ClientTicketDataUpdatedNotification(Guid.NewGuid(), "NewName",
            "BookingNumber", [], Guid.NewGuid());

        await fixture.NotificationPublisher.Ticket.Publish(notification);

        Assert.That(fixture.Instance.AllNotesByTicket.Count, Is.EqualTo(beforeNotes));
        Assert.That(fixture.Instance.AllNoteTypes.Count, Is.EqualTo(beforeTypes));
        Assert.That(fixture.Instance.AllTickets.Count, Is.EqualTo(beforeTickets));
    }

    private static (
        List<NoteClientModel> Notes,
        List<NoteTypeClientModel> NoteTypes,
        List<TicketClientModel> Tickets,
        Guid NoteId,
        Guid NoteTypeId,
        Guid TicketId) CreateInitialData()
    {
        var noteTypeId = Guid.NewGuid();
        var noteId = Guid.NewGuid();
        var ticketId = Guid.NewGuid();

        var note = new NoteClientModel(noteId, "InitialNoteText", noteTypeId, Guid.NewGuid(),
            DateTimeOffset.Now);
        var noteType = new NoteTypeClientModel(noteTypeId, "InitialNoteTypeName", "#FFFFF");
        var ticket = new TicketClientModel(ticketId, "InitialTicketName", "BookingNumber", "ChangeDocumentation", []);
        return ([note], [noteType], [ticket], noteId, noteTypeId, ticketId);
    }
}