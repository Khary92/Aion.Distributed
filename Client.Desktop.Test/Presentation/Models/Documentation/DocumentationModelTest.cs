using Client.Desktop.Communication.Notifications.Note.Records;
using Client.Desktop.Communication.Notifications.NoteType.Records;
using Client.Desktop.Communication.Notifications.Ticket.Records;
using Client.Desktop.Communication.Notifications.Wrappers;
using Client.Desktop.DataModels;
using Client.Desktop.Presentation.Models.Documentation;
using CommunityToolkit.Mvvm.Messaging;

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
        Assert.That(fixture.Instance.AllNotes, Has.Count.EqualTo(1));
    }

    [Test]
    public async Task ReceiveNewNoteMessage()
    {
        var initialData = CreateInitialData();
        var fixture =
            await DocumentationModelProvider.Create(initialData.Notes, initialData.NoteTypes, initialData.Tickets);

        var newNote = new NoteClientModel(Guid.NewGuid(), "NewNoteName", Guid.NewGuid(), Guid.NewGuid(),
            DateTimeOffset.Now);

        fixture.Messenger.Send(new NewNoteMessage(newNote));

        Assert.That(fixture.Instance.AllNoteTypes, Has.Count.EqualTo(1));
        Assert.That(fixture.Instance.AllTickets, Has.Count.EqualTo(1));
        Assert.That(fixture.Instance.AllNotes, Has.Count.EqualTo(2));
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

        fixture.Messenger.Send(noteUpdatedNotification);

        Assert.That(fixture.Instance.AllNoteTypes, Has.Count.EqualTo(1));
        Assert.That(fixture.Instance.AllTickets, Has.Count.EqualTo(1));
        Assert.That(fixture.Instance.AllNotes, Has.Count.EqualTo(1));
        Assert.That(fixture.Instance.AllNotes.First().Note.Text, Is.EqualTo(newNoteText));
    }

    [Test]
    public async Task ReceiveNewNoteTypeMessage()
    {
        var initialData = CreateInitialData();
        var fixture =
            await DocumentationModelProvider.Create(initialData.Notes, initialData.NoteTypes, initialData.Tickets);
        var newNoteType = new NoteTypeClientModel(Guid.NewGuid(), "NewNoteType", "Color");

        fixture.Messenger.Send(new NewNoteTypeMessage(newNoteType, Guid.NewGuid()));

        Assert.That(fixture.Instance.AllNoteTypes, Has.Count.EqualTo(2));
        Assert.That(fixture.Instance.AllTickets, Has.Count.EqualTo(1));
        Assert.That(fixture.Instance.AllNotes, Has.Count.EqualTo(1));
    }

    [Test]
    public async Task ReceiveClientNoteTypeNameChangedNotification()
    {
        var initialData = CreateInitialData();
        var fixture =
            await DocumentationModelProvider.Create(initialData.Notes, initialData.NoteTypes, initialData.Tickets);
        var newNoteTypeName = "NewNoteTypeName";
        var noteUpdatedNotification =
            new ClientNoteTypeNameChangedNotification(initialData.NoteTypeId, newNoteTypeName, Guid.NewGuid());
        ;

        fixture.Messenger.Send(noteUpdatedNotification);

        Assert.That(fixture.Instance.AllNoteTypes, Has.Count.EqualTo(1));
        Assert.That(fixture.Instance.AllTickets, Has.Count.EqualTo(1));
        Assert.That(fixture.Instance.AllNotes, Has.Count.EqualTo(1));
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
        ;

        fixture.Messenger.Send(noteUpdatedNotification);

        Assert.That(fixture.Instance.AllNoteTypes, Has.Count.EqualTo(1));
        Assert.That(fixture.Instance.AllTickets, Has.Count.EqualTo(1));
        Assert.That(fixture.Instance.AllNotes, Has.Count.EqualTo(1));
        Assert.That(fixture.Instance.AllNoteTypes.First().NoteType.Color, Is.EqualTo(newNoteTypeColor));
    }

    [Test]
    public async Task ReceiveNewTicketMessage()
    {
        var initialData = CreateInitialData();
        var fixture =
            await DocumentationModelProvider.Create(initialData.Notes, initialData.NoteTypes, initialData.Tickets);
        var noteUpdatedNotification =
            new TicketClientModel(Guid.NewGuid(), "NewTicketName", "BookingNumber", "Documentation", []);

        fixture.Messenger.Send(new NewTicketMessage(noteUpdatedNotification, Guid.NewGuid()));

        Assert.That(fixture.Instance.AllNoteTypes, Has.Count.EqualTo(1));
        Assert.That(fixture.Instance.AllTickets, Has.Count.EqualTo(2));
        Assert.That(fixture.Instance.AllNotes, Has.Count.EqualTo(1));
    }

    [Test]
    public async Task ReceiveClientTicketDataUpdatedNotification()
    {
        var initialData = CreateInitialData();
        var fixture =
            await DocumentationModelProvider.Create(initialData.Notes, initialData.NoteTypes, initialData.Tickets);
        var newTicketName = "NewTicketName";
        var noteUpdatedNotification = new ClientTicketDataUpdatedNotification(initialData.TicketId, newTicketName,
            "BookingNumber", [], Guid.NewGuid());

        fixture.Messenger.Send(noteUpdatedNotification);

        Assert.That(fixture.Instance.AllNoteTypes, Has.Count.EqualTo(1));
        Assert.That(fixture.Instance.AllTickets, Has.Count.EqualTo(1));
        Assert.That(fixture.Instance.AllNotes, Has.Count.EqualTo(1));
        Assert.That(fixture.Instance.AllTickets.First().Name, Is.EqualTo(newTicketName));
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
        var ticket = new TicketClientModel(ticketId, "InitialTicketName", "BookingNumber", "Documentation", []);
        return ([note], [noteType], [ticket], noteId, noteTypeId, ticketId);
    }
}