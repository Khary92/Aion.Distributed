using Core.Server.Communication.Endpoints.Tag;
using Core.Server.Communication.Records.Commands.Entities.Tags;
using Core.Server.Services.Entities.Tags;
using Core.Server.Tracing.Tracing.Tracers;
using Core.Server.Tracing.Tracing.Tracers.Tag.UseCase;
using Grpc.Core;
using Moq;
using Proto.Command.Tags;
using Proto.DTO.TraceData;

namespace Core.Server.Test.Communication.Endpoints.Tag;

[TestFixture]
[TestOf(typeof(TagCommandReceiver))]
public class TagCommandReceiverTest
{
    [SetUp]
    public void SetUp()
    {
        _serviceMock = new Mock<ITagCommandsService>();
        _tracerMock = new Mock<ITraceCollector>();
        _tracerMock.Setup(t => t.Tag.Create).Returns(Mock.Of<ICreateTagTraceCollector>());
        _tracerMock.Setup(t => t.Tag.Update).Returns(Mock.Of<IUpdateTagTraceCollector>());

        _receiver = new TagCommandReceiver(_serviceMock.Object, _tracerMock.Object);
    }

    private Mock<ITagCommandsService> _serviceMock;
    private Mock<ITraceCollector> _tracerMock;
    private TagCommandReceiver _receiver;

    [Test]
    public async Task CreateTag_ValidRequest_ReturnsSuccessResponse()
    {
        var request = new CreateTagCommandProto
        {
            TagId = Guid.NewGuid().ToString(),
            Name = "Backend",
            TraceData = new TraceDataProto { TraceId = Guid.NewGuid().ToString() }
        };

        var response = await _receiver.CreateTag(request, Mock.Of<ServerCallContext>());

        Assert.That(response.Success);
        _serviceMock.Verify(s => s.Create(It.IsAny<CreateTagCommand>()), Times.Once);
        _tracerMock.Verify(t => t.Tag.Create.CommandReceived(
                typeof(TagCommandReceiver),
                Guid.Parse(request.TraceData.TraceId),
                request),
            Times.Once);
    }

    [Test]
    public void CreateTag_InvalidTraceId_ThrowsFormatException()
    {
        var request = new CreateTagCommandProto
        {
            TagId = Guid.NewGuid().ToString(),
            Name = "Backend",
            TraceData = new TraceDataProto { TraceId = "invalid-guid" }
        };

        Assert.ThrowsAsync<FormatException>(() =>
            _receiver.CreateTag(request, Mock.Of<ServerCallContext>()));
    }

    [Test]
    public void CreateTag_ServiceThrowsException_ExceptionPropagates()
    {
        var request = new CreateTagCommandProto
        {
            TagId = Guid.NewGuid().ToString(),
            Name = "Backend",
            TraceData = new TraceDataProto { TraceId = Guid.NewGuid().ToString() }
        };

        _serviceMock
            .Setup(s => s.Create(It.IsAny<CreateTagCommand>()))
            .ThrowsAsync(new InvalidOperationException());

        Assert.ThrowsAsync<InvalidOperationException>(() =>
            _receiver.CreateTag(request, Mock.Of<ServerCallContext>()));

        _tracerMock.Verify(t => t.Tag.Create.CommandReceived(
                typeof(TagCommandReceiver),
                Guid.Parse(request.TraceData.TraceId),
                request),
            Times.Once);
    }

    [Test]
    public async Task UpdateTag_ValidRequest_ReturnsSuccessResponse()
    {
        var request = new UpdateTagCommandProto
        {
            TagId = Guid.NewGuid().ToString(),
            Name = "Platform",
            TraceData = new TraceDataProto { TraceId = Guid.NewGuid().ToString() }
        };

        var response = await _receiver.UpdateTag(request, Mock.Of<ServerCallContext>());

        Assert.That(response.Success);
        _serviceMock.Verify(s => s.Update(It.IsAny<UpdateTagCommand>()), Times.Once);
        _tracerMock.Verify(t => t.Tag.Update.CommandReceived(
                typeof(TagCommandReceiver),
                Guid.Parse(request.TraceData.TraceId),
                request),
            Times.Once);
    }

    [Test]
    public void UpdateTag_InvalidTraceId_ThrowsFormatException()
    {
        var request = new UpdateTagCommandProto
        {
            TagId = Guid.NewGuid().ToString(),
            Name = "Platform",
            TraceData = new TraceDataProto { TraceId = "invalid-guid" }
        };

        Assert.ThrowsAsync<FormatException>(() =>
            _receiver.UpdateTag(request, Mock.Of<ServerCallContext>()));
    }

    [Test]
    public void UpdateTag_ServiceThrowsException_ExceptionPropagates()
    {
        var request = new UpdateTagCommandProto
        {
            TagId = Guid.NewGuid().ToString(),
            Name = "Platform",
            TraceData = new TraceDataProto { TraceId = Guid.NewGuid().ToString() }
        };

        _serviceMock
            .Setup(s => s.Update(It.IsAny<UpdateTagCommand>()))
            .ThrowsAsync(new InvalidOperationException());

        Assert.ThrowsAsync<InvalidOperationException>(() =>
            _receiver.UpdateTag(request, Mock.Of<ServerCallContext>()));

        _tracerMock.Verify(t => t.Tag.Update.CommandReceived(
                typeof(TagCommandReceiver),
                Guid.Parse(request.TraceData.TraceId),
                request),
            Times.Once);
    }
}