using Client.Desktop.Communication.Requests;
using Client.Desktop.Communication.Requests.WorkDays.Records;
using Client.Desktop.DataModels;
using Client.Desktop.Presentation.Models.Export;
using Client.Desktop.Services.Authentication;
using Client.Desktop.Services.Export;
using Client.Desktop.Services.LocalSettings;
using Client.Tracing.Tracing.Tracers;
using Global.Settings;
using Moq;

namespace Client.Desktop.Test.Presentation.Models.Export;

public static class ExportModelProvider
{
    private static TestNotificationPublisherFacade CreateNotificationPublisherMock()
    {
        return new TestNotificationPublisherFacade(CreateGrpcUrlBuilderMock().Object, CreateTracerMock().Object,
            CreateTokenServiceMock().Object);
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

    private static Mock<IRequestSender> CreateRequestSenderMock()
    {
        return new Mock<IRequestSender>();
    }

    private static Mock<ILocalSettingsService> CreateLocalSettingsServiceMock()
    {
        return new Mock<ILocalSettingsService>();
    }

    private static Mock<IExportService> CreateExportServiceMock()
    {
        return new Mock<IExportService>();
    }

    public static async Task<ExportModelFixture> Create(List<WorkDayClientModel> initialWorkDays)
    {
        var requestSender = CreateRequestSenderMock();
        var tracer = CreateTracerMock();
        var publisherFacade = CreateNotificationPublisherMock();
        var localSettingsService = CreateLocalSettingsServiceMock();
        var exportService = CreateExportServiceMock();

        requestSender
            .Setup(rs => rs.Send(It.IsAny<ClientGetAllWorkDaysRequest>()))
            .ReturnsAsync(initialWorkDays);

        return await CreateFixture(requestSender, tracer, publisherFacade, localSettingsService, exportService);
    }

    private static async Task<ExportModelFixture> CreateFixture(
        Mock<IRequestSender> requestSender,
        Mock<ITraceCollector> tracer,
        TestNotificationPublisherFacade publisherFacade,
        Mock<ILocalSettingsService> localSettingsService,
        Mock<IExportService> exportService)
    {
        var instance = new ExportModel(requestSender.Object, exportService.Object, tracer.Object,
            localSettingsService.Object, publisherFacade);

        instance.RegisterMessenger();
        await instance.InitializeAsync();

        return new ExportModelFixture
        {
            Instance = instance,
            RequestSender = requestSender,
            Tracer = tracer,
            NotificationPublisher = publisherFacade,
            LocalSettingsService = localSettingsService,
            ExportService = exportService
        };
    }

    public sealed class ExportModelFixture
    {
        public required ExportModel Instance { get; init; }
        public required Mock<IRequestSender> RequestSender { get; init; }
        public required Mock<ITraceCollector> Tracer { get; init; }
        public required TestNotificationPublisherFacade NotificationPublisher { get; init; }
        public required Mock<IExportService> ExportService { get; init; }
        public required Mock<ILocalSettingsService> LocalSettingsService { get; init; }
    }
}