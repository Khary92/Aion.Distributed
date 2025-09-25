using Core.Server.Communication.Endpoints.StatisticsData;
using Core.Server.Communication.Records.Commands.Entities.StatisticsData;
using Core.Server.Services.Entities.StatisticsData;
using Core.Server.Tracing.Tracing.Tracers;
using Core.Server.Tracing.Tracing.Tracers.Statistics.UseCase;
using Grpc.Core;
using Moq;
using Proto.Command.StatisticsData;
using Proto.DTO.TraceData;

namespace Core.Server.Test.Communication.Endpoints.StatisticsData;

[TestFixture]
[TestOf(typeof(StatisticsDataCommandReceiver))]
public class StatisticsDataCommandReceiverTest
{
    private Mock<IStatisticsDataCommandsService> _serviceMock;
    private Mock<ITraceCollector> _tracerMock;
    private StatisticsDataCommandReceiver _receiver;

    [SetUp]
    public void SetUp()
    {
        _serviceMock = new Mock<IStatisticsDataCommandsService>();
        _tracerMock = new Mock<ITraceCollector>();

        _tracerMock.Setup(t => t.Statistics.ChangeProductivity).Returns(Mock.Of<IChangeProductivityTraceCollector>());
        _tracerMock.Setup(t => t.Statistics.ChangeTagSelection).Returns(Mock.Of<IChangeTagSelectionTraceCollector>());

        _receiver = new StatisticsDataCommandReceiver(_serviceMock.Object, _tracerMock.Object);
    }

    [Test]
    public async Task ChangeProductivity_ValidRequest_ReturnsSuccessResponse()
    {
        var request = new ChangeProductivityCommandProto
        {
            StatisticsDataId = Guid.NewGuid().ToString(),
            IsProductive = true,
            IsNeutral = false,
            IsUnproductive = false,
            TraceData = new TraceDataProto { TraceId = Guid.NewGuid().ToString() }
        };

        var response = await _receiver.ChangeProductivity(request, Mock.Of<ServerCallContext>());

        Assert.That(response.Success);
        _serviceMock.Verify(s => s.ChangeProductivity(It.IsAny<ChangeProductivityCommand>()), Times.Once);
        _tracerMock.Verify(t => t.Statistics.ChangeProductivity.CommandReceived(
                typeof(StatisticsDataCommandReceiver),
                Guid.Parse(request.TraceData.TraceId),
                request),
            Times.Once);
    }

    [Test]
    public void ChangeProductivity_InvalidTraceId_ThrowsFormatException()
    {
        var request = new ChangeProductivityCommandProto
        {
            StatisticsDataId = Guid.NewGuid().ToString(),
            IsProductive = true,
            IsNeutral = false,
            IsUnproductive = false,
            TraceData = new TraceDataProto { TraceId = "invalid-guid" }
        };

        Assert.ThrowsAsync<FormatException>(() =>
            _receiver.ChangeProductivity(request, Mock.Of<ServerCallContext>()));
    }

    [Test]
    public void ChangeProductivity_ServiceThrowsException_ExceptionPropagates()
    {
        var request = new ChangeProductivityCommandProto
        {
            StatisticsDataId = Guid.NewGuid().ToString(),
            IsProductive = true,
            IsNeutral = false,
            IsUnproductive = false,
            TraceData = new TraceDataProto { TraceId = Guid.NewGuid().ToString() }
        };

        _serviceMock
            .Setup(s => s.ChangeProductivity(It.IsAny<ChangeProductivityCommand>()))
            .ThrowsAsync(new InvalidOperationException());

        Assert.ThrowsAsync<InvalidOperationException>(() =>
            _receiver.ChangeProductivity(request, Mock.Of<ServerCallContext>()));

        _tracerMock.Verify(t => t.Statistics.ChangeProductivity.CommandReceived(
                typeof(StatisticsDataCommandReceiver),
                Guid.Parse(request.TraceData.TraceId),
                request),
            Times.Once);
    }

    [Test]
    public async Task ChangeTagSelection_ValidRequest_ReturnsSuccessResponse()
    {
        var request = new ChangeTagSelectionCommandProto
        {
            StatisticsDataId = Guid.NewGuid().ToString(),
            TraceData = new TraceDataProto { TraceId = Guid.NewGuid().ToString() }
        };

        var response = await _receiver.ChangeTagSelection(request, Mock.Of<ServerCallContext>());

        Assert.That(response.Success);
        _serviceMock.Verify(s => s.ChangeTagSelection(It.IsAny<ChangeTagSelectionCommand>()), Times.Once);
        _tracerMock.Verify(t => t.Statistics.ChangeTagSelection.CommandReceived(
                typeof(StatisticsDataCommandReceiver),
                Guid.Parse(request.TraceData.TraceId),
                request),
            Times.Once);
    }

    [Test]
    public void ChangeTagSelection_InvalidTraceId_ThrowsFormatException()
    {
        var request = new ChangeTagSelectionCommandProto
        {
            StatisticsDataId = Guid.NewGuid().ToString(),
            TraceData = new TraceDataProto { TraceId = "invalid-guid" }
        };

        Assert.ThrowsAsync<FormatException>(() =>
            _receiver.ChangeTagSelection(request, Mock.Of<ServerCallContext>()));
    }

    [Test]
    public void ChangeTagSelection_ServiceThrowsException_ExceptionPropagates()
    {
        var request = new ChangeTagSelectionCommandProto
        {
            StatisticsDataId = Guid.NewGuid().ToString(),
            TraceData = new TraceDataProto { TraceId = Guid.NewGuid().ToString() }
        };

        _serviceMock
            .Setup(s => s.ChangeTagSelection(It.IsAny<ChangeTagSelectionCommand>()))
            .ThrowsAsync(new InvalidOperationException());

        Assert.ThrowsAsync<InvalidOperationException>(() =>
            _receiver.ChangeTagSelection(request, Mock.Of<ServerCallContext>()));

        _tracerMock.Verify(t => t.Statistics.ChangeTagSelection.CommandReceived(
                typeof(StatisticsDataCommandReceiver),
                Guid.Parse(request.TraceData.TraceId),
                request),
            Times.Once);
    }
}