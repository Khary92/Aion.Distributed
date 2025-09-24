using Core.Server.Communication.Endpoints.Note;
using Core.Server.Communication.Records.Commands.Entities.Note;
using Core.Server.Services.Entities.Notes;
using Core.Server.Tracing.Tracing.Tracers;
using Core.Server.Tracing.Tracing.Tracers.Note.UseCase;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Moq;
using Proto.Command.Notes;
using Proto.DTO.TraceData;

namespace Core.Server.Test.Communication.Endpoints.Note;

[TestFixture]
[TestOf(typeof(NoteCommandReceiver))]
public class NoteCommandReceiverTest
{
    private Mock<INoteCommandsService> _noteCommandsServiceMock;
    private Mock<ITraceCollector> _traceCollectorMock;
    private NoteCommandReceiver _noteCommandReceiver;

    [SetUp]
    public void SetUp()
    {
        _noteCommandsServiceMock = new Mock<INoteCommandsService>();
        _traceCollectorMock = new Mock<ITraceCollector>();
        _traceCollectorMock.Setup(t => t.Note.Create).Returns(Mock.Of<ICreateNoteTraceCollector>());
        _traceCollectorMock.Setup(t => t.Note.Update).Returns(Mock.Of<IUpdateNoteTraceCollector>());

        _noteCommandReceiver = new NoteCommandReceiver(_noteCommandsServiceMock.Object, _traceCollectorMock.Object);
    }

    [Test]
    public async Task CreateNote_ValidRequest_ReturnsSuccessResponse()
    {
        var request = new CreateNoteCommandProto
        {
            NoteId = Guid.NewGuid().ToString(),
            Text = "Test Note",
            NoteTypeId = Guid.NewGuid().ToString(),
            TicketId = Guid.NewGuid().ToString(),
            TimeSlotId = Guid.NewGuid().ToString(),
            TimeStamp = Timestamp.FromDateTimeOffset(DateTimeOffset.UtcNow),
            TraceData = new TraceDataProto { TraceId = Guid.NewGuid().ToString() }
        };

        var response = await _noteCommandReceiver.CreateNote(request, Mock.Of<ServerCallContext>());

        Assert.That(response.Success);
        _noteCommandsServiceMock.Verify(s => s.Create(It.IsAny<CreateNoteCommand>()), Times.Once);
        _traceCollectorMock.Verify(
            t => t.Note.Create.CommandReceived(typeof(NoteCommandReceiver), Guid.Parse(request.TraceData.TraceId),
                request), Times.Once);
    }

    [Test]
    public async Task UpdateNote_ValidRequest_ReturnsSuccessResponse()
    {
        var request = new UpdateNoteCommandProto
        {
            NoteId = Guid.NewGuid().ToString(),
            Text = "Updated Note",
            NoteTypeId = Guid.NewGuid().ToString(),
            TimeSlotId = Guid.NewGuid().ToString(),
            TraceData = new TraceDataProto { TraceId = Guid.NewGuid().ToString() }
        };

        var response = await _noteCommandReceiver.UpdateNote(request, Mock.Of<ServerCallContext>());

        Assert.That(response.Success);
        _noteCommandsServiceMock.Verify(s => s.Update(It.IsAny<UpdateNoteCommand>()), Times.Once);
        _traceCollectorMock.Verify(
            t => t.Note.Update.CommandReceived(typeof(NoteCommandReceiver), Guid.Parse(request.TraceData.TraceId),
                request), Times.Once);
    }

    [Test]
    public void CreateNote_ServiceThrowsException_ReturnsFailureResponse()
    {
        var request = new CreateNoteCommandProto
        {
            NoteId = Guid.NewGuid().ToString(),
            Text = "Test Note",
            NoteTypeId = Guid.NewGuid().ToString(),
            TicketId = Guid.NewGuid().ToString(),
            TimeSlotId = Guid.NewGuid().ToString(),
            TimeStamp = Timestamp.FromDateTimeOffset(DateTimeOffset.UtcNow),
            TraceData = new TraceDataProto { TraceId = Guid.NewGuid().ToString() }
        };

        _noteCommandsServiceMock
            .Setup(s => s.Create(It.IsAny<CreateNoteCommand>()))
            .ThrowsAsync(new InvalidOperationException());

        Assert.ThrowsAsync<InvalidOperationException>(() =>
            _noteCommandReceiver.CreateNote(request, Mock.Of<ServerCallContext>()));
        _traceCollectorMock.Verify(
            t => t.Note.Create.CommandReceived(typeof(NoteCommandReceiver), Guid.Parse(request.TraceData.TraceId),
                request), Times.Once);
    }

    [Test]
    public void UpdateNote_ServiceThrowsException_ReturnsFailureResponse()
    {
        var request = new UpdateNoteCommandProto
        {
            NoteId = Guid.NewGuid().ToString(),
            Text = "Updated Note",
            NoteTypeId = Guid.NewGuid().ToString(),
            TimeSlotId = Guid.NewGuid().ToString(),
            TraceData = new TraceDataProto { TraceId = Guid.NewGuid().ToString() }
        };

        _noteCommandsServiceMock
            .Setup(s => s.Update(It.IsAny<UpdateNoteCommand>()))
            .ThrowsAsync(new InvalidOperationException());

        Assert.ThrowsAsync<InvalidOperationException>(() =>
            _noteCommandReceiver.UpdateNote(request, Mock.Of<ServerCallContext>()));
        _traceCollectorMock.Verify(
            t => t.Note.Update.CommandReceived(typeof(NoteCommandReceiver), Guid.Parse(request.TraceData.TraceId),
                request), Times.Once);
    }
}