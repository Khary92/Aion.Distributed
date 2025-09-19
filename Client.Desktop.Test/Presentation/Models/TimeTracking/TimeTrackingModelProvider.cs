using Client.Desktop.Communication.Commands;
using Client.Desktop.Communication.Requests;
using Client.Desktop.Communication.Requests.Ticket;
using Client.Desktop.DataModels;
using Client.Desktop.Presentation.Factories;
using Client.Desktop.Presentation.Models.TimeTracking;
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

    private static Mock<ITimeSlotViewModelFactory> CreateTimeSlotViewModelFactoryMock()
    {
        return new Mock<ITimeSlotViewModelFactory>();
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

    public static async Task<TimeTrackingModelFixture> Create(List<TicketClientModel> initialTickets)
    {
        var messenger = CreateMessenger();
        var requestSender = CreateRequestSenderMock();
        var commandSender = CreateCommandSenderMock();
        var timeSlotViewModelFactory = CreateTimeSlotViewModelFactoryMock();
        var tracer = CreateTracerMock();
        var localSettingsService = CreateLocalSettingsServiceMock();

        requestSender
            .Setup(rs => rs.Send(It.IsAny<ClientGetTicketsForCurrentSprintRequest>()))
            .ReturnsAsync(initialTickets);

        return await CreateFixture(messenger, requestSender, commandSender, timeSlotViewModelFactory, tracer,
            localSettingsService);
    }

    private static async Task<TimeTrackingModelFixture> CreateFixture(IMessenger messenger,
        Mock<IRequestSender> requestSender, Mock<ICommandSender> commandSender,
        Mock<ITimeSlotViewModelFactory> timeSlotViewModelFactory,
        Mock<ITraceCollector> tracer, Mock<ILocalSettingsService> localSettingsService)
    {
        var instance = new TimeTrackingModel(messenger, commandSender.Object, requestSender.Object,
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
            LocalSettingsService = localSettingsService
        };
    }

    public sealed class TimeTrackingModelFixture
    {
        public required TimeTrackingModel Instance { get; init; }
        public required IMessenger Messenger { get; init; }
        public required Mock<IRequestSender> RequestSender { get; init; }
        public required Mock<ICommandSender> CommandSender { get; init; }
        public required Mock<ITimeSlotViewModelFactory> TimeSlotViewModelFactory { get; init; }
        public required Mock<ILocalSettingsService> LocalSettingsService { get; init; }
    }
}