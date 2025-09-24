using Core.Server.Communication.Endpoints.NoteType;
using Core.Server.Communication.Records.Commands.Entities.NoteType;
using Core.Server.Services.Entities.NoteTypes;
using Core.Server.Tracing.Tracing.Tracers;
using Core.Server.Tracing.Tracing.Tracers.NoteType.UseCase;
using Grpc.Core;
using Moq;
using Proto.Command.NoteTypes;
using Proto.DTO.TraceData;

namespace Core.Server.Test.Communication.Endpoints.NoteType;

[TestFixture]
[TestOf(typeof(NoteTypeCommandReceiver))]
public class NoteTypeCommandReceiverTest
{
    private NoteTypeCommandReceiver _receiver;
    private Mock<INoteTypeCommandsService> _mockService;
    private Mock<ITraceCollector> _mockTracer;

    [SetUp]
    public void SetUp()
    {
        _mockService = new Mock<INoteTypeCommandsService>();
        _mockTracer = new Mock<ITraceCollector>();
        _mockTracer.Setup(t => t.NoteType.ChangeColor).Returns(Mock.Of<IChangeNoteTypeColorTraceCollector>());
        _mockTracer.Setup(t => t.NoteType.ChangeName).Returns(Mock.Of<IChangeNoteTypeNameTraceCollector>());
        _mockTracer.Setup(t => t.NoteType.Create).Returns(Mock.Of<ICreateNoteTypeTraceCollector>());

        _receiver = new NoteTypeCommandReceiver(_mockService.Object, _mockTracer.Object);
    }

    [Test]
    public async Task ChangeNoteTypeColor_SuccessfulCommand_ResponseSuccess()
    {
        var command = new ChangeNoteTypeColorCommandProto
        {
            NoteTypeId = Guid.NewGuid().ToString(),
            Color = "Red",
            TraceData = new TraceDataProto
            {
                TraceId = Guid.NewGuid().ToString()
            }
        };

        var response = await _receiver.ChangeNoteTypeColor(command, Mock.Of<ServerCallContext>());

        Assert.That(response.Success);
        _mockService.Verify(s => s.ChangeColor(It.IsAny<ChangeNoteTypeColorCommand>()), Times.Once);
        _mockTracer.Verify(t => t.NoteType.ChangeColor.CommandReceived(
                typeof(NoteTypeCommandReceiver),
                Guid.Parse(command.TraceData.TraceId),
                command),
            Times.Once);
    }

    [Test]
    public void ChangeNoteTypeColor_InvalidGuid_ThrowsFormatException()
    {
        var command = new ChangeNoteTypeColorCommandProto
        {
            NoteTypeId = "invalid-guid",
            Color = "Red",
            TraceData = new TraceDataProto
            {
                TraceId = Guid.NewGuid().ToString()
            }
        };

        Assert.ThrowsAsync<FormatException>(async () =>
            await _receiver.ChangeNoteTypeColor(command, Mock.Of<ServerCallContext>()));
    }

    [Test]
    public async Task ChangeNoteTypeName_SuccessfulCommand_ResponseSuccess()
    {
        var command = new ChangeNoteTypeNameCommandProto
        {
            NoteTypeId = Guid.NewGuid().ToString(),
            Name = "New Name",
            TraceData = new TraceDataProto
            {
                TraceId = Guid.NewGuid().ToString()
            }
        };

        var response = await _receiver.ChangeNoteTypeName(command, Mock.Of<ServerCallContext>());

        Assert.That(response.Success);
        _mockService.Verify(s => s.ChangeName(It.IsAny<ChangeNoteTypeNameCommand>()), Times.Once);
        _mockTracer.Verify(t => t.NoteType.ChangeName.CommandReceived(
                typeof(NoteTypeCommandReceiver),
                Guid.Parse(command.TraceData.TraceId),
                command),
            Times.Once);
    }

    [Test]
    public void ChangeNoteTypeName_InvalidGuid_ThrowsFormatException()
    {
        var command = new ChangeNoteTypeNameCommandProto
        {
            NoteTypeId = "invalid-guid",
            Name = "New Name",
            TraceData = new TraceDataProto
            {
                TraceId = Guid.NewGuid().ToString()
            }
        };

        Assert.ThrowsAsync<FormatException>(async () =>
            await _receiver.ChangeNoteTypeName(command, Mock.Of<ServerCallContext>()));
    }

