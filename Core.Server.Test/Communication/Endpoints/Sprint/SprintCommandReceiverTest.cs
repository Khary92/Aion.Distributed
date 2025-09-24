using Core.Server.Communication.Endpoints.Sprint;
using Core.Server.Communication.Endpoints.Sprint.Handlers;
using Core.Server.Communication.Records.Commands.Entities.Sprints;
using Core.Server.Services.Entities.Sprints;
using Core.Server.Tracing.Tracing.Tracers;
using Grpc.Core;
using Moq;
using Proto.Command.Sprints;
using Proto.DTO.TraceData;

namespace Core.Server.Test.Communication.Endpoints.Sprint;

[TestFixture]
[TestOf(typeof(SprintCommandReceiver))]
public class SprintCommandReceiverTest
{
    private Mock<ISprintCommandsService> _serviceMock;
    private Mock<IAddTicketToActiveSprintCommandHandler> _addTicketHandlerMock;
    private Mock<ISetSprintActiveStatusCommandHandler> _setActiveStatusHandlerMock;
    private Mock<ITraceCollector> _tracerMock;
    private SprintCommandReceiver _receiver;

    [SetUp]
    public void SetUp()
    {
        _serviceMock = new Mock<ISprintCommandsService>();
        _addTicketHandlerMock = new Mock<IAddTicketToActiveSprintCommandHandler>();
        _setActiveStatusHandlerMock = new Mock<ISetSprintActiveStatusCommandHandler>();
        _tracerMock = new Mock<ITraceCollector>()
        {
            DefaultValueProvider = DefaultValueProvider.Mock
        };

        _receiver = new SprintCommandReceiver(
            _serviceMock.Object,
            _addTicketHandlerMock.Object,
            _setActiveStatusHandlerMock.Object,
            _tracerMock.Object);
    }

    [Test]
    public async Task AddTicketToActiveSprint_ValidRequest_ReturnsSuccessResponse()
    {
        var request = new AddTicketToActiveSprintCommandProto
        {
            TicketId = Guid.NewGuid().ToString(),
            TraceData = new TraceDataProto { TraceId = Guid.NewGuid().ToString() }
        };

        var response = await _receiver.AddTicketToActiveSprint(request, Mock.Of<ServerCallContext>());

        Assert.That(response.Success);
        _addTicketHandlerMock.Verify(h => h.Handle(It.IsAny<AddTicketToActiveSprintCommand>()), Times.Once);
        _tracerMock.Verify(t => t.Sprint.AddTicketToSprint.CommandReceived(
                typeof(SprintCommandReceiver),
                Guid.Parse(request.TraceData.TraceId),
                It.Is<AddTicketToActiveSprintCommand>(c => c.TraceId == Guid.Parse(request.TraceData.TraceId))),
            Times.Once);
    }

    [Test]
    public void AddTicketToActiveSprint_InvalidTraceId_ThrowsFormatException()
    {
        var request = new AddTicketToActiveSprintCommandProto
        {
            TicketId = Guid.NewGuid().ToString(),
            TraceData = new TraceDataProto { TraceId = "invalid-guid" }
        };

        Assert.ThrowsAsync<FormatException>(() =>
            _receiver.AddTicketToActiveSprint(request, Mock.Of<ServerCallContext>()));
    }

    [Test]
    public void AddTicketToActiveSprint_HandlerThrowsException_ExceptionPropagates()
    {
        var request = new AddTicketToActiveSprintCommandProto
        {
            TicketId = Guid.NewGuid().ToString(),
            TraceData = new TraceDataProto { TraceId = Guid.NewGuid().ToString() }
        };

        _addTicketHandlerMock
            .Setup(h => h.Handle(It.IsAny<AddTicketToActiveSprintCommand>()))
            .ThrowsAsync(new InvalidOperationException());

        Assert.ThrowsAsync<InvalidOperationException>(() =>
            _receiver.AddTicketToActiveSprint(request, Mock.Of<ServerCallContext>()));

        _tracerMock.Verify(t => t.Sprint.AddTicketToSprint.CommandReceived(
                typeof(SprintCommandReceiver),
                Guid.Parse(request.TraceData.TraceId),
                It.Is<AddTicketToActiveSprintCommand>(c => c.TraceId == Guid.Parse(request.TraceData.TraceId))),
            Times.Once);
    }

    [Test]
    public void CreateSprint_InvalidTraceId_ThrowsFormatException()
    {
        var request = new CreateSprintCommandProto
        {
            SprintId = Guid.NewGuid().ToString(),
            Name = "Sprint 1",
            TraceData = new TraceDataProto { TraceId = "invalid-guid" }
        };

        Assert.ThrowsAsync<FormatException>(() =>
            _receiver.CreateSprint(request, Mock.Of<ServerCallContext>()));
    }
    

    [Test]
    public async Task SetSprintActiveStatus_ValidRequest_ReturnsSuccessResponse()
    {
        var request = new SetSprintActiveStatusCommandProto
        {
            SprintId = Guid.NewGuid().ToString(),
            IsActive = true,
            TraceData = new TraceDataProto { TraceId = Guid.NewGuid().ToString() }
        };

        var response = await _receiver.SetSprintActiveStatus(request, Mock.Of<ServerCallContext>());

        Assert.That(response.Success);
        _setActiveStatusHandlerMock.Verify(h => h.Handle(It.IsAny<SetSprintActiveStatusCommand>()), Times.Once);
        _tracerMock.Verify(t => t.Sprint.ActiveStatus.CommandReceived(
                typeof(SprintCommandReceiver),
                Guid.Parse(request.TraceData.TraceId),
                request),
            Times.Once);
    }

    [Test]
    public void SetSprintActiveStatus_InvalidTraceId_ThrowsFormatException()
    {
        var request = new SetSprintActiveStatusCommandProto
        {
            SprintId = Guid.NewGuid().ToString(),
            IsActive = true,
            TraceData = new TraceDataProto { TraceId = "invalid-guid" }
        };

        Assert.ThrowsAsync<FormatException>(() =>
            _receiver.SetSprintActiveStatus(request, Mock.Of<ServerCallContext>()));
    }

    [Test]
    public void SetSprintActiveStatus_HandlerThrowsException_ExceptionPropagates()
    {
        var request = new SetSprintActiveStatusCommandProto
        {
            SprintId = Guid.NewGuid().ToString(),
            IsActive = false,
            TraceData = new TraceDataProto { TraceId = Guid.NewGuid().ToString() }
        };

        _setActiveStatusHandlerMock
            .Setup(h => h.Handle(It.IsAny<SetSprintActiveStatusCommand>()))
            .ThrowsAsync(new InvalidOperationException());

        Assert.ThrowsAsync<InvalidOperationException>(() =>
            _receiver.SetSprintActiveStatus(request, Mock.Of<ServerCallContext>()));

        _tracerMock.Verify(t => t.Sprint.ActiveStatus.CommandReceived(
                typeof(SprintCommandReceiver),
                Guid.Parse(request.TraceData.TraceId),
                request),
            Times.Once);
    }
    
    [Test]
    public void UpdateSprintData_InvalidTraceId_ThrowsFormatException()
    {
        var request = new UpdateSprintDataCommandProto
        {
            SprintId = Guid.NewGuid().ToString(),
            Name = "New Name",
            TraceData = new TraceDataProto { TraceId = "invalid-guid" }
        };

        Assert.ThrowsAsync<FormatException>(() =>
            _receiver.UpdateSprintData(request, Mock.Of<ServerCallContext>()));
    }
}