using Core.Server.Communication.Endpoints.Note;
using Core.Server.Communication.Records.Commands.Entities.Note;
using Google.Protobuf.WellKnownTypes;
using Proto.Command.Notes;
using Proto.DTO.TraceData;

namespace Core.Server.Test.Communication.Endpoints.Note;

[TestFixture]
[TestOf(typeof(NoteProtoExtensions))]
public class NoteProtoExtensionsTest
{
    [Test]
    public void ToCommand_CreateNoteCommandProto_ReturnsCreateNoteCommand()
    {
        var proto = new CreateNoteCommandProto
        {
            NoteId = Guid.NewGuid().ToString(),
            Text = "Sample Note",
            NoteTypeId = Guid.NewGuid().ToString(),
            TicketId = Guid.NewGuid().ToString(),
            TimeSlotId = Guid.NewGuid().ToString(),
            TimeStamp = Timestamp.FromDateTimeOffset(DateTimeOffset.UtcNow),
            TraceData = new TraceDataProto { TraceId = Guid.NewGuid().ToString() }
        };

        var result = proto.ToCommand();

        Assert.That(result, Is.Not.Null);
        Assert.That(result.NoteId.ToString(), Is.EqualTo(proto.NoteId));
        Assert.That(result.Text, Is.EqualTo(proto.Text));
        Assert.That(result.NoteTypeId.ToString(), Is.EqualTo(proto.NoteTypeId));
        Assert.That(result.TicketId.ToString(), Is.EqualTo(proto.TicketId));
        Assert.That(result.TimeSlotId.ToString(), Is.EqualTo(proto.TimeSlotId));
        Assert.That(result.TimeStamp, Is.EqualTo(proto.TimeStamp.ToDateTimeOffset()));
        Assert.That(result.TraceId.ToString(), Is.EqualTo(proto.TraceData.TraceId));
    }

    [Test]
    public void ToCommand_UpdateNoteCommandProto_ReturnsUpdateNoteCommand()
    {
        var proto = new UpdateNoteCommandProto
        {
            NoteId = Guid.NewGuid().ToString(),
            Text = "Updated Note",
            NoteTypeId = Guid.NewGuid().ToString(),
            TimeSlotId = Guid.NewGuid().ToString(),
            TraceData = new TraceDataProto { TraceId = Guid.NewGuid().ToString() }
        };

        var result = proto.ToCommand();

        Assert.That(result, Is.Not.Null);
        Assert.That(result.NoteId.ToString(), Is.EqualTo(proto.NoteId));
        Assert.That(result.Text, Is.EqualTo(proto.Text));
        Assert.That(result.NoteTypeId.ToString(), Is.EqualTo(proto.NoteTypeId));
        Assert.That(result.TimeSlotId.ToString(), Is.EqualTo(proto.TimeSlotId));
        Assert.That(result.TraceId.ToString(), Is.EqualTo(proto.TraceData.TraceId));
    }
    
    [Test]
    public void ToNotification_CreateNoteCommand_ReturnsNoteNotification()
    {
        var command = new CreateNoteCommand(
            Guid.NewGuid(), "Note Created", Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
            DateTimeOffset.UtcNow, Guid.NewGuid());

        var result = command.ToNotification();

        Assert.That(result, Is.Not.Null);
        Assert.That(result.NoteCreated, Is.Not.Null);
        Assert.That(result.NoteCreated.NoteId, Is.EqualTo(command.NoteId.ToString()));
        Assert.That(result.NoteCreated.Text, Is.EqualTo(command.Text));
        Assert.That(result.NoteCreated.NoteTypeId, Is.EqualTo(command.NoteTypeId.ToString()));
        Assert.That(result.NoteCreated.TimeSlotId, Is.EqualTo(command.TimeSlotId.ToString()));
        Assert.That(result.NoteCreated.TimeStamp, Is.EqualTo(command.TimeStamp.ToTimestamp()));
        Assert.That(result.NoteCreated.TraceData.TraceId, Is.EqualTo(command.TraceId.ToString()));
    }
    
    [Test]
    public void ToNotification_UpdateNoteCommand_ReturnsNoteNotification()
    {
        var command = new UpdateNoteCommand(Guid.NewGuid(), "Updated Note", Guid.NewGuid(), Guid.NewGuid(),
            Guid.NewGuid());

        var result = command.ToNotification();

        Assert.That(result, Is.Not.Null);
        Assert.That(result.NoteUpdated, Is.Not.Null);
        Assert.That(result.NoteUpdated.NoteId, Is.EqualTo(command.NoteId.ToString()));
        Assert.That(result.NoteUpdated.Text, Is.EqualTo(command.Text));
        Assert.That(result.NoteUpdated.NoteTypeId, Is.EqualTo(command.NoteTypeId.ToString()));
        Assert.That(result.NoteUpdated.TimeSlotId, Is.EqualTo(command.TimeSlotId.ToString()));
        Assert.That(result.NoteUpdated.TraceData.TraceId, Is.EqualTo(command.TraceId.ToString()));
    }
    
    [Test]
    public void ToProtoList_ListOfNotes_ReturnsGetNotesResponseProto()
    {
        var notes = new List<Domain.Entities.Note>
        {
            new()
            {
                NoteId = Guid.NewGuid(),
                Text = "First Note",
                NoteTypeId = Guid.NewGuid(),
                TicketId = Guid.NewGuid(),
                TimeStamp = DateTimeOffset.UtcNow
            },
            new()
            {
                NoteId = Guid.NewGuid(),
                Text = "Second Note",
                NoteTypeId = Guid.NewGuid(),
                TicketId = Guid.NewGuid(),
                TimeStamp = DateTimeOffset.UtcNow
            }
        };

        var result = notes.ToProtoList();

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Notes.Count, Is.EqualTo(notes.Count));
        for (int i = 0; i < notes.Count; i++)
        {
            Assert.That(result.Notes[i].NoteId, Is.EqualTo(notes[i].NoteId.ToString()));
            Assert.That(result.Notes[i].Text, Is.EqualTo(notes[i].Text));
            Assert.That(result.Notes[i].NoteTypeId, Is.EqualTo(notes[i].NoteTypeId.ToString()));
            Assert.That(result.Notes[i].TimeSlotId, Is.EqualTo(notes[i].TicketId.ToString()));
            Assert.That(result.Notes[i].TimeStamp.ToDateTimeOffset(), Is.EqualTo(notes[i].TimeStamp));
        }
    }
}