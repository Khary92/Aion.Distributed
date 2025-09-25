using Core.Server.Communication.Endpoints.Client;
using Core.Server.Services.Client;
using Core.Server.Tracing.Tracing.Tracers;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Moq;
using Proto.Command.Client;
using Proto.DTO.TraceData;

namespace Core.Server.Test.Communication.Endpoints.Client;

[TestFixture]
[TestOf(typeof(ClientCommandReceiver))]
public class ClientCommandReceiverTest
{
    private Mock<ITrackingControlService> _serviceMock;

    private Mock<ITraceCollector> _tracerMock = new()
    {
        DefaultValueProvider = DefaultValueProvider.Mock
    };

    private ClientCommandReceiver _receiver;

    [SetUp]
    public void SetUp()
    {
        _serviceMock = new Mock<ITrackingControlService>();
        _receiver = new ClientCommandReceiver(_serviceMock.Object, _tracerMock.Object);
    }

    [Test]
    public async Task CreateTrackingControl_ValidRequest_ReturnsSuccessResponse()
    {
        var request = new CreateTrackingControlCommandProto
        {
            TicketId = Guid.NewGuid().ToString(),
            Date = Timestamp.FromDateTimeOffset(DateTimeOffset.UtcNow),
            TraceData = new TraceDataProto { TraceId = Guid.NewGuid().ToString() }
        };

        var response = await _receiver.CreateTimeSlotControl(request, Mock.Of<ServerCallContext>());

        Assert.That(response.Success);
        _serviceMock.Verify(s => s.Create(
                It.Is<Guid>(g => g == Guid.Parse(request.TicketId)),
                It.Is<DateTimeOffset>(d => d == request.Date.ToDateTimeOffset()),
                It.Is<Guid>(g => g == Guid.Parse(request.TraceData.TraceId))),
            Times.Once);
    }

    [Test]
    public void CreateTrackingControl_InvalidTicketId_ThrowsFormatException()
    {
        var request = new CreateTrackingControlCommandProto
        {
            TicketId = "invalid-guid",
            Date = Timestamp.FromDateTimeOffset(DateTimeOffset.UtcNow),
            TraceData = new TraceDataProto { TraceId = Guid.NewGuid().ToString() }
        };

        Assert.ThrowsAsync<FormatException>(() =>
            _receiver.CreateTimeSlotControl(request, Mock.Of<ServerCallContext>()));
    }

    [Test]
    public void CreateTrackingControl_InvalidTraceId_ThrowsFormatException()
    {
        var request = new CreateTrackingControlCommandProto
        {
            TicketId = Guid.NewGuid().ToString(),
            Date = Timestamp.FromDateTimeOffset(DateTimeOffset.UtcNow),
            TraceData = new TraceDataProto { TraceId = "invalid-guid" }
        };

        Assert.ThrowsAsync<FormatException>(() =>
            _receiver.CreateTimeSlotControl(request, Mock.Of<ServerCallContext>()));
    }

    [Test]
    public void CreateTrackingControl_ServiceThrowsException_ExceptionPropagates()
    {
        var request = new CreateTrackingControlCommandProto
        {
            TicketId = Guid.NewGuid().ToString(),
            Date = Timestamp.FromDateTimeOffset(DateTimeOffset.UtcNow),
            TraceData = new TraceDataProto { TraceId = Guid.NewGuid().ToString() }
        };

        _serviceMock
            .Setup(s => s.Create(
                It.IsAny<Guid>(),
                It.IsAny<DateTimeOffset>(),
                It.IsAny<Guid>()))
            .ThrowsAsync(new InvalidOperationException());

        Assert.ThrowsAsync<InvalidOperationException>(() =>
            _receiver.CreateTimeSlotControl(request, Mock.Of<ServerCallContext>()));

        _serviceMock.Verify(s => s.Create(
                It.Is<Guid>(g => g == Guid.Parse(request.TicketId)),
                It.Is<DateTimeOffset>(d => d == request.Date.ToDateTimeOffset()),
                It.Is<Guid>(g => g == Guid.Parse(request.TraceData.TraceId))),
            Times.Once);
    }
}