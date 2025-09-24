using Core.Server.Communication.Endpoints.TimerSettings;
using Core.Server.Communication.Records.Commands.Entities.TimerSettings;
using Core.Server.Services.Entities.TimerSettings;
using Core.Server.Tracing.Tracing.Tracers;
using Grpc.Core;
using Moq;
using Proto.Command.TimerSettings;
using Proto.DTO.TraceData;

namespace Core.Server.Test.Communication.Endpoints.TimerSettings;

[TestFixture]
[TestOf(typeof(TimerSettingsCommandReceiver))]
public class TimerSettingsCommandReceiverTest
{
    private Mock<ITimerSettingsCommandsService> _serviceMock;
    private Mock<ITraceCollector> _tracerMock;
    private TimerSettingsCommandReceiver _receiver;

    [SetUp]
    public void SetUp()
    {
        _serviceMock = new Mock<ITimerSettingsCommandsService>();
        _tracerMock = new Mock<ITraceCollector>()
        {
            DefaultValueProvider = DefaultValueProvider.Mock
        };

        _receiver = new TimerSettingsCommandReceiver(_serviceMock.Object, _tracerMock.Object);
    }

    [Test]
    public async Task CreateTimerSettings_ValidRequest_ReturnsSuccessResponse()
    {
        var request = new CreateTimerSettingsCommandProto
        {
            TimerSettingsId = Guid.NewGuid().ToString(),
            TraceData = new TraceDataProto { TraceId = Guid.NewGuid().ToString() }
        };

        var response = await _receiver.CreateTimerSettings(request, Mock.Of<ServerCallContext>());

        Assert.That(response.Success);
        _serviceMock.Verify(s => s.Create(It.IsAny<CreateTimerSettingsCommand>()), Times.Once);
        _tracerMock.Verify(t => t.TimerSettings.Create.CommandReceived(
                typeof(TimerSettingsCommandReceiver),
                Guid.Parse(request.TraceData.TraceId),
                request),
            Times.Once);
    }

    [Test]
    public void CreateTimerSettings_InvalidTraceId_ThrowsFormatException()
    {
        var request = new CreateTimerSettingsCommandProto
        {
            TimerSettingsId = Guid.NewGuid().ToString(),
            TraceData = new TraceDataProto { TraceId = "invalid-guid" }
        };

        Assert.ThrowsAsync<FormatException>(() =>
            _receiver.CreateTimerSettings(request, Mock.Of<ServerCallContext>()));
    }

    [Test]
    public void CreateTimerSettings_ServiceThrowsException_ExceptionPropagates()
    {
        var request = new CreateTimerSettingsCommandProto
        {
            TimerSettingsId = Guid.NewGuid().ToString(),
            TraceData = new TraceDataProto { TraceId = Guid.NewGuid().ToString() }
        };

        _serviceMock
            .Setup(s => s.Create(It.IsAny<CreateTimerSettingsCommand>()))
            .ThrowsAsync(new InvalidOperationException());

        Assert.ThrowsAsync<InvalidOperationException>(() =>
            _receiver.CreateTimerSettings(request, Mock.Of<ServerCallContext>()));

        _tracerMock.Verify(t => t.TimerSettings.Create.CommandReceived(
                typeof(TimerSettingsCommandReceiver),
                Guid.Parse(request.TraceData.TraceId),
                request),
            Times.Once);
    }

    [Test]
    public async Task ChangeDocuTimerSaveInterval_ValidRequest_ReturnsSuccessResponse()
    {
        var request = new ChangeDocuTimerSaveIntervalCommandProto
        {
            TimerSettingsId = Guid.NewGuid().ToString(),
            DocuTimerSaveInterval = 120,
            TraceData = new TraceDataProto { TraceId = Guid.NewGuid().ToString() }
        };

        var response = await _receiver.ChangeDocuTimerSaveInterval(request, Mock.Of<ServerCallContext>());

        Assert.That(response.Success);
        _serviceMock.Verify(s => s.ChangeDocumentationInterval(It.IsAny<ChangeDocuTimerSaveIntervalCommand>()),
            Times.Once);
        _tracerMock.Verify(t => t.TimerSettings.ChangeDocuTimerInterval.CommandReceived(
                typeof(TimerSettingsCommandReceiver),
                Guid.Parse(request.TraceData.TraceId),
                request),
            Times.Once);
    }

