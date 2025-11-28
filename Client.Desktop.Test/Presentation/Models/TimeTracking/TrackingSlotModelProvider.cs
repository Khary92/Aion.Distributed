using Client.Desktop.Communication.Commands.TimeSlots.Records;
using Client.Desktop.Communication.Local.LocalEvents.Publisher;
using Client.Desktop.Communication.Requests;
using Client.Desktop.Communication.Requests.Replays.Records;
using Client.Desktop.DataModels;
using Client.Desktop.DataModels.Decorators.Replays;
using Client.Desktop.Presentation.Models.Synchronization;
using Client.Desktop.Presentation.Models.TimeTracking;
using Client.Desktop.Services.Authentication;
using Client.Desktop.Services.Cache;
using Client.Tracing.Tracing.Tracers;
using Global.Settings;
using Moq;

namespace Client.Desktop.Test.Presentation.Models.TimeTracking;

public static class TrackingSlotModelProvider
{
    private static TestNotificationPublisherFacade CreateNotificationPublisherMock()
    {
        return new TestNotificationPublisherFacade(CreateGrpcUrlBuilderMock().Object, CreateTracerMock().Object,
            CreateTokenServiceMock().Object);
    }

    private static IClientTimerNotificationPublisher CreateClientTimerNotificationPublisherMock()
    {
        return new TestTimerSettingsPublisher();
    }

    private static Mock<IGrpcUrlService> CreateGrpcUrlBuilderMock()
    {
        return new Mock<IGrpcUrlService>();
    }

    private static Mock<ITokenService> CreateTokenServiceMock()
    {
        return new Mock<ITokenService>();
    }

    private static Mock<ITraceCollector> CreateTracerMock()
    {
        return new Mock<ITraceCollector> { DefaultValue = DefaultValue.Mock };
    }

    private static Mock<IDocumentationSynchronizer> CreateSynchronizerMock()
    {
        return new Mock<IDocumentationSynchronizer>();
    }

    private static Mock<IPersistentCache<ClientSetStartTimeCommand>> CreateStartTimeCacheMock()
    {
        return new Mock<IPersistentCache<ClientSetStartTimeCommand>>();
    }

    private static Mock<IPersistentCache<ClientSetEndTimeCommand>> CreateEndTimeCacheMock()
    {
        return new Mock<IPersistentCache<ClientSetEndTimeCommand>>();
    }

    private static Mock<IRequestSender> CreateRequestSenderMock()
    {
        return new Mock<IRequestSender>();
    }

    public static async Task<TrackingSlotModelFixture> Create(TimeSlotClientModel? initialTimeSlot)
    {
        var publisherFacade = CreateNotificationPublisherMock();
        var tracer = CreateTracerMock();
        var startTimeCache = CreateStartTimeCacheMock();
        var endTimeCache = CreateEndTimeCacheMock();
        var requestSender = CreateRequestSenderMock();
        var synchronizer = CreateSynchronizerMock();
        var timerNotificationPublisher = CreateClientTimerNotificationPublisherMock();

        requestSender.Setup(rs => rs.Send(It.IsAny<ClientGetTicketReplaysByIdRequest>()))
            .ReturnsAsync([
                new DocumentationReplay("ChangeDocumentation"),
                new DocumentationReplay("Second Part")
            ]);

        return await CreateFixture(publisherFacade, tracer, synchronizer, startTimeCache, endTimeCache,
            requestSender, timerNotificationPublisher, initialTimeSlot);
    }

    private static Task<TrackingSlotModelFixture> CreateFixture(
        TestNotificationPublisherFacade publisherFacade,
        Mock<ITraceCollector> tracer,
        Mock<IDocumentationSynchronizer> documentationSynchronizer,
        Mock<IPersistentCache<ClientSetStartTimeCommand>> startTimeCache,
        Mock<IPersistentCache<ClientSetEndTimeCommand>> endTimeCache,
        Mock<IRequestSender> requestSender,
        IClientTimerNotificationPublisher timerNotificationPublisher,
        TimeSlotClientModel? timeSlotClientModel)
    {
        var instance = new TrackingSlotModel(startTimeCache.Object,
            endTimeCache.Object, tracer.Object, publisherFacade, timerNotificationPublisher);

        instance.RegisterMessenger();

        var ticket = new TicketClientModel(Guid.NewGuid(), "Name", "bookingNumber", "documentation from ticket", []);
        ticket.DocumentationSynchronizer = documentationSynchronizer.Object;
        instance.Ticket = ticket;

        var timeSlot = timeSlotClientModel ?? new TimeSlotClientModel(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
            DateTimeOffset.Now, DateTimeOffset.Now, new List<Guid>(), false);
        instance.TimeSlot = timeSlot;

        return Task.FromResult(new TrackingSlotModelFixture
        {
            Instance = instance,
            NotificationPublisher = publisherFacade,
            Tracer = tracer,
            StartTimeCache = startTimeCache,
            EndTimeCache = endTimeCache,
            RequestSender = requestSender,
            TimeSlot = timeSlot,
            Ticket = ticket,
            ClientTimerNotificationPublisher = timerNotificationPublisher
        });
    }

    public sealed class TrackingSlotModelFixture
    {
        public required TrackingSlotModel Instance { get; init; }
        public required TestNotificationPublisherFacade NotificationPublisher { get; init; }
        public required IClientTimerNotificationPublisher ClientTimerNotificationPublisher { get; init; }
        public required Mock<IPersistentCache<ClientSetStartTimeCommand>> StartTimeCache { get; init; }
        public required Mock<IPersistentCache<ClientSetEndTimeCommand>> EndTimeCache { get; init; }
        public required Mock<ITraceCollector> Tracer { get; init; }
        public required Mock<IRequestSender> RequestSender { get; init; }
        public required TimeSlotClientModel TimeSlot { get; init; }
        public required TicketClientModel Ticket { get; init; }
    }
}