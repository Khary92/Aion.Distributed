using Grpc.Core;
using Moq;
using Proto.Notifications.Note;
using NoteNotificationService = Core.Server.Communication.Endpoints.Note.NoteNotificationService;

namespace Core.Server.Test.Communication.Endpoints.Note;

[TestFixture]
[TestOf(typeof(NoteNotificationService))]
public class NoteNotificationServiceTest
{
    [SetUp]
    public void SetUp()
    {
        _service = new NoteNotificationService();
    }

    private NoteNotificationService _service;

    [Test]
    public async Task Subscribe_and_send_writes_to_active_client()
    {
        var cts = new CancellationTokenSource();
        var streamMock = new Mock<IServerStreamWriter<NoteNotification>>(MockBehavior.Strict);
        streamMock
            .Setup(s => s.WriteAsync(It.IsAny<NoteNotification>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var context = FakeServerCallContext.WithCancellation(cts.Token);

        var subscribeTask = _service.SubscribeNoteNotifications(
            new SubscribeRequest(),
            streamMock.Object,
            context);

        await Task.Delay(50, cts.Token);

        var notificationId = Guid.NewGuid();
        var notification = new NoteNotification
        {
            NoteCreated = new NoteCreatedNotification
            {
                NoteId = notificationId.ToString()
            }
        };
        await _service.SendNotificationAsync(notification);

        streamMock.Verify(s => s.WriteAsync(
                It.Is<NoteNotification>(n => n.NoteCreated.NoteId == notificationId.ToString()),
                It.IsAny<CancellationToken>()),
            Times.Once);

        await cts.CancelAsync();
        await subscribeTask;
    }

    [Test]
    public async Task Canceled_client_is_removed_and_not_written_to()
    {
        var cts = new CancellationTokenSource();
        var streamMock = new Mock<IServerStreamWriter<NoteNotification>>(MockBehavior.Strict);

        var context = FakeServerCallContext.WithCancellation(cts.Token);
        var subscribeTask = _service.SubscribeNoteNotifications(
            new SubscribeRequest(),
            streamMock.Object,
            context);

        await Task.Delay(50, cts.Token);

        await cts.CancelAsync();
        await subscribeTask;

        var notification = new NoteNotification { NoteCreated = new NoteCreatedNotification() };
        await _service.SendNotificationAsync(notification);

        streamMock.Verify(s => s.WriteAsync(It.IsAny<NoteNotification>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test]
    public async Task Client_removed_when_write_fails()
    {
        var cts = new CancellationTokenSource();
        var streamMock = new Mock<IServerStreamWriter<NoteNotification>>(MockBehavior.Strict);

        streamMock
            .Setup(s => s.WriteAsync(It.IsAny<NoteNotification>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("stream broken"));

        var context = FakeServerCallContext.WithCancellation(cts.Token);
        var subscribeTask = _service.SubscribeNoteNotifications(
            new SubscribeRequest(),
            streamMock.Object,
            context);

        await Task.Delay(50, cts.Token);

        var notification = new NoteNotification { NoteCreated = new NoteCreatedNotification() };
        await _service.SendNotificationAsync(notification);

        streamMock.Verify(s => s.WriteAsync(It.IsAny<NoteNotification>(), It.IsAny<CancellationToken>()), Times.Once);

        await _service.SendNotificationAsync(new NoteNotification { NoteCreated = new NoteCreatedNotification() });
        streamMock.Verify(s => s.WriteAsync(It.IsAny<NoteNotification>(), It.IsAny<CancellationToken>()), Times.Once);

        await cts.CancelAsync();
        await subscribeTask;
    }

    private sealed class FakeServerCallContext : ServerCallContext
    {
        private FakeServerCallContext(
            Metadata requestHeaders,
            CancellationToken cancellationToken,
            string host,
            string method,
            string peer,
            DateTime deadline, WriteOptions writeOptionsCore, Status statusCore)
        {
            RequestHeadersCore = requestHeaders;
            CancellationTokenCore = cancellationToken;
            HostCore = host;
            MethodCore = method;
            PeerCore = peer;
            DeadlineCore = deadline;
            WriteOptionsCore = writeOptionsCore;
            StatusCore = statusCore;
        }

        protected override string MethodCore { get; }
        protected override string HostCore { get; }
        protected override string PeerCore { get; }
        protected override DateTime DeadlineCore { get; }
        protected override Metadata RequestHeadersCore { get; }
        protected override CancellationToken CancellationTokenCore { get; }
        protected override Metadata ResponseTrailersCore { get; } = [];
        protected override Status StatusCore { get; set; }
        protected override WriteOptions? WriteOptionsCore { get; set; }

        protected override AuthContext AuthContextCore { get; } =
            new("fake", new Dictionary<string, List<AuthProperty>>());

        public static ServerCallContext WithCancellation(CancellationToken token)
        {
            return new FakeServerCallContext(new Metadata(), token, "localhost",
                "/proto.NoteNotificationService/Subscribe",
                "ipv4:127.0.0.1:12345", DateTime.MaxValue, new WriteOptions(), new Status());
        }

        protected override ContextPropagationToken CreatePropagationTokenCore(ContextPropagationOptions? options)
        {
            return CreatePropagationToken(options);
        }

        protected override Task WriteResponseHeadersAsyncCore(Metadata responseHeaders)
        {
            return Task.CompletedTask;
        }
    }
}