    [Test]
    public void ChangeDocuTimerSaveInterval_InvalidTraceId_ThrowsFormatException()
    {
        var request = new ChangeDocuTimerSaveIntervalCommandProto
        {
            TimerSettingsId = Guid.NewGuid().ToString(),
            DocuTimerSaveInterval = 120,
            TraceData = new TraceDataProto { TraceId = "invalid-guid" }
        };

        Assert.ThrowsAsync<FormatException>(() =>
            _receiver.ChangeDocuTimerSaveInterval(request, Mock.Of<ServerCallContext>()));
    }

    [Test]
    public void ChangeDocuTimerSaveInterval_ServiceThrowsException_ExceptionPropagates()
    {
        var request = new ChangeDocuTimerSaveIntervalCommandProto
        {
            TimerSettingsId = Guid.NewGuid().ToString(),
            DocuTimerSaveInterval = 120,
            TraceData = new TraceDataProto { TraceId = Guid.NewGuid().ToString() }
        };

        _serviceMock
            .Setup(s => s.ChangeDocumentationInterval(It.IsAny<ChangeDocuTimerSaveIntervalCommand>()))
            .ThrowsAsync(new InvalidOperationException());

        Assert.ThrowsAsync<InvalidOperationException>(() =>
            _receiver.ChangeDocuTimerSaveInterval(request, Mock.Of<ServerCallContext>()));

        _tracerMock.Verify(t => t.TimerSettings.ChangeDocuTimerInterval.CommandReceived(
                typeof(TimerSettingsCommandReceiver),
                Guid.Parse(request.TraceData.TraceId),
                request),
            Times.Once);
    }

    [Test]
    public async Task ChangeSnapshotSaveInterval_ValidRequest_ReturnsSuccessResponse()
    {
        var request = new ChangeSnapshotSaveIntervalCommandProto
        {
            TimerSettingsId = Guid.NewGuid().ToString(),
            SnapshotSaveInterval = 60,
            TraceData = new TraceDataProto { TraceId = Guid.NewGuid().ToString() }
        };

        var response = await _receiver.ChangeSnapshotSaveInterval(request, Mock.Of<ServerCallContext>());

        Assert.That(response.Success);
        _serviceMock.Verify(s => s.ChangeSnapshotInterval(It.IsAny<ChangeSnapshotSaveIntervalCommand>()), Times.Once);
        _tracerMock.Verify(t => t.TimerSettings.ChangeSnapshotInterval.CommandReceived(
                typeof(TimerSettingsCommandReceiver),
                Guid.Parse(request.TraceData.TraceId),
                request),
            Times.Once);
    }

    [Test]
    public void ChangeSnapshotSaveInterval_InvalidTraceId_ThrowsFormatException()
    {
        var request = new ChangeSnapshotSaveIntervalCommandProto
        {
            TimerSettingsId = Guid.NewGuid().ToString(),
            TraceData = new TraceDataProto { TraceId = "invalid-guid" }
        };

        Assert.ThrowsAsync<FormatException>(() =>
            _receiver.ChangeSnapshotSaveInterval(request, Mock.Of<ServerCallContext>()));
    }

    [Test]
    public void ChangeSnapshotSaveInterval_ServiceThrowsException_ExceptionPropagates()
    {
        var request = new ChangeSnapshotSaveIntervalCommandProto
        {
            TimerSettingsId = Guid.NewGuid().ToString(),
            SnapshotSaveInterval = 10,
            TraceData = new TraceDataProto { TraceId = Guid.NewGuid().ToString() }
        };

        _serviceMock
            .Setup(s => s.ChangeSnapshotInterval(It.IsAny<ChangeSnapshotSaveIntervalCommand>()))
            .ThrowsAsync(new InvalidOperationException());

        Assert.ThrowsAsync<InvalidOperationException>(() =>
            _receiver.ChangeSnapshotSaveInterval(request, Mock.Of<ServerCallContext>()));

        _tracerMock.Verify(t => t.TimerSettings.ChangeSnapshotInterval.CommandReceived(
                typeof(TimerSettingsCommandReceiver),
                Guid.Parse(request.TraceData.TraceId),
                request),
            Times.Once);
    }
}