using Core.Server.Communication.Endpoints.Ticket;
using Core.Server.Communication.Records.Commands.Entities.Tickets;
using Core.Server.Services.Entities.Tickets;
using Core.Server.Tracing.Tracing.Tracers;
using Core.Server.Tracing.Tracing.Tracers.Ticket.UseCase;
using Grpc.Core;
using Moq;
using Proto.Command.Tickets;
using Proto.DTO.TraceData;

namespace Core.Server.Test.Communication.Endpoints.Ticket;

[TestFixture]
[TestOf(typeof(TicketCommandReceiver))]
public class TicketCommandReceiverTest
{
    private Mock<ITicketCommandsService> _serviceMock;
    private Mock<ITraceCollector> _tracerMock;
    private TicketCommandReceiver _receiver;

    [SetUp]
    public void SetUp()
    {
        _serviceMock = new Mock<ITicketCommandsService>();
        _tracerMock = new Mock<ITraceCollector>()
        {
            DefaultValueProvider = DefaultValueProvider.Mock
        };
        _tracerMock.Setup(t => t.Ticket.Create).Returns(Mock.Of<ICreateTicketTraceCollector>());
        _tracerMock.Setup(t => t.Ticket.Update).Returns(Mock.Of<IUpdateTicketTraceCollector>());

        _receiver = new TicketCommandReceiver(_serviceMock.Object, _tracerMock.Object);
    }

    [Test]
    public async Task CreateTicket_ValidRequest_ReturnsSuccessResponse()
    {
        var request = new CreateTicketCommandProto
        {
            TicketId = Guid.NewGuid().ToString(),
            Name = "Bug",
            TraceData = new TraceDataProto { TraceId = Guid.NewGuid().ToString() }
        };

        var response = await _receiver.CreateTicket(request, Mock.Of<ServerCallContext>());

        Assert.That(response.Success);
        _serviceMock.Verify(s => s.Create(It.IsAny<CreateTicketCommand>()), Times.Once);
        _tracerMock.Verify(t => t.Ticket.Create.CommandReceived(
                typeof(TicketCommandReceiver),
                Guid.Parse(request.TraceData.TraceId),
                request),
            Times.Once);
    }

    [Test]
    public void CreateTicket_InvalidTraceId_ThrowsFormatException()
    {
        var request = new CreateTicketCommandProto
        {
            TicketId = Guid.NewGuid().ToString(),
            Name = "Bug",
            TraceData = new TraceDataProto { TraceId = "invalid-guid" }
        };

        Assert.ThrowsAsync<FormatException>(() =>
            _receiver.CreateTicket(request, Mock.Of<ServerCallContext>()));
    }

    [Test]
    public void CreateTicket_ServiceThrowsException_ExceptionPropagates()
    {
        var request = new CreateTicketCommandProto
        {
            TicketId = Guid.NewGuid().ToString(),
            Name = "Bug",
            TraceData = new TraceDataProto { TraceId = Guid.NewGuid().ToString() }
        };

        _serviceMock
            .Setup(s => s.Create(It.IsAny<CreateTicketCommand>()))
            .ThrowsAsync(new InvalidOperationException());

        Assert.ThrowsAsync<InvalidOperationException>(() =>
            _receiver.CreateTicket(request, Mock.Of<ServerCallContext>()));

        _tracerMock.Verify(t => t.Ticket.Create.CommandReceived(
                typeof(TicketCommandReceiver),
                Guid.Parse(request.TraceData.TraceId),
                request),
            Times.Once);
    }

    [Test]
    public async Task UpdateTicketData_ValidRequest_ReturnsSuccessResponse()
    {
        var request = new UpdateTicketDataCommandProto
        {
            TicketId = Guid.NewGuid().ToString(),
            Name = "New Name",
            TraceData = new TraceDataProto { TraceId = Guid.NewGuid().ToString() }
        };

        var response = await _receiver.UpdateTicketData(request, Mock.Of<ServerCallContext>());

        Assert.That(response.Success);
        _serviceMock.Verify(s => s.UpdateData(It.IsAny<UpdateTicketDataCommand>()), Times.Once);
        _tracerMock.Verify(t => t.Ticket.Update.CommandReceived(
                typeof(TicketCommandReceiver),
                Guid.Parse(request.TraceData.TraceId),
                request),
            Times.Once);
    }

    [Test]
    public void UpdateTicketData_InvalidTraceId_ThrowsFormatException()
    {
        var request = new UpdateTicketDataCommandProto
        {
            TicketId = Guid.NewGuid().ToString(),
            Name = "New Name",
            TraceData = new TraceDataProto { TraceId = "invalid-guid" }
        };

        Assert.ThrowsAsync<FormatException>(() =>
            _receiver.UpdateTicketData(request, Mock.Of<ServerCallContext>()));
    }

    [Test]
    public void UpdateTicketData_ServiceThrowsException_ExceptionPropagates()
    {
        var request = new UpdateTicketDataCommandProto
        {
            TicketId = Guid.NewGuid().ToString(),
            Name = "New Name",
            TraceData = new TraceDataProto { TraceId = Guid.NewGuid().ToString() }
        };

        _serviceMock
            .Setup(s => s.UpdateData(It.IsAny<UpdateTicketDataCommand>()))
            .ThrowsAsync(new InvalidOperationException());

        Assert.ThrowsAsync<InvalidOperationException>(() =>
            _receiver.UpdateTicketData(request, Mock.Of<ServerCallContext>()));

        _tracerMock.Verify(t => t.Ticket.Update.CommandReceived(
                typeof(TicketCommandReceiver),
                Guid.Parse(request.TraceData.TraceId),
                request),
            Times.Once);
    }

    [Test]
    public async Task UpdateTicketDocumentation_ValidRequest_ReturnsSuccessResponse()
    {
        var request = new UpdateTicketDocumentationCommandProto
        {
            TicketId = Guid.NewGuid().ToString(),
            Documentation = "Updated docs",
            TraceData = new TraceDataProto { TraceId = Guid.NewGuid().ToString() }
        };

        var response = await _receiver.UpdateTicketDocumentation(request, Mock.Of<ServerCallContext>());

        Assert.That(response.Success);
        _serviceMock.Verify(s => s.UpdateDocumentation(It.IsAny<UpdateTicketDocumentationCommand>()), Times.Once);
    }

    [Test]
    public void UpdateTicketDocumentation_ServiceThrowsException_ExceptionPropagates()
    {
        var request = new UpdateTicketDocumentationCommandProto
        {
            TicketId = Guid.NewGuid().ToString(),
            Documentation = "Updated docs",
            TraceData = new TraceDataProto { TraceId = Guid.NewGuid().ToString() }
        };

        _serviceMock
            .Setup(s => s.UpdateDocumentation(It.IsAny<UpdateTicketDocumentationCommand>()))
            .ThrowsAsync(new InvalidOperationException());

        Assert.ThrowsAsync<InvalidOperationException>(() =>
            _receiver.UpdateTicketDocumentation(request, Mock.Of<ServerCallContext>()));
    }
}