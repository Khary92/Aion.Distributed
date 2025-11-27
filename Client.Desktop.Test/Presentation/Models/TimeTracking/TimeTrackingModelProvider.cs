using Client.Desktop.Communication.Commands;
using Client.Desktop.Communication.Commands.TimeSlots.Records;
using Client.Desktop.Communication.Local.LocalEvents.Publisher;
using Client.Desktop.Communication.Requests;
using Client.Desktop.Communication.Requests.Client.Records;
using Client.Desktop.Communication.Requests.Sprint;
using Client.Desktop.Communication.Requests.Ticket;
using Client.Desktop.DataModels;
using Client.Desktop.Presentation.Factories;
using Client.Desktop.Presentation.Models.TimeTracking;
using Client.Desktop.Services.Cache;
using Client.Desktop.Services.LocalSettings;
using Client.Tracing.Tracing.Tracers;
using Global.Settings;
using Moq;

namespace Client.Desktop.Test.Presentation.Models.TimeTracking;

public static class TimeTrackingModelProvider
{
    private static TestNotificationPublisherFacade CreateNotificationPublisherMock()
    {
        return new TestNotificationPublisherFacade(CreateGrpcUrlBuilderMock().Object, CreateTracerMock().Object);
    }

    private static IClientTimerNotificationPublisher CreateClientTimerNotificationPublisherMock()
    {
        return new TestTimerSettingsPublisher();
    }

    private static Mock<IGrpcUrlService> CreateGrpcUrlBuilderMock()
    {
        return new Mock<IGrpcUrlService>();
    }

    private static Mock<ITraceCollector> CreateTracerMock()
    {
        return new Mock<ITraceCollector> { DefaultValue = DefaultValue.Mock };
    }

    private static Mock<IRequestSender> CreateRequestSenderMock()
    {
        return new Mock<IRequestSender>();
    }

    private static Mock<ICommandSender> CreateCommandSenderMock()
    {
        return new Mock<ICommandSender>();
    }

    private static Mock<ITrackingSlotViewModelFactory> CreateTimeSlotViewModelFactoryMock()
    {
        return new Mock<ITrackingSlotViewModelFactory>();
    }

    private static Mock<ILocalSettingsService> CreateLocalSettingsServiceMock()
    {
        return new Mock<ILocalSettingsService>();
    }

    private static Mock<IPersistentCache<ClientSetStartTimeCommand>> CreateStartTimeCacheMock()
    {
        return new Mock<IPersistentCache<ClientSetStartTimeCommand>> { DefaultValue = DefaultValue.Mock };
    }

    private static Mock<IPersistentCache<ClientSetEndTimeCommand>> CreateEndTimeCacheMock()
    {
        return new Mock<IPersistentCache<ClientSetEndTimeCommand>> { DefaultValue = DefaultValue.Mock };
    }

    private static Mock<IStatisticsViewModelFactory> CreateStatisticsViewModelFactoryMock()
    {
        return new Mock<IStatisticsViewModelFactory>();
    }

    private static Mock<INoteStreamViewModelFactory> CreateNoteStreamViewModelFactoryMock()
    {
        return new Mock<INoteStreamViewModelFactory>();
    }

    private static TicketClientModel CreateTicketClientModel()
    {
        return new TicketClientModel(Guid.NewGuid(), "InitialTicketName", "InitialBookingNumber", "ChangeDocumentation",
            new List<Guid>());
    }

