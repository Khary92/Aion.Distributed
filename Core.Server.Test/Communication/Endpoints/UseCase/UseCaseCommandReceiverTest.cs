using Core.Server.Communication.Endpoints.UseCase;
using Core.Server.Services.UseCase;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Moq;
using Proto.Command.UseCases;
using Proto.DTO.TraceData;

namespace Core.Server.Test.Communication.Endpoints.UseCase;

[TestFixture]
[TestOf(typeof(UseCaseCommandReceiver))]
public class UseCaseCommandReceiverTest
{
    private Mock<ITimeSlotControlService> _serviceMock;
    private UseCaseCommandReceiver _receiver;

    [SetUp]
    public void SetUp()
    {
        _serviceMock = new Mock<ITimeSlotControlService>();
        _receiver = new UseCaseCommandReceiver(_serviceMock.Object);
    }

    [Test]
    public async Task CreateTimeSlotControl_ValidRequest_ReturnsSuccessResponse()
    {
        var request = new CreateTimeSlotControlCommandProto
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
    public void CreateTimeSlotControl_InvalidTicketId_ThrowsFormatException()
    {
        var request = new CreateTimeSlotControlCommandProto
        {
            TicketId = "invalid-guid",
            Date = Timestamp.FromDateTimeOffset(DateTimeOffset.UtcNow),
            TraceData = new TraceDataProto { TraceId = Guid.NewGuid().ToString() }
        };

        Assert.ThrowsAsync<FormatException>(() =>
            _receiver.CreateTimeSlotControl(request, Mock.Of<ServerCallContext>()));
    }

    [Test]
    public void CreateTimeSlotControl_InvalidTraceId_ThrowsFormatException()
    {
        var request = new CreateTimeSlotControlCommandProto
        {
            TicketId = Guid.NewGuid().ToString(),
            Date = Timestamp.FromDateTimeOffset(DateTimeOffset.UtcNow),
            TraceData = new TraceDataProto { TraceId = "invalid-guid" }
        };

        Assert.ThrowsAsync<FormatException>(() =>
            _receiver.CreateTimeSlotControl(request, Mock.Of<ServerCallContext>()));
    }

    [Test]
    public void CreateTimeSlotControl_ServiceThrowsException_ExceptionPropagates()
    {
        var request = new CreateTimeSlotControlCommandProto
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