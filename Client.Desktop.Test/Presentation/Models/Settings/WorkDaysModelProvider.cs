using Client.Desktop.Communication.Commands;
using Client.Desktop.Communication.Requests;
using Client.Desktop.Communication.Requests.WorkDays.Records;
using Client.Desktop.DataModels;
using Client.Desktop.Presentation.Models.Settings;
using Client.Desktop.Services.LocalSettings;
using Client.Tracing.Tracing.Tracers;
using Global.Settings;
using Moq;

namespace Client.Desktop.Test.Presentation.Models.Settings;

public static class WorkDaysModelProvider
{
    private static TestNotificationPublisherFacade CreateNotificationPublisherMock()
    {
        return new TestNotificationPublisherFacade(CreateGrpcUrlBuilderMock().Object, CreateTracerMock().Object);
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

    private static Mock<ILocalSettingsService> CreateLocalSettingsService()
    {
        return new Mock<ILocalSettingsService>();
    }

    public static async Task<WorkDaysModelFixture> Create(List<WorkDayClientModel> initialWorkDays,
        bool isWorkDayExisting)
    {
        var requestSender = CreateRequestSenderMock();
        var tracer = CreateTracerMock();
        var publisherFacade = CreateNotificationPublisherMock();
        var commandSender = CreateCommandSenderMock();
        var localSettingsService = CreateLocalSettingsService();

        requestSender
            .Setup(rs => rs.Send(It.IsAny<ClientGetAllWorkDaysRequest>()))
            .ReturnsAsync(initialWorkDays);

        requestSender
            .Setup(rs => rs.Send(It.IsAny<ClientIsWorkDayExistingRequest>()))
            .ReturnsAsync(isWorkDayExisting);

        return await CreateFixture(publisherFacade, commandSender, localSettingsService, requestSender, tracer);
    }

    private static async Task<WorkDaysModelFixture> CreateFixture(
        TestNotificationPublisherFacade publisherFacade,
        Mock<ICommandSender> commandSender,
        Mock<ILocalSettingsService> localSettingsService,
        Mock<IRequestSender> requestSender,
        Mock<ITraceCollector> tracer)
    {
        var instance = new WorkDaysModel(commandSender.Object, requestSender.Object, localSettingsService.Object,
            tracer.Object, publisherFacade);

        instance.RegisterMessenger();
        instance.ClientWorkDaySelectionChangedNotificationReceived += _ => Task.CompletedTask;
        await instance.InitializeAsync();

        return new WorkDaysModelFixture
        {
            Instance = instance,
            RequestSender = requestSender,
            Tracer = tracer,
            NotificationPublisher = publisherFacade,
            CommandSender = commandSender,
            LocalSettingsService = localSettingsService
        };
    }

    public sealed class WorkDaysModelFixture
    {
        public required WorkDaysModel Instance { get; init; }
        public required TestNotificationPublisherFacade NotificationPublisher { get; init; }
        public required Mock<ILocalSettingsService> LocalSettingsService { get; init; }
        public required Mock<IRequestSender> RequestSender { get; init; }
        public required Mock<ICommandSender> CommandSender { get; init; }
        public required Mock<ITraceCollector> Tracer { get; init; }
    }
}