    public static async Task<TimeTrackingModelFixture> Create(List<TicketClientModel> initialTickets,
        List<ClientGetTrackingControlResponse> initialTimeSlots)
    {
        var publisherFacade = CreateNotificationPublisherMock();
        var requestSender = CreateRequestSenderMock();
        var commandSender = CreateCommandSenderMock();
        var timeSlotViewModelFactory = CreateTimeSlotViewModelFactoryMock();
        var tracer = CreateTracerMock();
        var localSettingsService = CreateLocalSettingsServiceMock();
        var startTimeCache = CreateStartTimeCacheMock();
        var endTimeCache = CreateEndTimeCacheMock();
        var statisticsViewModelFactory = CreateStatisticsViewModelFactoryMock();
        var noteStreamViewModelFactory = CreateNoteStreamViewModelFactoryMock();
        var timerNotificationPublisher = CreateClientTimerNotificationPublisherMock();

        requestSender
            .Setup(rs => rs.Send(It.IsAny<ClientGetTicketsForCurrentSprintRequest>()))
            .ReturnsAsync(initialTickets);

        var currentSprintId = Guid.NewGuid();
        var sprintClientModel = new SprintClientModel(currentSprintId, "InitialSprintName", true, DateTimeOffset.UtcNow,
            DateTimeOffset.MaxValue, initialTickets.Select(t => t.TicketId).ToList());
        requestSender
            .Setup(rs => rs.Send(It.IsAny<ClientGetActiveSprintRequest>()))
            .ReturnsAsync(sprintClientModel);

        requestSender
            .Setup(rs => rs.Send(It.IsAny<ClientGetTrackingControlDataRequest>()))
            .ReturnsAsync(initialTimeSlots);

        requestSender
            .Setup(rs => rs.Send(It.IsAny<ClientGetAllTicketsRequest>()))
            .ReturnsAsync(initialTickets);

        var trackingSlotModel = new TrackingSlotModel(startTimeCache.Object, endTimeCache.Object, tracer.Object,
            publisherFacade, timerNotificationPublisher)
        {
            Ticket = CreateTicketClientModel()
        };

        trackingSlotModel.TimeSlot = new TimeSlotClientModel(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
            DateTimeOffset.Now, DateTimeOffset.Now, [], false);

        var trackingSlotViewModel = new TrackingSlotViewModel(trackingSlotModel, statisticsViewModelFactory.Object,
            noteStreamViewModelFactory.Object, timerNotificationPublisher)
        {
            NoteStreamViewModel = null,
            StatisticsViewModel = null
        };

        await trackingSlotViewModel.CreateSubViewModels(Guid.NewGuid(), Guid.NewGuid(), new StatisticsDataClientModel(
            Guid.NewGuid(),
            Guid.NewGuid(), new List<Guid>(), true, false, false));

        timeSlotViewModelFactory
            .Setup(tf => tf.Create(It.IsAny<TicketClientModel>(), It.IsAny<StatisticsDataClientModel>(),
                It.IsAny<TimeSlotClientModel>()))
            .ReturnsAsync(trackingSlotViewModel);

        return await CreateFixture(publisherFacade, requestSender, commandSender, timeSlotViewModelFactory, tracer,
            localSettingsService, timerNotificationPublisher, sprintClientModel);
    }

    private static async Task<TimeTrackingModelFixture> CreateFixture(
        TestNotificationPublisherFacade publisherFacade,
        Mock<IRequestSender> requestSender,
        Mock<ICommandSender> commandSender,
        Mock<ITrackingSlotViewModelFactory> timeSlotViewModelFactory,
        Mock<ITraceCollector> tracer,
        Mock<ILocalSettingsService> localSettingsService,
        IClientTimerNotificationPublisher timerNotificationPublisher,
        SprintClientModel sprintClientModel)
    {
        var instance = new TimeTrackingModel(commandSender.Object, requestSender.Object,
            timeSlotViewModelFactory.Object, localSettingsService.Object, tracer.Object, publisherFacade);

        instance.RegisterMessenger();
        await instance.InitializeAsync();

        return new TimeTrackingModelFixture
        {
            Instance = instance,
            NotificationPublisher = publisherFacade,
            RequestSender = requestSender,
            CommandSender = commandSender,
            TimeSlotViewModelFactory = timeSlotViewModelFactory,
            LocalSettingsService = localSettingsService,
            CurrentSprintId = sprintClientModel.SprintId,
            ClientTimerNotificationPublisher = timerNotificationPublisher
        };
    }

    public sealed class TimeTrackingModelFixture
    {
        public required TimeTrackingModel Instance { get; init; }
        public required TestNotificationPublisherFacade NotificationPublisher { get; init; }
        public required Mock<IRequestSender> RequestSender { get; init; }
        public required Mock<ICommandSender> CommandSender { get; init; }
        public required Mock<ITrackingSlotViewModelFactory> TimeSlotViewModelFactory { get; init; }
        public required Mock<ILocalSettingsService> LocalSettingsService { get; init; }

        public required IClientTimerNotificationPublisher ClientTimerNotificationPublisher { get; init; }
        public required Guid CurrentSprintId { get; init; }
    }
}