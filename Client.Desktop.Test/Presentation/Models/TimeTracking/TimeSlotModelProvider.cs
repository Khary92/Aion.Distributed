using Client.Desktop.Communication.Commands.TimeSlots.Records;
using Client.Desktop.Communication.Requests;
using Client.Desktop.Communication.Requests.Replays.Records;
using Client.Desktop.DataModels;
using Client.Desktop.DataModels.Decorators.Replays;
using Client.Desktop.Presentation.Models.Synchronization;
using Client.Desktop.Presentation.Models.TimeTracking;
using Client.Desktop.Services.Cache;
using Client.Tracing.Tracing.Tracers;
using CommunityToolkit.Mvvm.Messaging;
using Moq;

namespace Client.Desktop.Test.Presentation.Models.TimeTracking;

public static class TimeSlotModelProvider
{
    private static IMessenger CreateMessenger()
    {
        return new WeakReferenceMessenger();
    }

    private static Mock<ITraceCollector> CreateTracerMock()
    {
        return new Mock<ITraceCollector> { DefaultValue = DefaultValue.Mock };
    }

    private static Mock<IStateSynchronizer<TicketReplayDecorator, string>> CreateDocumentationSynchronizerMock() =>
        new();

    private static Mock<IPersistentCache<ClientSetStartTimeCommand>> CreateStartTimeCacheMock() => new();
    private static Mock<IPersistentCache<ClientSetEndTimeCommand>> CreateEndTimeCacheMock() => new();
    private static Mock<IRequestSender> CreateRequestSenderMock() => new();

    public static async Task<TimeSlotModelFixture> Create(TimeSlotClientModel? initialTimeSlot)
    {
        var messenger = CreateMessenger();
        var tracer = CreateTracerMock();
        var documentationSynchronizer = CreateDocumentationSynchronizerMock();
        var startTimeCache = CreateStartTimeCacheMock();
        var endTimeCache = CreateEndTimeCacheMock();
        var requestSender = CreateRequestSenderMock();

        requestSender.Setup(rs => rs.Send(It.IsAny<ClientGetTicketReplaysByIdRequest>())).ReturnsAsync([
            new DocumentationReplay("Documentation"), new DocumentationReplay("Second Part")
        ]);

        return await CreateFixture(messenger, tracer, documentationSynchronizer, startTimeCache, endTimeCache,
            requestSender, initialTimeSlot);
    }

    private static async Task<TimeSlotModelFixture> CreateFixture(IMessenger messenger, Mock<ITraceCollector> tracer,
        Mock<IStateSynchronizer<TicketReplayDecorator, string>> documentationSynchronizer,
        Mock<IPersistentCache<ClientSetStartTimeCommand>> startTimeCache,
        Mock<IPersistentCache<ClientSetEndTimeCommand>> endTimeCache, Mock<IRequestSender> requestSender,
        TimeSlotClientModel? timeSlotClientModel)
    {
        var instance = new TrackingSlotModel(messenger, documentationSynchronizer.Object, startTimeCache.Object,
            endTimeCache.Object, tracer.Object);

        instance.RegisterMessenger();

        var ticket = new TicketClientModel(Guid.NewGuid(), "Name", "bookingNumber", "documentation from ticket", []);
        var ticketReplayDecorator = new TicketReplayDecorator(requestSender.Object, ticket);
        instance.TicketReplayDecorator = ticketReplayDecorator;

        var timeSlot = timeSlotClientModel ?? new TimeSlotClientModel(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
            DateTimeOffset.Now, DateTimeOffset.Now, [], false);
        instance.TimeSlot = timeSlot;

        return new TimeSlotModelFixture
        {
            Instance = instance,
            Tracer = tracer,
            Messenger = messenger,
            DocumentationSynchronizer = documentationSynchronizer,
            StartTimeCache = startTimeCache,
            EndTimeCache = endTimeCache,
            RequestSender = requestSender,
            TicketReplayDecorator = ticketReplayDecorator,
            TimeSlot = timeSlot
        };
    }

    public sealed class TimeSlotModelFixture
    {
        public required TrackingSlotModel Instance { get; init; }
        public required Mock<IStateSynchronizer<TicketReplayDecorator, string>> DocumentationSynchronizer { get; init; }
        public required Mock<IPersistentCache<ClientSetStartTimeCommand>> StartTimeCache { get; init; }
        public required Mock<IPersistentCache<ClientSetEndTimeCommand>> EndTimeCache { get; init; }
        public required Mock<ITraceCollector> Tracer { get; init; }
        public required IMessenger Messenger { get; init; }
        public required TicketReplayDecorator TicketReplayDecorator { get; init; }
        public required TimeSlotClientModel TimeSlot { get; init; }
        public required Mock<IRequestSender> RequestSender { get; init; }
    }
}