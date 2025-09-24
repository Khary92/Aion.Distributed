using Core.Server.Communication.Endpoints.TimeSlot;
using Core.Server.Communication.Records.Commands.Entities.TimeSlots;
using Core.Server.Services.Entities.TimeSlots;
using Core.Server.Tracing.Tracing.Tracers;
using Core.Server.Tracing.Tracing.Tracers.TimeSlot.UseCase;
using Grpc.Core;
using Moq;
using Proto.Command.TimeSlots;
using Proto.DTO.TraceData;

namespace Core.Server.Test.Communication.Endpoints.TimeSlot;

[TestFixture]
[TestOf(typeof(TimeSlotCommandReceiver))]
public class TimeSlotCommandReceiverTest
{
    private Mock<ITimeSlotCommandsService> _serviceMock;
    private Mock<ITraceCollector> _tracerMock;
    private TimeSlotCommandReceiver _receiver;

    [SetUp]
    public void SetUp()
    {
        _serviceMock = new Mock<ITimeSlotCommandsService>();
        _tracerMock = new Mock<ITraceCollector>();

        _tracerMock.Setup(t => t.TimeSlot.Create).Returns(Mock.Of<ICreateTimeSlotTraceCollector>());
        _tracerMock.Setup(t => t.TimeSlot.AddNote).Returns(Mock.Of<IAddNoteTraceCollector>());
        _tracerMock.Setup(t => t.TimeSlot.SetStartTime).Returns(Mock.Of<ISetStartTimeTraceCollector>());
        _tracerMock.Setup(t => t.TimeSlot.SetEndTime).Returns(Mock.Of<ISetEndTimeTraceCollector>());

        _receiver = new TimeSlotCommandReceiver(_serviceMock.Object, _tracerMock.Object);
    }
    
    [Test]
    public void CreateTimeSlot_InvalidTraceId_ThrowsFormatException()
    {
        var request = new CreateTimeSlotCommandProto
        {
            TimeSlotId = Guid.NewGuid().ToString(),
            TraceData = new TraceDataProto { TraceId = "invalid-guid" }
        };

        Assert.ThrowsAsync<FormatException>(() =>
            _receiver.CreateTimeSlot(request, Mock.Of<ServerCallContext>()));
    }
    
    [Test]
    public async Task AddNote_ValidRequest_ReturnsSuccessResponse()
    {
        var request = new AddNoteCommandProto
        {
            TimeSlotId = Guid.NewGuid().ToString(),
            NoteId = Guid.NewGuid().ToString(),
            TraceData = new TraceDataProto { TraceId = Guid.NewGuid().ToString() }
        };

        var response = await _receiver.AddNote(request, Mock.Of<ServerCallContext>());

        Assert.That(response.Success);
        _serviceMock.Verify(s => s.AddNote(It.IsAny<AddNoteCommand>()), Times.Once);
        _tracerMock.Verify(t => t.TimeSlot.AddNote.CommandReceived(
                typeof(TimeSlotCommandReceiver),
                Guid.Parse(request.TraceData.TraceId),
                request),
            Times.Once);
    }

    [Test]
    public void AddNote_InvalidTraceId_ThrowsFormatException()
    {
        var request = new AddNoteCommandProto
        {
            TimeSlotId = Guid.NewGuid().ToString(),
            NoteId = Guid.NewGuid().ToString(),
            TraceData = new TraceDataProto { TraceId = "invalid-guid" }
        };

        Assert.ThrowsAsync<FormatException>(() =>
            _receiver.AddNote(request, Mock.Of<ServerCallContext>()));
    }

    [Test]
    public void AddNote_ServiceThrowsException_ExceptionPropagates()
    {
        var request = new AddNoteCommandProto
        {
            TimeSlotId = Guid.NewGuid().ToString(),
            NoteId = Guid.NewGuid().ToString(),
            TraceData = new TraceDataProto { TraceId = Guid.NewGuid().ToString() }
        };

        _serviceMock
            .Setup(s => s.AddNote(It.IsAny<AddNoteCommand>()))
            .ThrowsAsync(new InvalidOperationException());

        Assert.ThrowsAsync<InvalidOperationException>(() =>
            _receiver.AddNote(request, Mock.Of<ServerCallContext>()));

        _tracerMock.Verify(t => t.TimeSlot.AddNote.CommandReceived(
                typeof(TimeSlotCommandReceiver),
                Guid.Parse(request.TraceData.TraceId),
                request),
            Times.Once);
    }

    [Test]
    public void SetStartTime_InvalidTraceId_ThrowsFormatException()
    {
        var request = new SetStartTimeCommandProto
        {
            TimeSlotId = Guid.NewGuid().ToString(),
            TraceData = new TraceDataProto { TraceId = "invalid-guid" }
        };

        Assert.ThrowsAsync<FormatException>(() =>
            _receiver.SetStartTime(request, Mock.Of<ServerCallContext>()));
    }
    
    [Test]
    public void SetEndTime_InvalidTraceId_ThrowsFormatException()
    {
        var request = new SetEndTimeCommandProto
        {
            TimeSlotId = Guid.NewGuid().ToString(),
            TraceData = new TraceDataProto { TraceId = "invalid-guid" }
        };

        Assert.ThrowsAsync<FormatException>(() =>
            _receiver.SetEndTime(request, Mock.Of<ServerCallContext>()));
    }
}