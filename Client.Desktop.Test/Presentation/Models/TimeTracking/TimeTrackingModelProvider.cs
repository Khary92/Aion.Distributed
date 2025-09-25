using Client.Desktop.Communication.Commands;
using Client.Desktop.Communication.Commands.TimeSlots.Records;
using Client.Desktop.Communication.Requests;
using Client.Desktop.Communication.Requests.Client.Records;
using Client.Desktop.Communication.Requests.Sprint;
using Client.Desktop.Communication.Requests.Ticket;
using Client.Desktop.DataModels;
using Client.Desktop.Presentation.Factories;
using Client.Desktop.Presentation.Models.Synchronization;
using Client.Desktop.Presentation.Models.TimeTracking;
using Client.Desktop.Services.Cache;
using Client.Desktop.Services.LocalSettings;
using Client.Tracing.Tracing.Tracers;
using Moq;
using IMessenger = CommunityToolkit.Mvvm.Messaging.IMessenger;
using WeakReferenceMessenger = CommunityToolkit.Mvvm.Messaging.WeakReferenceMessenger;

namespace Client.Desktop.Test.Presentation.Models.TimeTracking;

public static class TimeTrackingModelProvider
{
    private static IMessenger CreateMessenger()
    {
        return new WeakReferenceMessenger();
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

    private static Mock<IDocumentationSynchronizer> CreateDocumentationSynchronizerMock()
    {
        return new Mock<IDocumentationSynchronizer>();
    }

    private static Mock<ILocalSettingsService> CreateLocalSettingsServiceMock()
    {
        return new Mock<ILocalSettingsService>();
    }

    private static Mock<ITraceCollector> CreateTracerMock()
    {
        return new Mock<ITraceCollector>
        {
            DefaultValue = DefaultValue.Mock
        };
    }

    private static Mock<IPersistentCache<ClientSetStartTimeCommand>> CreateStartTimeCacheMock()
    {
        return new Mock<IPersistentCache<ClientSetStartTimeCommand>>()
        {
            DefaultValue = DefaultValue.Mock
        };
    }

    private static Mock<IPersistentCache<ClientSetEndTimeCommand>> CreateEndTimeCacheMock()
    {
        return new Mock<IPersistentCache<ClientSetEndTimeCommand>>()
        {
            DefaultValue = DefaultValue.Mock
        };
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
            []);
    }

    public static async Task<TimeTrackingModelFixture> Create(List<TicketClientModel> initialTickets,
        List<ClientGetTrackingControlResponse> initialTimeSlots)
    {
        var messenger = CreateMessenger();
        var requestSender = CreateRequestSenderMock();
        var commandSender = CreateCommandSenderMock();
        var timeSlotViewModelFactory = CreateTimeSlotViewModelFactoryMock();
        var tracer = CreateTracerMock();
        var localSettingsService = CreateLocalSettingsServiceMock();
        var startTimeCache = CreateStartTimeCacheMock();
        var endTimeCache = CreateEndTimeCacheMock();
        var statisticsViewModelFactory = CreateStatisticsViewModelFactoryMock();
        var noteStreamViewModelFactory = CreateNoteStreamViewModelFactoryMock();
        var documentationSynchronizer = CreateDocumentationSynchronizerMock();

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

        var timeSlotModel = new TrackingSlotModel(messenger, startTimeCache.Object, endTimeCache.Object, tracer.Object)
        {
            Ticket = CreateTicketClientModel()
        };

        var timeSlotViewModel = new TrackingSlotViewModel(timeSlotModel, statisticsViewModelFactory.Object,
            noteStreamViewModelFactory.Object, messenger)
        {
            NoteStreamViewModel = null,
            StatisticsViewModel = null
        };

        timeSlotViewModel.CreateSubViewModels(Guid.NewGuid(), Guid.NewGuid(), new StatisticsDataClientModel(
            Guid.NewGuid(),
            Guid.NewGuid(), new List<Guid>(), true, false, false));
        timeSlotViewModelFactory
            .Setup(tf => tf.Create(It.IsAny<TicketClientModel>(), It.IsAny<StatisticsDataClientModel>(),
                It.IsAny<TimeSlotClientModel>())).ReturnsAsync(
                timeSlotViewModel);

        return await CreateFixture(messenger, requestSender, commandSender, timeSlotViewModelFactory, tracer,
            localSettingsService, documentationSynchronizer,sprintClientModel);
    }

    private static async Task<TimeTrackingModelFixture> CreateFixture(IMessenger messenger,
        Mock<IRequestSender> requestSender, Mock<ICommandSender> commandSender,
        Mock<ITrackingSlotViewModelFactory> timeSlotViewModelFactory,
        Mock<ITraceCollector> tracer, Mock<ILocalSettingsService> localSettingsService,
        Mock<IDocumentationSynchronizer> documentationSynchronizer,
        SprintClientModel sprintClientModel)
    {
        var instance = new TimeTrackingModel(messenger, commandSender.Object, requestSender.Object,
            documentationSynchronizer.Object,
            timeSlotViewModelFactory.Object, localSettingsService.Object, tracer.Object);

        instance.RegisterMessenger();
        await instance.InitializeAsync();

        return new TimeTrackingModelFixture
        {
            Instance = instance,
            Messenger = messenger,
            RequestSender = requestSender,
            CommandSender = commandSender,
            TimeSlotViewModelFactory = timeSlotViewModelFactory,
            LocalSettingsService = localSettingsService,
            CurrentSprintId = sprintClientModel.SprintId
        };
    }

    public sealed class TimeTrackingModelFixture
    {
        public required TimeTrackingModel Instance { get; init; }
        public required IMessenger Messenger { get; init; }
        public required Mock<IRequestSender> RequestSender { get; init; }
        public required Mock<ICommandSender> CommandSender { get; init; }
        public required Mock<ITrackingSlotViewModelFactory> TimeSlotViewModelFactory { get; init; }
        public required Mock<ILocalSettingsService> LocalSettingsService { get; init; }
        public required Guid CurrentSprintId { get; init; }
    }
}