    [Test]
    public async Task CreateNoteType_SuccessfulCommand_ResponseSuccess()
    {
        var command = new CreateNoteTypeCommandProto
        {
            NoteTypeId = Guid.NewGuid().ToString(),
            Name = "Test Name",
            Color = "Blue",
            TraceData = new TraceDataProto
            {
                TraceId = Guid.NewGuid().ToString()
            }
        };

        var response = await _receiver.CreateNoteType(command, Mock.Of<ServerCallContext>());

        Assert.That(response.Success);
        _mockService.Verify(s => s.Create(It.IsAny<CreateNoteTypeCommand>()), Times.Once);
        _mockTracer.Verify(t => t.NoteType.Create.CommandReceived(
                typeof(NoteTypeCommandReceiver),
                Guid.Parse(command.TraceData.TraceId),
                command),
            Times.Once);
    }

    [Test]
    public void CreateNoteType_InvalidGuid_ThrowsFormatException()
    {
        var command = new CreateNoteTypeCommandProto
        {
            NoteTypeId = "invalid-guid",
            Name = "Test Name",
            Color = "Blue",
            TraceData = new TraceDataProto
            {
                TraceId = Guid.NewGuid().ToString()
            }
        };

        Assert.ThrowsAsync<FormatException>(async () =>
            await _receiver.CreateNoteType(command, Mock.Of<ServerCallContext>()));
    }

    [Test]
    public void ChangeNoteTypeColor_ServiceThrowsException_ReturnsFailureResponse()
    {
        var command = new ChangeNoteTypeColorCommandProto
        {
            NoteTypeId = Guid.NewGuid().ToString(),
            Color = "Red",
            TraceData = new TraceDataProto { TraceId = Guid.NewGuid().ToString() }
        };

        _mockService
            .Setup(s => s.ChangeColor(It.IsAny<ChangeNoteTypeColorCommand>()))
            .ThrowsAsync(new InvalidOperationException());

        Assert.ThrowsAsync<InvalidOperationException>(() =>
            _receiver.ChangeNoteTypeColor(command, Mock.Of<ServerCallContext>()));
        _mockTracer.Verify(t => t.NoteType.ChangeColor.CommandReceived(
                typeof(NoteTypeCommandReceiver),
                Guid.Parse(command.TraceData.TraceId),
                command),
            Times.Once);
    }

    [Test]
    public void ChangeNoteTypeName_ServiceThrowsException_ReturnsFailureResponse()
    {
        var command = new ChangeNoteTypeNameCommandProto
        {
            NoteTypeId = Guid.NewGuid().ToString(),
            Name = "New Name",
            TraceData = new TraceDataProto { TraceId = Guid.NewGuid().ToString() }
        };

        _mockService
            .Setup(s => s.ChangeName(It.IsAny<ChangeNoteTypeNameCommand>()))
            .ThrowsAsync(new InvalidOperationException());

        Assert.ThrowsAsync<InvalidOperationException>(() =>
            _receiver.ChangeNoteTypeName(command, Mock.Of<ServerCallContext>()));
        _mockTracer.Verify(t => t.NoteType.ChangeName.CommandReceived(
                typeof(NoteTypeCommandReceiver),
                Guid.Parse(command.TraceData.TraceId),
                command),
            Times.Once);
    }

    [Test]
    public void CreateNoteType_ServiceThrowsException_ReturnsFailureResponse()
    {
        var command = new CreateNoteTypeCommandProto
        {
            NoteTypeId = Guid.NewGuid().ToString(),
            Name = "Test Name",
            Color = "Blue",
            TraceData = new TraceDataProto { TraceId = Guid.NewGuid().ToString() }
        };

        _mockService
            .Setup(s => s.Create(It.IsAny<CreateNoteTypeCommand>()))
            .ThrowsAsync(new InvalidOperationException());

        Assert.ThrowsAsync<InvalidOperationException>(() =>
            _receiver.CreateNoteType(command, Mock.Of<ServerCallContext>()));
        _mockTracer.Verify(t => t.NoteType.Create.CommandReceived(
                typeof(NoteTypeCommandReceiver),
                Guid.Parse(command.TraceData.TraceId),
                command),
            Times.Once);
    }
}